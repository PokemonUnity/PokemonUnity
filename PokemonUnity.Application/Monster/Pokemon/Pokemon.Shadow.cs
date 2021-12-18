using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Monster.Data;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity
{
	public partial class TempData : ITempMetadataPokemonShadow
	{
		public int[] heartgauges { get; protected set; }
	}

	public partial class Game : IGameShadowPokemon
	{
		public void pbPurify(IPokemonShadowPokemon pkmn, IPurifyChamberScene scene)
		{
			if (pkmn.heartgauge == 0 && pkmn.shadow && pkmn is IPokemon pokemon)
			{
				if (pkmn.savedev == null && pkmn.savedexp == 0) return;
				pkmn.shadow = false;
				pokemon.giveRibbon(Ribbons.NATIONAL);
				scene.pbDisplay(Game._INTL("{1} opened the door to its heart!", pokemon.name));
				Moves[] oldmoves = new Moves[4];
				//for (int i = 0; i < 4; i++) { oldmoves.Add(pokemon.moves[i].id); }
				for (int i = 0; i < 4; i++) { oldmoves[i] = pokemon.moves[i].id; }
				pkmn.pbUpdateShadowMoves();
				for (int i = 0; i < 4; i++)
				{
					if (pokemon.moves[i].id != 0 && pokemon.moves[i].id != oldmoves[i])
					{
						scene.pbDisplay(Game._INTL("{1} regained the move \n{2}!",
						   pokemon.name, pokemon.moves[i].id.ToString(TextScripts.Name)));
					}
				}
				pokemon.pbRecordFirstMoves();
				if (pkmn.savedev != null)
				{
					for (int i = 0; i < 6; i++)
					{
						pbApplyEVGain(pokemon, (Stats)i, pkmn.savedev[i]);
					}
					pkmn.savedev = null;
				}
				int newexp = Experience.pbAddExperience(pokemon.exp, pkmn.savedexp, pokemon.GrowthRate);
				pkmn.savedexp = 0;
				int newlevel = Experience.GetLevelFromExperience(pokemon.GrowthRate, newexp);
				int curlevel = pokemon.Level;
				if (newexp != pokemon.exp)
				{
					scene.pbDisplay(Game._INTL("{1} regained {2} Exp. Points!", pokemon.name, newexp - pokemon.exp));
				}
				if (newlevel == curlevel)
				{
					pokemon.exp = newexp;
					pokemon.calcStats();
				}
				else
				{
					//Game.pbChangeLevel(pokemon, newlevel, scene); // for convenience
					pokemon.exp = newexp;
				}
				string speciesname = pokemon.species.ToString(TextScripts.Name);
				if (scene.pbConfirm(Game._INTL("Would you like to give a nickname to {1}?", speciesname)))
				{
					string helptext = Game._INTL("{1}'s nickname?", speciesname);
					string newname = pbEnterPokemonName(helptext, 0, 10, "", pokemon);
					if (newname != "") pokemon.name = newname;
				}
			}
		}

		#region Use with Relic Stone (Move to separate class?)
		public void pbRelicStoneScreen(IPokemonShadowPokemon pkmn)
		{
			//bool retval = true;
			pbFadeOutIn(99999, block: () => {
				IRelicStoneScene scene = Scenes.RelicStone; //new RelicStoneScene();
				IRelicStoneScreen screen = Screens.RelicStone.initialize(scene); //new RelicStoneScreen(scene);
				screen.pbStartScreen(pkmn); //retval = 
				screen.pbStartScreen(pkmn);
			});
			//return retval;
		}

		public bool pbIsPurifiable(IPokemonShadowPokemon pkmn)
		{
			if (!(pkmn as IPokemon).IsNotNullOrNone()) return false;
			if (pkmn.isShadow && pkmn.heartgauge == 0)
			//   && pkmn.Species != Pokemons.LUGIA)
			{
				return true;
			}
			return false;
		}

		public bool pbHasPurifiableInParty()
		{
			return Trainer.party.Any(item => pbIsPurifiable(item as IPokemonShadowPokemon));
		}

		public void pbRelicStone()
		{
			if (pbHasPurifiableInParty())
			{
				(this as IGameMessage).pbMessage(Game._INTL("There's a Pokemon that may open the door to its heart!"));
				//  Choose a purifiable Pokemon
				//pbChoosePokemon(1, 2, proc {| poke |
				pbChoosePokemon(1, 2, ableProc: poke => //Trainer.party.Select(poke => 
					!poke.isEgg && poke.hp > 0 && poke is IPokemonShadowPokemon p && p.isShadow && p.heartgauge == 0
				);
				if ((int)GameVariables[1] >= 0)
				{
					pbRelicStoneScreen(Trainer.party[(int)GameVariables[1]] as IPokemonShadowPokemon);
				}
			}
			else
			{
				(this as IGameMessage).pbMessage(_INTL("You have no Pokemon that can be purified."));
			}
		}

		public bool pbRaiseHappinessAndReduceHeart(IPokemonShadowPokemon pokemon,IScene scene,int amount)
		{
			if (!pokemon.isShadow)
			{
				scene.pbDisplay(_INTL("It won't have any effect."));
				return false;
			}
			if ((pokemon as IPokemon).Happiness == 255 && pokemon.heartgauge == 0)
			{
				scene.pbDisplay(_INTL("It won't have any effect."));
				return false;
			}
			else if ((pokemon as IPokemon).Happiness == 255)
			{
				pokemon.adjustHeart(-amount);
				scene.pbDisplay(Game._INTL("{1} adores you!\nThe door to its heart opened a little.", (pokemon as IPokemon).name));
				pbReadyToPurify(pokemon);
				return true;
			}
			else if (pokemon.heartgauge == 0)
			{
				(pokemon as IPokemon).ChangeHappiness(HappinessMethods.VITAMIN);
				scene.pbDisplay(_INTL("{1} turned friendly.", (pokemon as IPokemon).name));
				return true;
			}
			else
			{
				(pokemon as IPokemon).ChangeHappiness(HappinessMethods.VITAMIN);
				pokemon.adjustHeart(-amount);
				scene.pbDisplay(_INTL("{1} turned friendly.\nThe door to its heart opened a little.", (pokemon as IPokemon).name));
				pbReadyToPurify(pokemon);
				return true;
			}
		}

		public void pbApplyEVGain(IPokemon pokemon,Stats ev,int evgain)
		{
			int totalev = 0;
			for (int i = 0; i < 6; i++)
			{
				totalev += pokemon.EV[i];
			}
			if (totalev + evgain > Pokemon.EVLIMIT)      // Can't exceed overall limit
			{
				evgain -= totalev + evgain - Pokemon.EVLIMIT;
			}
			if (pokemon.EV[(int)ev] + evgain > Pokemon.EVSTATLIMIT)
			{
				evgain -= totalev + evgain - Pokemon.EVSTATLIMIT;
			}
			if (evgain > 0)
			{
				//pokemon.EV[(int)ev] += evgain;
				pokemon.EV[(int)ev] = (byte)(pokemon.EV[(int)ev] + evgain);
			}
		}

		public void pbReplaceMoves(IPokemon pokemon,Moves move1,Moves move2= 0,Moves move3= 0,Moves move4= 0)
		{
			if (!pokemon.IsNotNullOrNone()) return;
			//new Moves[] { move1, move2, move3, move4 }.each{|move|
			foreach (Moves move in new Moves[] { move1, move2, move3, move4 })
			{
				int moveIndex = -1;
				if (move != 0)
				{
					//  Look for given move
					for (int i = 0; i < 4; i++)
					{
						if (pokemon.moves[i].id == move) moveIndex = i;
					}
				}
				if (moveIndex == -1)
				{
					//  Look for slot to replace move
					for (int i = 0; i < 4; i++)
					{
						if ((pokemon.moves[i].id == 0 && move != 0) || (
							pokemon.moves[i].id != move1 &&
							pokemon.moves[i].id != move2 &&
							pokemon.moves[i].id != move3 &&
							pokemon.moves[i].id != move4)) {
							//  Replace move
							pokemon.moves[i] = new Move(move);
							break;
						}
					}
				}
			}
		}
		#endregion

		public void pbReadyToPurify(IPokemonShadowPokemon pokemon)
		{
			if (!(pokemon as IPokemon).IsNotNullOrNone() || !pokemon.isShadow) return;
			pokemon.pbUpdateShadowMoves();
			if (pokemon.heartgauge == 0)
			{
				(this as IGameMessage).pbMessage(Game._INTL("{1} can now be purified!", (pokemon as IPokemon).name));
			}
		}

		/*Events.onStepTaken+=proc{
			foreach (Pokemon pkmn in Trainer.party) {
				if (pkmn.HP>0 && !pkmn.isEgg && pkmn.heartgauge>0) {
					pkmn.adjustHeart(-1);
					if (pkmn.heartgauge==0) pbReadyToPurify(pkmn);
				}
			}
			if ((Global.purifyChamber!- null)) { //rescue null
				Global.purifyChamber.update();
			}
			for (int i = 0; i< 2; i++) {
				Pokemon pkmn=Global.daycare[i][0];
				if (!pkmn) continue;
				pkmn.adjustHeart(-1);
				pkmn.pbUpdateShadowMoves();
			}
		}*/
	}

	namespace Monster
	{
		public partial class Pokemon : IPokemonShadowPokemon
		{
			#region Variables
			public int? heartgauge { get; protected set; }
			public bool shadow { get; set; }
			public bool hypermode { get; set; }
			public int[] savedev { get; set; }
			public int savedexp { get; set; }
			public Moves[] shadowmoves { get; protected set; }
			public int shadowmovenum { get; protected set; }
			public int heartStage
			{
				get
				{
					if (!@shadow) return 0;
					float hg = HEARTGAUGESIZE / 5.0f;
					return (int)Math.Ceiling(Math.Min(this.heartgauge.Value, HEARTGAUGESIZE) / hg);
				}
			}
			public int Exp
			{
				get
				{
					return this.Experience.Current;
				}
				set 
				{
					if (this.isShadow)
					{
						@savedexp += value - this.Experience.Current;
					}
					else
					{
						//__shadow_expeq(value);
						_Exp = value;
					}
				}
			}

			/// <summary>
			/// Sets this Pokemon's HP;
			/// Changes status on fainted
			/// </summary>
			public int HP
			{
				get { return this.hp; }
				set //ToDo: private set?
				{
					this.hp = value < 0 ? 0 : (value > this.TotalHP ? TotalHP : value);
					//this.hp = (this.HP + value).Clamp(0, this.TotalHP);
					if (isFainted()) 
					{ 
						this.Status = Status.FAINT; 
						StatusCount = 0; 
						ChangeHappiness(HappinessMethods.FAINT);
						@hypermode = false; 
					}
				}
			}

			public const int HEARTGAUGESIZE = 3840;
			int IPokemonShadowPokemon.exp { get; set; }
			int IPokemonShadowPokemon.hp { get; set; }
			#endregion

			#region Shadow
			/// <summary>
			/// Heart Gauge.
			/// The Heart Gauge is split into five equal bars. When a Shadow Pokémon is first snagged, all five bars are full.
			/// </summary>
			/// <remarks>
			/// If pokemon is purified, shadow level should be equal to -1
			/// If pokemon has never been shadowed, then value should be null
			/// HeartGuage max size should be determined by _base.database
			/// </remarks>
			public int? ShadowLevel { get; private set; }
			public int HeartGuageSize { get; private set; }
			public bool IsAbleToPurify { get { return ShadowLevel.HasValue && ShadowLevel.Value == 0 && HeartGuageSize != 0; } }
			public bool IsPurified { get { return (ShadowLevel.HasValue && ShadowLevel.Value == -1) || HeartGuageSize == 0; } }
			/// <summary>
			/// Shadow Pokémon don't have a set Nature or a set gender, but once encountered, the personality value, 
			/// Nature and IVs are saved to the memory for the Shadow Monitor to be able to keep track of their exact status and location. 
			/// This means that once a Shadow Pokémon is encountered for the first time, its Nature, IVs and gender will remain the same for the rest of the game, 
			/// even if the player fails to capture it or is forced to re-battle it later.
			/// </summary>
			public bool isShadow
			{
				get
				{
					//if (!ShadowLevel.HasValue || ShadowLevel.Value == -1) return false;
					//else return true;
					return @heartgauge != null && @heartgauge.Value >= 0 && @shadow;
				}
			}

			public void makeShadow()
			{
				this.shadow = true;
				this.heartgauge = HEARTGAUGESIZE;
				this.savedexp = 0;
				this.savedev = new int[] { 0, 0, 0, 0, 0, 0 };
				this.shadowmoves = new Moves[8];// { 0, 0, 0, 0, 0, 0, 0, 0 };
				//  Retrieve shadow moves
				//moves = load_data("Data/shadowmoves.dat"); //rescue []
				//if (moves[this.Species] && moves[this.Species].Length > 0)
				//{
				//	for (int i = 0; i < Math.Min(4, moves[this.Species].Length); i++)
				//	{
				//		this.shadowmoves[i] = moves[this.Species][i];
				//	}
				//	this.shadowmovenum = moves[this.Species].Length;
				//}
				//else
				//{
				//  No special shadow moves
					this.shadowmoves[0] = Moves.SHADOW_RUSH;// || 0;
					this.shadowmovenum = 1;
				//}
				for (int i = 0; i < 4; i++)
				{   // Save old moves
					this.shadowmoves[4 + i] = this.moves[i].id;
				}
				pbUpdateShadowMoves();
			}

			public void pbUpdateShadowMoves(bool allmoves = false)
			{
				if (@shadowmoves != null)
				{
					Moves[] m = @shadowmoves;
					if (!@shadow)
					{
						//  No shadow moves
						Game.pbReplaceMoves(this, m[4], m[5], m[6], m[7]);
						@shadowmoves = null;
					}
					else
					{
						List<Moves> moves = new List<Moves>();
						int relearning = new int[] { 3, 3, 2, 1, 1, 0 }[heartStage];
						if (allmoves) relearning = 3;
						int relearned = 0;
						//  Add all Shadow moves
						for (int i = 0; i < 4; i++) if (m[i] != 0) moves.Add(m[i]);
						//  Add X regular moves
						for (int i = 0; i < 4; i++)
						{
							if (i < @shadowmovenum) continue;
							if (m[i + 4] != 0 && relearned < relearning)
							{
								moves.Add(m[i + 4]); relearned += 1;
							}
						}
						Game.pbReplaceMoves(this, moves[0] != null ? moves[0] : Moves.NONE, moves[1] != null ? moves[1] : Moves.NONE, moves[2] != null ? moves[2] : Moves.NONE, moves[3] != null ? moves[3] : Moves.NONE);
					}
				}
			}

			public void decreaseShadowLevel(PokemonActions Action)
			{
				int points = 0;

				#region Pokemon Colosseum Shadow Switch
				/* Values from Pokémon Colosseum
				Nature	Battle	Callto	Party	DayCare	Scent
				Adamant 150  	225  	150  	300  	75
				Bashful 50  	300  	75  	450  	200
				Bold  	150  	225  	200  	300  	50
				Brave   200  	225  	150  	225  	75
				Calm  	50  	300  	100  	450  	150
				Careful 75  	300  	75  	450  	150
				Docile  75  	600  	100  	225  	100
				Gentle  50  	300  	75  	600  	150
				Hardy   150  	300  	150  	150  	100
				Hasty   200  	300  	75  	150  	150
				Impish  200  	300  	150  	150  	75
				Jolly   150  	300  	100  	150  	150
				Lax  	100  	225  	150  	225  	150
				Lonely  50  	450  	150  	150  	200
				Mild  	75  	225  	75  	450  	200
				Modest  75  	300  	75  	600  	100
				Naive   100  	300  	150  	225  	100
				Naughty 150  	225  	200  	225  	75
				Quiet   100  	300  	100  	300  	100
				Quirky  200  	225  	50  	600  	75
				Rash  	75  	300  	100  	300  	150
				Relaxed 75  	225  	75  	600  	150
				Sassy   200  	150  	150  	225  	100
				Serious 100  	450  	100  	300  	75
				Timid   50  	450  	50  	600  	150
				*/
				switch (Action)
				{
					case PokemonActions.Battle:
						if (this.Nature == Natures.ADAMANT) points = 150;
						if (this.Nature == Natures.BASHFUL) points = 50;
						if (this.Nature == Natures.BOLD) points = 150;
						if (this.Nature == Natures.BRAVE) points = 200;
						if (this.Nature == Natures.CALM) points = 50;
						if (this.Nature == Natures.CAREFUL) points = 75;
						if (this.Nature == Natures.DOCILE) points = 75;
						if (this.Nature == Natures.GENTLE) points = 50;
						if (this.Nature == Natures.HARDY) points = 150;
						if (this.Nature == Natures.HASTY) points = 200;
						if (this.Nature == Natures.IMPISH) points = 200;
						if (this.Nature == Natures.JOLLY) points = 150;
						if (this.Nature == Natures.LAX) points = 100;
						if (this.Nature == Natures.LONELY) points = 50;
						if (this.Nature == Natures.MILD) points = 75;
						if (this.Nature == Natures.MODEST) points = 75;
						if (this.Nature == Natures.NAIVE) points = 100;
						if (this.Nature == Natures.NAUGHTY) points = 150;
						if (this.Nature == Natures.QUIET) points = 100;
						if (this.Nature == Natures.QUIRKY) points = 200;
						if (this.Nature == Natures.RASH) points = 75;
						if (this.Nature == Natures.RELAXED) points = 75;
						if (this.Nature == Natures.SASSY) points = 200;
						if (this.Nature == Natures.SERIOUS) points = 100;
						if (this.Nature == Natures.TIMID) points = 50;
						break;
					case PokemonActions.CallTo:
						if (this.Nature == Natures.ADAMANT) points = 225;
						if (this.Nature == Natures.BASHFUL) points = 300;
						if (this.Nature == Natures.BOLD) points = 225;
						if (this.Nature == Natures.BRAVE) points = 225;
						if (this.Nature == Natures.CALM) points = 300;
						if (this.Nature == Natures.CAREFUL) points = 300;
						if (this.Nature == Natures.DOCILE) points = 600;
						if (this.Nature == Natures.GENTLE) points = 300;
						if (this.Nature == Natures.HARDY) points = 300;
						if (this.Nature == Natures.HASTY) points = 300;
						if (this.Nature == Natures.IMPISH) points = 300;
						if (this.Nature == Natures.JOLLY) points = 300;
						if (this.Nature == Natures.LAX) points = 225;
						if (this.Nature == Natures.LONELY) points = 450;
						if (this.Nature == Natures.MILD) points = 225;
						if (this.Nature == Natures.MODEST) points = 300;
						if (this.Nature == Natures.NAIVE) points = 300;
						if (this.Nature == Natures.NAUGHTY) points = 225;
						if (this.Nature == Natures.QUIET) points = 300;
						if (this.Nature == Natures.QUIRKY) points = 225;
						if (this.Nature == Natures.RASH) points = 300;
						if (this.Nature == Natures.RELAXED) points = 225;
						if (this.Nature == Natures.SASSY) points = 150;
						if (this.Nature == Natures.SERIOUS) points = 450;
						if (this.Nature == Natures.TIMID) points = 450;
						break;
					case PokemonActions.Party:
						if (this.Nature == Natures.ADAMANT) points = 150;
						if (this.Nature == Natures.BASHFUL) points = 75;
						if (this.Nature == Natures.BOLD) points = 200;
						if (this.Nature == Natures.BRAVE) points = 150;
						if (this.Nature == Natures.CALM) points = 100;
						if (this.Nature == Natures.CAREFUL) points = 75;
						if (this.Nature == Natures.DOCILE) points = 100;
						if (this.Nature == Natures.GENTLE) points = 75;
						if (this.Nature == Natures.HARDY) points = 150;
						if (this.Nature == Natures.HASTY) points = 75;
						if (this.Nature == Natures.IMPISH) points = 150;
						if (this.Nature == Natures.JOLLY) points = 100;
						if (this.Nature == Natures.LAX) points = 150;
						if (this.Nature == Natures.LONELY) points = 150;
						if (this.Nature == Natures.MILD) points = 75;
						if (this.Nature == Natures.MODEST) points = 75;
						if (this.Nature == Natures.NAIVE) points = 150;
						if (this.Nature == Natures.NAUGHTY) points = 200;
						if (this.Nature == Natures.QUIET) points = 100;
						if (this.Nature == Natures.QUIRKY) points = 50;
						if (this.Nature == Natures.RASH) points = 100;
						if (this.Nature == Natures.RELAXED) points = 75;
						if (this.Nature == Natures.SASSY) points = 150;
						if (this.Nature == Natures.SERIOUS) points = 100;
						if (this.Nature == Natures.TIMID) points = 50;
						break;
					case PokemonActions.DayCare:
					case PokemonActions.MysteryAction:
						if (this.Nature == Natures.ADAMANT) points = 300;
						if (this.Nature == Natures.BASHFUL) points = 450;
						if (this.Nature == Natures.BOLD) points = 300;
						if (this.Nature == Natures.BRAVE) points = 225;
						if (this.Nature == Natures.CALM) points = 450;
						if (this.Nature == Natures.CAREFUL) points = 450;
						if (this.Nature == Natures.DOCILE) points = 225;
						if (this.Nature == Natures.GENTLE) points = 600;
						if (this.Nature == Natures.HARDY) points = 150;
						if (this.Nature == Natures.HASTY) points = 150;
						if (this.Nature == Natures.IMPISH) points = 150;
						if (this.Nature == Natures.JOLLY) points = 150;
						if (this.Nature == Natures.LAX) points = 225;
						if (this.Nature == Natures.LONELY) points = 150;
						if (this.Nature == Natures.MILD) points = 450;
						if (this.Nature == Natures.MODEST) points = 600;
						if (this.Nature == Natures.NAIVE) points = 225;
						if (this.Nature == Natures.NAUGHTY) points = 225;
						if (this.Nature == Natures.QUIET) points = 300;
						if (this.Nature == Natures.QUIRKY) points = 600;
						if (this.Nature == Natures.RASH) points = 300;
						if (this.Nature == Natures.RELAXED) points = 600;
						if (this.Nature == Natures.SASSY) points = 225;
						if (this.Nature == Natures.SERIOUS) points = 300;
						if (this.Nature == Natures.TIMID) points = 600;
						break;
					case PokemonActions.Scent:
						if (this.Nature == Natures.ADAMANT) points = 75;
						if (this.Nature == Natures.BASHFUL) points = 200;
						if (this.Nature == Natures.BOLD) points = 50;
						if (this.Nature == Natures.BRAVE) points = 75;
						if (this.Nature == Natures.CALM) points = 150;
						if (this.Nature == Natures.CAREFUL) points = 150;
						if (this.Nature == Natures.DOCILE) points = 100;
						if (this.Nature == Natures.GENTLE) points = 150;
						if (this.Nature == Natures.HARDY) points = 100;
						if (this.Nature == Natures.HASTY) points = 150;
						if (this.Nature == Natures.IMPISH) points = 75;
						if (this.Nature == Natures.JOLLY) points = 150;
						if (this.Nature == Natures.LAX) points = 150;
						if (this.Nature == Natures.LONELY) points = 200;
						if (this.Nature == Natures.MILD) points = 200;
						if (this.Nature == Natures.MODEST) points = 100;
						if (this.Nature == Natures.NAIVE) points = 100;
						if (this.Nature == Natures.NAUGHTY) points = 75;
						if (this.Nature == Natures.QUIET) points = 100;
						if (this.Nature == Natures.QUIRKY) points = 75;
						if (this.Nature == Natures.RASH) points = 150;
						if (this.Nature == Natures.RELAXED) points = 150;
						if (this.Nature == Natures.SASSY) points = 100;
						if (this.Nature == Natures.SERIOUS) points = 75;
						if (this.Nature == Natures.TIMID) points = 150;
						break;
					default:
						break;
				}
				#endregion

				#region Pokemon XD Shadow Switch
				/* Values from Pokémon XD
				Nature	Battle	Callto	Party	???		Scent
				Adamant 110  	270  	110  	300  	80
				Bashful 80  	300  	90  	330  	130
				Bold    110  	270  	90  	300  	100
				Brave   130  	270  	90  	270  	80
				Calm    80  	300  	110  	330  	110
				Careful 90  	300  	100  	330  	110
				Docile  100  	360  	80  	270  	120
				Gentle  70  	300  	130  	360  	100
				Hardy   110  	300  	100  	240  	90
				Hasty   130  	300  	70  	240  	100
				Impish  120  	300  	100  	240  	80
				Jolly   120  	300  	90  	240  	90
				Lax     100  	270  	90  	270  	110
				Lonely  70  	330  	100  	240  	130
				Mild    80  	270  	100  	330  	120
				Modest  70  	300  	120  	360  	110
				Naive   100  	300  	120  	270  	80
				Naughty 120  	270  	110  	270  	70
				Quiet   100  	300  	100  	300  	100
				Quirky  130  	270  	80  	360  	90
				Rash    90  	300  	90  	300  	120
				Relaxed 90  	270  	110  	360  	100
				Sassy   130  	240  	100  	270  	70
				Serious 100  	330  	110  	300  	90
				Timid   70  	330  	110  	360  	120
				*
				switch (Action)
				{
					case pokemonActions.Battle:
						break;
					case pokemonActions.CallTo:
						break;
					case pokemonActions.Party:
						break;
					case pokemonActions.DayCare:
					case pokemonActions.MysteryAction:
						break;
					case pokemonActions.Scent:
						break;
					default:
						break;
				}*/
				#endregion
				if (ShadowLevel.Value > 0)
					ShadowLevel = (ShadowLevel.Value - points) < 0 ? 0 : ShadowLevel.Value - points;
			}

			public void adjustHeart(int value)
			{
				if (@shadow)
				{
					if (@heartgauge == null) @heartgauge = 0;
					@heartgauge += value;
					if (@heartgauge > HEARTGAUGESIZE) @heartgauge = HEARTGAUGESIZE;
					if (@heartgauge < 0) @heartgauge = 0;
				}
			}
			#endregion
		}
	}
}
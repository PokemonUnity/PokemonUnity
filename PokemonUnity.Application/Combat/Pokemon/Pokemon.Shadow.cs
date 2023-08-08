using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;


namespace PokemonUnity.Combat
{
	//ToDo: Rename class to Battler
	public partial class Pokemon : PokemonEssentials.Interface.PokeBattle.IBattlerShadowPokemon
	{
		public void InitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, int pkmnIndex) { //, params object[] placeholder
			if (pokemonIndex>=0 && pkmn is IPokemonShadowPokemon p && inHyperMode() && !isFainted()) { //ToDo: Should move this to an Event Listener based on Battle Menu Selection
				// Called out of hypermode
				p.hypermode=false;
				//p.isHyperMode=false;
				//p.adjustHeart(-50);
				p.decreaseShadowLevel(Monster.PokemonActions.CallTo);
			}
			(this as IBattler).InitPokemon((IPokemon)pkmn, pkmnIndex); //this._InitPokemon(pkmn, pkmnIndex);
			// Called into battle
			if (isShadow()) {
				//if (hasConst(Types.SHADOW))
					Type1=Types.SHADOW;
					Type2=Types.SHADOW;
				//}
				//if (@battle.OwnedByPlayer(@Index)) this.pokemon.adjustHeart(-30);
				if (@battle.OwnedByPlayer(@Index)) (this.pokemon as IPokemonShadowPokemon).decreaseShadowLevel(Monster.PokemonActions.Battle);
			}
		}

		public virtual void EndTurn(IBattleChoice choice) { (this as IBattlerShadowPokemon).EndTurn(choice); }
		void IBattlerShadowPokemon.EndTurn(IBattleChoice choice) { //, params object[] placeholder
			(this as IBattler).EndTurn(choice); //this._EndTurn(choice);
			if (inHyperMode() && !this.battle.AllFainted(this.battle.party1) &&
				!this.battle.AllFainted(this.battle.party2)) {
				this.battle.Display(Game._INTL("Its hyper mode attack hurt {1}!",this.ToString(true)));
				ConfusionDamage();
			}
		}

		public bool isShadow() {
			PokemonEssentials.Interface.PokeBattle.IPokemon pkmn=this.pokemon;
			if (pkmn.IsNotNullOrNone() && pkmn is IPokemonShadowPokemon p && p.heartgauge>0) //p.ShadowLevel.HasValue && p.ShadowLevel.Value>0)
				return p.isShadow;
			return false;
		}

		public bool inHyperMode() {
			if (isFainted()) return false;
			PokemonEssentials.Interface.PokeBattle.IPokemon pkmn=this.pokemon;
			if (pkmn.IsNotNullOrNone() && pkmn is IPokemonShadowPokemon p && p.hypermode)
				return true;
			return false;
		}

		public void HyperMode() {
			PokemonEssentials.Interface.PokeBattle.IPokemon pkmn=this.pokemon;
			if (pkmn is IPokemonShadowPokemon p && isShadow() && !IsHyperMode)
				if (@battle.Random(p.ShadowLevel.Value)<=Monster.Pokemon.HEARTGAUGESIZE/4) { //p.heartgauge
					isHyperMode=true;
					@battle.Display(Game._INTL("{1}'s emotions rose to a fever pitch!\nIt entered Hyper Mode!",this.ToString()));
				}
		}

		public bool HyperModeObedience(IBattleMove move) {
			if (!move.IsNotNullOrNone()) return true;
			if (this.inHyperMode() && move.Type!=Types.SHADOW)
				return @battle.Random(10)<8 ? false : true;
			return true;
		}

		//Events.onStartBattle+=delegate(object sender, EventArgs e) {
		protected void Events_OnStartBattle(object sender, System.EventArgs e) {
			if(Game.GameData.PokemonTemp is ITempMetadataPokemonShadow t)
			{
				//Game.GameData.PokemonTemp.heartgauges=[];
				t.heartgauges=new int?[Game.GameData.Trainer.party.Length];
				for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
					//Game.GameData.PokemonTemp.heartgauges[i]=Game.GameData.Trainer.party[i].heartgauge;
					if (Game.GameData.Trainer.party[i] is IPokemonShadowPokemon p)
						t.heartgauges[i]=p.heartgauge;
				}
			}
		}

		//Events.onEndBattle+=delegate(object sender, EventArgs e) {
		protected void Events_OnEndBattle(object sender, IOnEndBattleEventArgs e) {
			BattleResults decision=e.Decision; //[0];
			bool canlose=e.CanLose; //[1];
			if(Game.GameData.PokemonTemp is ITempMetadataPokemonShadow t)
				for (int i = 0; i < t.heartgauges?.Length; i++) {
					IPokemon pokemon = Game.GameData.Trainer.party[i];
					if (pokemon is IPokemonShadowPokemon p && (t.heartgauges[i].HasValue &&
						t.heartgauges[i]!=0 && p.heartgauge==0)) {
						if (Game.GameData is IGameShadowPokemon g) g.ReadyToPurify(p);
					}
				}
		}
	}

	public partial class Battle : PokemonEssentials.Interface.PokeBattle.IBattleShadowPokemon
	{
		/// <summary>
		/// Uses an item on a Pokémon in the player's party.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pkmnIndex"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		/// <remarks>Specifically for Shadow Pokemon Usage</remarks>
		public bool UseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene)
		{ return (this as IBattleShadowPokemon).UseItemOnPokemon(item, pkmnIndex, userPkmn, scene); }
		bool IBattleShadowPokemon.UseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene)
		{
			IPokemon pokemon = this.party1[pkmnIndex];
			if (pokemon is IPokemonShadowPokemon p && p.hypermode) { //&&
				//item != Items.JOY_SCENT &&
				//item != Items.EXCITE_SCENT &&
				//item != Items.VIVID_SCENT) {
				scene.Display(Game._INTL("This item can't be used on that Pokemon."));
				return false;
			}
			//return _UseItemOnPokemon(item,pkmnIndex,userPkmn,scene);
			return (this as IBattle).UseItemOnPokemon(item, pkmnIndex, (IBattler)userPkmn, scene);
		}
	}
}
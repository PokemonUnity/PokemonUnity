using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.UX
{
	/// <summary>
	/// A Pokemon placeholder class to be used while in-battle,
	/// to prevent changes from being permanent to original pokemon profile
	/// </summary>
	/// <remarks>
	/// This class is called once during battle and persist until end.
	/// Values and variables are overwritten using <see cref="IBattler.pbInitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon, int)"/>
	/// </remarks>
	public partial class Battler : PokemonUnity.Combat.Pokemon, IBattlerIE //ToDo: UnityEngine.MonoBehaviour instead...		
	{
#pragma warning disable 0162 //Warning CS0162  Unreachable code detected
		#region Variables
		new public IBattleIE battle					{ get { return (IBattleIE)base.battle; } }
		#endregion

		#region Constructors
		public Battler(IBattleIE btl, int idx) : base(btl, idx)
		{
			//(this as IBattlerIE).initialize(btl, idx);
		}
		public override void pbInitEffects(bool batonpass)
		{
			if (!batonpass)
			{
				//These effects are retained if Baton Pass is used
				//stages[0]	= 0; // [ATTACK]
				//stages[1]	= 0; // [DEFENSE]
				//stages[2]	= 0; // [SPEED]
				//stages[3]	= 0; // [SPATK]
				//stages[4]	= 0; // [SPDEF]
				//stages[5]	= 0; // [EVASION]
				//stages[6]	= 0; // [ACCURACY]
				stages = new int[7];//Enum.GetValues(typeof(PokemonUnity.Combat.Stats)).Length
				lastMoveUsedSketch 	= Moves.NONE; //-1;
				effects.AquaRing	= false;
				effects.Confusion	= 0;
				effects.Curse		= false;
				effects.Embargo		= 0;
				effects.FocusEnergy = 0;
				effects.GastroAcid  = false;
				effects.HealBlock   = 0;
				effects.Ingrain     = false;
				effects.LeechSeed   = -1;
				effects.LockOn      = 0;
				effects.LockOnPos   = -1;
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					//if (battle.battlers[i].Species == Pokemons.NONE) continue;
					if (battle.battlers[i].IsNotNullOrNone() && //) continue;
						battle.battlers[i].effects.LockOnPos == Index &&
						battle.battlers[i].effects.LockOn > 0)
					{
						battle.battlers[i].effects.LockOn = 0;
						battle.battlers[i].effects.LockOnPos = -1;
					}
				}
				effects.MagnetRise     = 0;
				effects.PerishSong     = 0;
				effects.PerishSongUser = -1;
				effects.PowerTrick     = false;
				effects.Substitute     = 0;
				effects.Telekinesis    = 0;
			}
			else
			{
				if (effects.LockOn>0)
					effects.LockOn=2;
				else
					effects.LockOn=0;
		
				//Moved to Property Getter
				//if (effects.PowerTrick)
				//{
				//	int a = this.ATK;
				//	this.ATK = this.DEF;
				//	this.DEF = this.ATK;
				//	this.DEF = a;
				//}
			}
			damagestate.Reset();
			fainted						= false;
			lastAttacker				= new List<int>();
			lastHPLost					= 0;
			tookDamage					= false;
			lastMoveUsed				= Moves.NONE;
			//lastMoveUsedType			= Types.NONE;
			lastRoundMoved				= -1;
			movesUsed					= new List<Moves>();
			turncount					= 0; //number of turns for battler, not the match battle
			effects.Attract				= -1;
			effects.BatonPass			= false;
			effects.Bide				= 0;
			effects.BideDamage			= 0;
			effects.BideTarget			= -1;
			effects.Charge				= 0;
			effects.ChoiceBand			= Moves.NONE;
			effects.Counter				= -1;
			effects.CounterTarget		= -1;
			effects.DefenseCurl			= false;
			effects.DestinyBond			= false;
			effects.Disable				= 0;
			effects.DisableMove			= 0;
			effects.Electrify			= false;
			effects.Encore				= 0;
			effects.EncoreIndex			= 0;
			effects.EncoreMove			= 0;
			effects.Endure				= false;
			effects.FirstPledge			= 0;
			effects.FlashFire			= false;
			effects.Flinch				= false;
			effects.FollowMe			= 0;
			effects.Foresight			= false;
			effects.FuryCutter			= 0;
			effects.Grudge				= false;
			effects.HelpingHand			= false;
			effects.HyperBeam			= 0;
			effects.Illusion			= null;
			effects.Imprison			= false;
			effects.KingsShield			= false;
			effects.LifeOrb				= false;
			effects.MagicCoat			= false;
			effects.MeanLook			= -1;
			effects.MeFirst				= false;
			effects.Metronome			= 0;
			effects.MicleBerry			= false;
			effects.Minimize			= false;
			effects.MiracleEye			= false;
			effects.MirrorCoat			= -1;
			effects.MirrorCoatTarget	= -1;
			effects.MoveNext			= false;
			effects.MudSport			= false;
			effects.MultiTurn			= 0;
			effects.MultiTurnAttack		= 0;
			effects.MultiTurnUser		= -1;
			effects.Nightmare			= false;
			effects.Outrage				= 0;
			effects.ParentalBond		= 0;
			effects.PickupItem			= 0;
			effects.PickupUse			= 0;
			effects.Pinch				= false;
			effects.Powder				= false;
			effects.Protect				= false;
			effects.ProtectNegation		= false;
			effects.ProtectRate			= 1;
			effects.Pursuit				= false;
			effects.Quash				= false;
			effects.Rage				= false;
			effects.Revenge				= 0;
			effects.Roar				= false;
			effects.Rollout				= 0;
			effects.Roost				= false;
			effects.SkipTurn			= false;
			effects.SkyDrop				= false;
			effects.SmackDown			= false;
			effects.Snatch				= false;
			effects.SpikyShield			= false;
			effects.Stockpile			= 0;
			effects.StockpileDef		= 0;
			effects.StockpileSpDef		= 0;
			effects.Taunt				= 0;
			effects.Torment				= false;
			effects.Toxic				= 0;
			effects.Transform			= false;
			effects.Truant				= false;
			effects.TwoTurnAttack		= 0;
			effects.Type3				= Types.NONE; //-1;
			effects.Unburden			= false;
			effects.Uproar				= 0;
			effects.Uturn				= false;
			effects.WaterSport			= false;
			effects.WeightChange		= 0;
			effects.Yawn				= 0;
			for (int i = 0; i < battle.battlers.Length; i++)
			{
				//if (battle.battlers[i].Species == Pokemons.NONE) continue;
				if (!battle.battlers[i].IsNotNullOrNone()) continue;
				if (battle.battlers[i].effects == null) continue; //ToDo: Initialize on all battlers before here?...
				if (battle.battlers[i].effects.Attract == Index)
				{
					battle.battlers[i].effects.Attract = -1;
				}
				if (battle.battlers[i].effects.MeanLook == Index)
				{
					battle.battlers[i].effects.MeanLook = -1;
				}
				if (battle.battlers[i].effects.MultiTurnUser == Index)
				{
					battle.battlers[i].effects.MultiTurn = 0;
					battle.battlers[i].effects.MultiTurnUser = -1;
				}
			}
			if (this.hasWorkingAbility(Abilities.ILLUSION))
			{
				int lastpoke = battle.pbGetLastPokeInTeam(Index);
				if (lastpoke != pokemonIndex){
					effects.Illusion = battle.pbParty(Index)[lastpoke];
				}
			}
		}
		#endregion

		#region About this Battler
		/*public string ToString(bool lowercase = false)
		{
			if (battle.pbIsOpposing(Index))
			{
				if (battle.opponent.Length == 0)//.ID == TrainerTypes.WildPokemon
					//return string.Format("The wild {0}", Name);
					//return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "WildPokemonL" : "WildPokemon", Name).Value;
					return Game._INTL("The wild {0}", lowercase ? Game._INTL(Species.ToString(TextScripts.Name)).ToLowerInvariant() : Game._INTL(Species.ToString(TextScripts.Name)));
				else
					//return string.Format("The opposing {0}", Name);
					//return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "OpponentPokemonL" : "OpponentPokemon", Name).Value;
					return Game._INTL("The opposing {0}", lowercase ? Game._INTL(Species.ToString(TextScripts.Name)).ToLowerInvariant() : Game._INTL(Species.ToString(TextScripts.Name)));
			}
			else if (battle.pbOwnedByPlayer(Index))
				return Name;
			else
				//return string.Format("The ally {0}", Name);
				//return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "AllyPokemonL" : "AllyPokemon", Name).Value;
				return Game._INTL("The ally {0}", lowercase ? Game._INTL(Species.ToString(TextScripts.Name)).ToLowerInvariant() : Game._INTL(Species.ToString(TextScripts.Name)));
			//return base.ToString();
		}

		public bool hasMoldBreaker() {
			//Pokemon pkmn = this;
			if (this.hasWorkingAbility(Abilities.MOLD_BREAKER) ||
				this.hasWorkingAbility(Abilities.TERAVOLT) ||
				this.hasWorkingAbility(Abilities.TURBOBLAZE)) return true;
			return false;
		}

		public bool hasWorkingAbility(Abilities ability, bool ignorefainted= false) {
			//Pokemon pkmn = this;
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.GastroAcid) return false;
			return this.Ability == ability;
		}

		public bool pbHasType(Types type) {
			if (type == Types.NONE || type < 0) return false;
			bool ret = (this.Type1 == type || this.Type2 == type);
			if (effects.Type3 >= 0) ret |= (effects.Type3 == type);
			return ret;
		}

		public bool pbHasMoveType(Types type) {
			if (type == Types.NONE || type < 0) return false;
			for (int i = 0; i < moves.Length; i++)
			{
				if (moves[i].Type == type) return true;
			}
			return false;
		}

		//public bool HasMoveFunction(string code) {
		//	if (string.IsNullOrEmpty(code)) return false;
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		if (moves[i].FunctionAsString == code) return true;
		//	}
		//	return false;
		//}
		//
		//public bool HasMoveFunction(short code) {
		//	//if (string.IsNullOrEmpty(code)) return false;
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		if ((short)moves[i].Function == code) return true;
		//	}
		//	return false;
		//}
		//
		//public bool HasMoveFunction(Move.Effect code) {
		//	//if (string.IsNullOrEmpty(code)) return false;
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		if ((Move.Effect)moves[i].Function == code) return true;
		//	}
		//	return false;
		//}

		public bool hasMovedThisRound() {
			if (!lastRoundMoved.HasValue) return false;
			return lastRoundMoved.Value == battle.turncount;
		}
		//ToDo: Double check this
		public bool hasMovedThisRound(int? turncount){
			if (!lastRoundMoved.HasValue) return false;
			return !turncount.HasValue
				? lastRoundMoved.Value == battle.turncount
				: lastRoundMoved.Value == turncount.Value;
		}

		public bool hasWorkingItem(Items item, bool ignorefainted= false) {
			//Pokemon pkmn = null;
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			return this.Item == item;
		}

		public bool hasWorkingBerry(bool ignorefainted= false) {
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			return Kernal.ItemData[this.Item].IsBerry;//.Pocket == ItemPockets.BERRY;//pbIsBerry?(@item)
		}

		public bool isAirborne(bool ignoreability=false){
			if (this.hasWorkingItem(Items.IRON_BALL)) return false;
			if (effects.Ingrain) return false;
			if (effects.SmackDown) return false;
			if (battle.field.Gravity > 0) return false;
			if (this.pbHasType(Types.FLYING) && !effects.Roost) return true;
			if (this.hasWorkingAbility(Abilities.LEVITATE) && !ignoreability) return true;
			if (this.hasWorkingItem(Items.AIR_BALLOON)) return true;
			if (effects.MagnetRise > 0) return true;
			if (effects.Telekinesis > 0) return true;
			return false;
		}*/
		#endregion

		#region Change HP
		public IEnumerator pbReduceHP(int amt, bool animate = false, bool registerDamage = true, System.Action<int> result = null)
		{
			if (amt >= HP)
				amt = HP;
			else if (amt < 1 && !isFainted())
				amt = 1;
			int oldhp = HP;
			HP -= amt;
			if (HP < 0)
				//yield return battle.pbDisplay(Game._INTL("HP less than 0"));
				//yield return battle.pbDisplay(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
				GameDebug.LogWarning("HP less than 0");
			if (HP > TotalHP)
				//yield return battle.pbDisplay(Game._INTL("HP greater than total HP"));
				//yield return battle.pbDisplay(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
				GameDebug.LogWarning("HP greater than total HP");
			if (amt > 0)
				//battle.scene.HPChanged(Index, oldhp, animate);
				if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbHPChanged(this, oldhp, animate);
			if (amt > 0 && registerDamage)
				tookDamage = true;
			result?.Invoke(amt);
		}

		public IEnumerator pbRecoverHP(int amount, bool animate = false, System.Action<int> result = null)
		{
			// the checks here are redundant, cause they're also placed on HP { set; }
			if (HP + amount > TotalHP)
				amount = TotalHP - HP;
			else if (amount < 1 && HP != TotalHP)
				amount = 1;
			int oldhp = HP;
			HP += amount;
			if (HP < 0)
				//yield return battle.pbDisplay(Game._INTL("HP less than 0"));
				//yield return battle.pbDisplay(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
				GameDebug.LogWarning("HP less than 0");
			if (HP > TotalHP)
				//yield return battle.pbDisplay(Game._INTL("HP greater than total HP"));
				//yield return battle.pbDisplay(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
				GameDebug.LogWarning("HP greater than total HP");
			if(amount > 0)
				//battle.scene.HPChanged(Index, oldhp, animate);
				if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbHPChanged(this, oldhp, animate);
			//ToDo: Fix return
			result?.Invoke(amount);
		}

		public IEnumerator pbFaint(bool showMessage = true, System.Action<bool> result = null)
		{
			if(!isFainted() && HP > 0)
			{
				GameDebug.LogWarning("Can't faint with HP greater than 0");
				result?.Invoke(true);
				yield break;
			}
			if(isFainted())
			{
				GameDebug.LogWarning("Can't faint if already fainted");
				result?.Invoke(true);
				yield break;
			}
			if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbFainted(this);
			pbInitEffects(false);
			// Reset status
			//Status = 0;
			StatusCount = 0;
			if (pokemon != null && battle.internalbattle)
				(pokemon as Monster.Pokemon).ChangeHappiness(HappinessMethods.FAINT);
			if (isMega) { 
				//Change form to before transformation
				if (pokemon is IPokemonMegaEvolution m) m.makeUnmega();
				form = pokemon is IPokemonMultipleForms f ? f.form : 0;
			}
			if (isPrimal) { 
				//Change form to before transformation
				if (pokemon is IPokemonMegaEvolution m) m.makeUnprimal();
				form = pokemon is IPokemonMultipleForms f ? f.form : 0;
			}
			HP = 0;
			fainted = true;
			Status = Status.FAINT;
			//reset choice
			battle.choices[Index] = new Choice(ChoiceAction.NoAction);
			OwnSide.LastRoundFainted = battle.turncount;
			if (showMessage)
				yield return battle.pbDisplay(Game._INTL("{1} fainted!", ToString()));
				//yield return battle.pbDisplay(LanguageExtension.Translate(Text.Errors, "Fainted", new string[] { ToString() }).Value);
			result?.Invoke(true);
		}
		#endregion

		#region Find other battlers/sides in relation to this battler
		/*// <summary>
		/// Returns the data structure for this battler's side
		/// </summary>
		/// Player: 0 and 2; Foe: 1 and 3
		public IEffectsSide OwnSide { get { return battle.sides[Index&1]; } }
		public IEffectsSide pbOwnSide { get { return OwnSide; } }
		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// Player: 1 and 3; Foe: 0 and 2
		public IEffectsSide pbOpposingSide { get { return battle.sides[(Index&1)^1]; } }
		/// <summary>
		/// Returns whether the position belongs to the opposing Pokémon's side
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public bool pbIsOpposing(int i)
		{
			return (Index & 1) != (i & 1);
		}*/
		/// <summary>
		/// Returns the battler's partner
		/// </summary>
		new public IBattlerIE Partner { get { return (IBattlerIE)base.Partner; } }
		new public IBattlerIE pbPartner { get { return Partner; } }
		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		new public IBattlerIE pbOppositeOpposing { get { return (IBattlerIE)base.pbOppositeOpposing; } }
		//ToDo: If not double battle return null?
		new public IBattlerIE pbOppositeOpposing2 { get { return (IBattlerIE)base.pbOppositeOpposing2; } }
		new public IBattlerIE pbOpposing1 { get { return (IBattlerIE)base.pbOpposing1; } }
		//ToDo: If not double battle return null?
		new public IBattlerIE pbOpposing2 { get { return (IBattlerIE)base.pbOpposing2; } }
		/*// <summary>
		/// Returns the battler's first opposing Pokémon Index
		/// </summary>
		public int OpposingIndex { get { return (Index ^ 1) | ((Index & 2) ^ 2); } }
		public int pbNonActivePokemonCount
		{
			get
			{
				int count = 0;
				PokemonEssentials.Interface.PokeBattle.IPokemon[] party = battle.pbParty(Index);
				for (int i = 0; i < party.Length; i++)
				{
					if (battle.doublebattle)
						if ((isFainted() || i != pokemonIndex) &&
							(Partner.isFainted() || i != Partner.pokemonIndex) &&
							party[i].IsNotNullOrNone() && !party[i].isEgg && party[i].HP > 0)
								count += 1;
					else
						if ((isFainted() || i != pokemonIndex) &&
							party[i].IsNotNullOrNone() && !party[i].isEgg && party[i].HP > 0)
								count += 1;
				}
				return count;
			}
		}*/
		#endregion

		#region Forms
		/// <summary>
		/// </summary>
		/// ToDo: Changes stats on form changes here?
		/// ToDo: Use PokemonUnity.Battle.Form to modify Pokemon._base,
		/// Which will override and modify base stats for values that inherit it
		/// Castform and Unown should use (int)Form, and others will use (PokemonData)Form
		new public IEnumerator pbCheckForm()
		{
			if (effects.Transform) yield break;
			if (isFainted()) yield break;
			bool transformed = false;
			//Forecast
			if (Species == Pokemons.CASTFORM)
			{
				if (hasWorkingAbility(Abilities.FORECAST))
				{
					switch (battle.pbWeather)
					{
						case Weather.SUNNYDAY:
						case Weather.HARSHSUN:
							if(Form.Id != Monster.Forms.CASTFORM_SUNNY)
							{
								form = 1;
								transformed = true;
							}
							break;
						case Weather.RAINDANCE:
						case Weather.HEAVYRAIN:
							if(Form.Id != Monster.Forms.CASTFORM_RAINY)
							{
								form = 2;
								transformed = true;
							}
							break;
						case Weather.HAIL:
							if(Form.Id != Monster.Forms.CASTFORM_SNOWY)
							{
								form = 3;
								transformed = true;
							}
							break;
						case Weather.NONE:
						default:
							if(Form.Id != Monster.Forms.CASTFORM)
							{
								form = 0;
								transformed = true; //Shouldn't normal be false?
							}
							break;
					}
				} else
					if(Form.Id != Monster.Forms.CASTFORM)
					{
						form = 0;
						transformed = true; //Shouldn't normal be false?
					}
			}
			if (Species == Pokemons.SHAYMIN)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.GIRATINA)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.ARCEUS && Ability == Abilities.MULTITYPE)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.DARMANITAN)
			{
				if(hasWorkingAbility(Abilities.ZEN_MODE) && HP <= Math.Floor(TotalHP/2f))
					if (Form.Id != Monster.Forms.DARMANITAN_ZEN)
					{
						form = 1;
						transformed = true;
					}
				else
					if (Form.Id != Monster.Forms.DARMANITAN_STANDARD)
					{
						form = 0;
						transformed = true;
					}
			}
			if (Species == Pokemons.KELDEO)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.GENESECT)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (transformed)
			{
				pbUpdate(true);
				if (@battle.scene is IPokeBattle_SceneIE s0) 
					//s0.ChangePokemon();
					s0.pbChangePokemon(this, @pokemon);
				yield return battle.pbDisplay(Game._INTL("{1} transformed!", ToString()));
				//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "Transformed", ToString()).Value);
				GameDebug.Log(string.Format("[Form changed] {0} changed to form {1}", ToString(), Game._INTL(Form.Id.ToString(TextScripts.Name))));
			}
		}
		/*public void pbResetForm()
		{
			if (!effects.Transform){
				if (Species == Pokemons.CASTFORM ||
					Species == Pokemons.CHERRIM ||
					Species == Pokemons.DARMANITAN ||
					Species == Pokemons.MELOETTA ||
					Species == Pokemons.AEGISLASH ||
					Species == Pokemons.XERNEAS)
					form = 0;
			}
			pbUpdate(true);
		}*/
		#endregion

		#region Ability Effects
		new public IEnumerator pbAbilitiesOnSwitchIn(bool onactive)
		{
			if (isFainted()) yield break;
			//if (onactive)
			//	battle.PrimalReversion(Index);
			#region Weather
			if (onactive)
			{
				if(hasWorkingAbility(Abilities.PRIMORDIAL_SEA) && battle.pbWeather != Weather.HEAVYRAIN)
				{
					battle.weather = (Weather.HEAVYRAIN);
					battle.weatherduration = -1;
					battle.pbCommonAnimation("HeavyRain", null, null);
					yield return battle.pbDisplay(Game._INTL("{1}'s {2} made a heavy rain begin to fall!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HeavyRainStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameDebug.Log(string.Format("[Ability triggered] {0}'s Primordial Sea made it rain heavily", ToString()));
				}
				if(hasWorkingAbility(Abilities.DESOLATE_LAND) && battle.pbWeather != Weather.HARSHSUN)
				{
					battle.weather = (Weather.HARSHSUN);
					battle.weatherduration = -1;
					battle.pbCommonAnimation("HarshSun", null, null);
					yield return battle.pbDisplay(Game._INTL("{1}'s {2} turned the sunlight extremely harsh!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HarshSunStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameDebug.Log(string.Format("[Ability triggered] {0}'s Desolate Land made the sun shine harshly", ToString()));
				}
				if(hasWorkingAbility(Abilities.DELTA_STREAM) && battle.pbWeather != Weather.STRONGWINDS)
				{
					battle.weather = (Weather.STRONGWINDS);
					battle.weatherduration = -1;
					battle.pbCommonAnimation("StrongWinds", null, null);
					yield return battle.pbDisplay(Game._INTL("{1}'s {2} caused a mysterious air current that protects Flying-type Pokémon!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "StrongWindsStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameDebug.Log(string.Format("[Ability triggered] {0}'s Delta Stream made an air current blow", ToString()));
				}
				if (battle.pbWeather != Weather.HEAVYRAIN &&
					battle.pbWeather != Weather.HARSHSUN &&
					battle.pbWeather != Weather.STRONGWINDS)
				{
					if (hasWorkingAbility(Abilities.DRIZZLE) &&
						(battle.pbWeather != Weather.RAINDANCE || battle.weatherduration != -1))
					{
						battle.weather = (Weather.RAINDANCE);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.DAMP_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Rain", null, null);
						yield return battle.pbDisplay(Game._INTL("{1}'s {2} made it rain!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "RainStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Drizzle made it rain", ToString()));
					}
					if (hasWorkingAbility(Abilities.DROUGHT) &&
						(battle.pbWeather != Weather.SUNNYDAY || battle.weatherduration != -1))
					{
						battle.weather = (Weather.SUNNYDAY);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.HEAT_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Sunny", null, null);
						//Output Below: 
						yield return battle.pbDisplay(Game._INTL("{1}'s {2} intensified the sun's rays!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SunnyStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Drought made it sunny", ToString()));
					}
					if (hasWorkingAbility(Abilities.SAND_STREAM) &&
						(battle.pbWeather != Weather.SANDSTORM || battle.weatherduration != -1))
					{
						battle.weather = (Weather.SANDSTORM);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.SMOOTH_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Sandstorm", null, null);
						yield return battle.pbDisplay(Game._INTL("{1}'s {2} whipped up a sandstorm!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SandstormStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Sand Stream made it sandstorm", ToString()));
					}
					if (hasWorkingAbility(Abilities.SNOW_WARNING) &&
						(battle.pbWeather != Weather.HAIL || battle.weatherduration != -1))
					{
						battle.weather = (Weather.HAIL);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.ICY_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Hail", null, null);
						yield return battle.pbDisplay(Game._INTL("{1}'s {2} made it hail!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HailStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Snow Warning made it hail", ToString()));
					}
				}
				if(hasWorkingAbility(Abilities.AIR_LOCK) || hasWorkingAbility(Abilities.CLOUD_NINE))
				{
					battle.weather = (Weather.NONE);
					battle.weatherduration = 0;
					yield return battle.pbDisplay(Game._INTL("{1} has {2}!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HasAbility", ToString(), Ability.ToString().Translate().Value).Value);
					yield return battle.pbDisplay(Game._INTL("The effects of the weather disappeared."));
					//yield return battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "WeatherNullified").Value);
					GameDebug.Log(string.Format("[Ability nullified] {0}'s Ability cancelled weather effects", ToString()));
				}
			}
			//battle.PrimordialWeather();
			#endregion Weather
			#region Trace
			if (hasWorkingAbility(Abilities.TRACE))
			{
				//IBattleChoice[] choices = new IBattleChoice[4];
				List<int> choices = new List<int>();
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					IBattlerIE foe = battle.battlers[i];
					if (pbIsOpposing(i) && !foe.isFainted())
					{
						Abilities abil = foe.Ability;
						if (abil > 0 &&
							abil != Abilities.TRACE &&
							abil != Abilities.MULTITYPE &&
							abil != Abilities.ILLUSION &&
							abil != Abilities.FLOWER_GIFT &&
							abil != Abilities.IMPOSTER &&
							abil != Abilities.STANCE_CHANGE)
							choices.Add(i);
					}
				}
				if (choices.Count > 0)
				{
					int choice = choices[@battle.pbRandom(choices.Count)];
					string battlername = @battle.battlers[choice].ToString(true);
					Abilities battlerability = @battle.battlers[choice].Ability;
					Ability = battlerability;
					string abilityname = battlerability.ToString();
					yield return @battle.pbDisplay(Game._INTL("{1} traced {2}'s {3}!", ToString(), battlername, abilityname));
					GameDebug.Log($"[Ability triggered] #{ToString()}'s Trace turned into #{abilityname} from #{battlername}");
				}
			}
			#endregion Trace
			#region Intimidate
			if (this.hasWorkingAbility(Abilities.INTIMIDATE) && onactive) {
				GameDebug.Log($"[Ability triggered] #{ToString()}'s Intimidate");
				for (int i = 0; i < 4; i++)
				if (pbIsOpposing(i) && !@battle.battlers[i].isFainted() && @battle.battlers[i] is IBattlerEffectIE b)
					b.pbReduceAttackStatIntimidate(this);
			}
			#endregion Intimidate
			#region Download
			if (this.hasWorkingAbility(Abilities.DOWNLOAD) && onactive) {
				int odef = 0; int ospdef = 0;
				if (pbOpposing1.IsNotNullOrNone() && !pbOpposing1.isFainted()) {
					odef+=pbOpposing1.DEF;
					ospdef+=pbOpposing1.SPD;
				}
				if (pbOpposing2.IsNotNullOrNone() && !pbOpposing2.isFainted()) {
					odef+=pbOpposing2.DEF;
					ospdef+=pbOpposing1.SPD;
				}
				if (ospdef > odef)
				{
					bool increaseStatWithCause = false;
					yield return pbIncreaseStatWithCause(Stats.ATTACK, 1, this, Game._INTL(Ability.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
					if (increaseStatWithCause)
					//if (pbIncreaseStatWithCause(Stats.ATTACK,1,this,Game._INTL(ability.ToString(TextScripts.Name))))
						GameDebug.Log($"[Ability triggered] #{ToString()}'s Download (raising Attack)");
					else
					{
						yield return pbIncreaseStatWithCause(Stats.SPATK, 1, this, Game._INTL(Ability.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
						if (increaseStatWithCause)
						//if (pbIncreaseStatWithCause(Stats.SPATK,1,this,Game._INTL(ability.ToString(TextScripts.Name))))
							GameDebug.Log($"[Ability triggered] #{ToString()}'s Download (raising Special Attack)");
					}
				}
			}
			#endregion Download
			#region Frisk
			if (this.hasWorkingAbility(Abilities.FRISK) && @battle.pbOwnedByPlayer(@Index) && onactive) {
				List<IBattlerIE> foes= new List<IBattlerIE>();
				if (pbOpposing1.Item>0 && !pbOpposing1.isFainted()) foes.Add(pbOpposing1);
				if (pbOpposing2.Item>0 && !pbOpposing2.isFainted()) foes.Add(pbOpposing2);
				if (Core.USENEWBATTLEMECHANICS) {
					if (foes.Count>0) GameDebug.Log($"[Ability triggered] #{ToString()}'s Frisk");
					foreach (var i in foes) {
						string itemname=Game._INTL(i.Item.ToString(TextScripts.Name));
						yield return @battle.pbDisplay(Game._INTL("{1} frisked {2} and found its {3}!",ToString(),i.ToString(true),itemname));
					}
				} else if (foes.Count>0) {
					GameDebug.Log($"[Ability triggered] #{ToString()}'s Frisk");
					IBattlerIE foe=foes[@battle.pbRandom(foes.Count)];
					string itemname=Game._INTL(foe.Item.ToString(TextScripts.Name));
					yield return @battle.pbDisplay(Game._INTL("{1} frisked the foe and found one {2}!",ToString(),itemname));
				}
			}
			#endregion Frisk
			#region Anticipation
			if (this.hasWorkingAbility(Abilities.ANTICIPATION) && @battle.pbOwnedByPlayer(@Index) && onactive) {
				GameDebug.Log($"[Ability triggered] #{ToString()} has Anticipation");
				bool found=false;
				foreach (var foe in new IBattlerIE[] { pbOpposing1, pbOpposing2 }) {
					if (foe.isFainted()) continue;
					foreach (var j in foe.moves) {
						Attack.Data.MoveData movedata=Kernal.MoveData[j.id];
						TypeEffective eff=movedata.Type.GetCombinedEffectiveness(Type1,Type2,@effects.Type3);
						if ((movedata.Power>0 && eff == TypeEffective.SuperEffective) ||
							(movedata.Effect== Attack.Data.Effects.x027 && eff != TypeEffective.Ineffective)) { // OHKO
							found=true;
							break;
						}
					}
					if (found) break;
				}
				if (found) yield return @battle.pbDisplay(Game._INTL("{1} shuddered with anticipation!",ToString()));
			}
			#endregion Anticipation
			#region Forewarn
			if (this.hasWorkingAbility(Abilities.FOREWARN) && @battle.pbOwnedByPlayer(@Index) && onactive) {
				GameDebug.Log($"[Ability triggered] #{ToString()} has Forewarn");
				int highpower=0;
				List<Moves> fwmoves= new List<Moves>();
				foreach (var foe in new IBattlerIE[] { pbOpposing1, pbOpposing2 }) {
					if (foe.isFainted()) continue;
					foreach (var j in foe.moves) {
						Attack.Data.MoveData movedata=Kernal.MoveData[j.id];
						int power=movedata.Power??0;
						if (movedata.Effect == Attack.Data.Effects.x027) power=160;    // OHKO
						if (movedata.Effect == Attack.Data.Effects.x0BF) power=150;    // Eruption
						if (movedata.Effect == Attack.Data.Effects.x05A || // Counter
									movedata.Effect == Attack.Data.Effects.x091 || // Mirror Coat
									movedata.Effect == Attack.Data.Effects.x0E4) power=120;// || // Metal Burst
						if (movedata.Effect == Attack.Data.Effects.x083 ||  // SonicBoom
									movedata.Effect == Attack.Data.Effects.x02A ||  // Dragon Rage
									movedata.Effect == Attack.Data.Effects.x058 ||  // Night Shade
									movedata.Effect == Attack.Data.Effects.x0BE ||  // Endeavor
									movedata.Effect == Attack.Data.Effects.x059 ||  // Psywave
									movedata.Effect == Attack.Data.Effects.x07A ||  // Return
									movedata.Effect == Attack.Data.Effects.x07C ||  // Frustration
									movedata.Effect == Attack.Data.Effects.x0EE ||  // Crush Grip
									movedata.Effect == Attack.Data.Effects.x0DC ||  // Gyro Ball
									movedata.Effect == Attack.Data.Effects.x088 ||  // Hidden Power
									movedata.Effect == Attack.Data.Effects.x0DF ||  // Natural Gift
									movedata.Effect == Attack.Data.Effects.x0EC ||  // Trump Card
									movedata.Effect == Attack.Data.Effects.x064 ||  // Flail
									movedata.Effect == Attack.Data.Effects.x0C5) power=80;     // Grass Knot
						if (power > highpower) {
							fwmoves=new List<Moves>() { j.id }; highpower=power;
						} else if (power==highpower)
							fwmoves.Add(j.id);
					}
				}
				if (fwmoves.Count>0) {
					Moves fwmove=fwmoves[@battle.pbRandom(fwmoves.Count)];
					string movename=Game._INTL(fwmove.ToString(TextScripts.Name));
					yield return @battle.pbDisplay(Game._INTL("{1}'s Forewarn alerted it to {2}!",ToString(),movename));
				}
			}
			#endregion Forewarn
			// Pressure message
			if (this.hasWorkingAbility(Abilities.PRESSURE) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} is exerting its pressure!",ToString()));
			// Mold Breaker message
			if (this.hasWorkingAbility(Abilities.MOLD_BREAKER) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} breaks the mold!",ToString()));
			// Turboblaze message
			if (this.hasWorkingAbility(Abilities.TURBOBLAZE) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} is radiating a blazing aura!",ToString()));
			// Teravolt message
			if (this.hasWorkingAbility(Abilities.TERAVOLT) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} is radiating a bursting aura!",ToString()));
			// Dark Aura message
			if (this.hasWorkingAbility(Abilities.DARK_AURA) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} is radiating a dark aura!",ToString()));
			// Fairy Aura message
			if (this.hasWorkingAbility(Abilities.FAIRY_AURA) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} is radiating a fairy aura!",ToString()));
			// Aura Break message
			if (this.hasWorkingAbility(Abilities.AURA_BREAK) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} reversed all other Pokémon's auras!",ToString()));
			// Imposter
			if (this.hasWorkingAbility(Abilities.IMPOSTER) && !@effects.Transform && onactive) {
				IBattlerIE choice=pbOppositeOpposing;
				List<Attack.Data.Effects> blacklist=new List<Attack.Data.Effects>() {
					Attack.Data.Effects.x09C,	// Fly
					Attack.Data.Effects.x101,	// Dig
					Attack.Data.Effects.x100,	// Dive
					Attack.Data.Effects.x108,	// Bounce
					Attack.Data.Effects.x138,	// Sky Drop
					//Attack.Data.Effects.x111,	// Shadow Force
					Attack.Data.Effects.x111	// Phantom Force
				};
				if (choice.effects.Transform ||
					choice.effects.Illusion.IsNotNullOrNone() ||
					choice.effects.Substitute>0 ||
					choice.effects.SkyDrop ||
					blacklist.Contains(Kernal.MoveData[choice.effects.TwoTurnAttack].Effect))
					GameDebug.Log($"[Ability triggered] #{ToString()}'s Imposter couldn't transform");
				else {
					GameDebug.Log($"[Ability triggered] #{ToString()}'s Imposter");
					@battle.pbAnimation(Moves.TRANSFORM,this,choice);
					@effects.Transform=true;
					@Type1=choice.Type1;
					@Type2=choice.Type2;
					@effects.Type3=Types.NONE;
					Ability=choice.Ability;
					@attack=choice.ATK;
					@defense=choice.DEF;
					@speed=choice.SPE;
					@spatk=choice.SPA;
					@spdef=choice.SPD;
					foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
								Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION })
						@stages[(int)i]=choice.stages[(int)i];
					for (int i = 0; i < 4; i++) {
						@moves[i]=Move.pbFromPBMove(@battle,new Attack.Move(choice.moves[i].id));
						@moves[i].PP=5;
						//@moves[i].TotalPP=5;
					}
					@effects.Disable=0;
					@effects.DisableMove=0;
					yield return @battle.pbDisplay(Game._INTL("{1} transformed into {2}!",ToString(),choice.ToString(true)));
					GameDebug.Log($"[Pokémon transformed] #{ToString()} transformed into #{choice.ToString(true)}");
				}
			}
			// Air Balloon message
			if (this.hasWorkingItem(Items.AIR_BALLOON) && onactive)
				yield return @battle.pbDisplay(Game._INTL("{1} floats in the air with its {2}!",ToString(),Game._INTL(this.Item.ToString(TextScripts.Name))));
		}
		public IEnumerator pbEffectsOnDealingDamage(IBattleMove move, IBattlerIE user,IBattlerIE target,int damage) {
			Types movetype=move.pbType(move.Type,user,target);
			if (damage>0 && move.Flags.Contact)
				if (!target.damagestate.Substitute) {
					if (target.hasWorkingItem(Items.STICKY_BARB,true) && user.Item==0 && !user.isFainted()) {
						user.Item=target.Item;
						target.Item=0;
						target.effects.Unburden=true;
						if (@battle.opponent.Length == 0 && !@battle.pbIsOpposing(user.Index))
							if (user.pokemon.itemInitial==Items.NONE && target.pokemon.itemInitial==user.Item) {
								user.pokemon.itemInitial=user.Item;
								target.pokemon.itemInitial=Items.NONE;
							}
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} was transferred to {3}!",
							target.ToString(),Game._INTL(user.Item.ToString(TextScripts.Name)),user.ToString(true)));
						GameDebug.Log($"[Item triggered] #{target.ToString()}'s Sticky Barb moved to #{user.ToString(true)}");
					}
					if (target.hasWorkingItem(Items.ROCKY_HELMET,true) && !user.isFainted())
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Item triggered] #{target.ToString()}'s Rocky Helmet");
							if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbDamageAnimation(user,0);
							yield return user.pbReduceHP((int)Math.Floor(user.TotalHP/6d));
							yield return @battle.pbDisplay(Game._INTL("{1} was hurt by the {2}!",user.ToString(),
								Game._INTL(target.Item.ToString(TextScripts.Name))));
						}
					if (target.hasWorkingAbility(Abilities.AFTERMATH,true) && target.isFainted() &&
						!user.isFainted())
						if (!@battle.pbCheckGlobalAbility(Abilities.DAMP).IsNotNullOrNone() &&
							!user.hasMoldBreaker() && !user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Aftermath");
							if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbDamageAnimation(user,0);
							yield return user.pbReduceHP((int)Math.Floor(user.TotalHP/4d));
							yield return @battle.pbDisplay(Game._INTL("{1} was caught in the aftermath!",user.ToString()));
						}
					if (target.hasWorkingAbility(Abilities.CUTE_CHARM) && @battle.pbRandom(10)<3)
						if (!user.isFainted() && user is IBattlerEffectIE u) {
							bool canAttract = false;
							yield return u.pbCanAttract(target,false,result:value=>canAttract=value);
							if (canAttract) {
								GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Cute Charm");
								yield return u.pbAttract(target,Game._INTL("{1}'s {2} made {3} fall in love!",target.ToString(),
									Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
							}
						}
					if (target.hasWorkingAbility(Abilities.EFFECT_SPORE,true) && @battle.pbRandom(10)<3)
						if (Core.USENEWBATTLEMECHANICS &&
							(user.pbHasType(Types.GRASS) ||
							user.hasWorkingAbility(Abilities.OVERCOAT) ||
							user.hasWorkingItem(Items.SAFETY_GOGGLES))) { //Not sure what goes here
						} else {
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Effect Spore");
							switch (@battle.pbRandom(3)) {
								case 0:
									if (user is IBattlerEffectIE b0) {
										bool canPoison = false; 
										yield return b0.pbCanPoison(null,false,result:value=>canPoison=value);
										if (canPoison)
											yield return b0.pbPoison(target,Game._INTL("{1}'s {2} poisoned {3}!",target.ToString(),
												Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
									}
									break;
								case 1:
									if (user is IBattlerClause b1 && b1.pbCanSleep(null,false))
										if (b1 is IBattlerEffectIE b1a) yield return b1a.pbSleep(Game._INTL("{1}'s {2} made {3} fall asleep!",target.ToString(),
											Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
									break;
								case 2:
									if (user is IBattlerEffectIE b2) {
										bool pbCanParalyze = false; 
										yield return b2.pbCanParalyze(null,false,result:value=>pbCanParalyze=value);
										if (pbCanParalyze)
											yield return b2.pbParalyze(target,Game._INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
												target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
									}
									break;
							}
						}
					if (target.hasWorkingAbility(Abilities.FLAME_BODY,true) && @battle.pbRandom(10)<3 &&
						user is IBattlerEffectIE u0) {
						bool canBurn = false; 
						yield return u0.pbCanBurn(null,false,result:value=>canBurn=value);
						if (canBurn) { 
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Flame Body");
							yield return u0.pbBurn(target,Game._INTL("{1}'s {2} burned {3}!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
						}
					}
					if (target.hasWorkingAbility(Abilities.MUMMY,true) && !user.isFainted())
						if (user.Ability != Abilities.MULTITYPE &&
							user.Ability != Abilities.STANCE_CHANGE &&
							user.Ability != Abilities.MUMMY) {
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Mummy copied onto #{user.ToString(true)}");
							user.Ability=Abilities.MUMMY;// || 0;
							yield return @battle.pbDisplay(Game._INTL("{1} was mummified by {2}!",
								user.ToString(),target.ToString(true)));
						}
					if (target.hasWorkingAbility(Abilities.POISON_POINT,true) && @battle.pbRandom(10)<3 &&
						user is IBattlerEffectIE u1) {
						bool canPoison = false; 
						yield return u1.pbCanPoison(null,false,result:value=>canPoison=value);
						if (canPoison) { 
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Poison Point");
							yield return u1.pbPoison(target,Game._INTL("{1}'s {2} poisoned {3}!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
						}
					}
					if ((target.hasWorkingAbility(Abilities.ROUGH_SKIN,true) ||
						target.hasWorkingAbility(Abilities.IRON_BARBS,true)) && !user.isFainted())
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s #{Game._INTL(target.Ability.ToString(TextScripts.Name))}");
							if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbDamageAnimation(user,0);
							yield return user.pbReduceHP((int)Math.Floor(user.TotalHP/8d));
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} hurt {3}!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
						}
					if (target.hasWorkingAbility(Abilities.STATIC,true) && @battle.pbRandom(10)<3 &&
						user is IBattlerEffectIE u2) {
						bool canParalyze = false; 
						yield return u2.pbCanParalyze(null,false,result:value=>canParalyze=value);
						if (canParalyze) { 
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Static");
							yield return u2.pbParalyze(target,Game._INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
								target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
						}
					}
					if (target.hasWorkingAbility(Abilities.GOOEY,true))
						if (user is IBattlerEffectIE u3) { 
							bool reduceStatWithCause = false; 
							yield return u3.pbReduceStatWithCause(Stats.SPEED,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name)),result:value=>reduceStatWithCause=value);
							if (reduceStatWithCause) 
								GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Gooey");
						}
					if (user.hasWorkingAbility(Abilities.POISON_TOUCH,true) &&
						target is IBattlerEffectIE t && @battle.pbRandom(10)<3) {
						bool canPoison = false; 
						yield return t.pbCanPoison(null,false,result:value=>canPoison=value);
						if (canPoison) { 
							GameDebug.Log($"[Ability triggered] #{user.ToString()}'s Poison Touch");
							yield return t.pbPoison(user,Game._INTL("{1}'s {2} poisoned {3}!",user.ToString(),
								Game._INTL(user.Ability.ToString(TextScripts.Name)),target.ToString(true)));
						}
					}
				}
			if (damage>0) {
				if (!target.damagestate.Substitute) {
					bool increaseStatWithCause = false; 
					if (target.hasWorkingAbility(Abilities.CURSED_BODY,true) && @battle.pbRandom(10)<3)
						if (user.effects.Disable<=0 && move.PP>0 && !user.isFainted()) {
							user.effects.Disable=3;
							user.effects.DisableMove=move.id;
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} disabled {3}!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Cursed Body disabled #{user.ToString(true)}");
						}
					if (target.hasWorkingAbility(Abilities.JUSTIFIED) && movetype == Types.DARK)
						if (target is IBattlerEffectIE t) { 
							yield return t.pbIncreaseStatWithCause(Stats.ATTACK,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause)
								GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Justified");
						}
					if (target.hasWorkingAbility(Abilities.RATTLED) &&
						(movetype == Types.BUG ||
						movetype == Types.DARK ||
						movetype == Types.GHOST))
						if (target is IBattlerEffectIE t) { 
							yield return t.pbIncreaseStatWithCause(Stats.SPEED,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause)
								GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Rattled");
						}
					if (target.hasWorkingAbility(Abilities.WEAK_ARMOR) && move.pbIsPhysical(movetype)) {
						if (target is IBattlerEffectIE t0) { 
							bool reduceStatWithCause = false; 
							yield return t0.pbReduceStatWithCause(Stats.DEFENSE,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name)),result:value=>reduceStatWithCause=value);
							if (reduceStatWithCause)
								GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Weak Armor (lower Defense)");
						}
						if (target is IBattlerEffectIE t1) { 
							yield return t1.pbIncreaseStatWithCause(Stats.SPEED,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause)
								GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Weak Armor (raise Speed)");
						}
					}
					if (target.hasWorkingItem(Items.AIR_BALLOON,true)) {
						GameDebug.Log($"[Item triggered] #{target.ToString()}'s Air Balloon popped");
						yield return @battle.pbDisplay(Game._INTL("{1}'s Air Balloon popped!",target.ToString()));
						yield return target.pbConsumeItem(true,false);
					} else if (target.hasWorkingItem(Items.ABSORB_BULB) && movetype == Types.WATER)
						if (target is IBattlerEffectIE t0) {
							yield return t0.pbIncreaseStatWithCause(Stats.SPATK,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause) { 
								GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{Game._INTL(target.Item.ToString(TextScripts.Name))}");
								yield return target.pbConsumeItem();
							}
						}
					else if (target.hasWorkingItem(Items.LUMINOUS_MOSS) && movetype == Types.WATER)
						if (target is IBattlerEffectIE t1) {
							yield return t1.pbIncreaseStatWithCause(Stats.SPDEF,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause) { 
								GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{Game._INTL(target.Item.ToString(TextScripts.Name))}");
								yield return target.pbConsumeItem();
							}
						}
					else if (target.hasWorkingItem(Items.CELL_BATTERY) && movetype == Types.ELECTRIC)
						if (target is IBattlerEffectIE t2) {
							yield return t2.pbIncreaseStatWithCause(Stats.ATTACK,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause) { 
								GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{Game._INTL(target.Item.ToString(TextScripts.Name))}");
								yield return target.pbConsumeItem();
							}
						}
					else if (target.hasWorkingItem(Items.SNOWBALL) && movetype == Types.ICE)
						if (target is IBattlerEffectIE t3) {
							yield return t3.pbIncreaseStatWithCause(Stats.ATTACK,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause) { 
								GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{Game._INTL(target.Item.ToString(TextScripts.Name))}");
								yield return target.pbConsumeItem();
							}
						}
					else if (target.hasWorkingItem(Items.WEAKNESS_POLICY) && target.damagestate.TypeMod>8) {
						bool showanim=true;
						if (target is IBattlerEffectIE t4) {
							yield return t4.pbIncreaseStatWithCause(Stats.ATTACK,2,target,Game._INTL(target.Item.ToString(TextScripts.Name)),showanim,result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause) { 
								GameDebug.Log($"[Item triggered] #{target.ToString()}'s Weakness Policy (Attack)");
								showanim=false;
							}
						}
						if (target is IBattlerEffectIE t5) {
							yield return t5.pbIncreaseStatWithCause(Stats.SPATK,2,target,Game._INTL(target.Item.ToString(TextScripts.Name)),showanim,result:value=>increaseStatWithCause=value);
							if (increaseStatWithCause) { 
								GameDebug.Log($"[Item triggered] #{target.ToString()}'s Weakness Policy (Special Attack)");
								showanim=false;
							}
						}
						if (!showanim) yield return target.pbConsumeItem();
					} else if (target.hasWorkingItem(Items.ENIGMA_BERRY) && target.damagestate.TypeMod>8)
						yield return target.pbActivateBerryEffect();
					else if ((target.hasWorkingItem(Items.JABOCA_BERRY) && move.pbIsPhysical(movetype)) ||
							(target.hasWorkingItem(Items.ROWAP_BERRY) && move.pbIsSpecial(movetype)))
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD) && !user.isFainted()) {
							GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{Game._INTL(target.Item.ToString(TextScripts.Name))}");
							if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbDamageAnimation(user,0);
							yield return user.pbReduceHP((int)Math.Floor(user.TotalHP/8d));
							yield return @battle.pbDisplay(Game._INTL("{1} consumed its {2} and hurt {3}!",target.ToString(),
								Game._INTL(target.Item.ToString(TextScripts.Name)),user.ToString(true)));
							yield return target.pbConsumeItem();
						}
					else if (target.hasWorkingItem(Items.KEE_BERRY) && move.pbIsPhysical(movetype))
						yield return target.pbActivateBerryEffect();
					else if (target.hasWorkingItem(Items.MARANGA_BERRY) && move.pbIsSpecial(movetype))
						yield return target.pbActivateBerryEffect();
				}
				if (target.hasWorkingAbility(Abilities.ANGER_POINT))
					if (target.damagestate.Critical && !target.damagestate.Substitute &&
						target is IBattlerEffectIE t) {
						bool canIncreaseStatStage = false; 
						yield return t.pbCanIncreaseStatStage(Stats.ATTACK,target,result:value=>canIncreaseStatStage=value);
						if (canIncreaseStatStage) { 
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Anger Point");
							target.stages[(int)Stats.ATTACK]=6;
							yield return @battle.pbCommonAnimation("StatUp",target,null);
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} maxed its {3}!",
								target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name)),Game._INTL(Stats.ATTACK.ToString(TextScripts.Name))));
						}
					}
			}
			yield return user.pbAbilityCureCheck();
			yield return target.pbAbilityCureCheck();
		}
		public IEnumerator pbEffectsAfterHit(IBattlerIE user,IBattlerIE target,IBattleMove thismove,IEffectsMove turneffects) {
			if (turneffects.TotalDamage==0) yield break;
			if (!(user.hasWorkingAbility(Abilities.SHEER_FORCE) && thismove.AddlEffect>0)) {
				bool canSwitch = false;
				yield return @battle.pbCanSwitch(user.Index,-1,false,result:value=>canSwitch=value);
				// Target's held items:
				// Red Card
				if (target.hasWorkingItem(Items.RED_CARD) && canSwitch) {
					user.effects.Roar=true;
					yield return @battle.pbDisplay(Game._INTL("{1} held up its {2} against the {3}!",
						target.ToString(),Game._INTL(target.Item.ToString(TextScripts.Name)),user.ToString(true)));
					yield return target.pbConsumeItem();
					// Eject Button
				} else if (target.hasWorkingItem(Items.EJECT_BUTTON) && @battle.pbCanChooseNonActive(target.Index)) {
					target.effects.Uturn=true;
					yield return @battle.pbDisplay(Game._INTL("{1} is switched out with the {2}!",
						target.ToString(),Game._INTL(target.Item.ToString(TextScripts.Name))));
					yield return target.pbConsumeItem();
				}
				// User's held items:
				// Shell Bell
				if (user.hasWorkingItem(Items.SHELL_BELL) && user.effects.HealBlock==0) {
					GameDebug.Log($"[Item triggered] #{user.ToString()}'s Shell Bell (total damage=#{turneffects.TotalDamage})");
					int hpgain=0;
					yield return user.pbRecoverHP((int)Math.Floor(turneffects.TotalDamage/8d),true,result:value=>hpgain=value);
					if (hpgain>0)
						yield return @battle.pbDisplay(Game._INTL("{1} restored a little HP using its {2}!",
							user.ToString(),Game._INTL(user.Item.ToString(TextScripts.Name))));
				}
				// Life Orb
				if (user.effects.LifeOrb && !user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					GameDebug.Log($"[Item triggered] #{user.ToString()}'s Life Orb (recoil)");
					int hploss=0;
					yield return user.pbReduceHP((int)Math.Floor(user.TotalHP/10d),true,result:value=>hploss=value);
					if (hploss>0)
						yield return @battle.pbDisplay(Game._INTL("{1} lost some of its HP!",user.ToString()));
				}
				if (user.isFainted()) user.pbFaint(); // no return
				// Color Change
				Types movetype=thismove.pbType(thismove.Type,user,target);
				if (target.hasWorkingAbility(Abilities.COLOR_CHANGE) &&
					!target.pbHasType(movetype)) {//!PBTypes.isPseudoType(movetype) &&
					GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Color Change made it #{Game._INTL(movetype.ToString(TextScripts.Name))}-type");
					target.Type1=movetype;
					target.Type2=movetype;
					target.effects.Type3=Types.NONE;
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} made it the {3} type!",target.ToString(),
						Game._INTL(target.Ability.ToString(TextScripts.Name)),Game._INTL(movetype.ToString(TextScripts.Name))));
				}
			}
			// Moxie
			if (user.hasWorkingAbility(Abilities.MOXIE) && target.isFainted())
				if (user is IBattlerEffectIE u) {
					bool increaseStatWithCause = false;
					yield return u.pbIncreaseStatWithCause(Stats.ATTACK,1,user,Game._INTL(user.Ability.ToString(TextScripts.Name)),result:value=>increaseStatWithCause=value);
					if (increaseStatWithCause)
						GameDebug.Log($"[Ability triggered] #{user.ToString()}'s Moxie");
				}
			// Magician
			if (user.hasWorkingAbility(Abilities.MAGICIAN))
				if (target.Item>0 && user.Item==0 &&
					user.effects.Substitute==0 &&
					target.effects.Substitute==0 &&
					!target.hasWorkingAbility(Abilities.STICKY_HOLD) &&
					!@battle.pbIsUnlosableItem(target,target.Item) &&
					!@battle.pbIsUnlosableItem(user,target.Item) &&
					(@battle.opponent.Length > 0 || !@battle.pbIsOpposing(user.Index))) {
					user.Item=target.Item;
					target.Item=0;
					target.effects.Unburden=true;
					if (@battle.opponent.Length != 0 &&   // In a wild battle
						user.pokemon.itemInitial==Items.NONE &&
						target.pokemon.itemInitial==user.Item) {
						user.pokemon.itemInitial=user.Item;
						target.pokemon.itemInitial=Items.NONE;
					}
					yield return @battle.pbDisplay(Game._INTL("{1} stole {2}'s {3} with {4}!",user.ToString(),
						target.ToString(true),Game._INTL(user.Item.ToString(TextScripts.Name)),Game._INTL(user.Ability.ToString(TextScripts.Name))));
					GameDebug.Log($"[Ability triggered] #{user.ToString()}'s Magician stole #{target.ToString(true)}'s #{Game._INTL(user.Item.ToString(TextScripts.Name))}");
				}
			// Pickpocket
			if (target.hasWorkingAbility(Abilities.PICKPOCKET))
				if (target.Item==0 && user.Item>0 &&
					user.effects.Substitute==0 &&
					target.effects.Substitute==0 &&
					!user.hasWorkingAbility(Abilities.STICKY_HOLD) &&
					!@battle.pbIsUnlosableItem(user,user.Item) &&
					!@battle.pbIsUnlosableItem(target,user.Item) &&
					(@battle.opponent.Length > 0 || !@battle.pbIsOpposing(target.Index))) {
					target.Item=user.Item;
					user.Item=0;
					user.effects.Unburden=true;
					if (@battle.opponent.Length != 0 &&   // In a wild battle
						target.pokemon.itemInitial==Items.NONE &&
						user.pokemon.itemInitial==target.Item) {
						target.pokemon.itemInitial=target.Item;
						user.pokemon.itemInitial=Items.NONE;
					}
					yield return @battle.pbDisplay(Game._INTL("{1} pickpocketed {2}'s {3}!",target.ToString(),
						user.ToString(true),Game._INTL(target.Item.ToString(TextScripts.Name))));
					GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pickpocket stole #{user.ToString(true)}'s #{Game._INTL(target.Item.ToString(TextScripts.Name))}");
				}
			}
		new public IEnumerator pbAbilityCureCheck() {
			if (this.isFainted()) yield break;
			switch (Status) {
				case Status.SLEEP:
					if (this.hasWorkingAbility(Abilities.VITAL_SPIRIT) || this.hasWorkingAbility(Abilities.INSOMNIA)) {
						GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))}");
						yield return pbCureStatus(false);
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} woke it up!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.POISON:
					if (this.hasWorkingAbility(Abilities.IMMUNITY)) {
						GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))}");
						yield return pbCureStatus(false);
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its poisoning!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.BURN:
					if (this.hasWorkingAbility(Abilities.WATER_VEIL)) {
						GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))}");
						yield return pbCureStatus(false);
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} healed its burn!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.PARALYSIS:
					if (this.hasWorkingAbility(Abilities.LIMBER)) {
						GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))}");
						yield return pbCureStatus(false);
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its paralysis!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.FROZEN:
					if (this.hasWorkingAbility(Abilities.MAGMA_ARMOR)) {
						GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))}");
						yield return pbCureStatus(false);
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} defrosted it!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
					}
					break;
			}
			if (@effects.Confusion>0 && this.hasWorkingAbility(Abilities.OWN_TEMPO)) {
				GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))} (attract)");
				pbCureConfusion(false);
				yield return @battle.pbDisplay(Game._INTL("{1}'s {2} snapped it out of its confusion!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
			}
			if (@effects.Attract>=0 && this.hasWorkingAbility(Abilities.OBLIVIOUS)) {
				GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))}");
				pbCureAttract();
				yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its infatuation status!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
			}
			if (Core.USENEWBATTLEMECHANICS && @effects.Taunt>0 && this.hasWorkingAbility(Abilities.OBLIVIOUS)) {
				GameDebug.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(Ability.ToString(TextScripts.Name))} (taunt)");
				@effects.Taunt=0;
				yield return @battle.pbDisplay(Game._INTL("{1}'s {2} made its taunt wear off!",ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
			}
		}
		#endregion

		#region Held Item effects
		new public IEnumerator pbConsumeItem(bool recycle=true,bool pickup=true) {
			string itemname=Game._INTL(this.Item.ToString(TextScripts.Name));
			if (recycle) itemRecycle=this.Item;
			if (itemInitial==this.Item) itemInitial=0;
			if (pickup) {
				@effects.PickupItem=this.Item;
				@effects.PickupUse=@battle.nextPickupUse;
			}
			this.Item=0;
			this.effects.Unburden=true;
			// Symbiosis
			if (Partner.IsNotNullOrNone() && Partner.hasWorkingAbility(Abilities.SYMBIOSIS) && recycle)
				if (Partner.Item>0 &&
					!@battle.pbIsUnlosableItem(Partner,Partner.Item) &&
					!@battle.pbIsUnlosableItem(this,Partner.Item)) {
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} let it share its {3} with {4}!",
						Partner.ToString(),Game._INTL(Partner.Ability.ToString(TextScripts.Name)),
						Game._INTL(Partner.Item.ToString(TextScripts.Name)),ToString(true)));
					this.Item=Partner.Item;
					Partner.Item=0;
					Partner.effects.Unburden=true;
					yield return pbBerryCureCheck();
				}
		}
		public IEnumerator pbConfusionBerry(Inventory.Plants.Flavours flavor,string message1,string message2,System.Action<bool> result) {
			int amt=0;
			yield return this.pbRecoverHP((int)Math.Floor(this.TotalHP/8d),true,result:value=>amt=value);
			if (amt>0) {
				yield return @battle.pbDisplay(message1);
				//if ((this.Nature%5)==flavor && (int)Math.Floor(this.Nature/5d)!=(this.Nature%5)) {
				if (Kernal.NatureData[pokemon.Nature].Dislikes==flavor) {
					yield return @battle.pbDisplay(message2);
					pbConfuseSelf();
				}
				result(true);
				yield break;
			}
			result(false);
		}
		public IEnumerator pbStatIncreasingBerry(Stats stat,string berryname,System.Action<bool> result) {
			yield return pbIncreaseStatWithCause(stat,1,this,berryname, result: value => result(value));
		}
		new public IEnumerator pbActivateBerryEffect(Items berry=Items.NONE,bool consume=true) {
			if (berry==0) berry=this.Item;
			string berryname=(berry==0) ? "" : Game._INTL(berry.ToString(TextScripts.Name));
			GameDebug.Log($"[Item triggered] #{ToString()}'s #{berryname}");
			bool consumed=false;
			if (berry == Items.ORAN_BERRY) {
				int amt=0;
				yield return this.pbRecoverHP(10,true,result:value=>amt=value);
				if (amt>0) {
					yield return @battle.pbDisplay(Game._INTL("{1} restored its health using its {2}!",ToString(),berryname));
					consumed=true;
				}
			} else if (berry == Items.SITRUS_BERRY ||
					berry == Items.ENIGMA_BERRY) {
				int amt=0;
				yield return this.pbRecoverHP((int)Math.Floor(this.TotalHP/4d),true,result:value=>amt=value);
				if (amt>0) {
					yield return @battle.pbDisplay(Game._INTL("{1} restored its health using its {2}!",ToString(),berryname));
					consumed=true;
				}
			} else if (berry == Items.CHESTO_BERRY)
				if (Status==Status.SLEEP) {
					pbCureStatus(false);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its sleep problem.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.PECHA_BERRY)
				if (Status==Status.POISON) {
					pbCureStatus(false);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its poisoning.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.RAWST_BERRY)
				if (Status==Status.BURN) {
					pbCureStatus(false);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} healed its burn.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.CHERI_BERRY)
				if (Status==Status.PARALYSIS) {
					pbCureStatus(false);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its paralysis.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.ASPEAR_BERRY)
				if (Status==Status.FROZEN) {
					pbCureStatus(false);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} thawed it out.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.LEPPA_BERRY) {
				List<int> found= new List<int>();
				for (int i = 0; i < @pokemon.moves.Length; i++)
					if (@pokemon.moves[i].id!=0)
						if ((consume && @pokemon.moves[i].PP==0) ||
							(!consume && @pokemon.moves[i].PP<@pokemon.moves[i].TotalPP))
							found.Add(i);
				if (found.Count>0) {
					int choice=(consume) ? found[0] : found[@battle.pbRandom(found.Count)];
					IMove pokemove=@pokemon.moves[choice];
					pokemove.PP+=10;
					if (pokemove.PP > pokemove.TotalPP) pokemove.PP=pokemove.TotalPP;
					this.moves[choice].PP=pokemove.PP;
					string movename=Game._INTL(pokemove.id.ToString(TextScripts.Name));
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} restored {3}'s PP!",ToString(),berryname,movename)) ;
					consumed=true;
				}
			} else if (berry == Items.PERSIM_BERRY)
				if (@effects.Confusion>0) {
					pbCureConfusion(false);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} snapped it out of its confusion!",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.LUM_BERRY)
				if (Status>0 || @effects.Confusion>0) {
					Status st=Status; bool conf=(@effects.Confusion>0);
					pbCureStatus(false);
					pbCureConfusion(false);
					switch (st) {
						case Status.SLEEP:
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} woke it up!",ToString(),berryname));
							break;
						case Status.POISON:
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its poisoning!",ToString(),berryname));
							break;
						case Status.BURN:
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} healed its burn!",ToString(),berryname));
							break;
						case Status.PARALYSIS:
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} cured its paralysis!",ToString(),berryname));
							break;
						case Status.FROZEN:
							yield return @battle.pbDisplay(Game._INTL("{1}'s {2} defrosted it!",ToString(),berryname));
							break;
					}
					if (conf)
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} snapped it out of its confusion!",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.FIGY_BERRY)
				yield return pbConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too spicy!",ToString(true),berryname), result: value=>consumed=value);
			else if (berry == Items.WIKI_BERRY)
				yield return pbConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too dry!",ToString(true),berryname), result: value=>consumed=value);
			else if (berry == Items.MAGO_BERRY)
				yield return pbConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too sweet!",ToString(true),berryname), result: value=>consumed=value);
			else if (berry == Items.AGUAV_BERRY)
				yield return pbConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too bitter!",ToString(true),berryname), result: value=>consumed=value);
			else if (berry == Items.IAPAPA_BERRY)
				yield return pbConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too sour!",ToString(true),berryname), result: value=>consumed=value);
			else if (berry == Items.LIECHI_BERRY)
				yield return pbStatIncreasingBerry(Stats.ATTACK,berryname, result: value=>consumed=value);
			else if (berry == Items.GANLON_BERRY ||
					berry == Items.KEE_BERRY)
				yield return pbStatIncreasingBerry(Stats.DEFENSE,berryname, result: value=>consumed=value);
			else if (berry == Items.SALAC_BERRY)
				yield return pbStatIncreasingBerry(Stats.SPEED,berryname, result: value=>consumed=value);
			else if (berry == Items.PETAYA_BERRY)
				yield return pbStatIncreasingBerry(Stats.SPATK,berryname, result: value=>consumed=value);
			else if (berry == Items.APICOT_BERRY ||
					berry == Items.MARANGA_BERRY)
				yield return pbStatIncreasingBerry(Stats.SPDEF,berryname, result: value=>consumed=value);
			else if (berry == Items.LANSAT_BERRY)
				if (@effects.FocusEnergy<2) {
					@effects.FocusEnergy=2;
					yield return @battle.pbDisplay(Game._INTL("{1} used its {2} to get pumped!",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.MICLE_BERRY)
				if (!@effects.MicleBerry) {
				@effects.MicleBerry=true;
				yield return @battle.pbDisplay(Game._INTL("{1} boosted the accuracy of its next move using its {2}!",
					ToString(),berryname));
				consumed=true;
				}
			else if (berry == Items.STARF_BERRY) {
				List<Stats> stats= new List<Stats>();
				foreach (Stats i in new Stats[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPATK, Stats.SPDEF, Stats.SPEED })
				{
					bool canIncreaseStatStage = false;
					yield return pbCanIncreaseStatStage(i,this, result: value=>canIncreaseStatStage=value);
					if (canIncreaseStatStage) stats.Add(i);
					//if (pbCanIncreaseStatStage(i,this)) stats.Add(i);
				}
				if (stats.Count>0) {
					Stats stat=stats[@battle.pbRandom(stats.Count)];
					yield return pbIncreaseStatWithCause(stat,2,this,berryname, result: value=>consumed=value);
				}
			}
			if (consumed) {
				// Cheek Pouch
				if (hasWorkingAbility(Abilities.CHEEK_POUCH)) {
					int amt=0;
					yield return this.pbRecoverHP((int)Math.Floor(@TotalHP/3d),true,result:value=>amt=value);
					if (amt>0)
						yield return @battle.pbDisplay(Game._INTL("{1}'s {2} restored its health!",
							ToString(),Game._INTL(Ability.ToString(TextScripts.Name))));
				}
				if (consume) yield return pbConsumeItem();
				if (this.pokemon.IsNotNullOrNone()) this.belch=true;
			}
		}
		new public IEnumerator pbBerryCureCheck(bool hpcure=false) {
			if (this.isFainted()) yield break;
			bool unnerver = false;
			if (battle.doublebattle)
				unnerver = (pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) || pbOpposing2.hasWorkingAbility(Abilities.UNNERVE));
			else
				unnerver = (pbOpposing1.hasWorkingAbility(Abilities.UNNERVE));
			string itemname=(this.Item==0) ? "" : Game._INTL(this.Item.ToString(TextScripts.Name));
			if (hpcure)
				if (this.hasWorkingItem(Items.BERRY_JUICE) && this.HP<= (int)Math.Floor(this.TotalHP/2d)) {
					int amt=0;
					yield return this.pbRecoverHP(20,true,result:value=>amt=value);
					if (amt>0) {
						@battle.pbCommonAnimation("UseItem",this,null);
						yield return @battle.pbDisplay(Game._INTL("{1} restored its health using its {2}!",ToString(),itemname));
						yield return pbConsumeItem();
						yield break;
					}
				}
			if (!unnerver) {
				if (hpcure) {
					if (this.HP<= (int)Math.Floor(this.TotalHP/2d)) {
						if (this.hasWorkingItem(Items.ORAN_BERRY) ||
							this.hasWorkingItem(Items.SITRUS_BERRY)) {
							yield return pbActivateBerryEffect();
							yield break;
						}
						if (this.hasWorkingItem(Items.FIGY_BERRY) ||
							this.hasWorkingItem(Items.WIKI_BERRY) ||
							this.hasWorkingItem(Items.MAGO_BERRY) ||
							this.hasWorkingItem(Items.AGUAV_BERRY) ||
							this.hasWorkingItem(Items.IAPAPA_BERRY)) {
							yield return pbActivateBerryEffect();
							yield break;
						}
					}
				} //<= Not sure if this should move to 1930
				if ((this.hasWorkingAbility(Abilities.GLUTTONY) && this.HP<= (int)Math.Floor(this.TotalHP/2d)) ||
					this.HP<= (int)Math.Floor(this.TotalHP/4d)) {
					if (this.hasWorkingItem(Items.LIECHI_BERRY) ||
						this.hasWorkingItem(Items.GANLON_BERRY) ||
						this.hasWorkingItem(Items.SALAC_BERRY) ||
						this.hasWorkingItem(Items.PETAYA_BERRY) ||
						this.hasWorkingItem(Items.APICOT_BERRY)) {
						yield return pbActivateBerryEffect();
						yield break;
					}
					if (this.hasWorkingItem(Items.LANSAT_BERRY) ||
						this.hasWorkingItem(Items.STARF_BERRY)) {
						yield return pbActivateBerryEffect();
						yield break;
					}
					if (this.hasWorkingItem(Items.MICLE_BERRY)) {
						yield return pbActivateBerryEffect();
						yield break;
					}
				}
				if (this.hasWorkingItem(Items.LEPPA_BERRY)) {
					yield return pbActivateBerryEffect();
					yield break;
				}
				if (this.hasWorkingItem(Items.CHESTO_BERRY) ||
					this.hasWorkingItem(Items.PECHA_BERRY) ||
					this.hasWorkingItem(Items.RAWST_BERRY) ||
					this.hasWorkingItem(Items.CHERI_BERRY) ||
					this.hasWorkingItem(Items.ASPEAR_BERRY) ||
					this.hasWorkingItem(Items.PERSIM_BERRY) ||
					this.hasWorkingItem(Items.LUM_BERRY)) {
					yield return pbActivateBerryEffect();
					yield break;
				}
			}
			if (this.hasWorkingItem(Items.WHITE_HERB)) {
				bool reducedstats=false;
				foreach (Stats i in new Stats[]{ Stats.ATTACK,Stats.DEFENSE,
						Stats.SPEED,Stats.SPATK,Stats.SPDEF,
						Stats.ACCURACY,Stats.EVASION })
					if (@stages[(int)i]<0) {
						@stages[(int)i]=0; reducedstats=true; 
					}
				if (reducedstats) {
					GameDebug.Log($"[Item triggered] #{ToString()}'s #{itemname}");
					yield return @battle.pbCommonAnimation("UseItem",this,null);
					yield return @battle.pbDisplay(Game._INTL("{1} restored its status using its {2}!",ToString(),itemname));
					yield return pbConsumeItem();
					yield break;
				}
			}
			if (this.hasWorkingItem(Items.MENTAL_HERB) &&
				(@effects.Attract>=0 ||
				@effects.Taunt>0 ||
				@effects.Encore>0 ||
				@effects.Torment ||
				@effects.Disable>0 ||
				@effects.HealBlock>0)) {
				GameDebug.Log($"[Item triggered] #{ToString()}'s #{itemname}");
				yield return @battle.pbCommonAnimation("UseItem",this,null);
				if (@effects.Attract>=0) yield return @battle.pbDisplay(Game._INTL("{1} cured its infatuation status using its {2}.",ToString(),itemname));
				if (@effects.Taunt>0) yield return @battle.pbDisplay(Game._INTL("{1}'s taunt wore off!",ToString()));
				if (@effects.Encore>0) yield return @battle.pbDisplay(Game._INTL("{1}'s encore ended!",ToString()));
				if (@effects.Torment) yield return @battle.pbDisplay(Game._INTL("{1}'s torment wore off!",ToString()));
				if (@effects.Disable>0) yield return @battle.pbDisplay(Game._INTL("{1} is no longer disabled!",ToString()));
				if (@effects.HealBlock>0) yield return @battle.pbDisplay(Game._INTL("{1}'s Heal Block wore off!",ToString()));
				this.pbCureAttract();
				@effects.Taunt=0;
				@effects.Encore=0;
				@effects.EncoreMove=0;
				@effects.EncoreIndex=0;
				@effects.Torment=false;
				@effects.Disable=0;
				@effects.HealBlock=0;
				yield return pbConsumeItem();
				yield break;
			}
			if (hpcure && this.hasWorkingItem(Items.LEFTOVERS) && this.HP!=this.TotalHP &&
				@effects.HealBlock==0) {
				GameDebug.Log($"[Item triggered] #{ToString()}'s Leftovers");
				yield return @battle.pbCommonAnimation("UseItem",this,null);
				yield return pbRecoverHP((int)Math.Floor(this.TotalHP/16d),true);
				yield return @battle.pbDisplay(Game._INTL("{1} restored a little HP using its {2}!",ToString(),itemname));
			}
			if (hpcure && this.hasWorkingItem(Items.BLACK_SLUDGE)) {
				if (pbHasType(Types.POISON))
					if (this.HP!=this.TotalHP &&
						(!Core.USENEWBATTLEMECHANICS || @effects.HealBlock==0)) {
						GameDebug.Log($"[Item triggered] #{ToString()}'s Black Sludge (heal)");
						yield return @battle.pbCommonAnimation("UseItem",this,null);
						yield return pbRecoverHP((int)Math.Floor(this.TotalHP/16d),true);
						yield return @battle.pbDisplay(Game._INTL("{1} restored a little HP using its {2}!",ToString(),itemname));
					}
				else if (!this.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					GameDebug.Log($"[Item triggered] #{ToString()}'s Black Sludge (damage)");
					yield return @battle.pbCommonAnimation("UseItem",this,null);
					yield return pbReduceHP((int)Math.Floor(this.TotalHP/8d),true);
					yield return @battle.pbDisplay(Game._INTL("{1} was hurt by its {2}!",ToString(),itemname));
				}
				if (this.isFainted()) yield return pbFaint();
			}
		}
		#endregion

		#region Move user and targets
		public IBattlerIE pbFindUser(IBattleChoice choice,IList<IBattlerIE> targets) {//ToDo: ref IList<IBattler> targets) {
			IBattleMove move=choice.Move;
			int target=choice.Target;
			IBattlerIE user = this;   // Normally, the user is this
			// Targets in normal cases
			switch (pbTarget(move)) { //ToDo: Missing `Select everyone` (including user)
				case Attack.Data.Targets.SELECTED_POKEMON: //Attack.Target.SingleNonUser:
				case Attack.Data.Targets.SELECTED_POKEMON_ME_FIRST: 
					if (target>=0) {
						IBattlerIE targetBattler=@battle.battlers[target];
						if (!pbIsOpposing(targetBattler.Index))
							if (!pbAddTarget(ref targets,targetBattler))
								if (!pbAddTarget(ref targets,pbOpposing1)) pbAddTarget(ref targets,pbOpposing2);
						else
							if (!pbAddTarget(ref targets,targetBattler)) pbAddTarget(ref targets,targetBattler.pbPartner);
					}
					else
						pbRandomTarget(ref targets);
					break;
				//case Attack.Data.Targets.SELECTED_POKEMON: //Attack.Target.SingleOpposing:
				//case Attack.Data.Targets.SELECTED_POKEMON_ME_FIRST: 
				//	if (target>=0) {
				//		IBattlerIE targetBattler=@battle.battlers[target];
				//		if (!IsOpposing(targetBattler.Index))
				//			if (!pbAddTarget(targets,targetBattler))
				//				if (!pbAddTarget(targets,pbOpposing1)) pbAddTarget(targets,pbOpposing2);
				//		else
				//			if (!pbAddTarget(targets,targetBattler)) pbAddTarget(targets,targetBattler.pbPartner);
				//	}
				//	else
				//		pbRandomTarget(targets);
				//	break;
				case Attack.Data.Targets.OPPONENTS_FIELD: //Attack.Target.pbOppositeOpposing:
					if (!pbAddTarget(ref targets,pbOppositeOpposing2)) pbAddTarget(ref targets,pbOppositeOpposing);
					break;
				case Attack.Data.Targets.RANDOM_OPPONENT: //Attack.Target.RandomOpposing:
					pbRandomTarget(ref targets);
					break;
				case Attack.Data.Targets.ALL_OPPONENTS: //Attack.Target.AllOpposing:
					// Just pbOpposing1 because partner is determined late
					if (!pbAddTarget(ref targets,pbOpposing1)) pbAddTarget(ref targets,pbOpposing2);
					break;
				case Attack.Data.Targets.ALL_OTHER_POKEMON: //Attack.Target.AllNonUsers:
					for (int i = 0; i < 4; i++) // not ordered by priority
					if (i!=@Index) pbAddTarget(ref targets,@battle.battlers[i]);
					break;
				case Attack.Data.Targets.USER_OR_ALLY: //Attack.Target.UserOrPartner:
					if (target>=0) { // Pre-chosen target
						IBattlerIE targetBattler=@battle.battlers[target];
						if (!pbAddTarget(ref targets,targetBattler)) pbAddTarget(ref targets,targetBattler.pbPartner);
					}
					else
						pbAddTarget(ref targets,this);
					break;
				case Attack.Data.Targets.ALLY: //Attack.TargetPartner:
					pbAddTarget(ref targets,Partner);
					break;
				default:
					(move as IBattleMoveIE).pbAddTarget(ref targets,this);
					break;
			}
			return user;
		}
		public IEnumerator pbChangeUser(IBattleMove thismove,IBattlerIE user, System.Action<IBattlerIE> result = null) {
			IBattlerIE[] priority=@battle.pbPriority();
			// Change user to user of Snatch
			if (thismove.Flags.Snatch)
				foreach (var i in priority)
					if (i.effects.Snatch) {
						yield return @battle.pbDisplay(Game._INTL("{1} snatched {2}'s move!",i.ToString(),user.ToString(true)));
						GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Snatch made it use #{user.ToString(true)}'s #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
						i.effects.Snatch=false;
						IBattlerIE target=user;
						user=i;
						// Snatch's PP is reduced if old user has Pressure
						IBattleChoice userchoice=@battle.choices[user.Index];
						if (target.hasWorkingAbility(Abilities.PRESSURE) && user.pbIsOpposing(target.Index) && userchoice.Action>=0) {
							IBattleMove pressuremove=user.moves[(int)userchoice.Move.id]; //ToDo: Change Choice.Move to int
							if (pressuremove.PP>0) pbSetPP(pressuremove,(byte)(pressuremove.PP-1));
						}
						if (Core.USENEWBATTLEMECHANICS) break;
					}
			result?.Invoke(user);
		}
		/*public Attack.Data.Targets pbTarget(IBattleMove move) {
			//Attack.Target target=move.Targets;
			Attack.Data.Targets target=move.Target;
			if (move.Effect == Attack.Data.Effects.x06E && pbHasType(Types.GHOST)) // Curse
				//target=Attack.Target.pbOppositeOpposing;
				target=Attack.Data.Targets.OPPONENTS_FIELD;
			return target;
		}*/
		public bool pbAddTarget(IList<IBattlerIE> targets,IBattlerIE target) {
			if (!target.isFainted()) {
				targets[targets.Count - 1]=target; // Add target to end of the list...
				return true;
			}
			return false;
		}
		public bool pbAddTarget(ref IList<IBattlerIE> targets,IBattlerIE target) {
			if (!target?.isFainted()??false) { //if the slot is empty, then pokemon is fainted
				targets.Add(target);
				return true;
			}
			return false;
		}
		public void pbRandomTarget(ref IList<IBattlerIE> targets) {
			IList<IBattlerIE> choices= new List<IBattlerIE>();
			pbAddTarget(ref choices,pbOpposing1);
			//if (battle.doublebattle) //Added a null conditional to below function
				pbAddTarget(ref choices,pbOpposing2);
			if (choices.Count>0)
				pbAddTarget(ref targets,choices[@battle.pbRandom(choices.Count)]);
		}
		public void pbRandomTarget(IList<IBattlerIE> targets) {
			IList<IBattlerIE> choices= new List<IBattlerIE>();
			pbAddTarget(ref choices,pbOpposing1);
			//if (battle.doublebattle) //Added a null conditional to below function
				pbAddTarget(ref choices,pbOpposing2);
			if (choices.Count>0)
				pbAddTarget(ref targets,choices[@battle.pbRandom(choices.Count)]);
		}
		public IEnumerator pbChangeTarget(IBattleMove thismove,IBattlerIE[] userandtarget,IBattlerIE[] targets,System.Action<bool> result) {
			IBattlerIE[] priority=@battle.pbPriority();
			int changeeffect=0;
			IBattlerIE user=userandtarget[0];
			IBattlerIE target=userandtarget[1];
			// Lightningrod
			if (targets.Length==1 && thismove.pbType(thismove.Type,user,target) == Types.ELECTRIC &&
				!target.hasWorkingAbility(Abilities.LIGHTNING_ROD))
				foreach (var i in priority) { // use Pokémon earliest in priority
					if (user.Index==i.Index || target.Index==i.Index) continue;
					if (i.hasWorkingAbility(Abilities.LIGHTNING_ROD)) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Lightningrod (change target)");
						target=i; // X's Lightningrod took the attack!
						changeeffect=1;
						break;
					}
				}
			// Storm Drain
			if (targets.Length==1 && thismove.pbType(thismove.Type,user,target) == Types.WATER &&
				!target.hasWorkingAbility(Abilities.STORM_DRAIN))
				foreach (var i in priority) { // use Pokémon earliest in priority
					if (user.Index==i.Index || target.Index==i.Index) continue;
					if (i.hasWorkingAbility(Abilities.STORM_DRAIN)) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Storm Drain (change target)");
						target=i; // X's Storm Drain took the attack!
						changeeffect=1;
						break;
					}
				}
			// Change target to user of Follow Me (overrides Magic Coat
			// because check for Magic Coat below uses this target)
			if (thismove.Target.TargetsOneOpponent()) {
				IBattlerIE newtarget=null; int strength=100;
				foreach (var i in priority) { // use Pokémon latest in priority
					if (!user.pbIsOpposing(i.Index)) continue;
					if (!i.isFainted() && !@battle.switching && !i.effects.SkyDrop &&
						i.effects.FollowMe>0 && i.effects.FollowMe<strength) {
						GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Follow Me");
						newtarget=i; strength=i.effects.FollowMe;
						changeeffect=0;
					}
				}
				if (newtarget.IsNotNullOrNone()) target=newtarget;
			}
			// TODO: Pressure here is incorrect if Magic Coat redirects target
			if (user.pbIsOpposing(target.Index) && target.hasWorkingAbility(Abilities.PRESSURE)) {
				GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pressure (in pbChangeTarget)");
				user.pbReducePP(thismove); // Reduce PP
			}
			// Change user to user of Snatch
			if (thismove.Flags.Snatch)
				foreach (var i in priority)
					if (i.effects.Snatch) {
						yield return @battle.pbDisplay(Game._INTL("{1} Snatched {2}'s move!",i.ToString(),user.ToString(true)));
						GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Snatch made it use #{user.ToString(true)}'s #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
						i.effects.Snatch=false;
						target=user;
						user=i;
						// Snatch's PP is reduced if old user has Pressure
						int userchoice=@battle.choices[user.Index].Index;
						if (target.hasWorkingAbility(Abilities.PRESSURE) && user.pbIsOpposing(target.Index) && userchoice>=0) {
							GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pressure (part of Snatch)");
							IBattleMove pressuremove=user.moves[userchoice];
							if (pressuremove.PP>0) pbSetPP(pressuremove,(byte)(pressuremove.PP-1));
						}
					}
			if (thismove.Flags.Reflectable)//canMagicCoat()
				if (target.effects.MagicCoat) {
					// switch user and target
					GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Magic Coat made it use #{user.ToString(true)}'s #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
					changeeffect=3;
					IBattlerIE tmp=user;
					user=target;
					target=tmp;
					// Magic Coat's PP is reduced if old user has Pressure
					int userchoice=@battle.choices[user.Index].Index;
					if (target.hasWorkingAbility(Abilities.PRESSURE) && user.pbIsOpposing(target.Index) && userchoice>=0) {
						GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pressure (part of Magic Coat)");
						IBattleMove pressuremove=user.moves[userchoice];
						if (pressuremove.PP>0) pbSetPP(pressuremove,(byte)(pressuremove.PP-1));
					}
				} else if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.MAGIC_BOUNCE)) {
					// switch user and target
					GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Magic Bounce made it use #{user.ToString(true)}'s #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
					changeeffect=3;
					IBattlerIE tmp=user;
					user=target;
					target=tmp;
				}
			if (changeeffect==1)
				yield return @battle.pbDisplay(Game._INTL("{1}'s {2} took the move!",target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name))));
			else if (changeeffect==3)
				yield return @battle.pbDisplay(Game._INTL("{1} bounced the {2} back!",user.ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
			userandtarget[0]=user;
			userandtarget[1]=target;
			if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.SOUNDPROOF) &&
				thismove.Flags.SoundBased && //isSoundBased()
				thismove.Effect != Attack.Data.Effects.x073 &&	// Perish Song handled elsewhere
				thismove.Effect != Attack.Data.Effects.x15B) {	// Parting Shot handled elsewhere
				GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Soundproof blocked #{user.ToString(true)}'s #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
				yield return @battle.pbDisplay(Game._INTL("{1}'s {2} blocks {3}!",target.ToString(),
					Game._INTL(target.Ability.ToString(TextScripts.Name)),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				result(false);
				yield break;
			}
			result(true);
		}
		#endregion

		#region Move PP
		//ToDo: Revisit SetPP, and maybe try (int move, byte pp) or ref move
		/*public void pbSetPP(IBattleMove move,int pp) {
			move.PP=pp;
			// Not effects.Mimic, since Mimic can't copy Mimic
			if (move.thismove.IsNotNullOrNone() && move.id==move.thismove.id && !@effects.Transform)
				move.thismove.PP=pp;
		}
		public bool pbReducePP(IBattleMove move) {
			if (@effects.TwoTurnAttack>0 ||
				@effects.Bide>0 ||
				@effects.Outrage>0 ||
				@effects.Rollout>0 ||
				@effects.HyperBeam>0 ||
				@effects.Uproar>0)
					// No need to reduce PP if two-turn attack
					return true;
			if (move.PP<0) return true;			// No need to reduce PP for special calls of moves
			if (move.TotalPP==0) return true;   // Infinite PP, can always be used
			if (move.PP==0) return false;
			if (move.PP>0)
				pbSetPP(move,(byte)(move.PP-1));
			return true;
		}
		public void pbReducePPOther(IBattleMove move) {
			if (move.PP>0) pbSetPP(move,(byte)(move.PP-1));
		}*/
		#endregion

		#region Using a move
		//public bool pbObedienceCheck(IBattleChoice choice) {
		public IEnumerator pbObedienceCheck(IBattleChoice choice, System.Action<bool> result) {
			if (choice.Action != ChoiceAction.UseMove)
			{
				result(true);
				yield break;
			}
			if (@battle.pbOwnedByPlayer(@Index) && @battle.internalbattle) {
				int badgelevel=10;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=1) badgelevel=20 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=2) badgelevel=30 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=3) badgelevel=40 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=4) badgelevel=50 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=5) badgelevel=60 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=6) badgelevel=70 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=7) badgelevel=80 ;
				if (@battle.pbPlayer().badges.Count(b => b == true)>=8) badgelevel=100;
				IBattleMove move=choice.Move;
				bool disobedient=false;
				if (@pokemon.isForeign(@battle.player[0]) && Level>badgelevel) {
					int a=(int)Math.Floor((Level+badgelevel)*@battle.pbRandom(256)/255d);
					disobedient|=a<badgelevel;
				}
				if (this is IBattlerShadowPokemon s) //this.respond_to("pbHyperModeObedience")
					disobedient|=!s.pbHyperModeObedience(move);
				if (disobedient) {
					GameDebug.Log($"[Disobedience] #{ToString()} disobeyed");
					@effects.Rage=false;
					if (Status==Status.SLEEP &&
						(move.Effect == Attack.Data.Effects.x05D || move.Effect == Attack.Data.Effects.x062)) { // Snore, Sleep Talk
						yield return @battle.pbDisplay(Game._INTL("{1} ignored orders while asleep!",ToString()));
						result(false);
						yield break;
					}
					int b=(int)Math.Floor((Level+badgelevel)*@battle.pbRandom(256)/255d);
					if (b<badgelevel) {
						bool canShowFightMenu = false;
						yield return @battle.pbCanShowFightMenu(@Index,result:value=>canShowFightMenu=value);
						if (!canShowFightMenu)
						{
							result(false);
							yield break;
						}
						List<int> othermoves= new List<int>();
						for (int i = 0; i < 4; i++) {
							if (i==choice.Index) continue;
							//if (@battle.pbCanChooseMove(@Index,i,false)) othermoves.Add(i);
							bool canChooseMove = false;
							yield return @battle.pbCanChooseMove(@Index, i, false, result: value => canChooseMove = value);
							if (canChooseMove) othermoves.Add(i);
						}
						if (othermoves.Count>0) {
							yield return @battle.pbDisplay(Game._INTL("{1} ignored orders!",ToString()));
							int newchoice=othermoves[@battle.pbRandom(othermoves.Count)];
							//choice.Action=ChoiceAction.UseMove;
							//choice.Move=@moves[newchoice];
							//choice.Target=-1;
							choice = new Choice(action: ChoiceAction.UseMove, moveIndex: newchoice, move: @moves[newchoice]);
						}
						result(true);
						yield break;
					}
					else if (Status!=Status.SLEEP) {
						int c=Level-b;
						int r=@battle.pbRandom(256);
						if (r<c && pbCanSleep(this,false)) {
							pbSleepSelf();
							yield return @battle.pbDisplay(Game._INTL("{1} took a nap!",ToString()));
							result(false);
							yield break;
						}
						r-=c;
						if (r<c) {
							yield return @battle.pbDisplay(Game._INTL("It hurt itself in its confusion!"));
							pbConfusionDamage();
						}
						else {
							int message=@battle.pbRandom(4);
							if (message==0) yield return @battle.pbDisplay(Game._INTL("{1} ignored orders!",ToString()));
							if (message==1) yield return @battle.pbDisplay(Game._INTL("{1} turned away!",ToString()));
							if (message==2) yield return @battle.pbDisplay(Game._INTL("{1} is loafing around!",ToString()));
							if (message==3) yield return @battle.pbDisplay(Game._INTL("{1} pretended not to notice!",ToString()));
						}
						result(false);
						yield break;
					}
				}
				result(true);
			}
			else
			{
				result(true);
			}
		}
		public IEnumerator pbSuccessCheck(IBattleMove thismove,IBattlerIE user,IBattlerIE target,IEffectsMove turneffects,bool accuracy=true, System.Action<bool> result=null) {
			if (user.effects.TwoTurnAttack > 0)
			{
				result?.Invoke(true);
				yield break;
			}
			// TODO: "Before Protect" applies to Counter/Mirror Coat
			if (thismove.Effect == Attack.Data.Effects.x009 && target.Status!=Status.SLEEP) { // Dream Eater
				yield return @battle.pbDisplay(Game._INTL("{1} wasn't affected!",target.ToString()));
				GameDebug.Log($"[Move failed] #{user.ToString()}'s Dream Eater's target isn't asleep");
				result?.Invoke(false);
				yield break;
			}
			if (thismove.Effect == Attack.Data.Effects.x0A2 && user.effects.Stockpile==0) { // Spit Up
				yield return @battle.pbDisplay(Game._INTL("But it failed to spit up a thing!"));
				GameDebug.Log($"[Move failed] #{user.ToString()}'s Spit Up did nothing as Stockpile's count is 0");
				result?.Invoke(false);
				yield break;
			}
			if (target.effects.Protect && thismove.Flags.Protectable &&
				!target.effects.ProtectNegation) {
				yield return @battle.pbDisplay(Game._INTL("{1} protected itself!",target.ToString()));
				@battle.successStates[user.Index].Protected=true;
				GameDebug.Log($"[Move failed] #{target.ToString()}'s Protect stopped the attack");
				result?.Invoke(false);
				yield break;
			}
			int p=thismove.Priority;
			if (Core.USENEWBATTLEMECHANICS) {
				if (user.hasWorkingAbility(Abilities.PRANKSTER) && thismove.Category == Attack.Category.STATUS) p+=1;
				if (user.hasWorkingAbility(Abilities.GALE_WINGS) && thismove.Type == Types.FLYING) p+=1;
			}
			if (target.pbOwnSide.QuickGuard && thismove.Flags.Protectable &&
				p>0 && !target.effects.ProtectNegation) {
				yield return @battle.pbDisplay(Game._INTL("{1} was protected by Quick Guard!",target.ToString()));
				GameDebug.Log($"[Move failed] The opposing side's Quick Guard stopped the attack");
				result?.Invoke(false);
				yield break;
			}
			if (target.pbOwnSide.WideGuard &&
				thismove.Target.HasMultipleTargets() && thismove.Category != Attack.Category.STATUS &&
				!target.effects.ProtectNegation) {
				yield return @battle.pbDisplay(Game._INTL("{1} was protected by Wide Guard!",target.ToString()));
				GameDebug.Log($"[Move failed] The opposing side's Wide Guard stopped the attack");
				result?.Invoke(false);
				yield break;
			}
			if (target.pbOwnSide.CraftyShield && thismove.Category == Attack.Category.STATUS &&
				thismove.Effect != Attack.Data.Effects.x073) { // Perish Song
				yield return @battle.pbDisplay(Game._INTL("Crafty Shield protected {1}!",target.ToString(true)));
				GameDebug.Log($"[Move failed] The opposing side's Crafty Shield stopped the attack");
				result?.Invoke(false);
				yield break;
			}
			if (target.pbOwnSide.MatBlock && thismove.Category != Attack.Category.STATUS &&
				thismove.Flags.Protectable && !target.effects.ProtectNegation) {
				yield return @battle.pbDisplay(Game._INTL("{1} was blocked by the kicked-up mat!",Game._INTL(thismove.id.ToString(TextScripts.Name))));
				GameDebug.Log($"[Move failed] The opposing side's Mat Block stopped the attack");
				result?.Invoke(false);
				yield break;
			}
			// TODO: Mind Reader/Lock-On
			// --Sketch/FutureSight/PsychUp work even on Fly/Bounce/Dive/Dig
			if (thismove.pbMoveFailed(user,target)) { // TODO: Applies to Snore/Fake Out
				yield return @battle.pbDisplay(Game._INTL("But it failed!"));
				GameDebug.Log(string.Format("[Move failed] Failed pbMoveFailed (function code %02X)",thismove.Effect));
				result?.Invoke(false);
				yield break;
			}
			// King's Shield (purposely after pbMoveFailed)
			if (target.effects.KingsShield && thismove.Category != Attack.Category.STATUS &&
				thismove.Flags.Protectable && !target.effects.ProtectNegation) {
				yield return @battle.pbDisplay(Game._INTL("{1} protected itself!",target.ToString()));
				@battle.successStates[user.Index].Protected=true;
				GameDebug.Log($"[Move failed] #{target.ToString()}'s King's Shield stopped the attack");
				if (thismove.Flags.Contact && user is IBattlerEffectIE u)
					yield return u.pbReduceStat(Stats.ATTACK,2,null,false);
				result?.Invoke(false);
				yield break;
			}
			// Spiky Shield
			if (target.effects.SpikyShield && thismove.Flags.Protectable &&
				!target.effects.ProtectNegation) {
				yield return @battle.pbDisplay(Game._INTL("{1} protected itself!",target.ToString()));
				@battle.successStates[user.Index].Protected=true;
				GameDebug.Log($"[Move failed] #{user.ToString()}'s Spiky Shield stopped the attack");
				if (thismove.Flags.Contact && !user.isFainted()) {
					if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbDamageAnimation(user,0);
					int amt=0;
					yield return user.pbReduceHP((int)Math.Floor(user.TotalHP/8d),result:value=>amt=value);
					if (amt>0) yield return @battle.pbDisplay(Game._INTL("{1} was hurt!",user.ToString()));
				}
				result?.Invoke(false);
				yield break;
			}
			// Immunity to powder-based moves
			if (Core.USENEWBATTLEMECHANICS && thismove.Flags.PowderBased &&
				(target.pbHasType(Types.GRASS) ||
				(!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.OVERCOAT)) ||
				target.hasWorkingItem(Items.SAFETY_GOGGLES))) {
				yield return @battle.pbDisplay(Game._INTL("It doesn't affect\r\n{1}...",target.ToString(true)));
				GameDebug.Log($"[Move failed] #{target.ToString()} is immune to powder-based moves somehow");
				result?.Invoke(false);
				yield break;
			}
			if (thismove.basedamage>0 && thismove.Effect != Attack.Data.Effects.x0FF && // Struggle
				thismove.Effect != Attack.Data.Effects.x095) { // Future Sight
				Types type=thismove.pbType(thismove.Type,user,target);
				float typemod=thismove.pbTypeModifier(type,user,target);
				// Airborne-based immunity to Ground moves
				if (type == Types.GROUND && target.isAirborne(user.hasMoldBreaker()) &&
					!target.hasWorkingItem(Items.RING_TARGET) && thismove.Effect != Attack.Data.Effects.x120) { // Smack Down
					if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.LEVITATE)) {
						yield return @battle.pbDisplay(Game._INTL("{1} makes Ground moves miss with Levitate!",target.ToString()));
						GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Levitate made the Ground-type move miss");
						result?.Invoke(false);
						yield break;
					}
					if (target.hasWorkingItem(Items.AIR_BALLOON)) {
						yield return @battle.pbDisplay(Game._INTL("{1}'s Air Balloon makes Ground moves miss!",target.ToString()));
						GameDebug.Log($"[Item triggered] #{target.ToString()}'s Air Balloon made the Ground-type move miss");
						result?.Invoke(false);
						yield break;
					}
					if (target.effects.MagnetRise>0) {
						yield return @battle.pbDisplay(Game._INTL("{1} makes Ground moves miss with Magnet Rise!",target.ToString()));
						GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Magnet Rise made the Ground-type move miss");
						result?.Invoke(false);
						yield break;
					}
					if (target.effects.Telekinesis>0) {
						yield return @battle.pbDisplay(Game._INTL("{1} makes Ground moves miss with Telekinesis!",target.ToString()));
						GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Telekinesis made the Ground-type move miss");
						result?.Invoke(false);
						yield break;
					}
				}
				if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.WONDER_GUARD) &&
					type>=0 && typemod<=8) {
					yield return @battle.pbDisplay(Game._INTL("{1} avoided damage with Wonder Guard!",target.ToString()));
					GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Wonder Guard");
					result?.Invoke(false);
					yield break;
				}
				if (typemod==0) {
					yield return @battle.pbDisplay(Game._INTL("It doesn't affect\r\n{1}...",target.ToString(true)));
					GameDebug.Log($"[Move failed] Type immunity");
					result?.Invoke(false);
					yield break;
				}
			}
			if (accuracy) {
				if (target.effects.LockOn>0 && target.effects.LockOnPos==user.Index) {
					GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Lock-On");
					result?.Invoke(true);
					yield break;
				}
				bool miss=false; bool _override=false;
				Attack.Data.Effects invulmove=Kernal.MoveData[target.effects.TwoTurnAttack].Effect;
				switch (invulmove) {
					case Attack.Data.Effects.x09C: case Attack.Data.Effects.x108:	// Fly, Bounce
						if (thismove.Effect != Attack.Data.Effects.x099 ||			// Thunder
							thismove.Effect != Attack.Data.Effects.x14E ||			// Hurricane
							thismove.Effect != Attack.Data.Effects.x096 ||			// Gust
							thismove.Effect != Attack.Data.Effects.x093 ||			// Twister
							thismove.Effect != Attack.Data.Effects.x0D0 ||			// Sky Uppercut
							thismove.Effect != Attack.Data.Effects.x120 ||			// Smack Down
							thismove.id != Moves.WHIRLWIND)miss=true;
						break;
					case Attack.Data.Effects.x101:									// Dig
						if (thismove.Effect != Attack.Data.Effects.x094 ||			// Earthquake
							thismove.Effect != Attack.Data.Effects.x07F)			// Magnitude
							miss=true;
						break;
					case Attack.Data.Effects.x100:									// Dive
						if (thismove.Effect != Attack.Data.Effects.x102 ||			// Surf
							thismove.Effect != Attack.Data.Effects.x106)			// Whirlpool
							miss=true;
						break;
					//case Attack.Data.Effects.x111:								// Shadow Force
					case Attack.Data.Effects.x111:									// Phantom Force
						miss=true;
						break;
					case Attack.Data.Effects.x138:									// Sky Drop
						if (thismove.Effect != Attack.Data.Effects.x099 ||			// Thunder
							thismove.Effect != Attack.Data.Effects.x14E ||			// Hurricane
							thismove.Effect != Attack.Data.Effects.x096 ||			// Gust
							thismove.Effect != Attack.Data.Effects.x093 ||			// Twister
							thismove.Effect != Attack.Data.Effects.x0D0 ||			// Sky Uppercut
							thismove.Effect != Attack.Data.Effects.x120)			// Smack Down
							miss=true;
						break;
				}
				if (target.effects.SkyDrop)
				if (thismove.Effect != Attack.Data.Effects.x099 ||					// Thunder
					thismove.Effect != Attack.Data.Effects.x14E ||					// Hurricane
					thismove.Effect != Attack.Data.Effects.x096 ||					// Gust
					thismove.Effect != Attack.Data.Effects.x093 ||					// Twister
					thismove.Effect != Attack.Data.Effects.x138 ||					// Sky Drop
					thismove.Effect != Attack.Data.Effects.x0D0 ||					// Sky Uppercut
					thismove.Effect != Attack.Data.Effects.x120)					// Smack Down
					miss=true;
				if (user.hasWorkingAbility(Abilities.NO_GUARD) ||
							target.hasWorkingAbility(Abilities.NO_GUARD) ||
							@battle.futuresight) miss=false;
				if (Core.USENEWBATTLEMECHANICS && thismove.Effect == Attack.Data.Effects.x022 && // Toxic
							thismove.basedamage==0 && user.pbHasType(Types.POISON)) _override=true;
				if (!miss && turneffects.SkipAccuracyCheck) _override=true; // Called by another move
				if (!_override && (miss || !thismove.pbAccuracyCheck(user,target))) { // Includes Counter/Mirror Coat
					GameDebug.Log(string.Format("[Move failed] Failed pbAccuracyCheck (Effect Id: {0}) or target is semi-invulnerable",thismove.Effect));
					if (thismove.Target==Attack.Data.Targets.ALL_OPPONENTS && //thismove.Targets==Attack.Target.AllOpposing
						(!user.pbOpposing1.isFainted() ? 1 : 0) + (!user.pbOpposing2.isFainted() ? 1 : 0) > 1)
						yield return @battle.pbDisplay(Game._INTL("{1} avoided the attack!",target.ToString()));
					else if (thismove.Target==Attack.Data.Targets.ALL_OTHER_POKEMON && //thismove.Targets==Attack.Target.AllNonUsers
						(!user.pbOpposing1.isFainted() ? 1 : 0) + (!user.pbOpposing2.isFainted() ? 1 : 0) + (!user.pbPartner.isFainted() ? 1 : 0) > 1)
						yield return @battle.pbDisplay(Game._INTL("{1} avoided the attack!",target.ToString()));
					else if (target.effects.TwoTurnAttack>0)
						yield return @battle.pbDisplay(Game._INTL("{1} avoided the attack!",target.ToString()));
					else if (thismove.Effect == Attack.Data.Effects.x055) // Leech Seed
						yield return @battle.pbDisplay(Game._INTL("{1} evaded the attack!",target.ToString()));
					else
						yield return @battle.pbDisplay(Game._INTL("{1}'s attack missed!",user.ToString()));
					result?.Invoke(false);
					yield break;
				}
			}
			result?.Invoke(true);
		}
		//public bool pbTryUseMove(IBattleChoice choice,IBattleMove thismove,IEffectsMove turneffects) {
		public IEnumerator pbTryUseMove(IBattleChoice choice,IBattleMove thismove,IEffectsMove turneffects, System.Action<bool> result) {
			if (turneffects.PassedTrying)
			{
				result(true);
				yield break;
			}
			// TODO: Return true if attack has been Mirror Coated once already
			//if (!turneffects.SkipAccuracyCheck)
			//{
			//	bool obedientCheck = false;
			//	yield return pbObedienceCheck(choice, value => obedientCheck = value);
			//	if (!obedientCheck)
			//	{
			//		result(false);
			//		yield break;
			//	}
			//}
			if (@effects.SkyDrop) { // Intentionally no message here
				GameDebug.Log($"[Move failed] #{ToString()} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of being Sky Dropped");
				result(false);
				yield break;
			}
			if (@battle.field.Gravity>0 && thismove.Flags.Gravity) {
				yield return @battle.pbDisplay(Game._INTL("{1} can't use {2} because of gravity!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				GameDebug.Log($"[Move failed] #{ToString()} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of Gravity");
				result(false);
				yield break;
			}
			if (@effects.Taunt>0 && thismove.basedamage==0) {
				yield return @battle.pbDisplay(Game._INTL("{1} can't use {2} after the taunt!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				GameDebug.Log($"[Move failed] #{ToString()} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of Taunt");
				result(false);
				yield break;
			}
			if (@effects.HealBlock>0 && thismove.isHealingMove()) {
				yield return @battle.pbDisplay(Game._INTL("{1} can't use {2} because of Heal Block!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				GameDebug.Log($"[Move failed] #{ToString()} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of Heal Block");
				result(false);
				yield break;
			}
			if (@effects.Torment && thismove.id==@lastMoveUsed &&
				thismove.id!=@battle.struggle.id && @effects.TwoTurnAttack==0) {
				yield return @battle.pbDisplayPaused(Game._INTL("{1} can't use the same move in a row due to the torment!",ToString()));
				GameDebug.Log($"[Move failed] #{ToString()} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of Torment");
				result(false);
				yield break;
			}
			if (pbOpposing1.effects.Imprison && !pbOpposing1.isFainted())
				if(thismove.id==pbOpposing1.moves[0].id ||
					thismove.id==pbOpposing1.moves[1].id ||
					thismove.id==pbOpposing1.moves[2].id ||
					thismove.id==pbOpposing1.moves[3].id) {
					yield return @battle.pbDisplay(Game._INTL("{1} can't use the sealed {2}!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					GameDebug.Log($"[Move failed] #{Game._INTL(thismove.id.ToString(TextScripts.Name))} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of #{pbOpposing1.ToString(true)}'s Imprison");
					result(false);
					yield break;
				}
			if (battle.doublebattle && pbOpposing2.effects.Imprison && !pbOpposing2.isFainted())
				if(thismove.id==pbOpposing2.moves[0].id ||
					thismove.id==pbOpposing2.moves[1].id ||
					thismove.id==pbOpposing2.moves[2].id ||
					thismove.id==pbOpposing2.moves[3].id) {
					yield return @battle.pbDisplay(Game._INTL("{1} can't use the sealed {2}!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					GameDebug.Log($"[Move failed] #{Game._INTL(thismove.id.ToString(TextScripts.Name))} can't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} because of #{pbOpposing2.ToString(true)}'s Imprison");
					result(false);
					yield break;
				}
			if (@effects.Disable>0 && thismove.id==@effects.DisableMove &&
				!@battle.switching) { // Pursuit ignores if it's disabled
				yield return @battle.pbDisplayPaused(Game._INTL("{1}'s {2} is disabled!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				GameDebug.Log($"[Move failed] #{ToString()}'s #{Game._INTL(thismove.id.ToString(TextScripts.Name))} is disabled");
				result(false);
				yield break;
			}
			if (choice.Index==-2) { // Battle Palace
				yield return @battle.pbDisplay(Game._INTL("{1} appears incapable of using its power!",ToString()));
				GameDebug.Log($"[Move failed] Battle Palace: #{ToString()} is incapable of using its power");
				result(false);
				yield break;
			}
			if (@effects.HyperBeam>0) {
				yield return @battle.pbDisplay(Game._INTL("{1} must recharge!",ToString()));
				GameDebug.Log($"[Move failed] #{ToString()} must recharge after using #{Combat.Move.pbFromPBMove(@battle,new Attack.Move(@currentMove)).ToString()}");
				result(false);
				yield break;
			}
			if (this.hasWorkingAbility(Abilities.TRUANT) && @effects.Truant) {
				yield return @battle.pbDisplay(Game._INTL("{1} is loafing around!",ToString()));
				GameDebug.Log($"[Ability triggered] #{ToString()}'s Truant");
				result(false);
				yield break;
			}
			if (!turneffects.SkipAccuracyCheck)
				if (Status==Status.SLEEP) {
					this.StatusCount-=1;
					if (this.StatusCount<=0)
						yield return pbCureStatus();
					else {
						yield return pbContinueStatus();
						GameDebug.Log($"[Status] #{ToString()} remained asleep (count: #{this.StatusCount.ToString()})");
						if (!thismove.pbCanUseWhileAsleep()) { // Snore/Sleep Talk/Outrage
							GameDebug.Log($"[Move failed] #{ToString()} couldn't use #{Game._INTL(thismove.id.ToString(TextScripts.Name))} while asleep");
							result(false);
							yield break;
						}
					}
				}
			if (Status==Status.FROZEN) {
				if (thismove.Flags.Defrost) {
					GameDebug.Log($"[Move effect triggered] #{ToString()} was defrosted by using #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
					yield return pbCureStatus(false);
					yield return @battle.pbDisplay(Game._INTL("{1} melted the ice!",ToString()));
					yield return pbCheckForm();
				}
				else if (@battle.pbRandom(10)<2 && !turneffects.SkipAccuracyCheck) {
					yield return pbCureStatus();
					yield return pbCheckForm();
				}
				else if (!thismove.Flags.Defrost) {
					yield return pbContinueStatus();
					GameDebug.Log($"[Status] #{ToString()} remained frozen and couldn't move");
					result(false);
					yield break;
				}
			}
			if (!turneffects.SkipAccuracyCheck)
				if (@effects.Confusion>0) {
					@effects.Confusion-=1;
					if (@effects.Confusion<=0)
						yield return pbCureConfusion();
					else {
						yield return pbContinueConfusion();
						GameDebug.Log($"[Status] #{ToString()} remained confused (count: #{@effects.Confusion})");
						if (@battle.pbRandom(2)==0) {
							pbConfusionDamage();
							yield return @battle.pbDisplay(Game._INTL("It hurt itself in its confusion!")) ;
							GameDebug.Log($"[Status] #{ToString()} hurt itself in its confusion and couldn't move");
							result(false);
							yield break;
						}
					}
				}
			if (@effects.Flinch) {
				@effects.Flinch=false;
				yield return @battle.pbDisplay(Game._INTL("{1} flinched and couldn't move!",this.ToString()));
				GameDebug.Log($"[Lingering effect triggered] #{ToString()} flinched");
				if (this.hasWorkingAbility(Abilities.STEADFAST))
				{
					bool increaseStatWithCause = false;
					yield return pbIncreaseStatWithCause(Stats.SPEED, 1, this, Game._INTL(Ability.ToString(TextScripts.Name)), result: value => increaseStatWithCause = value);
					if (increaseStatWithCause)
					//if (pbIncreaseStatWithCause(Stats.SPEED,1,this,Game._INTL(this.ability.ToString(TextScripts.Name))))
						GameDebug.Log($"[Ability triggered] #{ToString()}'s Steadfast");
				}
				result(false);
				yield break;
			}
			if (!turneffects.SkipAccuracyCheck) {
				if (@effects.Attract>=0) {
					pbAnnounceAttract(@battle.battlers[@effects.Attract]);
					if (@battle.pbRandom(2)==0) {
						yield return pbContinueAttract();
						GameDebug.Log($"[Lingering effect triggered] #{ToString()} was infatuated and couldn't move");
						result(false);
						yield break;
					}
				}
				if (Status==Status.PARALYSIS)
					if (@battle.pbRandom(4)==0) {
						yield return pbContinueStatus();
						GameDebug.Log($"[Status] #{ToString()} was fully paralyzed and couldn't move");
						result(false);
						yield break;
					}
			}
			turneffects.PassedTrying=true;
			result(true);
		}
		/*public void pbConfusionDamage() {
			this.damagestate.Reset();
			//PokeBattle_Confusion confmove=new PokeBattle_Confusion(@battle,null);
			IBattleMove confmove=new PokeBattle_Confusion().Initialize(@battle,null);
			confmove.pbEffect(this,this);
			if (this.isFainted()) pbFaint();
		}
		public void pbUpdateTargetedMove(IBattleMove thismove,IBattlerIE user) {
			// TODO: Snatch, moves that use other moves
			// TODO: All targeting cases
			// Two-turn attacks, Magic Coat, Future Sight, Counter/MirrorCoat/Bide handled
		}*/
		public IEnumerator pbProcessMoveAgainstTarget(IBattleMove thismove,IBattlerIE user,IBattlerIE target,int numhits,IEffectsMove turneffects,bool nocheck=false,int[] alltargets=null,bool showanimation=true) {
			int realnumhits=0;
			int totaldamage=0;
			bool destinybond=false;
			for (int i = 0; i < numhits; i++) {
				target.damagestate.Reset();
				// Check success (accuracy/evasion calculation)
				bool successCheck = false;
				yield return pbSuccessCheck(thismove, user, target, turneffects, i == 0 || thismove.successCheckPerHit(), result: value => successCheck = value);
				if (!nocheck &&
					//!pbSuccessCheck(thismove,user,target,turneffects,i==0 || thismove.successCheckPerHit())) {
					!successCheck) {
					if (thismove.Effect == Attack.Data.Effects.x069 && realnumhits>0)   // Triple Kick
						break;   // Considered a success if Triple Kick hits at least once
					else if (thismove.Effect == Attack.Data.Effects.x02E)  // Hi Jump Kick, Jump Kick
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Move effect triggered] #{user.ToString()} took crash damage");
							//TODO: Not shown if message is "It doesn't affect XXX..."
							yield return @battle.pbDisplay(Game._INTL("{1} kept going and crashed!",user.ToString()));
							int dmg=(int)Math.Floor(user.TotalHP/2d);
							if (dmg>0) {
								if (@battle.scene is IPokeBattle_SceneIE s0) yield return s0.pbDamageAnimation(user,0);
								yield return user.pbReduceHP(dmg);
							}
							if (user.isFainted()) yield return user.pbFaint();
						}
					if (thismove.Effect == Attack.Data.Effects.x01C) user.effects.Outrage=0;    // Outrage
					if (thismove.Effect == Attack.Data.Effects.x076) user.effects.Rollout=0;    // Rollout
					if (thismove.Effect == Attack.Data.Effects.x078) user.effects.FuryCutter=0; // Fury Cutter
					if (thismove.Effect == Attack.Data.Effects.x0A2) user.effects.Stockpile=0;  // Spit Up
					yield break;
				}
				// Add to counters for moves which increase them when used in succession
				if (thismove.Effect == Attack.Data.Effects.x078) // Fury Cutter
					if (user.effects.FuryCutter<4) user.effects.FuryCutter+=1;
				else
					user.effects.FuryCutter=0;
				if (thismove.Effect == Attack.Data.Effects.x12F) { // Echoed Voice
					if (!user.pbOwnSide.EchoedVoiceUsed &&
						user.pbOwnSide.EchoedVoiceCounter<5)
						user.pbOwnSide.EchoedVoiceCounter+=1;
					user.pbOwnSide.EchoedVoiceUsed=true;
				}
				// Count a hit for Parental Bond if it applies
				if (user.effects.ParentalBond>0) user.effects.ParentalBond-=1;
				// This hit will happen; count it
				realnumhits+=1;
				// Damage calculation and/or main effect
				int damage=thismove.pbEffect(user,target,(byte)i,alltargets,showanimation); // Recoil/drain, etc. are applied here
				if (damage>0) totaldamage+=damage;
				// Message and consume for type-weakening berries
				if (target.damagestate.BerryWeakened) {
					yield return @battle.pbDisplay(Game._INTL("The {1} weakened the damage to {2}!",
						Game._INTL(target.Item.ToString(TextScripts.Name)),target.ToString(true)));
					yield return target.pbConsumeItem();
				}
				// Illusion
				if (target.effects.Illusion.IsNotNullOrNone() && target.hasWorkingAbility(Abilities.ILLUSION) &&
					damage>0 && !target.damagestate.Substitute) {
					GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Illusion ended");
					target.effects.Illusion=null;
					if (@battle.scene is IPokeBattle_SceneIE s0) s0.pbChangePokemon(target,target.pokemon);
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} wore off!",target.ToString(),
						Game._INTL(target.Ability.ToString(TextScripts.Name))));
				}
				if (user.isFainted())
					yield return user.pbFaint(); // no return
				if (numhits>1 && target.damagestate.CalcDamage<=0) yield break;
				@battle.pbJudgeCheckpoint(user,thismove);
				// Additional effect
				if (target.damagestate.CalcDamage>0 &&
					!user.hasWorkingAbility(Abilities.SHEER_FORCE) &&
					(user.hasMoldBreaker() || !target.hasWorkingAbility(Abilities.SHIELD_DUST))) {
					int addleffect=thismove.AddlEffect;
					if ((user.hasWorkingAbility(Abilities.SERENE_GRACE) ||
						user.pbOwnSide.Rainbow>0) &&
						thismove.Effect != Attack.Data.Effects.x0C6) addleffect*=2; // Secret Power
					if (Core.DEBUG && Input.press((int)PokemonUnity.UX.InputKeys.DEBUG)) addleffect=100;
					if (@battle.pbRandom(100)<addleffect) {
						GameDebug.Log($"[Move effect triggered] #{Game._INTL(thismove.id.ToString(TextScripts.Name))}'s added effect");
						thismove.pbAdditionalEffect(user,target);
					}
				}
				// Ability effects
				yield return pbEffectsOnDealingDamage(thismove,user,target,damage);
				// Grudge
				if (!user.isFainted() && target.isFainted())
				if (target.effects.Grudge && target.pbIsOpposing(user.Index)) {
					thismove.PP=0;
					yield return @battle.pbDisplay(Game._INTL("{1}'s {2} lost all its PP due to the grudge!",
						user.ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Grudge made #{Game._INTL(thismove.id.ToString(TextScripts.Name))} lose all its PP");
				}
				if (target.isFainted())
					destinybond=destinybond || target.effects.DestinyBond;
				if (user.isFainted()) yield return user.pbFaint(); // no return
				if (user.isFainted()) break;
				if (target.isFainted()) break;
				// Make the target flinch
				if (target.damagestate.CalcDamage>0 && !target.damagestate.Substitute)
				if (user.hasMoldBreaker() || !target.hasWorkingAbility(Abilities.SHIELD_DUST)) {
					bool canflinch=false;
					if (user.hasWorkingItem(Items.KINGS_ROCK) || user.hasWorkingItem(Items.RAZOR_FANG)
						&& thismove.canKingsRock())
						canflinch=true;
					if (user.hasWorkingAbility(Abilities.STENCH) &&
						thismove.Effect != Attack.Data.Effects.x114 &&		// Thunder Fang
						thismove.Effect != Attack.Data.Effects.x112 &&		// Fire Fang
						thismove.Effect != Attack.Data.Effects.x113 &&		// Ice Fang
						thismove.Effect != Attack.Data.Effects.x097 &&		// Stomp
						thismove.Effect != Attack.Data.Effects.x05D &&		// Snore
						thismove.Effect != Attack.Data.Effects.x09F &&		// Fake Out
						thismove.Effect != Attack.Data.Effects.x093 &&		// Twister
						thismove.Effect != Attack.Data.Effects.x04C &&		// Sky Attack
						//thismove.Effect != Attack.Data.Effects.   &&		// flinch-inducing moves
						Kernal.MoveMetaData[thismove.id].FlinchChance == 0)	// flinch-inducing moves
						canflinch=true;
					if (canflinch && @battle.pbRandom(10)==0) {
						GameDebug.Log($"[Item/ability triggered] #{user.ToString()}'s King's Rock/Razor Fang or Stench");
						if (target is IBattlerEffectIE t0) t0.pbFlinch(user);
					}
				}
				if (target.damagestate.CalcDamage>0 && !target.isFainted() && target is IBattlerEffectIE t) {
					// Defrost
					if (target.Status==Status.FROZEN &&
						(thismove.pbType(thismove.Type,user,target) == Types.FIRE ||
						(Core.USENEWBATTLEMECHANICS && thismove.id == Moves.SCALD)))
						yield return t.pbCureStatus();
					// Rage
					if (target.effects.Rage && target.pbIsOpposing(user.Index))
					{
						bool increaseStatWithCause = false;
						yield return t.pbIncreaseStatWithCause(Stats.ATTACK, 1, target, "", true, false, result: value => increaseStatWithCause = value);
						if (increaseStatWithCause) {
						// TODO: Apparently triggers if opposing Pokémon uses Future Sight after a Future Sight attack
						//if (t.pbIncreaseStatWithCause(Stats.ATTACK,1,target,"",true,false)) {
							GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Rage");
							yield return @battle.pbDisplay(Game._INTL("{1}'s rage is building!",target.ToString()));
						}
					}
				}
				if (target.isFainted()) yield return target.pbFaint();		// no return
				if (user.isFainted()) yield return user.pbFaint();			// no return
				if (user.isFainted() || target.isFainted()) break;
				// Berry check (maybe just called by ability effect, since only necessary Berries are checked)
				for (int j = 0; j < battle.battlers.Length; j++)
					yield return @battle.battlers[j].pbBerryCureCheck();
				if (user.isFainted() || target.isFainted()) break;
				target.pbUpdateTargetedMove(thismove,user);
				if (target.damagestate.CalcDamage<=0) break;
			}
			if (totaldamage>0) turneffects.TotalDamage+=totaldamage;
			// Battle Arena only - attack is successful
			@battle.successStates[user.Index].UseState=null;
			@battle.successStates[user.Index].TypeMod=target.damagestate.TypeMod;
			// Type effectiveness
			if (numhits>1) {
				if (target.damagestate.TypeMod>8)
				if (alltargets.Length>1)
					yield return @battle.pbDisplay(Game._INTL("It's super effective on {1}!",target.ToString(true)));
				else
					yield return @battle.pbDisplay(Game._INTL("It's super effective!"));
				else if (target.damagestate.TypeMod>=1 && target.damagestate.TypeMod<8)
				if (alltargets.Length>1)
					yield return @battle.pbDisplay(Game._INTL("It's not very effective on {1}...",target.ToString(true)));
				else
					yield return @battle.pbDisplay(Game._INTL("It's not very effective..."));
				if (realnumhits==1)
					yield return @battle.pbDisplay(Game._INTL("Hit {1} time!",realnumhits.ToString()));
				else
					yield return @battle.pbDisplay(Game._INTL("Hit {1} times!",realnumhits.ToString()));
			}
			GameDebug.Log($"Move did #{numhits} hit(s), total damage=#{turneffects.TotalDamage}");
			// Faint if 0 HP
			if (target.isFainted()) yield return target.pbFaint();	// no return
			if (user.isFainted()) yield return user.pbFaint();		// no return
			thismove.pbEffectAfterHit(user,target,turneffects);		//ToDo: CONFIRM IF `pbFaint()` IS ASSIGNING `isFaint()` AS TRUE!~
			if (target.isFainted()) yield return target.pbFaint();	// no return
			if (user.isFainted()) yield return user.pbFaint();		// no return
			// Destiny Bond
			if (!user.isFainted() && target.isFainted())
				if (destinybond && target.pbIsOpposing(user.Index)) {
					GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Destiny Bond");
					yield return @battle.pbDisplay(Game._INTL("{1} took its attacker down with it!",target.ToString()));
					user.pbReduceHP(user.HP);
					yield return user.pbFaint(); // no return
					@battle.pbJudgeCheckpoint(user);
				}
			yield return pbEffectsAfterHit(user,target,thismove,turneffects);
			// Berry check
			for (int j = 0; j < battle.battlers.Length; j++)
				yield return @battle.battlers[j].pbBerryCureCheck();
			target.pbUpdateTargetedMove(thismove,user);
		}
		new public IEnumerator pbUseMoveSimple(Moves moveid,int index=-1,int target=-1) {
			//IBattleChoice choice= new Choice();
			//choice[0]=1;           // "Use move"
			//choice.Action=index;   // Index of move to be used in user's moveset
			//choice.Move=PokeBattle_Move.pbFromPBMove(@battle,new Attack.Move(moveid)); // PokeBattle_Move object of the move
			//choice.Move.PP=-1;
			//choice.Target=target;  // Target (-1 means no target yet)
			IBattleMove move = Combat.Move.pbFromPBMove(@battle, new Attack.Move(moveid));
			move.PP = -1;
			IBattleChoice choice = new Choice(action: ChoiceAction.UseMove, moveIndex: index, move: move, target: target);
			if (index>=0)
				//@battle.choices[@Index].Index=index;
				@battle.choices[@Index]=new Choice(action: @battle.choices[@Index].Action, moveIndex: index, move: @battle.choices[@Index].Move, target: @battle.choices[@Index].Target);
			GameDebug.Log($"#{ToString()} used simple move #{Game._INTL(choice.Move.id.ToString(TextScripts.Name))}");
			yield return pbUseMove(choice,true);
			yield break;
		}
		new public IEnumerator pbUseMove(IBattleChoice choice,bool specialusage=false) {
			// TODO: lastMoveUsed is not to be updated on nested calls
			// Note: user.lastMoveUsedType IS to be updated on nested calls; is used for Conversion 2
			IEffectsMove turneffects= new Effects.Move();
			turneffects.SpecialUsage=specialusage;
			turneffects.SkipAccuracyCheck=specialusage;
			turneffects.PassedTrying=false;
			turneffects.TotalDamage=0;
			// Start using the move
			yield return pbBeginTurn(choice);
			// Force the use of certain moves if they're already being used
			if (@effects.TwoTurnAttack>0 ||
				@effects.HyperBeam>0 ||
				@effects.Outrage>0 ||
				@effects.Rollout>0 ||
				@effects.Uproar>0 ||
				@effects.Bide>0) {
				//choice.Move=Combat.Move.pbFromPBMove(@battle,new Attack.Move(@currentMove));
				choice=new Choice(action: choice.Action, moveIndex: choice.Index, move: Combat.Move.pbFromPBMove(@battle, new Attack.Move(@currentMove)));
				turneffects.SpecialUsage=true;
				GameDebug.Log($"Continuing multi-turn move #{Game._INTL(choice.Move.id.ToString(TextScripts.Name))}");
			}
			else if (@effects.Encore>0) {
				bool canChooseMove = false;
				yield return @battle.pbCanChooseMove(@Index, @effects.EncoreIndex, false, result: value => canChooseMove = value);
				if (@battle.pbCanShowCommands(@Index) &&
					canChooseMove) { //@battle.pbCanChooseMove(@Index,@effects.EncoreIndex,false)
					if (choice.Index!=@effects.EncoreIndex) { // Was Encored mid-round
						//choice.Index=@effects.EncoreIndex;
						//choice.Move=@moves[@effects.EncoreIndex];
						//choice.Target=-1; // No target chosen
						choice = new Choice(action: ChoiceAction.UseMove, moveIndex: @effects.EncoreIndex, move: @moves[@effects.EncoreIndex]);
					}
					GameDebug.Log($"Using Encored move #{Game._INTL(choice.Move.id.ToString(TextScripts.Name))}");
				}
			}
			IBattleMove thismove=choice.Move;
			if (!thismove.IsNotNullOrNone() || thismove.id<=0) yield break; //if move was not chosen
			if (!turneffects.SpecialUsage) {
				// TODO: Quick Claw message
			}
			// Stance Change
			if (hasWorkingAbility(Abilities.STANCE_CHANGE) && Species == Pokemons.AEGISLASH &&
				!@effects.Transform)
				if (thismove.pbIsDamaging() && this.form!=1) {
					this.form=1;
					pbUpdate(true);
					if (@battle.scene is IPokeBattle_SceneIE s0) s0.pbChangePokemon(this,@pokemon);
					yield return @battle.pbDisplay(Game._INTL("{1} changed to Blade Forme!",ToString()));
					GameDebug.Log($"[Form changed] #{ToString()} changed to Blade Forme");
				}
				else if (thismove.id == Moves.KINGS_SHIELD && this.form!=0) {
					this.form=0;
					pbUpdate(true);
					if (@battle.scene is IPokeBattle_SceneIE s0) s0.pbChangePokemon(this,@pokemon);
					yield return @battle.pbDisplay(Game._INTL("{1} changed to Shield Forme!",ToString()));
					GameDebug.Log($"[Form changed] #{ToString()} changed to Shield Forme");
				}
			// Record that user has used a move this round (or at least tried to)
			this.lastRoundMoved=@battle.turncount;
			// Try to use the move
			bool tryUseMove = false;
			yield return pbTryUseMove(choice, thismove, turneffects, value => tryUseMove = value);
			//if (!pbTryUseMove(choice,thismove,turneffects)) {
			if (!tryUseMove) {
				this.lastMoveUsed=Moves.NONE;
				//this.lastMoveUsedType=Types.NONE;
				if (!turneffects.SpecialUsage) {
					if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=Moves.NONE;
					this.lastRegularMoveUsed=Moves.NONE;
				}
				pbCancelMoves();
				@battle.pbGainEXP();
				yield return pbEndTurn(choice);
				@battle.pbJudge();       //@battle.pbSwitch
				yield break;
			}
			if (!turneffects.SpecialUsage)
				if (!pbReducePP(thismove)) {
					yield return @battle.pbDisplay(Game._INTL("{1} used\r\n{2}!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					yield return @battle.pbDisplay(Game._INTL("But there was no PP left for the move!"));
					this.lastMoveUsed=Moves.NONE;
					//this.lastMoveUsedType=Types.NONE;
					if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=Moves.NONE;
					this.lastRegularMoveUsed=Moves.NONE;
					yield return pbEndTurn(choice);
					@battle.pbJudge();         //@battle.pbSwitch
					GameDebug.Log($"[Move failed] #{Game._INTL(thismove.id.ToString(TextScripts.Name))} has no PP left");
					yield break;
				}
			// Remember that user chose a two-turn move
			if (thismove.pbTwoTurnAttack(this)) {
				// Beginning use of two-turn attack
				@effects.TwoTurnAttack=thismove.id;
				@currentMove=thismove.id;
			}
			else
				@effects.TwoTurnAttack=0; // Cancel use of two-turn attack
			// Charge up Metronome Item
			if (this.lastMoveUsed==thismove.id)
				this.effects.Metronome+=1;
			else
				this.effects.Metronome=0;
			// "X used Y!" message
			int zxy = thismove.pbDisplayUseMessage(this); //switch (thismove.pbDisplayUseMessage(this)) {
				if (zxy == 2) //case 2:   // Continuing Bide
					yield break;
				else if (zxy == 1) { //case 1:   // Starting Bide
					this.lastMoveUsed=thismove.id;
					//this.lastMoveUsedType=thismove.pbType(thismove.Type,this,null);
					if (!turneffects.SpecialUsage) {
						if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=thismove.id;
						this.lastRegularMoveUsed=thismove.id;
					}
					@battle.lastMoveUsed=thismove.id;
					@battle.lastMoveUser=this.Index;
					@battle.successStates[this.Index].UseState=null;
					@battle.successStates[this.Index].TypeMod=8;
					yield break;
				} else if (zxy == -1) { //case -1:   // Was hurt while readying Focus Punch, fails use
					this.lastMoveUsed=thismove.id;
					//this.lastMoveUsedType=thismove.pbType(thismove.Type,this,null);
					if (!turneffects.SpecialUsage) {
						if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=thismove.id;
						this.lastRegularMoveUsed=thismove.id;
					}
					@battle.lastMoveUsed=thismove.id;
					@battle.lastMoveUser=this.Index;
					@battle.successStates[this.Index].UseState =null;// somehow treated as a success
					@battle.successStates[this.Index].TypeMod=8;
					GameDebug.Log($"[Move failed] #{ToString()} was hurt while readying Focus Punch");
					yield break;
			}
			// Find the user and target(s)
			IList<IBattlerIE> targets=new List<IBattlerIE>();
			//targets.Add(null); // Empty for slot 0
			IBattlerIE user=pbFindUser(choice,targets); //ToDo: ref List<> here?
			// Battle Arena only - assume failure
			@battle.successStates[user.Index].UseState=true;
			@battle.successStates[user.Index].TypeMod=8;
			// Check whether Selfdestruct works
			if (!thismove.pbOnStartUse(user)) { // Selfdestruct, Natural Gift, Beat Up can return false here
				GameDebug.Log(string.Format("[Move failed] Failed pbOnStartUse (function code %02X)",thismove.Effect));
				user.lastMoveUsed=thismove.id;
				//user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
				if (!turneffects.SpecialUsage) {
					if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
					user.lastRegularMoveUsed=thismove.id;
				}
				@battle.lastMoveUsed=thismove.id;
				@battle.lastMoveUser=user.Index;
				yield break;
			}
			// Primordial Sea, Desolate Land
			if (thismove.pbIsDamaging())
				switch (@battle.pbWeather) {
					case Weather.HEAVYRAIN:
						if (thismove.pbType(thismove.Type,user,null) == Types.FIRE) {
							GameDebug.Log($"[Move failed] Primordial Sea's rain cancelled the Fire-type #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
							yield return @battle.pbDisplay(Game._INTL("The Fire-type attack fizzled out in the heavy rain!"));
							user.lastMoveUsed=thismove.id;
							//user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
							if (!turneffects.SpecialUsage) {
								if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
								user.lastRegularMoveUsed=thismove.id;
							}
							@battle.lastMoveUsed=thismove.id;
							@battle.lastMoveUser=user.Index;
							yield break;
						}
						break;
					case Weather.HARSHSUN:
						if (thismove.pbType(thismove.Type,user,null) == Types.WATER) {
							GameDebug.Log($"[Move failed] Desolate Land's sun cancelled the Water-type #{Game._INTL(thismove.id.ToString(TextScripts.Name))}");
							yield return @battle.pbDisplay(Game._INTL("The Water-type attack evaporated in the harsh sunlight!"));
							user.lastMoveUsed=thismove.id;
							//user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
							if (!turneffects.SpecialUsage) {
								if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
								user.lastRegularMoveUsed=thismove.id;
							}
							@battle.lastMoveUsed=thismove.id;
							@battle.lastMoveUser=user.Index;
							yield break;
						}
						break;
				}
			// Powder
			if (user.effects.Powder && thismove.pbType(thismove.Type,user,null) == Types.FIRE) {
				GameDebug.Log($"[Lingering effect triggered] #{ToString()}'s Powder cancelled the Fire move");
				@battle.pbCommonAnimation("Powder",user,null);
				yield return @battle.pbDisplay(Game._INTL("When the flame touched the powder on the Pokémon, it exploded!"));
				if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) user.pbReduceHP(1+(int)Math.Floor(user.TotalHP/4d));
				user.lastMoveUsed=thismove.id;
				//user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
				if (!turneffects.SpecialUsage) {
					if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
					user.lastRegularMoveUsed=thismove.id;
				}
				@battle.lastMoveUsed=thismove.id;
				@battle.lastMoveUser=user.Index;
				if (user.isFainted()) user.pbFaint();
				yield return pbEndTurn(choice);
				yield break;
			}
			// Protean
			if (user.hasWorkingAbility(Abilities.PROTEAN) &&
				thismove.Effect != Attack.Data.Effects.x00A &&   // Mirror Move
				thismove.Effect != Attack.Data.Effects.x0F3 &&   // Copycat
				thismove.Effect != Attack.Data.Effects.x073 &&   // Me First
				thismove.Effect != Attack.Data.Effects.x0AE &&   // Nature Power
				thismove.Effect != Attack.Data.Effects.x062 &&   // Sleep Talk
				thismove.Effect != Attack.Data.Effects.x0B5 &&   // Assist
				thismove.Effect != Attack.Data.Effects.x054) {   // Metronome
				Types movetype=thismove.pbType(thismove.Type,user,null);
				if (!user.pbHasType(movetype)) {
					string typename=Game._INTL(movetype.ToString(TextScripts.Name));
					GameDebug.Log($"[Ability triggered] #{ToString()}'s Protean made it #{typename}-type");
					user.Type1=movetype;
					user.Type2=movetype;
					user.effects.Type3=Types.NONE;
					yield return @battle.pbDisplay(Game._INTL("{1} transformed into the {2} type!",user.ToString(),typename))  ;
				}
			}
			// Try to use move against user if there aren't any targets
			if (targets.Count==0) {
				//user=pbChangeUser(thismove,user);
				yield return pbChangeUser(thismove,user, result: value => user = value);
				if(thismove.Target==Attack.Data.Targets.SELECTED_POKEMON ||		//.Targets==Attack.Target.SingleNonUser ||
					thismove.Target==Attack.Data.Targets.RANDOM_OPPONENT ||		//.Targets==Attack.Target.RandomOpposing ||
					thismove.Target==Attack.Data.Targets.ALL_OPPONENTS ||		//.Targets==Attack.Target.AllOpposing ||
					thismove.Target==Attack.Data.Targets.ALL_OTHER_POKEMON ||	//.Targets==Attack.Target.AllNonUsers ||
					thismove.Target==Attack.Data.Targets.ALLY ||				//.Targets==Attack.TargetPartner ||
					thismove.Target==Attack.Data.Targets.USER_OR_ALLY ||		//.Targets==Attack.Target.UserOrPartner ||
					//thismove.Target==Attack.Data.Targets.SingleOpposing ||	//.Targets==Attack.Target.SingleOpposing ||
					thismove.Target==Attack.Data.Targets.OPPONENTS_FIELD)		//.Targets==Attack.Target.pbOppositeOpposing)
					yield return @battle.pbDisplay(Game._INTL("But there was no target..."));
				else
					//GameDebug.logonerr(thismove.pbEffect(user,null));
					try{ thismove.pbEffect(user, null); } catch { GameDebug.Log(""); }
			}
			else {
				// We have targets
				bool showanimation=true;
				List<int> alltargets= new List<int>();
				for (int x = 0; x < targets.Count; x++)
					if (!targets.Contains(targets[x])) alltargets.Add(targets[x].Index);
				// For each target in turn
				int i=0;
				do {
					// Get next target
					IBattlerIE[] userandtarget=new IBattlerIE[] { user, targets[i] };
					bool success=false;//pbChangeTarget(thismove,userandtarget,targets.ToArray());
					yield return pbChangeTarget(thismove,userandtarget,targets.ToArray(),result:value=>success=value);
					user=userandtarget[0];
					IBattlerIE target=userandtarget[1];
					if (battle.doublebattle && i==0 && thismove.Target==Attack.Data.Targets.ALL_OPPONENTS) //thismove.Targets==Attack.Target.AllOpposing
						// Add target's partner to list of targets
						pbAddTarget(ref targets,target.pbPartner);
					// If couldn't get the next target
					if (!success) {
						i+=1;
						continue;
					}
					// Get the number of hits
					int numhits=thismove.pbNumHits(user);
					// Reset damage state, set Focus Band/Focus Sash to available
					target.damagestate.Reset();
					// Use move against the current target
					yield return pbProcessMoveAgainstTarget(thismove,user,target,numhits,turneffects,false,alltargets.ToArray(),showanimation);
					showanimation=false;
					i+=1;
				} while (i<targets.Count);
			}
			List<int> switched=new List<int>();
			// Pokémon switching caused by Roar, Whirlwind, Circle Throw, Dragon Tail, Red Card
			if (!user.isFainted()) {
				switched=new List<int>();
				for (int i = 0; i < battle.battlers.Length; i++)
					if (@battle.battlers[i].effects.Roar) {
						@battle.battlers[i].effects.Roar=false;
						@battle.battlers[i].effects.Uturn=false;
						if (@battle.battlers[i].isFainted()) continue;
						bool canSwitch = false;
						yield return @battle.pbCanSwitch(i,-1,false,result:value=>canSwitch=value);
						if (!canSwitch) continue;
						List<int> choices= new List<int>();
						PokemonEssentials.Interface.PokeBattle.IPokemon[] party=@battle.pbParty(i);
						for (int j = 0; j< party.Length; j++) {
							bool canSwitchLax = false;
							yield return @battle.pbCanSwitchLax(i,j,false,result:value=>canSwitchLax=value);
							if (canSwitchLax) choices.Add(j);
						}
						if (choices.Count>0) {
							int newpoke=choices[@battle.pbRandom(choices.Count)];
							int newpokename=newpoke;
							if (party[newpoke].Ability == Abilities.ILLUSION)
								newpokename=@battle.pbGetLastPokeInTeam(i);
							switched.Add(i);
							@battle.battlers[i].pbResetForm();
							yield return @battle.pbRecallAndReplace(i,newpoke,newpokename,false,user.hasMoldBreaker());
							yield return @battle.pbDisplay(Game._INTL("{1} was dragged out!",@battle.battlers[i].ToString()));
							//@battle.choices[i]=[0,0,null,-1];		// Replacement Pokémon does nothing this round
							@battle.choices[i]=new Choice();		// Replacement Pokémon does nothing this round
						}
					}
				foreach(IBattlerIE i in @battle.pbPriority()) {
					if (!switched.Contains(i.Index)) continue;
					yield return i.pbAbilitiesOnSwitchIn(true);
				}
			}
			// Pokémon switching caused by U-Turn, Volt Switch, Eject Button
			switched= new List<int>();
			for (int i = 0; i < battle.battlers.Length; i++)
				if (@battle.battlers[i].effects.Uturn) {
					@battle.battlers[i].effects.Uturn=false;
					@battle.battlers[i].effects.Roar=false;
					if (!@battle.battlers[i].isFainted() && @battle.pbCanChooseNonActive(i) &&
						!@battle.pbAllFainted(@battle.pbOpposingParty(i))) {
						// TODO: Pursuit should go here, and negate this effect if it KO's attacker
						yield return @battle.pbDisplay(Game._INTL("{1} went back to {2}!",@battle.battlers[i].ToString(),@battle.pbGetOwner(i).name));
						int newpoke=0;
						@battle.pbSwitchInBetween(i,true,false,result:value=>newpoke=value);
						int newpokename=newpoke;
						if (@battle.pbParty(i)[newpoke].Ability == Abilities.ILLUSION)
							newpokename=@battle.pbGetLastPokeInTeam(i);
						switched.Add(i);
						@battle.battlers[i].pbResetForm();
						yield return @battle.pbRecallAndReplace(i,newpoke,newpokename,@battle.battlers[i].effects.BatonPass);
						//@battle.choices[i]=[0,0,null,-1];		// Replacement Pokémon does nothing this round
						@battle.choices[i]=new Choice();		// Replacement Pokémon does nothing this round
					}
				}
			foreach(IBattlerIE i in @battle.pbPriority()) {
				if (!switched.Contains(i.Index)) continue;
				yield return i.pbAbilitiesOnSwitchIn(true);
			}
			// Baton Pass
			if (user.effects.BatonPass) {
				user.effects.BatonPass=false;
				if (!user.isFainted() && @battle.pbCanChooseNonActive(user.Index) &&
					!@battle.pbAllFainted(@battle.pbParty(user.Index))) {
					int newpoke=0;
					@battle.pbSwitchInBetween(user.Index,true,false,result:value=>newpoke=value);
					int newpokename=newpoke;
					if (@battle.pbParty(user.Index)[newpoke].Ability == Abilities.ILLUSION)
						newpokename=@battle.pbGetLastPokeInTeam(user.Index);
					user.pbResetForm();
					yield return @battle.pbRecallAndReplace(user.Index,newpoke,newpokename,true);
					//@battle.choices[user.Index]=new Battle.Choice(0,0,null,-1);	// Replacement Pokémon does nothing this round
					@battle.choices[user.Index]=new Choice(ChoiceAction.NoAction);	// Replacement Pokémon does nothing this round
					yield return user.pbAbilitiesOnSwitchIn(true);
				}
			}
			// Record move as having been used
			user.lastMoveUsed=thismove.id;
			//user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
			if (!turneffects.SpecialUsage) {
				if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
				user.lastRegularMoveUsed=thismove.id;
				if (!user.movesUsed.Contains(thismove.id)) user.movesUsed.Add(thismove.id); // For Last Resort
			}
			@battle.lastMoveUsed=thismove.id;
			@battle.lastMoveUser=user.Index;
			// Gain Exp
			@battle.pbGainEXP();
			// Battle Arena only - update skills
			for (int i = 0; i < battle.battlers.Length; i++)
				@battle.successStates[i].UpdateSkill();
			// End of move usage
			yield return pbEndTurn(choice);
			@battle.pbJudge();     //@battle.pbSwitch();
			yield break;
		}
		/*public void pbCancelMoves() {
			// If failed pbTryUseMove or have already used Pursuit to chase a switching foe
			// Cancel multi-turn attacks (note: Hyper Beam effect is not canceled here)
			if (@effects.TwoTurnAttack>0) @effects.TwoTurnAttack=0;
			@effects.Outrage=0;
			@effects.Rollout=0;
			@effects.Uproar=0;
			@effects.Bide=0;
			@currentMove=0;
			// Reset counters for moves which increase them when used in succession
			@effects.FuryCutter=0;
			GameDebug.Log($"Cancelled using the move");
		}*/
		#endregion

		#region Turn processing
		new public IEnumerator pbBeginTurn(IBattleChoice choice) {
			// Cancel some lingering effects which only apply until the user next moves
			@effects.DestinyBond=false;
			@effects.Grudge=false;
			// Reset Parental Bond's count
			@effects.ParentalBond=0;
			// Encore's effect ends if the encored move is no longer available
			if (@effects.Encore>0 &&
				@moves[@effects.EncoreIndex].id!=@effects.EncoreMove) {
				GameDebug.Log($"Resetting Encore effect");
				@effects.Encore=0;
				@effects.EncoreIndex=0;
				@effects.EncoreMove=0;
			}
			// Wake up in an uproar
			if (Status==Status.SLEEP && !this.hasWorkingAbility(Abilities.SOUNDPROOF))
				for (int i = 0; i < 4; i++)
					if (@battle.battlers[i].effects.Uproar>0) {
						yield return pbCureStatus(false);
						yield return @battle.pbDisplay(Game._INTL("{1} woke up in the uproar!",ToString()));
					}
		}
		//private void _pbEndTurn(IBattleChoice choice) {
		IEnumerator IBattlerIE.pbEndTurn(IBattleChoice choice) {
			// True end(?)
			if (@effects.ChoiceBand<0 && @lastMoveUsed>=0 && !this.isFainted() &&
				(this.hasWorkingItem(Items.CHOICE_BAND) ||
				this.hasWorkingItem(Items.CHOICE_SPECS) ||
				this.hasWorkingItem(Items.CHOICE_SCARF)))
				@effects.ChoiceBand=@lastMoveUsed;
			yield return @battle.pbPrimordialWeather();
			for (int i = 0; i < battle.battlers.Length; i++)
				yield return @battle.battlers[i].pbBerryCureCheck();
			for (int i = 0; i < battle.battlers.Length; i++)
				yield return @battle.battlers[i].pbAbilityCureCheck();
			for (int i = 0; i < battle.battlers.Length; i++)
				yield return @battle.battlers[i].pbAbilitiesOnSwitchIn(false);
			for (int i = 0; i < battle.battlers.Length; i++)
				yield return @battle.battlers[i].pbCheckForm();
		}
		//public bool pbProcessTurn(IBattleChoice choice) {
		public IEnumerator pbProcessTurn(IBattleChoice choice, System.Action<bool> result) {
			// Can't use a move if fainted
			if (this.isFainted())
			{
				result(false);
				yield break;
			}
			// Wild roaming Pokémon always flee if possible
			if (@battle.opponent.Length != 0 && @battle.pbIsOpposing(this.Index) &&
				@battle.rules["alwaysflee"] && @battle.pbCanRun(this.Index)) {
				yield return pbBeginTurn(choice);
				yield return @battle.pbDisplay(Game._INTL("{1} fled!",this.ToString()));
				@battle.decision=BattleResults.FORFEIT;
				yield return pbEndTurn(choice);
				GameDebug.Log($"[Escape] #{ToString()} fled");
				result(true);
				yield break;
			}
			// If this battler's action for this round wasn't "use a move"
			if (choice.Action!=ChoiceAction.UseMove) {
				// Clean up effects that end at battler's turn
				yield return pbBeginTurn(choice);
				yield return pbEndTurn(choice);
				result(false);
				yield break;
			}
			// Turn is skipped if Pursuit was used during switch
			if (@effects.Pursuit) {
				@effects.Pursuit=false;
				pbCancelMoves();
				yield return pbEndTurn(choice);
				@battle.pbJudge();       //@battle.pbSwitch
				result(false);
				yield break;
			}
			// Use the move
			//   yield return @battle.pbDisplayPaused("Before: [#{@lastMoveUsedSketch},#{@lastMoveUsed}]"); //Log instead?
			GameDebug.Log($"#{ToString()} used #{Game._INTL(choice.Move.id.ToString(TextScripts.Name))}");
			//try{ pbUseMove(choice, choice.Move == @battle.struggle); } catch (Exception e) { GameDebug.Log(e.Message); } //GameDebug.Log(e.StackTrace);
			yield return pbUseMove(choice, choice.Move == @battle.struggle);
			//   yield return @battle.pbDisplayPaused("After: [#{@lastMoveUsedSketch},#{@lastMoveUsed}]");
			result(true);
		}
		#endregion

		#region Explicit Interface Implementation
		/*bool IBattler.inHyperMode { get { return inHyperMode(); } }
		bool IBattler.isShadow { get { return isShadow(); } }
		int IBattler.displayGender { get { if (Gender == true) return 1; else if (Gender == false) return 0; else return -1; } }
		bool IBattler.owned { get { return IsOwned; } }
		int IBattler.pbSpeed { get { return SPE; } }

		void IBattler.pbInitDummyPokemon(IPokemon pkmn, int pkmnIndex)
		{
			//throw new NotImplementedException();
		}

		bool IBattler.pbHasMove(Moves id)
		{
			throw new NotImplementedException();
		}

		bool IBattler.pbHasMoveFunction(int code)
		{
			throw new NotImplementedException();
		}

		IBattlerIE IBattlerIE.initialize(IBattle btl, int index)
		{
			return (IBattlerIE)((IBattler)this).initialize(btl, index);
		}*/

		IBattlerIE IBattlerIE.pbInitialize(IPokemon pkmn, int index, bool batonpass)
		{
			return (IBattlerIE)base.pbInitialize(pkmn, index, batonpass);
		}

		IBattlerIE IBattlerIE.pbReset()
		{
			return (IBattlerIE)base.pbReset();
		}
		#endregion

		#region 
		new public static IBattlerIE[] GetBattlers(PokemonEssentials.Interface.PokeBattle.IPokemon[] input, Battle btl)
		{
			IBattlerIE[] battlers = new IBattlerIE[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				//battlers[i] = (IBattler)input[i];
				battlers[i] = (IBattlerIE)new Pokemon(btl, (sbyte)i);
				battlers[i].pbInitPokemon(input[i], (sbyte)i);
			}
			return battlers;
		}

		//public static implicit operator PokemonEssentials.Interface.PokeBattle.IPokemon(PokemonUnity.Combat.Pokemon pkmn)
		//{
		//	if (pkmn == null) return new PokemonEssentials.Interface.PokeBattle.IPokemon();
		//	PokemonEssentials.Interface.PokeBattle.IPokemon pokemon = pkmn.pokemon;
		//	//if (pokemon == null) return null;
		//	if ((Pokemons)pokemon.Species == Pokemons.NONE) return new PokemonEssentials.Interface.PokeBattle.IPokemon(Pokemons.NONE);
		//	Ribbon[] ribbons = pokemon.Ribbons; //new Ribbon[pokemon.Ribbons.Length];
		//	//for (int i = 0; i < ribbons.Length; i++)
		//	//{
		//	//	ribbons[i] = (Ribbon)pokemon.Ribbons[i];
		//	//}
		//
		//	Attack.Move[] moves = new Attack.Move[pokemon.moves.Length];
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		moves[i] = pokemon.moves[i];
		//	}
		//
		//	PokemonEssentials.Interface.PokeBattle.IPokemon normalPokemon = //pokemon;
		//		new PokemonEssentials.Interface.PokeBattle.IPokemon
		//		(
		//			(Pokemons)pokemon.Species, pokemon.OT,
		//			pokemon.Name, pokemon.FormId, (Abilities)pokemon.Ability,
		//			(Monster.Natures)pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
		//			pokemon.Pokerus, pokemon.HeartGuageSize, /*pokemon.IsHyperMode,*/ pokemon.ShadowLevel,
		//			pokemon.HP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
		//			pokemon.ObtainLevel, /*pokemon.CurrentLevel,*/ pokemon.exp,
		//			pokemon.Happiness, (Status)pokemon.Status, pokemon.StatusCount,
		//			pokemon.EggSteps, (Items)pokemon.ballUsed, pokemon.Mail, moves,
		//			pokemon.MoveArchive, ribbons, pokemon.Markings, pokemon.PersonalId,
		//			(PokemonEssentials.Interface.PokeBattle.IPokemon.ObtainedMethod)pokemon.ObtainedMode,
		//			pokemon.TimeReceived, pokemon.TimeEggHatched
		//		);
		//	return normalPokemon;
		//}

		//public static implicit operator PokemonUnity.Combat.Pokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		//{
		//	Pokemon pkmn = new Pokemon();
		//	if (pokemon == null) return pkmn;
		//
		//	if (pokemon != null && pokemon.Species != Pokemons.NONE)
		//	{
		//		pkmn.PersonalId = pokemon.PersonalId;
		//		//PublicId in pokemon is null, so Pokemon returns null
		//		//pkmn.PublicId				= pokemon.TrainerId;
		//
		//		if (!pokemon.OT.Equals((object)null))
		//		{
		//			pkmn.TrainerName = pokemon.OT.Name;
		//			pkmn.TrainerIsMale = pokemon.OT.Gender;
		//			pkmn.TrainerTrainerId = pokemon.OT.TrainerID;
		//			pkmn.TrainerSecretId = pokemon.OT.SecretID;
		//		}
		//
		//		pkmn.Species = (int)pokemon.Species;
		//		pkmn.form = pokemon.Form;
		//		pkmn.NickName = pokemon.Name;
		//
		//		pkmn.Ability = (int)pokemon.Ability;
		//
		//		//pkmn.Nature				= pokemon.getNature();
		//		pkmn.Nature = (int)pokemon.Nature;
		//		pkmn.IsShiny = pokemon.IsShiny;
		//		pkmn.Gender = pokemon.Gender;
		//
		//		//pkmn.PokerusStage			= pokemon.PokerusStage;
		//		pkmn.Pokerus = pokemon.Pokerus;
		//		//pkmn.PokerusStrain		= pokemon.PokerusStrain;
		//
		//		pkmn.IsHyperMode = pokemon.isHyperMode;
		//		//pkmn.IsShadow				= pokemon.isShadow;
		//		pkmn.ShadowLevel = pokemon.ShadowLevel;
		//
		//		pkmn.CurrentHP = pokemon.HP;
		//		pkmn.Item = (int)pokemon.Item;
		//
		//		pkmn.IV = pokemon.IV;
		//		pkmn.EV = pokemon.EV;
		//
		//		pkmn.ObtainedLevel = pokemon.ObtainLevel;
		//		//pkmn.CurrentLevel			= pokemon.Level;
		//		pkmn.CurrentExp = pokemon.Exp;
		//
		//		pkmn.Happines = pokemon.Happiness;
		//
		//		pkmn.Status = (int)pokemon.Status;
		//		pkmn.StatusCount = pokemon.StatusCount;
		//
		//		pkmn.EggSteps = pokemon.EggSteps;
		//
		//		pkmn.BallUsed = (int)pokemon.ballUsed;
		//		if (pokemon.Item != Items.NONE && Kernal.ItemData[pokemon.Item].IsLetter)//PokemonUnity.Inventory.Mail.IsMail(pokemon.Item))
		//		{
		//			pkmn.Mail = new SeriMail(pokemon.Item, pokemon.Mail);
		//		}
		//
		//		pkmn.Moves = new SeriMove[4];
		//		for (int i = 0; i < 4; i++)
		//		{
		//			pkmn.Moves[i] = pokemon.moves[i];
		//		}
		//
		//		//Ribbons is also null, we add a null check
		//		if (pokemon.Ribbons != null)
		//		{
		//			pkmn.Ribbons = new int[pokemon.Ribbons.Count];
		//			for (int i = 0; i < pkmn.Ribbons.Length; i++)
		//			{
		//				pkmn.Ribbons[i] = (int)pokemon.Ribbons[i];
		//			}
		//		}
		//		//else //Dont need else, should copy whatever value is given, even if null...
		//		//{
		//		//	pkmn.Ribbons			= new int[0];
		//		//}
		//		pkmn.Markings = pokemon.Markings;
		//
		//		pkmn.ObtainedMethod = (int)pokemon.ObtainedMode;
		//		pkmn.TimeReceived = pokemon.TimeReceived;
		//		//try
		//		//{
		//		pkmn.TimeEggHatched = pokemon.TimeEggHatched;
		//		//}
		//		//catch (Exception) { seriPokemon.TimeEggHatched = new DateTimeOffset(); }
		//	}
		//
		//	return pkmn;
		//}
		#endregion
#pragma warning restore 0162
	}

	public interface IBattlerIE : PokemonEssentials.Interface.PokeBattle.IBattler
	{
		#region Battle Related
		new IBattleIE battle					{ get; }
		#endregion

		#region Creating a battler
		//new IBattlerIE initialize(IBattle btl, int index);
		//void pbInitPokemon(IPokemon pkmn, int pkmnIndex);
		//void pbInitDummyPokemon(IPokemon pkmn, int pkmnIndex);
		//void pbInitBlank();
		//void pbInitPermanentEffects();
		//void pbInitEffects(bool batonpass);
		//void pbUpdate(bool fullchange = false);
		new IBattlerIE pbInitialize(IPokemon pkmn, int index, bool batonpass);
		/// <summary>
		/// Used only to erase the battler of a Shadow Pokémon that has been snagged.
		/// </summary>
		new IBattlerIE pbReset();
		/// <summary>
		/// Update Pokémon who will gain EXP if this battler is defeated
		/// </summary>
		//void pbUpdateParticipants();
		#endregion

		#region About this Battler
		//string ToString(bool lowercase = false);
		//bool hasMoldBreaker();
		//bool hasWorkingAbility(Abilities ability, bool ignorefainted = false);
		//bool pbHasType(Types type);
		//bool pbHasMoveType(Types type);
		//bool hasMovedThisRound();
		//bool hasMovedThisRound(int? turncount);
		//bool hasWorkingItem(Items item, bool ignorefainted = false);
		//bool hasWorkingBerry(bool ignorefainted = false);
		//bool isAirborne(bool ignoreability = false);
		#endregion

		#region Change HP
		IEnumerator pbReduceHP(int amt, bool animate = false, bool registerDamage = true, System.Action<int> result = null);
		IEnumerator pbRecoverHP(int amount, bool animate = false, System.Action<int> result = null);
		IEnumerator pbFaint(bool showMessage = true, System.Action<bool> result = null);
		#endregion

		#region Find other battlers/sides in relation to this battler
		/// <summary>
		/// Returns the data structure for this battler's side
		/// </summary>
		/// Player: 0 and 2; Foe: 1 and 3
		//IEffectsSide OwnSide { get; }
		//IEffectsSide pbOwnSide { get; }
		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// Player: 1 and 3; Foe: 0 and 2
		//IEffectsSide pbOpposingSide { get; }
		/// <summary>
		/// Returns whether the position belongs to the opposing Pokémon's side
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		//bool pbIsOpposing(int i);
		/// <summary>
		/// Returns the battler's partner
		/// </summary>
		//IBattlerIE Partner { get; }
		new IBattlerIE pbPartner { get; }
		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		new IBattlerIE pbOppositeOpposing { get; }
		//ToDo: If not double battle return null?
		new IBattlerIE pbOppositeOpposing2 { get; }
		new IBattlerIE pbOpposing1 { get; }
		//ToDo: If not double battle return null?
		new IBattlerIE pbOpposing2 { get; }
		/// <summary>
		/// Returns the battler's first opposing Pokémon Index
		/// </summary>
		//int OpposingIndex { get; }
		//int pbNonActivePokemonCount();
		#endregion

		#region Forms
		/// <summary>
		/// </summary>
		/// ToDo: Changes stats on form changes here?
		/// ToDo: Use PokemonUnity.Battle.Form to modify Pokemon._base,
		/// Which will override and modify base stats for values that inherit it
		/// Castform and Unown should use (int)Form, and others will use (PokemonData)Form
		new IEnumerator pbCheckForm();
		//void pbResetForm();
		#endregion

		#region Ability Effects
		new IEnumerator pbAbilitiesOnSwitchIn(bool onactive);
		IEnumerator pbEffectsOnDealingDamage(IBattleMove move, IBattlerIE user, IBattlerIE target, int damage);
		IEnumerator pbEffectsAfterHit(IBattlerIE user, IBattlerIE target, IBattleMove thismove, IEffectsMove turneffects);
		new IEnumerator pbAbilityCureCheck();
		#endregion

		#region Held Item effects
		new IEnumerator pbConsumeItem(bool recycle = true, bool pickup = true);
		IEnumerator pbConfusionBerry(Inventory.Plants.Flavours flavor, string message1, string message2, System.Action<bool> result);
		IEnumerator pbStatIncreasingBerry(PokemonUnity.Combat.Stats stat, string berryname, System.Action<bool> result);
		new IEnumerator pbActivateBerryEffect(Items berry = Items.NONE, bool consume = true);
		new IEnumerator pbBerryCureCheck(bool hpcure = false);
		#endregion

		#region Move user and targets
		IBattlerIE pbFindUser(IBattleChoice choice, IList<IBattlerIE> targets);
		IEnumerator pbChangeUser(IBattleMove thismove, IBattlerIE user, System.Action<IBattlerIE> result = null);
		//Attack.Data.Targets pbTarget(IBattleMove move);
		bool pbAddTarget(IList<IBattlerIE> targets,IBattlerIE target);
		bool pbAddTarget(ref IList<IBattlerIE> targets, IBattlerIE target);
		void pbRandomTarget(IList<IBattlerIE> targets);
		IEnumerator pbChangeTarget(IBattleMove thismove, IBattlerIE[] userandtarget, IBattlerIE[] targets, System.Action<bool> result);
		#endregion

		#region Move PP
		//void pbSetPP(IBattleMove move, int pp);
		//void pbSetPP(Attack.Move move,byte pp);
		//bool pbReducePP(IBattleMove move);
		//void pbReducePPOther(IBattleMove move);
		#endregion

		#region Using a move
		//bool pbObedienceCheck(IBattleChoice choice) {
		IEnumerator pbObedienceCheck(IBattleChoice choice, System.Action<bool> result);
		IEnumerator pbSuccessCheck(IBattleMove thismove, IBattlerIE user, IBattlerIE target, IEffectsMove turneffects, bool accuracy = true, System.Action<bool> result = null);
		//bool pbTryUseMove(IBattleChoice choice,IBattleMove thismove,IEffectsMove turneffects) {
		IEnumerator pbTryUseMove(IBattleChoice choice, IBattleMove thismove, IEffectsMove turneffects, System.Action<bool> result);
		//void pbConfusionDamage();
		//void pbUpdateTargetedMove(IBattleMove thismove, IBattlerIE user);
		IEnumerator pbProcessMoveAgainstTarget(IBattleMove thismove, IBattlerIE user, IBattlerIE target, int numhits, IEffectsMove turneffects, bool nocheck = false, int[] alltargets = null, bool showanimation = true);
		new IEnumerator pbUseMoveSimple(Moves moveid, int index = -1, int target = -1);
		new IEnumerator pbUseMove(IBattleChoice choice, bool specialusage = false);
		//void pbCancelMoves();
		#endregion

		#region Turn processing
		new IEnumerator pbBeginTurn(IBattleChoice choice);
		//private void _pbEndTurn(IBattleChoice choice) {
		new IEnumerator pbEndTurn(IBattleChoice choice);
		//bool pbProcessTurn(IBattleChoice choice) {
		IEnumerator pbProcessTurn(IBattleChoice choice, System.Action<bool> result);
		#endregion

		#region Explicit Interface Implementation
		//bool inHyperMode { get; }
		//bool isShadow { get; }
		//int displayGender { get; }
		//bool owned { get; }
		//int pbSpeed { get; }
		//
		//void pbInitDummyPokemon(IPokemon pkmn, int pkmnIndex);
		//bool pbHasMove(Moves id);
		//bool pbHasMoveFunction(int code);
		#endregion
	}
}
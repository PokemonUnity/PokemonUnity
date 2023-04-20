using System.Collections;
using PokemonUnity.Combat;
using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;


namespace PokemonUnity.UX
{
	public partial class Battler : PokemonUnity.Combat.Pokemon, IBattlerShadowPokemonIE
	{
		//public void pbInitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, int pkmnIndex) { //, params object[] placeholder
		//	if (pokemonIndex>0 && pkmn is IPokemonShadowPokemon p && inHyperMode() && !isFainted()) { //ToDo: Should move this to an Event Listener based on Battle Menu Selection
		//		// Called out of hypermode
		//		p.hypermode=false;
		//		//p.isHyperMode=false;
		//		//p.adjustHeart(-50);
		//		p.decreaseShadowLevel(Monster.PokemonActions.CallTo);
		//	}
		//	(this as IBattler).pbInitPokemon((IPokemon)pkmn, pkmnIndex); //this._InitPokemon(pkmn, pkmnIndex);
		//	// Called into battle
		//	if (isShadow()) { 
		//		//if (hasConst(Types.SHADOW))
		//			Type1=Types.SHADOW;
		//			Type2=Types.SHADOW;
		//		//}
		//		//if (@battle.pbOwnedByPlayer(@Index)) this.pokemon.adjustHeart(-30);
		//		if (@battle.pbOwnedByPlayer(@Index)) (this.pokemon as IPokemonShadowPokemon).decreaseShadowLevel(Monster.PokemonActions.Battle);
		//	}
		//}

		new public virtual IEnumerator pbEndTurn(IBattleChoice choice) { yield return (this as IBattlerShadowPokemonIE).pbEndTurn(choice); }
		IEnumerator IBattlerShadowPokemonIE.pbEndTurn(IBattleChoice choice) { //, params object[] placeholder
			yield return (this as IBattlerIE).pbEndTurn(choice); //this._pbEndTurn(choice);
			if (inHyperMode() && !this.battle.pbAllFainted(this.battle.party1) &&
				!this.battle.pbAllFainted(this.battle.party2)) { 
				this.battle.pbDisplay(Game._INTL("Its hyper mode attack hurt {1}!",this.ToString(true)));
				pbConfusionDamage();
			}
		}

		/*public bool isShadow() {
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

		public void pbHyperMode() { 
			PokemonEssentials.Interface.PokeBattle.IPokemon pkmn=this.pokemon;
			if (pkmn is IPokemonShadowPokemon p && isShadow() && !IsHyperMode)
				if (@battle.pbRandom(p.ShadowLevel.Value)<=Monster.Pokemon.HEARTGAUGESIZE/4) { //p.heartgauge
					isHyperMode=true;
					@battle.pbDisplay(Game._INTL("{1}'s emotions rose to a fever pitch!\nIt entered Hyper Mode!",this.ToString()));
				}
		}

		public bool pbHyperModeObedience(IBattleMove move) { 
			if (!move.IsNotNullOrNone()) return true;
			if (this.inHyperMode() && move.Type!=Types.SHADOW)
				return @battle.pbRandom(10)<8 ? false : true;
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
						if (Game.GameData is IGameShadowPokemon g) g.pbReadyToPurify(p);
					}
				}
		}*/
	}

	public interface IBattlerShadowPokemonIE : PokemonEssentials.Interface.PokeBattle.IBattlerShadowPokemon
	{
		//void pbInitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, int pkmnIndex);
		new IEnumerator pbEndTurn(IBattleChoice choice);
		//bool isShadow();
		//bool inHyperMode();
		//void pbHyperMode();
		//bool pbHyperModeObedience(IBattleMove move);

		//Events.onStartBattle+=delegate(object sender, EventArgs e) {
		//void Events_OnStartBattle(object sender, System.EventArgs e);

		//Events.onEndBattle+=delegate(object sender, EventArgs e) {
		//void Events_OnEndBattle(object sender, IOnEndBattleEventArgs e);
	}

	//public partial class UnityBattle : PokemonUnity.Combat.Battle, IBattleShadowPokemonIE
	//{
	//	/// <summary>
	//	/// Uses an item on a Pokémon in the player's party.
	//	/// </summary>
	//	/// <param name="item"></param>
	//	/// <param name="pkmnIndex"></param>
	//	/// <param name="userPkmn"></param>
	//	/// <param name="scene"></param>
	//	/// <returns></returns>
	//	/// <remarks>Specifically for Shadow Pokemon Usage</remarks>
	//	public IEnumerator pbUseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result) 
	//	{ return (this as IBattleShadowPokemonIE).pbUseItemOnPokemon(item, pkmnIndex, userPkmn, scene, result: value => result(value)); }
	//	IEnumerator IBattleShadowPokemonIE.pbUseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result)
	//	{
	//		IPokemon pokemon = this.party1[pkmnIndex];
	//		if (pokemon is IPokemonShadowPokemon p && p.hypermode) { //&&
	//			//item != Items.JOY_SCENT &&
	//			//item != Items.EXCITE_SCENT &&
	//			//item != Items.VIVID_SCENT) {
	//			yield return scene.pbDisplay(Game._INTL("This item can't be used on that Pokemon."));
	//			result(false); yield break;
	//		}
	//		//return _pbUseItemOnPokemon(item,pkmnIndex,userPkmn,scene);
	//		yield return (this as IBattleIE).pbUseItemOnPokemon(item, pkmnIndex, (IBattlerIE)userPkmn, scene, result: value => result(value));
	//	}
	//}

	public partial class UnityBattleTest :  IBattleShadowPokemonIE
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
		public IEnumerator pbUseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result) 
		{ return (this as IBattleShadowPokemonIE).pbUseItemOnPokemon(item, pkmnIndex, userPkmn, scene, result: value => result(value)); }
		IEnumerator IBattleShadowPokemonIE.pbUseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result)
		{
			IPokemon pokemon = this.party1[pkmnIndex];
			if (pokemon is IPokemonShadowPokemon p && p.hypermode) { //&&
				//item != Items.JOY_SCENT &&
				//item != Items.EXCITE_SCENT &&
				//item != Items.VIVID_SCENT) {
				yield return scene.pbDisplay(Game._INTL("This item can't be used on that Pokemon."));
				result(false); yield break;
			}
			//return _pbUseItemOnPokemon(item,pkmnIndex,userPkmn,scene);
			yield return (this as IBattleIE).pbUseItemOnPokemon(item, pkmnIndex, (IBattlerIE)userPkmn, scene, result: value => result(value));
		}
	}

	public interface IBattleShadowPokemonIE : PokemonEssentials.Interface.PokeBattle.IBattleShadowPokemon
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
		IEnumerator pbUseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result);
	}
}
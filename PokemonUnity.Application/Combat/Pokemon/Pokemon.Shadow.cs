using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;


namespace PokemonUnity.Combat
{
	//ToDo: Rename class to Battler
	public partial class Pokemon : PokemonEssentials.Interface.PokeBattle.IBattlerShadowPokemon
	{
		public void pbInitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, int pkmnIndex) { //, params object[] placeholder
			if (pokemonIndex>0 && pkmn is IPokemonShadowPokemon p && inHyperMode() && !isFainted()) { //ToDo: Should move this to an Event Listener based on Battle Menu Selection
				// Called out of hypermode
				p.hypermode=false;
				//p.isHyperMode=false;
				//p.adjustHeart(-50);
				p.decreaseShadowLevel(Monster.PokemonActions.CallTo);
			}
			(this as IBattler).pbInitPokemon((IPokemon)pkmn, pkmnIndex); //this._InitPokemon(pkmn, pkmnIndex);
			// Called into battle
			if (isShadow()) { 
				//if (hasConst(Types.SHADOW))
					Type1=Types.SHADOW;
					Type2=Types.SHADOW;
				//}
				//if (@battle.pbOwnedByPlayer(@Index)) this.pokemon.adjustHeart(-30);
				if (@battle.pbOwnedByPlayer(@Index)) (this.pokemon as IPokemonShadowPokemon).decreaseShadowLevel(Monster.PokemonActions.Battle);
			}
		}

		public virtual void pbEndTurn(Choice choice) { (this as IBattlerShadowPokemon).pbEndTurn(choice); }
		void IBattlerShadowPokemon.pbEndTurn(Choice choice) { //, params object[] placeholder
			(this as IBattler).pbEndTurn(choice); //this._pbEndTurn(choice);
			if (inHyperMode() && !this.battle.pbAllFainted(this.battle.party1) &&
				!this.battle.pbAllFainted(this.battle.party2)) { 
				this.battle.pbDisplay(Game._INTL("Its hyper mode attack hurt {1}!",this.ToString(true)));
				pbConfusionDamage();
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
		
		/*Events.onStartBattle+=delegate(object sender, EventArgs e) {
			Game.GameData.PokemonTemp.heartgauges=[];
			for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
				Game.GameData.PokemonTemp.heartgauges[i]=Game.GameData.Trainer.party[i].heartgauge;
			}
		}

		Events.onEndBattle+=delegate(object sender, EventArgs e) {
			decision=e[0];
			canlose=e[1];
			for (int i = 0; i < Game.GameData.PokemonTemp.heartgauges.Length; i++) {
				pokemon=Game.GameData.Trainer.party[i];
				if (pokemon && (Game.GameData.PokemonTemp.heartgauges[i] &&
					Game.GameData.PokemonTemp.heartgauges[i]!=0 && pokemon.heartgauge==0)) {
					pbReadyToPurify(pokemon);
				}
			}
		}*/
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
		public bool pbUseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene) 
		{ return (this as IBattleShadowPokemon).pbUseItemOnPokemon(item, pkmnIndex, userPkmn, scene); }
		bool IBattleShadowPokemon.pbUseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene)
		{
			IBattler pokemon = this.party1[pkmnIndex];
			if (pokemon is IBattlerShadowPokemon p && p.hypermode) { //&&
				//item != Items.JOY_SCENT &&
				//item != Items.EXCITE_SCENT &&
				//item != Items.VIVID_SCENT) {
				scene.pbDisplay(Game._INTL("This item can't be used on that Pokemon."));
				return false;
			}
			//return _pbUseItemOnPokemon(item,pkmnIndex,userPkmn,scene);
			return (this as IBattle).pbUseItemOnPokemon(item, pkmnIndex, (IBattler)userPkmn, scene);
		}
	}
}
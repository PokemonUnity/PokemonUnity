using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonUnity.Monster;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.PokeBattle
{
	/*=begin;
	All types except Shadow have Shadow as a weakness
	Shadow has Shadow as a resistance.
	On a side note, the Shadow moves in Colosseum will not be affected by Weaknesses or Resistances while in XD, the Shadow Type is Super Effective against all other types.
	2/5 - display nature

	XD - Shadow Rush -- 55, 100 - Deals damage.
	Colosseum - Shadow Rush -- 90, 100
	If this attack is successful, user loses half of HP lost by opponent due to this attack (recoil).
	 If (user is in Hyper Mode, this attack has a good chance for a critical hit.
	=end;*/

	//public partial class PBWeather {
	//    public const int SHADOWSKY = 8;
	//}

	/// <summary>
	/// Extensions of <seealso cref="Interface.ITempMetadata"/>
	/// </summary>
	public interface ITempMetadataPokemonShadow {
		int[] heartgauges				{ get; }
	}

	/// <summary>
	/// Extensions of <seealso cref="IGame"/>
	/// </summary>
	public interface IGameShadowPokemon
	{
		void pbPurify(IPokemonShadowPokemon pokemon, IPurifyChamberScene scene);

		#region Relic Stone Logic
		void pbRelicStoneScreen(IPokemonShadowPokemon pkmn);

		bool pbIsPurifiable(IPokemonShadowPokemon pkmn);

		bool pbHasPurifiableInParty();

		void pbRelicStone();

		bool pbRaiseHappinessAndReduceHeart(IPokemonShadowPokemon pokemon, IScene scene, int amount);

		void pbApplyEVGain(IPokemon pokemon, Stats ev, int evgain);

		void pbReplaceMoves(IPokemon pokemon, Moves move1, Moves move2 = 0, Moves move3 = 0, Moves move4 = 0);
		#endregion

		void pbReadyToPurify(IPokemonShadowPokemon pokemon);
	}

	/// <summary>
	/// Extensions of <seealso cref="IPokemon"/>
	/// </summary>
	public interface IPokemonShadowPokemon
	{
		//public const int HEARTGAUGESIZE = 3840;
		int? heartgauge { get; }
		bool shadow { get; set; }
		bool hypermode { get; set; }
		int[] savedev { get; set; }
		int savedexp { get; set; }
		Moves[] shadowmoves { get; }
		int shadowmovenum { get; }

		int heartStage { get; }

		void adjustHeart(int value);

		bool isShadow { get; }

		void makeShadow();

		void pbUpdateShadowMoves(bool allmoves = false);

		//alias :__shadow_expeq :exp=;

		int exp { get; set; }

		//alias :__shadow_hpeq :hp=;

		int hp { get; set; }

		/// <summary>
		/// Heart Gauge.
		/// The Heart Gauge is split into five equal bars. When a Shadow Pokémon is first snagged, all five bars are full.
		/// </summary>
		/// <remarks>
		/// If pokemon is purified, shadow level should be equal to -1
		/// If pokemon has never been shadowed, then value should be null
		/// HeartGuage max size should be determined by _base.database
		/// </remarks>
		int? ShadowLevel	{ get; }
		int HeartGuageSize	{ get; }
		void decreaseShadowLevel(PokemonActions action);
	}

	/// <summary>
	/// Extensions of <seealso cref="IBattle"/>
	/// </summary>
	public interface IBattleShadowPokemon {
		//alias __shadow_pbUseItemOnPokemon pbUseItemOnPokemon;

		bool pbUseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene);
	}

	/// <summary>
	/// Extensions of <seealso cref="IBattler"/>
	/// </summary>
	public interface IBattlerShadowPokemon {
		//alias __shadow_pbInitPokemon pbInitPokemon;
		//alias __shadow_pbEndTurn pbEndTurn;

		void pbInitPokemon(IPokemon pkmn, int pkmnIndex);

		void pbEndTurn(IBattleChoice choice);

		bool isShadow();

		bool inHyperMode();

		void pbHyperMode();

		bool pbHyperModeObedience(IBattleMove move);
	}

	//Events.onStartBattle+=delegate(object sender, EventArgs e) {
	//   Game.GameData.PokemonTemp.heartgauges=[];
	//   for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
	//     Game.GameData.PokemonTemp.heartgauges[i]=Game.GameData.Trainer.party[i].heartgauge;
	//   }
	//}

	//Events.onEndBattle+=delegate(object sender, EventArgs e) {
	//   decision=e[0];
	//   canlose=e[1];
	//   for (int i = 0; i < Game.GameData.PokemonTemp.heartgauges.Length; i++) {
	//     pokemon=Game.GameData.Trainer.party[i];
	//     if (pokemon && (Game.GameData.PokemonTemp.heartgauges[i] &&
	//        Game.GameData.PokemonTemp.heartgauges[i]!=0 && pokemon.heartgauge==0)) {
	//       pbReadyToPurify(pokemon);
	//     }
	//   }
	//}
	//ItemHandlers.UseOnPokemon.add(:JOYSCENT,proc{|item,pokemon,scene|
	//   pbRaiseHappinessAndReduceHeart(pokemon,scene,500);
	//});

	//ItemHandlers.UseOnPokemon.add(:EXCITESCENT,proc{|item,pokemon,scene|
	//   pbRaiseHappinessAndReduceHeart(pokemon,scene,1000);
	//});

	//ItemHandlers.UseOnPokemon.add(:VIVIDSCENT,proc{|item,pokemon,scene|
	//   pbRaiseHappinessAndReduceHeart(pokemon,scene,2000);
	//});

	//ItemHandlers.UseOnPokemon.add(:TIMEFLUTE,proc{|item,pokemon,scene|
	//   if (!pokemon.isShadow?) {
	//     scene.pbDisplay(_INTL("It won't have any effect."));
	//     next false;
	//   }
	//   pokemon.heartgauge=0;
	//   pbReadyToPurify(pokemon);
	//   next true;
	//});

	//ItemHandlers.BattleUseOnBattler.add(:JOYSCENT,proc{|item,battler,scene|
	//   if (!battler.isShadow?) {
	//     scene.pbDisplay(_INTL("It won't have any effect."));
	//     return false;
	//   }
	//   if (battler.inHyperMode?) {
	//     battler.pokemon.hypermode=false;
	//     battler.pokemon.adjustHeart(-300);
	//     scene.pbDisplay(_INTL("{1} came to its senses from the {2}!",battler.ToString(),item.ToString(TextScripts.Name)));
	////     if battler.happiness!=255 || battler.pokemon.heartgauge!=0
	////       pbRaiseHappinessAndReduceHeart(battler.pokemon,scene,500)
	////     }
	//     return true;
	//   }
	////   return pbRaiseHappinessAndReduceHeart(battler.pokemon,scene,500)
	//   scene.pbDisplay(_INTL("It won't have any effect."));
	//   return false;
	//});

	//ItemHandlers.BattleUseOnBattler.add(:EXCITESCENT,proc{|item,battler,scene|
	//   if (!battler.isShadow?) {
	//     scene.pbDisplay(_INTL("It won't have any effect."));
	//     return false;
	//   }
	//   if (battler.inHyperMode?) {
	//     battler.pokemon.hypermode=false;
	//     battler.pokemon.adjustHeart(-300);
	//     scene.pbDisplay(_INTL("{1} came to its senses from the {2}!",battler.ToString(),item.ToString(TextScripts.Name)));
	////     if battler.happiness!=255 || battler.pokemon.heartgauge!=0
	////       pbRaiseHappinessAndReduceHeart(battler.pokemon,scene,1000)
	////     }
	//     return true;
	//   }
	////   return pbRaiseHappinessAndReduceHeart(battler.pokemon,scene,1000)
	//   scene.pbDisplay(_INTL("It won't have any effect."));
	//   return false;
	//});

	//ItemHandlers.BattleUseOnBattler.add(:VIVIDSCENT,proc{|item,battler,scene|
	//   if (!battler.isShadow?) {
	//     scene.pbDisplay(_INTL("It won't have any effect."));
	//     return false;
	//   }
	//   if (battler.inHyperMode?) {
	//     battler.pokemon.hypermode=false;
	//     battler.pokemon.adjustHeart(-300);
	//     scene.pbDisplay(_INTL("{1} came to its senses from the {2}!",battler.ToString(),item.ToString(TextScripts.Name)));
	////     if battler.happiness!=255 || battler.pokemon.heartgauge!=0
	////       pbRaiseHappinessAndReduceHeart(battler.pokemon,scene,2000)
	////     }
	//     return true;
	//   }
	////   return pbRaiseHappinessAndReduceHeart(battler.pokemon,scene,2000)
	//   scene.pbDisplay(_INTL("It won't have any effect."));
	//   return false;
	//});

//Events.onStepTaken+=proc{
//   foreach (var pkmn in Game.GameData.Trainer.party) {
//     if (pkmn.HP>0 && !pkmn.isEgg? && pkmn.heartgauge>0) {
//       pkmn.adjustHeart(-1);
//       if (pkmn.heartgauge==0) pbReadyToPurify(pkmn);
//     }
//   }
//   if ((Game.GameData.Global.purifyChamber rescue null)) {
//     Game.GameData.Global.purifyChamber.update();
//   }
//   for (int i = 0; i < 2; i++) {
//     pkmn=Game.GameData.Global.daycare[i][0];
//     if (!pkmn) continue;
//     pkmn.adjustHeart(-1);
//     pkmn.pbUpdateShadowMoves();
//   }
//}
}
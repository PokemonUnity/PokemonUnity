using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface
{
	namespace EventArg
	{
		#region Encounter Modifier EventArgs
		public class EncounterModifierEventArgs : EventArgs
		{
			public readonly int EventId = typeof(EncounterModifierEventArgs).GetHashCode();

			public int Id { get; }
			//ToDo: Either it's the encounter logic or the pokemon itself...
			//[species,min/max]; i dont think there's 3 parameters...
			public IEncounter Encounter { get; set; }
		}
		#endregion
	}

	namespace Field
	{
		/// <summary>
		/// This module stores encounter-modifying events that can happen during the game.
		/// A procedure can subscribe to an event by adding itself to the event.  It will
		/// then be called whenever the event occurs.
		/// </summary>
		public interface IEncounterModifier
		{
			//ToDo: Either it's the encounter logic or the pokemon itself...
			//List<Action<IEncounter>> procs { get; }
			//List<Action> procsEnd { get; }

			event EventHandler<EncounterModifierEventArgs> OnEncounter;
			event EventHandler OnEncounterEnd;

			//void register(Action<IEncounter> p);

			//void registerEncounterEnd(Action<IEncounter> p);
			//void registerEncounterEnd(Action p);

			//IEncounter trigger(IEncounter encounter);

			void triggerEncounterEnd();
		}

		/// <summary>
		/// Extension of base game class... <seealso cref="IGame"/>
		/// </summary>
		public interface IGameField
		{
			IPokeBattle_Scene pbNewBattleScene();

			IEnumerator pbSceneStandby();

			IEnumerator pbBattleAnimation(IAudioBGM bgm = null, int trainerid = -1, string trainername = "");

			// Alias and use this method if you want to add a custom battle intro animation
			// e.g. variants of the Vs. animation.
			// Note that Game.GameData.GameTemp.background_bitmap contains an image of the current game
			// screen.
			// When the custom animation has finished, the screen should have faded to black
			// somehow.
			bool pbBattleAnimationOverride(IViewport viewport, int trainerid = -1, string trainername = "");

			void pbPrepareBattle(IBattle battle);

			Environments pbGetEnvironment();

			IPokemon pbGenerateWildPokemon(Pokemons species, int level, bool isroamer = false);

			PokemonUnity.Combat.BattleResults pbWildBattle(Pokemons species, int level, int? variable = null, bool canescape = true, bool canlose = false);

			PokemonUnity.Combat.BattleResults pbDoubleWildBattle(Pokemons species1, int level1, Pokemons species2, int level2, int? variable = null, bool canescape = true, bool canlose = false);

			void pbCheckAllFainted();

			void pbEvolutionCheck(int currentlevels);

			Items[] pbDynamicItemList(params Items[] args);

			// Runs the Pickup event after a battle if a Pokemon has the ability Pickup.
			void pbPickup(IPokemon pokemon);

			bool pbEncounter(EncounterTypes enctype);

			//Events.onStartBattle+=delegate(object sender, EventArgs e) {
			//  Game.GameData.PokemonTemp.evolutionLevels=[];
			//  for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
			//	Game.GameData.PokemonTemp.evolutionLevels[i]=Game.GameData.Trainer.party[i].level;
			//  }
			//}

			//Events.onEndBattle+=delegate(object sender, EventArgs e) {
			//  decision=e[0];
			//  canlose=e[1];
			//  if (USENEWBATTLEMECHANICS || (decision!=2 && decision!=5)) {		// not a loss or a draw
			//	if (Game.GameData.PokemonTemp.evolutionLevels) {
			//	  pbEvolutionCheck(Game.GameData.PokemonTemp.evolutionLevels);
			//	  Game.GameData.PokemonTemp.evolutionLevels=null;
			//	}
			//  }
			//  if (decision==1) {
			//	foreach (var pkmn in Game.GameData.Trainer.party) {
			//	  Kernel.pbPickup(pkmn);
			//	  if (pkmn.ability == Abilities.HONEYGATHER && !pkmn.isEgg? && !pkmn.hasItem?) {
			//		if (hasConst(PBItems,:HONEY)) {
			//		  chance = 5 + ((pkmn.level-1)/10).floor*5;
			//		  if (Core.Rand.Next(100)<chance) pkmn.setItem(Items.HONEY);
			//		}
			//	  }
			//	}
			//  }
			//  if ((decision==2 || decision==5) && !canlose) {
			//	Game.GameData.GameSystem.bgm_unpause;
			//	Game.GameData.GameSystem.bgs_unpause;
			//	Kernel.pbStartOver;
			//  }
			//}

			//void pbOnSpritesetCreate(spriteset, IViewport viewport);

			#region Field movement
			bool pbLedge(float xOffset, float yOffset);

			void pbSlideOnIce(IEntity _event = null);

			void pbBattleOnStepTaken();

			void pbOnStepTaken(bool eventTriggered);

			// This method causes a lot of lag when the game is encrypted
			//ICharset pbGetPlayerCharset(meta, charset, trainer= null);

			void pbUpdateVehicle();

			/// <summary>
			/// </summary>
			/// <param name="destination">Map location id</param>
			void pbCancelVehicles(int? destination = null);

			bool pbCanUseBike(int mapid);

			void pbMountBike();

			void pbDismountBike();

			void pbSetPokemonCenter();
			#endregion

			#region Fishing
			void pbFishingBegin();

			void pbFishingEnd();

			bool pbFishing(bool hasencounter, int rodtype = 1);

			bool pbWaitForInput(IWindow msgwindow, string message, int frames);

			bool pbWaitMessage(IWindow msgwindow, int time);
			#endregion

			#region Moving between maps
			void pbStartOver(bool gameover = false);

			void pbCaveEntranceEx(bool exiting);

			void pbCaveEntrance();

			void pbCaveExit();

			void pbSetEscapePoint();

			void pbEraseEscapePoint();
			#endregion

			#region Partner trainer
			void pbRegisterPartner(TrainerTypes trainerid, string trainername, int partyid = 0);

			void pbDeregisterPartner();
			#endregion

			#region Constant checks
			/// <summary>
			/// Returns whether the Poké Center should explain Pokérus to the player, if a
			/// healed Pokémon has it.
			/// </summary>
			/// <returns></returns>
			bool pbPokerus();
			#endregion

			bool pbBatteryLow();

			#region Audio playing
			void pbCueBGM(IAudioBGM bgm, int seconds, int? volume = null, int? pitch = null);

			void pbAutoplayOnTransition();

			void pbAutoplayOnSave();
			#endregion

			#region Voice recorder
			/// <summary>
			/// </summary>
			/// BGM audio file?
			IAudioObject pbRecord(string text, float maxtime = 30.0f);

			bool pbRxdataExists(string file);
			#endregion

			#region Gaining items
			bool pbItemBall(Items item, int quantity = 1);

			bool pbReceiveItem(Items item, int quantity = 1);

			void pbUseKeyItem();
			#endregion

			#region Bridges
			void pbBridgeOn(int height = 2);

			void pbBridgeOff();
			#endregion

			#region Event locations, terrain tags
			bool pbEventFacesPlayer(IEntity _event, IGamePlayer player, int distance);

			bool pbEventCanReachPlayer(IEntity _event, IGamePlayer player, int distance);

			ITilePosition pbFacingTileRegular(int? direction = null, IEntity _event = null);

			ITilePosition pbFacingTile(int? direction = null, IEntity _event = null);

			bool pbFacingEachOther(IEntity event1, IEntity event2);

			Terrains pbGetTerrainTag(IEntity _event = null, bool countBridge = false);

			Terrains pbFacingTerrainTag(IEntity _event = null, int? dir = null);
			#endregion

			#region Event movement
			IEnumerator pbTurnTowardEvent(IEntity _event, IEntity otherEvent);

			IEnumerator pbMoveTowardPlayer(IEntity _event);

			bool pbJumpToward(int dist = 1, bool playSound = false, bool cancelSurf = false);

			void pbWait(int numframes);
			#endregion

			//MoveRoute pbMoveRoute(IEntity _event, string[] commands, bool waitComplete= false);

			#region Screen effects
			void pbToneChangeAll(ITone tone, int duration);

			void pbShake(int power, int speed, int frames);

			void pbFlash(IColor color, int frames);

			void pbScrollMap(int direction, int distance, int speed);
			#endregion
		}

		/// <summary>
		/// Extension of <see cref="Interface.Field.ITempMetadata"/>
		/// </summary>
		public interface ITempMetadataField
		{
			#region 
			EncounterTypes encounterType	{ get; set; }
			int evolutionLevels				{ get; set; }
			#endregion

			#region 
			bool batterywarning { get; set; }
			IAudioBGM cueBGM { get; set; }
			int cueFrames { get; set; }
			#endregion
		}

		// ===============================================================================
		// Scene_Map and Spriteset_Map
		// ===============================================================================
		//public interface Scene_Map
		//{
		//	void createSingleSpriteset(map);
		//}

		//public interface Spriteset_Map
		//{
		//	public void getAnimations();
		//
		//	public void restoreAnimations(anims);
		//}

		/*public static partial class PBMoveRoute
		{
			Down               = 1;
		  Left               = 2;
		  Right              = 3;
		  Up                 = 4;
		  LowerLeft          = 5;
		  LowerRight         = 6;
		  UpperLeft          = 7;
		  UpperRight         = 8;
		  Random             = 9;
		  TowardPlayer       = 10;
		  AwayFromPlayer     = 11;
		  Forward            = 12
		  Backward           = 13;
		  Jump               = 14; // xoffset, yoffset
		  Wait               = 15; // frames
		  TurnDown           = 16;
		  TurnLeft           = 17;
		  TurnRight          = 18;
		  TurnUp             = 19;
		  TurnRight90        = 20;
		  TurnLeft90         = 21;
		  Turn180            = 22;
		  TurnRightOrLeft90  = 23;
		  TurnRandom         = 24;
		  TurnTowardPlayer   = 25;
		  TurnAwayFromPlayer = 26;
		  SwitchOn           = 27; // 1 param
		  SwitchOff          = 28; // 1 param
		  ChangeSpeed        = 29; // 1 param
		  ChangeFreq         = 30; // 1 param
		  WalkAnimeOn        = 31;
		  WalkAnimeOff       = 32;
		  StepAnimeOn        = 33;
		  StepAnimeOff       = 34;
		  DirectionFixOn     = 35;
		  DirectionFixOff    = 36;
		  ThroughOn          = 37;
		  ThroughOff         = 38;
		  AlwaysOnTopOn      = 39;
		  AlwaysOnTopOff     = 40;
		  Graphic            = 41; // Name, hue, direction, pattern
		  Opacity            = 42; // 1 param
		  Blending           = 43; // 1 param
		  PlaySE             = 44; // 1 param
		  Script             = 45; // 1 param
		  ScriptAsync        = 101; // 1 param
		}*/

		// ===============================================================================
		// Events
		// ===============================================================================
		public interface IGameEventField
		{
			bool cooledDown(int seconds);

			bool cooledDownDays(int days);
		}

		public interface IInterpreterFieldMixinField
		{
			//  Used in boulder events. Allows an event to be pushed. To be used in
			//  a script event command.
			void pbPushThisEvent();

			void pbPushThisBoulder();

			void pbHeadbutt();

			void pbTrainerIntro(TrainerTypes symbol);

			void pbTrainerEnd();

			object pbParams { get; }

			void pbGetPokemon(Pokemons id);

			void pbSetEventTime(params int[] arg);

			object getVariable(params int[] arg);

			void setVariable(params int[] arg);

			bool tsOff(string c);

			bool tsOn(string c);

			//alias isTempSwitchOn? tsOn?;
			//alias isTempSwitchOff? tsOff?;

			void setTempSwitchOn(string c);

			void setTempSwitchOff(string c);

			// Must use this approach to share the methods because the methods already
			// defined in a class override those defined in an included module
			/*CustomEventCommands =<< _END_;

			bool command_352();

			bool command_125();

			bool command_132();

			bool command_133();

			bool command_353();

			bool command_314();

			_END_;*/
		}
	}
}
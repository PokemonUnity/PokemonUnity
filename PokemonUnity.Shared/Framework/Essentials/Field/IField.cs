using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface
{
	namespace EventArg
	{
		#region Encounter Modifier EventArgs
		public interface IEncounterModifierEventArgs : IEventArgs, IEncounterPokemon
		{
			//readonly int EventId = typeof(EncounterModifierEventArgs).GetHashCode();

			//int Id { get; }
			//ToDo: Either it's the encounter logic or the pokemon itself...
			//[species,min/max]
			//IEncounterPokemon Encounter { get; set; }
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
			//List<Action<IEncounterPokemon>> procs { get; }
			//List<Action> procsEnd { get; }

			//event EventHandler<IEncounterModifierEventArgs> OnEncounter;
			event Action<object, IEncounterModifierEventArgs> OnEncounter;
			event EventHandler OnEncounterEnd;

			//void register(Action<IEncounterPokemon> p);

			//void registerEncounterEnd(Action<IEncounterPokemon> p);
			//void registerEncounterEnd(Action p);

			//IEncounterPokemon trigger(IEncounterPokemon encounter);
			void triggerEncounter(IEncounterPokemon encounter);

			void triggerEncounterEnd();
		}

		/// <summary>
		/// Extension of base game class... <seealso cref="IGame"/>
		/// </summary>
		public interface IGameField
		{
			IPokeBattle_Scene NewBattleScene();

			IEnumerator SceneStandby(Action block = null);

			void BattleAnimation(IAudioBGM bgm = null, int trainerid = -1, string trainername = "", Action block = null);

			/// <summary>
			/// Override and use this method if you want to add a custom battle intro animation
			/// e.g. variants of the Vs. animation.
			/// </summary>
			/// <remarks>
			/// Note that <see cref="IGameTemp.background_bitmap"/> contains an image of the current game
			/// screen.
			/// When the custom animation has finished, the screen should have faded to black
			/// somehow.
			/// </remarks>
			/// <param name="viewport"></param>
			/// <param name="trainerid"></param>
			/// <param name="trainername"></param>
			/// <returns></returns>
			bool BattleAnimationOverride(IViewport viewport, int trainerid = -1, string trainername = "");

			void PrepareBattle(IBattle battle);

			Environments GetEnvironment();

			IPokemon GenerateWildPokemon(Pokemons species, int level, bool isroamer = false);

			//bool ControlledWildBattle(Pokemons species, int level, Moves[] moves = null, int? ability = null,
			//			PokemonUnity.Monster.Natures? nature = null, bool? gender = null, Items? item = null, bool? shiny = null,
			//			int outcomeVar = 1, bool canRun = true, bool canLose = false);

			//PokemonUnity.Combat.BattleResults WildBattleCore(IPokemon pkmn, int? variable = null, bool canescape = true, bool canlose = false);

			bool WildBattle(Pokemons species, int level, int? variable = null, bool canescape = true, bool canlose = false);
			//PokemonUnity.Combat.BattleResults WildBattle(Pokemons species, int level, int? variable = null, bool canescape = true, bool canlose = false);

			bool DoubleWildBattle(Pokemons species1, int level1, Pokemons species2, int level2, int? variable = null, bool canescape = true, bool canlose = false);
			//PokemonUnity.Combat.BattleResults DoubleWildBattle(Pokemons species1, int level1, Pokemons species2, int level2, int? variable = null, bool canescape = true, bool canlose = false);

			void CheckAllFainted();

			void EvolutionCheck(int[] currentlevels);

			Items[] DynamicItemList(params Items[] args);

			/// <summary>
			/// Runs the Pickup event after a battle if a Pokemon has the ability Pickup.
			/// </summary>
			/// <param name="pokemon"></param>
			void Pickup(IPokemon pokemon);

			bool Encounter(EncounterOptions enctype);
			//bool Encounter(Method enctype);

			event EventHandler OnStartBattle;
			//Events.onStartBattle+=delegate(object sender, EventArgs e) {
			//  Game.GameData.PokemonTemp.evolutionLevels=[];
			//  for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
			//	Game.GameData.PokemonTemp.evolutionLevels[i]=Game.GameData.Trainer.party[i].level;
			//  }
			//}

			//event EventHandler OnEndBattle;
			event Action<object, IOnEndBattleEventArgs> OnEndBattle;
			//Events.onEndBattle+=delegate(object sender, EventArgs e) {
			//  decision=e[0];
			//  canlose=e[1];
			//  if (USENEWBATTLEMECHANICS || (decision!=2 && decision!=5)) {		// not a loss or a draw
			//	if (Game.GameData.PokemonTemp.evolutionLevels) {
			//	  EvolutionCheck(Game.GameData.PokemonTemp.evolutionLevels);
			//	  Game.GameData.PokemonTemp.evolutionLevels=null;
			//	}
			//  }
			//  if (decision==1) {
			//	foreach (var pkmn in Game.GameData.Trainer.party) {
			//	  Kernel.Pickup(pkmn);
			//	  if (pkmn.ability == Abilities.HONEYGATHER && !pkmn.isEgg? && !pkmn.hasItem?) {
			//		if (hasConst(Items,:HONEY)) {
			//		  chance = 5 + ((pkmn.level-1)/10).floor*5;
			//		  if (Core.Rand.Next(100)<chance) pkmn.setItem(Items.HONEY);
			//		}
			//	  }
			//	}
			//  }
			//  if ((decision==2 || decision==5) && !canlose) {
			//	Game.GameData.GameSystem.bgm_unpause;
			//	Game.GameData.GameSystem.bgs_unpause;
			//	Kernel.StartOver;
			//  }
			//}

			void OnSpritesetCreate(ISpritesetMap spriteset, IViewport viewport);

			#region Field movement
			bool Ledge(float xOffset, float yOffset);

			void SlideOnIce(IGamePlayer _event = null);

			void BattleOnStepTaken();

			void OnStepTaken(bool eventTriggered);

			// This method causes a lot of lag when the game is encrypted
			//ICharset GetPlayerCharset(meta, charset, trainer= null);

			void UpdateVehicle();

			/// <summary>
			/// </summary>
			/// <param name="destination">Map location id</param>
			void CancelVehicles(int? destination = null);

			bool CanUseBike(int mapid);

			void MountBike();

			void DismountBike();

			void SetPokemonCenter();
			#endregion

			#region Fishing
			void FishingBegin();

			void FishingEnd();

			bool Fishing(bool hasencounter, int rodtype = 1);

			bool WaitForInput(IWindow msgwindow, string message, int frames);

			bool WaitMessage(IWindow msgwindow, int time);
			#endregion

			#region Moving between maps
			void StartOver(bool gameover = false);

			void CaveEntranceEx(bool exiting);

			void CaveEntrance();

			void CaveExit();

			void SetEscapePoint();

			void EraseEscapePoint();
			#endregion

			#region Partner trainer
			void RegisterPartner(TrainerTypes trainerid, string trainername, int partyid = 0);

			void DeregisterPartner();
			#endregion

			#region Constant checks
			/// <summary>
			/// Returns whether the Poké Center should explain Pokérus to the player, if a
			/// healed Pokémon has it.
			/// </summary>
			/// <returns></returns>
			bool Pokerus();
			#endregion

			//bool BatteryLow();

			#region Audio playing
			void CueBGM(string bgm, float seconds, int? volume = null, float? pitch = null);
			void CueBGM(IAudioBGM bgm, float seconds, int? volume = null, float? pitch = null);

			void AutoplayOnTransition();

			void AutoplayOnSave();
			#endregion

			#region Voice recorder
			/// <summary>
			/// </summary>
			/// BGM audio file?
			IWaveData Record(string text, float maxtime = 30.0f);

			bool RxdataExists(string file);
			#endregion

			#region Gaining items
			bool ItemBall(Items item, int quantity = 1);

			bool ReceiveItem(Items item, int quantity = 1);

			void UseKeyItem();
			#endregion

			#region Bridges
			void BridgeOn(float height = 2);

			void BridgeOff();
			#endregion

			#region Event locations, terrain tags
			bool EventFacesPlayer(IGameCharacter _event, IGamePlayer player, float distance);

			bool EventCanReachPlayer(IGameCharacter _event, IGamePlayer player, float distance);

			ITilePosition FacingTileRegular(float? direction = null, IGameCharacter _event = null);

			ITilePosition FacingTile(float? direction = null, IGameCharacter _event = null);

			bool FacingEachOther(IGameCharacter event1, IGameCharacter event2);

			Terrains GetTerrainTag(IGameCharacter _event = null, bool countBridge = false);

			Terrains? FacingTerrainTag(IGameCharacter _event = null, float? dir = null);
			#endregion

			#region Event movement
			void TurnTowardEvent(IGameCharacter _event, IGameCharacter otherEvent);

			void MoveTowardPlayer(IGameCharacter _event);

			bool JumpToward(int dist = 1, bool playSound = false, bool cancelSurf = false);

			void Wait(int numframes);
			#endregion

			//IMoveRoute MoveRoute(IEntity _event, string[] commands, bool waitComplete= false);

			#region Screen effects
			void ToneChangeAll(ITone tone, float duration);

			void Shake(int power, int speed, int frames);

			void Flash(IColor color, int frames);

			void ScrollMap(int direction, int distance, float speed);
			#endregion
		}

		/// <summary>
		/// Extension of <see cref="Interface.Field.ITempMetadata"/>
		/// </summary>
		public interface ITempMetadataField
		{
			#region
			Method? encounterType	{ get; set; }
			int[] evolutionLevels				{ get; set; }
			#endregion

			#region
			bool batterywarning { get; }
			IAudioBGM cueBGM { get; set; }
			float? cueFrames { get; set; }
			#endregion
		}

		#region Scene_Map and Spriteset_Map
		public interface ISceneMapField
		{
			void createSingleSpriteset(int map);
		}

		//public interface ISpritesetMap
		//{
		//	void getAnimations();
		//
		//	void restoreAnimations(anims);
		//}
		#endregion

		/*public static partial class IMoveRoute
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
		/// <summary>
		/// Extension of <see cref="IGame"/>
		/// </summary>
		public interface IGameEventField
		{
			bool cooledDown(int seconds);

			bool cooledDownDays(int days);
		}

		/// <summary>
		/// Extension of <see cref="IInterpreter"/>
		/// </summary>
		public interface IInterpreterFieldMixinField : IInterpreterFieldMixin
		{
			/// <summary>
			/// Used in boulder events. Allows an event to be pushed. To be used in
			/// a script event command.
			/// </summary>
			void PushThisEvent();

			void PushThisBoulder();

			void Headbutt();

			void TrainerIntro(TrainerTypes symbol);

			void TrainerEnd();

			object Params { get; }

			void GetPokemon(Pokemons id);

			void SetEventTime(params int[] arg);

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
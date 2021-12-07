using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;

namespace PokemonEssentials.Interface.PokeBattle
{
	/// <summary>
	/// Recording of a Pokemon Battle
	/// </summary>
	/// <typeparam name="TBattle">any <see cref="IBattle"/> entity</typeparam>
	//Should be both Recorded Data and the Battle logic itself...
	public interface IRecordedBattleModule<out TBattle> //: IBattleRecordData
		where TBattle : IBattle//, IBattleRecordData
	{
		IList<int> randomnums { get; }
		//IList<int[][]> rounds { get; }
		IList<KeyValuePair<MenuCommands,int>[]> rounds { get; }
		//int battletype { get; }
		//object properties { get; }
		//int roundindex { get; }
		//IList<int> switches { get; }

		#region Methods
		IBattle initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		int pbGetBattleType();
		ITrainer[] pbGetTrainerInfo(ITrainer[] trainer);
		BattleResults pbStartBattle(bool canlose = false);
		string pbDumpRecord();
		int pbSwitchInBetween(int i1, bool i2, bool i3);
		bool pbRegisterMove(int i1, int i2, bool showMessages = true);
		int pbRun(int i1, bool duringBattle = false);
		bool pbRegisterTarget(int i1, int i2);
		void pbAutoChooseMove(int i1, bool showMessages = true);
		bool pbRegisterSwitch(int i1, int i2);
		bool pbRegisterItem(int i1, Items i2);
		void pbCommandPhase();
		void pbStorePokemon(IPokemon pkmn);
		int pbRandom(int num);
		#endregion
	}

	public interface IBattlePlayerHelper{
		ITrainer[] pbGetOpponent(IBattle battle);

		IAudioBGM pbGetBattleBGM(IBattle battle);

		ITrainer[] pbCreateTrainerInfo(ITrainer[] trainer);
	}

	/// <summary>
	/// Playback Recorded Pokemon Battle
	/// </summary>
	/// <typeparam name="TBattle"></typeparam>
	public interface IBattlePlayerModule <out TBattle>
		where TBattle : IBattle
	{
		//int randomindex { get; }
		//int switchindex { get; }

		//IBattle should be a recorded battle data...
		TBattle initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, IBattle battle); 
		BattleResults pbStartBattle(bool canlose = false);
		int pbSwitchInBetween(int i1, int i2, bool i3);
		int pbRandom(int num);
		void pbDisplayPaused(string str);
		void pbCommandPhaseCore();
	}

	public interface IRecordedBattle : IBattle, IRecordedBattleModule<IBattle>, IBattleRecordData
	{
		//int pbGetBattleType();
	}
	public interface IRecordedBattlePalace : IBattlePalace, IRecordedBattleModule<IBattlePalace>, IBattleRecordData
	{
		//int pbGetBattleType();
	}
	public interface IRecordedBattleArena : IBattleArena, IRecordedBattleModule<IBattleArena>, IBattleRecordData
	{
		//int pbGetBattleType();
	}

	public interface IBattlePlayer : IBattle, IBattlePlayerModule<IBattle> { }

	public interface IBattlePalacePlayer : IBattlePalace, IBattlePlayerModule<IBattlePalace> { }

	public interface IBattleArenaPlayer : IBattleArena, IBattlePlayerModule<IBattleArena> { }
	/// <summary>
	/// Represents a json object that can be saved/loaded to re-play a recorded pokemon battle
	/// </summary>
	//ToDo: maybe add <out IBattle> to interface?
	public interface IBattleRecordData 
	{
		int pbGetBattleType { get; }
		//ToDo: this should be replaced with json object class
		//IDictionary<string, IBattleMetaData> properties { get; }
		IBattleMetaData properties { get; }
		//IList<int[][]> rounds { get; }
		IList<KeyValuePair<MenuCommands, int>[]> rounds { get; }
		IList<int> randomnumbers { get; }
		IList<int> switches { get; }
	}
	/// <summary>
	/// </summary>
	public interface IBattleMetaData 
	{
		bool internalbattle { get; set; }
		//SeriTrainer player { get; set; }
		//SeriTrainer opponent { get; set; }
		SeriPokemon[] party1 { get; set; }
		SeriPokemon[] party2 { get; set; }
		string endspeech { get; set; }
		string endspeech2 { get; set; }
		string endspeechwin { get; set; }
		string endspeechwin2 { get; set; }
		string doublebattle { get; set; }
		int weather { get; set; }
		int weatherduration { get; set; }
		bool cantescape { get; set; }
		bool shiftStyle { get; set; }
		int battlescene { get; set; }
		Items[] items { get; set; }
		int environment { get; set; }
		IDictionary<string,bool> rules { get; set; }
	}
}
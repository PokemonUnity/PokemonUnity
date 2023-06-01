using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.Combat
{
	//public class PokeBattle_RecordedBattleModule<IBattle> : Battle, IRecordedBattleModule<Battle>, IBattle, IBattleRecordData
	public class PokeBattle_RecordedBattleModule<TBattle> : Battle, IRecordedBattleModule<TBattle>, IBattle, IBattleRecordData
		where TBattle : IBattle, IBattleRecordData
	{
		#region Variables
		protected TBattle Instance;

		public IList<int> randomnumbers { get; protected set; }
		//public IList<int[][]> rounds { get; protected set; }
		public IList<KeyValuePair<MenuCommands,int>?[]> rounds { get; protected set; }
		public IList<int> switches { get; protected set; }
		public int roundindex { get; protected set; }
		public IBattleMetaData properties { get; protected set; }
		//public int GetBattleType { get { return battletype; } }
		public int battletype { get; protected set; }
		#endregion

		public PokeBattle_RecordedBattleModule(IPokeBattle_Scene scene,PokemonEssentials.Interface.PokeBattle.IPokemon[] p1,PokemonEssentials.Interface.PokeBattle.IPokemon[] p2,PokemonEssentials.Interface.PokeBattle.ITrainer[] player,PokemonEssentials.Interface.PokeBattle.ITrainer[] opponent)
			: base (scene, p1, p2, player, opponent)
		{ (this as IRecordedBattleModule<Battle>).initialize(scene, p1, p2, player, opponent); }
		public IBattle initialize(IPokeBattle_Scene scene,PokemonEssentials.Interface.PokeBattle.IPokemon[] p1,PokemonEssentials.Interface.PokeBattle.IPokemon[] p2,PokemonEssentials.Interface.PokeBattle.ITrainer[] player,PokemonEssentials.Interface.PokeBattle.ITrainer[] opponent)
		{
			battletype = GetBattleType();
			//@randomnumbers=new List<int>();
			randomnumbers=new List<int>();
			//@rounds=new List<int[][]>();
			@rounds=new List<KeyValuePair<MenuCommands,int>?[]>();
			@switches=new List<int>();
			@roundindex=-1;
			//@properties=new object();
			base.initialize(scene, p1, p2, player, opponent);
			return this;
		}

		#region Methods
		public virtual int GetBattleType() {
			return 0; // Battle Tower
		}

		public ITrainer[] GetTrainerInfo(ITrainer[] trainer) {
			if (trainer == null) return null;
			if (trainer.Length > 1) {
				return trainer;
				//return new Trainer[] {
				//   //new Trainer(trainer[0].trainertype,trainer[0].Name.Clone(),trainer[0].id,trainer[0].badges.Clone()),
				//   new Trainer(trainer[0].ID,trainer[0].Name.Clone(),trainer[0].TrainerID,trainer[0].Badges.Clong()),
				//   new Trainer(trainer[1].ID,trainer[1].Name.Clone(),trainer[1].TrainerID,trainer[0].Badges.Clong())
				//};
			}
			else {
				return trainer;
				//return new Trainer[] {
				//   //new Trainer(trainer.trainertype,trainer.Name,trainer.id,trainer.badges)
				//   new Trainer(trainer.trainertype,trainer.Name.Clone(),trainer.id,trainer.Badges.Clone)
				//};
			}
		}

		public override BattleResults StartBattle(bool canlose=false) {
			/*@properties=new IBattleMetaData();
			@properties["internalbattle"]=Core.INTERNAL;//@internalbattle;
			@properties["player"]=GetTrainerInfo(@player);
			@properties["opponent"]=GetTrainerInfo(@opponent);
			@properties["party1"]=@party1.Serialize();//Marshal.dump(@party1);
			@properties["party2"]=@party2.Serialize();//Marshal.dump(@party2);
			@properties["endspeech"]=@endspeech != null ? @endspeech : "";
			@properties["endspeech2"]=@endspeech2 != null ? @endspeech2 : "";
			@properties["endspeechwin"]=@endspeechwin != null ? @endspeechwin : "";
			@properties["endspeechwin2"]=@endspeechwin2 != null ? @endspeechwin2 : "";
			@properties["doublebattle"]=@doublebattle;
			@properties["weather"]=@weather;
			@properties["weatherduration"]=@weatherduration;
			@properties["cantescape"]=@cantescape;
			@properties["shiftStyle"]=@shiftStyle;
			@properties["battlescene"]=@battlescene;
			@properties["items"]=Marshal.dump(@items);
			@properties["environment"]=@environment;
			@properties["rules"]=Marshal.dump(@rules);*/
			return base.StartBattle(canlose);
		}

		public string DumpRecord() {
			//return Marshal.dump([GetBattleType,@properties,@rounds,@randomnumbers,@switches]);
			//return new { GetBattleType(), @properties, @rounds, randomnumbers, @switches }.ToString();
			return new { @battletype, @properties, @rounds, randomnumbers, @switches }.ToString();
		}

		public override int SwitchInBetween(int i1,bool i2,bool i3) {
			int ret=base.SwitchInBetween(i1,i2,i3);
			@switches.Add(ret);
			return ret;
		}

		public override bool RegisterMove(int i1,int i2, bool showMessages=true) {
			if (base.RegisterMove(i1,i2,showMessages)) {
				//@rounds[@roundindex][i1]=new int[] { MenuCommands.FIGHT, i2 };
				@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.FIGHT, i2);
				return true;
			}
			return false;
		}

		public override int Run(int i1,bool duringBattle=false) {
			int ret=base.Run(i1,duringBattle);
			//@rounds[@roundindex][i1]=new int[] { MenuCommands.RUN, (int)@decision };
			@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.RUN, (int)@decision);
			return ret;
		}

		public override bool RegisterTarget(int i1,int i2) {
			bool ret=base.RegisterTarget(i1,i2);
			//@rounds[@roundindex][i1][2]=i2; //ToDo: Select target for Move choosen
			@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.FIGHT, i2); //@rounds[@roundindex][i1].Value=i2;
			return ret;
		}

		public override void AutoChooseMove(int i1,bool showMessages=true) {
			//bool ret= //no return value...
				base.AutoChooseMove(i1,showMessages);
			//@rounds[@roundindex][i1]=new int[] { MenuCommands.FIGHT, -1 };
			@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.FIGHT, -1);
			return; //ret;
		}

		public override bool RegisterSwitch(int i1,int i2) {
			if (base.RegisterSwitch(i1,i2)) {
				//@rounds[@roundindex][i1]=new int[] { MenuCommands.POKEMON, i2 };
				@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.POKEMON, i2);
				return true;
			}
			return false;
		}

		public bool RegisterItem(int i1,Items i2) {
			if (base.RegisterItem(i1,i2)) {
				//@rounds[@roundindex][i1]=new int[] { MenuCommands.BAG, (int)i2 }; //MenuCommands.Item == Bag
				@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.BAG, (int)i2);
				return true;
			}
			return false;
		}

		public override void CommandPhase() {
			@roundindex+=1;
			//@rounds[@roundindex]=new int[4][]; //[[],[],[],[]];
			//@rounds.Add(new int[battlers.Length][]); //[[],[],[],[]];
			@rounds.Add(new KeyValuePair<MenuCommands, int>?[battlers.Length]);
			base.CommandPhase();
		}

		public override void StorePokemon(IPokemon pkmn) {
		}

		public override int Random(int num) {
			int ret=base.Random(num);
			//@randomnumbers.Add(ret);
			randomnumbers.Add(ret);
			return ret;
		}
		#endregion
	}

	public static class BattlePlayerHelper {
		public static ITrainer[] GetOpponent(IBattle battle) {
			//return this.CreateTrainerInfo(battle[1]["opponent"]);
			return CreateTrainerInfo(battle.opponent);
		}

		public static IAudioBGM GetBattleBGM(IBattle battle) {
			return (Game.GameData as IGameUtility).GetTrainerBattleBGM(BattlePlayerHelper.GetOpponent(battle));
		}

		public static ITrainer[] CreateTrainerInfo(TrainerData[] trainer) {
			if (trainer == null) return null;
			if (trainer.Length>1) {
				ITrainer[] ret=new Trainer[2];
				ret[0]=new Trainer(trainer[0].Name, trainer[0].ID); //trainer[0][1],trainer[0][0]
				ret[0].id=trainer[0].TrainerID; //trainer[0][2];
				//ret[0].badges=trainer[0].badges; //trainer[0][3];
				//ToDo: for-loop, foreach badge in array, assign same value to second variable
				ret[1]=new Trainer(trainer[1].Name, trainer[1].ID); //trainer[1][1],trainer[1][0]
				ret[1].id=trainer[1].TrainerID; //trainer[1][2];
				//ret[1].badges=trainer[1].badges; //trainer[1][3];
				return ret;
			}
			else {
				ITrainer[] ret=new Trainer[] { new Trainer(trainer[0].Name, trainer[0].ID) };//(trainer[0][1], trainer[0][0])
				ret[0].id=trainer[0].TrainerID; //trainer[0][2];
				//ret[0].badges=trainer[0].badges; //trainer[0][3];
				return ret;
			}
		}

		public static ITrainer[] CreateTrainerInfo(ITrainer[] trainer) {
			if (trainer == null) return null;
			if (trainer.Length>1) {
				ITrainer[] ret=new Trainer[2];
				ret[0]=new Trainer(trainer[0].fullname, trainer[0].trainertype); //trainer[0][1],trainer[0][0]
				ret[0].id=trainer[0].id; //trainer[0][2];
				ret[0].badges=trainer[0].badges; //trainer[0][3];
				//ToDo: for-loop, foreach badge in array, assign same value to second variable
				ret[1]=new Trainer(trainer[1].fullname, trainer[1].trainertype); //trainer[1][1],trainer[1][0]
				ret[1].id=trainer[1].id; //trainer[1][2];
				ret[1].badges=trainer[1].badges; //trainer[1][3];
				return ret;
			}
			else {
				ITrainer[] ret=new Trainer[] { new Trainer(trainer[0].fullname, trainer[0].trainertype) };//(trainer[0][1], trainer[0][0])
				ret[0].id=trainer[0].id; //trainer[0][2];
				ret[0].badges=trainer[0].badges; //trainer[0][3];
				return ret;
			}
		}
	}

	/// <summary>
	/// Playback?
	/// </summary>
	/// <typeparam name="IBattle"></typeparam>
	//public class PokeBattle_BattlePlayerModule : Battle, IBattlePlayerModule<Battle>, IBattle
	//public class PokeBattle_BattlePlayerModule<T> : PokeBattle_RecordedBattleModule<IBattle>, IBattlePlayerModule<IBattle> where T : IRecordedBattleModule<IBattle>
	//public class PokeBattle_BattlePlayerModule<IPokeBattle_RecordedBattleModule> : PokeBattle_RecordedBattleModule<IBattle>, IBattlePlayerModule<IPokeBattle_RecordedBattleModule>
	//		//where TBattle : PokeBattle_RecordedBattleModule<TBattle>, IRecordedBattleModule<TBattle>, IBattle, IBattleRecordData
	public class PokeBattle_BattlePlayerModule<T> : PokeBattle_RecordedBattleModule<T>, IBattlePlayerModule<T> where T : IRecordedBattleModule<T>, IBattle, IBattleRecordData
	{
		#region Variables
		public int randomindex { get; protected set; }
		public int switchindex { get; protected set; }

		//Inherited from IRecordedBattleModule
		/*public IList<int> randomnumbers { get; protected set; }
		//public IList<int[][]> rounds { get; protected set; }
		public IList<KeyValuePair<MenuCommands,int>?[]> rounds { get; protected set; }
		public int battletype { get; protected set; }
		public IBattleMetaData properties { get; protected set; }
		public int roundindex { get; protected set; }
		public IList<int> switches { get; protected set; }*/
		#endregion

		public PokeBattle_BattlePlayerModule(IPokeBattle_Scene scene, IBattle battle) : base (scene, battle.party1, battle.party2, battle.player, battle.opponent)
		//{ (this as IBattlePlayerModule<IBattle>).initialize(scene, (IRecordedBattleModule<Battle>)battle); }
		{ initialize(scene, (IRecordedBattleModule<T>)battle); }
		//public PokeBattle_BattlePlayerModule(IPokeBattle_Scene scene, IRecordedBattleModule<Battle> battle) : base (scene, battle.party1, battle.party2, battle.player, battle.opponent)
		//{ (this as IBattlePlayerModule<IBattle>).initialize(scene, battle); }
		public IBattlePlayerModule<T> initialize(IPokeBattle_Scene scene, IRecordedBattleModule<T> battle)
		{
			@battletype=battle.GetBattleType(); //battle.battletype;
			@properties=battle.properties;
			@rounds=battle.rounds;
			randomnumbers=battle.randomnumbers;
			@switches=battle.switches;
			@roundindex=-1;
			@randomindex=0;
			@switchindex=0;
			//base.initialize(scene,
			//	@properties.party1,	//(IPokemon[])@properties["party1"],		//Marshal.restore(new StringInput(@properties["party1"])),
			//	@properties.party2,	//(IPokemon[])@properties["party2"],		//Marshal.restore(new StringInput(@properties["party2"])),
			//	BattlePlayerHelper.CreateTrainerInfo(@properties.player),		//(ITrainer[])@properties["player"]),
			//	BattlePlayerHelper.CreateTrainerInfo(@properties.opponent)	//(ITrainer[])@properties["opponent"])
			//);
			return this;
		}

		//IBattlePlayerModule<Battle> IBattlePlayerModule<Battle>.initialize(IPokeBattle_Scene scene, IBattle battle)
		IBattlePlayerModule<T> IBattlePlayerModule<T>.initialize(IPokeBattle_Scene scene, IBattle battle)
		{
			// Check if battle can be cast to IRecordedBattleModule<Battle>
			//if (battle is IRecordedBattleModule<Battle> recordedBattle)
			//{
			//	return initialize(scene, recordedBattle);
			//}
			//throw new ArgumentException("Battle must be of type IRecordedBattleModule<Battle>");
			return initialize(scene, (IRecordedBattleModule<T>)battle);
		}

		#region Methods
		public override BattleResults StartBattle(bool canlose=false) {
			@internalbattle		=Core.INTERNAL;//@properties.internalbattle;		//Core.INTERNAL;//@properties["internalbattle"];
			@endspeech			=@properties.endspeech.ToString();					//@properties["endspeech"].ToString();
			@endspeech2			=@properties.endspeech2.ToString();					//@properties["endspeech2"].ToString();
			@endspeechwin		=@properties.endspeechwin.ToString();				//@properties["endspeechwin"].ToString();
			@endspeechwin2		=@properties.endspeechwin2.ToString();				//@properties["endspeechwin2"].ToString();
			@doublebattle		=(bool)@properties.doublebattle;					//(bool)@properties["doublebattle"];
			@weather			=(Weather)@properties.weather;						//(Weather)@properties["weather"];
			@weatherduration	=(int)@properties.weatherduration;					//(int)@properties["weatherduration"];
			@cantescape			=(bool)@properties.cantescape;						//(bool)@properties["cantescape"];
			@shiftStyle			=(bool)@properties.shiftStyle;						//(bool)@properties["shiftStyle"];
			@battlescene		=(bool)@properties.battlescene;						//(bool)@properties["battlescene"];
			@items				=(Items[][])@properties.items;						//(Items[][])@properties["items"];					//Marshal.restore(new StringInput(@properties["items"]));
			@rules				=(IDictionary<string,bool>)@properties.rules;		//(IDictionary<string,bool>)@properties["rules"];	//Marshal.restore(new StringInput(@properties["rules"]));
			@environment		=(Overworld.Environments)@properties.environment;	//(Overworld.Environments)@properties["environment"];
			return base.StartBattle(canlose);
		}

		public int SwitchInBetween(int i1,int i2,bool i3) {
			int ret=@switches[@switchindex];
			@switchindex+=1;
			return ret;
		}

		public override int Random(int num) {
			int ret=randomnumbers[@randomindex];
			@randomindex+=1;
			return ret;
		}

		public override void DisplayPaused(string str) {
			Display(str);
		}

		public void CommandPhaseCore() {
			@roundindex+=1;
			for (int i = 0; i < battlers.Length; i++) {
				//if (@rounds[@roundindex][i].Length==0) continue;
				if (@rounds[@roundindex][i] == null) continue;
				//@choices[i][0]=0;
				//@choices[i][1]=0;
				//@choices[i][2]=null;
				//@choices[i][3]=-1;
				@choices[i]=new Choice();
				switch (@rounds[@roundindex][i].Value.Key) { //@rounds[@roundindex][i][0]
					case MenuCommands.FIGHT:
						if (@rounds[@roundindex][i].Value.Value==-1) { //@rounds[@roundindex][i][1]==-1
							AutoChooseMove(i,false);
						}
						else {
							///RegisterMove(i,@rounds[@roundindex][i][1]);
							RegisterMove(i,@rounds[@roundindex][i].Value.Value);
						}
						//if (@rounds[@roundindex][i][2]!=null)
							//RegisterTarget(i,@rounds[@roundindex][i][2]);
							RegisterTarget(i,@rounds[@roundindex][i].Value.Value); //ToDo: Select target for Move chosen
						break;
					case MenuCommands.POKEMON:
						//RegisterSwitch(i,@rounds[@roundindex][i][1]);
						RegisterSwitch(i,@rounds[@roundindex][i].Value.Value);
						break;
					case MenuCommands.BAG:
						//RegisterItem(i,(Items)@rounds[@roundindex][i][1]);
						RegisterItem(i,(Items)@rounds[@roundindex][i].Value.Value);
						break;
					case MenuCommands.RUN:
						//@decision=(BattleResults)@rounds[@roundindex][i][1];
						@decision=(BattleResults)@rounds[@roundindex][i].Value.Value;
						break;
				}
			}
		}
		#endregion
	}

	//public class PokeBattle_RecordedBattle : PokeBattle_RecordedBattleModule<Battle>, IRecordedBattle {
	/*public class PokeBattle_RecordedBattle : PokeBattle_RecordedBattleModule<Battle>, IRecordedBattle {
		//include PokeBattle_RecordedBattleModule;
		public PokeBattle_RecordedBattle(IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, ITrainer[] player, ITrainer[] opponent) : base(scene, p1, p2, player, opponent)
		{
		}
		public override int GetBattleType() {
			return 0;
		}
	}

	public class PokeBattle_RecordedBattlePalace : PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace>, IRecordedBattlePalace {
		//include PokeBattle_RecordedBattleModule;
		public int[] BattlePalaceUsualTable { get { return Instance.BattlePalaceUsualTable; } }

		public int[] BattlePalacePinchTable { get { return Instance.BattlePalaceUsualTable; } }

		public PokeBattle_RecordedBattlePalace(IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, ITrainer[] player, ITrainer[] opponent) : base(scene, p1, p2, player, opponent)
		{
			//Instance = (TBattle)this;
		}

		PokemonEssentials.Interface.PokeBattle.IBattlePalace PokemonEssentials.Interface.PokeBattle.IBattlePalace.initialize(IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent)
		{
			throw new NotImplementedException();
		}

		public override int GetBattleType() {
			return 1;
		}

		public int MoveCategory(IBattleMove move)
		{
			throw new NotImplementedException();
		}

		public bool CanChooseMovePartial(int idxPokemon, int idxMove)
		{
			throw new NotImplementedException();
		}

		public void PinchChange(int idxPokemon)
		{
			throw new NotImplementedException();
		}
	}

	public class PokeBattle_RecordedBattleArena : PokeBattle_RecordedBattleModule<PokeBattle_BattleArena>, IRecordedBattleArena {
		//include PokeBattle_RecordedBattleModule;
		public PokeBattle_RecordedBattleArena(IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, ITrainer[] player, ITrainer[] opponent) : base(scene, p1, p2, player, opponent)
		{
		}
		public override int GetBattleType() {
			return 2;
		}

		PokemonEssentials.Interface.PokeBattle.IBattleArena PokemonEssentials.Interface.PokeBattle.IBattleArena.initialize(IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent)
		{
			throw new NotImplementedException();
		}

		public int MindScore(IBattleMove move)
		{
			throw new NotImplementedException();
		}
	}

	public class PokeBattle_BattlePlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<Battle>>, IBattlePlayerModule<Battle>, IBattlePlayer {
		//include PokeBattle_BattlePlayerModule;
		public PokeBattle_BattlePlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<Battle> battle) : base(scene, battle)
		{
		}
		public IBattle initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}

		IBattlePlayerModule<Battle> IBattlePlayerModule<Battle>.initialize(IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}

		IBattlePlayerModule<IBattle> IBattlePlayerModule<IBattle>.initialize(IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}
	}

	public class PokeBattle_BattlePalacePlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace>>, IBattlePlayerModule<PokeBattle_BattlePalace>, IBattlePalacePlayer {
		//include PokeBattle_BattlePlayerModule;

		public int[] BattlePalaceUsualTable => throw new NotImplementedException();

		public int[] BattlePalacePinchTable => throw new NotImplementedException();

		public PokeBattle_BattlePalacePlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace> battle) : base(scene, battle)
		{
		}

		public IBattle initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}

		IBattlePlayerModule<PokeBattle_BattlePalace> IBattlePlayerModule<PokeBattle_BattlePalace>.initialize(IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}

		PokemonEssentials.Interface.PokeBattle.IBattlePalace PokemonEssentials.Interface.PokeBattle.IBattlePalace.initialize(IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent)
		{
			throw new NotImplementedException();
		}

		IBattlePlayerModule<PokemonEssentials.Interface.PokeBattle.IBattlePalace> IBattlePlayerModule<PokemonEssentials.Interface.PokeBattle.IBattlePalace>.initialize(IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}

		public bool CanChooseMovePartial(int idxPokemon, int idxMove)
		{
			throw new NotImplementedException();
		}

		public int MoveCategory(IBattleMove move)
		{
			throw new NotImplementedException();
		}

		public void PinchChange(int idxPokemon)
		{
			throw new NotImplementedException();
		}
	}

	public class PokeBattle_BattleArenaPlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<PokeBattle_BattleArena>>, IBattlePlayerModule<PokeBattle_BattleArena>, IBattleArenaPlayer {
		//include PokeBattle_BattlePlayerModule;
		public PokeBattle_BattleArenaPlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<PokeBattle_BattleArena> battle) : base(scene, battle)
		{
		}
		public IBattle initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, IBattle battle)
		{
			throw new NotImplementedException();
		}
	}*/
}
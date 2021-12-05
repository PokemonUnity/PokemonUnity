using System;
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
	public class PokeBattle_RecordedBattleModule<out TBattle> : Battle, IRecordedBattleModule<TBattle>, IBattle, IBattleRecordData
			where TBattle : IBattle, IBattleRecordData
	{
		#region Variables
		public IList<int> randomnums { get; protected set; }
		//public IList<int[][]> rounds { get; protected set; }
		public IList<KeyValuePair<MenuCommands,int>[]> rounds { get; protected set; }
		public IList<int> switches { get; protected set; }
		public int roundindex { get; protected set; }
		public IBattleMetaData properties { get; protected set; }
		public int battletype { get; protected set; }
		#endregion

		public PokeBattle_RecordedBattleModule(IPokeBattle_Scene scene,PokemonEssentials.Interface.PokeBattle.IPokemon[] p1,PokemonEssentials.Interface.PokeBattle.IPokemon[] p2,PokemonEssentials.Interface.PokeBattle.ITrainer[] player,PokemonEssentials.Interface.PokeBattle.ITrainer[] opponent) //: base (scene, p1, p2, player, opponent)
		{ (this as IRecordedBattleModule<TBattle>).initialize(scene, p1, p2, player, opponent); }
		public IBattle initialize(IPokeBattle_Scene scene,PokemonEssentials.Interface.PokeBattle.IPokemon[] p1,PokemonEssentials.Interface.PokeBattle.IPokemon[] p2,PokemonEssentials.Interface.PokeBattle.ITrainer[] player,PokemonEssentials.Interface.PokeBattle.ITrainer[] opponent)
		{
			//@randomnumbers=new List<int>();
			@randomnums=new List<int>();
			//@rounds=new List<int[][]>();
			@rounds=new List<KeyValuePair<MenuCommands,int>[]>();
			@switches=new List<int>();
			@roundindex=-1;
			//@properties=new object();
			base.initialize(scene, p1, p2, player, opponent);
		}

		#region Methods
		public virtual int pbGetBattleType() {
			return 0; // Battle Tower
		}

		public ITrainer[] pbGetTrainerInfo(ITrainer[] trainer) {
			if (trainer == null) return null;
			if (trainer.Length > 0) {
				return trainer;
				//return new Trainer[] {
				//   //new Trainer(trainer[0].trainertype,trainer[0].Name,trainer[0].id,trainer[0].badges),
				//   new Trainer(trainer[0].ID,trainer[0].Name,trainer[0].TrainerID,trainer[0].Badges),
				//   new Trainer(trainer[1].ID,trainer[1].Name,trainer[1].TrainerID,trainer[0].Badges)
				//};
			}
			else {
				return trainer;
				//return new Trainer[] {
				//   new Trainer(trainer.trainertype,trainer.Name,trainer.id,trainer.badges)
				//};
			}
		}

		public override BattleResults pbStartBattle(bool canlose=false) {
			/*@properties=new object();
			@properties["internalbattle"]=@internalbattle;
			@properties["player"]=pbGetTrainerInfo(@player);
			@properties["opponent"]=pbGetTrainerInfo(@opponent);
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
			return base.pbStartBattle(canlose);
		}

		public string pbDumpRecord() {
			//return Marshal.dump([pbGetBattleType,@properties,@rounds,@randomnumbers,@switches]);
			return new { pbGetBattleType(), @properties, @rounds, @randomnums, @switches }.ToString();
		}

		public override int pbSwitchInBetween(int i1,bool i2,bool i3) {
			int ret=base.pbSwitchInBetween(i1,i2,i3);
			@switches.Add(ret);
			return ret;
		}

		public override bool pbRegisterMove(int i1,int i2, bool showMessages=true) {
			if (base.pbRegisterMove(i1,i2,showMessages)) {
				//@rounds[@roundindex][i1]=new int[] { MenuCommands.FIGHT, i2 };
				@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.FIGHT, i2);
				return true;
			}
			return false;
		}

		public override int pbRun(int i1,bool duringBattle=false) {
			int ret=base.pbRun(i1,duringBattle);
			//@rounds[@roundindex][i1]=new int[] { MenuCommands.RUN, (int)@decision };
			@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.RUN, (int)@decision);
			return ret;
		}

		public override bool pbRegisterTarget(int i1,int i2) {
			bool ret=base.pbRegisterTarget(i1,i2);
			//@rounds[@roundindex][i1][2]=i2; //ToDo: Select target for Move choosen
			@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.FIGHT, i2); //@rounds[@roundindex][i1].Value=i2;
			return ret;
		}

		public override void pbAutoChooseMove(int i1,bool showMessages=true) {
			//bool ret= //no return value...
				base.pbAutoChooseMove(i1,showMessages);
			//@rounds[@roundindex][i1]=new int[] { MenuCommands.FIGHT, -1 };
			@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.FIGHT, -1);
			return; //ret;
		}

		public override bool pbRegisterSwitch(int i1,int i2) {
			if (base.pbRegisterSwitch(i1,i2)) {
				//@rounds[@roundindex][i1]=new int[] { MenuCommands.POKEMON, i2 };
				@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.POKEMON, i2);
				return true;
			}
			return false;
		}

		public bool pbRegisterItem(int i1,Items i2) {
			if (base.pbRegisterItem(i1,i2)) {
				//@rounds[@roundindex][i1]=new int[] { MenuCommands.BAG, (int)i2 }; //MenuCommands.Item == Bag
				@rounds[@roundindex][i1]=new KeyValuePair<MenuCommands, int>(MenuCommands.BAG, (int)i2);
				return true;
			}
			return false;
		}

		public override void pbCommandPhase() {
			@roundindex+=1;
			//@rounds[@roundindex]=new int[4][]; //[[],[],[],[]];
			//@rounds.Add(new int[battlers.Length][]); //[[],[],[],[]];
			@rounds.Add(new KeyValuePair<MenuCommands, int>[battlers.Length]); 
			base.pbCommandPhase();
		}

		public void pbStorePokemon(IPokemon pkmn) {
		}

		public override int pbRandom(int num) {
			int ret=base.pbRandom(num);
			//@randomnumbers.Add(ret);
			@randomnums.Add(ret);
			return ret;
		}
		#endregion
	}

	public static class BattlePlayerHelper {
		public static ITrainer[] pbGetOpponent(IBattle battle) {
			//return this.pbCreateTrainerInfo(battle[1]["opponent"]);
			return pbCreateTrainerInfo(battle.opponent);
		}

		public static IAudioBGM pbGetBattleBGM(IBattle battle) {
			return pbGetTrainerBattleBGM(this.pbGetOpponent(battle));
		}

		public static ITrainer[] pbCreateTrainerInfo(ITrainer[] trainer) {
			if (trainer == null) return null;
			if (trainer.Length>1) {
				ITrainer[] ret=new Combat.Trainer[2];
				ret[0]=new Combat.Trainer(trainer[0].fullname, trainer[0].id); //trainer[0][1],trainer[0][0]
				ret[0].id=trainer[0].id; //trainer[0][2];
				ret[0].badges=trainer[0].badges; //trainer[0][3];
				ret[1]=new Combat.Trainer(trainer[1].fullname, trainer[1].id); //trainer[1][1],trainer[1][0]
				ret[1].id=trainer[1].id; //trainer[1][2];
				ret[1].badges=trainer[1].badges; //trainer[1][3];
				return ret;
			}
			else {
				Combat.Trainer[] ret=new Combat.Trainer[] { new Combat.Trainer(trainer[0].fullname, trainer[0].id) };//(trainer[0][1], trainer[0][0])
				ret[0].id=trainer[0][2];
				ret[0].badges=trainer[0][3];
				return ret;
			}
		}
	}

	/// <summary>
	/// Playback?
	/// </summary>
	/// <typeparam name="TBattle"></typeparam>
	public class PokeBattle_BattlePlayerModule<out TBattle> : PokeBattle_RecordedBattleModule<TBattle>, IBattlePlayerModule<TBattle>
			where TBattle : PokeBattle_RecordedBattleModule<TBattle>, IRecordedBattleModule<TBattle>, IBattle, IBattleRecordData
	{
		#region Variables
		public int randomindex { get; protected set; }
		public int switchindex { get; protected set; }
			#endregion

		public PokeBattle_BattlePlayerModule(IPokeBattle_Scene scene, TBattle battle) : base (scene, battle.party1, battle.party2, battle.player, battle.opponent)
		{ (this as IBattlePlayerModule<TBattle>).initialize(scene, battle); }
		public TBattle initialize(IPokeBattle_Scene scene, IBattle battle)
		{
			@battletype=battle.battletype;
			@properties=battle.properties;
			@rounds=battle.rounds;
			@randomnums=battle.randomnums;
			@switches=battle.switches;
			@roundindex=-1;
			@randomindex=0;
			@switchindex=0;
			//base.initialize(scene,
			//   Marshal.restore(new StringInput(@properties["party1"])),
			//   Marshal.restore(new StringInput(@properties["party2"])),
			//   BattlePlayerHelper.pbCreateTrainerInfo(@properties["player"]),
			//   BattlePlayerHelper.pbCreateTrainerInfo(@properties["opponent"])
			//);
		}

		#region Methods
		public override BattleResults pbStartBattle(bool canlose=false) {
			/*@internalbattle=@properties["internalbattle"];
			@endspeech=@properties["endspeech"].ToString();
			@endspeech2=@properties["endspeech2"].ToString();
			@endspeechwin=@properties["endspeechwin"].ToString();
			@endspeechwin2=@properties["endspeechwin2"].ToString();
			@doublebattle=(bool)@properties["doublebattle"];
			@weather=@properties["weather"];
			@weatherduration=@properties["weatherduration"];
			@cantescape=(bool)@properties["cantescape"];
			@shiftStyle=(bool)@properties["shiftStyle"];
			@battlescene=@properties["battlescene"];
			@items=Marshal.restore(new StringInput(@properties["items"]));
			@rules=Marshal.restore(new StringInput(@properties["rules"]));
			@environment=@properties["environment"];*/
			return base.pbStartBattle(canlose);
		}

		public int pbSwitchInBetween(int i1,int i2,bool i3) {
			int ret=@switches[@switchindex];
			@switchindex+=1;
			return ret;
		}

		public override int pbRandom(int num) {
			int ret=@randomnums[@randomindex];
			@randomindex+=1;
			return ret;
		}

		public override void pbDisplayPaused(string str) {
			pbDisplay(str);
		}

		public void pbCommandPhaseCore() {
			@roundindex+=1;
			for (int i = 0; i < battlers.Length; i++) {
				//if (@rounds[@roundindex][i].Length==0) continue;
				if (@rounds[@roundindex][i] == null) continue;
				//@choices[i][0]=0;
				//@choices[i][1]=0;
				//@choices[i][2]=null;
				//@choices[i][3]=-1;
				@choices[i]=new Choice();
				switch (@rounds[@roundindex][i].Key) { //@rounds[@roundindex][i][0]
					case MenuCommands.FIGHT:
						if (@rounds[@roundindex][i].Value==-1) { //@rounds[@roundindex][i][1]==-1
							pbAutoChooseMove(i,false);
						}
						else {
							///pbRegisterMove(i,@rounds[@roundindex][i][1]);
							pbRegisterMove(i,@rounds[@roundindex][i].Value);
						}
						//if (@rounds[@roundindex][i][2]!=null)
							//pbRegisterTarget(i,@rounds[@roundindex][i][2]);
							pbRegisterTarget(i,@rounds[@roundindex][i].Value); //ToDo: Select target for Move choosen
						break;
					case MenuCommands.POKEMON:
						//pbRegisterSwitch(i,@rounds[@roundindex][i][1]);
						pbRegisterSwitch(i,@rounds[@roundindex][i].Value);
						break;
					case MenuCommands.BAG:
						//pbRegisterItem(i,(Items)@rounds[@roundindex][i][1]);
						pbRegisterItem(i,(Items)@rounds[@roundindex][i].Value);
						break;
					case MenuCommands.RUN:
						//@decision=(BattleResults)@rounds[@roundindex][i][1];
						@decision=(BattleResults)@rounds[@roundindex][i].Value;
						break;
				}
			}
		}
		#endregion
	}

	public class PokeBattle_RecordedBattle : PokeBattle_RecordedBattleModule<Battle>, IRecordedBattle { 
		//include PokeBattle_RecordedBattleModule;
		public PokeBattle_RecordedBattle(IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, ITrainer[] player, ITrainer[] opponent) : base(scene, p1, p2, player, opponent)
		{
		}
		public override int pbGetBattleType() {
			return 0;
		}
	}

	public class PokeBattle_RecordedBattlePalace : PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace>, IRecordedBattlePalace {
		//include PokeBattle_RecordedBattleModule;
		public PokeBattle_RecordedBattlePalace(IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, ITrainer[] player, ITrainer[] opponent) : base(scene, p1, p2, player, opponent)
		{
		}
		public override int pbGetBattleType() {
			return 1;
		}
	}

	public class PokeBattle_RecordedBattleArena : PokeBattle_RecordedBattleModule<PokeBattle_BattleArena>, IRecordedBattleArena {
		//include PokeBattle_RecordedBattleModule;
		public PokeBattle_RecordedBattleArena(IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, ITrainer[] player, ITrainer[] opponent) : base(scene, p1, p2, player, opponent)
		{
		}
		public override int pbGetBattleType() {
			return 2;
		}
	}

	public class PokeBattle_BattlePlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<Battle>>, IBattlePlayer {
		//include PokeBattle_BattlePlayerModule;
		public PokeBattle_BattlePlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<Battle> battle) : base(scene, battle)
		{
		}
	}

	public class PokeBattle_BattlePalacePlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace>>, IBattlePalacePlayer {
		//include PokeBattle_BattlePlayerModule;
		public PokeBattle_BattlePalacePlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace> battle) : base(scene, battle)
		{
		}
	}

	public class PokeBattle_BattleArenaPlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<PokeBattle_BattleArena>>, IBattleArenaPlayer {
		//include PokeBattle_BattlePlayerModule;
		public PokeBattle_BattleArenaPlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<PokeBattle_BattleArena> battle) : base(scene, battle)
		{
		}
	}
}
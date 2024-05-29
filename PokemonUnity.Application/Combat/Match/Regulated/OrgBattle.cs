using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity
{
	public partial class Game : PokemonEssentials.Interface.Battle.IGameOrgBattle
	{
		// ===============================================================================
		// Pokémon Organized Battle
		// ===============================================================================
		public bool HasEligible () {
			return BattleChallenge.rules.ruleset.hasValidTeam(Game.GameData.Trainer.party);
		}

		public PokemonUnity.Character.TrainerMetaData[] GetBTTrainers(int challengeID) {
			//PokemonUnity.Character.TrainerMetaData[] trlists=null;//load_data("Data/trainerlists.dat"); //rescue []
			//for (int i = 0; i < trlists.Length; i++) {
			//	PokemonUnity.Character.TrainerMetaData tr=trlists[i];
			//	if (!tr[5] && tr[2].Contains(challengeID)) {
			//		return tr[0];
			//	}
			//}
			//for (int i = 0; i < trlists.Length; i++) {
			//	PokemonUnity.Character.TrainerMetaData tr=trlists[i];
			//	if (tr[5]) {		// is default list
			//		return tr[0];
			//	}
			//}
			return new PokemonUnity.Character.TrainerMetaData[0];
		}

		public IPokemonSerialized[] GetBTPokemon(int challengeID) {
			//PokemonUnity.Character.TrainerData[] trlists=null;//load_data("Data/trainerlists.dat"); //rescue []
			//foreach (PokemonUnity.Character.TrainerData tr in trlists) {
			//	if (!tr[5] && tr[2].Contains(challengeID)) {
			//		return tr.Party;//[1]
			//	}
			//}
			//foreach (PokemonUnity.Character.TrainerData tr in trlists) {
			//	if (tr[5]) {		// is default list
			//		return tr.Party;//[1]
			//	}
			//}
			return new IPokemonSerialized[0];
		}

		public void RecordLastBattle() {
			Game.GameData.Global.lastbattle=Game.GameData.PokemonTemp.lastbattle;
			Game.GameData.PokemonTemp.lastbattle=null;
		}

		public void PlayBattle(IBattleRecordData battledata) {
			if (battledata!=null) {
				//IPokeBattle_Scene scene=NewBattleScene();
				//scene.abortable=true;
				//IBattle lastbattle=null;//Marshal.restore(new StringInput(battledata));
				//IBattlePlayerModule<IBattle> battleplayer =null;
				//switch (lastbattle[0]) {
				//	case BattleChallenge.BattleTower:
				//		battleplayer=new PokeBattle_BattlePlayer(scene,lastbattle);
				//		break;
				//	case BattleChallenge.BattlePalace:
				//		battleplayer=new PokeBattle_BattlePalacePlayer(scene,lastbattle);
				//		break;
				//	case BattleChallenge.BattleArena:
				//		battleplayer=new PokeBattle_BattleArenaPlayer(scene,lastbattle);
				//		break;
				//}
				//IAudioBGM bgm=BattlePlayerHelper.GetBattleBGM(lastbattle);
				//BattleAnimation(bgm, block: () => {
				//	SceneStandby(() => {
				//		BattleResults decision=battleplayer.StartBattle();
				//	});
				//});
			}
		}

		public void DebugPlayBattle() {
			//IChooseNumberParams param=new ChooseNumberParams();
			IChooseNumberParams param=ChooseNumberParams.initialize();
			param.setRange(0,500);
			param.setInitialValue(0);
			param.setCancelValue(-1);
			int num=GameMessage.MessageChooseNumber(_INTL("Choose a battle."),param);
			if (num>=0) {
				PlayBattleFromFile(string.Format("Battles/Battle%03d.dat",num));
			}
		}

		public void PlayLastBattle() {
			PlayBattle(Game.GameData.Global.lastbattle);
		}

		public void PlayBattleFromFile(string filename) {
			//RgssOpen(filename,"rb"){|f|
			//	PlayBattle(f.read);
			//}
		}

		public IBattleChallenge BattleChallenge { get {
			if (Game.GameData.Global.challenge==null) {
				Game.GameData.Global.challenge=new BattleChallenge();
			}
			return Game.GameData.Global.challenge;
		} }

		public int BattleChallengeTrainer(int numwins,PokemonUnity.Character.TrainerMetaData[] bttrainers) {
			int[] table=new int[] {
				0,5,0,100,
				6,6,80,40,
				7,12,80,40,
				13,13,120,20,
				14,19,100,40,
				20,20,140,20,
				21,26,120,40,
				27,27,160,20,
				28,33,140,40,
				34,34,180,20,
				35,40,160,40,
				41,41,200,20,
				42,47,180,40,
				48,48,220,40,
				49,-1,200,100
			};
			for (int i = 0; i < table.Length/4; i++) {
				if (table[i*4]<=numwins) {
					if ((table[i*4+1]<0 || table[i*4+1]>=numwins)) {
						int offset=(int)Math.Floor(Math.Floor((float)table[i*4+2]*bttrainers.Length)/300);
						int length=(int)Math.Floor(Math.Floor((float)table[i*4+3]*bttrainers.Length)/300);
						return (int)Math.Floor((float)offset+Core.Rand.Next(length));
					}
				}
			}
			return 0;
		}

		public void BattleChallengeGraphic(IGameCharacter @event) {
			int nextTrainer=BattleChallenge.nextTrainer;
			PokemonUnity.Character.TrainerMetaData[] bttrainers=GetBTTrainers(BattleChallenge.currentChallenge);
			string filename=TrainerCharNameFile(bttrainers[nextTrainer].ID); //[0] rescue 0
			try { //begin;
				//IAnimatedBitmap bitmap=new AnimatedBitmap("Graphics/Characters/"+filename);
				//bitmap.Dispose();
				@event.character_name=filename;
			} catch { //rescue;
				@event.character_name="NPC 01";
			}
		}

		public string BattleChallengeBeginSpeech() {
			if (!BattleChallenge.InProgress) {
				return "...";
			} else {
				PokemonUnity.Character.TrainerMetaData[] bttrainers=GetBTTrainers(BattleChallenge.currentChallenge);
				PokemonUnity.Character.TrainerMetaData tr=bttrainers[BattleChallenge.nextTrainer];
				return tr.ScriptBattleIntro??"...";  //? GetMessageFromHash(MessageTypes.BeginSpeech,tr[2]) : "...";
			}
		}

		public bool EntryScreen() {
			bool retval=false;
			FadeOutIn(99999, block: () => {
				//IPartyDisplayScene scene=new PokemonScreen_Scene();
				//IPartyDisplayScreen screen=new PokemonScreen(scene,Game.GameData.Trainer.party);
				IPartyDisplayScene scene=Game.GameData.Scenes.Party;
				IPartyDisplayScreen screen=Game.GameData.Screens.Party.initialize(scene,Game.GameData.Trainer.party);
				IPokemon[] ret=screen.PokemonMultipleEntryScreenEx(BattleChallenge.rules.ruleset);
				//  Set party
				if (ret!=null) BattleChallenge.setParty(ret);
				//  Continue (return true) if Pokémon were chosen
				retval=(ret!=null && ret.Length>0);
			});
			return retval;
		}

		public bool BattleChallengeBattle{ get {
		//public IBattle BattleChallengeBattle{ get {
			return BattleChallenge.Battle;
		} }

		public IPokemon[] BattleFactoryPokemon(IPokemonChallengeRules rule,int numwins,int numswaps,IPokemon[] rentals) {
			int[] table=null;
			IPokemonSerialized[] btpokemon=GetBTPokemon(BattleChallenge.currentChallenge);
			int[] ivtable=new int[] {
				0,6,3,6,
				7,13,6,9,
				14,20,9,12,
				21,27,12,15,
				28,34,15,21,
				35,41,21,31,
				42,-1,31,31
			};
			int[] groups=new int[] {
				1,14,6,0,
				15,21,5,1,
				22,28,4,2,
				29,35,3,3,
				36,42,2,4,
				43,-1,1,5
			};
			if (rule.ruleset.suggestedLevel!=100) {
				table=new int[] {
					0,6,110,199,
					7,13,162,266,
					14,20,267,371,
					21,27,372,467,
					28,34,468,563,
					35,41,564,659,
					42,48,660,755,
					49,-1,372,849
				};
			} else { // Open Level (Level 100)
				table=new int[] {
					0,6,372,467,
					7,13,468,563,
					14,20,564,659,
					21,27,660,755,
					28,34,372,881,
					35,41,372,881,
					42,48,372,881,
					49,-1,372,881
				};
			}
			int[] pokemonNumbers= new int[]{ 0,0 };
			int[] ivs= new int[]{ 0,0 };
			int[] ivgroups= new int[]{ 6,0 };
			for (int i = 0; i < table.Length/4; i++) {
				if (table[i*4]<=numwins) {
					if ((table[i*4+1]<0 || table[i*4+1]>=numwins)) {
						pokemonNumbers=new int[] {
							table[i*4+2]*btpokemon.Length/882,
							table[i*4+3]*btpokemon.Length/882
						};
					}
				}
			}
			for (int i = 0; i < ivtable.Length/4; i++) {
				if (ivtable[i*4]<=numwins) {
					if ((ivtable[i*4+1]<0 || ivtable[i*4+1]>=numwins)) {
						ivs=new int[] {ivtable[i*4+2],ivtable[i*4+3]};
					}
				}
			}
			for (int i = 0; i < groups.Length/4; i++) {
				if (groups[i*4]<=numswaps) {
					if ((groups[i*4+1]<0 || groups[i*4+1]>=numswaps)) {
						ivgroups=new int[] {groups[i*4+2],groups[i*4+3]};
					}
				}
			}
			IList<IPokemon> party=new List<IPokemon>();
			do {
				party.Clear();
				while (party.Count<Core.MAXPARTYSIZE) {
					int rnd=pokemonNumbers[0]+Core.Rand.Next(pokemonNumbers[1]-pokemonNumbers[0]+1);
					IPokemonSerialized rndpoke=btpokemon[rnd];
					int indvalue=(party.Count<ivgroups[0]) ? ivs[0] : ivs[1];
					party.Add(rndpoke.createPokemon(rule.ruleset.suggestedLevel,indvalue,null));
				}
			} while (!rule.ruleset.isValid(party)); //end until rule.ruleset.isValid(party);
			return party.ToArray();
		}

		public ITrainer GenerateBattleTrainer(int trainerid,IPokemonChallengeRules rule) {
			PokemonUnity.Character.TrainerMetaData[] bttrainers=GetBTTrainers(BattleChallenge.currentChallenge);
			PokemonUnity.Character.TrainerMetaData trainerdata=bttrainers[trainerid];
			ITrainer opponent=new Trainer(//PokeBattle_Trainer
				trainerdata.ID.ToString(),//GetMessageFromHash(MessageTypes.TrainerNames,trainerdata[1]),
				trainerdata.ID);//[0]
			IPokemonSerialized[] btpokemon=GetBTPokemon(BattleChallenge.currentChallenge);
			//  Individual Values
			int indvalues=31;
			if (trainerid<220) indvalues=21;
			if (trainerid<200) indvalues=18;
			if (trainerid<180) indvalues=15;
			if (trainerid<160) indvalues=12;
			if (trainerid<140) indvalues=9;
			if (trainerid<120) indvalues=6;
			if (trainerid<100) indvalues=3;
			//int[] pokemonnumbers=trainerdata[5]; //party size
			int[] pokemonnumbers=new int[btpokemon.Length].Select(a=>Core.Rand.Next(a)).ToArray(); //list of random numbers matching the number of pokemon in the trainer's party
			// p trainerdata
			//if (pokemonnumbers.Length<rule.ruleset.suggestedNumber) {
			//	//foreach (int n in pokemonnumbers) {
			//	for (int n = 0; n < pokemonnumbers.Length; n++) {
			//		IPokemonSerialized rndpoke=btpokemon[n];
			//		IPokemon pkmn=rndpoke.createPokemon(
			//			rule.ruleset.suggestedLevel,indvalues,opponent);
			//		((IList<IPokemon>)opponent.party).Add(pkmn);
			//	}
			//	return opponent;
			//}
			do { //begin;
				((IList<IPokemon>)opponent.party).Clear();
				while (opponent.party.Length<rule.ruleset.suggestedNumber) {
					int rnd=pokemonnumbers[Core.Rand.Next(pokemonnumbers.Length)];
					IPokemonSerialized rndpoke=btpokemon[rnd];
					IPokemon pkmn=rndpoke.createPokemon(
						rule.ruleset.suggestedLevel,indvalues,opponent);
					((IList<IPokemon>)opponent.party).Add(pkmn);
				}
			} while (!rule.ruleset.isValid(opponent.party)); //end until rule.ruleset.isValid(opponent.party);
			return opponent;
		}

		public bool OrganizedBattleEx(ITrainer opponent,IPokemonChallengeRules challengedata,string endspeech,string endspeechwin) {
			if (challengedata==null) {
				challengedata=new PokemonChallengeRules();
			}
			IPokeBattle_Scene scene=NewBattleScene();
			for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
				Game.GameData.Trainer.party[i].Heal();
			}
			Items[] olditems=Game.GameData.Trainer.party.Select(p=>p.Item).ToArray();	//.transform{|p| p.Item }
			Items[] olditems2=Game.GameData.Trainer.party.Select(p=>p.Item).ToArray();  //.transform{|p| p.Item }
			IBattle battle=challengedata.createBattle(scene,new ITrainer[] { Game.GameData.Trainer },new ITrainer[] { opponent });
			battle.internalbattle=false;
			battle.endspeech=endspeech;
			battle.endspeechwin=endspeechwin;
			if (Input.press(PokemonUnity.Input.CTRL) && Core.DEBUG) {
				GameMessage.Message(_INTL("SKIPPING BATTLE..."));
				GameMessage.Message(_INTL("AFTER LOSING..."));
				GameMessage.Message(battle.endspeech);
				PokemonTemp.lastbattle=null;
				return true;
			}
			int?[][] oldlevels=challengedata.adjustLevels(Game.GameData.Trainer.party,opponent.party);
			PrepareBattle(battle);
			BattleResults decision=0;
			IAudioBGM trainerbgm=GetTrainerBattleBGM(opponent);
			BattleAnimation(trainerbgm, block: () => {
				SceneStandby(block: () => {
					decision=((IRecordedBattleModule<IBattle>)battle).StartBattle();
				});
			});
			challengedata.unadjustLevels(Game.GameData.Trainer.party,opponent.party,oldlevels);
			for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
				Game.GameData.Trainer.party[i].Heal();
				Game.GameData.Trainer.party[i].setItem(olditems[i]);
			}
			for (int i = 0; i < opponent.party.Length; i++) {
				opponent.party[i].Heal();
				opponent.party[i].setItem(olditems2[i]);
			}
			Input.update();
			if (decision==BattleResults.WON||decision==BattleResults.LOST||decision==BattleResults.DRAW) { //(decision==1||decision==2||decision==5)
				Game.GameData.PokemonTemp.lastbattle=((IRecordedBattleModule<IBattle>)battle).DumpRecord();
			} else {
				Game.GameData.PokemonTemp.lastbattle=null;
			}
			return decision==BattleResults.WON;//(decision==1)
		}

		public bool IsBanned (IPokemon pokemon) {
			//return new StandardSpeciesRestriction().isValid(pokemon); //Doesnt work...
			return new StandardRestriction().isValid(pokemon) //Could be either or...
				|| new SpeciesRestriction().isValid(pokemon);
		}
	}

	#region General purpose utilities
	public static class ArrayHelper
	{
		//public void shuffle() {
		//	dup.shuffle!;
		//	unless(method_defined ? : shuffle)
		//}
		//
		//public void ^(other) { // xor of two arrays
		//	return (self|other)-(self&other);
		//}

		//public void shuffle!() {
		public static IList<T> shuffle<T>(this IList<T> array) {
			int size = array.Count;
			IPokemon[] tempa = new IPokemon[size];
			int i = size; do { //|i|
				int r = Core.Rand.Next(i); //size
				//array[i], array[r] = array[r], array[i];
				T temp = array[i];
				array[i] = array[r];
				array[r] = temp;
				i--;
			} while (i > 0); //size.times
			return array; //return this;
			//unless (method_defined? :shuffle!)
		}
	}

	//public static partial class Enumerable {
	//	public void transform() {
	//		ret=[];
	//		this.each(){|item| ret.Add(yield(item)) }
	//		return ret;
	//	}
	//}
	#endregion

	[System.Serializable]
	public partial class PBPokemon : IPokemonSerialized, ISerializable
	{
		public Pokemons species			{ get; protected set; }
		public Items item				{ get; protected set; }
		public Natures nature			{ get; protected set; }
		public Moves move1				{ get; protected set; }
		public Moves move2				{ get; protected set; }
		public Moves move3				{ get; protected set; }
		public Moves move4				{ get; protected set; }
		public int ev					{ get; protected set; }

		public PBPokemon(Pokemons species,Items? item,Natures nature,Moves? move1,Moves? move2,Moves? move3,Moves? move4,int ev) {
			this.species=species;
			this.item=item.HasValue ? item.Value : 0;
			this.nature=nature;
			this.move1=move1.HasValue ? move1.Value : 0;
			this.move2=move2.HasValue ? move2.Value : 0;
			this.move3=move3.HasValue ? move3.Value : 0;
			this.move4=move4.HasValue ? move4.Value : 0;
			this.ev=ev;
		}

		/*=begin;
		public void _dump(depth) {
			return [@species,@item,@nature,@move1,@move2,
				@move3,@move4,@ev].pack("vvCvvvvC");
		}

		public static void _load(str) {
			data=str.unpack("vvCvvvvC");
			return new this(
				data[0],data[1],data[2],data[3],
				data[4],data[5],data[6],data[7];
			);
		}
		=end;*/

		public IPokemonSerialized fromInspected(string str) {
			string insp = str;//.gsub(/^\s+/,"").gsub(/\s+$/,"");
			string[] pieces = insp.Split(';').Select(text => text.Trim()).ToArray(); //(/\s*;\s*/);
			Pokemons species = 0;//1;
			//if ((Species.const_defined(pieces[0]))) { //rescue false
			//	species=Species.const_get(pieces[0]);
			//}
			Items item=0;
			//if ((Items.const_defined(pieces[1]))) { //rescue false
			//	item=Items.const_get(pieces[1]);
			//}
			Natures nature=(Natures)int.Parse(pieces[2]);//.const_get
			string[] ev = pieces[3].Split(',');//.Select(num => int.Parse(num)).ToArray(); //(/\s*,\s*/);
			int evvalue=0;
			for (int i = 0; i < 6; i++) {
				if (string.IsNullOrEmpty(ev[i].Trim())) continue; //if (ev[i]==null||ev[i]=="")
				string evupcase=ev[i].Trim().ToUpper();
				if (evupcase=="HP") evvalue|=0x01;
				if (evupcase=="ATK") evvalue|=0x02;
				if (evupcase=="DEF") evvalue|=0x04;
				if (evupcase=="SPD") evvalue|=0x08;
				if (evupcase=="SA") evvalue|=0x10;
				if (evupcase=="SD") evvalue|=0x20;
			}
			int[] moves=pieces[4].Split(',').Select(num => int.Parse(num)).ToArray(); //(/\s*,\s*/);
			IList<Moves?> moveid=new List<Moves?>();
			for (int i = 0; i < 4; i++) {
				//if ((Moves.const_defined(moves[i]))) { //rescue false
				//	moveid.Add(Moves.const_get(moves[i]));
				//}
			}
			if (moveid.Count==0) moveid=new Moves?[] { (Moves)1 };
			return new PBPokemon(species,item,nature,
				moveid[0],(moveid[1]??0),(moveid[2]??0),(moveid[3]??0),evvalue);
		}

		public IPokemonSerialized fromPokemon(IPokemon pokemon) {
			int evvalue=0;
			if (pokemon.EV[0]>60) evvalue|=0x01;
			if (pokemon.EV[1]>60) evvalue|=0x02;
			if (pokemon.EV[2]>60) evvalue|=0x04;
			if (pokemon.EV[3]>60) evvalue|=0x08;
			if (pokemon.EV[4]>60) evvalue|=0x10;
			if (pokemon.EV[5]>60) evvalue|=0x20;
			return new PBPokemon(pokemon.Species,pokemon.Item,pokemon.Nature,
				pokemon.moves[0].id,pokemon.moves[1].id,pokemon.moves[2].id,
				pokemon.moves[3].id,evvalue);
		}

		public string inspect() {
			string c1=species.ToString(TextScripts.Name);//getConstantName(Species,@species);
			string c2=(@item==0) ? "" : item.ToString(TextScripts.Name);//getConstantName(Items,@item)
			string c3=nature.ToString(TextScripts.Name); //getConstantName(Natures,@nature);
			string evlist="";
			for (int i = 0; i < @ev; i++) {
				if (((@ev&(1<<i))!=0)) {
				if (evlist.Length>0) evlist+=",";
					evlist+= new []{ "HP","ATK","DEF","SPD","SA","SD" }[i];
				}
			}
			string c4=(@move1==0) ? "" : move1.ToString(TextScripts.Name); //getConstantName(Moves,@move1)
			string c5=(@move2==0) ? "" : move2.ToString(TextScripts.Name); //getConstantName(Moves,@move2)
			string c6=(@move3==0) ? "" : move3.ToString(TextScripts.Name); //getConstantName(Moves,@move3)
			string c7=(@move4==0) ? "" : move4.ToString(TextScripts.Name); //getConstantName(Moves,@move4)
			return $"#{c1};#{c2};#{c3};#{evlist};#{c4},#{c5},#{c6},#{c7}";
		}

		public string tocompact() {
			return $"#{species},#{item},#{nature},#{move1},#{move2},#{move3},#{move4},#{ev}";
		}

		//public static void constFromStr(mod,str) {
		//	int maxconst=0;
		//	foreach (var constant in mod.constants) {
		//		maxconst=[maxconst,mod.const_get(constant.to_sym)].max;
		//	}
		//	for (int i = 1; i < maxconst; i++) {
		//		val=i.ToString(TextScripts.Name);
		//		if (!val || val=="") continue;
		//		if (val==str) return i;
		//	}
		//	return 0;
		//}
		//
		//public static void fromString(str) {
		//	return this.fromstring(str);
		//}

		public IPokemonSerialized fromString(string str) {
			string[] s=str.Split(','); //(/\s*,\s*/);
			Pokemons species=				(Pokemons)	int.Parse(s[1]);		//this.constFromStr(Species,s[1]);
			Items item=						(Items)		int.Parse(s[2]);		//this.constFromStr(Items,s[2]);
			Natures nature=					(Natures)	int.Parse(s[3]);		//this.constFromStr(Natures,s[3]);
			Moves move1=					(Moves)		int.Parse(s[4]);		//this.constFromStr(Moves,s[4]);
			Moves move2=(s.Length>=12) ?	(Moves)		int.Parse(s[5]) : 0;	//this.constFromStr(Moves,s[5])
			Moves move3=(s.Length>=13) ?	(Moves)		int.Parse(s[6]) : 0;	//this.constFromStr(Moves,s[6])
			Moves move4=(s.Length>=14) ?	(Moves)		int.Parse(s[7]) : 0;	//this.constFromStr(Moves,s[7])
			int ev=0;
			int slen=s.Length-6;
			if (int.Parse(s[slen]	)>0) ev|=0x01;
			if (int.Parse(s[slen+1]	)>0) ev|=0x02;
			if (int.Parse(s[slen+2]	)>0) ev|=0x04;
			if (int.Parse(s[slen+3]	)>0) ev|=0x08;
			if (int.Parse(s[slen+4]	)>0) ev|=0x10;
			if (int.Parse(s[slen+5]	)>0) ev|=0x20;
			return new PBPokemon(species,item,nature,move1,move2,move3,move4,ev);
		}

		public Moves convertMove(Moves move) {
			if (move == Moves.RETURN) {// && hasConst(Moves,:FRUSTRATION)
				move=Moves.FRUSTRATION;
			}
			return move;
		}

		public IPokemon createPokemon(int level,int iv,ITrainer trainer) {
			//IPokemon pokemon=new Monster.Pokemon(@species,level,player:trainer,withMoves:false); //PokeBattle_Pokemon
			IPokemon pokemon=new Monster.Pokemon(@species,level:(byte)level,original:trainer); //PokeBattle_Pokemon
			pokemon.setItem(@item);
			//pokemon.PersonalId=Core.Rand.Next(65536);
			//pokemon.PersonalId|=Core.Rand.Next(65536)<<8;
			//pokemon.PersonalId-=pokemon.PersonalId%25;
			//pokemon.PersonalId+=nature;
			//pokemon.PersonalId&=0xFFFFFFFF;
			//pokemon.Happiness=0;
			pokemon.moves[0]=new Attack.Move(this.convertMove(@move1));
			pokemon.moves[1]=new Attack.Move(this.convertMove(@move2));
			pokemon.moves[2]=new Attack.Move(this.convertMove(@move3));
			pokemon.moves[3]=new Attack.Move(this.convertMove(@move4));
			int evcount=0;
			for (int i = 0; i < 6; i++) {
				if (((@ev&(1<<i))!=0)) evcount+=1;
			}
			int evperstat=(evcount==0) ? 0 : Monster.Pokemon.EVLIMIT/evcount; //PokeBattle_Pokemon
			for (int i = 0; i < 6; i++) {
				pokemon.IV[i]=iv;
				pokemon.EV[i]=((@ev&(1<<i))!=0) ? (byte)evperstat : (byte)0;
			}
			pokemon.calcStats();
			return pokemon;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("species",	species);
			info.AddValue("item",		item);
			info.AddValue("nature",		nature);
			info.AddValue("move1",		move1);
			info.AddValue("move2",		move2);
			info.AddValue("move3",		move3);
			info.AddValue("move4",		move4);
			info.AddValue("ev",			ev);
		}
	}

	public partial class Game_Map : PokemonEssentials.Interface.Battle.IGameMapOrgBattle {
		public int map_id				{ get; set; }
	}

	public partial class Game_Player : PokemonEssentials.Interface.Battle.IGamePlayerOrgBattle {
		public int direction							{ get; protected set; }
		protected int @prelock_direction;//				{ get; protected set; }
		protected float x;//							{ get; protected set; }
		protected float y;//							{ get; protected set; }
		protected float real_x;//						{ get; protected set; }
		protected float real_y;//						{ get; protected set; }

		public void moveto2(float x,float y) {
			this.x = x;
			this.y = y;
			@real_x = this.x * 128;
			@real_y = this.y * 128;
			@prelock_direction = 0;
		}
	}

	public partial class BattleChallengeType : PokemonEssentials.Interface.Battle.IBattleChallengeType, ICloneable {
		public int currentWins				{ get; protected set; }
		public int previousWins				{ get; protected set; }
		public int maxWins					{ get; protected set; }
		public int currentSwaps				{ get; protected set; }
		public int previousSwaps			{ get; protected set; }
		public int maxSwaps					{ get; protected set; }
		public bool doublebattle			{ get; protected set; }
		public int numPokemon				{ get; protected set; }
		public int battletype				{ get; protected set; }
		public int mode						{ get; protected set; }
		public int numRounds						{ get; set; }

		public IBattleChallengeType initialize() {
			@previousWins=0;
			@maxWins=0;
			@currentWins=0;
			@currentSwaps=0;
			@previousSwaps=0;
			@maxSwaps=0;
			return this;
		}

		public void saveWins(IBattleChallengeData challenge) {
			if (challenge.decision!=0) {							// if won or lost
				if (challenge.decision==BattleResults.WON) {		// if won
					@currentWins=challenge.wins;
					@currentSwaps=challenge.swaps;
				} else {
					@currentWins=0;
					@currentSwaps=0;
				}
				@maxWins=Math.Max(@maxWins,challenge.wins);
				@previousWins=challenge.wins;
				@maxSwaps=Math.Max(@maxSwaps,challenge.swaps);
				@previousSwaps=challenge.swaps;
			} else { // if undecided
				@currentWins=0;
				@currentSwaps=0;
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}

	public partial class BattleChallengeData : PokemonEssentials.Interface.Battle.IBattleChallengeData {
		public bool resting								{ get; protected set; }
		public int wins									{ get; protected set; }
		public int swaps								{ get; protected set; }
		public bool inProgress							{ get; protected set; }
		public int battleNumber							{ get; protected set; }
		public int numRounds							{ get; protected set; }
		public BattleResults decision					{ get; set; }
		public IPokemon[] party							{ get; protected set; }
		public IBattleFactoryData extraData				{ get; protected set; }
		protected ITilePosition start;//				{ get; protected set; }
		protected IPokemon[] oldParty;//				{ get; protected set; }
		protected IList<int> trainers;//				{ get; protected set; }

		public BattleChallengeData() {
			initialize();
		}

		public IBattleChallengeData initialize() {
			reset();
			return this;
		}

		public void setExtraData(IBattleFactoryData value) {
			@extraData=value;
		}

		public void AddWin() {
			if (@inProgress) {
				@battleNumber+=1;
				@wins+=1;
			}
		}

		public void AddSwap() {
			if (@inProgress) {
				@swaps+=1;
			}
		}

		public bool MatchOver() {
			if (!@inProgress) return true;
			if (@decision!=0) return true;
			return (@battleNumber>@numRounds);
		}

		public int nextTrainer { get {
			return @trainers[@battleNumber-1];
		} }

		public void GoToStart() {
			if (Game.GameData.Scene is ISceneMap) {
				Game.GameData.GameTemp.player_transferring = true;
				Game.GameData.GameTemp.player_new_map_id = @start.MapId;	//@start[0];
				Game.GameData.GameTemp.player_new_x = @start.X;				//@start[1];
				Game.GameData.GameTemp.player_new_y = @start.Y;				//@start[2];
				Game.GameData.GameTemp.player_new_direction = 8;
				Game.GameData.Scene.transfer_player();
			}
		}

		public void setParty(IPokemon[] value) {
			if (@inProgress) {
				Game.GameData.Trainer.party=value;
				@party=value;
			} else {
				@party=value;
			}
		}

		public void Start(IBattleChallengeType t, int numRounds) {
			@inProgress=true;
			@resting=false;
			@decision=0;
			@swaps=t.currentSwaps;
			@wins=t.currentWins;
			@battleNumber=1;
			@trainers=new List<int>();
			//if (numRounds<=0) throw new Exception (Game._INTL("Number of rounds is 0 or less."));
			if (numRounds<=0) { GameDebug.LogError(Game._INTL("Number of rounds is 0 or less.")); numRounds = 1; }
			this.numRounds=numRounds;
			Character.TrainerMetaData[] btTrainers=new Character.TrainerMetaData[0];//GetBTTrainers(BattleChallenge.currentChallenge);
			if (Game.GameData is IGameOrgBattle gob) btTrainers=gob.GetBTTrainers(gob.BattleChallenge.currentChallenge);
			while (@trainers.Count<this.numRounds) {
				int? newtrainer = null;//BattleChallengeTrainer(@wins+@trainers.Count,btTrainers);
				if (Game.GameData is IGameOrgBattle gob2) newtrainer=gob2.BattleChallengeTrainer(@wins+@trainers.Count,btTrainers);
				bool found=false;
				foreach (int tr in @trainers) {
					if (tr==newtrainer) found=true;
				}
				if (!found) @trainers.Add(newtrainer.Value);
			}
			@start=new TilePosition(Game.GameData.GameMap.map_id,Game.GameData.GamePlayer.x,Game.GameData.GamePlayer.y);
			@oldParty=Game.GameData.Trainer.party;
			if (@party != null) Game.GameData.Trainer.party=@party;
			if (Game.GameData is IGameSave s) s.Save(true);
		}

		public void Cancel() {
			if (@oldParty != null) Game.GameData.Trainer.party=@oldParty;
			reset();
		}

		public void End() {
			Game.GameData.Trainer.party=@oldParty;
			if (!@inProgress) return;
			bool save=(@decision!=0);
			reset();
			Game.GameData.GameMap.need_refresh=true;
			if (save) {
				if (Game.GameData is IGameSave s) s.Save(true);
			}
		}

		public void GoOn() {
			if (!@inProgress) return;
			@resting=false;
			SaveInProgress();
		}

		public void Rest() {
			if (!@inProgress) return;
			@resting=true;
			SaveInProgress();
		}

		//private;

		private void reset() {
			@inProgress=false;
			@resting=false;
			@start=new TilePosition();//null;
			@decision=0;
			@wins=0;
			@swaps=0;
			@battleNumber=0;
			@trainers=new List<int>();
			@oldParty=null;
			@party=null;
			@extraData=null;
		}

		private void SaveInProgress() {
			int oldmapid=Game.GameData.GameMap.map_id;
			float oldx=Game.GameData.GamePlayer.x;
			float oldy=Game.GameData.GamePlayer.y;
			int olddirection=Game.GameData.GamePlayer.direction;
			Game.GameData.GameMap.map_id=@start.MapId; //@start[0];
			if (Game.GameData.GamePlayer is IGamePlayerOrgBattle gpob) gpob.moveto2(@start.X,@start.Y); //(@start[1],@start[2]);
			Game.GameData.GamePlayer.direction=8; // facing up
			if (Game.GameData is IGameSave s) s.Save(true);
			Game.GameData.GameMap.map_id=oldmapid;
			if (Game.GameData.GamePlayer is IGamePlayerOrgBattle gpob2) gpob2.moveto2(oldx,oldy);
			Game.GameData.GamePlayer.direction=olddirection;
		}
	}

	public partial class BattleChallenge : PokemonEssentials.Interface.Battle.IBattleChallenge {
		public int currentChallenge							{ get; protected set; }
		protected int id;//									{ get; protected set; }
		protected int numRounds;//							{ get; protected set; }
		protected IBattleChallengeData bc;//				{ get; protected set; }
		protected IPokemonChallengeRules _rules;//			{ get; protected set; }
		protected IList<IBattleChallengeType> types;//		{ get; protected set; }
		const int BattleTower   = 0;
		const int BattlePalace  = 1;
		const int BattleArena   = 2;
		const int BattleFactory = 3;

		public BattleChallenge() {
			initialize();
		}

		public IBattleChallenge initialize() {
			@bc=new BattleChallengeData();
			@currentChallenge=-1;
			@types=new List<IBattleChallengeType>();
			return this;
		}

		public IPokemonChallengeRules rules { get  {
			if (_rules == null) {
				_rules=modeToRules(this.data.doublebattle,
				this.data.numPokemon,
				this.data.battletype,this.data.mode);
			}
			return _rules;
		} }

		public IPokemonChallengeRules modeToRules(bool doublebattle, int numPokemon, int battletype, int mode) {
			IPokemonChallengeRules rules = new PokemonChallengeRules();
			if (battletype==BattlePalace) {
				rules.setBattleType(new BattlePalace());
			} else if (battletype==BattleArena) {
				rules.setBattleType(new BattleArena());
				doublebattle=false;
			} else {
				rules.setBattleType(new BattleTower());
			}
			if (mode==1) {		// Open Level
				rules.setRuleset(new StandardRules(numPokemon,Core.MAXIMUMLEVEL)); //Experiences.MAXLEVEL
				rules.setLevelAdjustment(new OpenLevelAdjustment(30));
			} else if (mode==2) {		// Battle Tent
				rules.setRuleset(new StandardRules(numPokemon,Core.MAXIMUMLEVEL)); //Experiences.MAXLEVEL
				rules.setLevelAdjustment(new OpenLevelAdjustment(60));
			} else {
				rules.setRuleset(new StandardRules(numPokemon,50));
				rules.setLevelAdjustment(new OpenLevelAdjustment(50)) ;
			}
			if (doublebattle) {
				rules.addBattleRule(new DoubleBattle());
			} else {
				rules.addBattleRule(new SingleBattle());
			}
			return rules;
		}

		public void set(int id, int numrounds, IPokemonChallengeRules rules) {
			this._rules=rules;
			this.id=id;
			this.numRounds=numrounds;
			if (Game.GameData is IGameOrgBattleGenerator gobg) gobg.WriteCup(id,rules);
		}

		public void start(params object[] args) {
			IBattleChallengeType t=ensureType(@id);
			@currentChallenge=@id; // must appear before Start
			@bc.Start(t,@numRounds);
		}

		public void register(int id, bool doublebattle, int numrounds, int numPokemon, int battletype, int mode= 1) {
			IBattleChallengeType t=ensureType(id);
			if (battletype==BattleFactory) {
				@bc.setExtraData(new BattleFactoryData(@bc));
				numPokemon=3;
				battletype=BattleTower;
			}
			t.numRounds=numrounds;
			_rules=modeToRules(doublebattle,numPokemon,battletype,mode);
		}

		public bool InChallenge { get {
			return InProgress;
		} }

		public IBattleChallengeType data { get {
			if (!InProgress || @currentChallenge<0) return null;
			return (IBattleChallengeType)ensureType(@currentChallenge).Clone();
		} }

		public int getCurrentWins(int challenge) {
			return ensureType(challenge).currentWins;
		}

		public int getPreviousWins(int challenge) {
			return ensureType(challenge).previousWins;
		}

		public int getMaxWins(int challenge) {
			return ensureType(challenge).maxWins;
		}

		public int getCurrentSwaps(int challenge) {
			return ensureType(challenge).currentSwaps;
		}

		public int getPreviousSwaps(int challenge) {
			return ensureType(challenge).previousSwaps;
		}

		public int getMaxSwaps(int challenge) {
			return ensureType(challenge).maxSwaps;
		}

		public void Start(int challenge) {
		}

		public void End() {
			if (@currentChallenge!=-1) {
				ensureType(@currentChallenge).saveWins(@bc);
				@currentChallenge=-1;
			}
			@bc.End();
		}

		public bool Battle { get {
		//public BattleResults Battle { get {
			if (@bc.extraData != null) return @bc.extraData.Battle(this);
			ITrainer opponent=null;//GenerateBattleTrainer(this.nextTrainer,this.rules);
			if (Game.GameData is IGameOrgBattle gob) opponent=gob.GenerateBattleTrainer(this.nextTrainer,this.rules);
			Character.TrainerMetaData[] bttrainers=null; //GetBTTrainers(@id);
			if (Game.GameData is IGameOrgBattle gob1) bttrainers = gob1.GetBTTrainers(@id);
			Character.TrainerMetaData trainerdata=bttrainers[this.nextTrainer];
			bool ret=false;//BattleResults.InProgress
			if (Game.GameData is IGameOrgBattle gob2) ret=gob2.OrganizedBattleEx(opponent,this.rules,
				trainerdata.ScriptBattleEnd[0],		//GetMessageFromHash(MessageTypes.EndSpeechLose,trainerdata[4]),
				trainerdata.ScriptBattleEnd[1]);	//GetMessageFromHash(MessageTypes.EndSpeechWin,trainerdata[3]));
			return ret;
		} }

		public bool InProgress { get {
			return @bc.inProgress;
		} }

		public bool Resting() {
			return @bc.resting;
		}

		public void setDecision(BattleResults value) {
			@bc.decision=value;
		}

		public void setParty(IPokemon[] value) {
			@bc.setParty(value);
		}

		public IBattleFactoryData extra { get { return @bc.extraData; } }
		public BattleResults decision { get { return @bc.decision; } }
		public int wins { get { return @bc.wins; } }
		public int swaps { get { return @bc.swaps; } }
		public int battleNumber { get { return @bc.battleNumber; } }
		public int nextTrainer { get { return @bc.nextTrainer; } }
		public void GoOn() { @bc.GoOn(); }
		public void AddWin() { @bc.AddWin(); }
		public void Cancel() { @bc.Cancel(); }
		public void Rest() { @bc.Rest(); }
		public bool MatchOver() { return @bc.MatchOver(); }
		public void GoToStart() { @bc.GoToStart(); }

		//private;

		private IBattleChallengeType ensureType(int id) {
			if (@types is Array) {
				IList<IBattleChallengeType> oldtypes=@types;
				@types=new List<IBattleChallengeType>();
				for (int i = 0; i < oldtypes.Count; i++) {
					if (oldtypes[i] != null) {
						@types[i]=oldtypes[i];
					}
				}
			}
			if (@types[id] == null) {
				@types[id]=new BattleChallengeType();
			}
			return @types[id];
		}
	}

	public partial class Game_Event : PokemonEssentials.Interface.Battle.IGameEventOrgBattle {
		public bool InChallenge { get {
			if (Game.GameData is IGameOrgBattle gob) return gob.BattleChallenge.InChallenge;
			return false;//BattleChallenge.InChallenge;
		} }
	}

	public partial class BattleFactoryData : PokemonEssentials.Interface.Battle.IBattleFactoryData {
		protected IBattleChallengeData bcdata;
		//protected IBattleChallenge bcdata;
		protected ITrainer opponent;
		protected IPokemon[] rentals;
		protected IPokemon[] oldopponent;
		protected int trainerid;

		public BattleFactoryData(IBattleChallengeData bcdata) {
			initialize(bcdata);
		}

		public IBattleFactoryData initialize(IBattleChallengeData bcdata) {
			this.bcdata=bcdata;
			return this;
		}

		public void PrepareRentals() {
			if (Game.GameData is IGameOrgBattle gob) @rentals=gob.BattleFactoryPokemon(null,@bcdata.wins,@bcdata.swaps,new IPokemon[0]);
			@trainerid=@bcdata.nextTrainer;
			Character.TrainerMetaData[] bttrainers=null;//GetBTTrainers(@bcdata.currentChallenge); //bcdata == IBattleChallenge
			//if (Game.GameData is IGameOrgBattle gob2) bttrainers=gob2.GetBTTrainers(@bcdata.currentChallenge); //ToDo: Uncomment
			Character.TrainerMetaData trainerdata=bttrainers[@trainerid];
			@opponent=new Trainer( //PokeBattle_Trainer(
				trainerdata.ID.ToString(),	//GetMessageFromHash(MessageTypes.TrainerNames,trainerdata[1]),
				trainerdata.ID);			//trainerdata[0]);
			IPokemon[] opponentPkmn=null;//BattleFactoryPokemon(1,@bcdata.wins,@bcdata.swaps,@rentals);
			if (Game.GameData is IGameOrgBattle gob3) opponentPkmn=gob3.BattleFactoryPokemon(null,@bcdata.wins,@bcdata.swaps,@rentals);
			@opponent.party=opponentPkmn.shuffle<IPokemon>().ToArray();//.Shuffle(0,3);//.shuffle[0,3];
		}

		public void ChooseRentals() {
			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				IBattleSwapScene scene=Game.GameData.Scenes.BattleSwapScene; //new BattleSwapScene();
				IBattleSwapScreen screen=Game.GameData.Screens.BattleSwapScreen.initialize(scene); //new BattleSwapScreen(scene);
				@rentals=screen.StartRent(@rentals);
				@bcdata.AddSwap();
				if (Game.GameData is IGameOrgBattle gob) gob.BattleChallenge.setParty(@rentals);
			});
		}

		public bool Battle(IBattleChallenge challenge) {
		//public PokemonUnity.Combat.BattleResults Battle(IBattleChallenge challenge) {
			Character.TrainerMetaData[] bttrainers=null;//GetBTTrainers(@bcdata.currentChallenge); //bcdata == IBattleChallenge
			//if (Game.GameData is IGameOrgBattle gob) bttrainers=gob.GetBTTrainers(@bcdata.currentChallenge); //ToDo: Uncomment
			Character.TrainerMetaData trainerdata=bttrainers[@trainerid];//Kernal.TrainerMetaData
			if (Game.GameData is IGameOrgBattle gob1) return gob1.OrganizedBattleEx(@opponent, challenge.rules,
				trainerdata.ScriptBattleEnd[0],		//GetMessageFromHash(MessageTypes.EndSpeechLose,trainerdata[4]),
				trainerdata.ScriptBattleEnd[1]);    //GetMessageFromHash(MessageTypes.EndSpeechWin,trainerdata[3]));
			return false; //BattleResults.InProgress;
		}

		public void PrepareSwaps() {
			@oldopponent=@opponent.party;
			int trainerid=@bcdata.nextTrainer;
			Character.TrainerMetaData[] bttrainers=null; //GetBTTrainers(@bcdata.currentChallenge);
			//if (Game.GameData is IGameOrgBattle gob) bttrainers=gob.GetBTTrainers(@bcdata.currentChallenge); //ToDo: Uncomment...
			Character.TrainerMetaData trainerdata=bttrainers[trainerid];
			@opponent=new Trainer( //PokeBattle_Trainer(
				trainerdata.ID.ToString(),	//GetMessageFromHash(MessageTypes.TrainerNames,trainerdata[1]),
				trainerdata.ID);			//trainerdata[0]);
			List<IPokemon> pool=new List<IPokemon>();pool.AddRange(@rentals);pool.AddRange(@oldopponent);
			IBattleChallenge challenge = null; //ToDo: Not sure how this is assigned... bcdata creates challenge somehow.
			IPokemon[] opponentPkmn=null;//BattleFactoryPokemon(
			if (Game.GameData is IGameOrgBattle gob2) opponentPkmn=gob2.BattleFactoryPokemon(
				challenge.rules,@bcdata.wins,@bcdata.swaps,
				pool.ToArray()); //new IPokemon[0].concat(@rentals).concat(@oldopponent));
			//ToDo: Uncomment and randomize the positions in the slots
			@opponent.party=opponentPkmn;//.Shuffle(0,3);//.shuffle[0,3];
		}

		public bool ChooseSwaps() {
			bool swapMade=true;
			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				IBattleSwapScene scene=Game.GameData.Scenes.BattleSwapScene; //new Game.GameData.Scenes.BattleSwapScene();
				IBattleSwapScreen screen=Game.GameData.Screens.BattleSwapScreen.initialize(scene); //new BattleSwapScreen(scene);
				swapMade=screen.StartSwap(@rentals,@oldopponent);
				if (swapMade) {
					@bcdata.AddSwap();
				}
				@bcdata.setParty(@rentals);
			});
			return swapMade;
		}
	}
}
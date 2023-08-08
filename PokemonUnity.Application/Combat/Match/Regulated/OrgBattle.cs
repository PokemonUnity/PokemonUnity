using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonUnity.Combat;

namespace PokemonUnity
{
	/*public partial class Game : PokemonEssentials.Interface.Battle.IGameOrgBattle
	{
		// ===============================================================================
		// Pokémon Organized Battle
		// ===============================================================================
		public bool HasEligible (*arg) {
			return BattleChallenge.rules.ruleset.hasValidTeam(Game.GameData.Trainer.party);
		}

		public void GetBTTrainers(challengeID) {
			trlists=(load_data("Data/trainerlists.dat") rescue []);
			for (int i = 0; i < trlists.Length; i++) {
				tr=trlists[i];
				if (!tr[5] && tr[2].Contains(challengeID)) {
					return tr[0];
				}
			}
			for (int i = 0; i < trlists.Length; i++) {
				tr=trlists[i];
				if (tr[5]) {		// is default list
					return tr[0];
				}
			}
			return [];
		}

		public void GetBTPokemon(challengeID) {
			trlists=(load_data("Data/trainerlists.dat") rescue []);
			foreach (var tr in trlists) {
				if (!tr[5] && tr[2].Contains(challengeID)) {
					return tr[1];
				}
			}
			foreach (var tr in trlists) {
				if (tr[5]) {		// is default list
					return tr[1];
				}
			}
			return [];
		}

		public void RecordLastBattle() {
			Game.GameData.Global.lastbattle=Game.GameData.PokemonTemp.lastbattle;
			Game.GameData.PokemonTemp.lastbattle=null;
		}

		public void PlayBattle(battledata) {
			if (battledata) {
			scene=NewBattleScene;
			scene.abortable=true;
			lastbattle=Marshal.restore(new StringInput(battledata));
			switch (lastbattle[0]) {
				case BattleChallenge.eattleTower:
					battleplayer=new PokeBattle_BattlePlayer(scene,lastbattle);
					break;
				case BattleChallenge.eattlePalace:
					battleplayer=new PokeBattle_BattlePalacePlayer(scene,lastbattle);
					break;
				case BattleChallenge.eattleArena:
					battleplayer=new PokeBattle_BattleArenaPlayer(scene,lastbattle);
					break;
				}
				bgm=BattlePlayerHelper.GetBattleBGM(lastbattle);
				BattleAnimation(bgm) {
					SceneStandby {
						decision=battleplayer.StartBattle;
					}
				}
			}
		}

		public void DebugPlayBattle() {
			params=new ChooseNumberParams();
			params.setRange(0,500);
			params.setInitialValue(0);
			params.setCancelValue(-1);
			num=Kernel.MessageChooseNumber(_INTL("Choose a battle."),params);
			if (num>=0) {
				PlayBattleFromFile(string.Format("Battles/Battle%03d.dat",num));
			}
		}

		public void PlayLastBattle() {
			PlayBattle(Game.GameData.Global.lastbattle);
		}

		public void PlayBattleFromFile(filename) {
			RgssOpen(filename,"rb"){|f|
				PlayBattle(f.read);
			}
		}

		public void BattleChallenge() {
			if (!Game.GameData.Global.challenge) {
				Game.GameData.Global.challenge=new BattleChallenge();
			}
			return Game.GameData.Global.challenge;
		}

		public void BattleChallengeTrainer(numwins,bttrainers) {
			table=new int[]
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
						offset=((table[i*4+2]*bttrainers.Length).floor/300).floor;
						length=((table[i*4+3]*bttrainers.Length).floor/300).floor;
						return (offset+Core.Rand.Next(length)).floor;
					}
				}
			}
			return 0;
		}

		public void BattleChallengeGraphic(event) {
			nextTrainer=BattleChallenge.nextTrainer;
			bttrainers=GetBTTrainers(BattleChallenge.currentChallenge);
			filename=TrainerCharNameFile((bttrainers[nextTrainer][0] rescue 0));
			begin;
				bitmap=new AnimatedBitmap("Graphics/Characters/"+filename);
				bitmap.dispose();
				event.character_name=filename;
			rescue;
				event.character_name="NPC 01";
			}
		}

		public void BattleChallengeBeginSpeech() {
			if (!BattleChallenge.InProgress?) {
				return "...";
			} else {
				bttrainers=GetBTTrainers(BattleChallenge.currentChallenge);
				tr=bttrainers[BattleChallenge.nextTrainer];
				return tr ? GetMessageFromHash(MessageTypes.seginSpeech,tr[2]) : "...";
			}
		}

		public void EntryScreen(*arg) {
			retval=false;
			FadeOutIn(99999){
				scene=new PokemonScreen_Scene();
				screen=new PokemonScreen(scene,Game.GameData.Trainer.party);
				ret=screen.PokemonMultipleEntryScreenEx(BattleChallenge.rules.ruleset);
				//  Set party
				if (ret) BattleChallenge.setParty(ret);
				//  Continue (return true) if Pokémon were chosen
				retval=(ret!=null && ret.Length>0);
			}
			return retval;
		}

		public void BattleChallengeBattle() {
			return BattleChallenge.Battle;
		}

		public void BattleFactoryPokemon(rule,numwins,numswaps,rentals) {
			table=null;
			btpokemon=GetBTPokemon(BattleChallenge.currentChallenge);
			ivtable=new int[] {
				0,6,3,6,
				7,13,6,9,
				14,20,9,12,
				21,27,12,15,
				28,34,15,21,
				35,41,21,31,
				42,-1,31,31
			};
			groups=new int[]
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
			pokemonNumbers= new []{ 0,0 };
			ivs= new []{ 0,0 };
			ivgroups= new []{ 6,0 };
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
						ivs=[ivtable[i*4+2],ivtable[i*4+3]];
					}
				}
			}
			for (int i = 0; i < groups.Length/4; i++) {
				if (groups[i*4]<=numswaps) {
					if ((groups[i*4+1]<0 || groups[i*4+1]>=numswaps)) {
						ivgroups=[groups[i*4+2],groups[i*4+3]];
					}
				}
			}
			party=new List<>();
			do {
				party.clear();
				while (party.Length<6) {
					rnd=pokemonNumbers[0]+Core.Rand.Next(pokemonNumbers[1]-pokemonNumbers[0]+1);
					rndpoke=btpokemon[rnd];
					indvalue=(party.Length<ivgroups[0]) ? ivs[0] : ivs[1];
					party.Add(rndpoke.createPokemon(rule.ruleset.suggestedLevel,indvalue,null));
				}
			} while (rule.ruleset.isValid(party)); //end until rule.ruleset.isValid(party);
			return party;
		}

		public void GenerateBattleTrainer(trainerid,rule) {
			bttrainers=GetBTTrainers(BattleChallenge.currentChallenge);
			trainerdata=bttrainers[trainerid];
			opponent=new PokeBattle_Trainer(
				GetMessageFromHash(MessageTypes.TrainerNames,trainerdata[1]),
				trainerdata[0]);
			btpokemon=GetBTPokemon(BattleChallenge.currentChallenge);
			//  Individual Values
			indvalues=31;
			if (trainerid<220) indvalues=21;
			if (trainerid<200) indvalues=18;
			if (trainerid<180) indvalues=15;
			if (trainerid<160) indvalues=12;
			if (trainerid<140) indvalues=9;
			if (trainerid<120) indvalues=6;
			if (trainerid<100) indvalues=3;
			pokemonnumbers=trainerdata[5];
			// p trainerdata
			if (pokemonnumbers.Length<rule.ruleset.suggestedNumber) {
				foreach (var n in pokemonnumbers) {
					rndpoke=btpokemon[n];
					pkmn=rndpoke.createPokemon(
						rule.ruleset.suggestedLevel,indvalues,opponent);
					opponent.party.Add(pkmn);
				}
				return opponent;
			}
			begin;
				opponent.party.clear();
				while (opponent.party.Length<rule.ruleset.suggestedNumber) {
					rnd=pokemonnumbers[Core.Rand.Next(pokemonnumbers.Length)];
					rndpoke=btpokemon[rnd];
					pkmn=rndpoke.createPokemon(
						rule.ruleset.suggestedLevel,indvalues,opponent);
					opponent.party.Add(pkmn);
				}
			end until rule.ruleset.isValid(opponent.party);
			return opponent;
		}

		public void OrganizedBattleEx(opponent,challengedata,endspeech,endspeechwin) {
			if (!challengedata) {
				challengedata=new PokemonChallengeRules();
			}
			scene=NewBattleScene;
			for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
				Game.GameData.Trainer.party[i].heal;
			}
			olditems=Game.GameData.Trainer.party.transform{|p| p.Item }
			olditems2=Game.GameData.Trainer.party.transform{|p| p.Item }
			battle=challengedata.createBattle(scene,Game.GameData.Trainer,opponent);
			battle.internalbattle=false;
			battle.endspeech=endspeech;
			battle.endspeechwin=endspeechwin;
			if (Input.press(Input.CTRL) && Core.DEBUG) {
				Kernel.Message(_INTL("SKIPPING BATTLE..."));
				Kernel.Message(_INTL("AFTER LOSING..."));
				Kernel.Message(battle.endspeech);
				Game.GameData.PokemonTemp.lastbattle=null;
				return true;
			}
			oldlevels=challengedata.adjustLevels(Game.GameData.Trainer.party,opponent.party);
			PrepareBattle(battle);
			decision=0;
			trainerbgm=GetTrainerBattleBGM(opponent);
			BattleAnimation(trainerbgm) {
				SceneStandby {
					decision=battle.StartBattle;
				}
			}
			challengedata.unadjustLevels(Game.GameData.Trainer.party,opponent.party,oldlevels);
			for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
				Game.GameData.Trainer.party[i].heal;
				Game.GameData.Trainer.party[i].setItem(olditems[i]);
			}
			for (int i = 0; i < opponent.party.Length; i++) {
				opponent.party[i].heal;
				opponent.party[i].setItem(olditems2[i]);
			}
			Input.update();
			if (decision==1||decision==2||decision==5) {
				Game.GameData.PokemonTemp.lastbattle=battle.DumpRecord;
			} else {
				Game.GameData.PokemonTemp.lastbattle=null;
			}
			return (decision==1);
		}

		public bool IsBanned (pokemon) {
			return new StandardSpeciesRestriction().isValid(pokemon);
		}
	}*/

	// ===============================================================================
	// General purpose utilities
	// ===============================================================================
	//public partial class Array {
	//	public void shuffle() {
	//		dup.shuffle!;
	//		unless(method_defined ? : shuffle)
	//	}
	//
	//	public void ^(other) { // xor of two arrays
	//		return (self|other)-(self&other);
	//	}
	//
	//	//public void shuffle!() {
	//	public IList<T> shuffle(this IList<T> array) {
	//		int size = array.Count;
	//		IPokemon[] tempa = new IPokemon[size];
	//		int i = size; do { //|i|
	//			int r = Kernel.Core.Rand.Next(i); //size
	//			//array[i], array[r] = array[r], array[i];
	//			T temp = array[i];
	//			array[i] = array[r];
	//			array[r] = temp;
	//			i--;
	//		} while (i > 0); //size.times
	//		return this;
	//		//unless (method_defined? :shuffle!)
	//	}
	//}

	//public static partial class Enumerable {
	//	public void transform() {
	//		ret=[];
	//		this.each(){|item| ret.Add(yield(item)) }
	//		return ret;
	//	}
	//}

	public partial class PBPokemon {
		public int species				{ get; protected set; }
		public int item				{ get; protected set; }
		public int nature				{ get; protected set; }
		public int move1				{ get; protected set; }
		public int move2				{ get; protected set; }
		public int move3				{ get; protected set; }
		public int move4				{ get; protected set; }
		public int ev				{ get; protected set; }

		//public Pokemon(species,item,nature,move1,move2,move3,move4,ev) {
		//	@species=species;
		//	@item=item ? item : 0;
		//	@nature=nature;
		//	@move1=move1 ? move1 : 0;
		//	@move2=move2 ? move2 : 0;
		//	@move3=move3 ? move3 : 0;
		//	@move4=move4 ? move4 : 0;
		//	@ev=ev;
		//}

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

		//public static void fromInspected(str) {
		//	insp=str.gsub(/^\s+/,"").gsub(/\s+$/,"");
		//	pieces=insp.split(/\s*;\s*/);
		//	species=1;
		//	if ((Species.const_defined(pieces[0]) rescue false)) {
		//		species=Species.const_get(pieces[0]);
		//	}
		//	item=0;
		//	if ((Items.const_defined(pieces[1]) rescue false)) {
		//		item=Items.const_get(pieces[1]);
		//	}
		//	nature=Natures.const_get(pieces[2]);
		//	ev=pieces[3].split(/\s*,\s*/);
		//	evvalue=0;
		//	for (int i = 0; i < 6; i++) {
		//		if (!ev[i]||ev[i]=="") continue;
		//		evupcase=ev[i].upcase;
		//		if (evupcase=="HP") evvalue|=0x01;
		//		if (evupcase=="ATK") evvalue|=0x02;
		//		if (evupcase=="DEF") evvalue|=0x04;
		//		if (evupcase=="SPD") evvalue|=0x08;
		//		if (evupcase=="SA") evvalue|=0x10;
		//		if (evupcase=="SD") evvalue|=0x20;
		//	}
		//	moves=pieces[4].split(/\s*,\s*/);
		//	moveid=[];
		//	for (int i = 0; i < 4; i++) {
		//		if ((Moves.const_defined(moves[i]) rescue false)) {
		//		moveid.Add(Moves.const_get(moves[i]));
		//		}
		//	}
		//	if (moveid.Length==0) moveid= new []{ 1 };
		//	return new this(species,item,nature,
		//		moveid[0],(moveid[1]||0),(moveid[2]||0),(moveid[3]||0),evvalue);
		//}
		//
		//public static void fromPokemon(pokemon) {
		//	evvalue=0;
		//	if (pokemon.ev[0]>60) evvalue|=0x01;
		//	if (pokemon.ev[1]>60) evvalue|=0x02;
		//	if (pokemon.ev[2]>60) evvalue|=0x04;
		//	if (pokemon.ev[3]>60) evvalue|=0x08;
		//	if (pokemon.ev[4]>60) evvalue|=0x10;
		//	if (pokemon.ev[5]>60) evvalue|=0x20;
		//	return new this(pokemon.Species,pokemon.Item,pokemon.nature,
		//		pokemon.moves[0].id,pokemon.moves[1].id,pokemon.moves[2].id,
		//		pokemon.moves[3].id,evvalue);
		//}
		//
		//public void inspect() {
		//	c1=getConstantName(Species,@species);
		//	c2=(@item==0) ? "" : getConstantName(Items,@item);
		//	c3=getConstantName(Natures,@nature);
		//	evlist="";
		//	for (int i = 0; i < @ev; i++) {
		//		if (((@ev&(1<<i))!=0)) {
		//		if (evlist.Length>0) evlist+=",";
		//		evlist+= new []{ "HP","ATK","DEF","SPD","SA","SD" }[i];
		//		}
		//	}
		//	c4=(@move1==0) ? "" : getConstantName(Moves,@move1);
		//	c5=(@move2==0) ? "" : getConstantName(Moves,@move2);
		//	c6=(@move3==0) ? "" : getConstantName(Moves,@move3);
		//	c7=(@move4==0) ? "" : getConstantName(Moves,@move4);
		//	return "#{c1};#{c2};#{c3};#{evlist};#{c4},#{c5},#{c6},#{c7}";
		//}
		//
		//public void tocompact() {
		//	return "#{species},#{item},#{nature},#{move1},#{move2},#{move3},#{move4},#{ev}";
		//}
		//
		//public static void constFromStr(mod,str) {
		//	maxconst=0;
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
		//
		//public static void fromstring(str) {
		//	s=str.split(/\s*,\s*/);
		//	species=this.constFromStr(Species,s[1]);
		//	item=this.constFromStr(Items,s[2]);
		//	nature=this.constFromStr(Natures,s[3]);
		//	move1=this.constFromStr(Moves,s[4]);
		//	move2=(s.Length>=12) ? this.constFromStr(Moves,s[5]) : 0;
		//	move3=(s.Length>=13) ? this.constFromStr(Moves,s[6]) : 0;
		//	move4=(s.Length>=14) ? this.constFromStr(Moves,s[7]) : 0;
		//	ev=0;
		//	slen=s.Length-6;
		//	if (s[slen].to_i>0) ev|=0x01;
		//	if (s[slen+1].to_i>0) ev|=0x02;
		//	if (s[slen+2].to_i>0) ev|=0x04;
		//	if (s[slen+3].to_i>0) ev|=0x08;
		//	if (s[slen+4].to_i>0) ev|=0x10;
		//	if (s[slen+5].to_i>0) ev|=0x20;
		//	return new this(species,item,nature,move1,move2,move3,move4,ev);
		//}
		//
		//public void convertMove(move) {
		//	if (move == Moves.RETURN && hasConst(Moves,:FRUSTRATION)) {
		//		move=Moves.FRUSTRATION;
		//	}
		//	return move;
		//}
		//
		//public void createPokemon(level,iv,trainer) {
		//	pokemon=new PokeBattle_Pokemon(@species,level,trainer,false);
		//	pokemon.setItem(@item);
		//	pokemon.personalID=Core.Rand.Next(65536);
		//	pokemon.personalID|=Core.Rand.Next(65536)<<8;
		//	pokemon.personalID-=pokemon.personalID%25;
		//	pokemon.personalID+=nature;
		//	pokemon.personalID&=0xFFFFFFFF;
		//	pokemon.happiness=0;
		//	pokemon.moves[0]=new Move(this.convertMove(@move1));
		//	pokemon.moves[1]=new Move(this.convertMove(@move2));
		//	pokemon.moves[2]=new Move(this.convertMove(@move3));
		//	pokemon.moves[3]=new Move(this.convertMove(@move4));
		//	evcount=0;
		//	for (int i = 0; i < 6; i++) {
		//		if (((@ev&(1<<i))!=0)) evcount+=1;
		//	}
		//	evperstat=(evcount==0) ? 0 : PokeBattle_Pokemon.EVLIMIT/evcount;
		//	for (int i = 0; i < 6; i++) {
		//		pokemon.iv[i]=iv;
		//		pokemon.ev[i]=((@ev&(1<<i))!=0) ? evperstat : 0;
		//	}
		//	pokemon.calcStats;
		//	return pokemon;
		//}
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
		protected TilePosition start;//					{ get; protected set; }
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
			Character.TrainerMetaData[] btTrainers=null;//GetBTTrainers(BattleChallenge.currentChallenge);
			if (Game.GameData is IGameOrgBattle gob) btTrainers=gob.GetBTTrainers(gob.BattleChallenge.currentChallenge);
			while (@trainers.Count<this.numRounds) {
				int? newtrainer = null;//BattleChallengeTrainer(@wins+@trainers.Count,btTrainers);
				if (Game.GameData is IGameOrgBattle gob2) newtrainer=gob2.BattleChallengeTrainer(@wins+@trainers.Count,btTrainers);
				bool found=false;
				foreach (var tr in @trainers) {
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

		public BattleResults Battle() {
			if (@bc.extraData != null) return @bc.extraData.Battle(this);
			ITrainer opponent=null;//GenerateBattleTrainer(this.nextTrainer,this.rules);
			if (Game.GameData is IGameOrgBattle gob) opponent=gob.GenerateBattleTrainer(this.nextTrainer,this.rules);
			Character.TrainerMetaData[] bttrainers=null; //GetBTTrainers(@id);
			if (Game.GameData is IGameOrgBattle gob1) bttrainers = gob1.GetBTTrainers(@id);
			Character.TrainerMetaData trainerdata=bttrainers[this.nextTrainer];
			BattleResults ret=BattleResults.InProgress;//OrganizedBattleEx(opponent,this.rules,
			if (Game.GameData is IGameOrgBattle gob2) ret=gob2.OrganizedBattleEx(opponent,this.rules,
				trainerdata.ScriptBattleEnd[0],		//GetMessageFromHash(MessageTypes.EndSpeechLose,trainerdata[4]),
				trainerdata.ScriptBattleEnd[1]);	//GetMessageFromHash(MessageTypes.EndSpeechWin,trainerdata[3]));
			return ret;
		}

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
			//if (Game.GameData is IGameOrgBattle gob) @rentals=gob.BattleFactoryPokemon(1,@bcdata.wins,@bcdata.swaps,new IPokemon[0]);
			//@trainerid=@bcdata.nextTrainer;
			//Character.TrainerMetaData[] bttrainers=null;//GetBTTrainers(@bcdata.currentChallenge);
			//if (Game.GameData is IGameOrgBattle gob2) bttrainers=gob2.GetBTTrainers(@bcdata.currentChallenge);
			//Character.TrainerMetaData trainerdata=bttrainers[@trainerid];
			//@opponent=new Trainer( //PokeBattle_Trainer(
			//	trainerdata.ID.ToString(),	//GetMessageFromHash(MessageTypes.TrainerNames,trainerdata[1]),
			//	trainerdata.ID);			//trainerdata[0]);
			//IPokemon[] opponentPkmn=null;//BattleFactoryPokemon(1,@bcdata.wins,@bcdata.swaps,@rentals);
			//if (Game.GameData is IGameOrgBattle gob3) opponentPkmn=gob3.BattleFactoryPokemon(1,@bcdata.wins,@bcdata.swaps,@rentals);
			//@opponent.party=opponentPkmn.Shuffle(0,3);//.shuffle[0,3];
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

		public PokemonUnity.Combat.BattleResults Battle(IBattleChallenge challenge) {
			//Character.TrainerMetaData[] bttrainers=null;//GetBTTrainers(@bcdata.currentChallenge);
			//if (Game.GameData is IGameOrgBattle gob) bttrainers=gob.GetBTTrainers(@bcdata.currentChallenge);
			//Character.TrainerMetaData trainerdata=bttrainers[@trainerid];//Kernal.TrainerMetaData
			//if (Game.GameData is IGameOrgBattle gob1) return gob1.OrganizedBattleEx(@opponent, challenge.rules,
			//	trainerdata.ScriptBattleEnd[0],		//GetMessageFromHash(MessageTypes.EndSpeechLose,trainerdata[4]),
			//	trainerdata.ScriptBattleEnd[1]);    //GetMessageFromHash(MessageTypes.EndSpeechWin,trainerdata[3]));
			return BattleResults.InProgress;
		}

		public void PrepareSwaps() {
			//@oldopponent=@opponent.party;
			//int trainerid=@bcdata.nextTrainer;
			//Character.TrainerMetaData[] bttrainers=null; //GetBTTrainers(@bcdata.currentChallenge);
			//if (Game.GameData is IGameOrgBattle gob) bttrainers=gob.GetBTTrainers(@bcdata.currentChallenge);
			//Character.TrainerMetaData trainerdata=bttrainers[trainerid];
			//@opponent=new Trainer( //PokeBattle_Trainer(
			//	trainerdata.ID.ToString(),	//GetMessageFromHash(MessageTypes.TrainerNames,trainerdata[1]),
			//	trainerdata.ID);			//trainerdata[0]);
			//List<IPokemon> pool=new List<IPokemon>();pool.AddRange(@rentals);pool.AddRange(@oldopponent);
			//IBattleChallenge challenge = null; //ToDo: Not sure how this is assigned... bcdata creates challenge somehow.
			//IPokemon[] opponentPkmn=null;//BattleFactoryPokemon(
			//if (Game.GameData is IGameOrgBattle gob2) opponentPkmn=gob2.BattleFactoryPokemon(
			//	challenge.rules,@bcdata.wins,@bcdata.swaps,
			//	pool.ToArray()); //new IPokemon[0].concat(@rentals).concat(@oldopponent));
			//@opponent.party=opponentPkmn.Shuffle(0,3);//.shuffle[0,3];
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
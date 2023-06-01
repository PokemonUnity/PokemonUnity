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
using System.Collections;

namespace PokemonUnity
{
	/*public partial class Game : PokemonEssentials.Interface.Battle.IGameOrgBattleGenerator
	{
		public Moves RandomMove() {
			do { //;loop
				Moves move = Moves.NONE;
				if (false) {
					move=(Moves)Core.Rand.Next(0xA6)+1;
				} else {
					//move=(Moves)Core.Rand.Next(Moves.maxValue)+1;
					move=(Moves)Core.Rand.Next(Kernal.MoveData.Keys.Count)+1;
					if (move>384 || move == Moves.SKETCH || move == Moves.STRUGGLE) continue;
				}
				//if (move.ToString(TextScripts.Name)!="") return move;
				if (move!=Moves.NONE) return move;
			} while (true);
		}

		public void addMove(ref List<Moves> moves, Moves move, int @base) {
			data=moveData(move);
			int count=@base+1;
			if (data.function==0 && data.basedamage<=40) {
				count=@base;
			}
			if (move == Moves.BUBBLE ||
				move == Moves.BUBBLE_BEAM) {
				count=0;
				return;
			}
			if (data.basedamage<=30 ||
				move == Moves.GROWL ||
				move == Moves.TAIL_WHIP ||
				move == Moves.LEER) {
				count=@base;
			}
			if (data.basedamage>=60 ||
				move == Moves.REFLECT||
				move == Moves.LIGHT_SCREEN ||
				move == Moves.SAFEGUARD ||
				move == Moves.SUBSTITUTE ||
				move == Moves.FAKE_OUT) {
				count=@base+2;
			}
			if (data.basedamage>=80 && data.type == Types.NORMAL) {
				count=@base+5;
			}
			if (data.basedamage>=80 && data.type == Types.NORMAL) {
				count=@base+3;
			}
			if (move == Moves.PROTECT ||
				move == Moves.DETECT ||
				move == Moves.TOXIC ||
				move == Moves.AERIAL_ACE ||
				move == Moves.WILL_O_WISP ||
				move == Moves.SPORE ||
				move == Moves.THUNDER_WAVE ||
				move == Moves.HYPNOSIS ||
				move == Moves.CONFUSE_RAY ||
				move == Moves.ENDURE ||
				move == Moves.SWORDS_DANCE) {
				count=@base+3;
			}
			if (!moves.Contains(move)) {
				//count.times{moves.Add(move)}
				for (int i = 0; i < count; i++) { moves.Add(move); }
			}
		}

		//private static IDictionary tmData          = null;
		//private static IDictionary legalMoves      = [];
		//private static IDictionary legalMovesLevel = 0;
		//private static IDictionary moveData        = [];
		//private static IDictionary baseStatTotal   = [];
		//private static IDictionary minimumLevel    = [];
		//private static IDictionary babySpecies     = [];
		//private static IDictionary evolutions      = [];
		//private static IDictionary tmMoves         = null;

		public IList<Moves> GetLegalMoves2(Pokemons species,int maxlevel) {
			IList<Moves> moves=new List<Moves>();
			if (species==null || species<=0) return moves;
			//RgssOpen("Data/attacksRS.dat","rb") {|atkdata|
			//	offset=atkdata.getOffset(species-1);
			//	length=atkdata.getLength(species-1)>>1;
			//	atkdata.pos=offset;
				for (int k = 0; k < length-1; k++) {
					int level=;//atkdata.fgetw;
					Moves move=;//atkdata.fgetw;
					if (level<=maxlevel) {
						addMove(moves,move,1);
					}
				}
			//}
			//if (tmData == null) $tmData=load_data("Data/tm.dat");
			if (tmMoves == null) {
				tmMoves=new List<>();
				if (itemData==null) itemData=readItemList("Data/items.dat");
				for (int i = 0; i < itemData.Length; i++) {
					if (itemData[i]==null) continue;
					atk=itemData[i][8];
					if (atk==null || atk==0) continue;
					if (tmData[atk]==null) continue;
					tmMoves.Add(atk);
				}
			}
			foreach (var atk in tmMoves) {
				if (tmData[atk].Contains(species)) {
					addMove(moves,atk,0);
				}
			}
			babyspecies=babySpecies(species);
			RgssOpen("Data/eggEmerald.dat","rb"){|f|
				f.pos=(babyspecies-1)*8;
				offset=f.fgetdw;
				length=f.fgetdw;
				if (length>0) {
					f.pos=offset;
					i=0; do { //break; loop
						atk=f.fgetw;
						addMove(moves,atk,2);
						i+=1;
					} unless(i < length);
				}
			}
			movedatas=[];
			foreach (var move in moves) {
				movedatas.Add([move,moveData(move)]);
			}
			//  Delete less powerful moves
			deleteAll=proc{|a,item|
				while (a.Contains(item)) {
					a.delete(item);
				}
			}
			foreach (var move in moves) {
				md=moveData(move);
				foreach (var move2 in movedatas) {
					if (md.function==0xA5 && move2[1].function==0 && md.type==move2[1].type &&
						md.basedamage>=move2[1].basedamage) {
					deleteAll.call(moves,move2[0]);
					} else if (md.function==move2[1].function && md.basedamage==0 &&
						md.accuracy>move2[1].accuracy) {
						//  Supersonic vs. Confuse Ray, etc.
						deleteAll.call(moves,move2[0]);
					} else if (md.function==0x06 && move2[1].function==0x05) {
						deleteAll.call(moves,move2[0]);
					} else if (md.function==move2[1].function && md.basedamage!=0 &&
						md.type==move2[1].type &&
						(md.totalpp==15 || md.totalpp==10 || md.totalpp==move2[1].totalpp) &&
						(md.basedamage>move2[1].basedamage ||
						(md.basedamage==move2[1].basedamage && md.accuracy>move2[1].accuracy))) {
						//  Surf, Flamethrower, Thunderbolt, etc.
						deleteAll.call(moves,move2[0]);
					}
				}
			}
			return moves;
		}

		public void baseStatTotal(move) {
			if (baseStatTotal[move]==null) {
				baseStatTotal[move]=BaseStatTotal(move);
			}
			return baseStatTotal[move];
		}

		public void babySpecies(move) {
			if (babySpecies[move]==null) {
				babySpecies[move]=GetBabySpecies(move);
			}
			return babySpecies[move];
		}

		public void minimumLevel(move) {
			if (minimumLevel[move]==null) {
				minimumLevel[move]=GetMinimumLevel(move);
			}
			return minimumLevel[move];
		}

		public void evolutions(move) {
			if (evolutions[move]==null) {
				evolutions[move]=GetEvolvedFormData(move);
			}
			return evolutions[move];
		}

		public void moveData(Moves move) {
			if (moveData[move]==null) {
				moveData[move]=new MoveData(move);
			}
			return moveData[move];
		}

		/*
		[3/10]
		0-266 - 0-500
		[106]
		267-372 - 380-500
		[95]
		373-467 - 400-555 (nonlegendary)
		468-563 - 400-555 (nonlegendary)
		564-659 - 400-555 (nonlegendary)
		660-755 - 400-555 (nonlegendary)
		756-799 - 580-600 [legendary] (compat1==15 or compat2==15, genderbyte=255)
		800-849 - 500-
		850-881 - 580-
		* /


		public void withRestr(rule,minbs,maxbs,legendary) {
			ret=new PokemonChallengeRules().addPokemonRule(new BaseStatRestriction(minbs,maxbs));
			if (legendary==0) {
				ret.addPokemonRule(new NonlegendaryRestriction());
			} else if (legendary==1) {
				ret.addPokemonRule(new InverseRestriction(new NonlegendaryRestriction()));
			}
			return ret;
		}

		// The Pokemon list is already roughly arranged by rank from weakest to strongest
		public void ArrangeByTier(pokemonlist,rule) {
			tiers=[
					withRestr(rule,0,500,0),
					withRestr(rule,380,500,0),
					withRestr(rule,400,555,0),
					withRestr(rule,400,555,0),
					withRestr(rule,400,555,0),
					withRestr(rule,400,555,0),
					withRestr(rule,580,680,1),
					withRestr(rule,500,680,0),
					withRestr(rule,580,680,2)
			];
			tierPokemon=[];
			for (int i = 0; i < tiers.Length; i++) {
				tierPokemon.Add([]);
			}
			for (int i = 0; i < pokemonlist.Length; i++) {
				if (!rule.ruleset.isPokemonValid(pokemonlist[i])) continue;
				validtiers=[];
				for (int j = 0; j < tiers.Length; j++) {
					tier=tiers[j];
					if (tier.ruleset.isPokemonValid(pokemonlist[i])) {
						validtiers.Add(j);
					}
				}
				if (validtiers.Length>0) {
					vt=validtiers.Length*i/pokemonlist.Length;
					tierPokemon[validtiers[vt]].Add(pokemonlist[i]);
				}
			}
			//  Now for each tier, sort the Pokemon in that tier
			ret=[];
			for (int i = 0; i < tiers.Length; i++) {
				tierPokemon[i].sort!{|a,b|
					bstA=baseStatTotal(a.Species);
					bstB=baseStatTotal(b.Species);
					if (bstA==bstB) {
						a.Species<=>b.Species;
					} else {
						bstA<=>bstB;
					}
				}
				ret.concat(tierPokemon[i]);
			}
			return ret;
		}

		public void hasMorePowerfulMove(moves,thismove) {
			thisdata=moveData(thismove);
			if (thisdata.basedamage==0) return false;
			foreach (var move in moves) {
				if (move==0) continue;
				if (moveData(move).type==thisdata.type &&
					moveData(move).basedamage>thisdata.basedamage) {
					return true;
				}
			}
			return false;
		}

		public void RandomPokemonFromRule(rule,trainer) {
			pkmn=null;
			i=0;
			iteration=-1;
			begin;
			iteration+=1;
			species=0;
			level=rule.ruleset.suggestedLevel;
			do { //;loop
				species=0;
				do { //;loop
				species=Core.Rand.Next(Species.maxValue)+1;
				cname=getConstantName(Species,species) rescue null;
				if (cname) break;
				}
				r=Core.Rand.Next(20);
				bst=baseStatTotal(species);
				if (level<minimumLevel(species)) continue;
				if (iteration%2==0) {
				if (r<16 && bst<400) {
					continue;
				}
				if (r<13 && bst<500) {
					continue;
				}
				} else {
				if (bst>400) {
					continue;
				}
				if (r<10 && babySpecies(species)!=species) {
					continue;
				}
				}
				if (r<10 && babySpecies(species)==species) {
				continue;
				}
				if (r<7 && evolutions(species).Length>0) {
				continue;
				}
				break;
			}
			ev=Core.Rand.Next(0x3F)+1;
			nature=0;
			do { //;loop
				nature=Core.Rand.Next(25);
				nd5=(nature/5).floor; // stat to increase
				nm5=(nature%5).floor; // stat to decrease
				if (nd5==nm5 || nature==Natures.LAX || nature==Natures.GENTLE) {
				//  Neutral nature, Lax, or Gentle
				if (Core.Rand.Next(20)<19) continue;
				} else {
				if (((ev>>(1+nd5))&1)==0) {
					//  If stat to increase isn't emphasized
					if (Core.Rand.Next(10)<6) continue;
				}
				if (((ev>>(1+nm5))&1)!=0) {
					//  If stat to decrease is emphasized
					if (Core.Rand.Next(10)<9) continue;
				}
				}
				break;
			}
			item=0;
			if (level!=$legalMovesLevel) {
				$legalMoves=[];
			}
			if (!$legalMoves[species]) {
				$legalMoves[species]=GetLegalMoves2(species,level);
			}
			itemlist=[
				:ORANBERRY,:SITRUSBERRY,:ADAMANTORB,:BABIRIBERRY,
				:BLACKSLUDGE,:BRIGHTPOWDER,:CHESTOBERRY,:CHOICEBAND,
				:CHOICESCARF,:CHOICESPECS,:CHOPLEBERRY,:DAMPROCK,
				:DEEPSEATOOTH,:EXPERTBELT,:FLAMEORB,:FOCUSSASH,
				:FOCUSBAND,:HEATROCK,:LEFTOVERS,:LIFEORB,:LIGHTBALL,
				:LIGHTCLAY,:LUMBERRY,:OCCABERRY,:PETAYABERRY,:SALACBERRY,
				:SCOPELENS,:SHEDSHELL,:SHELLBELL,:SHUCABERRY,:LIECHIBERRY,
				:SILKSCARF,:THICKCLUB,:TOXICORB,:WIDELENS,:YACHEBERRY,
				:HABANBERRY,:SOULDEW,:PASSHOBERRY,:QUICKCLAW,:WHITEHERB;
			];
			//  Most used: Leftovers, Life Orb, Choice Band, Choice Scarf, Focus Sash
			do { //;loop
				if (Core.Rand.Next(40)==0) {
				item=Items.LEFTOVERS;
				break;
				}
				itemsym=itemlist[Core.Rand.Next(itemlist.Length)];
				item=getID(Items,itemsym);
				if (item==0) continue;
				if (itemsym==:LIGHTBALL) {
				if (!species == Pokemons.PIKACHU) continue;
				}
				if (itemsym==:SHEDSHELL) {
				if (!species == Pokemons.FORRETRESS ||
						!species == Pokemons.SKARMORY) continue;
				}
				if (itemsym==:SOULDEW) {
				if (!species == Pokemons.LATIOS ||
						!species == Pokemons.LATIAS) continue;
				}
				if (itemsym==:LIECHIBERRY && (ev&0x02)==0) {
				if (Core.Rand.Next(2)==0) {
					continue;
				} else {
					ev|=0x02;
				}
				}
				if (itemsym==:FOCUSSASH) {
				if (baseStatTotal(species)>450 && Core.Rand.Next(10)<8) continue;
				}
				if (itemsym==:ADAMANTORB) {
				if (!species == Pokemons.DIALGA) continue;
				}
				if (itemsym==:PASSHOBERRY) {
				if (!species == Pokemons.STEELIX) continue;
				}
				if (itemsym==:BABIRIBERRY ) {
				if (!species == Pokemons.TYRANITAR) continue;
				}
				if (itemsym==:HABANBERRY) {
				if (!species == Pokemons.GARCHOMP) continue;
				}
				if (itemsym==:OCCABERRY) {
				if (!species == Pokemons.METAGROSS) continue;
				}
				if (itemsym==:CHOPLEBERRY) {
				if (!species == Pokemons.UMBREON) continue;
				}
				if (itemsym==:YACHEBERRY) {
				if (!species == Pokemons.TORTERRA &&
						!species == Pokemons.GLISCOR &&
						!species == Pokemons.DRAGONAIR) continue;
				}
				if (itemsym==:SHUCABERRY) {
				if (!species == Pokemons.HEATRAN) continue;
				}
				if (itemsym==:SALACBERRY && (ev&0x08)==0) {
				if (Core.Rand.Next(2)==0) {
					continue;
				} else {
					ev|=0x08;
				}
				}
				if (itemsym==:PETAYABERRY && (ev&0x10)==0) {
				if (Core.Rand.Next(2)==0) {
					continue;
				} else {
					ev|=0x10;
				}
				}
				if (itemsym==(:DEEPSEATOOTH)) {
				if (!species == Pokemons.CLAMPERL) continue;
				}
				if (itemsym==(:THICKCLUB)) {
				if (!species == Pokemons.CUBONE &&
						!species == Pokemons.MAROWAK) continue;
				}
				break;
			}
			if (level<10) {
				if (Core.Rand.Next(40)==0 ||
					item == Items.SITRUSBERRY) item=(Items.ORANBERRY || item);
			}
			if (level>20) {
				if (Core.Rand.Next(40)==0 ||
					item == Items.ORANBERRY) item=(Items.SITRUSBERRY || item);
			}
			moves=$legalMoves[species];
			sketch=false;
			if (moves[0] == Moves.SKETCH) {
				sketch=true;
				moves[0]=RandomMove;
				moves[1]=RandomMove;
				moves[2]=RandomMove;
				moves[3]=RandomMove;
			}
			if (moves.Length==0) continue;
			if ((moves|[]).Length<4) {
				if (moves.Length==0) moves=[Moves.TACKLE];
				moves|=[];
			} else {
				newmoves=[];
				rest=(Moves.REST || -1);
				spitup=(Moves.SPITUP || -1);
				swallow=(Moves.SWALLOW || -1);
				stockpile=(Moves.STOCKPILE || -1);
				snore=(Moves.SNORE || -1);
				sleeptalk=(Moves.SLEEPTALK || -1);
				do { //;loop
				newmoves.clear();
				while (newmoves.Length<4) {
					m=moves[Core.Rand.Next(moves.Length)];
					if (Core.Rand.Next(2)==0 && hasMorePowerfulMove(moves,m)) {
					continue;
					}
					if (!newmoves.Contains(m) && m!=0) {
					newmoves.Add(m);
					}
				}
				if ((newmoves.Contains(spitup) ||
					newmoves.Contains(swallow)) && !newmoves.Contains(stockpile)) {
					unless (sketch) continue;
				}
				if ((!newmoves.Contains(spitup) && !newmoves.Contains(swallow)) &&
					newmoves.Contains(stockpile)) {
					unless (sketch) continue;
				}
				if (newmoves.Contains(sleeptalk) && !newmoves.Contains(rest)) {
					unless ((sketch || !moves.Contains(rest)) && Core.Rand.Next(10)<2) continue;
				}
				if (newmoves.Contains(snore) && !newmoves.Contains(rest)) {
					unless ((sketch || !moves.Contains(rest)) && Core.Rand.Next(10)<2) continue;
				}
				totalbasedamage=0;
				hasPhysical=false;
				hasSpecial=false;
				hasNormal=false;
				foreach (var move in newmoves) {
					d=moveData(move);
					totalbasedamage+=d.basedamage;
					if (d.basedamage>=1) {
					if (d.type == Types.NORMAL) hasNormal=true;
					if (d.category==0) hasPhysical=true;
					if (d.category==1) hasSpecial=true;
					}
				}
				if (!hasPhysical && (ev&0x02)!=0 ) {
					//  No physical attack, but emphasizes Attack
					if (Core.Rand.Next(10)<8) continue;
				}
				if (!hasSpecial && (ev&0x10)!=0) {
					//  No special attack, but emphasizes Special Attack
					if (Core.Rand.Next(10)<8) continue;
				}
				r=Core.Rand.Next(10);
				if (r>6 && totalbasedamage>180) continue;
				if (r>8 && totalbasedamage>140) continue;
				if (totalbasedamage==0 && Core.Rand.Next(20)!=0) continue;
				// ###########
				//  Moves accepted
				if (hasPhysical && !hasSpecial) {
					if (Core.Rand.Next(10)<8) ev&=~0x10;	// Deemphasize Special Attack
					if (Core.Rand.Next(10)<8) ev|=0x02;	// Emphasize Attack
				}
				if (!hasPhysical && hasSpecial) {
					if (Core.Rand.Next(10)<8) ev|=0x10;	// Emphasize Special Attack
					if (Core.Rand.Next(10)<8) ev&=~0x02;	// Deemphasize Attack
				}
				if (!hasNormal && item == Items.SILKSCARF) {
					item=Items.LEFTOVERS;
				}
				moves=newmoves;
				break;
				}
			}
			for (int i = 0; i < 4; i++) {
				if (!moves[i] ) moves[i]=0;
			}
			if (item == Items.LIGHTCLAY &&
				!moves.Contains((Moves.LIGHTSCREEN || -1)) &&
				!moves.Contains((Moves.REFLECT || -1))) {
				item=Items.LEFTOVERS;
			}
			if (item == Items.BLACKSLUDGE) {
				dexdata=OpenDexData;
				DexDataOffset(dexdata,species,8);
				type1=dexdata.fgetb;
				type2=dexdata.fgetb;
				dexdata.close;
				if (!type1 == Types.POISON && !type2 == Types.POISON) {
				item=Items.LEFTOVERS;
				}
			}
			if (item == Items.HEATROCK &&
				!moves.Contains((Moves.SUNNYDAY || -1))) {
				item=Items.LEFTOVERS;
			}
			if (item == Items.DAMPROCK &&
				!moves.Contains((Moves.RAINDANCE || -1))) {
				item=Items.LEFTOVERS;
			}
			if (moves.Contains((Moves.REST || -1))) {
				if (Core.Rand.Next(3)==0) item=Items.LUMBERRY;
				if (Core.Rand.Next(4)==0) item=Items.CHESTOBERRY;
			}
			pk=new Pokemon(species,item,nature,moves[0],moves[1],moves[2],moves[3],ev);
			pkmn=pk.createPokemon(level,31,trainer);
			i+=1;
			} while (!rule.ruleset.isPokemonValid(pkmn));
			return pkmn;
		}

		public void DecideWinnerEffectiveness(move,otype1,otype2,ability,scores) {
			data=moveData(move);
			if (data.basedamage==0) return 0;
			atype=data.type;
			typemod=4;
			if (ability == Abilities.LEVITATE && data.type == Types.GROUND) {
			typemod=4;
			} else {
			mod1=Types.getEffectiveness(atype,otype1);
			mod2=(otype1==otype2) ? 2 : Types.getEffectiveness(atype,otype2);
			if (ability == Abilities.WONDERGUARD) {
				if (mod1!=4) mod1=2;
				if (mod2!=4) mod2=2;
			}
			typemod=mod1*mod2;
			}
			if (typemod==0) return scores[0];
			if (typemod==1) return scores[1];
			if (typemod==2) return scores[2];
			if (typemod==4) return scores[3];
			if (typemod==8) return scores[4];
			if (typemod==16) return scores[5];
			return 0;
		}

		public void DecideWinnerScore(party0,party1,rating) {
			score=0;
			types1=[];
			types2=[];
			abilities=[];
			for (int j = 0; j < party1.Length; j++) {
			types1.Add(party1[j].type1);
			types2.Add(party1[j].type2);
			abilities.Add(party1[j].ability);
			}
			for (int i = 0; i < party0.Length; i++) {
			foreach (var move in party0[i].moves) {
				if (move.id==0) continue;
				for (int j = 0; j < party1.Length; j++) {
				score+=DecideWinnerEffectiveness(move.id,
					types1[j],types2[j],abilities[j],[-16,-8,0,4,12,20]);
				}
			}
			basestatsum=baseStatTotal(party0[i].Species);
			score+=basestatsum/10;
			if (party0[i].Item!=0) score+=10;	// Not in Battle Dome ranking
			}
			score+=rating+Core.Rand.Next(32);
			return score;
		}

		public void DecideWinner(party0,party1,rating0,rating1) {
			rating0=(rating0*15.0/100).round;
			rating1=(rating1*15.0/100).round;
			score0=DecideWinnerScore(party0,party1,rating0);
			score1=DecideWinnerScore(party1,party0,rating1);
			if (score0==score1) {
			if (rating0==rating1) return 5;
			return (rating0>rating1) ? 1 : 2;
			} else {
			return (score0>score1) ? 1 : 2;
			}
		}

		public void RuledBattle(team1,team2,rule) {
			decision=0;
			if (Core.Rand.Next(100)!=0) {
			party1=[];
			party2=[];
			team1.Length.times {|i| party1.Add(team1[i]) }
			team2.Length.times {|i| party2.Add(team2[i]) }
			decision=DecideWinner(party1,party2,team1.rating,team2.rating);
			} else {
			scene=new PokeBattle_DebugSceneNoLogging();
			trainer1=new PokeBattle_Trainer("PLAYER1",1);
			trainer2=new PokeBattle_Trainer("PLAYER2",1);
			items1=[];
			items2=[];
			level=rule.ruleset.suggestedLevel;
			team1.Length.times {|i|
				p=team1[i];
				if (p.level!=level) {
					p.level=level;
					p.calcStats;
				}
				items1.Add(p.Item);
				trainer1.party.Add(p);
			}
			team2.Length.times {|i|
				p=team2[i];
				if (p.level!=level) {
					p.level=level;
					p.calcStats;
				}
				items2.Add(p.Item);
				trainer2.party.Add(p);
			}
			battle=rule.createBattle(scene,trainer1,trainer2);
			battle.debug=true;
			battle.controlPlayer=true;
			battle.endspeech="...";
			battle.internalbattle=false;
			decision=battle.StartBattle;
			// p [items1,items2]
			team1.Length.times {|i|
				p=team1[i];
				p.heal;
				p.setItem(items1[i]);
			}
			team2.Length.times {|i|
				p=team2[i];
				p.heal;
				p.setItem(items2[i]);
			}
			}
			if (decision==1) {		// Team 1 wins
			team1.addMatch(team2,1);
			team2.addMatch(team1,0);
			} else if (decision==2) {		// Team 2 wins
			team1.addMatch(team2,0);
			team2.addMatch(team1,1);
			} else {
			team1.addMatch(team2,-1);
			team2.addMatch(team1,-1);
			}
		}

		public void getTypes(species) {
			dexdata=OpenDexData;
			DexDataOffset(dexdata,species,8);
			type1=dexdata.fgetb;
			type2=dexdata.fgetb;
			dexdata.close;
			return type1==type2 ? [type1] : [type1,type2];
		}

		public void TrainerInfo(pokemonlist,trfile,rules) {
			bttrainers=GetBTTrainers(trfile);
			btpokemon=GetBTPokemon(trfile);
			trainertypes=load_data("Data/trainertypes.dat");
			if (bttrainers.Length==0) {
			for (int i = 0; i < 200; i++) {
				if (block_given? && i%50==0) yield(null);
				trainerid=0;
				money=30;
				do { //;loop
				trainerid=Core.Rand.Next(Trainers.maxValue)+1;
				if (Core.Rand.Next(30)==0) trainerid=getID(Trainers,:YOUNGSTER);
				if (trainerid.ToString(TextScripts.Name)=="") continue;
				money=(!trainertypes[trainerid] ||
						!trainertypes[trainerid][3]) ? 30 : trainertypes[trainerid][3];
				if (money>=100) continue;
				break;
				}
				gender=(!trainertypes[trainerid] ||
						!trainertypes[trainerid][7]) ? 2 : trainertypes[trainerid][7];
				randomName=getRandomNameEx(gender,null,0,12);
				tr=[trainerid,randomName,_INTL("Here I come!"),
					_INTL("Yes, I won!"),_INTL("Man, I lost!"),[]];
				bttrainers.Add(tr);
			}
			bttrainers.sort!{|a,b|
				money1=(!trainertypes[a[0]] ||
						!trainertypes[a[0]][3]) ? 30 : trainertypes[a[0]][3];
				money2=(!trainertypes[b[0]] ||
						!trainertypes[b[0]][3]) ? 30 : trainertypes[b[0]][3];
				money1==money2 ? a[0]<=>b[0] : money1<=>money2;
			}
			}
			if (block_given?) yield(null);
			suggestedLevel=rules.ruleset.suggestedLevel;
			rulesetTeam=rules.ruleset.copy.clearPokemonRules;
			pkmntypes=[];
			validities=[];
			t=new Time();
			foreach (var pkmn in pokemonlist) {
			if (pkmn.level!=suggestedLevel) pkmn.level=suggestedLevel;
			pkmntypes.Add(getTypes(pkmn.Species));
			validities.Add(rules.ruleset.isPokemonValid(pkmn));
			}
			newbttrainers=[];
			for (int btt = 0; btt < bttrainers.Length; btt++) {
			if (block_given? && btt%50==0) yield(null);
			trainerdata=bttrainers[btt];
			pokemonnumbers=trainerdata[5] || [];
			species=[];
			types=[];
			// p trainerdata[1]
			(Types.maxValue+1).times {|typ| types[typ]=0 }
			foreach (var pn in pokemonnumbers) {
				pkmn=btpokemon[pn];
				species.Add(pkmn.Species);
				t=getTypes(pkmn.Species);
				foreach (var typ in t) {
					types[typ]+=1;
				}
			}
			species|=[]; // remove duplicates
			count=0;
			(Types.maxValue+1).times {|typ|
				if (types[typ]>=5) {
					types[typ]/=4;
					if (types[typ]>10) types[typ]=10;
				} else {
					types[typ]=0;
				}
				count+=types[typ];
			}
			if (count==0) types[0]=1;
			if (pokemonnumbers.Length==0) {
				int typ = 0; do {|typ|
					types[typ]=1;
				} while (typ < ); //(Types.maxValue+1).times
			}
			numbers=[];
			if (pokemonlist) {
				numbersPokemon=[];
				//  p species
				for (int index = 0; index < pokemonlist.Length; index++) {
				pkmn=pokemonlist[index];
				if (!validities[index]) continue;
				absDiff=((index*8/pokemonlist.Length)-(btt*8/bttrainers.Length)).abs;
				sameDiff=(absDiff==0);
				if (species.Contains(pkmn.Species)) {
					weight= new []{ 32,12,5,2,1,0,0,0 }[[absDiff,7].min];
					if (Core.Rand.Next(40)<weight) {
					numbers.Add(index);
					numbersPokemon.Add(pokemonlist[index]);
					}
				} else {
					t=pkmntypes[index];
					foreach (var typ in t) {
						weight= new []{ 32,12,5,2,1,0,0,0 }[[absDiff,7].min];
						weight*=types[typ];
						if (Core.Rand.Next(40)<weight) {
						numbers.Add(index);
						numbersPokemon.Add(pokemonlist[index]);
						}
					}
				}
				}
				numbers|=[];
				if ((numbers.Length<6 ||
					!rulesetTeam.hasValidTeam(numbersPokemon))) {
				for (int index = 0; index < pokemonlist.Length; index++) {
					pkmn=pokemonlist[index];
					if (!validities[index]) continue;
					if (species.Contains(pkmn.Species)) {
					numbers.Add(index);
					numbersPokemon.Add(pokemonlist[index]);
					} else {
					t=pkmntypes[index];
					foreach (var typ in t) {
						if (types[typ]>0 && !numbers.Contains(index)) {
							numbers.Add(index);
							numbersPokemon.Add(pokemonlist[index]);
							break;
						}
					}
					}
					if (numbers.Length>=6 && rules.ruleset.hasValidTeam(numbersPokemon)) break;
				}
				if (numbers.Length<6 || !rules.ruleset.hasValidTeam(numbersPokemon)) {
					while (numbers.Length<pokemonlist.Length &&
						(numbers.Length<6 || !rules.ruleset.hasValidTeam(numbersPokemon))) {
					index=Core.Rand.Next(pokemonlist.Length);
					if (!numbers.Contains(index)) {
						numbers.Add(index);
						numbersPokemon.Add(pokemonlist[index]);
					}
					}
				}
				}
				numbers.sort!;
			}
			newbttrainers.Add([trainerdata[0],trainerdata[1],trainerdata[2],
								trainerdata[3],trainerdata[4],numbers])  ;
			}
			if (block_given?) yield(null);
			pokemonlist=[];
			foreach (var pkmn in pokemonlist) {
				pokemonlist.Add(Pokemon.fromPokemon(pkmn));
			}
			trlists=(load_data("Data/trainerlists.dat") rescue []);
			hasDefault=false;
			trIndex=-1;
			for (int i = 0; i < trlists.Length; i++) {
				if (trlists[i][5]) hasDefault=true;
			}
			for (int i = 0; i < trlists.Length; i++) {
				if (trlists[i][2].Contains(trfile)) {
					trIndex=i;
					trlists[i][0]=newbttrainers;
					trlists[i][1]=pokemonlist;
					trlists[i][5]=!hasDefault;
				}
			}
			if (block_given?) yield(null);
			if (trIndex<0) {
				info=[newbttrainers,pokemonlist,[trfile],
						trfile+"tr.txt",trfile+"pm.txt",!hasDefault];
				trlists.Add(info);
			}
			if (block_given?) yield(null);
			save_data(trlists,"Data/trainerlists.dat");
			if (block_given?) yield(null);
			SaveTrainerLists();
			if (block_given?) yield(null);
		}



		//if $FAKERGSS;
		//	public void Kernel.MessageDisplay(mw,txt,lbl) {
		//		puts txt;
		//	}
		//
		//	public void _INTL(*arg) {
		//		return arg[0];
		//	}
		//
		//	public void string.Format(*arg) {
		//		return arg[0];
		//	}
		//}



		public void isBattlePokemonDuplicate(pk,pk2) {
			if (pk.Species==pk2.Species) {
			moves1=[];
			moves2=[];
			4.times{
				moves1.Add(pk.moves[0].id);
				moves2.Add(pk.moves[1].id);
			}
			moves1.sort!;
			moves2.sort!;
			if (moves1[0]==moves2[0] &&
				moves1[1]==moves2[1] &&
				moves1[2]==moves2[2] &&
				moves1[3]==moves2[3]) {
				//  Accept as same if moves are same and there are four moves each
				if (moves1[3]!=0) return true;
			}
			if (pk.Item==pk2.Item &&
							pk.nature==pk2.nature &&
							pk.ev[0]==pk2.ev[0] &&
							pk.ev[1]==pk2.ev[1] &&
							pk.ev[2]==pk2.ev[2] &&
							pk.ev[3]==pk2.ev[3] &&
							pk.ev[4]==pk2.ev[4] &&
							pk.ev[5]==pk2.ev[5]) return true;
				return false;
			}
		}

		public void RemoveDuplicates(party) {
		// p "before: #{party.Length}"
			ret=[];
			foreach (var pk in party) {
			found=false;
			count=0;
			firstIndex=-1;
			for (int i = 0; i < ret.Length; i++) {
				pk2=ret[i];
				if (isBattlePokemonDuplicate(pk,pk2)) {
				found=true; break;
				}
				if (pk.Species==pk2.Species) {
				if (count==0) firstIndex=i;
				count+=1;
				}
			}
			if (!found) {
				if (count>=10) {
				ret.delete_at(firstIndex);
				}
				ret.Add(pk);
			}
			}
			return ret;
		}

		public void ReplenishBattlePokemon(party,rule) {
			while (party.Length<20) {
			pkmn=RandomPokemonFromRule(rule,null);
			found=false;
			foreach (var pk in party) {
				if (isBattlePokemonDuplicate(pkmn,pk)) {
				found=true; break;
				}
			}
			if (!found) party.Add(pkmn);
			}
		}

		public void GenerateChallenge(rule,tag) {
			oldrule=rule;
			yield(_INTL("Preparing to generate teams"));
			rule=rule.copy.setNumber(2);
			yield(null);
			party=load_data(tag+".rxdata") rescue [];
			teams=load_data(tag+"teams.rxdata") rescue [];
			if (teams.Length<10) {
			btpokemon=GetBTPokemon(tag);
			if (btpokemon && btpokemon.Length!=0) {
				suggestedLevel=rule.ruleset.suggestedLevel;
				foreach (var pk in btpokemon) {
				pkmn=pk.createPokemon(suggestedLevel,31,null);
				if (rule.ruleset.isPokemonValid(pkmn)) party.Add(pkmn);
				}
			}
			}
			yield(null);
			party=RemoveDuplicates(party);
			yield(null);
			maxteams=600;
			cutoffrating=65;
			toolowrating=40;
			iterations=11;
			iterations.times do |iter|
			save_data(party,tag+".rxdata");
			yield(_INTL("Generating teams ({1} of {2})",iter+1,iterations));
			i=0;while i<teams.Length;
				if (i%10==0) yield(null);
				ReplenishBattlePokemon(party,rule);
				if (teams[i].rating<cutoffrating && teams[i].totalGames>=80) {
				teams[i]=new RuledTeam(party,rule);
				} else if (teams[i].Length<2) {
				teams[i]=new RuledTeam(party,rule);
				} else if (i>=maxteams) {
				teams[i]=null;
				teams.compact!;
				} else if (teams[i].totalGames>=250) {
		//  retire
				for (int j = 0; j < teams[i].Length; j++) {
					party.Add(teams[i][j]);
				}
				teams[i]=new RuledTeam(party,rule);
				} else if (teams[i].rating<toolowrating) {
				teams[i]=new RuledTeam(party,rule);
				}
				i+=1;
			}
			save_data(teams,tag+"teams.rxdata");
			yield(null);
			while (teams.Length<maxteams) {
				if (teams.Length%10==0) yield(null);
				ReplenishBattlePokemon(party,rule);
				teams.Add(new RuledTeam(party,rule));
			}
			save_data(party,tag+".rxdata");
			teams=teams.sort{|a,b| b.rating<=>a.rating }
			yield(_INTL("Simulating battles ({1} of {2})",iter+1,iterations));
			i=0; loop do;
				changed=false;
				teams.Length.times {|j|
					yield(null);
					other=j;5.times do;
					other=Core.Rand.Next(teams.Length);
					if (other==j) continue;
					}
					if (other==j) continue;
					changed=true;
					RuledBattle(teams[j],teams[other],rule);
				}
		//  i+=1;break if i>=5
				i+=1;
				gameCount=0;
				foreach (var team in teams) {
				gameCount+=team.games;
				}
		// p [gameCount,teams.Length,gameCount/teams.Length]
				yield(null);
				if ((gameCount/teams.Length)>=12) {
		// p "Iterations: #{i}"
				foreach (var team in teams) {
					games=team.games;
					team.updateRating;
		// p [games,team.totalGames,team.ratingRaw] if $INTERNAL
				}
		// p [gameCount,teams.Length,gameCount/teams.Length]
				break;
				}
			}
			teams.sort!{|a,b| b.rating<=>a.rating }
			save_data(teams,tag+"teams.rxdata");
			}
			party=[];
			yield(null);
			teams.sort{|a,b| a.rating<=>b.rating }
			foreach (var team in teams) {
			if (team.rating>cutoffrating) {
				for (int i = 0; i < team.Length; i++) {
				party.Add(team[i]);
				}
			}
			}
			rule=oldrule;
			yield(null);
			party=RemoveDuplicates(party);
			yield(_INTL("Writing results"));
			party=ArrangeByTier(party,rule);
			yield(null);
			TrainerInfo(party,tag,rule) { yield(null) }
			yield(null);
		}

		public void WriteCup(id,rules) {
			if (!Core.DEBUG) return;
			bttrainers=[];
			trlists=(load_data("Data/trainerlists.dat") rescue []);
			list=[];
			for (int i = 0; i < trlists.Length; i++) {
			tr=trlists[i];
			if (tr[5]) {
				list.Add("*"+(tr[3].sub(/\.txt$/,"")));
			} else {
				list.Add((tr[3].sub(/\.txt$/,"")));
			}
			}
			cmd=0;
			if (trlists.Length!=0) {
			cmd=Kernel.Message(_INTL("Generate Pokemon teams for this challenge?"),
				[_INTL("NO"),_INTL("YES, USE EXISTING"),_INTL("YES, USE NEW")],1);
			} else {
			cmd=Kernel.Message(_INTL("Generate Pokemon teams for this challenge?"),
				[_INTL("YES"),_INTL("NO")],2);
			if (cmd==0) {
				cmd=2;
			} else if (cmd==1) {
				cmd=0;
			}
			}
			if (cmd==0  ) return;	// No
			if (cmd==1  ) {		// Yes, use existing
			cmd=Kernel.Message(_INTL("Choose a challenge."),list,-1);
			if (cmd>=0) {
				Kernel.Message(_INTL("This challenge will use the Pokemon list from {1}.",list[cmd]));
				for (int i = 0; i < trlists.Length; i++) {
				tr=trlists[i];
				while (!tr[5] && tr[2].Contains(id)) {
					tr[2].delete(id);
				}
				}
				if (!trlists[cmd][5]) {
				trlists[cmd][2].Add(id);
				}
				save_data(trlists,"Data/trainerlists.dat");
				Graphics.update();
				SaveTrainerLists();
				Graphics.update();
				return;
			} else {
				return;
			}
			//  Yes, use new
			} else if (cmd==2 && !Kernel.ConfirmMessage(_INTL("This may take a long time. Are you sure?"))) {
			return;
			}
			mw=Kernel.CreateMessageWindow;
			t=Time.now;
			GenerateChallenge(rules,id){|message|
				if ((Time.now-t)>=5) {
				Graphics.update(); t=Time.now;
				}
				if (message) {
				Kernel.MessageDisplay(mw,message,false);
				Graphics.update(); t=Time.now;
				}
			}
			Kernel.DisposeMessageWindow(mw);
			Kernel.Message(_INTL("Team generation complete."));
		}
	}*/

	public partial class BaseStatRestriction : PokemonEssentials.Interface.Battle.IBaseStatRestriction {
		protected int mn;
		protected int mx;

		public BaseStatRestriction(int mn,int mx) {
			initialize(mn, mx);
		}

		public IBaseStatRestriction initialize(int mn,int mx) {
			this.mn=mn;this.mx=mx;
			return this;
		}

		public bool isValid (IPokemon pkmn) {
			int bst=0;//baseStatTotal(pkmn.Species);
			if (Game.GameData is IGameOrgBattleGenerator gobg) bst=gobg.baseStatTotal(pkmn.Species);
			return bst>=@mn && bst<=@mx;
		}
	}

	public partial class NonlegendaryRestriction : PokemonEssentials.Interface.Battle.INonlegendaryRestriction {
		public bool isValid (IPokemon pkmn) {
			//dexdata=OpenDexData();
			//DexDataOffset(dexdata,pkmn.Species,31);
			PokemonUnity.Monster.Data.PokemonData dexdata = Kernal.PokemonData[pkmn.Species];
			EggGroups compat10=dexdata.EggGroup[0]; //dexdata.fgetb;
			EggGroups compat11=dexdata.EggGroup[1]; //dexdata.fgetb;
			//DexDataOffset(dexdata,pkmn.Species,18);
			//int genderbyte=dexdata.fgetb;
			GenderRatio genderbyte=dexdata.GenderEnum;
			//dexdata.close;
			return (compat10 != EggGroups.UNDISCOVERED &&
					compat11 != EggGroups.UNDISCOVERED) ||
					genderbyte!=GenderRatio.Genderless; //genderbyte!=255;
		}
	}

	public partial class InverseRestriction : PokemonEssentials.Interface.Battle.IInverseRestriction {
		protected IBattleRestriction r;

		public InverseRestriction(IBattleRestriction r) {
			initialize(r);
		}

		public IBattleRestriction initialize(IBattleRestriction r) {
			this.r=r;
			return this;
		}

		public bool isValid (IPokemon pkmn) {
			return !@r.isValid(pkmn);
		}
	}

	public partial class SingleMatch : PokemonEssentials.Interface.Battle.ISingleMatch {
		public float opponentRating				{ get; protected set; }
		public float opponentDeviation				{ get; protected set; }
		public int score				{ get; protected set; }
		public int kValue				{ get; protected set; }

		public SingleMatch(float opponentRating, float opponentDev, int score, int kValue=16) {
			initialize(opponentRating, opponentDev, score, kValue);
		}

		public ISingleMatch initialize(float opponentRating, float opponentDev, int score, int kValue=16) {
			this.opponentRating=opponentRating;
			this.opponentDeviation=opponentDev;
			this.score= score; // -1=draw, 0=lose, 1=win
			this.kValue = kValue;
			return this;
		}
	}

	public partial class MatchHistory : PokemonEssentials.Interface.Battle.IMatchHistory, IEnumerable<ISingleMatch> {
		//include Enumerable;
		protected List<ISingleMatch> matches;//				{ get; protected set; }
		protected IPlayerRating thisPlayer;//				{ get; protected set; }

		public IEnumerable<ISingleMatch> each() {
			foreach (var item in @matches) { yield return item; }
		}

		public int length { get {
			return @matches.Count;
		} }

		public ISingleMatch this[int i] { get {
			return @matches[i];
		} }

		public MatchHistory(IPlayerRating thisPlayer) {
			initialize(thisPlayer);
		}

		public IMatchHistory initialize(IPlayerRating thisPlayer) {
			this.matches=new List<ISingleMatch>();
			this.thisPlayer=thisPlayer;
			return this;
		}

		public void addMatch(IPlayerRating otherPlayer, int result) {
			//  1=I won; 0=Other player won; -1: Draw
			@matches.Add(new SingleMatch(
				otherPlayer.rating,otherPlayer.deviation,result));
		}

		public void updateAndClear() {
			@thisPlayer.update(@matches);
			@matches.Clear();
		}

		public IEnumerator<ISingleMatch> GetEnumerator()
		{
			return ((IEnumerable<ISingleMatch>)matches).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)matches).GetEnumerator();
		}
	}

	public partial class PlayerRatingElo : PlayerRating, PokemonEssentials.Interface.Battle.IPlayerRatingElo {
		//public float rating				{ get; protected set; }
		const int K_VALUE = 16;

		public PlayerRatingElo() {
			initialize();
		}

		new public IPlayerRatingElo initialize() {
			@rating=1600.0f;
			@deviation=0;
			@volatility=0;
			@estimatedRating=null;
			return this;
		}

		//public float winChancePercent() {
		public override float winChancePercent { get {
			if (@estimatedRating != null) return @estimatedRating.Value;
			float x=1+(float)Math.Pow(10.0f,(@rating-1600.0f)/400.0f);
			@estimatedRating=x==0 ? 1.0f : 1.0f/x;
			return @estimatedRating.Value;
		} }

		public void update(IList<ISingleMatch> matches) {
			if (matches.Count == 0) {
				return;
			}
			double stake=0;
			//for (int i = 0; i < matches.Length; i++) {
			foreach (ISingleMatch match in matches) {
				float score=(match.score==-1) ? 0.5f : match.score;
				double e=1+Math.Pow(10.0f,(@rating-match.opponentRating)/400.0f);
				stake+=match.kValue*(score-e);
			}
			@rating+=(float)stake;
		}
	}

	public partial class PlayerRating : PokemonEssentials.Interface.Battle.IPlayerRating {
		public float volatility				{ get; protected set; }
		public float deviation				{ get; protected set; }
		public float rating					{ get; protected set; }
		public float? estimatedRating;//					{ get; protected set; }

		public PlayerRating() {
			initialize();
		}

		public virtual IPlayerRating initialize() {
			@rating=1500.0f;
			@deviation=350.0f;
			@volatility=0.9f;
			@estimatedRating=null;
			return this;
		}

		public virtual float winChancePercent { get {
			if (@estimatedRating != null) return @estimatedRating.Value;
			if (@deviation > 100) {
				//  http://www.smogon.com/forums/showthread.php?t=55764
				float otherRating=1500.0f;
				float otherDeviation=350.0f;
				float s=(float)Math.Sqrt(100000.0f+@deviation*@deviation+otherDeviation*otherDeviation);
				float g=(float)Math.Pow(10.0f,(otherRating-@rating)*0.79f/s);
				@estimatedRating=(1.0f/(1.0f+g))*100.0f; // Percent chance that I win against opponent
			} else {
				//  GLIXARE method
				float rds = @deviation * @deviation;
				float sqr = (float)Math.Sqrt(15.905694331435d * (rds + 221781.21786254d));
				double inner = (1500.0f - @rating) * Math.PI / sqr;
				@estimatedRating=(10000.0f / (1.0f + (float)Math.Pow(10.0f,inner)) + 0.5f) / 100.0f;
			}
			return @estimatedRating.Value;
		} }

		public void update(IList<ISingleMatch> matches,float system=1.2f) {
			volatility = volatility2;
			deviation = deviation2;
			rating = rating2;
			if (matches.Count == 0) {
				setDeviation2((float)Math.Sqrt(deviation * deviation + volatility * volatility));
				return;
			}
			List<float> g=new List<float>();
			List<float> e=new List<float>();
			List<float> score=new List<float>();
			for (int i = 0; i < matches.Count; i++) {
				ISingleMatch match = matches[i];
				g[i] = (float)getGFactor(match.opponentDeviation);
				e[i] = (float)getEFactor(rating,match.opponentRating, g[i]);
				score[i] = match.score;
			}
			//  Estimated variance
			float variance = 0.0f;
			for (int i = 0; i < matches.Count; i++) {
				variance += g[i]*g[i]*e[i]*(1-e[i]);
			}
			variance=1.0f/variance;
			//  Improvement sum
			double sum = 0.0d;
			for (int i = 0; i < matches.Count; i++) {
					double v = score[i];
					if ((v != -1)) {
					sum += g[i]*(((float)v)-e[i]);
				}
			}
			volatility = getUpdatedVolatility(volatility,deviation,variance,(float)sum,system);
			//  Update deviation
			float t = deviation * deviation + volatility * volatility;
			deviation = 1.0f / (float)Math.Sqrt(1.0f / t + 1.0f / variance);
			//  Update rating
			rating = rating + deviation * deviation * (float)sum;
			setRating2(rating);
			setDeviation2(deviation);
			setVolatility2(volatility);
		}

		//private;

		//private float volatility				{ get; protected set; }

		private float rating2 { get {
			return (@rating-1500.0f)/173.7178f;
		} }

		private float deviation2 { get {
			return (@deviation)/173.7178f;
		} }

		private double getGFactor(float deviation) {
			//  deviation is not yet in glicko2
			deviation/=173.7178f;
			return 1.0d / Math.Sqrt(1.0d + (3.0d*deviation*deviation) / (Math.PI*Math.PI));
		}

		private double getEFactor(float rating,float opponentRating, float g) {
			//  rating is already in glicko2
			//  opponentRating is not yet in glicko2
			opponentRating=(opponentRating-1500.0f)/173.7178f;
			return 1.0f / (1.0f + Math.Exp(-g * (rating - opponentRating)));
		}

		//alias volatility2 volatility;
		private float volatility2				{ get { return volatility; } }

		private void setVolatility2(float value) {
			@volatility=value;
		}

		private void setRating2(float value) {
			@estimatedRating=null;
			@rating=(value*173.7178f)+1500.0f;
		}

		private void setDeviation2(float value) {
			@estimatedRating=null;
			@deviation=(value*173.7178f);
		}

		private float getUpdatedVolatility(float volatility, float deviation, float variance, float improvementSum, float system) {
			double improvement = improvementSum * variance;
			double a = Math.Log(volatility * volatility);
			double squSystem = system * system;
			double squDeviation = deviation * deviation;
			double squVariance = variance + variance;
			double squDevplusVar = squDeviation + variance;
			double x0 = a;
			int n = 0; do { // Up to 100 iterations to avoid potentially infinite loops
				double e = Math.Exp(x0);
				double d = squDevplusVar + e;
				double squD = d * d;
				double i = improvement / d;
				double h1 = -(x0 - a) / squSystem - 0.5 * e * i * i;
				double h2 = -1.0 / squSystem - 0.5 * e * squDevplusVar / squD;
				h2 += 0.5 * squVariance * e * (squDevplusVar - e) / (squD * d);
				double x1 = x0;
				x0 -= h1 / h2;
				if (Math.Abs(x1 - x0) < 0.000001) {
					break;
				}
			} while (n < 100); //100.times
			return (float)Math.Exp(x0 / 2.0f);
		}
	}

	public partial class RuledTeam : PokemonEssentials.Interface.Battle.IRuledTeam {
		protected IMatchHistory @history;
		protected IPlayerRating _rating;
		protected int? _totalGames;

		public float rating { get {
			return _rating.winChancePercent;
		} }

		public float[] ratingRaw { get {
			return new float[] { _rating.rating, _rating.deviation, _rating.volatility, _rating.winChancePercent };
		} }

		public IPlayerRating ratingData { get {
			return _rating;
		} }

		public int totalGames { get {
				return (_totalGames ?? 0) + this.games;
		} }

		public void updateRating() {
			if (_totalGames == null) _totalGames=0;
			int oldgames=this.games;
			@history.updateAndClear();
			int newgames=this.games;
			_totalGames+=(oldgames-newgames);
		}

		public bool compare(IRuledTeam other) {
			//return _rating.compare(other.ratingData);  //ToDo: Uncomment...
			return false;
		}

		public void addMatch(IRuledTeam other,int score) {
			@history.addMatch(other.ratingData,score);
		}

		//public void addMatch(IRuledTeam other,bool? score) {
		//	@history.addMatch(other.ratingData,score);
		//}

		public int games { get {
			return @history.length;
		} }

		public IList<IPokemon> team				{ get; protected set; }

		public RuledTeam(IList<IPokemon> party, IPokemonChallengeRules rule) {
			initialize(party, rule);
		}

		public IRuledTeam initialize(IList<IPokemon> party, IPokemonChallengeRules rule) {
			int count=rule.ruleset.suggestedNumber;
			@team=new List<IPokemon>();
			List<int> retnum=new List<int>();
			do { //;loop
				for (int i = 0; i < count; i++) {
					retnum[i] = Core.Rand.Next(party.Count);
					@team[i] = party[retnum[i]];
					party.RemoveAt(retnum[i]);
				}
				if (rule.ruleset.isValid(@team.ToArray())) break;
			} while (true);
			_totalGames=0;
			_rating=new PlayerRating();
			@history=new MatchHistory(_rating);
			return this;
		}

		public IPokemon this[int i] { get {
			return @team[i];
		} }

		public override string ToString() {
			return "["+((int)rating).ToString()+","+((int)@games).ToString()+"]";
		}

		public int length { get {
			return @team.Count;
		} }

		public IList<IPokemon> load(IList<IPokemon> party) {
			IList<IPokemon> ret = new List<IPokemon>();
			for (int i = 0; i < team.Count; i++) {
				//ret.Add(party[team[i]]); //ToDo: Uncomment...
			}
			return ret;
		}
	}
}
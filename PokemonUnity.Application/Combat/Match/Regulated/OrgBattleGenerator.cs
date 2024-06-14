using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Monster.Data;
using PokemonUnity.Inventory;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using System.Collections;
using PokemonUnity.Character;

namespace PokemonUnity
{
	public partial class Game : PokemonEssentials.Interface.Battle.IGameOrgBattleGenerator
	{
		public Moves RandomMove() {
			do { //;loop
				Moves move = Moves.NONE;
				if (false) {
					move=(Moves)Core.Rand.Next(0xA6)+1;
				} else {
					//species=(Moves)Core.Rand.Next(Moves.maxValue)+1;
					move=(Moves)Core.Rand.Next(Kernal.MoveData.Keys.Count)+1;
					if (move>(Moves)384 || move == Moves.SKETCH || move == Moves.STRUGGLE) continue;
				}
				//if (species.ToString(TextScripts.Name)!="") return species;
				if (move!=Moves.NONE) return move;
			} while (true);
		}

		public void addMove(IList<Moves> moves, Moves move, int @base) {
			Attack.Data.MoveData data=moveData(move);
			int count=@base+1;
			if (data.Effect==0 && data.Power<=40) {
				count=@base;
			}
			if (move == Moves.BUBBLE ||
				move == Moves.BUBBLE_BEAM) {
				count=0;
				return;
			}
			if (data.Power<=30 ||
				move == Moves.GROWL ||
				move == Moves.TAIL_WHIP ||
				move == Moves.LEER) {
				count=@base;
			}
			if (data.Power>=60 ||
				move == Moves.REFLECT||
				move == Moves.LIGHT_SCREEN ||
				move == Moves.SAFEGUARD ||
				move == Moves.SUBSTITUTE ||
				move == Moves.FAKE_OUT) {
				count=@base+2;
			}
			if (data.Power>=80 && data.Type == Types.NORMAL) {
				count=@base+5;
			}
			if (data.Power>=80 && data.Type == Types.NORMAL) {
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
				//count.times{moves.Add(species)}
				for (int i = 0; i < count; i++) { moves.Add(move); }
			}
		}

		private static int										legalMovesLevel	= 0;
		private static IDictionary<Pokemons, IList<Moves>>		legalMoves		= new Dictionary<Pokemons, IList<Moves>>();
		//private static IDictionary							tmData			= null;
		//private static IDictionary							moveData		= new List<>();
		private static IDictionary<Pokemons,int>				_baseStatTotal	= new Dictionary<Pokemons, int>();
		private static IDictionary<Pokemons,int>				_minimumLevel	= new Dictionary<Pokemons,int>();
		private static IDictionary<Pokemons,Pokemons>			_babySpecies	= new Dictionary<Pokemons,Pokemons>();
		private static IDictionary<Pokemons,IList<Pokemons>>	_evolutions		= new Dictionary<Pokemons,IList<Pokemons>>();
		private static IList<Moves>								tmMoves			= null;

		public IList<Moves> GetLegalMoves2(Pokemons species,int maxlevel) {
			IList<Moves> moves=new List<Moves>();
			if (species==null || species<=0) return moves;
			//RgssOpen("Data/attacksRS.dat","rb") {|atkdata|
			//	offset=atkdata.getOffset(species-1);
			//	length=atkdata.getLength(species-1)>>1;
			//	atkdata.pos=offset;
			//Monster.Data.PokemonMoveTree atkdata = Kernal.PokemonMovesData[species];
			//	for (int k = 0; k < atkdata.LevelUp.Count-1; k++) {
				foreach (KeyValuePair<Moves,int> atkdata in Kernal.PokemonMovesData[species].LevelUp) {
					int level= atkdata.Value;//atkdata.fgetw;
					Moves move= atkdata.Key;//atkdata.fgetw;
					if (level<=maxlevel) {
						addMove(moves,move,1);
					}
				}
			//}
			//if (tmData == null) $tmData=load_data("Data/tm.dat");
			if (tmMoves == null) { // Create list of every TM/HM pkmn available in game
				tmMoves=new List<Moves>();
				//if (itemData==null) itemData=readItemList("Data/items.dat");
				//for (int i = 0; i < itemData.Length; i++) {
				//	if (itemData[i]==null) continue;
				//	atk=itemData[i][8];
				//	if (atk==null || atk==0) continue;
				//	if (tmData[atk]==null) continue;
				foreach (Moves atk in Kernal.PokemonMovesData[species].Machine) {
					tmMoves.Add(atk);
				}
			}
			foreach (Moves atk in tmMoves) { // Filter Item list by species
				//if (tmData[atk].Contains(species)) {
				//	addMove(moves,atk,0);
				//}
				addMove(moves,atk,0); // List is already filtered for species
			}
			IList<Moves> eggMoves = Kernal.PokemonMovesData[species].Egg;
			//Pokemons babyspecies=babySpecies(species);
			//RgssOpen("Data/eggEmerald.dat","rb"){|f|
			//	f.pos=(babyspecies-1)*8;
			//	offset=f.fgetdw;
			//	length=f.fgetdw;
				if (eggMoves?.Count>0) {
					//f.pos=offset;
					int i=0; do { //break; loop
						Moves atk=eggMoves[i];//f.fgetw;
						addMove(moves,atk,2);
						i+=1;
					} while (i < eggMoves.Count);
				}
			//}
			//IList<KeyValuePair<Moves,Attack.Data.MoveData>> movedatas=new List<KeyValuePair<Moves,Attack.Data.MoveData>>();
			IList<Attack.Data.MoveData> movedatas=new List<Attack.Data.MoveData>();
			foreach (Moves move in moves) {
				//movedatas.Add(new KeyValuePair<Moves,Attack.Data.MoveData>(move,moveData(move)));
				movedatas.Add(Kernal.MoveData[move]);
			}
			//  Delete less powerful moves
			//Action<IList<Moves>, Attack.Data.MoveData> deleteAll = new Action<IList<Moves>, Attack.Data.MoveData>((a, item) => { //proc{|a,item|
			Action<IList<Moves>, Attack.Data.MoveData> deleteAll = new Action<IList<Moves>, Attack.Data.MoveData>((a, item) => { //proc{|a,item|
				while (a.Contains(item.ID)) {
					a.Remove(item.ID); //delete
				}
			});
			foreach (Moves move in moves) {
				Attack.Data.MoveData md=moveData(move);
				foreach (Attack.Data.MoveData move2 in movedatas) {
					if (md.Effect==Attack.Effects.x0A5 && move2.Effect==0 && md.Type==move2.Type && //ToDo: Get P. Essential Function for 0xA5
						md.Power>=move2.Power) {
						deleteAll.Invoke(moves,move2);
					} else if (md.Effect==move2.Effect && md.Power==0 &&
						md.Accuracy>move2.Accuracy) {
						//  Supersonic vs. Confuse Ray, etc.
						deleteAll.Invoke(moves,move2);
					} else if (md.Effect==Attack.Effects.x006 && move2.Effect==Attack.Effects.x005) { //ToDo: Get P. Essential Function for 0x05 and 0x06
						deleteAll.Invoke(moves,move2);
					} else if (md.Effect==move2.Effect && md.Power!=0 &&
						md.Type==move2.Type &&
						(md.PP==15 || md.PP==10 || md.PP==move2.PP) &&
						(md.Power>move2.Power ||
						(md.Power==move2.Power && md.Accuracy>move2.Accuracy))) {
						//  Surf, Flamethrower, Thunderbolt, etc.
						deleteAll.Invoke(moves,move2);
					}
				}
			}
			return moves;
		}

		public int baseStatTotal(Pokemons species) {
			//if (_baseStatTotal[species]==null) {
			//	_baseStatTotal[species]=BaseStatTotal(species);
			//}
			if (!_baseStatTotal.ContainsKey(species)) {
				_baseStatTotal.Add(species,BaseStatTotal(species));
			}
			return _baseStatTotal[species];
		}

		public Pokemons babySpecies(Pokemons pkmn) {
			//if (_babySpecies[pkmn]==null) {
			//	_babySpecies[pkmn]=GetBabySpecies(pkmn);
			//}
			if (!_babySpecies.ContainsKey(pkmn)) {
				_babySpecies.Add(pkmn,GetBabySpecies(pkmn));
			}
			return _babySpecies[pkmn];
		}

		public int minimumLevel(Pokemons pkmn) {
			//if (_minimumLevel[pkmn]==null) {
			//	_minimumLevel[pkmn]=GetMinimumLevel(pkmn);
			//}
			if (!_minimumLevel.ContainsKey(pkmn)) {
				_minimumLevel.Add(pkmn,GetMinimumLevel(pkmn));
			}
			return _minimumLevel[pkmn];
		}

		public IList<Pokemons> evolutions(Pokemons species) {
			//if (_evolutions[pkmn]==null) {
			//	_evolutions[pkmn]=GetEvolvedFormData(pkmn);
			//}
			//return _evolutions[pkmn];
			if (!_evolutions.ContainsKey(species))
			{
				_evolutions.Add(species, new List<Pokemons>());
				foreach (Monster.Data.PokemonEvolution evo in Kernal.PokemonEvolutionsData[species])
				{
					_evolutions[species].Add(evo.Species);
				}
			}
			return _evolutions[species];
		}

		public PokemonUnity.Attack.Data.MoveData moveData(Moves move) {
			//if (_moveData[pkmn]==null) {
			//	_moveData[pkmn]=new PokemonUnity.Attack.Data.MoveData(pkmn);
			//}
			//return _moveData[pkmn];
			return Kernal.MoveData[move];
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
		*/


		public IPokemonChallengeRules withRestr(IPokemonChallengeRules rule, int minbs, int maxbs, int legendary) {
			IPokemonChallengeRules ret = new PokemonChallengeRules().addPokemonRule(new BaseStatRestriction(minbs,maxbs));
			if (legendary==0) {
				ret.addPokemonRule(new NonlegendaryRestriction());
			} else if (legendary==1) {
				ret.addPokemonRule(new InverseRestriction(new NonlegendaryRestriction()));
			}
			return ret;
		}

		// The Pokemon list is already roughly arranged by rank from weakest to strongest
		public IList<IPokemon> ArrangeByTier(IList<IPokemon> pokemonlist, IPokemonChallengeRules rule) {
			IPokemonChallengeRules[] tiers = new[] {
					withRestr(rule,0,500,0),
					withRestr(rule,380,500,0),
					withRestr(rule,400,555,0),
					withRestr(rule,400,555,0),
					withRestr(rule,400,555,0),
					withRestr(rule,400,555,0),
					withRestr(rule,580,680,1),
					withRestr(rule,500,680,0),
					withRestr(rule,580,680,2)
			};
			IList<IList<IPokemon>> tierPokemon=new List<IList<IPokemon>>();
			for (int i = 0; i < tiers.Length; i++) {
				tierPokemon.Add(new List<IPokemon>());
			}
			for (int i = 0; i < pokemonlist.Count; i++) {
				if (!rule.ruleset.isPokemonValid(pokemonlist[i])) continue;
				IList<int> validtiers=new List<int>();
				for (int j = 0; j < tiers.Length; j++) {
					IPokemonChallengeRules tier = tiers[j];
					if (tier.ruleset.isPokemonValid(pokemonlist[i])) {
						validtiers.Add(j);
					}
				}
				if (validtiers.Count>0) {
					int vt=validtiers.Count*i/pokemonlist.Count;
					tierPokemon[validtiers[vt]].Add(pokemonlist[i]);
				}
			}
			//  Now for each tier, sort the Pokemon in that tier
			List<IPokemon> ret=new List<IPokemon>();
			for (int i = 0; i < tiers.Length; i++) {
				//tierPokemon[i].OrderBy((a, b) => { //sort!{|a,b|
				//    int bstA = baseStatTotal(a.Species);
				//    int bstB = baseStatTotal(b.Species);
				//    if (bstA == bstB) { // Sort by species if base stat total is equal
				//        return a.Species.CompareTo(b.Species);
				//    } else { // Sort by base stat total
				//        return bstA.CompareTo(bstB);
				//    }
				//});
				tierPokemon[i] = tierPokemon[i].OrderBy(a => baseStatTotal(a.Species)).ThenBy(b => b.Species).ToList();
				//ret.concat(tierPokemon[i]);
				ret.AddRange(tierPokemon[i]);
			}
			return ret;
		}

		public bool hasMorePowerfulMove(IList<Moves> moves,Moves thismove) {
			PokemonUnity.Attack.Data.MoveData thisdata=moveData(thismove);
			if (thisdata.Power==0) return false;
			foreach (Moves move in moves) {
				if (move==0) continue;
				if (moveData(move).Type==thisdata.Type &&
					moveData(move).Power>thisdata.Power) {
					return true;
				}
			}
			return false;
		}

		public IPokemon RandomPokemonFromRule(IPokemonChallengeRules rule, ITrainer trainer) {
			IPokemon pkmn = null;
			int i = 0;
			int iteration = -1;
			do { //begin;
				iteration += 1;
				Pokemons species = 0;
				int level = rule.ruleset.suggestedLevel;
				do { //;loop
					species = 0;
					do { //;loop
						species = (Pokemons)Core.Rand.Next(Core.PokemonIndexLimit) + 1; //Species.maxValue
						string cname = species.ToString(TextScripts.Name);//getConstantName(Species, species); //rescue null;
						if (string.IsNullOrEmpty(cname)) break;
					} while (true);
					int r = Core.Rand.Next(20);
					int bst = baseStatTotal(species);
					if (level < minimumLevel(species)) continue;
					if (iteration % 2 == 0) {
						if (r < 16 && bst < 400) {
							continue;
						}
						if (r < 13 && bst < 500) {
							continue;
						}
					} else {
						if (bst > 400) {
							continue;
						}
						if (r < 10 && babySpecies(species) != species) {
							continue;
						}
					}
					if (r < 10 && babySpecies(species) == species) {
						continue;
					}
					if (r < 7 && evolutions(species).Count > 0) {
						continue;
					}
					break;
				} while (true);
				int ev = Core.Rand.Next(0x3F) + 1;
				int nature = 0;
				do { //;loop
					nature = Core.Rand.Next(25);
					int nd5 = (int)Math.Floor(nature / 5d); // stat to increase
					int nm5 = (int)Math.Floor(nature % 5d); // stat to decrease
					if (nd5 == nm5 || (Natures)nature == Natures.LAX || (Natures)nature == Natures.GENTLE) {
						//  Neutral nature, Lax, or Gentle
						if (Core.Rand.Next(20) < 19) continue;
					} else {
						if (((ev >> (1 + nd5)) & 1) == 0) {
							//  If stat to increase isn't emphasized
							if (Core.Rand.Next(10) < 6) continue;
						}
						if (((ev >> (1 + nm5)) & 1) != 0) {
							//  If stat to decrease is emphasized
							if (Core.Rand.Next(10) < 9) continue;
						}
					}
					break;
				} while (true);
				Items item = 0;
				if (level != legalMovesLevel) {
					legalMoves = new Dictionary<Pokemons, IList<Moves>>();
				}
				if (legalMoves[species] == null) {
					legalMoves[species] = GetLegalMoves2(species, level);
				}
				Items[] itemlist = new Items[] {
					Items.ORAN_BERRY,Items.SITRUS_BERRY,Items.ADAMANT_ORB,Items.BABIRI_BERRY,
					Items.BLACK_SLUDGE,Items.BRIGHT_POWDER,Items.CHESTO_BERRY,Items.CHOICE_BAND,
					Items.CHOICE_SCARF,Items.CHOICE_SPECS,Items.CHOPLE_BERRY,Items.DAMP_ROCK,
					Items.DEEP_SEA_TOOTH,Items.EXPERT_BELT,Items.FLAME_ORB,Items.FOCUS_SASH,
					Items.FOCUS_BAND,Items.HEAT_ROCK,Items.LEFTOVERS,Items.LIFE_ORB,Items.LIGHT_BALL,
					Items.LIGHT_CLAY,Items.LUM_BERRY,Items.OCCA_BERRY,Items.PETAYA_BERRY,Items.SALAC_BERRY,
					Items.SCOPE_LENS,Items.SHED_SHELL,Items.SHELL_BELL,Items.SHUCA_BERRY,Items.LIECHI_BERRY,
					Items.SILK_SCARF,Items.THICK_CLUB,Items.TOXIC_ORB,Items.WIDE_LENS,Items.YACHE_BERRY,
					Items.HABAN_BERRY,Items.SOUL_DEW,Items.PASSHO_BERRY,Items.QUICK_CLAW,Items.WHITE_HERB
				};
				//  Most used: Leftovers, Life Orb, Choice Band, Choice Scarf, Focus Sash
				do { //;loop
					if (Core.Rand.Next(40) == 0) {
						item = Items.LEFTOVERS;
						break;
					}
					Items itemsym = itemlist[Core.Rand.Next(itemlist.Length)];
					//item = getID(Items, itemsym);
					if (item == 0) continue;
					if (itemsym == Items.LIGHT_BALL) {
						if (species != Pokemons.PIKACHU) continue;
					}
					if (itemsym == Items.SHED_SHELL) {
						if (species != Pokemons.FORRETRESS ||
							species != Pokemons.SKARMORY) continue;
					}
					if (itemsym == Items.SOUL_DEW) {
						if (species != Pokemons.LATIOS ||
							species != Pokemons.LATIAS) continue;
					}
					if (itemsym == Items.LIECHI_BERRY && (ev & 0x02) == 0) {
						if (Core.Rand.Next(2) == 0) {
							continue;
						} else {
							ev |= 0x02;
						}
					}
					if (itemsym == Items.FOCUS_SASH) {
						if (baseStatTotal(species) > 450 && Core.Rand.Next(10) < 8) continue;
					}
					if (itemsym == Items.ADAMANT_ORB) {
						if (species != Pokemons.DIALGA) continue;
					}
					if (itemsym == Items.PASSHO_BERRY) {
						if (species != Pokemons.STEELIX) continue;
					}
					if (itemsym == Items.BABIRI_BERRY) {
						if (species != Pokemons.TYRANITAR) continue;
					}
					if (itemsym == Items.HABAN_BERRY) {
						if (species != Pokemons.GARCHOMP) continue;
					}
					if (itemsym == Items.OCCA_BERRY) {
						if (species != Pokemons.METAGROSS) continue;
					}
					if (itemsym == Items.CHOPLE_BERRY) {
						if (species != Pokemons.UMBREON) continue;
					}
					if (itemsym == Items.YACHE_BERRY) {
						if (species != Pokemons.TORTERRA &&
							species != Pokemons.GLISCOR &&
							species != Pokemons.DRAGONAIR) continue;
					}
					if (itemsym == Items.SHUCA_BERRY) {
						if (species != Pokemons.HEATRAN) continue;
					}
					if (itemsym == Items.SALAC_BERRY && (ev & 0x08) == 0) {
						if (Core.Rand.Next(2) == 0) {
							continue;
						} else {
							ev |= 0x08;
						}
					}
					if (itemsym == Items.PETAYA_BERRY && (ev & 0x10) == 0) {
						if (Core.Rand.Next(2) == 0) {
							continue;
						} else {
							ev |= 0x10;
						}
					}
					if (itemsym == (Items.DEEP_SEA_TOOTH)) {
						if (species != Pokemons.CLAMPERL) continue;
					}
					if (itemsym == (Items.THICK_CLUB)) {
						if (species != Pokemons.CUBONE &&
							species != Pokemons.MAROWAK) continue;
					}
					break;
				} while (true);
				if (level<10) {
					if (Core.Rand.Next(40)==0 ||
						item == Items.SITRUS_BERRY) item=(Items.ORAN_BERRY); //|| item
				}
				if (level>20) {
					if (Core.Rand.Next(40)==0 ||
						item == Items.ORAN_BERRY) item=(Items.SITRUS_BERRY); //|| item
				}
				IList<Moves> moves=legalMoves[species];
				bool sketch=false;
				if (moves[0] == Moves.SKETCH) {
					sketch=true;
					moves[0]=RandomMove();
					moves[1]=RandomMove();
					moves[2]=RandomMove();
					moves[3]=RandomMove();
				}
				if (moves.Count==0) continue;
				if ((moves ?? new Moves[0]).Count<4) {
					if (moves.Count==0) moves=new Moves[] { Moves.TACKLE };
					moves=moves??new Moves[0];//moves|=[];
				} else {
					IList<Moves> newmoves=new List<Moves>();
					Moves rest=(Moves.REST); //|| -1
					Moves spitup=(Moves.SPIT_UP); //|| -1
					Moves swallow=(Moves.SWALLOW); //|| -1
					Moves stockpile=(Moves.STOCKPILE); //|| -1
					Moves snore=(Moves.SNORE); //|| -1
					Moves sleeptalk=(Moves.SLEEP_TALK); //|| -1
					do { //;loop
						newmoves.Clear();
						while (newmoves.Count<4) {
							Moves m=moves[Core.Rand.Next(moves.Count)];
							if (Core.Rand.Next(2)==0 && hasMorePowerfulMove(moves,m)) {
								continue;
							}
							if (!newmoves.Contains(m) && m!=0) {
								newmoves.Add(m);
							}
						}
						if ((newmoves.Contains(spitup) ||
							newmoves.Contains(swallow)) && !newmoves.Contains(stockpile)) {
							if (sketch==null) continue;
						}
						if ((!newmoves.Contains(spitup) && !newmoves.Contains(swallow)) &&
							newmoves.Contains(stockpile)) {
							if (sketch==null) continue;
						}
						if (newmoves.Contains(sleeptalk) && !newmoves.Contains(rest)) {
							if ((!sketch && moves.Contains(rest)) || Core.Rand.Next(10) >= 2) continue;
						}
						if (newmoves.Contains(snore) && !newmoves.Contains(rest)) {
							if ((!sketch && moves.Contains(rest)) || Core.Rand.Next(10) >=2 ) continue;
						}
						int totalbasedamage=0;
						bool hasPhysical=false;
						bool hasSpecial=false;
						bool hasNormal=false;
						foreach (var move in newmoves) {
							Attack.Data.MoveData d=moveData(move);
							totalbasedamage+=d.Power??0;
							if (d.Power>=1) {
								if (d.Type == Types.NORMAL) hasNormal=true;
								if (d.Category==Attack.Category.PHYSICAL) hasPhysical=true; //0
								if (d.Category==Attack.Category.SPECIAL) hasSpecial=true;	//1
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
						int r=Core.Rand.Next(10);
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
						if (!hasNormal && item == Items.SILK_SCARF) {
							item=Items.LEFTOVERS;
						}
						moves=newmoves;
						break;
					} while (true);
				}
				for (i = 0; i < 4; i++) {
					if (moves[i] == null) moves[i]=0;
				}
				if (item == Items.LIGHT_CLAY &&
					!moves.Contains((Moves.LIGHT_SCREEN)) && //|| -1
					!moves.Contains((Moves.REFLECT))) { //|| -1
					item=Items.LEFTOVERS;
				}
				if (item == Items.BLACK_SLUDGE) {
					//dexdata=OpenDexData();
					//DexDataOffset(dexdata,species,8);
					Monster.Data.PokemonData dexdata=Kernal.PokemonData[species];
					Types type1=dexdata.Type[0];//.fgetb;
					Types type2=dexdata.Type[1];//.fgetb;
					//dexdata.close();
					if (type1 != Types.POISON && type2 != Types.POISON) {
						item=Items.LEFTOVERS;
					}
				}
				if (item == Items.HEAT_ROCK &&
					!moves.Contains((Moves.SUNNY_DAY))) { //|| -1
					item=Items.LEFTOVERS;
				}
				if (item == Items.DAMP_ROCK &&
					!moves.Contains((Moves.RAIN_DANCE))) { //|| -1
					item=Items.LEFTOVERS;
				}
				if (moves.Contains((Moves.REST))) { //|| -1
					if (Core.Rand.Next(3)==0) item=Items.LUM_BERRY;
					if (Core.Rand.Next(4)==0) item=Items.CHESTO_BERRY;
				}
				IPokemonSerialized pk=new PBPokemon(species,item,(Natures)nature,moves[0],moves[1],moves[2],moves[3],ev);
				pkmn=pk.createPokemon(level,31,trainer);
				i+=1;
			} while (!rule.ruleset.isPokemonValid(pkmn));
			return pkmn;
		}

		public int DecideWinnerEffectiveness(Moves move, Types otype1, Types otype2, Abilities ability, int[] scores) {
			Attack.Data.MoveData data=moveData(move);
			if (data.Power==0) return 0;
			Types atype=data.Type;
			float typemod=4;
			if (ability == Abilities.LEVITATE && data.Type == Types.GROUND) {
				typemod=4;
			} else {
				float mod1=Monster.Data.Type.GetEffectiveness(atype,otype1);
				float mod2=(otype1==otype2) ? 2 : Monster.Data.Type.GetEffectiveness(atype,otype2);
				if (ability == Abilities.WONDER_GUARD) {
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

		public double DecideWinnerScore(IList<IPokemon> party0, IList<IPokemon> party1, double rating) {
			double score=0;
			IList<Types> types1=new List<Types>();
			IList<Types> types2=new List<Types>();
			IList<Abilities> abilities=new List<Abilities>();
			for (int j = 0; j < party1.Count; j++) {
				types1.Add(party1[j].Type1);
				types2.Add(party1[j].Type2);
				abilities.Add(party1[j].Ability);
			}
			for (int i = 0; i < party0.Count; i++) {
				foreach (var move in party0[i].moves) {
					if (move.id==0) continue;
					for (int j = 0; j < party1.Count; j++) {
						score+=DecideWinnerEffectiveness(move.id,
							types1[j],types2[j],abilities[j],new int[] { -16, -8, 0, 4, 12, 20 });
					}
				}
				int basestatsum=baseStatTotal(party0[i].Species);
				score+=basestatsum/10;
				if (party0[i].Item!=0) score+=10;	// Not in Battle Dome ranking
			}
			score+=rating+Core.Rand.Next(32);
			return score;
		}

		public int DecideWinner(IList<IPokemon> party0, IList<IPokemon> party1, double rating0, double rating1) {
			rating0=Math.Round(rating0*15.0/100);
			rating1=Math.Round(rating1*15.0/100);
			double score0=DecideWinnerScore(party0,party1,rating0);
			double score1=DecideWinnerScore(party1,party0,rating1);
			if (score0==score1) {
				if (rating0==rating1) return 5;
				return (rating0>rating1) ? 1 : 2;
			} else {
				return (score0>score1) ? 1 : 2;
			}
		}

		public void RuledBattle(IRuledTeam team1, IRuledTeam team2, IPokemonChallengeRules rule) {
			BattleResults decision=0;
			if (Core.Rand.Next(100)!=0) {
				IList<IPokemon> party1=new List<IPokemon>();
				IList<IPokemon> party2=new List<IPokemon>();
				for (int i = 0; i < team1.length; i++) { party1.Add(team1[i]); }
				for (int i = 0; i < team2.length; i++) { party2.Add(team2[i]); }
				decision=(BattleResults)DecideWinner(party1,party2,team1.rating,team2.rating);
			} else {
				IPokeBattle_DebugSceneNoLogging scene=null;//new PokeBattle_DebugSceneNoLogging(); //ToDo: Uncomment
				ITrainer[] trainer1=new ITrainer[] { new Trainer("PLAYER1", TrainerTypes.PLAYER) };
				ITrainer[] trainer2=new ITrainer[] { new Trainer("PLAYER2", TrainerTypes.PLAYER) };
				IList<Items> items1=new List<Items>();
				IList<Items> items2=new List<Items>();
				int level=rule.ruleset.suggestedLevel;
				for (int i = 0; i < team1.length; i++) {
					IPokemon p=team1[i];
					if (p.Level!=level) {
						//p.Level=level;
						p.Exp=Experience.GetStartExperience(p.GrowthRate, level);
						p.calcStats();
					}
					items1.Add(p.Item);
					((IList<IPokemon>)trainer1[0].party).Add(p);
				}
				for (int i = 0; i < team2.length; i++) {
					IPokemon p=team2[i];
					if (p.Level!=level) {
						//p.Level=level;
						p.Exp=Experience.GetStartExperience(p.GrowthRate, level);
						p.calcStats();
					}
					items2.Add(p.Item);
					((IList<IPokemon>)trainer2[0].party).Add(p);
				}
				IBattle battle=rule.createBattle((IPokeBattle_Scene)scene,trainer1,trainer2);
				//battle.debug=true; //ToDo: Uncomment this line and below
				//battle.controlPlayer=true;
				battle.endspeech="...";
				battle.internalbattle=false;
				decision=battle.StartBattle();
				// p [items1,items2]
				for (int i = 0; i < team1.length; i++) {
					IPokemon p=team1[i];
					p.Heal();
					p.setItem(items1[i]);
				}
				for (int i = 0; i < team2.length; i++) {
					IPokemon p=team2[i];
					p.Heal();
					p.setItem(items2[i]);
				}
			}
			if (decision==BattleResults.WON) {			// 1 => Team 1 wins
				team1.addMatch(team2,1);
				team2.addMatch(team1,0);
			} else if (decision==BattleResults.LOST) {	// 2 => Team 2 wins
				team1.addMatch(team2,0);
				team2.addMatch(team1,1);
			} else {
				team1.addMatch(team2,-1);
				team2.addMatch(team1,-1);
			}
		}

		public Types[] getTypes(Pokemons species) {
			//dexdata=OpenDexData();
			//DexDataOffset(dexdata,species,8);
			Monster.Data.PokemonData dexdata=Kernal.PokemonData[species];
			//Types type1=dexdata.Type[0];
			//Types type2=dexdata.Type[1];
			//dexdata.close;
			//return type1==type2 ? [type1] : [type1,type2];
			return dexdata.Type;
		}

		//public virtual IEnumerator TrainerInfo(IList<IPokemonSerialized> pokemonlist, int trfile, IPokemonChallengeRules rules, Action block_given = null) {
		public virtual IEnumerator TrainerInfo(IList<IPokemon> pokemonlist, int trfile, IPokemonChallengeRules rules, Action block_given = null) {
			IList<PokemonUnity.Character.ITrainerData> bttrainers=GetBTTrainers(trfile);
			IPokemonSerialized[] btpokemon=GetBTPokemon(trfile);
			//trainertypes=load_data("Data/trainertypes.dat");
			IDictionary<TrainerTypes,Character.TrainerMetaData> trainertypes=Kernal.TrainerMetaData;
			if (bttrainers.Count==0) {
				for (int i = 0; i < 200; i++) {
					if (block_given != null && i%50==0) yield return null; //block_given?
					TrainerTypes trainerid=0;
					int money=30;
					do { //;loop
						trainerid = (TrainerTypes)Core.Rand.Next(Kernal.TrainerMetaData.Count) + 1; //Trainers.maxValue
						if (Core.Rand.Next(30) == 0) trainerid = TrainerTypes.YOUNGSTER; //getID(Trainers,:YOUNGSTER);
						//if (trainerid.ToString(TextScripts.Name) == "") continue;
						money = (//trainertypes[trainerid]==null ||
								 trainertypes[trainerid].BaseMoney==null) ? 30 : trainertypes[trainerid].BaseMoney;
						if (money >= 100) continue;
						break;
					} while (true);
					int gender=(//trainertypes[trainerid] == null ||
								trainertypes[trainerid].Gender == null) ? 2 : trainertypes[trainerid].Gender.Value ? 1 : 0; //[7]
					string randomName=getRandomNameEx(gender,null,0,12);
					ITrainerData tr =new Character.TrainerMetaData(trainerid,@double: false,//randomName,
						scriptBattleIntro: _INTL("Here I come!"),scriptBattleEnd: new string[] { _INTL("Yes, I won!"), _INTL("Man, I lost!") });
					bttrainers.Add(tr);
				}
				//bttrainers.sort!{|a,b|
				//	money1=(!trainertypes[a[0]] ||
				//			!trainertypes[a[0]][3]) ? 30 : trainertypes[a[0]][3];
				//	money2=(!trainertypes[b[0]] ||
				//			!trainertypes[b[0]][3]) ? 30 : trainertypes[b[0]][3];
				//	money1==money2 ? a[0]<=>b[0] : money1<=>money2;
				//}
				bttrainers.OrderBy(a => trainertypes[a.ID].BaseMoney); //ToDo: Sort 2nd Filter Criteria
			}
			if (block_given != null) yield return null;
			int suggestedLevel=rules.ruleset.suggestedLevel;
			IPokemonRuleSet rulesetTeam=rules.ruleset.copy().clearPokemonRules();
			IList<Types[]> pkmntypes=new List<Types[]>();
			IList<bool> validities=new List<bool>();
			Types[] t=new Types[2];//new Time(); | random types generated by datetime?
			foreach (IPokemon pkmn in pokemonlist) {
				if (pkmn.Level!=suggestedLevel) ((Monster.Pokemon)pkmn).SetLevel((byte)suggestedLevel);//pkmn.Level=suggestedLevel;
				pkmntypes.Add(getTypes(pkmn.Species));
				validities.Add(rules.ruleset.isPokemonValid(pkmn));
			}
			IList<ITrainerData> newbttrainers=new List<ITrainerData>();
			for (int btt = 0; btt < bttrainers.Count; btt++) {
				if (block_given != null && btt%50==0) yield return null;
				ITrainerData trainerdata=bttrainers[btt];
				IList<int> pokemonnumbers=trainerdata.PokemonNos; //[5]; | Party data
				IList<Pokemons> species=new List<Pokemons>();
				IDictionary<Types,int> types=new Dictionary<Types,int>(); //count of each pokemon type in collection
				// p trainerdata[1]
				//for (int typ = 0; typ < (Types.maxValue+1); typ++) { types[typ]=0; }
				foreach (Types typ in Kernal.TypeData.Keys) { types.Add(typ,0); }
				foreach (int pn in pokemonnumbers) {
					IPokemonSerialized pkmn=btpokemon[pn];
					species.Add(pkmn.species);
					t=getTypes(pkmn.species);
					foreach (Types typ in t) {
						types[typ]+=1;
					}
				}
				species=species ?? new List<Pokemons>(); // remove duplicates
				int count=0;
				//for (int typ = 0; typ < (Types.maxValue+1); typ++) {
				foreach (Types typ in Kernal.TypeData.Keys) {
					if (types[typ]>=5) {
						types[typ]/=4;
						if (types[typ]>10) types[typ]=10;
					} else {
						types[typ]=0;
					}
					count+=(int)types[typ];
				}
				if (count==0) types[0]=(int)1;
				if (pokemonnumbers.Count==0) {
					//int typ = 0; do {//|typ|
					//	types[typ]=1; typ++;
					//} while (typ < ); //(Types.maxValue+1).times
					foreach (Types typ in Kernal.TypeData.Keys) //replaces the loop above
						types[typ]=1;
				}
				IList<int> numbers=new List<int>();
				if (pokemonlist!=null) {
					//IList<IPokemonSerialized> numbersPokemon=new List<IPokemonSerialized>();
					IList<IPokemon> numbersPokemon=new List<IPokemon>();
					//  p species
					for (int index = 0; index < pokemonlist.Count; index++) {
						//IPokemonSerialized pkmn=pokemonlist[index];
						IPokemon pkmn=pokemonlist[index];
						if (!validities[index]) continue;
						int absDiff=Math.Abs((index*8/pokemonlist.Count)-(btt*8/bttrainers.Count));
						bool sameDiff=(absDiff==0);
						if (species.Contains(pkmn.Species)) {
							int weight= new []{ 32,12,5,2,1,0,0,0 }[Math.Min(absDiff,7)];
							if (Core.Rand.Next(40)<weight) {
								numbers.Add(index);
								numbersPokemon.Add(pokemonlist[index]);
							}
						} else {
							t=pkmntypes[index];
							foreach (var typ in t) {
								int weight= new []{ 32,12,5,2,1,0,0,0 }[Math.Min(absDiff,7)];
								weight*=types[typ];
								if (Core.Rand.Next(40)<weight) {
									numbers.Add(index);
									numbersPokemon.Add(pokemonlist[index]);
								}
							}
						}
					}
					numbers=numbers??new List<int>();
					if ((numbers.Count<6 ||
						!rulesetTeam.hasValidTeam(numbersPokemon))) {
						for (int index = 0; index < pokemonlist.Count; index++) {
							IPokemon pkmn=pokemonlist[index]; //IPokemonSerialized
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
							if (numbers.Count>=6 && rules.ruleset.hasValidTeam(numbersPokemon)) break;
						}
						if (numbers.Count<6 || !rules.ruleset.hasValidTeam(numbersPokemon)) {
							while (numbers.Count<pokemonlist.Count &&
								(numbers.Count<6 || !rules.ruleset.hasValidTeam(numbersPokemon))) {
								int index=Core.Rand.Next(pokemonlist.Count);
								if (!numbers.Contains(index)) {
									numbers.Add(index);
									numbersPokemon.Add(pokemonlist[index]);
								}
							}
						}
					}
					numbers.OrderBy(a=>a);//.sort!;
				}
				//newbttrainers.Add([trainerdata[0],trainerdata[1],trainerdata[2],
				//					trainerdata[3],trainerdata[4],numbers]);
				//newbttrainers.Add(new PBPokemon(trainerdata[0],trainerdata[1],trainerdata[2],
				//					trainerdata[3],trainerdata[4],numbers));
				//((List<int>)trainerdata.PokemonNos).AddRange(numbers); Kernal.TrainerData.Add(Kernal.TrainerData.Count,trainerdata);
				((List<int>)trainerdata.PokemonNos).AddRange(numbers); newbttrainers.Add(trainerdata);
			}
			if (block_given!=null) yield return null;
			pokemonlist.Clear();//=new List<>();
			foreach (IPokemon pkmn in pokemonlist) {
				//pokemonlist.Add(PBPokemon.fromPokemon(pkmn));
				pokemonlist.Add(pkmn);
			}
			IList<ITrainerChallengeData> trlists=new List<ITrainerChallengeData>(); //Kernal.load_data("Data/trainerlists.dat"); //rescue []
			Kernal.load_data(trlists,"Data/trainerlists.dat"); //rescue []
			bool hasDefault=false;
			int trIndex=-1;
			for (int i = 0; i < trlists.Count; i++) {
				if (trlists[i].IsDefault) hasDefault=true;	//[5]
			}
			for (int i = 0; i < trlists.Count; i++) {
				if (trlists[i].Tags.Contains(trfile)) {		//[2]
					trIndex=i;
					trlists[i].Trainers=newbttrainers;		//[0]
					trlists[i].Pokemons=pokemonlist;		//[1]
					trlists[i].IsDefault=!hasDefault;		//[5]
				}
			}
			if (block_given!=null) yield return null;
			if (trIndex<0) {
				//ITrainerChallengeData info=new object { newbttrainers,pokemonlist,new int[] { trfile },
				//		trfile+"tr.txt",trfile+"pm.txt",!hasDefault };
				//trlists.Add(info); //ToDo: Uncomment after implementing interface class
			}
			if (block_given!=null) yield return null;
			Kernal.save_data(trlists,"Data/trainerlists.dat");
			if (block_given!=null) yield return null;
			SaveTrainerLists();
			if (block_given!=null) yield return null;
		}


		//if $FAKERGSS;
		//	public void (this as IGameMessage).MessageDisplay(mw,txt,lbl) {
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


		public bool isBattlePokemonDuplicate(IPokemon pk, IPokemon pk2) {
			if (pk.Species==pk2.Species) {
			IList<Moves> moves1=new List<Moves>();
			IList<Moves> moves2=new List<Moves>();
			for (int i = 0; i < 4; i++) { // 4 times
				moves1.Add(pk.moves[0].id);
				moves2.Add(pk.moves[1].id);
			}
			//moves1.sort!;
			//moves2.sort!;
			if (moves1[0]==moves2[0] &&
				moves1[1]==moves2[1] &&
				moves1[2]==moves2[2] &&
				moves1[3]==moves2[3]) {
				//  Accept as same if moves are same and there are four moves each
				if (moves1[3]!=0) return true;
			}
			if (pk.Item==pk2.Item &&
				pk.Nature==pk2.Nature &&
				pk.EV[0]==pk2.EV[0] &&
				pk.EV[1]==pk2.EV[1] &&
				pk.EV[2]==pk2.EV[2] &&
				pk.EV[3]==pk2.EV[3] &&
				pk.EV[4]==pk2.EV[4] &&
				pk.EV[5]==pk2.EV[5]) return true;
			}
			return false;
		}

		public IList<IPokemon> RemoveDuplicates(IList<IPokemon> party) {
			// p "before: #{party.Length}"
			IList<IPokemon> ret=new List<IPokemon>();
			foreach (var pk in party) {
				bool found=false;
				int count=0;
				int firstIndex=-1;
				for (int i = 0; i < ret.Count; i++) {
					IPokemon pk2=ret[i];
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
						ret.RemoveAt(firstIndex);
					}
					ret.Add(pk);
				}
			}
			return ret;
		}

		public void ReplenishBattlePokemon(IList<IPokemon> party, IPokemonChallengeRules rule) {
			while (party.Count<20) {
				IPokemon pkmn=RandomPokemonFromRule(rule,null);
				bool found=false;
				foreach (var pk in party) {
					if (isBattlePokemonDuplicate(pkmn,pk)) {
						found=true; break;
					}
				}
				if (!found) party.Add(pkmn);
			}
		}

		//public IEnumerator<string> GenerateChallenge(IPokemonChallengeRules rule, int tag) {
		public IEnumerator GenerateChallenge(IPokemonChallengeRules rule, int tag, Action<string> action) {
			IPokemonChallengeRules oldrule=rule;
			action?.Invoke(_INTL("Preparing to generate teams")); //yield return
			rule=rule.copy().setNumber(2);
			yield return null;
			IList<IPokemon> party=new List<IPokemon>();//Kernal.load_data(tag+".rxdata"); //rescue []
			Kernal.load_data(party,tag+".rxdata"); //rescue []
			IList<IRuledTeam> teams=new List<IRuledTeam>();//Kernal.load_data(tag+"teams.rxdata"); //rescue []
			Kernal.load_data(teams, tag+"teams.rxdata"); //rescue []
			if (teams.Count<10) {
				IPokemonSerialized[] btpokemon=GetBTPokemon(tag);
				if (btpokemon!=null && btpokemon.Length!=0) {
					int suggestedLevel=rule.ruleset.suggestedLevel;
					foreach (var pk in btpokemon) {
						IPokemon pkmn=pk.createPokemon(suggestedLevel,31,null);
						if (rule.ruleset.isPokemonValid(pkmn)) party.Add(pkmn);
					}
				}
			}
			yield return null;
			party=RemoveDuplicates(party);
			yield return null;
			int maxteams=600;
			int cutoffrating=65;
			int toolowrating=40;
			int iterations=11;
			int iter = 0; do {//|iter|
				Kernal.save_data(party,tag+".rxdata");
				yield return _INTL("Generating teams ({1} of {2})",iter+1,iterations);
				int i=0;while (i<teams.Count) {
					if (i%10==0) yield return null;
					ReplenishBattlePokemon(party,rule);
					if (teams[i].rating<cutoffrating && teams[i].totalGames>=80) {
						teams[i]=new RuledTeam(party,rule);
					} else if (teams[i].length<2) {
						teams[i]=new RuledTeam(party,rule);
					} else if (i>=maxteams) {
						teams[i]=null;
						//teams.compact!;
					} else if (teams[i].totalGames>=250) {
						//  retire
						for (int j = 0; j < teams[i].length; j++) {
							party.Add(teams[i][j]);
						}
						teams[i]=new RuledTeam(party,rule);
					} else if (teams[i].rating<toolowrating) {
						teams[i]=new RuledTeam(party,rule);
					}
					i+=1;
				}
				Kernal.save_data(teams,tag+"teams.rxdata");
				yield return null;
				while (teams.Count<maxteams) {
					if (teams.Count%10==0) yield return null;
					ReplenishBattlePokemon(party,rule);
					teams.Add(new RuledTeam(party,rule));
				}
				Kernal.save_data(party,tag+".rxdata");
				teams=teams.OrderBy(a => a.rating).ToList(); //.sort{|a,b| b.rating<=>a.rating }
				yield return _INTL("Simulating battles ({1} of {2})",iter+1,iterations);
				i=0;  do { //;loop
					bool changed=false;
					for (int j = 0; j < teams.Count; j++) {
						yield return null;
						int other=j; int n = 0; do {//;
							other=Core.Rand.Next(teams.Count);
							if (other==j) continue; n++;
						} while (n < 5); //5.times
						if (other==j) continue;
						changed=true;
						RuledBattle(teams[j],teams[other],rule);
					}
					//  i+=1; if (i>=5) break;
					i+=1;
					int gameCount=0;
					foreach (var team in teams) {
						gameCount+=team.games;
					}
					// p [gameCount,teams.Length,gameCount/teams.Length]
					yield return null;
					if ((gameCount/teams.Count)>=12) {
						// p "Iterations: #{i}"
						foreach (var team in teams) {
							int games=team.games;
							team.updateRating();
							// if (Core.INTERNAL) p [games,team.totalGames,team.ratingRaw]
						}
						// p [gameCount,teams.Length,gameCount/teams.Length]
						break;
					}
				} while (true);
				teams.OrderBy(a => a.rating); //.sort!{|a,b| b.rating<=>a.rating }
				Kernal.save_data(teams,tag+"teams.rxdata");
			} while (iter < iterations); //iterations.times
			party.Clear(); //=new List<IPokemon>();
			yield return null;
			teams.OrderBy(a => a.rating); //.sort{|a,b| a.rating<=>b.rating }
			foreach (var team in teams) {
				if (team.rating>cutoffrating) {
					for (int i = 0; i < team.length; i++) {
						party.Add(team[i]);
					}
				}
			}
			rule=oldrule;
			yield return null;
			party=RemoveDuplicates(party);
			yield return _INTL("Writing results");
			party=ArrangeByTier(party,rule);
			yield return null;
			TrainerInfo(party, tag, rule, block_given: () => { return; });
			yield return null;
		}

		public void WriteCup(int id, IPokemonChallengeRules rules) {
			if (!Core.DEBUG) return;
			IList<ITrainerData> bttrainers=new ITrainerData[0];
			IList<ITrainerChallengeData> trlists=new List<ITrainerChallengeData>();//(Kernal.load_data("Data/trainerlists.dat")); //rescue []
			Kernal.load_data(trlists,"Data/trainerlists.dat"); //rescue []
			IList<string> list=new List<string>();
			for (int i = 0; i < trlists.Count; i++) {
				ITrainerChallengeData tr =trlists[i];
				if (tr.IsDefault) { //[5]
					//list.Add("*"+(tr[3].sub(/\.txt$/,"")));			//String ends with ".txt"
					list.Add("*"+tr.TrainerFile.Replace(".txt",""));	//[3] | String ends with ".txt"
				} else {
					//list.Add((tr[3].sub(/\.txt$/,"")));				//String ends with ".txt"
					list.Add(tr.TrainerFile.Replace(".txt",""));		//[3] | String ends with ".txt"
				}
			}
			int cmd=0;
			if (trlists.Count!=0) {
				cmd=(this as IGameMessage).Message(_INTL("Generate Pokemon teams for this challenge?"),
				new string[] { _INTL("NO"), _INTL("YES, USE EXISTING"), _INTL("YES, USE NEW") },1);
			} else {
				cmd=(this as IGameMessage).Message(_INTL("Generate Pokemon teams for this challenge?"),
					new string[] { _INTL("YES"), _INTL("NO") },2);
				if (cmd==0) {
					cmd=2;
				} else if (cmd==1) {
					cmd=0;
				}
			}
			if (cmd==0) return;	// No
			if (cmd==1) {		// Yes, use existing
				cmd=(this as IGameMessage).Message(_INTL("Choose a challenge."),list.ToArray(),-1);
				if (cmd>=0) {
					(this as IGameMessage).Message(_INTL("This challenge will use the Pokemon list from {1}.",list[cmd]));
					for (int i = 0; i < trlists.Count; i++) {
						ITrainerChallengeData tr=trlists[i];
						while (!tr.IsDefault && tr.Tags.Contains(id)) {	//[5] | tr[2] is a list of tags
							//tr[2].delete(id);
							tr.Tags.Remove(id);
						}
					}
					if (!trlists[cmd].IsDefault) {	//[5]
						trlists[cmd].Tags.Add(id);	//[2]
					}
					Kernal.save_data(trlists,"Data/trainerlists.dat");
					Graphics.update();
					SaveTrainerLists();
					Graphics.update();
					return;
				} else {
					return;
				}
				//  Yes, use new
			} else if (cmd==2 && !(this as IGameMessage).ConfirmMessage(_INTL("This may take a long time. Are you sure?"))) {
				return;
			}
			IWindow_AdvancedTextPokemon mw=(this as IGameMessage).CreateMessageWindow();
			//t=Time.now;
			DateTime t=Game.GetTimeNow;
			GenerateChallenge(rules, id, (message) => {
				if ((Game.GetTimeNow - t).Ticks >= 5) { //Time.now
					Graphics.update(); t = Game.GetTimeNow; //Time.now;
				}
				if (message != null) {
					(this as IGameMessage).MessageDisplay(mw, message, false);
					Graphics.update(); t = Game.GetTimeNow; //Time.now;
				}
			});
			(this as IGameMessage).DisposeMessageWindow(mw);
			(this as IGameMessage).Message(_INTL("Team generation complete."));
		}
	}

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

		public bool isValid (IPokemonSerialized pkmn) {
			int bst=0;//baseStatTotal(pkmn.Species);
			if (Game.GameData is IGameOrgBattleGenerator gobg) bst=gobg.baseStatTotal(pkmn.species);
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
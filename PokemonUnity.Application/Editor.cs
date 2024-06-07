using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.EventArg;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	public partial class Game : IGameEditor
	{
		//public	UIntProperty			 	UIntProperty;
		//public	LimitProperty			 	LimitProperty;
		//public	NonzeroLimitProperty	 	NonzeroLimitProperty;
		//public	ReadOnlyProperty		 	ReadOnlyProperty;
		//public	EnumProperty			 	EnumProperty;
		//public	LimitStringProperty	 		LimitStringProperty;
		//public	UndefinedProperty		 	UndefinedProperty;
		//public	BooleanProperty		 		BooleanProperty;
		//public	StringProperty		 		StringProperty;
		//public	BGMProperty			 		BGMProperty;
		//public	MEProperty			 		MEProperty;
		//public	WindowskinProperty	 		WindowskinProperty;
		//public	TrainerTypeProperty	 		TrainerTypeProperty;
		//public	SpeciesProperty		 		SpeciesProperty;
		//public	TypeProperty			 	TypeProperty;
		//public	MoveProperty			 	MoveProperty;
		//public	ItemProperty			 	ItemProperty;
		//public	NatureProperty		 		NatureProperty;
		protected	IWindow_CommandPokemon		 		Window_CommandPokemon;
		protected	IViewport					 		Viewport;

		public bool IsOldSpecialType (Types type) {
			return type == Types.FIRE ||
					type == Types.WATER ||
					type == Types.ICE ||
					type == Types.GRASS ||
					type == Types.ELECTRIC ||
					type == Types.PSYCHIC ||
					type == Types.DRAGON ||
					type == Types.DARK;
		}

		public string GetTypeConst(int i) {
			string ret=string.Empty;//MakeshiftConsts.get(MessageTypes.types,i,Types);
			if (ret==null) {
				ret=new string[] {"NORMAL","FIGHTING","FLYING","POISON","GROUND",
						"ROCK","BUG","GHOST","STEEL","QMARKS",
						"FIRE","WATER","GRASS","ELECTRIC",
						"PSYCHIC","ICE","DRAGON","DARK" }[i];
			}
			return ret;
		}

		public string GetEvolutionConst(int i) {
			string[] ret=new string[] { "Unknown",
				"Happiness","HappinessDay","HappinessNight","Level","Trade",
				"TradeItem","Item","AttackGreater","AtkDefEqual","DefenseGreater",
				"Silcoon","Cascoon","Ninjask","Shedinja","Beauty",
				"ItemMale","ItemFemale","DayHoldItem","NightHoldItem","HasMove",
				"HasInParty","LevelMale","LevelFemale","Location","TradeSpecies",
				"Custom1","Custom2","Custom3","Custom4","Custom5","Custom6","Custom7"
			};
			if (i>=ret.Length || i<0) i=0;
			return ret[i];
		}

		public string GetEggGroupConst(int i) {
			string[] ret=new string[] { "Undiscovered",
				"Monster","Water1","Bug","Flying","Field",
				"Fairy","Grass","Humanlike","Water3","Mineral",
				"Amorphous","Water2","Ditto","Dragon","Undiscovered"
			};
			if (i>=ret.Length || i<0) i=0;
			return ret[i];
		}

		public string GetColorConst(int i) {
			string[] ret=new string[] { "Red","Blue","Yellow","Green","Black",
				"Brown","Purple","Gray","White","Pink"
			};
			if (i>=ret.Length || i<0) i=0;
			return ret[i];
		}

		public string GetAbilityConst(int i) {
			return string.Empty; //MakeshiftConsts.get(MessageTypes.abilities,i,Abilities);
		}

		public string GetMoveConst(int i) {
			return string.Empty; //MakeshiftConsts.get(MessageTypes.moves,i,Moves);
		}

		public string GetItemConst(int i) {
			return string.Empty; //MakeshiftConsts.get(MessageTypes.items,i,Items);
		}

		public string GetSpeciesConst(int i) {
			return string.Empty; //MakeshiftConsts.get(MessageTypes.species,i,Species);
		}

		public string GetTrainerConst(int i) {
			string name=string.Empty; //MakeshiftConsts.get(MessageTypes.TrainerTypes,i,Trainers);
			return name;
		}



		// ###############################################################################
		// Save data to PBS files
		// ###############################################################################
		#region Save data to PBS files
		public void SavePokemonData() {
			/*string dexdata=File.open("Data/dexdata.dat","rb") rescue null;
			string messages=new Messages("Data/messages.dat") rescue null;
			if (dexdata==null || messages==null) return;
			string metrics=load_data("Data/metrics.dat") rescue null;
			string atkdata=File.open("Data/attacksRS.dat","rb");
			string eggEmerald=File.open("Data/eggEmerald.dat","rb");
			string regionaldata=File.open("Data/regionals.dat","rb");
			int numRegions=regionaldata.fgetw;
			int numDexDatas=regionaldata.fgetw;
			string pokedata=File.open("S/pokemon.txt","wb") rescue null;
			//pokedata.write(0xEF.chr);
			//pokedata.write(0xBB.chr);
			//pokedata.write(0xBF.chr);
			//foreach (var i in 1..(Species.maxValue ?? Species.getCount-1 ?? messages.getCount(MessageTypes.species)-1)) {
			for (int i = 0; i < Core.PokemonIndexLimit; i++) {
				string cname=getConstantName(Species,i); //rescue continue;
				string speciesname=messages.get(MessageTypes.species,i);
				string kind=messages.get(MessageTypes.kinds,i);
				string entry=messages.get(MessageTypes.entries,i);
				string formnames=messages.get(MessageTypes.formNames,i);
				//DexDataOffset(dexdata,i,2);
				Abilities ability1=dexdata.fgetw;
				Abilities ability2=dexdata.fgetw;
				IColor color=dexdata.fgetb;
				Habitat habitat=dexdata.fgetb;
				Types type1=dexdata.fgetb;
				Types type2=dexdata.fgetb;
				IList<> basestats=new List<>();
				for (int j = 0; j < 6; j++) {
					basestats.Add(dexdata.fgetb);
				}
				rareness=dexdata.fgetb;
				//DexDataOffset(dexdata,i,18);
				int gender=dexdata.fgetb;
				int happiness=dexdata.fgetb;
				LevelingRate growthrate=dexdata.fgetb;
				int stepstohatch=dexdata.fgetw;
				IList<> effort=[];
				for (int j = 0; j < 6; j++) {
					effort.Add(dexdata.fgetb);
				}
				DexDataOffset(dexdata,i,31);
				compat1=dexdata.fgetb;
				compat2=dexdata.fgetb;
				float height=dexdata.fgetw;
				float weight=dexdata.fgetw;
				//DexDataOffset(dexdata,i,38);
				int baseexp=dexdata.fgetw;
				Abilities hiddenability1=dexdata.fgetw;
				Abilities hiddenability2=dexdata.fgetw;
				Abilities hiddenability3=dexdata.fgetw;
				Abilities hiddenability4=dexdata.fgetw;
				Items item1=dexdata.fgetw;
				Items item2=dexdata.fgetw;
				Items item3=dexdata.fgetw;
				Items incense=dexdata.fgetw;
				pokedata.write("[#{i}]\r\nName=#{speciesname}\r\n");
				pokedata.write("InternalName=#{cname}\r\n");
				Types ctype1=getConstantName(Types,type1) rescue GetTypeConst(type1) || GetTypeConst(0) || "NORMAL";
				pokedata.write("Type1=#{ctype1}\r\n");
				if (type1!=type2) {
					Types ctype2=getConstantName(Types,type2) rescue GetTypeConst(type2) || GetTypeConst(0) || "NORMAL";
					pokedata.write("Type2=#{ctype2}\r\n");
				}
				pokedata.write("BaseStats=#{basestats[0]},#{basestats[1]},#{basestats[2]},#{basestats[3]},#{basestats[4]},#{basestats[5]}\r\n");
				switch (gender) {
					case 0:
						pokedata.write("GenderRate=AlwaysMale\r\n");
						break;
					case 31:
						pokedata.write("GenderRate=FemaleOneEighth\r\n");
						break;
					case 63:
						pokedata.write("GenderRate=Female25Percent\r\n");
						break;
					case 127:
						pokedata.write("GenderRate=Female50Percent\r\n");
						break;
					case 191:
						pokedata.write("GenderRate=Female75Percent\r\n");
						break;
					case 223:
						pokedata.write("GenderRate=FemaleSevenEighths\r\n");
						break;
					case 254:
						pokedata.write("GenderRate=AlwaysFemale\r\n");
						break;
					case 255:
						pokedata.write("GenderRate=Genderless\r\n");
						break;
				}
				pokedata.write("GrowthRate=" + ["Medium","Erratic","Fluctuating","Parabolic","Fast","Slow"][growthrate]+"\r\n");
				pokedata.write("BaseEXP=#{baseexp}\r\n");
				pokedata.write("EffortPoints=#{effort[0]},#{effort[1]},#{effort[2]},#{effort[3]},#{effort[4]},#{effort[5]}\r\n");
				pokedata.write("Rareness=#{rareness}\r\n");
				pokedata.write("Happiness=#{happiness}\r\n");
				pokedata.write("Abilities=");
				if (ability1!=0) {
					Abilities cability1=getConstantName(Abilities,ability1) rescue GetAbilityConst(ability1);
					pokedata.write("#{cability1}");
					if (ability2!=0) pokedata.write(",");
				}
				if (ability2!=0) {
					Abilities cability2=getConstantName(Abilities,ability2) rescue GetAbilityConst(ability2);
					pokedata.write("#{cability2}");
				}
				pokedata.write("\r\n");
				if (hiddenability1>0 || hiddenability2>0 || hiddenability3>0 || hiddenability4>0) {
					pokedata.write("HiddenAbility=");
					bool needcomma=false;
					Abilities cabilityh = Abilities.NONE;
					if (hiddenability1>0) {
						cabilityh=getConstantName(Abilities,hiddenability1) rescue GetAbilityConst(hiddenability1);
						pokedata.write("#{cabilityh}"); needcomma=true;
					}
					if (hiddenability2>0) {
						if (needcomma) pokedata.write(",");
						cabilityh=getConstantName(Abilities,hiddenability2) rescue GetAbilityConst(hiddenability2);
						pokedata.write("#{cabilityh}"); needcomma=true;
					}
					if (hiddenability3>0) {
						if (needcomma) pokedata.write(",");
						cabilityh=getConstantName(Abilities,hiddenability3) rescue GetAbilityConst(hiddenability3);
						pokedata.write("#{cabilityh}"); needcomma=true;
					}
					if (hiddenability4>0) {
						if (needcomma) pokedata.write(",");
						cabilityh=getConstantName(Abilities,hiddenability4) rescue GetAbilityConst(hiddenability4);
						pokedata.write("#{cabilityh}");
					}
					pokedata.write("\r\n");
				}
				pokedata.write("Moves=");
				int offset=atkdata.getOffset(i-1);
				int length=atkdata.getLength(i-1)>>1;
				atkdata.pos=offset;
				IList<> movelist=new List<>();
				for (int j = 0; j < length; j++) {
					int alevel=atkdata.fgetw;
					move=atkdata.fgetw;
					//movelist.Add([j,alevel,move]);
				}
				//movelist.sort!{|a,b| a[1]==b[1] ? a[0]<=>b[0] : a[1]<=>b[1] }
				for (int j = 0; j < movelist.Length; j++) {
					alevel=movelist[j][1];
					move=movelist[j][2];
					if (j>0) pokedata.write(",");
					cmove=getConstantName(Moves,move) rescue GetMoveConst(move);
					pokedata.write(string.Format("%d,%s",alevel,cmove));
				}
				pokedata.write("\r\n");
				eggEmerald.pos=(i-1)*8;
				offset=eggEmerald.fgetdw;
				length=eggEmerald.fgetdw;
				if (length>0) {
					pokedata.write("EggMoves=");
					eggEmerald.pos=offset;
					bool first=true;
					j=0; do {
						atk=eggEmerald.fgetw;
						if (!first) pokedata.write(",");
						if (atk==0) break;
						if (atk>0) {
							cmove=getConstantName(Moves,atk) rescue GetMoveConst(atk);
							pokedata.write("#{cmove}");
							first=false;
						}
						j+=1;
					} unless (j<length) break; //loop
					pokedata.write("\r\n");
				}
				comp1=getConstantName(EggGroups,compat1) rescue GetEggGroupConst(compat1);
				comp2=getConstantName(EggGroups,compat2) rescue GetEggGroupConst(compat2);
				if (compat1==compat2) {
					pokedata.write("Compatibility=#{comp1}\r\n");
				} else {
					pokedata.write("Compatibility=#{comp1},#{comp2}\r\n");
				}
				pokedata.write("StepsToHatch=#{stepstohatch}\r\n");
				pokedata.write("Height=");
				if (height) pokedata.write(string.Format("%.1f",height/10.0));
				pokedata.write("\r\n");
				pokedata.write("Weight=");
				if (weight) pokedata.write(string.Format("%.1f",weight/10.0));
				pokedata.write("\r\n");
				string colorname=getConstantName(Colors,color) rescue GetColorConst(color);
				pokedata.write("Color=#{colorname}\r\n");
				if (habitat>0) pokedata.write("Habitat="+new string[] { "", "Grassland", "Forest", "WatersEdge", "Sea", "Cave", "Mountain", "RoughTerrain", "Urban", "Rare" }[habitat]+"\r\n");
				IList<> regionallist=[];
				for (int region = 0; region < numRegions; region++) {
					regionaldata.pos=4+region*numDexDatas*2+(i*2);
					regionallist.Add(regionaldata.fgetw);
				}
				int numb = regionallist.size-1;
				while (numb>=0) { // remove every 0 at end of array
					//(regionallist[numb] == 0) ? regionallist.pop : break;
					if (regionallist[numb] == 0)
					regionallist.pop();
					else break;
					numb-=1;
				}
				if (!regionallist.empty) {
					pokedata.write("RegionalNumbers="+regionallist[0].ToString());
					for (numb = 1; numb < regionallist.size; numb++) {
					pokedata.write(","+regionallist[numb].ToString());
					}
					pokedata.write("\r\n");
				}
				pokedata.write("Kind=#{kind}\r\n");
				pokedata.write("Pokedex=#{entry}\r\n");
				if (formnames!=null && formnames!="") {
					pokedata.write("FormNames=#{formnames}\r\n");
				}
				if (item1>0) {
					citem1=getConstantName(Items,item1) rescue GetItemConst(item1);
					pokedata.write("WildItemCommon=#{citem1}\r\n");
				}
				if (item2>0) {
					citem2=getConstantName(Items,item2) rescue GetItemConst(item2);
					pokedata.write("WildItemUncommon=#{citem2}\r\n");
				}
				if (item3>0) {
					citem3=getConstantName(Items,item3) rescue GetItemConst(item3);
					pokedata.write("WildItemRare=#{citem3}\r\n");
				}
				if (metrics) {
					pokedata.write("BattlerPlayerY=#{metrics[0][i] || 0}\r\n");
					pokedata.write("BattlerEnemyY=#{metrics[1][i] || 0}\r\n");
					pokedata.write("BattlerAltitude=#{metrics[2][i] || 0}\r\n");
				}
				pokedata.write("Evolutions=");
				int count=0;
				foreach (var form in GetEvolvedFormData(i)) {
					evonib=form[0];
					level=form[1];
					poke=form[2];
					if (poke==0 || evonib==Evolutions.Unknown) continue;
					cpoke=getConstantName(Species,poke) rescue GetSpeciesConst(poke);
					evoname=getConstantName(Evolution,evonib) rescue GetEvolutionConst(evonib);
					if (!cpoke || cpoke=="") continue;
					if (count>0) pokedata.write(",");
					pokedata.write(string.Format("%s,%s,",cpoke,evoname));
					switch (Evolutions.EVOPARAM[evonib]) {
						case 1:
							pokedata.write(string.Format("{1:d}",level));
							break;
						case 2:
							clevel=getConstantName(Items,level) rescue GetItemConst(level);
							pokedata.write("#{clevel}");
							break;
						case 3:
							clevel=getConstantName(Moves,level) rescue GetMoveConst(level);
							pokedata.write("#{clevel}");
							break;
						case 4:
							clevel=getConstantName(Species,level) rescue GetSpeciesConst(level);
							pokedata.write("#{clevel}");
							break;
						case 5:
							clevel=getConstantName(Types,level) rescue GetTypeConst(level);
							pokedata.write("#{clevel}");
							break;
					}
					count+=1;
				}
				pokedata.write("\r\n");
				if (incense>0) {
					initem=getConstantName(Items,incense) rescue GetItemConst(incense);
					pokedata.write("Incense=#{initem}\r\n");
				}
				//if (i%20==0) {
				//	Graphics.update();
				//	Win32API.SetWindowText(Game._INTL("Processing species {1}...",i));
				//}
			}
			//dexdata.close();
			//atkdata.close();
			//eggEmerald.close();
			//regionaldata.close();
			//pokedata.close();
			//Graphics.update();*/
		}

		public void SaveTypes() {
			//if ((Types.maxValue)==0) return; //rescue 0
			//File.open("PBS/types.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	//foreach (var i in 0..(Types.maxValue rescue 25)) {
			//	for (int i = 0; i < Kernal.TypeData.Count; i++) {
			//		string name=i.ToString(TextScripts.Name); //rescue null
			//		if (!name || name=="") continue;
			//		string constname=getConstantName(Types,i); //rescue GetTypeConst(i)
			//		//f.write(string.Format("[%d]\r\n",i));
			//		//f.write(string.Format("Name=%s\r\n",name));
			//		//f.write(string.Format("InternalName=%s\r\n",constname));
			//		//if ((Types.isPseudoType(i) rescue i == QMARKS)) {
			//		//	f.write("IsPseudoType=true\r\n");
			//		//}
			//		//if ((Types.isSpecialType(i) rescue IsOldSpecialType(i))) {
			//		//	f.write("IsSpecialType=true\r\n");
			//		//}
			//		weak=[];
			//		resist=[];
			//		immune=[];
			//		//foreach (var j in 0..(Types.maxValue rescue 25)) {
			//		for (int j = 0; j < Kernal.TypeData.Count; j++) {
			//			cname=getConstantName(Types,j); //rescue GetTypeConst(j)
			//			if (!cname || cname=="") continue;
			//			eff=Types.getEffectiveness(j,i);
			//			if (eff==4) weak.Add(cname);
			//			if (eff==1) resist.Add(cname);
			//			if (eff==0) immune.Add(cname);
			//		}
			//		if (weak.Length>0) f.write("Weaknesses="+weak.join(",")+"\r\n");
			//		if (resist.Length>0) f.write("Resistances="+resist.join(",")+"\r\n");
			//		if (immune.Length>0) f.write("Immunities="+immune.join(",")+"\r\n");
			//		f.write("\r\n");
			//	}
			//}
		}

		public void SaveAbilities() {
			//File.open("PBS/abilities.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	//foreach (var i in 1..(Abilities.maxValue rescue Abilities.getCount-1 rescue GetMessageCount(MessageTypes.sbilities)-1)) {
			//	for (int i = 1; i < Abilities.maxValue; i++) {
			//		string abilname=getConstantName(Abilities,i) rescue GetAbilityConst(i);
			//		if (!abilname || abilname=="") continue;
			//		string name=GetMessage(MessageTypes.sbilities,i);
			//		if (!name || name=="") continue;
			//		f.write(string.Format("%d,%s,%s,%s\r\n",i,
			//			csvquote(abilname),
			//			csvquote(name),
			//			csvquote(GetMessage(MessageTypes.sbilityDescs,i));
			//		));
			//	}
			//}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// 0 => ID Number
		/// 1 => ID
		/// 2 => Name
		/// 3 => Name Plural
		/// 4 => Pocket
		/// 5 => Price
		/// 6 => Description
		/// 7 => Usability outside of battle
		/// 8 => Usability in battle
		/// 9 => Special Items
		/// 10 => HM/TM/TR Move
		/// </remarks>
		public void SaveItems() {
			//itemData=readItemList("Data/items.dat") rescue null;
			//if (!itemData || itemData.Length==0) return;
			//File.open("PBS/items.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	for (int i = 0; i < itemData.Length; i++) {
			//		if (!itemData[i]) continue;
			//		data=itemData[i];
			//		cname=getConstantName(Items,i) rescue string.Format("ITEM%03d",i);
			//		if (!cname || cname=="" || data[0]==0) continue;
			//		machine="";
			//		if (data[ITEMMACHINE]>0) {
			//			machine=getConstantName(Moves,data[ITEMMACHINE]) rescue GetMoveConst(data[ITEMMACHINE]) rescue "";
			//		}
			//		f.write(string.Format("%d,%s,%s,%s,%d,%d,%s,%d,%d,%d,%s\r\n",
			//			data[ITEMID],csvquote(cname),csvquote(data[ITEMNAME]),
			//			csvquote(data[ITEMPLURAL]),data[ITEMPOCKET],data[ITEMPRICE],
			//			csvquote(data[ITEMDESC]),data[ITEMUSE],data[ITEMBATTLEUSE],
			//			data[ITEMTYPE],csvquote(machine);
			//		));
			//	}
			//}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// 1 => ID number	This number must be different for each move. It must be a whole number greater than 0. You can skip numbers (e.g. the sequence 23,24,25,197,198,199,... is allowed). The order in which moves are numbered is not important.
		/// 2 => ID	This must be different for each move. This is how the scripts refer to the move. Typically this is the same as the move name, but written in all capital letters and with no spaces or symbols. In the scripts, the ID is used as a symbol (i.e. with a colon in front of it, e.g. :TACKLE). The ID is never seen by the player.
		/// 3 => Name	The name of the move, as seen by the player.
		/// 4 => Function code	The move's function code. This is a string of text. Each function code represents a different effect (e.g. poisons the target).
		/// 5 => Base power	The move's base power value. Status moves have a base power of 0, while moves with a variable base power are defined here with a base power of 1. For multi-hit moves, this is the base power of a single hit.
		/// 6 => Type	The ID of the move's elemental type.
		/// 7 => Damage category	Is one of the following:Physical,Special,Status
		/// 8 => Accuracy	The move's accuracy, as a percentage. An accuracy of 0 means the move doesn't perform an accuracy check (i.e. it will always hit, barring effects like semi-invulnerability).
		/// 9 => Total PP	The maximum amount of PP this move can have, not counting modifiers such as the item PP Up. If the total PP is 0, the move can be used infinitely.
		/// 10 => Additional effect chance	The probability that the move's additional effect occurs, as a percentage. If the move has no additional effect (e.g. all status moves), this value is 0.
		/// 11 => Target	The Pokémon that the move will strike. Is one of the following:
		/// 12 => Priority	The move's priority, between -6 and 6 inclusive. This is usually 0. A higher priority move will be used before all moves of lower priority, regardless of Speed calculations. Moves with equal priority will be used depending on which move user is faster.
		/// 13 => Flags	Any combination of the following letters:
		/// 14 => Description	The move's description.
		/// </remarks>
		public void SaveMoveData() {
			//if (!RgssExists("Data/moves.dat")) return;
			//File.open("PBS/moves.txt","wb"){|f|
			// f.write(0xEF.chr);
			// f.write(0xBB.chr);
			// f.write(0xBF.chr);
			//	foreach (var i in 1..(Moves.maxValue rescue Moves.getCount-1 rescue GetMessageCount(MessageTypes.soves)-1)) {
			//		moveconst=getConstantName(Moves,i) rescue GetMoveConst(i) rescue null;
			//		if (!moveconst || moveconst=="") continue;
			//		movename=GetMessage(MessageTypes.soves,i);
			//		movedata=new MoveData(i);
			//		flags="";
			//		if ((movedata.flags&0x00001)!=0) flags+="a";	// a - The move makes physical contact with the target.
			//		if ((movedata.flags&0x00002)!=0) flags+="b";	// b - The target can use Protect or Detect to protect itself from the move.
			//		if ((movedata.flags&0x00004)!=0) flags+="c";	// c - The target can use Magic Coat to redirect the effect of the move. Use this flag if the move deals no damage but causes a negative effect on the target. (Flags c and d are mutually exclusive.)
			//		if ((movedata.flags&0x00008)!=0) flags+="d";	// d - The target can use Snatch to steal the effect of the move. Use this flag for most moves that target the user. (Flags c and d are mutually exclusive.)
			//		if ((movedata.flags&0x00010)!=0) flags+="e";	// e - The move can be copied by Mirror Move.
			//		if ((movedata.flags&0x00020)!=0) flags+="f";	// f - The move has a 10% chance of making the opponent flinch if the user is holding a King's Rock/Razor Fang. Use this flag for all damaging moves that don't already have a flinching effect.
			//		if ((movedata.flags&0x00040)!=0) flags+="g";	// g - If the user is frozen, the move will thaw it out before it is used.
			//		if ((movedata.flags&0x00080)!=0) flags+="h";	// h - The move has a high critical hit rate.
			//		if ((movedata.flags&0x00100)!=0) flags+="i";	// i - The move is a biting move (powered up by the ability Strong Jaw).
			//		if ((movedata.flags&0x00200)!=0) flags+="j";	// j - The move is a punching move (powered up by the ability Iron Fist).
			//		if ((movedata.flags&0x00400)!=0) flags+="k";	// k - The move is a sound-based move.
			//		if ((movedata.flags&0x00800)!=0) flags+="l";	// l - The move is a powder-based move (Grass-type Pokémon are immune to them).
			//		if ((movedata.flags&0x01000)!=0) flags+="m";	// m - The move is a pulse-based move (powered up by the ability Mega Launcher).
			//		if ((movedata.flags&0x02000)!=0) flags+="n";	// n - The move is a bomb-based move (resisted by the ability Bulletproof).
			//		if ((movedata.flags&0x04000)!=0) flags+="o";	// o - The move is a dance move (repeated by the ability Dancer).
			//		if ((movedata.flags&0x08000)!=0) flags+="p";
			//		f.write(string.Format("%d,%s,%s,%03X,%d,%s,%s,%d,%d,%d,%02X,%d,%s,%s\r\n",
			//			i,csvquote(moveconst),csvquote(movename),
			//			movedata.function,
			//			movedata.basedamage,
			//			csvquote((getConstantName(Types,movedata.type) rescue GetTypeConst(movedata.type) rescue "")),
			//			csvquote(["Physical","Special","Status"][movedata.category]),
			//			movedata.accuracy,
			//			movedata.totalpp,
			//			movedata.addlEffect,
			//			movedata.target,
			//			movedata.priority,
			//			flags,
			//			csvquote(GetMessage(MessageTypes.soveDescriptions,i));
			//		));
			//	}
			//}
		}

		public void SaveMachines() {
			//machines=load_data("Data/tm.dat") rescue null;
			//if (!machines) return;
			//File.open("PBS/tm.txt","wb"){|f|
			//	for (int i = 1; i < machines.Length; i++) {
			//		if (i%50==0) Graphics.update();
			//		if (!machines[i]) continue;
			//		movename=getConstantName(Moves,i) rescue GetMoveConst(i) rescue null;
			//		if (!movename || movename=="") continue;
			//		f.write("\#-------------------\r\n");
			//		f.write(string.Format("[%s]\r\n",movename));
			//		x=[];
			//		for (int j = 0; j < machines[i].Length; j++) {
			//			speciesname=getConstantName(Species,machines[i][j]) rescue GetSpeciesConst(machines[i][j]) rescue null;
			//			if (!speciesname || speciesname=="") continue;
			//			x.Add(speciesname);
			//		}
			//		f.write(x.join(",")+"\r\n");
			//	}
			//}
		}

		public void SaveShadowMoves() {
			//moves=load_data("Data/shadowmoves.dat") rescue [];
			//File.open("PBS/shadowmoves.txt","wb"){|f|
			//	for (int i = 0; i < moves.Length; i++) {
			//		move=moves[i];
			//		if (move && moves.Length>0) {
			//			constname=(getConstantName(Species,i) rescue GetSpeciesConst(i) rescue null);
			//			if (!constname) continue;
			//			f.write(string.Format("%s=",constname));
			//			movenames=[];
			//			foreach (var m in move) {
			//				movenames.Add((getConstantName(Moves,m) rescue GetMoveConst(m) rescue null));
			//			}
			//			f.write(string.Format("%s\r\n",movenames.compact.join(",")));
			//		}
			//	}
			//}
		}
		/// <summary>
		/// </summary>
		/// <remarks>
		/// 0 => ID number
		/// 1 => ID
		/// 2 => Name
		/// 3 => Base Money
		/// 4 => Battle BGM
		/// 5 => Victory ME
		/// 6 => Intro ME
		/// 7 => Gender
		/// 8 => Skill Level
		/// 9 => Skill Codes
		/// </remarks>
		public void SaveTrainerTypes() {
			//data=load_data("Data/trainertypes.dat") rescue null;
			//if (!data) return;
			//File.open("PBS/trainertypes.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	f.write("\# "+Game._INTL("See the documentation on the wiki to learn how to edit this file."))
			//	f.write("\r\n");
			//	//for (int i = 0; i < data.Length; i++) {
			//	for (int i = 0; i < Kernal.TrainerMetaData.Count; i++) {
			//		//record=data[i];
			//		TrainerMetaData record=Kernal.TrainerMetaData[i];
			//		if (record) {
			//			dataline=string.Format("%d,%s,%s,%d,%s,%s,%s,%s,%s,%s\r\n",
			//			i,record[1],record[2],		//type, name,
			//			record[3],					//moneyEarned
			//			record[4] ? record[4] : "",	//BattleBGM
			//			record[5] ? record[5] : "",	//VictoryBGM
			//			record[6] ? record[6] : "",	//IntroME
			//			record[7] ? ["Male","Female","Mixed"][record[7]] : "Mixed", //gender
			//			(record[8]!=record[3]) ? record[8] : "",					//skill
			//			record[9] ? record[9] : "";									//skillCode
			//			);
			//			f.write(dataline);
			//		}
			//	}
			//}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// 0 => [Trainer type, trainer's name, version number]
		/// 1 => Items
		/// 2 => Lose Text
		/// 3 => Pokemon
		/// </remarks>
		public void SaveTrainerBattles() {
			//data=load_data("Data/trainers.dat") rescue null;
			//if (!data) return;
			//File.open("PBS/trainers.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	f.write("\// "+Game._INTL("See the documentation on the wiki to learn how to edit this file."))
			//	f.write("\r\n");
			//	foreach (var trainer in data) {
			//		string trname=getConstantName(Trainers,trainer[0]); //rescue GetTrainerConst(trainer[0]) rescue null
			//		if (trname==null) continue;
			//		f.write("\#-------------------\r\n");
			//		f.write(string.Format("%s\r\n",trname));
			//		string trainername=trainer[1] ? trainer[1].gsub(/,/,";") : "???";
			//		if (trainer[4]==0) {
			//			f.write(string.Format("%s\r\n",trainername));
			//		} else {
			//			f.write(string.Format("%s,%d\r\n",trainername,trainer[4]));
			//		}
			//		f.write(string.Format("%d",trainer[3].Length));
			//		for (int i = 0; i < 8; i++) {
			//			itemname=getConstantName(Items,trainer[2][i]) rescue GetItemConst(trainer[2][i]) rescue null;
			//			if (trainer[2][i]) f.write(string.Format(",%s",itemname));
			//		}
			//		f.write("\r\n");
			//		foreach (var poke in trainer[3]) {
			//			maxindex=0;
			//			towrite=[];
			//			thistemp=getConstantName(Species,poke[TPSPECIES]);	//rescue GetSpeciesConst(poke[TPSPECIES]) rescue ""
			//			towrite[TPSPECIES]=thistemp;
			//			towrite[TPLEVEL]=poke[TPLEVEL].ToString();
			//			thistemp=getConstantName(Items,poke[TPITEM]);		//rescue GetItemConst(poke[TPITEM]) rescue ""
			//			towrite[TPITEM]=thistemp;
			//			thistemp=getConstantName(Moves,poke[TPMOVE1]);		//rescue GetMoveConst(poke[TPMOVE1]) rescue ""
			//			towrite[TPMOVE1]=thistemp;
			//			thistemp=getConstantName(Moves,poke[TPMOVE2]);		//rescue GetMoveConst(poke[TPMOVE2]) rescue ""
			//			towrite[TPMOVE2]=thistemp;
			//			thistemp=getConstantName(Moves,poke[TPMOVE3]);		//rescue GetMoveConst(poke[TPMOVE3]) rescue ""
			//			towrite[TPMOVE3]=thistemp;
			//			thistemp=getConstantName(Moves,poke[TPMOVE4]);		//rescue GetMoveConst(poke[TPMOVE4]) rescue ""
			//			towrite[TPMOVE4]=thistemp;
			//			towrite[TPABILITY]=(poke[TPABILITY] ? poke[TPABILITY].ToString() : "");
			//			towrite[TPGENDER]=(poke[TPGENDER] ? ["M","F"][poke[TPGENDER]] : "");
			//			towrite[TPFORM]=(poke[TPFORM] && poke[TPFORM]!=TPDEFAULTS[TPFORM] ? poke[TPFORM].ToString() : "");
			//			towrite[TPSHINY]=(poke[TPSHINY] ? "shiny" : "");
			//			towrite[TPNATURE]=(poke[TPNATURE] ? getConstantName(Natures,poke[TPNATURE]) : "");
			//			towrite[TPIV]=(poke[TPIV] && poke[TPIV]!=TPDEFAULTS[TPIV] ? poke[TPIV].ToString() : "");
			//			towrite[TPHAPPINESS]=(poke[TPHAPPINESS] && poke[TPHAPPINESS]!=TPDEFAULTS[TPHAPPINESS] ? poke[TPHAPPINESS].ToString() : "");
			//			towrite[TPNAME]=(poke[TPNAME] ? poke[TPNAME] : "");
			//			towrite[TPSHADOW]=(poke[TPSHADOW] ? "true" : "");
			//			towrite[TPBALL]=(poke[TPBALL] && poke[TPBALL]!=TPDEFAULTS[TPBALL] ? poke[TPBALL].ToString() : "");
			//			for (int i = 0; i < towrite.Length; i++) {
			//				if (!towrite[i]) towrite[i]="";
			//				if (towrite[i] && towrite[i]!="") maxindex=i;
			//			}
			//			for (int i = 0; i < maxindex; i++) {
			//				if (i>0) f.write(",");
			//				f.write(towrite[i]);
			//			}
			//			f.write("\r\n");
			//		}
			//	}
			//}
		}

		public void normalizeConnectionPoint(object conn) {
			//ret=conn.clone();
			//if (conn[1]<0 && conn[4]<0) {
			//} else if (conn[1]<0 || conn[4]<0) {
			//	ret[4]=-conn[1];
			//	ret[1]=-conn[4];
			//}
			//if (conn[2]<0 && conn[5]<0) {
			//} else if (conn[2]<0 || conn[5]<0) {
			//	ret[5]=-conn[2];
			//	ret[2]=-conn[5];
			//}
			//return ret;
		}

		/// <summary>
		/// </summary>
		/// <param name="map1"></param>
		/// <param name="x1"  ></param>
		/// <param name="y1"  ></param>
		/// <param name="map2"></param>
		/// <param name="x2"  ></param>
		/// <param name="y2"  ></param>
		/// <returns></returns>
		public string writeConnectionPoint(int map1,int x1,int y1,int map2,int x2,int y2) {
			//IPoint dims1=MapFactoryHelper.getMapDims(map1);
			//IPoint dims2=MapFactoryHelper.getMapDims(map2);
			//if (x1==0 && x2==dims2[0]) {
			//	return string.Format("%d,West,%d,%d,East,%d\r\n",map1,y1,map2,y2);
			//} else if (y1==0 && y2==dims2[1]) {
			//	return string.Format("%d,North,%d,%d,South,%d\r\n",map1,x1,map2,x2);
			//} else if (x1==dims1[0] && x2==0) {
			//	return string.Format("%d,East,%d,%d,West,%d\r\n",map1,y1,map2,y2);
			//} else if (y1==dims1[1] && y2==0) {
			//	return string.Format("%d,South,%d,%d,North,%d\r\n",map1,x1,map2,x2);
			//} else {
			//	return string.Format("%d,%d,%d,%d,%d,%d\r\n",map1,x1,y1,map2,x2,y2);
			//}
			return string.Empty;
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// 0 => Map 1's ID number
		/// 1 => Map 1's edge (one of N, North, S, South, E, East, W or West)
		/// 2 => Map 1's connecting point (a positive integer)
		/// 3 => Map 2's ID number
		/// 4 => Map 2's edge (one of N, North, S, South, E, East, W or West)
		/// 5 => Map 2's connecting point (a positive integer)
		/// </remarks>
		public void SaveConnectionData() {
			//data=load_data("Data/connections.dat") rescue null;
			//if (!data) return;
			//SerializeConnectionData(data,LoadRxData("Data/MapInfos"));
		}

		public void SerializeConnectionData(object conndata,object mapinfos) {
			//File.open("PBS/connections.txt","wb"){|f|
			//	foreach (var conn in conndata) {
			//		if (mapinfos != null) {
			//			//  Skip if map no longer exists
			//			if (mapinfos[conn[0]] == null || mapinfos[conn[3]] == null) continue;
			//			f.write(string.Format("// %s (%d) - %s (%d)\r\n",
			//			mapinfos[conn[0]] ? mapinfos[conn[0]].name : "???",conn[0],
			//			mapinfos[conn[3]] ? mapinfos[conn[3]].name : "???",conn[3]));
			//		}
			//		if (conn[1] is String || conn[4] is String) {
			//			f.write(string.Format("%d,%s,%d,%d,%s,%d\r\n",conn[0],conn[1],
			//				conn[2],conn[3],conn[4],conn[5]));
			//		} else {
			//			ret=normalizeConnectionPoint(conn);
			//			f.write(writeConnectionPoint(
			//				ret[0],
			//				ret[1],
			//				ret[2],
			//				ret[3],
			//				ret[4],
			//				ret[5]
			//			));
			//		}
			//	}
			//}
			//Kernal.save_data(conndata,"Data/connections.dat");
		}

		public void SaveMetadata() {
			//data=Kernal.load_data("Data/metadata.dat"); //rescue null
			//if (data==null) return;
			//SerializeMetadata(data,LoadRxData("Data/MapInfos"));
		}

		public void SerializeMetadata(object metadata,object mapinfos) {
			//Kernal.save_data(metadata,"Data/metadata.dat");
			//File.open("PBS/metadata.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	for (int i = 0; i < metadata.Length; i++) {
			//		if (!metadata[i]) continue;
			//		f.write(string.Format("[%03d]\r\n",i));
			//		if (i==0) {
			//			types=PokemonMetadata.globalTypes;
			//		} else {
			//			if (mapinfos && mapinfos[i]) {
			//				f.write(string.Format("// %s\r\n",mapinfos[i].name))
			//			}
			//			types=PokemonMetadata.aonGlobalTypes;
			//		}
			//		foreach (var key in types.keys) {
			//			schema=types[key];
			//			record=metadata[i][schema[0]];
			//			if (record==null) continue;
			//			f.write(string.Format("%s=",key));
			//			WriteCsvRecord(record,f,schema);
			//			f.write(string.Format("\r\n"));
			//		}
			//	}
			//}
		}

		public void SaveEncounterData() {
			//IDictionary<int, IEncounters> encdata = null;//load_data("Data/encounters.dat");
			//Kernal.load_data(out encdata, "Data/encounters.dat"); //rescue null
			//if (encdata==null) return;
			//mapinfos=LoadRxData("Data/MapInfos");
			//File.open("PBS/encounters.txt","wb"){|f|
			//	//sortedkeys=encdata.keys.sort{|a,b| a<=>b}
			//	foreach (int i in encdata.Keys) { //sortedkeys
			//		if (encdata[i]!=null) {
			//			IEncounters e=encdata[i];
			//			string mapname="";
			//			if (mapinfos[i]!=null) {
			//				string map=mapinfos[i].name;
			//				mapname=$" # #{map}";
			//			}
			//			//f.write(string.Format("#########################\r\n"));										//
			//			//f.write(string.Format("%03d%s\r\n",i,mapname));												//Map id
			//			//f.write(string.Format("%d,%d,%d\r\n",e[0][EncounterTypes.Land],								//Encounter Density
			//			//	e[0][EncounterTypes.Cave],e[0][EncounterTypes.Water]));										//array[1-3]
			//			//f.write(string.Format("%d,%d,%d\r\n",e.EnctypeDensities[EncounterOptions.Land],				//Encounter Density
			//			//	e.EnctypeDensities[EncounterOptions.Cave],e.EnctypeDensities[EncounterOptions.Water]));		//array[1-3]
			//			//for (int j = 0; j < e.EnctypeEncounters.Count; j++) {											//[1]
			//			foreach (EncounterOptions j in e.EnctypeEncounters.Keys) {										//[1]
			//				//IEncounterPokemon enc=e[1][j];															//Pokemon Encounter Data array
			//				IList<IEncounterPokemon> enc=e.EnctypeEncounters[j];										//Pokemon Encounter Data array
			//				if (enc==null) continue;																	//
			//				//f.write(string.Format("%s\r\n",EncounterTypes.Names[j]));									//Encounter Method
			//				//for (int k = 0; k < EncounterTypes.EnctypeChances[j].Length; k++) {						//List of Pokemons
			//				//f.write(string.Format("%s\r\n",Game.GameData.PokemonEncounters.Names[j]));				//Encounter Method
			//				for (int k = 0; k < Game.GameData.PokemonEncounters.EnctypeChances[j].Length; k++) {		//List of Pokemons
			//					//int[] encentry=(enc[k]!=null) ? enc[k] : [1,5,5];										//enc[k]==[Pokemon Id,Min,Max]
			//					//string species=getConstantName(Species,encentry[0]);								//rescue GetSpeciesConst(encentry[0])
			//					IEncounterPokemon encentry=(enc[k]!=null) ? enc[k] : [1,5,5];							//enc[k]==[Pokemon Id,Min,Max]
			//					string species=encentry.Pokemon.ToString();												//rescue GetSpeciesConst(encentry[0])
			//					//if (encentry.MinLevel==encentry.MinLevel) {											//if min[1] == max[2]
			//					//	f.write(string.Format("%s,%d\r\n",species,encentry[1]));							//show only one number
			//					//	f.write(string.Format("%s,%d\r\n",species,encentry.MinLevel));						//show only one number
			//					//} else {																				//else show both number as upper and lower limit
			//					//	f.write(string.Format("%s,%d,%d\r\n",species,encentry[1],encentry[2]));
			//					//	f.write(string.Format("%s,%d,%d\r\n",species,encentry.MinLevel,encentry.MaxLevel));
			//					//}
			//				}
			//			}
			//		}
			//	}
			//}
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Current Position	The coordinates of the point on the region map, in squares. Every point must have these coordinates. This is filled in automatically when you click a part of the map in "townmapgen.html".
		/// Name	The location's name (e.g. Route 101, Rustboro City, Mt. Moon).
		/// Point of Interest	Optional. The name of a particular building or cave or other feature within that location (e.g. Pokémon Tower, Desert, Safari Zone). You can only have one Point of Interest per square.
		/// Fly Destination	Optional. This is a Fly destination, and is three comma-separated numbers: the map ID followed by X and Y coordinates of the location Fly will take you to when you choose this square to Fly to. This location is usually directly in front of a Poké Center building, hence its old name "Healing Spot".
		/// Switch	Optional. If this is a number, then the Game Switch of that number must be ON in order to show the square's information. That is, the map's name and Point of Interest will not be shown unless that Game Switch is ON. This only affects the player's version of the region map; the public version never shows the information for these squares.
		/// </remarks>
		public void SaveTownMap() {
			//IList<> mapdata=load_data("Data/townmap.dat") rescue null;
			//if (mapdata==null) return;
			//File.open("PBS/townmap.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	for (int i = 0; i < mapdata.Length; i++) {
			//		map=mapdata[i];
			//		if (!map) return;
			//		f.write(string.Format("[%d]\r\n",i));						//region id (town map for region)
			//		f.write(string.Format("Name=%s\r\nFilename=%s\r\n",			//
			//			csvquote(map[0] is Array ? map[0][0] : map[0]),			//name (name of region/map)
			//			csvquote(map[1] is Array ? map[1][0] : map[1])));		//filename (image used for map display)
			//		foreach (var loc in map[2]) {								//array of named locations on map {x,y,name,[fly target name,mapid,x,y]}
			//			f.write("Point=");
			//			WriteCsvRecord(loc,f,[null,"uussUUUU"]); //Point=15,8,"Lerucean Town",,23,11,15
			//			f.write("\r\n");
			//		}
			//	}
			//}
		}

		public void SavePhoneData() {
			//IList<IPhoneMessageData> data=load_data("Data/phone.dat"); //rescue null
			//if (data==null) return;
			//File.open("PBS/phone.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	f.write("[<Generics>]\r\n");
			//	f.write(data.generics.join("\r\n")+"\r\n");
			//	f.write("[<BattleRequests>]\r\n");
			//	f.write(data.battleRequests.join("\r\n")+"\r\n");
			//	f.write("[<GreetingsMorning>]\r\n");
			//	f.write(data.greetingsMorning.join("\r\n")+"\r\n");
			//	f.write("[<GreetingsEvening>]\r\n");
			//	f.write(data.greetingsEvening.join("\r\n")+"\r\n");
			//	f.write("[<Greetings>]\r\n");
			//	f.write(data.greetings.join("\r\n")+"\r\n");
			//	f.write("[<Bodies1>]\r\n");
			//	f.write(data.bodies1.join("\r\n")+"\r\n");
			//	f.write("[<Bodies2>]\r\n");
			//	f.write(data.bodies2.join("\r\n")+"\r\n");
			//}
		}

		public void SaveTrainerLists() {
			//trainerlists=load_data("Data/trainerlists.dat") rescue null;
			//if (!trainerlists) return;
			//File.open("PBS/trainerlists.txt","wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	foreach (var tr in trainerlists) {
			//		f.write((tr[5] ? "[DefaultTrainerList]" : "[TrainerList]")+"\r\n");	//bool
			//		f.write("Trainers="+tr[3]+"\r\n");									//filename
			//		f.write("Pokemon="+tr[4]+"\r\n");									//filename
			//		if (!tr[5]) {														//bool
			//			f.write("Challenges="+tr[2].join(",")+"\r\n");					//string array, references game mode
			//		}
			//		SaveBTTrainers(tr[0],"PBS/"+tr[3]);
			//		SaveBattlePokemon(tr[1],"PBS/"+tr[4]);
			//	}
			//}
		}

		public void SaveBTTrainers(ITrainer bttrainers,string filename) {
			//if (bttrainers==null || filename==null) return;
			//var btTrainersRequiredTypes=new KeyValuePair<string,object>{
			//	Type=[0,"e",null],				//"Type"=>[0,"e",null],				// Specifies a trainer
			//	Name=[1,"s"],					//"Name"=>[1,"s"],
			//	BeginSpeech=[2,"s"],			//"BeginSpeech"=>[2,"s"],
			//	EndSpeechWin=[3,"s"],			//"EndSpeechWin"=>[3,"s"],
			//	EndSpeechLose=[4,"s"],			//"EndSpeechLose"=>[4,"s"],
			//	PokemonNos=[5,"*u"]				//"PokemonNos"=>[5,"*u"]
			//}
			//File.open(filename,"wb"){|f|
			//	f.write(0xEF.chr);
			//	f.write(0xBB.chr);
			//	f.write(0xBF.chr);
			//	for (int i = 0; i < bttrainers.Length; i++) {
			//		if (!bttrainers[i]) continue;
			//		f.write(string.Format("[%03d]\r\n",i));
			//		foreach (string key in btTrainersRequiredTypes.Keys) {
			//			schema=btTrainersRequiredTypes[key];
			//			record=bttrainers[i][schema[0]];
			//			if (record==null) continue;
			//				f.write(string.Format("%s=",key));
			//			if (key=="Type") {
			//				f.write((getConstantName(Trainers,record))); //rescue GetTrainerConst(record)
			//			} else if (key=="PokemonNos") {
			//				f.write(record.join(",")); // WriteCsvRecord somehow won't work here
			//			} else {
			//				WriteCsvRecord(record,f,schema);
			//			}
			//			f.write(string.Format("\r\n"));
			//		}
			//	}
			//}
		}

		public void SaveBattlePokemon(IList<IPokemon> btpokemon,string filename) {
			//if (btpokemon==null || filename==null) return;
			//species={0=>""}
			//moves={0=>""}
			//items={0=>""}
			//natures={}
			//File.open(filename,"wb"){|f|
			//	for (int i = 0; i < btpokemon.Length; i++) {
			//		if (i%500==0) Graphics.update();
			//		IPokemon pkmn=btpokemon[i];
			//		f.write(FastInspect(pkmn,moves,species,items,natures));
			//		f.write("\r\n");
			//	}
			//}
		}

		public string FastInspect(IPokemon pkmn,Moves[] moves,Pokemons species,Items items,Natures natures) {
			//Pokemons c1=species[pkmn.Species] ? species[pkmn.Species] :
			//	(species[pkmn.Species]=pkmn.Species.ToString()); //getConstantName(Species,pkmn.Species) ?? GetSpeciesConst(pkmn.Species)
			//Items c2=items[pkmn.Item] ? items[pkmn.Item] :
			//	(items[pkmn.Item]=pkmn.Item.ToString()); //getConstantName(Items,pkmn.Item) ?? GetItemConst(pkmn.Item)
			//Natures c3=natures[pkmn.nature] ? natures[pkmn.nature] :
			//	(natures[pkmn.nature]=pkmn.nature.ToString()); //getConstantName(Natures,pkmn.nature)
			//string evlist="";
			//byte[] ev=pkmn.EV;
			//string[] evs=new string[]{ "HP","ATK","DEF","SPD","SA","SD" };
			//for (int i = 0; i < ev.Length; i++) {
			//	if (((ev&(1<<i))!=0)) {
			//		if (evlist.Length>0) evlist+=",";
			//		evlist+=evs[i];
			//	}
			//}
			//string c4=moves[pkmn.move1] ? moves[pkmn.move1] :
			//	(moves[pkmn.move1]=getConstantName(Moves,pkmn.move1)); //rescue GetMoveConst(pkmn.move1)
			//string c5=moves[pkmn.move2] ? moves[pkmn.move2] :
			//	(moves[pkmn.move2]=getConstantName(Moves,pkmn.move2)); //rescue GetMoveConst(pkmn.move2)
			//string c6=moves[pkmn.move3] ? moves[pkmn.move3] :
			//	(moves[pkmn.move3]=getConstantName(Moves,pkmn.move3)); //rescue GetMoveConst(pkmn.move3)
			//string c7=moves[pkmn.move4] ? moves[pkmn.move4] :
			//	(moves[pkmn.move4]=getConstantName(Moves,pkmn.move4)); //rescue GetMoveConst(pkmn.move4)
			return "#{c1};#{c2};#{c3};#{evlist};#{c4},#{c5},#{c6},#{c7}";
		}

		public void SaveAllData() {
			SaveTypes();			//Graphics.update();
			SaveAbilities();		//Graphics.update();
			SaveMoveData();			//Graphics.update();
			SaveConnectionData();	//Graphics.update();
			SaveMetadata();			//Graphics.update();
			SaveItems();			//Graphics.update();
			SaveTrainerLists();		//Graphics.update();
			SaveMachines();			//Graphics.update();
			SaveEncounterData();	//Graphics.update();
			SaveTrainerTypes();		//Graphics.update();
			SaveTrainerBattles();	//Graphics.update();
			SaveTownMap();			//Graphics.update();
			SavePhoneData();		//Graphics.update();
			SavePokemonData();		//Graphics.update();
			SaveShadowMoves();		//Graphics.update();
		}
		#endregion



		// ###############################################################################
		// Lists
		// ###############################################################################
		#region Lists
		public IWindow_CommandPokemon ListWindow(string[] cmds,float width=256) {
			//IWindow_CommandPokemon list=new Window_CommandPokemon().WithSize(cmds,0,0,width,Graphics.height);
			IWindow_CommandPokemon list=Window_CommandPokemon.WithSize(cmds,0,0,width,Graphics.height);
			list.index=0;
			list.rowHeight=24;
			SetSmallFont(list.contents);
			list.refresh();
			return list;
		}

		public int ChooseSpecies(int @default) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IList<string> commands=new List<string>();
			//for (int i = 1; i < Species.maxValue; i++) {
			foreach (Pokemons i in Kernal.PokemonData.Keys) {
				//string cname=getConstantName(Species,i); //rescue null
				//if (cname!=null)
					commands.Add(string.Format("{1:03d} {2:s}",i,i.ToString(TextScripts.Name)));
			}
			int ret=Commands2(cmdwin,commands,-1,@default-1,true);
			cmdwin.Dispose();
			return ret>=0 ? ret+1 : 0;
		}

		public Pokemons ChooseSpeciesOrdered(int @default) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Pokemons,string> commands=new SortedDictionary<Pokemons,string>();
			//for (int i = 1; i < Species.maxValue; i++) {
			foreach (Pokemons i in Kernal.PokemonData.Keys) {
				//string cname=getConstantName(Species,i); //rescue null
				//if (cname)
					commands.Add(i,i.ToString(TextScripts.Name));
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			IList<string> realcommands=new List<string>();
			foreach (KeyValuePair<Pokemons,string> command in commands) {
				//realcommands.Add(string.Format("{1:03d} {2:s}",command[0],command[1]));
				realcommands.Add(string.Format("{1:03d} {2:s}",command.Key,command.Value));
			}
			int ret=Commands2(cmdwin,realcommands,-1,@default-1,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret].Key : 0;
			return ret>=0 ? commands.ElementAt(ret).Key : 0;
		}

		/// <summary>
		/// Displays a sorted list of Pokémon species
		/// </summary>
		/// <param name="defaultItemID">if specified, indicates the ID of the species initially shown on the list.</param>
		/// <returns>returns the ID of the species selected or 0 if the selection was canceled.</returns>
		public Pokemons ChooseSpeciesList(Pokemons defaultItemID=0) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Pokemons,string> commands=new SortedDictionary<Pokemons,string>();
			int itemDefault=0;
			//foreach (var c in Species.constants) {
			foreach (Pokemons i in Kernal.PokemonData.Keys) {
				//i=Species.const_get(c);
				//if (i is Integer) {
					commands.Add(i,i.ToString(TextScripts.Name));
				//}
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			if (defaultItemID>0) {
				//commands.each_with_index {|item,index|
				for (int index=0;index<commands.Count;index++) {
					//if (item[0]==defaultItemID) itemDefault=index;
					if (commands.ElementAt(index).Key==defaultItemID) itemDefault=index;
				}
			}
			IList<string> realcommands=new List<string>();
			foreach (KeyValuePair<Pokemons, string> command in commands) {
				realcommands.Add(string.Format("{1:s}",command.Value));
			}
			int ret=Commands2(cmdwin,realcommands,-1,itemDefault,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret][0] : 0;
			return ret>=0 ? commands.ElementAt(ret).Key : 0;
		}

		/// <summary>
		/// Displays a sorted list of moves
		/// </summary>
		/// <param name="defaultMoveID">if specified, indicates the ID of the move initially shown on the list.</param>
		/// <returns>returns the ID of the move selected or 0 if the selection was canceled.
		/// </returns>
		public Moves ChooseMoveList(Moves defaultMoveID=0) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Moves,string> commands=new SortedDictionary<Moves,string>();
			int moveDefault=0;
			//for (int i = 1; i < Moves.maxValue; i++) {
			foreach (Moves i in Kernal.MoveData.Keys) {
				string name=i.ToString(TextScripts.Name);
				if (name!=null && name!="") commands.Add(i,name);
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			if (defaultMoveID>0) {
				//commands.each_with_index {|item,index|
				for (int index=0;index<commands.Count;index++) {
					//if (item[0]==defaultMoveID) moveDefault=index;
					if (commands.ElementAt(index).Key==defaultMoveID) moveDefault=index;
				}
			}
			IList<string> realcommands=new List<string>();
			foreach (var command in commands) {
				realcommands.Add(string.Format("{1:s}",command.Value));
			}
			int ret=Commands2(cmdwin,realcommands,-1,moveDefault,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret][0] : 0;
			return ret>=0 ? commands.ElementAt(ret).Key : 0;
		}

		public Types ChooseTypeList(Types defaultMoveID=0,bool movetype=false) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Types,string> commands=new SortedDictionary<Types,string>();
			int moveDefault=0;
			//for (int i = 0; i < Types.maxValue; i++) {
			foreach (Types i in Kernal.TypeData.Keys) {
				//if (!Types.isPseudoType(i)) {
					commands.Add(i,i.ToString(TextScripts.Name));
				//}
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			if (defaultMoveID>0) {
				//commands.each_with_index {|item,index|
				for (int index=0;index<commands.Count;index++) {
					//if (item[0]==defaultMoveID) moveDefault=index;
					if (commands.ElementAt(index).Key==defaultMoveID) moveDefault=index;
				}
			}
			IList<string> realcommands=new List<string>();
			foreach (var command in commands) {
				realcommands.Add(string.Format("{1:s}",command.Value));
			}
			do { //;loop
				int ret=Commands2(cmdwin,realcommands,-1,moveDefault,true);
				//Types retval=ret>=0 ? commands[ret][0] : 0;
				Types retval=ret>=0 ? commands.ElementAt(ret).Key : 0;
				cmdwin.Dispose();
				return retval;
			} while (true);
		}

		// Displays a sorted list of items, and returns the ID of the item selected or
		// 0 if the selection was canceled.  defaultItemID, if specified, indicates the
		// ID of the item initially shown on the list.
		public Items ChooseItemList(Items defaultItemID=0) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Items,string> commands=new SortedDictionary<Items,string>();
			int moveDefault=0;
			//foreach (var c in Items.constants) {
			foreach (Items i in Kernal.ItemData.Keys) {
				//i=Items.const_get(c);
				//if (i is Integer) {
					commands.Add(i,i.ToString(TextScripts.Name));
				//}
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			if (defaultItemID>0) {
				//commands.each_with_index {|item,index|
				for (int index=0;index<commands.Count;index++) {
					//if (item[0]==defaultItemID) moveDefault=index;
					if (commands.ElementAt(index).Key==defaultItemID) moveDefault=index;
				}
			}
			IList<string> realcommands=new List<string>();
			foreach (var command in commands) {
				realcommands.Add(string.Format("{1:s}",command.Value));
			}
			int ret=Commands2(cmdwin,realcommands,-1,moveDefault,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret][0] : 0;
			return ret>=0 ? commands.ElementAt(ret).Key : 0;
		}

		// Displays a sorted list of abilities, and returns the ID of the ability selected
		// or 0 if the selection was canceled.  defaultItemID, if specified, indicates the
		// ID of the ability initially shown on the list.
		public Abilities ChooseAbilityList(Abilities defaultAbilityID=0) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Abilities,string> commands=new SortedDictionary<Abilities,string>();
			int abilityDefault=0;
			//foreach (var c in Abilities.constants) {
			foreach (Abilities i in Enum.GetValues(typeof(Abilities))) { //Kernal.AbilityData.Keys
				//i=Abilities.const_get(c);
				//if (i is Integer) {
					commands.Add(i,i.ToString(TextScripts.Name));
				//}
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			if (defaultAbilityID>0) {
				//commands.each_with_index {|item,index|
				for (int index=0;index<commands.Count;index++) {
					//if (item[0]==defaultAbilityID) abilityDefault=index;
					if (commands.ElementAt(index).Key==defaultAbilityID) abilityDefault=index;
				}
			}
			IList<string> realcommands=new List<string>();
			foreach (var command in commands) {
				realcommands.Add(string.Format($"#{command.Value}"));
			}
			int ret=Commands2(cmdwin,realcommands,-1,abilityDefault,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret][0] : 0;
			return ret>=0 ? commands.ElementAt(ret).Key : 0;
		}

		public int Commands2(IWindow_CommandPokemon cmdwindow,IList<string> commands,int cmdIfCancel,int defaultindex=-1,bool noresize=false) {
			cmdwindow.z=99999;
			cmdwindow.visible=true;
			cmdwindow.commands=commands.ToArray();
			if (!noresize) {
				cmdwindow.width=256;
			} else {
				cmdwindow.height=Graphics.height;
			}
			if (cmdwindow.height>Graphics.height) cmdwindow.height=Graphics.height;
			cmdwindow.x=0;
			cmdwindow.y=0;
			cmdwindow.active=true;
			if (defaultindex>=0) cmdwindow.index=defaultindex;
			int ret=0;
			int command=0;
			do { //;loop
				Graphics.update();
				Input.update();
				cmdwindow.update();
				if (Input.trigger(PokemonUnity.Input.B)) {
					if (cmdIfCancel>0) {
						command=cmdIfCancel-1;
						break;
					} else if (cmdIfCancel<0) {
						command=cmdIfCancel;
						break;
					}
				}
				if (Input.trigger(PokemonUnity.Input.C) || (cmdwindow.doubleclick)) { //rescue false
					command=cmdwindow.index;
					break;
				}
			} while(true);
			ret=command;
			cmdwindow.active=false;
			return ret;
		}

		public int[] Commands3(IWindow_CommandPokemon cmdwindow,IList<string> commands,int cmdIfCancel,int defaultindex=-1,bool noresize=false) {
			cmdwindow.z=99999;
			cmdwindow.visible=true;
			cmdwindow.commands=commands.ToArray();
			if (!noresize) {
				cmdwindow.width=256;
			} else {
				cmdwindow.height=Graphics.height;
			}
			if (cmdwindow.height>Graphics.height) cmdwindow.height=Graphics.height;
			cmdwindow.x=0;
			cmdwindow.y=0;
			cmdwindow.active=true;
			if (defaultindex>=0) cmdwindow.index=defaultindex;
			//KeyValuePair<int,int> ret=new KeyValuePair<int, int>(0,0); //[];
			//KeyValuePair<int,int> command=new KeyValuePair<int, int>(0,0); //0;
			int[] ret=new int[2];
			int[] command=new int[0]; //0;
			do { //;loop
				Graphics.update();
				Input.update();
				cmdwindow.update();
				if (Input.trigger(PokemonUnity.Input.X)) {
					command=new int[] { 5,cmdwindow.index };
					break;
				}
				if (Input.press(PokemonUnity.Input.A)) {
					if (Input.repeat(PokemonUnity.Input.UP)) {
						command=new int[] { 1,cmdwindow.index };
						break;
					} else if (Input.repeat(PokemonUnity.Input.DOWN)) {
						command=new int[] { 2,cmdwindow.index };
						break;
					} else if (Input.press(PokemonUnity.Input.LEFT)) {
						command=new int[] { 3,cmdwindow.index };
						break;
					} else if (Input.press(PokemonUnity.Input.RIGHT)) {
						command=new int[] { 4,cmdwindow.index };
						break;
					}
				}
				if (Input.trigger(PokemonUnity.Input.B)) {
					if (cmdIfCancel>0) {
						command= new []{ 0,cmdIfCancel-1 };
						break;
					} else if (cmdIfCancel<0) {
						command= new []{ 0,cmdIfCancel };
						break;
					}
				}
				if (Input.trigger(PokemonUnity.Input.C) || (cmdwindow.doubleclick)) { //rescue false
					command=new int[] { 0,cmdwindow.index };
					break;
				}
			} while(true);
			ret=command;
			cmdwindow.active=false;
			return ret;
		}
		#endregion



		// ###############################################################################
		// Core lister script
		// ###############################################################################
		#region Core lister script
		public string ListScreen(IWindow_UnformattedTextPokemon title, ILister lister) {
			//IViewport viewport=new Viewport(0,0,Graphics.width,Graphics.height);
			IViewport viewport=Viewport.initialize(0,0,Graphics.width,Graphics.height);
			//viewport.z=99999;
			IWindow_CommandPokemon list=ListWindow(new string[0],256);
			list.viewport=viewport;
			list.z=2;
			//title=new Window_UnformattedTextPokemon(title); //ToDo: change title to string
			title.initialize(title.text);
			title.x=256;
			title.y=0;
			title.width=Graphics.width-256;
			title.height=64;
			title.viewport=viewport;
			title.z=2;
			lister.setViewport(viewport);
			int selectedmap=-1;
			string[] commands=lister.commands.ToArray();
			string value= string.Empty;
			int selindex=lister.startIndex;
			if (commands.Length==0) {
				value=(string)lister.value(-1);
				lister.Dispose();
				return value;
			}
			list.commands=commands;
			list.index=selindex;
			do { //;loop
				Graphics.update();
				Input.update();
				list.update();
				if (list.index!=selectedmap) {
					lister.refresh(list.index);
					selectedmap=list.index;
				}
				if (Input.trigger(PokemonUnity.Input.C) || (list.doubleclick)) { //rescue false
					break;
				} else if (Input.trigger(PokemonUnity.Input.B)) {
					selectedmap=-1;
					break;
				}
			} while(true);
			value=(string)lister.value(selectedmap);
			lister.Dispose();
			title.Dispose();
			list.Dispose();
			Input.update();
			return value;
		}

		public void ListScreenBlock(IWindow_UnformattedTextPokemon title, ILister lister, Action<int,TrainerMetaData?> action = null) {
			//IViewport viewport=new Viewport(0,0,Graphics.width,Graphics.height);
			IViewport viewport=Viewport.initialize(0,0,Graphics.width,Graphics.height);
			viewport.initialize(0,0,Graphics.width,Graphics.height);
			//viewport.z=99999;
			IWindow_CommandPokemon list=ListWindow(new string[0],256);
			list.viewport=viewport;
			list.z=2;
			//IWindow_UnformattedTextPokemon title=new Window_UnformattedTextPokemon(title);
			title.initialize(title.text);
			title.x=256;
			title.y=0;
			title.width=Graphics.width-256;
			title.height=64;
			title.viewport=viewport;
			title.z=2;
			lister.setViewport(viewport);
			int selectedmap=-1;
			string[] commands=lister.commands.ToArray();
			int selindex=lister.startIndex;
			string value=string.Empty;
			if (commands.Length==0) {
				value=(string)lister.value(-1);
				lister.Dispose();
				return; //value;
			}
			list.commands=commands;
			list.index=selindex;
			do { //;loop
				Graphics.update();
				Input.update();
				list.update();
				if (list.index!=selectedmap) {
					lister.refresh(list.index);
					selectedmap=list.index;
				}
				if (Input.trigger(PokemonUnity.Input.A)) {
					//yield (PokemonUnity.Input.t, lister.value(selectedmap));
					action?.Invoke(PokemonUnity.Input.A, (TrainerMetaData?)lister.value(selectedmap));
					list.commands=lister.commands.ToArray();
					if (list.index==list.commands.Length) {
						list.index=list.commands.Length;
					}
					lister.refresh(list.index);
				} else if (Input.trigger(PokemonUnity.Input.C) || (list.doubleclick)) { //rescue false
					//yield (PokemonUnity.Input.t, lister.value(selectedmap));
					action?.Invoke(PokemonUnity.Input.C, (TrainerMetaData?)lister.value(selectedmap));
					list.commands=lister.commands.ToArray();
					if (list.index==list.commands.Length) {
						list.index=list.commands.Length;
					}
					lister.refresh(list.index);
				} else if (Input.trigger(PokemonUnity.Input.B)) {
					break;
				}
			} while(true);
			lister.Dispose();
			title.Dispose();
			list.Dispose();
			Input.update();
		}
		#endregion




		// ###############################################################################
		// Core property editor script
		// ###############################################################################
		#region Core property editor script
		public bool PropertyList(string title_,object data,object properties,bool saveprompt=false) {
			//IViewport viewport=new Viewport(0,0,Graphics.width,Graphics.height);
			IViewport viewport=Viewport.initialize(0,0,Graphics.width,Graphics.height);
			//viewport.z=99999;
			IWindow_CommandPokemon list=ListWindow(new string[0],Graphics.width*5/10);
			list.viewport=viewport;
			list.z=2;
			//IWindow_UnformattedTextPokemon title=new Window_UnformattedTextPokemon(title);
			IWindow_UnformattedTextPokemon title=null;
			title.initialize(title_);
			title.x=list.width;
			title.y=0;
			title.width=Graphics.width*5/10;
			title.height=64;
			title.viewport=viewport;
			title.z=2;
			//IWindow_UnformattedTextPokemon desc=new Window_UnformattedTextPokemon("");
			IWindow_UnformattedTextPokemon desc=null;
			desc.x=list.width;
			desc.y=title.height;
			desc.width=Graphics.width*5/10;
			desc.height=Graphics.height-title.height;
			desc.viewport=viewport;
			desc.z=2;
			int selectedmap=-1;
			//int index=0;
			bool? retval=null;
			IList<string> commands=new List<string>();
			IProperty propobj=null;
			//for (int i = 0; i < properties.Length; i++) {
			//	propobj=properties[i][1];
			//	commands.Add(string.Format("%s=%s",properties[i][0],propobj.format(data[i])));
			//}
			//list.commands=commands.ToArray();
			//list.index=0;
			//do { //begin;
			//	do { //;loop
			//		Graphics.update();
			//		Input.update();
			//		list.update();
			//		desc.update();
			//		if (list.index!=selectedmap) {
			//			desc.text=properties[list.index][2];
			//			selectedmap=list.index;
			//		}
			//		if (Input.trigger(PokemonUnity.Input.A)) {
			//			propobj=properties[selectedmap][1];
			//			if (!(propobj is ReadOnlyProperty) && //propobj!=ReadOnlyProperty &&
			//				Game.GameData.GameMessage.ConfirmMessage(Game._INTL("Reset the setting {1}?",properties[selectedmap][0]))) {
			//				if (propobj is IHasDefaultProperty<string> p) { //propobj.respond_to("defaultValue")
			//					data[selectedmap]=p.defaultValue;
			//				} else {
			//					data[selectedmap]=null;
			//				}
			//			}
			//			commands.Clear();
			//			for (int i = 0; i < properties.Length; i++) {
			//				propobj=properties[i][1];
			//				commands.Add(string.Format("%s=%s",properties[i][0],propobj.format(data[i])));
			//			}
			//			list.commands=commands.ToArray();
			//		} else if (Input.trigger(PokemonUnity.Input.C) || (list.doubleclick)) { //rescue false
			//			propobj=properties[selectedmap][1];
			//			oldsetting=data[selectedmap];
			//			newsetting=propobj.set(properties[selectedmap][0],oldsetting);
			//			data[selectedmap]=newsetting;
			//			commands.Clear();
			//			for (int i = 0; i < properties.Length; i++) {
			//				propobj=properties[i][1];
			//				commands.Add(string.Format("%s=%s",properties[i][0],propobj.format(data[i])));
			//			}
			//			list.commands=commands.ToArray();
			//			break;
			//		} else if (Input.trigger(PokemonUnity.Input.B)) {
			//			selectedmap=-1;
			//			break;
			//		}
			//	} while (true);
			//	if (selectedmap==-1 && saveprompt) {
			//		int cmd=Game.GameData.GameMessage.Message(Game._INTL("Save changes?"),
			//			new string[] { Game._INTL("Yes"),Game._INTL("No"),Game._INTL("Cancel") },3);
			//		if (cmd==2) {
			//			selectedmap=list.index;
			//		} else {
			//			retval=(cmd==0);
			//		}
			//	}
			//} while (selectedmap!=-1);
			title.Dispose();
			list.Dispose();
			desc.Dispose();
			Input.update();
			return retval==true;
		}
		#endregion



		// ###############################################################################
		// Encounters editor
		// ###############################################################################
		#region Encounters editor
		public int EncounterEditorTypes(IEncounters enc, IWindow_CommandPokemon enccmd) {
			IList<string> commands=new List<string>();
			IList<int> indexes=new List<int>();
			bool haveblank=false;
			if (enc!=null) {
				//Display the encounter rates for area the player is currently located at...
				//using map id, and encounter method, return the int of frequency rate.
				commands.Add(Game._INTL("Density: {1},{2},{3}",
					enc.EnctypeDensities[EncounterOptions.Land],		//[0]
					enc.EnctypeDensities[EncounterOptions.Cave],		//[0]
					enc.EnctypeDensities[EncounterOptions.Water]));		//[0]
				indexes.Add(-2);
				//for (int i = 0; i < EncounterTypes.EnctypeChances.Length; i++) {
				//for (int i = 0; i < Game.GameData.PokemonEncounters.EnctypeChances.Count; i++) {
				foreach (EncounterOptions i in Game.GameData.PokemonEncounters.EnctypeChances.Keys) {
					if (enc.EnctypeEncounters[i]!=null) {	//[1]
						//commands.Add(EncounterTypes.Names[i]);
						//commands.Add(Enum.GetNames(typeof(EncounterOptions))[i]);
						commands.Add(i.ToString());
						indexes.Add((int)i);
					} else {
						haveblank=true;
					}
				}
			} else {
				commands.Add(Game._INTL("Density: Not Defined Yet"));
				indexes.Add(-2);
				haveblank=true;
			}
			if (haveblank) {
				commands.Add(Game._INTL("[New Encounter Type]"));
				indexes.Add(-3);
			}
			//enccmd.z=99999;
			enccmd.visible=true;
			enccmd.commands=commands.ToArray();
			if (enccmd.height>Graphics.height) enccmd.height=Graphics.height;
			enccmd.x=0;
			enccmd.y=0;
			enccmd.active=true;
			enccmd.index=0;
			int ret=0;
			int command=0;
			do { //;loop
				Graphics.update();
				Input.update();
				enccmd.update();
				if (Input.trigger(PokemonUnity.Input.A) && indexes[enccmd.index]>=0) {
					if (Game.GameData.GameMessage.ConfirmMessage(Game._INTL("Delete the encounter type {1}?",commands[enccmd.index]))) {
						//enc.EnctypeEncounters[indexes[enccmd.index]]=null;	//[1]
						enc.EnctypeEncounters[(EncounterOptions)indexes[enccmd.index]].Clear();	//[1]
						commands.RemoveAt(enccmd.index);	//commands.delete_at(enccmd.index);
						indexes.RemoveAt(enccmd.index);		//indexes.delete_at(enccmd.index);
						enccmd.commands=commands.ToArray();
						if (enccmd.index>=enccmd.commands.Length) {
							enccmd.index=enccmd.commands.Length;
						}
					}
				}
				if (Input.trigger(PokemonUnity.Input.B)) {
					command=-1;
					break;
				}
				if (Input.trigger(PokemonUnity.Input.C) || (enccmd.doubleclick)) { //rescue false
					command=enccmd.index;
					break;
				}
			} while(true);
			ret=command;
			enccmd.active=false;
			return ret<0 ? -1 : indexes[ret];
		}

		public int NewEncounterType(IEncounters enc) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0]);
			IList<string> commands=new List<string>();
			IList<int> indexes=new List<int>();
			//for (int i = 0; i < EncounterTypes.EnctypeChances.Length; i++) {
			//for (int n = 0; n < Game.GameData.PokemonEncounters.EnctypeChances.Count; n++) {
			foreach (EncounterOptions i in Game.GameData.PokemonEncounters.EnctypeChances.Keys) {
				bool dogen=false;//EncounterTypes i = (EncounterTypes)Enum.GetValues(typeof(EncounterTypes)).GetValue(n);
				if (enc.EnctypeEncounters[(EncounterOptions)i]==null) {								//[1]
					if (i==0) {
						if (enc.EnctypeEncounters[EncounterOptions.Cave]==null) dogen=true;			//[1]
					} else if (i==EncounterOptions.Land) {											//i=1 || EncounterTypes.Walking
						if (enc.EnctypeEncounters[EncounterOptions.Land]==null ||					//[1]
							enc.EnctypeEncounters[EncounterOptions.LandMorning]==null || 			//[1]
							enc.EnctypeEncounters[EncounterOptions.LandDay]==null || 				//[1]
							enc.EnctypeEncounters[EncounterOptions.LandNight]==null || 				//[1]
							enc.EnctypeEncounters[EncounterOptions.BugContest]==null) dogen=true;	//[1]
					} else {
						dogen=true;
					}
				}
				if (dogen) {
					//commands.Add(EncounterOptions.Names[i]);
					commands.Add(i.ToString()); //(Enum.GetNames(typeof(EncounterOptions))[n]);
					indexes.Add((int)i); //indexes.Add(n);
				}
			}
			int ret=Commands2(cmdwin,commands,-1);
			ret=(ret<0) ? -1 : indexes[ret];
			if (ret>=0) {
				//int[] chances=EncounterTypes.EnctypeChances[ret];
				int[] chances=Game.GameData.PokemonEncounters.EnctypeChances[(EncounterOptions)ret];
				enc.EnctypeEncounters[(EncounterOptions)ret]=new List<IEncounterPokemon>(); //[1]
				for (int i = 0; i < chances.Length; i++) {
					//enc[1][ret].Add(new IEncounterPokemon(1,5,5));//(IEncounterPokemon)[1,5,5]
					enc.EnctypeEncounters[(EncounterOptions)ret].Add(new EncounterSlotData(Pokemons.NONE,5,5));
				}
			}
			cmdwin.Dispose();
			return ret;
		}

		public void EditEncounterType(IEncounters enc,EncounterOptions etype) {
			IList<string> commands=new List<string>();
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0]);
			//int[] chances=EncounterTypes.EnctypeChances[etype];
			int[] chances=Game.GameData.PokemonEncounters.EnctypeChances[etype];
			int chancetotal=0;
			//chances.each {|a| chancetotal+=a}
			IEncounterPokemon[] enctype=enc.EnctypeEncounters[etype].ToArray(); //[1]
			for (int i = 0; i < chances.Length; i++) {
				if (enctype[i]==null) enctype[i]=new EncounterSlotData(Pokemons.NONE,5,5); //new IEncounterPokemon[]{ 1,5,5 };
			}
			int ret=0;
			do { //;loop
				commands.Clear();
				for (int i = 0; i < enctype.Length; i++) { //i=mapId
					string ch=chances[i].ToString();
					if (chancetotal!=100) ch=string.Format("%.1f",100.0f*chances[i]/chancetotal);
					if (enctype[i].MinLevel==enctype[i].MaxLevel) { //min[1] == max[2]
						commands.Add(Game._INTL("{1}% {2} (Lv.{3})",
							ch,enctype[i].Pokemon.ToString(TextScripts.Name), //[0]
							enctype[i].MinLevel		//[1]
						));
					} else {
						commands.Add(Game._INTL("{1}% {2} (Lv.{3}-Lv.{4})",
							ch,enctype[i].Pokemon.ToString(TextScripts.Name), //[0]
							enctype[i].MinLevel,	//[1]
							enctype[i].MaxLevel		//[2]
						));
					}
				}
				ret=Commands2(cmdwin,commands,-1,ret);
				if (ret<0) break;
				Pokemons species=enctype[ret].Pokemon; //ChooseSpecies(enctype[ret][0]);
				if (species<=0) continue;
				//if (species>0) enctype[ret].Pokemon=species; //[0]
				int minlevel=0;
				int maxlevel=0;
				IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
				param.setRange(1,Core.MAXIMUMLEVEL);
				param.setDefaultValue(enctype[ret].MinLevel);//[1]
				minlevel=Game.GameData.GameMessage.MessageChooseNumber(Game._INTL("Set the minimum level."),param);
				//param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
				param.initialize();
				param.setRange(minlevel,Core.MAXIMUMLEVEL);
				param.setDefaultValue(minlevel);
				maxlevel=Game.GameData.GameMessage.MessageChooseNumber(Game._INTL("Set the maximum level."),param);
				//enctype[ret].MinLevel=minlevel;	//[1]
				//enctype[ret].MaxLevel=maxlevel;	//[2]
				enctype[ret]=new EncounterSlotData(species, minlevel, maxlevel);
			} while (true);
			cmdwin.Dispose();
		}

		public void EncounterEditorDensity(IEncounters enc) {
			IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
			param.setRange(0,100);
			param.setDefaultValue(enc.EnctypeDensities[EncounterOptions.Land]);								//[0]
			enc.EnctypeDensities[EncounterOptions.Land]=Game.GameData.GameMessage.MessageChooseNumber(	//[0]
				Game._INTL("Set the density of Pokémon on land (default {1}).",
				//EncounterTypes.EnctypeDensities[EncounterTypes.Land]),param);
				Game.GameData.PokemonEncounters.EnctypeDensities[(int)EncounterOptions.Land]),param);
			param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
			param.setRange(0,100);
			param.setDefaultValue(enc.EnctypeDensities[EncounterOptions.Cave]);								//[0]
			enc.EnctypeDensities[EncounterOptions.Cave]=Game.GameData.GameMessage.MessageChooseNumber(	//[0]
				Game._INTL("Set the density of Pokémon in caves (default {1}).",
				//EncounterTypes.EnctypeDensities[EncounterTypes.Cave]),param);
				Game.GameData.PokemonEncounters.EnctypeDensities[EncounterOptions.Cave]),param);
			//param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
			param.initialize();
			param.setRange(0,100);
			param.setDefaultValue(enc.EnctypeDensities[EncounterOptions.Water]);							//[0]
			enc.EnctypeDensities[EncounterOptions.Water]=Game.GameData.GameMessage.MessageChooseNumber(	//[0]
				Game._INTL("Set the density of Pokémon on water (default {1}).",
					//EncounterTypes.EnctypeDensities[EncounterTypes.Water]),param);
					Game.GameData.PokemonEncounters.EnctypeDensities[EncounterOptions.Water]),param);
			//for (int i = 0; i < EncounterTypes.EnctypeCompileDens.Length; i++) {
			//	int? t=EncounterTypes.EnctypeCompileDens[i];
			//for (int i = 0; i < Game.GameData.PokemonEncounters.EnctypeCompileDens.Length; i++) {
			//	int? t=Game.GameData.PokemonEncounters.EnctypeCompileDens[i];
			//	if (t==null || t==0) continue;
			//	if (t==1) enc.EnctypeDensities[(EncounterOptions)i]=enc.EnctypeDensities[EncounterOptions.Land];	//[0]
			//	if (t==2) enc.EnctypeDensities[(EncounterOptions)i]=enc.EnctypeDensities[EncounterOptions.Cave];	//[0]
			//	if (t==3) enc.EnctypeDensities[(EncounterOptions)i]=enc.EnctypeDensities[EncounterOptions.Water];	//[0]
			//}
		}

		public void EncounterEditorMap(IDictionary<int,IEncounters> encdata,int map) {
			IWindow_CommandPokemon enccmd=ListWindow(new string[0]);
			//  This window displays the help text
			//IWindow_UnformattedTextPokemon enchelp=new Window_UnformattedTextPokemon("");
			IWindow_UnformattedTextPokemon enchelp=null;
			//enchelp.z=99999;
			enchelp.x=256;
			enchelp.y=0;
			enchelp.width=224;
			enchelp.height=96;
			//string mapinfos=load_data("Data/MapInfos.rxdata");
			string mapname="MAP_NAME"; //mapinfos[map].name;
			int lastchoice=0;
			do { //;loop
				//Display the encounter rates for area the player is currently located at...
				//using map id, and encounter method, return the int of frequency rate.
				IEncounters enc=encdata[map];
				enchelp.text=Game._INTL("{1}",mapname);
				int choice=EncounterEditorTypes(enc,enccmd);
				if (enc==null) {
					//enc=[EncounterTypes.EnctypeDensities.Clone,[]];
					//enc=new Dictionary<Game.GameData.PokemonEncounters.EnctypeDensities.Clone,IList<IEncounterPokemon>>();
					enc=new PokemonEncounter();
					encdata[map]=enc;
				}
				if (choice==-2) {
					EncounterEditorDensity(enc);
				} else if (choice==-1) {
					break;
				} else if (choice==-3) {
					int ret=NewEncounterType(enc);
					if (ret>=0) {
						//enchelp.text=Game._INTL("{1}\r\n{2}",mapname,EncounterTypes.Names[ret]);
						enchelp.text=Game._INTL("{1}\r\n{2}",mapname,((EncounterOptions)ret).ToString());
						EditEncounterType(enc,(EncounterOptions)ret);
					}
				} else {
					//enchelp.text=Game._INTL("{1}\r\n{2}",mapname,EncounterTypes.Names[choice]);
					enchelp.text=Game._INTL("{1}\r\n{2}",mapname,((EncounterOptions)choice).ToString());
					EditEncounterType(enc,(EncounterOptions)choice);
				}
			} while(true);
			if (encdata[map].EnctypeEncounters.Count==0) {	//[1]
				encdata[map]=null;
			}
			enccmd.Dispose();
			enchelp.Dispose();
			Input.update();
		}
		#endregion



		// ###############################################################################
		// Trainer type editor
		// ###############################################################################
		#region Trainer type editor
		public TrainerTypes TrainerTypeEditorNew(string trconst) {
			//IList<TrainerMetaData> data=load_data("Data/trainertypes.dat");
			////  Get the first unused ID after all existing t-types for the new t-type to use.
			//int maxid=-1;
			//foreach (TrainerMetaData rec in data) {
			//	//if (rec==null) continue;
			//	maxid=Math.Max(maxid,(int)rec.ID);
			//}
			//TrainerTypes trainertype=(TrainerTypes)(maxid+1);
			//string trname=Game.GameData.GameMessage.MessageFreeText(Game._INTL("Please enter the trainer type's name."),
			//	trconst ?? "" //trconst != null ? trconst.gsub(/_+/," ") : ""
			//	,false,256);
			//if (trname=="" && trconst==null) {
			//	return -1;
			//} else {
			//	//  Create a default name if there is none.
			//	if (trconst==null) {
			//		//trconst=trname.gsub(/[^A-Za-z0-9_]/,"");
			//		//trconst=trconst.sub(/^([a-z])/){ $1.upcase }
			//		if (trconst.Length==0) {
			//			trconst=string.Format("T_%03d",trainertype);
			//		} else if (!trconst[0,1][/[A-Z]/]) {
			//			trconst="T_"+trconst;
			//		}
			//	}
			//	if (trname=="") trname=trconst;
			//	//  Create an internal name based on the trainer type's name.
			//	string cname=trname.gsub(/é/,"e");
			//	//cname=cname.gsub(/[^A-Za-z0-9_]/,"");
			//	cname=cname.ToUpper();
			//	if (hasConst(Trainers,cname)) {
			//		int suffix=1;
			//		int n=0;do {
			//			string tname=string.Format("%s_%d",cname,suffix);
			//			if (!hasConst(Trainers,tname)) {
			//				cname=tname;
			//				break;
			//			}
			//			suffix+=1;n++;
			//		} while (n<100); //100.times
			//	}
			//	if (hasConst(Trainers,cname)) {
			//		Game.GameData.GameMessage.Message(Game._INTL("Failed to create the trainer type. Choose a different name."));
			//		return -1;
			//	}
			//	TrainerMetaData record=[];
			//	record[0]=trainertype;
			//	record[1]=cname;
			//	record[2]=trname;
			//	record[7]=Game.GameData.GameMessage.Message(Game._INTL("Is the Trainer male, female, or mixed gender?"),new string[]{
			//		Game._INTL("Male"),Game._INTL("Female"),Game._INTL("Mixed") },0);
			//	IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
			//	param.setRange(0,255);
			//	param.setDefaultValue(30);
			//	record[3]=Game.GameData.GameMessage.MessageChooseNumber(
			//		Game._INTL("Set the money per level won for defeating the Trainer."),param);
			//	record[8]=record[3];
			//	record[9]="";
			//	Trainers.const_set(cname,record[0]);
			//	data[record[0]]=record;
			//	Kernal.save_data(data,"Data/trainertypes.dat");
			//	ConvertTrainerData();
			//	Game.GameData.GameMessage.Message(Game._INTL("The Trainer type was created (ID: {1}).",record[0]));
			//	Game.GameData.GameMessage.Message(
			//		string.Format("Put the Trainer's graphic (trainer{1:03d}.png or trainer{2:s}.png) in Graphics/Characters, or it will be blank.",
			//		record[0],getConstantName(Trainers,record[0])));
			//	return record[0];
			//}
			return TrainerTypes.WildPokemon;
		}

		public void TrainerTypeEditorSave(TrainerTypes trainertype,TrainerMetaData ttdata) {
			TrainerMetaData record=new TrainerMetaData();
			//record[0]=trainertype;
			//for (int i = 0; i < ttdata.Length; i++) {
			//	record.Add(ttdata[i]);
			//}
			//setConstantName(Trainers,trainertype,ttdata[0]);
			//IList<TrainerMetaData> data=load_data("Data/trainertypes.dat");
			//data[record[0]]=record;
			//data=Kernal.save_data(data,"Data/trainertypes.dat");
			//ConvertTrainerData();
		}

		public void TrainerTypeEditor() {
			TrainerTypes selection=0;
			object[] trainerTypes=new object[] {
		//		new { Game._INTL("Internal Name"),ReadOnlyProperty,
		//			Game._INTL("Internal name that appears in constructs like Trainers.XXX.") },
		//		new { Game._INTL("Trainer Name"),StringProperty,
		//			Game._INTL("Name of the trainer type as displayed by the game.") },
		//		new { Game._INTL("Money Per Level"),LimitProperty.initialize(255),
		//			Game._INTL("Player earns this amount times the highest level among the trainer's Pokémon.") },
		//		new { Game._INTL("Battle BGM"),BGMProperty,
		//			Game._INTL("BGM played in battles against trainers of this type.") },
		//		new { Game._INTL("Battle End ME"),MEProperty,
		//			Game._INTL("ME played when player wins battles against trainers of this type.") },
		//		new { Game._INTL("Battle Intro ME"),MEProperty,
		//			Game._INTL("ME played before battles against trainers of this type.") },
		//		new { Game._INTL("Gender"),EnumProperty.initialize(new string[] { Game._INTL("Male"),Game._INTL("Female"),Game._INTL("Mixed gender") }),
		//			Game._INTL("Gender of this Trainer type."}),
		//		new { Game._INTL("Skill"),LimitProperty.initialize(255),
		//			Game._INTL("Skill level of this Trainer type.") },
		//		new { Game._INTL("Skill Codes"),StringProperty,
		//			Game._INTL("Letters/phrases representing AI modifications of trainers of this type.") }
			};
			//ListScreenBlock(Game._INTL("Trainer Types"),new TrainerTypeLister(selection,true)){|button,TrainerMetaData trtype|
			//ListScreenBlock(Game._INTL("Trainer Types"),trainerTypeLister.initialize(selection,true), action: (button,trtype) => {
			//	if (trtype!=null) {
			//		if (button==PokemonUnity.Input.A) {
			//			if (trtype.Value.ID>=0) { //trtype[0]
			//				if (Game.GameData.GameMessage.ConfirmMessageSerious("Delete this trainer type?")) {
			//					data=load_data("Data/trainertypes.dat");
			//					removeConstantValue(Trainers,trtype[0]);
			//					data[trtype[0]]=null;
			//					Kernal.save_data(data,"Data/trainertypes.dat");
			//					ConvertTrainerData();
			//					Game.GameData.GameMessage.Message(Game._INTL("The Trainer type was deleted."));
			//				}
			//			}
			//		} else if (button==PokemonUnity.Input.C) {
			//			selection=trtype[0];
			//			if (selection<0) {
			//				TrainerTypes newid=TrainerTypeEditorNew(null);
			//				if (newid>=0) {
			//					selection=newid;
			//				}
			//			} else {
			//				TrainerMetaData data=new TrainerMetaData();
			//				for (int i = 1; i < trtype.Length; i++) {
			//					data.Add(trtype[i]);
			//				}
			//				//  trtype[2] contains trainer's name to display as title
			//				bool save=PropertyList(trtype[2],data,trainerTypes,true);
			//				if (save) {
			//					TrainerTypeEditorSave(selection,data);
			//				}
			//			}
			//		}
			//	}
			//});
		}



		public void TrainerBattleEditor() {
			//int selection=0;
			//trainers=load_data("Data/trainers.dat");
			//trainertypes=load_data("Data/trainertypes.dat");
			//bool modified=false;
			//foreach (var trainer in trainers) {
			//	trtype=trainer[0];
			//	if (trainertypes==null || trainertypes[trtype]==null) {
			//		trainer[0]=0;
			//		modified=true;
			//	}
			//}
			//if (modified) {
			//	Kernal.save_data(trainers,"Data/trainers.dat");
			//	ConvertTrainerData();
			//}
			//ListScreenBlock(Game._INTL("Trainer Battles"),new TrainerBattleLister(selection,true), action: (button,trtype) => {
			//	Character.TrainerData data = null;
			//	if (trtype!=null) {
			//		int index=trtype[0];
			//		trainerdata=trtype[1];
			//		if (button==PokemonUnity.Input.A) {
			//			if (index>=0) {
			//				if (Game.GameData.GameMessage.ConfirmMessageSerious("Delete this trainer battle?")) {
			//					data=load_data("Data/trainers.dat");
			//					data.delete_at(index);
			//					Kernal.save_data(data,"Data/trainers.dat");
			//					ConvertTrainerData();
			//					Game.GameData.GameMessage.Message(Game._INTL("The Trainer battle was deleted."));
			//				}
			//			}
			//		} else if (button==PokemonUnity.Input.C) {
			//			selection=index;
			//			if (selection<0) {
			//				int ret=Game.GameData.GameMessage.Message(Game._INTL("First, define the type of trainer."),new string[] {
			//					Game._INTL("Use existing type"),Game._INTL("Use new type"),Game._INTL("Cancel") },3);
			//				TrainerTypes trainertype=-1;
			//				string trainername="";
			//				if (ret==0) {
			//					trainertype=ListScreen(Game._INTL("Trainer Type"),new TrainerTypeLister(0,false));
			//					if (trainertype==null) continue;
			//					trainertype=trainertype[0];
			//					if (trainertype<0) continue;
			//				} else if (ret==1) {
			//					trainertype=TrainerTypeEditorNew(null);
			//					if (trainertype<0) continue;
			//				} else {
			//					continue;
			//				}
			//				trainername=Game.GameData.GameMessage.MessageFreeText(Game._INTL("Now enter the trainer's name."),"",false,32);
			//				if (trainername=="") continue;
			//				trainerparty=GetFreeTrainerParty(trainertype,trainername);
			//				if (trainerparty<0) {
			//					Game.GameData.GameMessage.Message(Game._INTL("There is no room to create a trainer of that type and name."));
			//					continue;
			//				}
			//				// ##############
			//				NewTrainer(trainertype,trainername,trainerparty);
			//			} else {
			//				data=new string[] {
			//					trainerdata[0],		// Trainer type
			//					trainerdata[1],		// Trainer name
			//					trainerdata[4],		// ID
			//					trainerdata[3][0],	// Pokémon 1
			//					trainerdata[3][1],	// Pokémon 2
			//					trainerdata[3][2],	// Pokémon 3
			//					trainerdata[3][3],	// Pokémon 4
			//					trainerdata[3][4],	// Pokémon 5
			//					trainerdata[3][5],	// Pokémon 6
			//					trainerdata[2][0],	// Item 1
			//					trainerdata[2][1],	// Item 2
			//					trainerdata[2][2],	// Item 3
			//					trainerdata[2][3],	// Item 4
			//					trainerdata[2][4],	// Item 5
			//					trainerdata[2][5],	// Item 6
			//					trainerdata[2][6],	// Item 7
			//					trainerdata[2][7]	// Item 8
			//				};
			//				bool save=false;
			//				while (true) {
			//					//data=TrainerBattleProperty.set(trainerdata[1],data);
			//					//if (data) {
			//					//	ITrainer trainerdata=new string[] {
			//					//		data[0],
			//					//		data[1],
			//					//		[data[9],data[10],data[11],data[12],data[13],data[14],data[15],data[16]].find_all {|i| i && i!=0 },   // Item list
			//					//		[data[3],data[4],data[5],data[6],data[7],data[8]].find_all {|i| i && i[TPSPECIES]!=0 },   // Pokémon list
			//					//		data[2]
			//					//	};
			//					//	if (trainerdata[3].Length==0) {
			//					//		Game.GameData.GameMessage.Message(Game._INTL("Can't save. The Pokémon list is empty."));
			//					//	} else if (!trainerdata[1] || trainerdata[1].Length==0) {
			//					//		Game.GameData.GameMessage.Message(Game._INTL("Can't save. No name was entered."));
			//					//	} else {
			//					//		save=true;
			//					//		break;
			//					//	}
			//					//} else {
			//						break;
			//					//}
			//				}
			//				if (save) {
			//					data=load_data("Data/trainers.dat");
			//					data[index]=trainerdata;
			//					Kernal.save_data(data,"Data/trainers.dat");
			//					ConvertTrainerData();
			//				}
			//			}
			//		}
			//	}
			//});
		}

		public Items ChooseBallList(Items defaultMoveID=0) { //-1
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Items,string> commands=new SortedDictionary<Items,string>();
			int moveDefault=0;
			//foreach (var key in $BallTypes.keys) {
			foreach (Items item in BallHandlers.BallTypes) {
				//Items item=getID(Items,$BallTypes[key]);
				//if (item && item>0) commands.Add([key,item,item.ToString(TextScripts.Name)]);
				if (item>0) commands.Add(item,item.ToString(TextScripts.Name));
			}
			//commands.sort! {|a,b| a[2]<=>b[2]}
			if (defaultMoveID>=0) {
				//for (int i = 0; i < commands.Count; i++) {
				for (int i = 0; i < BallHandlers.BallTypes.Length; i++) {
					//if (defaultMoveID==commands[i][0]) moveDefault=i;
					if (defaultMoveID==BallHandlers.BallTypes[i]) moveDefault=i;
				}
			}
			IList<string> realcommands = new List<string>();
			foreach (KeyValuePair<Items,string> i in commands) {
				realcommands.Add(i.Value);
			}
			int ret=Commands2(cmdwin,realcommands,-1,moveDefault,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret][0] : defaultMoveID;
			return ret>=0 ? commands.ElementAt(ret).Key : defaultMoveID;
		}


		public IList<Moves> GetLegalMoves(Pokemons species) {
			IList<Moves> moves=new List<Moves>();
			if (species<=0) return moves; //!species ||
			//RgssOpen("Data/attacksRS.dat","rb") {|atkdata|
			//	int offset=atkdata.getOffset(species-1);
			//	int length=atkdata.getLength(species-1)>>1;
			//	//atkdata.pos=offset;
			//	for (int k = 0; k < length-1; k++) {
				for (int k = 0; k < Kernal.PokemonMovesData[species].LevelUp.Count-1; k++) {
					//int level=atkdata.fgetw;
					Moves move=Kernal.PokemonMovesData[species].LevelUp.Keys[k];
					moves.Add(move);
				}
			//}
			//itemData=readItemList("Data/items.dat");
			//tmdat=load_data("Data/tm.dat");
			//for (int i = 0; i < itemData.Length; i++) {
			for (int i = 0; i < Kernal.PokemonMovesData[species].Machine.Length-1; i++) {
				//if (!itemData[i]) continue;
				//Moves atk=itemData[i][8];
				Moves atk=Kernal.PokemonMovesData[species].Machine[i];
				//if (!atk || atk==0) continue;
				//if (!tmdat[atk]) continue;
				//if (tmdat[atk].any? {|item| item==species }) {
					moves.Add(atk);
				//}
			}
			//babyspecies=GetBabySpecies(species);
			//RgssOpen("Data/eggEmerald.dat","rb"){|f|
			for (int i = 0; i < Kernal.PokemonMovesData[species].Egg.Length-1; i++) {
				//f.pos=(babyspecies-1)*8;
				//offset=f.fgetdw;
				//length=f.fgetdw;
				//if (length>0) {
				//	f.pos=offset;
				//	i=0; do { unless (i<length) break; //loop
				//		atk=f.fgetw;
						Moves atk=Kernal.PokemonMovesData[species].Egg[i];
						moves.Add(atk);
				//		i+=1;
				//	}
				//}
			}
			//moves|=[];
			return moves;
		}

		public Moves ChooseMoveListForSpecies(Pokemons species,Moves defaultMoveID=0) {
			IWindow_CommandPokemon cmdwin=ListWindow(new string[0],200);
			IDictionary<Moves,string> commands=new SortedDictionary<Moves,string>();
			int moveDefault=0;
			IList<Moves> legalMoves=GetLegalMoves(species);
			foreach (Moves move in legalMoves) {
				commands.Add(move,move.ToString(TextScripts.Name));
			}
			//commands.sort! {|a,b| a[1]<=>b[1]}
			if (defaultMoveID>0) {
				//commands.each_with_index {|item,index|
				//foreach (KeyValuePair<Moves,string> index in commands) {
				for (int index=0;index<commands.Count;index++) {
					if (moveDefault==0) {
						//if (index[0]==defaultMoveID) moveDefault=index;
						//if (index[0]==defaultMoveID) moveDefault=index;
						if (commands.ElementAt(index).Key==defaultMoveID) moveDefault=index;
					}
				}
			}
			IDictionary<Moves,string> commands2=new SortedDictionary<Moves,string>();
			//for (int i = 1; i < Moves.maxValue; i++) {
			foreach (Moves i in Kernal.MoveData.Keys) {
				if (i.ToString(TextScripts.Name)!=null && i.ToString(TextScripts.Name)!="") {
					commands2.Add(i,i.ToString(TextScripts.Name));
				}
			}
			//commands2.sort! {|a,b| a[1]<=>b[1]}
			if (defaultMoveID>0) {
				//commands2.each_with_index {|item,index|
				//foreach (KeyValuePair<Moves,string> index in commands2) {
				for (int index=0;index<commands.Count;index++) {
					if (moveDefault==0) {
						//if (index[0]==defaultMoveID) moveDefault=index;
						if (commands.ElementAt(index).Key==defaultMoveID) moveDefault=index;
					}
				}
			}
			//commands.concat(commands2);
			IList<string> realcommands = new List<string>();
			foreach (KeyValuePair<Moves,string> command in commands) {
				realcommands.Add(string.Format("{2:s}",command.Key,command.Value));
			}
			int ret=Commands2(cmdwin,realcommands,-1,moveDefault,true);
			cmdwin.Dispose();
			//return ret>=0 ? commands[ret][0] : 0;
			return ret>=0 ? commands.ElementAt(ret).Key : 0;
		}


		public void MetadataScreen(int? defaultMapId=null) {
			//metadata=null;
			//mapinfos=LoadRxData("Data/MapInfos");
			//metadata=load_data("Data/metadata.dat");
			//int map=defaultMapId!=null ? defaultMapId.Value : 0;
			//do { //;loop
			//	map=ListScreen(Game._INTL("SET METADATA"),new MapLister(map,true));
			//	if (map<0) break;
			//	string mapname=(map==0) ? Game._INTL("Global Metadata") : mapinfos[map].name;
			//	data=[];
			//	properties=(map==0) ? MapScreenScene.GLOBALMETADATA :
			//							MapScreenScene.LOCALMAPS;
			//	for (int i = 0; i < properties.Length; i++) {
			//		data.Add(metadata[map] ? metadata[map][i+1] : null);
			//	}
			//	PropertyList(mapname,data,properties);
			//	for (int i = 0; i < properties.Length; i++) {
			//		if (!metadata[map]) {
			//			metadata[map]=[];
			//		}
			//		metadata[map][i+1]=data[i];
			//	}
			//} while(true);
			//if (metadata!=null) SerializeMetadata(metadata,mapinfos);
		}



		public void createRegionMap(int map) {
			//RgssOpen("Data/townmap.dat","rb"){|f|
			//	@mapdata=Marshal.load(f);
			//}
			//@map=@mapdata[map];
			//IAnimatedBitmap bitmap=new AnimatedBitmap("Graphics/Pictures/#{@map[1]}").deanimate;
			//retbitmap=new BitmapWrapper(bitmap.width/2,bitmap.height/2);
			//retbitmap.stretch_blt(
			//	new Rect(0,0,bitmap.width/2,bitmap.height/2),
			//	bitmap,
			//	new Rect(0,0,bitmap.width,bitmap.height)
			//);
			//bitmap.Dispose();
			//return retbitmap;
		}

		public IList<string> getMapNameList() {
			//RgssOpen("Data/townmap.dat","rb"){|f|
			//	@mapdata=Marshal.load(f);
			//}
			IList<string> ret=new List<string>();
			//for (int i = 0; i < @mapdata.Length; i++) {
			//	if (@mapdata[i]==null) continue;
			//	ret.Add(
			//		i,GetMessage(MessageTypes.RegionNames,i)
			//	);
			//}
			return ret;
		}

		public void createMinimap2(int mapid) {
			//map=load_data(string.Format("Data/Map%03d.rxdata",mapid)); //rescue null
			//if (!map) return new BitmapWrapper(32,32);
			//IBitmap bitmap=new BitmapWrapper(map.width*4,map.height*4);
			//IColor black=new Color(0,0,0);
			//IBitmap bigmap=(map.width>40 && map.height>40);
			//tilesets=load_data("Data/Tilesets.rxdata");
			//tileset=tilesets[map.tileset_id];
			//if (!tileset) return bitmap;
			//helper=TileDrawingHelper.fromTileset(tileset);
			//for (int y = 0; y < map.height; y++) {
			//	for (int x = 0; x < map.width; x++) {
			//		if (bigmap) {
			//			if ((x>8 && x<=map.width-8 && y>8 && y<=map.height-8)) continue;
			//		}
			//		for (int z = 0; z < 2; z++) {
			//			int id=map.data[x,y,z];
			//			if (id==0 || !id) continue;
			//			helper.bltSmallTile(bitmap,x*4,y*4,4,4,id);
			//		}
			//	}
			//}
			//bitmap.fill_rect(0,0,bitmap.width,1,black);
			//bitmap.fill_rect(0,bitmap.height-1,bitmap.width,1,black);
			//bitmap.fill_rect(0,0,1,bitmap.height,black);
			//bitmap.fill_rect(bitmap.width-1,0,1,bitmap.height,black);
			//return bitmap;
		}

		public void createMinimap(int mapid) {
			//map=load_data(string.Format("Data/Map%03d.rxdata",mapid)); //rescue null
			//if (!map) return new BitmapWrapper(32,32);
			//bitmap=new BitmapWrapper(map.width*4,map.height*4);
			//black=new Color(0,0,0);
			//tilesets=load_data("Data/Tilesets.rxdata");
			//tileset=tilesets[map.tileset_id];
			//if (!tileset) return bitmap;
			//helper=TileDrawingHelper.fromTileset(tileset);
			//for (int y = 0; y < map.height; y++) {
			//	for (int x = 0; x < map.width; x++) {
			//		for (int z = 0; z < 2; z++) {
			//			id=map.data[x,y,z];
			//			if (!id) id=0;
			//			helper.bltSmallTile(bitmap,x*4,y*4,4,4,id);
			//		}
			//	}
			//}
			//bitmap.fill_rect(0,0,bitmap.width,1,black);
			//bitmap.fill_rect(0,bitmap.height-1,bitmap.width,1,black);
			//bitmap.fill_rect(0,0,1,bitmap.height,black);
			//bitmap.fill_rect(bitmap.width-1,0,1,bitmap.height,black);
			//return bitmap;
		}

		public IPoint chooseMapPoint(int map,bool rgnmap=false) {
			//IViewport viewport=new Viewport(0,0,Graphics.width,Graphics.height);
			IViewport viewport=Viewport.initialize(0,0,Graphics.width,Graphics.height);
			//viewport.z=99999;
			//IWindow_UnformattedTextPokemon title=new Window_UnformattedTextPokemon(Game._INTL("Click a point on the map."));
			//title.x=0;
			//title.y=Graphics.height-64;
			//title.width=Graphics.width;
			//title.height=64;
			//title.viewport=viewport;
			//title.z=2;
			//ISprite sprite=null;
			//if (rgnmap) {
			//	sprite=new RegionMapSprite(map,viewport);
			//} else {
			//	sprite=new MapSprite(map,viewport);
			//}
			//sprite.z=2;
			IPoint ret=null;
			//do { //;loop
			//	Graphics.update();
			//	Input.update();
			//	IPoint xy=sprite.getXY();
			//	if (xy!=null) {
			//		ret=xy;
			//		break;
			//	}
			//	if (Input.trigger(PokemonUnity.Input.B)) {
			//		ret=null;
			//		break;
			//	}
			//} while (true);
			//sprite.Dispose();
			//title.Dispose();
			return ret;
		}




		public void EditorScreen() {
			//CriticalCode {
			//	IMapScreenScene mapscreen=new MapScreenScene();
			//	mapscreen.mapScreen();
			//	mapscreen.MapScreenLoop();
			//	mapscreen.close();
			//}
		}
		#endregion
	}
}


// ###############################################################################
// Make up internal names for things based on their actual names.
// ###############################################################################
#region Make up internal names for things based on their actual names.
public static partial class MakeshiftConsts {
	//@@consts=[];

	public static string get(int c,int i,string modname=null) {
		//if (!@@consts[c]) {
		//	@@consts[c]=[];
		//}
		//if (@@consts[c][i]) {
		//	return @@consts[c][i];
		//}
		//if (modname!=null) {
		//	string v=getConstantName(modname,i); //rescue null
		//	if (v!=null) {
		//		@@consts[c][i]=v;
		//		return v;
		//	}
		//}
		//string trname=GetMessage(c,i);
		//string trconst=trname.gsub(/é/,"e");
		//trconst=trconst.upcase;
		//trconst=trconst.gsub(/♀/,"fE");
		//trconst=trconst.gsub(/♂/,"mA");
		//trconst=trconst.gsub(/[^A-Za-z0-9_]/,"");
		//if (trconst.Length==0) {
		//	if (trname.Length==0) return null;
		//	trconst=string.Format("T_%03d",i);
		//} else if (!trconst[0,1][/[A-Z]/]) {
		//	trconst="T_"+trconst;
		//}
		//while (@@consts[c].Contains(trconst)) {
		//	trconst=string.Format("%s_%03d",trconst,i);
		//}
		//@@consts[c][i]=trconst;
		//return trconst;
		return "";
	}
}
#endregion



/*
// ###############################################################################
// General listers
// ###############################################################################
#region General listers
public partial class GraphicsLister : ILister {
	protected IIconSprite @sprite;
	protected int @index;
	protected string @folder;
	protected string @selection;
	protected IList<string> _commands;

	public ILister initialize(string folder,string selection) {
		//@sprite=new IconSprite(0,0);
		//@sprite.initialize(0,0);
		//@sprite.bitmap=null;
		//@sprite.z=2;
		this.folder=folder;
		this.selection=selection;
		_commands=new List<string>();
		@index=0;
		return this;
	}

	public void setViewport(IViewport viewport) {
		@sprite.viewport=viewport;
	}

	public int startIndex { get {
		return @index;
	} }

	public IList<string> commands { get {
		_commands.Clear();
		//Dir.chdir(@folder){
		//	Dir.glob("*.png"){|f|	_commands.Add(f) }
		//	Dir.glob("*.PNG"){|f|	_commands.Add(f) }
		//	Dir.glob("*.gif"){|f|	_commands.Add(f) }
		//	Dir.glob("*.GIF"){|f|	_commands.Add(f) }
		//	Dir.glob("*.bmp"){|f|	_commands.Add(f) }
		//	Dir.glob("*.BMP"){|f|	_commands.Add(f) }
		//	Dir.glob("*.jpg"){|f|	_commands.Add(f) }
		//	Dir.glob("*.JPG"){|f|	_commands.Add(f) }
		//	Dir.glob("*.jpeg"){|f|	_commands.Add(f) }
		//	Dir.glob("*.JPEG"){|f|	_commands.Add(f) }
		//}
		//@commands.sort!;
		int i = 0; do { //|i|
			if (_commands[i]==@selection) @index=i; i++;
		} while(i<_commands.Count);//@commands.Length.times
		if (_commands.Count==0) {
			Game.GameData.GameMessage.Message(Game._INTL("There are no files."));
		}
		return _commands;
	} }

	public object value(int index) {
		return (index<0) ? "" : @commands[index];
	}

	public void Dispose() {
		//if (@sprite.bitmap!=null) @sprite.bitmap.Dispose();
		@sprite.Dispose();
	}

	public void refresh(int index) {
		if (index<0) return;
		@sprite.setBitmap(@folder+@commands[index]);
		//float ww=@sprite.bitmap.width;
		//float wh=@sprite.bitmap.height;
		//float sx=(Graphics.width-256).to_f()/ww;
		//float sy=(Graphics.height-64).to_f()/wh;
		//if (sx<1.0 || sy<1.0) {
		//	if (sx>sy) {
		//		ww=sy*ww;
		//		wh=(Graphics.height-64).to_f();
		//	} else {
		//		wh=sx*wh;
		//		ww=(Graphics.width-256).to_f();
		//	}
		//}
		//@sprite.zoom_x=ww*1.0/@sprite.bitmap.width;
		//@sprite.zoom_y=wh*1.0/@sprite.bitmap.height;
		//@sprite.x=(Graphics.width-((Graphics.width-256)/2))-(ww/2);
		//@sprite.y=(Graphics.height-((Graphics.height-64)/2))-(wh/2);
	}
}

public partial class MusicFileLister : ILister {
	protected IAudioBGM @oldbgm;
	protected IAudioBGM _bgm;
	protected int @index=0;
	protected string _setting;
	protected IList<string> _commands;
	public IAudioBGM getPlayingBGM { get {
		return Game.GameData.GameSystem!=null ? Game.GameData.GameSystem.getPlayingBGM() : null;
	} }

	public void PlayBGM(IAudioBGM bgm) {
		//replaced with below...
		if (Game.GameData.GameSystem is IGameAudioPlay g)
		{
			if (bgm!=null) {
				g.BGMPlay(bgm);
			} else {
				g.BGMStop();
			}
		}
		if (Game.GameData.GameSystem is IGameSystem s)
			if (bgm!=null) {
				s.bgm_play(bgm);
			} else {
				s.bgm_stop();
			}
	}

	public void PlayBGM(string bgm) {
		if (string.IsNullOrEmpty(bgm)) return;
		IAudioBGM audio = null;
		if (Game.GameData.GameSystem is IGameAudioPlay gp)
		{
			audio = gp.ResolveAudioFile(bgm) as IAudioBGM;
			PlayBGM(audio);
		}
		//if (Game.GameData.GameSystem is IGameAudioPlay g)
		//	if (bgm!=null) {
		//		g.BGMPlay(bgm);
		//	} else {
		//		g.BGMStop();
		//	}
		//if (Game.GameData.GameSystem is IGameSystem s)
		//	if (bgm!=null) {
		//		s.bgm_play(bgm);
		//	} else {
		//		s.bgm_stop();
		//	}
	}

	public ILister initialize(IAudioBGM bgm,string setting) {
		@oldbgm=getPlayingBGM;
		_commands=new List<string>();
		_bgm=bgm;
		_setting=setting;
		@index=0;
		return this;
	}

	public int startIndex { get {
		return @index;
	} }

	public void setViewport(IViewport viewport) { }

	public IList<string> commands { get {
		string folder=(_bgm!=null) ? "Audio/BGM/" : "Audio/ME/";
		_commands.Clear();
		//Dir.chdir(folder){
		//	Dir.glob("*.mp3"){|f| @commands.Add(f) }
		//	Dir.glob("*.MP3"){|f| @commands.Add(f) }
		//	Dir.glob("*.ogg"){|f| @commands.Add(f) }
		//	Dir.glob("*.OGG"){|f| @commands.Add(f) }
		//	Dir.glob("*.wav"){|f| @commands.Add(f) }
		//	Dir.glob("*.WAV"){|f| @commands.Add(f) }
		//	Dir.glob("*.mid"){|f| @commands.Add(f) }
		//	Dir.glob("*.MID"){|f| @commands.Add(f) }
		//	Dir.glob("*.midi"){|f| @commands.Add(f) }
		//	Dir.glob("*.MIDI"){|f| @commands.Add(f) }
		//}
		//@commands.sort!;
		int i = 0; do { //|i|
			if (_commands[i]==_setting) @index=i;
		} while(i<_commands.Count); //@commands.Length.times
		if (_commands.Count==0) {
			Game.GameData.GameMessage.Message(Game._INTL("There are no files."));
		}
		return _commands;
	} }

	public object value(int index) {
		return (index<0) ? "" : _commands[index];
	}

	public void Dispose() {
		PlayBGM(@oldbgm);
	}

	public void refresh(int index) {
		if (index<0) return;
		if (_bgm!=null) {
			PlayBGM(_commands[index]);
		} else {
			PlayBGM("../../Audio/ME/"+_commands[index]);
		}
	}
}


public partial class MapLister : ILister {
	protected ISpriteWrapper @sprite;
	protected int @addGlobalOffset;
	protected int @index;
	protected IList<object> @maps;
	protected IList<string> _commands;

	public ILister initialize(int selmap,bool addGlobal=false) {
		@sprite=new SpriteWrapper();
		@sprite.bitmap=null;
		@sprite.z=2;
		_commands=new List<string>();
		if (Game.GameData is IGameDebug gd) @maps=gd.MapTree();
		@addGlobalOffset=(addGlobal) ? 1 : 0;
		@index=0;
		for (int i = 0; i < @maps.Count; i++) {
			if (@maps[i][0]==selmap) @index=i+@addGlobalOffset;
		}
		return this;
	}

	public void setViewport(IViewport viewport) {
		@sprite.viewport=viewport;
	}

	public int startIndex { get {
		return @index;
	} }

	public IList<string> commands { get {
		_commands.Clear();
		if (@addGlobalOffset==1) {
			_commands.Add(Game._INTL("[GLOBAL]"));
		}
		for (int i = 0; i < @maps.Count; i++) {
			_commands.Add(string.Format("%s%03d %s",("  "*@maps[i][2]),@maps[i][0],@maps[i][1]));
		}
		return _commands;
	} }

	public int value(int index) {
		if (@addGlobalOffset==1) {
			if (index==0) return 0;
		}
		return (index<0) ? -1 : @maps[index-@addGlobalOffset][0];
	}

	public void Dispose() {
		if (@sprite.bitmap!=null) @sprite.bitmap.Dispose();
		@sprite.Dispose();
	}

	public void refresh(int index) {
		if (@sprite.bitmap!=null) @sprite.bitmap.Dispose();
		if (index<0) return;
		if (index==0 && @addGlobalOffset==1) return;
		@sprite.bitmap=createMinimap(@maps[index-@addGlobalOffset][0]);
		@sprite.x=(Graphics.width-((Graphics.width-256)/2))-(@sprite.bitmap.width/2);
		@sprite.y=(Graphics.height-((Graphics.height-64)/2))-(@sprite.bitmap.height/2);
	}
}



public partial class ItemLister : ILister {
	protected IIconSprite @sprite;
	protected int _selection;
	protected IList<string> _commands;
	protected IList<Items> @ids;
	protected bool _includeNew;
	protected ITrainer @trainers=null;
	protected int @index=0;

	public ILister initialize(int selection,bool includeNew=false) {
		@sprite=new IconSprite(0,0);
		@sprite.bitmap=null;
		@sprite.z=2;
		_selection=selection;
		_commands=new List<string>();
		@ids=new List<Items>();
		_includeNew=includeNew;
		@trainers=null;
		@index=0;
		return this;
	}

	public void setViewport(IViewport viewport) {
		@sprite.viewport=viewport;
	}

	public int startIndex { get {
		return @index;
	} }

	public IList<string> commands() {	// Sorted alphabetically
		_commands.Clear();
		@ids.Clear();
		//@itemdata=readItemList("Data/items.dat");
		IDictionary<Items,string> cmds=new Dictionary<Items,string>();
		//for (int i = 1; i < Items.maxValue; i++) {
		foreach (KeyValuePair<Items,ItemData> i in Kernal.ItemData) {
			//string name=@itemdata[i][ITEMNAME];
			//if (name && name!="" && @itemdata[i][ITEMPOCKET]!=0) {
			//	cmds.Add(i,name);
			//}
		}
		//cmds.sort! {|a,b| a[1]<=>b[1]}
		if (_includeNew) {
			_commands.Add(string.Format("[NEW ITEM]"));
			@ids.Add(Items.NONE); //-1
		}
		foreach (KeyValuePair<Items,string> i in cmds) {
			_commands.Add(string.Format("{1:03d}: {2:s}",i.Key,i.Value));
			@ids.Add(i.Key);
		}
		@index=_selection;
		if (@index>=_commands.Count) @index=_commands.Count-1;
		if (@index<0) @index=0;
		return _commands;
	}

//=begin;
//	public void commands() {	// Sorted by item index number
//		@commands.Clear();
//		@ids.Clear();
//		@itemdata=readItemList("Data/items.dat");
//		if (@includeNew) {
//			@commands.Add(string.Format("[NEW ITEM]"));
//			@ids.Add(-1);
//		}
//		for (int i = 1; i < Items.maxValue; i++) {
//			//  Number: Item name
//			name=@itemdata[i][1];
//			if (name && name!="" && @itemdata[i][2]!=0) {
//				@commands.Add(string.Format("{1:3d}: {2:s}",i,name));
//				@ids.Add(i);
//			}
//		}
//		@index=@selection;
//		if (@index>=@commands.Length) @index=@commands.Length-1;
//		if (@index<0) @index=0;
//		return @commands;
//	}
//=end;

	public Items? value(int index) {
		if (index<0) return null;
		if (index==0 && _includeNew) return Items.NONE; //-1;
		int realIndex=index;
		return @ids[realIndex];
	}

	public void Dispose() {
		if (@sprite.bitmap!=null) @sprite.bitmap.Dispose();
		@sprite.Dispose();
	}

	public void refresh(int index) {
		if (@sprite.bitmap!=null) @sprite.bitmap.Dispose();
		if (index<0) return;
		try { //begin;
			string filename=ItemIconFile(@ids[index]);
			@sprite.setBitmap(filename,0);
		} catch (Exception) { //rescue;
			@sprite.setBitmap(null);
		}
		float ww=@sprite.bitmap.width;
		float wh=@sprite.bitmap.height;
		float sx=(Graphics.width-256).to_f()/ww;
		float sy=(Graphics.height-64).to_f()/wh;
		if (sx<1.0 || sy<1.0) {
			if (sx>sy) {
				ww=sy*ww;
				wh=(Graphics.height-64).to_f();
			} else {
				wh=sx*wh;
				ww=(Graphics.width-256).to_f();
			}
		}
		@sprite.zoom_x=ww*1.0/@sprite.bitmap.width;
		@sprite.zoom_y=wh*1.0/@sprite.bitmap.height;
		@sprite.x=(Graphics.width-((Graphics.width-256)/2))-(ww/2);
		@sprite.y=(Graphics.height-((Graphics.height-64)/2))-(wh/2);
	}
}



public partial class TrainerTypeLister : ILister {
	protected IIconSprite @sprite;
	protected TrainerTypes _selection;
	protected IList<string> _commands;
	protected IList<int> ids;
	protected bool _includeNew;
	protected ITrainer @trainers=null;
	protected int @index;

	public ILister initialize(TrainerTypes selection,bool includeNew) {
		//@sprite=new IconSprite(0,0);
		@sprite.initialize(0,0, null);
		@sprite.bitmap=null;
		@sprite.z=2;
		_selection=selection;
		_commands=new List<string>();
		@ids=new List<int>();
		_includeNew=includeNew;
		@trainers=null;
		@index=0;
		return this;
	}

	public void setViewport(IViewport viewport) {
		@sprite.viewport=viewport;
	}

	public int startIndex { get {
		return @index;
	} }

	public IList<string> commands() {
		_commands.Clear();
		@ids.Clear();
		@trainers=load_data("Data/trainertypes.dat");
		if (_includeNew) {
			_commands.Add(string.Format("[NEW TRAINER TYPE]"));
			@ids.Add(-1);
		}
		int i = 0; do {//|i|
			if (!@trainers[i]) continue;
			_commands.Add(string.Format("{1:3d}: {2:s}",i,@trainers[i][2]));
			@ids.Add(@trainers[i][0]); i++;
		} while(i<@trainers.Length); //@trainers.Length.times
		i = 0; do { //|i|
			if (@ids[i] == @selection) @index = i; i++;
		} while (i<_commands.Count); //@commands.Length.times
		return _commands;
	}

	public void value(int index) {
		if (index<0) return null;
		if (@ids[index]==-1) return [-1];
		return @trainers[@ids[index]];
	}

	public void Dispose() {
		if (@sprite.bitmap) @sprite.bitmap.Dispose();
		@sprite.Dispose();
	}

	public void refresh(int index) {
		if (@sprite.bitmap!=null) @sprite.bitmap.Dispose();
		if (index<0) return;
		try { //begin;
			@sprite.setBitmap(TrainerSpriteFile(@ids[index]),0);
		catch (Exception) { //rescue;
			@sprite.setBitmap(null);
		}
		float ww=@sprite.bitmap.width;
		float wh=@sprite.bitmap.height;
		float sx=(Graphics.width-256).to_f()/ww;
		float sy=(Graphics.height-64).to_f()/wh;
		if (sx<1.0 || sy<1.0) {
			if (sx>sy) {
				ww=sy*ww;
				wh=(Graphics.height-64).to_f();
			} else {
				wh=sx*wh;
				ww=(Graphics.width-256).to_f();
			}
		}
		@sprite.zoom_x=ww*1.0/@sprite.bitmap.width;
		@sprite.zoom_y=wh*1.0/@sprite.bitmap.height;
		@sprite.x=(Graphics.width-((Graphics.width-256)/2))-(ww/2);
		@sprite.y=(Graphics.height-((Graphics.height-64)/2))-(wh/2);
	}
}
#endregion



// ###############################################################################
// General properties
// ###############################################################################
#region General properties
public partial class UIntProperty : IProperty {
	protected int @maxdigits;
	//IChooseNumberParams param;

	public IProperty initialize(int maxdigits) {
		this.maxdigits=maxdigits;
		return this;
	}

	public int set(string settingname,int? oldsetting) {
		IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
		param.setMaxDigits(@maxdigits);
		param.setDefaultValue(oldsetting??0);
		return Game.GameData.GameMessage.MessageChooseNumber(
			Game._INTL("Set the value for {1}.",settingname),param);
	}

	public string format(string value) {
		//return value.inspect;
		return value;
	}

	public int defaultValue() {
		return 0;
	}
}

public partial class LimitProperty : IProperty {
	protected int @maxvalue;

	public IProperty initialize(int maxvalue) {
		this.maxvalue=maxvalue;
		return this;
	}

	public int set(string settingname,int? oldsetting) {
		if (oldsetting==null) oldsetting=1;
		IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
		param.setRange(0,@maxvalue);
		param.setDefaultValue(oldsetting.Value);
		int ret=Game.GameData.GameMessage.MessageChooseNumber(
			Game._INTL("Set the value for {1}.",settingname),param);
		return ret;
	}

	public string format(string value) {
		//return value.inspect;
		return value;
	}

	public int defaultValue() {
		return 0;
	}
}

public partial class NonzeroLimitProperty : IProperty {
	protected int @maxvalue;

	public IProperty initialize(int maxvalue) {
		this.maxvalue=maxvalue;
		return this;
	}

	public int set(string settingname,int? oldsetting) {
		if (oldsetting==null) oldsetting=1;
		IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
		param.setRange(1,@maxvalue);
		param.setDefaultValue(oldsetting.Value);
		int ret=Game.GameData.GameMessage.MessageChooseNumber(
			Game._INTL("Set the value for {1}.",settingname),param);
		return ret;
	}

	public string format(string value) {
		//return value.inspect;
		return value;
	}

	public int defaultValue() {
		return 0;
	}
}

public partial class ReadOnlyProperty {
	public static string set(string settingname,string oldsetting) {
		Game.GameData.GameMessage.Message(Game._INTL("This property cannot be edited."));
		return oldsetting;
	}

	public string format(string value) {
		//return value.inspect;
		return value;
	}
}



public static partial class UndefinedProperty {
	public static string set(string settingname,string oldsetting) {
		Game.GameData.GameMessage.Message(Game._INTL("This property can't be edited here at this time."));
		return oldsetting;
	}

	public string format(string value) {
		//return value.inspect;
		return value;
	}
}



public partial class EnumProperty {
	public void initialize(values) {
		@values=values;
	}

	public void set(string settingname,oldsetting) {
		commands=[];
		foreach (var value in @values) {
			commands.Add(value);
		}
		cmd=Game.GameData.GameMessage.Message(Game._INTL("Choose a value for {1}.",settingname),commands,-1);
		if (cmd<0) return oldsetting;
		return cmd;
	}

	public void defaultValue() {
		return 0;
	}

	public void format(value) {
		return value ? @values[value] : value.inspect;
	}
}



public static partial class BooleanProperty {
	public static string set(string settingname,string oldsetting) {
		return Game.GameData.GameMessage.ConfirmMessage(Game._INTL("Enable the setting {1}?",settingname)) ? true : false;
	}

	public static string format(string value) {
		//return value.inspect;
		return value;
	}
}



public static partial class StringProperty {
	public static void set(string settingname,string oldsetting) {
		string message=Game.GameData.GameMessage.MessageFreeText(Game._INTL("Set the value for {1}.",settingname),
			oldsetting!=null ? oldsetting : "",false,256,Graphics.width);
	}

	public static void format(value) {
		return value;
	}
}



public partial class LimitStringProperty {
	public void initialize(limit) {
		@limit=limit;
	}

	public string set(string settingname,string oldsetting) {
		string message=Game.GameData.GameMessage.MessageFreeText(Game._INTL("Set the value for {1}.",settingname),
			oldsetting ? oldsetting : "",false,@limit);
	}

	public void format(value) {
		return value;
	}
}



public static partial class BGMProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,new MusicFileLister(true,oldsetting));
		return chosenmap && chosenmap!="" ? chosenmap : oldsetting;
	}

	public static void format(value) {
		return value;
	}
}



public static partial class MEProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,new MusicFileLister(false,oldsetting));
		return chosenmap && chosenmap!="" ? chosenmap : oldsetting;
	}

	public static void format(value) {
		return value;
	}
}



public static partial class WindowskinProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,
			new GraphicsLister("Graphics/Windowskins/",oldsetting));
		return chosenmap && chosenmap!="" ? chosenmap : oldsetting;
	}

	public static void format(value) {
		return value;
	}
}



public static partial class TrainerTypeProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,
			new TrainerTypeLister(oldsetting,false));
		return chosenmap ? chosenmap[0] : oldsetting;
	}

	public static void format(value) {
		return !value ? value.inspect : value.ToString(TextScripts.Name);
	}
}



public static partial class SpeciesProperty {
	public static string set(string settingname,string oldsetting) {
		ret=ChooseSpeciesList(oldsetting ? oldsetting : 1);
		return (ret<=0) ? (oldsetting ? oldsetting : 0) : ret;
	}

	public static void format(value) {
		return value ? value.ToString(TextScripts.Name) : "-";
	}

	public static void defaultValue() {
		return 0;
	}
}



public static partial class TypeProperty {
	public static string set(string settingname,string oldsetting) {
		ret=ChooseTypeList(oldsetting ? oldsetting : 0);
		return (ret<0) ? (oldsetting ? oldsetting : 0) : ret;
	}

	public static void format(value) {
		return value ? value.ToString(TextScripts.Name) : "-";
	}

	public static void defaultValue() {
		return 0;
	}
}



public static partial class MoveProperty {
	public static string set(string settingname,string oldsetting) {
		ret=ChooseMoveList(oldsetting ? oldsetting : 1);
		return (ret<=0) ? (oldsetting ? oldsetting : 0) : ret;
	}

	public static void format(value) {
		return value ? value.ToString(TextScripts.Name) : "-";
	}

	public static void defaultValue() {
		return 0;
	}
}



public static partial class ItemProperty {
	public static string set(string settingname,string oldsetting) {
		ret=ChooseItemList(oldsetting ? oldsetting : 1);
		return (ret<=0) ? (oldsetting ? oldsetting : 0) : ret;
	}

	public static void format(value) {
		return value ? value.ToString(TextScripts.Name) : "-";
	}

	public static void defaultValue() {
		return 0;
	}
}



public static partial class NatureProperty {
	public static string set(string settingname,string oldsetting) {
		IList<string> commands=new List<string>();
		foreach (Natures i in Kernal.NatureData.Keys) { //int i = 0; do { //|i|(Natures.getCount).times
			commands.Add(i.ToString(TextScripts.Name));
		}
		string ret=Game.GameData.GameMessage.ShowCommands(null,commands.ToArray(),-1);
		return ret;
	}

	public static void format(value) {
		if (!value) return "";
		return (value>=0) ? getConstantName(Natures,value) : "";
	}

	public static void defaultValue() {
		return 0;
	}
}
#endregion



// ###############################################################################
// Trainer editor
// ###############################################################################
#region Trainer editor
public partial class TrainerBattleLister : ITrainerBattleLister, ILister {
	public void initialize(int selection,bool includeNew) {
		@sprite=new IconSprite();
		@sprite.bitmap=null;
		//@sprite.z=2;
		@selection=selection;
		@commands=[];
		@ids=[];
		@includeNew=includeNew;
		@trainers=null;
		@index=0;
	}

	public void setViewport(IViewport viewport) {
		@sprite.viewport=viewport;
	}

	public int startIndex() {
		return @index;
	}

	public string[] commands() {
		//@commands.Clear();
		//@ids.Clear();
		//@trainers=load_data("Data/trainers.dat");
		//if (@includeNew) {
		//	@commands.Add(string.Format("[NEW TRAINER BATTLE]"));
		//	@ids.Add(-1);
		//}
		//@trainers.Length.times do |i|
		//	if (!@trainers[i]) continue;
		//	//  Index: TrainerType TrainerName (version)
		//	@commands.Add(string.Format("{1:3d}: {2:s} {3:s} ({4:s})",i,
		//			@trainers[i][0].ToString(TextScripts.Name),@trainers[i][1],@trainers[i][4])); // Trainer's name must not be localized
		//	//  Trainer type ID
		//	@ids.Add(@trainers[i][0]);
		//}
		//@index=@selection;
		//if (@index>=@commands.Length) @index=@commands.Length-1;
		//if (@index<0) @index=0;
		return @commands;
	}

	public object value(int index) {
		if ((index<0)) return null;
		if (index==0 && @includeNew) return new KeyValuePair<int,ITrainer>(-1,null);
		int realIndex=(@includeNew) ? index-1 : index;
		return new KeyValuePair<int,ITrainer>(realIndex,@trainers[realIndex]);
	}

	public void Dispose() {
		//if (@sprite.bitmap) @sprite.bitmap.Dispose();
		//@sprite.Dispose();
	}

	public void refresh(int index) {
		//if (@sprite.bitmap) @sprite.bitmap.Dispose();
		//if (index<0) return;
		//try {//begin;
		//	@sprite.setBitmap(TrainerSpriteFile(@ids[index]),0);
		//} catch { //rescue;
		//	@sprite.setBitmap(null);
		//}
		//ww=@sprite.bitmap.width;
		//wh=@sprite.bitmap.height;
		//sx=(Graphics.width-256).to_f()/ww;
		//sy=(Graphics.height-64).to_f()/wh;
		//if (sx<1.0 || sy<1.0) {
		//	if (sx>sy) {
		//		ww=sy*ww;
		//		wh=(Graphics.height-64).to_f();
		//	} else {
		//		wh=sx*wh;
		//		ww=(Graphics.width-256).to_f();
		//	}
		//}
		//@sprite.zoom_x=ww*1.0/@sprite.bitmap.width;
		//@sprite.zoom_y=wh*1.0/@sprite.bitmap.height;
		//@sprite.x=(Graphics.width-((Graphics.width-256)/2))-(ww/2);
		//@sprite.y=(Graphics.height-((Graphics.height-64)/2))-(wh/2);
	}
}



public static partial class TrainerBattleProperty {
	public static string set(string settingname,string oldsetting) {
		if (!oldsetting) return oldsetting;
		//properties=new string[] {
		//   [Game._INTL("Trainer Type"),TrainerTypeProperty,
		//	   Game._INTL("Name of the trainer type for this Trainer.")],
		//   [Game._INTL("Trainer Name"),StringProperty,
		//	   Game._INTL("Name of the Trainer.")],
		//   [Game._INTL("Battle ID"),new LimitProperty(255),
		//	   Game._INTL("ID used to distinguish Trainers with the same name and trainer type.")],
		//   [Game._INTL("Pokémon 1"),TrainerPokemonProperty,
		//	   Game._INTL("First Pokémon.")],
		//   [Game._INTL("Pokémon 2"),TrainerPokemonProperty,
		//	   Game._INTL("Second Pokémon.")],
		//   [Game._INTL("Pokémon 3"),TrainerPokemonProperty,
		//	   Game._INTL("Third Pokémon.")],
		//   [Game._INTL("Pokémon 4"),TrainerPokemonProperty,
		//	   Game._INTL("Fourth Pokémon.")],
		//   [Game._INTL("Pokémon 5"),TrainerPokemonProperty,
		//	   Game._INTL("Fifth Pokémon.")],
		//   [Game._INTL("Pokémon 6"),TrainerPokemonProperty,
		//	   Game._INTL("Sixth Pokémon.")],
		//   [Game._INTL("Item 1"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 2"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 3"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 4"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 5"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 6"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 7"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")],
		//   [Game._INTL("Item 8"),ItemProperty,
		//	   Game._INTL("Item used by the trainer during battle.")]
		//};
		if (!PropertyList(settingname,oldsetting,properties,true)) {
			oldsetting=null;
		} else {
			if (!oldsetting[0] || oldsetting[0]==0) oldsetting=null;
		}
		return oldsetting;
	}

	public static string format(object value) {
		return value.inspect;
	}
}
#endregion




// ###############################################################################
// Trainer Pokémon editor
// ###############################################################################
#region Trainer Pokémon editor
public static partial class TrainerPokemonProperty {
	public static string set(string settingname,string oldsetting) {
		if (!oldsetting) oldsetting=TPDEFAULTS.clone();
		//properties=new string[] {
		//   [Game._INTL("Species"),SpeciesProperty,
		//	   Game._INTL("Species of the Pokémon.")],
		//   [Game._INTL("Level"),new NonzeroLimitProperty(Experiences.MAXLEVEL),
		//	   Game._INTL("Level of the Pokémon.")],
		//   [Game._INTL("Held item"),ItemProperty,
		//	   Game._INTL("Item held by the Pokémon.")],
		//   [Game._INTL("Move 1"),new MoveProperty2(oldsetting),
		//	   Game._INTL("First move. Leave all moves blank (use Z key) to give it a wild move set.")],
		//   [Game._INTL("Move 2"),new MoveProperty2(oldsetting),
		//	   Game._INTL("Second move. Leave all moves blank (use Z key) to give it a wild move set.")],
		//   [Game._INTL("Move 3"),new MoveProperty2(oldsetting),
		//	   Game._INTL("Third move. Leave all moves blank (use Z key) to give it a wild move set.")],
		//   [Game._INTL("Move 4"),new MoveProperty2(oldsetting),
		//	   Game._INTL("Fourth move. Leave all moves blank (use Z key) to give it a wild move set.")],
		//   [Game._INTL("Ability"),new LimitProperty(5),
		//	   Game._INTL("Ability flag. 0=first ability, 1=second ability, 2-5=hidden ability.")],
		//   [Game._INTL("Gender"),new LimitProperty(1),
		//	   Game._INTL("Gender flag. 0=male, 1=female.")],
		//   [Game._INTL("Form"),new LimitProperty(100),
		//	   Game._INTL("Form of the Pokémon.")],
		//   [Game._INTL("Shiny"),BooleanProperty,
		//	   Game._INTL("If set to true, the Pokémon is a different-colored Pokémon.")],
		//   [Game._INTL("Nature"),NatureProperty,
		//	   Game._INTL("Nature of the Pokémon.")],
		//   [Game._INTL("IVs"),new LimitProperty(31),
		//	   Game._INTL("Individual values of each of the Pokémon's stats.")],
		//   [Game._INTL("Happiness"),new LimitProperty(255),
		//	   Game._INTL("Happiness of the Pokémon.")],
		//   [Game._INTL("Nickname"),StringProperty,
		//	   Game._INTL("Name of the Pokémon.")],
		//   [Game._INTL("Shadow"),BooleanProperty,
		//	   Game._INTL("If set to true, the Pokémon is a Shadow Pokémon.")],
		//   [Game._INTL("Ball"),new BallProperty(oldsetting),
		//	   Game._INTL("Number of the Poké Ball the Pokémon is kept in.")]
		//};
		PropertyList(string settingname,oldsetting,properties,false);
		for (int i = 0; i < TPDEFAULTS.Length; i++) {
			if (!oldsetting[i]) oldsetting[i]=TPDEFAULTS[i];
		}
		moves=[];
		foreach (var i in [TPMOVE1,TPMOVE2,TPMOVE3,TPMOVE4]) {
			if (oldsetting[i]!=0) moves.Add(oldsetting[i]);
		}
		oldsetting[TPMOVE1]=moves[0] ? moves[0] : TPDEFAULTS[TPMOVE1];
		oldsetting[TPMOVE2]=moves[1] ? moves[1] : TPDEFAULTS[TPMOVE2];
		oldsetting[TPMOVE3]=moves[2] ? moves[2] : TPDEFAULTS[TPMOVE3];
		oldsetting[TPMOVE4]=moves[3] ? moves[3] : TPDEFAULTS[TPMOVE4];
		if (!oldsetting[TPSPECIES] || oldsetting[TPSPECIES]==0) oldsetting=null;
		return oldsetting;
	}

	public static void format(value) {
		return (!value || !value[TPSPECIES] || value[TPSPECIES]==0) ? "-" : value[TPSPECIES].ToString(TextScripts.Name);
	}
}



public partial class BallProperty {
	public void initialize(pokemondata) {
		@pokemondata=pokemondata;
	}

	public string set(string settingname,string oldsetting) {
		ret=ChooseBallList(oldsetting ? oldsetting : -1);
		return (ret<=0) ? (oldsetting ? oldsetting : 0) : ret;
	}

	public string format(Items value) {
		return value ? Items.getName(BallTypeToBall(value)) : "-";
	}

	public void defaultValue() {
		return 0;
	}
}




public partial class MoveProperty2 {
	public void initialize(pokemondata) {
		@pokemondata=pokemondata;
	}

	public string set(string settingname,string oldsetting) {
		ret=ChooseMoveListForSpecies(@pokemondata[0],oldsetting ? oldsetting : 1);
		return (ret<=0) ? (oldsetting ? oldsetting : 0) : ret;
	}

	public void format(Moves value) {
		return value ? value.ToString(TextScripts.Name) : "-";
	}

	public void defaultValue() {
		return 0;
	}
}
#endregion



// ###############################################################################
// Metadata editor
// ###############################################################################
#region Metadata editor
public static partial class CharacterProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,
		   new GraphicsLister("Graphics/Characters/",oldsetting));
		return chosenmap && chosenmap!="" ? chosenmap : oldsetting;
	}

	public static void format(value) {
		return value;
	}
}



public static partial class PlayerProperty {
	public static string set(string settingname,string oldsetting) {
		if (!oldsetting) oldsetting= new []{ 0,"xxx","xxx","xxx","xxx","xxx","xxx","xxx" };
		//properties=new string[] {
		//   [Game._INTL("Trainer Type"),TrainerTypeProperty,
		//	   Game._INTL("Trainer type of this player.")],
		//   [Game._INTL("Sprite"),CharacterProperty,
		//	   Game._INTL("Walking character sprite.")],
		//   [Game._INTL("Bike"),CharacterProperty,
		//	   Game._INTL("Cycling character sprite.")],
		//   [Game._INTL("Surfing"),CharacterProperty,
		//	   Game._INTL("Surfing character sprite.")],
		//   [Game._INTL("Running"),CharacterProperty,
		//	   Game._INTL("Running character sprite.")],
		//   [Game._INTL("Diving"),CharacterProperty,
		//	   Game._INTL("Diving character sprite.")],
		//   [Game._INTL("Fishing"),CharacterProperty,
		//	   Game._INTL("Fishing character sprite.")],
		//   [Game._INTL("Surf-Fishing"),CharacterProperty,
		//	   Game._INTL("Fishing while surfing character sprite.")]
		//};
		PropertyList(settingname,oldsetting,properties,false);
		return oldsetting;
	}

	public static void format(value) {
		return value.inspect;
	}
}



public static partial class MapSizeProperty {
	public static string set(string settingname,string oldsetting) {
		if (!oldsetting) oldsetting= new []{ 0,"" };
		properties=new string[] {
			[Game._INTL("Width"),new NonzeroLimitProperty(30),
				Game._INTL("The width of this map in Region Map squares.")],
			[Game._INTL("Valid Squares"),StringProperty,
				Game._INTL("A series of 1s and 0s marking which squares are part of this map (1=part, 0=not part).")],
		};
		PropertyList(settingname,oldsetting,properties,false);
		return oldsetting;
	}

	public static void format(value) {
		return value.inspect;
	}
}



public static partial class MapCoordsProperty {
  public static string set(string settingname,string oldsetting) {
	chosenmap=ListScreen(settingname,new MapLister(oldsetting ? oldsetting[0] : 0));
	if (chosenmap>=0) {
	  mappoint=chooseMapPoint(chosenmap);
	  if (mappoint) {
		return [chosenmap,mappoint[0],mappoint[1]];
	  } else {
		return oldsetting;
	  }
	} else {
	  return oldsetting;
	}
  }

  public static void format(value) {
	return value.inspect;
  }
}



public static partial class MapCoordsFacingProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,new MapLister(oldsetting ? oldsetting[0] : 0));
		if (chosenmap>=0) {
			mappoint=chooseMapPoint(chosenmap);
			if (mappoint) {
					facing=Game.GameData.GameMessage.Message(Game._INTL("Choose the direction to face in."),
						[Game._INTL("Down"),Game._INTL("Left"),Game._INTL("Right"),Game._INTL("Up")],-1);
				if (facing<0) {
					return oldsetting;
				} else {
					return [chosenmap,mappoint[0],mappoint[1],[2,4,6,8][facing]];
				}
			} else {
				return oldsetting;
			}
		} else {
		  return oldsetting;
		}
	}

	public static void format(value) {
		return value.inspect;
	}
}



public static partial class RegionMapCoordsProperty {
	public static string set(string settingname,string oldsetting) {
		regions=getMapNameList;
		selregion=-1;
		if (regions.Length==0) {
			Game.GameData.GameMessage.Message(Game._INTL("No region maps are defined."));
			return oldsetting;
		} else if (regions.Length==1) {
			selregion=regions[0][0];
		} else {
			cmds=[];
			foreach (var region in regions) {
				cmds.Add(region[1]);
			}
			selcmd=Game.GameData.GameMessage.Message(Game._INTL("Choose a region map."),cmds,-1);
			if (selcmd>=0) {
				selregion=regions[selcmd][0];
			} else {
				return oldsetting;
			}
		}
		mappoint=chooseMapPoint(selregion,true);
		if (mappoint) {
			return [selregion,mappoint[0],mappoint[1]];
		} else {
			return oldsetting;
		}
	}

	public static void format(value) {
		return value.inspect;
	}
}



public static partial class WeatherEffectProperty {
	public static string set(string settingname,string oldsetting) {
		options=[];
		for (int i = 0; i < FieldWeather.maxValue; i++) {
			options.Add(getConstantName(FieldWeather,i) || "ERROR");
		}
		cmd=Game.GameData.GameMessage.Message(Game._INTL("Choose a weather effect."),options,1);
		if (cmd==0) {
			return null;
		} else {
			params=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
			params.setRange(0,100);
			params.setDefaultValue(oldsetting ? oldsetting[1] : 100);
			number=Game.GameData.GameMessage.MessageChooseNumber(Game._INTL("Set the probability of the weather."),params);
			return [cmd,number];
		}
	}

	public static void format(value) {
		return value.inspect;
	}
}



public static partial class MapProperty {
	public static string set(string settingname,string oldsetting) {
		chosenmap=ListScreen(settingname,new MapLister(oldsetting ? oldsetting : 0));
		return chosenmap>0 ? chosenmap : oldsetting;
	}

	public static void format(value) {
		return value.inspect;
	}

	public static void defaultValue() {
		return 0;
	}
}
#endregion




// ###############################################################################
// Map drawing
// ###############################################################################
#region Map drawing
public partial class MapSprite {
	public void initialize(map,viewport=null) {
		@sprite=new Sprite(viewport);
		@sprite.bitmap=createMinimap(map);
		@sprite.x=(Graphics.width/2)-(@sprite.bitmap.width/2);
		@sprite.y=(Graphics.height/2)-(@sprite.bitmap.height/2);
	}

	public void dispose() {
		@sprite.bitmap.Dispose();
		@sprite.Dispose();
	}

	public void z=(value) {
		@sprite.z=value;
	}

	public void getXY() {
		if (!Input.triggerex(0x01)) return null;
		mouse=Mouse.eetMousePos(true);
		if (mouse[0]<@sprite.x||mouse[0]>=@sprite.x+@sprite.bitmap.width) {
			return null;
		}
		if (mouse[1]<@sprite.y||mouse[1]>=@sprite.y+@sprite.bitmap.height) {
			return null;
		}
		x=mouse[0]-@sprite.x;
		y=mouse[1]-@sprite.y;
		return [x/4,y/4];
	}
}



public partial class SelectionSprite : Sprite {
	public void initialize(viewport=null) {
		@sprite=new Sprite(viewport);
		@sprite.bitmap=null;
		@sprite.z=2;
		@othersprite=null;
	}

	public bool disposed() {
		return @sprite.disposed?;
	}

	public void dispose() {
		if (@sprite.bitmap) @sprite.bitmap.Dispose();
		@othersprite=null;
		@sprite.Dispose();
	}

	public void othersprite=(value) {
		@othersprite=value;
		if (@othersprite && !@othersprite.disposed? &&
		   @othersprite.bitmap && !@othersprite.bitmap.disposed?) {
		  @sprite.bitmap=DoEnsureBitmap(
			 @sprite.bitmap,@othersprite.bitmap.width,@othersprite.bitmap.height);
		  red=new Color(255,0,0);
		  @sprite.bitmap.Clear();
		  @sprite.bitmap.fill_rect(0,0,@othersprite.bitmap.width,2,red);
		  @sprite.bitmap.fill_rect(0,@othersprite.bitmap.height-2,
			 @othersprite.bitmap.width,2,red);
		  @sprite.bitmap.fill_rect(0,0,2,@othersprite.bitmap.height,red);
		  @sprite.bitmap.fill_rect(@othersprite.bitmap.width-2,0,2,
			 @othersprite.bitmap.height,red);
		}
	}

	public void update() {
		if (@othersprite && !@othersprite.disposed?) {
			@sprite.visible=@othersprite.visible;
			@sprite.x=@othersprite.x;
			@sprite.y=@othersprite.y;
		} else {
			@sprite.visible=false;
		}
	}
}



public partial class RegionMapSprite {
	public void initialize(map,viewport=null) {
		@sprite=new Sprite(viewport);
		@sprite.bitmap=createRegionMap(map);
		@sprite.x=(Graphics.width/2)-(@sprite.bitmap.width/2);
		@sprite.y=(Graphics.height/2)-(@sprite.bitmap.height/2);
	}

	public void dispose() {
		@sprite.bitmap.Dispose();
		@sprite.Dispose();
	}

	public void z=(value) {
		@sprite.z=value;
	}

	public void getXY() {
		if (!Input.triggerex(0x01)) return null;
		mouse=Mouse.eetMousePos(true);
		if (mouse[0]<@sprite.x||mouse[0]>=@sprite.x+@sprite.bitmap.width) {
			return null;
		}
		if (mouse[1]<@sprite.y||mouse[1]>=@sprite.y+@sprite.bitmap.height) {
			return null;
		}
		x=mouse[0]-@sprite.x;
		y=mouse[1]-@sprite.y;
		return [x/8,y/8];
	}
}
#endregion




// ###############################################################################
// Visual Editor (map connections)
// ###############################################################################
#region Visual Editor (map connections)
public partial class MapScreenScene {
public static readonly LOCALMAPS=new string[] {
   ["Outdoor",BooleanProperty,
		Game._INTL("If true, this map is an outdoor map and will be tinted according to time of day.")],
   ["ShowArea",BooleanProperty,
	   Game._INTL("If true, the game will display the map's name upon entry.")],
   ["Bicycle",BooleanProperty,
	   Game._INTL("If true, the bicycle can be used on this map.")],
   ["BicycleAlways",BooleanProperty,
	   Game._INTL("If true, the bicycle will be mounted automatically on this map and cannot be dismounted.")],
   ["HealingSpot",MapCoordsProperty,
		Game._INTL("Map ID of this Pokemon Center's town, and X and Y coordinates of its entrance within that town.")],
   ["Weather",WeatherEffectProperty,
	   Game._INTL("Weather conditions in effect for this map.")],
   ["MapPosition",RegionMapCoordsProperty,
	   Game._INTL("Identifies the point on the regional map for this map.")],
   ["DiveMap",MapProperty,
	   Game._INTL("Specifies the underwater layer of this map. Use only if this map has deep water.")],
   ["DarkMap",BooleanProperty,
	   Game._INTL("If true, this map is dark and a circle of light appears around the player. Flash can be used to expand the circle.")],
   ["SafariMap",BooleanProperty,
	   Game._INTL("If true, this map is part of the Safari Zone (both indoor and outdoor). Not to be used in the reception desk.")],
   ["SnapEdges",BooleanProperty,
	   Game._INTL("If true, when the player goes near this map's edge, the game doesn't center the player as usual.")],
   ["Dungeon",BooleanProperty,
	   Game._INTL("If true, this map has a randomly generated layout. See the wiki for more information.")],
   ["BattleBack",StringProperty,
	   Game._INTL("PNG files named 'battlebgXXX', 'enemybaseXXX', 'playerbaseXXX' in Battlebacks folder, where XXX is this property's value.")],
   ["WildBattleBGM",BGMProperty,
	   Game._INTL("Default BGM for wild Pokémon battles on this map.")],
   ["TrainerBattleBGM",BGMProperty,
	   Game._INTL("Default BGM for trainer battles on this map.")],
   ["WildVictoryME",MEProperty,
	   Game._INTL("Default ME played after winning a wild Pokémon battle on this map.")],
   ["TrainerVictoryME",MEProperty,
	   Game._INTL("Default ME played after winning a Trainer battle on this map.")],
   ["MapSize",MapSizeProperty,
	   Game._INTL("The width of the map in Town Map squares, and a string indicating which squares are part of this map.")],
 };
public static readonly GLOBALMETADATA=new string[] {
   ["Home",MapCoordsFacingProperty,
	   Game._INTL("Map ID and X and Y coordinates of where the player goes if no Pokémon Center was entered after a loss.")],
   ["WildBattleBGM",BGMProperty,
	   Game._INTL("Default BGM for wild Pokémon battles.")],
   ["TrainerBattleBGM",BGMProperty,
	   Game._INTL("Default BGM for Trainer battles.")],
   ["WildVictoryME",MEProperty,
	   Game._INTL("Default ME played after winning a wild Pokémon battle.")],
   ["TrainerVictoryME",MEProperty,
	   Game._INTL("Default ME played after winning a Trainer battle.")],
   ["SurfBGM",BGMProperty,
	   Game._INTL("BGM played while surfing.")],
   ["BicycleBGM",BGMProperty,
	   Game._INTL("BGM played while on a bicycle.")],
   ["PlayerA",PlayerProperty,
	   Game._INTL("Specifies player A.")],
   ["PlayerB",PlayerProperty,
	   Game._INTL("Specifies player B.")],
   ["PlayerC",PlayerProperty,
	   Game._INTL("Specifies player C.")],
   ["PlayerD",PlayerProperty,
	   Game._INTL("Specifies player D.")],
   ["PlayerE",PlayerProperty,
	   Game._INTL("Specifies player E.")],
   ["PlayerF",PlayerProperty,
	   Game._INTL("Specifies player F.")],
   ["PlayerG",PlayerProperty,
	   Game._INTL("Specifies player G.")],
   ["PlayerH",PlayerProperty,
	   Game._INTL("Specifies player H.")]
};

	public void getMapSprite(id) {
		if (!@mapsprites[id]) {
		  @mapsprites[id]=new Sprite(@viewport);
		  @mapsprites[id].z=0;
		  @mapsprites[id].bitmap=null;
		}
		if (!@mapsprites[id].bitmap || @mapsprites[id].bitmap.disposed?) {
		  @mapsprites[id].bitmap=createMinimap(id);
		}
		return @mapsprites[id];
	}

	public void close() {
		DisposeSpriteHash(@sprites);
		DisposeSpriteHash(@mapsprites);
		@viewport.Dispose();
	}

	public void setMapSpritePos(id,x,y) {
		sprite=getMapSprite(id);
		sprite.x=x;
		sprite.y=y;
		sprite.visible=true;
	}

	public void putNeighbors(id,sprites) {
		conns=@mapconns;
		mapsprite=getMapSprite(id);
		dispx=mapsprite.x;
		dispy=mapsprite.y;
		foreach (var conn in conns) {
			if (conn[0]==id) {
				b=sprites.any? {|i| i==conn[3] }
				if (!b) {
				  x=(conn[1]-conn[4])*4+dispx;
				  y=(conn[2]-conn[5])*4+dispy;
				  setMapSpritePos(conn[3],x,y);
				  sprites.Add(conn[3]);
				  putNeighbors(conn[3],sprites);
				}
			} else if (conn[3]==id) {
				b=sprites.any? {|i| i==conn[0] }
				if (!b) {
				  x=(conn[4]-conn[1])*4+dispx;
				  y=(conn[5]-conn[2])*4+dispy;
				  setMapSpritePos(conn[0],x,y);
				  sprites.Add(conn[3]);
				  putNeighbors(conn[0],sprites);
				}
			}
		}
	}

	public bool hasConnections (conns,id) {
		foreach (var conn in conns) {
		  if (conn[0]==id || conn[3]==id) return true;
		}
		return false;
	}

	public bool connectionsSymmetric (conn1,conn2) {
		if (conn1[0]==conn2[0]) {
			//  Equality
		  if (conn1[1]!=conn2[1]) return false;
		  if (conn1[2]!=conn2[2]) return false;
		  if (conn1[3]!=conn2[3]) return false;
		  if (conn1[4]!=conn2[4]) return false;
		  if (conn1[5]!=conn2[5]) return false;
		  return true;
		} else if (conn1[0]==conn2[3]) {
			//  Symmetry
		  if (conn1[1]!=-conn2[1]) return false;
		  if (conn1[2]!=-conn2[2]) return false;
		  if (conn1[3]!=conn2[0]) return false;
		  if (conn1[4]!=-conn2[4]) return false;
		  if (conn1[5]!=-conn2[5]) return false;
		  return true;
		}
		return false;
	}

	public void removeOldConnections(ret,mapid) {
		for (int i = 0; i < ret.Length; i++) {
		  if (ret[i][0]==mapid || ret[i][3]==mapid) ret[i]=null;
		}
		ret.compact!;
	}

	// Returns the maps within _keys_ that are directly connected to this map, _map_.
	public void getDirectConnections(keys,map) {
		thissprite=getMapSprite(map);
		thisdims=MapFactoryHelper.getMapDims(map);
		ret=[];
		foreach (var i in keys) {
			if (i==map) continue;
			othersprite=getMapSprite(i);
			otherdims=MapFactoryHelper.getMapDims(i);
			x1=(thissprite.x-othersprite.x)/4;
			y1=(thissprite.y-othersprite.y)/4;
			if ((x1==otherdims[0] || x1==-thisdims[0] ||
				  y1==otherdims[1] || y1==-thisdims[1])) {
				ret.Add(i);
			}
		}
		//  If no direct connections, add an indirect connection
		if (ret.Length==0) {
			key=(map==keys[0]) ? keys[1] : keys[0];
			ret.Add(key);
		}
		return ret;
	}

	public void generateConnectionData() {
		ret=[];
		//  Create a clone of current map connection
		foreach (var conn in @mapconns) {
		  ret.Add(conn.clone);
		}
		keys=@mapsprites.keys;
		if (keys.Length<2) return ret;
		//  Remove all connections containing any sprites on the canvas from the array
		foreach (var i in keys) {
		  removeOldConnections(ret,i);
		}
		//  Rebuild connections
		foreach (var i in keys) {
		  refs=getDirectConnections(keys,i);
		  foreach (var refmap in refs) {
			othersprite=getMapSprite(i);
			refsprite=getMapSprite(refmap);
			c1=(refsprite.x-othersprite.x)/4;
			c2=(refsprite.y-othersprite.y)/4;
			conn= new []{ refmap,0,0,i,c1,c2 };
			j=0;while j<ret.Length && !connectionsSymmetric(ret[j],conn);
			  j+=1;
			}
			if (j==ret.Length) {
			  ret.Add(conn);
			}
		  }
		}
		return ret;
	}

	public void serializeConnectionData() {
		conndata=generateConnectionData();
		SerializeConnectionData(conndata,@mapinfos);
		@mapconns=conndata;
	}

	public void putSprite(id) {
		addSprite(id);
		putNeighbors(id,[]);
	}

	public void addSprite(id) {
		mapsprite=getMapSprite(id);
		x=(Graphics.width-mapsprite.bitmap.width)/2;
		y=(Graphics.height-mapsprite.bitmap.height)/2;
		mapsprite.x=x.to_i&~3;
		mapsprite.y=y.to_i&~3;
	}

	public void saveMapSpritePos() {
		@mapspritepos.Clear();
		foreach (var i in @mapsprites.keys) {
		  s=@mapsprites[i];
		  if (s && !s.disposed?) @mapspritepos[i]=[s.x,s.y];
		}
	}

	public void mapScreen() {
		@sprites={}
		@mapsprites={}
		@mapspritepos={}
		@viewport=new Viewport(0,0,Graphics.width,Graphics.height);
		@viewport.z=99999;
		@lasthitmap=-1;
		@lastclick=-1;
		@oldmousex=null;
		@oldmousey=null;
		@dragging=false;
		@dragmapid=-1;
		@dragOffsetX=0;
		@dragOffsetY=0;
		@selmapid=-1;
		addBackgroundPlane(@sprites,"background","trainercardbg",@viewport);
		@sprites["selsprite"]=new SelectionSprite(@viewport);
		@sprites["title"]=new Window_UnformattedTextPokemon(Game._INTL("F5: Help"));
		@sprites["title"].x=0;
		@sprites["title"].y=Graphics.height-64;
		@sprites["title"].width=Graphics.width;
		@sprites["title"].height=64;
		@sprites["title"].viewport=@viewport;
		@sprites["title"].z=2;
		@mapinfos=load_data("Data/MapInfos.rxdata");
		@encdata=load_data("Data/encounters.dat");
		conns=MapFactoryHelper.getMapConnections;
		@mapconns=[];
		foreach (var c in conns) {
		  @mapconns.Add(c.clone);
		}
		@metadata=load_data("Data/metadata.dat");
		if (Game.GameData.GameMap) {
		  @currentmap=Game.GameData.GameMap.map_id;
		} else {
		  system=load_data("Data/System.rxdata");
		  @currentmap=system.edit_map_id;
		}
		putSprite(@currentmap);
	}

	public void setTopSprite(id) {
		foreach (var i in @mapsprites.keys) {
		  if (i==id) {
			@mapsprites[i].z=1;
		  } else {
			@mapsprites[i].z=0;
		  }
		}
	}

	public void getMetadata(mapid,metadataType) {
		if (@metadata[mapid]) return @metadata[mapid][metadataType];
	}

	public void setMetadata(mapid,metadataType,data) {
		if (!@metadata[mapid]) @metadata[mapid]=[];
		@metadata[mapid][metadataType]=data;
	}

	public void serializeMetadata() {
		SerializeMetadata(@metadata,@mapinfos);
	}

	public void helpWindow() {
		helptext=Game._INTL("A: Add map to canvas\r\n");
		helptext+=Game._INTL("DEL: Delete map from canvas\r\n");
		helptext+=Game._INTL("S: Go to another map\r\n");
		helptext+=Game._INTL("Click to select a map\r\n");
		helptext+=Game._INTL("Double-click: Edit map's metadata\r\n");
		helptext+=Game._INTL("E: Edit map's encounters\r\n");
		helptext+=Game._INTL("Drag map to move it\r\n");
		helptext+=Game._INTL("Arrow keys/drag canvas: Move around canvas");
		title=new Window_UnformattedTextPokemon(helptext);
		title.x=0;
		title.y=0;
		title.width=Graphics.width*8/10;
		title.height=Graphics.height;
		title.viewport=@viewport;
		title.z=2;
		do { //;loop
		  Graphics.update();
		  Input.update();
		  if (Input.trigger(PokemonUnity.Input.C)) break;
		  if (Input.trigger(PokemonUnity.Input.B)) break;
		}
		Input.update();
		title.Dispose();
	}

	public void propertyList(map,properties) {
		IDictionary<int, string> infos=load_data("Data/MapInfos.rxdata");
		string mapname=(map==0) ? Game._INTL("Global Metadata") : infos[map].name;
		data=[];
		for (int i = 0; i < properties.Length; i++) {
		  data.Add(getMetadata(map,i+1));
		}
		PropertyList(mapname,data,properties);
		for (int i = 0; i < properties.Length; i++) {
		  setMetadata(map,i+1,data[i]);
		}
	}

	public IRect getMapRect(mapid) {
		sprite=getMapSprite(mapid);
		if (sprite) {
		  return [
			 sprite.x,
			 sprite.y,
			 sprite.x+sprite.bitmap.width,
			 sprite.y+sprite.bitmap.height
		  ];
		} else {
		  return null;
		}
	}

	public void onDoubleClick(mapid) {
		if (mapid>=0) {
			propertyList(mapid,LOCALMAPS);
		} else {
			propertyList(0,GLOBALMETADATA);
		}
	}

	public void onClick(mapid,x,y) {
		if (@lastclick>0 && Graphics.frame_count-@lastclick<15) {
		  onDoubleClick(mapid);
		  @lastclick=-1;
		} else {
		  @lastclick=Graphics.frame_count;
		  if (mapid>=0) {
			@dragging=true;
			@dragmapid=mapid;
			sprite=getMapSprite(mapid);
			@sprites["selsprite"].othersprite=sprite;
			@selmapid=mapid;
			@dragOffsetX=sprite.x-x;
			@dragOffsetY=sprite.y-y;
			setTopSprite(mapid);
		  } else {
			@sprites["selsprite"].othersprite=null;
			@dragging=true;
			@dragmapid=mapid;
			@selmapid=-1;
			@dragOffsetX=x;
			@dragOffsetY=y;
			saveMapSpritePos;
		  }
		}
	}

	public void onRightClick(mapid,x,y) {
		//   echo("rightclick (#{mapid})\r\n")
	}

	public void onMouseUp(mapid) {
		//   echo("mouseup (#{mapid})\r\n")
		if (@dragging) @dragging=false;
	}

	public void onRightMouseUp(mapid) {
		//   echo("rightmouseup (#{mapid})\r\n")
	}

	public void onMouseOver(mapid,x,y) {
		//   echo("mouseover (#{mapid},#{x},#{y})\r\n")
	}

	public void onMouseMove(mapid,x,y) {
		//   echo("mousemove (#{mapid},#{x},#{y})\r\n")
		if (@dragging) {
		  if (@dragmapid>=0) {
			sprite=getMapSprite(@dragmapid);
			x=x+@dragOffsetX;
			y=y+@dragOffsetY;
			sprite.x=x&~3;
			sprite.y=y&~3;
			@sprites["title"].text=string.Format("F5: Help [{1:03d} {2:s}]",mapid,@mapinfos[@dragmapid].name);
		  } else {
			xpos=x-@dragOffsetX;
			ypos=y-@dragOffsetY;
			foreach (var i in @mapspritepos.keys) {
			  sprite=getMapSprite(i);
			  sprite.x=(@mapspritepos[i][0]+xpos)&~3;
			  sprite.y=(@mapspritepos[i][1]+ypos)&~3;
			}
			@sprites["title"].text=Game._INTL("F5: Help");
		  }
		} else {
		  if (mapid>=0) {
			@sprites["title"].text=string.Format("F5: Help [{1:03d} {2:s}]",mapid,@mapinfos[mapid].name);
		  } else {
			@sprites["title"].text=Game._INTL("F5: Help");
		  }
		}
	}

	public void hittest(x,y) {
		foreach (var i in @mapsprites.keys) {
		  sx=@mapsprites[i].x;
		  sy=@mapsprites[i].y;
		  sr=sx+@mapsprites[i].bitmap.width;
		  sb=sy+@mapsprites[i].bitmap.height;
		  if (x>=sx && x<sr && y>=sy && y<sb) return i;
		}
		return -1;
	}

	public void chooseMapScreen(title,currentmap) {
		return ListScreen(title,new MapLister(currentmap));
	}

	public void update() {
		mousepos=Mouse.eetMousePos;
		if (mousepos) {
		  hitmap=hittest(mousepos[0],mousepos[1]);
		  if (Input.triggerex(0x01)) {
			onClick(hitmap,mousepos[0],mousepos[1]);
		  } else if (Input.triggerex(0x02)) {
			onRightClick(hitmap,mousepos[0],mousepos[1]);
		  } else if (Input.releaseex(0x01)) {
			onMouseUp(hitmap);
		  } else if (Input.releaseex(0x02)) {
			onRightMouseUp(hitmap);
		  } else {
			if (@lasthitmap!=hitmap) {
			  onMouseOver(hitmap,mousepos[0],mousepos[1]);
			  @lasthitmap=hitmap;
			}
			if (@oldmousex!=mousepos[0]||@oldmousey!=mousepos[1]) {
			  onMouseMove(hitmap,mousepos[0],mousepos[1]);
			  @oldmousex=mousepos[0];
			  @oldmousey=mousepos[1];
			}
		  }
		}
		if (Input.press(PokemonUnity.Input.UP)) {
		  foreach (var i in @mapsprites) {
			if (!i) continue;
			i[1].y+=4;
		  }
		}
		if (Input.press(PokemonUnity.Input.DOWN)) {
		  foreach (var i in @mapsprites) {
			if (!i) continue;
			i[1].y-=4;
		  }
		}
		if (Input.press(PokemonUnity.Input.LEFT)) {
		  foreach (var i in @mapsprites) {
			if (!i) continue;
			i[1].x+=4;
		  }
		}
		if (Input.press(PokemonUnity.Input.RIGHT)) {
		  foreach (var i in @mapsprites) {
			if (!i) continue;
			i[1].x-=4;
		  }
		}
		if (Input.triggerex("A"[0])) {
		  id=chooseMapScreen(Game._INTL("Add Map"),@currentmap);
		  if (id>0) {
			addSprite(id);
			setTopSprite(id);
			@mapconns=generateConnectionData;
		  }
		} else if (Input.triggerex("S"[0])) {
		  id=chooseMapScreen(Game._INTL("Go to Map"),@currentmap);
		  if (id>0) {
			@mapconns=generateConnectionData;
			DisposeSpriteHash(@mapsprites);
			@mapsprites.Clear();
			@sprites["selsprite"].othersprite=null;
			@selmapid=-1;
			putSprite(id);
			@currentmap=id;
		  }
		} else if (Input.triggerex(0x2E)) {		// Delete
		  if (@mapsprites.keys.Length>1 && @selmapid>=0) {
			@mapsprites[@selmapid].bitmap.Dispose();
			@mapsprites[@selmapid].Dispose();
			@mapsprites.delete(@selmapid);
			@sprites["selsprite"].othersprite=null;
			@selmapid=-1;
		  }
		} else if (Input.triggerex("E"[0])) {
		  if (@selmapid>=0) EncounterEditorMap(@encdata,@selmapid);
		} else if (Input.trigger(PokemonUnity.Input.t5)) {
		  helpWindow;
		}
		UpdateSpriteHash(@sprites);
	}

	public void MapScreenLoop() {
		do { //;loop
			Graphics.update();
			Input.update();
			update();
			if (Input.trigger(PokemonUnity.Input.B)) {
				if (Game.GameData.GameMessage.ConfirmMessage(Game._INTL("Save changes?"))) {
					serializeConnectionData;
					serializeMetadata;
					Kernal.save_data(@encdata,"Data/encounters.dat");
					SaveEncounterData();
					ClearData;
				}
				if (Game.GameData.GameMessage.ConfirmMessage(Game._INTL("Exit from the editor?"))) break;
			}
		} while (true);
	}
}
#endregion
*/
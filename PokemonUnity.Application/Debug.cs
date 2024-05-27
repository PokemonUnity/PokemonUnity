using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public partial class PokemonDataCopy : IPokemonDataCopy {
		public string dataOldHash				{ get; protected set; }
		public string dataNewHash				{ get; protected set; }
		public DateTime dataTime				{ get; protected set; }
		public string data						{ get; protected set; }
		private string @datafile;
		private string @datasave;

		public string crc32(string x) {
			//return Zlib.brc32(x);
			return string.Empty;
		}

		public string readfile(string filename) {
				//File.open(filename, "rb"){|f|
				//	f.read;
				//}
				return string.Empty;
		}

		public void writefile(string str, string filename) {
			//File.open(filename, "wb"){|f|
			//	f.write(str);
			//}
		}

		public DateTime filetime(string filename) {
			//File.open(filename, "r"){|f|
			//	f.mtime;
			//}
			return DateTime.Now;
		}

		public void initialize(string data,string datasave) {
			this.datafile=data;
			this.datasave=datasave;
			this.data=readfile(@datafile);
			this.dataOldHash=crc32(@data);
			this.dataTime=filetime(@datafile);
		}

		public bool changed { get {
			string ts=readfile(@datafile);
			DateTime tsDate=filetime(@datafile);
			string tsHash=crc32(ts);
			return tsHash!=@dataNewHash && tsHash!=@dataOldHash && tsDate > @dataTime;
		} }

		public void save(string newtilesets) {
			//string newdata=Marshal.dump(newtilesets);
			//if (!changed) {
			//	@data=newdata;
			//	@dataNewHash=crc32(newdata);
			//	writefile(newdata,@datafile);
			//} else {
			//	@dataOldHash=crc32(@data);
			//	@dataNewHash=crc32(newdata);
			//	@dataTime=filetime(@datafile);
			//	@data=newdata;
			//	writefile(newdata,@datafile);
			//}
			//save_data(self,@datasave);
		}
	}

	public partial class PokemonDataWrapper : IPokemonDataWrapper {
		public int data				{ get; protected set; }
		private PokemonDataCopy @ts;

		public void initialize(string file,string savefile,string prompt) {
			//this.savefile=savefile;
			//this.file=file;
			//if (RgssExists(@savefile)) {
			//	@ts=load_data(@savefile);
			//	if (!@ts.changed? || prompt.call==true) {
			//		@data=Marshal.load(new StringInput(@ts.data));
			//	} else {
			//		@ts=new PokemonDataCopy(@file,@savefile);
			//		@data=load_data(@file);
			//	}
			//} else {
			//	@ts=new PokemonDataCopy(@file,@savefile);
			//	@data=load_data(@file);
			//}
		}

		public void save() {
			//@ts.save(@data);
		}
	}

	public partial class CommandList : ICommandList {
		protected IDictionary<string,int> @commandHash;
		protected IList<string> @commands;
		public void initialize() {
			@commandHash=new Dictionary<string, int>();
			@commands=new List<string>();
		}

		public string getCommand(int index) {
			foreach (string key in @commandHash.Keys) {
				if (@commandHash[key]==index) return key;
			}
			return null;
		}

		public void add(string key,string value) {
			@commandHash[key]=@commands.Count;
			@commands.Add(value);
		}

		public void list() {
			//@commands.Clone();
		}
	}

	//ToDo: Replace all `Game.GameData` with `this` while inside Game class?
	public partial class Game : IGameDebug {
		public IList<object> MapTree() {
			//object mapinfos=LoadRxData("Data/MapInfos");
			//IList<int[]> maplevels=new List<int[]>();
			IList<object> retarray=new List<object>();
			//foreach (var i in mapinfos.Keys) {
			//	info=mapinfos[i];
			//	int level=-1;
			//	while (info!=null) {
			//		info=mapinfos[info.parent_id];
			//		level+=1;
			//	}
			//	if (level>=0) {
			//		info=mapinfos[i];
			//		//maplevels.Add([i,level,info.parent_id,info.order]);
			//		maplevels.Add(new[] { i, level, info.parent_id, info.order });
			//	}
			//}
			////maplevels.sort!{|a,b|
			////	if (a[1]!=b[1]) next a[1]<=>b[1];	// level
			////	if (a[2]!=b[2]) next a[2]<=>b[2];	// parent ID
			////	next a[3]<=>b[3];					// order
			////}
			//Stack<int> stack=new Stack<int>();
			////stack.Add(0,0);
			//stack.Push(0); stack.Push(0);
			//while (stack.Count>0) {
			//	int parent = stack[stack.Count-1];
			//	int index = stack[stack.Count-2];
			//	if (index>=maplevels.Count) {
			//		stack.Pop();
			//		stack.Pop();
			//		continue;
			//	}
			//	IList<int> maplevel=maplevels[index];
			//	stack[stack.Count-2]+=1;
			//	if (maplevel[2]!=parent) {
			//		stack.Pop();
			//		stack.Pop();
			//		continue;
			//	}
			//	retarray.Add(new List<object> { maplevel[0], mapinfos[maplevel[0]].name, maplevel[1] });
			//	for (int i=index+1;i < maplevels.Count;i++) {
			//		if (maplevels[i][2]==maplevel[0]) {
			//			stack.Push(i);
			//			stack.Push(maplevel[0]);
			//			break;
			//		}
			//	}
			//}
			return retarray;
		}

		public void ExtractText() {
			//msgwindow=Kernal.CreateMessageWindow();
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,Game._INTL("Please wait.\\wtnp[0]"));
			//MessageTypes.extract("intl.txt");
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,
			//	Game._INTL("All text in the game was extracted and saved to intl.txt.\1"));
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,
			//	Game._INTL("To localize the text for a particular language, translate every second line in the file.\1"));
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,
			//	Game._INTL("After translating, choose \"Compile Text.\""));
			//Kernal.DisposeMessageWindow(msgwindow);
		}

		public void CompileTextUI() {
			//msgwindow=Kernal.CreateMessageWindow();
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,Game._INTL("Please wait.\\wtnp[0]"));
			//begin;
			//CompileText;
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,
			//	Game._INTL("Successfully compiled text and saved it to intl.dat."));
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,
			//	Game._INTL("To use the file in a game, place the file in the Data folder under a different name, and edit the LANGUAGES array in the Settings script."));
			//rescue RuntimeError;
			//Game.GameData.GameMessage.MessageDisplay(msgwindow,
			//	Game._INTL("Failed to compile text:  {1}",$!.message));
			//}
			//Kernal.DisposeMessageWindow(msgwindow);
		}


		public int DefaultMap() {
			if (Game.GameData.GameMap!=null) return Game.GameData.GameMap.map_id;
			if (Game.GameData.DataSystem!=null) return Game.GameData.DataSystem.edit_map_id;
			return 0;
		}

		public ITilePosition WarpToMap() {
			int mapid=ListScreen(Game._INTL("WARP TO MAP"),new MapLister(DefaultMap()));
			if (mapid>0) {
				//IGameMap map=new Game_Map();
				IGameMap map=Game.GameData.GameMap.initialize();
				map.setup(mapid);
				bool success=false;
				int x=0;
				int y=0;
				int i=0; do {
					x=Core.Rand.Next(map.width);
					y=Core.Rand.Next(map.height);
					if (!map.passableStrict(x,y,0,Game.GameData.GamePlayer)) continue;
					bool blocked=false;
					foreach (IGameCharacter @event in map.events.Values) {
						if (@event.x == x && @event.y == y && !@event.through) {
							if (this != Game.GameData.GamePlayer || @event.character_name != "") blocked=true;
						}
					}
					if (blocked) continue;
					success=true;
					break;
				} while (i<100); //100.times
				if (!success) {
					x=Core.Rand.Next(map.width);
					y=Core.Rand.Next(map.height);
				}
				return new TilePosition (mapid,x,y);
			}
			return null;
		}

		public void DebugMenu() {
			//IViewport viewport=new Viewport(0,0,Graphics.width,Graphics.height);
			IViewport viewport=Viewport.initialize(0,0,Graphics.width,Graphics.height);
			//viewport.z=99999;
			IDictionary<string,ISprite> sprites=new Dictionary<string,ISprite>();
			CommandList commands=new CommandList();
			commands.add("switches",Game._INTL("Switches"));
			commands.add("variables",Game._INTL("Variables"));
			commands.add("refreshmap",Game._INTL("Refresh Map"));
			commands.add("warp",Game._INTL("Warp to Map"));
			commands.add("healparty",Game._INTL("Heal Party"));
			commands.add("additem",Game._INTL("Add Item"));
			commands.add("fillbag",Game._INTL("Fill Bag"));
			commands.add("clearbag",Game._INTL("Empty Bag"));
			commands.add("addpokemon",Game._INTL("Add Pokémon"));
			commands.add("fillboxes",Game._INTL("Fill Storage Boxes"));
			commands.add("clearboxes",Game._INTL("Clear Storage Boxes"));
			commands.add("usepc",Game._INTL("Use PC"));
			commands.add("setplayer",Game._INTL("Set Player Character"));
			commands.add("renameplayer",Game._INTL("Rename Player"));
			commands.add("randomid",Game._INTL("Randomise Player's ID"));
			commands.add("changeoutfit",Game._INTL("Change Player Outfit"));
			commands.add("setmoney",Game._INTL("Set Money"));
			commands.add("setcoins",Game._INTL("Set Coins"));
			commands.add("setbadges",Game._INTL("Set Badges"));
			commands.add("demoparty",Game._INTL("Give Demo Party"));
			commands.add("toggleshoes",Game._INTL("Toggle Running Shoes Ownership"));
			commands.add("togglepokegear",Game._INTL("Toggle Pokégear Ownership"));
			commands.add("togglepokedex",Game._INTL("Toggle Pokédex Ownership"));
			commands.add("dexlists",Game._INTL("Dex List Accessibility"));
			commands.add("readyrematches",Game._INTL("Ready Phone Rematches"));
			commands.add("mysterygift",Game._INTL("Manage Mystery Gifts"));
			commands.add("daycare",Game._INTL("Day Care Options..."));
			commands.add("quickhatch",Game._INTL("Quick Hatch"));
			commands.add("roamerstatus",Game._INTL("Roaming Pokémon Status"));
			commands.add("roam",Game._INTL("Advance Roaming"));
			commands.add("setencounters",Game._INTL("Set Encounters")) ;
			commands.add("setmetadata",Game._INTL("Set Metadata")) ;
			commands.add("terraintags",Game._INTL("Set Terrain Tags"));
			commands.add("trainertypes",Game._INTL("Edit Trainer Types"));
			commands.add("resettrainers",Game._INTL("Reset Trainers"));
			commands.add("testwildbattle",Game._INTL("Test Wild Battle"));
			commands.add("testdoublewildbattle",Game._INTL("Test Double Wild Battle"));
			commands.add("testtrainerbattle",Game._INTL("Test Trainer Battle"));
			commands.add("testdoubletrainerbattle",Game._INTL("Test Double Trainer Battle"));
			commands.add("relicstone",Game._INTL("Relic Stone"));
			commands.add("purifychamber",Game._INTL("Purify Chamber"));
			commands.add("extracttext",Game._INTL("Extract Text"));
			commands.add("compiletext",Game._INTL("Compile Text"));
			commands.add("compiledata",Game._INTL("Compile Data"));
			commands.add("mapconnections",Game._INTL("Map Connections"));
			commands.add("animeditor",Game._INTL("Animation Editor"));
			commands.add("debugconsole",Game._INTL("Debug Console"));
			commands.add("togglelogging",Game._INTL("Toggle Battle Logging"));
			sprites["cmdwindow"]=new Window_CommandPokemonEx(commands.list);
			IWindow_CommandPokemonEx cmdwindow=(IWindow_CommandPokemonEx)sprites["cmdwindow"];
			cmdwindow.viewport=viewport;
			cmdwindow.resizeToFit(cmdwindow.commands);
			if (cmdwindow.height>Graphics.height) cmdwindow.height=Graphics.height;
			cmdwindow.x=0;
			cmdwindow.y=0;
			cmdwindow.visible=true;
			FadeInAndShow(sprites);
			int ret=-1;
			IChooseNumberParams param=null;
			do { //;loop
				do { //;loop
					cmdwindow.update();
					Graphics.update();
					Input.update();
					if (Input.trigger(PokemonUnity.Input.B)) {
						ret = -1;
						break;
					}
					if (Input.trigger(PokemonUnity.Input.C)) {
						ret = cmdwindow.index;
						break;
					}
				} while (true);
				if (ret==-1) break;
				string cmd=commands.getCommand(ret);
				if (cmd=="switches") {
					FadeOutIn(99999, block: () => { DebugScreen(0); });
				} else if (cmd=="variables") {
					FadeOutIn(99999, block: () => { DebugScreen(1); });
				} else if (cmd=="refreshmap") {
					Game.GameData.GameMap.need_refresh = true;
					Game.GameData.GameMessage.Message(Game._INTL("The map will refresh."));
				} else if (cmd=="warp") {
					ITilePosition map=WarpToMap();
					if (map!=null) {
						FadeOutAndHide(sprites);
						DisposeSpriteHash(sprites);
						viewport.Dispose();
						if (Game.GameData.Scene is ISceneMap) {
							Game.GameData.GameTemp.player_new_map_id=map.MapId;	//map[0];
							Game.GameData.GameTemp.player_new_x=map.X;			//map[1];
							Game.GameData.GameTemp.player_new_y=map.Y;			//map[2];
							Game.GameData.GameTemp.player_new_direction=2;
							Game.GameData.Scene.transfer_player();
							Game.GameData.GameMap.refresh();
						} else {
							if (Game.GameData is IGameField gf) gf.CancelVehicles();
							Game.GameData.MapFactory.setup(map.MapId);			//setup(map[0]);
							Game.GameData.GamePlayer.moveto(map.X,map.Y);		//moveto(map[1],map[2]);
							Game.GameData.GamePlayer.turn_down();
							Game.GameData.GameMap.update();
							Game.GameData.GameMap.autoplay();
							Game.GameData.GameMap.refresh();
						}
						return;
					}
				} else if (cmd=="healparty") {
					foreach (IPokemon i in Game.GameData.Trainer.party) {
						i.Heal();
					}
					Game.GameData.GameMessage.Message(Game._INTL("Your Pokémon were healed."));
				} else if (cmd=="additem") {
					Items item=ListScreen(Game._INTL("ADD ITEM"),new ItemLister(0));
					if (item!=Items.NONE && item>0) {
						param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
						param.setRange(1,Core.BAGMAXPERSLOT);
						param.setInitialValue(1);
						param.setCancelValue(0);
						int qty=Game.GameData.GameMessage.MessageChooseNumber(
							Game._INTL("Choose the number of items."),param
						);
						if (qty>0) {
							if (qty==1) {
								if (Game.GameData is IGameField gf) gf.ReceiveItem(item);
							} else {
								Game.GameData.GameMessage.Message(Game._INTL("The item was added."));
								Game.GameData.Bag.StoreItem(item,qty);
							}
						}
					}
				} else if (cmd=="fillbag") {
					param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
					param.setRange(1,Core.BAGMAXPERSLOT);
					param.setInitialValue(1);
					param.setCancelValue(0);
					int qty=Game.GameData.GameMessage.MessageChooseNumber(
						Game._INTL("Choose the number of items."),param
					);
					if (qty>0) {
						//IList<Items> itemconsts=new List<Items>();
						//foreach (var i in Items.constants) {
						//	itemconsts.Add(Items.const_get(i));
						//}
						//itemconsts.sort!{|a,b| a<=>b}
						//foreach (Items i in itemconsts) {
						foreach (Items i in Kernal.ItemData.Keys) {
							Game.GameData.Bag.StoreItem(i,qty);
						}
						Game.GameData.GameMessage.Message(Game._INTL("The Bag was filled with {1} of each item.",qty));
					}
				} else if (cmd=="clearbag") {
					Game.GameData.Bag.clear();
					Game.GameData.GameMessage.Message(Game._INTL("The Bag was cleared."));
				} else if (cmd=="addpokemon") {
					Pokemons species=ChooseSpeciesOrdered(1);
					if (species!=0) {
						param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
						param.setRange(1,Core.MAXIMUMLEVEL);
						param.setInitialValue(5);
						param.setCancelValue(0);
						int level=Game.GameData.GameMessage.MessageChooseNumber(
							Game._INTL("Set the Pokémon's level."),param);
						if (level>0) {
							AddPokemon(species,level);
						}
					}
				} else if (cmd=="fillboxes") {
					if (Game.GameData.Trainer.formseen==null) Game.GameData.Trainer.formseen=new int?[Kernal.PokemonData.Count][];
					if (Game.GameData.Trainer.formlastseen==null) Game.GameData.Trainer.formlastseen=new KeyValuePair<int,int?>[0];
					int added=0; bool completed=true;
					//for (int i = 1; i < Kernal.PokemonData.Count; i++) {
					foreach (Pokemons i in Kernal.PokemonData.Keys) {
						if (added>=Core.STORAGEBOXES*30) {
							completed=false; break;
						}
						string cname=i.ToString(TextScripts.Name);//getConstantName(Species,i); //rescue null
						if (cname==null) continue;
						//IPokemon pkmn=new Monster.Pokemon(i,50,Game.GameData.Trainer); //PokeBattle_Pokemon
						IPokemon pkmn=new Monster.Pokemon(i,Game.GameData.Trainer,50); //PokeBattle_Pokemon
						Game.GameData.PokemonStorage[((int)i-1)/Game.GameData.PokemonStorage.maxPokemon(0),
										((int)i-1)%Game.GameData.PokemonStorage.maxPokemon(0)]=pkmn;
						Game.GameData.Trainer.seen[i]=true;
						Game.GameData.Trainer.owned[i]=true;
						//if (Game.GameData.Trainer.formlastseen[i]==null) Game.GameData.Trainer.formlastseen[i]=new KeyValuePair<int,int?>(0,null);
						//if (Game.GameData.Trainer.formlastseen[i]==[]) Game.GameData.Trainer.formlastseen[i]=new KeyValuePair<int,int?>(0,null);
						//if (Game.GameData.Trainer.formseen[i]==null) Game.GameData.Trainer.formseen[i]=new int?[2];
						//for (int j = 0; j < 27; j++) { //ToDo: What is the 27 for?
						//	Game.GameData.Trainer.formseen[i][0][j]=true;
						//	Game.GameData.Trainer.formseen[i][1][j]=true;
						//}
						added+=1;
					}
					Game.GameData.GameMessage.Message(Game._INTL("Boxes were filled with one Pokémon of each species."));
					if (!completed) {
						Game.GameData.GameMessage.Message(Game._INTL("Note: The number of storage spaces ({1} boxes of 30) is less than the number of species.",Core.STORAGEBOXES));
					}
				} else if (cmd=="clearboxes") {
					for (int i = 0; i < Game.GameData.PokemonStorage.maxBoxes; i++) {
						for (int j = 0; j < Game.GameData.PokemonStorage.maxPokemon(i); j++) {
							Game.GameData.PokemonStorage[i,j]=null;
						}
					}
					Game.GameData.GameMessage.Message(Game._INTL("The Boxes were cleared."));
				} else if (cmd=="usepc") {
					PokeCenterPC();
				} else if (cmd=="setplayer") {
					int limit=0;
					for (int i = 0; i < 8; i++) {
						//meta=GetMetadata(0,MetadataPlayerA+i);
						MetadataPlayer? meta=GetMetadata(0).Global.Players[i];
						if (meta==null) {
							limit=i;
							break;
						}
					}
					if (limit<=1) {
						Game.GameData.GameMessage.Message(Game._INTL("There is only one player defined."));
					} else {
						param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
						param.setRange(0,limit-1);
						param.setDefaultValue(Game.GameData.Global.playerID);
						int newid=Game.GameData.GameMessage.MessageChooseNumber(
							Game._INTL("Choose the new player character."),param);
						if (newid!=Game.GameData.Global.playerID) {
							ChangePlayer(newid);
							Game.GameData.GameMessage.Message(Game._INTL("The player character was changed."));
						}
					}
				} else if (cmd=="renameplayer") {
					string trname=Game.GameData.Trainer.name;//Game.GameData.GameMessage.EnterPlayerName("Your name?",0,7,Game.GameData.Trainer.name);
					if (Game.GameData is IGameTextEntry gte) trname=gte.EnterPlayerName("Your name?",0,7,Game.GameData.Trainer.name);
					if (trname=="") {
						TrainerTypes trainertype=GetPlayerTrainerType();
						int gender=GetTrainerTypeGender(trainertype);
						trname=SuggestTrainerName(gender);
					}
					Game.GameData.Trainer.name=trname;
					Game.GameData.GameMessage.Message(Game._INTL("The player's name was changed to {1}.",Game.GameData.Trainer.name));
				} else if (cmd=="randomid") {
					Game.GameData.Trainer.id=Core.Rand.Next(256);
					Game.GameData.Trainer.id|=Core.Rand.Next(256)<<8;
					Game.GameData.Trainer.id|=Core.Rand.Next(256)<<16;
					Game.GameData.Trainer.id|=Core.Rand.Next(256)<<24;
					Game.GameData.GameMessage.Message(Game._INTL("The player's ID was changed to {1} (2).",Game.GameData.Trainer.publicID(),Game.GameData.Trainer.id));
				} else if (cmd=="changeoutfit") {
					int oldoutfit=Game.GameData.Trainer.outfit??0; //Added null conditional here...
					param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
					param.setRange(0,99);
					param.setDefaultValue(oldoutfit);
					Game.GameData.Trainer.outfit=Game.GameData.GameMessage.MessageChooseNumber(Game._INTL("Set the player's outfit."),param);
					if (Game.GameData.Trainer.outfit!=oldoutfit) Game.GameData.GameMessage.Message(Game._INTL("Player's outfit was changed."));
				} else if (cmd=="setmoney") {
					param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
					param.setMaxDigits(6);
					param.setDefaultValue(Game.GameData.Trainer.Money);
					Game.GameData.Trainer.Money=Game.GameData.GameMessage.MessageChooseNumber(
						Game._INTL("Set the player's money."),param);
					Game.GameData.GameMessage.Message(Game._INTL("You now have ${1}.",Game.GameData.Trainer.Money));
				} else if (cmd=="setcoins") {
					param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
					param.setRange(0,Core.MAXCOINS);
					param.setDefaultValue(Game.GameData.Global.coins);
					Game.GameData.Global.coins=Game.GameData.GameMessage.MessageChooseNumber(
						Game._INTL("Set the player's Coin amount."),param);
					Game.GameData.GameMessage.Message(Game._INTL("You now have {1} Coins.",Game.GameData.Global.coins));
				} else if (cmd=="setbadges") {
					int badgecmd=0;
					do { //;loop
						IList<string> badgecmds=new List<string>();
						for (int i = 0; i < 32; i++) {
							badgecmds.Add(Game._INTL("{1} Badge {2}",Game.GameData.Trainer.badges[i] ? "[Y]" : "[  ]",i+1));
						}
						badgecmd=Game.GameData.GameMessage.ShowCommands(null,badgecmds.ToArray(),-1,badgecmd);
						if (badgecmd<0) break;
						Game.GameData.Trainer.badges[badgecmd]=!Game.GameData.Trainer.badges[badgecmd];
					} while (true);
				} else if (cmd=="demoparty") {
					if (Game.GameData is IGameUtility gu) gu.CreatePokemon();
					Game.GameData.GameMessage.Message(Game._INTL("Filled party with demo Pokémon."));
				} else if (cmd=="toggleshoes") {
					Game.GameData.Global.runningShoes=!Game.GameData.Global.runningShoes;
					if (Game.GameData.Global.runningShoes) Game.GameData.GameMessage.Message(Game._INTL("Gave Running Shoes."));
					if (!Game.GameData.Global.runningShoes) Game.GameData.GameMessage.Message(Game._INTL("Lost Running Shoes."));
				} else if (cmd=="togglepokegear") {
					Game.GameData.Trainer.pokegear=!Game.GameData.Trainer.pokegear;
					if (Game.GameData.Trainer.pokegear) Game.GameData.GameMessage.Message(Game._INTL("Gave Pokégear."));
					if (!Game.GameData.Trainer.pokegear) Game.GameData.GameMessage.Message(Game._INTL("Lost Pokégear."));
				} else if (cmd=="togglepokedex") {
					Game.GameData.Trainer.pokedex=!Game.GameData.Trainer.pokedex;
					if (Game.GameData.Trainer.pokedex) Game.GameData.GameMessage.Message(Game._INTL("Gave Pokédex."));
					if (!Game.GameData.Trainer.pokedex) Game.GameData.GameMessage.Message(Game._INTL("Lost Pokédex."));
				} else if (cmd=="dexlists") {
					int dexescmd=0;int dexindex=0;
					do { //;loop
						IList<string> dexescmds=new List<string>();
						IList<string> d=Kernal.DexNames();
						for (int i = 0; i < d.Count; i++) {
							string name=d[i];
							//if (name is Array) name=name[0];
							dexindex=i;
							bool unlocked=Game.GameData.Global.pokedexUnlocked[dexindex];
							dexescmds.Add(Game._INTL("{1} {2}",unlocked ? "[Y]" : "[  ]",name));
						}
						dexescmd=Game.GameData.GameMessage.ShowCommands(null,dexescmds.ToArray(),-1,dexescmd);
						if (dexescmd<0) break;
						dexindex=dexescmd;
						if (Game.GameData.Global.pokedexUnlocked[dexindex]) {
							LockDex(dexindex);
						} else {
							UnlockDex(dexindex);
						}
					} while (true);
				} else if (cmd=="readyrematches") {
					if (Game.GameData.Global.phoneNumbers==null || Game.GameData.Global.phoneNumbers.Count==0) {
						Game.GameData.GameMessage.Message(Game._INTL("There are no trainers in the Phone."));
					} else {
						//foreach (IPhoneContact i in Game.GameData.Global.phoneNumbers) {
						for (int i=0;i < Game.GameData.Global.phoneNumbers.Count;i++) {
							//if (i.Length==8) {		// A trainer with an event
							//	i[4]=2;
							if (Game.GameData.Global.phoneNumbers[i] is IPhoneTrainerContact i4 && Game.GameData is IGamePhone gph) {		// A trainer with an event
								i4.CanBattle=PhoneBattleStatuses.IDLE;
								gph.SetReadyToBattle(i4);
							}
						}
						Game.GameData.GameMessage.Message(Game._INTL("All trainers in the Phone are now ready to rebattle."));
					}
				} else if (cmd=="mysterygift") {
					if (Game.GameData is IGameMysteryGift gmg) gmg.ManageMysteryGifts();
				} else if (cmd=="daycare") {
					int daycarecmd=0;
					do { //;loop
						string[] daycarecmds=new string[] {
							Game._INTL("Summary"),
							Game._INTL("Deposit Pokémon"),
							Game._INTL("Withdraw Pokémon"),
							Game._INTL("Generate egg"),
							Game._INTL("Collect egg"),
							Game._INTL("Dispose egg")
						};
						daycarecmd=Game.GameData.GameMessage.ShowCommands(null,daycarecmds,-1,daycarecmd);
						if (daycarecmd<0) break;
						if (Game.GameData.Global.daycare is IDayCare dc)
						switch (daycarecmd) {
							case 0: // Summary
								if (Game.GameData.Global.daycare!=null) {
									int num=dc.DayCareDeposited();
									Game.GameData.GameMessage.Message(Game._INTL("{1} Pokémon are in the Day Care.",num));
									if (num>0) {
										string txt="";
										for (int i = 0; i < num; i++) {
											//if (!Game.GameData.Global.daycare[i][0]) continue;
											//IPokemon pkmn=Game.GameData.Global.daycare[i][0];
											//int initlevel=Game.GameData.Global.daycare[i][1];
											if (!Game.GameData.Global.daycare[i].IsNotNullOrNone()) continue;
											IPokemon pkmn=Game.GameData.Global.daycare[i];
											int initlevel=Game.GameData.Global.daycare[i].Level;
											string gender=new string[] { Game._INTL("♂"), Game._INTL("♀"), Game._INTL("genderless") }[pkmn.IsGenderless?2:(pkmn.IsMale?0:1)];
											txt+=Game._INTL("{1}) {2} ({3}), Lv.{4} (deposited at Lv.{5})",
												i,pkmn.Name,gender,pkmn.Level,initlevel);
											if (i<num-1) txt+="\n";
										}
										Game.GameData.GameMessage.Message(txt);
									}
									if (Game.GameData.Global.daycareEgg) {//daycareEgg==1
										Game.GameData.GameMessage.Message(Game._INTL("An egg is waiting to be picked up."));
									} else if (dc.DayCareDeposited()==2) {
										if (dc.DayCareGetCompat()==0) {
											Game.GameData.GameMessage.Message(Game._INTL("The deposited Pokémon can't breed."));
										} else {
											Game.GameData.GameMessage.Message(Game._INTL("The deposited Pokémon can breed."));
										}
									}
								}
								break;
							case 1: // Deposit Pokémon
								if (dc.EggGenerated()) {
									Game.GameData.GameMessage.Message(Game._INTL("Egg is available, can't deposit Pokémon."));
								} else if (dc.DayCareDeposited()==2) {
									Game.GameData.GameMessage.Message(Game._INTL("Two Pokémon are deposited already."));
								} else if (Game.GameData.Trainer.party.Length==0) {
									Game.GameData.GameMessage.Message(Game._INTL("Party is empty, can't deposit Pokémon."));
								} else {
									ChooseNonEggPokemon(1,3);
									if ((int)Get(1)>=0) {
										dc.DayCareDeposit((int)Get(1));
										Game.GameData.GameMessage.Message(Game._INTL("Deposited {1}.",Get(3)));
									}
								}
								break;
							case 2: // Withdraw Pokémon
								if (dc.EggGenerated()) {
									Game.GameData.GameMessage.Message(Game._INTL("Egg is available, can't withdraw Pokémon."));
								} else if (dc.DayCareDeposited()==0) {
									Game.GameData.GameMessage.Message(Game._INTL("No Pokémon are in the Day Care."));
								} else if (Game.GameData.Trainer.party.Length>=6) {
									Game.GameData.GameMessage.Message(Game._INTL("Party is full, can't withdraw Pokémon."));
								} else {
									dc.DayCareChoose(Game._INTL("Which one do you want back?"),1);
									if ((int)Get(1)>=0) {
										dc.DayCareGetDeposited((int)Get(1),3,4);
										dc.DayCareWithdraw((int)Get(1));
										Game.GameData.GameMessage.Message(Game._INTL("Withdrew {1}.",Get(3)));
									}
								}
								break;
							case 3: // Generate egg
								if (Game.GameData.Global.daycareEgg) {//daycareEgg==1
									Game.GameData.GameMessage.Message(Game._INTL("An egg is already waiting."));
								} else if (dc.DayCareDeposited()!=2) {
									Game.GameData.GameMessage.Message(Game._INTL("There aren't 2 Pokémon in the Day Care."));
								} else if (dc.DayCareGetCompat()==0) {
									Game.GameData.GameMessage.Message(Game._INTL("The Pokémon in the Day Care can't breed."));
								} else {
									Game.GameData.Global.daycareEgg=true; //1;
									Game.GameData.GameMessage.Message(Game._INTL("An egg is now waiting in the Day Care."));
								}
								break;
							case 4: // Collect egg
								if (!Game.GameData.Global.daycareEgg) { //daycareEgg!=1
									Game.GameData.GameMessage.Message(Game._INTL("There is no egg available."));
								} else if (Game.GameData.Trainer.party.Length>=6) {
									Game.GameData.GameMessage.Message(Game._INTL("Party is full, can't collect the egg."));
								} else {
									dc.DayCareGenerateEgg();
									Game.GameData.Global.daycareEgg=false; //daycareEgg=0;
									Game.GameData.Global.daycareEggSteps=0;
									Game.GameData.GameMessage.Message(Game._INTL("Collected the {1} egg.",
										Game.GameData.Trainer.party[Game.GameData.Trainer.party.Length-1].Species.ToString(TextScripts.Name)));
								}
								break;
							case 5: // Dispose egg
								if (!Game.GameData.Global.daycareEgg) {//daycareEgg!=1
									Game.GameData.GameMessage.Message(Game._INTL("There is no egg available."));
								} else {
									Game.GameData.Global.daycareEgg=false;//daycareEgg=0;
									Game.GameData.Global.daycareEggSteps=0;
									Game.GameData.GameMessage.Message(Game._INTL("Disposed of the egg."));
								}
								break;
						}
					} while (true);
				} else if (cmd=="quickhatch") {
					foreach (var pokemon in Game.GameData.Trainer.party) {
						if (pokemon.isEgg) pokemon.EggSteps=1;
					}
					Game.GameData.GameMessage.Message(Game._INTL("All eggs on your party now require one step to hatch."));
				} else if (cmd=="roamerstatus") {
					if (Kernal.RoamingSpecies.Length==0) {
						Game.GameData.GameMessage.Message(Game._INTL("No roaming Pokémon defined."));
					} else {
						string text="\\l[8]";
						for (int i = 0; i < Kernal.RoamingSpecies.Length; i++) {
							RoamingEncounterData poke=Kernal.RoamingSpecies[i];
							if (Game.GameData.GameSwitches[poke.SwitchId]) {
								bool status=Game.GameData.Global.roamPokemon[i];
								if (status==true) {
									if (Game.GameData.Global.roamPokemonCaught[i]) {
										text+=Game._INTL("{1} (Lv.{2}) caught.",
											//Species.getName(getID(Species,poke[0])),poke[1]);
											poke.Pokemon,poke.Level);
									} else {
										text+=Game._INTL("{1} (Lv.{2}) defeated.",
											//Species.getName(getID(Species,poke[0])),poke[1]);
											poke.Pokemon,poke.Level);
									}
								} else {
									int curmap=Game.GameData.Global.roamPosition[i];
									if (curmap!=null) {
										mapinfos=RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
										text+=Game._INTL("{1} (Lv.{2}) roaming on map {3} ({4}){5}",
											//Species.getName(getID(Species,poke[0])),poke[1],curmap,
											poke.Pokemon,poke.Level,curmap,
											mapinfos[curmap].name,(curmap==Game.GameData.GameMap.map_id) ? Game._INTL("(this map)") : "");
									} else {
										text+=Game._INTL("{1} (Lv.{2}) roaming (map not set).",
											//Species.getName(getID(Species,poke[0])),poke[1]);
											poke.Pokemon,poke.Level);
									}
								}
							} else {
								text+=Game._INTL("{1} (Lv.{2}) not roaming (switch {3} is off).",
									//Species.getName(getID(Species,poke[0])),poke[1],poke[2]);
									poke.Pokemon,poke.Level);
							}
							if (i<Kernal.RoamingSpecies.Length-1) text+="\n";
						}
						Game.GameData.GameMessage.Message(text);
					}
				} else if (cmd=="roam") {
					if (Kernal.RoamingSpecies.Length==0) {
						Game.GameData.GameMessage.Message(Game._INTL("No roaming Pokémon defined."));
					} else {
						if(Game.GameData is IGameRoaming gr) gr.RoamPokemon(true);
						Game.GameData.Global.roamedAlready=false;
						Game.GameData.GameMessage.Message(Game._INTL("Pokémon have roamed."));
					}
				} else if (cmd=="setencounters") {
					IDictionary<int,IEncounters> encdata=null;//load_data("Data/encounters.dat");
					Kernal.load_data(out encdata, "Data/encounters.dat");
					//string oldencdata=Marshal.dump(encdata);
					bool mapedited=false;
					int map=DefaultMap();
					do { //;loop
						map=ListScreen(Game._INTL("SET ENCOUNTERS"),new MapLister(map));
						if (map<=0) break;
						if (map==DefaultMap()) mapedited=true;
						EncounterEditorMap(encdata,map);
					} while (true);
					Kernal.save_data(encdata,"Data/encounters.dat");
					SaveEncounterData();
					ClearData();
				} else if (cmd=="setmetadata") {
					MetadataScreen(DefaultMap());
					ClearData();
				//} else if (cmd=="terraintags") {
				//	FadeOutIn(99999, block: () => { TilesetScreen(); });
				} else if (cmd=="trainertypes") {
					FadeOutIn(99999, block: () => { TrainerTypeEditor(); });
				} else if (cmd=="resettrainers") {
					if (Game.GameData.GameMap!=null) {
						foreach (IGameCharacter @event in Game.GameData.GameMap.events.Values) {
							if (@event.name[/Trainer\(\d+\)/]) {
								Game.GameData.GameSelfSwitches[new SelfSwitchVariable(Game.GameData.GameMap.map_id,@event.id,"A")]=false;
								Game.GameData.GameSelfSwitches[new SelfSwitchVariable(Game.GameData.GameMap.map_id,@event.id,"B")]=false;
							}
						}
						Game.GameData.GameMap.need_refresh=true;
						Game.GameData.GameMessage.Message(Game._INTL("All Trainers on this map were reset."));
					} else {
						Game.GameData.GameMessage.Message(Game._INTL("This command can't be used here."));
					}
				} else if (cmd=="testwildbattle") {
					Pokemons species=ChooseSpeciesOrdered(1);
					if (species!=0) {
						param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
						param.setRange(1,Core.MAXIMUMLEVEL);
						param.setInitialValue(5);
						param.setCancelValue(0);
						int level=Game.GameData.GameMessage.MessageChooseNumber(
							Game._INTL("Set the Pokémon's level."),param);
						if (level>0) {
							WildBattle(species,level);
						}
					}
				} else if (cmd=="testdoublewildbattle") {
					Game.GameData.GameMessage.Message(Game._INTL("Choose the first Pokémon."));
					Pokemons species1=ChooseSpeciesOrdered(1);
					if (species1!=0) {
						param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
						param.setRange(1,Core.MAXIMUMLEVEL);
						param.setInitialValue(5);
						param.setCancelValue(0);
						int level1=Game.GameData.GameMessage.MessageChooseNumber(
							Game._INTL("Set the first Pokémon's level."),param);
						if (level1>0) {
							Game.GameData.GameMessage.Message(Game._INTL("Choose the second Pokémon."));
							Pokemons species2=ChooseSpeciesOrdered(1);
							if (species2!=0) {
								param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
								param.setRange(1,Core.MAXIMUMLEVEL);
								param.setInitialValue(5);
								param.setCancelValue(0);
								int level2=Game.GameData.GameMessage.MessageChooseNumber(
									Game._INTL("Set the second Pokémon's level."),param);
								if (level2>0) {
									DoubleWildBattle(species1,level1,species2,level2);
								}
							}
						}
					}
				} else if (cmd=="testtrainerbattle") {
					IBattle battle=ListScreen(Game._INTL("SINGLE TRAINER"),new TrainerBattleLister(0,false));
					if (battle!=null) {
						ITrainer trainerdata=battle.player[0];//battle[1]
						if (Game.GameData is IGameTrainer gt) gt.TrainerBattle(trainerdata.trainertype,trainerdata.name,"...",false,trainerdata.id,true);
					}
				} else if (cmd=="testdoubletrainerbattle") {
					IBattle battle1=ListScreen(Game._INTL("DOUBLE TRAINER 1"),new TrainerBattleLister(0,false));
					if (battle1!=null) {
						battle2=ListScreen(Game._INTL("DOUBLE TRAINER 2"),new TrainerBattleLister(0,false));
						if (battle2!=null) {
							ITrainer trainerdata1=battle1[1];
							ITrainer trainerdata2=battle2[1];
							if (Game.GameData is IGameTrainer gt) gt.DoubleTrainerBattle(trainerdata1.trainertype,trainerdata1.name,trainerdata1.id,"...",
												trainerdata2.trainertype,trainerdata2.name,trainerdata2.id,"...",
												true);
						}
					}
				} else if (cmd=="relicstone") {
					if (Game.GameData is IGameShadowPokemon gsp) gsp.RelicStone();
				} else if (cmd=="purifychamber") {
					if (Game.GameData is IGamePurifyChamber gpc) gpc.PurifyChamber();
				} else if (cmd=="extracttext") {
					ExtractText();
				} else if (cmd=="compiletext") {
					CompileTextUI();
				//} else if (cmd=="compiledata") {
				//	msgwindow=Kernal.CreateMessageWindow();
				//	//CompileAllData(true) {|msg| Game.GameData.GameMessage.MessageDisplay(msgwindow,msg,false) }
				//	Game.GameData.GameMessage.MessageDisplay(msgwindow,Game._INTL("All game data was compiled."));
				//	Kernal.DisposeMessageWindow(msgwindow);
				} else if (cmd=="mapconnections") {
					FadeOutIn(99999, block: () => { EditorScreen(); });
				//} else if (cmd=="animeditor") {
				//	FadeOutIn(99999, block: () => { AnimationEditor(); });
				} else if (cmd=="debugconsole") {
					//Console.setup_console(); //Open UI Panel?
				} else if (cmd=="togglelogging") {
					//Core.INTERNAL=!Core.INTERNAL; //ToDo: Make it runtime toggle?
					if (Core.INTERNAL) Game.GameData.GameMessage.Message(Game._INTL("Debug logs for battles will be made in the Data folder."));
					if (!Core.INTERNAL) Game.GameData.GameMessage.Message(Game._INTL("Debug logs for battles will not be made."));
				}
			} while (true);
			FadeOutAndHide(sprites);
			DisposeSpriteHash(sprites);
			viewport.Dispose();
		}

		public void DebugSetVariable(int id,int diff) {
			if (Game.GameData is IGameAudioPlay ap) ap.PlayCursorSE();
			if (Game.GameData.GameVariables[id]==null) Game.GameData.GameVariables[id]=0;
			if (Game.GameData.GameVariables[id] is int) { //Numeric
				Game.GameData.GameVariables[id]=Math.Min((int)Game.GameData.GameVariables[id]+diff,99999999);
				Game.GameData.GameVariables[id]=Math.Max((int)Game.GameData.GameVariables[id],-99999999);
			}
		}

		public void DebugVariableScreen(int id) {
			int value=0;
			if (Game.GameData.GameVariables[id] is int) {//Numeric
				value=(int)Game.GameData.GameVariables[id];
			}
			//IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();	//new ChooseNumberParams();
			IChooseNumberParams param=Game.GameData.ChooseNumberParams.initialize();
			param.setDefaultValue(value);
			param.setMaxDigits(8);
			param.setNegativesAllowed(true);
			value=Game.GameData.GameMessage.MessageChooseNumber(Game._INTL("Set variable {1}.",id),param);
			Game.GameData.GameVariables[id]=Math.Min(value,99999999);
			Game.GameData.GameVariables[id]=Math.Max((int)Game.GameData.GameVariables[id],-99999999);
		}

		public void DebugScreen(int mode) {
			//IViewport viewport=new Viewport(0,0,Graphics.width,Graphics.height);
			////viewport.z=99999;
			////sprites={}
			//sprites["right_window"] = new SpriteWindow_DebugRight();
			//ISpriteWindow_DebugRight right_window=sprites["right_window"];
			//right_window.mode=mode;
			//right_window.viewport=viewport;
			//right_window.active=true;
			//right_window.index=0;
			//FadeInAndShow(sprites);
			//do { //;loop
			//	Graphics.update();
			//	Input.update();
			//	UpdateSpriteHash(sprites);
			//	if (Input.trigger(PokemonUnity.Input.B)) {
			//		PlayCancelSE();
			//		break;
			//	}
			//	int current_id = right_window.index+1;
			//	if (mode == 0) {
			//		if (Input.trigger(PokemonUnity.Input.C)) {
			//			PlayDecisionSE();
			//			Game.GameData.GameSwitches[current_id] = (!Game.GameData.GameSwitches[current_id]);
			//			right_window.refresh();
			//		}
			//	} else if (mode == 1) {
			//		if (Input.repeat(PokemonUnity.Input.RIGHT)) {
			//			DebugSetVariable(current_id,1);
			//			right_window.refresh();
			//		} else if (Input.repeat(PokemonUnity.Input.LEFT)) {
			//			DebugSetVariable(current_id,-1);
			//			right_window.refresh();
			//		} else if (Input.trigger(PokemonUnity.Input.C)) {
			//			DebugVariableScreen(current_id);
			//			right_window.refresh();
			//		}
			//	}
			//} while (true);
			//FadeOutAndHide(sprites);
			//DisposeSpriteHash(sprites);
			//viewport.Dispose();
		}
	}




	//public partial class SpriteWindow_DebugRight : Window_DrawableCommand, IWindow_DrawableCommand {
	//	public int mode				{ get; protected set; }
	//
	//	public void initialize() {
	//		super(0, 0, Graphics.width, Graphics.height);
	//	}
	//
	//	public void shadowtext(x,y,w,h,t,align=0) {
	//		width=this.contents.text_size(t).width;
	//		if (align==2) {
	//			x+=(w-width);
	//		} else if (align==1) {
	//			x+=(w/2)-(width/2);
	//		}
	//		DrawShadowText(this.contents,x,y,[width,w].max,h,t,
	//			new Color(12*8,12*8,12*8),new Color(26*8,26*8,25*8));
	//	}
	//
	//	public void drawItem(index,count,rect) {
	//		SetNarrowFont(this.contents);
	//		if (@mode == 0) {
	//			name = Game.GameData.DataSystem.switches[index+1];
	//			status = Game.GameData.GameSwitches[index+1] ? "[ON]" : "[OFF]";
	//		} else {
	//			name = Game.GameData.DataSystem.variables[index+1];
	//			status = Game.GameData.GameVariables[index+1].ToString();
	//		}
	//		if (name == null) {
	//			name = '';
	//		}
	//		id_text = string.Format("%04d:", index+1);
	//		width = this.contents.text_size(id_text).width;
	//		rect=drawCursor(index,rect);
	//		totalWidth=rect.width;
	//		idWidth=totalWidth*15/100;
	//		nameWidth=totalWidth*65/100;
	//		statusWidth=totalWidth*20/100;
	//		this.shadowtext(rect.x, rect.y, idWidth, rect.height, id_text);
	//		this.shadowtext(rect.x+idWidth, rect.y, nameWidth, rect.height, name);
	//		this.shadowtext(rect.x+idWidth+nameWidth, rect.y, statusWidth, rect.height, status, 2);
	//	}
	//
	//	public void itemCount() {
	//		return (@mode==0) ? Game.GameData.DataSystem.switches.size-1 : Game.GameData.DataSystem.variables.size-1;
	//	}
	//
	//	public void mode=(mode) {
	//		@mode = mode;
	//		refresh();
	//	}
	//}




	public partial class Scene_Debug : ISceneDebug {
		public void main() {
			//Game.GameData.Graphics.transition(15);
			if (Game.GameData is IGameDebug gd) gd.DebugMenu();
			//Game.GameData.Scene=new Scene_Map();
			//Game.GameData.Scene.initialize();
			Game.GameData.GameMap.refresh();
			Game.GameData.Graphics.freeze();
		}
	}
}
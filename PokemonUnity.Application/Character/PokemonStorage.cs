using System;
using System.Collections.Generic;
using System.Text;
using PokemonUnity;
using PokemonUnity.Utility;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity
{
	namespace Character
	{
		public partial class PokemonBox : PokemonEssentials.Interface.Screen.IPokemonBox {
			public IList<PokemonEssentials.Interface.PokeBattle.IPokemon> pokemon				{ get; protected set; }
			public string name						{ get; set; }
			public string background				{ get; set; }

			public PokemonBox(string name, int maxPokemon = 30)
			{
				initialize(name, maxPokemon);
			}

			public IPokemonBox initialize (string name,int maxPokemon=30) {
				@pokemon=new List<PokemonEssentials.Interface.PokeBattle.IPokemon>();
				this.name=name;
				@background=null;
				for (int i = 0; i < maxPokemon; i++) {
					@pokemon[i]=null;
				}
				return this;
			}

			public bool full { get {
				//return (@pokemon.nitems==this.Length);
				return (this.nitems==this.length);
			} }

			public int nitems { get { //item count
				return @pokemon.Count;
			} }

			public int length { get { //capacity
				return (@pokemon as List<Pokemon>).Capacity;
			} }

			public IEnumerable<PokemonEssentials.Interface.PokeBattle.IPokemon> each() {
				foreach (PokemonEssentials.Interface.PokeBattle.IPokemon item in @pokemon) { yield return item; }
			}

			public PokemonEssentials.Interface.PokeBattle.IPokemon this[int i] { set {
					@pokemon[i]=value;
				}

				get {
					return @pokemon[i];
			} }

			public static implicit operator PokemonBox (PokemonEssentials.Interface.PokeBattle.IPokemon[] party)
			{
				IPokemonBox box = new PokemonBox("party", (Game.GameData as Game).Features.LimitPokemonPartySize);
				int i = 0;
				foreach(PokemonEssentials.Interface.PokeBattle.IPokemon p in party)
				{
					if(p.IsNotNullOrNone())
						box[i] = p;
					i++;
				}
				return (PokemonBox)box;
			}
			//public static implicit operator PokemonEssentials.Interface.PokeBattle.IPokemon[] (PokemonBox box)
			//{
			//	return box.pokemon.ToArray();
			//}
		}

		public partial class PokemonStorage : PokemonEssentials.Interface.Screen.IPokemonStorage {
			public IPokemonBox[] boxes				{ get; protected set; }
			//public IPokemon party					{ get; }
			public int currentBox					{ get; set; }
			private int boxmode						{ get; set; }

			public int maxBoxes { get {
				return @boxes.Length;
			} }

			public virtual PokemonEssentials.Interface.PokeBattle.IPokemon[] party{ get {
					return Game.GameData.Trainer.party;
				}
				set {
					//throw new Exception("Not supported");
					GameDebug.LogError("Not supported");
			} }

			/// <summary>
			/// ●, ▲, ■, ♥︎, ★, and ♦︎.
			/// </summary>
			public static string[] MARKINGCHARS=new string[] { "●", "▲", "■", "♥", "★", "♦︎" };

			public PokemonStorage(int maxBoxes = Core.STORAGEBOXES, int maxPokemon = 30) { initialize(maxBoxes, maxPokemon); }
			public PokemonEssentials.Interface.Screen.IPokemonStorage initialize (int maxBoxes=Core.STORAGEBOXES,int maxPokemon=30) {
				@boxes=new PokemonBox[maxBoxes];
				for (int i = 0; i < maxBoxes; i++) {
					int ip1=i+1;
					@boxes[i]=new PokemonBox(string.Format("Box {0}",ip1),maxPokemon);
					int backid=i%24;
					@boxes[i].background=$"box#{backid}";
				}
				@currentBox=0;
				@boxmode=-1;
				return this;
			}

			public int maxPokemon(int box) {
				if (box>=this.maxBoxes) return 0;
				return box<0 ? (Game.GameData as Game).Features.LimitPokemonPartySize : this[box].length;
			}

			public IPokemonBox this[int x] { get {
					return (x==-1) ? (IPokemonBox)(PokemonBox)this.party : @boxes[x];
				}
			}
			public PokemonEssentials.Interface.PokeBattle.IPokemon this[int x,int y] { get {
					//if (y==null) {
					//	return (x==-1) ? (IList<IPokemon)this.party : (IList<IPokemon)@boxes[x];
					//} else {
					//	foreach (var i in @boxes) {
					//		if (i is PokemonEssentials.Interface.PokeBattle.IPokemon) raise "Box is a Pokémon, not a box";
					//	}
						return (x == -1) ? this.party[y] : @boxes[x][y];
					//}
				}
				set {
					if (x==-1) {
						this.party[y]=value;
					} else {
						@boxes[x][y]=value;
					}
			} }

			public bool full { get {
				for (int i = 0; i < this.maxBoxes; i++) {
					if (!@boxes[i].full) return false;
				}
				return true;
			} }

			public int pbFirstFreePos(int box) {
				int ret = 0;
				if (box==-1) {
					//ret=this.party.nitems;
					ret=this.party.GetCount();
					return (ret==6) ? -1 : ret;
				} else {
					for (int i = 0; i < maxPokemon(box); i++) {
						if (!this[box,i].IsNotNullOrNone()) return i;
					}
					return -1;
				}
			}

			public bool pbCopy(int boxDst,int indexDst,int boxSrc,int indexSrc) {
				if (indexDst<0 && boxDst<this.maxBoxes) {
					bool found=false;
					for (int i = 0; i < maxPokemon(boxDst); i++) {
						if (!this[boxDst,i].IsNotNullOrNone()) {
							found=true;
							indexDst=i;
							break;
						}
					}
					if (!found) return false;
				}
				if (boxDst==-1) {
					if (((PokemonBox)this.party).nitems>=6) {
						//if (this.party.GetCount()>=6) {
						return false;
					}
					this.party[this.party.Length]=this[boxSrc,indexSrc];
					this.party.PackParty();
				} else {
					if (!this[boxSrc,indexSrc].IsNotNullOrNone()) {
						//throw new Exception("Trying to copy null to storage"); 
						GameDebug.LogWarning("Trying to copy null to storage"); // not localized
					}
					this[boxSrc,indexSrc].Heal();
					if (this[boxSrc,indexSrc] is IPokemonMultipleForms f && //this[boxSrc,indexSrc].respond_to("formTime") &&
						f.formTime != null) f.formTime=null;
					this[boxDst,indexDst]=this[boxSrc,indexSrc];
				}
				return true;
			}

			public bool pbMove(int boxDst,int indexDst,int boxSrc,int indexSrc) {
				if (!pbCopy(boxDst,indexDst,boxSrc,indexSrc)) return false;
				pbDelete(boxSrc,indexSrc);
				return true;
			}

			public void pbMoveCaughtToParty(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn) {
				if (((PokemonBox)this.party).nitems>=6) {
				//if (this.party.GetCount()>=6) {
					return;
				}
				this.party[this.party.Length]=pkmn;
			}

			public bool pbMoveCaughtToBox(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn,int box) {
				for (int i = 0; i < maxPokemon(box); i++) {
					if (this[box,i]==null) {
						if (box>=0) {
							pkmn.Heal();
							if (pkmn is IPokemonMultipleForms p && //pkmn.respond_to("formTime") && 
								p.formTime != null) p.formTime=null;
						}
						this[box,i]=pkmn;
						return true;
					}
				}
				return false;
			}

			public int pbStoreCaught(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn) {
				for (int i = 0; i < maxPokemon(@currentBox); i++) {
					if (this[@currentBox,i]==null) {
						this[@currentBox,i]=pkmn;
						return @currentBox;
					}
				}
				for (int j = 0; j < this.maxBoxes; j++) {
					for (int i = 0; i < maxPokemon(j); i++) {
						if (this[j,i]==null) {
							this[j,i]=pkmn;
							@currentBox=j;
							return @currentBox;
						}
					}
				}
				return -1;
			}

			public void pbDelete(int box,int index) {
				if (this[box,index].IsNotNullOrNone()) {
					this[box,index]=null;
					if (box==-1) {
						this.party.PackParty();
					}
				}
			}
		}

		public partial class PokemonStorageWithParty : PokemonStorage, PokemonEssentials.Interface.Screen.IPokemonStorageWithParty {
			public override PokemonEssentials.Interface.PokeBattle.IPokemon[] party { get {
					return base.party;
				}
				set {
					base.party=party;
			} }

			public PokemonStorageWithParty(int maxBoxes = 24, int maxPokemon = 30, PokemonEssentials.Interface.PokeBattle.IPokemon[] party = null) //: base (maxBoxes,maxPokemon)
			{ initialize(maxBoxes, maxPokemon, party); }
			public IPokemonStorageWithParty initialize(int maxBoxes=24,int maxPokemon=30,PokemonEssentials.Interface.PokeBattle.IPokemon[] party=null) 
			{
				base.initialize(maxBoxes,maxPokemon);
				if (party != null) {
					//ToDo: Feature not setup... 
					this.party=party;
				} else {
					@party=new Pokemon[(Game.GameData as Game).Features.LimitPokemonPartySize];
				}
				return this;
			}
		}

		// ###############################################################################
		// Regional Storage scripts
		// ###############################################################################
		public partial class RegionalStorage : IRegionalStorage {
			protected IList<IPokemonStorage> storages { get; set; }
			protected int lastmap { get; set; }
			protected int rgnmap { get; set; }

			public IRegionalStorage initialize() {
				@storages=new List<IPokemonStorage>();
				@lastmap=-1;
				@rgnmap=-1;
				return this;
			}

			public IPokemonStorage getCurrentStorage { get {
				if (Game.GameData.GameMap == null) {
					//throw Exception(Game._INTL("The player is not on a map, so the region could not be determined."));
					GameDebug.LogError(Game._INTL("The player is not on a map, so the region could not be determined."));
				}
				if (@lastmap!=Game.GameData.GameMap.map_id) {
					@rgnmap=Game.GameData is IGameUtility g ? g.pbGetCurrentRegion() : -1; // may access file IO, so caching result
					@lastmap=Game.GameData.GameMap.map_id;
				}
				if (@rgnmap<0) {
					//throw Exception(Game._INTL("The current map has no region set. Please set the MapPosition metadata setting for this map."));
					GameDebug.LogError(Game._INTL("The current map has no region set. Please set the MapPosition metadata setting for this map."));
				}
				if (@storages[@rgnmap] == null) {
					@storages[@rgnmap]=new PokemonStorage();
				}
				return @storages[@rgnmap];
			} }

			public IPokemonBox[] boxes { get {
				return getCurrentStorage.boxes;
			} }

			public PokemonEssentials.Interface.PokeBattle.IPokemon[] party { get {
				return getCurrentStorage.party;
			} }

			public int currentBox { get {
					return getCurrentStorage.currentBox;
				} 
				set {
					getCurrentStorage.currentBox=value;
			} }

			public int maxBoxes { get {
				return getCurrentStorage.maxBoxes;
			} }

			public int maxPokemon(int box) {
				return getCurrentStorage.maxPokemon(box);
			}

			public PokemonEssentials.Interface.Screen.IPokemonBox this[int x] { get {
				return getCurrentStorage[x];
			} }
			public PokemonEssentials.Interface.PokeBattle.IPokemon this[int x,int y] { get {
					return getCurrentStorage[x,y];
				}
				set {
					getCurrentStorage[x,y]=value;
			} }

			public bool full { get {
				return getCurrentStorage.full;
			} }

			public void pbFirstFreePos(int box) {
				getCurrentStorage.pbFirstFreePos(box);
			}

			public void pbCopy(int boxDst,int indexDst,int boxSrc,int indexSrc) {
				getCurrentStorage.pbCopy(boxDst,indexDst,boxSrc,indexSrc);
			}

			public void pbMove(int boxDst,int indexDst,int boxSrc,int indexSrc) {
				getCurrentStorage.pbCopy(boxDst,indexDst,boxSrc,indexSrc);
			}

			public void pbMoveCaughtToParty(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn) {
				getCurrentStorage.pbMoveCaughtToParty(pkmn);
			}

			public void pbMoveCaughtToBox(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn,int box) {
				getCurrentStorage.pbMoveCaughtToBox(pkmn,box);
			}

			public void pbStoreCaught(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn) {
				getCurrentStorage.pbStoreCaught(pkmn);
			}

			public void pbDelete(int box,int index) {
				getCurrentStorage.pbDelete(box, index);
			}
		}

		public partial class TrainerPC : IPCMenuUser {
			public bool shouldShow { get {
				return true;
			} }

			public string name { get {
				return Game._INTL("{1}'s PC",Game.GameData.Trainer.name);
			} }

			public void access() {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("\\se[accesspc]Accessed {1}'s PC.",Game.GameData.Trainer.name));
				if (Game.GameData is IGamePCStorage s) s.pbTrainerPCMenu();
			}
		}

		public partial class StorageSystemPC : IPCMenuUser {
			public bool shouldShow { get {
				return true;
			} }

			public string name { get {
				if (Game.GameData.Global.seenStorageCreator && Game.GameData is IGamePCStorage s) {
					return Game._INTL("{1}'s PC",s.pbGetStorageCreator());
				} else {
					return Game._INTL("Someone's PC");
				}
			} }

			public void access() {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("\\se[accesspc]The Pokémon Storage System was opened."));
				do { //;loop
					int command=Game.GameData is IGameMessage m0 ? m0.pbShowCommandsWithHelp(null,
						new string[] {Game._INTL("Withdraw Pokémon"),
							Game._INTL("Deposit Pokémon"),
							Game._INTL("Move Pokémon"),
							Game._INTL("See ya!") },
						new string[] {Game._INTL("Move Pokémon stored in Boxes to your party."),
							Game._INTL("Store Pokémon in your party in Boxes."),
							Game._INTL("Organize the Pokémon in Boxes and in your party."),
							Game._INTL("Return to the previous menu.")},-1
					) : -1;
					if (command>=0 && command<3) {
						if (command==0 && Game.GameData.PokemonStorage.party.Length>=6) {
							if (Game.GameData is IGameMessage m1) m1.pbMessage(Game._INTL("Your party is full!"));
							continue;
						}
						int count=0;
						foreach (var p in Game.GameData.PokemonStorage.party) {
							if (p.IsNotNullOrNone() && !p.isEgg && p.HP>0) count+=1;
						}
						if (command==1 && count<=1) {
							if (Game.GameData is IGameMessage m1) m1.pbMessage(Game._INTL("Can't deposit the last Pokémon!"));
							continue;
						}
						if (Game.GameData is IGameSpriteWindow g) g.pbFadeOutIn(99999, block: () => {
							IPokemonStorageScene scene=(Game.GameData as Game).Scenes.PokemonStorageScene; //new PokemonStorageScene();
							IPokemonStorageScreen screen=(Game.GameData as Game).Screens.PokemonStorageScreen.initialize(scene,Game.GameData.PokemonStorage); //new PokemonStorageScreen(scene,Game.GameData.PokemonStorage);
							screen.pbStartScreen(command);
						});
					} else {
						break;
					}
				} while (true);
			}
		}

		public static class PokemonPCList //: IPokemonPCList 
		{
			//PokemonPCList.registerPC(new StorageSystemPC());
			//PokemonPCList.registerPC(new TrainerPC());
			public static IList<IPCMenuUser> @pclist=new List<IPCMenuUser>();

			public static void registerPC(IPCMenuUser pc) {
				@pclist.Add(pc);
			}

			public static string[] getCommandList() {
				List<string> commands=new List<string>();
				foreach (var pc in @pclist) {
					if (pc.shouldShow) {
						commands.Add(pc.name);
					}
				}
				commands.Add(Game._INTL("Log Off"));
				return commands.ToArray();
			}

			public static bool callCommand(int cmd) {
				if (cmd<0 || cmd>=@pclist.Count) {
					return false;
				}
				int i=0;
				foreach (var pc in @pclist) {
					if (pc.shouldShow) {
						if (i==cmd) {
							pc.access();
							return true;
						}
						i+=1;
					}
				}
				return false;
			}
		}
	}

	public partial class Game : PokemonEssentials.Interface.Screen.IGamePCStorage { 
		// ###############################################################################
		// PC menus
		// ###############################################################################
		public string pbGetStorageCreator() {
			string creator=null;//pbStorageCreator();
			//if (creator == null || creator=="") creator=Game._INTL("Bill");
			if (string.IsNullOrEmpty(creator)) creator=Game._INTL("Bill");
			return creator;
		}
		
		public void pbPCItemStorage() {
			//Items ret = Items.NONE;
			do { //;loop
				int command=(this as IGameMessage).pbShowCommandsWithHelp(null,
					new string[] { Game._INTL("Withdraw Item"),
						Game._INTL("Deposit Item"),
						Game._INTL("Toss Item"),
						Game._INTL("Exit") },
					new string[] { Game._INTL("Take out items from the PC."),
						Game._INTL("Store items in the PC."),
						Game._INTL("Throw away items stored in the PC."),
						Game._INTL("Go back to the previous menu.") },-1
				);
				if (command==0) {		// Withdraw Item
					if (Global.pcItemStorage == null) {
						Global.pcItemStorage=new PCItemStorage();
					}
					if (Global.pcItemStorage.empty()) {
						(this as IGameMessage).pbMessage(Game._INTL("There are no items."));
					} else {
						pbFadeOutIn(99999, block: () => {
							IWithdrawItemScene scene = Scenes.Bag_ItemWithdraw;//new WithdrawItemScene();
							IBagScreen screen = Screens.Bag.initialize((IBagScene)scene,Bag);//new PokemonBagScreen(scene,Bag);
							screen.pbWithdrawItemScreen();//ret=
						});
					}
				} else if (command==1) {		// Deposit Item
					pbFadeOutIn(99999, block: () => {
						IBagScene scene = Scenes.Bag;//new PokemonBag_Scene();
						IBagScreen screen = Screens.Bag.initialize(scene,Bag);//new PokemonBagScreen(scene,Bag);
						screen.pbDepositItemScreen();//ret=
					});
				} else if (command==2) {		// Toss Item
					if (Global.pcItemStorage == null) {
						Global.pcItemStorage=new PCItemStorage();
					}
					if (Global.pcItemStorage.empty()) {
						(this as IGameMessage).pbMessage(Game._INTL("There are no items."));
					} else {
						pbFadeOutIn(99999, block: () => {
							ITossItemScene scene = Scenes.Bag_ItemToss;//new TossItemScene();
							IBagScreen screen = Screens.Bag.initialize((IBagScene)scene,Bag);//new PokemonBagScreen(scene,Bag);
							screen.pbTossItemScreen();//ret=
						});
					}
				} else {
					break;
				}
			} while (true);
		}

		public void pbPCMailbox() {
			if (Global.mailbox == null || Global.mailbox.Count==0) {
				(this as IGameMessage).pbMessage(Game._INTL("There's no Mail here."));
			} else {
				do { //;loop
					List<string> commands=new List<string>();
					foreach (IMail mail in Global.mailbox) {
						commands.Add(mail.sender);
					}
					commands.Add(Game._INTL("Cancel"));
					int command=(this as IGameMessage).pbShowCommands(null,commands.ToArray(),-1);
					if (command>=0 && command<Global.mailbox.Count) {
						int mailIndex=command;
						command=(this as IGameMessage).pbMessage(Game._INTL("What do you want to do with {1}'s Mail?",
							Global.mailbox[mailIndex].sender),new string[] {
								_INTL("Read"),
								_INTL("Move to Bag"),
								_INTL("Give"),
								_INTL("Cancel")
							},-1);
						if (command==0) {		// Read
							pbFadeOutIn(99999, block: () => {
								if(this is IGameMail m) m.pbDisplayMail(Global.mailbox[mailIndex]);
							});
						} else if (command==1) {		// Move to Bag
							if ((this as IGameMessage).pbConfirmMessage(_INTL("The message will be lost. Is that OK?"))) {
								if (Bag.pbStoreItem(Global.mailbox[mailIndex].item)) {
									(this as IGameMessage).pbMessage(_INTL("The Mail was returned to the Bag with its message erased."));
									//Global.mailbox.delete_at(mailIndex);
									Global.mailbox.RemoveAt(mailIndex);
								} else {
									(this as IGameMessage).pbMessage(_INTL("The Bag is full."));
								}
							}
						} else if (command==2) {		// Give
							pbFadeOutIn(99999, block: () => {
								IPartyDisplayScene sscene=Scenes.Party; //new PokemonScreen_Scene();
								IPartyDisplayScreen sscreen=Screens.Party.initialize(sscene,Trainer.party); //new PokemonScreen(sscene,Trainer.party);
								sscreen.pbPokemonGiveMailScreen(mailIndex);
							});
						}
					} else {
						break;
					}
				} while (true);
			}
		}

		public void pbTrainerPCMenu() {
			do { //;loop
				int command=(this as IGameMessage).pbMessage(Game._INTL("What do you want to do?"),new string[] {
					Game._INTL("Item Storage"),
					Game._INTL("Mailbox"),
					Game._INTL("Turn Off")
				},-1);
				if (command==0) {
					pbPCItemStorage();
				} else if (command==1) {
					pbPCMailbox();
				} else {
					break;
				}
			} while (true);
		}

		public void pbTrainerPC() {
			(this as IGameMessage).pbMessage(Game._INTL("\\se[computeropen]{1} booted up the PC.",Trainer.name));
			pbTrainerPCMenu();
			(this as IGameAudioPlay).pbSEPlay("computerclose");
		}

		public void pbPokeCenterPC() {
			(this as IGameMessage).pbMessage(Game._INTL("\\se[computeropen]{1} booted up the PC.",Trainer.name));
			do { //;loop
				string[] commands=PokemonPCList.getCommandList();
				int command=(this as IGameMessage).pbMessage(Game._INTL("Which PC should be accessed?"),
					commands,commands.Length);
				if (!PokemonPCList.callCommand(command)) {
					break;
				}
			} while (true);
			(this as IGameAudioPlay).pbSEPlay("computerclose");
		}
	}
}
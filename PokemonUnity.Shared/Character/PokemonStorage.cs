using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Utility;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;

namespace PokemonUnity
{
namespace Character
{
public partial class PokemonBox {
  public List<Pokemon> pokemon				{ get; protected set; }
  public string name				{ get; set; }
  public string background				{ get; set; }

  public PokemonBox(string name,int maxPokemon=30) {
	@pokemon=new List<Pokemon>();
	this.name=name;
	@background=null;
	for (int i = 0; i < maxPokemon; i++) {
	  @pokemon[i]=null;
	}
  }

  public bool full { get {
	//return (@pokemon.nitems==this.Length);
	return (this.nitems==this.length);
  } }

  public int nitems { get { //item count
	return @pokemon.Count;
  } }

  public int length { get { //capacity
	return @pokemon.Capacity;
  } }

  public IEnumerable<Pokemon> each() {
	foreach (Pokemon item in @pokemon) { yield return item; }
  }

  public Pokemon this[int i] { set {
	@pokemon[i]=value;
  }

  get {
	return @pokemon[i];
  } }

		public static implicit operator PokemonBox (Pokemon[] party)
		{
			PokemonBox box = new PokemonBox("party", Game.GameData.Features.LimitPokemonPartySize);
			int i = 0;
			foreach(Pokemon p in party)
			{
				if(p.IsNotNullOrNone())
					box[i] = p;
				i++;
			}
			return box;
		}
		public static implicit operator Pokemon[] (PokemonBox box)
		{
			return box.pokemon.ToArray();
		}
}

public partial class PokemonStorage {
  public PokemonBox[] boxes				{ get; protected set; }
  //public int party				{ get; protected set; }
  public int currentBox				{ get; set; }
  private int boxmode				{ get; set; }

  public int maxBoxes { get {
	return @boxes.Length;
  } }

  public virtual Pokemon[] party{ get {
	return Game.GameData.Trainer.party;
  }
  set {
	//throw new Exception("Not supported");
	GameDebug.LogError("Not supported");
  } }

  public static string[] MARKINGCHARS=new string[] { "●", "■", "▲", "♥" };

  public PokemonStorage(int maxBoxes=Core.STORAGEBOXES,int maxPokemon=30) {
	@boxes=new PokemonBox[maxBoxes];
	for (int i = 0; i < maxBoxes; i++) {
	  int ip1=i+1;
	  @boxes[i]=new PokemonBox(string.Format("Box {0}",ip1),maxPokemon);
	  int backid=i%24;
	  @boxes[i].background=$"box#{backid}";
	}
	@currentBox=0;
	@boxmode=-1;
  }

  public int maxPokemon(int box) {
	if (box>=this.maxBoxes) return 0;
	return box<0 ? Game.GameData.Features.LimitPokemonPartySize : this[box].length;
  }

  public PokemonBox this[int x] { get {
	  return (x==-1) ? (PokemonBox)this.party : @boxes[x];
	}
  }
  public Pokemon this[int x,int y] { get {
	//if (y==null) {
	//  return (x==-1) ? this.party : @boxes[x];
	//} else {
	  //foreach (var i in @boxes) {
		//if (i is Pokemon) raise "Box is a Pokémon, not a box";
	  //}
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
	  if (//this[boxSrc,indexSrc].respond_to("formTime") &&
											this[boxSrc,indexSrc].formTime != null) this[boxSrc,indexSrc].formTime=null;
	  this[boxDst,indexDst]=this[boxSrc,indexSrc];
	}
	return true;
  }

  public bool pbMove(int boxDst,int indexDst,int boxSrc,int indexSrc) {
	if (!pbCopy(boxDst,indexDst,boxSrc,indexSrc)) return false;
	pbDelete(boxSrc,indexSrc);
	return true;
  }

  public void pbMoveCaughtToParty(Pokemon pkmn) {
	if (((PokemonBox)this.party).nitems>=6) {
	//if (this.party.GetCount()>=6) {
	  return;
	}
	this.party[this.party.Length]=pkmn;
  }

  public bool pbMoveCaughtToBox(Pokemon pkmn,int box) {
	for (int i = 0; i < maxPokemon(box); i++) {
	  if (this[box,i]==null) {
		if (box>=0) {
		  pkmn.Heal();
		  if (//pkmn.respond_to("formTime") && 
							pkmn.formTime != null) pkmn.formTime=null;
		}
		this[box,i]=pkmn;
		return true;
	  }
	}
	return false;
  }

  public int pbStoreCaught(Pokemon pkmn) {
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

public partial class PokemonStorageWithParty : PokemonStorage {
  public override Pokemon[] party { get {
	return base.party;
  } 
  set {
	base.party=party;
  } }

  public PokemonStorageWithParty(int maxBoxes=24,int maxPokemon=30,Pokemon[] party=null) : base (maxBoxes,maxPokemon){
	//super(maxBoxes,maxPokemon);
	if (party != null) {
		//ToDo: Feature not setup... 
	  this.party=party;
	} else {
	  @party=new Pokemon[Game.GameData.Features.LimitPokemonPartySize];
	}
  }
}

// ###############################################################################
// Regional Storage scripts
// ###############################################################################
/*public partial class RegionalStorage {
  public List<PokemonStorage> storages { get; set; }
  public int lastmap { get; set; }
  public int rgnmap { get; set; }

  public void initialize() {
	@storages=new List<PokemonStorage>();
	@lastmap=-1;
	@rgnmap=-1;
  }

  public PokemonStorage getCurrentStorage { get {
	if (Game.GameData.GameMap == null) {
	  //throw Exception(Game._INTL("The player is not on a map, so the region could not be determined."));
	  GameDebug.LogError(Game._INTL("The player is not on a map, so the region could not be determined."));
	}
	if (@lastmap!=Game.GameData.GameMap.map_id) {
	  @rgnmap=pbGetCurrentRegion(); // may access file IO, so caching result
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

  public PokemonBox[] boxes { get {
	return getCurrentStorage.boxes;
  } }

  public Pokemon[] party { get {
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

  public Pokemon[] this[int x] { get {
	return getCurrentStorage[x];
  } }
  public Pokemon this[int x,int y] { get {
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

  public void pbMoveCaughtToParty(Pokemon pkmn) {
	getCurrentStorage.pbMoveCaughtToParty(pkmn);
  }

  public void pbMoveCaughtToBox(Pokemon pkmn,int box) {
	getCurrentStorage.pbMoveCaughtToBox(pkmn,box);
  }

  public void pbStoreCaught(Pokemon pkmn) {
	getCurrentStorage.pbStoreCaught(pkmn);
  }

  public void pbDelete(int box,int index) {
	getCurrentStorage.pbDelete(box, index);
  }
}

public partial class TrainerPC : IUserPC {
  public bool shouldShow { get {
	return true;
  } }

  public string name { get {
	return Game._INTL("{1}'s PC",Game.GameData.Trainer.name);
  } }

  public void access() {
	Game.pbMessage(Game._INTL("\\se[accesspc]Accessed {1}'s PC.",Game.GameData.Trainer.name));
	Game.GameData.pbTrainerPCMenu();
  }
}

public partial class StorageSystemPC : IUserPC {
  public bool shouldShow { get {
	return true;
  } }

  public string name { get {
	if (Game.GameData.Global.seenStorageCreator) {
	  return Game._INTL("{1}'s PC",Game.GameData.pbGetStorageCreator());
	} else {
	  return Game._INTL("Someone's PC");
	}
  } }

  public void access() {
	Game.pbMessage(Game._INTL("\\se[accesspc]The Pokémon Storage System was opened."));
	do { //;loop
	  int command=Game.pbShowCommandsWithHelp(null,
		 new string[] {Game._INTL("Withdraw Pokémon"),
		 Game._INTL("Deposit Pokémon"),
		 Game._INTL("Move Pokémon"),
		 Game._INTL("See ya!") },
		 new string[] {Game._INTL("Move Pokémon stored in Boxes to your party."),
		 Game._INTL("Store Pokémon in your party in Boxes."),
		 Game._INTL("Organize the Pokémon in Boxes and in your party."),
		 Game._INTL("Return to the previous menu.")},-1
	  );
	  if (command>=0 && command<3) {
		if (command==0 && Game.GameData.PokemonStorage.party.Length>=6) {
		  Game.pbMessage(Game._INTL("Your party is full!"));
		  continue;
		}
		int count=0;
		foreach (var p in Game.GameData.PokemonStorage.party) {
		  if (p.IsNotNullOrNone() && !p.isEgg && p.HP>0) count+=1;
		}
		if (command==1 && count<=1) {
		  Game.pbMessage(Game._INTL("Can't deposit the last Pokémon!"));
		  continue;
		}
		Game.UI.pbFadeOutIn(99999, () => {
		   //IPokemonStorageScene scene=new PokemonStorageScene();
		   //IPokemonStorageScreen screen=new PokemonStorageScreen(scene,Game.GameData.PokemonStorage);
		   IPokemonStorageScene scene=Game.GameData.PokemonStorageScene; scene.initialize();
		   IPokemonStorageScreen screen=Game.GameData.PokemonStorageScreen; screen.initialize(scene,Game.GameData.PokemonStorage);
		   screen.pbStartScreen(command);
		});
	  } else {
		break;
	  }
	} while (true);
  }
}

public static partial class PokemonPCList {
//PokemonPCList.registerPC(new StorageSystemPC());
//PokemonPCList.registerPC(new TrainerPC());
  public static List<IUserPC> @pclist=new List<IUserPC>();

  public static void registerPC(IUserPC pc) {
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
}*/

	public interface IUserPC
	{
		string name { get; }
		bool shouldShow { get; }

		void access();
	}
}

	public partial class Game { 
// ###############################################################################
// PC menus
// ###############################################################################
public string pbGetStorageCreator() {
  string creator=null;//pbStorageCreator();
  //if (creator == null || creator=="") creator=Game._INTL("Bill");
  if (string.IsNullOrEmpty(creator)) creator=Game._INTL("Bill");
  return creator;
}
		
/*public void pbPCItemStorage() {
		Items ret = Items.NONE;
  do { //;loop
	int command=Game.pbShowCommandsWithHelp(null,
	   new string[] {Game._INTL("Withdraw Item"),
	   Game._INTL("Deposit Item"),
	   Game._INTL("Toss Item"),
	   Game._INTL("Exit")},
	   new string[] {Game._INTL("Take out items from the PC."),
	   Game._INTL("Store items in the PC."),
	   Game._INTL("Throw away items stored in the PC."),
	   Game._INTL("Go back to the previous menu.")},-1
	);
	if (command==0) {		// Withdraw Item
	  if (!Game.GameData.Global.pcItemStorage) {
		Game.GameData.Global.pcItemStorage=new PCItemStorage();
	  }
	  if (Game.GameData.Global.pcItemStorage.empty) {
		Game.pbMessage(Game._INTL("There are no items."));
	  } else {
		Game.UI.pbFadeOutIn(99999, () => {
		   IWithdrawItemScene scene=new WithdrawItemScene();
		   IPokemonBagScreen screen=new PokemonBagScreen(scene,Game.GameData.Bag);
		   ret=screen.pbWithdrawItemScreen();
		});
	  }
	} else if (command==1) {		// Deposit Item
	  Game.UI.pbFadeOutIn(99999, () => {
		 IPokemonBag_Scene scene=new PokemonBag_Scene();
		 IPokemonBagScreen screen=new PokemonBagScreen(scene,Game.GameData.Bag);
		 ret=screen.pbDepositItemScreen();
	  });
	} else if (command==2) {		// Toss Item
	  if (!Game.GameData.Global.pcItemStorage) {
		Game.GameData.Global.pcItemStorage=new PCItemStorage();
	  }
	  if (Game.GameData.Global.pcItemStorage.empty) {
		Game.pbMessage(Game._INTL("There are no items."));
	  } else {
		Game.UI.pbFadeOutIn(99999, () => {
		   ITossItemScene scene=new TossItemScene();
		   IPokemonBagScreen screen=new PokemonBagScreen(scene,Game.GameData.Bag);
		   ret=screen.pbTossItemScreen();
		});
	  }
	} else {
	  break;
	}
  } while (true);
}

public void pbPCMailbox() {
  if (!Game.GameData.Global.mailbox || Game.GameData.Global.mailbox.Length==0) {
	Game.pbMessage(Game._INTL("There's no Mail here."));
  } else {
	do { //;loop
	  List<string> commands=new List<string>();
	  foreach (var mail in Game.GameData.Global.mailbox) {
		commands.Add(mail.sender);
	  }
	  commands.Add(Game._INTL("Cancel"));
	  int command=Game.pbShowCommands(null,commands,-1);
	  if (command>=0 && command<Game.GameData.Global.mailbox.Length) {
		int mailIndex=command;
		command=Game.pbMessage(Game._INTL("What do you want to do with {1}'s Mail?",
		   Game.GameData.Global.mailbox[mailIndex].sender),new string[] {
		   Game._INTL("Read"),
		   Game._INTL("Move to Bag"),
		   Game._INTL("Give"),
		   Game._INTL("Cancel")
		   },-1);
		if (command==0) {		// Read
		  Game.UI.pbFadeOutIn(99999, () => {
			 pbDisplayMail(Game.GameData.Global.mailbox[mailIndex]);
		  });
		} else if (command==1) {		// Move to Bag
		  if (Game.pbConfirmMessage(Game._INTL("The message will be lost. Is that OK?"))) {
			if (Game.GameData.Bag.pbStoreItem(Game.GameData.Global.mailbox[mailIndex].Item)) {
			  Game.pbMessage(Game._INTL("The Mail was returned to the Bag with its message erased."));
			  Game.GameData.Global.mailbox.delete_at(mailIndex);
			} else {
			  Game.pbMessage(Game._INTL("The Bag is full."));
			}
		  }
		} else if (command==2) {		// Give
		  Game.UI.pbFadeOutIn(99999, () => {
			 IPokemonScreen_Scene sscene=new PokemonScreen_Scene();
			 IPokemonScreen sscreen=new PokemonScreen(sscene,Game.GameData.Trainer.party);
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
	int command=Game.pbMessage(Game._INTL("What do you want to do?"),new string[] {
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
  Game.pbMessage(Game._INTL("\\se[computeropen]{1} booted up the PC.",Game.GameData.Trainer.name));
  pbTrainerPCMenu();
  pbSEPlay("computerclose");
}

public void pbPokeCenterPC() {
  Game.pbMessage(Game._INTL("\\se[computeropen]{1} booted up the PC.",Game.GameData.Trainer.name));
  do { //;loop
	string[] commands=PokemonPCList.getCommandList();
	int command=Game.pbMessage(Game._INTL("Which PC should be accessed?"),
	   commands,commands.Length);
	if (!PokemonPCList.callCommand(command)) {
	  break;
	}
  } while (true);
  pbSEPlay("computerclose");
}*/
}
}
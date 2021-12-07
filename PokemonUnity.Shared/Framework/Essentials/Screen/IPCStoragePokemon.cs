using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{	
	public interface IPokemonBox {
		IList<IPokemon> pokemon				{ get; }
		string name							{ get; set; }
		string background					{ get; set; }

		IPokemonBox initialize(string name, int maxPokemon = 30);

		bool full { get; }

		int nitems { get; } //item count

		int length { get; } //capacity

		IEnumerable<IPokemon> each();

		IPokemon this[int i] { get; set; }
	}

	public interface IPokemonStorage {
		IPokemonBox[] boxes				{ get; }
		//IPokemon[] party				{ get; }
		int currentBox					{ get; set; }
		int maxBoxes					{ get; }
		IPokemon[] party				{ get; }

		//public void party=(value) {
		//raise new ArgumentError("Not supported");
		//}

		//string[] MARKINGCHARS { get; } //= new string[] { "●", "■", "▲", "♥" };

		IPokemonStorage initialize(int maxBoxes = Core.STORAGEBOXES, int maxPokemon = 30);

		int maxPokemon(int box);

		IPokemonBox this[int x] { get; }

		IPokemon this[int x,int y] { get; set; }

		bool full { get; }

		int pbFirstFreePos(int box);

		bool pbCopy(int boxDst, int indexDst, int boxSrc, int indexSrc);

		bool pbMove(int boxDst, int indexDst, int boxSrc, int indexSrc);

		void pbMoveCaughtToParty(IPokemon pkmn);

		bool pbMoveCaughtToBox(IPokemon pkmn,int box);

		int pbStoreCaught(IPokemon pkmn);

		void pbDelete(int box, int index);
	}

	public interface IPokemonStorageWithParty : IPokemonStorage {
		new IPokemon[] party { get; set; }
		//IPokemon[] party { set; }
		
		//IPokemonStorageWithParty initialize(int maxBoxes = 24, int maxPokemon = 30, IPokemon[] party = null); //: base (maxBoxes,maxPokemon)
	}
	
	// ###############################################################################
	// Regional Storage scripts
	// ###############################################################################
	public interface IRegionalStorage {
		IRegionalStorage initialize();

		IPokemonStorage getCurrentStorage { get; }

		IPokemonBox[] boxes { get; }

		IPokemon[] party { get; }

		int currentBox { get; set; }

		int maxBoxes { get; }

		int maxPokemon(int box);

		IPokemonStorage this[int x] { get; }
		IPokemon this[int x,int y] { get; set; }

		bool full { get; }

		void pbFirstFreePos(int box);

		void pbCopy(int boxDst, int indexDst, int boxSrc, int indexSrc);

		void pbMove(int boxDst, int indexDst, int boxSrc, int indexSrc);

		void pbMoveCaughtToParty(IPokemon pkmn);

		void pbMoveCaughtToBox(IPokemon pkmn, int box);

		void pbStoreCaught(IPokemon pkmn);

		void pbDelete(int box, int index);
	}


	public interface IGamePCStorage
	{
		// ###############################################################################
		// PC menus
		// ###############################################################################

		/// <summary>
		/// Name of PC Admin, for Pokemon Storage
		/// </summary>
		/// <remarks>
		/// Maybe check region accessing pc from, and associate region prof with creator
		/// ...until maybe the player encounter's the prof and exchanges pokedex (or however it works)
		/// </remarks>
		string pbGetStorageCreator();

		Items pbPCItemStorage();

		void pbPCMailbox();

		void pbTrainerPCMenu();



		void pbTrainerPC();

		void pbPokeCenterPC();
	}

	
	public interface IPCMenuUser {
		bool shouldShow { get; }

		string name { get; }

		void access();
	}

	public interface IPokemonPCList {
		//IList<IPCMenuUser> @pclist { get; } // = new List<IPCMenuUser>();

		void registerPC(IPCMenuUser pc);

		string[] getCommandList();

		bool callCommand(int cmd);
	}


	#region PC Storage Scene
	public interface IPokemonStorageScene : IScene
	{
		void drawMarkings(IBitmap bitmap, float x, float y, float width, float height, bool[] markings);
		//void drawMarkings(bool[] markings);
		string[] getMarkingCommands(bool[] markings);
		IPokemonStorageScene initialize();
		void pbBoxName(string helptext, int minchars, int maxchars);
		void pbChangeBackground(int wp);
		int pbChangeSelection(int key, int selection);
		void pbChooseBox(string msg);
		Items pbChooseItem(IBattler bag);
		void pbCloseBox();
		//void pbDisplay(string message);
		void pbDropDownPartyTab();
		void pbHardRefresh();
		void pbHidePartyTab();
		void pbHold(KeyValuePair<int, int> selected);
		void pbJumpToBox(int newbox);
		void pbMark(KeyValuePair<int, int> selected, IPokemon heldpoke);
		int pbPartyChangeSelection(int key, int selection);
		void pbPartySetArrow(int selection);//arrow , 
		void pbPlace(KeyValuePair<int, int> selected, IPokemon heldpoke);
		//void pbRefresh();
		void pbRelease(KeyValuePair<int, int> selected, IPokemon heldpoke);
		int[] pbSelectBox(IPokemon[] party);
		int[] pbSelectBoxInternal(IPokemon[] party);
		int pbSelectParty(IPokemon[] party);
		int pbSelectPartyInternal(IPokemon[] party, bool depositing);
		void pbSetArrow(int selection);//arrow , 
		void pbSetMosaic(int selection);
		int pbShowCommands(string message, string[] commands, int index = 0);
		void pbStartBox(IPokemonStorageScreen screen, int command);
		void pbStore(KeyValuePair<int, int> selected, IPokemon heldpoke, int destbox, int firstfree);
		void pbSummary(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void pbSwap(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void pbSwitchBoxToLeft(int newbox);
		void pbSwitchBoxToRight(int newbox);
		void pbUpdateOverlay(int selection, IPokemon[] party = null);
		void pbWithdraw(KeyValuePair<int, int> selected, IPokemon heldpoke, int partyindex);
	}

	public interface IPokemonStorageScreen : IScreen
	{
		int pbAbleCount { get; }
		IPokemon pbHeldPokemon { get; }
		IPokemonStorageScene scene { get; }
		IPokemonStorage storage { get; }

		void debugMenu(KeyValuePair<int, int> selected, IPokemon pkmn, IPokemon heldpoke);
		void IPokemonStorageScreen(IPokemonStorageScene scene, IPokemonStorage storage);
		bool pbAble(IPokemon pokemon);
		void pbBoxCommands();
		int? pbChoosePokemon(IPokemon[] party = null);
		bool pbConfirm(string str);
		void pbDisplay(string message);
		void pbHold(KeyValuePair<int, int> selected);
		void pbItem(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void pbMark(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void pbPlace(KeyValuePair<int, int> selected);
		void pbRelease(KeyValuePair<int, int> selected, IPokemon heldpoke);
		int pbShowCommands(string msg, string[] commands);
		void pbStartScreen(int command);
		void pbStore(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void pbSummary(KeyValuePair<int, int> selected, IPokemon heldpoke);
		bool pbSwap(KeyValuePair<int, int> selected);
		bool pbWithdraw(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void selectPokemon(int index);
	}
	#endregion

	#region UI Elements
	public interface IInterpolator {
		//const int ZOOM_X  = 1;
		//const int ZOOM_Y  = 2;
		//const int X       = 3;
		//const int Y       = 4;
		//const int OPACITY = 5;
		//const int COLOR   = 6;
		//const int WAIT    = 7;

		IInterpolator initialize();

		bool tweening();

		void tween(ISprite sprite, Items items, int frames);

		void update();
	}

	public interface IPokemonBoxIcon //: IIconSprite 
	{
		IPokemonBoxIcon initialize(IPokemon pokemon, IViewport viewport = null);

		void release();

		bool releasing();

		void update();
	}

	public interface IPokemonBoxArrow : ISpriteWrapper {
		//IPokemonBoxArrow initialize(IViewport viewport = null);

		IPokemon heldPokemon { get; }
		//bool visible { set; }
		//IColor color { set; }
		//void dispose();
		bool holding { get; }
		bool grabbing { get; }
		bool placing { get; }
		//float x { set; }
		//float y { set; }

		void setSprite(ISprite sprite);

		void deleteSprite();

		void grab(ISprite sprite);

		void place();

		void release();

		//void update();
	}

	public interface IPokemonBoxPartySprite : ISpriteWrapper {
		void deletePokemon(int index);

		ISprite getPokemon(int index);

		void setPokemon(int index, ISprite sprite);

		//void grabPokemon(int index, IAnimatedBitmap arrow);
		void grabPokemon(int index, IBitmap arrow);

		//float x { set; }

		//float y { set; }

		//IColor color { set; }

		//bool visible { set; }

		IPokemonBoxPartySprite initialize(IPokemon[] party, IViewport viewport = null);

		//void dispose();

		void refresh();
		//void update();
	}

	public interface IMosaicPokemonSprite //: IPokemonSprite 
	{
		//void initialize(*args);

		int mosaic				{ get; set; }

		void dispose();

		IBitmap bitmap { set; }

		void mosaicRefresh(IBitmap bitmap);
	}

	public interface IAutoMosaicPokemonSprite : IMosaicPokemonSprite {
		void update();
	}

	public interface IPokemonBoxSprite : ISpriteWrapper {
		int refreshBox				{ get; set; }
		int refreshSprites			{ get; set; }

		void deletePokemon(int index);

		ISprite getPokemon(int index);

		void setPokemon(int index, ISprite sprite);

		//void grabPokemon(int index, IAnimatedBitmap arrow);
		void grabPokemon(int index, IBitmap arrow);

		//float x { set; }

		//float y { set; }

		//IColor color { set; }

		//bool visible { set; }

		void getBoxBitmap();

		IPokemonBoxSprite initialize(IPokemonStorage storage, int boxnumber, IViewport viewport = null);

		//void dispose();

		void refresh();

		//void update();
	}
	#endregion
}
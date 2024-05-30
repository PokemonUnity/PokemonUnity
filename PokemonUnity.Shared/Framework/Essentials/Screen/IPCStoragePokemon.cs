using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Interface;
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
	public interface IPokemonBox : IList<PokemonEssentials.Interface.PokeBattle.IPokemon>, ICollection<PokemonEssentials.Interface.PokeBattle.IPokemon>, IEnumerable<PokemonEssentials.Interface.PokeBattle.IPokemon> {
		IList<IPokemon> pokemon				{ get; }
		string name							{ get; set; }
		string background					{ get; set; }

		IPokemonBox initialize(string name, int maxPokemon = 30);

		bool full { get; }

		/// <summary>
		/// number of pokemons stored...
		/// </summary>
		int nitems { get; } //item count

		/// <summary>
		/// size of the storage box...
		/// </summary>
		int length { get; } //capacity

		IEnumerable<IPokemon> each();

		//IPokemon this[int i] { get; set; }
	}

	public interface IPCPokemonStorage {
		IPokemonBox[] boxes				{ get; }
		//IPokemon[] party				{ get; }
		int currentBox					{ get; set; }
		int maxBoxes					{ get; }
		IPokemon[] party				{ get; }

		//public void party=(value) {
		//raise new ArgumentError("Not supported");
		//}

		//string[] MARKINGCHARS { get; } //= new string[] { "●", "■", "▲", "♥" };

		IPCPokemonStorage initialize(int maxBoxes = Core.STORAGEBOXES, int maxPokemon = 30);

		int maxPokemon(int box);

		IPokemonBox this[int x] { get; }

		/// <summary>
		///
		/// </summary>
		/// <param name="x">-1 to access player's party, box index begins at 0</param>
		/// <param name="y">slot id</param>
		/// <returns></returns>
		IPokemon this[int x,int y] { get; set; }

		bool full { get; }

		int FirstFreePos(int box);

		bool Copy(int boxDst, int indexDst, int boxSrc, int indexSrc);

		bool Move(int boxDst, int indexDst, int boxSrc, int indexSrc);

		void MoveCaughtToParty(IPokemon pkmn);

		bool MoveCaughtToBox(IPokemon pkmn,int box);

		int StoreCaught(IPokemon pkmn);

		void Delete(int box, int index);
	}

	public interface IPokemonStorageWithParty : IPCPokemonStorage {
		//new IPokemon[] party { get; set; }
		//IPokemon[] party { set; }

		IPokemonStorageWithParty initialize(int maxBoxes = 24, int maxPokemon = 30, IPokemon[] party = null); //: base (maxBoxes,maxPokemon)
	}

	// ###############################################################################
	// Regional Storage scripts
	// ###############################################################################
	public interface IRegionalStorage {
		IRegionalStorage initialize();

		IPCPokemonStorage getCurrentStorage { get; }

		IPokemonBox[] boxes { get; }

		IPokemon[] party { get; }

		int currentBox { get; set; }

		int maxBoxes { get; }

		int maxPokemon(int box);

		IPokemonBox this[int x] { get; }
		IPokemon this[int x,int y] { get; set; }

		bool full { get; }

		void FirstFreePos(int box);

		void Copy(int boxDst, int indexDst, int boxSrc, int indexSrc);

		void Move(int boxDst, int indexDst, int boxSrc, int indexSrc);

		void MoveCaughtToParty(IPokemon pkmn);

		void MoveCaughtToBox(IPokemon pkmn, int box);

		void StoreCaught(IPokemon pkmn);

		void Delete(int box, int index);
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
		string GetStorageCreator();

		//Items PCItemStorage();
		void PCItemStorage();

		void PCMailbox();

		void TrainerPCMenu();



		void TrainerPC();

		void PokeCenterPC();
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
		void BoxName(string helptext, int minchars, int maxchars);
		void ChangeBackground(int wp);
		int ChangeSelection(int key, int selection);
		void ChooseBox(string msg);
		Items ChooseItem(IBattler bag);
		void CloseBox();
		//void Display(string message);
		void DropDownPartyTab();
		void HardRefresh();
		void HidePartyTab();
		void Hold(KeyValuePair<int, int> selected);
		void JumpToBox(int newbox);
		void Mark(KeyValuePair<int, int> selected, IPokemon heldpoke);
		int PartyChangeSelection(int key, int selection);
		void PartySetArrow(int selection);//arrow ,
		void Place(KeyValuePair<int, int> selected, IPokemon heldpoke);
		//void Refresh();
		void Release(KeyValuePair<int, int> selected, IPokemon heldpoke);
		int[] SelectBox(IPokemon[] party);
		int[] SelectBoxInternal(IPokemon[] party);
		int SelectParty(IPokemon[] party);
		int SelectPartyInternal(IPokemon[] party, bool depositing);
		void SetArrow(int selection);//arrow ,
		void SetMosaic(int selection);
		int ShowCommands(string message, string[] commands, int index = 0);
		void StartBox(IPokemonStorageScreen screen, int command);
		void Store(KeyValuePair<int, int> selected, IPokemon heldpoke, int destbox, int firstfree);
		void Summary(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void Swap(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void SwitchBoxToLeft(int newbox);
		void SwitchBoxToRight(int newbox);
		void UpdateOverlay(int selection, IPokemon[] party = null);
		void Withdraw(KeyValuePair<int, int> selected, IPokemon heldpoke, int partyindex);
	}

	public interface IPokemonStorageScreen : IScreen
	{
		int AbleCount { get; }
		IPokemon HeldPokemon { get; }
		IPokemonStorageScene scene { get; }
		IPCPokemonStorage storage { get; }

		void debugMenu(KeyValuePair<int, int> selected, IPokemon pkmn, IPokemon heldpoke);
		IPokemonStorageScreen initialize(IPokemonStorageScene scene, IPCPokemonStorage storage);
		bool Able(IPokemon pokemon);
		void BoxCommands();
		int? ChoosePokemon(IPokemon[] party = null);
		bool Confirm(string str);
		void Display(string message);
		void Hold(KeyValuePair<int, int> selected);
		void Item(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void Mark(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void Place(KeyValuePair<int, int> selected);
		void Release(KeyValuePair<int, int> selected, IPokemon heldpoke);
		int ShowCommands(string msg, string[] commands);
		void StartScreen(int command);
		void Store(KeyValuePair<int, int> selected, IPokemon heldpoke);
		void Summary(KeyValuePair<int, int> selected, IPokemon heldpoke);
		bool Swap(KeyValuePair<int, int> selected);
		bool Withdraw(KeyValuePair<int, int> selected, IPokemon heldpoke);
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

		IPokemonBoxSprite initialize(IPCPokemonStorage storage, int boxnumber, IViewport viewport = null);

		//void dispose();

		void refresh();

		//void update();
	}
	#endregion
}
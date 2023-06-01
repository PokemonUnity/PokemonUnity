using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Utility;
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
	public interface IPurifyChamberSet {
		int facing								{ get; set; }
		IPokemonShadowPokemon shadow				{ get; set; }

		int partialSum(int x);

		int length();

		IPurifyChamberSet initialize();

		//=begin;
		//Main component is tempo;
		//Boosted if center has advantage over facing Pokemon;
		//Boosted based on number of best circles;
		//=end;
		int flow();

		int shadowAffinity();

		int affinity(int i);

		// Tempo refers to the type advantages of each Pokemon in a certain set in a
		// clockwise direction. Tempo also depends on the number of Pokemon in the set
		int tempo();

		//void list() {
		//	return @list.clone();
		//}

		//this[int index] { get; } //return @list[index];

		void insertAfter(int index, IPokemon value);

		void insertAt(int index, IPokemon value);

		// Purify Chamber treats Normal/Normal matchup as super effective
		//static bool typeAdvantage(IPokemon p1, IPokemon p2);

		//static bool isSuperEffective(IPokemon p1, IPokemon p2);
	}

	public interface IPurifyChamber { // German: der Kryptorbis
		//const int NUMSETS=9;
		//const int SETSIZE=4;
		int currentSet				{ get; set; } // German: das Forum

		// Calculates the maximum possible tempo
		//static int maximumTempo();

		IPurifyChamber initialize();

		// Number of regular Pokemon in a set
		int setCount(int set);

		int[] setList(int set);
		//	if (set<0 || set>=NUMSETS) return [];
		//	return @sets[set].list;
		//}

		// for speeding up purification. German: Fluidum
		int chamberFlow(int chamber);

		IPokemonShadowPokemon getShadow(int chamber);

		// allow only "shadow" Pokemon
		void setShadow(int chamber, IPokemonShadowPokemon value);

		void Switch(int set1,int set2);

		void insertAfter(int set, int index, IPokemon value);

		void insertAt(int set, int index, IPokemon value);

		//void this[int chamber, int? slot= null] { get; }
		//	if (slot==null) {
		//		return @sets[chamber];
		//	}
		//	if (chamber<0 || chamber>=NUMSETS) return null;
		//	if (slot<0 || slot>=SETSIZE) return null;
		//	return @sets[chamber][slot];
		//}

		bool isPurifiableIgnoreRegular(int set);

		bool isPurifiable(int set);

		// called each step
		void update();

		void debugAddShadow(int set, Pokemons species);

		void debugAddNormal(int set, Pokemons species);

		void debugAdd(int set, IPokemonShadowPokemon shadow, Types type1, Types? type2 = null);
	}

	public interface IGlobalMetadataPurifyChamber {
		IPurifyChamber purifyChamber				{ get; }
		int seenPurifyChamber						{ get; }
	}

	public interface IPurifyChamberPC : IPCMenuUser
	{
		//bool shouldShow();
		//string name();
		//void access();
	}

	//PokemonPCList.registerPC(new PurifyChamberPC());

	// ####################
	// 
	//  General purpose utilities
	// 

	public interface IGamePurifyChamber
	{
		void DrawGauge(IBitmap bitmap, IRect rect, IColor color, int value, int maxValue);

		// angle in degrees
		IPoint calcPoint(float x, float y, int distance, float angle);

		void PurifyChamber();
	}

// ####################

	public interface IPurifyChamberHelper  //static
	{
		IPokemonShadowPokemon GetPokemon2(IPurifyChamberSet chamber, int set, int position);

		IPokemonShadowPokemon GetPokemon(IPurifyChamberSet chamber, int position);

		int adjustOnInsert(int position);

		void SetPokemon(IPurifyChamberSet chamber, int position, int value);
	}

	public interface IPurifyChamberScreen : IScreen {
		IPurifyChamberScreen initialize(IPurifyChamberScene scene);

		bool Place(IPokemon pkmn, int position);

		/// <summary>
		/// </summary>
		/// <param name="pos">x: storage box, y: box index</param>
		/// <param name="position"></param>
		/// <returns></returns>
		bool PlacePokemon(int[] pos, int position);

		void OnPlace(IPokemon pkmn);

		bool OpenSetDetail();

		void Display(string msg);

		void Confirm(string msg);

		void Refresh();

		bool CheckPurify();

		void DoPurify();

		void StartPurify();
	}

// ###############################################

	public interface IWindow_PurifyChamberSets //: IWindow_DrawableCommand 
	{
		int switching				{ get; set; }

		IWindow_PurifyChamberSets initialize(IPurifyChamber chamber, float x, float y, int width, int height, IViewport viewport = null);

		int itemCount();

		void drawItem(int index, int count, IRect rect);
	}

	public interface IDirectFlowDiagram {
		IDirectFlowDiagram initialize(IViewport viewport = null);

		// 0=none, 1=weak, 2=strong
		void setFlowStrength(int strength);

		bool visible { set; }
		//	foreach (var point in @points) {
		//		point.visible=value;
		//	}
		//}

		void dispose();

		void ensurePoint(int j);

		void update();

		IColor color { set; }

		void setAngle(float angle1);
	}

	public interface IFlowDiagram {
		IFlowDiagram initialize(IViewport viewport = null);

		// 0=none, 1=weak, 2=strong
		void setFlowStrength(int strength);

		bool visible { set; }
		//foreach (var point in @points) {
		//	point.visible=value;
		//}
		//}

		void dispose();

		void ensurePoint(int j);

		bool withinRange(float angle, float startAngle, float endAngle);

		void update();

		IColor color { set; }

		void setRange(float angle1,float angle2);
	}

	public interface IPurifyChamberSetView : ISpriteWrapper {
		IPurifyChamberSet set			{ get; set; }
		int cursor						{ get; }
		IPokemon heldpkmn				{ get; set; }

		IPurifyChamberSetView initialize(IPurifyChamber chamber, IPurifyChamberSet set, IViewport viewport = null);

		void refreshFlows();

		void moveCursor(int button);

		void checkCursor(int index);

		void refresh();

		IPokemonShadowPokemon getCurrent();

		//void cursor=(value) {
		//@cursor=value;
		//refresh();
		//}
		//
		//IPokemon heldpkmn=(value) {
		//@heldpkmn=value;
		//refresh();
		//}
		//
		//void set=(value) {
		//@set=value;
		//refresh();
		//}

		//bool visible { set; }

		//IColor color { set; }

		//void update();

		//void dispose();
	}

	public interface IPurifyChamberScene : IScene {
		void Update();

		//void Refresh();

		void Start(IPurifyChamber chamber);

		void End();

		void OpenSet(IPurifyChamberSet set);

		void CloseSet();

		void OpenSetDetail(IPurifyChamberSet set);

		void CloseSetDetail();

		void Purify();

		void Move(int pos);

		void Shift(int pos, IPokemon heldpoke);

		void Place(int pos, IPokemon heldpoke);

		void Replace(int pos, int storagePos);

		void Rotate(int facing);

		void Withdraw(int pos, IPokemon heldpoke);

		//void Display(string msg);

		bool Confirm(string msg);

		void ShowCommands(string msg, string[] commands);

		int[] SetScreen();

		int ChooseSet();

		int Switch(int set);

		void Summary(int pos, IPokemon heldpkmn);

		void PositionHint(int pos);

		void ChangeSet(int set);

		int ChoosePokemon();
	}
}
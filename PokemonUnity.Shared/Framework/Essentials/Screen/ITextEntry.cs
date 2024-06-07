using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
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
	public interface IPokemonEntryScreen : IScreen
	{
		void initialize(IPokemonEntryScene scene);
		string StartScreen(string helptext, int minlength, int maxlength, string initialText, PokemonUnity.Interface.TextEntryTypes mode = 0, IPokemon pokemon = null);
	}

	/// <summary>
	/// Scene used for entering text, such as naming a Pokémon or a box.
	/// </summary>
	/// <remarks>
	/// Text entry screen - free typing.
	/// </remarks>
	public interface IPokemonEntryScene : IScene
	{
		void StartScene(string helptext, int minlength, int maxlength, string initialText, PokemonUnity.Interface.TextEntryTypes subject = 0, IPokemon pokemon = null);
		void EndScene();
		string Entry();
		//string Entry1();
		//string Entry2();
	}

	/// <summary>
	/// Scene used for entering text, such as naming a Pokémon or a box.
	/// </summary>
	/// <remarks>
	/// Text entry screen - arrows to select letter.
	/// </remarks>
	public interface IPokemonEntryScene2 : IScene, IPokemonEntryScene
	{
		//void StartScene(string helptext, int minlength, int maxlength, string initialText, int subject = 0, Pokemon pokemon = null);
		//void EndScene();
		//string Entry();
		void Update();
		void ChangeTab(int newtab = 0);
		bool ColumnEmpty(int m);
		void UpdateOverlay();
		void DoUpdateOverlay();
		void DoUpdateOverlay2();
		bool MoveCursor();
		int wrapmod(int x, int y);
	}

	/// <summary>
	/// Extension of <see cref="IGame"/>
	/// </summary>
	public interface IGameTextEntry
	{
		string EnterText(string helptext, int minlength, int maxlength, string initialText = "", int mode = 0, IPokemon pokemon = null, bool nofadeout = false);

		string EnterPlayerName(string helptext, int minlength, int maxlength, string initialText = "", bool nofadeout = false);

		string EnterPokemonName(string helptext, int minlength, int maxlength, string initialText = "", IPokemon pokemon = null, bool nofadeout = false);

		string EnterBoxName(string helptext, int minlength, int maxlength, string initialText = "", bool nofadeout = false);

		string EnterNPCName(string helptext, int minlength, int maxlength, string initialText = "", int id = 0, bool nofadeout = false);
	}

	public interface ICharacterEntryHelper {
		string text { get; set; }
		int maxlength { get; set; }
		char? passwordChar  { get; set; }
		int cursor { get; }

		char[] textChars();

		ICharacterEntryHelper initialize(string text);

		int length();

		bool canInsert();

		bool insert(string ch);

		bool canDelete();

		bool delete();

		//private
		//void ensure;
	}

	public interface IWindow_TextEntry : ISpriteWindow_Base {
		string text { get; set; }
		int maxlength { get; set; }
		char? passwordChar  { get; set; }

		IWindow_TextEntry initialize(string text,int x,int y,int width,int height,string heading=null,bool usedarkercolor=false);

		bool insert(char? ch);
		bool insert(string ch);

		void delete();

		//void update();

		void refresh();
	}

	public interface IWindow_TextEntry_Keyboard : IWindow_TextEntry {
		new void update();
	}

	public interface IWindow_CharacterEntry : IWindow_DrawableCommand {
		//XSIZE=13
		//YSIZE=4

		IWindow_CharacterEntry initialize(string charset,IViewport viewport=null);

		void setOtherCharset(string value);

		void setCharset(string value);

		char? character();

		int command();

		//int itemCount();

		//void drawItem(int index,int count,IRect rect);
	}

	public interface IWindow_MultilineTextEntry : ISpriteWindow_Base {
		string text { get; set; }
		int maxlength { get; set; }
		char? passwordChar  { get; set; }
		IColor baseColor { get; set; }
		IColor shadowColor { get; set; }

		IWindow_MultilineTextEntry initialize(string text,float x,float y,int width,int height);

		bool insert(string ch);

		bool delete();

		string getTextChars();

		int getTotalLines();

		int getLineY(int line);

		int getColumnsInLine(int line);

		int getPosFromLineAndColumn(int line,int column);

		int getLastVisibleLine();

		void updateCursorPos(bool doRefresh);

		void moveCursor(int lineOffset,int columnOffset);

		//void update();

		void refresh();
	}
}
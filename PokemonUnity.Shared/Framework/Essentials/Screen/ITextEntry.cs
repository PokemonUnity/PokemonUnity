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
	/// Text entry screen - free typing.
	/// </summary>
	public interface IPokemonEntryScene : IScene
	{
		void StartScene(string helptext, int minlength, int maxlength, string initialText, PokemonUnity.Interface.TextEntryTypes subject = 0, IPokemon pokemon = null);
		void EndScene();
		string Entry();
		//string Entry1();
		//string Entry2();
	}

	/// <summary>
	/// Text entry screen - arrows to select letter.
	/// </summary>
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
}
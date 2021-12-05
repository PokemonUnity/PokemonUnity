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
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	public interface IWindow_PokemonOption //: Window_DrawableCommand 
	{
		bool mustUpdateOptions				{ get; set; }

		IWindow_PokemonOption initialize(IOptionValue[] options, float x, float y, float width, float height);

		IOptionValue this[int i] { get; set; }

		int itemCount();

		void drawItem(int index, int count, IRect rect);

		void update();
	}

	public interface IPropertyMixin<T> {
		Func<int, T> get();

		void set(Action<int> value);
	}

	public interface IEnumOption : IOptionValue, IPropertyMixin<string>, IEnumOption<string>
	{
		//string values			{ get; }
		//string name			{ get; }

		//IEnumOption initialize(string name, string options, Func<int,string> getProc, Action<int> setProc);

		//int next(int current);

		//int prev(int current);
	}

	public interface IEnumOption<T> : IOptionValue, IPropertyMixin<T>
	{
		T values				{ get; }
		//string name				{ get; }

		IEnumOption<T> initialize(string name, T options, Func<int,T> getProc, Action<int> setProc);

		//int next(int current);

		//int prev(int current);
	}

	public interface INumberOption : IOptionValue, IPropertyMixin<int>
	{
		//string name				{ get; }
		int optstart			{ get; }
		int optend				{ get; }

		INumberOption initialize(string name, int optstart, int optend, Func<int,int> getProc, Action<int> setProc);

		//int next(int current);

		//int prev(int current);
	}

	public interface ISliderOption : IOptionValue, IPropertyMixin<int>
	{
		//string name				{ get; }
		int optstart			{ get; }
		int optend				{ get; }

		ISliderOption initialize(string name, int optstart, int optend, int optinterval, Func<int,int> getProc, Action<int> setProc);

		//int next(int current);

		//int prev(int current);
	}

	// ####################
	// Stores game options
	// Default options are at the top of script section SpriteWindow.
	/// <summary>
	/// Extension of <see cref="IGame"/>
	/// </summary>
	public interface IGameOption
	{

		//string[] SpeechFrames;
		//string[] TextFrames;
		//string[][] VersionStyles;

		int pbSettingToTextSpeed(int speed);
	}

	//public interface IMessageConfig {
	//	public static IBitmap pbDefaultSystemFrame();
	//
	//	public static IBitmap pbDefaultSpeechFrame();
	//
	//	public static string pbDefaultSystemFontName();
	//
	//	public static int pbDefaultTextSpeed();
	//
	//	public int pbGetSystemTextSpeed();
	//}

	public interface IPokemonSystemOption {
		int textspeed				{ get; set; }
		int battlescene			    { get; set; }
		int battlestyle			    { get; set; }
		int frame				    { get; set; }
		int textskin				{ get; }
		string font				    { get; set; }
		int screensize				{ get; set; }
		int language				{ get; }
		int border				    { get; }
		int runstyle				{ get; }
		int bgmvolume				{ get; }
		int sevolume				{ get; }

		int tilemap                 { get; }

		IPokemonSystemOption initialize();
	}

	public interface IOptionScene : IScene {
		void pbUpdate();
		void pbStartScene(bool inloadscreen = false);
		object pbAddOnOptions(object options);
		void pbOptions();
		void pbEndScene();
	}

	public interface IOptionScreen : IScreen {
		IOptionScreen initialize(IOptionScene scene);
		void pbStartScreen(bool inloadscreen = false);
	}

	public interface IOptionValue
	{
		string name { get; }
		int next(int current);
		int prev(int current);
	}
}
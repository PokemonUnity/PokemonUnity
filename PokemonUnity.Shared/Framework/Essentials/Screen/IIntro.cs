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
	public interface IEventScene
	{
		/// <summary>
		/// Action Button Pressed...
		/// </summary>
		bool onATrigger { get; set; }
		/// <summary>
		/// Back/Cancel Button Pressed...
		/// </summary>
		bool onBTrigger { get; set; }
		bool onUpdate { get; set; }
		IEnumerator main();
		IEventScene initialize();
		bool disposed { get; }
		void dispose();
		//void addBitmap(float x, float y, Bitmap bitmap);
		void addLabel(float x, float y, float width, string text);
		void addImage(float x, float y, string name);
		void getPicture(int num);
		void wait(int frames);
		void pictureWait(int extraframes = 0);
		//void addUserSprite(Sprite sprite);
		IEnumerator update();
	}
	/// <summary>
	/// First scene to load when game boots up.
	/// Displays logos and intro credits/sponsors
	/// </summary>
	public interface IIntroEventScene : IEventScene
	{
		void openPic();
		void timer();
		void closePic();
		void openSplash();
		/// <summary>
		/// </summary>
		/// If you press and hold special keys, go to delete screen
		void splashUpdate();
		/// <summary>
		/// Ends intro scene and transition to load/new game screen
		/// </summary>
		void closeSplash();
		/// <summary>
		/// Ends intro scene and transition to delete save data screen
		/// </summary>
		void closeSplashDelete();
	}
	public interface IIntroEventScreen : PokemonEssentials.Interface.Screen.IScreen
	{
		IIntroEventScreen initialize();
		void main();
	}
	/// <summary>
	/// Shows a help screen listing the keyboard controls.
	/// </summary>
	public interface IButtonEventScene : IEventScene
	{
		void pbOnScreen1();
	}
}
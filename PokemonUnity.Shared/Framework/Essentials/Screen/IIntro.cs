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
	public interface IEventScene : IDisposable
	{
		/// <summary>
		/// Action Button Pressed...
		/// </summary>
		//bool onATrigger { get; set; }
		event EventHandler onATrigger;
		/// <summary>
		/// Back/Cancel Button Pressed...
		/// </summary>
		//bool onBTrigger { get; set; }
		event EventHandler onBTrigger;
		/// <summary>
		/// Update has been triggered, and a new frame is called
		/// </summary>
		//bool onUpdate { get; set; }
		event EventHandler onUpdate;
		/// <summary>
		/// Beginning starting point that runs and operates the entire game application
		/// </summary>
		/// <returns>
		/// Enumerates through each frame tick and calls `update` across entire game assembly
		/// </returns>
		/// This is supposed to mimic behavior of Unity's Monobehavior `OnUpdate`
		IEnumerator main();
		IEventScene initialize(IViewport viewport = null);
		bool disposed { get; }
		//void dispose();
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
		void OnScreen1();
	}
}
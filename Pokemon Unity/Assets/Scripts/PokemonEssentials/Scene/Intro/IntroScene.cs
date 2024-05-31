using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace PokemonUnity
{
	public class IntroScene : EventScene, IIntroEventScene
	{
		/// <summary>
		/// Array of images to display before Title/Splash Card
		/// </summary>
		public Image[] pics;
		/// <summary>
		/// Active/Current Image being displayed in Scene
		/// </summary>
		public Image pic;
		/// <summary>
		/// flashing "Press Start" picture
		/// </summary>
		public Image pic2;
		/// <summary>
		/// Background Image used for "Press Start" display. (Title Card)
		/// </summary>
		public Image splash;
		/// <summary>
		/// Id used for Pics array
		/// </summary>
		public int index;
		/// <summary>
		/// Timer for Picture display
		/// </summary>
		public int Timer;
		private IAudioObject title_bgm;

		// Start is called before the first frame update
		void Start()
		{
			initialize();
			StartCoroutine(main()); //While Loop to run indefinitely...
		}

		// Update is called once per frame
		void Update()
		{
			//Every action will be executed once, and the script ends with `update()` which will pause and wait for brief moment.
			//StartCoroutine(main()); //If Check to run once...
		}

		//public void initialize(pics, splash, viewport= null)
		public IIntroEventScene initialize()
		{
			//super(null);
			base.initialize(null);
			//@pics = pics;
			//@splash = splash;
			//@pic = addImage(0, 0, "");
			//@pic.moveOpacity(0, 0, 0); // fade to opacity 0 in 0 frames after waiting 0 frames
			//@pic2 = addImage(0, 322, ""); // flashing "Press Enter" picture
			//@pic2.moveOpacity(0, 0, 0);
			@index = 0;
			//data_system = pbLoadRxData("Data/System");
			//pbBGMPlay(data_system.title_bgm);
			//pbBGMPlay(title_bgm);
			openPic();
			return this;
		}

		public void openPic()
		{
			//onCTrigger.clear();
			ClearOnTriggerA();
			//@pic.name = "Graphics/Titles/" + @pics[@index];
			@pic = @pics[@index];
			//@pic.moveOpacity(15, 0, 255); // fade to opacity 255 in 15 frames after waiting 0 frames
			//we'll handle fading image in co-routine...
			pictureWait();
			Timer = 0; // reset the timer
			//onUpdate.set(method(:timer)); // call timer every frame
			//if (onUpdate) timer();
			onUpdate += IntroScene_onUpdate_Timer;
			//onCTrigger.set(method(:closePic)); // call closePic when C key is pressed
			//if (onATrigger) closePic();
			onATrigger += IntroScene_onATrigger_Pic;
		}

		public void timer()
		{
			@Timer += 1;
			if (@Timer > 80)
			{
				@Timer = 0;
				closePic(); // Close the picture
			}
		}

		public void closePic()
		{
			//onCTrigger.clear();
			ClearOnTriggerA();
			//onUpdate.clear();
			ClearOnUpdate();
			//@pic.moveOpacity(15, 0, 0);
			//set image alpha to 0
			pictureWait();
			@index += 1; // Move to the next picture
			if (@index >= @pics.Length)
			{
				//if index is at end, then change scene to splash scene
				openSplash();
			}
			else
			{
				//else next image
				openPic();
			}
		}

		public void openSplash()
		{
			//onCTrigger.clear();
			ClearOnTriggerA();
			//onUpdate.clear();
			ClearOnUpdate();
			//@pic.name = "Graphics/Titles/" + @splash;
			@pic= @splash;
			//@pic.moveOpacity(15, 0, 255);
			//@pic2.name = "Graphics/Titles/start";
			//@pic2.moveOpacity(15, 0, 255);
			//pic2 is set in inspector, just change opacity
			pictureWait();
			//onUpdate.set(method(:splashUpdate));  // call splashUpdate every frame
			//if (onUpdate) splashUpdate();
			onUpdate += IntroScene_onUpdate_Splash;
			//onCTrigger.set(method(:closeSplash)); // call closeSplash when C key is pressed
			//if (onATrigger) closeSplash();
			onUpdate += IntroScene_onATrigger_Splash;
			//this is should be moved up `update()` add an if (splash) check
		}

		public void splashUpdate()
		{
			@Timer += 1;
			if (@Timer >= 80) @Timer = 0;
			if (@Timer >= 32)
			{
				//@pic2.moveOpacity(0, 0, 8 * (@Timer - 32));
				//Change opacity -- fade out?
			}
			else
			{
				//@pic2.moveOpacity(0, 0, 255 - (8 * @Timer));
				//Change opacity -- fade in?
			}
			// Can be whatever combination of buttons you design in your game
			if (PokemonUnity.Input.press(PokemonUnity.Input.DOWN) &&
				PokemonUnity.Input.press(PokemonUnity.Input.A) &&
				PokemonUnity.Input.press(PokemonUnity.Input.CTRL))
			{
			    closeSplashDelete();
			}
		}

		public void closeSplash()
		{
			//onCTrigger.clear();
			ClearOnTriggerA();
			//onUpdate.clear();
			ClearOnUpdate();
			//  Play random cry
			//IAudioObject cry = pbCryFile(1 + Core.Rand.Next(PBSpecies.maxValue));
			//if (cry != null) pbSEPlay(cry, 80, 100);
			//Unity Custom Sound Script...
			//  Fade out
			//@pic.moveOpacity(15, 0, 0);
			//@pic2.moveOpacity(15, 0, 0);
			//pbBGMStop(1.0);
			pictureWait();
			//scene.dispose(); // Close the scene
			(this as IDisposable).Dispose(); // Close the scene
			//Destroy scene and load next scene
			//IPokemonLoadScene sscene = new PokemonLoadScene();
			//IPokemonLoad sscreen = new PokemonLoad(sscene);
			ILoadScene sscene = GameEvents.game.Scenes.Load;
			ILoadScreen sscreen = GameEvents.game.Screens.Load.initialize(sscene);
			sscreen.StartLoadScreen();
			//transition to load/new game scene
			//Havent finished translating best way to register data between scenes yet
		}

		/// <summary>
		/// Load the scene that gives player the option to delete their Saved game state
		/// </summary>
		public void closeSplashDelete()
		{
			//onCTrigger.clear();
			ClearOnTriggerA();
			//onUpdate.clear();
			ClearOnUpdate();
			//  Play random cry
			//cry = pbCryFile(1 + Core.Rand.Next(PBSpecies.maxValue));
			//IAudioObject cry = pbCryFile(1 + Core.Rand.Next(Kernal.PokemonData.Count));
			//if (cry != null) pbSEPlay(cry, 80, 100);
			//  Fade out
			//@pic.moveOpacity(15, 0, 0);
			//@pic2.moveOpacity(15, 0, 0);
			//pbBGMStop(1.0);
			pictureWait();
			//scene.dispose(); // Close the scene
			(this as IDisposable).Dispose(); // Close the scene
			//Destroy scene and load next scene
			//IPokemonLoadScene sscene = new PokemonLoadScene();
			//IPokemonLoad sscreen = new PokemonLoad(sscene);
			ILoadScene sscene = GameEvents.game.Scenes.Load;
			ILoadScreen sscreen = GameEvents.game.Screens.Load.initialize(sscene);
			sscreen.StartDeleteScreen();
			//transition to delete save scene
		}

		/// <summary>
		/// call timer every frame
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void IntroScene_onUpdate_Timer(object sender, System.EventArgs e)
		{
			timer();
		}

		/// <summary>
		/// call splashUpdate every frame
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void IntroScene_onUpdate_Splash(object sender, System.EventArgs e)
		{
			splashUpdate();
		}

		/// <summary>
		/// call closePic when Action key is pressed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void IntroScene_onATrigger_Pic(object sender, System.EventArgs e)
		{
			closePic();
		}

		/// <summary>
		/// call closeSplash when Action key is pressed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void IntroScene_onATrigger_Splash(object sender, System.EventArgs e)
		{
			closePic();
		}
	}
}
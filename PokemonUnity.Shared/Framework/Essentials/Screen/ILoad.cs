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
	public interface ILoadScene : IScene {
		IEnumerator pbUpdate();

		void pbStartScene(string[] commands, bool showContinue, ITrainer trainer, int framecount, int mapid);

		void pbStartScene2();

		void pbStartDeleteScene();

		void pbSetParty(ITrainer trainer);

		void pbChoose(string[] commands);

		void pbEndScene();

		void pbCloseScene();
	}

	public interface ILoadScreen : IScreen {
		ILoadScreen initialize(ILoadScene scene);

		//return [trainer,framecount,game_system,pokemonSystem,mapid]
		object pbTryLoadFile(string savefile);

		void pbStartDeleteScreen();

		void pbStartLoadScreen();
	}

	#region UI Elements
	public interface ILoadPanel : ISpriteWrapper, IDisposable {
		bool selected				{ get; set; }

		ILoadPanel initialize(int index, string title, bool isContinue, ITrainer trainer, int framecount, int mapid, IViewport viewport = null);

		//void dispose();

		//void selected=(value) {
		//  if (@selected!=value) {
		//    @selected=value;
		//    @refreshBitmap=true;
		//    refresh();
		//  }
		//}

		void pbRefresh();

		void refresh();
	}
	#endregion
}
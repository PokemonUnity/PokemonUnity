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
	public interface IPokemonSummaryScene : IScene
	{
		//IPokemonSummaryScene initialize();
		//void drawMarkings(bitmap,int x,int y,int width,int height, bool[] markings);
		void drawPageOne(IPokemon pokemon);
		void drawPageOneEgg(IPokemon pokemon);
		void drawPageTwo(IPokemon pokemon);
		void drawPageThree(IPokemon pokemon);
		void drawPageFour(IPokemon pokemon);
		void drawPageFive(IPokemon pokemon);
		void drawMoveSelection(IPokemon pokemon, Moves moveToLearn);
		void drawSelectedMove(IPokemon pokemon, Moves moveToLearn, Moves moveid);
		void pbChooseMoveToForget(Moves moveToLearn);
		void pbEndScene();
		void pbGoToNext();
		void pbGoToPrevious();
		void pbMoveSelection();
		void pbPokerus(IPokemon pkmn);
		void pbScene();
		void pbStartForgetScene(IPokemon party, int partyindex, Moves moveToLearn);
		void pbStartScene(IPokemon party, int partyindex);
		void pbUpdate();
	}

	public interface IPokemonSummary : IScreen
	{
		IPokemonSummary initialize(IPokemonSummaryScene scene);
		void pbStartScreen(IPokemon[] party, int partyindex);
		//int pbStartForgetScreen(IPokemon[] party, int partyindex, Moves moveToLearn);
		int pbStartForgetScreen(IPokemon party, int partyindex, Moves moveToLearn);
		void pbStartChooseMoveScreen(IPokemon[] party, int partyindex, string message);
	}

	public interface IMoveSelectionSprite : ISpriteWrapper, IDisposable 
    {
        int preselected				{ get; set; }
        int index				    { get; set; }
		bool visible { set; }

		IMoveSelectionSprite initialize(IViewport viewport = null, bool fifthmove = false);

		void dispose();

		//void index=(value) {
		//@index=value;
		//refresh();
		//}

		//public void preselected=(value) {
		//@preselected=value;
		//refresh();
		//}

		void refresh();

		void update();
	}
}
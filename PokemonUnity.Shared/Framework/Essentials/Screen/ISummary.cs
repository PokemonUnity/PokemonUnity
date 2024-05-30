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
	public interface IPokemonSummaryScene : IScene
	{
		IPokemonSummaryScene initialize();
		//void drawMarkings(bitmap,int x,int y,int width,int height, bool[] markings);
		void drawPageOne(IPokemon pokemon);
		void drawPageOneEgg(IPokemon pokemon);
		void drawPageTwo(IPokemon pokemon);
		void drawPageThree(IPokemon pokemon);
		void drawPageFour(IPokemon pokemon);
		void drawPageFive(IPokemon pokemon);
		void drawMoveSelection(IPokemon pokemon, Moves moveToLearn);
		void drawSelectedMove(IPokemon pokemon, Moves moveToLearn, Moves moveid);
		void ChooseMoveToForget(Moves moveToLearn);
		void EndScene();
		void GoToNext();
		void GoToPrevious();
		void MoveSelection();
		void Pokerus(IPokemon pkmn);
		void Scene();
		void StartForgetScene(IPokemon party, int partyindex, Moves moveToLearn);
		void StartScene(IPokemon party, int partyindex);
		void Update();
	}

	public interface IPokemonSummaryScreen : IScreen
	{
		IPokemonSummaryScreen initialize(IPokemonSummaryScene scene);
		void StartScreen(IPokemon[] party, int partyindex);
		//int StartForgetScreen(IPokemon[] party, int partyindex, Moves moveToLearn);
		int StartForgetScreen(IPokemon party, int partyindex, Moves moveToLearn);
		void StartChooseMoveScreen(IPokemon[] party, int partyindex, string message);
		int StartChooseMoveScreen(IPokemon party, int partyindex, string message);
	}

	public interface IMoveSelectionSprite : ISpriteWrapper, IDisposable
	{
		int preselected				{ get; set; }
		int index				    { get; set; }
		//bool visible				{ set; }

		IMoveSelectionSprite initialize(IViewport viewport = null, bool fifthmove = false);

		//void dispose();

		//void index=(value) {
		//@index=value;
		//refresh();
		//}

		//public void preselected=(value) {
		//@preselected=value;
		//refresh();
		//}

		void refresh();

		//void update();
	}
}
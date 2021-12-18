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
	public interface IPartyDisplayScene : IScene
	{
		//IPartyDisplayScene initialize();
		void pbShowCommands(string helptext, string[] commands,int index= 0);
		void update();
		void pbSetHelpText(string helptext);
		void pbStartScene(IPokemon[] party,string starthelptext,string[] annotations= null,bool multiselect= false);
		void pbEndScene();
		/// <summary>
		/// </summary>
		/// <param name="key">Controller Input Key</param>
		/// <param name="currentsel"></param>
		void pbChangeSelection(int key,int currentsel);
		//void pbRefresh();
		void pbHardRefresh();
		void pbPreSelect(IPokemon pkmn);
		void pbChoosePokemon(bool switching= false, int initialsel= -1);
		void pbSelect(Items item);
		//void pbDisplay(string text);
		void pbSwitchBegin(int oldid,int newid);
		void pbSwitchEnd(int oldid,int newid);
		void pbDisplayConfirm(string text);
		void pbAnnotate(string[] annot);
		void pbSummary(int pkmnid);
		void pbChooseItem(Items[] bag);
		void pbUseItem(Items[] bag,IPokemon pokemon); 
		void pbMessageFreeText(string text,string startMsg,int maxlength);
	}

	public interface IPartyDisplayScreen : IScreen, IHasDisplayMessage
	{
		IPartyDisplayScreen initialize(IPartyDisplayScene scene, IPokemon[] party);
		void pbHardRefresh();
		void pbRefresh();
		void pbRefreshSingle(int i);
		//void pbDisplay(string text);
		void pbConfirm(string text);
		void pbSwitch(int oldid,int newid);
		void pbMailScreen(Items item, IPokemon pkmn, int pkmnid);
		void pbTakeMail(IPokemon pkmn);
		void pbGiveMail(Items item,IPokemon pkmn,int pkmnid= 0);
		void pbPokemonGiveScreen(Items item);
		void pbPokemonGiveMailScreen(int mailIndex);
		void pbStartScene(string helptext,bool doublebattle,string[] annotations= null);
		int pbChoosePokemon(string helptext= null);
		void pbChooseMove(IPokemon pokemon,string helptext);
		void pbEndScene();
		/// <summary>
		/// Checks for identical species
		/// </summary>
		/// <param name=""></param>
		void pbCheckSpecies(Pokemons[] array);
		/// <summary>
		/// Checks for identical held items
		/// </summary>
		/// <param name=""></param>
		void pbCheckItems(Items[] array);
		void pbPokemonMultipleEntryScreenEx(string[] ruleset);
		int pbChooseAblePokemon(Func<IPokemon,bool> ableProc,bool allowIneligible= false);
		void pbRefreshAnnotations(bool ableProc);
		void pbClearAnnotations();
		void pbPokemonDebug(IPokemon pkmn, int pkmnid);
		void pbPokemonScreen();
	}

	#region UI Elements
	public interface IPokeSelectionPlaceholderSprite : ISpriteWrapper 
	{
		string text				{ get; set; }

		void initialize(IPokemon pokemon, int index, IViewport viewport= null);

		//void update();

		bool selected { get; set; }

		bool preselected { get; set; }

		bool switching { get; set; }

		void refresh();

		//void dispose();
	}

	public interface IPokeSelectionConfirmCancelSprite : ISpriteWrapper, IDisposable {
		//int selected				{ get; set; }

		IPokeSelectionConfirmCancelSprite initialize(string text, float x, float y, bool narrowbox = false, IViewport viewport = null);

		//void dispose();

		//IViewport viewport { get; set; }

		//IColor color { get; set; }

		//float x { get; set; }

		//float y { get; set; }

		bool selected { get; set; }

		void refresh();
	}

	public interface IPokeSelectionCancelSprite : IPokeSelectionConfirmCancelSprite {
		//IPokeSelectionCancelSprite initialize(IViewport viewport= null);
	}

	public interface IPokeSelectionConfirmSprite : IPokeSelectionConfirmCancelSprite {
		//IPokeSelectionConfirmSprite initialize(IViewport viewport=null);
	}

	public interface IPokeSelectionCancelSprite2 : IPokeSelectionConfirmCancelSprite {
		//IPokeSelectionCancelSprite2 initialize(IViewport viewport= null);
	}

	public interface IChangelingSprite : ISpriteWrapper {
		IChangelingSprite initialize(float x=0,float y=0,IViewport viewport=null);

		void addBitmap(int key,string path);

		void changeBitmap(int key);

		//void dispose();

		//void update();
	}

	public interface IPokeSelectionSprite : ISpriteWrapper {
		//bool selected				{ get; protected set; }
		//bool preselected			{ get; protected set; }
		//bool switching			{ get; protected set; }
		//IPokemon pokemon			{ get; protected set; }
		//bool active				{ get; protected set; }
		//string text				{ get; protected set; }

		IPokeSelectionSprite initialize(IPokemon pokemon, int index, IViewport viewport = null);

		//void dispose();

		bool selected { get; set; }

		string text { get; set; }

		IPokemon pokemon { get; set; }

		bool preselected { get; set; }

		bool switching { get; set; }

		//IColor color { get; set; }

		//float x { get; set; }

		//float y { get; set; }

		int hp { get; }

		void refresh();

		//void update();
	}
	#endregion
}
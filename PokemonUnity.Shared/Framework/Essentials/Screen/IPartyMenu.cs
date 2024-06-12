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
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	public interface IPartyDisplayScene : IScene
	{
		IPartyDisplayScene initialize();
		int ShowCommands(string helptext, string[] commands,int index= 0);
		void update();
		void SetHelpText(string helptext);
		void StartScene(IPokemon[] party,string starthelptext,string[] annotations= null,bool multiselect= false);
		void EndScene();
		/// <summary>
		/// </summary>
		/// <param name="key">Controller Input Key</param>
		/// <param name="currentsel"></param>
		void ChangeSelection(int key,int currentsel);
		//void Refresh();
		void HardRefresh();
		void PreSelect(IPokemon pkmn);
		void ChoosePokemon(bool switching= false, int initialsel= -1);
		int ChoosePokemon(string helptext= null);			//Moved from DisplayScreen
		int ChooseMove(IPokemon pokemon,string helptext);	//Moved from DisplayScreen
		void Select(Items item);
		//void Display(string text);
		void SwitchBegin(int oldid,int newid);
		void SwitchEnd(int oldid,int newid);
		//void DisplayConfirm(string text);
		void Annotate(string[] annot);
		void Summary(int pkmnid);
		void ChooseItem(Items[] bag);
		void UseItem(Items[] bag,IPokemon pokemon);
		void MessageFreeText(string text,string startMsg,int maxlength);
	}

	public interface IPartyDisplayScreen : IScreen, IHasDisplayMessage
	{
		IPartyDisplayScreen initialize(IPartyDisplayScene scene, IList<IPokemon> party);
		void HardRefresh();
		//void Refresh();
		void RefreshSingle(int i);
		//void Display(string text);
		//void Confirm(string text);
		void Switch(int oldid,int newid);
		void MailScreen(Items item, IPokemon pkmn, int pkmnid);
		void TakeMail(IPokemon pkmn);
		void GiveMail(Items item,IPokemon pkmn,int pkmnid= 0);
		void PokemonGiveScreen(Items item);
		void PokemonGiveMailScreen(int mailIndex);
		void StartScene(string helptext,bool doublebattle,string[] annotations= null);
		int ChoosePokemon(string helptext= null);				//Moved to Scene...
		//void ChooseMove(IPokemon pokemon,string helptext);	//Moved to Scene...
		void EndScene();
		/// <summary>
		/// Checks for identical species
		/// </summary>
		/// <param name=""></param>
		void CheckSpecies(Pokemons[] array);
		/// <summary>
		/// Checks for identical held items
		/// </summary>
		/// <param name=""></param>
		void CheckItems(Items[] array);
		IPokemon[] PokemonMultipleEntryScreenEx(IPokemonRuleSet ruleset);
		int ChooseAblePokemon(Predicate<IPokemon> ableProc,bool allowIneligible= false);
		//void RefreshAnnotations(bool ableProc);
		void RefreshAnnotations(Predicate<IPokemon> ableProc);
		void ClearAnnotations();
		void PokemonDebug(IPokemon pkmn, int pkmnid);
		void PokemonScreen();
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
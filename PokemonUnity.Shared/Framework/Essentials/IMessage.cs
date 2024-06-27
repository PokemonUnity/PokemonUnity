using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// <see cref="IGameTemp"/>
	/// </summary>
	public interface IGameTempMessage : IGameTemp
	{
		int background							{ get; set; }
		bool message_window_showing			    { get; }
		bool player_transferring				{ get; }
		bool transition_processing				{ get; }

		//bool message_window_showing();
		//bool player_transferring();
		//bool transition_processing();
	}

	public interface IMessage {
		int background					{ get; }
		bool visible					{ get; }
		//bool visible();
		//int background();
	}

	/// <summary>
	/// Extension of <seealso cref="IGameSystem"/>
	/// </summary>
	public interface IGameSystemMessage : IGameSystem {
		int message_position				{ get; }

		//int message_position();
	}

// ########

	public interface ISceneMapMessage : ISceneMap {
		void updatemini();
	}

	//public interface ISceneBattleMessage {
	//	void updatemini();
	//}

	/// <summary>
	/// Extension of <see cref="IGame"/>
	/// </summary>
	public interface IGameMessage : IGame
	{
		bool MapInterpreterRunning();

		IInterpreter MapInterpreter();
		//	if (Game.GameData.GameMap && Game.GameData.GameMap.respond_to("interpreter")) {
		//		return Game.GameData.GameMap.interpreter;
		//	} else if (Game.GameData.GameSystem) {
		//		return Game.GameData.GameSystem.map_interpreter;
		//	}
		//	return null;
		//}

		void RefreshSceneMap();

		void UpdateSceneMap();
		// ########

		string CsvField(string str);

		int CsvPosInt(string str);

		int? EventCommentInput(IGameCharacter @event, int elements, string trigger);

		int? CurrentEventCommentInput(int elements, string trigger);



		int ButtonInputProcessing(int variableNumber = 0, int timeoutFrames = 0);



		int ChooseNumber(IWindow msgwindow, IChooseNumberParams param, Action block = null);

		int ShowCommandsWithHelp(IWindow msgwindow,string[] commands,string[] help,int cmdIfCancel=0,int defaultCmd=0, Action block = null);

		int ShowCommands(IWindow msgwindow,string[] commands=null,int cmdIfCancel=0,int defaultCmd=0, Action block = null);

		void PositionFaceWindow(IWindow facewindow,IWindow msgwindow);

		void PositionNearMsgWindow(IWindow cmdwindow,IWindow msgwindow,bool side);

		string GetBasicMapNameFromId(int id);

		string GetMapNameFromId(int id);

		int Message(string message,string[] commands=null,int cmdIfCancel=0,int? skin=null,int defaultCmd=0,Action block = null);

		//int MessageChooseNumber(string message,string[] param,Action block = null);
		int MessageChooseNumber(string message,IChooseNumberParams param,Action block = null);

		bool ConfirmMessage(string message,Action block = null);

		bool ConfirmMessageSerious(string message,Action block = null);

		IWindow_AdvancedTextPokemon CreateStatusWindow(IViewport viewport=null);

		IWindow_AdvancedTextPokemon CreateMessageWindow(IViewport viewport=null,int? skin=null);

		//ToDo: Return IDisposable?...
		void DisposeMessageWindow(IWindow msgwindow);



		string  itemIconTag(Items item);

		IColor getSkinColor(IWindow windowskin,IColor color,bool isDarkSkin);

		// internal function
		void RepositionMessageWindow(IWindow msgwindow, int linecount=2);

		// internal function
		void UpdateMsgWindowPos(IWindow msgwindow,IGameEvent @event,bool eventChanged=false);

		// internal function
		string GetGoldString();

		IWindow_AdvancedTextPokemon DisplayGoldWindow(IWindow msgwindow);

		IWindow_AdvancedTextPokemon DisplayCoinsWindow(IWindow msgwindow,IWindow goldwindow);

		void MessageWaitForInput(IWindow msgwindow,int frames,bool showPause=false);

		void MessageDisplay(IWindow msgwindow,string message,bool letterbyletter=true,Action commandProc=null);
	}

	public interface IInterpreterMixinMessage { //static
		/// <summary>
		/// Freezes all events on the map (for use at the beginning of common events)
		/// </summary>
		void GlobalLock();

		/// <summary>
		/// Unfreezes all events on the map (for use at the end of common events)
		/// </summary>
		void GlobalUnlock();

		int RepeatAbove(int index);

		int BreakLoop(int index);

		void JumpToLabel(int index, string label_name);

		/// <summary>
		/// Gets the next index in the interpreter, ignoring
		/// certain events between messages
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		int NextIndex(int index);

		/// <summary>
		/// Helper function that shows a picture in a script.  To be used in
		/// a script event command.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="name"></param>
		/// <param name="origin"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="zoomX"></param>
		/// <param name="zoomY"></param>
		/// <param name="opacity"></param>
		/// <param name="blendType"></param>
		void ShowPicture(int number, string name, float origin, float x, float y, int zoomX = 100, int zoomY = 100, byte opacity = 255, int blendType = 0);

		/// <summary>
		/// Erases an event and adds it to the list of erased events so that
		/// it can stay erased when the game is saved then loaded again.  To be used in
		/// a script event command.
		/// </summary>
		/// <returns></returns>
		bool EraseThisEvent();

		/// <summary>
		/// Runs a common event.  To be used in a script event command.
		/// </summary>
		/// <param name="id"></param>
		void CommonEvent(int id);

		/// <summary>
		/// Sets another event's self switch (eg. SetSelfSwitch(20,"A",true) ).
		/// To be used in a script event command.
		/// </summary>
		/// <param name="event"></param>
		/// <param name="swtch"></param>
		/// <param name="value"></param>
		void SetSelfSwitch(int @event, string swtch, bool value);

		// Must use this approach to share the methods because the methods already
		// defined in a class override those defined in an included module
		//CustomEventCommands=<<_END_;

		bool command_242();
		bool command_246();
		bool command_251();
		bool command_241();
		bool command_245();
		bool command_249();
		bool command_250();
		//_END_;
	}

	/// <summary>
	/// Used by RMVX
	/// </summary>
	public interface IGameInterpreterMessage {   //static
		//include IInterpreterMixinMessage; InterpreterMixin;
		//eval(InterpreterMixin.CustomEventCommands);
		//@@immediateDisplayAfterWait=false;
		//@buttonInput=false;

		//void Params() {
		//	return @params;
		//}

		#region Commands
		bool command_105();
		bool command_101();
		bool command_102();
		bool command_103();
		#endregion
	}

	/// <summary>
	/// Used by RMXP
	/// </summary>
	public interface IInterpreterMessage {
		//include InterpreterMixin;
		//eval(InterpreterMixin.CustomEventCommands);
		//@@immediateDisplayAfterWait=false;
		//@buttonInput=false;

		//void Params() {
		//	return @parameters;
		//}

		bool command_105();
		bool command_101();
		bool command_102();
		bool command_103();
	}

	public interface IChooseNumberParams {
		IChooseNumberParams initialize();

		void setMessageSkin(int value);

		/// <summary>
		/// Set the full path for the message's window skin
		/// </summary>
		void messageSkin();

		void setSkin(int value);

		void skin();

		void setNegativesAllowed(bool value);

		void negativesAllowed();

		void setRange(int minNumber,int maxNumber);

		void setDefaultValue(int number);

		void setInitialValue(int number);

		void setCancelValue(int number);

		int initialNumber();

		int cancelNumber();

		int minNumber();

		int maxNumber();

		void setMaxDigits(int value);

		int maxDigits();

		//private;

		//void clamp(int v,int mn,int mx);

		//int numDigits(int number);
	}

	public interface IFaceWindowVX : ISpriteWindow_Base
	{
		IFaceWindowVX initialize(string face);

		//void update();
		//void dispose();
	}
}
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
	public interface IGameTempMessage 
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

	public interface IGameSystemMessage {
		int message_position				{ get; }

		//int message_position();
	}

// ########

	public interface ISceneMapMessage {
		void updatemini();
	}

	public interface ISceneBattleMessage {
		void updatemini();
	}

	/// <summary>
	/// <see cref="IGame"/>
	/// </summary>
	public interface IGameMessage
	{
		bool pbMapInterpreterRunning();

		IInterpreter pbMapInterpreter();
		//	if (Game.GameData.GameMap && Game.GameData.GameMap.respond_to("interpreter")) {
		//	return Game.GameData.GameMap.interpreter;
		//	} else if (Game.GameData.GameSystem) {
		//	return Game.GameData.GameSystem.map_interpreter;
		//	}
		//	return null;
		//}

		void pbRefreshSceneMap();

		void pbUpdateSceneMap();
		// ########

		string pbCsvField(string str);

		int pbCsvPosInt(string str);

		int? pbEventCommentInput(IGameCharacter @event, int elements, string trigger);

		int? pbCurrentEventCommentInput(int elements, string trigger);



		int pbButtonInputProcessing(int variableNumber = 0, int timeoutFrames = 0);



		int pbChooseNumber(IWindow msgwindow, IChooseNumberParams param, Action block = null);

		int pbShowCommandsWithHelp(IWindow msgwindow,string[] commands,string[] help,int cmdIfCancel=0,int defaultCmd=0, Action block = null);

		int pbShowCommands(IWindow msgwindow,string[] commands=null,int cmdIfCancel=0,int defaultCmd=0, Action block = null);

		void pbPositionFaceWindow(IWindow facewindow,IWindow msgwindow);

		void pbPositionNearMsgWindow(IWindow cmdwindow,IWindow msgwindow,bool side);

		string pbGetBasicMapNameFromId(int id);

		string pbGetMapNameFromId(int id);

		int pbMessage(string message,string[] commands=null,int cmdIfCancel=0,int? skin=null,int defaultCmd=0, Action block = null); //ref Action block

		int pbMessageChooseNumber(string message,string[] param, ref Action block); //Action block = null

		bool pbConfirmMessage(string message,Action block = null);

		bool pbConfirmMessageSerious(string message,Action block = null);

		IWindow_AdvancedTextPokemon pbCreateStatusWindow(IViewport viewport=null);

		IWindow_AdvancedTextPokemon pbCreateMessageWindow(IViewport viewport=null,int? skin=null);

		void pbDisposeMessageWindow(IWindow msgwindow);



		string  itemIconTag(Items item);

		IColor getSkinColor(IWindow windowskin,IColor color,bool isDarkSkin);

		// internal function
		void pbRepositionMessageWindow(IWindow msgwindow, int linecount=2);

		// internal function
		void pbUpdateMsgWindowPos(IWindow msgwindow,IGameEvent @event,bool eventChanged=false);

		// internal function

		string pbGetGoldString();

		IWindow_AdvancedTextPokemon pbDisplayGoldWindow(IWindow msgwindow);

		IWindow_AdvancedTextPokemon pbDisplayCoinsWindow(IWindow msgwindow,IWindow goldwindow);

		void pbMessageWaitForInput(IWindow msgwindow,int frames,bool showPause=false);

		void pbMessageDisplay(IWindow msgwindow,string message,bool letterbyletter=true,Action commandProc=null);
	}

	public interface IInterpreterMixinMessage { //static
		/// <summary>
		/// Freezes all events on the map (for use at the beginning of common events)
		/// </summary>
		void pbGlobalLock();

		/// <summary>
		/// Unfreezes all events on the map (for use at the end of common events)
		/// </summary>
		void pbGlobalUnlock();

		int pbRepeatAbove(int index);

		int pbBreakLoop(int index);

		void pbJumpToLabel(int index, string label_name);

		/// <summary>
		/// Gets the next index in the interpreter, ignoring
		/// certain events between messages
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		int pbNextIndex(int index);

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
		void pbShowPicture(int number, string name, float origin, float x, float y, int zoomX = 100, int zoomY = 100, byte opacity = 255, int blendType = 0);

		/// <summary>
		/// Erases an event and adds it to the list of erased events so that
		/// it can stay erased when the game is saved then loaded again.  To be used in
		/// a script event command.
		/// </summary>
		/// <returns></returns>
		bool pbEraseThisEvent();

		/// <summary>
		/// Runs a common event.  To be used in a script event command.
		/// </summary>
		/// <param name="id"></param>
		void pbCommonEvent(int id);

		/// <summary>
		/// Sets another event's self switch (eg. pbSetSelfSwitch(20,"A",true) ).
		/// To be used in a script event command.
		/// </summary>
		/// <param name="event"></param>
		/// <param name="swtch"></param>
		/// <param name="value"></param>
		void pbSetSelfSwitch(int @event, string swtch, bool value);

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

		//void pbParams() {
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

		//void pbParams() {
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
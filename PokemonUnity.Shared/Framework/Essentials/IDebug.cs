using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface
{
	public interface IPokemonDataCopy {
		string dataOldHash				{ get; }
		string dataNewHash				{ get; }
		DateTime dataTime				{ get; }
		string data						{ get; }

		string crc32(string x);

		string readfile(string filename);

		void writefile(string str, string filename);

		DateTime filetime(string filename);

		void initialize(string data, string datasave);

		bool changed { get; }

		void save(string newtilesets);
	}

	public interface IPokemonDataWrapper {
		int data				{ get; }

		void initialize(string file, string savefile, string prompt);

		void save();
	}

	public interface ICommandList {
		void initialize();

		string getCommand(int index);

		void add(string key, string value);

		void list();
	}

	public interface IGameDebug : IGame {
		IList<object> pbMapTree();

		void pbExtractText();

		void pbCompileTextUI();


		int pbDefaultMap();

		ITilePosition pbWarpToMap();

		void pbDebugMenu();

		void pbDebugSetVariable(int id, int diff);

		void pbDebugVariableScreen(int id);

		void pbDebugScreen(int mode);
	}


	//public interface ISpriteWindow_DebugRight : IWindow_DrawableCommand {
	//	public int mode				{ get; protected set; }
	//
	//	public void initialize() {
	//		super(0, 0, Graphics.width, Graphics.height);
	//	}
	//
	//	public void shadowtext(x,y,w,h,t,align=0) {
	//		width=this.contents.text_size(t).width;
	//		if (align==2) {
	//			x+=(w-width);
	//		} else if (align==1) {
	//			x+=(w/2)-(width/2);
	//		}
	//		pbDrawShadowText(this.contents,x,y,[width,w].max,h,t,
	//			new Color(12*8,12*8,12*8),new Color(26*8,26*8,25*8));
	//	}
	//
	//	public void drawItem(index,count,rect) {
	//		pbSetNarrowFont(this.contents);
	//		if (@mode == 0) {
	//			name = Game.GameData.DataSystem.switches[index+1];
	//			status = Game.GameData.GameSwitches[index+1] ? "[ON]" : "[OFF]";
	//		} else {
	//			name = Game.GameData.DataSystem.variables[index+1];
	//			status = Game.GameData.GameVariables[index+1].ToString();
	//		}
	//		if (name == null) {
	//			name = '';
	//		}
	//		id_text = string.Format("%04d:", index+1);
	//		width = this.contents.text_size(id_text).width;
	//		rect=drawCursor(index,rect);
	//		totalWidth=rect.width;
	//		idWidth=totalWidth*15/100;
	//		nameWidth=totalWidth*65/100;
	//		statusWidth=totalWidth*20/100;
	//		this.shadowtext(rect.x, rect.y, idWidth, rect.height, id_text);
	//		this.shadowtext(rect.x+idWidth, rect.y, nameWidth, rect.height, name);
	//		this.shadowtext(rect.x+idWidth+nameWidth, rect.y, statusWidth, rect.height, status, 2);
	//	}
	//
	//	public void itemCount() {
	//		return (@mode==0) ? Game.GameData.DataSystem.switches.size-1 : Game.GameData.DataSystem.variables.size-1;
	//	}
	//
	//	public void mode=(mode) {
	//		@mode = mode;
	//		refresh();
	//	}
	//}


	public interface ISceneDebug { //: IScene
		void main();
	}
}
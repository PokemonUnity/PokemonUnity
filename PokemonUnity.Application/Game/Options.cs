using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;


namespace PokemonUnity
{
	// ####################
	// 
	// Stores game options
	// Default options are at the top of script section SpriteWindow.
	public partial class Game { 
		/// <summary>
		/// Folder Directory: Dialog Skin
		/// </summary>
		public static string[] SpeechFrames=new string[] {
			//MessageConfig.TextSkinName, // Default: speech hgss 1
			"speech hgss 1",
			"speech hgss 2",
			"speech hgss 3",
			"speech hgss 4",
			"speech hgss 5",
			"speech hgss 6",
			"speech hgss 7",
			"speech hgss 8",
			"speech hgss 9",
			"speech hgss 10",
			"speech hgss 11",
			"speech hgss 12",
			"speech hgss 13",
			"speech hgss 14",
			"speech hgss 15",
			"speech hgss 16",
			"speech hgss 17",
			"speech hgss 18",
			"speech hgss 19",
			"speech hgss 20",
			"speech pl 18",
			"speech dp 1.png",
			"speech dp 2.png",
			"speech dp 3.png",
			"speech dp 4.png",
			"speech dp 5.png",
			"speech dp 6.png",
			"speech dp 7.png",
			"speech dp 8.png",
			"speech dp 9.png",
			"speech dp 10.png",
			"speech dp 11.png",
			"speech dp 12.png",
			"speech dp 13.png",
			"speech dp 14.png",
			"speech dp 15.png",
			"speech dp 16.png",
			"speech dp 17.png",
			"speech dp 18.png",
			"speech dp 19.png",
			"speech dp 20.png",
			"speech em.png",
			"speech frlg.png",
			"speech hgss 1.png",
			"speech hgss 2.png",
			"speech hgss 3.png",
			"speech hgss 4.png",
			"speech hgss 5.png",
			"speech hgss 6.png",
			"speech hgss 7.png",
			"speech hgss 8.png",
			"speech hgss 9.png",
			"speech hgss 10.png",
			"speech hgss 11.png",
			"speech hgss 12.png",
			"speech hgss 13.png",
			"speech hgss 14.png",
			"speech hgss 15.png",
			"speech hgss 16.png",
			"speech hgss 17.png",
			"speech hgss 18.png",
			"speech hgss 19.png",
			"speech hgss 20.png",
			"speech pl 1.png",
			"speech pl 2.png",
			"speech pl 3.png",
			"speech pl 4.png",
			"speech pl 5.png",
			"speech pl 6.png",
			"speech pl 7.png",
			"speech pl 8.png",
			"speech pl 9.png",
			"speech pl 10.png",
			"speech pl 11.png",
			"speech pl 12.png",
			"speech pl 13.png",
			"speech pl 14.png",
			"speech pl 15.png",
			"speech pl 16.png",
			"speech pl 17.png",
			"speech pl 18.png",
			"speech pl 19.png",
			"speech pl 20.png",
			"speech rs.png",
			"speech ug 1.png",
			"speech ug 2.png",
			"textbox0.png",
			"textbox1.png",
			"textbox2.png",
			"textbox3.png",
			"textbox4.png",
			"textbox5.png",
			"textbox6.png",
			"textbox7.png",
			"textbox8.png",
			"textbox9.png",
			"textbox10.png",
			"textbox11.png",
			"textbox12.png",
			"textbox13.png",
			"textbox14.png",
			"textbox15.png",
			"textbox16.png",
			"textbox17.png",
			"textbox18.png",
			"textbox19.png"
		};
		/// <summary>
		/// Folder Directory: Window Skin
		/// </summary>
		public static string[] TextFrames=new string[] {
			//"Graphics/Windowskins/"+MessageConfig.ChoiceSkinName, // Default: choice 1
			"Graphics/Windowskins/choice 1",
			"Graphics/Windowskins/choice 2",
			"Graphics/Windowskins/choice 3",
			"Graphics/Windowskins/choice 4",
			"Graphics/Windowskins/choice 5",
			"Graphics/Windowskins/choice 6",
			"Graphics/Windowskins/choice 7",
			"Graphics/Windowskins/choice 8",
			"Graphics/Windowskins/choice 9",
			"Graphics/Windowskins/choice 10",
			"Graphics/Windowskins/choice 11",
			"Graphics/Windowskins/choice 12",
			"Graphics/Windowskins/choice 13",
			"Graphics/Windowskins/choice 14",
			"Graphics/Windowskins/choice 15",
			"Graphics/Windowskins/choice 16",
			"Graphics/Windowskins/choice 17",
			"Graphics/Windowskins/choice 18",
			"Graphics/Windowskins/choice 19",
			"Graphics/Windowskins/choice 20",
			"Graphics/Windowskins/choice 21",
			"Graphics/Windowskins/choice 22",
			"Graphics/Windowskins/choice 23",
			"Graphics/Windowskins/choice 24",
			"Graphics/Windowskins/choice 25",
			"Graphics/Windowskins/choice 26",
			"Graphics/Windowskins/choice 27",
			"Graphics/Windowskins/choice 28",
			"skin0.png",
			"skin1.png",
			"skin2.png",
			"skin3.png",
			"skin4.png",
			"skin5.png",
			"skin6.png",
			"skin7.png",
			"skin8.png",
			"skin9.png",
			"skin10.png",
			"skin11.png",
			"skin12.png",
			"skin13.png",
			"skin14.png",
			"skin15.png",
			"skin16.png",
			"skin17.png",
			"skin18.png",
			"skin19.png",
			"skin20.png",
			"skin21.png",
			"skin22.png",
			"skin23.png",
			"skin24.png",
			"skin25.png",
			"skin26.png",
			"skin27.png",
			"skin28.png",
			"skin29.png",
			"skin30.png",
			"Windowskin.PNG"
		};

		public static string[][] VersionStyles=new string[][] {
			//new string[] { MessageConfig.FontName }, // Default font style - Power Green/"Pokemon Emerald"
			new string[] { "Power Green" },
			new string[] { "Power Red and Blue" },
			new string[] { "Power Red and Green" },
			new string[] { "Power Clear" }
		};

		public int pbSettingToTextSpeed(int speed) {
			if (speed==0) return 2;
			if (speed==1) return 1;
			if (speed==2) return -2;
			//if (MessageConfig.TextSpeed != null) return MessageConfig.TextSpeed;
			//return ((Graphics.frame_rate>40) ? -2 : 1);
			return 1;
		}
	}

/*public static partial class MessageConfig {
  public static string pbDefaultSystemFrame { get {
	if (Game.GameData.PokemonSystem == null) {
	  return pbResolveBitmap("Graphics/Windowskins/"+MessageConfig.ChoiceSkinName)??"";
	} else {
	  return pbResolveBitmap(TextFrames[Game.GameData.PokemonSystem.frame])??"";
	}
  } }

  public static string pbDefaultSpeechFrame { get {
	if (Game.GameData.PokemonSystem == null) {
	  return pbResolveBitmap("Graphics/Windowskins/"+MessageConfig.TextSkinName)??"";
	} else {
	  return pbResolveBitmap("Graphics/Windowskins/"+SpeechFrames[Game.GameData.PokemonSystem.textskin])??"";
	}
  } }

  public static string pbDefaultSystemFontName { get {
	if (Game.GameData.PokemonSystem == null) {
	  return MessageConfig.pbTryFonts(MessageConfig.FontName,"Arial Narrow","Arial");
	} else {
	  return MessageConfig.pbTryFonts(VersionStyles[Game.GameData.PokemonSystem.font][0],"Arial Narrow","Arial");
	}
  } }

  public static int? pbDefaultTextSpeed { get {
	return pbSettingToTextSpeed(Game.GameData.PokemonSystem != null ? Game.GameData.PokemonSystem.textspeed : null);
  } }

  public static int pbGetSystemTextSpeed { get {
	return Game.GameData.PokemonSystem != null ? Game.GameData.PokemonSystem.textspeed : ((Graphics.frame_rate>40) ? 2 :  3);
  } }
}*/

	public partial class PokemonSystem : PokemonEssentials.Interface.Screen.IPokemonSystemOption {
		/// <summary>
		/// Text speed (0=slow, 1=normal, 2=fast)
		/// </summary>
		public int textspeed				{ get; set; }
		/// <summary>
		/// Battle effects (animations) (0=on, 1=off)
		/// </summary>
		public int battlescene				{ get; set; }
		/// <summary>
		/// Battle style (0=switch, 1=set)
		/// </summary>
		public int battlestyle				{ get; set; }
		/// <summary>
		/// Default window frame (see also <seealso cref="TextFrames"/>)
		/// </summary>
		public int frame				    { get; set; }
		/// <summary>
		/// Speech frame
		/// </summary>
		public int textskin					{ get; set; }
		/// <summary>
		/// Font (see also <seealso cref="VersionStyles"/>)
		/// </summary>
		public int font						{ get; set; }
		/// <summary>
		/// 0=half size, 1=full size, 2=double size
		/// </summary>
		public int screensize				{ get; set; }
		/// <summary>
		/// Language (see also LANGUAGES in script PokemonSystem)
		/// </summary>
		public int language					{ get; set; }
		/// <summary>
		/// Screen border (0=off, 1=on)
		/// </summary>
		public int border				    { get; set; }
		/// <summary>
		/// Run key functionality (0=hold to run, 1=toggle auto-run)
		/// </summary>
		public int runstyle					{ get; set; }
		/// <summary>
		/// Volume of background music and ME
		/// </summary>
		public int bgmvolume				{ get; set; }
		/// <summary>
		/// Volume of sound effects
		/// </summary>
		public int sevolume					{ get; set; }

		//public void language() {
		//  return (!@language) ? 0 : @language;
		//}
		//
		//public void textskin() {
		//  return (!@textskin) ? 0 : @textskin;
		//}
		//
		//public void border() {
		//  return (!@border) ? 0 : @border;
		//}
		//
		//public void runstyle() {
		//  return (!@runstyle) ? 0 : @runstyle;
		//}
		//
		//public void bgmvolume() {
		//  return (!@bgmvolume) ? 100 : @bgmvolume;
		//}
		//
		//public void sevolume() {
		//  return (!@sevolume) ? 100 : @sevolume;
		//}

		public int tilemap { get { return Core.MAPVIEWMODE; } }

		public PokemonSystem() { initialize(); }
		public PokemonEssentials.Interface.Screen.IPokemonSystemOption initialize() {
			@textspeed   = 1;   // Text speed (0=slow, 1=normal, 2=fast)
			@battlescene = 0;   // Battle effects (animations) (0=on, 1=off)
			@battlestyle = 0;   // Battle style (0=switch, 1=set)
			@frame       = 0;   // Default window frame (see also $TextFrames)
			@textskin    = 0;   // Speech frame
			@font        = 0;   // Font (see also $VersionStyles)
			@screensize  = (int)Math.Floor(Core.DEFAULTSCREENZOOM); // 0=half size, 1=full size, 2=double size
			@border      = 0;   // Screen border (0=off, 1=on)
			@language    = 0;   // Language (see also LANGUAGES in script PokemonSystem)
			@runstyle    = 0;   // Run key functionality (0=hold to run, 1=toggle auto-run)
			@bgmvolume   = 100; // Volume of background music and ME
			@sevolume    = 100; // Volume of sound effects
			return this;
		}
	}
}
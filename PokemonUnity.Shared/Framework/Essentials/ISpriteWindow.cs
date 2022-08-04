using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface
{
	public interface IMessageConfig {
		string FontName			{ get; } //= "Power Green";
		//  in Graphics/Windowskins/ (specify empty string to use the default windowskin)
		string TextSkinName		{ get; } //= "speech hgss 1";
		string ChoiceSkinName	{ get; } //= "choice 1";
		int WindowOpacity		{ get; } //= 255;
		int? TextSpeed			{ get; } //= null; // can be positive to wait frames or negative to
		//  show multiple characters in a single frame
		IColor LIGHTTEXTBASE	{ get; } //= new Color(248,248,248);
		IColor LIGHTTEXTSHADOW	{ get; } //= new Color(72,80,88);
		IColor DARKTEXTBASE		{ get; } //= new Color(88,88,80);
		IColor DARKTEXTSHADOW	{ get; } //= new Color(168,184,184);
		//  0 = Pause cursor is displayed at end of text
		//  1 = Pause cursor is displayed at bottom right
		//  2 = Pause cursor is displayed at lower middle side
		int CURSORMODE				{ get; } //= 1;
		string[] FontSubstitutes	{ get; } //= {
		//											 "Power Red and Blue"=>"Pokemon RS",
		//											 "Power Red and Green"=>"Pokemon FireLeaf",
		//											 "Power Green"=>"Pokemon Emerald",
		//											 "Power Green Narrow"=>"Pokemon Emerald Narrow",
		//											 "Power Green Small"=>"Pokemon Emerald Small",
		//											 "Power Clear"=>"Pokemon DP"
		//										}
		//systemFrame     = null;
		//defaultTextSkin = null;
		//systemFont      = null;
		//textSpeed       = null;

		//string pbTryFonts(*args);

		int pbDefaultTextSpeed();

		string pbDefaultSystemFrame();

		string pbDefaultSpeechFrame();

		string pbDefaultSystemFontName();

		string pbDefaultWindowskin();

		string pbGetSpeechFrame();

		int pbGetTextSpeed();

		string pbGetSystemFontName();

		string pbGetSystemFrame();

		void pbSetSystemFrame(string value);

		void pbSetSpeechFrame(string value);

		void pbSetSystemFontName(string value);

		void pbSetTextSpeed(int value);
	}

	// ############################
	// ############################

	public interface IGameSpriteWindow {
		// Works around a problem with FileTest.directory if directory contains accent marks
		bool safeIsDirectory(string f);

		// Works around a problem with FileTest.exist if path contains accent marks
		bool safeExists(string f);

		// Similar to "Dir.glob", but designed to work around a problem with accessing
		// files if a path contains accent marks.
		// "dir" is the directory path, "wildcard" is the filename pattern to match.
		string[] safeGlob(string dir, string wildcard);



		IBitmap pbGetTileBitmap(string filename, int tile_id, int hue);

		IBitmap[] pbGetAnimation(string name, int hue = 0);

		IBitmap[] pbGetTileset(string name, int hue = 0);

		IBitmap[] pbGetAutotile(string name, int hue = 0);


		// ########################


		void pbDrawShadow(IBitmap bitmap, float x, float y, int width, int height, string text);

		void pbDrawShadowText(IBitmap bitmap, float x, float y, int width, int height, string text, IColor baseColor, IColor shadowColor = null, int align = 0);

		void pbDrawOutlineText(IBitmap bitmap, float x, float y, int width, int height, string text, IColor baseColor, IColor shadowColor = null, int align = 0);

		//void pbCopyBitmap(dstbm,srcbm,float x,float y,int opacity=255) {
		//	rc=new Rect(0,0,srcbm.width,srcbm.height);
		//	dstbm.blt(x,y,srcbm,rc,opacity);
		//}

		//void using(window)
		//IDisposable (IWindow window, Action action)

		void pbBottomRight(IWindow window);

		void pbBottomLeft(IWindow window);

		void pbBottomLeftLines(IWindow window, int lines, int? width = null);

		bool pbDisposed(IViewport x);

		bool isDarkBackground(int background, IRect rect = null);

		bool isDarkWindowskin(int windowskin);

		IColor getDefaultTextColors(int windowskin);

		IBitmap pbDoEnsureBitmap(IBitmap bitmap, int dwidth, int dheight);

		void pbUpdateSpriteHash(IWindow[] windows);

		/// <summary>
		/// Disposes all objects in the specified hash.
		/// </summary>
		/// <param name="sprites"></param>
		void pbDisposeSpriteHash(ISprite[] sprites);

		/// <summary>
		/// Disposes the specified graphics object within the specified hash. Basically like:
		///   sprites[id].dispose
		/// </summary>
		/// <param name="sprites"></param>
		/// <param name="id"></param>
		void pbDisposeSprite(ISprite[] sprites, int id);

		// Draws text on a bitmap. _textpos_ is an array
		// of text commands. Each text command is an array
		// that contains the following:
		//  0 - Text to draw
		//  1 - X coordinate
		//  2 - Y coordinate
		//  3 - If true or 1, the text is right aligned. If 2, the text is centered.
		//      Otherwise, the text is left aligned.
		//  4 - Base color
		//  5 - Shadow color
		void pbDrawTextPositions(IBitmap bitmap, IList<ITextPosition> textpos);

		void pbDrawImagePositions(IBitmap bitmap, IList<ITextPosition> textpos);



		void pbPushFade();

		void pbPopFade();

		bool pbIsFaded { get; }

		// pbFadeOutIn(z) { block }
		/// <summary>
		/// Fades out the screen before a <paramref name="block"/> is run
		/// and fades it back in after the <paramref name="block"/> exits.
		/// </summary>
		/// <param name="z">z indicates the z-coordinate of the viewport used for this effect</param>
		/// <param name="nofadeout"></param>
		/// <param name="block"></param>
		void pbFadeOutIn(int z, bool nofadeout = false, Action block = null);

		void pbFadeOutAndHide(ISprite[] sprites);

		void pbFadeInAndShow(ISprite[] sprites, IList<ISprite> visiblesprites = null, Action block = null);

		// Restores which windows are active for the given sprite hash.
		// _activeStatuses_ is the result of a previous call to pbActivateWindows
		void pbRestoreActivations(ISprite[] sprites, object activeStatuses);

		// Deactivates all windows. If a code block is given, deactivates all windows,
		// runs the code in the block, and reactivates them.
		void pbDeactivateWindows(IWindow[] sprites, Action action = null);

		// Activates a specific window of a sprite hash. _key_ is the key of the window
		// in the sprite hash. If a code block is given, deactivates all windows except
		// the specified window, runs the code in the block, and reactivates them.
		void pbActivateWindow(IWindow[] sprites, int key, Action action = null);

		IColor pbAlphaBlend(IColor dstColor, IColor srcColor);

		IColor pbSrcOver(IColor dstColor, IColor srcColor);

		void pbSetSpritesToColor(ISprite[] sprites, IColor color);

		string pbTryString(string x);

		// Finds the real path for an image file.  This includes paths in encrypted
		// archives.  Returns _x_ if the path can't be found.
		string pbBitmapName(string x);

		/// <summary>
		/// Finds the real path for an image file. This includes paths in encrypted archives.  
		/// </summary>
		/// <param name="x"></param>
		/// <returns>Returns null if the path can't be found.</returns>
		string pbResolveBitmap(string x);

		// Adds a background to the sprite hash.
		// _planename_ is the hash key of the background.
		// _background_ is a filename within the Graphics/Pictures/ folder and can be
		//     an animated image.
		// _viewport_ is a viewport to place the background in.
		void addBackgroundPlane(ISprite[] sprites, string planename, string background, IViewport viewport = null);

		// Adds a background to the sprite hash.
		// _planename_ is the hash key of the background.
		// _background_ is a filename within the Graphics/Pictures/ folder and can be
		//       an animated image.
		// _color_ is the color to use if the background can't be found.
		// _viewport_ is a viewport to place the background in.
		void addBackgroundOrColoredPlane(ISprite[] sprites, string planename, string background, IColor color, IViewport viewport = null);

		/// <summary>
		/// Sets a bitmap's font to the system font.
		/// </summary>
		/// <param name="bitmap"></param>
		void pbSetSystemFont(IBitmap bitmap);

		/// <summary>
		/// Gets the name of the system small font.
		/// </summary>
		/// <returns></returns>
		string pbSmallFontName();

		/// <summary>
		/// Gets the name of the system narrow font.
		/// </summary>
		/// <returns></returns>
		string pbNarrowFontName();

		/// <summary>
		/// Sets a bitmap's font to the system small font.
		/// </summary>
		/// <param name="bitmap"></param>
		void pbSetSmallFont(IBitmap bitmap);

		/// <summary>
		/// Sets a bitmap's font to the system narrow font.
		/// </summary>
		/// <param name="bitmap"></param>
		void pbSetNarrowFont(IBitmap bitmap);

		// Used to determine whether a data file exists (rather than a graphics or
		// audio file). Doesn't check RTP, but does check encrypted archives.
		bool pbRgssExists(string filename);

		/// <summary>
		/// Opens an IO, even if the file is in an encrypted archive.
		/// Doesn't check RTP for the file.
		/// </summary>
		/// <param name="file">path to file</param>
		/// <param name="mode"></param>
		/// <param name="action">logic to perform on file</param>
		/// <returns></returns>
		IDisposable pbRgssOpen(string file, int? mode = null, Action action = null);

		/// <summary>
		/// Gets at least the first byte of a file. 
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		/// Doesn't check RTP, but does check encrypted archives.
		string pbGetFileChar(string file);

		/// <summary>
		/// Gets the contents of a file. 
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		/// Doesn't check RTP, but does check encrypted archives.
		string pbGetFileString(string file);
	}

// ############################
// ############################


//public interface IGifLibrary {
// // @@loadlib=new Win32API("Kernel32.dll","LoadLibrary",'p','');
// // if (safeExists("gif.dll")) {
//	//PngDll=@@loadlib.call("gif.dll");
//	//GifToPngFiles=new Win32API("gif.dll","GifToPngFiles",'pp','l');
//	//GifToPngFilesInMemory=new Win32API("gif.dll","GifToPngFilesInMemory",'plp','l');
//	//CopyDataString=new Win32API("gif.dll","CopyDataString",'lpl','l');
//	//FreeDataString=new Win32API("gif.dll","FreeDataString",'l','');
// // } else {
//	//PngDll=null;
// // }

//  void getDataFromResult(result) {
//	datasize=CopyDataString.call(result,"",0);
//	ret=null;
//	if (datasize!=0) {
//	  data="0"*datasize;
//	  CopyDataString.call(result,data,datasize);
//	  ret=data.unpack("V*");
//	}
//	FreeDataString.call(result);
//	return ret;
//  }
//}


// ############################
// ############################

	public interface IAnimatedBitmap : IBitmap, IDisposable {
		IAnimatedBitmap initialize(string file, int hue= 0); //{
		//	if (file==null) raise "filename is null";
		//	if (file[/^\[(\d+)\]/]  ) {		// Starts with 1 or more digits in brackets
		//	  @bitmap=new PngAnimatedBitmap(file,hue);
		//	} else {
		//	  @bitmap=new GifBitmap(file,hue);
		//	}
		//}

		IBitmap this[int index] { get; } //{ @bitmap[index]; }
		int width(); //{ @bitmap.bitmap.width; }
		int height(); //{ @bitmap.bitmap.height; }
		int length(); //{ @bitmap.Length; }
		IEnumerable<IBitmap> each(); //{ @bitmap.each {|item| yield item }; }
		IBitmap bitmap(); //{ @bitmap.bitmap; }
		int currentIndex(); //{ @bitmap.currentIndex; }
		int frameDelay(); //{ @bitmap.frameDelay; }
		int totalFrames(); //{ @bitmap.totalFrames; }
		bool disposed { get; } //{ @bitmap.disposed(); }
		void update(); //{ @bitmap.update(); }
		//void dispose(); //{ @bitmap.dispose(); }
		IBitmap deanimate(); //{ @bitmap.deanimate; }
		IAnimatedBitmap copy(); //{ @bitmap.copy; }
	}

//// ########################
//// 
//// Message support
//// 
//// ########################
//if !defined(_INTL);
//  void _INTL(*args) { 
//	string=args[0].clone();
//	for (int i = 1; i < args.Length; i++) {
//	  string.gsub!(/\{#{i}\}/,"#{args[i]}");
//	}
//	return string;
//  }
//}

//if !defined(_ISPRINTF);
//  void string.Format(*args) {
//	string=args[0].clone();
//	for (int i = 1; i < args.Length; i++) {
//	  string.gsub!(/\{#{i}\:([^\}]+?)\}/){|m|
//		 next string.Format("%"+$1,args[i]);
//	  }
//	}
//	return string;
//  }
//}

//if !defined(_MAPINTL);
//  void _MAPINTL(*args) {
//	string=args[1].clone();
//	for (int i = 2; i < args.Length; i++) {
//	  string.gsub!(/\{#{i}\}/,"#{args[i+1]}");
//	}
//	return string;
//  }
//}

/*public interface IGraphics {
  if (!this.respond_to("width")) {
	void width; return 640; }() {
  }
  if (!this.respond_to("height")) {
	void height; return 480; }() {
  }
}*/

// ############################
// ############################


//public interface IMiniRegistry {
//  HKEY_CLASSES_ROOT = 0x80000000;
//  HKEY_CURRENT_USER = 0x80000001;
//  HKEY_LOCAL_MACHINE = 0x80000002;
//  HKEY_USERS = 0x80000003;
//  FormatMessageA=new Win32API("kernel32","FormatMessageA","LPLLPLP","L")
//  RegOpenKeyExA=new Win32API("advapi32","RegOpenKeyExA","LPLLP","L");
//  RegCloseKey=new Win32API("advapi32","RegCloseKey","L","L");
//  RegQueryValueExA=new Win32API("advapi32","RegQueryValueExA","LPLPPP","L");

//  void open(hkey,subkey,bit64=false) {
//	key=0.chr*4;
//	flag=bit64 ? 0x20119 : 0x20019;
//	rg=RegOpenKeyExA.call(hkey, subkey, 0, flag, key);
//	if (rg!=0) {
//	  return null;
//	}
//	key=key.unpack("V")[0];
//	if (block_given?) {
//	  begin;
//		yield(key);
//	  ensure;
//		check(RegCloseKey.call(key));
//	  }
//	} else {
//	  return key;
//	}
//  }

//  void close(hkey) { if (hkey) check(RegCloseKey.call(hkey)); }

//  void get(hkey,subkey,name,defaultValue=null,bit64=false) {
//	this.open(hkey,subkey,bit64){|key| 
//	   return this.read(key,name) rescue defaultValue;
//	}
//	return defaultValue;
//  }

//  void read(hkey,name) {
//	if (!hkey) hkey=0;
//	type=0.chr*4; size=0.chr*4;
//	check(RegQueryValueExA.call(hkey,name,0,type,0,size));
//	data=" "*size.unpack("V")[0];
//	check(RegQueryValueExA.call(hkey,name,0,type,data,size));
//	type=type.unpack("V")[0];
//	data=data[0,size.unpack("V")[0]];
//	switch (type) {
//	  break;
//	case 1:
//	  return data.chop; // REG_SZ
//	  break;
//	case 2:
//	  return data.gsub(/%([^%]+)%/) { ENV[$1] || $& } // REG_EXPAND_SZ
//	  break;
//	case 3:
//	  return data; // REG_BINARY
//	  break;
//	case 4:
//	  return data.unpack("V")[0]; // REG_DWORD
//	  break;
//	case 5:
//	  return data.unpack("V")[0]; // REG_DWORD_BIG_ENDIAN
//	  break;
//	case 11:
//	  qw=data.unpack("VV"); return (data[1]<<32|data[0]); // REG_QWORD
//	else; raise "Type #{type} not supported.";
//	}
//  }
//}



//void getUnicodeStringFromAnsi(addr) {
//  if (addr==0) return "";
//  rtlMoveMemory_pi = new Win32API('kernel32', 'RtlMoveMemory', 'pii', 'i');
//  ret="";
//  data="x";
//  index=(addr is String) ? 0 : addr;
//  do { //;loop
//	if (addr is String) {
//	  data=addr[index,1];
//	} else {
//	  rtlMoveMemory_pi.call(data, index, 1);
//	}
//	index+=1;
//	codepoint=data.unpack("C")[0];
//	if (codepoint==0 || !codepoint) break;
//	if (codepoint==0) break;
//	if (codepoint<=0x7F) {
//	  ret+=codepoint.chr;
//	} else {
//	  ret+=(0xC0|((codepoint>>6)&0x1F)).chr;
//	  ret+=(0x80|(codepoint   &0x3F)).chr;
//	}
//  }
//  return ret;
//}

//void getUnicodeString(addr) {
//  if (addr==0) return "";
//  rtlMoveMemory_pi = new Win32API('kernel32', 'RtlMoveMemory', 'pii', 'i');
//  ret="";
//  data="xx";
//  index=(addr is String) ? 0 : addr;
//  do { //;loop
//	if (addr is String) {
//	  data=addr[index,2];
//	} else {
//	  rtlMoveMemory_pi.call(data, index, 2);
//	}
//	codepoint=data.unpack("v")[0];
//	if (codepoint==0) break;
//	index+=2;
//	if (codepoint<=0x7F) {
//	  ret+=codepoint.chr;
//	} else if (codepoint<=0x7FF) {
//	  ret+=(0xC0|((codepoint>>6)&0x1F)).chr;
//	  ret+=(0x80|(codepoint   &0x3F)).chr;
//	} else if (codepoint<=0xFFFF) {
//	  ret+=(0xE0|((codepoint>>12)&0x0F)).chr;
//	  ret+=(0x80|((codepoint>>6)&0x3F)).chr;
//	  ret+=(0x80|(codepoint   &0x3F)).chr;
//	} else if (codepoint<=0x10FFFF) {
//	  ret+=(0xF0|((codepoint>>18)&0x07)).chr;
//	  ret+=(0x80|((codepoint>>12)&0x3F)).chr;
//	  ret+=(0x80|((codepoint>>6)&0x3F)).chr;
//	  ret+=(0x80|(codepoint   &0x3F)).chr;
//	}
//  }
//  return ret;
//}

//void getKnownFolder(guid) {
//  packedGuid=guid.pack("VvvC*");
//  shGetKnownFolderPath=new Win32API("shell32.dll","SHGetKnownFolderPath","pllp","i") rescue null;
//  coTaskMemFree=new Win32API("ole32.dll","CoTaskMemFree","i","") rescue null;
//  if (shGetKnownFolderPath && coTaskMemFree) {
//	path="\0"*4;
//	ret=shGetKnownFolderPath.call(packedGuid,0,0,path);
//	path=path.unpack("V")[0];
//	ret=getUnicodeString(path);
//	coTaskMemFree.call(path);
//	return ret;
//  }
//  return "";
//}



//public interface IRTP {
//  @rtpPaths=null;

//  void exists(filename,extensions=[]) {
//	if (!filename || filename=="") return false;
//	eachPathFor(filename) {|path|
//	   if (safeExists(path)) return true;
//	   foreach (var ext in extensions) {
//		 if (safeExists(path+ext)) return true;
//	   }
//	}
//	return false;
//  }

//  void getImagePath(filename) {
//	return this.getPath(filename,["",".png",".jpg",".gif",".bmp",".jpeg"]);
//  }

//  void getAudioPath(filename) {
//	return this.getPath(filename,["",".mp3",".wav",".wma",".mid",".ogg",".midi"]);
//  }

//  void getPath(filename,extensions=[]) {
//	if (!filename || filename=="") return filename;
//	eachPathFor(filename) {|path|
//	   if (safeExists(path)) return path;
//	   foreach (var ext in extensions) {
//		 file=path+ext;
//		 if (safeExists(file)) return file;
//	   }
//	}
//	return filename;
//  }

////  Gets the absolute RGSS paths for the given file name
//  void eachPathFor(filename) {
//	if (!filename) return;
//	if (filename[/^[A-Za-z]\:[\/\\]/] || filename[/^[\/\\]/] ) {
////  filename is already absolute
//	  yield filename;
//	} else {
////  relative path
//	  RTP.eachPath {|path| 
//		 if (path=="./") {
//		   yield filename;
//		 } else {
//		   yield path+filename;
//		 }
//	  }
//	}
//  }

////  Gets all RGSS search paths
//  void eachPath() {
////  XXX: Use "." instead of Dir.pwd because of problems retrieving files if
////  the current directory contains an accent mark
//	yield ".".gsub(/[\/\\]/,"/").gsub(/[\/\\]$/,"")+"/";
//	if (!@rtpPaths) {
//	  tmp=new Sprite();
//	  isRgss2=tmp.respond_to("wave_amp");
//	  tmp.dispose();
//	  @rtpPaths=[];
//	  if (isRgss2) {
//		rtp=getGameIniValue("Game","RTP");
//		if (rtp!="") {
//		  rtp=MiniRegistry.get(MiniRegistry.yKEY_LOCAL_MACHINE,
//			 "SOFTWARE\\Enterbrain\\RGSS2\\RTP",rtp,null);
//		  if (rtp && safeIsDirectory(rtp)) {
//			@rtpPaths.Add(rtp.sub(/[\/\\]$/,"")+"/");
//		  }
//		}
//	  } else {
//		%w( RTP1 RTP2 RTP3 ).each{|v|
//		   rtp=getGameIniValue("Game",v);
//		   if (rtp!="") {
//			 rtp=MiniRegistry.get(MiniRegistry.yKEY_LOCAL_MACHINE,
//				"SOFTWARE\\Enterbrain\\RGSS\\RTP",rtp,null);
//			 if (rtp && safeIsDirectory(rtp)) {
//			   @rtpPaths.Add(rtp.sub(/[\/\\]$/,"")+"/");
//			 }
//		   }
//		}
//	  }
//	}
//	foreach (var x in @rtpPaths) { yield return x; }
//  }
//}

	public interface IFileTest {
		string[] Image_ext { get; } //= ['.bmp', '.png', '.jpg', '.jpeg', '.gif'];
		string[] Audio_ext { get; } //= ['.mp3', '.mid', '.midi', '.ogg', '.wav', '.wma'];

		bool audio_exist(string filename);

		string image_exist(string filename);
	}

// ##########

	public interface IPngAnimatedBitmap { // :nodoc:
		//  Creates an animated bitmap from a PNG file.  
		IPngAnimatedBitmap initialize(string file, int hue = 0);

		IBitmap this[int index] { get; } //return @frames[index];

		int width { get; } //() { this.bitmap.width; }

		int height { get; } //() { this.bitmap.height; }

		IBitmap deanimate();

		IBitmap bitmap { get; }

		int currentIndex { get; }

		int frameDelay(int index);

		int length { get; }

		IEnumerable<IBitmap> each();

		int totalFrames { get; }

		bool disposed { get; }

		void update();

		void dispose();

		int frames				{ get; } // internal

		IPngAnimatedBitmap copy();
	}

// internal class
/*public interface IGifBitmap {
//  Creates a bitmap from a GIF file with the specified
//  optional viewport.  Can also load non-animated bitmaps.
  void initialize(file,hue=0) {
	@gifbitmaps=[];
	@gifdelays=[];
	@totalframes=0;
	@framecount=0;
	@currentIndex=0;
	@disposed=false;
	bitmap=null;
	filestring=null;
	filestrName=null;
	if (!file) file="";
	file=canonicalize(file);
	begin;
	  bitmap=BitmapCache.load_bitmap(file,hue);
	rescue;
	  bitmap=null;
	}
	if (!bitmap || (bitmap.width==32 && bitmap.height==32)) {
	  if (!file || file.Length<1 || file[file.Length-1]!=0x2F) {
		if ((filestring=pbGetFileChar(file))) {
		  filestrName=file;
		} else if ((filestring=pbGetFileChar(file+".gif"))) {
		  filestrName=file+".gif";
		} else if ((filestring=pbGetFileChar(file+".png"))) {
		  filestrName=file+".png";
		} else if ((filestring=pbGetFileChar(file+".jpg"))) {
		  filestrName=file+".jpg";
		} else if ((filestring=pbGetFileChar(file+".bmp"))) {
		  filestrName=file+".bmp";
		}
	  }
	}
	if (bitmap && filestring && filestring[0]==0x47 &&
	   bitmap.width==32 && bitmap.height==32) {
// File.open("debug.txt","ab"){|f| f.puts("rejecting bitmap") }
	  bitmap.dispose();
	  bitmap=null;
	}
	if (bitmap) {
// File.open("debug.txt","ab"){|f| f.puts("reusing bitmap") }
//  Have a regular non-animated bitmap
	  @totalframes=1;
	  @framecount=0;
	  @gifbitmaps= new []{ bitmap };
	  @gifdelays= new []{ 1 };
	} else {
	  tmpBase=File.basename(file)+"_tmp_";
	  if (filestring) filestring=pbGetFileString(filestrName);
	  Dir.chdir(ENV["TEMP"]){ // navigate to temp folder since game might be on a CD-ROM
		 if (filestring && filestring[0]==0x47 && GifLibrary.yngDll) {
		   result=GifLibrary.yifToPngFilesInMemory.call(filestring,
			  filestring.Length,tmpBase);
		 } else {
		   result=0;
		 }
		 if (result>0) {
		   @gifdelays=GifLibrary.getDataFromResult(result);
		   @totalframes=@gifdelays.pop;
		   for (int i = 0; i < @gifdelays.Length; i++) {
			 @gifdelays[i]=[@gifdelays[i],1].max;
			 bmfile=string.Format("%s%d.png",tmpBase,i);
			 if (safeExists(bmfile)) {
			   gifbitmap=new BitmapWrapper(bmfile);
			   @gifbitmaps.Add(gifbitmap);
			   if (hue!=0) bmfile.hue_change(hue);
			   if (hue==0 && @gifdelays.Length==1) {
				 BitmapCache.setKey(file,gifbitmap);
			   }
			   File.delete(bmfile);
			 } else {
			   @gifbitmaps.Add(new BitmapWrapper(32,32));
			 }
		   }
		 }
	  }
	  if (@gifbitmaps.Length==0) {
		@gifbitmaps= new []{ new BitmapWrapper(32,32) };
		@gifdelays= new []{ 1 };
	  }
	  if (@gifbitmaps.Length==1) {
		BitmapCache.setKey(file,@gifbitmaps[0]);
	  }
	}
  }

  void [](index) {
	return @gifbitmaps[index];
  }

  void width() { this.bitmap.width; }

  void height() { this.bitmap.height; }

  void deanimate() {
	for (int i = 1; i < @gifbitmaps.Length; i++) {
	  @gifbitmaps[i].dispose();
	}
	@gifbitmaps=[@gifbitmaps[0]];
	@currentIndex=0;
	return @gifbitmaps[0];
  }

  void bitmap() {
	@gifbitmaps[@currentIndex];
  }

  void currentIndex() {
	@currentIndex;
  }

  void frameDelay(index) {
	return @gifdelay[index]/2; // Due to frame count being incremented by 2
  }

  void length() {
	@gifbitmaps.Length;
  }

  void each() {
	@gifbitmaps.each {|item| yield item }
  }

  void totalFrames() {
	@totalframes/2; // Due to frame count being incremented by 2
  }

  bool disposed() {
	@disposed;
  }

  void width() {
	@gifbitmaps.Length==0 ? 0 : @gifbitmaps[0].width;
  }

  void height() {
	@gifbitmaps.Length==0 ? 0 : @gifbitmaps[0].height;
  }

//  This function must be called in order to animate the GIF image.
  void update() {
	if (disposed?) return;
	if (@gifbitmaps.Length>0) {
	  @framecount+=2;
	  @framecount=@totalframes<=0 ? 0 : @framecount%@totalframes;
	  frametoshow=0;
	  for (int i = 0; i < @gifdelays.Length; i++) {
		if (@gifdelays[i]<=@framecount) frametoshow=i;
	  }
	  @currentIndex=frametoshow;
	}
  }

  void dispose() {
	if (!@disposed) {
	  foreach (var i in @gifbitmaps) {
		i.dispose();
	  }
	}
	@disposed=true;
  }

  //int gifbitmaps			{ get; set; } // internal
  //int gifdelays				{ get; set; } // internal

  void copy() {
	x=this.clone();
	x.gifbitmaps=x.gifbitmaps.clone();
	x.gifdelays=x.gifdelays.clone();
	for (int i = 0; i < x.gifbitmaps.Length; i++) {
	  x.gifbitmaps[i]=x.gifbitmaps[i].copy;
	}
	return x;
  }
}*/

	public interface IGameTempSpriteWindow {
		//private int? _fadestate				{ get; protected set; }

		int fadestate { get; }
	}

	// ###############################################################################
	// SpriteWrapper is a class based on Sprite which wraps Sprite's properties.
	// ###############################################################################
	public interface ISpriteWrapper : ISprite, IDisposable {
		float angle { get; set; }
		IBitmap bitmap { get; set; }
		int blend_type { get; set; }
		int bush_depth { get; set; }
		IColor color { get; set; }
		bool disposed { get; }
		bool mirror { get; set; }
		float opacity { get; set; }
		float ox { get; set; }
		float oy { get; set; }
		IRect src_rect { get; set; }
		ITone tone { get; set; }
		IViewport viewport { get; set; }
		bool visible { get; set; }
		float x { get; set; }
		float y { get; set; }
		float z { get; set; }
		float zoom_x { get; set; }
		float zoom_y { get; set; }

		ISpriteWrapper initialize(IViewport viewport= null);
		void Dispose();
		void flash(IColor color, int duration);
		//void update();
	}

// ########################################################################

	public interface IStringInput //: IEnumerable 
	{
		//include Enumerable;

		// class << self
		//	void new( str ) {
		//	  if (block_given?) {
		//		begin;
		//		  f = super;
		//		  yield f;
		//		ensure;
		//		  if (f) f.close;
		//		}
		//	  } else {
		//		super;
		//	  }
		//	}
		//	alias open new;
		// }

		IStringInput initialize(string str);

		int lineno				{ get; }
		int @string				{ get; }

		string inspect();

		void close();

		bool closed();

		int pos { get; set; }
		//int pos=(value) { seek(value); }

		//alias tell pos;

		void rewind();


		int seek(int offset, int whence=0);//IO.SEEK_SET

		bool eof();

		IEnumerable<string> each(ref string[] block);

		string gets();

		char getc();

		string read(int? len = null);

		string read_all();

		//alias sysread read;
	}

	//public interface I::Marshal {
	//  class << self
	//	if (!@oldloadAliased) {
	//	  alias oldload load;
	//	  @oldloadAliased=true;
	//	}

	//	void neverload() {
	//	  return @@neverload;
	//	}

	//	@@neverload=false;

	//	void neverload=(value) {
	//	  @@neverload=value;
	//	}

	//	void load(port,*arg) {
	//	  if (@@neverload) {
	//		if (port is IO) {
	//		  return port.read;
	//		} else {
	//		  return port;
	//		}
	//	  }
	//	  if (port is IO) oldpos=port.pos;
	//	  begin;
	//		oldload(port,*arg);
	//	  rescue;
	//		p [$!.class,$!.message,$!.backtrace];
	//		if (port is IO) {
	//		  port.pos=oldpos;
	//		  return port.read;
	//		} else {
	//		  return port;
	//		}
	//	  }
	//	}
	//  }
	//}

	// ===============================================================================
	// SpriteWindow is a class based on Window which emulates Window's functionality.
	// This class is necessary in order to change the viewport of windows (with
	// viewport=) and to make windows fade in and out (with tone=).
	// ===============================================================================
	public interface ISpriteWindowCursorRect : IRect {
		ISpriteWindowCursorRect initialize(IWindow window);

		int x				{ get; set; }
		int y				{ get; set; }
		int width			{ get; set; }
		int height		{ get; set; }

		void empty();

		bool isEmpty();

		void set(float x,float y,float width,float height);
	}

	public interface ISpriteWindow : IWindow {
		ITone tone				{ get; set; }
		//IColor color				{ get; set; }
		//IViewport viewport		{ get; set; }
		int contents				{ get; set; }
		//int ox					{ get; set; }
		//int oy					{ get; set; }
		//int x						{ get; set; }
		//int y						{ get; set; }
		//int z						{ get; set; }
		int zoom_x				{ get; set; }
		int zoom_y				{ get; set; }
		int offset_x				{ get; set; }
		int offset_y				{ get; set; }
		//int width					{ get; set; }
		//int active				{ get; set; }
		//int pause					{ get; set; }
		//int height				{ get; set; }
		//int opacity				{ get; set; }
		//int back_opacity			{ get; set; }
		//int contents_opacity		{ get; set; }
		//bool visible				{ get; set; }
		int cursor_rect			{ get; set; }
		int contents_blend_type	{ get; set; }
		int blend_type			{ get; set; }
		//int openness				{ get; set; }

		//int windowskin { get; } //();
		//	@_windowskin;
		//}

		//  Flags used to preserve compatibility
		//  with RGSS/RGSS2's version of Window
		//  public interface ICompatBits  {
		//	CorrectZ         = 1;
		//	ExpandBack       = 2;
		//	ShowScrollArrows = 4;
		//	StretchSides     = 8;
		//	ShowPause        = 16;
		//	ShowCursor       = 32;
		//  }

		//  int compat				{ get; protected set; }

		//  void compat=(value) {
		//	@compat=value;
		//	privRefresh(true);
		//  }

		ISpriteWindow initialize(IViewport viewport=null);

		//void dispose();

		//  void stretch=(value) {
		//	@stretch=value;
		//	privRefresh(true);
		//  }

		//  void visible=(value) {
		//	@visible=value;
		//	privRefresh;
		//  }

		//  void viewport=(value) {
		//	@viewport=value;
		//	foreach (var i in @spritekeys) {
		//	  if (@sprites[i]) @sprites[i].dispose();
		//	}
		//	foreach (var i in @spritekeys) {
		//	  if (@sprites[i] is Sprite) {
		//		@sprites[i]=new Sprite(@viewport);
		//	  } else {
		//		@sprites[i]=null;
		//	  }
		//	}
		//	privRefresh(true);
		//  }

		//  void z=(value) {
		//	@z=value;
		//	privRefresh;
		//  }

		//  bool disposed() {
		//	return @disposed;
		//  }

		//  void contents=(value) {
		//	if (@contents!=value) {
		//	  @contents=value;
		//	  if (@visible) privRefresh;
		//	}
		//  }

		//  void ox=(value) {
		//	if (@ox!=value) {
		//	  @ox=value;
		//	  if (@visible) privRefresh;
		//	}
		//  }

		//  void oy=(value) {
		//	if (@oy!=value) {
		//	  @oy=value;
		//	  if (@visible) privRefresh;
		//	}
		//  }

		//  void active=(value) {
		//	 @active=value;
		//	 privRefresh(true);
		//  }

		//  void cursor_rect=(value) {
		//	if (!value) {
		//	  @cursor_rect.empty;
		//	} else {
		//	  @cursor_rect.set(value.x,value.y,value.width,value.height);
		//	}
		//  }

		//  void openness=(value) {
		//	@openness=value;
		//	if (@openness<0) @openness=0;
		//	if (@openness>255) @openness=255;
		//	privRefresh;
		//  }

		//  void width=(value) {
		//	@width=value;
		//	privRefresh(true);
		//  }

		//  void height=(value) {
		//	@height=value;
		//	privRefresh(true);
		//  }

		//  void pause=(value) {
		//	@pause=value;
		//	if (!value) @pauseopacity=0;
		//	if (@visible) privRefresh;
		//  }

		//  void x=(value) {
		//	@x=value;
		//	if (@visible) privRefresh;
		//  }

		//  void y=(value) {
		//	@y=value;
		//	if (@visible) privRefresh;
		//  }

		//  void zoom_x=(value) {
		//	@zoom_x=value;
		//	if (@visible) privRefresh;
		//  }

		//  void zoom_y=(value) {
		//	@zoom_y=value;
		//	if (@visible) privRefresh;
		//  }

		//  void offset_x=(value) {
		//	@x=value;
		//	if (@visible) privRefresh;
		//  }

		//  void offset_y=(value) {
		//	@y=value;
		//	if (@visible) privRefresh;
		//  }

		//  void opacity=(value) {
		//	@opacity=value;
		//	if (@opacity<0) @opacity=0;
		//	if (@opacity>255) @opacity=255;
		//	if (@visible) privRefresh;
		//  }

		//  void back_opacity=(value) {
		//	@back_opacity=value;
		//	if (@back_opacity<0) @back_opacity=0;
		//	if (@back_opacity>255) @back_opacity=255;
		//	if (@visible) privRefresh;
		//  }

		//  void contents_opacity=(value) {
		//	@contents_opacity=value;
		//	if (@contents_opacity<0) @contents_opacity=0;
		//	if (@contents_opacity>255) @contents_opacity=255;
		//	if (@visible) privRefresh;
		//  }

		//  void tone=(value) {
		//	@tone=value;
		//	if (@visible) privRefresh;
		//  }

		//  void color=(value) {
		//	@color=value;
		//	if (@visible) privRefresh;
		//  }

		//  void blend_type=(value) {
		//	@blend_type=value;
		//	if (@visible) privRefresh;
		//  }

		//  void flash(color,duration) {
		//	if (disposed?) return;
		//	@flash=duration+1;
		//	foreach (var i in @sprites) {
		//	  i[1].flash(color,duration);
		//	}
		//  }

		//  void update() {
		//	if (disposed?) return;
		//	mustchange=false;
		//	if (@active) {
		//	  if (@cursorblink==0) {
		//		@cursoropacity-=8;
		//		if (@cursoropacity<=128) @cursorblink=1;
		//	  } else {
		//		@cursoropacity+=8;
		//		if (@cursoropacity>=255) @cursorblink=0;
		//	  }
		//	  privRefreshCursor;
		//	} else {
		//	  @cursoropacity=128;
		//	  privRefreshCursor;
		//	}
		//	if (@pause) {
		//	  oldpauseframe=@pauseframe;
		//	  oldpauseopacity=@pauseopacity;
		//	  @pauseframe=(Graphics.frame_count / 8) % 4;
		//	  @pauseopacity=[@pauseopacity+64,255].min;
		//	  mustchange=@pauseframe!=oldpauseframe || @pauseopacity!=oldpauseopacity;
		//	}
		//	if (mustchange) privRefresh;
		//	if (@flash>0) {
		//	  foreach (var i in @sprites.values) {
		//		i.update();
		//	  }
		//	  @flash-=1;
		//	}
		//  }

		//// ############
		//  int skinformat				{ get; protected set; }
		//  int skinrect				{ get; protected set; }

		//  void loadSkinFile(file) {
		//	if ((this.windowskin.width==80 || this.windowskin.width==96) &&
		//	   this.windowskin.height==48) {
		////  Body = X, Y, width, height of body rectangle within windowskin
		//	  @skinrect.set(32,16,16,16);
		////  Trim = X, Y, width, height of trim rectangle within windowskin
		//	  @trim= new []{ 32,16,16,16 };
		//	} else if (this.windowskin.width==80 && this.windowskin.height==80) {
		//	  @skinrect.set(32,32,16,16);
		//	  @trim= new []{ 32,16,16,48 };
		//	}
		//  }

		//  void windowskin=(value) {
		//	oldSkinWidth=(@_windowskin && !@_windowskin.disposed?) ? @_windowskin.width : -1;
		//	oldSkinHeight=(@_windowskin && !@_windowskin.disposed?) ? @_windowskin.height : -1;
		//	@_windowskin=value;
		//	if (@skinformat==1) {
		//	  @rpgvx=false;
		//	  if (@_windowskin && !@_windowskin.disposed?) {
		//		if (@_windowskin.width!=oldSkinWidth || @_windowskin.height!=oldSkinHeight) {
		////  Update skinrect and trim if windowskin's dimensions have changed
		//		  @skinrect.set((@_windowskin.width-16)/2,(@_windowskin.height-16)/2,16,16);
		//		  @trim=[@skinrect.x,@skinrect.y,@skinrect.x,@skinrect.y];
		//		}
		//	  } else {
		//		@skinrect.set(16,16,16,16);
		//		@trim= new []{ 16,16,16,16 };
		//	  }
		//	} else {
		//	  if (value && value is Bitmap && !value.disposed? && value.width==128) {
		//		@rpgvx=true;
		//	  } else {
		//		@rpgvx=false;
		//	  }
		//	  @trim= new []{ 16,16,16,16 };
		//	}
		//	privRefresh(true);
		//  }

		//  void skinrect=(value) {
		//	@skinrect=value;
		//	privRefresh;
		//  }

		//  void skinformat=(value) {
		//	if (@skinformat!=value) {
		//	  @skinformat=value;
		//	  privRefresh(true);
		//	}
		//  }

		//  void borderX() {
		//	if (!@trim || skinformat==0) return 32;
		//	if (@_windowskin && !@_windowskin.disposed?) {
		//	  return @trim[0]+(@_windowskin.width-@trim[2]-@trim[0]);
		//	}
		//	return 32;
		//  }

		//  void borderY() {
		//	if (!@trim || skinformat==0) return 32;
		//	if (@_windowskin && !@_windowskin.disposed?) {
		//	  return @trim[1]+(@_windowskin.height-@trim[3]-@trim[1]);
		//	}
		//	return 32;
		//  }

		//  void leftEdge() { this.startX; }
		//  void topEdge() { this.startY; }
		//  void rightEdge() { this.borderX-this.leftEdge; }
		//  void bottomEdge() { this.borderY-this.topEdge; }

		//  void startX() {
		//	return !@trim || skinformat==0  ? 16 : @trim[0];
		//  }

		//  void startY() {
		//	return !@trim || skinformat==0  ? 16 : @trim[1];
		//  }

		//  void endX() {
		//	return !@trim || skinformat==0  ? 16 : @trim[2];
		//  }

		//  void endY() {
		//	return !@trim || skinformat==0  ? 16 : @trim[3];
		//  }

		//  void startX=(value) {
		//	@trim[0]=value;
		//	privRefresh;
		//  }

		//  void startY=(value) {
		//	@trim[1]=value;
		//	privRefresh;
		//  }

		//  void endX=(value) {
		//	@trim[2]=value;
		//	privRefresh;
		//  }

		//  void endY=(value) {
		//	@trim[3]=value;
		//	privRefresh;
		//  }
	}

	public interface ISpriteWindow_Base : ISpriteWindow {
		//TEXTPADDING=4; // In pixels

		ISpriteWindow_Base initialize(float x, float y, float width, float height);

		void __setWindowskin(int skin);

		void __resolveSystemFrame();

		// Filename of windowskin to apply. Supports XP, VX, and animated skins.
		void setSkin(int skin);

		void setSystemFrame();

		void update();

		//void dispose();
	}

	public interface ISpriteWindow_Selectable : ISpriteWindow_Base {
		int index				{ get; set; }
		int itemCount			{ get; }
		int count				{ get; }
		int rowHeight			{ get; set; }
		int columns				{ get; set; }
		int columnSpacing		{ get; set; }
		int row_max				{ get; set; }
		int top_row				{ get; set; }
		int top_item			{ get; set; }
		int page_row_max			{ get; set; }
		int page_item_max			{ get; set; }

		ISpriteWindow_Selectable initialize(float x, float y, float width, float height);

		bool ignore_input { set; } 
		//
		//void count() {
		//return @item_max;
		//}
		//
		//void row_max() {
		//return ((@item_max + @column_max - 1) / @column_max).to_i;
		//}
		//
		//void top_row() {
		//return (@virtualOy / (@row_height || 32)).to_i;
		//}
		//
		//void top_item() {
		//return top_row * @column_max;
		//}

		void update_cursor_rect();

		//void top_row=(row) {
		//	if (row>row_max-1) {
		//		row=row_max-1;
		//	}
		//	if (row<0) {		// NOTE: The two comparison checks must be reversed since row_max can be 0
		//		row=0;
		//	}
		//	@virtualOy=row*@row_height;
		//}
		//
		//void page_row_max() {
		//return priv_page_row_max.to_i;
		//}
		//
		//void page_item_max() {
		//return priv_page_item_max.to_i;
		//}

		IRect itemRect(int item);

		void update();

		void refresh();
	}

	public interface IUpDownArrowMixin {
		void initUpDownArrow();

		void dispose();

		IViewport viewport { set; }

		IColor color { set; }
  
		void adjustForZoom(ISprite sprite);

		void update();
	}

	public interface ISpriteWindow_SelectableEx : ISpriteWindow_Selectable, IUpDownArrowMixin
	{
		//include UpDownArrowMixin;

		new IViewport viewport { set; }

		//void initialize(*arg);
	}

	public interface IWindow_DrawableCommand : ISpriteWindow_SelectableEx {
		int baseColor				{ get; set; }
		int shadowColor			{ get; set; }

		float textWidth(IBitmap bitmap, string text);

		void getAutoDims(string[] commands, ref int[] dims, float? width = null);

		void initialize(float x, float y, float width, float height, IViewport viewport = null);

		IRect drawCursor(int index, IRect rect);

		//void dispose();

		// to be implemented by derived classes
		int itemCount();

		// to be implemented by derived classes
		void drawItem(int index, int count, IRect rect);

		void refresh();

		void update();
	}

	public interface IWindow_CommandPokemon : IWindow_DrawableCommand {
		new IColor color				{ get; set; }
		string[] commands				{ get; set; }

		IWindow_CommandPokemon initialize(string[] commands,float? width=null);

		IWindow_CommandPokemon WithSize(string[] commands, float x, float y, float width, float height, IViewport viewport= null);

		IWindow_CommandPokemon Empty(float x, float y, float width, float height, IViewport viewport= null);

		int index { get; set; }

		float width { get; set; }

		float height { get; set; }

		void resizeToFit(string[] commands, float? width = null);

		int itemCount();

		void drawItem(int index, int count, IRect rect);
	}

	public interface IWindow_AdvancedCommandPokemon : IWindow_DrawableCommand {
		IList<string> commands				{ get; set; }

		int textWidth(IBitmap bitmap, string text);

		IWindow_AdvancedCommandPokemon initialize(string[] commands, int? width= null);

		IWindow_AdvancedCommandPokemon WithSize(string[] commands,float x,float y,int width,int height,IViewport viewport= null);

		IWindow_AdvancedCommandPokemon Empty(float x, float y, int width, int height, IViewport viewport = null);

		int index { set; }

		//string[] commands { set; }

		int width { set; }

		int height { set; }

		void resizeToFit(string[] commands, int? width = null);

		int itemCount();

		void drawItem(int index, int count, IRect rect);
	}

	// Represents a window with no formatting capabilities.  Its text color can be set,
	// though, and line breaks are supported, but the text is generally unformatted.
	public interface IWindow_UnformattedTextPokemon : ISpriteWindow_Base {
		string text				{ get; set; }
		IColor baseColor				{ get; set; }
		IColor shadowColor				{ get; set; }
		//  Letter-by-letter mode.  This mode is not supported in this class.
		bool letterbyletter				{ get; set; }

		//void text=(value) {
		//@text=value;
		//refresh();
		//}

		//void baseColor=(value) {
		//@baseColor=value;
		//refresh();
		//}

		//void shadowColor=(value) {
		//@shadowColor=value;
		//refresh();
		//}

		IWindow_UnformattedTextPokemon initialize(string text="");

		IWindow_UnformattedTextPokemon WithSize(string text, float x, float y, int width, int height, IViewport viewport = null);

		/// <summary>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="maxwidth">maxwidth is maximum acceptable window width</param>
		/// <returns>returns width and hieght</returns>
		int[] resizeToFitInternal(string text, int maxwidth);

		void setTextToFit(string text, int maxwidth = -1);

		// maxwidth is maximum acceptable window width
		void resizeToFit(string text, int maxwidth = -1);

		// width is current window width
		void resizeHeightToFit(string text, int width = -1);

		void refresh();
	}

	public interface IWindow_AdvancedTextPokemon : ISpriteWindow_Base {
		string text				{ get; set; }
		IColor baseColor		{ get; set; }
		IColor shadowColor		{ get; set; }
		bool letterbyletter		{ get; set; }
		int lineHeight			{ get; set; }

		//void lineHeight(value) {
		//	@lineHeight=value;
		//	this.text=this.text;
		//}
		//
		//void text=(value) {
		//	setText(value);
		//}

		int textspeed { get; set; }

		int waitcount { get; set; }

		void setText(string value);
		//
		//void baseColor=(value) {
		//	@baseColor=value;
		//	refresh();
		//}
		//
		//void shadowColor=(value) {
		//	@shadowColor=value;
		//	refresh();
		//}

		bool busy { get; }

		bool pausing { get; }

		bool resume();

		//void dispose();

		int cursorMode				{ get; set; }

		//void cursorMode=(value) {
		//	@cursorMode=value;
		//	moveCursor;
		//}

		void moveCursor();

		IWindow_AdvancedTextPokemon initialize(string text = "");

		IWindow_AdvancedTextPokemon WithSize(string text, float x, float y, int width, int height, IViewport viewport= null);

		//void width=(value) {
		//	super;
		//	if (!@starting) {
		//		this.text=this.text;
		//	}
		//}

		//void height=(value) {
		//	super;
		//	if (!@starting) {
		//		this.text=this.text;
		//	}
		//}

		int[] resizeToFitInternal(string text, int maxwidth);

		void resizeToFit2(string text, int maxwidth, int maxheight);

		void setTextToFit(string text, int maxwidth = -1);

		void resizeToFit(string text, int maxwidth = -1);

		void resizeHeightToFit(string text, int width = -1);

		void refresh();

		int maxPosition();

		int position();

		void redrawText();

		void updateInternal();

		void update();

		void allocPause();

		void startPause();

		void stopPause();
	}

	public interface IWindow_InputNumberPokemon : ISpriteWindow_Base {
		int number			{ get; set; }
		bool sign				{ get; set; }
		bool active			{ get; set; }

		void initialize(int digits_max);

		void refresh();

		void update();
	}

	public interface IAnimatedSprite : ISpriteWrapper {
		int frame						{ get; set; }
		int framewidth					{ get; set; }
		int frameheight					{ get; set; }
		int framecount					{ get; set; }
		int animname					{ get; set; }

		void initializeLong(string animname, int framecount, int framewidth, int frameheight, int frameskip);

		//  Shorter version of AnimationSprite.  All frames are placed on a single row
		//  of the bitmap, so that the width and height need not be defined beforehand
		void initializeShort(string animname, int framecount, int frameskip);

		//IAnimatedSprite initialize(*args);

		IAnimatedSprite create(string animname, int framecount, int frameskip, IViewport viewport = null);

		void dispose();

		bool playing();

		//void frame=(value) {
		//	@frame=value;
		//	@realframes=0;
		//	this.src_rect.x=@frame%@framesperrow*@framewidth;
		//	this.src_rect.y=@frame/@framesperrow*@frameheight;
		//}

		void start();

		//alias play start;

		void stop();

		void update();
	}

	/// <summary>
	/// Displays an icon bitmap in a sprite. Supports animated images.
	/// </summary>
	public interface IIconSprite : ISpriteWrapper {
		/// <summary>
		/// Sets the icon's filename.  Alias for <seealso cref="setBitmap"/>.
		/// </summary>
		string name				{ get; set; }
		//void name=(value) {
		//	setBitmap(value);
		//}

		//IIconSprite initialize(*args);
		IIconSprite initialize(float x, float y, IViewport viewport);

		//void dispose();

		void update();

		void clearBitmaps();

		/// <summary>
		/// Sets the icon's filename.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="hue"></param>
		void setBitmap(string file, int hue= 0);
	}

	// Old GifSprite class, retained for compatibility
	public interface IGifSprite : IIconSprite {
		IGifSprite initialize(string path);
	}

	// Sprite class that maintains a bitmap of its own.
	// This bitmap can't be changed to a different one.
	public interface IBitmapSprite : ISpriteWrapper {
		IBitmapSprite initialize(int width,int height,IViewport viewport=null);

		IBitmap bitmap { set; }

		void Dispose();
	}

	/// <summary>
	/// extension
	/// </summary>
	public interface IPlaneSpriteWindow {
		void update();
		void refresh();
	}

	/*
	// This class works around a limitation that planes are always
	// 640 by 480 pixels in size regardless of the window's size.
	public interface ILargePlane : IPlane {
		int borderX				{ get; protected set; }
		int borderY				{ get; protected set; }

		void initialize(viewport=null) {
		@__sprite=new Sprite(viewport);
		@__disposed=false;
		@__ox=0;
		@__oy=0;
		@__bitmap=null;
		@__visible=true;
		@__sprite.visible=false;
		@borderX=0;
		@borderY=0;
		}

		bool disposed() {
		return @__disposed;
		}

		void dispose() {
		if (!@__disposed) {
			if (@__sprite.bitmap) @__sprite.bitmap.dispose();
			@__sprite.dispose();
			@__sprite=null;
			@__bitmap=null;
			@__disposed=true;
		}
		super;
		}

		void ox() { @__ox; }
		void oy() { @__oy; }
  
		void ox=(value) { 
		if (@__ox!=value) {
			@__ox=value; refresh();
		}
		}

		void oy=(value) { 
		if (@__oy!=value) {
			@__oy=value; refresh();
		}
		}

		void bitmap() {
		return @__bitmap;
		}

		void bitmap=(value) {
		if (value==null) {
			if (@__bitmap!=null) {
			@__bitmap=null;
			@__sprite.visible=(@__visible && !@__bitmap.null?);
			}
		} else if (@__bitmap!=value && !value.disposed?) {
			@__bitmap=value;
			refresh();
		} else if (value.disposed?) {
			if (@__bitmap!=null) {
			@__bitmap=null;
			@__sprite.visible=(@__visible && !@__bitmap.null?);
			}
		}
		}

		void viewport() { @__sprite.viewport; }
		void zoom_x() { @__sprite.zoom_x; }
		void zoom_y() { @__sprite.zoom_y; }
		void opacity() { @__sprite.opacity; }
		void blend_type() { @__sprite.blend_type; }
		void visible() { @__visible; }
		void z() { @__sprite.z; }
		void color() { @__sprite.color; }
		void tone() { @__sprite.tone; }

		void zoom_x=(v) { 
		if (@__sprite.zoom_x!=v) {
			@__sprite.zoom_x=v; refresh();
		}
		}

		void zoom_y=(v) { 
		if (@__sprite.zoom_y!=v) {
			@__sprite.zoom_y=v; refresh();
		}
		}

		void opacity=(v) { @__sprite.opacity=(v); }
		void blend_type=(v) { @__sprite.blend_type=(v); }
		void visible=(v) { @__visible=v; @__sprite.visible=(@__visible && !@__bitmap.null?); }
		void z=(v) { @__sprite.z=(v); }
		void color=(v) { @__sprite.color=(v); }
		void tone=(v) { @__sprite.tone=(v); }
		void update() { end

		void refresh() {
		@__sprite.visible=(@__visible && !@__bitmap.null?);
		if (@__bitmap) {
			if (!@__bitmap.disposed?) {
			if (@__ox<0) @__ox+=@__bitmap.width*@__sprite.zoom_x;
			if (@__oy<0) @__oy+=@__bitmap.height*@__sprite.zoom_y;
			if (@__ox>@__bitmap.width) @__ox-=@__bitmap.width*@__sprite.zoom_x;
			if (@__oy>@__bitmap.height) @__oy-=@__bitmap.height*@__sprite.zoom_y;
			dwidth=(Graphics.width/@__sprite.zoom_x+@borderX).to_i; // +2
			dheight=(Graphics.height/@__sprite.zoom_y+@borderY).to_i; // +2
			@__sprite.bitmap=ensureBitmap(@__sprite.bitmap,dwidth,dheight);
			@__sprite.bitmap.clear();
			tileBitmap(@__sprite.bitmap,@__bitmap,@__bitmap.rect);
			} else {
			@__sprite.visible=false;
			}
		}
		}

		private;

		void ensureBitmap(IBitmap bitmap,dwidth,dheight) {
		if (!bitmap||bitmap.disposed?||bitmap.width<dwidth||bitmap.height<dheight) {
			if (bitmap) bitmap.dispose();
			bitmap=new Bitmap([1,dwidth].max,[1,dheight].max);
		}
		return bitmap;
		}

		void tileBitmap(dstbitmap,srcbitmap,srcrect) {
		if (!srcbitmap || srcbitmap.disposed?) return;
		dstrect=dstbitmap.rect;
		left=dstrect.x-@__ox/@__sprite.zoom_x;
		top=dstrect.y-@__oy/@__sprite.zoom_y;
		left=left.to_i; top=top.to_i;
		while left>0; left-=srcbitmap.width; }
		while top>0; top-=srcbitmap.height; }
		y=top; while y<dstrect.height;
			x=left; while x<dstrect.width;
			dstbitmap.blt(x+@borderX,y+@borderY,srcbitmap,srcrect);
			x+=srcrect.width;
			}
			y+=srcrect.height;
		}
		}
	}

	// A plane class that displays a single color.
	public interface IColoredPlane : ILargePlane {
		void initialize(color,viewport=null) {
		super(viewport);
		this.bitmap=new Bitmap(32,32);
		setPlaneColor(color);
		}

		void dispose() {
		if (this.bitmap) this.bitmap.dispose();
		super;
		}

		void update() { super; }

		void setPlaneColor(value) {
		this.bitmap.fill_rect(0,0,this.bitmap.width,this.bitmap.height,value);
		this.refresh();
		}
	}

	// A plane class that supports animated images.
	public interface IAnimatedPlane : ILargePlane {
		void initialize(viewport) {
		super(viewport);
		@bitmap=null;
		}

		void dispose() {
		clearBitmaps();
		super;
		}

		void update() {
		super;
		if (@bitmap) {
			@bitmap.update();
			this.bitmap=@bitmap.bitmap;
		}
		}

		void clearBitmaps() {
		if (@bitmap) @bitmap.dispose();
		@bitmap=null;
		if (!this.disposed?) this.bitmap=null;
		}

		void setPanorama(file, hue=0) {
		clearBitmaps();
		if (file==null) return;
		@bitmap=new AnimatedBitmap("Graphics/Panoramas/"+file,hue);
		}

		void setFog(file, hue=0) {
		clearBitmaps();
		if (file==null) return;
		@bitmap=new AnimatedBitmap("Graphics/Fogs/"+file,hue);
		}

		void setBitmap(file, hue=0) {
		clearBitmaps();
		if (file==null) return;
		@bitmap=new AnimatedBitmap(file,hue);
		}
	}
	*/

	// Displays an icon bitmap in a window. Supports animated images.
	public interface IIconWindow : ISpriteWindow_Base {
		//  Sets the icon's filename.  Alias for setBitmap.
		string name				{ get; set; }
		//void name=(value) {
		//	setBitmap(value);
		//}

		IIconWindow initialize(float x, float y, int width, int height, IViewport viewport= null);

		void dispose();

		void update();

		void clearBitmaps();

		//  Sets the icon's filename.
		void setBitmap(string file, int hue= 0);
	}

	// Displays an icon bitmap in a window. Supports animated images.
	// Accepts bitmaps and paths to bitmap files in its constructor
	public interface IPictureWindow : ISpriteWindow_Base {
		IPictureWindow initialize(string pathOrBitmap);

		void dispose();

		void update();

		void clearBitmaps();

		//  Sets the icon's bitmap or filename. (hue parameter
		//  is ignored unless pathOrBitmap is a filename)
		void setBitmap(string pathOrBitmap, int hue= 0);
	}

	public interface IWindow_CommandPokemonEx : IWindow_CommandPokemon { }

	public interface IWindow_AdvancedCommandPokemonEx : IWindow_AdvancedCommandPokemon { }
}
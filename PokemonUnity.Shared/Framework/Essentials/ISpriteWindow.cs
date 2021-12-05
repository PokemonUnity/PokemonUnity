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

		bool isDarkBackground(background, IRect rect = null);

		bool isDarkWindowskin(windowskin);

		IColor getDefaultTextColors(windowskin);

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
		void pbDrawTextPositions(IBitmap bitmap, object[] textpos);

		void pbDrawImagePositions(IBitmap bitmap, object[] textpos);



		void pbPushFade();

		void pbPopFade();

		bool pbIsFaded { get; }

		// pbFadeOutIn(z) { block }
		// Fades out the screen before a block is run and fades it back in after the
		// block exits.  z indicates the z-coordinate of the viewport used for this effect
		void pbFadeOutIn(int z, bool nofadeout = false, Action action = null);

		void pbFadeOutAndHide(ISprite[] sprites);

		void pbFadeInAndShow(ISprite[] sprites, IList<> visiblesprites = null, Action action = null);

// Restores which windows are active for the given sprite hash.
// _activeStatuses_ is the result of a previous call to pbActivateWindows
void pbRestoreActivations(sprites,activeStatuses) {
  if (!sprites || !activeStatuses) return;
  foreach (var k in activeStatuses.keys) {
	if (sprites[k] && sprites[k] is Window && !pbDisposed(sprites[k])) {
	  sprites[k].active=activeStatuses[k] ? true : false;
	}
  }
}

		// Deactivates all windows. If a code block is given, deactivates all windows,
		// runs the code in the block, and reactivates them.
		void pbDeactivateWindows(IWindow[] sprites, Action action = null);

		// Activates a specific window of a sprite hash. _key_ is the key of the window
		// in the sprite hash. If a code block is given, deactivates all windows except
		// the specified window, runs the code in the block, and reactivates them.
		void pbActivateWindow(IWindow[] sprites, int key, Action action = null);

		IColor pbAlphaBlend(IColor dstColor, IColor srcColor);

		IColor pbSrcOver(IColor dstColor, IColor srcColor);

void pbSetSpritesToColor(sprites,color) {
  if (!sprites || !color) return;
  colors={}
  foreach (var i in sprites) {
	if (!i[1] || pbDisposed(i[1])) continue;
	colors[i[0]]=i[1].color.clone();
	i[1].color=pbSrcOver(i[1].color,color);
  }
  Graphics.update();
  Input.update();
  foreach (var i in colors) {
	if (!sprites[i[0]]) continue;
	sprites[i[0]].color=i[1];
  }
}

void pbTryString(x) {
  ret=pbGetFileChar(x);
  return (ret!=null && ret!="") ? x : null;
}

// Finds the real path for an image file.  This includes paths in encrypted
// archives.  Returns _x_ if the path can't be found.
void pbBitmapName(x) {
  ret=pbResolveBitmap(x);
  return ret ? ret : x;
}

// Finds the real path for an image file.  This includes paths in encrypted
// archives.  Returns null if the path can't be found.
void pbResolveBitmap(x) {
  if (!x) return null;
  noext=x.gsub(/\.(bmp|png|gif|jpg|jpeg)$/,"");
  filename=null;
//  RTP.eachPathFor(x) {|path|
//     filename=pbTryString(path) if !filename
//     filename=pbTryString(path+".gif") if !filename
//  }
  RTP.eachPathFor(noext) {|path|
	 if (!filename) filename=pbTryString(path+".png");
	 if (!filename) filename=pbTryString(path+".gif");
//     filename=pbTryString(path+".jpg") if !filename
//     filename=pbTryString(path+".jpeg") if !filename
//     filename=pbTryString(path+".bmp") if !filename
  }
  return filename;
}

// Adds a background to the sprite hash.
// _planename_ is the hash key of the background.
// _background_ is a filename within the Graphics/Pictures/ folder and can be
//     an animated image.
// _viewport_ is a viewport to place the background in.
void addBackgroundPlane(sprites,planename,background,viewport=null) {
  sprites[planename]=new AnimatedPlane(viewport);
  bitmapName=pbResolveBitmap("Graphics/Pictures/#{background}");
  if (bitmapName==null) {
//  Plane should exist in any case
	sprites[planename].bitmap=null;
	sprites[planename].visible=false;
  } else {
	sprites[planename].setBitmap(bitmapName);
	foreach (var spr in sprites.values) {
	  if (spr is Window) {
		spr.windowskin=null;
	  }
	}
  }
}

// Adds a background to the sprite hash.
// _planename_ is the hash key of the background.
// _background_ is a filename within the Graphics/Pictures/ folder and can be
//       an animated image.
// _color_ is the color to use if the background can't be found.
// _viewport_ is a viewport to place the background in.
void addBackgroundOrColoredPlane(sprites,planename,background,color,viewport=null) {
  bitmapName=pbResolveBitmap("Graphics/Pictures/#{background}");
  if (bitmapName==null) {
//  Plane should exist in any case
	sprites[planename]=new ColoredPlane(color,@viewport);
  } else {
	sprites[planename]=new AnimatedPlane(viewport);
	sprites[planename].setBitmap(bitmapName);
	foreach (var spr in sprites.values) {
	  if (spr is Window) {
		spr.windowskin=null;
	  }
	}
  }
}

// Sets a bitmap's font to the system font.
void pbSetSystemFont(bitmap) {
  fontname=MessageConfig.pbGetSystemFontName();
  bitmap.font.name=fontname;
  if (fontname=="Pokemon FireLeaf" || fontname=="Power Red and Green") {
	bitmap.font.size=29;
  } else if (fontname=="Pokemon Emerald Small" || fontname=="Power Green Small") {
	bitmap.font.size=25;
  } else {
	bitmap.font.size=31;
  }
}

// Gets the name of the system small font.
string pbSmallFontName() {
  return MessageConfig.pbTryFonts("Power Green Small","Pokemon Emerald Small",
	 "Arial Narrow","Arial");
}

// Gets the name of the system narrow font.
string pbNarrowFontName() {
  return MessageConfig.pbTryFonts("Power Green Narrow","Pokemon Emerald Narrow",
	 "Arial Narrow","Arial");
}

// Sets a bitmap's font to the system small font.
void pbSetSmallFont(bitmap) {
  bitmap.font.name=pbSmallFontName();
  bitmap.font.size=25;
}

// Sets a bitmap's font to the system narrow font.
void pbSetNarrowFont(bitmap) {
  bitmap.font.name=pbNarrowFontName();
  bitmap.font.size=31;
}



// Used to determine whether a data file exists (rather than a graphics or
// audio file). Doesn't check RTP, but does check encrypted archives.
bool pbRgssExists (string filename) {
  filename=canonicalize(filename);
  if ((safeExists("./Game.rgssad") || safeExists("./Game.rgss2a"))) {
	return pbGetFileChar(filename)!=null;
  } else {
	return safeExists(filename);
  }
}

// Opens an IO, even if the file is in an encrypted archive.
// Doesn't check RTP for the file.
void pbRgssOpen(string file,int? mode=null, Action action = null) {
// File.open("debug.txt","ab"){|fw| fw.write([file,mode,Time.now.to_f].inspect+"\r\n") }
  if (!safeExists("./Game.rgssad") && !safeExists("./Game.rgss2a")) {
	if (block_given?) {
	  File.open(file,mode){|f| yield f }
	  return null;
	} else {
	  return File.open(file,mode);
	}
  }
  file=canonicalize(file);
  Marshal.neverload=true;
  begin;
	str=load_data(file);
  ensure;
	Marshal.neverload=false;
  }
  if (block_given?) {
	StringInput.open(str){|f| yield f }
	return null;
  } else {
	return StringInput.open(str);
  }
}

// Gets at least the first byte of a file. Doesn't check RTP, but does check
// encrypted archives.
void pbGetFileChar(string file) {
  file=canonicalize(file);
  if (!(safeExists("./Game.rgssad") || safeExists("./Game.rgss2a"))) {
	if (!safeExists(file)) return null;
	begin;
	  File.open(file,"rb"){|f|
		 return f.read(1); // read one byte
	  }
	rescue Errno.ENOENT, Errno.EINVAL, Errno.EACCES;
	  return null;
	}
  }
  Marshal.neverload=true;
  str=null;
  begin;
	str=load_data(file);
  rescue Errno.ENOENT, Errno.EINVAL, Errno.EACCES, RGSSError;
	str=null;
  ensure;
	Marshal.neverload=false;
  }
  return str;
}

// Gets the contents of a file. Doesn't check RTP, but does check
// encrypted archives.
void pbGetFileString(file) {
  file=canonicalize(file);
  if (!(safeExists("./Game.rgssad") || safeExists("./Game.rgss2a"))) {
	if (!safeExists(file)) return null;
	begin;
	  File.open(file,"rb"){|f|
		 return f.read; // read all data
	  }
	rescue Errno.ENOENT, Errno.EINVAL, Errno.EACCES;
	  return null;
	}
  }
  Marshal.neverload=true;
  str=null;
  begin;
	str=load_data(file);
  rescue Errno.ENOENT, Errno.EINVAL, Errno.EACCES, RGSSError;
	str=null;
  ensure;
	Marshal.neverload=false;
  }
  return str;
}
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


	public interface IAnimatedBitmap : IBitmap {
		//void initialize(file,hue=0) {
		//	if (file==null) raise "filename is null";
		//	if (file[/^\[(\d+)\]/]  ) {		// Starts with 1 or more digits in brackets
		//	  @bitmap=new PngAnimatedBitmap(file,hue);
		//	} else {
		//	  @bitmap=new GifBitmap(file,hue);
		//	}
		//}

		//void [](index) { @bitmap[index]; }
		//void width() { @bitmap.bitmap.width; }
		//void height() { @bitmap.bitmap.height; }
		//void length() { @bitmap.Length; }
		//void each() { @bitmap.each {|item| yield item }; }
		//void bitmap() { @bitmap.bitmap; }
		//void currentIndex() { @bitmap.currentIndex; }
		//void frameDelay() { @bitmap.frameDelay; }
		//void totalFrames() { @bitmap.totalFrames; }
		//bool disposed { @bitmap.disposed(); }
		//void update() { @bitmap.update(); }
		//void dispose() { @bitmap.dispose(); }
		//void deanimate() { @bitmap.deanimate; }
		//void copy() { @bitmap.copy; }
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
  void initialize(string file,int hue=0) {
	@frames=[];
	@currentFrame=0;
	@framecount=0;
	panorama=BitmapCache.load_bitmap(file,hue);
	if (file[/^\[(\d+)\]/]  ) {		// Starts with 1 or more digits in brackets
//  File has a frame count
	  numFrames=$1.to_i;
	  if (numFrames<=0) {
		raise "Invalid frame count in #{file}";
	  }
	  if (panorama.width % numFrames != 0) {
		raise "Bitmap's width (#{panorama.width}) is not divisible by frame count: #{file}";
	  }
	  subWidth=panorama.width/numFrames;
	  for (int i = 0; i < numFrames; i++) {
		subBitmap=new BitmapWrapper(subWidth,panorama.height);
		subBitmap.blt(0,0,panorama,new Rect(subWidth*i,0,subWidth,panorama.height));
		@frames.Add(subBitmap);
	  }
	  panorama.dispose();
	} else {
	  @frames= new []{ panorama };
	}
  }

  void [int index] { get; } //return @frames[index];

  void width() { this.bitmap.width; }
  
  void height() { this.bitmap.height; }
  
  void deanimate() {
	for (int i = 1; i < @frames.Length; i++) {
	  @frames[i].dispose();
	}
	@frames=[@frames[0]];
	@currentFrame=0;
	return @frames[0];
  }

  void bitmap() {
	@frames[@currentFrame];
  }

  void currentIndex() {
	@currentFrame;
  }

  void frameDelay(index) {
	return 10;
  }

  void length() {
	@frames.Length;
  }

  void each() {
	@frames.each {|item| yield item}
  }

  void totalFrames() {
	10*@frames.Length;
  }

  bool disposed() {
	@disposed;
  }

  void update() {
	if (disposed?) return;
	if (@frames.Length>1) {
	  @framecount+=1;
	  if (@framecount>=10) {
		@framecount=0;
		@currentFrame+=1;
		@currentFrame%=@frames.Length;
	  }
	}
  }

  void dispose() {
	if (!@disposed) {
	  foreach (var i in @frames) {
		i.dispose();
	  }
	}
	@disposed=true;
  }

  int frames				{ get; protected set; } // internal

  void copy() {
	x=this.clone();
	x.frames=x.frames.clone();
	for (int i = 0; i < x.frames.Length; i++) {
	  x.frames[i]=x.frames[i].copy;
	}
	return x;
  }
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
	public interface ISpriteWrapper : ISprite {
		ISpriteWrapper initialize(viewport= null);

		void dispose();
		bool disposed();
		void flash(color, duration);
		void update();
		IViewport viewport { get; set; }
		//void viewport();
		//void x() {                     @sprite.x; }
		//void x=(value) {             @sprite.x=value; }
		//void y() {                     @sprite.y; }
		//void y=(value) {             @sprite.y=value; }
		//void bitmap() {                @sprite.bitmap; }
		//void bitmap=(value) {        @sprite.bitmap=value; }
		//void src_rect() {              @sprite.src_rect; }
		//void src_rect=(value) {      @sprite.src_rect=value; }
		//void visible() {               @sprite.visible; }
		//void visible=(value) {       @sprite.visible=value; }
		//void z() {                     @sprite.z; }
		//void z=(value) {             @sprite.z=value; }
		//void ox() {                    @sprite.ox; }
		//void ox=(value) {            @sprite.ox=value; }
		//void oy() {                    @sprite.oy; }
		//void oy=(value) {            @sprite.oy=value; }
		//void zoom_x() {                @sprite.zoom_x; }
		//void zoom_x=(value) {        @sprite.zoom_x=value; }
		//void zoom_y() {                @sprite.zoom_y; }
		//void zoom_y=(value) {        @sprite.zoom_y=value; }
		//void angle() {                 @sprite.angle; }
		//void angle=(value) {         @sprite.angle=value; }
		//void mirror() {                @sprite.mirror; }
		//void mirror=(value) {        @sprite.mirror=value; }
		//void bush_depth() {            @sprite.bush_depth; }
		//void bush_depth=(value) {    @sprite.bush_depth=value; }
		//void opacity() {               @sprite.opacity; }
		//void opacity=(value) {       @sprite.opacity=value; }
		//void blend_type() {            @sprite.blend_type; }
		//void blend_type=(value) {    @sprite.blend_type=value; }
		//void color() {                 @sprite.color; }
		//void color=(value) {         @sprite.color=value; }
		//void tone() {                  @sprite.tone; }
		//void tone=(value) {          @sprite.tone=value; }
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


int seek(int offset, int whence= IO.SEEK_SET);

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
  int tone					{ get; set; }
  int color					{ get; set; }
  int viewport				{ get; set; }
  int contents				{ get; set; }
  int ox					{ get; set; }
  int oy					{ get; set; }
  int x						{ get; set; }
  int y						{ get; set; }
  int z						{ get; set; }
  int zoom_x				{ get; set; }
  int zoom_y				{ get; set; }
  int offset_x				{ get; set; }
  int offset_y				{ get; set; }
  int width					{ get; set; }
  int active				{ get; set; }
  int pause					{ get; set; }
  int height				{ get; set; }
  int opacity				{ get; set; }
  int back_opacity			{ get; set; }
  int contents_opacity		{ get; set; }
  int visible				{ get; set; }
  int cursor_rect			{ get; set; }
  int contents_blend_type	{ get; set; }
  int blend_type			{ get; set; }
  int openness				{ get; set; }

  void windowskin() {
	@_windowskin;
  }

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

//  void initialize(viewport=null) {
//	@sprites={}
//	@spritekeys=[
//	   "back",
//	   "corner0","side0","scroll0",
//	   "corner1","side1","scroll1",
//	   "corner2","side2","scroll2",
//	   "corner3","side3","scroll3",
//	   "cursor","contents","pause";
//	];
//	@viewport=viewport;
//	@sidebitmaps= new []{ null,null,null,null };
//	@cursorbitmap=null;
//	@bgbitmap=null;
//	foreach (var i in @spritekeys) {
//	  @sprites[i]=new Sprite(@viewport);
//	}
//	@disposed=false;
//	@tone=new Tone(0,0,0);
//	@color=new Color(0,0,0,0);
//	@blankcontents=new Bitmap(1,1); // RGSS2 requires this
//	@contents=@blankcontents;
//	@_windowskin=null;
//	@rpgvx=false;
//	@compat=CompatBits.sxpandBack|CompatBits.stretchSides;
//	@x=0;
//	@y=0;
//	@width=0;
//	@height=0;
//	@offset_x=0;
//	@offset_y=0;
//	@zoom_x=1.0;
//	@zoom_y=1.0;
//	@ox=0;
//	@oy=0;
//	@z=0;
//	@stretch=true;
//	@visible=true;
//	@active=true;
//	@openness=255;
//	@opacity=255;
//	@back_opacity=255;
//	@blend_type=0;
//	@contents_blend_type=0;
//	@contents_opacity=255;
//	@cursor_rect=new SpriteWindowCursorRect(self);
//	@cursorblink=0;
//	@cursoropacity=255;
//	@pause=false;
//	@pauseframe=0;
//	@flash=0;
//	@pauseopacity=0;
//	@skinformat=0;
//	@skinrect=new Rect(0,0,0,0);
//	@trim= new []{ 16,16,16,16 };
//	privRefresh(true);
//  }

//  void dispose() {
//	if (!this.disposed?) {
//	  foreach (var i in @sprites) {
//		if (i[1]) i[1].dispose();
//		@sprites[i[0]]=null;
//	  }
//	  for (int i = 0; i < @sidebitmaps.Length; i++) {
//		if (@sidebitmaps[i]) @sidebitmaps[i].dispose();
//		@sidebitmaps[i]=null;
//	  }
//	  @blankcontents.dispose();
//	  if (@cursorbitmap) @cursorbitmap.dispose();
//	  if (@backbitmap) @backbitmap.dispose();
//	  @sprites.clear();
//	  @sidebitmaps.clear();
//	  @_windowskin=null;
//	  @disposed=true;
//	}
//  }

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
//}



public interface ISpriteWindow_Base : ISpriteWindow {
  TEXTPADDING=4; // In pixels

  ISpriteWindow_Base initialize(float x, float y, float width, float height);

  void __setWindowskin(skin) {
	if (skin && (skin.width==192 && skin.height==128) || 		// RPGXP Windowskin
			   (skin.width==128 && skin.height==128)) {     // RPGVX Windowskin
	  this.skinformat=0;
	} else {
	  this.skinformat=1;
	}
	this.windowskin=skin;
  }

  void __resolveSystemFrame() {
	if (this.skinformat==1) {
	  if (!@resolvedFrame) {
		@resolvedFrame=MessageConfig.pbGetSystemFrame();
		@resolvedFrame.sub!(/\.[^\.\/\\]+$/,"");
	  }
	  if (@resolvedFrame!="") this.loadSkinFile("#{@resolvedFrame}.txt");
	}
  }

  void setSkin(skin) { // Filename of windowskin to apply. Supports XP, VX, and animated skins.
	if (@customskin) @customskin.dispose();
	@customskin=null;
	resolvedName=pbResolveBitmap(skin);
	if (!resolvedName || resolvedName=="") return;
	@customskin=new AnimatedBitmap(resolvedName);
	__setWindowskin(@customskin.bitmap);
	if (this.skinformat==1) {
	  skinbase=resolvedName.sub(/\.[^\.\/\\]+$/,"");
	  this.loadSkinFile("#{skinbase}.txt");
	}
  }

  void setSystemFrame() {
	if (@customskin) @customskin.dispose();
	@customskin=null;
	__setWindowskin(@sysframe.bitmap);
	__resolveSystemFrame();
  }

  void update() {
	super;
	if (this.windowskin) {
	  if (@customskin) {
		if (@customskin.totalFrames>1) {
		  @customskin.update();
		  __setWindowskin(@customskin.bitmap);
		}
	  } else if (@sysframe) {
		if (@sysframe.totalFrames>1) {
		  @sysframe.update();
		  __setWindowskin(@sysframe.bitmap);
		}
	  }
	}
	if (@curframe!=MessageConfig.pbGetSystemFrame()) {
	  @curframe=MessageConfig.pbGetSystemFrame();
	  if (@sysframe && !@customskin) {
		if (@sysframe) @sysframe.dispose();
		@sysframe=new AnimatedBitmap(@curframe);
		@resolvedFrame=null;
		__setWindowskin(@sysframe.bitmap);
		__resolveSystemFrame()   ;
	  }
	  begin;
		refresh();
	  rescue NoMethodError;
	  }
	}
	if (@curfont!=MessageConfig.pbGetSystemFontName()) {
	  @curfont=MessageConfig.pbGetSystemFontName();
	  if (this.contents && !this.contents.disposed?) {
		pbSetSystemFont(this.contents);
	  }
	  begin;
		refresh();
	  rescue NoMethodError;
	  }
	}
  }

  void dispose() {
	if (this.contents) this.contents.dispose();
	@sysframe.dispose();
	if (@customskin) @customskin.dispose();
	super;
  }
}



public interface ISpriteWindow_Selectable : ISpriteWindow_Base {
  int index				{ get; set; }
  int itemCount			{ get; }
  int count				{ get; }
  int rowHeight			{ get; set; }
  int columns			{ get; set; }
  int columnSpacing		{ get; set; }
  int top_row			{ get; set; }
  int top_row			{ get; set; }
  int top_row			{ get; set; }
  int top_row			{ get; set; }

  void initialize(float x, float y, float width, float height);

  void ignore_input=(value) {
	@ignore_input=value;
  }

  void count() {
	return @item_max;
  }

  void row_max() {
	return ((@item_max + @column_max - 1) / @column_max).to_i;
  }

  void top_row() {
	return (@virtualOy / (@row_height || 32)).to_i;
  }

  void top_item() {
	return top_row * @column_max;
  }

  void update_cursor_rect() {
	priv_update_cursor_rect;
  }

  void top_row=(row) {
	if (row>row_max-1) {
	  row=row_max-1;
	}
	if (row<0) {		// NOTE: The two comparison checks must be reversed since row_max can be 0
	  row=0;
	}
	@virtualOy=row*@row_height;
  }

  void page_row_max() {
	return priv_page_row_max.to_i;
  }

  void page_item_max() {
	return priv_page_item_max.to_i;
  }

  void itemRect(item) {
	if (item<0 || item>=@item_max || item<this.top_item ||
	   item>this.top_item+this.page_item_max) {
	  return new Rect(0,0,0,0);
	} else {
	  cursor_width = (this.width-this.borderX-(@column_max-1)*@column_spacing) / @column_max;
	  x = item % @column_max * (cursor_width + @column_spacing);
	  y = item / @column_max * @row_height - @virtualOy;
	  return new Rect(x, y, cursor_width, @row_height);
	}
  }

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

	//void initialize(*arg);
}



public interface IWindow_DrawableCommand : ISpriteWindow_SelectableEx {
  int baseColor				{ get; set; }
  int shadowColor			{ get; set; }

	float textWidth(IBitmap bitmap, string text);

	void getAutoDims(string[] commands, ref int[] dims, float? width = null);

	void initialize(float x, float y, float width, float height, IViewport viewport = null);

	IRect drawCursor(int index, IRect rect);

	void dispose();

	// to be implemented by derived classes
	int itemCount();

	// to be implemented by derived classes
	void drawItem(int index, int count, IRect rect);

	void refresh();

	void update();
}

public interface IWindow_CommandPokemon : IWindow_DrawableCommand {
	string[] commands				{ get; set; }

	IWindow_CommandPokemon initialize(string[] commands,float? width=null);

	IWindow_CommandPokemon WithSize(string[] commands, float x, float y, float width, float height, IViewport viewport= null);

	IWindow_CommandPokemon Empty(float x, float y, float width, float height, IViewport viewport= null);

	float index { get; set; }

	float width { get; set; }

	float height { get; set; }

	void resizeToFit(string[] commands, float? width = null);

	int itemCount();

	void drawItem(int index, int count, IRect rect);
}

	public interface IWindow_AdvancedCommandPokemon : IWindow_DrawableCommand {
		string[] commands				{ get; set; }

		int textWidth(IBitmap bitmap, string text);

		IWindow_AdvancedCommandPokemon initialize(string[] commands, int? width= null);

		IWindow_AdvancedCommandPokemon WithSize(string[] commands,float x,float y,int width,int height,IViewport viewport= null);

		IWindow_AdvancedCommandPokemon Empty(float x, float y, int width, int height, IViewport viewport = null);

		int index { set; }

		string[] commands { set; }

		int width { set; }

		int height { set; }

		void resizeToFit(string[] commands, int width = null);

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
  IColor baseColor			{ get; set; }
  IColor shadowColor		{ get; set; }
  bool letterbyletter		{ get; set; }
  int lineHeight			{ get; set; }

  void lineHeight(value) {
	@lineHeight=value;
	this.text=this.text;
  }

  void text=(value) {
	setText(value);
  }

  void textspeed() {
	@frameskip;
  }

  void textspeed=(value) {
	@frameskip=value;
	@frameskipChanged=true;
  }

  void waitcount() {
	@waitcount;
  }

  void waitcount=(value) {
	@waitcount=(value<=0) ? 0 : value;
  }

  void setText(value) {
	@waitcount=0;
	@curchar=0;
	@drawncurchar=-1;
	@lastDrawnChar=-1;
	oldtext=@text;
	@text=value;
	@textlength=unformattedTextLength(value);
	@scrollstate=0;
	@scrollY=0;
	@linesdrawn=0;
	@realframes=0;
	@textchars=[];
	width=1;
	height=1;
	numlines=0;
	visiblelines=(this.height-this.borderY)/32;
	if (value.Length==0) {
	  @fmtchars=[];
	  @bitmapwidth=width;
	  @bitmapheight=height;
	  @numtextchars=0;
	} else {
	  if (!@letterbyletter) {
		@fmtchars=getFormattedText(this.contents,0,0,
		   this.width-this.borderX-SpriteWindow_Base.eEXTPADDING,-1,
		   shadowctag(@baseColor,@shadowColor)+value,32,true);
		@oldfont=this.contents.font.clone();
		foreach (var ch in @fmtchars) {
		  chx=ch[1]+ch[3];
		  chy=ch[2]+ch[4];
		  if (width<chx) width=chx;
		  if (height<chy) height=chy;
		  @textchars.Add(ch[5] ? "" : ch[0]);
		}
	  } else {
		@fmtchars=[];
		fmt=getFormattedText(this.contents,0,0,
		   this.width-this.borderX-SpriteWindow_Base.eEXTPADDING,-1,
		   shadowctag(@baseColor,@shadowColor)+value,32,true);
		@oldfont=this.contents.font.clone();
		foreach (var ch in fmt) {
		  chx=ch[1]+ch[3];
		  chy=ch[2]+ch[4];
		  if (width<chx) width=chx;
		  if (height<chy) height=chy;
		  if (!ch[5] && ch[0]=="\n" && @letterbyletter) {
			numlines+=1;
			if (numlines>=visiblelines) {
			  fclone=ch.clone();
			  fclone[0]="\1";
			  @fmtchars.Add(fclone);
			  @textchars.Add("\1");
			}
		  }
//  Don't add newline characters, since they
//  can slow down letter-by-letter display
		  if (ch[5] || (ch[0]!="\r")) {
			@fmtchars.Add(ch);
			@textchars.Add(ch[5] ? "" : ch[0]);
		  }
		}
		fmt.clear();
	  }
	  @bitmapwidth=width;
	  @bitmapheight=height;
	  @numtextchars=@textchars.Length;
	}
	stopPause;
	@displaying=@letterbyletter;
	@needclear=true;
	@nodraw=@letterbyletter;
	refresh();
  }

  void baseColor=(value) {
	@baseColor=value;
	refresh();
  }

  void shadowColor=(value) {
	@shadowColor=value;
	refresh();
  }

  bool busy() {
	return @displaying;
  }

  bool pausing() {
	return @pausing && @displaying;
  }

  void resume() {
	if (!busy?) {
	  this.stopPause;
	  return true;
	}
	if (@pausing) {
	  @pausing=false;
	  this.stopPause;
	  return false;
	} else {
	  return true;
	}
  }

  void dispose() {
	if (disposed?) return;
	if (@pausesprite) @pausesprite.dispose();
	@pausesprite=null;
	super;
  }

  int cursorMode				{ get; protected set; }

  void cursorMode=(value) {
	@cursorMode=value;
	moveCursor;
  }

  void moveCursor() {
	if (@pausesprite) {
	  cursor=@cursorMode;
	  if (cursor==0 && !@endOfText) cursor=2;
	  switch (cursor) {
		break;
	  case 0: // End of text
		@pausesprite.x=this.x+this.startX+@endOfText.x+@endOfText.width-2;
		@pausesprite.y=this.y+this.startY+@endOfText.y-@scrollY;
		break;
	  case 1: // Lower right
		pauseWidth=@pausesprite.bitmap ? @pausesprite.framewidth : 16;
		pauseHeight=@pausesprite.bitmap ? @pausesprite.frameheight : 16;
		@pausesprite.x=this.x+this.width-(20*2)+(pauseWidth/2);
		@pausesprite.y=this.y+this.height-(30*2)+(pauseHeight/2);
		break;
	  case 2: // Lower middle
		pauseWidth=@pausesprite.bitmap ? @pausesprite.framewidth : 16;
		pauseHeight=@pausesprite.bitmap ? @pausesprite.frameheight : 16;
		@pausesprite.x=this.x+(this.width/2)-(pauseWidth/2);
		@pausesprite.y=this.y+this.height-(18*2)+(pauseHeight/2);
	  }
	}
  }

  IWindow_AdvancedTextPokemon initialize(string text = "");

IWindow_AdvancedTextPokemon WithSize(string text, x, y, width, height, viewport= null);

  void width=(value) {
	super;
	if (!@starting) {
	  this.text=this.text;
	}
  }

  void height=(value) {
	super;
	if (!@starting) {
	  this.text=this.text;
	}
  }

  void resizeToFitInternal(string text,maxwidth) {
	dims= new []{ 0,0 };
	cwidth=maxwidth<0 ? Graphics.width : maxwidth;
	chars=getFormattedTextForDims(this.contents,0,0,
	   cwidth-this.borderX-2-6,-1,text,@lineHeight,true);
	foreach (var ch in chars) {
	  dims[0]=[dims[0],ch[1]+ch[3]].max;
	  dims[1]=[dims[1],ch[2]+ch[4]].max;
	}
	return dims;
  }

  void resizeToFit2(string text,maxwidth,maxheight) {
	dims=resizeToFitInternal(text,maxwidth);
	oldstarting=@starting;
	@starting=true;
	this.width=[dims[0]+this.borderX+SpriteWindow_Base.eEXTPADDING,maxwidth].min;
	this.height=[dims[1]+this.borderY,maxheight].min;
	@starting=oldstarting;
	redrawText;
  }

  void setTextToFit(string text,maxwidth=-1) {
	resizeToFit(text,maxwidth);
	this.text=text;
  }

  void resizeToFit(string text,maxwidth=-1) {
	dims=resizeToFitInternal(text,maxwidth);
	oldstarting=@starting;
	@starting=true;
	this.width=dims[0]+this.borderX+SpriteWindow_Base.eEXTPADDING;
	this.height=dims[1]+this.borderY;
	@starting=oldstarting;
	redrawText;
  }

  void resizeHeightToFit(string text,width=-1) {
	dims=resizeToFitInternal(text,width);
	oldstarting=@starting;
	@starting=true;
	this.width=width<0 ? Graphics.width : width;
	this.height=dims[1]+this.borderY;
	@starting=oldstarting;
	redrawText;
  }

  void refresh() {
	oldcontents=this.contents;
	this.contents=pbDoEnsureBitmap(oldcontents,@bitmapwidth,@bitmapheight);
	this.oy=@scrollY;
	numchars=@numtextchars;
	startchar=0;
	if (this.letterbyletter) numchars=[@curchar,@numtextchars].min;
	if (busy? && @drawncurchar==@curchar && @scrollstate==0) {
	  return;
	}
	if (!this.letterbyletter || !oldcontents.equal(this.contents)) {
	  @drawncurchar=-1;
	  @needclear=true;
	}
	if (@needclear) {
	  if (@oldfont) this.contents.font=@oldfont;
	  this.contents.clear();
	  @needclear=false;
	}
	if (@nodraw) {
	  @nodraw=false;
	  return;
	}
	maxX=this.width-this.borderX;
	maxY=this.height-this.borderY;
	foreach (var i in @drawncurchar+1..numchars) {
	  if (i>=@fmtchars.Length) continue;
	  if (!this.letterbyletter) {
		if (@fmtchars[i][1]>=maxX) continue;
		if (@fmtchars[i][2]>=maxY) continue;
	  }
	  drawSingleFormattedChar(this.contents,@fmtchars[i]);
	  @lastDrawnChar=i;
	}
	if (!this.letterbyletter) {
//  all characters were drawn, reset old font
	  if (@oldfont) this.contents.font=@oldfont;
	}
	if (numchars>0 && numchars!=@numtextchars) {
	  fch=@fmtchars[numchars-1];
	  if (fch) {
		rcdst=new Rect(fch[1],fch[2],fch[3],fch[4]);
		if (@textchars[numchars]=="\1") {
		  @endOfText=rcdst;
		  allocPause;
		  moveCursor();
		} else {
		  @endOfText=new Rect(rcdst.x+rcdst.width,rcdst.y,8,1);
		}
	  }
	}
	@drawncurchar=@curchar;
  }

  void maxPosition() {
	pos=0;
	foreach (var ch in @fmtchars) {
//  index after the last character's index
	  if (pos<ch[14]+1) pos=ch[14]+1;
	}
	return pos;
  }

  void position() {
	if (@lastDrawnChar<0) {
	  return 0;
	} else if (@lastDrawnChar>=@fmtchars.Length) {
	  return @numtextchars;
	} else {
//  index after the last character's index
	  return @fmtchars[@lastDrawnChar][14]+1;
	}
  }

  void redrawText() {
	if (!@letterbyletter) {
	  this.text=this.text;
	} else {
	  oldPosition=this.position;
	  this.text=this.text;
	  if (oldPosition>@numtextchars) oldPosition=@numtextchars;
	  while (this.position!=oldPosition) {
		refresh();
		updateInternal;
	  }
	}
  }

  void updateInternal() {
	curcharskip=@frameskip<0 ? @frameskip.abs : 1;
	visiblelines=(this.height-this.borderY)/@lineHeight;
	if (@textchars[@curchar]=="\1") {
	  if (!@pausing) {
		@realframes+=1;
		if (@realframes>=@frameskip || @frameskip<0) {
		  curcharSkip(curcharskip);
		  @realframes=0;
		}
	  }
	} else if (@textchars[@curchar]=="\n") {
	  if (@linesdrawn>=visiblelines-1) {
		if (@scrollstate<@lineHeight) {
		  @scrollstate+=[(@lineHeight/4),1].max;
		  @scrollY+=[(@lineHeight/4),1].max;
		}
		if (@scrollstate>=@lineHeight) {
		  @realframes+=1;
		  if (@realframes>=@frameskip || @frameskip<0) {
			curcharSkip(curcharskip);
			@linesdrawn+=1;
			@scrollstate=0;
			@realframes=0;
		  }
		}
	  } else {
		@realframes+=1;
		if (@realframes>=@frameskip || @frameskip<0) {
		  curcharSkip(curcharskip);
		  @linesdrawn+=1;
		  @realframes=0;
		}
	  }
	} else if (@curchar<=@numtextchars) {
	  @realframes+=1;
	  if (@realframes>=@frameskip || @frameskip<0) {
		curcharSkip(curcharskip);
		@realframes=0;
	  }
	  if (@textchars[@curchar]=="\1") {
		if (@curchar<@numtextchars-1) @pausing=true;
		this.startPause;
		refresh();
	  }
	} else {
	  @displaying=false;
	  @scrollstate=0;
	  @scrollY=0;
	  @linesdrawn=0;
	}
  }

  void update() {
	super;
	if (@pausesprite && @pausesprite.visible) {
	  @pausesprite.update();
	}
	if (@waitcount>0) {
	  @waitcount-=1;
	  return;
	}
	if (busy?) {
	  if (!@frameskipChanged) refresh();
	  updateInternal;
//  following line needed to allow "textspeed=-999" to work seamlessly
	  if (@frameskipChanged) refresh();
	}
	@frameskipChanged=false;
  }

  void allocPause() {
	if (!@pausesprite) {
	  @pausesprite=AnimatedSprite.create("Graphics/Pictures/pause",4,3);
	  @pausesprite.z=100000;
	  @pausesprite.visible=false;
	}
  }

  void startPause() {
	allocPause;
	@pausesprite.visible=true;
	@pausesprite.frame=0;
	@pausesprite.start;
	moveCursor;
  }

  void stopPause() {
	if (@pausesprite) {
	  @pausesprite.stop;
	  @pausesprite.visible=false;
	}
  }
  }
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
  int framewidth				{ get; set; }
  int frameheight				{ get; set; }
  int framecount				{ get; set; }
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

// Displays an icon bitmap in a sprite. Supports animated images.
public interface IIconSprite : ISpriteWrapper {
//  Sets the icon's filename.  Alias for setBitmap.
string name				{ get; set; }
	//void name=(value) {
	//	setBitmap(value);
	//}

	//IIconSprite initialize(*args);

	void dispose();

	void update();

	void clearBitmaps();

	//  Sets the icon's filename.
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

	void dispose();
}

public interface IPlane {
	void update();
	void refresh();
}

/*
// This class works around a limitation that planes are always
// 640 by 480 pixels in size regardless of the window's size.
public interface ILargePlane : Plane {
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

  void ensureBitmap(bitmap,dwidth,dheight) {
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
public interface IColoredPlane : LargePlane {
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
public interface IAnimatedPlane : LargePlane {
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
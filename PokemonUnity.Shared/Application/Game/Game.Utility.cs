using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	public partial class Game 
	{
		//On Project start...
		//all XML files are opened, and locked
		//one by one, they're scanned and checked to see if they're up to date (match latest with compile time info)
		//if not the latest, perform an update by fetching and downloading latest info...
		//else continue, and load the XML data into the variables
		public static event EventHandler<OnLoadEventArgs> OnLoad;
		public class OnLoadEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnLoadEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public int Check { get; set; }
			public int Total { get; set; }
			public int Piece { get; set; }
			public int TotalPieces { get; set; }
		}
		public static string DatabasePath  = @"Data Source=..\..\..\\veekun-pokedex.sqlite";
		public static System.Data.SQLite.SQLiteConnection con { get; private set; }
        public static void ResetSqlConnection() { con = new System.Data.SQLite.SQLiteConnection(DatabasePath); }

		public static string LockFileStream (string filepath)
		{
			UnicodeEncoding uniEncoding = new UnicodeEncoding();
			//int recordNumber = 13;
			//int byteCount = uniEncoding.GetByteCount(recordNumber.ToString());
			string tempString;

			using (FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
			{
				// Write the original file data.
				//if (fileStream.Length == 0) //Get GUID
				//{
				//	fileStream.Write(uniEncoding.GetBytes(tempString),
				//		0, uniEncoding.GetByteCount(tempString));
				//}

				byte[] readText = new byte[fileStream.Length];

				//if ((tempString = Console.ReadLine()).Length == 0)
				//{
				//	break;
				//}
				try
				{
					fileStream.Seek(0, SeekOrigin.Begin);
					fileStream.Read(
						readText, 0, (int)fileStream.Length);
					tempString = new String(
						uniEncoding.GetChars(
						readText, 0, readText.Length));
				}

				// Catch the IOException generated if the 
				// specified part of the file is locked.
				//catch (IOException e)
				//{
				//	Console.WriteLine("{0}: The read " +
				//		"operation could not be performed " +
				//		"because the specified part of the " +
				//		"file is locked.",
				//		e.GetType().Name);
				//}
				finally
				{
					//xmlString = tempString;
				}
				return tempString;
			}
        }

        public static string _INTL(string message, params object[] param)
        {
            for (int i = 5; i > 1; i--)
                message.Replace($"{{{i}}}", $"{{{i - 1}}}");
            return string.Format(message, param);
		}

	
#region General purpose utilities
public bool _pbNextComb(int[] comb,int length) {
  int i=comb.Length-1;
  do {
    bool valid=true;
    for (int j = i; j < comb.Length; j++) {
      if (j==i) {
        comb[j]+=1;
      } else {
        comb[j]=comb[i]+(j-i);
      }
      if (comb[j]>=length) {
        valid=false;
        break;
      }
    }
    if (valid) return true;
    i-=1;
  } while (i>=0);
  return false;
}

/// <summary>
/// Iterates through the array and yields each combination of _num_ elements in
/// the array.
/// </summary>
/// <param name=""></param>
/// <param name="num"></param>
/// <returns></returns>
public IEnumerator<T[]> pbEachCombination<T>(T[] array,int num) {
  if (array.Length<num || num<=0) yield break; //return;
  if (array.Length==num) {
    yield return array;
    yield break; //return;
  } else if (num==1) {
    foreach (var x in array) {
      yield return new T[] { x };
    }
    yield break; //return;
  }
  int[] currentComb=new int[num];
  T[] arr=new T[num];
  for (int i = 0; i < num; i++) {
    currentComb[i]=i;
  }
  do {
    for (int i = 0; i < num; i++) {
      arr[i]=array[currentComb[i]];
    }
    yield return arr;
  } while (_pbNextComb(currentComb,array.Length));
}

/// <summary>
/// Returns a language ID
/// </summary>
//public void pbGetLanguage() {
//  getUserDefaultLangID=new Win32API("kernel32","GetUserDefaultLangID","","i"); //rescue null
//  int ret=0;
//  if (getUserDefaultLangID) {
//    ret=getUserDefaultLangID.call()&0x3FF;
//  }
//  if (ret==0) {		// Unknown
//    ret=MiniRegistry.get(MiniRegistry::HKEY_CURRENT_USER,
//       "Control Panel\\Desktop\\ResourceLocale","",0);
//    if (ret==0) ret=MiniRegistry.get(MiniRegistry::HKEY_CURRENT_USER,
//       "Control Panel\\International","Locale","0").to_i(16);
//    ret=ret&0x3FF;
//    if (ret==0 ) return 0;	// Unknown
//  }
//  if (ret==0x11) return 1;	// Japanese
//  if (ret==0x09) return 2;	// English
//  if (ret==0x0C) return 3;	// French
//  if (ret==0x10) return 4;	// Italian
//  if (ret==0x07) return 5;	// German
//  if (ret==0x0A) return 7;	// Spanish
//  if (ret==0x12) return 8;	// Korean
//  return 2; // Use 'English' by default
//}

/// <summary>
/// Converts a Celsius temperature to Fahrenheit.
/// </summary>
/// <param name="celsius"></param>
/// <returns></returns>
public double toFahrenheit(float celsius) {
  return Math.Round(celsius*9.0f/5.0f)+32;
}
 
/// <summary>
/// Converts a Fahrenheit temperature to Celsius.
/// </summary>
/// <param name="fahrenheit"></param>
/// <returns></returns>
public double toCelsius(float fahrenheit) {
  return Math.Round((fahrenheit-32)*5.0f/9.0f);
}
#endregion

#region Player-related utilities, random name generator
//public  bool pbChangePlayer(int id) {
//  if (id<0 || id>=8) return false;
//  meta=pbGetMetadata(0,MetadataPlayerA+id);
//  if (meta == null) return false;
//  if (Game.GameData.Trainer != null) Game.GameData.Trainer.trainertype=meta[0];
//  Game.GameData.GamePlayer.character_name=meta[1];
//  Game.GameData.GamePlayer.character_hue=0;
//  Game.GameData.Global.playerID=id;
//  if (Game.GameData.Trainer != null) Game.GameData.Trainer.metaID=id;
//}

//public  void pbGetPlayerGraphic() {
//  int id=Game.GameData.Global.playerID;
//  if (id<0 || id>=8) return "";
//  meta=pbGetMetadata(0,MetadataPlayerA+id);
//  if (meta == null) return "";
//  return pbPlayerSpriteFile(meta[0]);
//}

/*public  TrainerTypes pbGetPlayerTrainerType() {
  int id=Game.GameData.Global.playerID;
  if (id<0 || id>=8) return 0;
  meta=pbGetMetadata(0,MetadataPlayerA+id);
  if (meta == null) return 0;
  return meta[0];
}

public int pbGetTrainerTypeGender(TrainerTypes trainertype) {
  int? ret=2; // 2 = gender unknown
  //pbRgssOpen("Data/trainertypes.dat","rb"){|f|
  //   trainertypes=Marshal.load(f);
  //   if (!trainertypes[trainertype]) {
     if (!TrainerMetaData.ContainsKey(trainertype)) {
       ret=2;
     } else {
       //ret=trainertypes[trainertype][7];
       ret=TrainerMetaData[trainertype].Gender == true ? 1 : (TrainerMetaData[trainertype].Gender == false ? 0 : (int?)null);
       if (!ret.HasValue) ret=2;
     }
  //}
  return ret.Value;
}

public void pbTrainerName(string name=null,int outfit=0) {
  if (Game.GameData.Global.playerID<0) {
    pbChangePlayer(0);
  }
  TrainerTypes trainertype=pbGetPlayerTrainerType();
  string trname=name;
  Game.GameData.Trainer=new Combat.Trainer(trname,trainertype);
  Game.GameData.Trainer.outfit=outfit;
  if (trname==null) {
    trname=pbEnterPlayerName(Game._INTL("Your name?"),0,7);
    if (trname=="") {
      bool gender=pbGetTrainerTypeGender(trainertype) ;
      trname=pbSuggestTrainerName(gender);
    }
  }
  //Game.GameData.Trainer.name=trname;
  Game.GameData.Trainer=new Combat.Trainer(trname,trainertype);
  Game.GameData.Bag=new Character.PokemonBag();
  Game.GameData.PokemonTemp.begunNewGame=true;
}

public string pbSuggestTrainerName(bool gender) {
  string userName=pbGetUserName();
  //userName=userName.gsub(/\s+.*$/,""); // trim space
  if (userName.Length>0 && userName.Length<7) {
    //userName[0,1]=userName[0,1].upcase; //make first two characters cap
    userName[0]=userName.ToUpper()[0];
    userName[1]=userName.ToUpper()[1];
    return userName;
  }
  //userName=userName.gsub(/\d+$/,""); // trim numbers
  if (userName.Length>0 && userName.Length<7) {
    //userName[0,1]=userName[0,1].upcase; //make first two characters cap
    userName[0]=userName.ToUpper()[0];
    userName[1]=userName.ToUpper()[1];
    return userName;
  }
  string owner=MiniRegistry.get(MiniRegistry::HKEY_LOCAL_MACHINE,
     "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
     "RegisteredOwner","");
  owner=owner.Trim();//.gsub(/\s+.*$/,""); //trim spaces
  if (owner.Length>0 && owner.Length<7) {
    //owner[0,1]=owner[0,1].upcase; //make first two characters cap
    owner[0]=owner.ToUpper()[0];
    owner[1]=owner.ToUpper()[1];
    return owner;
  }
  return getRandomNameEx(gender,null,1,7);
}

public  string pbGetUserName() {
  int buffersize=100;
  string getUserName=""; //new Win32API('advapi32.dll','GetUserName','pp','i');
  int i = 0; do { //10.times;
    int[] size= new int[buffersize];//.pack("V")
    //string buffer="\0"*buffersize; //"0000..." x100 
    //if (getUserName.call(buffer,size)!=0) { 
    //  return buffer.gsub(/\0/,""); //replace all 0s with "", using regex
    //}
    buffersize+=200; i++;
  } while (i < 10);
  return "";
}*/

public  string getRandomNameEx(int type,int? variable,int? upper,int maxLength=100) {
  if (maxLength<=0) return "";
  string name="";
  int n = 0; do { //50.times
    name="";
    string[] formats=new string[0];
    switch (type) {
    case 0: // Names for males
      formats=new string[] { "F5", "BvE", "FE", "FE5", "FEvE" };
      break;
    case 1: // Names for females
      formats=new string[] { "vE6", "vEvE6", "BvE6", "B4", "v3", "vEv3", "Bv3" };
      break;
    case 2: // Neutral gender names
      formats=new string[] { "WE", "WEU", "WEvE", "BvE", "BvEU", "BvEvE" };
      break;
    default:
      return "";
    }
    string format=formats[Core.Rand.Next(formats.Length)];
    //format.scan(/./) {|c|
    foreach(Char c in format) {
       switch (c.ToString()) {
       case "c": // consonant
         string[] set=new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "x", "z" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "v": // vowel
         set=new string[] { "a", "a", "a", "e", "e", "e", "i", "i", "i", "o", "o", "o", "u", "u", "u" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "W": // beginning vowel
         set=new string[] { "a", "a", "a", "e", "e", "e", "i", "i", "i", "o", "o", "o", "u", "u", "u", "au", "au", "ay", "ay", 
            "ea", "ea", "ee", "ee", "oo", "oo", "ou", "ou" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "U": // ending vowel
         set=new string[] { "a", "a", "a", "a", "a", "e", "e", "e", "i", "i", "i", "o", "o", "o", "o", "o", "u", "u", "ay", "ay", "ie", "ie", "ee", "ue", "oo" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "B": // beginning consonant
         string[] set1=new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "l", "m", "n", "n", "p", "r", "r", "s", "s", "t", "t", "v", "w", "y", "z" };
         string[] set2=new string[] {
            "bl", "br", "ch", "cl", "cr", "dr", "fr", "fl", "gl", "gr", "kh", "kl", "kr", "ph", "pl", "pr", "sc", "sk", "sl",
            "sm", "sn", "sp", "st", "sw", "th", "tr", "tw", "vl", "zh" };
         name+=Core.Rand.Next(3)>0 ? set1[Core.Rand.Next(set1.Length)] : set2[Core.Rand.Next(set2.Length)];
         break;
       case "E": // ending consonant
         set1=new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "k", "l", "l", "m", "n", "n", "p", "r", "r", "s", "s", "t", "t", "v", "z" };
         set2=new string[] { "bb", "bs", "ch", "cs", "ds", "fs", "ft", "gs", "gg", "ld", "ls",
            "nd", "ng", "nk", "rn", "kt", "ks",
            "ms", "ns", "ph", "pt", "ps", "sk", "sh", "sp", "ss", "st", "rd",
            "rn", "rp", "rm", "rt", "rk", "ns", "th", "zh" };
         name+=Core.Rand.Next(3)>0 ? set1[Core.Rand.Next(set1.Length)] : set2[Core.Rand.Next(set2.Length)];
         break;
       case "f": // consonant and vowel
         set=new string[] { "iz", "us", "or" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "F": // consonant and vowel
         set=new string[] { "bo", "ba", "be", "bu", "re", "ro", "si", "mi", "zho", "se", "nya", "gru", "gruu", "glee", "gra", "glo", "ra", "do", "zo", "ri",
            "di", "ze", "go", "ga", "pree", "pro", "po", "pa", "ka", "ki", "ku", "de", "da", "ma", "mo", "le", "la", "li" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "2":
         set=new string[] { "c", "f", "g", "k", "l", "p", "r", "s", "t" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "3":
         set=new string[] { "nka", "nda", "la", "li", "ndra", "sta", "cha", "chie" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "4":
         set=new string[] { "una", "ona", "ina", "ita", "ila", "ala", "ana", "ia", "iana" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "5":
         set=new string[] { "e", "e", "o", "o", "ius", "io", "u", "u", "ito", "io", "ius", "us" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       case "6":
         set=new string[] { "a", "a", "a", "elle", "ine", "ika", "ina", "ita", "ila", "ala", "ana" };
         name+=set[Core.Rand.Next(set.Length)];
         break;
       }
    }
    if (name.Length<=maxLength) break; n++;
  } while (n < 50);
  name=name.Substring(0,maxLength);
  //switch (upper) {
  if (upper == 0) //case 0:
    name=name.ToUpper();
  //  break;
  if (upper == 1) //case 1:
    //name[0,1]=name[0,1].upcase;
    name.ReplaceAt(0,name.ToUpper()[0]);
    name.ReplaceAt(1,name.ToUpper()[1]);
  //  break;
  //}
  if (Game.GameData.GameVariables != null && variable != null) {
    Game.GameData.GameVariables[variable.Value]=name;
    if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
  }
  return name;
}

public  string getRandomName(int maxLength=100) {
  return getRandomNameEx(2,null,null,maxLength);
}
#endregion

#region Event timing utilities
public  void pbTimeEvent(int? variableNumber,int secs=86400) {
  if (variableNumber != null && variableNumber.Value>=0) {
    if (Game.GameData.GameVariables != null) {
      if (secs<0) secs=0;
      DateTime timenow=Game.pbGetTimeNow();
      Game.GameData.GameVariables[variableNumber.Value]=new float[] {timenow.Ticks,secs};
      if (Game.GameData.GameMap != null) Game.GameData.GameMap.refresh();
    }
  }
}

public  void pbTimeEventDays(int? variableNumber,int days=0) {
  if (variableNumber != null && variableNumber>=0) {
    if (Game.GameData.GameVariables != null) {
      if (days<0) days=0;
      DateTime timenow=Game.pbGetTimeNow();
      float time=timenow.Ticks;
      float expiry=(time%86400.0f)+(days*86400.0f);
      Game.GameData.GameVariables[variableNumber.Value]= new []{ time,expiry-time };
      if (Game.GameData.GameMap != null) Game.GameData.GameMap.refresh();
    }
  }
}

public  bool pbTimeEventValid(int? variableNumber) {
  bool retval=false;
  if (variableNumber != null && variableNumber>=0 && Game.GameData.GameVariables != null) {
    float[] value=(float[])Game.GameData.GameVariables[variableNumber.Value];
    if (value is Array) {
      DateTime timenow=Game.pbGetTimeNow();
      retval=(timenow.Ticks - value[0] > value[1]); // value[1] is age in seconds
      if (value[1]<=0) retval=false;	// zero age
    }
    if (!retval) {
      Game.GameData.GameVariables[variableNumber.Value]=0;
      if (Game.GameData.GameMap != null) Game.GameData.GameMap.refresh();
    }
  }
  return retval;
}
#endregion
   
#region General-purpose utilities with dependencies
// Similar to pbFadeOutIn, but pauses the music as it fades out.
// Requires scripts "Audio" (for bgm_pause) and "SpriteWindow" (for pbFadeOutIn).
//public  void pbFadeOutInWithMusic(zViewport) {
//  playingBGS=Game.GameData.GameSystem.getPlayingBGS;
//  playingBGM=Game.GameData.GameSystem.getPlayingBGM;
//  Game.GameData.GameSystem.bgm_pause(1.0);
//  Game.GameData.GameSystem.bgs_pause(1.0);
//  int pos=Game.GameData.GameSystem.bgm_position;
//  pbFadeOutIn(zViewport) {
//     yield;
//     Game.GameData.GameSystem.bgm_position=pos;
//     Game.GameData.GameSystem.bgm_resume(playingBGM);
//     Game.GameData.GameSystem.bgs_resume(playingBGS);
//  }
//}

// Gets the wave data from a file and displays an message if an error occurs.
// Can optionally delete the wave file (this is useful if the file was a
// temporary file created by a recording).
// Requires the script AudioUtilities
// Requires the script "PokemonMessages"
//public  void getWaveDataUI(string filename,bool deleteFile=false) {
//  error=getWaveData(filename);
//  if (deleteFile) {
//    try {
//      File.delete(filename);
//    } catch (Exception) { //Errno::EINVAL, Errno::EACCES, Errno::ENOENT;
//    }
//  }
//  switch (error) {
//  case 1:
//    Game.pbMessage(Game._INTL("The recorded data could not be found or saved."));
//    break;
//  case 2:
//    Game.pbMessage(Game._INTL("The recorded data was in an invalid format."));
//    break;
//  case 3:
//    Game.pbMessage(Game._INTL("The recorded data's format is not supported."));
//    break;
//  case 4:
//    Game.pbMessage(Game._INTL("There was no sound in the recording. Please ensure that a microphone is attached to the computer /and/ is ready."));
//    break;
//  default:
//    return error;
//  }
//  return null;
//}

// Starts recording, and displays a message if the recording failed to start.
// Returns true if successful, false otherwise
// Requires the script AudioUtilities
// Requires the script "PokemonMessages"
//public  bool beginRecordUI() {
//  int code=beginRecord;
//  switch (code) {
//  case 0:
//    return true;
//  case 256+66:
//    Game.pbMessage(Game._INTL("All recording devices are in use. Recording is not possible now."));
//    return false;
//  case 256+72:
//    Game.pbMessage(Game._INTL("No supported recording device was found. Recording is not possible."));
//    return false;
//  default:
//    string buffer="\0"*256;
//    MciErrorString.call(code,buffer,256);
//    Game.pbMessage(Game._INTL("Recording failed: {1}",buffer));//.gsub(/\x00/,"")
//    return false;
//  }
//}

//public  void pbHideVisibleObjects() {
//  visibleObjects=[];
//  ObjectSpace.each_object(Sprite){|o|
//     if (!o.disposed && o.visible) {
//       visibleObjects.Add(o);
//       o.visible=false;
//     }
//  }
//  ObjectSpace.each_object(Viewport){|o|
//     if (!pbDisposed(o) && o.visible) {
//       visibleObjects.Add(o);
//       o.visible=false;
//     }
//  }
//  ObjectSpace.each_object(Plane){|o|
//     if (!o.disposed && o.visible) {
//       visibleObjects.Add(o);
//       o.visible=false;
//     }
//  }
//  ObjectSpace.each_object(Tilemap){|o|
//     if (!o.disposed && o.visible) {
//       visibleObjects.Add(o);
//       o.visible=false;
//     }
//  }
//  ObjectSpace.each_object(Window){|o|
//     if (!o.disposed && o.visible) {
//       visibleObjects.Add(o);
//       o.visible=false;
//     }
//  }
//  return visibleObjects;
//}

//public  void pbShowObjects(visibleObjects) {
//  foreach (var o in visibleObjects) {
//    if (!pbDisposed(o)) {
//      o.visible=true;
//    }
//  }
//}

//public  void pbLoadRpgxpScene(scene) {
//  if (!Game.GameData.Scene is Scene_Map) return;
//  oldscene=Game.GameData.Scene;
//  Game.GameData.Scene=scene;
//  Graphics.freeze();
//  oldscene.disposeSpritesets();
//  visibleObjects=pbHideVisibleObjects();
//  Graphics.transition(15);
//  Graphics.freeze();
//  while (Game.GameData.Scene != null && !Game.GameData.Scene is Scene_Map) {
//    Game.GameData.Scene.main();
//  }
//  Graphics.transition(15);
//  Graphics.freeze();
//  oldscene.createSpritesets();
//  pbShowObjects(visibleObjects);
//  Graphics.transition(20);
//  Game.GameData.Scene=oldscene;
//}

// Gets the value of a variable.
public  object pbGet(int? id) {
  if (id == null || Game.GameData.GameVariables == null) return 0;
  return Game.GameData.GameVariables[id.Value];
}

// Sets the value of a variable.
public  void pbSet(int? id,object value) {
  if (id != null && id>=0) {
    if (Game.GameData.GameVariables != null) Game.GameData.GameVariables[id.Value]=value;
    if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
  }
}

/// <summary>
/// Runs a common event and waits until the common event is finished.
/// </summary>
/// Requires the script "PokemonMessages"
/// <param name="id"></param>
/// <returns></returns>
public  bool pbCommonEvent(int id) {
  if (id<0) return false;
  //Game_CommonEvent ce=Game.GameData.DataCommonEvents[id];
  //if (ce==null) return false;
  //List<> celist=ce.list;
  //Interpreter interp=new Interpreter();
  //interp.setup(celist,0);
  //do {
  //  Graphics.update();
  //  Input.update();
  //  interp.update();
  //  pbUpdateSceneMap();
  //} while (interp.running);
  return true;
}

//public  void pbExclaim(Avatar.Character @event,int id=EXCLAMATION_ANIMATION_ID,bool tinting=false) {
//  if (@event is Array) {
//    sprite=null;
//    done=[];
//    foreach (var i in @event) {
//      if (!done.Contains(i.id)) {
//        sprite=Game.GameData.Scene.spriteset.addUserAnimation(id,i.x,i.y,tinting);
//        done.Add(i.id);
//      }
//    }
//  } else {
//    sprite=Game.GameData.Scene.spriteset.addUserAnimation(id,@event.x,@event.y,tinting);
//  }
//  while (!sprite.disposed()) {
//    Graphics.update();
//    Input.update();
//    pbUpdateSceneMap();
//  }
//}

//public void pbNoticePlayer(Avatar.Character @event) {
//  if (!pbFacingEachOther(@event,Game.GameData.GamePlayer)) {
//    Game.pbExclaim(@event);
//  }
//  pbTurnTowardEvent(Game.GameData.GamePlayer,@event);
//  Game.pbMoveTowardPlayer(@event);
//}
#endregion

#region Loads music and sound effects
//public IAudioObject pbResolveAudioSE(string file) {
//  if (file == null) return null;
//  if (RTP.exists("Audio/SE/"+file,new string[] { "", ".wav", ".mp3", ".ogg" })) {
//    return RTP.getPath("Audio/SE/"+file,new string[] { "", ".wav", ".mp3", ".ogg" });
//  }
//  return null;
//}
//
//public void pbCryFrameLength(Pokemon pokemon,float? pitch=null) {
//  if (!pokemon.IsNotNullOrNone()) return 0;
//  if (pitch == null) pitch=100;
//  pitch=pitch/100;
//  if (pitch<=0) return 0;
//  float playtime=0.0f;
//  if (pokemon is Numeric) {
//    pkmnwav=pbResolveAudioSE(pbCryFile(pokemon));
//    if (pkmnwav != null) playtime=getPlayTime(pkmnwav);
//  } else if (!pokemon.isEgg) {
//    if (pokemon.respond_to("chatter") && pokemon.chatter) {
//      playtime=pokemon.chatter.time;
//      pitch=1.0f;
//    } else {
//      pkmnwav=pbResolveAudioSE(pbCryFile(pokemon));
//      if (pkmnwav) playtime=getPlayTime(pkmnwav);
//    }
//  }
//  playtime/=pitch; // sound is lengthened the lower the pitch
//  //  4 is added to provide a buffer between sounds
//  return (playtime*Graphics.frame_rate).ceil+4;
//}
//
//public void pbPlayCry(Monster.Pokemon pokemon,int volume=90,float? pitch=null) {
//  if (!pokemon) return;
//  if (pokemon is Numeric) {
//    pkmnwav=pbCryFile(pokemon);
//    if (pkmnwav) {
//      pbSEPlay(new RPG.AudioFile(pkmnwav,volume,pitch != null ? pitch : 100)); //rescue null
//    }
//  } else if (!pokemon.isEgg) {
//    if (pokemon.respond_to("chatter") && pokemon.chatter) {
//      pokemon.chatter.play;
//    } else {
//      pkmnwav=pbCryFile(pokemon);
//      if (pkmnwav) {
//        pbSEPlay(new RPG.AudioFile(pkmnwav,volume,
//           pitch != null ? pitch : (pokemon.HP*25/pokemon.TotalHP)+75)); //rescue null
//      }
//    }
//  }
//}
//
//public void pbCryFile(Monster.Pokemon pokemon) {
//  if (!pokemon) return null;
//  if (pokemon is Numeric) {
//    filename=string.Format("Cries/{0}Cry",getConstantName(PBSpecies,pokemon)); //rescue null
//    if (!pbResolveAudioSE(filename)) filename=string.Format("Cries/%03dCry",pokemon);
//    if (pbResolveAudioSE(filename)) return filename;
//  } else if (!pokemon.isEgg) {
//    filename=string.Format("Cries/{0}Cry_{1}",getConstantName(PBSpecies,pokemon.Species),(pokemon.form)); //rescue 0 rescue null
//    if (!pbResolveAudioSE(filename)) filename=string.Format("Cries/{0}Cry_{1}",pokemon.Species,(pokemon.form)); //rescue 0
//    if (!pbResolveAudioSE(filename)) {
//      filename=string.Format("Cries/{0}Cry",getConstantName(PBSpecies,pokemon.Species)) rescue null;
//    }
//    if (!pbResolveAudioSE(filename)) filename=string.Format("Cries/{0}Cry",pokemon.Species);
//    if (pbResolveAudioSE(filename)) return filename;
//  }
//  return null;
//}
//
//public IAudioObject pbGetWildBattleBGM(species) {
//  if (Game.GameData.Global.nextBattleBGM) {
//    return Game.GameData.Global.nextBattleBGM.clone();
//  }
//  ret=null;
//  if (ret == null && Game.GameData.GameMap) {
//    //  Check map-specific metadata
//    music=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataMapWildBattleBGM);
//    if (music != null && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (ret == null {
//    //  Check global metadata
//    music=pbGetMetadata(0,MetadataWildBattleBGM);
//    if (music != null && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (ret == null) ret=pbStringToAudioFile("002-Battle02");
//  return ret;
//}
//
//public IAudioObject pbGetWildVictoryME() {
//  if (Game.GameData.Global.nextBattleME) {
//    return Game.GameData.Global.nextBattleME.clone();
//  }
//  ret=null;
//  if (ret == null && Game.GameData.GameMap) {
//    //  Check map-specific metadata
//    music=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataMapWildVictoryME);
//    if (music != null && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (ret == null) {
//    //  Check global metadata
//    music=pbGetMetadata(0,MetadataWildVictoryME);
//    if (music != null && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (ret == null) ret=pbStringToAudioFile("001-Victory01");
//  ret.name="../../Audio/ME/"+ret.name;
//  return ret;
//}
//
//public IAudioObject pbPlayTrainerIntroME(trainertype) {
//  pbRgssOpen("Data/trainertypes.dat","rb"){|f|
//     trainertypes=Marshal.load(f);
//     if (trainertypes[trainertype]) {
//       bgm=trainertypes[trainertype][6];
//       if (bgm && bgm!="") {
//         bgm=pbStringToAudioFile(bgm);
//         pbMEPlay(bgm);
//         return;
//       }
//     }
//  }
//}
//
//public IAudioObject pbGetTrainerBattleBGM(Combat.Trainer[] trainer) { // can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
//  if (Game.GameData.Global.nextBattleBGM) {
//    return Game.GameData.Global.nextBattleBGM.clone();
//  }
//  music=null;
//  pbRgssOpen("Data/trainertypes.dat","rb"){|f|
//     trainertypes=Marshal.load(f);
//     if (!trainer is Array) {
//       trainerarray= new []{ trainer };
//     } else {
//       trainerarray=trainer;
//     }
//     for (int i = 0; i < trainerarray.Length; i++) {
//       trainertype=trainerarray[i].trainertype;
//       if (trainertypes[trainertype]) {
//         music=trainertypes[trainertype][4];
//       }
//     }
//  }
//  ret=null;
//  if (music != null && music!="") {
//    ret=pbStringToAudioFile(music);
//  }
//  if (ret == null && Game.GameData.GameMap) {
//    //  Check map-specific metadata
//    music=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataMapTrainerBattleBGM);
//    if (music && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (!ret) {
//    //  Check global metadata
//    music=pbGetMetadata(0,MetadataTrainerBattleBGM);
//    if (music && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (!ret) ret=pbStringToAudioFile("005-Boss01");
//  return ret;
//}
//
//public IAudioObject pbGetTrainerBattleBGMFromType(trainertype) {
//  if (Game.GameData.Global.nextBattleBGM) {
//    return Game.GameData.Global.nextBattleBGM.clone();
//  }
//  music=null;
//  pbRgssOpen("Data/trainertypes.dat","rb"){|f|
//    trainertypes=Marshal.load(f);
//    if (trainertypes[trainertype]) {
//      music=trainertypes[trainertype][4];
//    }
//  }
//  ret=null;
//  if (music && music!="") {
//    ret=pbStringToAudioFile(music);
//  }
//  if (!ret && Game.GameData.GameMap) {
////  Check map-specific metadata
//    music=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataMapTrainerBattleBGM);
//    if (music && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (!ret) {
////  Check global metadata
//    music=pbGetMetadata(0,MetadataTrainerBattleBGM);
//    if (music && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (!ret) ret=pbStringToAudioFile("005-Boss01");
//  return ret;
//}
//
//public IAudioObject pbGetTrainerVictoryME(trainer) { // can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
//  if (Game.GameData.Global.nextBattleME) {
//    return Game.GameData.Global.nextBattleME.clone();
//  }
//  music=null;
//  pbRgssOpen("Data/trainertypes.dat","rb"){|f|
//     trainertypes=Marshal.load(f);
//     if (!trainer is Array) {
//       trainerarray= new []{ trainer };
//     } else {
//       trainerarray=trainer;
//     }
//     for (int i = 0; i < trainerarray.Length; i++) {
//       trainertype=trainerarray[i].trainertype;
//       if (trainertypes[trainertype]) {
//         music=trainertypes[trainertype][5];
//       }
//     }
//  }
//  ret=null;
//  if (music && music!="") {
//    ret=pbStringToAudioFile(music);
//  }
//  if (!ret && Game.GameData.GameMap) {
////  Check map-specific metadata
//    music=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataMapTrainerVictoryME);
//    if (music && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (!ret) {
////  Check global metadata
//    music=pbGetMetadata(0,MetadataTrainerVictoryME);
//    if (music && music!="") {
//      ret=pbStringToAudioFile(music);
//    }
//  }
//  if (!ret) ret=pbStringToAudioFile("001-Victory01");
//  ret.name="../../Audio/ME/"+ret.name;
//  return ret;
//}
#endregion

#region Creating and storing Pokémon
public  bool pbBoxesFull() {
  return Game.GameData.Trainer == null || (Game.GameData.Trainer.party.Length==Game.GameData.Features.LimitPokemonPartySize && Game.GameData.PokemonStorage.full);
}

public  void pbNickname(Monster.Pokemon pokemon) {
  string speciesname=pokemon.Species.ToString(TextScripts.Name);
  if (UI.pbConfirmMessage(Game._INTL("Would you like to give a nickname to {1}?",speciesname))) {
    string helptext=Game._INTL("{1}'s nickname?",speciesname);
    string newname=UI.pbEnterPokemonName(helptext,0,Pokemon.NAMELIMIT,"",pokemon);
    //if (newname!="") pokemon.Name=newname;
    if (newname!="") pokemon.SetNickname(newname);
  }
}

public  void pbStorePokemon(Pokemon pokemon) {
  if (pbBoxesFull()) {
    Game.pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
    Game.pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
    return;
  }
  pokemon.RecordFirstMoves();
  if (Game.GameData.Trainer.party.Length< Game.GameData.Features.LimitPokemonPartySize) {
    //ToDo: Change to `.Add(Pokemon)`
    Game.GameData.Trainer.party[Game.GameData.Trainer.party.Length]=pokemon;
  } else {
    int oldcurbox=Game.GameData.PokemonStorage.currentBox;
    int storedbox=Game.GameData.PokemonStorage.pbStoreCaught(pokemon);
    string curboxname=Game.GameData.PokemonStorage[oldcurbox].name;
    string boxname=Game.GameData.PokemonStorage[storedbox].name;
    string creator=null;
    if (Game.GameData.Global.seenStorageCreator) creator=Game.GameData.pbGetStorageCreator();
    if (storedbox!=oldcurbox) {
      if (!string.IsNullOrEmpty(creator)) {
        Game.pbMessage(Game._INTL(@"Box ""{1}"" on {2}'s PC was full.\1",curboxname,creator));
      } else {
        Game.pbMessage(Game._INTL(@"Box ""{1}"" on someone's PC was full.\1",curboxname));
      }
      Game.pbMessage(Game._INTL("{1} was transferred to box \"{2}.\"",pokemon.Name,boxname));
    } else {
      if (!string.IsNullOrEmpty(creator)) {
        Game.pbMessage(Game._INTL(@"{1} was transferred to {2}'s PC.\1",pokemon.Name,creator));
      } else {
        Game.pbMessage(Game._INTL(@"{1} was transferred to someone's PC.\1",pokemon.Name));
      }
      Game.pbMessage(Game._INTL("It was stored in box \"{1}.\"",boxname));
    }
  }
}

public  void pbNicknameAndStore(Pokemon pokemon) {
  if (pbBoxesFull()) {
    Game.pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
    Game.pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
    return;
  }
  Game.GameData.Trainer.seen[pokemon.Species]=true;
  Game.GameData.Trainer.owned[pokemon.Species]=true;
  pbNickname(pokemon);
  pbStorePokemon(pokemon);
}

public  bool pbAddPokemon(Pokemons? pkmn,int? level=null,bool seeform=true) {
  Monster.Pokemon pokemon = null;
  //if (!pokemon.IsNotNullOrNone() || Game.GameData.Trainer == null) return false;
  if (pkmn == null || Game.GameData.Trainer == null) return false;
  if (pbBoxesFull()) {
    Game.pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
    Game.pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
    return false;
  }
  //if (pokemon is String || pokemon is Symbol) {
  //  pokemon=getID(PBSpecies,pokemon);
  //}
  if (level != null) { //pokemon is Integer && level is int
    //pokemon=new Pokemon(pokemon.Species,level:(byte)level.Value,original:Game.GameData.Trainer);
    pokemon=new Monster.Pokemon(pkmn.Value,level:(byte)level.Value,original:Game.GameData.Trainer);
  }
  //string speciesname=pokemon.Species.ToString(TextScripts.Name);
  string speciesname=pkmn.Value.ToString(TextScripts.Name);
  Game.pbMessage(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Game.GameData.Trainer.name,speciesname));
  pbNicknameAndStore(pokemon);
  if (seeform) pbSeenForm(pokemon);
  return true;
}

public  bool pbAddPokemonSilent(Pokemon pokemon,int? level=null,bool seeform=true) {
  if (!pokemon.IsNotNullOrNone() || pbBoxesFull() || Game.GameData.Trainer == null) return false;
  //if (pokemon is String || pokemon is Symbol) {
  //  pokemon=getID(PBSpecies,pokemon);
  //}
  if (level is int) { //pokemon is Integer && 
    pokemon=new Pokemon(pokemon.Species,level:(byte)level.Value,original:Game.GameData.Trainer);
  }
  Game.GameData.Trainer.seen[pokemon.Species]=true;
  Game.GameData.Trainer.owned[pokemon.Species]=true;
  if (seeform) pbSeenForm(pokemon);
  pokemon.RecordFirstMoves();
  if (Game.GameData.Trainer.party.Length<Game.GameData.Features.LimitPokemonPartySize) {
    //ToDo: Change to `.Add(Pokemon)`
    Game.GameData.Trainer.party[Game.GameData.Trainer.party.Length]=pokemon;
  } else {
    Game.GameData.PokemonStorage.pbStoreCaught(pokemon);
  }
  return true;
}

public  bool pbAddToParty(Pokemon pokemon,int? level=null,bool seeform=true) {
  if (!pokemon.IsNotNullOrNone() || Game.GameData.Trainer == null || Game.GameData.Trainer.party.Length>=Game.GameData.Features.LimitPokemonPartySize) return false;
  //if (pokemon is String || pokemon is Symbol) {
  //  pokemon=getID(PBSpecies,pokemon);
  //}
  if (level is int) { //pokemon is Integer && 
    //ToDo: Modify exisiting variable instead of generating new
    pokemon=new Pokemon(pokemon.Species,level:(byte)level.Value,original:Game.GameData.Trainer);
  }
  string speciesname=pokemon.Species.ToString(TextScripts.Name);
  Game.pbMessage(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Game.GameData.Trainer.name,speciesname));
  pbNicknameAndStore(pokemon);
  if (seeform) pbSeenForm(pokemon);
  return true;
}

public  bool pbAddToPartySilent(Pokemon pokemon,int? level=null,bool seeform=true) {
  if (!pokemon.IsNotNullOrNone() || Game.GameData.Trainer == null || Game.GameData.Trainer.party.Length>=Game.GameData.Features.LimitPokemonPartySize) return false;
  //if (pokemon is String || pokemon is Symbol) {
  //  pokemon=getID(PBSpecies,pokemon);
  //}
  if (level is int) { //pokemon is Integer && 
    pokemon=new Pokemon(pokemon.Species,level:(byte)level.Value,original:Game.GameData.Trainer);
  }
  Game.GameData.Trainer.seen[pokemon.Species]=true;
  Game.GameData.Trainer.owned[pokemon.Species]=true;
  if (seeform) pbSeenForm(pokemon);
  pokemon.RecordFirstMoves();
  Game.GameData.Trainer.party[Game.GameData.Trainer.party.Length]=pokemon;
  return true;
}

//public  bool pbAddForeignPokemon(Pokemon pokemon,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true) {
//  if (!pokemon.IsNotNullOrNone() || Game.GameData.Trainer == null || Game.GameData.Trainer.party.Length>=Game.GameData.Features.LimitPokemonPartySize) return false;
//  //if (pokemon is String || pokemon is Symbol) {
//  //  pokemon=getID(PBSpecies,pokemon);
//  //}
//  if (level is int) { //pokemon is Integer && 
//    pokemon=new PokeBattle_Pokemon(pokemon,level,Game.GameData.Trainer);
//  }
//  //Set original trainer to a foreign one (if ID isn't already foreign)
//  if (pokemon.trainerID==Game.GameData.Trainer.id) {
//    pokemon.trainerID=Game.GameData.Trainer.getForeignID;
//    if (!string.IsNullOrEmpty(ownerName)) pokemon.ot=ownerName;
//    pokemon.otgender=ownerGender;
//  }
//  //Set nickname
//  if (!string.IsNullOrEmpty(nickname)) pokemon.Name=nickname[0,10];
//  //Recalculate stats
//  pokemon.calcStats();
//  if (ownerName != null) {
//    Game.pbMessage(Game._INTL("{1} received a Pokémon from {2}.\\se[PokemonGet]\1",Game.GameData.Trainer.name,ownerName));
//  } else {
//    Game.pbMessage(Game._INTL("{1} received a Pokémon.\\se[PokemonGet]\1",Game.GameData.Trainer.name));
//  }
//  pbStorePokemon(pokemon);
//  Game.GameData.Trainer.seen[pokemon.Species]=true;
//  Game.GameData.Trainer.owned[pokemon.Species]=true;
//  if (seeform) pbSeenForm(pokemon);
//  return true;
//}
public  bool pbGenerateEgg(Pokemon pokemon,string text="") {
  if (!pokemon.IsNotNullOrNone() || Game.GameData.Trainer == null || Game.GameData.Trainer.party.Length>=Game.GameData.Features.LimitPokemonPartySize) return false;
  //if (pokemon is String || pokemon is Symbol) {
  //  pokemon=getID(PBSpecies,pokemon);
  //}
  //if (pokemon is int) {
    //pokemon=new Pokemon(pokemon.Species,level:Core.EGGINITIALLEVEL,orignal:Game.GameData.Trainer);
    pokemon=new Pokemon(pokemon.Species,isEgg:true);
  //}
  // Get egg steps
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,pokemon.Species,21);
  //int eggsteps=dexdata.fgetw();
  //dexdata.close();
  // Set egg's details
  //pokemon.Name=Game._INTL("Egg");
  //pokemon.eggsteps=eggsteps;
  //pokemon.obtainText=text;
  pokemon.calcStats();
  // Add egg to party
  Game.GameData.Trainer.party[Game.GameData.Trainer.party.Length]=pokemon;
  return true;
}

public  bool pbRemovePokemonAt(int index) {
  if (index<0 || Game.GameData.Trainer == null || index>=Game.GameData.Trainer.party.Length) return false;
  bool haveAble=false;
  for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
    if (i==index) continue;
    if (Game.GameData.Trainer.party[i].HP>0 && !Game.GameData.Trainer.party[i].isEgg) haveAble=true;
  }
  if (!haveAble) return false;
  //Game.GameData.Trainer.party.delete_at(index);
  Game.GameData.Trainer.party[index] = new Pokemon();
  return true;
}

public  void pbSeenForm(Pokemon poke,int gender=0,int form=0) {
  //if (Game.GameData.Trainer.formseen==null) Game.GameData.Trainer.formseen=new[];
  //if (Game.GameData.Trainer.formlastseen==null) Game.GameData.Trainer.formlastseen=new[];
  //if (poke is String || poke is Symbol) {
  //  poke=getID(PBSpecies,poke);
  //}
  Pokemons species = Pokemons.NONE;
  if (poke is Pokemon) {
    //gender=poke.Gender;
    form=poke.FormId; //rescue 0
    species=poke.Species;
  //} else {
  //  species=poke;
  }
  if (species<=0) return; //!species || 
  if (gender>1) gender=0;
  //string formnames=pbGetMessage(MessageTypes.FormNames,species);
  //if (string.IsNullOrEmpty(formnames)) form=0;
  //if (Game.GameData.Trainer.formseen[species] == null) Game.GameData.Trainer.formseen[species]=new int?[0][] { new int?[0],new int?[0] };
  //Game.GameData.Trainer.formseen[species][gender][form]=true;
  //if (Game.GameData.Trainer.formlastseen[species] == null) Game.GameData.Trainer.formlastseen[species]=new [];
  //if (Game.GameData.Trainer.formlastseen[species]==[]) Game.GameData.Trainer.formlastseen[species]= new []{ gender,form };
  if(Game.GameData.Player.Pokedex[(int)species, 2] < 0)
    Game.GameData.Player.Pokedex[(int)species,2] = (byte)form; 
}
#endregion

#region Analysing Pokémon
// Heals all Pokémon in the party.
public  void pbHealAll() {
  if (Game.GameData.Trainer == null) return;
  foreach (Pokemon i in Game.GameData.Trainer.party) {
    i.Heal();
  }
}

// Returns the first unfainted, non-egg Pokémon in the player's party.
public  Pokemon pbFirstAblePokemon(int variableNumber) {
  for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
    Pokemon p=Game.GameData.Trainer.party[i];
    if (p != null && !p.isEgg && p.HP>0) {
      pbSet(variableNumber,i);
      return Game.GameData.Trainer.party[i];
    }
  }
  pbSet(variableNumber,-1);
  return null;
}

// Checks whether the player would still have an unfainted Pokémon if the
// Pokémon given by _pokemonIndex_ were removed from the party.
public  bool pbCheckAble(int pokemonIndex) {
  for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
    Pokemon p=Game.GameData.Trainer.party[i];
    if (i==pokemonIndex) continue;
    if (p.IsNotNullOrNone() && !p.isEgg && p.HP>0) return true;
  }
  return false;
}

// Returns true if there are no usable Pokémon in the player's party.
public  bool pbAllFainted() {
  foreach (var i in Game.GameData.Trainer.party) {
    if (!i.isEgg && i.HP>0) return false;
  }
  return true;
}

public  double pbBalancedLevel(Pokemon[] party) {
  if (party.Length==0) return 1;
//  Calculate the mean of all levels
  int sum=0;
  foreach (var p in party) { sum+=p.Level; }
  if (sum==0) return 1;
  double average=(double)sum/(double)party.Length;
//  Calculate the standard deviation
  double varianceTimesN=0;
  for (int i = 0; i < party.Length; i++) {
    double deviation=party[i].Level-average;
    varianceTimesN+=deviation*deviation;
  }
//  Note: This is the "population" standard deviation calculation, since no
//  sample is being taken
  double stdev=Math.Sqrt(varianceTimesN/party.Length);
  double mean=0;
  List<double> weights=new List<double>();
//  Skew weights according to standard deviation
  for (int i = 0; i < party.Length; i++) {
    double weight=(double)party[i].Level/(double)sum;
    if (weight<0.5f) {
      weight-=(stdev/(double)Core.MAXIMUMLEVEL);
      if (weight<=0.001f) weight=0.001f;
    } else {
      weight+=(stdev/(double)Core.MAXIMUMLEVEL);
      if (weight>=0.999f) weight=0.999f;
    }
    weights.Add(weight);
  }
  double weightSum=0;
  foreach (var weight in weights) { weightSum += weight; }
//  Calculate the weighted mean, assigning each weight to each level's
//  contribution to the sum
  for (int i = 0; i < party.Length; i++) {
    mean+=party[i].Level*weights[i];
  }
  mean/=weightSum;
//  Round to nearest number
  mean=Math.Round(mean);
//  Adjust level to minimum
  if (mean<1) mean=1;
//  Add 2 to the mean to challenge the player
  mean+=2;
//  Adjust level to maximum
  if (mean>Core.MAXIMUMLEVEL) mean=Core.MAXIMUMLEVEL;
  return mean;
}

/// <summary>
/// Returns the Pokémon's size in millimeters.
/// </summary>
/// <param name="pokemon"></param>
/// <returns></returns>
public  int pbSize(Pokemon pokemon) {
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,pokemon.Species,33);
  //int baseheight=dexdata.fgetw(); // Gets the base height in tenths of a meter
  float baseheight=Game.PokemonData[pokemon.Species].Height;
  //dexdata.close();
  int hpiv=pokemon.IV[0]&15;
  int ativ=pokemon.IV[1]&15;
  int dfiv=pokemon.IV[2]&15;
  int spiv=pokemon.IV[3]&15;
  int saiv=pokemon.IV[4]&15;
  int sdiv=pokemon.IV[5]&15;
  int m=pokemon.PersonalId&0xFF;
  int n=(pokemon.PersonalId>>8)&0xFF;
  double s=(((ativ^dfiv)*hpiv)^m)*256+(((saiv^sdiv)*spiv)^n);
  int[] xyz=new int[0];
  if (s<10) {
    xyz= new int[]{ 290,1,0 };
  } else if (s<110) {
    xyz= new int[]{ 300,1,10 };
  } else if (s<310) {
    xyz= new int[]{ 400,2,110 };
  } else if (s<710) {
    xyz= new int[]{ 500,4,310 };
  } else if (s<2710) {
    xyz= new int[]{ 600,20,710 };
  } else if (s<7710) {
    xyz= new int[]{ 700,50,2710 };
  } else if (s<17710) {
    xyz= new int[]{ 800,100,7710 };
  } else if (s<32710) {
    xyz= new int[]{ 900,150,17710 };
  } else if (s<47710) {
    xyz= new int[]{ 1000,150,32710 };
  } else if (s<57710) {
    xyz= new int[]{ 1100,100,47710 };
  } else if (s<62710) {
    xyz= new int[]{ 1200,50,57710 };
  } else if (s<64710) {
    xyz= new int[]{ 1300,20,62710 };
  } else if (s<65210) {
    xyz= new int[]{ 1400,5,64710 };
  } else if (s<65410) {
    xyz= new int[]{ 1500,2,65210 };
  } else {
    xyz= new int[]{ 1700,1,65510 };
  }
  return (int)Math.Floor(Math.Floor((s-xyz[2])/xyz[1]+xyz[0])*baseheight/10);
}

/// <summary>
/// Returns true if the given species can be legitimately obtained as an egg.
/// </summary>
/// <param name="species"></param>
/// <returns></returns>
public  bool pbHasEgg (Pokemons species) {
  //if (species is String || species is Symbol) {
  //  species=getID(PBSpecies,species);
  //}
  //Pokemons[][] evospecies=pbGetEvolvedFormData(species); //Game.PokemonEvolutionsData[species][0].
  //Game.PokemonData[species].IsBaby
  //Pokemons compatspecies=(evospecies != null && evospecies[0] != null) ? evospecies[0][2] : species;
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,compatspecies,31);
  EggGroups compat1=Game.PokemonData[species].EggGroup[0]; //dexdata.fgetb();   // Get egg group 1 of this species
  EggGroups compat2=Game.PokemonData[species].EggGroup[1]; //dexdata.fgetb();   // Get egg group 2 of this species
  //dexdata.close();
  if (compat1 == EggGroups.DITTO ||
                  compat1 == EggGroups.UNDISCOVERED ||
                  compat2 == EggGroups.DITTO ||
                  compat2 == EggGroups.UNDISCOVERED) return false;
  Pokemons baby=Evolution.pbGetBabySpecies(species);
  if (species==baby) return true;	// Is a basic species
  baby=Evolution.pbGetBabySpecies(species,0,0);
  if (species==baby) return true;	// Is an egg species without incense
  return false;
}
#endregion

#region Look through Pokémon in storage, choose a Pokémon in the party
/// <summary>
/// Yields every Pokémon/egg in storage in turn.
/// </summary>
/// <returns></returns>
public  IEnumerable<KeyValuePair<Pokemon,int>> pbEachPokemon() {
  for (int i = -1; i < Game.GameData.PokemonStorage.maxBoxes; i++) {
    for (int j = 0; j < Game.GameData.PokemonStorage.maxPokemon(i); j++) {
      Pokemon poke=Game.GameData.PokemonStorage[i][j];
      if (poke.IsNotNullOrNone()) yield return new KeyValuePair<Pokemon,int>(poke,i);
    }
  }
}

/// <summary>
/// Yields every Pokémon in storage in turn.
/// </summary>
/// <returns></returns>
public  IEnumerable<KeyValuePair<Pokemon,int>> pbEachNonEggPokemon() {
  //pbEachPokemon(){|pokemon,box|
  foreach(KeyValuePair<Pokemon,int> pokemon in pbEachPokemon()) {
     //if (!pokemon.isEgg) yield return (pokemon,box);
     if (!pokemon.Key.isEgg) yield return pokemon; //(pokemon.Key,pokemon.Value);
  }
}

/// <summary>
/// Choose a Pokémon/egg from the party.
/// Stores result in variable _variableNumber_ and the chosen Pokémon's name in
/// variable _nameVarNumber_; result is -1 if no Pokémon was chosen
/// </summary>
/// <param name="variableNumber"></param>
/// <param name="nameVarNumber"></param>
/// <param name="ableProc">an array of which pokemons are affected</param>
/// <param name="allowIneligible"></param>
/// Supposed to return a value of pokemon chosen by player... as an int...
/// ToDo: Instead of assigning value to variable, change void to return int
public  void pbChoosePokemon(int variableNumber,int nameVarNumber,Func<Pokemon, bool> ableProc=null,bool allowIneligible=false) {
  int chosen=0;
  Game.UI.pbFadeOutIn(99999, () => {
     IPokemonScreen_Scene scene=PokemonScreenScene; //new PokemonScreen_Scene();
     IPokemonScreen screen=PokemonScreen.initialize(scene,Game.GameData.Trainer.party); //new PokemonScreen(scene,Game.GameData.Trainer.party);
     if (ableProc != null) {
       chosen=screen.pbChooseAblePokemon(ableProc,allowIneligible);
     } else {
       screen.pbStartScene(Game._INTL("Choose a Pokémon."),false);
       chosen=screen.pbChoosePokemon();
       screen.pbEndScene();
     }
  });
  pbSet(variableNumber,chosen);
  if (chosen>=0) {
    pbSet(nameVarNumber,Game.GameData.Trainer.party[chosen].Name);
  } else {
    pbSet(nameVarNumber,"");
  }
}

public  void pbChooseNonEggPokemon(int variableNumber,int nameVarNumber) {
  //pbChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
  pbChoosePokemon(variableNumber,nameVarNumber, poke => 
     !poke.isEgg
  );
}

public  void pbChooseAblePokemon(int variableNumber,int nameVarNumber) {
  //pbChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
  pbChoosePokemon(variableNumber,nameVarNumber, poke =>
     !poke.isEgg && poke.HP>0
  );
}

public  void pbChoosePokemonForTrade(int variableNumber,int nameVarNumber,Pokemons wanted) {
  //pbChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
  pbChoosePokemon(variableNumber, nameVarNumber, poke =>
     //if (wanted is String || wanted is Symbol) {
     //  wanted=getID(PBSpecies,wanted);
     //}
     !poke.isEgg && !poke.isShadow && poke.Species==wanted ///rescue false
  );
}
#endregion

#region Checks through the party for something
public  bool pbHasSpecies (Pokemons species) {
  //if (species is String || species is Symbol) {
  //  species=getID(PBSpecies,species);
  //}
  foreach (var pokemon in Game.GameData.Trainer.party) {
    if (pokemon.isEgg) continue;
    if (pokemon.Species==species) return true;
  }
  return false;
}

public  bool pbHasFatefulSpecies (Pokemons species) {
  //if (species is String || species is Symbol) {
  //  species=getID(PBSpecies,species);
  //}
  foreach (Pokemon pokemon in Game.GameData.Trainer.party) {
    if (pokemon.isEgg) continue;
    if (pokemon.Species==species && pokemon.ObtainedMode==Pokemon.ObtainedMethod.FATEFUL_ENCOUNTER) return true;
  }
  return false;
}

public  bool pbHasType (Types type) {
  //if (type is String || type is Symbol) {
  //  type=getID(PBTypes,type);
  //}
  foreach (var pokemon in Game.GameData.Trainer.party) {
    if (pokemon.isEgg) continue;
    if (pokemon.hasType(type)) return true;
  }
  return false;
}

// Checks whether any Pokémon in the party knows the given move, and returns
// the index of that Pokémon, or null if no Pokémon has that move.
public  Monster.Pokemon pbCheckMove(Moves move) {
  //move=getID(PBMoves,move);
  if (move<=0) return null; //!move ||
  foreach (Monster.Pokemon i in Game.GameData.Trainer.party) {
    if (i.isEgg) continue;
    foreach (Attack.Move j in i.moves) {
      if (j.MoveId==move) return i;
    }
  }
  return null;
}
#endregion

#region Regional and National Pokédexes
// Gets the Regional Pokédex number of the national species for the specified
// Regional Dex.  The parameter "region" is zero-based.  For example, if two
// regions are defined, they would each be specified as 0 and 1.
public  int pbGetRegionalNumber(int region,Pokemons nationalSpecies) {
  if (nationalSpecies<=0 || (int)nationalSpecies>Game.PokemonData.Count) {
//  Return 0 if national species is outside range
    return 0;
  }
  //pbRgssOpen("Data/regionals.dat","rb"){|f|
  //   int numRegions=f.fgetw;
  //   int numDexDatas=f.fgetw;
  //   if (region>=0 && region<numRegions) {
  //     f.pos=4+region*numDexDatas*2;
  //     f.pos+=nationalSpecies*2;
  //     return f.fgetw;
  //  }
  //}
  return 0;
}

// Gets the National Pokédex number of the specified species and region.  The
// parameter "region" is zero-based.  For example, if two regions are defined,
// they would each be specified as 0 and 1.
public  int pbGetNationalNumber(int region,Pokemons regionalSpecies) {
  //pbRgssOpen("Data/regionals.dat","rb"){|f|
  //   int numRegions=f.fgetw;
  //   int numDexDatas=f.fgetw;
  //   if (region>=0 && region<numRegions) {
  //     f.pos=4+region*numDexDatas*2;
  //     //  "i" specifies the national species
  //     for (int i = 0; i < numDexDatas; i++) {
  //       int regionalNum=f.fgetw;
  //       if (regionalNum==regionalSpecies) return i;
  //     }
  //   }
  //}
  return 0;
}

// Gets an array of all national species within the given Regional Dex, sorted by
// Regional Dex number.  The number of items in the array should be the
// number of species in the Regional Dex plus 1, since index 0 is considered
// to be empty.  The parameter "region" is zero-based.  For example, if two
// regions are defined, they would each be specified as 0 and 1.
public  Pokemons[] pbAllRegionalSpecies(int region) {
  Pokemons[] ret= new Pokemons[]{ 0 };
  //pbRgssOpen("Data/regionals.dat","rb"){|f|
  //   int numRegions=f.fgetw;
  //   int numDexDatas=f.fgetw;
  //   if (region>=0 && region<numRegions) {
  //     f.pos=4+region*numDexDatas*2;
  //     //  "i" specifies the national species
  //     for (int i = 0; i < numDexDatas; i++) {
  //       int regionalNum=f.fgetw;
  //       if (regionalNum!=0) ret[regionalNum]=i;
  //     }
  //     //  Replace unspecified regional
  //     //  numbers with zeros
  //     for (int i = 0; i < ret.Length; i++) {
  //       //if (ret[i] == null) ret[i]=0;
  //     }
  //   }
  //}
  return ret;
}

// Gets the ID number for the current region based on the player's current
// position.  Returns the value of "defaultRegion" (optional, default is -1) if
// no region was defined in the game's metadata.  The ID numbers returned by
// this function depend on the current map's position metadata.
//public int pbGetCurrentRegion(int defaultRegion=-1) {
//  int[] mappos=Game.GameData.GameMap == null ? null : (int[])Game.pbGetMetadata(Game.GameData.GameMap.map_id,MapMetadatas.MetadataMapPosition);
//  if (mappos == null) {
//    return defaultRegion; // No region defined
//  } else {
//    return mappos[0];
//  }
//}

// Decides which Dex lists are able to be viewed (i.e. they are unlocked and have
// at least 1 seen species in them), and saves all viable dex region numbers
// (National Dex comes after regional dexes).
// If the Dex list shown depends on the player's location, this just decides if
// a species in the current region has been seen - doesn't look at other regions.
// Here, just used to decide whether to show the Pokédex in the Pause menu.
//public  void pbSetViableDexes() {
//  Game.GameData.Global.pokedexViable=new int[0];
//  if (Core.DEXDEPENDSONLOCATION) {
//    int region=pbGetCurrentRegion();
//    if (region>=Game.GameData.Global.pokedexUnlocked.Length-1) region=-1;
//    if (Game.GameData.Trainer.pokedexSeen((Regions)region)>0) {
//      Game.GameData.Global.pokedexViable[0]=region;
//    }
//  } else {
//    int numDexes=Game.GameData.Global.pokedexUnlocked.Length;
//    switch (numDexes) {
//    case 1:          // National Dex only
//      if (Game.GameData.Global.pokedexUnlocked[0] != null) {
//        if (Game.GameData.Trainer.pokedexSeen>0) {
//          Game.GameData.Global.pokedexViable.Add(0);
//        }
//      }
//      break;
//    default:            // Regional dexes + National Dex
//      for (int i = 0; i < numDexes; i++) {
//        int regionToCheck=(i==numDexes-1) ? -1 : i;
//        if (Game.GameData.Global.pokedexUnlocked[i] != null) {
//          if (Game.GameData.Trainer.pokedexSeen(regionToCheck)>0) {
//            Game.GameData.Global.pokedexViable.Add(i);
//          }
//        }
//      }
//      break;
//    }
//  }
//}

// Unlocks a Dex list.  The National Dex is -1 here (or null argument).
//public  void pbUnlockDex(int dex=-1) {
//  int index=dex;
//  if (index<0) index=Game.GameData.Global.pokedexUnlocked.Length-1;
//  if (index>Game.GameData.Global.pokedexUnlocked.Length-1) index=Game.GameData.Global.pokedexUnlocked.Length-1;
//  Game.GameData.Global.pokedexUnlocked[index]=true;
//}

// Locks a Dex list.  The National Dex is -1 here (or null argument).
//public  void pbLockDex(int dex=-1) {
//  int index=dex;
//  if (index<0) index=Game.GameData.Global.pokedexUnlocked.Length-1;
//  if (index>Game.GameData.Global.pokedexUnlocked.Length-1) index=Game.GameData.Global.pokedexUnlocked.Length-1;
//  Game.GameData.Global.pokedexUnlocked[index]=false;
//}
#endregion

#region Other utilities
public  void pbTextEntry(string helptext,int minlength,int maxlength,int variableNumber) {
  Game.GameData.GameVariables[variableNumber]=Game.UI.pbEnterText(helptext,minlength,maxlength);
  if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
}

public  string[] pbMoveTutorAnnotations(Moves move,Pokemons[] movelist=null) {
  string[] ret=new string[Core.MAXPARTYSIZE];
  for (int i = 0; i < Core.MAXPARTYSIZE; i++) {
    ret[i]=null;
    if (i>=Game.GameData.Trainer.party.Length) continue;
    bool found=false;
    for (int j = 0; j < 4; j++) {
      if (!Game.GameData.Trainer.party[i].isEgg && Game.GameData.Trainer.party[i].moves[j].MoveId==move) {
        ret[i]=Game._INTL("LEARNED");
        found=true;
      }
    }
    if (found) continue;
    Pokemons species=Game.GameData.Trainer.party[i].Species;
    if (!Game.GameData.Trainer.party[i].isEgg && movelist != null && movelist.Any(j => j==species)) {
//  Checked data from movelist
      ret[i]=Game._INTL("ABLE");
    } else if (!Game.GameData.Trainer.party[i].isEgg && Game.GameData.Trainer.party[i].isCompatibleWithMove(move)) {
//  Checked data from PBS/tm.txt
      ret[i]=Game._INTL("ABLE");
    } else {
      ret[i]=Game._INTL("NOT ABLE");
    }
  }
  return ret;
}

public  bool pbMoveTutorChoose(Moves move,Pokemons[] movelist=null,bool bymachine=false) {
  bool ret=false;
  //if (move is String || move is Symbol) {
  //  move=getID(PBMoves,move);
  //}
  if (movelist!=null && movelist is Array) {
    for (int i = 0; i < movelist.Length; i++) {
      //if (movelist[i] is String || movelist[i] is Symbol) {
      //  movelist[i]=getID(PBSpecies,movelist[i]);
      //}
    }
  }
  Game.UI.pbFadeOutIn(99999, () => {
     IPokemonScreen_Scene scene=PokemonScreenScene; //new PokemonScreen_Scene();
     string movename=move.ToString(TextScripts.Name);
     IPokemonScreen screen=PokemonScreen.initialize(scene,Game.GameData.Trainer.party); //new PokemonScreen(scene,Game.GameData.Trainer.party);
     string[] annot=pbMoveTutorAnnotations(move,movelist);
     screen.pbStartScene(Game._INTL("Teach which Pokémon?"),false,annot);
     do { //;loop
       int chosen=screen.pbChoosePokemon();
       if (chosen>=0) {
         Pokemon pokemon=Game.GameData.Trainer.party[chosen];
         if (pokemon.isEgg) {
           Game.pbMessage(Game._INTL("{1} can't be taught to an Egg.",movename));
         } else if ((pokemon.isShadow)) { //rescue false
           Game.pbMessage(Game._INTL("Shadow Pokémon can't be taught any moves."));
         } else if (movelist != null && !movelist.Any(j => j==pokemon.Species )) {
           Game.pbMessage(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
           Game.pbMessage(Game._INTL("{1} can't be learned.",movename));
         } else if (!pokemon.isCompatibleWithMove(move)) {
           Game.pbMessage(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
           Game.pbMessage(Game._INTL("{1} can't be learned.",movename));
         } else {
           if (Item.pbLearnMove(pokemon,move,false,bymachine)) {
             ret=true;
             break;
           }
         }
       } else {
         break;
       }
     } while (true);
     screen.pbEndScene();
  });
  return ret; // Returns whether the move was learned by a Pokemon
}

public  void pbChooseMove(Pokemon pokemon,int variableNumber,int nameVarNumber) {
  if (!pokemon.IsNotNullOrNone()) return;
  int ret=-1;
  Game.UI.pbFadeOutIn(99999, () => {
     IPokemonSummaryScene scene=PokemonSummaryScene; //new PokemonSummaryScene();
     IPokemonSummary screen=PokemonSummary.initialize(scene); //new PokemonSummary(scene);
     ret=screen.pbStartForgetScreen(pokemon,0,0);
  });
  Game.GameData.GameVariables[variableNumber]=ret;
  if (ret>=0) {
    Game.GameData.GameVariables[nameVarNumber]=pokemon.moves[ret].MoveId.ToString(TextScripts.Name);
  } else {
    Game.GameData.GameVariables[nameVarNumber]="";
  }
  if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
}

// Opens the Pokémon screen
//public  void pbPokemonScreen() {
//  if (!Game.GameData.Trainer) return;
//  IPokemonScreen_Scene sscene=new PokemonScreen_Scene();
//  IPokemonScreen sscreen=new PokemonScreen(sscene,Game.GameData.Trainer.party);
//  pbFadeOutIn(99999, () => { sscreen.pbPokemonScreen(); });
//}

//public  void pbSaveScreen() {
//  bool ret=false;
//  IPokemonSaveScene scene=new PokemonSaveScene();
//  IPokemonSave screen=new PokemonSave(scene);
//  ret=screen.pbSaveScreen();
//  return ret;
//}

//public  void pbConvertItemToItem(variable,array) {
//  item=pbGet(variable);
//  pbSet(variable,0);
//  for (int i = 0; i < (array.Length/2); i++) {
//    if (isConst(item,PBItems,array[2*i])) {
//      pbSet(variable,getID(PBItems,array[2*i+1]));
//      return;
//    }
//  }
//}
//
//public  void pbConvertItemToPokemon(variable,array) {
//  item=pbGet(variable);
//  pbSet(variable,0);
//  for (int i = 0; i < (array.Length/2); i++) {
//    if (isConst(item,PBItems,array[2*i])) {
//      pbSet(variable,getID(PBSpecies,array[2*i+1]));
//      return;
//    }
//  }
//}
#endregion

//public  bool pbRecordTrainer() {
//  IAudioObject wave=UI.pbRecord(null,10);
//  if (wave != null) {
//    Game.GameData.Global.trainerRecording=wave;
//    return true;
//  }
//  return false;
//}

    public partial class GlobalMetadata
    {
        public IAudioObject trainerRecording { get; set; }
  public bool bicycle				{ get; set; }
  public bool surfing				{ get; set; }
  public bool diving				{ get; set; }
  public bool sliding				{ get; set; }
  public bool fishing				{ get; set; }
  public bool runtoggle				{ get; set; }
        /// <summary>
        /// </summary>
        /// Should not stack (encourage users to deplete excessive money); 
        /// reset count based on repel used.
		///ToDo: Missing Variables for RepelType, Swarm
  public int repel				    { get; set; }
  public bool flashUsed				{ get; set; }
  public float bridge				{ get; set; }
  public bool runningShoes			{ get; set; }
  public bool snagMachine			{ get; set; }
  public bool seenStorageCreator	{ get; set; }
  public DateTime startTime			{ get; set; }
        /// <summary>
        /// Has player beaten the game already and viewed credits from start to end
        /// </summary>
        /// New Game Plus
  public bool creditsPlayed			                { get; set; }
  public int playerID				                { get; set; }
  public int coins				                    { get; set; }
  public int sootsack				                { get; set; }
  public int? mailbox				                { get; set; }
  //public Character.PCItemStorage pcItemStorage	{ get; set; }
  public int stepcount				                { get; set; }
  public int happinessSteps				            { get; set; }
  public int? pokerusTime				            { get; set; }
  public Character.DayCare daycare		            { get; set; }
  public bool daycareEgg				            { get; set; } //ToDo: int?...
  public int daycareEggSteps			            { get; set; }
  public bool[] pokedexUnlocked			            { get; set; } // Array storing which Dexes are unlocked
  public List<int> pokedexViable		            { get; set; } // All Dexes of non-zero length and unlocked
  public int pokedexDex				                { get; set; } // Dex currently looking at (-1 is National Dex)
  public int[] pokedexIndex				            { get; set; } // Last species viewed per Dex
  public int pokedexMode				            { get; set; } // Search mode
  public int? healingSpot				            { get; set; }
  public float[] escapePoint			            { get; set; }
  public int pokecenterMapId			            { get; set; }
  public float pokecenterX				            { get; set; }
  public float pokecenterY				            { get; set; }
  public int pokecenterDirection		            { get; set; }
  //public Overworld.TilePosition pokecenter			{ get; set; }
  public List<int> visitedMaps				        { get; set; }
  public List<int> mapTrail				            { get; set; }
  public IAudioObject nextBattleBGM				            { get; set; }
  public IAudioObject nextBattleME				            { get; set; }
  public IAudioObject nextBattleBack			            { get; set; }
  public SafariState safariState				    { get; set; }
  //public BugContestState bugContestState			{ get; set; }
  public Combat.Trainer partner			            { get; set; }
  public int? challenge				                { get; set; }
  public int? lastbattle				            { get; set; }
  public List<int> phoneNumbers			            { get; set; }
  public int phoneTime				                { get; set; }
  public bool safesave				                { get; set; }
  public Dictionary<KeyValuePair<int,int>,int> eventvars				{ get; set; }

  public GlobalMetadata() {
    @bicycle              = false;
    @surfing              = false;
    @diving               = false;
    @sliding              = false;
    @fishing              = false;
    @runtoggle            = false;
    @repel                = 0;
    @flashUsed            = false;
    @bridge               = 0;
    @runningShoes         = false;
    @snagMachine          = false;
    @seenStorageCreator   = false;
    @startTime            = Game.GetTimeNow;
    @creditsPlayed        = false;
    @playerID             = -1;
    @coins                = 0;
    @sootsack             = 0;
    @mailbox              = null;
    //@pcItemStorage        = null;
    @stepcount            = 0;
    @happinessSteps       = 0;
    @pokerusTime          = null;
    @daycare              = new Character.DayCare(2); //{ { null, 0 }, { null, 0 } };
    @daycareEgg           = false;//0;
    @daycareEggSteps      = 0;
    int numRegions        = 0;
    //pbRgssOpen("Data/regionals.dat","rb"){|f| numRegions = f.fgetw }
    @pokedexUnlocked      = new bool[numRegions];
    @pokedexViable        = new List<int>();
    @pokedexDex           = (numRegions==0) ? -1 : 0;
    @pokedexIndex         = new int[numRegions];
    @pokedexMode          = 0;
    for (int i = 0; i < numRegions+1; i++) {	// National Dex isn't a region, but is included
      @pokedexIndex[i]    = 0;
      @pokedexUnlocked[i] = (i==0);
    }
    @healingSpot          = null;
    @escapePoint          = new float[0];
    @pokecenterMapId      = -1;
    @pokecenterX          = -1;
    @pokecenterY          = -1;
    @pokecenterDirection  = -1;
    @visitedMaps          = new List<int>();
    @mapTrail             = new List<int>();
    @nextBattleBGM        = null;
    @nextBattleME         = null;
    @nextBattleBack       = null;
    @safariState          = null;
    //@bugContestState      = null;
    @partner              = null;
    @challenge            = null;
    @lastbattle           = null;
    @phoneNumbers         = new List<int>();
    @phoneTime            = 0;
    @eventvars            = new Dictionary<KeyValuePair<int, int>, int>();
    @safesave             = false;
  }

  //public float bridge { get {
  //  if (@bridge == null) @bridge=0;
  //  return @bridge;
  //} }

  //public Pokemons[] roamPokemonCaught { get {
  //  if (@roamPokemonCaught == null) {
  //    @roamPokemonCaught= new Pokemons[0];
  //  }
  //  return @roamPokemonCaught;
  //} }
    }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	public partial class Game : PokemonEssentials.Interface.IGameUtility
	{
		public static PokemonUnity.Localization.XmlStringRes LocalizationDictionary;
		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		//ToDo: Might need to redo function to use a global/const identifier string (KeyValuePair)
		public static string _INTL(string message, params object[] param)
		{
			string msg = message;
			//Lookup Localization Dictionary for message string in translations...
			if(LocalizationDictionary != null)
			{
				msg = LocalizationDictionary.GetStr(message);
				//if (msg == message) GameDebug.Log("Couldn't map localization string for: `{0}`", message);
			}
			if(!msg.Contains("{0}") && param?.Length > 0) //only loop if params start at 1+
				for (int i = 0; param.Length >= i; i++) //(int i = param.Length; i > 0; i--)
					msg = msg.Replace($"{{{i}}}", $"{{{i - 1}}}");
			return string.Format(msg, param);
		}

		#region General purpose utilities
		public bool _NextComb(int[] comb,int length) {
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
		/// Iterates through the array and yields each combination of <paramref name="num"/>
		/// elements in the array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="num"></param>
		/// <returns></returns>
		//public IEnumerator<T[]> EachCombination<T>(T[] array,int num) {
		public IEnumerable<T[]> EachCombination<T>(IList<T> array,int num) {
			if (array.Count<num || num<=0) yield break; //return;
			if (array.Count==num) {
				yield return array.ToArray();
				yield break; //return;
			} else if (num==1) {
				foreach (T x in array) {
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
			} while (_NextComb(currentComb,array.Count));
		}

		/// <summary>
		/// Gets the path of the user's "My Documents" folder.
		/// </summary>
		/// <returns></returns>
		//ToDo?...
		public string GetMyDocumentsFolder() { return ""; }

		/// <summary>
		/// Returns a country ID
		/// </summary>
		/// http://msdn.microsoft.com/en-us/library/dd374073%28VS.85%29.aspx?
		//ToDo?...
		public int GetCountry() { return 0; }

		/// <summary>
		/// Returns a language ID
		/// </summary>
		public int GetLanguage() {
			//getUserDefaultLangID=new Win32API("kernel32","GetUserDefaultLangID","","i"); //rescue null
			int ret=0;
			//if (getUserDefaultLangID) {
			//  ret=getUserDefaultLangID.call()&0x3FF;
			//}
			//if (ret==0) {		// Unknown
			//  ret=MiniRegistry.get(MiniRegistry::HKEY_CURRENT_USER,
			//     "Control Panel\\Desktop\\ResourceLocale","",0);
			//  if (ret==0) ret=MiniRegistry.get(MiniRegistry::HKEY_CURRENT_USER,
			//     "Control Panel\\International","Locale","0").to_i(16);
			//  ret=ret&0x3FF;
			//  if (ret==0 ) return 0;	// Unknown
			//}
			if (ret==0x11) return 1;	// Japanese
			if (ret==0x09) return 2;	// English
			if (ret==0x0C) return 3;	// French
			if (ret==0x10) return 4;	// Italian
			if (ret==0x07) return 5;	// German
			if (ret==0x0A) return 7;	// Spanish
			if (ret==0x12) return 8;	// Korean
			return 2; // Use 'English' by default
		}

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
		//public bool ChangePlayer(int id) {
		public void ChangePlayer(int id) {
			if (id < 0 || id >= 8) return; //false;
			//IPokemonMetadata meta=GetMetadata(0,MetadataPlayerA+id);
			MetadataPlayer? meta=GetMetadata(0).Global.Players[id];
			if (meta == null) return; //false;
			if (Trainer != null) Trainer.trainertype=(TrainerTypes)meta?.Type; //meta[0];
			GamePlayer.character_name=meta?.Name; //meta[1];
			GamePlayer.character_hue=0;
			Global.playerID=id;
			if (Trainer != null) Trainer.metaID=id;
		}

		public string GetPlayerGraphic() {
			int id=Global.playerID;
			if (id<0 || id>=8) return "";
			//IPokemonMetadata meta=GetMetadata(0,MetadataPlayerA+id);
			MetadataPlayer? meta=GetMetadata(0).Global.Players[id];
			if (meta == null) return "";
			return PlayerSpriteFile((TrainerTypes)meta?.Type); //meta[0]
		}

		public TrainerTypes GetPlayerTrainerType() {
			int id=Global.playerID;
			if (id<0 || id>=8) return 0;
			//IPokemonMetadata meta=GetMetadata(0,MetadataPlayerA+id);
			MetadataPlayer? meta=GetMetadata(0).Global.Players[id];
			if (meta == null) return 0;
			return (TrainerTypes)meta?.Type; //meta[0];
		}

		public int GetTrainerTypeGender(TrainerTypes trainertype) {
		//public bool? GetTrainerTypeGender(TrainerTypes trainertype) {
			int? ret=2; // 2 = gender unknown
			//RgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
			//	if (!trainertypes[trainertype]) {
				if (!Kernal.TrainerMetaData.ContainsKey(trainertype)) {
					ret=2;
				} else {
					//ret=trainertypes[trainertype][7];
					ret= Kernal.TrainerMetaData[trainertype].Gender == true ? 1 : (Kernal.TrainerMetaData[trainertype].Gender == false ? 0 : (int?)null);
					if (!ret.HasValue) ret=2;
				}
			//}
			return ret.Value;
		}

		public void TrainerName(string name=null,int outfit=0) {
			if (Global.playerID<0) {
				ChangePlayer(0);
			}
			TrainerTypes trainertype=GetPlayerTrainerType();
			string trname=name;
			Trainer=new Trainer(trname,trainertype);
			Trainer.outfit=outfit;
			if (trname==null && this is IGameTextEntry t) {
				trname=t.EnterPlayerName(Game._INTL("Your name?"),0,7);
				if (trname=="") {
					int gender=GetTrainerTypeGender(trainertype);
					trname=SuggestTrainerName(gender);
				}
			}
			Trainer.name=trname;
			Bag=new Character.PokemonBag();
			PokemonTemp.begunNewGame=true; //new game because you changed your name?
		}

		public string SuggestTrainerName(int gender) {
			string userName=GetUserName();
			//userName=userName.gsub(/\s+.*$/,""); // trim space
			if (userName.Length>0 && userName.Length<7) {
				//userName[0,1]=userName[0,1].upcase; //make first two characters cap
				char[] temp = userName.ToCharArray();
				temp[0]=userName.ToUpper()[0];
				temp[1]=userName.ToUpper()[1];
				return new string(temp); //userName;
			}
			//userName=userName.gsub(/\d+$/,""); // trim numbers
			if (userName.Length>0 && userName.Length<7) {
				//userName[0,1]=userName[0,1].upcase; //make first two characters cap
				char[] temp = userName.ToCharArray();
				temp[0]=userName.ToUpper()[0];
				temp[1]=userName.ToUpper()[1];
				return new string(temp); //userName;
			}
			//string owner=MiniRegistry.get(MiniRegistry::HKEY_LOCAL_MACHINE,
			//	"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
			//	"RegisteredOwner","");
			string owner = "";
			owner=owner.Trim();//.gsub(/\s+.*$/,""); //trim spaces
			if (owner.Length>0 && owner.Length<7) {
				//owner[0,1]=owner[0,1].upcase; //make first two characters cap
				char[] temp = owner.ToCharArray();
				temp[0]=owner.ToUpper()[0];
				temp[1]=owner.ToUpper()[1];
				return new string(temp); //owner;
			}
			return getRandomNameEx(gender,null,1,7);
		}

		public string GetUserName() {
			int buffersize=100;
			string getUserName=""; //new Win32API('advapi32.dll','GetUserName','pp','i');
			int i = 0; do { //10.times;
				int[] size= new int[buffersize];//.pack("V")
				//string buffer="\0"*buffersize; //"0000..." x100
				//if (getUserName.call(buffer,size)!=0) {
				//	return buffer.gsub(/\0/,""); //replace all 0s with "", using regex
				//}
				buffersize+=200; i++;
			} while (i < 10);
			return "";
		}

		public string getRandomNameEx(int type,int? variable,int? upper,int maxLength=100) {
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
			if (upper == 1) { //case 1:
				//name[0,1]=name[0,1].upcase;
				name.ReplaceAt(0,name.ToUpper()[0]);
				name.ReplaceAt(1,name.ToUpper()[1]);
				//  break;
			}
			if (GameVariables != null && variable != null) {
				GameVariables[variable.Value]=name;
				if (GameMap != null) GameMap.need_refresh = true;
			}
			return name;
		}

		public string getRandomName(int maxLength=100) {
			return getRandomNameEx(2,null,null,maxLength);
		}
		#endregion

		#region Event timing utilities
		public void TimeEvent(int? variableNumber,int secs=86400) {
			if (variableNumber != null && variableNumber.Value>=0) {
				if (GameVariables != null) {
					if (secs<0) secs=0;
					DateTime timenow=Game.GetTimeNow;
					GameVariables[variableNumber.Value]=new float[] {timenow.Ticks,secs};
					if (GameMap != null) GameMap.refresh();
				}
			}
		}

		public void TimeEventDays(int? variableNumber,int days=0) {
			if (variableNumber != null && variableNumber>=0) {
				if (GameVariables != null) {
					if (days<0) days=0;
					DateTime timenow=Game.GetTimeNow;
					float time=timenow.Ticks;
					float expiry=(time%86400.0f)+(days*86400.0f);
					GameVariables[variableNumber.Value]= new []{ time,expiry-time };
					if (GameMap != null) GameMap.refresh();
				}
			}
		}

		public bool TimeEventValid(int? variableNumber) {
			bool retval=false;
			if (variableNumber != null && variableNumber>=0 && GameVariables != null) {
				object value=GameVariables[variableNumber.Value];
				if (value is float[] v && v.Length > 0) { //is Array
					DateTime timenow=Game.GetTimeNow;
					retval=(timenow.Ticks - v[0] > v[1]); // value[1] is age in seconds
					if (v[1]<=0) retval=false;	// zero age
				}
				if (!retval) {
					GameVariables[variableNumber.Value]=0;
					if (GameMap != null) GameMap.refresh();
				}
			}
			return retval;
		}
		#endregion

		#region General-purpose utilities with dependencies
		/// <summary>
		/// Similar to <see cref="FadeOutIn"/>, but pauses the music as it fades out.
		/// </summary>
		/// <remarks>
		/// Requires scripts "Audio" (for <seealso cref="IGameSystem.bgm_pause"/>)
		/// and "SpriteWindow" (for <seealso cref="FadeOutIn"/>).
		/// </remarks>
		/// <param name="zViewport"></param>
		public void FadeOutInWithMusic(int zViewport, Action block = null) {
			PokemonEssentials.Interface.IAudioBGS playingBGS=GameSystem.getPlayingBGS();
			PokemonEssentials.Interface.IAudioBGM playingBGM=GameSystem.getPlayingBGM();
			GameSystem.bgm_pause(1.0f);
			GameSystem.bgs_pause(1.0f);
			int pos=GameSystem.bgm_position;
			FadeOutIn(zViewport, block: () => {
				if (block != null) block.Invoke(); //(block_given ?) yield;
				GameSystem.bgm_position=pos;
				GameSystem.bgm_resume(playingBGM);
				GameSystem.bgs_resume(playingBGS);
			});
		}

		/// <summary>
		/// Gets the wave data from a file and displays an message if an error occurs.
		/// Can optionally delete the wave file (this is useful if the file was a
		/// temporary file created by a recording).
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="deleteFile"></param>
		/// <returns></returns>
		/// Requires the script AudioUtilities
		/// Requires the script "PokemonMessages"
		public IWaveData getWaveDataUI(string filename,bool deleteFile=false) {
			IWaveData error = null; //getWaveData(filename);
			if (deleteFile) {
				try {
					File.Delete(filename);
				} catch (Exception) { } //Errno::EINVAL, Errno::EACCES, Errno::ENOENT;
			}
			//switch (error) {
			//	if (error == 1) //case 1:
			//		(this as IGameMessage).Message(Game._INTL("The recorded data could not be found or saved."));
			//		break;
			//	else if (error == 2) //case 2:
			//		(this as IGameMessage).Message(Game._INTL("The recorded data was in an invalid format."));
			//		break;
			//	else if (error == 3) //case 3:
			//		(this as IGameMessage).Message(Game._INTL("The recorded data's format is not supported."));
			//		break;
			//	else if (error == 4) //case 4:
			//		(this as IGameMessage).Message(Game._INTL("There was no sound in the recording. Please ensure that a microphone is attached to the computer /and/ is ready."));
			//		break;
			//	else //default:
			//		return error;
			//}
			return null;
		}

		/// <summary>
		/// Starts recording, and displays a message if the recording failed to start.
		/// </summary>
		/// <returns>Returns true if successful, false otherwise</returns>
		/// Requires the script AudioUtilities
		/// Requires the script "PokemonMessages"
		public bool beginRecordUI() {
			int code = 0; //beginRecord();
			switch (code) {
				case 0:
					return true;
				case 256+66:
					(this as IGameMessage).Message(Game._INTL("All recording devices are in use. Recording is not possible now."));
					return false;
				case 256+72:
					(this as IGameMessage).Message(Game._INTL("No supported recording device was found. Recording is not possible."));
					return false;
				default:
					string buffer=new int[256].ToString(); //"\0"*256;
					//MciErrorString.call(code,buffer,256);
					(this as IGameMessage).Message(Game._INTL("Recording failed: {1}",buffer));//.gsub(/\x00/,"")
					return false;
			}
		}

		public virtual IList<object> HideVisibleObjects()
		{
			IList<object> visibleObjects= new List<object>(); //ToDo: ISpriteWindow?
			////ObjectSpace.each_object(Sprite){|o|
			//ObjectSpace.each_object(Sprite){|o|
			//	if (!o.disposed && o.visible) {
			//		visibleObjects.Add(o);
			//		o.visible=false;
			//	}
			//}
			////ObjectSpace.each_object(Viewport){|o|
			//ObjectSpace.each_object(Viewport){|o|
			//	if (!Disposed(o) && o.visible) {
			//		visibleObjects.Add(o);
			//		o.visible=false;
			//	}
			//}
			//ObjectSpace.each_object(Plane){|o|
			//	if (!o.disposed && o.visible) {
			//		visibleObjects.Add(o);
			//		o.visible=false;
			//	}
			//}
			//ObjectSpace.each_object(Tilemap){|o|
			//	if (!o.disposed && o.visible) {
			//		visibleObjects.Add(o);
			//		o.visible=false;
			//	}
			//}
			//ObjectSpace.each_object(Window){|o|
			//	if (!o.disposed && o.visible) {
			//		visibleObjects.Add(o);
			//		o.visible=false;
			//	}
			//}
			return visibleObjects;
		}

		public void ShowObjects(IList<object> visibleObjects) {
			foreach (IViewport o in visibleObjects) {
				if (this is IGameSpriteWindow s && !s.Disposed(o)) {
					o.visible=true;
				}
			}
		}

		public void LoadRpgxpScene(ISceneMap scene) {
			if (!(Scene is ISceneMap)) return;
			ISceneMap oldscene=Scene;
			Scene=scene;
			Graphics.freeze();
			oldscene.disposeSpritesets();
			IList<object> visibleObjects=HideVisibleObjects();
			Graphics.transition(15,null,0); //ToDo: revisit this again...
			Graphics.freeze();
			while (Scene != null && !(Scene is ISceneMap)) {
				Scene.main();
			}
			Graphics.transition(15,null,0); //ToDo: revisit this again...
			Graphics.freeze();
			oldscene.createSpritesets();
			ShowObjects(visibleObjects);
			Graphics.transition(20,null,0); //ToDo: revisit this again...
			Scene=oldscene;
		}

		/// <summary>
		/// Gets the value of a variable.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public object Get(int? id) {
			if (id == null || GameVariables == null) return 0;
			return GameVariables[id.Value];
		}

		/// <summary>
		/// Sets the value of a variable.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		public void Set(int? id,object value) {
			if (id != null && id>=0) {
				if (GameVariables != null) GameVariables[id.Value]=value;
				if (GameMap != null) GameMap.need_refresh = true;
			}
		}

		/// <summary>
		/// Runs a common event and waits until the common event is finished.
		/// </summary>
		/// Requires the script "PokemonMessages"
		/// <param name="id"></param>
		/// <returns></returns>
		public bool CommonEvent(int id) {
			if (id<0) return false;
			IGameCommonEvent ce=DataCommonEvents[id];
			if (ce==null) return false;
			IList<PokemonEssentials.Interface.RPGMaker.Kernal.IEventCommand> celist=ce.list;
			IInterpreter interp=Interpreter.initialize(); //new Interpreter();
			interp.setup(celist,0);
			do {
				Graphics?.update();
				Input.update();
				interp.update();
				if (this is IGameMessage m) m.UpdateSceneMap();
			} while (interp.running);
			return true;
		}

		public void Exclaim(IGameCharacter[] @event,int id=Core.EXCLAMATION_ANIMATION_ID,bool tinting=false) {
			ISprite sprite=null;
			if (@event?.Length > 0) { //is Array
				//List<IGameCharacter> done=new List<IGameCharacter>();
				List<int> done=new List<int>();
				foreach (var i in @event) {
					if (!done.Contains(i.id)) {
						sprite=((ISpritesetMapAnimation)Scene.spriteset).addUserAnimation(id,i.x,i.y,tinting);
						done.Add(i.id);
					}
				}
			//} else {
			//	sprite=((ISpritesetMapAnimation)Scene.spriteset()).addUserAnimation(id,@event.x,@event.y,tinting);
			}
			while (!sprite.disposed) {
				Graphics?.update();
				Input.update();
				if (this is IGameMessage m) m.UpdateSceneMap();
			}
		}

		public void NoticePlayer(IGameCharacter @event) {
			if (this is IGameField f) {
				if (!f.FacingEachOther(@event,GamePlayer)) {
					Exclaim(new IGameCharacter[] { @event });
				}
				f.TurnTowardEvent(GamePlayer,@event);
				f.MoveTowardPlayer(@event);
			}
		}
		#endregion

		#region Loads Pokémon/item/trainer graphics
		[System.Obsolete("Unused")]
		public string PokemonBitmapFile(Pokemons species,bool shiny,bool back=false) {
			if (shiny) {
				//  Load shiny bitmap
				string ret=string.Format("Graphics/Battlers/{0}s{1}",species.ToString(),back ? "b" : ""); //rescue null
				if ((this is IGameSpriteWindow g0) && g0.ResolveBitmap(ret) == null) {
					ret=string.Format("Graphics/Battlers/{0}s{1}",species,back ? "b" : "");
				}
				return ret;
			} else {
				//  Load normal bitmap
				string ret=string.Format("Graphics/Battlers/{0}{1}",species.ToString(),back ? "b" : ""); //rescue null
				if ((this is IGameSpriteWindow g1) && g1.ResolveBitmap(ret) == null) {
					ret=string.Format("Graphics/Battlers/{0}{1}",species,back ? "b" : "");
				}
				return ret;
			}
		}

		public IAnimatedBitmap LoadPokemonBitmap(IPokemon pokemon, bool back = false)
		{
			return LoadPokemonBitmapSpecies(pokemon, pokemon.Species, back);
		}

		//Note: Returns an AnimatedBitmap, not a Bitmap
		public virtual IAnimatedBitmap LoadPokemonBitmapSpecies(IPokemon pokemon, Pokemons species, bool back = false)
		{
			IAnimatedBitmap ret = null;
			//if (pokemon.isEgg?) {
			//	bitmapFileName=string.Format("Graphics/Battlers/%segg",species.ToString()); //rescue null
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Battlers/%03degg",species);
			//		if (!ResolveBitmap(bitmapFileName)) {
			//			bitmapFileName=string.Format("Graphics/Battlers/egg");
			//		}
			//	}
			//	bitmapFileName=ResolveBitmap(bitmapFileName);
			//} else {
			//	bitmapFileName=CheckPokemonBitmapFiles([species,back,
			//												(pokemon.isFemale?),
			//												pokemon.isShiny?,
			//												(pokemon.form rescue 0),
			//												(pokemon.isShadow? rescue false)])
			//	//  Alter bitmap if supported
			//	alterBitmap=(MultipleForms.getFunction(species,"alterBitmap") rescue null);
			//}
			//if (bitmapFileName != null && alterBitmap) {
			//	animatedBitmap=new AnimatedBitmap(bitmapFileName);
			//	copiedBitmap=animatedBitmap.copy;
			//	animatedBitmap.dispose();
			//	copiedBitmap.each {|bitmap|
			//		alterBitmap.call(pokemon,bitmap);
			//	}
			//	ret=copiedBitmap;
			//} else if (bitmapFileName) {
			//	ret=new AnimatedBitmap(bitmapFileName);
			//}
			return ret;
		}

		//Note: Returns an AnimatedBitmap, not a Bitmap
		public virtual IAnimatedBitmap LoadSpeciesBitmap(Pokemons species, bool female = false, int form = 0, bool shiny = false, bool shadow = false, bool back = false, bool egg = false)
		{
			IAnimatedBitmap ret = null;
			//if (egg) {
			//	bitmapFileName=string.Format("Graphics/Battlers/%segg",getConstantName(Species,species)) rescue null;
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Battlers/%03degg",species);
			//		if (!ResolveBitmap(bitmapFileName)) {
			//			bitmapFileName=string.Format("Graphics/Battlers/egg");
			//		}
			//	}
			//	bitmapFileName=ResolveBitmap(bitmapFileName);
			//} else {
			//	bitmapFileName=CheckPokemonBitmapFiles([species,back,female,shiny,form,shadow]);
			//}
			//if (bitmapFileName) {
			//	ret=new AnimatedBitmap(bitmapFileName);
			//}
			return ret;
		}

		public virtual string CheckPokemonBitmapFiles(params object[] args) {
			//Pokemons species=params[0];
			//bool back=params[1];
			//List<> factors=[];
			//if (params[5] && params[5]!=false    ) factors.Add([5,params[5],false]);	// shadow
			//if (params[2] && params[2]!=false    ) factors.Add([2,params[2],false]);	// gender
			//if (params[3] && params[3]!=false    ) factors.Add([3,params[3],false]);	// shiny
			//if (params[4] && params[4].ToString()!="" &&
			//	params[4].ToString()!="0") factors.Add([4,params[4].ToString(),""]); // form
			//bool tshadow=false;
			//bool tgender=false;
			//bool tshiny=false;
			//string tform="";
			//for (int i = 0; i < 2**factors.Length; i++) {
			//	for (int j = 0; j < factors.Length; j++) {
			//		switch (factors[j][0]) {
			//			case 2:   // gender
			//				tgender=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//				break;
			//			case 3:   // shiny
			//				tshiny=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//				break;
			//			case 4:   // form
			//				tform=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//				break;
			//			case 5:   // shadow
			//				tshadow=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//				break;
			//		}
			//	}
			//	string bitmapFileName=string.Format("Graphics/Battlers/%s%s%s%s%s%s",
			//		getConstantName(Species,species),
			//		tgender ? "f" : "",
			//		tshiny ? "s" : "",
			//		back ? "b" : "",
			//		(tform!="" ? "_"+tform : ""),
			//		tshadow ? "_shadow" : ""); //rescue null
			//	string ret=ResolveBitmap(bitmapFileName);
			//	if (ret) return ret;
			//	bitmapFileName=string.Format("Graphics/Battlers/%03d%s%s%s%s%s",
			//		species,
			//		tgender ? "f" : "",
			//		tshiny ? "s" : "",
			//		back ? "b" : "",
			//		(tform!="" ? "_"+tform : ""),
			//		tshadow ? "_shadow" : "");
			//	ret=ResolveBitmap(bitmapFileName);
			//	if (ret != null) return ret;
			//}
			return null;
		}

		public string LoadPokemonIcon(IPokemon pokemon) {
			return null; //new AnimatedBitmap(PokemonIconFile(pokemon)).deanimate;
		}

		public virtual string PokemonIconFile(IPokemon pokemon) {
			string bitmapFileName=null;
			//bitmapFileName=CheckPokemonIconFiles([pokemon.Species,
			//										(pokemon.isFemale?),
			//										pokemon.isShiny?,
			//										(pokemon.form rescue 0),
			//										(pokemon.isShadow? rescue false)],
			//										pokemon.isEgg?);
			return bitmapFileName;
		}

		public virtual string CheckPokemonIconFiles(object[] param,bool egg=false) {
			//Pokemons species=params[0];
			//if (egg) {
			//	bitmapFileName=string.Format("Graphics/Icons/icon%segg",getConstantName(Species,species)); //rescue null
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Icons/icon%03degg",species) ;
			//		if (!ResolveBitmap(bitmapFileName)) {
			//			bitmapFileName=string.Format("Graphics/Icons/iconEgg");
			//		}
			//	}
			//	return ResolveBitmap(bitmapFileName);
			//} else {
			//	List<> factors=[];
			//	if (params[4] && params[4]!=false    ) factors.Add([4,params[4],false]);	// shadow
			//	if (params[1] && params[1]!=false    ) factors.Add([1,params[1],false]);	// gender
			//	if (params[2] && params[2]!=false    ) factors.Add([2,params[2],false]);	// shiny
			//	if (params[3] && params[3].ToString()!="" &&
			//		params[3].ToString()!="0") factors.Add([3,params[3].ToString(),""]);	// form
			//	bool tshadow=false;
			//	bool tgender=false;
			//	bool tshiny=false;
			//	string tform="";
			//	for (int i = 0; i < 2**factors.Length; i++) {
			//		for (int j = 0; j < factors.Length; j++) {
			//			switch (factors[j][0]) {
			//				case 1:   // gender
			//					tgender=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//					break;
			//				case 2:   // shiny
			//					tshiny=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//					break;
			//				case 3:   // form
			//					tform=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//					break;
			//				case 4:   // shadow
			//					tshadow=((i/(2**j))%2==0) ? factors[j][1] : factors[j][2];
			//					break;
			//			}
			//		}
			//		bitmapFileName=string.Format("Graphics/Icons/icon%s%s%s%s%s",
			//			getConstantName(Species,species),
			//			tgender ? "f" : "",
			//			tshiny ? "s" : "",
			//			(tform!="" ? "_"+tform : ""),
			//			tshadow ? "_shadow" : "") rescue null;
			//		string ret=ResolveBitmap(bitmapFileName);
			//		if (ret != null) return ret;
			//		bitmapFileName=string.Format("Graphics/Icons/icon%03d%s%s%s%s",
			//			species,
			//			tgender ? "f" : "",
			//			tshiny ? "s" : "",
			//			(tform!="" ? "_"+tform : ""),
			//			tshadow ? "_shadow" : "");
			//		ret=ResolveBitmap(bitmapFileName);
			//		if (ret != null) return ret;
			//	}
			//}
			return null;
		}

		/// <summary>
		/// Used by the Pokédex
		/// </summary>
		/// <param name="pokemon"></param>
		/// <returns></returns>
		public virtual string PokemonFootprintFile(Pokemons pokemon) {
			//if (!pokemon) return null;
			string bitmapFileName = null;
			//if (pokemon is Numeric) {
				//bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%s",getConstantName(Species,pokemon)) rescue null;
				//if (!ResolveBitmap(bitmapFileName)) bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%03d",pokemon);
			//}
			//return ResolveBitmap(bitmapFileName);
			return null;
		}
		public virtual string PokemonFootprintFile(IPokemon pokemon) {
			if (pokemon == null) return null;
			string bitmapFileName = null;
			//{
			//	bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%s_%d",getConstantName(Species,pokemon.Species),(pokemon.form rescue 0)); //rescue null
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%03d_%d",pokemon.Species,(pokemon.form rescue 0)); //rescue null
			//		if (!ResolveBitmap(bitmapFileName)) {
			//			bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%s",getConstantName(Species,pokemon.Species)); //rescue null
			//			if (!ResolveBitmap(bitmapFileName)) {
			//				bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%03d",pokemon.Species);
			//			}
			//		}
			//	}
			//}
			//return ResolveBitmap(bitmapFileName);
			return null;
		}

		public virtual string ItemIconFile(Items item) {
			//if (!item) return null;
			string bitmapFileName=null;
			//if (item==0) {
			//	bitmapFileName=string.Format("Graphics/Icons/itemBack");
			//} else {
			//	bitmapFileName=string.Format("Graphics/Icons/item%s",getConstantName(Items,item)); //rescue null
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Icons/item%03d",item);
			//	}
			//}
			return bitmapFileName;
		}

		public virtual string MailBackFile(Items item) {
			//if (!item) return null;
			//string bitmapFileName=string.Format("Graphics/Pictures/mail%s",getConstantName(Items,item)); //rescue null
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Pictures/mail%03d",item);
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string TrainerCharFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("Graphics/Characters/trchar%s",getConstantName(Trainers,type)) rescue null;
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trchar%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string TrainerCharNameFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("trchar%s",getConstantName(Trainers,type)) rescue null;
			//if (!ResolveBitmap(string.Format("Graphics/Characters/"+bitmapFileName))) {
			//	bitmapFileName=string.Format("trchar%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string TrainerHeadFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%s",getConstantName(Trainers,type)) rescue null;
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string PlayerHeadFile(TrainerTypes type) {
			//if (!type) return null;
			//int outfit=Game.GameData.Trainer ? Game.GameData.Trainer.outfit : 0;
			//string bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%s_%d",
			//	getConstantName(Trainers,type),outfit); //rescue null
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%03d_%d",type,outfit);
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=TrainerHeadFile(type);
			//	}
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string TrainerSpriteFile(TrainerTypes type) {
			//if (!type) return null;
			//string bitmapFileName=string.Format("Graphics/Characters/trainer%s",getConstantName(Trainers,type)) rescue null;
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trainer%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string TrainerSpriteBackFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("Graphics/Characters/trback%s",getConstantName(Trainers,type)) rescue null;
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trback%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string PlayerSpriteFile(TrainerTypes type) {
			//if (!type) return null;
			//int outfit=Game.GameData.Trainer ? Game.GameData.Trainer.outfit : 0;
			//string bitmapFileName=string.Format("Graphics/Characters/trainer%s_%d",
			//	getConstantName(Trainers,type),outfit); //rescue null
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trainer%03d_%d",type,outfit);
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=TrainerSpriteFile(type);
			//	}
			//}
			//return bitmapFileName;
			return null;
		}

		public virtual string PlayerSpriteBackFile(TrainerTypes type) {
			//if (!type) return null;
			//int outfit=Game.GameData.Trainer ? Game.GameData.Trainer.outfit : 0;
			//string bitmapFileName=string.Format("Graphics/Characters/trback%s_%d",
			//	getConstantName(Trainers,type),outfit); //rescue null
			//if (!ResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trback%03d_%d",type,outfit);
			//	if (!ResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=TrainerSpriteBackFile(type);
			//	}
			//}
			//return bitmapFileName;
			return null;
		}
		#endregion

		#region Loads music and sound effects
		public string ResolveAudioSE(string file) {
			if (file == null) return null;
			//if (RTP.exists("Audio/SE/"+file,new string[] { "", ".wav", ".mp3", ".ogg" })) {
			//	return RTP.getPath("Audio/SE/"+file,new string[] { "", ".wav", ".mp3", ".ogg" });
			//}
			return null;
		}

		public int CryFrameLength(Pokemons pokemon,float? pitch=null) {
			if (pokemon == Pokemons.NONE) return 0;
			if (pitch == null) pitch=100;
			pitch=pitch/100;
			if (pitch<=0) return 0;
			float playtime=0.0f;
			//if (pokemon is Numeric) {
				string pkmnwav=ResolveAudioSE(CryFile(pokemon));
				if (pkmnwav != null) playtime = 0; //getPlayTime(pkmnwav); //ToDo: uncomment and finish...
			//} else if (!pokemon.isEgg) {
			//	if (pokemon is IPokemonChatter p && p.chatter != null) { //pokemon.respond_to("chatter")
			//		playtime=p.chatter.time;
			//		pitch=1.0f;
			//	} else {
			//		string pkmnwav=ResolveAudioSE(CryFile(pokemon));
			//		if (pkmnwav != null) playtime=getPlayTime(pkmnwav);
			//	}
			//}
			playtime/=pitch.Value; // sound is lengthened the lower the pitch
			//  4 is added to provide a buffer between sounds
			return (int)Math.Ceiling(playtime*Graphics.frame_rate)+4;
		}

		public int CryFrameLength(IPokemon pokemon,float? pitch=null) {
			if (!pokemon.IsNotNullOrNone()) return 0;
			if (pitch == null) pitch=100;
			pitch=pitch/100;
			if (pitch<=0) return 0;
			float playtime=0.0f;
			//if (pokemon is Numeric) {
			//	string pkmnwav=ResolveAudioSE(CryFile(pokemon));
			//	if (pkmnwav != null) playtime=getPlayTime(pkmnwav);
			//} else
			if (!pokemon.isEgg) {
				if (pokemon is IPokemonChatter p && p.chatter != null) { //pokemon.respond_to("chatter")
					playtime=p.chatter.time();
					pitch=1.0f;
				} else {
					string pkmnwav=ResolveAudioSE(CryFile(pokemon));
					if (pkmnwav != null) playtime = 0; //getPlayTime(pkmnwav); //ToDo: uncomment and finish...
				}
			}
			playtime/=pitch.Value; // sound is lengthened the lower the pitch
			//  4 is added to provide a buffer between sounds
			return (int)Math.Ceiling(playtime*Graphics.frame_rate)+4;
		}

		public void PlayCry(IPokemon pokemon,int volume=90,float? pitch=null) {
			if (pokemon == null) return;
			if (!pokemon.isEgg) {
				if (pokemon is IPokemonChatter p && p.chatter != null) { //pokemon.respond_to("chatter")
					p.chatter.play();
				} else {
					string pkmnwav=CryFile(pokemon);
					if (pkmnwav != null) {
						//SEPlay(new AudioTrack().initialize(pkmnwav,volume,
						//	pitch != null ? pitch : (pokemon.HP*25/pokemon.TotalHP)+75)); //rescue null
					}
				}
			}
		}

		public void PlayCry(Pokemons pokemon,int volume=90,float? pitch=null) {
			if (pokemon == Pokemons.NONE) return;
			//if (pokemon is Numeric) {
				string pkmnwav=CryFile(pokemon);
				if (pkmnwav != null) {
					//SEPlay(new AudioTrack().initialize(pkmnwav,volume,pitch != null ? pitch : 100)); //rescue null
				}
			//}
		}

		public string CryFile(Pokemons pokemon) {
			if (pokemon == Pokemons.NONE) return null;
			//if (pokemon is Numeric) {
				string filename=string.Format("Cries/{0}Cry",pokemon.ToString()); //rescue null
				if (ResolveAudioSE(filename) == null) filename=string.Format("Cries/%03dCry",pokemon);
				if (ResolveAudioSE(filename) != null) return filename;
			//}
			return null;
		}

		public string CryFile(IPokemon pokemon) {
			if (pokemon == null) return null;
			if (!pokemon.isEgg) {
				string filename=string.Format("Cries/{0}Cry_{1}",pokemon.Species.ToString(),pokemon is IPokemonMultipleForms f0 ? f0.form : 0); //rescue 0 rescue null
				if (ResolveAudioSE(filename) == null) filename=string.Format("Cries/{0}Cry_{1}",pokemon.Species,pokemon is IPokemonMultipleForms f1 ? f1.form : 0); //rescue 0
				if (ResolveAudioSE(filename) == null) {
					filename=string.Format("Cries/{0}Cry",pokemon.Species.ToString()); //rescue null;
				}
				if (ResolveAudioSE(filename) == null) filename=string.Format("Cries/{0}Cry",pokemon.Species);
				if (ResolveAudioSE(filename) != null) return filename;
			}
			return null;
		}

		public IAudioBGM GetWildBattleBGM(Pokemons species) {
			if (Global.nextBattleBGM != null) {
				return (IAudioBGM)Global.nextBattleBGM.Clone();
			}
			IAudioBGM ret=null;
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//IPokemonMetadata music=GetMetadata(GameMap.map_id,MetadataMapWildBattleBGM);
				string music=GetMetadata(GameMap.map_id).Map.WildBattleBGM;
				if (music != null && music!="") {
					ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//IPokemonMetadata music=GetMetadata(0,MetadataWildBattleBGM);
				string music=GetMetadata(0).Map.WildBattleBGM;
				if (music != null && music!="") {
					ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile("002-Battle02");
			return ret;
		}

		public IAudioME GetWildVictoryME() {
			if (Global.nextBattleME != null) {
				return (IAudioME)Global.nextBattleME.Clone();
			}
			IAudioME ret=null;
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//string music=GetMetadata(GameMap.map_id,MetadataMapWildVictoryME);
				string music=GetMetadata(GameMap.map_id).Map.WildVictoryME;
				if (music != null && music!="") {
					ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//string music=GetMetadata(0,MetadataWildVictoryME);
				string music=GetMetadata(GameMap.map_id).Map.WildVictoryME;
				if (music != null && music!="") {
					ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile("001-Victory01");
			ret.name="../../Audio/ME/"+ret.name;
			return ret;
		}

		public void PlayTrainerIntroME(TrainerTypes trainertype) {
			//RgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				if (Kernal.TrainerMetaData.ContainsKey(trainertype)) { //Kernal.TrainerMetaData[trainertype] != null
					string bgm=Kernal.TrainerMetaData[trainertype].IntroME; //trainertypes[trainertype][6];
					if (!string.IsNullOrEmpty(bgm)) { //!=null && bgm!=""
						IAudioME bgm_=(IAudioME)(this as IGameAudioPlay).StringToAudioFile(bgm);
						(this as IGameAudioPlay).MEPlay(bgm_);
						return;
					}
				}
			//}
		}

		// can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
		public IAudioBGM GetTrainerBattleBGM(params ITrainer[] trainer) {
			if (Global.nextBattleBGM != null) {
				return (IAudioBGM)Global.nextBattleBGM.Clone();
			}
			string music=null;
			//RgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				ITrainer[] trainerarray=new ITrainer[0];
				if (trainer == null) { //is Array && trainer.Length > 1
					trainerarray = new ITrainer[0]; //{ trainer[0] };
				} else {
					trainerarray=trainer;
				}
				for (int i = 0; i < trainerarray.Length; i++) {
					TrainerTypes trainertype=trainerarray[i].trainertype;
					if (Kernal.TrainerMetaData.ContainsKey(trainertype)) { //trainertypes[trainertype] != null
						music=Kernal.TrainerMetaData[trainertype].BattleBGM; //trainertypes[trainertype][4];
					}
				}
			//}
			IAudioBGM ret=null;
			if (music != null && music!="") {
				ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
			}
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//music=GetMetadata(GameMap.map_id,MetadataMapTrainerBattleBGM);
				music=GetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//music=GetMetadata(0,MetadataTrainerBattleBGM);
				music=GetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile("005-Boss01");
			return ret;
		}

		public IAudioBGM GetTrainerBattleBGMFromType(TrainerTypes trainertype) {
			if (Global.nextBattleBGM != null) {
				return (IAudioBGM)Global.nextBattleBGM.Clone();
			}
			string music=null;
			//RgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				if (Kernal.TrainerMetaData.ContainsKey(trainertype)) { //trainertypes[trainertype] != null
					music=Kernal.TrainerMetaData[trainertype].BattleBGM; //trainertypes[trainertype][4];
				}
			//}
			IAudioBGM ret=null;
			if (music!=null && music!="") {
				ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
			}
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//music=GetMetadata(GameMap.map_id,MetadataMapTrainerBattleBGM);
				music=GetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//music=GetMetadata(0,MetadataTrainerBattleBGM);
				music=GetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) ret=(IAudioBGM)(this as IGameAudioPlay).StringToAudioFile("005-Boss01");
			return ret;
		}

		// can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
		public IAudioME GetTrainerVictoryME(params ITrainer[] trainer) {
			if (Global.nextBattleME != null) {
				return (IAudioME)Global.nextBattleME.Clone();
			}
			string music=null;
			//RgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				//if (!trainer is Array) {
				//	trainerarray= new []{ trainer };
				//} else {
					ITrainer[] trainerarray=trainer;
				//}
				for (int i = 0; i < trainerarray?.Length; i++) {
					TrainerTypes trainertype=trainerarray[i].trainertype;
					if (Kernal.TrainerMetaData.ContainsKey(trainertype)) { //trainertypes[trainertype] != null
						music=Kernal.TrainerMetaData[trainertype].VictoryBGM; //trainertypes[trainertype][5];
					}
				}
			//}
			IAudioME ret=null;
			if (music!=null && music!="") {
				ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile(music);
			}
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//music=GetMetadata(GameMap.map_id,MetadataMapTrainerVictoryME);
				music=GetMetadata(GameMap.map_id).Map.TrainerVictoryME;
				if (music!=null && music!="") {
					ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//music=GetMetadata(0,MetadataTrainerVictoryME);
				music=GetMetadata(GameMap.map_id).Map.TrainerVictoryME;
				if (music!=null && music!="") {
					ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile(music);
				}
			}
			if (ret == null) ret=(IAudioME)(this as IGameAudioPlay).StringToAudioFile("001-Victory01");
			ret.name="../../Audio/ME/"+ret.name;
			return ret;
		}
		#endregion

		#region Creating and storing Pokémon
		/// <summary>
		/// For demonstration purposes only, not to be used in a real game.
		/// </summary>
		public void CreatePokemon() { }

		public bool BoxesFull() {
			return Trainer == null || (Trainer.party.Length==Features.LimitPokemonPartySize && PokemonStorage.full);
		}

		public void Nickname(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			string speciesname=Game._INTL(pokemon.Species.ToString(TextScripts.Name));
			if ((this as IGameMessage).ConfirmMessage(Game._INTL("Would you like to give a nickname to {1}?",speciesname))) {
				string helptext=Game._INTL("{1}'s nickname?",speciesname);
				string newname=this is IGameTextEntry t ? t.EnterPokemonName(helptext,0,Pokemon.NAMELIMIT,"",pokemon) : speciesname;
				//if (newname!="") pokemon.Name=newname;
				if (newname!="") (pokemon as Pokemon).SetNickname(newname);
			}
		}

		public void StorePokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			if (BoxesFull()) {
				(this as IGameMessage).Message(Game._INTL(@"There's no more room for Pokémon!\1"));
				(this as IGameMessage).Message(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return;
			}
			pokemon.RecordFirstMoves();
			if (Trainer.party.Length < Features.LimitPokemonPartySize) {
				//ToDo: Change to `.Add(Pokemon)`?
				Trainer.party[Trainer.party.Length]=pokemon;
			} else {
				int oldcurbox=PokemonStorage.currentBox;
				int storedbox=PokemonStorage.StoreCaught(pokemon);
				string curboxname=PokemonStorage[oldcurbox].name;
				string boxname=PokemonStorage[storedbox].name;
				string creator=null;
				if (Global.seenStorageCreator) creator=GetStorageCreator();
				if (storedbox!=oldcurbox) {
					if (!string.IsNullOrEmpty(creator)) {
						(this as IGameMessage).Message(Game._INTL(@"Box ""{1}"" on {2}'s PC was full.\1",curboxname,creator));
					} else {
						(this as IGameMessage).Message(Game._INTL(@"Box ""{1}"" on someone's PC was full.\1",curboxname));
					}
					(this as IGameMessage).Message(Game._INTL("{1} was transferred to box \"{2}.\"",pokemon.Name,boxname));
				} else {
					if (!string.IsNullOrEmpty(creator)) {
						(this as IGameMessage).Message(Game._INTL(@"{1} was transferred to {2}'s PC.\1",pokemon.Name,creator));
					} else {
						(this as IGameMessage).Message(Game._INTL(@"{1} was transferred to someone's PC.\1",pokemon.Name));
					}
					(this as IGameMessage).Message(Game._INTL("It was stored in box \"{1}.\"",boxname));
				}
			}
		}

		public void NicknameAndStore(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			if (BoxesFull()) {
				(this as IGameMessage).Message(Game._INTL(@"There's no more room for Pokémon!\1"));
				(this as IGameMessage).Message(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return;
			}
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			Nickname(pokemon);
			StorePokemon(pokemon);
		}

		public bool AddPokemon(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null) return false;
			IPokemon pokemon = null;
			if (BoxesFull()) {
				(this as IGameMessage).Message(Game._INTL(@"There's no more room for Pokémon!\1"));
				(this as IGameMessage).Message(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return false;
			}
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(Species,pokemon);
			//}
			if (level != null) { //pokemon is Pokemons && level is int
				//pokemon=new Pokemon(pokemon.Species,level:(byte)level.Value,original:Trainer);
				pokemon=new Monster.Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			string speciesname=Game._INTL(pokemon.Species.ToString(TextScripts.Name));
			(this as IGameMessage).Message(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			NicknameAndStore(pokemon);
			if (seeform) SeenForm(pokemon);
			return true;
		}

		public bool AddPokemon(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null) return false;
			if (BoxesFull()) {
				(this as IGameMessage).Message(Game._INTL(@"There's no more room for Pokémon!\1"));
				(this as IGameMessage).Message(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return false;
			}
			string speciesname=Game._INTL(pokemon.Species.ToString(TextScripts.Name));
			(this as IGameMessage).Message(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			NicknameAndStore(pokemon);
			if (seeform) SeenForm(pokemon);
			return true;
		}

		public bool AddPokemonSilent(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || BoxesFull() || Trainer == null) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(Species,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) SeenForm(pokemon);
			pokemon.RecordFirstMoves();
			if (Trainer.party.Length<Features.LimitPokemonPartySize) {
				//ToDo: Change to `.Add(Pokemon)`?
				Trainer.party[Trainer.party.Length]=pokemon;
			} else {
				PokemonStorage.StoreCaught(pokemon);
			}
			return true;
		}

		public bool AddPokemonSilent(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || BoxesFull() || Trainer == null) return false;
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) SeenForm(pokemon);
			pokemon.RecordFirstMoves();
			if (Trainer.party.Length<Features.LimitPokemonPartySize) {
				//ToDo: Change to `.Add(Pokemon)`?
				Trainer.party[Trainer.party.Length]=pokemon;
			} else {
				PokemonStorage.StoreCaught(pokemon);
			}
			return true;
		}

		public bool AddToParty(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(Species,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			string speciesname=Game._INTL(pokemon.Species.ToString(TextScripts.Name));
			(this as IGameMessage).Message(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			NicknameAndStore(pokemon);
			if (seeform) SeenForm(pokemon);
			return true;
		}

		public bool AddToParty(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			string speciesname=Game._INTL(pokemon.Species.ToString(TextScripts.Name));
			(this as IGameMessage).Message(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			NicknameAndStore(pokemon);
			if (seeform) SeenForm(pokemon);
			return true;
		}

		public bool AddToPartySilent(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(Species,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) SeenForm(pokemon);
			pokemon.RecordFirstMoves();
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool AddToPartySilent(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) SeenForm(pokemon);
			pokemon.RecordFirstMoves();
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool AddForeignPokemon(Pokemons pkmn,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(Species,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			//Set original trainer to a foreign one (if ID isn't already foreign)
			if (pokemon.trainerID==Trainer.id) {
				pokemon.trainerID=Trainer.getForeignID();
				if (!string.IsNullOrEmpty(ownerName)) pokemon.ot=ownerName;
				pokemon.otgender=ownerGender;
			}
			//Set nickname
			//if (!string.IsNullOrEmpty(nickname)) pokemon.Name=nickname.Substring(0,10);
			if (!string.IsNullOrEmpty(nickname)) (pokemon as Pokemon).SetNickname(nickname.Substring(0,10));
			//Recalculate stats
			pokemon.calcStats();
			if (ownerName != null) {
				(this as IGameMessage).Message(Game._INTL("{1} received a Pokémon from {2}.\\se[PokemonGet]\\1",Trainer.name,ownerName));
			} else {
				(this as IGameMessage).Message(Game._INTL("{1} received a Pokémon.\\se[PokemonGet]\\1",Trainer.name));
			}
			StorePokemon(pokemon);
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) SeenForm(pokemon);
			return true;
		}

		public bool AddForeignPokemon(IPokemon pokemon,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			//Set original trainer to a foreign one (if ID isn't already foreign)
			if (pokemon.trainerID==Trainer.id) {
				pokemon.trainerID=Trainer.getForeignID();
				if (!string.IsNullOrEmpty(ownerName)) pokemon.ot=ownerName;
				pokemon.otgender=ownerGender;
			}
			//Set nickname
			//if (!string.IsNullOrEmpty(nickname)) pokemon.Name=nickname.Substring(0,10);
			if (!string.IsNullOrEmpty(nickname)) (pokemon as Pokemon).SetNickname(nickname.Substring(0,10));
			//Recalculate stats
			pokemon.calcStats();
			if (ownerName != null) {
				(this as IGameMessage).Message(Game._INTL("{1} received a Pokémon from {2}.\\se[PokemonGet]\\1",Trainer.name,ownerName));
			} else {
				(this as IGameMessage).Message(Game._INTL("{1} received a Pokémon.\\se[PokemonGet]\\1",Trainer.name));
			}
			StorePokemon(pokemon);
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) SeenForm(pokemon);
			return true;
		}

		public bool GenerateEgg(Pokemons pkmn,string text="") {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(Species,pokemon);
			//}
			//if (pokemon is int) {
				//pokemon=new Pokemon(pokemon.Species,level:Core.EGGINITIALLEVEL,orignal:Trainer);
				pokemon=new Pokemon(pokemon.Species,isEgg:true);
			//}
			// Get egg steps
			//dexdata=OpenDexData();
			//DexDataOffset(dexdata,pokemon.Species,21);
			int eggsteps=0; //dexdata.fgetw();
			//dexdata.close();
			// Set egg's details
			//pokemon.Name=Game._INTL("Egg"); //ToDo: Uncomment and assign?
			pokemon.EggSteps = eggsteps;
			pokemon.obtainText=text;
			pokemon.calcStats();
			// Add egg to party
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool GenerateEgg(IPokemon pokemon,string text="") {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			// Get egg steps
			//dexdata=OpenDexData();
			//DexDataOffset(dexdata,pokemon.Species,21);
			int eggsteps=0; //dexdata.fgetw();
			//dexdata.close();
			// Set egg's details
			//pokemon.Name=Game._INTL("Egg"); //ToDo: Uncomment and assign?
			pokemon.EggSteps = eggsteps;
			pokemon.obtainText=text;
			pokemon.calcStats();
			// Add egg to party
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool RemovePokemonAt(int index) {
			if (index<0 || Trainer == null || index>=Trainer.party.Length) return false;
			bool haveAble=false;
			for (int i = 0; i < Trainer.party.Length; i++) {
				if (i==index) continue;
				if (Trainer.party[i].HP>0 && !Trainer.party[i].isEgg) haveAble=true;
			}
			if (!haveAble) return false;
			//Trainer.party.delete_at(index);
			Trainer.party[index] = new Pokemon();
			return true;
		}

		public void SeenForm(IPokemon poke) {//, int gender = 0, int form = 0
			//if (Trainer.formseen==null) Trainer.formseen=new[];
			//if (Trainer.formlastseen==null) Trainer.formlastseen=new[];
			//if (poke is String || poke is Symbol) {
			//  poke=getID(Species,poke);
			//}
			Pokemons species = Pokemons.NONE;
			//if (poke is IPokemon) {
				int gender=0; //poke.Gender;
				int form = poke is IPokemonMultipleForms f ? f.form : 0; //rescue 0
				species = poke.Species;
			//}
			SeenForm(species, gender, form);
		}

		public void SeenForm(Pokemons poke,int gender=0,int form=0) { //ToDo: redo this function...
			if (Trainer.formseen==null) Trainer.formseen=new int?[0][] { };
			if (Trainer.formlastseen==null) Trainer.formlastseen=new KeyValuePair<int, int?>[0]; //int?[0];
			//if (poke is String || poke is Symbol) {
			//  poke=getID(Species,poke);
			//}
			Pokemons species = Pokemons.NONE;
			//if (poke is IPokemon) {
			//	//gender=poke.Gender;
			//	form=poke.FormId; //rescue 0
			//	species=poke.Species;
			//} else {
				species=poke;
			//}
			if (species<=0) return; //!species ||
			if (gender>1) gender=0;
			string formnames = ""; //GetMessage(MessageTypes.FormNames,species);
			if (string.IsNullOrEmpty(formnames)) form=0; //ToDo: Rework pokedex logic below...
			//if (Trainer.formseen[(int)species] == null) Trainer.formseen[(int)species] = new int?[0];
			//Trainer.formseen[(int)species][gender][form]=true;
			//if (Trainer.formlastseen[species] == null) Trainer.formlastseen[species]=new [];
			//if (Trainer.formlastseen[species] == []) Trainer.formlastseen[species]= new []{ gender,form };
			//if (Trainer.formlastseen[(int)species].Value == null) Trainer.formlastseen[(int)species] = new KeyValuePair<int, int?>(gender,form);
			//if(Player.Pokedex[(int)species, 2] < 0)
			//Player.Pokedex[(int)species,2] = (byte)form;
		}
		#endregion

		#region Analysing Pokémon
		// Heals all Pokémon in the party.
		public void HealAll() {
			if (Trainer == null) return;
			foreach (IPokemon i in Trainer.party) {
				i.Heal();
			}
		}

		// Returns the first unfainted, non-egg Pokémon in the player's party.
		public IPokemon FirstAblePokemon(int variableNumber) {
			for (int i = 0; i < Trainer.party.Length; i++) {
				IPokemon p=Trainer.party[i];
				if (p != null && !p.isEgg && p.HP>0) {
					Set(variableNumber,i);
					return Trainer.party[i];
				}
			}
			Set(variableNumber,-1);
			return null;
		}

		// Checks whether the player would still have an unfainted Pokémon if the
		// Pokémon given by _pokemonIndex_ were removed from the party.
		public bool CheckAble(int pokemonIndex) {
			for (int i = 0; i < Trainer.party.Length; i++) {
				IPokemon p=Trainer.party[i];
				if (i==pokemonIndex) continue;
				if (p.IsNotNullOrNone() && !p.isEgg && p.HP>0) return true;
			}
			return false;
		}

		// Returns true if there are no usable Pokémon in the player's party.
		public bool AllFainted() {
			foreach (var i in Trainer.party) {
				if (!i.isEgg && i.HP>0) return false;
			}
			return true;
		}

		public double BalancedLevel(IPokemon[] party) {
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
		public int Size(IPokemon pokemon) {
			//dexdata=OpenDexData();
			//DexDataOffset(dexdata,pokemon.Species,33);
			//int baseheight=dexdata.fgetw(); // Gets the base height in tenths of a meter
			float baseheight=Kernal.PokemonData[pokemon.Species].Height;
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
		public bool HasEgg (Pokemons species) {
			//if (species is String || species is Symbol) {
			//  species=getID(Species,species);
			//}
			//Pokemons[][] evospecies=GetEvolvedFormData(species); //Kernal.PokemonEvolutionsData[species][0].
			//Kernal.PokemonData[species].IsBaby
			//Pokemons compatspecies=(evospecies != null && evospecies[0] != null) ? evospecies[0][2] : species;
			//dexdata=OpenDexData();
			//DexDataOffset(dexdata,compatspecies,31);
			EggGroups compat1=Kernal.PokemonData[species].EggGroup[0]; //dexdata.fgetb();   // Get egg group 1 of this species
			EggGroups compat2=Kernal.PokemonData[species].EggGroup[1]; //dexdata.fgetb();   // Get egg group 2 of this species
			//dexdata.close();
			if (compat1 == EggGroups.DITTO ||
				compat1 == EggGroups.UNDISCOVERED ||
				compat2 == EggGroups.DITTO ||
				compat2 == EggGroups.UNDISCOVERED) return false;
			Pokemons baby=GetBabySpecies(species);
			if (species==baby) return true;	// Is a basic species
			baby=GetBabySpecies(species,0,0);
			if (species==baby) return true;	// Is an egg species without incense
			return false;
		}
		#endregion

		#region Look through Pokémon in storage, choose a Pokémon in the party
		/// <summary>
		/// Yields every Pokémon/egg in storage in turn.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<KeyValuePair<IPokemon,int>> EachPokemon() {
			for (int i = -1; i < PokemonStorage.maxBoxes; i++) {
				for (int j = 0; j < PokemonStorage.maxPokemon(i); j++) {
					IPokemon poke=PokemonStorage[i][j];
					if (poke.IsNotNullOrNone()) yield return new KeyValuePair<IPokemon,int>(poke,i);
				}
			}
		}

		/// <summary>
		/// Yields every Pokémon in storage in turn.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<KeyValuePair<IPokemon,int>> EachNonEggPokemon() {
			//EachPokemon(){|pokemon,box|
			foreach(KeyValuePair<IPokemon,int> pokemon in EachPokemon()) {
				//if (!pokemon.isEgg) yield return (pokemon,box);
				if (!pokemon.Key.isEgg) yield return pokemon; //(pokemon.Key,pokemon.Value);
			}
		}

		/// <summary>
		/// Choose a Pokémon/egg from the party.
		/// Stores result in variable <paramref name="variableNumber"/> and the chosen Pokémon's name in
		/// variable <paramref name="nameVarNumber"/>; result is -1 if no Pokémon was chosen
		/// </summary>
		/// <param name="variableNumber">Stores result in variable</param>
		/// <param name="nameVarNumber">the chosen Pokémon's name in variable</param>
		/// <param name="ableProc">an array of which pokemons are affected</param>
		/// <param name="allowIneligible"></param>
		/// Supposed to return a value of pokemon chosen by player... as an int...
		/// ToDo: Instead of assigning value to variable, change void to return int
		public void ChoosePokemon(int variableNumber,int nameVarNumber,Predicate<IPokemon> ableProc=null,bool allowIneligible=false) {
			int chosen=0;
			FadeOutIn(99999, block: () => {
				IPartyDisplayScene scene=Scenes.Party; //new PokemonScreen_Scene();
				IPartyDisplayScreen screen=Screens.Party.initialize(scene,Trainer.party); //new PokemonScreen(scene,Trainer.party);
				if (ableProc != null) {
					chosen=screen.ChooseAblePokemon(ableProc,allowIneligible);
				} else {
					screen.StartScene(Game._INTL("Choose a Pokémon."),false);
					chosen=screen.ChoosePokemon();
					screen.EndScene();
				}
			});
			Set(variableNumber,chosen);
			if (chosen>=0) {
				Set(nameVarNumber,Trainer.party[chosen].Name);
			} else {
				Set(nameVarNumber,"");
			}
		}

		public void ChooseNonEggPokemon(int variableNumber,int nameVarNumber) {
			//ChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
			ChoosePokemon(variableNumber,nameVarNumber, poke =>
				!poke.isEgg
			);
		}

		public void ChooseAblePokemon(int variableNumber,int nameVarNumber) {
			//ChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
			ChoosePokemon(variableNumber,nameVarNumber, poke =>
				!poke.isEgg && poke.HP>0
			);
		}

		public void ChoosePokemonForTrade(int variableNumber,int nameVarNumber,Pokemons wanted) {
			//ChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
			ChoosePokemon(variableNumber, nameVarNumber, poke =>
				//if (wanted is String || wanted is Symbol) {
				//  wanted=getID(Species,wanted);
				//}
				!poke.isEgg && !((IPokemonShadowPokemon)poke).isShadow && poke.Species==wanted ///rescue false
			);
		}
		#endregion

		#region Checks through the party for something
		public bool HasSpecies (Pokemons species) {
			//if (species is String || species is Symbol) {
			//  species=getID(Species,species);
			//}
			foreach (var pokemon in Trainer.party) {
				if (pokemon.isEgg) continue;
				if (pokemon.Species==species) return true;
			}
			return false;
		}

		public bool HasFatefulSpecies (Pokemons species) {
			//if (species is String || species is Symbol) {
			//  species=getID(Species,species);
			//}
			foreach (IPokemon pokemon in Trainer.party) {
				if (pokemon.isEgg) continue;
				//if (pokemon.Species==species && pokemon.ObtainedMode==Overworld.ObtainedMethod.FATEFUL_ENCOUNTER) return true;
				if (pokemon.Species==species && pokemon.obtainMode==(int)Overworld.ObtainedMethod.FATEFUL_ENCOUNTER) return true;
			}
			return false;
		}

		public bool HasType (Types type) {
			//if (type is String || type is Symbol) {
			//  type=getID(Types,type);
			//}
			foreach (var pokemon in Trainer.party) {
				if (pokemon.isEgg) continue;
				if (pokemon.hasType(type)) return true;
			}
			return false;
		}

		// Checks whether any Pokémon in the party knows the given move, and returns
		// the index of that Pokémon, or null if no Pokémon has that move.
		public IPokemon CheckMove(Moves move) {
			//move=getID(Moves,move);
			if (move<=0) return null; //!move ||
			foreach (IPokemon i in Trainer.party) {
				if (i.isEgg) continue;
				foreach (IMove j in i.moves) {
					if (j.id==move) return i;
				}
			}
			return null;
		}
		#endregion

		#region Regional and National Pokédexes
		// Gets the Regional Pokédex number of the national species for the specified
		// Regional Dex.  The parameter "region" is zero-based.  For example, if two
		// regions are defined, they would each be specified as 0 and 1.
		public int GetRegionalNumber(int region,Pokemons nationalSpecies) {
			if (nationalSpecies<=0 || (int)nationalSpecies>Kernal.PokemonData.Count) {
				//  Return 0 if national species is outside range
				return 0;
			}
			//RgssOpen("Data/regionals.dat","rb"){|f|
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
		public int GetNationalNumber(int region,Pokemons regionalSpecies) {
			//RgssOpen("Data/regionals.dat","rb"){|f|
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
		public Pokemons[] AllRegionalSpecies(int region) {
			Pokemons[] ret= new Pokemons[]{ 0 };
			//RgssOpen("Data/regionals.dat","rb"){|f|
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

		/// <summary>
		/// Gets the ID number for the current region based on the player's current
		/// position.
		/// </summary>
		/// <param name="defaultRegion">
		/// Returns the value of "defaultRegion" (optional, default is -1) if
		/// no region was defined in the game's metadata.
		/// </param>
		/// <returns>
		/// The ID numbers returned by
		/// this function depend on the current map's position metadata.
		/// </returns>
		public int GetCurrentRegion(int defaultRegion=-1) {
			//int[] mappos=GameMap == null ? null : (int[])GetMetadata(GameMap.map_id,MapMetadatas.MetadataMapPosition);
			ITilePosition mappos=GameMap == null ? null : GetMetadata(GameMap.map_id).Map.MapPosition;
			if (mappos == null) {
				return defaultRegion; // No region defined
			} else {
				return mappos.MapId; //[0];
			}
		}

		/// <summary>
		/// Decides which Dex lists are able to be viewed (i.e. they are unlocked and have
		/// at least 1 seen species in them), and saves all viable dex region numbers
		/// (National Dex comes after regional dexes).<para>
		/// If the Dex list shown depends on the player's location, this just decides if
		/// a species in the current region has been seen - doesn't look at other regions.</para>
		/// Here, just used to decide whether to show the Pokédex in the Pause menu.
		/// </summary>
		public void SetViableDexes() {
			Global.pokedexViable=new List<int>();
			if (Core.DEXDEPENDSONLOCATION) {
				int region=GetCurrentRegion();
				if (region>=Global.pokedexUnlocked.Length-1) region=-1;
				if (Trainer.pokedexSeen((Regions)region)>0) {
					Global.pokedexViable.Add(region); //[0]=region;
				}
			} else {
				int numDexes=Global.pokedexUnlocked.Length;
				switch (numDexes) {
					case 1:          // National Dex only
						if (Global.pokedexUnlocked[0] != null) {
							if (Trainer.pokedexSeen()>0) {
								Global.pokedexViable.Add(0);
							}
						}
						break;
					default:            // Regional dexes + National Dex
						for (int i = 0; i < numDexes; i++) {
						//int regionToCheck=(i==numDexes-1) ? -1 : i;
						Regions? regionToCheck=(i==numDexes-1) ? (Regions?)null : (Regions)i;
							if (Global.pokedexUnlocked[i] != null) {
								if (Trainer.pokedexSeen(regionToCheck)>0) {
									Global.pokedexViable.Add(i);
								}
							}
						}
						break;
				}
			}
		}

		/// <summary>
		/// Unlocks a Dex list.  The National Dex is -1 here (or null argument).
		/// </summary>
		/// <param name="dex"></param>
		public void UnlockDex(int dex=-1) {
			int index=dex;
			if (index<0) index=Global.pokedexUnlocked.Length-1;
			if (index>Global.pokedexUnlocked.Length-1) index=Global.pokedexUnlocked.Length-1;
			Global.pokedexUnlocked[index]=true;
		}

		/// <summary>
		/// Locks a Dex list.  The National Dex is -1 here (or null argument).
		/// </summary>
		/// <param name="dex"></param>
		public void LockDex(int dex=-1) {
			int index=dex;
			if (index<0) index=Global.pokedexUnlocked.Length-1;
			if (index>Global.pokedexUnlocked.Length-1) index=Global.pokedexUnlocked.Length-1;
			Global.pokedexUnlocked[index]=false;
		}
		#endregion

		#region Other utilities
		public void TextEntry(string helptext,int minlength,int maxlength,int variableNumber) {
			if (this is IGameTextEntry t) GameVariables[variableNumber]=t.EnterText(helptext,minlength,maxlength);
			if (GameMap != null) GameMap.need_refresh = true;
		}

		public string[] MoveTutorAnnotations(Moves move,Pokemons[] movelist=null) {
			string[] ret=new string[Core.MAXPARTYSIZE];
			for (int i = 0; i < Core.MAXPARTYSIZE; i++) {
				ret[i]=null;
				if (i>=Trainer.party.Length) continue;
				bool found=false;
				for (int j = 0; j < 4; j++) {
					if (!Trainer.party[i].isEgg && Trainer.party[i].moves[j].id==move) {
						ret[i]=Game._INTL("LEARNED");
						found=true;
					}
				}
				if (found) continue;
				Pokemons species=Trainer.party[i].Species;
				if (!Trainer.party[i].isEgg && movelist != null && movelist.Any(j => j==species)) {
					//  Checked data from movelist
					ret[i]=Game._INTL("ABLE");
				} else if (!Trainer.party[i].isEgg && Trainer.party[i].isCompatibleWithMove(move)) {
					//  Checked data from S/tm.txt
					ret[i]=Game._INTL("ABLE");
				} else {
					ret[i]=Game._INTL("NOT ABLE");
				}
			}
			return ret;
		}

		public bool MoveTutorChoose(Moves move,Pokemons[] movelist=null,bool bymachine=false) {
			bool ret=false;
			//if (move is String || move is Symbol) {
			//  move=getID(Moves,move);
			//}
			if (movelist!=null && movelist.Length>0) { //&& movelist is Array
				for (int i = 0; i < movelist.Length; i++) {
					//if (movelist[i] is String || movelist[i] is Symbol) {
					//  movelist[i]=getID(Species,movelist[i]);
					//}
				}
			}
			FadeOutIn(99999, block: () => {
				IPartyDisplayScene scene=Scenes.Party; //new PokemonScreen_Scene();
				string movename=Game._INTL(move.ToString(TextScripts.Name));
				IPartyDisplayScreen screen=Screens.Party.initialize(scene,Trainer.party); //new PokemonScreen(scene,Trainer.party);
				string[] annot=MoveTutorAnnotations(move,movelist);
				screen.StartScene(Game._INTL("Teach which Pokémon?"),false,annot);
				do { //;loop
					int chosen=screen.ChoosePokemon();
					if (chosen>=0) {
						IPokemon pokemon=Trainer.party[chosen];
						if (pokemon.isEgg) {
							(this as IGameMessage).Message(Game._INTL("{1} can't be taught to an Egg.",movename));
						} else if (pokemon is IPokemonShadowPokemon p && p.isShadow) { //rescue false
							(this as IGameMessage).Message(Game._INTL("Shadow Pokémon can't be taught any moves."));
						} else if (movelist != null && !movelist.Any(j => j==pokemon.Species )) {
							(this as IGameMessage).Message(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
							(this as IGameMessage).Message(Game._INTL("{1} can't be learned.",movename));
						} else if (!pokemon.isCompatibleWithMove(move)) {
							(this as IGameMessage).Message(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
							(this as IGameMessage).Message(Game._INTL("{1} can't be learned.",movename));
						} else {
							if (LearnMove(pokemon,move,false,bymachine)) {
								ret=true;
								break;
							}
						}
					} else {
						break;
					}
				} while (true);
				screen.EndScene();
			});
			return ret; // Returns whether the move was learned by a Pokemon
		}

		public void ChooseMove(IPokemon pokemon,int variableNumber,int nameVarNumber) {
			if (!pokemon.IsNotNullOrNone()) return;
			int ret=-1;
			FadeOutIn(99999, block: () => {
				IPokemonSummaryScene scene=Scenes.Summary; //new PokemonSummaryScene();
				IPokemonSummaryScreen screen=Screens.Summary.initialize(scene); //new PokemonSummary(scene);
				ret=screen.StartForgetScreen(pokemon,0,0);
			});
			GameVariables[variableNumber]=ret;
			if (ret>=0) {
				GameVariables[nameVarNumber]=Game._INTL(pokemon.moves[ret].id.ToString(TextScripts.Name));
			} else {
				GameVariables[nameVarNumber]="";
			}
			if (GameMap != null) GameMap.need_refresh = true;
		}
		#endregion
	}
}
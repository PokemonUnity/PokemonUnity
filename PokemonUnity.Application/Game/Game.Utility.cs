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
		public static string _INTL(string message, params object[] param)
		{
			for (int i = 0; param.Length > i; i++) //(int i = param.Length; i > 0; i--)
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
		/// <param name="array"></param>
		/// <param name="num"></param>
		/// <returns></returns>
		//public IEnumerator<T[]> pbEachCombination<T>(T[] array,int num) {
		public IEnumerator<int[]> pbEachCombination(int[] array,int num) {
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
			//T[] arr=new T[num];
			int[] arr=new int[num];
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
		/// Gets the path of the user's "My Documents" folder.
		/// </summary>
		/// <returns></returns>
		//ToDo?...
		public string pbGetMyDocumentsFolder() { return ""; }

		/// <summary>
		/// Returns a country ID
		/// </summary>
		/// http://msdn.microsoft.com/en-us/library/dd374073%28VS.85%29.aspx?
		//ToDo?...
		public int pbGetCountry() { return 0; }

		/// <summary>
		/// Returns a language ID
		/// </summary>
		public int pbGetLanguage() {
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
		//public bool pbChangePlayer(int id) {
		public void pbChangePlayer(int id) {
			if (id < 0 || id >= 8) return; //false;
			//IPokemonMetadata meta=pbGetMetadata(0,MetadataPlayerA+id);
			MetadataPlayer meta=pbGetMetadata(0).Global.Players[MetadataPlayerA+id];
			if (meta == null) return; //false;
			if (Trainer != null) Trainer.trainertype=(TrainerTypes)meta.Type; //meta[0];
			GamePlayer.character_name=meta.Name; //meta[1];
			GamePlayer.character_hue=0;
			Global.playerID=id;
			if (Trainer != null) Trainer.metaID=id;
		}

		public string pbGetPlayerGraphic() {
			int id=Global.playerID;
			if (id<0 || id>=8) return "";
			//IPokemonMetadata meta=pbGetMetadata(0,MetadataPlayerA+id);
			MetadataPlayer meta=pbGetMetadata(0).Global.Players[MetadataPlayerA+id];
			if (meta == null) return "";
			return pbPlayerSpriteFile((TrainerTypes)meta.Type); //meta[0]
		}

		public TrainerTypes pbGetPlayerTrainerType() {
			int id=Global.playerID;
			if (id<0 || id>=8) return 0;
			//IPokemonMetadata meta=pbGetMetadata(0,MetadataPlayerA+id);
			MetadataPlayer meta=pbGetMetadata(0).Global.Players[MetadataPlayerA+id];
			if (meta == null) return 0;
			return (TrainerTypes)meta.Type; //meta[0];
		}

		public int pbGetTrainerTypeGender(TrainerTypes trainertype) {
		//public bool? pbGetTrainerTypeGender(TrainerTypes trainertype) {
			int? ret=2; // 2 = gender unknown
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
			//	if (!trainertypes[trainertype]) {
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
			if (Global.playerID<0) {
				pbChangePlayer(0);
			}
			TrainerTypes trainertype=pbGetPlayerTrainerType();
			string trname=name;
			Trainer=new Combat.Trainer(trname,trainertype);
			Trainer.outfit=outfit;
			if (trname==null) {
				trname=pbEnterPlayerName(Game._INTL("Your name?"),0,7);
				if (trname=="") {
					bool gender=pbGetTrainerTypeGender(trainertype) ;
					trname=pbSuggestTrainerName(gender);
				}
			}
			Trainer.name=trname;
			Bag=new Character.PokemonBag();
			PokemonTemp.begunNewGame=true;
		}

		public string pbSuggestTrainerName(bool gender) {
			string userName=pbGetUserName();
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

		public string pbGetUserName() {
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
		public void pbTimeEvent(int? variableNumber,int secs=86400) {
			if (variableNumber != null && variableNumber.Value>=0) {
				if (GameVariables != null) {
					if (secs<0) secs=0;
					DateTime timenow=Game.pbGetTimeNow();
					GameVariables[variableNumber.Value]=new float[] {timenow.Ticks,secs};
					if (GameMap != null) GameMap.refresh();
				}
			}
		}

		public void pbTimeEventDays(int? variableNumber,int days=0) {
			if (variableNumber != null && variableNumber>=0) {
				if (GameVariables != null) {
					if (days<0) days=0;
					DateTime timenow=Game.pbGetTimeNow();
					float time=timenow.Ticks;
					float expiry=(time%86400.0f)+(days*86400.0f);
					GameVariables[variableNumber.Value]= new []{ time,expiry-time };
					if (GameMap != null) GameMap.refresh();
				}
			}
		}

		public bool pbTimeEventValid(int? variableNumber) {
			bool retval=false;
			if (variableNumber != null && variableNumber>=0 && GameVariables != null) {
				object value=GameVariables[variableNumber.Value];
				if (value is float[] v && v.Length > 0) { //is Array
					DateTime timenow=Game.pbGetTimeNow();
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
		/// Similar to <see cref="pbFadeOutIn"/>, but pauses the music as it fades out.
		/// </summary>
		/// <remarks>
		/// Requires scripts "Audio" (for <seealso cref="IGameSystem.bgm_pause"/>) 
		/// and "SpriteWindow" (for <seealso cref="pbFadeOutIn"/>).
		/// </remarks>
		/// <param name="zViewport"></param>
		public void pbFadeOutInWithMusic(int zViewport, Action action = null) {
			PokemonEssentials.Interface.IAudioBGS playingBGS=GameSystem.getPlayingBGS();
			PokemonEssentials.Interface.IAudioBGM playingBGM=GameSystem.getPlayingBGM();
			GameSystem.bgm_pause(1.0f);
			GameSystem.bgs_pause(1.0f);
			int pos=GameSystem.bgm_position;
			pbFadeOutIn(zViewport, block: () => {
				if (action != null) action.Invoke(); //(block_given ?) yield;
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
		public int? getWaveDataUI(string filename,bool deleteFile=false) {
			int error=getWaveData(filename);
			if (deleteFile) {
				try {
					File.Delete(filename);
				} catch (Exception) { } //Errno::EINVAL, Errno::EACCES, Errno::ENOENT;
			}
			switch (error) {
				case 1:
					pbMessage(Game._INTL("The recorded data could not be found or saved."));
					break;
				case 2:
					pbMessage(Game._INTL("The recorded data was in an invalid format."));
					break;
				case 3:
					pbMessage(Game._INTL("The recorded data's format is not supported."));
					break;
				case 4:
					pbMessage(Game._INTL("There was no sound in the recording. Please ensure that a microphone is attached to the computer /and/ is ready."));
					break;
				default:
					return error;
			}
			return null;
		}

		/// <summary>
		/// Starts recording, and displays a message if the recording failed to start.
		/// </summary>
		/// <returns>Returns true if successful, false otherwise</returns>
		/// Requires the script AudioUtilities
		/// Requires the script "PokemonMessages"
		public bool beginRecordUI() {
			int code=beginRecord();
			switch (code) {
				case 0:
					return true;
				case 256+66:
					pbMessage(Game._INTL("All recording devices are in use. Recording is not possible now."));
					return false;
				case 256+72:
					pbMessage(Game._INTL("No supported recording device was found. Recording is not possible."));
					return false;
				default:
					string buffer=new int[256].ToString(); //"\0"*256;
					//MciErrorString.call(code,buffer,256);
					pbMessage(Game._INTL("Recording failed: {1}",buffer));//.gsub(/\x00/,"")
					return false;
			}
		}

		public IList<object> pbHideVisibleObjects() 
		{
			IList<object> visibleObjects= new List<object>();
			////ObjectSpace.each_object(Sprite){|o|
			//ObjectSpace.each_object(Sprite){|o|
			//	if (!o.disposed && o.visible) {
			//		visibleObjects.Add(o);
			//		o.visible=false;
			//	}
			//}
			////ObjectSpace.each_object(Viewport){|o|
			//ObjectSpace.each_object(Viewport){|o|
			//	if (!pbDisposed(o) && o.visible) {
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

		public void pbShowObjects(IList<object> visibleObjects) {
			foreach (var o in visibleObjects) {
				if (!pbDisposed(o)) {
					o.visible=true;
				}
			}
		}

		public void pbLoadRpgxpScene(ISceneMap scene) {
			if (!(Scene is ISceneMap)) return;
			ISceneMap oldscene=Scene;
			Scene=scene;
			Graphics.freeze();
			oldscene.disposeSpritesets();
			IList<object> visibleObjects=pbHideVisibleObjects();
			Graphics.transition(15);
			Graphics.freeze();
			while (Scene != null && !(Scene is ISceneMap)) {
				Scene.main();
			}
			Graphics.transition(15);
			Graphics.freeze();
			oldscene.createSpritesets();
			pbShowObjects(visibleObjects);
			Graphics.transition(20);
			Scene=oldscene;
		}

		/// <summary>
		/// Gets the value of a variable.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public object pbGet(int? id) {
			if (id == null || GameVariables == null) return 0;
			return GameVariables[id.Value];
		}

		/// <summary>
		/// Sets the value of a variable.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		public void pbSet(int? id,object value) {
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
		public bool pbCommonEvent(int id) {
			if (id<0) return false;
			IGameCommonEvent ce=DataCommonEvents[id];
			if (ce==null) return false;
			List<> celist=ce.list;
			IInterpreter interp=new Interpreter();
			interp.setup(celist,0);
			do {
				Graphics.update();
				Input.update();
				interp.update();
				pbUpdateSceneMap();
			} while (interp.running);
			return true;
		}

		public void pbExclaim(IGameCharacter[] @event,int id=Core.EXCLAMATION_ANIMATION_ID,bool tinting=false) {
			ISprite sprite=null;
			if (@event.Length > 0) { //is Array
				List<IGameCharacter> done=new List<IGameCharacter>();
				foreach (var i in @event) {
					if (!done.Contains(i.id)) {
						sprite=((ISpritesetMapAnimation)Scene).spriteset.addUserAnimation(id,i.x,i.y,tinting);
						done.Add(i.id);
					}
				}
			} else {
				sprite=((ISpritesetMapAnimation)Scene.spriteset).addUserAnimation(id,@event.x,@event.y,tinting);
			}
			while (!sprite.disposed()) {
				Graphics.update();
				Input.update();
				pbUpdateSceneMap();
			}
		}

		public void pbNoticePlayer(IGameCharacter @event) {
			if (!pbFacingEachOther(@event,GamePlayer)) {
				pbExclaim(new IGameCharacter[] { @event });
			}
			pbTurnTowardEvent(GamePlayer,@event);
			pbMoveTowardPlayer(@event);
		}
		#endregion

		#region Loads Pokémon/item/trainer graphics
		[System.Obsolete("Unused")] 
		public string pbPokemonBitmapFile(Pokemons species,bool shiny,bool back=false) {   
			if (shiny) {
				//  Load shiny bitmap
				string ret=string.Format("Graphics/Battlers/{0}s{1}",species.ToString(),back ? "b" : ""); //rescue null
				if (!pbResolveBitmap(ret)) {
					ret=string.Format("Graphics/Battlers/{0}s{1}",species,back ? "b" : "");
				}
				return ret;
			} else {
				//  Load normal bitmap
				string ret=string.Format("Graphics/Battlers/{0}{1}",species.ToString(),back ? "b" : ""); //rescue null
				if (!pbResolveBitmap(ret)) {
					ret=string.Format("Graphics/Battlers/{0}{1}",species,back ? "b" : "");
				}
				return ret;
			}
		}

		//public IAnimatedBitmap pbLoadPokemonBitmap(IPokemon pokemon,bool back=false) {
		//	return pbLoadPokemonBitmapSpecies(pokemon,pokemon.Species,back);
		//}

		// Note: Returns an AnimatedBitmap, not a Bitmap
		//public IAnimatedBitmap pbLoadPokemonBitmapSpecies(IPokemon pokemon,Pokemons species, bool back=false) {
		//	IAnimatedBitmap ret=null;
		//	//if (pokemon.isEgg?) {
		//	//	bitmapFileName=string.Format("Graphics/Battlers/%segg",species.ToString()); //rescue null
		//	//	if (!pbResolveBitmap(bitmapFileName)) {
		//	//		bitmapFileName=string.Format("Graphics/Battlers/%03degg",species);
		//	//		if (!pbResolveBitmap(bitmapFileName)) {
		//	//			bitmapFileName=string.Format("Graphics/Battlers/egg");
		//	//		}
		//	//	}
		//	//	bitmapFileName=pbResolveBitmap(bitmapFileName);
		//	//} else {
		//	//	bitmapFileName=pbCheckPokemonBitmapFiles([species,back,
		//	//												(pokemon.isFemale?),
		//	//												pokemon.isShiny?,
		//	//												(pokemon.form rescue 0),
		//	//												(pokemon.isShadow? rescue false)])
		//	//	//  Alter bitmap if supported
		//	//	alterBitmap=(MultipleForms.getFunction(species,"alterBitmap") rescue null);
		//	//}
		//	//if (bitmapFileName != null && alterBitmap) {
		//	//	animatedBitmap=new AnimatedBitmap(bitmapFileName);
		//	//	copiedBitmap=animatedBitmap.copy;
		//	//	animatedBitmap.dispose();
		//	//	copiedBitmap.each {|bitmap|
		//	//		alterBitmap.call(pokemon,bitmap);
		//	//	}
		//	//	ret=copiedBitmap;
		//	//} else if (bitmapFileName) {
		//	//	ret=new AnimatedBitmap(bitmapFileName);
		//	//}
		//	return ret;
		//}

		// Note: Returns an AnimatedBitmap, not a Bitmap
		//public IAnimatedBitmap pbLoadSpeciesBitmap(Pokemons species,bool female=false,int form=0,bool shiny=false,bool shadow=false,bool back=false,bool egg=false) {
		//	IAnimatedBitmap ret=null;
		//	//if (egg) {
		//	//	bitmapFileName=string.Format("Graphics/Battlers/%segg",getConstantName(PBSpecies,species)) rescue null;
		//	//	if (!pbResolveBitmap(bitmapFileName)) {
		//	//		bitmapFileName=string.Format("Graphics/Battlers/%03degg",species);
		//	//		if (!pbResolveBitmap(bitmapFileName)) {
		//	//			bitmapFileName=string.Format("Graphics/Battlers/egg");
		//	//		}
		//	//	}
		//	//	bitmapFileName=pbResolveBitmap(bitmapFileName);
		//	//} else {
		//	//	bitmapFileName=pbCheckPokemonBitmapFiles([species,back,female,shiny,form,shadow]);
		//	//}
		//	//if (bitmapFileName) {
		//	//	ret=new AnimatedBitmap(bitmapFileName);
		//	//}
		//	return ret;
		//}

		public string pbCheckPokemonBitmapFiles(params object[] args) {
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
			//		getConstantName(PBSpecies,species),
			//		tgender ? "f" : "",
			//		tshiny ? "s" : "",
			//		back ? "b" : "",
			//		(tform!="" ? "_"+tform : ""),
			//		tshadow ? "_shadow" : ""); //rescue null
			//	string ret=pbResolveBitmap(bitmapFileName);
			//	if (ret) return ret;
			//	bitmapFileName=string.Format("Graphics/Battlers/%03d%s%s%s%s%s",
			//		species,
			//		tgender ? "f" : "",
			//		tshiny ? "s" : "",
			//		back ? "b" : "",
			//		(tform!="" ? "_"+tform : ""),
			//		tshadow ? "_shadow" : "");
			//	ret=pbResolveBitmap(bitmapFileName);
			//	if (ret != null) return ret;
			//}
			return null;
		}

		public string pbLoadPokemonIcon(IPokemon pokemon) {
			return null; //new AnimatedBitmap(pbPokemonIconFile(pokemon)).deanimate;
		}

		public string pbPokemonIconFile(IPokemon pokemon) {
			string bitmapFileName=null;
			//bitmapFileName=pbCheckPokemonIconFiles([pokemon.Species,
			//										(pokemon.isFemale?),
			//										pokemon.isShiny?,
			//										(pokemon.form rescue 0),
			//										(pokemon.isShadow? rescue false)],
			//										pokemon.isEgg?);
			return bitmapFileName;
		}

		public string pbCheckPokemonIconFiles(object[] param,bool egg=false) {
			//Pokemons species=params[0];
			//if (egg) {
			//	bitmapFileName=string.Format("Graphics/Icons/icon%segg",getConstantName(PBSpecies,species)); //rescue null
			//	if (!pbResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Icons/icon%03degg",species) ;
			//		if (!pbResolveBitmap(bitmapFileName)) {
			//			bitmapFileName=string.Format("Graphics/Icons/iconEgg");
			//		}
			//	}
			//	return pbResolveBitmap(bitmapFileName);
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
			//			getConstantName(PBSpecies,species),
			//			tgender ? "f" : "",
			//			tshiny ? "s" : "",
			//			(tform!="" ? "_"+tform : ""),
			//			tshadow ? "_shadow" : "") rescue null;
			//		string ret=pbResolveBitmap(bitmapFileName);
			//		if (ret != null) return ret;
			//		bitmapFileName=string.Format("Graphics/Icons/icon%03d%s%s%s%s",
			//			species,
			//			tgender ? "f" : "",
			//			tshiny ? "s" : "",
			//			(tform!="" ? "_"+tform : ""),
			//			tshadow ? "_shadow" : "");
			//		ret=pbResolveBitmap(bitmapFileName);
			//		if (ret != null) return ret;
			//	}
			//}
			return null;
		}

		// Used by the Pokédex
		public string pbPokemonFootprintFile(Pokemons pokemon) {   
			//if (!pokemon) return null;
			string bitmapFileName = null;
			//if (pokemon is Numeric) {
				//bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%s",getConstantName(PBSpecies,pokemon)) rescue null;
				//if (!pbResolveBitmap(bitmapFileName)) bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%03d",pokemon);
			//}
			//return pbResolveBitmap(bitmapFileName);
			return null;
		}
		public string pbPokemonFootprintFile(IPokemon pokemon) {   
			if (pokemon == null) return null;
			string bitmapFileName = null;
			//{
			//	bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%s_%d",getConstantName(PBSpecies,pokemon.Species),(pokemon.form rescue 0)); //rescue null
			//	if (!pbResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%03d_%d",pokemon.Species,(pokemon.form rescue 0)); //rescue null
			//		if (!pbResolveBitmap(bitmapFileName)) {
			//			bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%s",getConstantName(PBSpecies,pokemon.Species)); //rescue null
			//			if (!pbResolveBitmap(bitmapFileName)) {
			//				bitmapFileName=string.Format("Graphics/Icons/Footprints/footprint%03d",pokemon.Species);
			//			}
			//		}
			//	}
			//}
			//return pbResolveBitmap(bitmapFileName);
			return null;
		}

		public string pbItemIconFile(Items item) {
			//if (!item) return null;
			string bitmapFileName=null;
			//if (item==0) {
			//	bitmapFileName=string.Format("Graphics/Icons/itemBack");
			//} else {
			//	bitmapFileName=string.Format("Graphics/Icons/item%s",getConstantName(PBItems,item)); //rescue null
			//	if (!pbResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=string.Format("Graphics/Icons/item%03d",item);
			//	}
			//}
			return bitmapFileName;
		}

		public string pbMailBackFile(Items item) {
			//if (!item) return null;
			//string bitmapFileName=string.Format("Graphics/Pictures/mail%s",getConstantName(PBItems,item)); //rescue null
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Pictures/mail%03d",item);
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbTrainerCharFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("Graphics/Characters/trchar%s",getConstantName(PBTrainers,type)) rescue null;
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trchar%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbTrainerCharNameFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("trchar%s",getConstantName(PBTrainers,type)) rescue null;
			//if (!pbResolveBitmap(string.Format("Graphics/Characters/"+bitmapFileName))) {
			//	bitmapFileName=string.Format("trchar%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbTrainerHeadFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%s",getConstantName(PBTrainers,type)) rescue null;
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbPlayerHeadFile(TrainerTypes type) {
			//if (!type) return null;
			//int outfit=Game.GameData.Trainer ? Game.GameData.Trainer.outfit : 0;
			//string bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%s_%d",
			//	getConstantName(PBTrainers,type),outfit); //rescue null
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Pictures/mapPlayer%03d_%d",type,outfit);
			//	if (!pbResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=pbTrainerHeadFile(type);
			//	}
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbTrainerSpriteFile(TrainerTypes type) {
			//if (!type) return null;
			//string bitmapFileName=string.Format("Graphics/Characters/trainer%s",getConstantName(PBTrainers,type)) rescue null;
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trainer%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbTrainerSpriteBackFile(TrainerTypes type) {
			//if (!type) return null;
			//bitmapFileName=string.Format("Graphics/Characters/trback%s",getConstantName(PBTrainers,type)) rescue null;
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trback%03d",type);
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbPlayerSpriteFile(TrainerTypes type) {
			//if (!type) return null;
			//int outfit=Game.GameData.Trainer ? Game.GameData.Trainer.outfit : 0;
			//string bitmapFileName=string.Format("Graphics/Characters/trainer%s_%d",
			//	getConstantName(PBTrainers,type),outfit); //rescue null
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trainer%03d_%d",type,outfit);
			//	if (!pbResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=pbTrainerSpriteFile(type);
			//	}
			//}
			//return bitmapFileName;
			return null;
		}

		public string pbPlayerSpriteBackFile(TrainerTypes type) {
			//if (!type) return null;
			//int outfit=Game.GameData.Trainer ? Game.GameData.Trainer.outfit : 0;
			//string bitmapFileName=string.Format("Graphics/Characters/trback%s_%d",
			//	getConstantName(PBTrainers,type),outfit); //rescue null
			//if (!pbResolveBitmap(bitmapFileName)) {
			//	bitmapFileName=string.Format("Graphics/Characters/trback%03d_%d",type,outfit);
			//	if (!pbResolveBitmap(bitmapFileName)) {
			//		bitmapFileName=pbTrainerSpriteBackFile(type);
			//	}
			//}
			//return bitmapFileName;
			return null;
		}
		#endregion

		#region Loads music and sound effects
		public string pbResolveAudioSE(string file) {
			if (file == null) return null;
			//if (RTP.exists("Audio/SE/"+file,new string[] { "", ".wav", ".mp3", ".ogg" })) {
			//	return RTP.getPath("Audio/SE/"+file,new string[] { "", ".wav", ".mp3", ".ogg" });
			//}
			return null;
		}
		
		public int pbCryFrameLength(IPokemon pokemon,float? pitch=null) {
			if (!pokemon.IsNotNullOrNone()) return 0;
			if (pitch == null) pitch=100;
			pitch=pitch/100;
			if (pitch<=0) return 0;
			float playtime=0.0f;
			if (pokemon is Numeric) {
				string pkmnwav=pbResolveAudioSE(pbCryFile(pokemon));
				if (pkmnwav != null) playtime=getPlayTime(pkmnwav);
			} else if (!pokemon.isEgg) {
				if (pokemon is IPokemonChatter p && p.chatter != null) { //pokemon.respond_to("chatter")
					playtime=p.chatter.time;
					pitch=1.0f;
				} else {
					string pkmnwav=pbResolveAudioSE(pbCryFile(pokemon));
					if (pkmnwav) playtime=getPlayTime(pkmnwav);
				}
			}
			playtime/=pitch; // sound is lengthened the lower the pitch
			//  4 is added to provide a buffer between sounds
			return (int)Math.Ceiling(playtime*Graphics.frame_rate)+4;
		}
		
		public void pbPlayCry(IPokemon pokemon,int volume=90,float? pitch=null) {
			if (pokemon == null) return;
			if (!pokemon.isEgg) {
				if (pokemon is IPokemonChatter p && p.chatter != null) { //pokemon.respond_to("chatter")
					p.chatter.play();
				} else {
					string pkmnwav=pbCryFile(pokemon);
					if (pkmnwav != null) {
						//pbSEPlay(new RPG.AudioFile(pkmnwav,volume,
						//	pitch != null ? pitch : (pokemon.HP*25/pokemon.TotalHP)+75)); //rescue null
					}
				}
			}
		}
		
		public void pbPlayCry(Pokemons pokemon,int volume=90,float? pitch=null) {
			if (pokemon == Pokemons.NONE) return;
			//if (pokemon is Numeric) {
				string pkmnwav=pbCryFile(pokemon);
				if (pkmnwav != null) {
					//pbSEPlay(new RPG.AudioFile(pkmnwav,volume,pitch != null ? pitch : 100)); //rescue null
				}
			//}
		}
		
		public string pbCryFile(Pokemons pokemon) {
			if (pokemon == Pokemons.NONE) return null;
			//if (pokemon is Numeric) {
				string filename=string.Format("Cries/{0}Cry",pokemon.ToString()); //rescue null
				if (!pbResolveAudioSE(filename)) filename=string.Format("Cries/%03dCry",pokemon);
				if (pbResolveAudioSE(filename)) return filename;
			//}
			return null;
		}
		
		public string pbCryFile(IPokemon pokemon) {
			if (pokemon == null) return null;
			if (!pokemon.isEgg) {
				string filename=string.Format("Cries/{0}Cry_{1}",pokemon.Species.ToString(),pokemon.form); //rescue 0 rescue null
				if (!pbResolveAudioSE(filename)) filename=string.Format("Cries/{0}Cry_{1}",pokemon.Species,pokemon.form); //rescue 0
				if (!pbResolveAudioSE(filename)) {
					filename=string.Format("Cries/{0}Cry",pokemon.Species.ToString()); //rescue null;
				}
				if (!pbResolveAudioSE(filename)) filename=string.Format("Cries/{0}Cry",pokemon.Species);
				if (pbResolveAudioSE(filename)) return filename;
			}
			return null;
		}
		
		public IAudioBGM pbGetWildBattleBGM(Pokemons species) {
			if (Global.nextBattleBGM != null) {
				return Global.nextBattleBGM.clone();
			}
			IAudioBGM ret=null;
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//IPokemonMetadata music=pbGetMetadata(GameMap.map_id,MetadataMapWildBattleBGM);
				string music=pbGetMetadata(GameMap.map_id).Map.WildBattleBGM;
				if (music != null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//IPokemonMetadata music=pbGetMetadata(0,MetadataWildBattleBGM);
				string music=pbGetMetadata(0).Map.WildBattleBGM;
				if (music != null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) ret=pbStringToAudioFile("002-Battle02");
			return ret;
		}
		
		public IAudioME pbGetWildVictoryME() {
			if (Global.nextBattleME != null) {
				return Global.nextBattleME.clone();
			}
			IAudioME ret=null;
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//string music=pbGetMetadata(GameMap.map_id,MetadataMapWildVictoryME);
				string music=pbGetMetadata(GameMap.map_id).Map.WildVictoryME;
				if (music != null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//string music=pbGetMetadata(0,MetadataWildVictoryME);
				string music=pbGetMetadata(GameMap.map_id).Map.WildVictoryME;
				if (music != null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) ret=pbStringToAudioFile("001-Victory01");
			ret.name="../../Audio/ME/"+ret.name;
			return ret;
		}
		
		public void pbPlayTrainerIntroME(TrainerTypes trainertype) {
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				if (trainertypes[trainertype] != null) {
					string bgm=trainertypes[trainertype][6];
					if (bgm!=null && bgm!="") {
						bgm=pbStringToAudioFile(bgm);
						pbMEPlay(bgm);
						return;
					}
				}
			//}
		}
		
		// can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
		public IAudioBGM pbGetTrainerBattleBGM(params ITrainer[] trainer) { 
			if (Global.nextBattleBGM != null) {
				return Global.nextBattleBGM.clone();
			}
			string music=null;
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				ITrainer[] trainerarray=new ITrainer[0];
				if (trainer == null) { //is Array && trainer.Length > 1
					trainerarray = new ITrainer[0]; //{ trainer[0] };
				} else {
					ITrainer[] trainerarray=trainer;
				}
				for (int i = 0; i < trainerarray.Length; i++) {
					TrainerTypes trainertype=trainerarray[i].trainertype;
					if (trainertypes[trainertype]) {
						music=trainertypes[trainertype][4];
					}
				}
			//}
			IAudioBGM ret=null;
			if (music != null && music!="") {
				ret=pbStringToAudioFile(music);
			}
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//music=pbGetMetadata(GameMap.map_id,MetadataMapTrainerBattleBGM);
				music=pbGetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//music=pbGetMetadata(0,MetadataTrainerBattleBGM);
				music=pbGetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) ret=pbStringToAudioFile("005-Boss01");
			return ret;
		}
		
		public IAudioBGM pbGetTrainerBattleBGMFromType(TrainerTypes trainertype) {
			if (Global.nextBattleBGM != null) {
				return Global.nextBattleBGM.clone();
			}
			string music=null;
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				if (trainertypes[trainertype]) {
					music=trainertypes[trainertype][4];
				}
			//}
			IAudioBGM ret=null;
			if (music!=null && music!="") {
				ret=pbStringToAudioFile(music);
			}
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//music=pbGetMetadata(GameMap.map_id,MetadataMapTrainerBattleBGM);
				music=pbGetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//music=pbGetMetadata(0,MetadataTrainerBattleBGM);
				music=pbGetMetadata(GameMap.map_id).Map.TrainerBattleBGM;
				if (music!=null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) ret=pbStringToAudioFile("005-Boss01");
			return ret;
		}
		
		// can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
		public IAudioME pbGetTrainerVictoryME(params ITrainer[] trainer) { 
			if (Global.nextBattleME != null) {
				return Global.nextBattleME.clone();
			}
			string music=null;
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//	trainertypes=Marshal.load(f);
				//if (!trainer is Array) {
				//	trainerarray= new []{ trainer };
				//} else {
					ITrainer[] trainerarray=trainer;
				//}
				for (int i = 0; i < trainerarray?.Length; i++) {
					trainertype=trainerarray[i].trainertype;
					if (trainertypes[trainertype] != null) {
						music=trainertypes[trainertype][5]; //trainertypes[trainertype][5]
					}
				}
			//}
			IAudioME ret=null;
			if (music!=null && music!="") {
				ret=pbStringToAudioFile(music);
			}
			if (ret == null && GameMap != null) {
				//  Check map-specific metadata
				//music=pbGetMetadata(GameMap.map_id,MetadataMapTrainerVictoryME);
				music=pbGetMetadata(GameMap.map_id).Map.TrainerVictoryME;
				if (music!=null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) {
				//  Check global metadata
				//music=pbGetMetadata(0,MetadataTrainerVictoryME);
				music=pbGetMetadata(GameMap.map_id).Map.TrainerVictoryME;
				if (music!=null && music!="") {
					ret=pbStringToAudioFile(music);
				}
			}
			if (ret == null) ret=pbStringToAudioFile("001-Victory01");
			ret.name="../../Audio/ME/"+ret.name;
			return ret;
		}
		#endregion

		#region Creating and storing Pokémon
		public bool pbBoxesFull() {
			return Trainer == null || (Trainer.party.Length==Features.LimitPokemonPartySize && PokemonStorage.full);
		}

		public void pbNickname(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			string speciesname=pokemon.Species.ToString(TextScripts.Name);
			if (UI.pbConfirmMessage(Game._INTL("Would you like to give a nickname to {1}?",speciesname))) {
				string helptext=Game._INTL("{1}'s nickname?",speciesname);
				string newname=UI.pbEnterPokemonName(helptext,0,Pokemon.NAMELIMIT,"",pokemon);
				//if (newname!="") pokemon.Name=newname;
				if (newname!="") pokemon.SetNickname(newname);
			}
		}

		public void pbStorePokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			if (pbBoxesFull()) {
				pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
				pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return;
			}
			pokemon.RecordFirstMoves();
			if (Trainer.party.Length < Features.LimitPokemonPartySize) {
				//ToDo: Change to `.Add(Pokemon)`?
				Trainer.party[Trainer.party.Length]=pokemon;
			} else {
				int oldcurbox=PokemonStorage.currentBox;
				int storedbox=PokemonStorage.pbStoreCaught(pokemon);
				string curboxname=PokemonStorage[oldcurbox].name;
				string boxname=PokemonStorage[storedbox].name;
				string creator=null;
				if (Global.seenStorageCreator) creator=pbGetStorageCreator();
				if (storedbox!=oldcurbox) {
					if (!string.IsNullOrEmpty(creator)) {
						pbMessage(Game._INTL(@"Box ""{1}"" on {2}'s PC was full.\1",curboxname,creator));
					} else {
						pbMessage(Game._INTL(@"Box ""{1}"" on someone's PC was full.\1",curboxname));
					}
					pbMessage(Game._INTL("{1} was transferred to box \"{2}.\"",pokemon.Name,boxname));
				} else {
					if (!string.IsNullOrEmpty(creator)) {
						pbMessage(Game._INTL(@"{1} was transferred to {2}'s PC.\1",pokemon.Name,creator));
					} else {
						pbMessage(Game._INTL(@"{1} was transferred to someone's PC.\1",pokemon.Name));
					}
					pbMessage(Game._INTL("It was stored in box \"{1}.\"",boxname));
				}
			}
		}

		public void pbNicknameAndStore(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			if (pbBoxesFull()) {
				pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
				pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return;
			}
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			pbNickname(pokemon);
			pbStorePokemon(pokemon);
		}

		public bool pbAddPokemon(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null) return false;
			IPokemon pokemon = null;
			if (pbBoxesFull()) {
				pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
				pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return false;
			}
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(PBSpecies,pokemon);
			//}
			if (level != null) { //pokemon is Pokemons && level is int
				//pokemon=new Pokemon(pokemon.Species,level:(byte)level.Value,original:Trainer);
				pokemon=new Monster.Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			string speciesname=pokemon.Species.ToString(TextScripts.Name);
			pbMessage(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			pbNicknameAndStore(pokemon);
			if (seeform) pbSeenForm(pokemon);
			return true;
		}

		public bool pbAddPokemon(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null) return false;
			if (pbBoxesFull()) {
				pbMessage(Game._INTL(@"There's no more room for Pokémon!\1"));
				pbMessage(Game._INTL("The Pokémon Boxes are full and can't accept any more!"));
				return false;
			}
			string speciesname=pokemon.Species.ToString(TextScripts.Name);
			pbMessage(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			pbNicknameAndStore(pokemon);
			if (seeform) pbSeenForm(pokemon);
			return true;
		}

		public bool pbAddPokemonSilent(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || pbBoxesFull() || Trainer == null) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(PBSpecies,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) pbSeenForm(pokemon);
			pokemon.RecordFirstMoves();
			if (Trainer.party.Length<Features.LimitPokemonPartySize) {
				//ToDo: Change to `.Add(Pokemon)`?
				Trainer.party[Trainer.party.Length]=pokemon;
			} else {
				PokemonStorage.pbStoreCaught(pokemon);
			}
			return true;
		}

		public bool pbAddPokemonSilent(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || pbBoxesFull() || Trainer == null) return false;
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) pbSeenForm(pokemon);
			pokemon.RecordFirstMoves();
			if (Trainer.party.Length<Features.LimitPokemonPartySize) {
				//ToDo: Change to `.Add(Pokemon)`?
				Trainer.party[Trainer.party.Length]=pokemon;
			} else {
				PokemonStorage.pbStoreCaught(pokemon);
			}
			return true;
		}

		public bool pbAddToParty(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(PBSpecies,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			string speciesname=pokemon.Species.ToString(TextScripts.Name);
			pbMessage(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			pbNicknameAndStore(pokemon);
			if (seeform) pbSeenForm(pokemon);
			return true;
		}

		public bool pbAddToParty(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			string speciesname=pokemon.Species.ToString(TextScripts.Name);
			pbMessage(Game._INTL(@"{1} obtained {2}!\\se[PokemonGet]\1",Trainer.name,speciesname));
			pbNicknameAndStore(pokemon);
			if (seeform) pbSeenForm(pokemon);
			return true;
		}

		public bool pbAddToPartySilent(Pokemons pkmn,int? level=null,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(PBSpecies,pokemon);
			//}
			if (level != null) { //pokemon is Integer && level is int
				pokemon=new Pokemon(pkmn,level:(byte)level.Value,original:Trainer);
			}
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) pbSeenForm(pokemon);
			pokemon.RecordFirstMoves();
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool pbAddToPartySilent(IPokemon pokemon,int? level=null,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) pbSeenForm(pokemon);
			pokemon.RecordFirstMoves();
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool pbAddForeignPokemon(Pokemons pkmn,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true) {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(PBSpecies,pokemon);
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
			if (!string.IsNullOrEmpty(nickname)) pokemon.Name=nickname.Substring(0,10);
			//Recalculate stats
			pokemon.calcStats();
			if (ownerName != null) {
				pbMessage(Game._INTL("{1} received a Pokémon from {2}.\\se[PokemonGet]\\1",Trainer.name,ownerName));
			} else {
				pbMessage(Game._INTL("{1} received a Pokémon.\\se[PokemonGet]\\1",Trainer.name));
			}
			pbStorePokemon(pokemon);
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) pbSeenForm(pokemon);
			return true;
		}

		public bool pbAddForeignPokemon(IPokemon pokemon,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true) {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			//Set original trainer to a foreign one (if ID isn't already foreign)
			if (pokemon.trainerID==Trainer.id) {
				pokemon.trainerID=Trainer.getForeignID();
				if (!string.IsNullOrEmpty(ownerName)) pokemon.ot=ownerName;
				pokemon.otgender=ownerGender;
			}
			//Set nickname
			if (!string.IsNullOrEmpty(nickname)) pokemon.Name=nickname.Substring(0,10);
			//Recalculate stats
			pokemon.calcStats();
			if (ownerName != null) {
				pbMessage(Game._INTL("{1} received a Pokémon from {2}.\\se[PokemonGet]\\1",Trainer.name,ownerName));
			} else {
				pbMessage(Game._INTL("{1} received a Pokémon.\\se[PokemonGet]\\1",Trainer.name));
			}
			pbStorePokemon(pokemon);
			Trainer.seen[pokemon.Species]=true;
			Trainer.owned[pokemon.Species]=true;
			if (seeform) pbSeenForm(pokemon);
			return true;
		}

		public bool pbGenerateEgg(Pokemons pkmn,string text="") {
			if (pkmn == Pokemons.NONE || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			IPokemon pokemon = null;
			//if (pokemon is String || pokemon is Symbol) {
			//  pokemon=getID(PBSpecies,pokemon);
			//}
			//if (pokemon is int) {
				//pokemon=new Pokemon(pokemon.Species,level:Core.EGGINITIALLEVEL,orignal:Trainer);
				pokemon=new Pokemon(pokemon.Species,isEgg:true);
			//}
			// Get egg steps
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,pokemon.Species,21);
			int eggsteps=0; //dexdata.fgetw();
			//dexdata.close();
			// Set egg's details
			pokemon.Name=Game._INTL("Egg");
			pokemon.eggsteps=eggsteps;
			pokemon.obtainText=text;
			pokemon.calcStats();
			// Add egg to party
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool pbGenerateEgg(IPokemon pokemon,string text="") {
			if (!pokemon.IsNotNullOrNone() || Trainer == null || Trainer.party.Length>=Features.LimitPokemonPartySize) return false;
			// Get egg steps
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,pokemon.Species,21);
			int eggsteps=0; //dexdata.fgetw();
			//dexdata.close();
			// Set egg's details
			pokemon.Name=Game._INTL("Egg");
			pokemon.eggsteps=eggsteps;
			pokemon.obtainText=text;
			pokemon.calcStats();
			// Add egg to party
			Trainer.party[Trainer.party.Length]=pokemon;
			return true;
		}

		public bool pbRemovePokemonAt(int index) {
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

		public void pbSeenForm(IPokemon poke) {//, int gender = 0, int form = 0
			//if (Trainer.formseen==null) Trainer.formseen=new[];
			//if (Trainer.formlastseen==null) Trainer.formlastseen=new[];
			//if (poke is String || poke is Symbol) {
			//  poke=getID(PBSpecies,poke);
			//}
			Pokemons species = Pokemons.NONE;
			//if (poke is IPokemon) {
				int gender=0; //poke.Gender;
				int form = poke.FormId; //rescue 0
				species = poke.Species;
			//}
			pbSeenForm(species, gender, form);
		}

		public void pbSeenForm(Pokemons poke,int gender=0,int form=0) {
			if (Trainer.formseen==null) Trainer.formseen=new int[];
			if (Trainer.formlastseen==null) Trainer.formlastseen=new int[];
			//if (poke is String || poke is Symbol) {
			//  poke=getID(PBSpecies,poke);
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
			string formnames=pbGetMessage(MessageTypes.FormNames,species);
			if (string.IsNullOrEmpty(formnames)) form=0;
			if (Trainer.formseen[species] == null) Trainer.formseen[species]=new int?[0][] { new int?[0],new int?[0] };
			Trainer.formseen[species][gender][form]=true;
			//if (Trainer.formlastseen[species] == null) Trainer.formlastseen[species]=new [];
			//if (Trainer.formlastseen[species] == []) Trainer.formlastseen[species]= new []{ gender,form };
			if (Trainer.formlastseen[species] == null) Trainer.formlastseen[species] = new KeyValuePair<int, int>(gender,form);
			//if(Player.Pokedex[(int)species, 2] < 0)
			//Player.Pokedex[(int)species,2] = (byte)form; 
		}
		#endregion

		#region Analysing Pokémon
		// Heals all Pokémon in the party.
		public void pbHealAll() {
			if (Trainer == null) return;
			foreach (IPokemon i in Trainer.party) {
				i.Heal();
			}
		}

		// Returns the first unfainted, non-egg Pokémon in the player's party.
		public IPokemon pbFirstAblePokemon(int variableNumber) {
			for (int i = 0; i < Trainer.party.Length; i++) {
				IPokemon p=Trainer.party[i];
				if (p != null && !p.isEgg && p.HP>0) {
					pbSet(variableNumber,i);
					return Trainer.party[i];
				}
			}
			pbSet(variableNumber,-1);
			return null;
		}

		// Checks whether the player would still have an unfainted Pokémon if the
		// Pokémon given by _pokemonIndex_ were removed from the party.
		public bool pbCheckAble(int pokemonIndex) {
			for (int i = 0; i < Trainer.party.Length; i++) {
				IPokemon p=Trainer.party[i];
				if (i==pokemonIndex) continue;
				if (p.IsNotNullOrNone() && !p.isEgg && p.HP>0) return true;
			}
			return false;
		}

		// Returns true if there are no usable Pokémon in the player's party.
		public bool pbAllFainted() {
			foreach (var i in Trainer.party) {
				if (!i.isEgg && i.HP>0) return false;
			}
			return true;
		}

		public double pbBalancedLevel(IPokemon[] party) {
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
		public int pbSize(IPokemon pokemon) {
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,pokemon.Species,33);
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
		public bool pbHasEgg (Pokemons species) {
			//if (species is String || species is Symbol) {
			//  species=getID(PBSpecies,species);
			//}
			//Pokemons[][] evospecies=pbGetEvolvedFormData(species); //Game.PokemonEvolutionsData[species][0].
			//Kernal.PokemonData[species].IsBaby
			//Pokemons compatspecies=(evospecies != null && evospecies[0] != null) ? evospecies[0][2] : species;
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,compatspecies,31);
			EggGroups compat1=Kernal.PokemonData[species].EggGroup[0]; //dexdata.fgetb();   // Get egg group 1 of this species
			EggGroups compat2=Kernal.PokemonData[species].EggGroup[1]; //dexdata.fgetb();   // Get egg group 2 of this species
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
		public IEnumerable<KeyValuePair<IPokemon,int>> pbEachPokemon() {
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
		public IEnumerable<KeyValuePair<IPokemon,int>> pbEachNonEggPokemon() {
			//pbEachPokemon(){|pokemon,box|
			foreach(KeyValuePair<IPokemon,int> pokemon in pbEachPokemon()) {
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
		public void pbChoosePokemon(int variableNumber,int nameVarNumber,Predicate<IPokemon> ableProc=null,bool allowIneligible=false) {
			int chosen=0;
			pbFadeOutIn(99999, block: () => {
				IPartyDisplayScene scene=Scenes.Party; //new PokemonScreen_Scene();
				IPartyDisplayScreen screen=Screens.Party.initialize(scene,Trainer.party); //new PokemonScreen(scene,Trainer.party);
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
				pbSet(nameVarNumber,Trainer.party[chosen].name);
			} else {
				pbSet(nameVarNumber,"");
			}
		}

		public void pbChooseNonEggPokemon(int variableNumber,int nameVarNumber) {
			//pbChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
			pbChoosePokemon(variableNumber,nameVarNumber, poke => 
				!poke.isEgg
			);
		}

		public void pbChooseAblePokemon(int variableNumber,int nameVarNumber) {
			//pbChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
			pbChoosePokemon(variableNumber,nameVarNumber, poke =>
				!poke.isEgg && poke.HP>0
			);
		}

		public void pbChoosePokemonForTrade(int variableNumber,int nameVarNumber,Pokemons wanted) {
			//pbChoosePokemon(variableNumber,nameVarNumber,proc {|poke|
			pbChoosePokemon(variableNumber, nameVarNumber, poke =>
				//if (wanted is String || wanted is Symbol) {
				//  wanted=getID(PBSpecies,wanted);
				//}
				!poke.isEgg && !((IPokemonShadowPokemon)poke).isShadow && poke.Species==wanted ///rescue false
			);
		}
		#endregion

		#region Checks through the party for something
		public bool pbHasSpecies (Pokemons species) {
			//if (species is String || species is Symbol) {
			//  species=getID(PBSpecies,species);
			//}
			foreach (var pokemon in Trainer.party) {
				if (pokemon.isEgg) continue;
				if (pokemon.Species==species) return true;
			}
			return false;
		}

		public bool pbHasFatefulSpecies (Pokemons species) {
			//if (species is String || species is Symbol) {
			//  species=getID(PBSpecies,species);
			//}
			foreach (Pokemon pokemon in Trainer.party) {
				if (pokemon.isEgg) continue;
				if (pokemon.Species==species && pokemon.ObtainedMode==Pokemon.ObtainedMethod.FATEFUL_ENCOUNTER) return true;
			}
			return false;
		}

		public bool pbHasType (Types type) {
			//if (type is String || type is Symbol) {
			//  type=getID(PBTypes,type);
			//}
			foreach (var pokemon in Trainer.party) {
				if (pokemon.isEgg) continue;
				if (pokemon.hasType(type)) return true;
			}
			return false;
		}

		// Checks whether any Pokémon in the party knows the given move, and returns
		// the index of that Pokémon, or null if no Pokémon has that move.
		public IPokemon pbCheckMove(Moves move) {
			//move=getID(PBMoves,move);
			if (move<=0) return null; //!move ||
			foreach (IPokemon i in Trainer.party) {
				if (i.isEgg) continue;
				foreach (IMove j in i.moves) {
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
		public int pbGetRegionalNumber(int region,Pokemons nationalSpecies) {
			if (nationalSpecies<=0 || (int)nationalSpecies>Kernal.PokemonData.Count) {
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
		public int pbGetNationalNumber(int region,Pokemons regionalSpecies) {
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
		public Pokemons[] pbAllRegionalSpecies(int region) {
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
		public int pbGetCurrentRegion(int defaultRegion=-1) {
			//int[] mappos=GameMap == null ? null : (int[])pbGetMetadata(GameMap.map_id,MapMetadatas.MetadataMapPosition);
			ITilePosition mappos=GameMap == null ? null : pbGetMetadata(GameMap.map_id).Map.MapPosition;
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
		public void pbSetViableDexes() {
			Global.pokedexViable=new List<int>();
			if (Core.DEXDEPENDSONLOCATION) {
				int region=pbGetCurrentRegion();
				if (region>=Global.pokedexUnlocked.Length-1) region=-1;
				if (Trainer.pokedexSeen((Regions)region)>0) {
					Global.pokedexViable.Add(region); //[0]=region;
				}
			} else {
				int numDexes=Global.pokedexUnlocked.Length;
				switch (numDexes) {
					case 1:          // National Dex only
						if (Global.pokedexUnlocked[0] != null) {
							if (Trainer.pokedexSeen>0) {
								Global.pokedexViable.Add(0);
							}
						}
						break;
					default:            // Regional dexes + National Dex
						for (int i = 0; i < numDexes; i++) {
						int regionToCheck=(i==numDexes-1) ? -1 : i;
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
		public void pbUnlockDex(int dex=-1) {
			int index=dex;
			if (index<0) index=Global.pokedexUnlocked.Length-1;
			if (index>Global.pokedexUnlocked.Length-1) index=Global.pokedexUnlocked.Length-1;
			Global.pokedexUnlocked[index]=true;
		}

		/// <summary>
		/// Locks a Dex list.  The National Dex is -1 here (or null argument).
		/// </summary>
		/// <param name="dex"></param>
		public void pbLockDex(int dex=-1) {
			int index=dex;
			if (index<0) index=Global.pokedexUnlocked.Length-1;
			if (index>Global.pokedexUnlocked.Length-1) index=Global.pokedexUnlocked.Length-1;
			Global.pokedexUnlocked[index]=false;
		}
		#endregion

		#region Other utilities
		public void pbTextEntry(string helptext,int minlength,int maxlength,int variableNumber) {
			GameVariables[variableNumber]=pbEnterText(helptext,minlength,maxlength);
			if (GameMap != null) GameMap.need_refresh = true;
		}

		public string[] pbMoveTutorAnnotations(Moves move,Pokemons[] movelist=null) {
			string[] ret=new string[Core.MAXPARTYSIZE];
			for (int i = 0; i < Core.MAXPARTYSIZE; i++) {
				ret[i]=null;
				if (i>=Trainer.party.Length) continue;
				bool found=false;
				for (int j = 0; j < 4; j++) {
					if (!Trainer.party[i].isEgg && Trainer.party[i].moves[j].MoveId==move) {
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
					//  Checked data from PBS/tm.txt
					ret[i]=Game._INTL("ABLE");
				} else {
					ret[i]=Game._INTL("NOT ABLE");
				}
			}
			return ret;
		}

		public bool pbMoveTutorChoose(Moves move,Pokemons[] movelist=null,bool bymachine=false) {
			bool ret=false;
			//if (move is String || move is Symbol) {
			//  move=getID(PBMoves,move);
			//}
			if (movelist!=null && movelist.Length>0) { //&& movelist is Array
				for (int i = 0; i < movelist.Length; i++) {
					//if (movelist[i] is String || movelist[i] is Symbol) {
					//  movelist[i]=getID(PBSpecies,movelist[i]);
					//}
				}
			}
			pbFadeOutIn(99999, () => {
				IPartyDisplayScene scene=Scenes.Party; //new PokemonScreen_Scene();
				string movename=move.ToString(TextScripts.Name);
				IPartyDisplayScreen screen=Screens.Party.initialize(scene,Trainer.party); //new PokemonScreen(scene,Trainer.party);
				string[] annot=pbMoveTutorAnnotations(move,movelist);
				screen.pbStartScene(Game._INTL("Teach which Pokémon?"),false,annot);
				do { //;loop
					int chosen=screen.pbChoosePokemon();
					if (chosen>=0) {
						Pokemon pokemon=Trainer.party[chosen];
						if (pokemon.isEgg) {
							pbMessage(Game._INTL("{1} can't be taught to an Egg.",movename));
						} else if ((pokemon.isShadow)) { //rescue false
							pbMessage(Game._INTL("Shadow Pokémon can't be taught any moves."));
						} else if (movelist != null && !movelist.Any(j => j==pokemon.Species )) {
							pbMessage(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
							pbMessage(Game._INTL("{1} can't be learned.",movename));
						} else if (!pokemon.isCompatibleWithMove(move)) {
							pbMessage(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
							pbMessage(Game._INTL("{1} can't be learned.",movename));
						} else {
							if (pbLearnMove(pokemon,move,false,bymachine)) {
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

		public void pbChooseMove(IPokemon pokemon,int variableNumber,int nameVarNumber) {
			if (!pokemon.IsNotNullOrNone()) return;
			int ret=-1;
			pbFadeOutIn(99999, () => {
				IPokemonSummaryScene scene=Scenes.Summary; //new PokemonSummaryScene();
				IPokemonSummaryScreen screen=Screens.Summary.initialize(scene); //new PokemonSummary(scene);
				ret=screen.pbStartForgetScreen(pokemon,0,0);
			});
			GameVariables[variableNumber]=ret;
			if (ret>=0) {
				GameVariables[nameVarNumber]=pokemon.moves[ret].MoveId.ToString(TextScripts.Name);
			} else {
				GameVariables[nameVarNumber]="";
			}
			if (GameMap != null) GameMap.need_refresh = true;
		}
		#endregion
	}
}
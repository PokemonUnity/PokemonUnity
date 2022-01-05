using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	public partial class Game 
	{
		public static bool Swarm { get; private set; }
		public static byte Time { get; private set; }
		public static bool Radar { get; private set; }
		public static byte Slot { get; private set; }
		public static byte Radio { get; private set; }
		public static bool IsAurora { get; private set; }
		//public static Environments Environment { get; set; }
		public static Overworld.FieldWeathers Weather { get; set; }
		public static Season Season
		{
			get
			//{
			//	int month = System.DateTime.Today.Month;
			//
			//	if (Jan, May, Sept)
			//	{
			//		return Season.Spring;
			//	}
			//	if (Feb, Jun, Oct)
			//	{
			//		return Season.Summer;
			//	}
			//	if (Mar,Jul, Nov)
			//	{
			//		return Season.Autumn;
			//	}
			//	if (Apr, Aug, Dec)
			//	{
			//		return Season.Spring
			//	}
			//}
			{
				//if (IsMainMenu)
				//	return Seasons.Summer;
				//if (NeedServerObject())
				//	return ServerSeason;
				if (WeekOfYear % 4 == 1)
					return Season.Winter;
				if (WeekOfYear % 4 == 2)
					return Season.Spring;
				if (WeekOfYear % 4 == 3)
					return Season.Summer;
				if (WeekOfYear % 4 == 0)
					return Season.Fall;
				return Season.Summer;
			}
		}
		public static DayTime GetTime
		{
			get
			{
				//if (IsMainMenu)
				//	return DayTime.Day;

				DayTime time = DayTime.Day;

				//ToDo: DateTime UTC to Local
				int Hour = DateTime.UtcNow.Hour;
				//if (NeedServerObject())
				//{
				//	string[] data = ServerTimeData.Split(System.Convert.ToChar(","));
				//	Hour = System.Convert.ToInt32(data[0]);
				//}

				switch (Season)//CurrentSeason
				{
					case Season.Winter:
					{
						if (Hour >= 18 || Hour < 7)
							time = DayTime.Night;
						else if (Hour >= 6 & Hour < 11)
							time = DayTime.Morning;
						else if (Hour >= 10 & Hour < 17)
							time = DayTime.Day;
						else if (Hour >= 16 & Hour < 19)
							time = DayTime.Evening;
						break;
					}
					case Season.Spring:
					{
						if (Hour >= 19 || Hour < 5)
							time = DayTime.Night;
						else if (Hour >= 4 & Hour < 10)
							time = DayTime.Morning;
						else if (Hour >= 9 & Hour < 17)
							time = DayTime.Day;
						else if (Hour >= 16 & Hour < 20)
							time = DayTime.Evening;
						break;
					}
					case Season.Summer:
					{
						if (Hour >= 20 || Hour < 4)
							time = DayTime.Night;
						else if (Hour >= 3 & Hour < 9)
							time = DayTime.Morning;
						else if (Hour >= 8 & Hour < 19)
							time = DayTime.Day;
						else if (Hour >= 18 & Hour < 21)
							time = DayTime.Evening;
						break;
					}
					case Season.Fall:
					{
						if (Hour >= 19 || Hour < 6)
							time = DayTime.Night;
						else if (Hour >= 5 & Hour < 10)
							time = DayTime.Morning;
						else if (Hour >= 9 & Hour < 18)
							time = DayTime.Day;
						else if (Hour >= 17 & Hour < 20)
							time = DayTime.Evening;
						break;
					}
				}

				return time;
			}
		}
		public static int WeekOfYear
		{
			get
			{
				return System.Convert.ToInt32(((DateTime.UtcNow.DayOfYear - ((double)DateTime.UtcNow.DayOfWeek - 1)) / (double)7) + 1);
			}
		}
		public static int MinutesOfDay
		{
			get
			{
				//if (NeedServerObject())
				//{
				//	string[] data = ServerTimeData.Split(System.Convert.ToChar(","));
				//	int hours = System.Convert.ToInt32(data[0]);
				//	int minutes = System.Convert.ToInt32(data[1]);
				//
				//	minutes += System.Convert.ToInt32(Math.Abs((DateTime.Now - LastServerDataReceived).Minutes));
				//
				//	return hours * 60 + minutes;
				//}
				//else
					return DateTime.UtcNow.Hour * 60 + DateTime.UtcNow.Minute;
			}
		}
		public static int SecondsOfDay
		{
			get
			{
				//if (NeedServerObject())
				//{
				//	string[] data = ServerTimeData.Split(System.Convert.ToChar(","));
				//	int hours = System.Convert.ToInt32(data[0]);
				//	int minutes = System.Convert.ToInt32(data[1]);
				//	int seconds = System.Convert.ToInt32(data[2]);
				//
				//	seconds += System.Convert.ToInt32(Math.Abs((DateTime.Now - LastServerDataReceived).Seconds));
				//
				//	return hours * 3600 + minutes * 60 + seconds;
				//}
				//else
					return DateTime.UtcNow.Hour * 3600 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second;
			}
		}
		public static DateTime GetTimeNow { get { return DateTime.UtcNow; } }

		#region Time
		/// <summary>
		/// Is ON during the day (i.e. between 5am and 8pm), and OFF otherwise (i.e. during the night).
		/// </summary>
		public static bool IsDay		{ get { return GetTime == DayTime.Day; } }
		/// <summary>
		/// Is ON during the night (i.e. between 8pm and 5am), and OFF otherwise (i.e. during the day).
		/// </summary>
		public static bool IsNight		{ get { return GetTime == DayTime.Night; } }
		/// <summary>
		/// Is ON during the morning (i.e. between 5am and 10am), and OFF otherwise.
		/// </summary>
		public static bool IsMorning	{ get { return GetTime == DayTime.Morning; } }
		/// <summary>
		/// Is ON during the afternoon (i.e. between 2pm and 5pm), and OFF otherwise.
		/// </summary>
		public static bool IsAfternoon	{ get { return GetTime == DayTime.Day; } }
		/// <summary>
		/// Is ON during the evening (i.e. between 5pm and 8pm), and OFF otherwise.
		/// </summary>
		public static bool IsEvening	{ get { return GetTime == DayTime.Evening; } }
		/// <summary>
		/// Is ON if the current day is Tuesday, Thursday or Saturday, and OFF otherwise.
		/// </summary>
		public static bool IsWeekday	{ get { return GetTime == DayTime.Day; } }
		#endregion

		//public void DefaultConditions()
		//{
		//	Swarm = false;
		//	Time = (byte)ConditionValue.TIME_DAY;
		//	Radar = false;
		//	Slot = (byte)ConditionValue.SLOT_NONE;
		//	Radio = (byte)ConditionValue.RADIO_OFF;
		//	//Season = Season.Spring;
		//}
		public void SetCondition(ConditionValue condition)
		{
			switch (condition)
			{
				case ConditionValue.SWARM_YES:
					Swarm = true;
					break;
				case ConditionValue.SWARM_NO:
					Swarm = false;
					break;
				case ConditionValue.TIME_MORNING:
					Time = (byte)ConditionValue.TIME_MORNING;
					break;
				case ConditionValue.TIME_DAY:
					Time = (byte)ConditionValue.TIME_MORNING;
					break;
				case ConditionValue.TIME_NIGHT:
					Time = (byte)ConditionValue.TIME_MORNING;
					break;
				case ConditionValue.RADAR_ON:
					Radar = true;
					break;
				case ConditionValue.RADAR_OFF:
					Radar = false;
					break;
				case ConditionValue.SLOT_NONE:
					Slot = (byte)ConditionValue.SLOT_NONE;
					break;
				case ConditionValue.SLOT_RUBY:
					Slot = (byte)ConditionValue.SLOT_RUBY;
					break;
				case ConditionValue.SLOT_SAPPHIRE:
					Slot = (byte)ConditionValue.SLOT_SAPPHIRE;
					break;
				case ConditionValue.SLOT_EMERALD:
					Slot = (byte)ConditionValue.SLOT_EMERALD;
					break;
				case ConditionValue.SLOT_FIRERED:
					Slot = (byte)ConditionValue.SLOT_FIRERED;
					break;
				case ConditionValue.SLOT_LEAFGREEN:
					Slot = (byte)ConditionValue.SLOT_LEAFGREEN;
					break;
				case ConditionValue.RADIO_OFF:
					Radio = (byte)ConditionValue.RADIO_OFF;
					break;
				case ConditionValue.RADIO_HOENN:
					Radio = (byte)ConditionValue.RADIO_HOENN;
					break;
				case ConditionValue.RADIO_SINNOH:
					Radio = (byte)ConditionValue.RADIO_SINNOH;
					break;
				//case ConditionValue.SEASON_SPRING:
				//	Season = Season.Spring;
				//	break;
				//case ConditionValue.SEASON_SUMMER:
				//	Season = Season.Summer;
				//	break;
				//case ConditionValue.SEASON_AUTUMN:
				//	Season = Season.Fall;
				//	break;
				//case ConditionValue.SEASON_WINTER:
				//	Season = Season.Winter;
				//	break;
				case ConditionValue.NONE:
				default:
					break;
			}
		}
		public static Condition GetCondition(ConditionValue condition)
		{
			switch (condition)
			{
				case ConditionValue.SWARM_YES:
				case ConditionValue.SWARM_NO:
					return Condition.SWARM;
				case ConditionValue.TIME_DAY:
				case ConditionValue.TIME_MORNING:
				case ConditionValue.TIME_NIGHT:
					return Condition.TIME;
				case ConditionValue.RADAR_ON:
				case ConditionValue.RADAR_OFF:
					return Condition.RADAR;
				case ConditionValue.RADIO_OFF:
				case ConditionValue.RADIO_HOENN:
				case ConditionValue.RADIO_SINNOH:
					return Condition.RADIO;
				case ConditionValue.SEASON_SPRING:
				case ConditionValue.SEASON_SUMMER:
				case ConditionValue.SEASON_AUTUMN:
				case ConditionValue.SEASON_WINTER:
					return Condition.SEASON;
				case ConditionValue.NONE:
				case ConditionValue.SLOT_NONE:
				case ConditionValue.SLOT_RUBY:
				case ConditionValue.SLOT_SAPPHIRE:
				case ConditionValue.SLOT_EMERALD:
				case ConditionValue.SLOT_FIRERED:
				case ConditionValue.SLOT_LEAFGREEN:
				default:
					return Condition.SLOT;
			}
		}
		public static bool IsConditionDefault(ConditionValue condition)
		{
			switch (condition)
			{
				case ConditionValue.NONE:
				case ConditionValue.SWARM_NO:
				case ConditionValue.TIME_DAY:
				case ConditionValue.RADAR_OFF:
				case ConditionValue.SLOT_NONE:
				case ConditionValue.RADIO_OFF:
					return true;
				case ConditionValue.SWARM_YES:
				case ConditionValue.TIME_MORNING:
				case ConditionValue.TIME_NIGHT:
				case ConditionValue.RADAR_ON:
				case ConditionValue.SLOT_RUBY:
				case ConditionValue.SLOT_SAPPHIRE:
				case ConditionValue.SLOT_EMERALD:
				case ConditionValue.SLOT_FIRERED:
				case ConditionValue.SLOT_LEAFGREEN:
				case ConditionValue.RADIO_HOENN:
				case ConditionValue.RADIO_SINNOH:
				case ConditionValue.SEASON_SPRING:
				case ConditionValue.SEASON_SUMMER:
				case ConditionValue.SEASON_AUTUMN:
				case ConditionValue.SEASON_WINTER:
				default:
					return false;
			}
		}
		public static FieldWeathers GetRegionWeather(Season season)
		{
			//if (IsMainMenu)
			//	return FieldWeathers.Clear;

			int r = Core.Rand.Next(0, 100);

			switch (season)
			{
				case Season.Winter:
				{
					if (r < 20)
						return FieldWeathers.Rain;
					else if (r >= 20 & r < 50)
						return FieldWeathers.Clear;
					else
						return FieldWeathers.Snow;
				}
				case Season.Spring:
				{
					if (r < 5)
						return FieldWeathers.Snow;
					else if (r >= 5 & r < 40)
						return FieldWeathers.Rain;
					else
						return FieldWeathers.Clear;
				}
				case Season.Summer:
				{
					if (r < 10)
						return FieldWeathers.Rain;
					else
						return FieldWeathers.Clear;
				}
				case Season.Fall:
				{
					if (r < 5)
						return FieldWeathers.Snow;
					else if (r >= 5 & r < 80)
						return FieldWeathers.Rain;
					else
						return FieldWeathers.Clear;
				}
			}

			return FieldWeathers.Clear;
		}

#region Day and night system
public static DateTime pbGetTimeNow() {
  return DateTime.Now;
}

public static partial class PBDayNight {
  public static float oneOverSixty=1/60.0f;
  //public static Tone HourlyTones=new Tone[] {
  //   new Tone(-142.5,-142.5,-22.5,68),     // Midnight
  //   new Tone(-135.5,-135.5,-24,  68),
  //   new Tone(-127.5,-127.5,-25.5,68),
  //   new Tone(-127.5,-127.5,-25.5,68),
  //   new Tone(-119,  -96.3, -45.3,45.3),
  //   new Tone(-51,   -73.7, -73.7,22.7),
  //   new Tone(17,    -51,   -102, 0),      // 6AM
  //   new Tone(14.2,  -42.5, -85,  0),
  //   new Tone(11.3,  -34,   -68,  0),
  //   new Tone(8.5,   -25.5, -51,  0),
  //   new Tone(5.7,   -17,   -34,  0),
  //   new Tone(2.8,   -8.5,  -17,  0),
  //   new Tone(0,     0,     0,    0),      // Noon
  //   new Tone(0,     0,     0,    0),
  //   new Tone(0,     0,     0,    0),
  //   new Tone(0,     0,     0,    0),
  //   new Tone(-3,    -7,    -2,   0),
  //   new Tone(-10,   -18,   -5,   0),
  //   new Tone(-36,   -75,   -13,  0),      // 6PM
  //   new Tone(-72,   -136,  -34,  3),
  //   new Tone(-88.5, -133,  -31,  34),
  //   new Tone(-108.5,-129,  -28,  68),
  //   new Tone(-127.5,-127.5,-25.5,68),
  //   new Tone(-142.5,-142.5,-22.5,68)
  //};
  //@cachedTone=null;
  //@dayNightToneLastUpdate=null;
  //@oneOverSixty=1/60.0f;

/// <summary>
/// Returns true if it's day.
/// </summary>
/// <param name="time"></param>
/// <returns></returns>
  public static bool isDay(DateTime? @time=null) {
    if (@time == null) @time=pbGetTimeNow();
    return (@time.Value.Hour>=6 && @time.Value.Hour<20);
  }

/// <summary>
/// Returns true if it's night.
/// </summary>
/// <param name="time"></param>
/// <returns></returns>
  public static bool isNight(DateTime? @time=null) {
    if (@time == null) @time=pbGetTimeNow();
    return (@time.Value.Hour>=20 || @time.Value.Hour<6);
  }

/// <summary>
/// Returns true if it's morning.
/// </summary>
/// <param name="time"></param>
/// <returns></returns>
  public static bool isMorning(DateTime? @time=null) {
    if (@time == null) @time=pbGetTimeNow();
    return (@time.Value.Hour>=6 && @time.Value.Hour<12);
  }

/// <summary>
/// Returns true if it's the afternoon.
/// </summary>
/// <param name="time"></param>
/// <returns></returns>
  public static bool isAfternoon(DateTime? @time=null) {
    if (@time == null) @time=pbGetTimeNow();
    return (@time.Value.Hour>=12 && @time.Value.Hour<20);
  }

/// <summary>
/// Returns true if it's the evening.
/// </summary>
/// <param name="time"></param>
/// <returns></returns>
  public static bool isEvening(DateTime? @time=null) {
    if (@time == null) @time=pbGetTimeNow();
    return (@time.Value.Hour>=17 && @time.Value.Hour<20);
  }

  public static int pbGetDayNightMinutes() {
    DateTime now=pbGetTimeNow();   // Get the current in-game time
    return (now.Hour*60)+now.Minute;
  }

/// <summary>
/// Gets a number representing the amount of daylight (0=full night, 255=full day).
/// </summary>
/// <returns></returns>
  public static int getShade() {
    int @time=pbGetDayNightMinutes();
    if (@time>(12*60)) @time=(24*60)-@time;
    int shade=255*@time/(12*60);
    return shade;
  }

/*// <summary>
/// Gets a Tone object representing a suggested shading
/// tone for the current time of day.
/// </summary>
  public Tone getTone() {
    if (!@cachedTone) @cachedTone=new Tone(0,0,0);
    if (!Core.ENABLESHADING) return @cachedTone;
    if (!@dayNightToneLastUpdate || @dayNightToneLastUpdate!=Graphics.frame_count) {
      getToneInternal();
      @dayNightToneLastUpdate=Graphics.frame_count;
    }
    return @cachedTone;
  }

  public void getToneInternal() {
	//Calculates the tone for the current frame, used for day/night effects
    int realMinutes=pbGetDayNightMinutes();
    int hour=realMinutes/60;
    int minute=realMinutes%60;
    Tone tone=HourlyTones[hour];
    Tone nexthourtone=HourlyTones[(hour+1)%24];
	//Calculate current tint according to current and next hour's tint and
	//depending on current minute
    @cachedTone.red=((nexthourtone.red-tone.red)*minute*@oneOverSixty)+tone.red;
    @cachedTone.green=((nexthourtone.green-tone.green)*minute*@oneOverSixty)+tone.green;
    @cachedTone.blue=((nexthourtone.blue-tone.blue)*minute*@oneOverSixty)+tone.blue;
    @cachedTone.gray=((nexthourtone.gray-tone.gray)*minute*@oneOverSixty)+tone.gray;
  }*/
}

//public void pbDayNightTint(object obj) {
//  if (!(Game.GameData.Scene is Scene_Map)) {
//    return;
//  } else {
//    if (Core.ENABLESHADING && Game.GameData.GameMap != null && pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor)) {
//      Tone tone=PBDayNight.getTone();
//      obj.tone.set(tone.red,tone.green,tone.blue,tone.gray);
//    } else {
//      obj.tone.set(0,0,0,0);
//    }
//  }
//}
#endregion

#region Moon phases and Zodiac
// Calculates the phase of the moon.
// 0 - New Moon
// 1 - Waxing Crescent
// 2 - First Quarter
// 3 - Waxing Gibbous
// 4 - Full Moon
// 5 - Waning Gibbous
// 6 - Last Quarter
// 7 - Waning Crescent
public static int moonphase(DateTime? @time=null) { // in UTC
  if (@time == null) time=pbGetTimeNow();
  double[] transitions=new double[] {
     1.8456618033125,
     5.5369854099375,
     9.2283090165625,
     12.9196326231875,
     16.6109562298125,
     20.3022798364375,
     23.9936034430625,
     27.6849270496875 };
  int yy=time.Value.Year-(int)Math.Floor((12-time.Value.Month)/10.0);
  double j=Math.Floor(365.25*(4712+yy)) + Math.Floor(((time.Value.Month+9)%12)*30.6+0.5) + time.Value.Day+59;
  if (j>2299160) j-=(int)Math.Floor(Math.Floor((yy/100.0)+49)*0.75)-38;
  j+=(((time.Value.Hour*60)+time.Value.Minute*60)+time.Value.Second)/86400.0;
  double v=(j-2451550.1)/29.530588853d;
  v=((v-Math.Floor(v))+(v<0 ? 1 : 0));
  double ag=v*29.53;
  for (int i = 0; i < transitions.Length; i++) {
    if (ag<=transitions[i]) return i;
  }
  return 0;
}

/// <summary>
/// Calculates the zodiac sign based on the given month and day:
/// 0 is Aries, 11 is Pisces. Month is 1 if January, and so on.
/// </summary>
/// <param name="month"></param>
/// <param name="day"></param>
/// <returns></returns>
public static int zodiac(int month,int day) {
  int[] time=new int[] {
     3,21,4,19,   // Aries
     4,20,5,20,   // Taurus
     5,21,6,20,   // Gemini
     6,21,7,20,   // Cancer
     7,23,8,22,   // Leo
     8,23,9,22,   // Virgo 
     9,23,10,22,  // Libra
     10,23,11,21, // Scorpio
     11,22,12,21, // Sagittarius
     12,22,1,19,  // Capricorn
     1,20,2,18,   // Aquarius
     2,19,3,20    // Pisces
  };
  for (int i = 0; i < 12; i++) {
    if (month==time[i*4] && day>=time[i*4+1]) return i;
    if (month==time[i*4+2] && day<=time[i*4+2]) return i;
  }
  return 0;
}
 
/// <summary>
/// Returns the opposite of the given zodiac sign.
/// 0 is Aries, 11 is Pisces.
/// </summary>
/// <param name="sign"></param>
/// <returns></returns>
public static int zodiacOpposite(int sign) {
  return (sign+6)%12;
}

/// <summary>
/// 0 is Aries, 11 is Pisces.
/// </summary>
/// <param name="sign"></param>
/// <returns></returns>
public static int[] zodiacPartners(int sign) {
  return new int[] { (sign + 4) % 12, (sign + 8) % 12 };
}

/// <summary>
/// 0 is Aries, 11 is Pisces.
/// </summary>
/// <param name="sign"></param>
/// <returns></returns>
public static int[] zodiacComplements(int sign) {
  return new int[] { (sign + 1) % 12, (sign + 11) % 12 };
}
#endregion

#region Days of the week
public static bool pbIsWeekday(int wdayVariable,params int[] arg) {
  DateTime timenow=pbGetTimeNow();
  int wday=(int)timenow.DayOfWeek;//.wday;
  bool ret=false;
  foreach (int wd in arg) {
    if (wd==wday) ret=true;
  }
  if (wdayVariable>0) {
    Game.GameData.GameVariables[wdayVariable]=new string[] {
       Game._INTL("Sunday"),
       Game._INTL("Monday"),
       Game._INTL("Tuesday"),
       Game._INTL("Wednesday"),
       Game._INTL("Thursday"),
       Game._INTL("Friday"),
       Game._INTL("Saturday")}[wday];
    //if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
  }
  return ret;
}
#endregion

#region Months
public static bool pbIsMonth(int monVariable,params int[] arg) {
  DateTime timenow=pbGetTimeNow();
  int thismon=timenow.Month;
  bool ret=false;
  foreach (int wd in arg) {
    if (wd==thismon) ret=true;
  }
  if (monVariable>0) {
    Game.GameData.GameVariables[monVariable]=new string[] {
       Game._INTL("January"),
       Game._INTL("February"),
       Game._INTL("March"),
       Game._INTL("April"),
       Game._INTL("May"),
       Game._INTL("June"),
       Game._INTL("July"),
       Game._INTL("August"),
       Game._INTL("September"),
       Game._INTL("October"),
       Game._INTL("November"),
       Game._INTL("December")}[thismon-1];
    //if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
  }
  return ret;
}

public static string pbGetAbbrevMonthName(int month) {
  return new string[] {"",
          Game._INTL("Jan."),
          Game._INTL("Feb."),
          Game._INTL("Mar."),
          Game._INTL("Apr."),
          Game._INTL("May"),
          Game._INTL("Jun."),
          Game._INTL("Jul."),
          Game._INTL("Aug."),
          Game._INTL("Sep."),
          Game._INTL("Oct."),
          Game._INTL("Nov."),
          Game._INTL("Dec.") }[month];
}
#endregion

#region Seasons
public static int pbGetSeason() {
  return (pbGetTimeNow().Month-1)%4;
}

public static bool pbIsSeason(int seasonVariable,params int[] arg) {
  int thisseason=pbGetSeason();
  bool ret=false;
  foreach (int wd in arg) {
    if (wd==thisseason) ret=true;
  }
  if (seasonVariable>0) {
    Game.GameData.GameVariables[seasonVariable]=new string[] {
       Game._INTL("Spring"),
       Game._INTL("Summer"),
       Game._INTL("Autumn"),
       Game._INTL("Winter")}[thisseason];
    //if (Game.GameData.GameMap != null) Game.GameData.GameMap.need_refresh = true;
  }
  return ret;
}

public static bool pbIsSpring() { return pbIsSeason(0,0); } // Jan, May, Sep
public static bool pbIsSummer() { return pbIsSeason(0,1); } // Feb, Jun, Oct
public static bool pbIsAutumn() { return pbIsSeason(0,2); } // Mar, Jul, Nov
public static bool pbIsFall() { return pbIsAutumn(); }
public static bool pbIsWinter() { return pbIsSeason(0,3); } // Apr, Aug, Dec

public static string pbGetSeasonName(int season) {
  return new string[] { Game._INTL("Spring"),
          Game._INTL("Summer"),
          Game._INTL("Autumn"),
          Game._INTL("Winter") }[season];
}
#endregion
	}
}
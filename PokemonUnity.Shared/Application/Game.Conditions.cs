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
						if (Hour > 18 | Hour < 7)
							time = DayTime.Night;
						else if (Hour > 6 & Hour < 11)
							time = DayTime.Morning;
						else if (Hour > 10 & Hour < 17)
							time = DayTime.Day;
						else if (Hour > 16 & Hour < 19)
							time = DayTime.Evening;
						break;
					}
					case Season.Spring:
					{
						if (Hour > 19 | Hour < 5)
							time = DayTime.Night;
						else if (Hour > 4 & Hour < 10)
							time = DayTime.Morning;
						else if (Hour > 9 & Hour < 17)
							time = DayTime.Day;
						else if (Hour > 16 & Hour < 20)
							time = DayTime.Evening;
						break;
					}
					case Season.Summer:
					{
						if (Hour > 20 | Hour < 4)
							time = DayTime.Night;
						else if (Hour > 3 & Hour < 9)
							time = DayTime.Morning;
						else if (Hour > 8 & Hour < 19)
							time = DayTime.Day;
						else if (Hour > 18 & Hour < 21)
							time = DayTime.Evening;
						break;
					}
					case Season.Fall:
					{
						if (Hour > 19 | Hour < 6)
							time = DayTime.Night;
						else if (Hour > 5 & Hour < 10)
							time = DayTime.Morning;
						else if (Hour > 9 & Hour < 18)
							time = DayTime.Day;
						else if (Hour > 17 & Hour < 20)
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
	}
}
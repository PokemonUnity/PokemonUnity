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
//using System.Security.Cryptography.MD5;

namespace PokemonUnity
{
	public partial class Game 
	{
		public bool Swarm { get; private set; }
		public byte Time { get; private set; }
		public bool Radar { get; private set; }
		public byte Slot { get; private set; }
		public byte Radio { get; private set; }
		public Season Season
		{
			get;
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
			private set;
		}

		public void DefaultConditions()
		{
			Swarm = false;
			Time = (byte)ConditionValue.TIME_DAY;
			Radar = false;
			Slot = (byte)ConditionValue.SLOT_NONE;
			Radio = (byte)ConditionValue.RADIO_OFF;
			Season = Season.Spring;
		}
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
					Slot = (byte)ConditionValue.RADIO_OFF;
					break;
				case ConditionValue.RADIO_HOENN:
					Slot = (byte)ConditionValue.RADIO_HOENN;
					break;
				case ConditionValue.RADIO_SINNOH:
					Slot = (byte)ConditionValue.RADIO_SINNOH;
					break;
				case ConditionValue.SEASON_SPRING:
					Season = Season.Spring;
					break;
				case ConditionValue.SEASON_SUMMER:
					Season = Season.Summer;
					break;
				case ConditionValue.SEASON_AUTUMN:
					Season = Season.Fall;
					break;
				case ConditionValue.SEASON_WINTER:
					Season = Season.Winter;
					break;
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
	}
}
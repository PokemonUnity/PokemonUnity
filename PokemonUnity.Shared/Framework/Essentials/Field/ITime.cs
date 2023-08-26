using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	public interface IGameTime
	{
		#region Day and night system
		DateTime pbGetTimeNow();

		void pbDayNightTint(object obj);
		#endregion

		#region Moon phases and Zodiac
		/// <summary>
		/// Calculates the phase of the moon.
		/// </summary>
		/// <param name="time">in UTC</param>
		/// <returns>
		/// 0 - New Moon
		/// 1 - Waxing Crescent
		/// 2 - First Quarter
		/// 3 - Waxing Gibbous
		/// 4 - Full Moon
		/// 5 - Waning Gibbous
		/// 6 - Last Quarter
		/// 7 - Waning Crescent
		/// </returns>
		int moonphase(DateTime? @time = null);

		/// <summary>
		/// Calculates the zodiac sign based on the given month and day:
		/// 0 is Aries, 11 is Pisces. Month is 1 if January, and so on.
		/// </summary>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <returns></returns>
		int zodiac(int month, int day);

		/// <summary>
		/// Returns the opposite of the given zodiac sign.
		/// 0 is Aries, 11 is Pisces.
		/// </summary>
		/// <param name="sign"></param>
		/// <returns></returns>
		int zodiacOpposite(int sign);

		/// <summary>
		/// 0 is Aries, 11 is Pisces.
		/// </summary>
		/// <param name="sign"></param>
		/// <returns></returns>
		int[] zodiacPartners(int sign);

		/// <summary>
		/// 0 is Aries, 11 is Pisces.
		/// </summary>
		/// <param name="sign"></param>
		/// <returns></returns>
		int[] zodiacComplements(int sign);
		#endregion

		#region Days of the week
		bool pbIsWeekday(int wdayVariable, params int[] arg);
		#endregion

		#region Months
		bool pbIsMonth(int monVariable, params int[] arg);

		string pbGetAbbrevMonthName(int month);
		#endregion

		#region Seasons
		int pbGetSeason();

		bool pbIsSeason(int seasonVariable, params int[] arg);

		/// <summary>
		/// Jan, May, Sep
		/// </summary>
		/// <returns></returns>
		bool pbIsSpring();
		/// <summary>
		/// Feb, Jun, Oct
		/// </summary>
		/// <returns></returns>
		bool pbIsSummer();
		/// <summary>
		/// Mar, Jul, Nov
		/// </summary>
		/// <returns></returns>
		bool pbIsAutumn();
		/// <summary>
		/// <seealso cref="pbIsAutumn"/>
		/// </summary>
		/// <returns></returns>
		bool pbIsFall();
		/// <summary>
		/// Apr, Aug, Dec
		/// </summary>
		/// <returns></returns>
		bool pbIsWinter();

		string pbGetSeasonName(int season);
		#endregion
	}

	public interface IDayNight {
		ITone[] HourlyTones { get; }
		ITone cachedTone { get; }
		int? dayNightToneLastUpdate { get; }
		//DateTime? dayNightToneLastUpdate { get; }
		float oneOverSixty { get; } //=1/60.0f;

		/// <summary>
		/// Returns true if it's day.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		bool isDay(DateTime? @time = null);

		/// <summary>
		/// Returns true if it's night.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		bool isNight(DateTime? @time = null);

		/// <summary>
		/// Returns true if it's morning.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		bool isMorning(DateTime? @time = null);

		/// <summary>
		/// Returns true if it's the afternoon.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		bool isAfternoon(DateTime? @time = null);

		/// <summary>
		/// Returns true if it's the evening.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		bool isEvening(DateTime? @time = null);

		/// <summary>
		/// Gets a number representing the amount of daylight (0=full night, 255=full day).
		/// </summary>
		/// <returns></returns>
		int getShade();

		/// <summary>
		/// Gets a Tone object representing a suggested shading
		/// tone for the current time of day.
		/// </summary>
		ITone getTone();

		int pbGetDayNightMinutes();
	}
}
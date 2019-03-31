using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld
{
	public class World
	{
		private static Weathers _regionWeather = Weathers.Clear;
		private static bool _regionWeatherSet = false;

		public static bool IsMainMenu = false;
		public static bool IsAurora = false;

		public enum Seasons : int
		{
			Winter = 0,
			Spring = 1,
			Summer = 2,
			Fall = 3
		}

		public enum Weathers : int
		{
			Clear = 0,
			Rain = 1,
			Snow = 2,
			Underwater = 3,
			Sunny = 4,
			Fog = 5,
			Thunderstorm = 6,
			Sandstorm = 7,
			Ash = 8,
			Blizzard = 9
		}

		public enum EnvironmentTypes : int
		{
			Outside = 0,
			Inside = 1,
			Cave = 2,
			Dark = 3,
			Underwater = 4,
			Forest = 5
		}

		public enum DayTime : int
		{
			Night = 0,
			Morning = 1,
			Day = 2,
			Evening = 3
		}

		public static int WeekOfYear
		{
			get
			{
				return System.Convert.ToInt32(((DateTime.UtcNow.DayOfYear - ((double)DateTime.UtcNow.DayOfWeek - 1)) / (double)7) + 1);
			}
		}

		public static Seasons CurrentSeason
		{
			get
			{
				if (IsMainMenu)
					return Seasons.Summer;

				if (NeedServerObject())
					return ServerSeason;
				switch (WeekOfYear % 4)
				{
					case 1:
						{
							return Seasons.Winter;
						}
					case 2:
						{
							return Seasons.Spring;
						}
					case 3:
						{
							return Seasons.Summer;
						}
					case 0:
						{
							return Seasons.Fall;
						}
				}
				return Seasons.Summer;
			}
		}

		public static DayTime GetTime
		{
			get
			{
				if (IsMainMenu)
					return DayTime.Day;

				DayTime time = DayTime.Day;

				//ToDo: DateTime UTC to Local
				int Hour = DateTime.UtcNow.Hour;
				if (NeedServerObject())
				{
					string[] data = ServerTimeData.Split(System.Convert.ToChar(","));
					Hour = System.Convert.ToInt32(data[0]);
				}

				switch (CurrentSeason)
				{
					case Seasons.Winter:
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
					case Seasons.Spring:
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
					case Seasons.Summer:
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
					case Seasons.Fall:
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

		public static void SetRenderDistance(EnvironmentTypes EnvironmentType, Weathers Weather)
		{
			//if (Weather == Weathers.Fog)
			//{
			//    Screen.Effect.FogStart = -40;
			//    Screen.Effect.FogEnd = 12;
			//
			//    Game.Camera.FarPlane = 15;
			//    goto endsub;
			//}
			//
			//if (Weather == Weathers.Blizzard)
			//{
			//    Screen.Effect.FogStart = -40;
			//    Screen.Effect.FogEnd = 20;
			//
			//    Game.Camera.FarPlane = 24;
			//    goto endsub;
			//}
			//
			//if (Weather == Weathers.Thunderstorm)
			//{
			//    Screen.Effect.FogStart = -40;
			//    Screen.Effect.FogEnd = 20;
			//
			//    Game.Camera.FarPlane = 24;
			//    goto endsub;
			//}
			//
			//switch (EnvironmentType)
			//{
			//    case EnvironmentTypes.Cave:
			//    case EnvironmentTypes.Dark:
			//    case EnvironmentTypes.Forest:
			//        {
			//            switch (Core.GameOptions.RenderDistance)
			//            {
			//                case 0:
			//                    {
			//                        Screen.Effect.FogStart = -2;
			//                        Screen.Effect.FogEnd = 19;
			//
			//                        Game.Camera.FarPlane = 20;
			//                        break;
			//                    }
			//                case 1:
			//                    {
			//                        Screen.Effect.FogStart = -2;
			//                        Screen.Effect.FogEnd = 39;
			//
			//                        Game.Camera.FarPlane = 40;
			//                        break;
			//                    }
			//                case 2:
			//                    {
			//                        Screen.Effect.FogStart = -2;
			//                        Screen.Effect.FogEnd = 59;
			//
			//                        Game.Camera.FarPlane = 60;
			//                        break;
			//                    }
			//                case 3:
			//                    {
			//                        Screen.Effect.FogStart = -5;
			//                        Screen.Effect.FogEnd = 79;
			//
			//                        Game.Camera.FarPlane = 80;
			//                        break;
			//                    }
			//                case 4:
			//                    {
			//                        Screen.Effect.FogStart = -20;
			//                        Screen.Effect.FogEnd = 99;
			//
			//                        Game.Camera.FarPlane = 100;
			//                        break;
			//                    }
			//            }
			//
			//            break;
			//        }
			//    case EnvironmentTypes.Inside:
			//        {
			//            switch (Core.GameOptions.RenderDistance)
			//            {
			//                case 0:
			//                    {
			//                        Screen.Effect.FogStart = 16;
			//                        Screen.Effect.FogEnd = 19;
			//
			//                        Game.Camera.FarPlane = 20;
			//                        break;
			//                    }
			//                case 1:
			//                    {
			//                        Screen.Effect.FogStart = 36;
			//                        Screen.Effect.FogEnd = 39;
			//
			//                        Game.Camera.FarPlane = 40;
			//                        break;
			//                    }
			//                case 2:
			//                    {
			//                        Screen.Effect.FogStart = 56;
			//                        Screen.Effect.FogEnd = 59;
			//
			//                        Game.Camera.FarPlane = 60;
			//                        break;
			//                    }
			//                case 3:
			//                    {
			//                        Screen.Effect.FogStart = 76;
			//                        Screen.Effect.FogEnd = 79;
			//
			//                        Game.Camera.FarPlane = 80;
			//                        break;
			//                    }
			//                case 4:
			//                    {
			//                        Screen.Effect.FogStart = 96;
			//                        Screen.Effect.FogEnd = 99;
			//
			//                        Game.Camera.FarPlane = 100;
			//                        break;
			//                    }
			//            }
			//
			//            break;
			//        }
			//    case EnvironmentTypes.Outside:
			//        {
			//            switch (World.GetTime)
			//            {
			//                case DayTime.Night:
			//                    {
			//                        switch (Core.GameOptions.RenderDistance)
			//                        {
			//                            case 0:
			//                                {
			//                                    Screen.Effect.FogStart = -2;
			//                                    Screen.Effect.FogEnd = 19;
			//
			//                                    Game.Camera.FarPlane = 20;
			//                                    break;
			//                                }
			//                            case 1:
			//                                {
			//                                    Screen.Effect.FogStart = -2;
			//                                    Screen.Effect.FogEnd = 39;
			//
			//                                    Game.Camera.FarPlane = 40;
			//                                    break;
			//                                }
			//                            case 2:
			//                                {
			//                                    Screen.Effect.FogStart = -2;
			//                                    Screen.Effect.FogEnd = 59;
			//
			//                                    Game.Camera.FarPlane = 60;
			//                                    break;
			//                                }
			//                            case 3:
			//                                {
			//                                    Screen.Effect.FogStart = -5;
			//                                    Screen.Effect.FogEnd = 79;
			//
			//                                    Game.Camera.FarPlane = 80;
			//                                    break;
			//                                }
			//                            case 4:
			//                                {
			//                                    Screen.Effect.FogStart = -20;
			//                                    Screen.Effect.FogEnd = 99;
			//
			//                                    Game.Camera.FarPlane = 100;
			//                                    break;
			//                                }
			//                        }
			//
			//                        break;
			//                    }
			//                case DayTime.Morning:
			//                    {
			//                        switch (Core.GameOptions.RenderDistance)
			//                        {
			//                            case 0:
			//                                {
			//                                    Screen.Effect.FogStart = 16;
			//                                    Screen.Effect.FogEnd = 19;
			//
			//                                    Game.Camera.FarPlane = 20;
			//                                    break;
			//                                }
			//                            case 1:
			//                                {
			//                                    Screen.Effect.FogStart = 36;
			//                                    Screen.Effect.FogEnd = 39;
			//
			//                                    Game.Camera.FarPlane = 40;
			//                                    break;
			//                                }
			//                            case 2:
			//                                {
			//                                    Screen.Effect.FogStart = 56;
			//                                    Screen.Effect.FogEnd = 59;
			//
			//                                    Game.Camera.FarPlane = 60;
			//                                    break;
			//                                }
			//                            case 3:
			//                                {
			//                                    Screen.Effect.FogStart = 76;
			//                                    Screen.Effect.FogEnd = 79;
			//
			//                                    Game.Camera.FarPlane = 80;
			//                                    break;
			//                                }
			//                            case 4:
			//                                {
			//                                    Screen.Effect.FogStart = 96;
			//                                    Screen.Effect.FogEnd = 99;
			//
			//                                    Game.Camera.FarPlane = 100;
			//                                    break;
			//                                }
			//                        }
			//
			//                        break;
			//                    }
			//                case DayTime.Day:
			//                    {
			//                        switch (Core.GameOptions.RenderDistance)
			//                        {
			//                            case 0:
			//                                {
			//                                    Screen.Effect.FogStart = 16;
			//                                    Screen.Effect.FogEnd = 19;
			//
			//                                    Game.Camera.FarPlane = 20;
			//                                    break;
			//                                }
			//                            case 1:
			//                                {
			//                                    Screen.Effect.FogStart = 36;
			//                                    Screen.Effect.FogEnd = 39;
			//
			//                                    Game.Camera.FarPlane = 40;
			//                                    break;
			//                                }
			//                            case 2:
			//                                {
			//                                    Screen.Effect.FogStart = 56;
			//                                    Screen.Effect.FogEnd = 59;
			//
			//                                    Game.Camera.FarPlane = 60;
			//                                    break;
			//                                }
			//                            case 3:
			//                                {
			//                                    Screen.Effect.FogStart = 76;
			//                                    Screen.Effect.FogEnd = 79;
			//
			//                                    Game.Camera.FarPlane = 80;
			//                                    break;
			//                                }
			//                            case 4:
			//                                {
			//                                    Screen.Effect.FogStart = 96;
			//                                    Screen.Effect.FogEnd = 99;
			//
			//                                    Game.Camera.FarPlane = 100;
			//                                    break;
			//                                }
			//                        }
			//
			//                        break;
			//                    }
			//                case DayTime.Evening:
			//                    {
			//                        switch (Core.GameOptions.RenderDistance)
			//                        {
			//                            case 0:
			//                                {
			//                                    Screen.Effect.FogStart = 0;
			//                                    Screen.Effect.FogEnd = 19;
			//
			//                                    Game.Camera.FarPlane = 20;
			//                                    break;
			//                                }
			//                            case 1:
			//                                {
			//                                    Screen.Effect.FogStart = 0;
			//                                    Screen.Effect.FogEnd = 39;
			//
			//                                    Game.Camera.FarPlane = 40;
			//                                    break;
			//                                }
			//                            case 2:
			//                                {
			//                                    Screen.Effect.FogStart = 0;
			//                                    Screen.Effect.FogEnd = 59;
			//
			//                                    Game.Camera.FarPlane = 60;
			//                                    break;
			//                                }
			//                            case 3:
			//                                {
			//                                    Screen.Effect.FogStart = 0;
			//                                    Screen.Effect.FogEnd = 79;
			//
			//                                    Game.Camera.FarPlane = 80;
			//                                    break;
			//                                }
			//                            case 4:
			//                                {
			//                                    Screen.Effect.FogStart = 0;
			//                                    Screen.Effect.FogEnd = 99;
			//
			//                                    Game.Camera.FarPlane = 100;
			//                                    break;
			//                                }
			//                        }
			//
			//                        break;
			//                    }
			//            }
			//
			//            break;
			//        }
			//    case EnvironmentTypes.Underwater:
			//        {
			//            switch (Core.GameOptions.RenderDistance)
			//            {
			//                case 0:
			//                    {
			//                        Screen.Effect.FogStart = 0;
			//                        Screen.Effect.FogEnd = 19;
			//
			//                        Game.Camera.FarPlane = 20;
			//                        break;
			//                    }
			//                case 1:
			//                    {
			//                        Screen.Effect.FogStart = 0;
			//                        Screen.Effect.FogEnd = 39;
			//
			//                        Game.Camera.FarPlane = 40;
			//                        break;
			//                    }
			//                case 2:
			//                    {
			//                        Screen.Effect.FogStart = 0;
			//                        Screen.Effect.FogEnd = 59;
			//
			//                        Game.Camera.FarPlane = 60;
			//                        break;
			//                    }
			//                case 3:
			//                    {
			//                        Screen.Effect.FogStart = 0;
			//                        Screen.Effect.FogEnd = 79;
			//
			//                        Game.Camera.FarPlane = 80;
			//                        break;
			//                    }
			//                case 4:
			//                    {
			//                        Screen.Effect.FogStart = 0;
			//                        Screen.Effect.FogEnd = 99;
			//
			//                        Game.Camera.FarPlane = 100;
			//                        break;
			//                    }
			//            }
			//
			//            break;
			//        }
			//}
			//
			//if (Core.GameOptions.RenderDistance >= 5)
			//{
			//    Screen.Effect.FogStart = 999;
			//    Screen.Effect.FogEnd = 1000;
			//
			//    Game.Camera.FarPlane = 1000;
			//}

			//endsub:
			//;
			Game.Camera.CreateNewProjection(Game.Camera.FOV);
		}

		private static Weathers GetRegionWeather(Seasons Season)
		{
			if (IsMainMenu)
				return Weathers.Clear;

			int r = Settings.Rand.Next(0, 100);

			switch (Season)
			{
				case Seasons.Winter:
					{
						if (r < 20)
							return Weathers.Rain;
						else if (r >= 20 & r < 50)
							return Weathers.Clear;
						else
							return Weathers.Snow;
					}
				case Seasons.Spring:
					{
						if (r < 5)
							return Weathers.Snow;
						else if (r >= 5 & r < 40)
							return Weathers.Rain;
						else
							return Weathers.Clear;
					}
				case Seasons.Summer:
					{
						if (r < 10)
							return Weathers.Rain;
						else
							return Weathers.Clear;
					}
				case Seasons.Fall:
					{
						if (r < 5)
							return Weathers.Snow;
						else if (r >= 5 & r < 80)
							return Weathers.Rain;
						else
							return Weathers.Clear;
					}
			}

			return Weathers.Clear;
		}

		public Weathers CurrentMapWeather = Weathers.Clear;

		public EnvironmentTypes EnvironmentType = EnvironmentTypes.Outside;
		public bool UseLightning = false;

		public World(int EnvironmentType, int WeatherType)
		{
			Initialize(EnvironmentType, WeatherType);
		}

		public static Weathers GetWeatherFromWeatherType(int WeatherType)
		{
			if (IsMainMenu)
				return Weathers.Clear;

			switch (WeatherType)
			{
				case 0: // Region Weather
					{
						return World.GetCurrentRegionWeather();
					}
				case 1: // Clear
					{
						return Weathers.Clear;
					}
				case 2: // Rain
					{
						return Weathers.Rain;
					}
				case 3: // Snow
					{
						return Weathers.Snow;
					}
				case 4: // Underwater
					{
						return Weathers.Underwater;
					}
				case 5: // Sunny
					{
						return Weathers.Sunny;
					}
				case 6: // Fog
					{
						return Weathers.Fog;
					}
				case 7: // Sandstorm
					{
						return Weathers.Sandstorm;
					}
				case 8: // Ash
					{
						return Weathers.Ash;
					}
				case 9: // Blizzard
					{
						return Weathers.Blizzard;
					}
				case 10: // Thunderstorm
					{
						return Weathers.Thunderstorm;
					}
			}
			return Weathers.Clear;
		}

		public static int GetWeatherTypeFromWeather(Weathers Weather)
		{
			if (IsMainMenu)
				return 1;

			switch (Weather)
			{
				case Weathers.Clear:
					{
						return 1;
					}
				case Weathers.Rain:
					{
						return 2;
					}
				case Weathers.Snow:
					{
						return 3;
					}
				case Weathers.Underwater:
					{
						return 4;
					}
				case Weathers.Sunny:
					{
						return 5;
					}
				case Weathers.Fog:
					{
						return 6;
					}
				case Weathers.Sandstorm:
					{
						return 7;
					}
				case Weathers.Ash:
					{
						return 8;
					}
				case Weathers.Blizzard:
					{
						return 9;
					}
				case Weathers.Thunderstorm:
					{
						return 10;
					}
				default:
					{
						return 0;
					}
			}
		}

		public void Initialize(int EnvironmentType, int WeatherType)
		{
			if (!_regionWeatherSet)
			{
				World._regionWeather = World.GetRegionWeather(World.CurrentSeason);
				World._regionWeatherSet = true;
			}

			this.CurrentMapWeather = GetWeatherFromWeatherType(WeatherType);

			switch (EnvironmentType)
			{
				case 0: // Overworld
					{
						this.EnvironmentType = EnvironmentTypes.Outside;
						this.UseLightning = true;
						break;
					}
				case 1: // Permanent Day
					{
						this.EnvironmentType = EnvironmentTypes.Inside;
						this.UseLightning = false;
						break;
					}
				case 2: // Cave
					{
						this.EnvironmentType = EnvironmentTypes.Cave;
						if (WeatherType == 0)
							this.CurrentMapWeather = Weathers.Clear;
						this.UseLightning = false;
						break;
					}
				case 3: // Permanent Night
					{
						this.EnvironmentType = EnvironmentTypes.Dark;
						if (WeatherType == 0)
							this.CurrentMapWeather = Weathers.Clear;
						this.UseLightning = false;
						break;
					}
				case 4: // Underwater
					{
						this.EnvironmentType = EnvironmentTypes.Underwater;
						if (WeatherType == 0)
							this.CurrentMapWeather = Weathers.Underwater;
						this.UseLightning = true;
						break;
					}
				case 5: // Forest
					{
						this.EnvironmentType = EnvironmentTypes.Forest;
						this.UseLightning = true;
						break;
					}
			}

			SetWeatherLevelColor();
			ChangeEnvironment();
			SetRenderDistance(this.EnvironmentType, this.CurrentMapWeather);
		}

		private void SetWeatherLevelColor()
		{
			//switch (CurrentMapWeather)
			//{
			//    case Weathers.Clear:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(1f, 1f, 1f);
			//            break;
			//        }
			//    case Weathers.Rain:
			//    case Weathers.Thunderstorm:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.4f, 0.4f, 0.7f);
			//            break;
			//        }
			//    case Weathers.Snow:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.8f, .8f, .8f);
			//            break;
			//        }
			//    case Weathers.Underwater:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.1f, 0.3f, 0.9f);
			//            break;
			//        }
			//    case Weathers.Sunny:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(1.6f, 1.3f, 1.3f);
			//            break;
			//        }
			//    case Weathers.Fog:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.6f);
			//            break;
			//        }
			//    case Weathers.Sandstorm:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.8f, 0.5f, 0.2f);
			//            break;
			//        }
			//    case Weathers.Ash:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
			//            break;
			//        }
			//    case Weathers.Blizzard:
			//        {
			//            Screen.Effect.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
			//            break;
			//        }
			//}
			//
			//Screen.Effect.DiffuseColor = Screen.SkyDome.GetWeatherColorMultiplier(Screen.Effect.DiffuseColor);
		}

		private UnityEngine.Color GetWeatherBackgroundColor(UnityEngine.Color defaultColor)
		{
			Vector3 v = Vector3.one;

			switch (CurrentMapWeather)
			{
				case World.Weathers.Clear:
				case Weathers.Sunny:
					{
						v = new Vector3(1, 1, 1);
						break;
					}
				case World.Weathers.Rain:
				case Weathers.Thunderstorm:
					{
						v = new Vector3(0.4f, 0.4f, 0.7f);
						break;
					}
				case World.Weathers.Snow:
					{
						v = new Vector3(0.8f, .8f, .8f);
						break;
					}
				case World.Weathers.Underwater:
					{
						v = new Vector3(0.1f, 0.3f, 0.9f);
						break;
					}
				case World.Weathers.Fog:
					{
						v = new Vector3(0.7f, 0.7f, 0.8f);
						break;
					}
				case World.Weathers.Sandstorm:
					{
						v = new Vector3(0.8f, 0.5f, 0.2f);
						break;
					}
				case Weathers.Ash:
					{
						v = new Vector3(0.5f, 0.5f, 0.5f);
						break;
					}
				case Weathers.Blizzard:
					{
						v = new Vector3(0.6f, 0.6f, 0.6f);
						break;
					}
			}

			Vector3 colorV = new Vector3();//defaultColor.ToVector3() * Screen.SkyDome.GetWeatherColorMultiplier(v);
			return colorV.ToColor();
		}

		private void ChangeEnvironment()
		{
			//switch (this.EnvironmentType)
			//{
			//    case EnvironmentTypes.Outside:
			//        {
			//            Core.BackgroundColor = GetWeatherBackgroundColor(SkyDome.GetDaytimeColor(false));
			//            Screen.Effect.FogColor = Core.BackgroundColor.ToVector3();
			//            if (IsAurora)
			//                Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\AuroraBoralis");
			//            else
			//                Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Clouds1");
			//            Screen.SkyDome.TextureDown = TextureManager.GetTexture(@"SkyDomeResource\Stars");
			//            break;
			//        }
			//    case EnvironmentTypes.Inside:
			//        {
			//            Core.BackgroundColor = GetWeatherBackgroundColor(new UnityEngine.Color(173, 216, 255));
			//            Screen.Effect.FogColor = Core.BackgroundColor.ToVector3();
			//            Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Clouds");
			//            Screen.SkyDome.TextureDown = null;
			//            break;
			//        }
			//    case EnvironmentTypes.Dark:
			//        {
			//            Core.BackgroundColor = GetWeatherBackgroundColor(new UnityEngine.Color(29, 29, 50));
			//            Screen.Effect.FogColor = Core.BackgroundColor.ToVector3();
			//            Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Dark");
			//            Screen.SkyDome.TextureDown = null;
			//            break;
			//        }
			//    case EnvironmentTypes.Cave:
			//        {
			//            Core.BackgroundColor = GetWeatherBackgroundColor(new UnityEngine.Color(34, 19, 12));
			//            Screen.Effect.FogColor = Core.BackgroundColor.ToVector3();
			//            Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Cave");
			//            Screen.SkyDome.TextureDown = null;
			//            break;
			//        }
			//    case EnvironmentTypes.Underwater:
			//        {
			//            Core.BackgroundColor = GetWeatherBackgroundColor(new UnityEngine.Color(19, 54, 117));
			//            Screen.Effect.FogColor = Core.BackgroundColor.ToVector3();
			//            Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Underwater");
			//            Screen.SkyDome.TextureDown = TextureManager.GetTexture(@"SkyDomeResource\UnderwaterGround");
			//            break;
			//        }
			//    case EnvironmentTypes.Forest:
			//        {
			//            Core.BackgroundColor = GetWeatherBackgroundColor(new UnityEngine.Color(30, 66, 21));
			//            Screen.Effect.FogColor = Core.BackgroundColor.ToVector3();
			//            Screen.SkyDome.TextureUp = TextureManager.GetTexture(@"SkyDomeResource\Forest");
			//            Screen.SkyDome.TextureDown = null;
			//            break;
			//        }
			//}
		}

		private static Vector2 WeatherOffset = new Vector2(0, 0);
		private static List<Vector4> ObjectsList = new List<Vector4>();

		public static Weathers[] NoParticlesList = new[] { Weathers.Clear, Weathers.Sunny, Weathers.Fog };

		public static void DrawWeather(Weathers MapWeather)
		{
			if (!NoParticlesList.Contains(MapWeather))
			{
				//if (Core.GameOptions.GraphicStyle == 1)
				//{
				//    Screen.Identifications[] identifications = new[] { Screen.Identifications.OverworldScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen };
				//    if (identifications.Contains(Core.CurrentScreen.Identification))
				//    {
				//        if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
				//        {
				//            if (!Game.TextBox.Showing)
				//                GenerateParticles(0, MapWeather);
				//        }
				//        else
				//            GenerateParticles(0, MapWeather);
				//    }
				//}
				//else
				//{
				Texture2D T = null;// TODO Change to default(_) if this is not a reference type

				int size = 128;
				int opacity = 30;

				switch (MapWeather)
				{
					case Weathers.Rain:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\rain");

							WeatherOffset.x += 8;
							WeatherOffset.y += 16;
							break;
						}
					case Weathers.Thunderstorm:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\rain");

							WeatherOffset.x += 12;
							WeatherOffset.y += 20;

							opacity = 50;
							break;
						}
					case Weathers.Snow:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\snow");

							WeatherOffset.x += 1;
							WeatherOffset.y += 1;
							break;
						}
					case Weathers.Blizzard:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\snow");

							WeatherOffset.x += 8;
							WeatherOffset.y += 2;

							opacity = 80;
							break;
						}
					case Weathers.Sandstorm:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\sand");

							WeatherOffset.x += 4;
							WeatherOffset.y += 1;

							opacity = 80;
							size = 48;
							break;
						}
					case Weathers.Underwater:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\bubble");

							//if (Settings.Rand.Next(0, 100) == 0)
							//    ObjectsList.Add(new Vector4(Settings.Rand.Next(0, Core.windowSize.width - 32), Core.windowSize.height, 32, 32));
							//
							//for (var i = 0; i <= ObjectsList.Count - 1; i++)
							//{
							//    Vector4 r = ObjectsList[i];
							//    ObjectsList[i] = new Vector4(r.x, r.y - 2, r.width, r.height);
							//
							//    Core.SpriteBatch.Draw(T, ObjectsList[i], new UnityEngine.Color(255, 255, 255, 150));
							//}

							break;
						}

					case Weathers.Ash:
						{
							T = TextureManager.GetTexture(@"Textures\Weather\ash2");

							WeatherOffset.y += 1;
							opacity = 65;
							size = 48;
							break;
						}
				}

				if (WeatherOffset.x >= size)
					WeatherOffset.x = 0;
				if (WeatherOffset.y >= size)
					WeatherOffset.y = 0;

				switch (MapWeather)
				{
					case Weathers.Rain:
					case Weathers.Snow:
					case Weathers.Sandstorm:
					case Weathers.Ash:
					case Weathers.Blizzard:
					case Weathers.Thunderstorm:
						{
							//for (var x = -size; x <= Core.windowSize.width; x += size)
							//{
							//    for (var y = -size; y <= Core.windowSize.height; y += size)
							//        Core.SpriteBatch.Draw(T, new Vector4(System.Convert.ToInt32(x + WeatherOffset.x), System.Convert.ToInt32(y + WeatherOffset.y), size, size), new UnityEngine.Color(255, 255, 255, opacity));
							//}

							break;
						}
				}
				//}
			}
		}

		public static void GenerateParticles(int chance, Weathers MapWeather)
		{
			if (MapWeather == Weathers.Thunderstorm)
			{
				if (Settings.Rand.Next(0, 250) == 0)
				{
					float pitch = -(Settings.Rand.Next(8, 11) / 10.0f);
					Game.DebugLog(pitch.ToString());
					SoundManager.PlaySound(@"Battle\Effects\effect_thunderbolt", pitch, 0F, Game.sVol, false);
				}
			}

			//if (!LevelLoader.IsBusy)
			//{
			//    Screen.Identifications[] validScreen = new[] { Screen.Identifications.OverworldScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen };
			//    if (validScreen.Contains(Core.CurrentScreen.Identification))
			//    {
			//        if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
			//        {
			//            if (!(OverworldScreen)Core.CurrentScreen.ActionScript.IsReady)
			//                return;
			//        }
			//
			//        Texture2D T = null;// TODO Change to default(_) if this is not a reference type
			//
			//        float speed;
			//        Vector3 scale = new Vector3(1,1,1);
			//        int range = 3;
			//
			//        switch (MapWeather)
			//        {
			//            case Weathers.Rain:
			//                {
			//                    speed = 0.1f;
			//                    T = TextureManager.GetTexture(@"Textures\Weather\rain3");
			//                    if (chance > -1)
			//                        chance = 3;
			//                    scale = new Vector3(0.03f, 0.06f, 0.1f);
			//                    break;
			//                }
			//            case Weathers.Thunderstorm:
			//                {
			//                    speed = 0.15f;
			//                    switch (Settings.Rand.Next(0, 4))
			//                    {
			//                        case 0:
			//                            {
			//                                T = TextureManager.GetTexture(@"Textures\Weather\rain2");
			//                                scale = new Vector3(0.1f, 0.1f, 0.1f);
			//                                break;
			//                            }
			//
			//                        default:
			//                            {
			//                                T = TextureManager.GetTexture(@"Textures\Weather\rain3");
			//                                scale = new Vector3(0.03f, 0.06f, 0.1f);
			//                                break;
			//                            }
			//                    }
			//                    if (chance > -1)
			//                        chance = 1;
			//                    break;
			//                }
			//            case Weathers.Snow:
			//                {
			//                    speed = 0.02f;
			//                    T = TextureManager.GetTexture(@"Textures\Weather\snow2");
			//                    if (chance > -1)
			//                        chance = 5;
			//                    scale = new Vector3(0.03f, 0.03f, 0.1f);
			//                    break;
			//                }
			//            case Weathers.Underwater:
			//                {
			//                    speed = -0.02f;
			//                    T = TextureManager.GetTexture(@"Textures\Weather\bubble");
			//                    if (chance > -1)
			//                        chance = 60;
			//                    scale = new Vector3(0.5f, 0.5f, 0.5f);
			//                    range = 1;
			//                    break;
			//                }
			//            case Weathers.Sandstorm:
			//                {
			//                    speed = 0.1f;
			//                    T = TextureManager.GetTexture(@"Textures\Weather\sand");
			//                    if (chance > -1)
			//                        chance = 4;
			//                    scale = new Vector3(0.03f, 0.03f, 0.1f);
			//                    break;
			//                }
			//            case Weathers.Ash:
			//                {
			//                    speed = 0.02f;
			//                    T = TextureManager.GetTexture(@"Textures\Weather\ash");
			//                    if (chance > -1)
			//                        chance = 20;
			//                    scale = new Vector3(0.03f, 0.03f, 0.1f);
			//                    break;
			//                }
			//            case Weathers.Blizzard:
			//                {
			//                    speed = 0.1f;
			//                    T = TextureManager.GetTexture(@"Textures\Weather\snow");
			//                    if (chance > -1)
			//                        chance = 1;
			//                    scale = new Vector3(0.12f, 0.12f, 0.1f);
			//                    break;
			//                }
			//        }
			//
			//        if (chance == -1)
			//            chance = 1;
			//
			//        Vector3 cameraPosition = Game.Camera.Position;
			//        if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
			//            cameraPosition = (OverworldCamera)Game.Camera.CPosition;
			//        else if (Core.CurrentScreen.Identification == Screen.Identifications.BattleScreen)
			//            cameraPosition = (BattleCamera)Game.Camera.CPosition;
			//
			//        if (Settings.Rand.Next(0, chance) == 0)
			//        {
			//            for (var x = cameraPosition.x - range; x <= cameraPosition.x + range; x++)
			//            {
			//                for (var z = cameraPosition.z - range; z <= cameraPosition.z + range; z++)
			//                {
			//                    if (z != 0 | x != 0)
			//                    {
			//                        float rY = System.Convert.ToSingle(Settings.Rand.Next(0, 40) / (double)10) - 2.0f;
			//                        float rX = System.Convert.ToSingle(Settings.Rand.NextDouble()) - 0.5f;
			//                        float rZ = System.Convert.ToSingle(Settings.Rand.NextDouble()) - 0.5f;
			//                        Particle p = new Particle(new Vector3(x + rX, cameraPosition.y + 1.8f + rY, z + rZ), new[] { T }, new int[] { 0, 0 }, Settings.Rand.Next(0, 2), scale/*, UnityEngine.Mesh.BillModel*/, new Vector3(1f, 1f, 1f));
			//                        p.MoveSpeed = speed;
			//                        if (MapWeather == Weathers.Rain)
			//                            p.Opacity = 0.7f;
			//                        if (MapWeather == Weathers.Thunderstorm)
			//                            p.Opacity = 1.0f;
			//                        if (MapWeather == Weathers.Underwater)
			//                        {
			//                            p.Position.y = 0.0f;
			//                            p.Destination = 10;
			//                            p.Behavior = Particle.Behaviors.Rising;
			//                        }
			//                        if (MapWeather == Weathers.Sandstorm)
			//                        {
			//                            p.Behavior = Particle.Behaviors.LeftToRight;
			//                            p.Destination = cameraPosition.x + 5;
			//                            p.Position.x -= 2;
			//                        }
			//                        if (MapWeather == Weathers.Blizzard)
			//                            p.Opacity = 1.0f;
			//                        Game.Level.Entities.Add(p);
			//                    }
			//                }
			//            }
			//        }
			//    }
			//}
		}

		private static Dictionary<Texture2D, Texture2D> SeasonTextureBuffer = new Dictionary<Texture2D, Texture2D>();
		private static Seasons BufferSeason = Seasons.Fall;

		public static Texture2D GetSeasonTexture(Texture2D seasonTexture, Texture2D T)
		{
			if (BufferSeason != CurrentSeason)
			{
				BufferSeason = CurrentSeason;
				SeasonTextureBuffer.Clear();
			}

			if (T != null)
			{
				if (SeasonTextureBuffer.ContainsKey(T))
					return SeasonTextureBuffer[T];

				int x = 0;
				int y = 0;
				switch (CurrentSeason)
				{
					case Seasons.Winter:
						{
							x = 0;
							y = 0;
							break;
						}
					case Seasons.Spring:
						{
							x = 2;
							y = 0;
							break;
						}
					case Seasons.Summer:
						{
							x = 0;
							y = 2;
							break;
						}
					case Seasons.Fall:
						{
							x = 2;
							y = 2;
							break;
						}
				}

				UnityEngine.Color[] inputColors = new UnityEngine.Color[]
				{
				new UnityEngine.Color(0, 0, 0),
				new UnityEngine.Color(85, 85, 85),
				new UnityEngine.Color(170, 170, 170),
				new UnityEngine.Color(255, 255, 255)
				}.Reverse().ToArray();
				List<UnityEngine.Color> outputColors = new List<UnityEngine.Color>();

				UnityEngine.Color[] Data = new UnityEngine.Color[4];
				//seasonTexture.GetData(0, new Vector4(x, y, 2, 2), Data, 0, 4);

				SeasonTextureBuffer.Add(T, T.ReplaceColors(inputColors, Data));
				return SeasonTextureBuffer[T];
			}
			return null;// TODO Change to default(_) if this is not a reference type
		}

		public static Seasons ServerSeason = Seasons.Spring;
		public static Weathers ServerWeather = Weathers.Clear;
		public static string ServerTimeData = "0,0,0"; // Format: Hours,Minutes,Seconds
		public static DateTime LastServerDataReceived = DateTime.Now;

		public static int SecondsOfDay
		{
			get
			{
				if (NeedServerObject())
				{
					string[] data = ServerTimeData.Split(System.Convert.ToChar(","));
					int hours = System.Convert.ToInt32(data[0]);
					int minutes = System.Convert.ToInt32(data[1]);
					int seconds = System.Convert.ToInt32(data[2]);

					seconds += System.Convert.ToInt32(Math.Abs((DateTime.Now - LastServerDataReceived).Seconds));

					return hours * 3600 + minutes * 60 + seconds;
				}
				else
					return DateTime.UtcNow.Hour * 3600 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second;
			}
		}

		public static int MinutesOfDay
		{
			get
			{
				if (NeedServerObject())
				{
					string[] data = ServerTimeData.Split(System.Convert.ToChar(","));
					int hours = System.Convert.ToInt32(data[0]);
					int minutes = System.Convert.ToInt32(data[1]);

					minutes += System.Convert.ToInt32(Math.Abs((DateTime.Now - LastServerDataReceived).Minutes));

					return hours * 60 + minutes;
				}
				else
					return DateTime.UtcNow.Hour * 60 + DateTime.UtcNow.Minute;
			}
		}

		private static bool NeedServerObject()
		{
			return false;//JoinServerScreen.Online & ConnectScreen.Connected;
		}

		/// <summary>
		/// Returns the region weather and gets the server weather if needed.
		/// </summary>
		public static Weathers GetCurrentRegionWeather()
		{
			if (NeedServerObject())
				return ServerWeather;
			else
				return _regionWeather;
		}

		public static Weathers RegionWeather
		{
			get
			{
				return _regionWeather;
			}
			set
			{
				_regionWeather = value;
			}
		}

		public static bool RegionWeatherSet
		{
			get
			{
				return _regionWeatherSet;
			}
			set
			{
				_regionWeatherSet = value;
			}
		}
	}
}
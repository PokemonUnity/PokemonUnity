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
	public static partial class MoveRoutes { 
		public const int Down               = 1;
		public const int Left               = 2;
		public const int Right              = 3;
		public const int Up                 = 4;
		public const int LowerLeft          = 5;
		public const int LowerRight         = 6;
		public const int UpperLeft          = 7;
		public const int UpperRight         = 8;
		public const int Random             = 9;
		public const int TowardPlayer       = 10;
		public const int AwayFromPlayer     = 11;
		public const int Forward            = 12;
		public const int Backward           = 13;
		public const int Jump               = 14; // xoffset, yoffset
		public const int Wait               = 15; // frames
		public const int TurnDown           = 16;
		public const int TurnLeft           = 17;
		public const int TurnRight          = 18;
		public const int TurnUp             = 19;
		public const int TurnRight90        = 20;
		public const int TurnLeft90         = 21;
		public const int Turn180            = 22;
		public const int TurnRightOrLeft90  = 23;
		public const int TurnRandom         = 24;
		public const int TurnTowardPlayer   = 25;
		public const int TurnAwayFromPlayer = 26;
		public const int SwitchOn           = 27; // 1 param
		public const int SwitchOff          = 28; // 1 param
		public const int ChangeSpeed        = 29; // 1 param
		public const int ChangeFreq         = 30; // 1 param
		public const int WalkAnimeOn        = 31;
		public const int WalkAnimeOff       = 32;
		public const int StepAnimeOn        = 33;
		public const int StepAnimeOff       = 34;
		public const int DirectionFixOn     = 35;
		public const int DirectionFixOff    = 36;
		public const int ThroughOn          = 37;
		public const int ThroughOff         = 38;
		public const int AlwaysOnTopOn      = 39;
		public const int AlwaysOnTopOff     = 40;
		public const int Graphic            = 41; // Name, hue, direction, pattern
		public const int Opacity            = 42; // 1 param
		public const int Blending           = 43; // 1 param
		public const int PlaySE             = 44; // 1 param
		public const int Script             = 45; // 1 param
		public const int ScriptAsync        = 101; // 1 param
	}
}
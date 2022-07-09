using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;

namespace PokemonUnity
{
	public static partial class PokeBattle_SceneConstants
	{
		public static readonly bool USECOMMANDBOX			= true; // If true, expects the file Graphics/Pictures/battleCommand.png
		public static readonly bool USEFIGHTBOX				= true; // If true, expects the file Graphics/Pictures/battleFight.png

		//  Text colors
		public static readonly IColor MESSAGEBASECOLOR			; //= new Color(80,80,88);
		public static readonly IColor MESSAGESHADOWCOLOR		; //= new Color(160,160,168);
		public static readonly IColor MENUBASECOLOR				; //= MESSAGEBASECOLOR;
		public static readonly IColor MENUSHADOWCOLOR			; //= MESSAGESHADOWCOLOR;
		public static readonly IColor BOXTEXTBASECOLOR			; //= new Color(72,72,72);
		public static readonly IColor BOXTEXTSHADOWCOLOR		; //= new Color(184,184,184);
		public static readonly IColor PPTEXTBASECOLOR			; //= MESSAGEBASECOLOR;        // More than 1/2 of total PP
		public static readonly IColor PPTEXTSHADOWCOLOR			; //= MESSAGESHADOWCOLOR;
		public static readonly IColor PPTEXTBASECOLORYELLOW		; //= new Color(248,192,0);    // 1/2 of total PP or less
		public static readonly IColor PPTEXTSHADOWCOLORYELLOW	; //= new Color(144,104,0);
		public static readonly IColor PPTEXTBASECOLORORANGE		; //= new Color(248,136,32);   // 1/4 of total PP or less
		public static readonly IColor PPTEXTSHADOWCOLORORANGE	; //= new Color(144,72,24);
		public static readonly IColor PPTEXTBASECOLORRED		; //= new Color(248,72,72);    // Zero PP
		public static readonly IColor PPTEXTSHADOWCOLORRED		; //= new Color(136,48,48);
																
		//  HP bar colors										
		public static readonly IColor HPCOLORGREEN				; //= new Color(24,192,32);
		public static readonly IColor HPCOLORGREENDARK			; //= new Color(0,144,0);
		public static readonly IColor HPCOLORYELLOW				; //= new Color(248,176,0);
		public static readonly IColor HPCOLORYELLOWDARK			; //= new Color(176,104,8);
		public static readonly IColor HPCOLORRED				; //= new Color(248,88,40);
		public static readonly IColor HPCOLORREDDARK			; //= new Color(168,48,56);
																
		//  Exp bar colors										
		public static readonly IColor EXPCOLORBASE				; //= new Color(72,144,248);
		public static readonly IColor EXPCOLORSHADOW			; //= new Color(48,96,216);

		//  Position and width of HP/Exp bars
		public static readonly float HPGAUGE_X				= 102;
		public static readonly float HPGAUGE_Y				= 40;
		public static readonly float HPGAUGESIZE			= 96;
		public static readonly float EXPGAUGE_X				= 6;
		public static readonly float EXPGAUGE_Y				= 76;
		public static readonly float EXPGAUGESIZE			= 192;

		//  Coordinates of the top left of the player's data boxes
		public static readonly float PLAYERBOX_X			= (Game.GameData as Game).Graphics.width - 244;
		public static readonly float PLAYERBOX_Y			= (Game.GameData as Game).Graphics.height - 192;
		public static readonly float PLAYERBOXD1_X			= PLAYERBOX_X - 12;
		public static readonly float PLAYERBOXD1_Y			= PLAYERBOX_Y - 20;
		public static readonly float PLAYERBOXD2_X			= PLAYERBOX_X;
		public static readonly float PLAYERBOXD2_Y			= PLAYERBOX_Y + 34;

		//  Coordinates of the top left of the foe's data boxes
		public static readonly float FOEBOX_X				= -16;
		public static readonly float FOEBOX_Y				= 36;
		public static readonly float FOEBOXD1_X				= FOEBOX_X + 12;
		public static readonly float FOEBOXD1_Y				= FOEBOX_Y - 34;
		public static readonly float FOEBOXD2_X				= FOEBOX_X;
		public static readonly float FOEBOXD2_Y				= FOEBOX_Y + 20;

		//  Coordinates of the top left of the player's Safari game data box
		public static readonly float SAFARIBOX_X			= (Game.GameData as Game).Graphics.width - 232;
		public static readonly float SAFARIBOX_Y			= (Game.GameData as Game).Graphics.height - 184;

		//  Coordinates of the party bars and balls of both sides
		//  Coordinates are the top left of the graphics except where specified
		public static readonly float PLAYERPARTYBAR_X		= (Game.GameData as Game).Graphics.width - 248;
		public static readonly float PLAYERPARTYBAR_Y		= (Game.GameData as Game).Graphics.height - 142;
		public static readonly float PLAYERPARTYBALL1_X		= PLAYERPARTYBAR_X + 44;
		public static readonly float PLAYERPARTYBALL1_Y		= PLAYERPARTYBAR_Y - 30;
		public static readonly float PLAYERPARTYBALL_GAP	= 32;
		public static readonly float FOEPARTYBAR_X			= 248;   // Coordinates of end of bar nearest screen middle
		public static readonly float FOEPARTYBAR_Y			= 114;
		public static readonly float FOEPARTYBALL1_X		= FOEPARTYBAR_X - 44 - 30;   // 30 is width of ball icon
		public static readonly float FOEPARTYBALL1_Y		= FOEPARTYBAR_Y - 30;
		public static readonly float FOEPARTYBALL_GAP		= 32;   // Distance between centers of two adjacent balls

		//  Coordinates of the center bottom of the player's battler's sprite
		//  Is also the center middle of its shadow
		public static readonly float PLAYERBATTLER_X		= 128;
		public static readonly float PLAYERBATTLER_Y		= (Game.GameData as Game).Graphics.height - 80;
		public static readonly float PLAYERBATTLERD1_X		= PLAYERBATTLER_X - 48;
		public static readonly float PLAYERBATTLERD1_Y		= PLAYERBATTLER_Y;
		public static readonly float PLAYERBATTLERD2_X		= PLAYERBATTLER_X + 32;
		public static readonly float PLAYERBATTLERD2_Y		= PLAYERBATTLER_Y + 16;

		//  Coordinates of the center bottom of the foe's battler's sprite
		//  Is also the center middle of its shadow
		public static readonly float FOEBATTLER_X			= (Game.GameData as Game).Graphics.width - 128;
		public static readonly float FOEBATTLER_Y			= ((Game.GameData as Game).Graphics.height* 3/4) - 112;
		public static readonly float FOEBATTLERD1_X			= FOEBATTLER_X + 48;
		public static readonly float FOEBATTLERD1_Y			= FOEBATTLER_Y;
		public static readonly float FOEBATTLERD2_X			= FOEBATTLER_X - 32;
		public static readonly float FOEBATTLERD2_Y			= FOEBATTLER_Y - 16;

		//  center bottom of the player's side base graphic
		public static readonly float PLAYERBASEX			= PLAYERBATTLER_X;
		public static readonly float PLAYERBASEY			= PLAYERBATTLER_Y;

		//  center middle of the foe's side base graphic
		public static readonly float FOEBASEX				= FOEBATTLER_X;
		public static readonly float FOEBASEY				= FOEBATTLER_Y;

		//  Coordinates of the center bottom of the player's sprite
		public static readonly float PLAYERTRAINER_X		= PLAYERBATTLER_X;
		public static readonly float PLAYERTRAINER_Y		= PLAYERBATTLER_Y - 16;
		public static readonly float PLAYERTRAINERD1_X		= PLAYERBATTLERD1_X;
		public static readonly float PLAYERTRAINERD1_Y		= PLAYERTRAINER_Y;
		public static readonly float PLAYERTRAINERD2_X		= PLAYERBATTLERD2_X;
		public static readonly float PLAYERTRAINERD2_Y		= PLAYERTRAINER_Y;

		//  Coordinates of the center bottom of the foe trainer's sprite
		public static readonly float FOETRAINER_X			= FOEBATTLER_X;
		public static readonly float FOETRAINER_Y			= FOEBATTLER_Y + 6;
		public static readonly float FOETRAINERD1_X			= FOEBATTLERD1_X;
		public static readonly float FOETRAINERD1_Y			= FOEBATTLERD1_Y + 6;
		public static readonly float FOETRAINERD2_X			= FOEBATTLERD2_X;
		public static readonly float FOETRAINERD2_Y			= FOEBATTLERD2_Y + 6;

		#region Default focal points of user and target in animations - do not change!
		public static readonly float FOCUSUSER_X			= 128;   // 144
		public static readonly float FOCUSUSER_Y			= 224;   // 188
		public static readonly float FOCUSTARGET_X			= 384;   // 352
		public static readonly float FOCUSTARGET_Y			= 96;    // 108, 98
		#endregion
	}
}
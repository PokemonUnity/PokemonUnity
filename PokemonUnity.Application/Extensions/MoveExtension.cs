using System.Collections;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity
{
	public static class MoveExtension
	{
		#region Targets
		public static bool HasMultipleTargets(this PokemonUnity.Attack.Data.Targets target)
		{
			return
				//target == Attack.Data.Targets.ENTIRE_FIELD ||
				//target == Attack.Data.Targets.OPPONENTS_FIELD ||
				//target == Attack.Data.Targets.USERS_FIELD ||
				target == Attack.Data.Targets.ALL_OPPONENTS ||
				target == Attack.Data.Targets.ALL_OTHER_POKEMON ||
				target == Attack.Data.Targets.ALL_POKEMON;
		}
		public static bool TargetsOneOpponent(this PokemonUnity.Attack.Data.Targets target)
		{
			return
				target == Attack.Data.Targets.SELECTED_POKEMON ||
				target == Attack.Data.Targets.SELECTED_POKEMON_ME_FIRST ||
				target == Attack.Data.Targets.RANDOM_OPPONENT;
		}
		#endregion
		public static bool IsNotNullOrNone(this PokemonUnity.Attack.Move move)
		{
			return move != null || move.MoveId != Moves.NONE;
		}
		public static bool IsNotNullOrNone(this PokemonUnity.Combat.Move move)
		{
			return move != null || move.MoveId != Moves.NONE;
		}
		public static bool IsNotNullOrNone(this IBattleMove move)
		{
			return move != null || move.MoveId != Moves.NONE;
		}
		public static string ToString(this PokemonUnity.Moves move, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return move.ToString();
		}

		public static IBattleMove ToBattleMove(this Attack.Data.Effects effect)
		{
			switch (effect)
			{
				case Attack.Data.Effects.NONE:
					break;
				case Attack.Data.Effects.x001:
					return new Combat.PokeBattle_Move_000();
				case Attack.Data.Effects.x002:
					return new Combat.PokeBattle_Move_003();
				case Attack.Data.Effects.x003:
					return new Combat.PokeBattle_Move_005();
				case Attack.Data.Effects.x004:
					return new Combat.PokeBattle_Move_0DD();
				case Attack.Data.Effects.x005:
					return new Combat.PokeBattle_Move_00A();
				case Attack.Data.Effects.x006:
					return new Combat.PokeBattle_Move_00C();
				case Attack.Data.Effects.x007:
					return new Combat.PokeBattle_Move_007();
				case Attack.Data.Effects.x008:
					return new Combat.PokeBattle_Move_0E0();
				case Attack.Data.Effects.x009:
					return new Combat.PokeBattle_Move_0DE();
				case Attack.Data.Effects.x00A:
					return new Combat.PokeBattle_Move_0AE();
				case Attack.Data.Effects.x00B:
					return new Combat.PokeBattle_Move_01C();
				case Attack.Data.Effects.x00C:
					return new Combat.PokeBattle_Move_01D();
				case Attack.Data.Effects.x00E:
					break;
				case Attack.Data.Effects.x011:
					return new Combat.PokeBattle_Move_022();
				case Attack.Data.Effects.x012:
					return new Combat.PokeBattle_Move_0A5();
				case Attack.Data.Effects.x013:
					break;
				case Attack.Data.Effects.x014:
					//ToDo: Tail whip has just one effect
					return new Combat.PokeBattle_Move_043();
				case Attack.Data.Effects.x015:
					//ToDo: low-sweep and electroweb
					return new Combat.PokeBattle_Move_044();
				case Attack.Data.Effects.x018:
					//ToDo: Only has one effect
					return new Combat.PokeBattle_Move_047();
				case Attack.Data.Effects.x019:
					return new Combat.PokeBattle_Move_048();
				case Attack.Data.Effects.x01A:
					return new Combat.PokeBattle_Move_051();
				case Attack.Data.Effects.x01B:
					return new Combat.PokeBattle_Move_0D4();
				case Attack.Data.Effects.x01C:
					return new Combat.PokeBattle_Move_0D2();
				case Attack.Data.Effects.x01D:
					return new Combat.PokeBattle_Move_0EB();
				case Attack.Data.Effects.x01E:
					return new Combat.PokeBattle_Move_0C0();
				case Attack.Data.Effects.x01F:
					return new Combat.PokeBattle_Move_05E();
				case Attack.Data.Effects.x020:
					return new Combat.PokeBattle_Move_00F();
				case Attack.Data.Effects.x021:
					return new Combat.PokeBattle_Move_0D5();
				case Attack.Data.Effects.x022:
					return new Combat.PokeBattle_Move_006();
				case Attack.Data.Effects.x023:
					return new Combat.PokeBattle_Move_109();
				case Attack.Data.Effects.x024:
					return new Combat.PokeBattle_Move_0A3();
				case Attack.Data.Effects.x025:
					return new Combat.PokeBattle_Move_017();
				case Attack.Data.Effects.x026:
					return new Combat.PokeBattle_Move_0D9();
				case Attack.Data.Effects.x027:
					return new Combat.PokeBattle_Move_070();
				case Attack.Data.Effects.x028:
					return new Combat.PokeBattle_Move_0C3();
				case Attack.Data.Effects.x029:
					return new Combat.PokeBattle_Move_06C();
				case Attack.Data.Effects.x02A:
					return new Combat.PokeBattle_Move_06B();
				case Attack.Data.Effects.x02B:
					return new Combat.PokeBattle_Move_0CF();
				case Attack.Data.Effects.x02C:
					//ToDo: has an effect...
					return new Combat.PokeBattle_Move_000();
				case Attack.Data.Effects.x02D:
					return new Combat.PokeBattle_Move_0BD();
				case Attack.Data.Effects.x02E:
					return new Combat.PokeBattle_Move_10B();
				case Attack.Data.Effects.x02F:
					return new Combat.PokeBattle_Move_056();
				case Attack.Data.Effects.x030:
					return new Combat.PokeBattle_Move_023();
				case Attack.Data.Effects.x031:
					return new Combat.PokeBattle_Move_0FA();
				case Attack.Data.Effects.x032:
					return new Combat.PokeBattle_Move_12A();
				case Attack.Data.Effects.x033:
					return new Combat.PokeBattle_Move_02E();
				case Attack.Data.Effects.x034:
					return new Combat.PokeBattle_Move_02F();
				case Attack.Data.Effects.x035:
					return new Combat.PokeBattle_Move_030();
				case Attack.Data.Effects.x036:
					return new Combat.PokeBattle_Move_032();
				case Attack.Data.Effects.x037:
					return new Combat.PokeBattle_Move_033();
				case Attack.Data.Effects.x03A:
					return new Combat.PokeBattle_Move_069();
				case Attack.Data.Effects.x03B:
					return new Combat.PokeBattle_Move_04B();
				case Attack.Data.Effects.x03C:
					return new Combat.PokeBattle_Move_04C();
				case Attack.Data.Effects.x03D:
					return new Combat.PokeBattle_Move_04D();
				case Attack.Data.Effects.x03E:
					return new Combat.PokeBattle_Move_13D();
				case Attack.Data.Effects.x03F:
					return new Combat.PokeBattle_Move_04F();
				case Attack.Data.Effects.x042:
					return new Combat.PokeBattle_Move_0A2();
				case Attack.Data.Effects.x043:
					break;
				case Attack.Data.Effects.x044:
					break;
				case Attack.Data.Effects.x045:
					break;
				case Attack.Data.Effects.x046:
					return new Combat.PokeBattle_Move_043();
				case Attack.Data.Effects.x047:
					return new Combat.PokeBattle_Move_044();
				case Attack.Data.Effects.x048:
					return new Combat.PokeBattle_Move_045();
				case Attack.Data.Effects.x049:
					return new Combat.PokeBattle_Move_046();
				case Attack.Data.Effects.x04A:
					return new Combat.PokeBattle_Move_047();
				case Attack.Data.Effects.x04B:
					break;
				case Attack.Data.Effects.x04C:
					return new Combat.PokeBattle_Move_0C7();
				case Attack.Data.Effects.x04D:
					return new Combat.PokeBattle_Move_013();
				case Attack.Data.Effects.x04E:
					return new Combat.PokeBattle_Move_0BE();
				case Attack.Data.Effects.x04F:
					break;
				case Attack.Data.Effects.x050:
					return new Combat.PokeBattle_Move_10C();
				case Attack.Data.Effects.x051:
					return new Combat.PokeBattle_Move_0C2();
				case Attack.Data.Effects.x052:
					return new Combat.PokeBattle_Move_093();
				case Attack.Data.Effects.x053:
					return new Combat.PokeBattle_Move_05C();
				case Attack.Data.Effects.x054:
					return new Combat.PokeBattle_Move_0B6();
				case Attack.Data.Effects.x055:
					return new Combat.PokeBattle_Move_0DC();
				case Attack.Data.Effects.x056:
					return new Combat.PokeBattle_Move_001();
				case Attack.Data.Effects.x057:
					return new Combat.PokeBattle_Move_0B9();
				case Attack.Data.Effects.x058:
					return new Combat.PokeBattle_Move_06D();
				case Attack.Data.Effects.x059:
					return new Combat.PokeBattle_Move_06F();
				case Attack.Data.Effects.x05A:
					return new Combat.PokeBattle_Move_071();
				case Attack.Data.Effects.x05B:
					return new Combat.PokeBattle_Move_0BC();
				case Attack.Data.Effects.x05C:
					return new Combat.PokeBattle_Move_05A();
				case Attack.Data.Effects.x05D:
					return new Combat.PokeBattle_Move_011();
				case Attack.Data.Effects.x05E:
					return new Combat.PokeBattle_Move_05F();
				case Attack.Data.Effects.x05F:
					return new Combat.PokeBattle_Move_0A6();
				case Attack.Data.Effects.x060:
					return new Combat.PokeBattle_Move_05D();
				case Attack.Data.Effects.x062:
					return new Combat.PokeBattle_Move_0B4();
				case Attack.Data.Effects.x063:
					return new Combat.PokeBattle_Move_0E7();
				case Attack.Data.Effects.x064:
					return new Combat.PokeBattle_Move_098();
				case Attack.Data.Effects.x065:
					return new Combat.PokeBattle_Move_10E();
				case Attack.Data.Effects.x066:
					return new Combat.PokeBattle_Move_0E9();
				case Attack.Data.Effects.x067:
					return new Combat.PokeBattle_Move_019();
				case Attack.Data.Effects.x068:
					break;
				case Attack.Data.Effects.x069:
					return new Combat.PokeBattle_Move_0BF();
				case Attack.Data.Effects.x06A:
					return new Combat.PokeBattle_Move_0F1();
				case Attack.Data.Effects.x06B:
					return new Combat.PokeBattle_Move_0EF();
				case Attack.Data.Effects.x06C:
					return new Combat.PokeBattle_Move_10F();
				case Attack.Data.Effects.x06D:
					return new Combat.PokeBattle_Move_034();
				case Attack.Data.Effects.x06E:
					return new Combat.PokeBattle_Move_10D();
				case Attack.Data.Effects.x070:
					return new Combat.PokeBattle_Move_0AA();
				case Attack.Data.Effects.x071:
					return new Combat.PokeBattle_Move_103();
				case Attack.Data.Effects.x072:
					return new Combat.PokeBattle_Move_0A7();
				case Attack.Data.Effects.x073:
					return new Combat.PokeBattle_Move_0E5();
				case Attack.Data.Effects.x074:
					return new Combat.PokeBattle_Move_101();
				case Attack.Data.Effects.x075:
					return new Combat.PokeBattle_Move_0E8();
				case Attack.Data.Effects.x076:
					return new Combat.PokeBattle_Move_0D3();
				case Attack.Data.Effects.x077:
					return new Combat.PokeBattle_Move_041();
				case Attack.Data.Effects.x078:
					return new Combat.PokeBattle_Move_091();
				case Attack.Data.Effects.x079:
					return new Combat.PokeBattle_Move_016();
				case Attack.Data.Effects.x07A:
					return new Combat.PokeBattle_Move_089();
				case Attack.Data.Effects.x07B:
					return new Combat.PokeBattle_Move_094();
				case Attack.Data.Effects.x07C:
					return new Combat.PokeBattle_Move_08A();
				case Attack.Data.Effects.x07D:
					return new Combat.PokeBattle_Move_01A();
				case Attack.Data.Effects.x07E:
					break;
				case Attack.Data.Effects.x07F:
					return new Combat.PokeBattle_Move_095();
				case Attack.Data.Effects.x080:
					return new Combat.PokeBattle_Move_0ED();
				case Attack.Data.Effects.x081:
					return new Combat.PokeBattle_Move_088();
				case Attack.Data.Effects.x082:
					return new Combat.PokeBattle_Move_110();
				case Attack.Data.Effects.x083:
					return new Combat.PokeBattle_Move_06A();
				case Attack.Data.Effects.x085:
					return new Combat.PokeBattle_Move_0D8();
				case Attack.Data.Effects.x088:
					return new Combat.PokeBattle_Move_090();
				case Attack.Data.Effects.x089:
					return new Combat.PokeBattle_Move_100();
				case Attack.Data.Effects.x08A:
					return new Combat.PokeBattle_Move_0FF();
				case Attack.Data.Effects.x08B:
					//ToDo: Has other effects beyond this class
					return new Combat.PokeBattle_Move_01D();
				case Attack.Data.Effects.x08C:
					break;
				case Attack.Data.Effects.x08D:
					return new Combat.PokeBattle_Move_02D();
				case Attack.Data.Effects.x08F:
					return new Combat.PokeBattle_Move_03A();
				case Attack.Data.Effects.x090:
					return new Combat.PokeBattle_Move_055();
				case Attack.Data.Effects.x091:
					return new Combat.PokeBattle_Move_072();
				case Attack.Data.Effects.x092:
					return new Combat.PokeBattle_Move_0C8();
				case Attack.Data.Effects.x093:
					return new Combat.PokeBattle_Move_078();
				case Attack.Data.Effects.x094:
					return new Combat.PokeBattle_Move_076();
				case Attack.Data.Effects.x095:
					return new Combat.PokeBattle_Move_111();
				case Attack.Data.Effects.x096:
					return new Combat.PokeBattle_Move_077();
				case Attack.Data.Effects.x097:
					return new Combat.PokeBattle_Move_010();
				case Attack.Data.Effects.x098:
					return new Combat.PokeBattle_Move_0C4();
				case Attack.Data.Effects.x099:
					return new Combat.PokeBattle_Move_008();
				case Attack.Data.Effects.x09A:
					return new Combat.PokeBattle_Move_0EA();
				case Attack.Data.Effects.x09B:
					return new Combat.PokeBattle_Move_0C1();
				case Attack.Data.Effects.x09C:
					return new Combat.PokeBattle_Move_0C9();
				case Attack.Data.Effects.x09D:
					return new Combat.PokeBattle_Move_01E();
				case Attack.Data.Effects.x09F:
					return new Combat.PokeBattle_Move_012();
				case Attack.Data.Effects.x0A0:
					return new Combat.PokeBattle_Move_0D1();
				case Attack.Data.Effects.x0A1:
					return new Combat.PokeBattle_Move_112();
				case Attack.Data.Effects.x0A2:
					return new Combat.PokeBattle_Move_113();
				case Attack.Data.Effects.x0A3:
					return new Combat.PokeBattle_Move_114();
				case Attack.Data.Effects.x0A5:
					return new Combat.PokeBattle_Move_102();
				case Attack.Data.Effects.x0A6:
					return new Combat.PokeBattle_Move_0B7();
				case Attack.Data.Effects.x0A7:
					return new Combat.PokeBattle_Move_040();
				case Attack.Data.Effects.x0A8:
					break;
				case Attack.Data.Effects.x0A9:
					return new Combat.PokeBattle_Move_0E2();
				case Attack.Data.Effects.x0AA:
					return new Combat.PokeBattle_Move_07E();
				case Attack.Data.Effects.x0AB:
					return new Combat.PokeBattle_Move_115();
				case Attack.Data.Effects.x0AC:
					return new Combat.PokeBattle_Move_07C();
				case Attack.Data.Effects.x0AD:
					return new Combat.PokeBattle_Move_117();
				case Attack.Data.Effects.x0AE:
					return new Combat.PokeBattle_Move_0B3();
				case Attack.Data.Effects.x0AF:
					return new Combat.PokeBattle_Move_021();
				case Attack.Data.Effects.x0B0:
					return new Combat.PokeBattle_Move_0BA();
				case Attack.Data.Effects.x0B1:
					return new Combat.PokeBattle_Move_09C();
				case Attack.Data.Effects.x0B2:
					return new Combat.PokeBattle_Move_0F2();
				case Attack.Data.Effects.x0B3:
					return new Combat.PokeBattle_Move_065();
				case Attack.Data.Effects.x0B4:
					return new Combat.PokeBattle_Move_0D7();
				case Attack.Data.Effects.x0B5:
					return new Combat.PokeBattle_Move_0B5();
				case Attack.Data.Effects.x0B6:
					return new Combat.PokeBattle_Move_0DB();
				case Attack.Data.Effects.x0B7:
					return new Combat.PokeBattle_Move_03B();
				case Attack.Data.Effects.x0B8:
					return new Combat.PokeBattle_Move_0B1();
				case Attack.Data.Effects.x0B9:
					return new Combat.PokeBattle_Move_0F6();
				case Attack.Data.Effects.x0BA:
					return new Combat.PokeBattle_Move_081();
				case Attack.Data.Effects.x0BB:
					return new Combat.PokeBattle_Move_10A();
				case Attack.Data.Effects.x0BC:
					return new Combat.PokeBattle_Move_004();
				case Attack.Data.Effects.x0BD:
					return new Combat.PokeBattle_Move_0F0();
				case Attack.Data.Effects.x0BE:
					return new Combat.PokeBattle_Move_06E();
				case Attack.Data.Effects.x0BF:
					return new Combat.PokeBattle_Move_08B();
				case Attack.Data.Effects.x0C0:
					return new Combat.PokeBattle_Move_067();
				case Attack.Data.Effects.x0C1:
					return new Combat.PokeBattle_Move_0B8();
				case Attack.Data.Effects.x0C2:
					return new Combat.PokeBattle_Move_018();
				case Attack.Data.Effects.x0C3:
					return new Combat.PokeBattle_Move_0E6();
				case Attack.Data.Effects.x0C4:
					return new Combat.PokeBattle_Move_0B2();
				case Attack.Data.Effects.x0C5:
					return new Combat.PokeBattle_Move_09A();
				case Attack.Data.Effects.x0C6:
					return new Combat.PokeBattle_Move_0A4();
				case Attack.Data.Effects.x0C7:
					return new Combat.PokeBattle_Move_0FB();
				case Attack.Data.Effects.x0C8:
					break;
				case Attack.Data.Effects.x0C9:
					break;
				case Attack.Data.Effects.x0CA:
					return new Combat.PokeBattle_Move_09D();
				case Attack.Data.Effects.x0CB:
					break;
				case Attack.Data.Effects.x0CC:
					return new Combat.PokeBattle_Move_087();
				case Attack.Data.Effects.x0CD:
					return new Combat.PokeBattle_Move_03F();
				case Attack.Data.Effects.x0CE:
					return new Combat.PokeBattle_Move_04A();
				case Attack.Data.Effects.x0CF:
					return new Combat.PokeBattle_Move_02A();
				case Attack.Data.Effects.x0D0:
					return new Combat.PokeBattle_Move_11B();
				case Attack.Data.Effects.x0D1:
					return new Combat.PokeBattle_Move_024();
				case Attack.Data.Effects.x0D2:
					break;
				case Attack.Data.Effects.x0D3:
					return new Combat.PokeBattle_Move_09E();
				case Attack.Data.Effects.x0D4:
					return new Combat.PokeBattle_Move_02C();
				case Attack.Data.Effects.x0D5:
					return new Combat.PokeBattle_Move_026();
				case Attack.Data.Effects.x0D6:
					return new Combat.PokeBattle_Move_060();
				case Attack.Data.Effects.x0D7:
					return new Combat.PokeBattle_Move_0D6();
				case Attack.Data.Effects.x0D8:
					return new Combat.PokeBattle_Move_118();
				case Attack.Data.Effects.x0D9:
					return new Combat.PokeBattle_Move_0A8();
				case Attack.Data.Effects.x0DA:
					return new Combat.PokeBattle_Move_07D();
				case Attack.Data.Effects.x0DB:
					return new Combat.PokeBattle_Move_03E();
				case Attack.Data.Effects.x0DC:
					return new Combat.PokeBattle_Move_08D();
				case Attack.Data.Effects.x0DD:
					return new Combat.PokeBattle_Move_0E3();
				case Attack.Data.Effects.x0DE:
					return new Combat.PokeBattle_Move_080();
				case Attack.Data.Effects.x0DF:
					return new Combat.PokeBattle_Move_096();
				case Attack.Data.Effects.x0E0:
					return new Combat.PokeBattle_Move_0AD();
				case Attack.Data.Effects.x0E1:
					return new Combat.PokeBattle_Move_0F4();
				case Attack.Data.Effects.x0E2:
					return new Combat.PokeBattle_Move_05B();
				case Attack.Data.Effects.x0E3:
					return new Combat.PokeBattle_Move_037();
				case Attack.Data.Effects.x0E4:
					return new Combat.PokeBattle_Move_073();
				case Attack.Data.Effects.x0E5:
					return new Combat.PokeBattle_Move_0EE();
				case Attack.Data.Effects.x0E6:
					return new Combat.PokeBattle_Move_03C();
				case Attack.Data.Effects.x0E7:
					return new Combat.PokeBattle_Move_084();
				case Attack.Data.Effects.x0E8:
					return new Combat.PokeBattle_Move_082();
				case Attack.Data.Effects.x0E9:
					return new Combat.PokeBattle_Move_0F8();
				case Attack.Data.Effects.x0EA:
					return new Combat.PokeBattle_Move_0F7();
				case Attack.Data.Effects.x0EB:
					return new Combat.PokeBattle_Move_01B();
				case Attack.Data.Effects.x0EC:
					return new Combat.PokeBattle_Move_097();
				case Attack.Data.Effects.x0ED:
					return new Combat.PokeBattle_Move_0BB();
				case Attack.Data.Effects.x0EE:
					return new Combat.PokeBattle_Move_08C();
				case Attack.Data.Effects.x0EF:
					return new Combat.PokeBattle_Move_057();
				case Attack.Data.Effects.x0F0:
					return new Combat.PokeBattle_Move_068();
				case Attack.Data.Effects.x0F1:
					return new Combat.PokeBattle_Move_0A1();
				case Attack.Data.Effects.x0F2:
					return new Combat.PokeBattle_Move_0B0();
				case Attack.Data.Effects.x0F3:
					return new Combat.PokeBattle_Move_0AF();
				case Attack.Data.Effects.x0F4:
					return new Combat.PokeBattle_Move_052();
				case Attack.Data.Effects.x0F5:
					return new Combat.PokeBattle_Move_053();
				case Attack.Data.Effects.x0F6:
					return new Combat.PokeBattle_Move_08F();
				case Attack.Data.Effects.x0F7:
					return new Combat.PokeBattle_Move_125();
				case Attack.Data.Effects.x0F8:
					return new Combat.PokeBattle_Move_064();
				case Attack.Data.Effects.x0F9:
					return new Combat.PokeBattle_Move_116();
				case Attack.Data.Effects.x0FA:
					return new Combat.PokeBattle_Move_104();
				case Attack.Data.Effects.x0FB:
					return new Combat.PokeBattle_Move_054();
				case Attack.Data.Effects.x0FC:
					return new Combat.PokeBattle_Move_0DA();
				case Attack.Data.Effects.x0FD:
					return new Combat.PokeBattle_Move_119();
				case Attack.Data.Effects.x0FE:
					return new Combat.PokeBattle_Move_0FE();
				case Attack.Data.Effects.x0FF:
					return new Combat.PokeBattle_Move_002();
				case Attack.Data.Effects.x100:
					return new Combat.PokeBattle_Move_0CB();
				case Attack.Data.Effects.x101:
					return new Combat.PokeBattle_Move_0CA();
				case Attack.Data.Effects.x102:
					return new Combat.PokeBattle_Move_075();
				case Attack.Data.Effects.x103:
					return new Combat.PokeBattle_Move_049();
				case Attack.Data.Effects.x104:
					return new Combat.PokeBattle_Move_11F();
				case Attack.Data.Effects.x105:
					return new Combat.PokeBattle_Move_00D();
				case Attack.Data.Effects.x106:
					return new Combat.PokeBattle_Move_0D0();
				case Attack.Data.Effects.x107:
					return new Combat.PokeBattle_Move_0FD();
				case Attack.Data.Effects.x108:
					return new Combat.PokeBattle_Move_0CC();
				case Attack.Data.Effects.x10A:
					return new Combat.PokeBattle_Move_04E();
				case Attack.Data.Effects.x10B:
					return new Combat.PokeBattle_Move_105();
				case Attack.Data.Effects.x10C:
					return new Combat.PokeBattle_Move_014();
				case Attack.Data.Effects.x10D:
					return new Combat.PokeBattle_Move_09F();
				case Attack.Data.Effects.x10E:
					return new Combat.PokeBattle_Move_0FC();
				case Attack.Data.Effects.x10F:
					return new Combat.PokeBattle_Move_0E4();
				case Attack.Data.Effects.x110:
					//ToDo: Seed Flare additional effect
					return new Combat.PokeBattle_Move_04F();
				case Attack.Data.Effects.x111:
					return new Combat.PokeBattle_Move_0CD();
					//ToDo: Phantom and shadow force are the same...
					//return new Combat.PokeBattle_Move_14D();
				case Attack.Data.Effects.x112:
					return new Combat.PokeBattle_Move_00B();
				case Attack.Data.Effects.x113:
					return new Combat.PokeBattle_Move_00E();
				case Attack.Data.Effects.x114:
					return new Combat.PokeBattle_Move_009();
				case Attack.Data.Effects.x115:
					return new Combat.PokeBattle_Move_020();
				case Attack.Data.Effects.x116:
					return new Combat.PokeBattle_Move_029();
				case Attack.Data.Effects.x117:
					return new Combat.PokeBattle_Move_0AC();
				case Attack.Data.Effects.x118:
					return new Combat.PokeBattle_Move_059();
				case Attack.Data.Effects.x119:
					return new Combat.PokeBattle_Move_058();
				case Attack.Data.Effects.x11A:
					return new Combat.PokeBattle_Move_124();
				case Attack.Data.Effects.x11B:
					return new Combat.PokeBattle_Move_122();
				case Attack.Data.Effects.x11C:
					return new Combat.PokeBattle_Move_07B();
				case Attack.Data.Effects.x11D:
					return new Combat.PokeBattle_Move_031();
				case Attack.Data.Effects.x11E:
					return new Combat.PokeBattle_Move_11A();
				case Attack.Data.Effects.x11F:
					return new Combat.PokeBattle_Move_0F9();
				case Attack.Data.Effects.x120:
					//ToDo: Smack down has different effect
					return new Combat.PokeBattle_Move_11C();
				case Attack.Data.Effects.x121:
					return new Combat.PokeBattle_Move_0A0();
				case Attack.Data.Effects.x122:
					return new Combat.PokeBattle_Move_074();
				case Attack.Data.Effects.x123:
					return new Combat.PokeBattle_Move_02B();
				case Attack.Data.Effects.x124:
					return new Combat.PokeBattle_Move_09B();
				case Attack.Data.Effects.x125:
					return new Combat.PokeBattle_Move_123();
				case Attack.Data.Effects.x126:
					return new Combat.PokeBattle_Move_099();
				case Attack.Data.Effects.x127:
					return new Combat.PokeBattle_Move_061();
				case Attack.Data.Effects.x128:
					return new Combat.PokeBattle_Move_01F();
				case Attack.Data.Effects.x129:
					//ToDo: Acid-spray additional effects
					return new Combat.PokeBattle_Move_04F();
				case Attack.Data.Effects.x12A:
					return new Combat.PokeBattle_Move_121();
				case Attack.Data.Effects.x12B:
					return new Combat.PokeBattle_Move_063();
				case Attack.Data.Effects.x12C:
					return new Combat.PokeBattle_Move_066();
				case Attack.Data.Effects.x12D:
					return new Combat.PokeBattle_Move_11D();
				case Attack.Data.Effects.x12E:
					return new Combat.PokeBattle_Move_083();
				case Attack.Data.Effects.x12F:
					return new Combat.PokeBattle_Move_092();
				case Attack.Data.Effects.x130:
					return new Combat.PokeBattle_Move_0A9();
				case Attack.Data.Effects.x131:
					return new Combat.PokeBattle_Move_050();
				case Attack.Data.Effects.x132:
					return new Combat.PokeBattle_Move_08E();
				case Attack.Data.Effects.x133:
					return new Combat.PokeBattle_Move_0AB();
				case Attack.Data.Effects.x134:
					return new Combat.PokeBattle_Move_120();
				case Attack.Data.Effects.x135:
					return new Combat.PokeBattle_Move_035();
				case Attack.Data.Effects.x136:
					return new Combat.PokeBattle_Move_0DF();
				case Attack.Data.Effects.x137:
					return new Combat.PokeBattle_Move_07F();
				case Attack.Data.Effects.x138:
					return new Combat.PokeBattle_Move_0CE();
				case Attack.Data.Effects.x139:
					return new Combat.PokeBattle_Move_036();
				case Attack.Data.Effects.x13A:
					return new Combat.PokeBattle_Move_0EC();
				case Attack.Data.Effects.x13B:
					return new Combat.PokeBattle_Move_0F5();
				case Attack.Data.Effects.x13C:
					return new Combat.PokeBattle_Move_11E();
				case Attack.Data.Effects.x13D:
					return new Combat.PokeBattle_Move_028();
				case Attack.Data.Effects.x13E:
					return new Combat.PokeBattle_Move_086();
				case Attack.Data.Effects.x13F:
					return new Combat.PokeBattle_Move_062();
				case Attack.Data.Effects.x140:
					return new Combat.PokeBattle_Move_085();
				case Attack.Data.Effects.x141:
					return new Combat.PokeBattle_Move_0E1();
				case Attack.Data.Effects.x142:
					return new Combat.PokeBattle_Move_039();
				case Attack.Data.Effects.x143:
					return new Combat.PokeBattle_Move_025();
				case Attack.Data.Effects.x144:
					return new Combat.PokeBattle_Move_0F3();
				case Attack.Data.Effects.x145:
					return new Combat.PokeBattle_Move_108();
				case Attack.Data.Effects.x146:
					return new Combat.PokeBattle_Move_107();
				case Attack.Data.Effects.x147:
					return new Combat.PokeBattle_Move_106();
				case Attack.Data.Effects.x148:
					return new Combat.PokeBattle_Move_027();
				case Attack.Data.Effects.x149:
					return new Combat.PokeBattle_Move_038();
				case Attack.Data.Effects.x14A:
					break;
				case Attack.Data.Effects.x14B:
					//ToDo: Glaciate
					return new Combat.PokeBattle_Move_044();
				case Attack.Data.Effects.x14C:
					return new Combat.PokeBattle_Move_0C5();
				case Attack.Data.Effects.x14D:
					return new Combat.PokeBattle_Move_0C6();
				case Attack.Data.Effects.x14E:
					return new Combat.PokeBattle_Move_015();
				case Attack.Data.Effects.x14F:
					return new Combat.PokeBattle_Move_03D();
				case Attack.Data.Effects.x150:
					return new Combat.PokeBattle_Move_07A();
				case Attack.Data.Effects.x151:
					return new Combat.PokeBattle_Move_079();
				case Attack.Data.Effects.x152:
					return new Combat.PokeBattle_Move_144();
				case Attack.Data.Effects.x153:
					return new Combat.PokeBattle_Move_158();
				case Attack.Data.Effects.x154:
					return new Combat.PokeBattle_Move_13E();
				case Attack.Data.Effects.x155:
					return new Combat.PokeBattle_Move_153();
				case Attack.Data.Effects.x156:
					return new Combat.PokeBattle_Move_150();
				case Attack.Data.Effects.x157:
					return new Combat.PokeBattle_Move_142();
				case Attack.Data.Effects.x158:
					return new Combat.PokeBattle_Move_13A();
				case Attack.Data.Effects.x159:
					return new Combat.PokeBattle_Move_146();
				case Attack.Data.Effects.x15A:
					//ToDo: Parabolic charge has different effect
					return new Combat.PokeBattle_Move_0DD();
				case Attack.Data.Effects.x15B:
					return new Combat.PokeBattle_Move_151();
				case Attack.Data.Effects.x15C:
					return new Combat.PokeBattle_Move_141();
				case Attack.Data.Effects.x15D:
					return new Combat.PokeBattle_Move_14F();
				case Attack.Data.Effects.x15E:
					return new Combat.PokeBattle_Move_14A();
				case Attack.Data.Effects.x15F:
					return new Combat.PokeBattle_Move_13F();
				case Attack.Data.Effects.x160:
					return new Combat.PokeBattle_Move_155();
				case Attack.Data.Effects.x161:
					return new Combat.PokeBattle_Move_156();
				case Attack.Data.Effects.x162:
					return new Combat.PokeBattle_Move_145();
				case Attack.Data.Effects.x163:
					return new Combat.PokeBattle_Move_152();
				case Attack.Data.Effects.x164:
					return new Combat.PokeBattle_Move_14B();
				case Attack.Data.Effects.x165:
					return new Combat.PokeBattle_Move_139();
				case Attack.Data.Effects.x166:
					return new Combat.PokeBattle_Move_13C();
				case Attack.Data.Effects.x167:
					return new Combat.PokeBattle_Move_136();
				case Attack.Data.Effects.x168:
					//ToDo: I think the function both do the samething
					//MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13B());
					return new Combat.PokeBattle_Move_147();
				case Attack.Data.Effects.x169:
					//ToDo: Water Shuriken has different effect
					return new Combat.PokeBattle_Move_0C0();
				case Attack.Data.Effects.x16A:
					return new Combat.PokeBattle_Move_14C();
				case Attack.Data.Effects.x16B:
					return new Combat.PokeBattle_Move_138();
				case Attack.Data.Effects.x16C:
					return new Combat.PokeBattle_Move_140();
				case Attack.Data.Effects.x16D:
					return new Combat.PokeBattle_Move_042();
				case Attack.Data.Effects.x16E:
					return new Combat.PokeBattle_Move_14E();
				case Attack.Data.Effects.x16F:
					return new Combat.PokeBattle_Move_137();
				case Attack.Data.Effects.x170:
					return new Combat.PokeBattle_Move_157();
				case Attack.Data.Effects.x171:
					return new Combat.PokeBattle_Move_154();
				case Attack.Data.Effects.x172:
					return new Combat.PokeBattle_Move_134();
				case Attack.Data.Effects.x173:
					break;
				case Attack.Data.Effects.x174:
					break;
				case Attack.Data.Effects.x175:
					//ToDo: Thousand arrows....
					return new Combat.PokeBattle_Move_11C();
				case Attack.Data.Effects.x176:
					//ToDo: Thousand wave has different effects
					return new Combat.PokeBattle_Move_0EF();
				case Attack.Data.Effects.x177:
					break;
				case Attack.Data.Effects.x178:
					return new Combat.PokeBattle_Move_143();
				case Attack.Data.Effects.x179:
					return new Combat.PokeBattle_Move_149();
				case Attack.Data.Effects.x17A:
					return new Combat.PokeBattle_Move_148();
				case Attack.Data.Effects.x17B:
					break;
				case Attack.Data.Effects.x17C:
					return new Combat.PokeBattle_Move_135();
				case Attack.Data.Effects.x17D:
					//Disarming voice, inflices extra effects
					return new Combat.PokeBattle_Move_0A5();
				case Attack.Data.Effects.x17E:
					break;
				case Attack.Data.Effects.x17F:
					break;
				case Attack.Data.Effects.x180:
					break;
				case Attack.Data.Effects.x181:
					break;
				case Attack.Data.Effects.x182:
					break;
				case Attack.Data.Effects.x183:
					break;
				case Attack.Data.Effects.x184:
					break;
				case Attack.Data.Effects.x185:
					break;
				case Attack.Data.Effects.x186:
					break;
				case Attack.Data.Effects.x187:
					break;
				case Attack.Data.Effects.x188:
					break;
				case Attack.Data.Effects.x189:
					break;
				case Attack.Data.Effects.x18A:
					break;
				case Attack.Data.Effects.x18B:
					break;
				case Attack.Data.Effects.x18C:
					break;
				case Attack.Data.Effects.x18D:
					break;
				case Attack.Data.Effects.x18E:
					break;
				case Attack.Data.Effects.x18F:
					break;
				case Attack.Data.Effects.x190:
					break;
				case Attack.Data.Effects.x191:
					break;
				case Attack.Data.Effects.x192:
					break;
				case Attack.Data.Effects.x193:
					break;
				case Attack.Data.Effects.x194:
					break;
				case Attack.Data.Effects.x195:
					break;
				case Attack.Data.Effects.x196:
					break;
				case Attack.Data.Effects.x197:
					break;
				case Attack.Data.Effects.x198:
					break;
				case Attack.Data.Effects.x199:
					break;
				case Attack.Data.Effects.x19A:
					break;
				case Attack.Data.Effects.x19B:
					break;
				case Attack.Data.Effects.x19C:
					break;
				case Attack.Data.Effects.x19D:
					break;
				case Attack.Data.Effects.x19E:
					break;
				case Attack.Data.Effects.x19F:
					break;
				case Attack.Data.Effects.x1A0:
					break;
				case Attack.Data.Effects.x1A1:
					break;
				case Attack.Data.Effects.x1A2:
					break;
				case Attack.Data.Effects.x1A3:
					break;
				case Attack.Data.Effects.x1A4:
					break;
				case Attack.Data.Effects.x711:
					//126	No effect.	Shadow Blast, Shadow Blitz, Shadow Break, Shadow Rave, Shadow Rush, Shadow Wave
					//Effect x711 is only used by Shadow Rush... the others just deal regular damage, with no additional effects 
					return new Combat.PokeBattle_Move_126();
				case Attack.Data.Effects.x712:
					return new Combat.PokeBattle_Move_130();
				case Attack.Data.Effects.x713:
					return new Combat.PokeBattle_Move_12E();
				case Attack.Data.Effects.x714:
					return new Combat.PokeBattle_Move_12C();
				case Attack.Data.Effects.x715:
					return new Combat.PokeBattle_Move_132();
				case Attack.Data.Effects.x716:
					return new Combat.PokeBattle_Move_131();
				default:
					break;
			}
			return new Combat.PokeBattle_UnimplementedMove();
		}
	}
}
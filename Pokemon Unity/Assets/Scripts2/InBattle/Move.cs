using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	/// <summary>
	/// A Move placeholder class to be used while in-battle, 
	/// to prevent temp changes from being permanent to original pokemon profile
	/// </summary>
	public class Move : PokemonUnity.Attack.Move
	{
		#region Variables
		public bool NOTYPE						{ get; set; } //= 0x01
		public bool IGNOREPKMNTYPES				{ get; set; } //= 0x02
		public bool NOWEIGHTING					{ get; set; } //= 0x04
		public bool NOCRITICAL					{ get; set; } //= 0x08
		public bool NOREFLECT					{ get; set; } //= 0x10
		public bool SELFCONFUSE					{ get; set; } //= 0x20

		public Attack.Category Category			{ get; set; }
		new public Moves MoveId					{ get; set; }
		new public Attack.Target Targets		{ get; set; }
		new public Types Type					{ get; set; }
		new public Flags Flag					{ get; set; }
		new public byte PP						{ get; set; }
		new public int TotalPP					{ get; set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0.
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur. 
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
		/// </summary>
		public int AddlEffect					{ get; set; }
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int Accuracy						{ get; set; }
		public int BaseDamage					{ get; set; }
		public int CritRatio					{ get; set; }
		public int Priority						{ get; set; }
		public bool IsPhysical					{ get { return Category == Attack.Category.PHYSICAL; } }
		public bool IsSpecial					{ get { return Category == Attack.Category.SPECIAL; } }
		public bool UnuseableInGravity			{ get; set; }
		public bool pbIsStatus(){ return false; }
		public string EffectString				{ get; set; }
		public string Nothing = "But nothing happened!";
		#endregion

		private Battle Battle { get { return Game.battle; } }

		//NOTYPE          = 0x01,
		//IGNOREPKMNTYPES = 0x02,
		//NOWEIGHTING     = 0x04,
		//NOCRITICAL      = 0x08,
		//NOREFLECT       = 0x10,
		//SELFCONFUSE     = 0x20
		/// <summary>
		/// </summary>
		/// Needed another place to put this, might remove from code later
		public enum SpecialCondition
		{
			NOTYPE			= 0x01,
			IGNOREPKMNTYPES	= 0x02,
			NOWEIGHTING		= 0x04,
			NOCRITICAL		= 0x08,
			NOREFLECT		= 0x10,
			SELFCONFUSE		= 0x20
		}

		public Move(Moves move) : base(move)
		{
			//battle	= battle
			BaseDamage	= _base.BaseDamage;
			Type		= _base.Type;
			Accuracy	= _base.Accuracy;
			PP			= base.PP;
			TotalPP		= base.TotalPP;
			AddlEffect	= _base.Effects;
			Targets		= base.Targets;
			Priority	= _base.Priority;
			Flag		= base.Flag;
			//thismove	= move
			//name		= ""
			MoveId		= base.MoveId;
		}

		public Move(Move move) : this(move.MoveId)
		{
			PP		= move.PP;
			TotalPP = move.TotalPP;
			CalcMoveFunc();
		}

		/*public Move(Battle battle, Move move) : base(move.MoveId)
		{
		}

		public static implicit operator Move(Battle.Move input)
		{

		}

		public void GetBattle(Battle battle)
		{

		}*/
		public void CalcMoveFunc()//(ref Battle.Move move)
		{
			//Effect function;
			switch ((Effect)Function)
			{
				case Effect.Confusion:
					//battle		= battle
					BaseDamage = 40;
					Type = Types.NONE;
					Accuracy = 100;
					PP = 0; //-1;
					//TotalPP	= ;
					AddlEffect = 0;
					Targets = Attack.Target.NoTarget;
					Priority = 0;
					Flag = new Flags();
					//thismove		= move
					//name			= ""
					MoveId = Moves.NONE;
					Category = Attack.Category.PHYSICAL;
					break;
				case Effect.Struggle:
				case Effect.x000:
					break;
				case Effect.x001:
					UnuseableInGravity = true;
					break;
				case Effect.x002:
				case Effect.x003:
				case Effect.x004:
				case Effect.x005:
				case Effect.x006:
				case Effect.x007:
				case Effect.x008:
				case Effect.x009:
				case Effect.x00A:
				case Effect.x00B:
				case Effect.x00C:
				case Effect.x00D:
				case Effect.x00E:
				case Effect.x00F:
				case Effect.x010:
				case Effect.x011:
				case Effect.x012:
				case Effect.x013:
				case Effect.x014:
				case Effect.x015:
				case Effect.x016:
				case Effect.x017:
				case Effect.x018:
				case Effect.x019:
				case Effect.x01A:
				case Effect.x01B:
				case Effect.x01C:
				case Effect.x01D:
				case Effect.x01E:
				case Effect.x01F:
				case Effect.x020:
				case Effect.x021:
				case Effect.x022:
				case Effect.x023:
				case Effect.x024:
				case Effect.x025:
				case Effect.x026:
				case Effect.x027:
				case Effect.x028:
				case Effect.x029:
				case Effect.x02A:
				case Effect.x02B:
				case Effect.x02C:
				case Effect.x02D:
				case Effect.x02E:
				case Effect.x02F:
				case Effect.x030:
				case Effect.x031:
				case Effect.x032:
				case Effect.x033:
				case Effect.x034:
				case Effect.x035:
				case Effect.x036:
				case Effect.x037:
				case Effect.x038:
				case Effect.x039:
				case Effect.x03A:
				case Effect.x03B:
				case Effect.x03C:
				case Effect.x03D:
				case Effect.x03E:
				case Effect.x03F:
				case Effect.x040:
				case Effect.x041:
				case Effect.x042:
				case Effect.x043:
				case Effect.x044:
				case Effect.x045:
				case Effect.x046:
				case Effect.x047:
				case Effect.x048:
				case Effect.x049:
				case Effect.x04A:
				case Effect.x04B:
				case Effect.x04C:
				case Effect.x04D:
				case Effect.x04E:
				case Effect.x04F:
				case Effect.x050:
				case Effect.x051:
				case Effect.x052:
				case Effect.x053:
				case Effect.x054:
				case Effect.x055:
				case Effect.x056:
				case Effect.x057:
				case Effect.x058:
				case Effect.x059:
				case Effect.x05A:
				case Effect.x05B:
				case Effect.x05C:
				case Effect.x05D:
				case Effect.x05E:
				case Effect.x05F:
				case Effect.x060:
				case Effect.x061:
				case Effect.x062:
				case Effect.x063:
				case Effect.x064:
				case Effect.x065:
				case Effect.x066:
				case Effect.x067:
				case Effect.x068:
				case Effect.x069:
				case Effect.x06A:
				case Effect.x06B:
				case Effect.x06C:
				case Effect.x06D:
				case Effect.x06E:
				case Effect.x06F:
				case Effect.x070:
				case Effect.x071:
				case Effect.x072:
				case Effect.x073:
				case Effect.x074:
				case Effect.x075:
				case Effect.x076:
				case Effect.x077:
				case Effect.x078:
				case Effect.x079:
				case Effect.x07A:
				case Effect.x07B:
				case Effect.x07C:
				case Effect.x07D:
				case Effect.x07E:
				case Effect.x07F:
				case Effect.x080:
				case Effect.x081:
				case Effect.x082:
				case Effect.x083:
				case Effect.x084:
				case Effect.x085:
				case Effect.x086:
				case Effect.x087:
				case Effect.x088:
				case Effect.x089:
				case Effect.x08A:
				case Effect.x08B:
				case Effect.x08C:
				case Effect.x08D:
				case Effect.x08E:
				case Effect.x08F:
				case Effect.x090:
				case Effect.x091:
				case Effect.x092:
				case Effect.x093:
				case Effect.x094:
				case Effect.x095:
				case Effect.x096:
				case Effect.x097:
				case Effect.x098:
				case Effect.x099:
				case Effect.x09A:
				case Effect.x09B:
				case Effect.x09C:
				case Effect.x09D:
				case Effect.x09E:
				case Effect.x09F:
				case Effect.x0A0:
				case Effect.x0A1:
				case Effect.x0A2:
				case Effect.x0A3:
				case Effect.x0A4:
				case Effect.x0A5:
				case Effect.x0A6:
				case Effect.x0A7:
				case Effect.x0A8:
				case Effect.x0A9:
				case Effect.x0AA:
				case Effect.x0AB:
				case Effect.x0AC:
				case Effect.x0AD:
				case Effect.x0AE:
				case Effect.x0AF:
				case Effect.x0B0:
				case Effect.x0B1:
				case Effect.x0B2:
				case Effect.x0B3:
				case Effect.x0B4:
				case Effect.x0B5:
				case Effect.x0B6:
				case Effect.x0B7:
				case Effect.x0B8:
				case Effect.x0B9:
				case Effect.x0BA:
				case Effect.x0BB:
				case Effect.x0BC:
				case Effect.x0BD:
				case Effect.x0BE:
				case Effect.x0BF:
				case Effect.x0C0:
				case Effect.x0C1:
				case Effect.x0C2:
				case Effect.x0C3:
				case Effect.x0C4:
				case Effect.x0C5:
				case Effect.x0C6:
				case Effect.x0C7:
				case Effect.x0C8:
				case Effect.x0C9:
				case Effect.x0CA:
				case Effect.x0CB:
				case Effect.x0CC:
				case Effect.x0CD:
				case Effect.x0CE:
				case Effect.x0CF:
				case Effect.x0D0:
				case Effect.x0D1:
				case Effect.x0D2:
				case Effect.x0D3:
				case Effect.x0D4:
				case Effect.x0D5:
				case Effect.x0D6:
				case Effect.x0D7:
				case Effect.x0D8:
				case Effect.x0D9:
				case Effect.x0DA:
				case Effect.x0DB:
				case Effect.x0DC:
				case Effect.x0DD:
				case Effect.x0DE:
				case Effect.x0DF:
				case Effect.x0E0:
				case Effect.x0E1:
				case Effect.x0E2:
				case Effect.x0E3:
				case Effect.x0E4:
				case Effect.x0E5:
				case Effect.x0E6:
				case Effect.x0E7:
				case Effect.x0E8:
				case Effect.x0E9:
				case Effect.x0EA:
				case Effect.x0EB:
				case Effect.x0EC:
				case Effect.x0ED:
				case Effect.x0EE:
				case Effect.x0EF:
				case Effect.x0F0:
				case Effect.x0F1:
				case Effect.x0F2:
				case Effect.x0F3:
				case Effect.x0F4:
				case Effect.x0F5:
				case Effect.x0F6:
				case Effect.x0F7:
				case Effect.x0F8:
				case Effect.x0F9:
				case Effect.x0FA:
				case Effect.x0FB:
				case Effect.x0FC:
				case Effect.x0FD:
				case Effect.x0FE:
				case Effect.x0FF:
				case Effect.x100:
				case Effect.x101:
				case Effect.x102:
				case Effect.x103:
				case Effect.x104:
				case Effect.x105:
				case Effect.x106:
				case Effect.x107:
				case Effect.x108:
				case Effect.x109:
				case Effect.x10A:
				case Effect.x10B:
				case Effect.x10C:
				case Effect.x10D:
				case Effect.x10E:
				case Effect.x10F:
				case Effect.x110:
				case Effect.x111:
				case Effect.x112:
				case Effect.x113:
				case Effect.x114:
				case Effect.x115:
				case Effect.x116:
				case Effect.x117:
				case Effect.x118:
				case Effect.x119:
				case Effect.x11A:
				case Effect.x11B:
				case Effect.x11C:
				case Effect.x11D:
				case Effect.x11E:
				case Effect.x11F:
				case Effect.x120:
				case Effect.x121:
				case Effect.x122:
				case Effect.x123:
				case Effect.x124:
				case Effect.x125:
				case Effect.x133:
				case Effect.x134:
				case Effect.x135:
				case Effect.x136:
				case Effect.x137:
				case Effect.x138:
				case Effect.x139:
				case Effect.x13A:
				case Effect.x13B:
				case Effect.x13C:
				case Effect.x13D:
				case Effect.x13E:
				case Effect.x13F:
				case Effect.x140:
				case Effect.x141:
				case Effect.x142:
				case Effect.x143:
				case Effect.x144:
				case Effect.x145:
				case Effect.x146:
				case Effect.x147:
				case Effect.x148:
				case Effect.x149:
				case Effect.x14A:
				case Effect.x14B:
				case Effect.x14C:
				case Effect.x14D:
				case Effect.x14E:
				case Effect.x14F:
				case Effect.x150:
				case Effect.x151:
				case Effect.x152:
				case Effect.x153:
				case Effect.x154:
				case Effect.x155:
				case Effect.x156:
				case Effect.x157:
				case Effect.x158:
					break;
				case Effect.FailedMove:
					//by default, both should result in failure
					//Game.battle.pbDisplay("But it failed!");
					break;
				case Effect.UnimplementedMove:
				default:
					break;
			}
			//return move;
		}
	}

	#region Move Interfaces
	public interface IMoveEffect
	{
		int Effect(Pokemon attacker, Pokemon opponent, int hitnum, Attack.Target alltargets, bool showanimation);
	}
	public interface IMoveAdditionalEffect
	{
		void AdditionalEffect(Pokemon attacker, Pokemon opponent);
	}
	public interface IMoveModifyAccuracy
	{
		void ModifyAccuracy(int moveAccuracy, Pokemon attacker, Pokemon opponent);
	}
	#endregion
}
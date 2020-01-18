using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack
{
	/*public partial class Move
	{
		public class Flags
		{
			/// <summary>
			/// The move makes physical contact with the target
			/// </summary>
			public bool Contact;
			/// <summary>
			/// The target can use <see cref="Moves.Protect"/> or <see cref="Moves.Detect"/> to protect itself from the move
			/// </summary>
			public bool Protectable;
			/// <summary>
			/// The target can use <see cref="Moves.Magic_Coat"/> to redirect the effect of the move. 
			/// Use this flag if the move deals no damage but causes a negative effect on the target.
			/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
			/// </summary>
			public bool Reflectable;
			/// <summary>
			/// The target can use <see cref="Moves.Snatch"/> to steal the effect of the move. 
			/// Use this flag for most moves that target the user.
			/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
			/// </summary>
			public bool Snatch;
			/// <summary>
			/// The move can be copied by <see cref="Moves.Mirror_Move"/>.
			/// </summary>
			public bool Mirror;
			/// <summary>
			/// The move has a 10% chance of making the opponent flinch if the user is holding a 
			/// <see cref="PokemonUnity.Item.Items.KINGS_ROCK"/>/<see cref="PokemonUnity.Item.Items.RAZOR_FANG"/>. 
			/// Use this flag for all damaging moves that don't already have a flinching effect.
			/// </summary>
			public bool Flinch;
			/// <summary>
			/// If the user is <see cref="Status.Frozen"/>, the move will thaw it out before it is used.
			/// </summary>
			/// Thaw
			public bool Defrost;
			/// <summary>
			/// The move has a high critical hit rate.
			/// </summary>
			public bool Crit;
			/// <summary>
			/// The move is a biting move (powered up by the ability Strong Jaw).
			/// </summary>
			public bool Bite;
			/// <summary>
			/// The move is a punching move (powered up by the ability Iron Fist).
			/// </summary>
			public bool Punching;
			/// <summary>
			/// The move is a sound-based move.
			/// </summary>
			public bool SoundBased;
			/// <summary>
			/// The move is a powder-based move (Grass-type Pokémon are immune to them).
			/// </summary>
			public bool PowderBased;
			/// <summary>
			/// The move is a pulse-based move (powered up by the ability Mega Launcher).
			/// </summary>
			public bool PulseBased;
			/// <summary>
			/// The move is a bomb-based move (resisted by the ability Bulletproof).
			/// </summary>
			public bool BombBased;
			public Flags(bool authentic = false, bool bite = false, bool bullet = false, bool charge = false, bool contact = false, bool crit = false, bool dance = false, bool defrost = false,
						bool distance = false, bool flinch = false, bool gravity = false, bool heal = false, bool mirror = false, bool mystery = false, bool nonsky = false, bool powder = false,
						bool protect = false, bool pulse = false, bool punch = false, bool recharge = false, bool reflectable = false, bool snatch = false, bool sound = false)
			{
				//this.Authentic = authentic;
				this.Bite = bite;
				this.BombBased = bullet;
				//this.Charge = charge;
				this.Contact = contact;
				this.Crit = crit;
				//this.Dance = dance;
				this.Defrost = defrost;
				//this.Distance = distance;
				this.Flinch = flinch;
				//this.Gravity = gravity;
				//this.Heal = heal;
				this.Mirror = mirror;
				//this.Mystery = mystery;
				//this.Nonsky = nonsky;
				this.PowderBased = powder;
				this.Protectable = protect;
				this.PulseBased = pulse;
				this.Punching = punch;
				//this.Recharge = recharge;
				this.Reflectable = reflectable;
				this.Snatch = snatch;
				this.SoundBased = sound;
			}
		}
	}*/
}
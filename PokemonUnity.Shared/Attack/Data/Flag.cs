using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack.Data
{
	public struct Flag
	{
		/// <summary>
		/// User touches the target.  This triggers some abilities (e.g., <see cref="Abilities.STATIC"/>) and items (e.g., <see cref="Items.STICKY_BARB"/>).
		/// The move makes physical contact with the target
		/// </summary>
		public bool Contact;
		/// <summary>
		/// The target can use <see cref="Moves.PROTECT"/> or <see cref="Moves.DETECT"/> to protect itself from the move
		/// </summary>
		public bool Protectable;
		/// <summary>
		/// This move may be reflected back at the user with <see cref="Moves.MAGIC_COAT"/> or <see cref="Abilities.MAGIC_BOUNCE"/>.
		/// The target can use <see cref="Moves.MAGIC_COAT"/> to redirect the effect of the move. 
		/// Use this flag if the move deals no damage but causes a negative effect on the target.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		public bool Reflectable;
		/// <summary>
		/// This move will be stolen if another Pokémon has used <see cref="Moves.SNATCH"/> this turn.
		/// The target can use <see cref="Moves.SNATCH"/> to steal the effect of the move. 
		/// Use this flag for most moves that target the user.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		public bool Snatch;
		/// <summary>
		/// The move can be copied by <see cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		public bool Mirror;
		/// <summary>
		/// This move can be used while frozen to force the Pokémon to defrost.
		/// If the user is <see cref="Status.Frozen"/>, the move will thaw it out before it is used.
		/// </summary>
		/// Thaw
		public bool Defrost;
		/// <summary>
		/// This move has a charging turn that can be skipped with a <see cref="Items.POWER_HERB"/>.
		/// </summary>
		public bool Charge;
		/// <summary>
		/// This move triggers <see cref="Abilities.DANCER"/>.
		/// </summary>
		private bool Dance;
		/// <summary>
		/// This move has 1.5× its usual power when used by a Pokémon with <see cref="Abilities.STRONG_JAW"/>.
		/// The move is a biting move (powered up by the ability Strong Jaw).
		/// </summary>
		public bool Bite;
		/// <summary>
		/// This move has 1.2× its usual power when used by a Pokémon with <see cref="Abilities.IRON_FIST"/>.
		/// The move is a punching move (powered up by the ability Iron Fist).
		/// </summary>
		public bool Punching;
		/// <summary>
		/// Pokémon with <see cref="Abilities.SOUNDPROOF"/> are immune to this move.
		/// The move is a sound-based move.
		/// </summary>
		public bool SoundBased;
		/// <summary>
		/// Pokémon with <see cref="Abilities.OVERCOAT"/> and <see cref="Types.GRASS"/>-type Pokémon are immune to this move.
		/// The move is a powder-based move (Grass-type Pokémon are immune to them).
		/// </summary>
		public bool PowderBased;
		/// <summary>
		/// This move has 1.5× its usual power when used by a Pokémon with <see cref="Abilities.MEGA_LAUNCHER"/>.
		/// The move is a pulse-based move (powered up by the ability Mega Launcher).
		/// </summary>
		public bool PulseBased;
		/// <summary>
		/// This move ignores the target's <see cref="Moves.SUBSTITUTE"/>.
		/// </summary>
		private bool Authentic;
		/// <summary>
		/// This move is blocked by <see cref="Abilities.BULLETPROOF"/>.
		/// The move is a bomb-based move (resisted by the ability Bulletproof).
		/// </summary>
		public bool Ballistics;
		/// <summary>
		/// In triple battles, this move can be used on either side to target the farthest away opposing Pokémon.
		/// </summary>
		public bool Distance;
		/// <summary>
		/// This move cannot be used in high <see cref="Moves.GRAVITY"/>.
		/// </summary>
		public bool Gravity;
		/// <summary>
		/// This move is blocked by <see cref="Moves.HEAL_BLOCK"/>.
		/// </summary>
		public bool Heal;
		/// <summary>
		/// This move is unusable during Sky Battles.
		/// </summary>
		public bool NonSky;
		/// <summary>
		/// The turn after this move is used, the Pokémon's action is skipped so it can recharge.
		/// </summary>
		public bool Recharge;
		/// <summary>
		/// This move is blocked by <see cref="Abilities.AROMA_VEIL"/> and cured by <see cref="Items.MENTAL_HERB"/>.
		/// </summary>
		public bool Mental;
		public Flag(bool authentic = false, bool bite = false, bool bullet = false, bool charge = false, bool contact = false, bool crit = false, bool dance = false, bool defrost = false,
					bool distance = false, bool flinch = false, bool gravity = false, bool heal = false, bool mirror = false, bool mental = false, bool nonsky = false, bool powder = false,
					bool protect = false, bool pulse = false, bool punch = false, bool recharge = false, bool reflectable = false, bool snatch = false, bool sound = false)
		{
			this.Authentic = authentic; //14
			this.Ballistics = bullet;
			this.Bite = bite;
			this.Charge = charge; //2
			this.Contact = contact;
			//this.Crit = crit;
			this.Dance = dance; //21
			this.Defrost = defrost;
			this.Distance = distance; //12
			//this.Flinch = flinch;
			this.Gravity = gravity; //10
			this.Heal = heal; //13
			this.Mirror = mirror;
			this.Mental = mental; 
			this.NonSky = nonsky; //20
			this.PowderBased = powder;
			this.Protectable = protect;
			this.PulseBased = pulse;
			this.Punching = punch;
			this.Recharge = recharge; //3
			this.Reflectable = reflectable;
			this.Snatch = snatch;
			this.SoundBased = sound;
		}
	}
}
namespace PokemonUnity.Attack
{
	/// <summary>
	/// Is one of the following:
	/// Physical, Special, or Status
	/// </summary>
	public enum Category
	{
		PHYSICAL= 2,
		SPECIAL	= 3,
		STATUS	= 1
	};
	public enum MetaCategory
	{
		/// <summary>
		/// Inflicts damage
		/// </summary>
		DAMAGE				= 0,
		/// <summary>
		/// No damage; inflicts status ailment
		/// </summary>
		AILMENT				= 1,
		/// <summary>
		/// No damage; lowers target's stats or raises user's stats
		/// </summary>
		NET_GOOD_STATS		= 2,
		/// <summary>
		/// No damage; heals the user
		/// </summary>
		HEAL				= 3,
		/// <summary>
		/// Inflicts damage; inflicts status ailment
		/// </summary>
		DAMAGE_AILMENT		= 4,
		/// <summary>
		/// No damage; inflicts status ailment; raises target's stats
		/// </summary>
		SWAGGER				= 5,
		/// <summary>
		/// Inflicts damage; lowers target's stats
		/// </summary>
		DAMAGE_LOWER		= 6,
		/// <summary>
		/// Inflicts damage; raises user's stats
		/// </summary>
		DAMAGE_RAISE		= 7,
		/// <summary>
		/// Inflicts damage; absorbs damage done to heal the user
		/// </summary>
		DAMAGE_HEAL			= 8,
		/// <summary>
		/// One-hit KO
		/// </summary>
		OHKO				= 9,
		/// <summary>
		/// Effect on the whole field
		/// </summary>
		WHOLE_FIELD_EFFECT	= 10,
		/// <summary>
		/// Effect on one side of the field
		/// </summary>
		FIELD_EFFECT		= 11,
		/// <summary>
		/// Forces target to switch out
		/// </summary>
		FORCE_SWITCH		= 12,
		/// <summary>
		/// Unique effect
		/// </summary>
		UNIQUE				= 13
	}
	public enum MoveMetaAilments
	{
		UNKNOWN				= -1,
		NONE				= 0,
		PARALYSIS			= 1,
		SLEEP				= 2,
		FREEZE				= 3,
		BURN				= 4,
		POISON				= 5,
		CONFUSION			= 6,
		INFATUATION			= 7,
		TRAP				= 8,
		NIGHTMARE			= 9,
		TORMENT				= 12,
		DISABLE				= 13,
		YAWN				= 14,
		HEAL_BLOCK			= 15,
		NO_TYPE_IMMUNITY	= 17,
		LEECH_SEED			= 18,
		EMBARGO				= 19,
		PERISH_SONG			= 20,
		INGRAIN				= 21,
		SILENCE				= 24,
	}
}
namespace PokemonUnity.Combat
{
	public enum Stats
	{
		ATTACK	= 1
		,DEFENSE= 2
		,SPEED	= 3
		,SPATK	= 4
		,SPDEF	= 5
		,HP		= 0
		/// <summary>
		/// battle only stat
		/// </summary>
		,ACCURACY	= 6
		/// <summary>
		/// battle only stat
		/// </summary>
		,EVASION	= 7
	}
	public enum TypeEffective
	{
		//Broken,
		Ineffective,
		NotVeryEffective,
		NormalEffective,
		SuperEffective
	}
	#region Battle
	public enum ChoiceAction
	{
		/// <summary>
		/// IsFainted
		/// </summary>
		NoAction = 0,
		UseMove = 1,
		SwitchPokemon = 2,
		UseItem = 3,
		CallPokemon = 4,
		Run = 5
	}

	public enum BattleResults : int
	{
		InProgress = -1,
		/// <summary>
		/// 0 - Undecided or aborted
		/// </summary>
		ABORTED = 0,
		/// <summary>
		/// 1 - Player won
		/// </summary>
		WON = 1,
		/// <summary>
		/// 2 - Player lost
		/// </summary>
		LOST = 2,
		/// <summary>
		/// 3 - Player or wild Pokémon ran from battle, or player forfeited the match
		/// </summary>
		FORFEIT = 3,
		/// <summary>
		/// 4 - Wild Pokémon was caught
		/// </summary>
		CAPTURED = 4,
		/// <summary>
		/// 5 - Draw
		/// </summary>
		DRAW = 5
	}

    public enum MenuCommands : int
    {
        CANCEL  = -1,
        FIGHT   = 0,
        BAG     = 1,
        POKEMON = 2,
        RUN     = 3,
        CALL    = 4
    }
	#endregion
	public enum Weather
	{
		NONE,
		RAINDANCE,
		HEAVYRAIN,
		SUNNYDAY,
		HARSHSUN,
		SANDSTORM,
		STRONGWINDS,
		HAIL,
		SHADOWSKY
	}
}
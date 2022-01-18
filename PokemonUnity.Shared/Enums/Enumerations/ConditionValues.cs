namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// default: swarm-no, time-day, radar-off, slot-none, radio-off
	/// </remarks>
	/// ToDo: Change from Enum to class with Bool values
	public class ConditionValues : Enumeration
	{
		public int ConditionId { get; private set; }
		public ConditionValues(int id, string name) : base(id, name) { }
		public static readonly ConditionValues NONE			= new ConditionValues(0,	"NONE");

		/// <summary>
		/// During a swarm
		/// <para>
		/// <seealso cref="Condition.SWARM"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SWARM_YES			= new ConditionValues(1,	"SWARM_YES");
		/// <summary>
		/// Not during a swarm
		/// <para>
		/// <seealso cref="Condition.SWARM"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SWARM_NO			= new ConditionValues(2,	"SWARM_NO");

		/// <summary>
		/// In the morning
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues TIME_MORNING			= new ConditionValues(3,	"TIME_MORNING");
		/// <summary>
		/// During the day
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues TIME_DAY			= new ConditionValues(4,	"TIME_DAY");
		/// <summary>
		/// At night
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues TIME_NIGHT			= new ConditionValues(5,	"TIME_NIGHT");

		/// <summary>
		/// Using PokeRadar
		/// <para>
		/// <seealso cref="Condition.RADAR"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues RADAR_ON			= new ConditionValues(6,	"RADAR_ON");
		/// <summary>
		/// Not using PokeRadar
		/// <para>
		/// <seealso cref="Condition.RADAR"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues RADAR_OFF			= new ConditionValues(7,	"RADAR_OFF");

		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SLOT_NONE			= new ConditionValues(8,	"SLOT_NONE");
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SLOT_RUBY			= new ConditionValues(9,	"SLOT_RUBY");
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SLOT_SAPPHIRE			= new ConditionValues(10,	"SLOT_SAPPHIRE");
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SLOT_EMERALD			= new ConditionValues(11,	"SLOT_EMERALD");
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SLOT_FIRERED			= new ConditionValues(12,	"SLOT_FIRERED");
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SLOT_LEAFGREEN			= new ConditionValues(13,	"SLOT_LEAFGREEN");

		/// <summary>
		/// Radio off
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues RADIO_OFF			= new ConditionValues(14,	"RADIO_OFF");
		/// <summary>
		/// Hoenn radio
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues RADIO_HOENN			= new ConditionValues(15,	"RADIO_HOENN");
		/// <summary>
		/// Sinnoh radio
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues RADIO_SINNOH			= new ConditionValues(16,	"RADIO_SINNOH");

		/// <summary>
		/// During Spring
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SEASON_SPRING			= new ConditionValues(17,	"SEASON_SPRING");
		/// <summary>
		/// During Summer
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SEASON_SUMMER			= new ConditionValues(18,	"SEASON_SUMMER");
		/// <summary>
		/// During Autumn
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SEASON_AUTUMN			= new ConditionValues(19,	"SEASON_AUTUMN");
		/// <summary>
		/// During Winter
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		public static readonly ConditionValues SEASON_WINTER			= new ConditionValues(20,	"SEASON_WINTER");
	}
}
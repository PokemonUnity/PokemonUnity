using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Inventory.Plants;

namespace PokemonUnity.Inventory
{
	/*public partial class Item
	{
		public bool IsBerry
		{
			get
			{
				if (ItemPocket.HasValue && ItemPocket.Value == ItemPockets.BERRY)
				{
					return true; //new Berry(this);
				}
				else
					return false; //null;
			}
		}
		public bool IsApricon
		{
			get
			{
				switch (ItemId)
				{
					case Items.BLACK_APRICORN:
					case Items.BLUE_APRICORN:
					case Items.GREEN_APRICORN:
					case Items.PINK_APRICORN:
					case Items.RED_APRICORN:
					case Items.WHITE_APRICORN:
					case Items.YELLOW_APRICORN:
						return true;
					default:
						return false;
				}
			}
		}

		public class Berry : Item//: MedicineItem
		{
			#region Variables
			public int PhaseTime			{ get; private set; }
			public bool IsFruit				{ get; private set; }

			/// <summary>
			/// In inches...
			/// </summary>
			public float Size				{ get; private set; }
			public FirmnessLevel Firmness	{ get; private set; }
			public int Smooth				{ get; private set; }
			/// <summary>
			/// Berry Id
			/// </summary>
			public int BerryIndex			{ get; private set; }
			public int minBerries			{ get; private set; }
			/// <summary>
			/// The berry yield is set to the maximum berry yield whenever a new Berry is planted 
			/// or whenever a new plant grows where the old plant used to be.
			/// </summary>
			public int maxBerries			{ get; private set; }

			/// <summary>
			/// The time it takes to advance from one growth stage to the next is:
			/// <para></para>
			/// <list type="">
			/// <item>If planted with Growth Mulch, 45 minutes times the growth rate.</item>
			/// <para>
			/// <item>If planted with Damp Mulch, 90 minutes times the growth rate.</item>
			/// </para>
			/// <item>Otherwise, 60 minutes times the growth rate.</item>
			/// </list>
			/// </summary>
			/// Pokemon Gen4 (2nd gen with berries), plants take days to mature...
			/// <seealso cref="growthTime"/>
			public int growthRate			{ get; private set; }
			/// <summary>
			/// The time it takes for berry plant to fully mature, and reach final stage.
			/// int duration is measured in hours
			/// </summary>
			/// Gen 5 onward reduced plant growth time from days to hours...
			public int growthTime			{ get; private set; }
			/// <summary>
			/// The soil moisture is reduced by X times the moisture intake value, rounded down, 
			/// but not to less than 0, where X is 0.5 if planted with Damp Mulch, 
			/// 1.5 if planted with Growth Mulch, and 1 otherwise.
			/// </summary>
			/// <remarks>
			/// Every 60 minutes after that point or after a new plant grows where the old plant used to be 
			/// (whichever occurs last), if the plant hasn't reached its final growth stage 
			/// (no berries can be picked yet)
			/// </remarks>
			/// <example>
			/// If the soil moisture is 0, the berry yield for that plant is reduced 
			/// by the maximum berry yield divided by 5, but not to less than 2.
			/// </example>
			public int moistIntake			{ get; private set; }
			public float watering			{ get; private set; }
			public float weeding			{ get; private set; }
			public float pests				{ get; private set; }

			public int Spicy				{ get; private set; }
			public int Dry					{ get; private set; }
			public int Sweet				{ get; private set; }
			public int Bitter				{ get; private set; }
			public int Sour					{ get; private set; }

			public int Power				{ get; private set; }
			public Types Type				{ get; private set; }

			//This isnt actually a thing...
			//Berry growth arnt seasonal in official game
			public int WinterGrow = 0;
			public int SpringGrow = 3;
			public int SummerGrow = 2;
			public int FallGrow = 1;

			//public override int PokeDollarPrice { get { return 100; } }
			//public override int FlingDamage { get { return 10;} } 
			//public override ItemTypes ItemType { get { return ItemTypes.Plants; } } 
			//public override int SortValue { get; private set; }
			//public override string Description { get; private set; }

			public Flavours Flavour
			{
				get
				{
					Flavours returnFlavour = Flavours.Spicy;
					int highestFlavour = Spicy;

					if (Dry > highestFlavour)
					{
						highestFlavour = Dry;
						returnFlavour = Flavours.Dry;
					}
					if (Sweet > highestFlavour)
					{
						highestFlavour = Sweet;
						returnFlavour = Flavours.Sweet;
					}
					if (Bitter > highestFlavour)
					{
						highestFlavour = Bitter;
						returnFlavour = Flavours.Bitter;
					}
					if (Sour > highestFlavour)
					{
						highestFlavour = Sour;
						returnFlavour = Flavours.Sour;
					}

					return returnFlavour;
				}
			}
			#endregion

			#region Constructors
			public Berry(Item berry) : this(berry.ItemId)
			{
			}
			
			private Berry(Items berry) : base(berry)
			{
				Spicy = 0;
				Dry = 0;
				Sweet = 0;
				Bitter = 0;
				Sour = 0;

				//WinterGrow = 0;
				//SpringGrow = 3;
				//SummerGrow = 2;
				//FallGrow = 1;

				Power = 60;
				this.Type = Types.NONE;

				IsFruit = true;
				//gen 4 is loaded by default
				switch (berry)
				{
					//(\d+)\s+(\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)
					//BerryIndex = $1;\tmaxBerries = $3;\tgrowthRate = $4;\tmoistIntake = $5;\tSpicy = $6;\tDry = $7;\tSweet = $8;\tBitter = $9;\tSour = $10;\tSmooth = $11;\tPower = 60;\tthis.Type = Types.NONE;\tbreak;\t//$2
					case Items.AGUAV_BERRY:
BerryIndex = 14;	maxBerries = 5;		growthRate = 5;	moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 15;	Sour = 0;	Smooth = 25;	break;	//Aguav
BerryIndex = 57;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 10;	Dry = 30;	Sweet = 0;	Bitter = 0;	Sour = 30;	Smooth = 40;	break;	//Apicot
BerryIndex = 5;		maxBerries = 5;		growthRate = 3;	moistIntake = 15;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 10;	Smooth = 25;	break;	//Aspear
BerryIndex = 51;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 25;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 35;	break;	//Babiri
BerryIndex = 35;	maxBerries = 15;	growthRate = 15;	moistIntake = 8;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 30;	Smooth = 35;	break;	//Belue
BerryIndex = 17;	maxBerries = 10;	growthRate = 2;	moistIntake = 35;	Spicy = 0;	Dry = 10;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 20;	break;	//Bluk
BerryIndex = 47;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 10;	Dry = 20;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 35;	break;	//Charti
BerryIndex = 1;		maxBerries = 5;		growthRate = 3;	moistIntake = 15;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 25;	break;	//Cheri
BerryIndex = 2;		maxBerries = 5;		growthRate = 3;	moistIntake = 15;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 25;	break;	//Chesto
BerryIndex = 52;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 25;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 35;	break;	//Chilan
BerryIndex = 41;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 15;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 0;	Smooth = 30;	break;	//Chople
BerryIndex = 44;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 15;	Sour = 0;	Smooth = 30;	break;	//Coba
BerryIndex = 50;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 20;	Smooth = 35;	break;	//Colbur
BerryIndex = 27;	maxBerries = 10;	growthRate = 6;	moistIntake = 10;	Spicy = 0;	Dry = 20;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 30;	break;	//Cornn
BerryIndex = 62;	maxBerries = 5;		growthRate = 24;	moistIntake = 7;	Spicy = 0;	Dry = 0;	Sweet = 40;	Bitter = 10;	Sour = 0;	Smooth = 60;	break;	//Custap
BerryIndex = 34;	maxBerries = 15;	growthRate = 15;	moistIntake = 8;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 30;	Sour = 10;	Smooth = 35;	break;	//Durin
BerryIndex = 60;	maxBerries = 5;		growthRate = 24;	moistIntake = 7;	Spicy = 40;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 60;	break;	//Enigma
BerryIndex = 11;	maxBerries = 5;		growthRate = 5;	moistIntake = 10;	Spicy = 15;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 25;	break;	//Figy
BerryIndex = 54;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 0;	Dry = 30;	Sweet = 10;	Bitter = 30;	Sour = 0;	Smooth = 40;	break;	//Ganlon
BerryIndex = 25;	maxBerries = 5;		growthRate = 8;	moistIntake = 8;	Spicy = 0;	Dry = 10;	Sweet = 10;	Bitter = 0;	Sour = 10;	Smooth = 20;	break;	//Grepa
BerryIndex = 49;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 20;	Sour = 0;	Smooth = 35;	break;	//Haban
BerryIndex = 24;	maxBerries = 5;		growthRate = 8;	moistIntake = 8;	Spicy = 10;	Dry = 10;	Sweet = 0;	Bitter = 10;	Sour = 0;	Smooth = 20;	break;	//Hondew
BerryIndex = 15;	maxBerries = 5;		growthRate = 5;	moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 15;	Smooth = 25;	break;	//Iapapa
BerryIndex = 63;	maxBerries = 5;		growthRate = 24;	moistIntake = 7;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 40;	Sour = 10;	Smooth = 60;	break;	//Jaboca
BerryIndex = 48;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 10;	Sweet = 20;	Bitter = 0;	Sour = 0;	Smooth = 35;	break;	//Kasib
BerryIndex = 42;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 15;	Sweet = 0;	Bitter = 0;	Sour = 10;	Smooth = 30;	break;	//Kebia
BerryIndex = 22;	maxBerries = 5;		growthRate = 8;	moistIntake = 8;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 10;	Sour = 10;	Smooth = 20;	break;	//Kelpsy
BerryIndex = 58;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 10;	Sweet = 30;	Bitter = 10;	Sour = 30;	Smooth = 50;	break;	//Lansat
BerryIndex = 6;		maxBerries = 5;		growthRate = 4;	moistIntake = 15;	Spicy = 10;	Dry = 0;	Sweet = 10;	Bitter = 10;	Sour = 10;	Smooth = 20;	break;	//Leppa
BerryIndex = 53;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 10;	Sweet = 30;	Bitter = 0;	Sour = 0;	Smooth = 40;	break;	//Liechi
BerryIndex = 9;		maxBerries = 5;		growthRate = 12;	moistIntake = 8;	Spicy = 10;	Dry = 10;	Sweet = 10;	Bitter = 10;	Sour = 0;	Smooth = 20;	break;	//Lum
BerryIndex = 13;	maxBerries = 5;		growthRate = 5;	moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 15;	Bitter = 0;	Sour = 0;	Smooth = 25;	break;	//Mago
BerryIndex = 28;	maxBerries = 10;	growthRate = 6;	moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 20;	Bitter = 10;	Sour = 0;	Smooth = 30;	break;	//Magost
BerryIndex = 61;	maxBerries = 5;		growthRate = 24;	moistIntake = 7;	Spicy = 0;	Dry = 40;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 60;	break;	//Micle
BerryIndex = 18;	maxBerries = 10;	growthRate = 2;	moistIntake = 35;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 10;	Sour = 0;	Smooth = 20;	break;	//Nanab
BerryIndex = 30;	maxBerries = 10;	growthRate = 6;	moistIntake = 10;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 20;	Smooth = 30;	break;	//Nomel
BerryIndex = 36;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 15;	Dry = 0;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 30;	break;	//Occa
BerryIndex = 7;		maxBerries = 5;		growthRate = 4;	moistIntake = 15;	Spicy = 10;	Dry = 10;	Sweet = 0;	Bitter = 10;	Sour = 10;	Smooth = 20;	break;	//Oran
BerryIndex = 32;	maxBerries = 15;	growthRate = 15;	moistIntake = 8;	Spicy = 0;	Dry = 30;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 35;	break;	//Pamtre
BerryIndex = 37;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 15;	Sweet = 0;	Bitter = 10;	Sour = 0;	Smooth = 30;	break;	//Passho
BerryIndex = 45;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 0;	Sour = 15;	Smooth = 30;	break;	//Payapa
BerryIndex = 3;		maxBerries = 5;		growthRate = 3;	moistIntake = 15;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 0;	Sour = 0;	Smooth = 25;	break;	//Pecha
BerryIndex = 8;		maxBerries = 5;		growthRate = 4;	moistIntake = 15;	Spicy = 10;	Dry = 10;	Sweet = 10;	Bitter = 0;	Sour = 10;	Smooth = 20;	break;	//Persim
BerryIndex = 56;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 0;	Sweet = 0;	Bitter = 30;	Sour = 10;	Smooth = 40;	break;	//Petaya
BerryIndex = 20;	maxBerries = 10;	growthRate = 2;	moistIntake = 35;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 10;	Smooth = 20;	break;	//Pinap
BerryIndex = 21;	maxBerries = 5;		growthRate = 8;	moistIntake = 8;	Spicy = 10;	Dry = 0;	Sweet = 10;	Bitter = 10;	Sour = 0;	Smooth = 20;	break;	//Pomeg
BerryIndex = 23;	maxBerries = 5;		growthRate = 8;	moistIntake = 8;	Spicy = 10;	Dry = 0;	Sweet = 10;	Bitter = 0;	Sour = 10;	Smooth = 20;	break;	//Qualot
BerryIndex = 29;	maxBerries = 10;	growthRate = 6;	moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 20;	Sour = 10;	Smooth = 30;	break;	//Rabuta
BerryIndex = 4;		maxBerries = 5;		growthRate = 3;	moistIntake = 15;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 0;	Smooth = 25;	break;	//Rawst
BerryIndex = 16;	maxBerries = 10;	growthRate = 2;	moistIntake = 35;	Spicy = 10;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 20;	break;	//Razz
BerryIndex = 39;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 15;	Sour = 0;	Smooth = 30;	break;	//Rindo
BerryIndex = 64;	maxBerries = 5;		growthRate = 24;	moistIntake = 7;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 40;	Smooth = 60;	break;	//Rowap
BerryIndex = 55;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 0;	Dry = 0;	Sweet = 30;	Bitter = 10;	Sour = 30;	Smooth = 40;	break;	//Salac
BerryIndex = 43;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 10;	Dry = 0;	Sweet = 15;	Bitter = 0;	Sour = 0;	Smooth = 30;	break;	//Shuca
BerryIndex = 10;	maxBerries = 5;		growthRate = 8;	moistIntake = 7;	Spicy = 0;	Dry = 10;	Sweet = 10;	Bitter = 10;	Sour = 10;	Smooth = 20;	break;	//Sitrus
BerryIndex = 31;	maxBerries = 15;	growthRate = 15;	moistIntake = 8;	Spicy = 30;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 35;	break;	//Spelon
BerryIndex = 59;	maxBerries = 5;		growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 10;	Sweet = 30;	Bitter = 10;	Sour = 30;	Smooth = 50;	break;	//Starf
BerryIndex = 26;	maxBerries = 5;		growthRate = 8;	moistIntake = 8;	Spicy = 20;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 30;	break;	//Tamato
BerryIndex = 46;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 20;	Dry = 0;	Sweet = 0;	Bitter = 0;	Sour = 10;	Smooth = 35;	break;	//Tanga
BerryIndex = 38;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 15;	Bitter = 0;	Sour = 10;	Smooth = 30;	break;	//Wacan
BerryIndex = 33;	maxBerries = 15;	growthRate = 15;	moistIntake = 8;	Spicy = 0;	Dry = 0;	Sweet = 30;	Bitter = 10;	Sour = 0;	Smooth = 35;	break;	//Watmel
BerryIndex = 19;	maxBerries = 10;	growthRate = 2;	moistIntake = 35;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 10;	Smooth = 20;	break;	//Wepear
BerryIndex = 12;	maxBerries = 5;		growthRate = 5;	moistIntake = 10;	Spicy = 0;	Dry = 15;	Sweet = 0;	Bitter = 0;	Sour = 0;	Smooth = 25;	break;	//Wiki
BerryIndex = 40;	maxBerries = 5;		growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 0;	Sour = 15;	Smooth = 30;	break;	//Yache
					case Items.APICOT_BERRY:
					case Items.ASPEAR_BERRY:
					case Items.BABIRI_BERRY:
					case Items.BELUE_BERRY:
					case Items.BLUK_BERRY:
					case Items.CHARTI_BERRY:
					case Items.CHERI_BERRY:
					case Items.CHESTO_BERRY:
					case Items.CHILAN_BERRY:
					case Items.CHOPLE_BERRY:
					case Items.COBA_BERRY:
					case Items.COLBUR_BERRY:
					case Items.CORNN_BERRY:
					case Items.CUSTAP_BERRY:
					case Items.DURIN_BERRY:
					case Items.ENIGMA_BERRY:
					case Items.FIGY_BERRY:
					case Items.GANLON_BERRY:
					case Items.GREPA_BERRY:
					case Items.HABAN_BERRY:
					case Items.HONDEW_BERRY:
					case Items.IAPAPA_BERRY:
					case Items.JABOCA_BERRY:
					case Items.KASIB_BERRY:
					case Items.KEBIA_BERRY:
					case Items.KEE_BERRY:
					case Items.KELPSY_BERRY:
					case Items.LANSAT_BERRY:
					case Items.LEPPA_BERRY:
					case Items.LIECHI_BERRY:
					case Items.LUM_BERRY:
					case Items.MAGOST_BERRY:
					case Items.MAGO_BERRY:
					case Items.MARANGA_BERRY:
					case Items.MICLE_BERRY:
					case Items.NANAB_BERRY:
					case Items.NOMEL_BERRY:
					case Items.OCCA_BERRY:
					case Items.ORAN_BERRY:
					case Items.PAMTRE_BERRY:
					case Items.PASSHO_BERRY:
					case Items.PAYAPA_BERRY:
					case Items.PECHA_BERRY:
					case Items.PERSIM_BERRY:
					case Items.PETAYA_BERRY:
					case Items.PINAP_BERRY:
					case Items.POMEG_BERRY:
					case Items.QUALOT_BERRY:
					case Items.RABUTA_BERRY:
					case Items.RAWST_BERRY:
					case Items.RAZZ_BERRY:
					case Items.RINDO_BERRY:
					case Items.ROSELI_BERRY:
					case Items.ROWAP_BERRY:
					case Items.SALAC_BERRY:
					case Items.SHUCA_BERRY:
					case Items.SITRUS_BERRY:
					case Items.SPELON_BERRY:
					case Items.STARF_BERRY:
					case Items.TAMATO_BERRY:
					case Items.TANGA_BERRY:
					case Items.WACAN_BERRY:
					case Items.WATMEL_BERRY:
					case Items.WEPEAR_BERRY:
					case Items.WIKI_BERRY:
					case Items.YACHE_BERRY:
					#region Apricons
					case Items.BLACK_APRICORN:
					case Items.BLUE_APRICORN:
					case Items.GREEN_APRICORN:
					case Items.PINK_APRICORN:
					case Items.RED_APRICORN:
					case Items.WHITE_APRICORN:
					case Items.YELLOW_APRICORN:
					#endregion
						break;
					default:
						IsFruit = false;
						break;
				}
			}

			public Berry(Items berry, int PhaseTime, float Size, FirmnessLevel Firmness = 0, int minBerries = 0, int maxBerries = 0) : this(berry)
			{
				//SortValue = ID - 1999;

				this.PhaseTime = PhaseTime;
				this.Size = Size;
				this.Firmness = Firmness;
				//BerryIndex = this.ID - 2000;
				this.minBerries = minBerries;
				this.maxBerries = maxBerries;

				int x = BerryIndex * 128;
				int y = 0;
				while (x >= 512)
				{
					x -= 512;
					y += 32;
				}

				//Description = Description;
				//_textureSource = @"Textures\Berries";
				//_textureRectangle = new Vector4(x, y, 32, 32);
			}
			#endregion			

			#region Methods
			/// <summary>
			/// Returns if a Pokémon likes this berry based on its flavour.
			/// </summary>
			/// <param name="p">The Pokémon to test this berry for.</param>
			public bool PokemonLikes(Monster.Pokemon p)
			{
				switch (p.Nature)
				{
					case Monster.Natures.LONELY:
						{
							if (Flavour == Flavours.Spicy)
								return true;
							else if (Flavour == Flavours.Sour)
								return false;
							break;
						}
					case Monster.Natures.ADAMANT:
						{
							if (Flavour == Flavours.Spicy)
								return true;
							else if (Flavour == Flavours.Dry)
								return false;
							break;
						}
					case Monster.Natures.NAUGHTY:
						{
							if (Flavour == Flavours.Spicy)
								return true;
							else if (Flavour == Flavours.Bitter)
								return false;
							break;
						}
					case Monster.Natures.BRAVE:
						{
							if (Flavour == Flavours.Spicy)
								return true;
							else if (Flavour == Flavours.Sweet)
								return false;
							break;
						}
					case Monster.Natures.BOLD:
						{
							if (Flavour == Flavours.Sour)
								return true;
							else if (Flavour == Flavours.Spicy)
								return false;
							break;
						}
					case Monster.Natures.IMPISH:
						{
							if (Flavour == Flavours.Sour)
								return true;
							else if (Flavour == Flavours.Dry)
								return false;
							break;
						}
					case Monster.Natures.LAX:
						{
							if (Flavour == Flavours.Sour)
								return true;
							else if (Flavour == Flavours.Bitter)
								return false;
							break;
						}
					case Monster.Natures.RELAXED:
						{
							if (Flavour == Flavours.Sour)
								return true;
							else if (Flavour == Flavours.Sweet)
								return false;
							break;
						}
					case Monster.Natures.MODEST:
						{
							if (Flavour == Flavours.Dry)
								return true;
							else if (Flavour == Flavours.Spicy)
								return false;
							break;
						}
					case Monster.Natures.MILD:
						{
							if (Flavour == Flavours.Dry)
								return true;
							else if (Flavour == Flavours.Sour)
								return false;
							break;
						}
					case Monster.Natures.RASH:
						{
							if (Flavour == Flavours.Dry)
								return true;
							else if (Flavour == Flavours.Bitter)
								return false;
							break;
						}
					case Monster.Natures.QUIET:
						{
							if (Flavour == Flavours.Dry)
								return true;
							else if (Flavour == Flavours.Sweet)
								return false;
							break;
						}
					case Monster.Natures.CALM:
						{
							if (Flavour == Flavours.Bitter)
								return true;
							else if (Flavour == Flavours.Spicy)
								return false;
							break;
						}
					case Monster.Natures.GENTLE:
						{
							if (Flavour == Flavours.Bitter)
								return true;
							else if (Flavour == Flavours.Sour)
								return false;
							break;
						}
					case Monster.Natures.CAREFUL:
						{
							if (Flavour == Flavours.Bitter)
								return true;
							else if (Flavour == Flavours.Dry)
								return false;
							break;
						}
					case Monster.Natures.SASSY:
						{
							if (Flavour == Flavours.Bitter)
								return true;
							else if (Flavour == Flavours.Sweet)
								return false;
							break;
						}
					case Monster.Natures.TIMID:
						{
							if (Flavour == Flavours.Sweet)
								return true;
							else if (Flavour == Flavours.Spicy)
								return false;
							break;
						}
					case Monster.Natures.HASTY:
						{
							if (Flavour == Flavours.Sweet)
								return true;
							else if (Flavour == Flavours.Sour)
								return false;
							break;
						}
					case Monster.Natures.JOLLY:
						{
							if (Flavour == Flavours.Sweet)
								return true;
							else if (Flavour == Flavours.Dry)
								return false;
							break;
						}
					case Monster.Natures.NAIVE:
						{
							if (Flavour == Flavours.Sweet)
								return true;
							else if (Flavour == Flavours.Bitter)
								return false;
							break;
						}
					default:
						{
							return true;
						}
				}
				return true;
			}

			public Berry ToGenIV()
			{
				return this;
			}
			public Berry ToGenV()
			{
				return this;
			}
			public Berry ToGenVI()
			{
				return this;
			}
			#endregion		
		}
	}*/
}
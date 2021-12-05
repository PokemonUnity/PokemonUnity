using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Inventory.Plants;

namespace PokemonUnity.Inventory
{
	public struct BerryData 
	{
		#region Variables
		public Items Id					{ get; private set; }

		/// <summary>
		/// In inches...
		/// </summary>
		public float Size				{ get; private set; }
		public FirmnessLevel Firmness	{ get; private set; }
		public int Smooth				{ get; private set; }
		//public int minBerries			{ get; private set; }
		/// <summary>
		/// The berry yield is set to the maximum berry yield whenever a new Berry is planted 
		/// or whenever a new plant grows where the old plant used to be.
		/// </summary>
		public int maxBerries			{ get; private set; }

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
		/*public int PhaseTime			{ get; private set; }
		public float watering			{ get; private set; }
		public float weeding			{ get; private set; }
		public float pests				{ get; private set; }*/
		public PokemonUnity.Color Color	{ get; private set; }

		public int Power				{ get; private set; }
		public Types Type				{ get; private set; }

		public int Spicy				{ get; private set; }
		public int Dry					{ get; private set; }
		public int Sweet				{ get; private set; }
		public int Bitter				{ get; private set; }
		public int Sour					{ get; private set; }

		public int Cool				{ get; private set; }
		public int Beauty			{ get; private set; }
		public int Cute				{ get; private set; }
		public int Smart			{ get; private set; }
		public int Tough			{ get; private set; }

		//This isnt actually a thing...
		//Berry growth arnt seasonal in official game
		//public int WinterGrow = 0;
		//public int SpringGrow = 3;
		//public int SummerGrow = 2;
		//public int FallGrow = 1;

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
		public BerryData(Items berry, FirmnessLevel firmness, int power = 0, Types type = Types.NONE, float size = 0, int max = 0, int growth = 0, int soil = 0, int smooth = 0, int cool = 0, int beauty = 0, int cute = 0, int smart = 0, int tough = 0, int spicy = 0, int dry = 0, int sweet = 0, int bitter = 0, int sour = 0, bool @default = true) 
		{
			Id = berry;

			//this.PhaseTime = phaseTime;
			this.Size = size;
			this.Smooth = smooth;
			this.Firmness = firmness;
			//this.minBerries = minBerries;
			this.maxBerries = max;

			growthRate = 0;
			growthTime = growth;
			moistIntake = soil;
			Color = PokemonUnity.Color.NONE;

			Spicy = spicy;
			Dry = dry;
			Sweet = sweet;
			Bitter = bitter;
			Sour = sour;

			Cool = cool;
			Beauty = beauty;
			Cute = cute;
			Smart = smart;
			Tough = tough;

			Power = power;
			this.Type = type;

			//Description = Description;
			//_textureSource = @"Textures\Berries";
			//_textureRectangle = new Vector4(x, y, 32, 32);

			if(@default)
			switch (berry)
			{
				//(\d+)\s+(\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)
				//BerryIndex = $1;\tmaxBerries = $3;\tgrowthRate = $4;\tmoistIntake = $5;\tSpicy = $6;\tDry = $7;\tSweet = $8;\tBitter = $9;\tSour = $10;\tSmooth = $11;\tPower = 60;\tthis.Type = Types.NONE;\tColor = Color.	break;\t//$2
				case Items.AGUAV_BERRY:
					growthRate = 5;		moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 15;	Sour = 0;	Color = Color.GREEN;	break;	//Aguav
				case Items.APICOT_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 10;	Dry = 30;	Sweet = 0;	Bitter = 0;		Sour = 30;	Color = Color.BLUE;	break;	//Apicot
				case Items.ASPEAR_BERRY:
					growthRate = 3;		moistIntake = 15;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 10;	Color = Color.YELLOW;	break;	//Aspear
				case Items.BABIRI_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 25;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.GREEN;	break;	//Babiri
				case Items.BELUE_BERRY:
					growthRate = 15;	moistIntake = 8;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 30;	Color = Color.PURPLE;	break;	//Belue
				case Items.BLUK_BERRY:
					growthRate = 2;		moistIntake = 35;	Spicy = 0;	Dry = 10;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Bluk
				case Items.CHARTI_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 10;	Dry = 20;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.YELLOW;	break;	//Charti
				case Items.CHERI_BERRY:
					growthRate = 3;		moistIntake = 15;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Cheri
				case Items.CHESTO_BERRY:
					growthRate = 3;		moistIntake = 15;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Chesto
				case Items.CHILAN_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 25;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.YELLOW;	break;	//Chilan
				case Items.CHOPLE_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 15;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 0;	Color = Color.RED;	break;	//Chople
				case Items.COBA_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 15;	Sour = 0;	Color = Color.BLUE;	break;	//Coba
				case Items.COLBUR_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 20;	Color = Color.PURPLE;	break;	//Colbur
				case Items.CORNN_BERRY:
					growthRate = 6;		moistIntake = 10;	Spicy = 0;	Dry = 20;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Cornn
				case Items.CUSTAP_BERRY:
					growthRate = 24;	moistIntake = 7;	Spicy = 0;	Dry = 0;	Sweet = 40;	Bitter = 10;	Sour = 0;	Color = Color.RED;	break;	//Custap
				case Items.DURIN_BERRY:
					growthRate = 15;	moistIntake = 8;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 30;	Sour = 10;	Color = Color.GREEN;	break;	//Durin
				case Items.ENIGMA_BERRY:
					growthRate = 24;	moistIntake = 7;	Spicy = 40;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Enigma
				case Items.FIGY_BERRY:
					growthRate = 5;		moistIntake = 10;	Spicy = 15;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Figy
				case Items.GANLON_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 0;	Dry = 30;	Sweet = 10;	Bitter = 30;	Sour = 0;	Color = Color.PURPLE;	break;	//Ganlon
				case Items.GREPA_BERRY:
					growthRate = 8;		moistIntake = 8;	Spicy = 0;	Dry = 10;	Sweet = 10;	Bitter = 0;		Sour = 10;	Color = Color.YELLOW;	break;	//Grepa
				case Items.HABAN_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 20;	Sour = 0;	Color = Color.RED;	break;	//Haban
				case Items.HONDEW_BERRY:
					growthRate = 8;		moistIntake = 8;	Spicy = 10;	Dry = 10;	Sweet = 0;	Bitter = 10;	Sour = 0;	Color = Color.GREEN;	break;	//Hondew
				case Items.IAPAPA_BERRY:
					growthRate = 5;		moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 15;	Color = Color.YELLOW;	break;	//Iapapa
				case Items.JABOCA_BERRY:
					growthRate = 24;	moistIntake = 7;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 40;	Sour = 10;	Color = Color.YELLOW;	break;	//Jaboca
				case Items.KASIB_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 10;	Sweet = 20;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Kasib
				case Items.KEBIA_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 15;	Sweet = 0;	Bitter = 0;		Sour = 10;	Color = Color.GREEN;	break;	//Kebia
				case Items.KEE_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 30;	Sweet = 10;	Bitter = 10;	Sour = 10;	Color = Color.YELLOW;	break;	//Copied: Liechi
				case Items.KELPSY_BERRY:
					growthRate = 8;		moistIntake = 8;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 10;	Sour = 10;	Color = Color.BLUE;	break;	//Kelpsy
				case Items.LANSAT_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 10;	Sweet = 30;	Bitter = 10;	Sour = 30;	Color = Color.RED;	break;	//Lansat
				case Items.LEPPA_BERRY:
					growthRate = 4;		moistIntake = 15;	Spicy = 10;	Dry = 0;	Sweet = 10;	Bitter = 10;	Sour = 10;	Color = Color.RED;	break;	//Leppa
				case Items.LIECHI_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 10;	Sweet = 30;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Liechi
				case Items.LUM_BERRY:
					growthRate = 12;	moistIntake = 8;	Spicy = 10;	Dry = 10;	Sweet = 10;	Bitter = 10;	Sour = 0;	Color = Color.GREEN;	break;	//Lum
				case Items.MAGO_BERRY:
					growthRate = 5;		moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 15;	Bitter = 0;		Sour = 0;	Color = Color.PINK;	break;	//Mago
				case Items.MAGOST_BERRY:
					growthRate = 6;		moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 20;	Bitter = 10;	Sour = 0;	Color = Color.PINK;	break;	//Magost
				case Items.MARANGA_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 10;	Dry = 10;	Sweet = 30;	Bitter = 30;	Sour = 10;	Color = Color.BLUE;	break;	//Copied: Liechi
				case Items.MICLE_BERRY:
					growthRate = 24;	moistIntake = 7;	Spicy = 0;	Dry = 40;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.GREEN;	break;	//Micle
				case Items.NANAB_BERRY:
					growthRate = 2;		moistIntake = 35;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 10;	Sour = 0;	Color = Color.PINK;	break;	//Nanab
				case Items.NOMEL_BERRY:
					growthRate = 6;		moistIntake = 10;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 20;	Color = Color.YELLOW;	break;	//Nomel
				case Items.OCCA_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 15;	Dry = 0;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Occa
				case Items.ORAN_BERRY:
					growthRate = 4;		moistIntake = 15;	Spicy = 10;	Dry = 10;	Sweet = 0;	Bitter = 10;	Sour = 10;	Color = Color.BLUE;	break;	//Oran
				case Items.PAMTRE_BERRY:
					growthRate = 15;	moistIntake = 8;	Spicy = 0;	Dry = 30;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Pamtre
				case Items.PASSHO_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 15;	Sweet = 0;	Bitter = 10;	Sour = 0;	Color = Color.BLUE;	break;	//Passho
				case Items.PAYAPA_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 0;		Sour = 15;	Color = Color.PURPLE;	break;	//Payapa
				case Items.PECHA_BERRY:
					growthRate = 3;		moistIntake = 15;	Spicy = 0;	Dry = 0;	Sweet = 10;	Bitter = 0;		Sour = 0;	Color = Color.PINK;	break;	//Pecha
				case Items.PERSIM_BERRY:
					growthRate = 4;		moistIntake = 15;	Spicy = 10;	Dry = 10;	Sweet = 10;	Bitter = 0;		Sour = 10;	Color = Color.PINK;	break;	//Persim
				case Items.PETAYA_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 0;	Sweet = 0;	Bitter = 30;	Sour = 10;	Color = Color.PINK;	break;	//Petaya
				case Items.PINAP_BERRY:
					growthRate = 2;		moistIntake = 35;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 10;	Color = Color.YELLOW;	break;	//Pinap
				case Items.POMEG_BERRY:
					growthRate = 8;		moistIntake = 8;	Spicy = 10;	Dry = 0;	Sweet = 10;	Bitter = 10;	Sour = 0;	Color = Color.RED;	break;	//Pomeg
				case Items.QUALOT_BERRY:
					growthRate = 8;		moistIntake = 8;	Spicy = 10;	Dry = 0;	Sweet = 10;	Bitter = 0;		Sour = 10;	Color = Color.YELLOW;	break;	//Qualot
				case Items.RABUTA_BERRY:
					growthRate = 6;		moistIntake = 10;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 20;	Sour = 10;	Color = Color.GREEN;	break;	//Rabuta
				case Items.RAWST_BERRY:
					growthRate = 3;		moistIntake = 15;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 0;	Color = Color.GREEN;	break;	//Rawst
				case Items.RAZZ_BERRY:
					growthRate = 2;		moistIntake = 35;	Spicy = 10;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Razz
				case Items.RINDO_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 15;	Sour = 0;	Color = Color.GREEN;	break;	//Rindo
				case Items.ROSELI_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 25;	Bitter = 10;	Sour = 0;	Color = Color.PINK;	break;	//Copied: Occa
				case Items.ROWAP_BERRY:
					growthRate = 24;	moistIntake = 7;	Spicy = 10;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 40;	Color = Color.BLUE;	break;	//Rowap
				case Items.SALAC_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 0;	Dry = 0;	Sweet = 30;	Bitter = 10;	Sour = 30;	Color = Color.GREEN;	break;	//Salac
				case Items.SHUCA_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 10;	Dry = 0;	Sweet = 15;	Bitter = 0;		Sour = 0;	Color = Color.YELLOW;	break;	//Shuca
				case Items.SITRUS_BERRY:
					growthRate = 8;		moistIntake = 7;	Spicy = 0;	Dry = 10;	Sweet = 10;	Bitter = 10;	Sour = 10;	Color = Color.YELLOW;	break;	//Sitrus
				case Items.SPELON_BERRY:
					growthRate = 15;	moistIntake = 8;	Spicy = 30;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Spelon
				case Items.STARF_BERRY:
					growthRate = 24;	moistIntake = 4;	Spicy = 30;	Dry = 10;	Sweet = 30;	Bitter = 10;	Sour = 30;	Color = Color.GREEN;	break;	//Starf
				case Items.TAMATO_BERRY:
					growthRate = 8;		moistIntake = 8;	Spicy = 20;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.RED;	break;	//Tamato
				case Items.TANGA_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 20;	Dry = 0;	Sweet = 0;	Bitter = 0;		Sour = 10;	Color = Color.GREEN;	break;	//Tanga
				case Items.WACAN_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 0;	Sweet = 15;	Bitter = 0;		Sour = 10;	Color = Color.YELLOW;	break;	//Wacan
				case Items.WATMEL_BERRY:
					growthRate = 15;	moistIntake = 8;	Spicy = 0;	Dry = 0;	Sweet = 30;	Bitter = 10;	Sour = 0;	Color = Color.PINK;	break;	//Watmel
				case Items.WEPEAR_BERRY:
					growthRate = 2;		moistIntake = 35;	Spicy = 0;	Dry = 0;	Sweet = 0;	Bitter = 10;	Sour = 10;	Color = Color.GREEN;	break;	//Wepear
				case Items.WIKI_BERRY:
					growthRate = 5;		moistIntake = 10;	Spicy = 0;	Dry = 15;	Sweet = 0;	Bitter = 0;		Sour = 0;	Color = Color.PURPLE;	break;	//Wiki
				case Items.YACHE_BERRY:
					growthRate = 18;	moistIntake = 6;	Spicy = 0;	Dry = 10;	Sweet = 0;	Bitter = 0;		Sour = 15;	Color = Color.BLUE;	break;	//Yache
				default:
					break;
			}
		}
		#endregion			

		#region Methods
		/// <summary>
		/// Returns if a Pokémon likes this berry based on its flavour.
		/// </summary>
		/// <param name="p">The Pokémon to test this berry for.</param>
		public bool PokemonLikes(PokemonEssentials.Interface.PokeBattle.IPokemon p)
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

		public bool ConfusionBerry()
		{
			return	Id == Items.AGUAV_BERRY		|| 
					Id == Items.FIGY_BERRY		||
					Id == Items.IAPAPA_BERRY	||
					Id == Items.MAGO_BERRY		||
					Id == Items.WIKI_BERRY;
		}
		#endregion		
	}
}
namespace PokemonUnity.Item
{
	public class Berry : Item//: MedicineItem
	{
		#region Variables
		public int PhaseTime;

		public string Size		{ get; private set; }
		public string Firmness	{ get; private set; }
		public int BerryIndex	{ get; private set; }
		public int minBerries	{ get; private set; }
		public int maxBerries	{ get; private set; }

		public int Spicy	{ get; private set; }
		public int Dry		{ get; private set; }
		public int Sweet	{ get; private set; }
		public int Bitter	{ get; private set; }
		public int Sour		{ get; private set; }

		public int WinterGrow = 0;
		public int SpringGrow = 3;
		public int SummerGrow = 2;
		public int FallGrow = 1;

		public Types Type;
		public int Power = 60;

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

		private Berry(Items berry) : base(berry)
		{
			Spicy = 0;
			Dry = 0;
			Sweet = 0;
			Bitter = 0;
			Sour = 0;
		}

		public Berry(Items berry, int PhaseTime, string Size, string Firmness, int minBerries, int maxBerries) : this(berry)
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
			//_textureRectangle = new Rectangle(x, y, 32, 32);
		}

		/// <summary>
		/// Returns if a Pokémon likes this berry based on its flavour.
		/// </summary>
		/// <param name="p">The Pokémon to test this berry for.</param>
		public bool PokemonLikes(Pokemon.Pokemon p)
		{
			switch (p.Nature)
			{
				case Pokemon.Natures.LONELY:
                    {
						if (Flavour == Flavours.Spicy)
							return true;
						else if (Flavour == Flavours.Sour)
							return false;
						break;
					}
				case Pokemon.Natures.ADAMANT:
                    {
						if (Flavour == Flavours.Spicy)
							return true;
						else if (Flavour == Flavours.Dry)
							return false;
						break;
					}
				case Pokemon.Natures.NAUGHTY:
                    {
						if (Flavour == Flavours.Spicy)
							return true;
						else if (Flavour == Flavours.Bitter)
							return false;
						break;
					}
				case Pokemon.Natures.BRAVE:
                    {
						if (Flavour == Flavours.Spicy)
							return true;
						else if (Flavour == Flavours.Sweet)
							return false;
						break;
					}

                    {
						if (Flavour == Flavours.Sour)
							return true;
						else if (Flavour == Flavours.Spicy)
							return false;
						break;
					}
				case Pokemon.Natures.IMPISH:
                    {
						if (Flavour == Flavours.Sour)
							return true;
						else if (Flavour == Flavours.Dry)
							return false;
						break;
					}
				case Pokemon.Natures.LAX:
                    {
						if (Flavour == Flavours.Sour)
							return true;
						else if (Flavour == Flavours.Bitter)
							return false;
						break;
					}
				case Pokemon.Natures.RELAXED:
                    {
						if (Flavour == Flavours.Sour)
							return true;
						else if (Flavour == Flavours.Sweet)
							return false;
						break;
					}
				case Pokemon.Natures.MODEST:
                    {
						if (Flavour == Flavours.Dry)
							return true;
						else if (Flavour == Flavours.Spicy)
							return false;
						break;
					}
				case Pokemon.Natures.MILD:
                    {
						if (Flavour == Flavours.Dry)
							return true;
						else if (Flavour == Flavours.Sour)
							return false;
						break;
					}
				case Pokemon.Natures.RASH:
                    {
						if (Flavour == Flavours.Dry)
							return true;
						else if (Flavour == Flavours.Bitter)
							return false;
						break;
					}
				case Pokemon.Natures.QUIET:
                    {
						if (Flavour == Flavours.Dry)
							return true;
						else if (Flavour == Flavours.Sweet)
							return false;
						break;
					}
				case Pokemon.Natures.CALM:
                    {
						if (Flavour == Flavours.Bitter)
							return true;
						else if (Flavour == Flavours.Spicy)
							return false;
						break;
					}
				case Pokemon.Natures.GENTLE:
                    {
						if (Flavour == Flavours.Bitter)
							return true;
						else if (Flavour == Flavours.Sour)
							return false;
						break;
					}
				case Pokemon.Natures.CAREFUL:
                    {
						if (Flavour == Flavours.Bitter)
							return true;
						else if (Flavour == Flavours.Dry)
							return false;
						break;
					}
				case Pokemon.Natures.SASSY:
                    {
						if (Flavour == Flavours.Bitter)
							return true;
						else if (Flavour == Flavours.Sweet)
							return false;
						break;
					}
				case Pokemon.Natures.TIMID:
                    {
						if (Flavour == Flavours.Sweet)
							return true;
						else if (Flavour == Flavours.Spicy)
							return false;
						break;
					}
				case Pokemon.Natures.HASTY:
                    {
						if (Flavour == Flavours.Sweet)
							return true;
						else if (Flavour == Flavours.Sour)
							return false;
						break;
					}
				case Pokemon.Natures.JOLLY:
                    {
						if (Flavour == Flavours.Sweet)
							return true;
						else if (Flavour == Flavours.Dry)
							return false;
						break;
					}
				case Pokemon.Natures.NAIVE:
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
		
		#region Enum
		public enum Flavours
		{
			Spicy,
			Dry,
			Sweet,
			Bitter,
			Sour
		}
		public enum FirmnessLevel
		{
			SuperHard
		}
		#endregion
	}
}
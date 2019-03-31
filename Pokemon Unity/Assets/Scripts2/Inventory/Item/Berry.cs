namespace PokemonUnity.Inventory
{
	public partial class Item
	{
		//public bool IsBerry { get; private set; }
		public Berry IsBerry {
			get
			{
				if (ItemPocket.HasValue && ItemPocket.Value == ItemPockets.BERRY)
				{
					return new Berry(this);
				}
				else
					return null;
			}
		}
		private Berry berry { get; set; }

		public class Berry : Item//: MedicineItem
		{
			#region Variables
			public int PhaseTime;
			public Items ID					{ get; private set; }
			public bool IsFruit				{ get; private set; }

			public string Size				{ get; private set; }
			public FirmnessLevel Firmness	{ get; private set; }
			public int BerryIndex			{ get; private set; }
			public int minBerries			{ get; private set; }
			public int maxBerries			{ get; private set; }

			public int Spicy				{ get; private set; }
			public int Dry					{ get; private set; }
			public int Sweet				{ get; private set; }
			public int Bitter				{ get; private set; }
			public int Sour					{ get; private set; }

			public int Power				{ get; private set; }
			public Types Type				{ get; private set; }

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
				//assign background art of letter based on item
				switch (berry)
				{
					case Items.AGUAV_BERRY:
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
					//ToDo: Finish other half...
						break;
					default:
						IsFruit = false;
						break;
				}
			}

			public Berry(Items berry, int PhaseTime, string Size = "", FirmnessLevel Firmness = 0, int minBerries = 0, int maxBerries = 0) : this(berry)
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
}
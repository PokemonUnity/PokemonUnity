using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	public struct Form 
	{
		public Forms Id { get; private set; }
		public string Identifier { get; private set; }
		/// <summary>
		/// Use this for stats. <see cref="Pokemons"/> 
		/// Identifier for <seealso cref="Id"/>.
		/// Use this with <see cref="Game.PokemonData"/> 
		/// (for data accuracy).
		/// Pokemon Data (Stats/Exp) is stored under <see cref="PokemonData"/> 
		/// which is indexed by <see cref="Pokemons"/> and not <see cref="Forms"/> 
		/// </summary>
		public Pokemons Pokemon { get; private set; }
		/// <summary>
		/// Root species shared across all variations and forms.
		/// </summary>
		public Pokemons Base { get; private set; }
		public bool IsMega { get; private set; }
		public bool IsBattleOnly { get; private set; }
		public bool IsDefault { get; private set; }
		public byte FormOrder { get; private set; }
		public int Order { get; private set; }

		public Form(Forms id, Pokemons pkmn, Pokemons species, string identifier = null, bool isMega = false, bool isBattleOnly = false, bool isDefault = false, byte formOrder = 0, int order = 0)
		{
			Id = id;
			Identifier = identifier;
			Pokemon = pkmn;
			Base = species; //GetSpecies(id);
			IsMega = isMega;
			IsBattleOnly = isBattleOnly;
			IsDefault = isDefault;
			FormOrder = formOrder;
			Order = order;
		}

		public int GetArrayId()
		{
			for (int i = 0; i < Game.PokemonFormsData[Base].Length; i++)
			{
				if(Game.PokemonFormsData[Base][i].Id == Id)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// </summary>
		/// <param name="form"></param>
		/// <returns>Outputs values for <seealso cref="Pokemon"/></returns>
		/// <remarks>
		///	<see cref="Pokemons.UNOWN"/> = letter of the alphabet.
		///	<see cref="Pokemons.DEOXYS"/> = which of the four forms.
		///	<see cref="Pokemons.BURMY"/>/<see cref="Pokemons.WORMADAM"/> = cloak type. Does not change for Wormadam.
		///	<see cref="Pokemons.SHELLOS"/>/<see cref="Pokemons.GASTRODON"/> = west/east alt colours.
		///	<see cref="Pokemons.ROTOM"/> = different possesed appliance forms.
		///	<see cref="Pokemons.GIRATINA"/> = Origin/Altered form.
		///	<see cref="Pokemons.SHAYMIN"/> = Land/Sky form.
		///	<see cref="Pokemons.ARCEUS"/> = Type.
		///	<see cref="Pokemons.BASCULIN"/> = appearance.
		///	<see cref="Pokemons.DEERLING"/>/<see cref="Pokemons.SAWSBUCK"/> = appearance.
		///	<see cref="Pokemons.TORNADUS"/>/<see cref="Pokemons.THUNDURUS"/>/<see cref="Pokemons.LANDORUS"/> = Incarnate/Therian forms.
		///	<see cref="Pokemons.KYUREM"/> = Normal/White/Black forms.
		///	<see cref="Pokemons.KELDEO"/> = Ordinary/Resolute forms.
		///	<see cref="Pokemons.MELOETTA"/> = Aria/Pirouette forms.
		///	<see cref="Pokemons.GENESECT"/> = different Drives.
		///	<see cref="Pokemons.VIVILLON"/> = different Patterns.
		///	<see cref="Pokemons.FLABEBE"/>/<see cref="Pokemons.FLOETTE"/>/<see cref="Pokemons.FLORGES"/> = Flower colour.
		///	<see cref="Pokemons.FURFROU"/> = haircut.
		///	<see cref="Pokemons.PUMPKABOO"/>/<see cref="Pokemons.GOURGEIST"/> = small/average/large/super sizes. 
		///	<see cref="Pokemons.HOOPA"/> = Confined/Unbound forms.
		///	<see cref="Pokemons.CASTFORM"/>? = different weather forms
		///	<see cref="Pokemons.PIKACHU"/>, 
		/// and MegaEvolutions?
		/// </remarks>
		/// This isnt needed... 
		private static Pokemons GetSpecies(Forms form)
		{
			switch (form)
			{
				#region
				case Forms.VENUSAUR_MEGA:
					return Pokemons.VENUSAUR_MEGA;
				case Forms.CHARIZARD_MEGA_X:
					return Pokemons.CHARIZARD_MEGA_X;
				case Forms.CHARIZARD_MEGA_Y:
					return Pokemons.CHARIZARD_MEGA_Y;
				case Forms.BLASTOISE_MEGA:
					return Pokemons.BLASTOISE_MEGA;
				case Forms.BEEDRILL_MEGA:
					return Pokemons.BEEDRILL_MEGA;
				case Forms.PIDGEOT_MEGA:
					return Pokemons.PIDGEOT_MEGA;
				case Forms.RATTATA_ALOLA:
					return Pokemons.RATTATA_ALOLA;
				case Forms.RATICATE_ALOLA:
					return Pokemons.RATICATE_ALOLA;
				case Forms.RATICATE_TOTEM_ALOLA:
					return Pokemons.RATICATE_TOTEM_ALOLA;
				case Forms.PIKACHU_ROCK_STAR:					//return Pokemons.PIKACHU_ROCK_STAR;
				case Forms.PIKACHU_BELLE:						//return Pokemons.PIKACHU_BELLE;
				case Forms.PIKACHU_POP_STAR:					//return Pokemons.PIKACHU_POP_STAR;
				case Forms.PIKACHU_PHD:							//return Pokemons.PIKACHU_PHD;
				case Forms.PIKACHU_LIBRE:						//return Pokemons.PIKACHU_LIBRE;
				case Forms.PIKACHU_COSPLAY:						//return Pokemons.PIKACHU_COSPLAY;
				case Forms.PIKACHU_ORIGINAL_CAP:				//return Pokemons.PIKACHU_ORIGINAL_CAP;
				case Forms.PIKACHU_HOENN_CAP:					//return Pokemons.PIKACHU_HOENN_CAP;
				case Forms.PIKACHU_SINNOH_CAP:					//return Pokemons.PIKACHU_SINNOH_CAP;
				case Forms.PIKACHU_UNOVA_CAP:					//return Pokemons.PIKACHU_UNOVA_CAP;
				case Forms.PIKACHU_KALOS_CAP:					//return Pokemons.PIKACHU_KALOS_CAP;
				case Forms.PIKACHU_ALOLA_CAP:					//return Pokemons.PIKACHU_ALOLA_CAP;
				case Forms.PIKACHU_PARTNER_CAP:					//return Pokemons.PIKACHU_PARTNER_CAP;
					return Pokemons.PIKACHU;
				case Forms.RAICHU_ALOLA:
					return Pokemons.RAICHU_ALOLA;
				case Forms.SANDSHREW_ALOLA:
					return Pokemons.SANDSHREW_ALOLA;
				case Forms.SANDSLASH_ALOLA:
					return Pokemons.SANDSLASH_ALOLA;
				case Forms.VULPIX_ALOLA:
					return Pokemons.VULPIX_ALOLA;
				case Forms.NINETALES_ALOLA:
					return Pokemons.NINETALES_ALOLA;
				case Forms.DIGLETT_ALOLA:
					return Pokemons.DIGLETT_ALOLA;
				case Forms.DUGTRIO_ALOLA:
					return Pokemons.DUGTRIO_ALOLA;
				case Forms.MEOWTH_ALOLA:
					return Pokemons.MEOWTH_ALOLA;
				case Forms.PERSIAN_ALOLA:
					return Pokemons.PERSIAN_ALOLA;
				case Forms.ALAKAZAM_MEGA:
					return Pokemons.ALAKAZAM_MEGA;
				case Forms.GEODUDE_ALOLA:
					return Pokemons.GEODUDE_ALOLA;
				case Forms.GRAVELER_ALOLA:
					return Pokemons.GRAVELER_ALOLA;
				case Forms.GOLEM_ALOLA:
					return Pokemons.GOLEM_ALOLA;
				case Forms.SLOWBRO_MEGA:
					return Pokemons.SLOWBRO_MEGA;
				case Forms.GRIMER_ALOLA:
					return Pokemons.GRIMER_ALOLA;
				case Forms.MUK_ALOLA:
					return Pokemons.MUK_ALOLA;
				case Forms.GENGAR_MEGA:
					return Pokemons.GENGAR_MEGA;
				case Forms.EXEGGUTOR_ALOLA:
					return Pokemons.EXEGGUTOR_ALOLA;
				case Forms.MAROWAK_ALOLA:
					return Pokemons.MAROWAK_ALOLA;
				case Forms.MAROWAK_TOTEM:
					return Pokemons.MAROWAK_TOTEM;
				case Forms.KANGASKHAN_MEGA:
					return Pokemons.KANGASKHAN_MEGA;
				case Forms.PINSIR_MEGA:
					return Pokemons.PINSIR_MEGA;
				case Forms.GYARADOS_MEGA:
					return Pokemons.GYARADOS_MEGA;
				case Forms.AERODACTYL_MEGA:
					return Pokemons.AERODACTYL_MEGA;
				case Forms.MEWTWO_MEGA_X:
					return Pokemons.MEWTWO_MEGA_X;
				case Forms.MEWTWO_MEGA_Y:
					return Pokemons.MEWTWO_MEGA_Y;
				case Forms.PICHU_SPIKY_EARED:
					return Pokemons.PICHU;
				case Forms.AMPHAROS_MEGA:
					return Pokemons.AMPHAROS_MEGA;
				case Forms.UNOWN_A:
				case Forms.UNOWN_B:
				case Forms.UNOWN_C:
				case Forms.UNOWN_D:
				case Forms.UNOWN_E:
				case Forms.UNOWN_F:
				case Forms.UNOWN_G:
				case Forms.UNOWN_H:
				case Forms.UNOWN_I:
				case Forms.UNOWN_J:
				case Forms.UNOWN_K:
				case Forms.UNOWN_L:
				case Forms.UNOWN_M:
				case Forms.UNOWN_N:
				case Forms.UNOWN_O:
				case Forms.UNOWN_P:
				case Forms.UNOWN_Q:
				case Forms.UNOWN_R:
				case Forms.UNOWN_S:
				case Forms.UNOWN_T:
				case Forms.UNOWN_U:
				case Forms.UNOWN_V:
				case Forms.UNOWN_W:
				case Forms.UNOWN_X:
				case Forms.UNOWN_Y:
				case Forms.UNOWN_Z:
				case Forms.UNOWN_EXCLAMATION:
				case Forms.UNOWN_QUESTION:
					return Pokemons.UNOWN;
				case Forms.STEELIX_MEGA:
					return Pokemons.STEELIX_MEGA;
				case Forms.SCIZOR_MEGA:
					return Pokemons.SCIZOR_MEGA;
				case Forms.HERACROSS_MEGA:
					return Pokemons.HERACROSS_MEGA;
				case Forms.HOUNDOOM_MEGA:
					return Pokemons.HOUNDOOM_MEGA;
				case Forms.TYRANITAR_MEGA:
					return Pokemons.TYRANITAR_MEGA;
				case Forms.SCEPTILE_MEGA:
					return Pokemons.SCEPTILE_MEGA;
				case Forms.BLAZIKEN_MEGA:
					return Pokemons.BLAZIKEN_MEGA;
				case Forms.SWAMPERT_MEGA:
					return Pokemons.SWAMPERT_MEGA;
				case Forms.GARDEVOIR_MEGA:
					return Pokemons.GARDEVOIR_MEGA;
				case Forms.SABLEYE_MEGA:
					return Pokemons.SABLEYE_MEGA;
				case Forms.MAWILE_MEGA:
					return Pokemons.MAWILE_MEGA;
				case Forms.AGGRON_MEGA:
					return Pokemons.AGGRON_MEGA;
				case Forms.MEDICHAM_MEGA:
					return Pokemons.MEDICHAM_MEGA;
				case Forms.MANECTRIC_MEGA:
					return Pokemons.MANECTRIC_MEGA;
				case Forms.SHARPEDO_MEGA:
					return Pokemons.SHARPEDO_MEGA;
				case Forms.CAMERUPT_MEGA:
					return Pokemons.CAMERUPT_MEGA;
				case Forms.ALTARIA_MEGA:
					return Pokemons.ALTARIA_MEGA;
				case Forms.CASTFORM_SUNNY:
					return Pokemons.CASTFORM_SUNNY;
				case Forms.CASTFORM_RAINY:
					return Pokemons.CASTFORM_RAINY;
				case Forms.CASTFORM_SNOWY:
					return Pokemons.CASTFORM_SNOWY;
				case Forms.BANETTE_MEGA:
					return Pokemons.BANETTE_MEGA;
				case Forms.ABSOL_MEGA:
					return Pokemons.ABSOL_MEGA;
				case Forms.GLALIE_MEGA:
					return Pokemons.GLALIE_MEGA;
				case Forms.SALAMENCE_MEGA:
					return Pokemons.SALAMENCE_MEGA;
				case Forms.METAGROSS_MEGA:
					return Pokemons.METAGROSS_MEGA;
				case Forms.LATIAS_MEGA:
					return Pokemons.LATIAS_MEGA;
				case Forms.LATIOS_MEGA:
					return Pokemons.LATIOS_MEGA;
				case Forms.KYOGRE_PRIMAL:
					return Pokemons.KYOGRE_PRIMAL;
				case Forms.GROUDON_PRIMAL:
					return Pokemons.GROUDON_PRIMAL;
				case Forms.RAYQUAZA_MEGA:
					return Pokemons.RAYQUAZA_MEGA;
				case Forms.DEOXYS_NORMAL:
					return Pokemons.DEOXYS;
				case Forms.DEOXYS_ATTACK:
					return Pokemons.DEOXYS_ATTACK;
				case Forms.DEOXYS_DEFENSE:
					return Pokemons.DEOXYS_DEFENSE;
				case Forms.DEOXYS_SPEED:
					return Pokemons.DEOXYS_SPEED;
				case Forms.BURMY_PLANT:
				case Forms.BURMY_SANDY:
				case Forms.BURMY_TRASH:
					return Pokemons.BURMY;
				case Forms.WORMADAM_SANDY:
					return Pokemons.WORMADAM_SANDY;
				case Forms.WORMADAM_TRASH:
					return Pokemons.WORMADAM_TRASH;
				case Forms.MOTHIM_PLANT:
				case Forms.MOTHIM_SANDY:
				case Forms.MOTHIM_TRASH:
					return Pokemons.MOTHIM;
				case Forms.CHERRIM_OVERCAST:
				case Forms.CHERRIM_SUNSHINE:
					return Pokemons.CHERRIM;
				case Forms.SHELLOS_WEST:
				case Forms.SHELLOS_EAST:
					return Pokemons.SHELLOS;
				case Forms.GASTRODON_WEST:
				case Forms.GASTRODON_EAST:
					return Pokemons.GASTRODON;
				case Forms.LOPUNNY_MEGA:
					return Pokemons.LOPUNNY_MEGA;
				case Forms.GARCHOMP_MEGA:
					return Pokemons.GARCHOMP_MEGA;
				case Forms.LUCARIO_MEGA:
					return Pokemons.LUCARIO_MEGA;
				case Forms.ABOMASNOW_MEGA:
					return Pokemons.ABOMASNOW_MEGA;
				case Forms.GALLADE_MEGA:
					return Pokemons.GALLADE_MEGA;
				case Forms.ROTOM_HEAT:
					return Pokemons.ROTOM_HEAT;
				case Forms.ROTOM_WASH:
					return Pokemons.ROTOM_WASH;
				case Forms.ROTOM_FROST:
					return Pokemons.ROTOM_FROST;
				case Forms.ROTOM_FAN:
					return Pokemons.ROTOM_FAN;
				case Forms.ROTOM_MOW:
					return Pokemons.ROTOM_MOW;
				case Forms.GIRATINA_ORIGIN:
					return Pokemons.GIRATINA_ORIGIN;
				case Forms.SHAYMIN_LAND:
					return Pokemons.SHAYMIN;
				case Forms.SHAYMIN_SKY:
					return Pokemons.SHAYMIN_SKY;
				case Forms.ARCEUS_NORMAL:
				case Forms.ARCEUS_BUG:
				case Forms.ARCEUS_DARK:
				case Forms.ARCEUS_DRAGON:
				case Forms.ARCEUS_ELECTRIC:
				case Forms.ARCEUS_FIGHTING:
				case Forms.ARCEUS_FIRE:
				case Forms.ARCEUS_FLYING:
				case Forms.ARCEUS_GHOST:
				case Forms.ARCEUS_GRASS:
				case Forms.ARCEUS_GROUND:
				case Forms.ARCEUS_ICE:
				case Forms.ARCEUS_POISON:
				case Forms.ARCEUS_PSYCHIC:
				case Forms.ARCEUS_ROCK:
				case Forms.ARCEUS_STEEL:
				case Forms.ARCEUS_WATER:
				case Forms.ARCEUS_UNKNOWN:
				case Forms.ARCEUS_FAIRY:
					return Pokemons.ARCEUS;
				case Forms.AUDINO_MEGA:
					return Pokemons.AUDINO_MEGA;
				case Forms.BASCULIN_BLUE_STRIPED:
					return Pokemons.BASCULIN_BLUE_STRIPED;
				case Forms.DARMANITAN_ZEN:
					return Pokemons.DARMANITAN_ZEN;
				case Forms.DEERLING_SPRING:
				case Forms.DEERLING_SUMMER:
				case Forms.DEERLING_AUTUMN:
				case Forms.DEERLING_WINTER:
					return Pokemons.DEERLING;
				case Forms.SAWSBUCK_SPRING:
				case Forms.SAWSBUCK_SUMMER:
				case Forms.SAWSBUCK_AUTUMN:
				case Forms.SAWSBUCK_WINTER:
					return Pokemons.SAWSBUCK;
				case Forms.TORNADUS_THERIAN:
					return Pokemons.TORNADUS_THERIAN;
				case Forms.THUNDURUS_THERIAN:
					return Pokemons.THUNDURUS_THERIAN;
				case Forms.LANDORUS_THERIAN:
					return Pokemons.LANDORUS_THERIAN;
				case Forms.KYUREM_BLACK:
					return Pokemons.KYUREM_BLACK;
				case Forms.KYUREM_WHITE:
					return Pokemons.KYUREM_WHITE;
				case Forms.KELDEO_RESOLUTE:
					return Pokemons.KELDEO_RESOLUTE;
				case Forms.MELOETTA_PIROUETTE:
					return Pokemons.MELOETTA_PIROUETTE;
				case Forms.GENESECT_DOUSE:
				case Forms.GENESECT_SHOCK:
				case Forms.GENESECT_BURN:
				case Forms.GENESECT_CHILL:
					return Pokemons.GENESECT;
				case Forms.GRENINJA_BATTLE_BOND:
					return Pokemons.GRENINJA_BATTLE_BOND;
				case Forms.GRENINJA_ASH:
					return Pokemons.GRENINJA_ASH;
				case Forms.SCATTERBUG_ICY_SNOW:
				case Forms.SCATTERBUG_POLAR:
				case Forms.SCATTERBUG_TUNDRA:
				case Forms.SCATTERBUG_CONTINENTAL:
				case Forms.SCATTERBUG_GARDEN:
				case Forms.SCATTERBUG_ELEGANT:
				case Forms.SCATTERBUG_MEADOW:
				case Forms.SCATTERBUG_MODERN:
				case Forms.SCATTERBUG_MARINE:
				case Forms.SCATTERBUG_ARCHIPELAGO:
				case Forms.SCATTERBUG_HIGH_PLAINS:
				case Forms.SCATTERBUG_SANDSTORM:
				case Forms.SCATTERBUG_RIVER:
				case Forms.SCATTERBUG_MONSOON:
				case Forms.SCATTERBUG_SAVANNA:
				case Forms.SCATTERBUG_SUN:
				case Forms.SCATTERBUG_OCEAN:
				case Forms.SCATTERBUG_JUNGLE:
				case Forms.SCATTERBUG_FANCY:
				case Forms.SCATTERBUG_POKE_BALL:
					return Pokemons.SCATTERBUG;
				case Forms.SPEWPA_ICY_SNOW:
				case Forms.SPEWPA_POLAR:
				case Forms.SPEWPA_TUNDRA:
				case Forms.SPEWPA_CONTINENTAL:
				case Forms.SPEWPA_GARDEN:
				case Forms.SPEWPA_ELEGANT:
				case Forms.SPEWPA_MEADOW:
				case Forms.SPEWPA_MODERN:
				case Forms.SPEWPA_MARINE:
				case Forms.SPEWPA_ARCHIPELAGO:
				case Forms.SPEWPA_HIGH_PLAINS:
				case Forms.SPEWPA_SANDSTORM:
				case Forms.SPEWPA_RIVER:
				case Forms.SPEWPA_MONSOON:
				case Forms.SPEWPA_SAVANNA:
				case Forms.SPEWPA_SUN:
				case Forms.SPEWPA_OCEAN:
				case Forms.SPEWPA_JUNGLE:
				case Forms.SPEWPA_FANCY:
				case Forms.SPEWPA_POKE_BALL:
					return Pokemons.SPEWPA;
				case Forms.VIVILLON_MEADOW:
				case Forms.VIVILLON_ICY_SNOW:
				case Forms.VIVILLON_POLAR:
				case Forms.VIVILLON_TUNDRA:
				case Forms.VIVILLON_CONTINENTAL:
				case Forms.VIVILLON_GARDEN:
				case Forms.VIVILLON_ELEGANT:
				case Forms.VIVILLON_MODERN:
				case Forms.VIVILLON_MARINE:
				case Forms.VIVILLON_ARCHIPELAGO:
				case Forms.VIVILLON_HIGH_PLAINS:
				case Forms.VIVILLON_SANDSTORM:
				case Forms.VIVILLON_RIVER:
				case Forms.VIVILLON_MONSOON:
				case Forms.VIVILLON_SAVANNA:
				case Forms.VIVILLON_SUN:
				case Forms.VIVILLON_OCEAN:
				case Forms.VIVILLON_JUNGLE:
				case Forms.VIVILLON_FANCY:
				case Forms.VIVILLON_POKE_BALL:
					return Pokemons.VIVILLON;
				case Forms.FLABEBE_RED:
				case Forms.FLABEBE_YELLOW:
				case Forms.FLABEBE_ORANGE:
				case Forms.FLABEBE_BLUE:
				case Forms.FLABEBE_WHITE:
					return Pokemons.FLABEBE;
				case Forms.FLOETTE_RED:
				case Forms.FLOETTE_YELLOW:
				case Forms.FLOETTE_ORANGE:
				case Forms.FLOETTE_BLUE:
				case Forms.FLOETTE_WHITE:
					return Pokemons.FLOETTE;
				case Forms.FLORGES_RED:
				case Forms.FLORGES_YELLOW:
				case Forms.FLORGES_ORANGE:
				case Forms.FLORGES_BLUE:
				case Forms.FLORGES_WHITE:
					return Pokemons.FLORGES;
				case Forms.FURFROU_NATURAL:
				case Forms.FURFROU_HEART:
				case Forms.FURFROU_STAR:
				case Forms.FURFROU_DIAMOND:
				case Forms.FURFROU_DEBUTANTE:
				case Forms.FURFROU_MATRON:
				case Forms.FURFROU_DANDY:
				case Forms.FURFROU_LA_REINE:
				case Forms.FURFROU_KABUKI:
				case Forms.FURFROU_PHARAOH:
					return Pokemons.FURFROU;
				case Forms.MEOWSTIC_FEMALE:
					return Pokemons.MEOWSTIC_FEMALE;
				case Forms.AEGISLASH_BLADE:
					return Pokemons.AEGISLASH_BLADE;
				case Forms.PUMPKABOO_SMALL:
					return Pokemons.PUMPKABOO_SMALL;
				case Forms.PUMPKABOO_LARGE:
					return Pokemons.PUMPKABOO_LARGE;
				case Forms.PUMPKABOO_SUPER:
					return Pokemons.PUMPKABOO_SUPER;
				case Forms.GOURGEIST_SMALL:
					return Pokemons.GOURGEIST_SMALL;
				case Forms.GOURGEIST_LARGE:
					return Pokemons.GOURGEIST_LARGE;
				case Forms.GOURGEIST_SUPER:
					return Pokemons.GOURGEIST_SUPER;
				case Forms.XERNEAS_ACTIVE:
					return Pokemons.XERNEAS;
				case Forms.XERNEAS_NEUTRAL:
					return Pokemons.XERNEAS;
				case Forms.ZYGARDE_10:
					return Pokemons.ZYGARDE_10;
				case Forms.ZYGARDE_50:
					return Pokemons.ZYGARDE_50;
				case Forms.ZYGARDE_COMPLETE:
					return Pokemons.ZYGARDE_COMPLETE;
				case Forms.DIANCIE_MEGA:
					return Pokemons.DIANCIE_MEGA;
				case Forms.HOOPA_UNBOUND:
					return Pokemons.HOOPA_UNBOUND;
				case Forms.GUMSHOOS_TOTEM:
					return Pokemons.GUMSHOOS_TOTEM;
				case Forms.VIKAVOLT_TOTEM:
					return Pokemons.VIKAVOLT_TOTEM;
				case Forms.ORICORIO_POM_POM:
					return Pokemons.ORICORIO_POM_POM;
				case Forms.ORICORIO_PAU:
					return Pokemons.ORICORIO_PAU;
				case Forms.ORICORIO_SENSU:
					return Pokemons.ORICORIO_SENSU;
				case Forms.RIBOMBEE_TOTEM:
					return Pokemons.RIBOMBEE_TOTEM;
				case Forms.ROCKRUFF_OWN_TEMPO:
					return Pokemons.ROCKRUFF_OWN_TEMPO;
				case Forms.LYCANROC_MIDNIGHT:
					return Pokemons.LYCANROC_MIDNIGHT;
				case Forms.LYCANROC_DUSK:
					return Pokemons.LYCANROC_DUSK;
				case Forms.WISHIWASHI_SCHOOL:
					return Pokemons.WISHIWASHI_SCHOOL;
				case Forms.ARAQUANID_TOTEM:
					return Pokemons.ARAQUANID_TOTEM;
				case Forms.LURANTIS_TOTEM:
					return Pokemons.LURANTIS_TOTEM;
				case Forms.SALAZZLE_TOTEM:
					return Pokemons.SALAZZLE_TOTEM;
				case Forms.SILVALLY_NORMAL:
				case Forms.SILVALLY_FIGHTING:
				case Forms.SILVALLY_FLYING:
				case Forms.SILVALLY_POISON:
				case Forms.SILVALLY_GROUND:
				case Forms.SILVALLY_ROCK:
				case Forms.SILVALLY_BUG:
				case Forms.SILVALLY_GHOST:
				case Forms.SILVALLY_STEEL:
				case Forms.SILVALLY_FIRE:
				case Forms.SILVALLY_WATER:
				case Forms.SILVALLY_GRASS:
				case Forms.SILVALLY_ELECTRIC:
				case Forms.SILVALLY_PSYCHIC:
				case Forms.SILVALLY_ICE:
				case Forms.SILVALLY_DRAGON:
				case Forms.SILVALLY_DARK:
				case Forms.SILVALLY_FAIRY:
					return Pokemons.SILVALLY;
				case Forms.MINIOR_ORANGE_METEOR:
					return Pokemons.MINIOR_ORANGE_METEOR;
				case Forms.MINIOR_YELLOW_METEOR:
					return Pokemons.MINIOR_YELLOW_METEOR;
				case Forms.MINIOR_GREEN_METEOR:
					return Pokemons.MINIOR_GREEN_METEOR;
				case Forms.MINIOR_BLUE_METEOR:
					return Pokemons.MINIOR_BLUE_METEOR;
				case Forms.MINIOR_INDIGO_METEOR:
					return Pokemons.MINIOR_INDIGO_METEOR;
				case Forms.MINIOR_VIOLET_METEOR:
					return Pokemons.MINIOR_VIOLET_METEOR;
				case Forms.MINIOR_RED:
					return Pokemons.MINIOR_RED;
				case Forms.MINIOR_ORANGE:
					return Pokemons.MINIOR_ORANGE;
				case Forms.MINIOR_YELLOW:
					return Pokemons.MINIOR_YELLOW;
				case Forms.MINIOR_GREEN:
					return Pokemons.MINIOR_GREEN;
				case Forms.MINIOR_BLUE:
					return Pokemons.MINIOR_BLUE;
				case Forms.MINIOR_INDIGO:
					return Pokemons.MINIOR_INDIGO;
				case Forms.MINIOR_VIOLET:
					return Pokemons.MINIOR_VIOLET;
				case Forms.TOGEDEMARU_TOTEM:
					return Pokemons.TOGEDEMARU_TOTEM;
				case Forms.MIMIKYU_BUSTED:
					return Pokemons.MIMIKYU_BUSTED;
				case Forms.MIMIKYU_TOTEM_DISGUISED:
					return Pokemons.MIMIKYU_TOTEM_DISGUISED;
				case Forms.MIMIKYU_TOTEM_BUSTED:
					return Pokemons.MIMIKYU_TOTEM_BUSTED;
				case Forms.KOMMO_O_TOTEM:
					return Pokemons.KOMMO_O_TOTEM;
				case Forms.NECROZMA_DUSK:
					return Pokemons.NECROZMA_DUSK;
				case Forms.NECROZMA_DAWN:
					return Pokemons.NECROZMA_DAWN;
				case Forms.NECROZMA_ULTRA:
					return Pokemons.NECROZMA_ULTRA;
				case Forms.MAGEARNA_ORIGINAL:
					return Pokemons.MAGEARNA_ORIGINAL;
				#endregion
				default:
					return (Pokemons)form;//form.ToString().ToEnum<Pokemons>(Pokemons.NONE);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Monster
{
	// ###############################################################################
	// Mega Evolutions and Primal Reversions are treated as form changes in
	// Essentials. The code below is just more of what's in the Pokemon_MultipleForms
	// script section, but specifically and only for the Mega Evolution and Primal
	// Reversion forms.
	// ###############################################################################
	public partial class Pokemon : PokemonEssentials.Interface.PokeBattle.IPokemonMegaEvolution
	{
		public bool IsMega { get { return Kernal.PokemonFormsData[Species][form].IsMega; } }
		//public override bool hasMegaForm { get { if (effects.Transform) return false; return base.hasMegaForm; } }
		public bool IsPrimal { get; private set; }
		//public override bool hasPrimalForm { get { if (effects.Transform) return false; return base.hasPrimalForm; } }
		public bool hasMegaForm() {
			//v=MultipleForms.getMegaForm(this);
			int? v=MultipleForms.getMegaForm(this);
			return v!=null;
		}

		public bool isMega() {
			int? v=MultipleForms.getMegaForm(this);
			return v!=null && v.Value==@form;
		}

		public void makeMega() {
			int? v=MultipleForms.getMegaForm(this);
			if (v!=null) this.form=v.Value;
		}

		public void makeUnmega() {
			//int? v=MultipleForms.getUnmegaForm(this);
			int? v=0; //ToDo?...
			if (v!=null) this.form=v.Value;
		}

		public string megaName() {
			string v=null; //MultipleForms.getMegaName(this);
			return (v!=null) ? v : Game._INTL("Mega {1}",Game._INTL(this.Species.ToString(TextScripts.Name)));
		}

		public int megaMessage() {
			int? v=null; //MultipleForms.megaMessage(this);
			if (Species == Pokemons.RAYQUAZA) v = 1;
			return (v!=null) ? v.Value : 0;   // 0=default message, 1=Rayquaza message
		}

		public bool hasPrimalForm() {
			int? v=MultipleForms.getPrimalForm(this);
			return v!=null;
		}

		public bool isPrimal() {
			int? v=MultipleForms.getPrimalForm(this);
			return v!=null && v.Value==@form;
		}

		public void makePrimal() {
			int? v=MultipleForms.getPrimalForm(this);
			if (v!=null) this.form=v.Value;
		}

		public void makeUnprimal() {
			//int? v=MultipleForms.getUnprimalForm(this);
			int? v=0; //ToDo?...
			if (v!=null) this.form=v.Value;
		}
	}

	public static partial class MultipleForms {
		public static int? getMegaForm(Pokemon pokemon) {
			#region XY Mega Evolution
			if (pokemon.Species == Pokemons.VENUSAUR) {
				if (pokemon.Item == Items.VENUSAURITE) return 1;
			}
			if (pokemon.Species == Pokemons.CHARIZARD) {
				if (pokemon.Item == Items.CHARIZARDITE_X) return 1;
				if (pokemon.Item == Items.CHARIZARDITE_Y) return 2;
			}
			if (pokemon.Species == Pokemons.BLASTOISE) {
				if (pokemon.Item == Items.BLASTOISINITE) return 1;
			}
			if (pokemon.Species == Pokemons.ALAKAZAM) {
				if (pokemon.Item == Items.ALAKAZITE) return 1;
			}
			if (pokemon.Species == Pokemons.GENGAR) {
				if (pokemon.Item == Items.GENGARITE) return 1;
			}
			if (pokemon.Species == Pokemons.KANGASKHAN) {
				if (pokemon.Item == Items.KANGASKHANITE) return 1;
			}
			if (pokemon.Species == Pokemons.PINSIR) {
				if (pokemon.Item == Items.PINSIRITE) return 1;
			}
			if (pokemon.Species == Pokemons.GYARADOS) {
				if (pokemon.Item == Items.GYARADOSITE) return 1;
			}
			if (pokemon.Species == Pokemons.AERODACTYL) {
				if (pokemon.Item == Items.AERODACTYLITE) return 1;
			}
			if (pokemon.Species == Pokemons.MEWTWO) {
				if (pokemon.Item == Items.MEWTWONITE_X) return 1;
				if (pokemon.Item == Items.MEWTWONITE_Y) return 2;
			}
			if (pokemon.Species == Pokemons.AMPHAROS) {
				if (pokemon.Item == Items.AMPHAROSITE) return 1;
			}
			if (pokemon.Species == Pokemons.SCIZOR) {
				if (pokemon.Item == Items.SCIZORITE) return 1;
			}
			if (pokemon.Species == Pokemons.HERACROSS) {
				if (pokemon.Item == Items.HERACRONITE) return 1;
			}
			if (pokemon.Species == Pokemons.HOUNDOOM) {
				if (pokemon.Item == Items.HOUNDOOMINITE) return 1;
			}
			if (pokemon.Species == Pokemons.TYRANITAR) {
				if (pokemon.Item == Items.TYRANITARITE) return 1;
			}
			if (pokemon.Species == Pokemons.BLAZIKEN) {
				if (pokemon.Item == Items.BLAZIKENITE) return 1;
			}
			if (pokemon.Species == Pokemons.GARDEVOIR) {
				if (pokemon.Item == Items.GARDEVOIRITE) return 1;
			}
			if (pokemon.Species == Pokemons.MAWILE) {
				if (pokemon.Item == Items.MAWILITE) return 1;
			}
			if (pokemon.Species == Pokemons.AGGRON) {
				if (pokemon.Item == Items.AGGRONITE) return 1;
			}
			if (pokemon.Species == Pokemons.MEDICHAM) {
				if (pokemon.Item == Items.MEDICHAMITE) return 1;
			}
			if (pokemon.Species == Pokemons.MANECTRIC) {
				if (pokemon.Item == Items.MANECTITE) return 1;
			}
			if (pokemon.Species == Pokemons.BANETTE) {
				if (pokemon.Item == Items.BANETTITE) return 1;
			}
			if (pokemon.Species == Pokemons.ABSOL) {
				if (pokemon.Item == Items.ABSOLITE) return 1;
			}
			if (pokemon.Species == Pokemons.GARCHOMP) {
				if (pokemon.Item == Items.GARCHOMPITE) return 1;
			}
			if (pokemon.Species == Pokemons.LUCARIO) {
				if (pokemon.Item == Items.LUCARIONITE) return 1;
			}
			if (pokemon.Species == Pokemons.ABOMASNOW) {
				if (pokemon.Item == Items.ABOMASITE) return 1;
			}
			#endregion

			#region ORAS Mega Evolution 
			if (pokemon.Species == Pokemons.BEEDRILL) {
				if (pokemon.Item == Items.BEEDRILLITE) return 1;
			}
			if (pokemon.Species == Pokemons.PIDGEOT) {
				if (pokemon.Item == Items.PIDGEOTITE) return 1;
			}
			if (pokemon.Species == Pokemons.SLOWBRO) {
				if (pokemon.Item == Items.SLOWBRONITE) return 1;
			}
			if (pokemon.Species == Pokemons.STEELIX) {
				if (pokemon.Item == Items.STEELIXITE) return 1;
			}
			if (pokemon.Species == Pokemons.SCEPTILE) {
				if (pokemon.Item == Items.SCEPTILITE) return 1;
			}
			if (pokemon.Species == Pokemons.SWAMPERT) {
				if (pokemon.Item == Items.SWAMPERTITE) return 1;
			}
			if (pokemon.Species == Pokemons.SABLEYE) {
				if (pokemon.Item == Items.SABLENITE) return 1;
			}
			if (pokemon.Species == Pokemons.SHARPEDO) {
				if (pokemon.Item == Items.SHARPEDONITE) return 1;
			}
			if (pokemon.Species == Pokemons.CAMERUPT) {
				if (pokemon.Item == Items.CAMERUPTITE) return 1;
			}
			if (pokemon.Species == Pokemons.ALTARIA) {
				if (pokemon.Item == Items.ALTARIANITE) return 1;
			}
			if (pokemon.Species == Pokemons.GLALIE) {
				if (pokemon.Item == Items.GLALITITE) return 1;
			}
			if (pokemon.Species == Pokemons.SALAMENCE) {
				if (pokemon.Item == Items.SALAMENCITE) return 1;
			}
			if (pokemon.Species == Pokemons.METAGROSS) {
				if (pokemon.Item == Items.METAGROSSITE) return 1;
			}
			if (pokemon.Species == Pokemons.LATIAS) {
				if (pokemon.Item == Items.LATIASITE) return 1;
			}
			if (pokemon.Species == Pokemons.LATIOS) {
				if (pokemon.Item == Items.LATIOSITE) return 1;
			}
			if (pokemon.Species == Pokemons.RAYQUAZA) {
				if (pokemon.hasMove(Moves.DRAGON_ASCENT)) return 1;
			}
			if (pokemon.Species == Pokemons.LOPUNNY) {
				if (pokemon.Item == Items.LOPUNNITE) return 1;
			}
			if (pokemon.Species == Pokemons.GALLADE) {
				if (pokemon.Item == Items.GALLADITE) return 1;
			}
			if (pokemon.Species == Pokemons.AUDINO) {
				if (pokemon.Item == Items.AUDINITE) return 1;
			}
			if (pokemon.Species == Pokemons.DIANCIE) {
				if (pokemon.Item == Items.DIANCITE) return 1;
			}
			#endregion
			
			//return (int)pokemon.form;
			return null;
		}

		public static int? getPrimalForm(Pokemon pokemon) {
			#region Primal Reversion
			if (pokemon.Species == Pokemons.KYOGRE) {
				if (pokemon.Item == Items.BLUE_ORB) return 1;
			}
			if (pokemon.Species == Pokemons.GROUDON) {
				if (pokemon.Item == Items.RED_ORB) return (int)Forms.GROUDON_PRIMAL;
			}
			#endregion
			
			//return (int)pokemon.form;
			return null;
		}
		/*public int getMegaForm(Pokemon pokemon) {
	#region XY Mega Evolution
if (pokemon.Species == Pokemons.VENUSAUR) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.VENUSAURITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,100,123,80,122,120];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.THICKFAT,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 24;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1555;
   return;
}
});

if (pokemon.Species == Pokemons.CHARIZARD) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.CHARIZARDITEX) return 1;
   if (pokemon.Item == Items.CHARIZARDITEY) return 2;
   return;
},
"getMegaName"=>proc{|pokemon|
   if (pokemon.form==1) return Game._INTL("Mega Charizard X");
   if (pokemon.form==2) return Game._INTL("Mega Charizard Y");
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [78,130,111,100,130,85];
   if (pokemon.form==2) return [78,104,78,100,159,115];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.DRAGON;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.TOUGHCLAWS,0]];
   if (pokemon.form==2) return [[Abilities.DROUGHT,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1105;
   if (pokemon.form==2) return 1005;
   return;
}
});

if (pokemon.Species == Pokemons.BLASTOISE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.BLASTOISINITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [79,103,120,78,135,115];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.MEGALAUNCHER,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1011;
   return;
}
});

if (pokemon.Species == Pokemons.ALAKAZAM) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.ALAKAZITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [55,50,65,150,175,95];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.TRACE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 12;
   return;
}
});

if (pokemon.Species == Pokemons.GENGAR) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.GENGARITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [60,65,80,130,170,95];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SHADOWTAG,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 14;
   return;
}
});

if (pokemon.Species == Pokemons.KANGASKHAN) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.KANGASKHANITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [105,125,100,100,60,100];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.PARENTALBOND,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1000;
   return;
}
});

if (pokemon.Species == Pokemons.PINSIR) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.PINSIRITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [65,155,120,105,65,90];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.FLYING;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.AERILATE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 17;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 590;
   return;
}
});

if (pokemon.Species == Pokemons.GYARADOS) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.GYARADOSITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [95,155,109,81,70,130];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.DARK;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.MOLDBREAKER,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 3050;
   return;
}
});

if (pokemon.Species == Pokemons.AERODACTYL) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.AERODACTYLITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,135,85,150,70,95];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.TOUGHCLAWS,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 21;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 790;
   return;
}
});

if (pokemon.Species == Pokemons.MEWTWO) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.MEWTWONITEX) return 1;
   if (pokemon.Item == Items.MEWTWONITEY) return 2;
   return;
},
"getMegaName"=>proc{|pokemon|
   if (pokemon.form==1) return Game._INTL("Mega Mewtwo X");
   if (pokemon.form==2) return Game._INTL("Mega Mewtwo Y");
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [106,190,100,130,154,100];
   if (pokemon.form==2) return [106,150,70,140,194,120];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.FIGHTING;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.STEADFAST,0]];
   if (pokemon.form==2) return [[Abilities.INSOMNIA,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 23;
   if (pokemon.form==2) return 15;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1270;
   if (pokemon.form==2) return 330;
   return;
}
});

if (pokemon.Species == Pokemons.AMPHAROS) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.AMPHAROSITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [90,95,105,45,165,110];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.DRAGON;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.MOLDBREAKER,0]];
   return;
}
});

if (pokemon.Species == Pokemons.SCIZOR) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SCIZORITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,150,140,75,65,100];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.TECHNICIAN,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 20;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1250;
   return;
}
});

if (pokemon.Species == Pokemons.HERACROSS) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.HERACRONITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,185,115,75,40,105];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SKILLLINK,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 17;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 625;
   return;
}
});

if (pokemon.Species == Pokemons.HOUNDOOM) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.HOUNDOOMINITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [75,90,90,115,140,90];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SOLARPOWER,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 19;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 495;
   return;
}
});

if (pokemon.Species == Pokemons.TYRANITAR) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.TYRANITARITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [100,164,150,71,95,120];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SANDSTREAM,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 25;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 2550;
   return;
}
});

if (pokemon.Species == Pokemons.BLAZIKEN) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.BLAZIKENITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,160,80,100,130,80];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SPEEDBOOST,0]];
   return;
}
});

if (pokemon.Species == Pokemons.GARDEVOIR) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.GARDEVOIRITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [68,85,65,100,165,135];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.PIXILATE,0]];
   return;
}
});

if (pokemon.Species == Pokemons.MAWILE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.MAWILITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [50,105,125,50,55,95];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.HUGEPOWER,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 10;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 235;
   return;
}
});

if (pokemon.Species == Pokemons.AGGRON) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.AGGRONITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,140,230,50,60,80];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.STEEL;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.FILTER,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 22;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 3950;
   return;
}
});

if (pokemon.Species == Pokemons.MEDICHAM) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.MEDICHAMITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [60,100,85,100,80,85];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.PUREPOWER,0]];
   return;
}
});

if (pokemon.Species == Pokemons.MANECTRIC) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.MANECTITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,75,80,135,135,80];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.INTIMIDATE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 18;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 440;
   return;
}
});

if (pokemon.Species == Pokemons.BANETTE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.BANETTTITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [64,165,75,75,93,83];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.PRANKSTER,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 12;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 130;
   return;
}
});

if (pokemon.Species == Pokemons.ABSOL) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.ABSOLITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [65,150,60,115,115,60];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.MAGICBOUNCE,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 490;
   return;
}
});

if (pokemon.Species == Pokemons.GARCHOMP) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.GARCHOMPITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [108,170,115,92,120,95];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SANDFORCE,0]];
   return;
}
});

if (pokemon.Species == Pokemons.LUCARIO) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.LUCARIONITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,145,88,112,140,70];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.ADAPTABILITY,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 13;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 575;
   return;
}
});

if (pokemon.Species == Pokemons.ABOMASNOW) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.ABOMASITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [90,132,105,30,132,105];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SNOWWARNING,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 27;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1850;
   return;
}
});
	#endregion

	#region ORAS Mega Evolution 
if (pokemon.Species == Pokemons.BEEDRILL) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.BEEDRILLITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [65,150,40,145,15,80];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.ADAPTABILITY,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 14;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 405;
   return;
}
});

if (pokemon.Species == Pokemons.PIDGEOT) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.PIDGEOTITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [83,80,80,121,135,80];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.NOGUARD,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 22;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 505;
   return;
}
});

if (pokemon.Species == Pokemons.SLOWBRO) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SLOWBRONITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [95,75,180,30,130,80];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SHELLARMOR,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 20;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1200;
   return;
}
});

if (pokemon.Species == Pokemons.STEELIX) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.STEELIXITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [75,125,230,30,55,95];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SANDFORCE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 105;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 7400;
   return;
}
});

if (pokemon.Species == Pokemons.SCEPTILE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SCEPTILITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,110,75,145,145,85];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.DRAGON;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.LIGHTNINGROD,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 19;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 552;
   return;
}
});

if (pokemon.Species == Pokemons.SWAMPERT) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SWAMPERTITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [100,150,110,70,95,110];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SWIFTSWIM,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 19;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1020;
   return;
}
});

if (pokemon.Species == Pokemons.SABLEYE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SABLENITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [50,85,125,20,85,115];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.MAGICBOUNCE,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1610;
   return;
}
});

if (pokemon.Species == Pokemons.SHARPEDO) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SHARPEDONITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,140,70,105,110,65];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.STRONGJAW,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 25;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1303;
   return;
}
});

if (pokemon.Species == Pokemons.CAMERUPT) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.CAMERUPTITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [70,120,100,20,145,105];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SHEERFORCE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 25;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 3205;
   return;
}
});

if (pokemon.Species == Pokemons.ALTARIA) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.ALTARIANITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [75,110,110,80,110,105];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.FAIRY;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.PIXILATE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 15;
   return;
}
});

if (pokemon.Species == Pokemons.GLALIE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.GLALITITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,120,80,100,120,80];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.REFRIGERATE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 21;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 3502;
   return;
}
});

if (pokemon.Species == Pokemons.SALAMENCE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.SALAMENCITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [95,145,130,120,120,90];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.AERILATE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 18;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 1126;
   return;
}
});

if (pokemon.Species == Pokemons.METAGROSS) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.METAGROSSITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,145,150,110,105,110];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.TOUGHCLAWS,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 25;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 9429;
   return;
}
});

if (pokemon.Species == Pokemons.LATIAS) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.LATIASITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,100,120,110,140,150];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 18;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 520;
   return;
}
});

if (pokemon.Species == Pokemons.LATIOS) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.LATIOSITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [80,130,100,110,160,120];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 23;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 700;
   return;
}
});

if (pokemon.Species == Pokemons.RAYQUAZA) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.hasMove?(:DRAGONASCENT)) return 1;
   return;
},
"megaMessage"=>proc{|pokemon|
   return 1;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [105,180,100,115,180,100];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.DELTASTREAM,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 108;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 3920;
   return;
}
});

if (pokemon.Species == Pokemons.LOPUNNY) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.LOPUNNITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [65,136,94,135,54,96];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.FIGHTING;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.SCRAPPY,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 13;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 283;
   return;
}
});

if (pokemon.Species == Pokemons.GALLADE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.GALLADITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [68,165,95,110,65,115];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.INNERFOCUS,0]];
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 564;
   return;
}
});

if (pokemon.Species == Pokemons.AUDINO) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.AUDINITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [103,60,126,50,80,126];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.FAIRY;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.HEALER,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 15;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 320;
   return;
}
});

if (pokemon.Species == Pokemons.DIANCIE) {
"getMegaForm"=>proc{|pokemon|
   if (pokemon.Item == Items.DIANCITE) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [50,160,110,110,160,110];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.MAGICBOUNCE,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 11;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 278;
   return;
}
});
	#endregion

	#region Primal Reversion
if (pokemon.Species == Pokemons.KYOGRE) {
"getPrimalForm"=>proc{|pokemon|
   if (pokemon.Item == Items.BLUEORB) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [100,150,90,90,180,160];
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.PRIMORDIALSEA,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 98;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 4300;
   return;
}
});

if (pokemon.Species == Pokemons.GROUDON) {
"getPrimalForm"=>proc{|pokemon|
   if (pokemon.Item == Items.REDORB) return 1;
   return;
},
"getBaseStats"=>proc{|pokemon|
   if (pokemon.form==1) return [100,180,160,90,150,90];
   return;
},
"type2"=>proc{|pokemon|
   if (pokemon.form==1) return Types.FIRE;
   return;
},
"getAbilityList"=>proc{|pokemon|
   if (pokemon.form==1) return [[Abilities.DESOLATELAND,0]];
   return;
},
"height"=>proc{|pokemon|
   if (pokemon.form==1) return 50;
   return;
},
"weight"=>proc{|pokemon|
   if (pokemon.form==1) return 9997;
   return;
}
})
	#endregion
	}*/
	}
}
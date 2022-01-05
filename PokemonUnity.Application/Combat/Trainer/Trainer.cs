using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Combat;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity//.Combat 
{ 
	public partial class Trainer : PokemonEssentials.Interface.PokeBattle.ITrainer {
		#region Variables
		public string name { get; set; }
		public int id { get; set; }
		public int? metaID { get; set; }
		public TrainerTypes trainertype { get; set; }
		public int? outfit { get; set; }
		public bool[] badges { get; private set; }
		public int money { get; private set; }
		//public bool[] seen { get; private set; }
		//ToDo: list<forms>?
		public IDictionary<Pokemons,bool> seen { get; private set; }
		//public bool[] owned { get; private set; }
		//ToDo: if false: seen, if true: caught?
		public IDictionary<Pokemons,bool> owned { get; private set; }
		public int?[][] formseen { get; set; }
		//public int[][] formlastseen { get; private set; }
		public KeyValuePair<int, int?>[] formlastseen { get; set; }
		//public bool[] shadowcaught { get; private set; }
		public IList<Pokemons> shadowcaught { get; set; }
		public PokemonEssentials.Interface.PokeBattle.IPokemon[] party { get; set; }
		/// <summary>
		/// Whether the Pokédex was obtained
		/// </summary>
		public bool pokedex { get;  set; }   
		/// <summary>
		/// Whether the Pokégear was obtained
		/// </summary>
		public bool pokegear { get; set; }  
		public Languages? language { get; private set; }
		/// <summary>
		/// Name of this trainer type (localized)
		/// </summary>
		public string trainerTypeName { get { 
			return @trainertype.ToString() ?? "PkMn Trainer"; }
		}
		#endregion

		#region Constructor
		public Trainer (string name,TrainerTypes trainertype) {
			this.name=name;
			@language=(Languages)(Game.GameData as PokemonEssentials.Interface.IGameUtility).pbGetLanguage();
			this.trainertype=trainertype;
			@id=Core.Rand.Next(256);
			@id|=Core.Rand.Next(256)<<8;
			@id|=Core.Rand.Next(256)<<16;
			@id|=Core.Rand.Next(256)<<24;
			@metaID=0;
			@outfit=0;
			@pokegear=false;
			@pokedex=false;
			seen = new Dictionary<Pokemons, bool>();
			owned = new Dictionary<Pokemons, bool>();
			clearPokedex();
			//@shadowcaught=new bool[0];
			//for (int i = 1; i < Kernal.PokemonData.Count; i++) {
			//  @shadowcaught[i]=false;
			//}
			@shadowcaught=new List<Pokemons>();
			@badges=new bool[8]; //ToDo: Dynamic Badge Count...
			for (int i = 0; i < 8; i++) {
				@badges[i]=false;
			}
			@money=Core.INITIALMONEY;
			@party=new PokemonEssentials.Interface.PokeBattle.IPokemon[Core.MAXPARTYSIZE];
		}

		ITrainer ITrainer.initialize(string name, TrainerTypes trainertype)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Methods
		public string fullname { get {
			return string.Format("{0} {1}",this.trainerTypeName,@name);
		} }
		/// <summary>
		/// Portion of the ID which is visible on the Trainer Card
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int publicID(int? id=null) {   
			return id.HasValue ? (int)id.Value&0xFFFF : this.id&0xFFFF;
		}

		/// <summary>
		/// Other portion of the ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int secretID(int? id=null) {   
			return id.HasValue ? (int)id.Value>>16 : this.id>>16;
		}

		/// <summary>
		/// Random ID other than this Trainer's ID
		/// </summary>
		/// <returns></returns>
		public int getForeignID() {   
			int fid=0;
			do { //loop;
				fid=Core.Rand.Next(256);
				fid|=Core.Rand.Next(256)<<8;
				fid|=Core.Rand.Next(256)<<16;
				fid|=Core.Rand.Next(256)<<24;
				if (fid!=@id) break;
			} while (true);
			return fid;
		}

		public void setForeignID(ITrainer other) {
			@id=other.getForeignID();
		}

		public int MetaID { get {
			if (!@metaID.HasValue && Game.GameData != null) @metaID=Game.GameData.Player.GetHashCode();//ID;
			if (!@metaID.HasValue) @metaID=0;
			return @metaID.Value;
		} }

		public int Outfit { get {
			if (!@outfit.HasValue) @outfit=0;
			return @outfit.Value;
		} }

		public Languages Language { get {
			if (!@language.HasValue) @language=(Languages)(Game.GameData as PokemonEssentials.Interface.IGameUtility).pbGetLanguage();
			return @language.Value;
		} }

		public int Money{ get { return money; } set {
			@money=(int)Math.Max((int)Math.Min(value,Core.MAXMONEY),0);
		} }

		/// <summary>
		/// Money won when trainer is defeated
		/// </summary>
		/// <returns></returns>
		public int moneyEarned { get {   
			int ret=0;
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//   trainertypes=Marshal.load(f);
			//   if (!Kernal.TrainerMetaData[@trainertype]) return 30;
				ret=Kernal.TrainerMetaData[@trainertype].BaseMoney;
			//}
			return ret;
		} }

		/// <summary>
		/// Skill level (for AI)
		/// </summary>
		/// <returns></returns>
		public int skill { get {
			int ret=0;
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//   trainertypes=Marshal.load(f);
			//   if (!trainertypes[@trainertype]) return 30;
				ret= Kernal.TrainerMetaData[@trainertype].SkillLevel;
			//}
			return ret;
		} }

		public string skillCode { get {
			string ret="";
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//   trainertypes=Marshal.load(f);
			//   if (!trainertypes[@trainertype]) return "";
				ret= Kernal.TrainerMetaData[@trainertype].SkillCodes.Value.ToString();
			//}
			return ret;
		} }

		public bool hasSkillCode(string code) {
			string c=skillCode;
			if (c!=null && c!="" && c.Contains(code)) return true;
			return false;
		}

		/// <summary>
		/// Number of badges
		/// </summary>
		/// <returns></returns>
		public int numbadges { get {   
			int ret=0;
			for (int i = 0; i < @badges.Length; i++) {
				if (@badges[i]) ret+=1;
			}
			return ret;
		} }

		int ITrainer.gender { get; }
		public bool? gender { get { 
			bool? ret=null;   // 2 = gender unknown
			//pbRgssOpen("Data/trainertypes.dat","rb"){|f|
			//   trainertypes=Marshal.load(f);
			//	if (!trainertypes || !trainertypes[trainertype]) {
			//	  ret=null;
			//	}
			//	else {
					ret= Kernal.TrainerMetaData[@trainertype].Gender;
			//		if (!ret.HasValue) ret=null;
			//	}
			//}
			return ret;
		} }

		public bool isMale { get { 
			return this.gender==true; } }
		public bool isFemale { get { 
			return this.gender==false; } }

		public IEnumerable<PokemonEssentials.Interface.PokeBattle.IPokemon> pokemonParty { get {
			return @party.Where(item => item.IsNotNullOrNone() && !item.isEgg );
		} }

		public IEnumerable<PokemonEssentials.Interface.PokeBattle.IPokemon> ablePokemonParty { get {
			return @party.Where(item => item.IsNotNullOrNone() && !item.isEgg && item.HP>0 );
		} }

		public int partyCount { get {
			return @party.Length;
		} }

		public int pokemonCount { get {
			int ret=0;
			for (int i = 0; i < @party.Length; i++) {
				if (@party[i].IsNotNullOrNone() && !@party[i].isEgg) ret+=1;
			}
			return ret;
		} }

		public int ablePokemonCount { get {
			int ret=0;
			for (int i = 0; i < @party.Length; i++) {
				if (@party[i].IsNotNullOrNone() && !@party[i].isEgg && @party[i].HP>0) ret+=1;
			}
			return ret;
		} }

		public PokemonEssentials.Interface.PokeBattle.IPokemon firstParty { get {
			if (@party.Length==0) return new PokemonUnity.Monster.Pokemon();
			return @party[0];
		} }

		public PokemonEssentials.Interface.PokeBattle.IPokemon firstPokemon { get {
			PokemonEssentials.Interface.PokeBattle.IPokemon[] p=this.pokemonParty.ToArray();
			if (p.Length==0) return new PokemonUnity.Monster.Pokemon();
			return p[0];
		} }

		public PokemonEssentials.Interface.PokeBattle.IPokemon firstAblePokemon { get {
			PokemonEssentials.Interface.PokeBattle.IPokemon[] p=this.ablePokemonParty.ToArray();
			if (p.Length==0) return new PokemonUnity.Monster.Pokemon();
			return p[0];
		} }

		public PokemonEssentials.Interface.PokeBattle.IPokemon lastParty { get {
			if (@party.Length==0) return new PokemonUnity.Monster.Pokemon();
			return @party[@party.Length-1];
		} }

		public PokemonEssentials.Interface.PokeBattle.IPokemon lastPokemon { get {
			PokemonEssentials.Interface.PokeBattle.IPokemon[] p=this.pokemonParty.ToArray();
			if (p.Length==0) return new PokemonUnity.Monster.Pokemon();
			return p[p.Length-1];
		} }

		public PokemonEssentials.Interface.PokeBattle.IPokemon lastAblePokemon { get {
			PokemonEssentials.Interface.PokeBattle.IPokemon[] p=this.ablePokemonParty.ToArray();
			if (p.Length==0) return new PokemonUnity.Monster.Pokemon();
			return p[p.Length-1];
		} }

		/// <summary>
		/// Number of Pokémon seen
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public int pokedexSeen(Regions? region=null) {   
			int ret=0;
			if (region==null) {
				//for (int i = 0; i < Kernal.PokemonData.Count; i++) {
				//  if (@seen[i]) ret+=1;
				//}
				return seen.Count;
			}
			else {
				//int[] regionlist=Game.pbAllRegionalSpecies(region);
				Pokemons[] regionlist=new Pokemons[0];
				foreach (Pokemons i in regionlist) {
				if (@seen[i]) ret+=1;
				}
			}
			return ret;
		}

		/// <summary>
		/// Number of Pokémon owned
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public int pokedexOwned(Regions? region=null) {   
			int ret=0;
			if (region==null) {
				//for (int i = 0; i < Kernal.PokemonData.Count; i++) {
				//  if (@owned[i]) ret+=1;
				//}
				return owned.Count;
			}
			else {
				//int[] regionlist=Game.GameData.pbAllRegionalSpecies(region);
				Pokemons[] regionlist=new Pokemons[0];
				foreach (Pokemons i in regionlist) {
				if (@owned[i]) ret+=1;
				}
			}
			return ret;
		}

		public int numFormsSeen(Pokemons species) {
			int ret=0;
			int?[][] array=new int?[0][];//@formseen[species];
			//Game.GameData.Player.Pokedex[(int)species,2];
			for (int i = 0; i < (int)Math.Max(array[0].Length,array[1].Length); i++) {
				if (array[0][i].HasValue || array[1][i].HasValue) ret+=1;
			}
			return ret;
		}

		public bool hasSeen (Pokemons species) {
			//if (Pokemons.is_a(String) || Pokemons.is_a(Symbol)) {
			//  species=getID(PBSpecies,species);
			//}
			return species>0 ? @seen[species] : false;
		}

		public bool hasOwned (Pokemons species) {
			//if (species.is_a(String) || species.is_a(Symbol)) {
			//  species=getID(PBSpecies,species);
			//}
			return species>0 ? @owned[species] : false;
		}

		public void setSeen(Pokemons species) {
			//if (species.is_a(String) || species.is_a(Symbol)) {
			//  species=getID(PBSpecies,species);
			//}
			if (species>0) @seen[species]=true;
		}

		public void setOwned(Pokemons species) {
			//if (species.is_a(String) || species.is_a(Symbol)) {
			//  species=getID(PBSpecies,species);
			//}
			if (species>0) @owned[species]=true;
		}

		public void clearPokedex() {
			@seen.Clear(); //=new Dictionary<Pokemons, bool>();
			@owned.Clear(); //=new Dictionary<Pokemons, bool>();
			@formseen=new int?[0][];
			@formlastseen=new KeyValuePair<int, int?>[0];
			//for (int i = 1; i < Kernal.PokemonData.Count; i++) {
			//  @seen[i]=false;
			//  @owned[i]=false;
			//  @formlastseen[i]=[];
			//  @formseen[i]=new int?[]{null};
			//}
		}
		#endregion

		//public static implicit operator TrainerData(PokemonEssentials.Interface.PokeBattle.ITrainer trainer)
		//{
		//	return new TrainerData(trainer.name, trainer.gender.Value, trainer.publicID(), trainer.secretID());
		//}
	}
}
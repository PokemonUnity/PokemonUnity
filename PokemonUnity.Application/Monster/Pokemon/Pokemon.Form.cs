using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Overworld;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Monster
{
	public partial class Pokemon : PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms
	{
		/// <summary>
		///	Used only for a few pokemon to specify what form it's in. 
		/// </summary>
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
		//public Forms Form { get { return form.Id; } } //set { foreach(Form f in Kernal.PokemonFormsData[pokemons]) if (value == f.Id) formId = f.Order; } }
		public Monster.Data.Form Form { get { return Kernal.PokemonFormsData[pokemons][formId]; } }// set { if (value >= 0 && value <= _base.Forms) _base.Form = value; } }
		public int FormId { get {
				if (@forcedform!=null) return @forcedform.Value;
				//int v=MultipleForms.getForm(this);
				int? v=MultipleForms.getForm(this);
				if (v!=null) {
					//if form is not null, then value has been set already?...
					if (v!=@formId) this.formId=v.Value; //|| @form == null
					return v.Value;
				}
				return @formId; //|| 0
			}
			set {
				//if (value >= 0 && value <= Kernal.PokemonFormsData[pokemons].Length) 
					@formId=value;
				//MultipleForms.call("onSetForm",this,value);
				MultipleForms.onSetForm(this,value);
				this.calcStats();
				if(Game.GameData is PokemonEssentials.Interface.IGameUtility g) g.pbSeenForm(this);
			} }
		// Maybe a method, where when a form is changed
		// checks pokemon value and overwrites name and stats
		// Some forms have a SpeciesId and others are battle only
		public bool SetForm(int value) { if (value >= 0 && value <= Kernal.PokemonFormsData[pokemons].Length) { formId = value; return true; } else return false; } //Overwrite pokemons with when form changes? `pokemons = Form.Pokemon;`
		public bool SetForm(Forms value) { int i = 0; foreach (Form f in Kernal.PokemonFormsData[pokemons]) { if (value == f.Id) return SetForm(i); i++; } return false; }


		//public int form { get; internal set; } //ToDo: private get
		/// <summary>
		///  Time when Furfrou's/Hoopa's form was set
		/// </summary>
		public DateTime? formTime { get; set; }
		public int? @forcedform { get; set; }
		private int formId { get; set; }
		public int form { get {
				if (pokemons == Pokemons.NONE) return 0;
				if (@forcedform!=null) return @forcedform.Value;
				//int v=MultipleForms.getForm(this); //ToDo: Deal with infinite recursive loop...
				//int? v=MultipleForms.getForm(this);
				//if (v!=null) {
				//	//if form is not null, then value has been set already?...
				//	if (v!=@formId) this.formId=v.Value; //|| @form == null
				//	return v.Value;
				//}
				return @formId; //|| 0
			}
			set {
				@formId=value;
				//MultipleForms.call("onSetForm",this,value);
				MultipleForms.onSetForm(this,value);
				this.calcStats();
				if(Game.GameData is PokemonEssentials.Interface.IGameUtility g) g.pbSeenForm(this);
			} }

		public int formNoCall { set {
			//@form=value;
			SetForm(value);
			this.calcStats();
		} }

		/// <summary>
		/// Used by the Pokédex only
		/// </summary>
		/// <param name="value"></param>
		public void forceForm(int value) {
			@forcedform=value;
		}

		/*alias __mf_baseStats baseStats;
		alias __mf_ability ability;
		alias __mf_getAbilityList getAbilityList;
		alias __mf_type1 type1;
		alias __mf_type2 type2;
		alias __mf_height height;
		alias __mf_weight weight;
		alias __mf_getMoveList getMoveList;
		alias __mf_isCompatibleWithMove? isCompatibleWithMove?;
		alias __mf_wildHoldItems wildHoldItems;
		alias __mf_baseExp baseExp;
		alias __mf_evYield evYield;
		alias __mf_kind kind;
		alias __mf_dexEntry dexEntry;
		alias __mf_initialize initialize;
		public void baseStats() {
			v=MultipleForms.getBaseStats(this);
			if (v!=null) return v;
			return this.__mf_baseStats;
		}

		[System.Obsolete("DEPRECATED - do not use")]
		public Abilities ability() {   
			Abilities v=MultipleForms.ability(this);
			if (v!=null) return v;
			return this.__mf_ability;
		}

		public Abilities getAbilityList() {
			Abilities v=MultipleForms.getAbilityList(this);
			if (v!=null && v.Length>0) return v;
			return this.__mf_getAbilityList;
		}

		public Types type1() {
			Types v=MultipleForms.type1(this);
			if (v!=null) return v;
			return this.__mf_type1;
		}

		public Types type2() {
			Types v=MultipleForms.type2(this);
			if (v!=null) return v;
			return this.__mf_type2;
		}

		public int height() {
			int v=MultipleForms.height(this);
			if (v!=null) return v;
			return this.__mf_height;
		}

		public float weight() {
			float v=MultipleForms.weight(this);
			if (v!=null) return v;
			return this.__mf_weight;
		}

		public void getMoveList() {
			v=MultipleForms.getMoveList(this);
			if (v!=null) return v;
			return this.__mf_getMoveList;
		}

		public bool isCompatibleWithMove(Moves move) {
			v=MultipleForms.getMoveCompatibility(this);
			if (v!=null) {
				return v.any? {|j| j==move };
			}
			return this.__mf_isCompatibleWithMove(move);
		}

		public void wildHoldItems() {
			v=MultipleForms.wildHoldItems(this);
			if (v!=null) return v;
			return this.__mf_wildHoldItems;
		}

		public void baseExp() {
			v=MultipleForms.baseExp(this);
			if (v!=null) return v;
			return this.__mf_baseExp;
		}

		public void evYield() {
			v=MultipleForms.evYield(this);
			if (v!=null) return v;
			return this.__mf_evYield;
		}

		public void kind() {
			v=MultipleForms.kind(this);
			if (v!=null) return v;
			return this.__mf_kind;
		}

		public void dexEntry() {
			v=MultipleForms.dexEntry(this);
			if (v!=null) return v;
			return this.__mf_dexEntry;
		}*/

		/// <summary>
		/// Creates a new Pokémon object.
		/// </summary>
		/// <param name="species">Pokémon species.</param>
		/// <param name="level">Pokémon level.</param>
		/// <param name="player">Trainer object for the original trainer.</param>
		/// <param name="withMoves">If false, this Pokémon has no moves.</param>
		/// <returns></returns>
		public PokemonEssentials.Interface.PokeBattle.IPokemon initialize(Pokemons species, int level, PokemonEssentials.Interface.PokeBattle.ITrainer player = null, bool withMoves = true) {
			(this as PokemonEssentials.Interface.PokeBattle.IPokemon).initialize(species, level, player, withMoves); //__mf_initialize(*args);
			int f=MultipleForms.getFormOnCreation(this);
			if (f > -1) { //.Hasvalue
				(this as PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms).form=f;
				this.resetMoves();
			}
			return this;
		}
	}

	public partial class PokeBattle_RealBattlePeer : PokemonEssentials.Interface.PokeBattle.IBattlePeerMultipleForms { 
		public void pbOnEnteringBattle(PokemonEssentials.Interface.PokeBattle.IBattle battle,PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			if (pokemon is PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms p) {
				//int? f=MultipleForms.call("getFormOnEnteringBattle",pokemon);
				int? f=MultipleForms.getFormOnEnteringBattle(pokemon);
				if (f.HasValue) {
					p.form=f.Value;
					//pokemon.SetForm(f.Value);
				}
				//pokemon.SetForm(f);
			}
		}
	}

	/*public void drawSpot(bitmap,spotpattern,x,y,red,green,blue) {
		height=spotpattern.Length;
		width=spotpattern[0].Length;
		for (int yy = 0; yy < height; yy++) {
			spot=spotpattern[yy];
			for (int xx = 0; xx < width; xx++) {
				if (spot[xx]==1) {
				xOrg=(x+xx)<<1;
				yOrg=(y+yy)<<1;
				color=bitmap.get_pixel(xOrg,yOrg);
				r=color.red+red;
				g=color.green+green;
				b=color.blue+blue;
				color.red=(int)Math.Min((int)Math.Max(r,0),255);
				color.green=(int)Math.Min((int)Math.Max(g,0),255);
				color.blue=(int)Math.Min((int)Math.Max(b,0),255);
				bitmap.set_pixel(xOrg,yOrg,color);
				bitmap.set_pixel(xOrg+1,yOrg,color);
				bitmap.set_pixel(xOrg,yOrg+1,color);
				bitmap.set_pixel(xOrg+1,yOrg+1,color);
				}   
			}
		}
	}

	public void pbSpindaSpots(pokemon,bitmap) {
		spot1=[
			[0,0,1,1,1,1,0,0],
			[0,1,1,1,1,1,1,0],
			[1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1],
			[0,1,1,1,1,1,1,0],
			[0,0,1,1,1,1,0,0]
		];
		spot2=[
			[0,0,1,1,1,0,0],
			[0,1,1,1,1,1,0],
			[1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1],
			[0,1,1,1,1,1,0],
			[0,0,1,1,1,0,0]
		];
		spot3=[
			[0,0,0,0,0,1,1,1,1,0,0,0,0],
			[0,0,0,1,1,1,1,1,1,1,0,0,0],
			[0,0,1,1,1,1,1,1,1,1,1,0,0],
			[0,1,1,1,1,1,1,1,1,1,1,1,0],
			[0,1,1,1,1,1,1,1,1,1,1,1,0],
			[1,1,1,1,1,1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1,1,1,1,1,1],
			[0,1,1,1,1,1,1,1,1,1,1,1,0],
			[0,1,1,1,1,1,1,1,1,1,1,1,0],
			[0,0,1,1,1,1,1,1,1,1,1,0,0],
			[0,0,0,1,1,1,1,1,1,1,0,0,0],
			[0,0,0,0,0,1,1,1,0,0,0,0,0]
		];
		spot4=[
			[0,0,0,0,1,1,1,0,0,0,0,0],
			[0,0,1,1,1,1,1,1,1,0,0,0],
			[0,1,1,1,1,1,1,1,1,1,0,0],
			[0,1,1,1,1,1,1,1,1,1,1,0],
			[1,1,1,1,1,1,1,1,1,1,1,0],
			[1,1,1,1,1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1,1,1,1,1],
			[1,1,1,1,1,1,1,1,1,1,1,0],
			[0,1,1,1,1,1,1,1,1,1,1,0],
			[0,0,1,1,1,1,1,1,1,1,0,0],
			[0,0,0,0,1,1,1,1,1,0,0,0]
		];
		id=pokemon.personalID;
		h=(id>>28)&15;
		g=(id>>24)&15;
		f=(id>>20)&15;
		e=(id>>16)&15;
		d=(id>>12)&15;
		c=(id>>8)&15;
		b=(id>>4)&15;
		a=(id)&15;
		if (pokemon.isShiny?) {
			drawSpot(bitmap,spot1,b+33,a+25,-75,-10,-150);
			drawSpot(bitmap,spot2,d+21,c+24,-75,-10,-150);
			drawSpot(bitmap,spot3,f+39,e+7,-75,-10,-150);
			drawSpot(bitmap,spot4,h+15,g+6,-75,-10,-150);
		}
		else {
			drawSpot(bitmap,spot1,b+33,a+25,0,-115,-75);
			drawSpot(bitmap,spot2,d+21,c+24,0,-115,-75);
			drawSpot(bitmap,spot3,f+39,e+7,0,-115,-75);
			drawSpot(bitmap,spot4,h+15,g+6,0,-115,-75);
		}
	}*/

	// ###############################################################################
	public static partial class MultipleForms{
		public static int getForm(PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms pkmn)
		{
			if (pkmn is PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
			if (pokemon.Species == Pokemons.GIRATINA)
			{
				int[] maps=new int[] { 49, 50, 51, 72, 73 };   // Map IDs for Origin Forme
				if (pokemon.Item == Items.GRISEOUS_ORB ||
					(Game.GameData.GameMap != null && maps.Contains(Game.GameData.GameMap.map_id))) {
					//Game.GameData != null && maps.Contains(Game.GameData.Player.Area)) {
					return 1;
				}
				return 0;
			}
			else if (pokemon.Species == Pokemons.SHAYMIN)
			{
				if (pokemon.HP<=0 || pokemon.Status==Status.FROZEN || Game.IsNight) return 0; //null;
			}
			else if (pokemon.Species == Pokemons.ARCEUS)
			{
				if (pokemon.Item == Items.FIST_PLATE) return 1 ;
				if (pokemon.Item == Items.SKY_PLATE) return 2 ;
				if (pokemon.Item == Items.TOXIC_PLATE) return 3 ;
				if (pokemon.Item == Items.EARTH_PLATE) return 4 ;
				if (pokemon.Item == Items.STONE_PLATE) return 5 ;
				if (pokemon.Item == Items.INSECT_PLATE) return 6 ;
				if (pokemon.Item == Items.SPOOKY_PLATE) return 7 ;
				if (pokemon.Item == Items.IRON_PLATE) return 8 ;
				if (pokemon.Item == Items.FLAME_PLATE) return 10;
				if (pokemon.Item == Items.SPLASH_PLATE) return 11;
				if (pokemon.Item == Items.MEADOW_PLATE) return 12;
				if (pokemon.Item == Items.ZAP_PLATE) return 13;
				if (pokemon.Item == Items.MIND_PLATE) return 14;
				if (pokemon.Item == Items.ICICLE_PLATE) return 15;
				if (pokemon.Item == Items.DRACO_PLATE) return 16;
				if (pokemon.Item == Items.DREAD_PLATE) return 17;
				if (pokemon.Item == Items.PIXIE_PLATE) return 18;
				return 0;
			}
			else if (pokemon.Species == Pokemons.DEERLING || pokemon.Species == Pokemons.SAWSBUCK)
			{
				return (int)Game.Season;//pbGetSeason();
			}
			else if (pokemon.Species == Pokemons.KELDEO)
			{
				if (pokemon.hasMove(Moves.SECRET_SWORD)) return 1;	// Resolute Form
				return 0;											// Ordinary Form
			}
			else if (pokemon.Species == Pokemons.GENESECT)
			{
				if (pokemon.Item == Items.SHOCK_DRIVE) return 1;
				if (pokemon.Item == Items.BURN_DRIVE) return 2;
				if (pokemon.Item == Items.CHILL_DRIVE) return 3;
				if (pokemon.Item == Items.DOUSE_DRIVE) return 4;
				return 0;
			}
			else if (pokemon.Species == Pokemons.FURFROU)
			{
				if (!pkmn.formTime.HasValue || Game.GetTimeNow>pkmn.formTime.Value.AddDays(5)) {		// 5 days => +60*60*24*5
					return 0;
				}
				//continue;
			}
			else if (pokemon.Species == Pokemons.HOOPA)
			{
				if (!pkmn.formTime.HasValue || Game.GetTimeNow>pkmn.formTime.Value.AddDays(3)) {		// 3 days => +60*60*24*3
					return 0;
				}
				//continue;
			}
			//else 
				return pkmn.form; //0;
		}
		public static int? getFormOnEnteringBattle(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			if (pokemon.Species == Pokemons.BURMY)
			{
				Environments env=Game.GameData is PokemonEssentials.Interface.Field.IGameField f ? f.pbGetEnvironment() : Environments.None;
				//if (!Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor)) {
				if (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor) {
				//if (Kernal.TileData[Game.GameData.Player.Area] == Indoor) {
					return 2; // Trash Cloak
				} else if (env==Environments.Sand ||
							env==Environments.Rock ||
							env==Environments.Cave) {
					return 1; // Sandy Cloak
				}
				else {
					return 0; // Plant Cloak
				}
			}
			else if (pokemon.Species == Pokemons.XERNEAS)
			{
				return 1;
			}
			else return pokemon is PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms f ? f.form : (int?)null;
		}
		public static void onSetForm(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon,int form)
		{
			if (pokemon.Species == Pokemons.ROTOM)
			{
				Moves[] moves=new Moves[] {
					Moves.OVERHEAT,		// Heat, Microwave
					Moves.HYDRO_PUMP,	// Wash, Washing Machine
					Moves.BLIZZARD,		// Frost, Refrigerator
					Moves.AIR_SLASH,	// Fan
					Moves.LEAF_STORM	// Mow, Lawnmower
				};
				int hasoldmove=-1;
				for (int i = 0; i < 4; i++) {
					for (int j = 0; j < moves.Length; j++) {
						if (pokemon.moves[i].id == moves[j]) {
							hasoldmove=i; break;
						}
					}
					if (hasoldmove>=0) break;
				}
				if (form>0) {
					Moves newmove=moves[form-1];
					if (newmove!=Moves.NONE) { //&& hasConst(PBMoves,newmove)
						if (hasoldmove>=0 && Game.GameData is PokemonEssentials.Interface.IGameMessage m) {
							// Automatically replace the old form's special move with the new one's
							string oldmovename=Game._INTL(pokemon.moves[hasoldmove].id.ToString(TextScripts.Name));
							string newmovename=Game._INTL(newmove.ToString(TextScripts.Name));
							pokemon.moves[hasoldmove]=new Attack.Move(newmove);
							m.pbMessage(Game._INTL("\\se[]1,\\wt[4] 2,\\wt[4] and...\\wt[8] ...\\wt[8] ...\\wt[8] Poof!\\se[balldrop]\\1"));
							m.pbMessage(Game._INTL("{1} forgot how to\r\nuse {2}.\\1",pokemon.Name,oldmovename));
							m.pbMessage(Game._INTL("And...\\1"));
							m.pbMessage(Game._INTL("\\se[]{1} learned {2}!\\se[MoveLearnt]",pokemon.Name,newmovename));
						}
						else {
							// Try to learn the new form's special move
							if (Game.GameData is PokemonEssentials.Interface.Item.IGameItem i) i.pbLearnMove(pokemon,newmove,true);
							//pokemon.LearnMove(newmove, out bool s);
						}
					}
				}
				else {
					if (hasoldmove>=0) {
						// Forget the old form's special move
						string oldmovename=Game._INTL(pokemon.moves[hasoldmove].id.ToString(TextScripts.Name));
						pokemon.pbDeleteMoveAtIndex(hasoldmove);
						if (Game.GameData is PokemonEssentials.Interface.IGameMessage m) m.pbMessage(Game._INTL("{1} forgot {2}...",pokemon.Name,oldmovename));
						if (pokemon.moves.Count(i => i.id!=0)==0) {
							if (Game.GameData is PokemonEssentials.Interface.Item.IGameItem i) i.pbLearnMove(pokemon,Moves.THUNDER_SHOCK);
							//pokemon.LearnMove(Moves.THUNDER_SHOCK, out bool s);
						}
					}
				}
			}
			else if ((pokemon.Species == Pokemons.FURFROU || pokemon.Species == Pokemons.HOOPA) && pokemon is PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms f)
			{
				f.formTime=(form>0) ? Game.GetTimeNow : (DateTime?)null;
			}
		}
		public static int getFormOnCreation(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			if (pokemon.Species == Pokemons.UNOWN)
			{
				//return Core.Rand.Next(28);
				return Core.Rand.Next(Kernal.PokemonFormsData[pokemon.Species].Length);
			}
			else if (pokemon.Species == Pokemons.BURMY)
			{
				Environments env=Game.GameData is PokemonEssentials.Interface.Field.IGameField f ? f.pbGetEnvironment() : Environments.None;
				//if (!Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor)) {
				if (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor) {
				//if (Kernal.TileData[Game.GameData.Player.Area] == Indoor) {
					return 2; // Trash Cloak
				} else if (env==Environments.Sand ||
							env==Environments.Rock ||
							env==Environments.Cave) {
					return 1; // Sandy Cloak
				}
				else {
					return 0; // Plant Cloak
				}
			}
			else if (pokemon.Species == Pokemons.WORMADAM)
			{
				Environments env=Game.GameData is PokemonEssentials.Interface.Field.IGameField f ? f.pbGetEnvironment() : Environments.None;
				//if (!Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor)) {
				if (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor) {
				//if (Kernal.TileData[Game.GameData.Player.Area] == Indoor) {
					return 2; // Trash Cloak
				} else if (env==Environments.Sand || env==Environments.Rock ||
							env==Environments.Cave) {
					return 1; // Sandy Cloak
				}
				else {
					return 0; // Plant Cloak
				}
			}
			else if (pokemon.Species == Pokemons.SHELLOS || pokemon.Species == Pokemons.GASTRODON)
			{
				int[] maps=new int[] { 2, 5, 39, 41, 44, 69 };   // Map IDs for second form
				if (Game.GameData.GameMap != null && maps.Contains(Game.GameData.GameMap.map_id)) {
				//if (Game.GameData != null && maps.Contains(Game.GameData.Player.Area)) {
					return 1;
				}
				else {
					return 0;
				}
			}
			else if (pokemon.Species == Pokemons.BASCULIN)
			{
				return Core.Rand.Next(2);
			}
			else if (pokemon.Species == Pokemons.SCATTERBUG || pokemon.Species == Pokemons.SPEWPA || pokemon.Species == Pokemons.VIVILLON)
			{
				return Game.GameData.Trainer.secretID()%18;
			}
			else if (pokemon.Species == Pokemons.FLABEBE || pokemon.Species == Pokemons.FLOETTE || pokemon.Species == Pokemons.FLORGES)
			{
				return Core.Rand.Next(5);
			}
			else if (pokemon.Species == Pokemons.PUMPKABOO)
			{
				return (int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4));
			}
			else if (pokemon.Species == Pokemons.GOURGEIST)
			{
				return (int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4));
			}
			else return pokemon is PokemonEssentials.Interface.PokeBattle.IPokemonMultipleForms f ? f.form : 0; //.pokemon.FormId; //0;
		}
		public static Forms? getFormOnCreation(Pokemons pokemon, bool nullable)
		{
			if (pokemon == Pokemons.UNOWN)
			{
				//return Core.Rand.Next(28);
				//return Core.Rand.Next(Kernal.PokemonFormsData[pokemon].Length);
				return new Forms[] { Forms.UNOWN_A	
					,Forms.UNOWN_B		
					,Forms.UNOWN_C		
					,Forms.UNOWN_D		
					,Forms.UNOWN_E		
					,Forms.UNOWN_F		
					,Forms.UNOWN_G		
					,Forms.UNOWN_H		
					,Forms.UNOWN_I		
					,Forms.UNOWN_J		
					,Forms.UNOWN_K		
					,Forms.UNOWN_L		
					,Forms.UNOWN_M		
					,Forms.UNOWN_N		
					,Forms.UNOWN_O		
					,Forms.UNOWN_P		
					,Forms.UNOWN_Q		
					,Forms.UNOWN_R		
					,Forms.UNOWN_S		
					,Forms.UNOWN_T		
					,Forms.UNOWN_U		
					,Forms.UNOWN_V		
					,Forms.UNOWN_W		
					,Forms.UNOWN_X		
					,Forms.UNOWN_Y		
					,Forms.UNOWN_Z		
					,Forms.UNOWN_EXCLAMATION
					,Forms.UNOWN_QUESTION	}[Core.Rand.Next(Kernal.PokemonFormsData[pokemon].Length)];
			}
			else if (pokemon == Pokemons.BURMY)
			{
				Environments env=Game.GameData is PokemonEssentials.Interface.Field.IGameField f ? f.pbGetEnvironment() : Environments.None;
				//if (!Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor)) {
				if (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor) {
				//if (Kernal.TileData[Game.GameData.Player.Area] == Indoor) {
					return Forms.BURMY_TRASH; //2; // Trash Cloak
				} else if (env==Environments.Sand ||
							env==Environments.Rock ||
							env==Environments.Cave) {
					return Forms.BURMY_SANDY; //1; // Sandy Cloak
				}
				else {
					return Forms.BURMY_PLANT; //0; // Plant Cloak
				}
			}
			else if (pokemon == Pokemons.WORMADAM)
			{
				Environments env=Game.GameData is PokemonEssentials.Interface.Field.IGameField f ? f.pbGetEnvironment() : Environments.None;
				//if (!Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor)) {
				if (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor) {
				//if (Kernal.TileData[Game.GameData.Player.Area] == Indoor) {
					return Forms.WORMADAM_TRASH; //2; // Trash Cloak
				} else if (env==Environments.Sand || env==Environments.Rock ||
					env==Environments.Cave) {
					return Forms.WORMADAM_SANDY; //1; // Sandy Cloak
				}
				else {
					return Forms.WORMADAM_PLANT; //0; // Plant Cloak
				}
			}
			else if (pokemon == Pokemons.SHELLOS)
			{
				int[] maps=new int[] { 2, 5, 39, 41, 44, 69 };   // Map IDs for second form
				if (Game.GameData.GameMap != null && maps.Contains(Game.GameData.GameMap.map_id)) {
				//if (Game.GameData != null && maps.Contains(Game.GameData.Player.Area)) {
					return Forms.SHELLOS_EAST; //1;
				}
				else {
					return Forms.SHELLOS_WEST; //0;
				}
			}
			else if (pokemon == Pokemons.GASTRODON)
			{
				int[] maps=new int[] { 2, 5, 39, 41, 44, 69 };   // Map IDs for second form
				if (Game.GameData.GameMap != null && maps.Contains(Game.GameData.GameMap.map_id)) {
				//if (Game.GameData != null && maps.Contains(Game.GameData.Player.Area)) {
					return Forms.GASTRODON_EAST; //1;
				}
				else {
					return Forms.GASTRODON_WEST; //0;
				}
			}
			else if (pokemon == Pokemons.BASCULIN)
			{
				//return Core.Rand.Next(2);
				return new Forms[] { Forms.BASCULIN_RED_STRIPED, Forms.BASCULIN_BLUE_STRIPED }[Core.Rand.Next(2)];
			}
			else if (pokemon == Pokemons.SCATTERBUG)
			{
				//return Game.GameData.Player.Trainer.SecretID%18;
				return new Forms[] { Forms.SCATTERBUG_ICY_SNOW
					,Forms.SCATTERBUG_POLAR			
					,Forms.SCATTERBUG_TUNDRA			
					,Forms.SCATTERBUG_CONTINENTAL		
					,Forms.SCATTERBUG_GARDEN			
					,Forms.SCATTERBUG_ELEGANT			
					,Forms.SCATTERBUG_MEADOW			
					,Forms.SCATTERBUG_MODERN			
					,Forms.SCATTERBUG_MARINE			
					,Forms.SCATTERBUG_ARCHIPELAGO		
					,Forms.SCATTERBUG_HIGH_PLAINS		
					,Forms.SCATTERBUG_SANDSTORM		
					,Forms.SCATTERBUG_RIVER			
					,Forms.SCATTERBUG_MONSOON			
					,Forms.SCATTERBUG_SAVANNA			
					,Forms.SCATTERBUG_SUN			
					,Forms.SCATTERBUG_OCEAN			
					,Forms.SCATTERBUG_JUNGLE			
					,Forms.SCATTERBUG_FANCY			
					,Forms.SCATTERBUG_POKE_BALL		}[(int)Game.GameData.Trainer.secretID()%18];
			}
			else if (pokemon == Pokemons.SPEWPA)
			{
				//return Game.GameData.Player.Trainer.SecretID%18;
				return new Forms[] { Forms.SPEWPA_ICY_SNOW
					,Forms.SPEWPA_POLAR			
					,Forms.SPEWPA_TUNDRA			
					,Forms.SPEWPA_CONTINENTAL		
					,Forms.SPEWPA_GARDEN			
					,Forms.SPEWPA_ELEGANT			
					,Forms.SPEWPA_MEADOW			
					,Forms.SPEWPA_MODERN			
					,Forms.SPEWPA_MARINE			
					,Forms.SPEWPA_ARCHIPELAGO		
					,Forms.SPEWPA_HIGH_PLAINS		
					,Forms.SPEWPA_SANDSTORM		
					,Forms.SPEWPA_RIVER			
					,Forms.SPEWPA_MONSOON			
					,Forms.SPEWPA_SAVANNA			
					,Forms.SPEWPA_SUN			
					,Forms.SPEWPA_OCEAN			
					,Forms.SPEWPA_JUNGLE			
					,Forms.SPEWPA_FANCY			
					,Forms.SPEWPA_POKE_BALL		}[(int)Game.GameData.Trainer.secretID()%18];
			}
			else if (pokemon == Pokemons.VIVILLON)
			{
				//return Game.GameData.Player.Trainer.SecretID%18;
				return new Forms[] { Forms.VIVILLON_MEADOW
					,Forms.VIVILLON_ICY_SNOW		
					,Forms.VIVILLON_POLAR			
					,Forms.VIVILLON_TUNDRA		
					,Forms.VIVILLON_CONTINENTAL	
					,Forms.VIVILLON_GARDEN		
					,Forms.VIVILLON_ELEGANT		
					,Forms.VIVILLON_MODERN		
					,Forms.VIVILLON_MARINE		
					,Forms.VIVILLON_ARCHIPELAGO	
					,Forms.VIVILLON_HIGH_PLAINS	
					,Forms.VIVILLON_SANDSTORM		
					,Forms.VIVILLON_RIVER			
					,Forms.VIVILLON_MONSOON		
					,Forms.VIVILLON_SAVANNA		
					,Forms.VIVILLON_SUN			
					,Forms.VIVILLON_OCEAN			
					,Forms.VIVILLON_JUNGLE		}[(int)Game.GameData.Trainer.secretID()%18];
			}
			else if (pokemon == Pokemons.FLABEBE)
			{
				//return Core.Rand.Next(5);
				return new Forms[] { Forms.FLABEBE_RED
					,Forms.FLABEBE_YELLOW
					,Forms.FLABEBE_ORANGE
					,Forms.FLABEBE_BLUE  
					,Forms.FLABEBE_WHITE }[Core.Rand.Next(5)];
			}
			else if (pokemon == Pokemons.FLOETTE)
			{
				//return Core.Rand.Next(5);
				return new Forms[] { Forms.FLOETTE_RED
					,Forms.FLOETTE_YELLOW
					,Forms.FLOETTE_ORANGE
					,Forms.FLOETTE_BLUE  
					,Forms.FLOETTE_WHITE }[Core.Rand.Next(5)];
			}
			else if (pokemon == Pokemons.FLORGES)
			{
				//return Core.Rand.Next(5);
				return new Forms[] { Forms.FLORGES_RED
					,Forms.FLORGES_YELLOW
					,Forms.FLORGES_ORANGE
					,Forms.FLORGES_BLUE  
					,Forms.FLORGES_WHITE }[Core.Rand.Next(5)];
			}
			else if (pokemon == Pokemons.PUMPKABOO)
			{
				//return (int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4));
				return new Forms[] { Forms.PUMPKABOO_AVERAGE
					,Forms.PUMPKABOO_SMALL 
					,Forms.PUMPKABOO_LARGE 
					,Forms.PUMPKABOO_SUPER }[(int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4))];
			}
			else if (pokemon == Pokemons.GOURGEIST)
			{
				//return (int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4));
				return new Forms[] { Forms.GOURGEIST_AVERAGE
					,Forms.GOURGEIST_SMALL 
					,Forms.GOURGEIST_LARGE 
					,Forms.GOURGEIST_SUPER }[(int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4))];
			}
			else if (pokemon == Pokemons.DEOXYS_ATTACK)
			{
				return Forms.DEOXYS_ATTACK;
			}
			else if (pokemon == Pokemons.DEOXYS_DEFENSE)
			{
				return Forms.DEOXYS_DEFENSE;
			}
			else if (pokemon == Pokemons.DEOXYS_SPEED)
			{
				return Forms.DEOXYS_SPEED;
			}
			else if (pokemon == Pokemons.KYUREM_BLACK)
			{
				return Forms.KYUREM_BLACK;
			}
			else if (pokemon == Pokemons.KYUREM_WHITE)
			{
				return Forms.KYUREM_WHITE;
			}
			else if (pokemon == Pokemons.ROTOM_FAN)
			{
				return Forms.ROTOM_FAN;
			}
			else if (pokemon == Pokemons.ROTOM_HEAT)
			{
				return Forms.ROTOM_HEAT;
			}
			else if (pokemon == Pokemons.ROTOM_FROST)
			{
				return Forms.ROTOM_FROST;
			}
			else if (pokemon == Pokemons.ROTOM_MOW)
			{
				return Forms.ROTOM_MOW;
			}
			else if (pokemon == Pokemons.ROTOM_WASH)
			{
				return Forms.ROTOM_WASH;
			}
			//ToDo: Pikachu
			else return nullable? (Forms?)null : (Forms)pokemon;//.pokemon.FormId;
		}
	}
}
//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
//using System.Data;
//using System.Data.Common;
//using Mono.Data.Sqlite;
//using System.Data.SQLite;
//using DataEnums;

[System.Serializable]
[System.Obsolete("Use `Pokemon.cs` in `/Scripts2` folder...")]
public class PokemonOld {

	/// <summary>
	/// Set value using Pokemon's <see cref="PokemonDataOld.ID"/>
	/// <seealso cref="PokemonDataOld.ID"/>
	/// </summary>
	private int pokemonID;
	private string nickname;
	public enum Status{
		NONE,
		BURNED,
		FROZEN,
		PARALYZED,
		POISONED,
		ASLEEP,
		FAINTED
	}
	public enum Gender{
		NONE,
		MALE,
		FEMALE,
		CALCULATE
	}
	
	/// <summary>
	///	Used only for a few pokemon to specify what form it's in. 
	/// <seealso cref="PokemonDataOld.Form"/>
	/// <returns>by default is null/0</returns>
	/// </summary>
	/// <remarks>
	///	Unown = letter of the alphabet.
	///	Deoxys = which of the four forms.
	///	Burmy/Wormadam = cloak type. Does not change for Wormadam.
	///	Shellos/Gastrodon = west/east alt colours.
	///	Rotom = different possesed appliance forms.
	///	Giratina = Origin/Altered form.
	///	Shaymin = Land/Sky form.
	///	Arceus = Type.
	///	Basculin = appearance.
	///	Deerling/Sawsbuck = appearance.
	///	Tornadus/Thundurus/Landorus = Incarnate/Therian forms.
	///	Kyurem = Normal/White/Black forms.
	///	Keldeo = Ordinary/Resolute forms.
	///	Meloetta = Aria/Pirouette forms.
	///	Genesect = different Drives.
	///	Vivillon = different Patterns.
	///	Flabebe/Floette/Florges = Flower colour.
	///	Furfrou = haircut.
	///	Pumpkaboo/Gourgeist = small/average/large/super sizes. 
	///	Hoopa = Confined/Unbound forms.
	///	Castform? = different weather forms
	/// </remarks>
	private int form; // select pokemon_id from pokemon_forms where is_battle_only=0 ORDER BY [order] --form_identifier!=null | form_order!=1
						

	private Gender gender;
	private int level;
	/// <summary>
	/// Pokemon experience points
	/// <example>
	/// lv1->lv2=5xp
	/// lv2->lv3=10xp
	/// if pokemon is lvl 3 and 0xp, it should have a total of 15xp
	/// but display counter should still say 0
	/// </example>
	/// </summary>
	/// <remarks>
	/// experience should accumulate accross past levels.
	/// Should also rename to "currentExp"?
	/// </remarks>
	private int exp; 
	/// <summary>
	/// Should be a method
	/// </summary>
	/// <remarks>should perform math equation to access this value</remarks>
	private int nextLevelExp; 

    /// <summary>
    /// This is the samething as "friendship";
    /// Max value of 255 -- i think.
    /// </summary>
	private int happiness; 
    public enum HappinessMethods
    {
        WALKING,
        LEVELUP,
        GROOM,
        FAINT,
        VITAMIN,
        EVBERRY,
        POWDER,
        ENERGYROOT,
        REVIVALHERB
    }

    /// <summary>
    /// 3 Values; Not Infected, Cured, Infected.
    /// </summary>
    /// <remarks>
    /// Can be a enum, for 1 of 3 values
    /// or if null, not infected; 
    /// true infected, and false cured?
    /// </remarks>
	private bool pokerus;
	private bool isShiny;
    /// <summary>
    /// </summary>
    /// <remarks>what is this and why do we need it?</remarks>
	private int rareValue;

	private Status status;
	private int sleepTurns;

	/// <summary>
	/// </summary>
	/// <remarks>This should be an enum value</remarks>
	private string caughtBallString;
    /// <summary>
    /// <seealso cref="ItemDataOld.ItemCategory.STANDARD_BALLS"/>
    /// <seealso cref="ItemDataOld.ItemCategory.SPECIAL_BALLS"/>
    /// </summary>
    /// <remarks>
    /// Does a check against <see cref="ItemDataOld.ItemCategory"/> to confirm if valid
    /// or returns null
    /// </remarks>
    private eItems.Item caughtBall;
	/// <summary>
	/// The held item.
	/// <returns>Int item value</returns>
	/// </summary>
	private eItems.Item heldItem;

	/// <summary>
	/// The met date.
	/// <returns>DateTimeOffset</returns>
	/// </summary>
    /// <remarks>this should be a <see cref="System.DateTimeOffset.UtcDateTime"/></remarks>
	private string metDate;
	/// <summary>
	/// This value should be the unity map value.
	/// The database will convert unity value into string name
	/// </summary>
    /// <remarks>Maybe an {int mapId, int x, int y, int z}?</remarks>
	private string metMap;
	private int metLevel;

	/// <summary>
	/// if OriginalTrainer = null, pokemon can be caught.
	/// </summary>
	/// <remarks>this should be a <see cref="System.Guid"/>, int, or hash (for multiplayer)</remarks>
	private string OT;
	/// <summary>
	/// Pokemon Serial number
	/// <para>only last 6 digits are visible; string value incorporates leading 0s</para>
	/// <value>(last6 of) finalId = trainerid+secretId*65536</value>
	/// </summary>
	/// <remarks>Missing other values, like secretId</remarks>
	private string IDno;

	private int IV_HP;
	private int IV_ATK;
	private int IV_DEF;
	private int IV_SPA;
	private int IV_SPD;
	private int IV_SPE;

	private int EV_HP;
	private int EV_ATK;
	private int EV_DEF;
	private int EV_SPA;
	private int EV_SPD;
	private int EV_SPE;

	/// <summary>
	/// The nature.
	/// </summary>
	/// <remarks>Should be an enum</remarks>
	private string natureString;
	private NatureDatabaseOld.Nature nature;
		
	private int currentHP;
	private int HP; //is this the same as the maxHP.value?
	private int ATK;
	private int DEF;
	private int SPA;
	private int SPD;
	private int SPE;

	/// <summary>
	/// Pokemon abilities 1 and 2 are visible in pokemon's Summary card
    /// the 3rd is hidden but still remains active.
    /// Multiples of same abilities count as 1. 
    /// (Shouldnt contain multiples of same abilities)
    ///	<seealso cref="PokemonDataOld.Abilities"/>
	/// </summary>
	///	<remarks>
	/// Should be [int? a1,int? a2,int? a3]
    /// <code>int?[] ability = new int?[3]</code>
	/// </remarks> 
	private int ability;	// (0/1/2(hiddenability)) if higher than number of abilites, rounds down to nearest ability.
							// if is 2, but pokemon only has 1 ability and no hidden, will use the one ability it does have.

    /// <summary>
    /// deprecated
    /// </summary>
    /// <remarks>This should also be an enum...</remarks>
	private string[] movesetString;
    ///	<remarks>
    /// Should be [eMoves.Move? m1,eMoves.Move? m2,eMoves.Move? m3,eMoves.Move? m4]
    /// <code>eMoves.Move?[] moveset = new eMoves.Move?[4]</code>
    /// or add "eMoves.Move.NONE" as default null value.
    /// </remarks> 
    private eMoves.Move?[] moveset;
	private string [] moveHistoryString;
	private eMoves.Move[] moveHistory;
	private int[] PPups;//Also need log for "RareCandy" and "Vitamins"
	private int[] maxPP;
	private int[] PP;
    

	//New Pokemon with: random IVS, and Shininess 
	//					default moveset, and EVS (0)
	public PokemonOld(int pokemonID, Gender gender, int level, string caughtBall, string heldItem, string OT, int ability){
	}

    #region Methods
    //Recalculate the pokemon's Stats.
    public void calculateStats(){
	}

	//set Nickname
	public void setNickname(string nickname){
		this.nickname = nickname;
	}

    /// <summary>
    /// Deprecated. Please use <see cref="swapHeldItem(eItems.Item)"/>
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns>returns empty string value</returns>
	public string swapHeldItem(string newItem){
        string oldItem = "";// this.heldItem;
		//this.heldItem = newItem;
		return oldItem;
	}

	public eItems.Item swapHeldItem(eItems.Item newItem){
		eItems.Item oldItem = this.heldItem;
		this.heldItem = newItem;
		return oldItem;
	}

	public void addExp(int expAdded){
	}

	public bool addEVs(string stat, float amount){
		int intAmount = Mathf.FloorToInt(amount);
		int evTotal = EV_HP + EV_ATK + EV_DEF + EV_SPA + EV_SPD + EV_SPE;
		if(evTotal < 510){ //if total EV cap is already reached.
			if(evTotal + intAmount > 510){ //if this addition will pass the total EV cap.
				intAmount = 510 - evTotal; //set intAmount to be the remaining points before cap is reached.
			}
			if(stat == "HP"){ //if adding to HP.
				if(EV_HP < 252){ //if HP is not full.
					EV_HP += intAmount;
					if(EV_HP > 252){ //if single stat EV cap is passed.
						EV_HP = 252;} //set stat back to the cap.
					return true;}
			}
			else if(stat == "ATK"){ //if adding to ATK.
				if(EV_ATK < 252){ //if ATK is not full.
					EV_ATK += intAmount;
					if(EV_ATK > 252){ //if single stat EV cap is passed.
						EV_ATK = 252;} //set stat back to the cap.
					return true;}
			}
			else if(stat == "DEF"){ //if adding to DEF.
				if(EV_DEF < 252){ //if DEF is not full.
					EV_DEF += intAmount;
					if(EV_DEF > 252){ //if single stat EV cap is passed.
						EV_DEF = 252;} //set stat back to the cap.
					return true;}
			}
			else if(stat == "SPA"){ //if adding to SPA.
				if(EV_SPA < 252){ //if SPA is not full.
					EV_SPA += intAmount;
					if(EV_SPA > 252){ //if single stat EV cap is passed.
						EV_SPA = 252;} //set stat back to the cap.
					return true;}
			}
			else if(stat == "SPD"){ //if adding to SPD.
				if(EV_SPD < 252){ //if SPD is not full.
					EV_SPD += intAmount;
					if(EV_SPD > 252){ //if single stat EV cap is passed.
						EV_SPD = 252;} //set stat back to the cap.
					return true;}
			}
			else if(stat == "SPE"){ //if adding to SPE.
				if(EV_SPE < 252){ //if SPE is not full.
					EV_SPE += intAmount;
					if(EV_SPE > 252){ //if single stat EV cap is passed.
						EV_SPE = 252;} //set stat back to the cap.
					return true;}
			}
		}
		return false; //returns false if total or relevant EV cap was reached before running.
	}

	public int getEvolutionID(string currentMethod){
		return -1;
	}

	//Check PokemonData.cs for list of evolution method names.
	public bool canEvolve(string currentMethod){
		return false;
	}
	
    /// <summary>
    /// check that the evolution can be 
    /// <para>Deprecated... broke at held item</para>
    /// </summary>
    /// <param name="currentMethod"></param>
    /// <param name="evolutionRequirements"></param>
    /// <returns></returns>
	public bool checkEvolutionMethods(string currentMethod, string evolutionRequirements){
		string[] evolutionSplit = evolutionRequirements.Split(',');
		string[] methods = evolutionSplit[0].Split('\\');
		string[] currentMethodSplit = currentMethod.Split(','); //if currentMethod needs a parameter attached, it will be separated by a ' , '
		string[] parameters = new string[]{};
		if (evolutionSplit.Length > 0){ //if true, there is a parameter attached
			parameters = evolutionSplit[1].Split('\\');
		}
		for(int i = 0; i < methods.Length; i++){ //for every method for the currently checked evolution
			//Debug.Log(evolutionRequirements +" | "+ currentMethodSplit[0] +" "+ methods[i] +" "+ parameters[i]);
			if(methods[i] == "Level"){ //if method contains a Level requirement
				if(currentMethodSplit[0] != "Level"){ //and system is not checking for a level evolution
					return false; //cannot evolve. return false and stop checking.
				}
				else{
					if(this.level < int.Parse(parameters[i])){ //and pokemon's level is not high enough to evolve,
						return false; //cannot evolve. return false stop checking.
					}
				}
			}
			else if(methods[i] == "Stone"){ //if method contains a Stone requirement
				if(currentMethodSplit[0] != "Stone"){ //and system is not checking for a stone evolution
					return false; //cannot evolve. return false and stop checking.
				}
				else{ //if it is checking for a stone evolution,
					if(currentMethodSplit[1] != parameters[i]){ //and parameter being checked does not match the required one
						return false; //cannot evolve. return false and stop checking.
					}
				}
			}
			else if(methods[i] == "Trade"){ //if method contains a Trade requirement
				if(currentMethodSplit[0] != "Trade"){ //and system is not checking for a trade evolution
					return false; //cannot evolve. return false and stop checking.
				}
			}
			else if(methods[i] == "Friendship"){ //if method contains a Friendship requirement
				if(this.happiness < 220){ //and pokemon's friendship is less than 220
					return false; //cannot evolve. return false and stop checking.
				}
			}
			else if(methods[i] == "Item"){ //if method contains an Item requirement
				/*if(this.heldItem == parameters[i]){ //and pokemon's Held Item is not the specified Item
					return false; //cannot evolve. return false and stop checking.
				}*/
			}
			else if(methods[i] == "Gender"){ //if method contains a Gender requirement
				if(this.gender.ToString() != parameters[i]){ //and pokemon's gender is not the required gender to evolve,
					return false; //cannot evolve. return false and stop checking.
				}
			}
			else if(methods[i] == "Move"){ //if method contains a Move requirement
				if(!HasMove(parameters[i])){ //and pokemon does not have the specified move
					return false; //cannot evolve. return false and stop checking.
				}
			}
			else if(methods[i] == "Map"){ //if method contains a Map requirement
				string mapName = PlayerMovement.player.currentMap.name; 
				if(mapName != parameters[i]){ //and current map is not the required map to evolve,
					return false; //cannot evolve. return false and stop checking.
				}
			}
			else if(methods[i] == "Time"){ //if method contains a Time requirement
				string dayNight = "Day";
				if(System.DateTime.Now.Hour >= 21 || System.DateTime.Now.Hour < 4){ //if time is night time
					dayNight = "Night"; //set dayNight to be "Night"
				}
				if(dayNight != parameters[i]){ //if time is not what the evolution requires (Day/Night)
					return false; //cannot evolve. return false and stop checking.
				}
			}
			else{ //if methods[i] did not equal to anything above, methods[i] is an invalid method.
				return false;
			}
		}
		//if the code did not return false once, then the evolution requirements must have been met.
		return true;
	}

	public bool evolve(string currentMethod){
		return false;
	}

	//return a string that contains all of this pokemon's data
	public override string ToString(){
		string result = pokemonID +": "+ this.getName() +"("+ Pokemon.PokemonData.GetPokemon((PokemonUnity.Pokemon.Pokemons)pokemonID).ToString() +"), "+
				gender.ToString() +", Level "+ level +", EXP: "+ exp +", To next: "+ (nextLevelExp - exp) +
				", Friendship: "+ happiness +", RareValue="+ rareValue +", Pokerus="+ pokerus.ToString() +", Shiny="+ isShiny.ToString() +
				", Status: "+  status +", Ball: "+ caughtBallString +", Item: "+ heldItem +
				", met at Level " + metLevel +" on "+ metDate +" at "+ metMap +
				", OT: "+ OT +", ID: "+ IDno +
				", IVs: "+ IV_HP +","+ IV_ATK +","+ IV_DEF +","+ IV_SPA +","+ IV_SPD +","+ IV_SPE +
				", EVs: "+ EV_HP +","+ EV_ATK +","+ EV_DEF +","+ EV_SPA +","+ EV_SPD +","+ EV_SPE +
				", Stats: "+ currentHP +"/"+ HP +","+ ATK +","+ DEF +","+ SPA +","+ SPD +","+ SPE +
				", Nature: "+ natureString +", "+Pokemon.PokemonData.GetPokemon((PokemonUnity.Pokemon.Pokemons)pokemonID).Ability;
		result += ", [";
		for(int i = 0; i < 4; i++){
			if(!string.IsNullOrEmpty(movesetString[i])){
				result += movesetString[i] +": "+PP[i]+"/"+maxPP[i]+", ";
			}
		}
		result = result.Remove(result.Length-2,2);
		result += "]";

		return result;
	}

	//Heal the pokemon
	public void healFull(){
		currentHP = HP;
		PP[0] = maxPP[0];
		PP[1] = maxPP[1];
		PP[2] = maxPP[2];
		PP[3] = maxPP[3];
		status = Status.NONE;
	}

    /// <summary>
	/// returns the excess hp
    /// </summary>
	public int healHP(float amount){
		int excess = 0; 
		int intAmount = Mathf.RoundToInt(amount);
		currentHP += intAmount;
		if(currentHP > HP){
			excess = currentHP - HP; 
			currentHP = HP;
		}
		if(status == Status.FAINTED && currentHP > 0){
			status = Status.NONE;}
		return intAmount-excess;
	}

	public int healPP(int move, float amount){
		int excess = 0; 
		int intAmount = Mathf.RoundToInt(amount);
		this.PP[move] += intAmount;
		if(this.PP[move] > this.maxPP[move]){
			excess = this.PP[move] - this.maxPP[move]; 
			this.PP[move] = this.maxPP[move];
		}
		return intAmount-excess;
	}

	public void healStatus(){
		status = Status.NONE;
	}

	public void removeHP(float amount){
		int intAmount = Mathf.RoundToInt(amount);
		this.currentHP -= intAmount;
		if(this.currentHP <= 0){
			this.currentHP = 0;
			this.status = Status.FAINTED;
		}
	}


	public void removePP(string move, float amount){
		removePP(getMoveIndex(move), amount);}
	public void removePP(int move, float amount){
		if(move >= 0){
			int intAmount = Mathf.RoundToInt(amount);
			this.PP[move] -= intAmount;
			if(this.PP[move] < 0){
				this.PP[move] = 0;
			}
		}
	}
    

	public bool setStatus(Status status){
		if(this.status == Status.NONE){
			this.status = status;
			if(status == Status.ASLEEP){ //if applying sleep, set sleeping 
				sleepTurns = Random.Range(1,4);} //turns to 1, 2, or 3
			return true;
		}
		else{
			if(status == Status.NONE || status == Status.FAINTED){
				this.status = status;
				sleepTurns = 0;
				return true;
			}
		}
		return false;
	}


	public void removeSleepTurn(){
		if(status == Status.ASLEEP){
			sleepTurns -= 1;
			if(sleepTurns <= 0){
				setStatus(Status.NONE);}
		}
	}
	public int getSleepTurns(){
		return sleepTurns;}


	public string getFirstFEInstance(string moveName){
		for(int i = 0; i < movesetString.Length; i++){
			if(MoveDatabase.getMove(movesetString[i]).getFieldEffect() == moveName){
				return movesetString[i];
			}
		}
		return null;
	}

	public int getID(){
		return pokemonID;}

	public string getLongID(){
		string result = pokemonID.ToString();
		while(result.Length < 3){
			result = "0" + result;}
		return result;}
	
	public static string convertLongID(int ID){
		string result = ID.ToString();
		//while(result.Length < 3){result = "0" + result;}
		return result.PadLeft(3,'0');}

	//Get the pokemon's nickname, or regular name if it has none.
	public string getName(){
		if (string.IsNullOrEmpty(nickname)){
			return Pokemon.PokemonData.GetPokemon((PokemonUnity.Pokemon.Pokemons)pokemonID).Name;
		}
		else{
			return nickname;
		}
	}

	public Gender getGender(){
		return gender;}

	public int getLevel(){
		return level;}
	public int getExp(){
		return exp;}
	public int getExpNext(){
		return nextLevelExp;}

	public int getFriendship(){
		return happiness;}
	
	public bool getPokerus(){
		return pokerus;}
	public int getRareValue(){
		return rareValue;}
	public bool getIsShiny(){
		return isShiny;}
	
	public Status getStatus(){
		return status;}
	
	public string getCaughtBall(){
		return caughtBallString;}
    /// <summary>
    /// Deprecated...
    /// </summary>
    /// <returns>Returns an <see cref="eItems.Item"/></returns>
    /// <remarks>Needs to be corrected...</remarks>
    public string getHeldItem(){
		return heldItem.ToString();}
	
	public string getMetDate(){
		return metDate;}
	public string getMetMap(){
		return metMap;}
	public int getMetLevel(){
		return metLevel;}
	
	public string getOT(){
		return OT;}
    /// <summary>
    /// Last six of trainer's hashId is pokemon's Id
    /// </summary>
    /// <returns>returns pokemons 6 digit ID number</returns>
	public string getIDno(){ 
		return IDno;}
	
	public int GetIV(int index){
		if(index == 0){ return IV_HP; }
		else if(index == 1){ return IV_ATK; }
		else if(index == 2){ return IV_DEF; }
		else if(index == 3){ return IV_SPA; }
		else if(index == 4){ return IV_SPD; }
		else if(index == 5){ return IV_SPE; }
		return -1;
	}
	public int getIV_HP(){
		return IV_HP;}
	public int getIV_ATK(){
		return IV_ATK;}
	public int getIV_DEF(){
		return IV_DEF;}
	public int getIV_SPA(){
		return IV_SPA;}
	public int getIV_SPD(){
		return IV_SPD;}
	public int getIV_SPE(){
		return IV_SPE;}

	public int GetHighestIV(){
		int highestIVIndex = 0;
		int highestIV = IV_HP;
		//by default HP is highest. Check if others are higher. Use RareValue to consistantly break a tie
		if(IV_ATK > highestIV || (IV_ATK == highestIV && rareValue > 10922)){ highestIVIndex = 1; highestIV = IV_ATK; }
		if(IV_DEF > highestIV || (IV_DEF == highestIV && rareValue > 21844)){ highestIVIndex = 2; highestIV = IV_DEF; }
		if(IV_SPA > highestIV || (IV_SPA == highestIV && rareValue > 32766)){ highestIVIndex = 3; highestIV = IV_SPA; }
		if(IV_SPD > highestIV || (IV_SPD == highestIV && rareValue > 43688)){ highestIVIndex = 4; highestIV = IV_SPD; }
		if(IV_SPE > highestIV || (IV_SPE == highestIV && rareValue > 54610)){ highestIVIndex = 5; highestIV = IV_SPE; }
		return highestIVIndex;
	}
	
	public int getEV_HP(){
		return EV_HP;}
	public int getEV_ATK(){
		return EV_ATK;}
	public int getEV_DEF(){
		return EV_DEF;}
	public int getEV_SPA(){
		return EV_SPA;}
	public int getEV_SPD(){
		return EV_SPD;}
	public int getEV_SPE(){
		return EV_SPE;}
	
	public string getNature(){
		return natureString;}

	public int getHP(){
        //changed Return types must be float. And then rounded up to int when displaying
	    //public float TotalHP(){
		//return (Pokemon_BaseStats.Health((Pokemon_Names)number)+50)*level/50 + 10;
		return HP;}
	public int getCurrentHP(){
		return currentHP;}
	public float getPercentHP(){
		return 1f-(((float)HP-(float)currentHP)/(float)HP);}
	public int getATK(){
		return ATK;}
	public int getDEF(){
		return DEF;}
	public int getSPA(){
		return SPA;}
	public int getSPD(){
		return SPD;}
	public int getSPE(){
		return SPE;}

	public int getAbility(){
		return ability;}


	public int getMoveIndex(string move){
		for(int i = 0; i < movesetString.Length; i++){
			if(!string.IsNullOrEmpty(movesetString[i])){
				if(movesetString[i] == move){
					return i;}
			}
		}
		return -1;
	}

	public string[] getMoveset(){
		string[] result = new string[4];
		for(int i = 0; i < 4; i++){
			result[i] = movesetString[i];
		}
		return result;}

	public void swapMoves(int target1, int target2){
		string temp = movesetString[target1];
		this.movesetString[target1] = movesetString[target2];
		this.movesetString[target2] = temp;
	}

	private void ResetPP(int index){
		PPups[index] = 0;
		maxPP[index] = Mathf.FloorToInt(MoveDatabase.getMove(movesetString[index]).getPP()*((PPups[index]*0.2f)+1));
		PP[index] = maxPP[index];
	}

	/// Returns false if no room to add the new move OR move already is learned.
	public bool addMove(string newMove){
		if(!HasMove(newMove) && string.IsNullOrEmpty(movesetString[3])){
			movesetString[3] = newMove;
			ResetPP(3);
			packMoveset();
			return true;}
		return false;
	}

	public void replaceMove(int index, string newMove){
		if(index >= 0 && index < 4){
			movesetString[index] = newMove;
			addMoveToHistory(newMove);
			ResetPP(index);}
	}

	/// Returns false if only one move is left in the moveset.
	public bool forgetMove(int index){
		if(getMoveCount() > 1){
			movesetString[index] = null;
			packMoveset();
			return true;}
		return false;
	}

	public int getMoveCount(){
		int count = 0;
		for(int i = 0; i < 4; i++){
			if(!string.IsNullOrEmpty(movesetString[i])){
				count += 1;}
		}
		return count;
	}

	private void packMoveset(){
		string[] packedMoveset = new string[4];
		int[] packedPP = new int[4];
		int[] packedMaxPP = new int[4];
		int[] packedPPups = new int[4];

		int i2 = 0; //counter for packed array
		for(int i = 0; i < 4; i++){
			if(!string.IsNullOrEmpty(movesetString[i])){ //if next move in moveset is not null
				packedMoveset[i2] = movesetString[i]; //add to packed moveset
				packedPP[i2] = PP[i];
				packedMaxPP[i2] = maxPP[i];
				packedPPups[i2] = PPups[i];
				i2 += 1;}				//ready packed moveset's next position
		}
		movesetString = packedMoveset;
		PP = packedPP;
		maxPP = packedMaxPP;
		PPups = packedPPups;
	}

	private void addMoveToHistory(string move){
		if(!HasMoveInHistory(move)){
			string[] newHistory = new string[moveHistoryString.Length+1];
			for(int i = 0; i < moveHistoryString.Length; i++){
				newHistory[i] = moveHistoryString[i];}
			newHistory[moveHistoryString.Length] = move;
			moveHistoryString = newHistory;
		}
	}

	public bool HasMove(string move){
		if(string.IsNullOrEmpty(move)){ return false; }
		for(int i = 0; i < movesetString.Length; i++){
			if(movesetString[i] == move){
				return true;}
		}
		return false;
	}

	public bool HasMoveInHistory(string move){
		for(int i = 0; i < movesetString.Length; i++){
			if(movesetString[i] == move){
				return true;}
		}
		return false;
	}

	public bool CanLearnMove(string move){
		return false;
	}

	public string MoveLearnedAtLevel(int level){
		return null;
	}


	public int[] getPPups(){
		return PPups;}
	public int[] getMaxPP(){
		return maxPP;}
	public int[] getPP(){
		return PP;}
	public int getPP(int index){
		return PP[index];}


	public Sprite[] GetFrontAnim_(){
		return GetAnimFromID_("PokemonSprites", pokemonID, gender, isShiny);}
	
	public Sprite[] GetBackAnim_(){
		return GetAnimFromID_("PokemonBackSprites", pokemonID, gender, isShiny);}
	
	public static Sprite[] GetFrontAnimFromID_(int ID, Gender gender, bool isShiny){
		return GetAnimFromID_("PokemonSprites", ID, gender, isShiny);}
	
	public static Sprite[] GetBackAnimFromID_(int ID, Gender gender, bool isShiny){
		return GetAnimFromID_("PokemonBackSprites", ID, gender, isShiny);}
	
	private static Sprite[] GetAnimFromID_(string folder, int ID, Gender gender, bool isShiny){
		Sprite[] animation;
		string shiny = (isShiny)? "s" : "";
		if(gender == Gender.FEMALE){
			//Attempt to load Female Variant
			animation = Resources.LoadAll<Sprite>(folder+"/"+convertLongID(ID)+"f"+shiny+"/");
			if(animation.Length == 0){
				Debug.LogWarning("Female Variant NOT Found");
				//Attempt to load Base Variant (possibly Shiny)
				animation = Resources.LoadAll<Sprite>(folder+"/"+convertLongID(ID)+shiny+"/");}
		//	else{ Debug.Log("Female Variant Found"); }
		}
		else{
			//Attempt to load Base Variant (possibly Shiny)
			animation = Resources.LoadAll<Sprite>(folder+"/"+convertLongID(ID)+shiny+"/");}
		if(animation.Length == 0 && isShiny){
			Debug.LogWarning("Shiny Variant NOT Found");
			//No Shiny Variant exists, Attempt to load Regular Variant
			animation = Resources.LoadAll<Sprite>(folder+"/"+convertLongID(ID)+"/");}
		return animation;
	}

	public Sprite[] GetIcons_(){
		return GetIconsFromID_(pokemonID, isShiny);}

	public static Sprite[] GetIconsFromID_(int ID, bool isShiny){
		string shiny = (isShiny)? "s" : "";
		Sprite[] icons = Resources.LoadAll<Sprite>("PokemonIcons/icon"+convertLongID(ID)+shiny);
		if(icons == null){
			Debug.LogWarning("Shiny Variant NOT Found");
			icons = Resources.LoadAll<Sprite>("PokemonIcons/icon"+convertLongID(ID));}
		return icons;
	}

	public float GetCryPitch(){
		return (true)? 0.9f : 1f-(0.06f*(1-getPercentHP()));}

	public AudioClip GetCry(){
		return GetCryFromID(pokemonID);}

	public static AudioClip GetCryFromID(int ID){
		return Resources.Load<AudioClip>("Audio/cry/"+convertLongID(ID));}

	public Texture[] GetFrontAnim(){
		return GetAnimFromID("PokemonSprites", pokemonID, gender, isShiny);}

	public Texture[] GetBackAnim(){
		return GetAnimFromID("PokemonBackSprites", pokemonID, gender, isShiny);}

	public Texture GetIcons(){
		return GetIconsFromID(pokemonID, isShiny);}

	public Sprite[] GetSprite(bool getLight){
		return GetSpriteFromID(pokemonID, isShiny, getLight);}

	private static Texture[] GetAnimFromID(string folder, int ID, Gender gender, bool isShiny){
		Texture[] animation;
		string shiny = (isShiny)? "s" : "";
		if(gender == Gender.FEMALE){
			//Attempt to load Female Variant
			animation = Resources.LoadAll<Texture>(folder+"/"+convertLongID(ID)+"f"+shiny+"/");
			if(animation.Length == 0){
				Debug.LogWarning("Female Variant NOT Found (may not be required)");
				//Attempt to load Base Variant (possibly Shiny)
				animation = Resources.LoadAll<Texture>(folder+"/"+convertLongID(ID)+shiny+"/");}
		//	else{ Debug.Log("Female Variant Found");}
		}
		else{
			//Attempt to load Base Variant (possibly Shiny)
			animation = Resources.LoadAll<Texture>(folder+"/"+convertLongID(ID)+shiny+"/");}
		if(animation.Length == 0 && isShiny){
			Debug.LogWarning("Shiny Variant NOT Found");
			//No Shiny Variant exists, Attempt to load Regular Variant
			animation = Resources.LoadAll<Texture>(folder+"/"+convertLongID(ID)+"/");}
		return animation;
	}

	public static Texture GetIconsFromID(int ID, bool isShiny){
		string shiny = (isShiny)? "s" : "";
		Texture icons = Resources.Load<Texture>("PokemonIcons/icon"+convertLongID(ID)+shiny);
		if(icons == null){
			Debug.LogWarning("Shiny Variant NOT Found");
			icons = Resources.Load<Texture>("PokemonIcons/icon"+convertLongID(ID));}
		return icons;
	}
    
	public static Sprite[] GetSpriteFromID(int ID, bool isShiny, bool getLight){
		string shiny = (isShiny)? "s" : "";
		string light = (getLight)? "Lights/" : "";
		Sprite[] spriteSheet = Resources.LoadAll<Sprite>("OverworldPokemonSprites/"+light+convertLongID(ID)+shiny);
		if(spriteSheet.Length == 0){
			//No Light found AND/OR No Shiny found, load non-shiny
			if(isShiny){ 
				if(getLight){ Debug.LogWarning("Shiny Light NOT Found (may not be required)"); }
				else{ Debug.LogWarning("Shiny Variant NOT Found"); } 
			}
			spriteSheet = Resources.LoadAll<Sprite>("OverworldPokemonSprites/"+light+convertLongID(ID));
		}
		if(spriteSheet.Length == 0){
			//No Light found OR No Sprite found, return 8 blank sprites
			if(!getLight){ Debug.LogWarning("Sprite NOT Found"); }
			else{ Debug.LogWarning("Light NOT Found (may not be required)"); }
			return new Sprite[8];
		}
		return spriteSheet;
	}
    #endregion
}
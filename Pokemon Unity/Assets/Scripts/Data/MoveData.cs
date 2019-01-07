//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MoveDataOld {

    /// <summary>
    /// comments in brackets are the float parameters
    /// </summary>
	public enum Effect{ 
        /// <summary>
        /// if this check fails, no later effects will run
        /// </summary>
		Chance, 
        /// <summary>
        /// for the very specific move effects
        /// </summary>
		Unique, 
		Sound, Punch, Powder,
        /// <summary>
        /// (only works on 0= opposite, 1= opposite or genderless)
        /// </summary>
        Gender, 
		Burn, Freeze, Paralyze, Poison, Sleep, Toxic,
        /// <summary>
        /// (chance)
        /// </summary>
		ATK,     DEF,     SPA,     SPD,     SPE,     ACC,     EVA,
		ATKself, DEFself, SPAself, SPDself, SPEself, ACCself, EVAself,
        /// <summary>
        /// (x=set, 0=2-5)
        /// </summary>
		xHits,
        /// <summary>
        /// (amount)
        /// </summary>
        Heal,
        /// <summary>
        /// (amount)
        /// </summary>
        HPDrain,
        /// <summary>
        /// (x=set, 0=level)
        /// </summary>
        SetDamage,     
        /// <summary>
        /// based off of user's maxHp
        /// </summary>
		Critical, Recoil,
        Flinch, RecoilMax //Added these 2
	};

	public enum Category{
		PHYSICAL,
		SPECIAL,
		STATUS
	};
	public enum Contest{
		COOL,
		BEAUTIFUL,
		CUTE,
		CLEVER,
		TOUGH
	}
	public enum Target{
		SELF,
		ADJACENT,
		ANY,
		ADJACENTALLY,
		ADJACENTOPPONENT,
		ADJACENTALLYSELF,
		ALL,
		ALLADJACENT,
		ALLADJACENTOPPONENT,
		ALLOPPONENT,
		ALLALLY
	}

	private string name;

	//private PokemonDataOld.Type type { get; set; }
	private Category category;
	private int power;
	private float accuracy;
	private int PP;
	private Target target;
	private int priority; 
	private bool contact;
	private bool protectable;
	private bool magicCoatable;
	private bool snatchable;
	private Effect[] moveEffects;
	private float[] moveParameters;
	private Contest contest;
	private int appeal;
	private int jamming;
	private string description;
	private string fieldEffect;
	

	public string getName(){
		return name;}

	//public PokemonDataOld.Type getType(){
	//	return type;
	//}

	public Category getCategory(){
		return category;}

	public int getPower(){
		return power;}

	public float getAccuracy(){
		return accuracy;}
	
	public int getPP(){
		return PP;}

	public Target getTarget(){
		return target;}

	public int getPriority(){
		return priority;}

	public bool getContact(){
		return contact;}
	
	public bool getProtectable(){
		return protectable;}

	public bool getMagicCoatable(){
		return magicCoatable;}

	public bool getSnatchable(){
		return snatchable;}

	public Effect[] getMoveEffects(){
		return moveEffects;}

	public float[] getMoveParameters(){
		return moveParameters;}
	
	public Contest getContest(){
		return contest;}
	
	public int getAppeal(){
		return appeal;}
	
	public int getJamming(){
		return jamming;}

	public string getDescription(){
		return description;}

	public string getFieldEffect(){
		return fieldEffect;}


	
	public bool hasMoveEffect(Effect effect){
		for(int i = 0; i < moveEffects.Length; i++){
			if(moveEffects[i] == effect){
				return true;}
		}
		return false;
	}
	
	public float getMoveParameter(Effect effect){
		for(int i = 0; i < moveEffects.Length; i++){
			if(moveParameters.Length > i){
				if(moveEffects[i] == effect){
					return moveParameters[i];}
			}
		}
		return 0f;
	}
}

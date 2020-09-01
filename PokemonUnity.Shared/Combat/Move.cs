using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Overworld;

namespace PokemonUnity.Combat
{
	/// <summary>
	/// A Move placeholder class to be used while in-Battle, 
	/// to prevent temp changes from being permanent to original pokemon profile
	/// </summary>
	// ToDo: Rename to `MoveFactory`
	public abstract class Move : IPokeBattle_Move
	{
		#region Variables
		public Attack.Move thismove			{ get; protected set; }
		public Attack.Category Category		{ get; set; }
		public Moves MoveId					{ get; set; }
		public Attack.Target Targets		{ get; set; }
		public Attack.Data.Targets Target	{ get; set; }
		public Types Type					{ get; set; }
		public Attack.MoveFlags Flag		{ get; set; }
		public Attack.Data.Flag Flags		{ get; set; }
		public byte PP						{ get; set; }
		internal int totalpp				{ get; set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0.
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur. 
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
		/// </summary>
		public virtual int AddlEffect		{ get; set; }
		public Attack.Data.Effects Effect	{ get; set; }
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int Accuracy					{ get; set; }
		public int BaseDamage				{ get; set; }
		public int CritRatio				{ get; set; }
		public int Priority					{ get; set; }
		public bool IsPhysical				{ get; set; }// { return Category == Attack.Category.PHYSICAL; } }
		public bool IsSpecial				{ get; set; }// { return Category == Attack.Category.SPECIAL; } }
		public virtual bool unuseableInGravity	{ get; set; }
		public bool PowerBoost				{ get; set; }
		//public bool pbIsStatus()			{ return false; }
		public string Name					{ get { return Game.MoveData[MoveId].Name; } }
		public string EffectString			{ get; set; }
		public string Nothing = "But nothing happened!";
		//public Battle Battle				{ get { return this.battle ?? Game.battle; } }
		internal Battle Battle				{ get; set; }
		#endregion

		public Move() { }

		public virtual Move Initialize(Battle battle, PokemonUnity.Attack.Move move) 
		{
			if (move == null) move = new Attack.Move(Moves.NONE);
			Attack.Data.MoveData movedata    = Game.MoveData[move.MoveId];
			Battle			= battle;
			BaseDamage		= movedata.Power ?? 0; //.BaseDamage;
			Type			= movedata.Type;
			Accuracy		= movedata.Accuracy ?? 0;
			Effect			= movedata.Effect;
			Target			= movedata.Target;
			Priority		= movedata.Priority;
			Flags			= movedata.Flags;
			Category		= movedata.Category;
			thismove		= move;
			//name			= ""
			MoveId			= move.MoveId;
			//PP			= base.PP;
			//TotalPP		= base.TotalPP;
			PP				= move.PP;
			totalpp			= move.TotalPP;
			PowerBoost		= false;

			return this;
		}

		/// <summary>
		/// This is the code actually used to generate a PokeBattle_Move object.  The
		/// object generated is a subclass of this one which depends on the move's
		/// function code (found in the script section PokeBattle_MoveEffect).
		/// </summary>
		/// <param name="battle"></param>
		/// <param name="move"></param>
		/// <returns></returns>
		public IPokeBattle_Move pbFromPBMove(Battle battle, Attack.Move move)
		{
			if (move == null) move = new Attack.Move(Moves.NONE);
			//Attack.Data.MoveData movedata = Game.MoveData[move.MoveId];
			//Type className = Type.GetType(string.Format("PokeBattle_Move_{0}X", movedata.Effect));
			////if Object.const_defined(className)
			//	//return (className).new (battle, move);
			//	return Activitor.CreateInstance(className, battle, move);
			if (Game.MoveEffectData.ContainsKey(move.Effect))
				return Game.MoveEffectData[move.Effect].Initialize(battle, move);
			else
				return new PokeBattle_UnimplementedMove().Initialize(battle, move);
		}

#region About the move
public virtual int TotalPP { get {
   if (totalpp>0) return totalpp; //totalpp != null && 
   if (Game.MoveData.ContainsKey(MoveId)) return Game.MoveData[MoveId].PP;
	return 0;}
  }

  //public addlEffect
  //  return @addlEffect
  //}
  //
  //public to_int
  //  return MoveId
  //}

  public virtual Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent){
	if (type>=0){
	  if (attacker.hasWorkingAbility(Abilities.NORMALIZE)) //&& hasConst? (Types.NORMAL)
		type=Types.NORMAL;
	  else if (type == Types.NORMAL) {
		if (attacker.hasWorkingAbility(Abilities.AERILATE)) { //&& hasConst? (Types.FLYING)
			type = Types.FLYING;
			PowerBoost = true; }
		else if (attacker.hasWorkingAbility(Abilities.REFRIGERATE)){ //&& hasConst? (Types.ICE)
			type = Types.ICE;
			PowerBoost = true; }
		else if (attacker.hasWorkingAbility(Abilities.PIXILATE)){ //&& hasConst? (Types.FAIRY)
			type = Types.FAIRY;
			PowerBoost = true; }
	  }
	}
	return type;
  }

  public virtual Types pbType(Types type, Pokemon attacker, Pokemon opponent) {
	PowerBoost = false;
	type = pbModifyType(type, attacker, opponent);
	if (type>=0){ //&& hasConst? (Types.ELECTRIC)
		if (Battle.field.IonDeluge && type == Types.NORMAL) {
			type = Types.ELECTRIC;
			PowerBoost = false;
		}
		if (attacker.effects.Electrify) {
			type = Types.ELECTRIC;
			PowerBoost = false;
		}
	}
	return type;
  }

  public virtual bool pbIsPhysical(Types type){
	if (Core.USEMOVECATEGORY)
		return Category == Attack.Category.PHYSICAL;
	else
		return Game.TypeData[type].Category == Attack.Category.PHYSICAL;     
  }

  public virtual bool pbIsSpecial(Types type){
	if (Core.USEMOVECATEGORY)
	  return Category == Attack.Category.SPECIAL;
	else
	  return Game.TypeData[type].Category == Attack.Category.SPECIAL;     
  }

  public virtual bool pbIsStatus{ get{
	return Category == Attack.Category.STATUS;}
  }

  public virtual bool pbIsDamaging() { //get{
	return !pbIsStatus;//}
  }

  public virtual bool pbTargetsMultiple(Pokemon attacker){
	int numtargets = 0;
	if (Targets == Attack.Target.AllOpposing) { 
// TODO: should apply even if partner faints during an attack
	  if (!attacker.pbOpposing1.isFainted())numtargets+=1; 
	  if (!attacker.pbOpposing2.isFainted())numtargets+=1; 
	  return numtargets>1;
   } else if (Targets== Attack.Target.AllNonUsers) {
// TODO: should apply even if partner faints during an attack
	  if (!attacker.pbOpposing1.isFainted())numtargets+=1; 
	  if (!attacker.pbOpposing2.isFainted())numtargets+=1; 
	  if (!attacker.Partner.isFainted())numtargets+=1; 
	  return numtargets>1;
	}
	return false;
  }

  public virtual int pbPriority(Pokemon attacker){
	int ret=Priority;
	return ret;
  }

  public virtual int pbNumHits(Pokemon attacker){
	// Parental Bond goes here (for single target moves only)
	if (attacker.hasWorkingAbility(Abilities.PARENTAL_BOND))
	  if (pbIsDamaging() && !pbTargetsMultiple(attacker) &&
		!pbIsMultiHit() && !pbTwoTurnAttack(attacker)){
		List<Attack.Data.Effects> exceptions= new List<Attack.Data.Effects>(){
			Attack.Data.Effects.x0BE,	// Endeavor
			Attack.Data.Effects.x008,	// Selfdestruct/Explosion
			Attack.Data.Effects.x141,	// Final Gambit
			Attack.Data.Effects.x0EA	// Fling
		};
		if (!exceptions.Contains(Effect)){
		   attacker.effects.ParentalBond= 3;
		   return 2;
		}
	   }
	if (pbIsMultiHit()) 
		Core.Rand.Next(Game.MoveMetaData[MoveId].MinHits.Value, Game.MoveMetaData[MoveId].MaxHits.Value);
	 //ToDo: Need to record that Parental Bond applies, to weaken the second attack
	 //ParentalBond = true;
	return 1;
   }

		/// <summary>
		/// not the same as pbNumHits>1
		/// </summary>
   public virtual bool pbIsMultiHit() { //get {   
	return (!Game.MoveMetaData[MoveId].MinHits.HasValue || 
		 !Game.MoveMetaData[MoveId].MaxHits.HasValue);//}
   }

   public virtual bool pbTwoTurnAttack(Pokemon attacker){
	 return false;
   }

   public virtual void pbAdditionalEffect(Pokemon attacker, Pokemon opponent){ }
 
   public virtual bool pbCanUseWhileAsleep() { //get {
	return false;//}
   }

   public virtual bool isHealingMove() { //get {
	 //return Flags.Heal;//}
	 return Game.MoveMetaData[MoveId].Healing > 0;//}
   }

   public virtual bool isRecoilMove() { //get {
	 return false;//}
   }

   public virtual bool UnusableInGravity() { //get; set;// {
	 return false;
   }
				
		/// <summary>
		///# flag h: Has high critical hit rate
		/// </summary>
  public virtual bool hasHighCriticalRate { get{
	//return (@flags&0x80)!=0;} //# flag h: Has high critical hit rate
	return Game.MoveMetaData[MoveId].CritRate > 0;} 
  }		

		/// <summary>
		/// Causes perfect accuracy and double damage
		/// </summary>
  public virtual bool tramplesMinimize (int param=1){ 
	if (!Core.USENEWBATTLEMECHANICS) return false;
	return MoveId == Moves.BODY_SLAM ||
		   MoveId == Moves.FLYING_PRESS ||
		   MoveId == Moves.PHANTOM_FORCE;
	return true;
  }

  public virtual bool successCheckPerHit() { //get{
	return false;//}
  }

  public virtual bool ignoresSubstitute (Pokemon attacker){
	if (Core.USENEWBATTLEMECHANICS){
	  if (Flags.SoundBased) return true; 
	  if (attacker != null && attacker.hasWorkingAbility(Abilities.INFILTRATOR)) return true; 
	}
	return false;
  }
#endregion

#region This move's type effectiveness
  public virtual bool pbTypeImmunityByAbility(Types type, Pokemon attacker, Pokemon opponent){
	if (attacker.Index==opponent.Index)return false; 
	if (attacker.hasMoldBreaker())return false; 
	if (opponent.hasWorkingAbility(Abilities.SAP_SIPPER) && type == Types.GRASS){
	  GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s Sap Sipper (made #{Game.MoveData[MoveId].Name} ineffective)");
	  if (opponent.pbCanIncreaseStatStage(Stats.ATTACK, opponent))
		 opponent.pbIncreaseStatWithCause(Stats.ATTACK,1, opponent, opponent.Ability.ToString(TextScripts.Name));
	  else
		Battle.pbDisplay(Game._INTL("{1}'s {2} made {3} ineffective!",
		   opponent.ToString(),opponent.Ability.ToString(TextScripts.Name),Game.MoveData[MoveId].Name));
	  return true;
	}
	if ((opponent.hasWorkingAbility(Abilities.STORM_DRAIN) && type == Types.WATER) ||
	   (opponent.hasWorkingAbility(Abilities.LIGHTNING_ROD) && type == Types.ELECTRIC)){
	  GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s #{opponent.Ability.ToString(TextScripts.Name)} (made #{Game.MoveData[MoveId].Name} ineffective)");
	  if (opponent.pbCanIncreaseStatStage(Stats.SPATK, opponent))
		 opponent.pbIncreaseStatWithCause(Stats.SPATK,1, opponent, opponent.Ability.ToString(TextScripts.Name));
	  else
		Battle.pbDisplay(Game._INTL("{1}'s {2} made {3} ineffective!",
		   opponent.ToString(),opponent.Ability.ToString(TextScripts.Name),Game.MoveData[MoveId].Name));
	  return true;
	}
	if (opponent.hasWorkingAbility(Abilities.MOTOR_DRIVE) && type == Types.ELECTRIC){
	  GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s Motor Drive (made #{Game.MoveData[MoveId].Name} ineffective)");
	  if (opponent.pbCanIncreaseStatStage (Stats.SPEED, opponent))
		 opponent.pbIncreaseStatWithCause(Stats.SPEED,1, opponent, opponent.Ability.ToString(TextScripts.Name));
	  else
		Battle.pbDisplay(Game._INTL("{1}'s {2} made {3} ineffective!",
		   opponent.ToString(),opponent.Ability.ToString(TextScripts.Name),Game.MoveData[MoveId].Name));
	  return true;
	}
	if ((opponent.hasWorkingAbility(Abilities.DRY_SKIN) && type == Types.WATER) ||
	   (opponent.hasWorkingAbility(Abilities.VOLT_ABSORB) && type == Types.ELECTRIC) ||
	   (opponent.hasWorkingAbility(Abilities.WATER_ABSORB) && type == Types.WATER)){
	  GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s #{opponent.Ability.ToString(TextScripts.Name)} (made #{@Name} ineffective)");
	  if (opponent.effects.HealBlock==0){
		if (opponent.RecoverHP((int)Math.Floor(opponent.TotalHP/4d),true)>0)
		  Battle.pbDisplay(Game._INTL("{1}'s {2} restored its HP!",
			 opponent.ToString(),opponent.Ability.ToString(TextScripts.Name)));
		else
		  Battle.pbDisplay(Game._INTL("{1}'s {2} made {3} useless!",
			 opponent.ToString(),opponent.Ability.ToString(TextScripts.Name),Game.MoveData[MoveId].Name));
		return true;
	  }
	}
	if (opponent.hasWorkingAbility(Abilities.FLASH_FIRE) && type == Types.FIRE){
	  GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s Flash Fire (made #{@Name} ineffective)");
	  if (!opponent.effects.FlashFire){
		opponent.effects.FlashFire= true;
		Battle.pbDisplay(Game._INTL("{1}'s {2} raised its Fire power!",
		   opponent.ToString(), opponent.Ability.ToString(TextScripts.Name)));}
	  else
		Battle.pbDisplay(Game._INTL("{1}'s {2} made {3} ineffective!",
		   opponent.ToString(),opponent.Ability.ToString(TextScripts.Name),Game.MoveData[MoveId].Name));
	  return true;
	}
	if (opponent.hasWorkingAbility(Abilities.TELEPATHY) && pbIsDamaging() &&
	   !opponent.IsOpposing(attacker.Index)){
	   GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s Telepathy (made #{@Name} ineffective)");
	  Battle.pbDisplay(Game._INTL("{1} avoids attacks by its ally Pokémon!",opponent.ToString()));
	  return true;
	}
	if (opponent.hasWorkingAbility(Abilities.BULLETPROOF) && Flags.Ballistics){
	  GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s Bulletproof (made #{@Name} ineffective)");
	  Battle.pbDisplay(Game._INTL("{1}'s {2} made {3} ineffective!",
		 opponent.ToString(),opponent.Ability.ToString(TextScripts.Name),Game.MoveData[MoveId].Name));
	  return true;
	}
	return false;
  }

  public virtual float pbTypeModifier(Types type, Pokemon attacker, Pokemon opponent){
	if (type<0) return 8; 
	if (opponent.hasType(Types.FLYING) && type == Types.GROUND && 
				opponent.hasWorkingItem(Items.IRON_BALL) && !Core.USENEWBATTLEMECHANICS) return 8; 
	Types atype = type; //# attack type
	Types otype1= opponent.Type1;
	Types otype2= opponent.Type2;
	Types otype3= opponent.effects.Type3;// || -1;
	// Roost
	if (otype1 == Types.FLYING && opponent.effects.Roost){//
	  if (otype2 == Types.FLYING && otype3 == Types.FLYING)
		otype1=Types.NORMAL;//getConst(Types.NORMAL) || 0
	  else
		otype1=otype2;
	}
	if (otype2 == Types.FLYING && opponent.effects.Roost){//
		otype2 = otype1;
	}
// Get effectivenesses
	float mod1 = atype.GetEffectiveness(otype1); 
	float mod2=(otype1==otype2) ? 2 : atype.GetEffectiveness(otype2);
	float mod3 = (otype3 < 0 || otype1 == otype3 || otype2 == otype3) ? 2 : atype.GetEffectiveness(otype3);
	if (opponent.hasWorkingItem(Items.RING_TARGET)){
	  if (mod1==0)mod1=2; 
	  if (mod2==0)mod2=2; 
	  if (mod3==0)mod3=2; 
	}
	// Foresight
	if (attacker.hasWorkingAbility(Abilities.SCRAPPY) || opponent.effects.Foresight) { 
	  if (otype1 == Types.GHOST && atype.GetCombinedEffectiveness(otype1) == TypeEffective.Ineffective)mod1 = 2; //
		   if (otype2 == Types.GHOST && atype.GetCombinedEffectiveness(otype2) == TypeEffective.Ineffective)mod2 = 2; //
				if (otype3 == Types.GHOST && atype.GetCombinedEffectiveness(otype3) == TypeEffective.Ineffective)mod3 = 2; //
			}
	// Miracle Eye
	if (opponent.effects.MiracleEye) { 
	  if (otype1 == Types.DARK && atype.GetCombinedEffectiveness(otype1) == TypeEffective.Ineffective)mod1 = 2; //
		   if (otype2 == Types.DARK && atype.GetCombinedEffectiveness(otype2) == TypeEffective.Ineffective)mod2 = 2; //
				if (otype3 == Types.DARK && atype.GetCombinedEffectiveness(otype3) == TypeEffective.Ineffective)mod3 = 2; //
			}
	// Delta Stream's weather
	if (Battle.Weather==Weather.STRONGWINDS) { 
	  if (otype1 == Types.FLYING && atype.GetCombinedEffectiveness(otype1) == TypeEffective.SuperEffective)mod1 = 2; //
		   if (otype2 == Types.FLYING && atype.GetCombinedEffectiveness(otype2) == TypeEffective.SuperEffective)mod2 = 2; //
				if (otype3 == Types.FLYING && atype.GetCombinedEffectiveness(otype3) == TypeEffective.SuperEffective)mod3 = 2; //
			}
	// Smack Down makes Ground moves work against fliers
	if (!opponent.isAirborne(attacker.hasMoldBreaker()) || Effect==Attack.Data.Effects.x120 // Smack Down
	   && atype == Types.GROUND){
	  if (otype1 == Types.FLYING)mod1=2; 
	  if (otype2 == Types.FLYING)mod2=2; 
	  if (otype3 == Types.FLYING)mod3=2;
	}
	if (Effect==Attack.Data.Effects.x17C && !attacker.effects.Electrify){ // Freeze-Dry
	  if (otype1 == Types.WATER)
		mod1 = 4;
	  if (otype2 == Types.WATER)
		mod2=(otype1==otype2) ? 2 : 4;
	  if (otype3 == Types.WATER)
		mod3=(otype1==otype3 || otype2==otype3) ? 2 : 4;
	}
	return mod1* mod2*mod3;
   }

  public virtual double pbTypeModMessages(Types type, Pokemon attacker, Pokemon opponent){
	if (type<0) return 8;
	double typemod=pbTypeModifier(type, attacker, opponent);
	if (typemod==0)
	  Battle.pbDisplay(Game._INTL("It doesn't affect {1}...",opponent.ToString(true)));
	else
	  if (pbTypeImmunityByAbility(type, attacker, opponent)) return 0; 
	return typemod;
  }
  #endregion

#region This move's accuracy check
  public virtual int pbModifyBaseAccuracy(int baseaccuracy, Pokemon attacker, Pokemon opponent){
	return baseaccuracy;
  }

  public virtual bool pbAccuracyCheck(Pokemon attacker, Pokemon opponent){
	int baseaccuracy=Accuracy;
	baseaccuracy = pbModifyBaseAccuracy(baseaccuracy, attacker, opponent);
	if (opponent.effects.Minimize && tramplesMinimize(1)) baseaccuracy=0;
	if (baseaccuracy==0) return true;
	if (attacker.hasWorkingAbility(Abilities.NO_GUARD) ||
	   opponent.hasWorkingAbility(Abilities.NO_GUARD)) return true;            
	if (opponent.hasWorkingAbility(Abilities.STORM_DRAIN)
	   && pbType(this.Type, attacker, opponent) == Types.WATER) 
	  return true;           
	if (opponent.hasWorkingAbility(Abilities.LIGHTNING_ROD) 
	   && pbType(this.Type, attacker, opponent) == Types.ELECTRIC) 
	  return true;          
	if (opponent.effects.Telekinesis>0) return true; 
	// One-hit KO accuracy handled elsewhere
	int accstage=attacker.stages[(int)Stats.ACCURACY];
	if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.UNAWARE)) accstage = 0;
	double accuracy=(accstage>=0) ? (accstage+3)*100.0/3 : 300.0/(3-accstage);
	int evastage=opponent.stages[(int)Stats.EVASION];
	if (Battle.field.Gravity>0) evastage-=2;
	if (evastage<-6) evastage=-6;
	if (evastage>0 && Core.USENEWBATTLEMECHANICS &&
	  attacker.hasWorkingAbility(Abilities.KEEN_EYE)) evastage=0;            
	if (opponent.effects.Foresight || 
	  opponent.effects.MiracleEye ||            
	  Effect==Attack.Data.Effects.x130 || // Chip Away            
	  attacker.hasWorkingAbility(Abilities.UNAWARE)) evastage=0;          
	double evasion=(evastage>=0) ? (evastage+3)*100.0/3 : 300.0/(3-evastage);
	if (attacker.hasWorkingAbility(Abilities.COMPOUND_EYES))
	  accuracy*=1.3;
	if (attacker.hasWorkingAbility(Abilities.HUSTLE) && pbIsDamaging() &&
	   pbIsPhysical(pbType(this.Type, attacker, opponent)))
	  accuracy*=0.8;
	if (attacker.hasWorkingAbility(Abilities.VICTORY_STAR))
	  accuracy*=1.1;
	Pokemon partner = attacker.Partner;
	if (partner.IsNotNullOrNone() && partner.hasWorkingAbility(Abilities.VICTORY_STAR))
	  accuracy*=1.1;
	if (attacker.effects.MicleBerry){
	  attacker.effects.MicleBerry= false;
	  accuracy*=1.2;
	}
	if (attacker.hasWorkingItem(Items.WIDE_LENS))
	  accuracy*=1.1;
	if (attacker.hasWorkingItem(Items.ZOOM_LENS) &&
	   (Battle.choices[opponent.Index].Action != ChoiceAction.UseMove || // Didn't choose a move
	   opponent.hasMovedThisRound())) // Used a move already
	  accuracy*=1.2;
	if (!attacker.hasMoldBreaker()){
	  if (opponent.hasWorkingAbility(Abilities.WONDER_SKIN) && pbIsStatus &&
		 attacker.IsOpposing(opponent.Index))
		 if (accuracy>50) accuracy = 50;
	  if (opponent.hasWorkingAbility(Abilities.TANGLED_FEET) &&
		 opponent.effects.Confusion>0)
		evasion*=1.2;
	  if (opponent.hasWorkingAbility(Abilities.SAND_VEIL) &&
		 Battle.Weather==Weather.SANDSTORM)
		evasion*=1.25;
	  if (opponent.hasWorkingAbility(Abilities.SNOW_CLOAK) &&
		 Battle.Weather==Weather.HAIL)
		evasion*=1.25;
	}
	if (opponent.hasWorkingItem(Items.BRIGHT_POWDER))
	  evasion*=1.1;
	if (opponent.hasWorkingItem(Items.LAX_INCENSE))
	  evasion*=1.1;
	return Battle.pbRandom(100)<(baseaccuracy* accuracy/evasion);
  }
#endregion

#region Damage calculation and modifiers
  public virtual bool pbCritialOverride(Pokemon attacker, Pokemon opponent){
	return false;
  }

  public virtual bool pbIsCritical (Pokemon attacker, Pokemon opponent){
	if (!attacker.hasMoldBreaker())
	  if (opponent.hasWorkingAbility(Abilities.BATTLE_ARMOR) ||
		 opponent.hasWorkingAbility(Abilities.SHELL_ARMOR))
		return false;
	if (opponent.OwnSide.LuckyChant>0) return false;
	if (pbCritialOverride(attacker, opponent)) return true;
	int c=0;
	int[] ratios=(Core.USENEWBATTLEMECHANICS)?new int[] { 16, 8, 2, 1, 1 } : new int[] { 16, 8, 4, 3, 2 };
		c+=attacker.effects.FocusEnergy;
		if (hasHighCriticalRate) c+=1;
	if (attacker.isHyperMode && Type == Types.SHADOW)
	  c+=1;
	if (attacker.hasWorkingAbility(Abilities.SUPER_LUCK)) c+=1;
	if (attacker.hasWorkingItem(Items.STICK) 
	   && attacker.Species == Pokemons.FARFETCHD)
	  c+=2;
	if (attacker.hasWorkingItem(Items.LUCKY_PUNCH) 
	   && attacker.Species == Pokemons.CHANSEY)
	  c+=2;
	if (attacker.hasWorkingItem(Items.RAZOR_CLAW))c+=1;
	if (attacker.hasWorkingItem(Items.SCOPE_LENS))c+=1;
	if (c>4)c=4;
	return Battle.pbRandom(ratios[c])==0;
  }

  public virtual int pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent){
	return basedmg;
  }

  public virtual double pbBaseDamageMultiplier(double damagemult, Pokemon attacker, Pokemon opponent){
	return damagemult;
  }

  public virtual double pbModifyDamage(double damagemult, Pokemon attacker, Pokemon opponent){
	return damagemult;
  }

//ToDo: Do single round at end of calculation and not during...
  public virtual int pbCalcDamage(Pokemon attacker,Pokemon opponent, int options= 0){
	opponent.damagestate.Critical=false;
	opponent.damagestate.TypeMod=0;
	opponent.damagestate.CalcDamage=0;
	opponent.damagestate.HPLost=0;
	if (BaseDamage==0) return 0; 
	List<int> stagemul= new List<int>() { 10,10,10,10,10,10,10,15,20,25,30,35,40 };
	List<int> stagediv= new List<int>() { 40,35,30,25,20,15,10,10,10,10,10,10,10 };
	Types type = Types.NONE;
	if (!Core.NOTYPE) 
	  type=pbType(Type, attacker, opponent);
	else
	  type=Types.NONE; // Will be treated as physical
	if (Core.NOCRITICAL)
	  opponent.damagestate.Critical=pbIsCritical (attacker, opponent);
	#region ##### Calcuate base power of move #####
	int basedmg = BaseDamage; // From PBS file
	basedmg = pbBaseDamage(basedmg, attacker, opponent); // Some function codes alter base power
	double damagemult=0x1000;
	if (attacker.hasWorkingAbility(Abilities.TECHNICIAN) && basedmg<=60 && MoveId>0)
	  damagemult=Math.Round(damagemult*1.5);
	if (attacker.hasWorkingAbility(Abilities.IRON_FIST) && Flags.Punching)
	  damagemult = Math.Round(damagemult * 1.2);
	if (attacker.hasWorkingAbility(Abilities.STRONG_JAW) && Flags.Bite)
	  damagemult = Math.Round(damagemult * 1.5);
	if (attacker.hasWorkingAbility(Abilities.MEGA_LAUNCHER) && Flags.PulseBased)
	  damagemult = Math.Round(damagemult * 1.5);
	if (attacker.hasWorkingAbility(Abilities.RECKLESS) && isRecoilMove())
	  damagemult = Math.Round(damagemult * 1.2);
	if (attacker.hasWorkingAbility(Abilities.FLARE_BOOST) &&
	   attacker.Status==Status.BURN && pbIsSpecial(type))
	   damagemult = Math.Round(damagemult * 1.5);
	if (attacker.hasWorkingAbility(Abilities.TOXIC_BOOST) &&
	   attacker.Status==Status.POISON && pbIsPhysical(type))
	   damagemult = Math.Round(damagemult * 1.5);
	if (attacker.hasWorkingAbility(Abilities.ANALYTIC) &&
	   (Battle.choices[opponent.Index].Action != ChoiceAction.UseMove || // Didn't choose a move
	   opponent.hasMovedThisRound())) // Used a move already
	  damagemult=Math.Round(damagemult*1.3);
	if (attacker.hasWorkingAbility(Abilities.RIVALRY) &&
	   attacker.Gender.HasValue && opponent.Gender.HasValue){
	  if (attacker.Gender.Value==opponent.Gender.Value)
		damagemult = Math.Round(damagemult * 1.25);
	  else
		damagemult=Math.Round(damagemult*0.75);
	}
	if (attacker.hasWorkingAbility(Abilities.SAND_FORCE) &&
	   Battle.Weather==Weather.SANDSTORM 
	   && (type == Types.ROCK ||
	   type == Types.GROUND ||
	   type == Types.STEEL))
	  damagemult=Math.Round(damagemult*1.3);
	if (attacker.hasWorkingAbility(Abilities.SHEER_FORCE) && AddlEffect>0)
	  damagemult=Math.Round(damagemult*1.3);
	if (attacker.hasWorkingAbility(Abilities.TOUGH_CLAWS) && Flags.Contact)
	  damagemult = Math.Round(damagemult * 4 / 3);
	if (attacker.hasWorkingAbility(Abilities.AERILATE) ||
	   attacker.hasWorkingAbility(Abilities.REFRIGERATE) ||
	   attacker.hasWorkingAbility(Abilities.PIXILATE) && PowerBoost)
	  damagemult = Math.Round(damagemult * 1.3);
	if ((Battle.pbCheckGlobalAbility(Abilities.DARK_AURA).IsNotNullOrNone() && type == Types.DARK) 
	   || (Battle.pbCheckGlobalAbility(Abilities.FAIRY_AURA).IsNotNullOrNone() && type == Types.FAIRY)){
	  if (Battle.pbCheckGlobalAbility(Abilities.AURA_BREAK).IsNotNullOrNone())
		damagemult=Math.Round(damagemult*2/3);
	  else
		damagemult=Math.Round(damagemult*4/3);
	}
	if (!attacker.hasMoldBreaker()){
	  if (opponent.hasWorkingAbility(Abilities.HEATPROOF) && type == Types.FIRE)
		damagemult=Math.Round(damagemult*0.5);
	  if (opponent.hasWorkingAbility(Abilities.THICK_FAT) 
		 && (type == Types.ICE || type == Types.FIRE))
		damagemult=Math.Round(damagemult*0.5);
	  if (opponent.hasWorkingAbility(Abilities.FUR_COAT) &&
		 (pbIsPhysical(type) || Effect==Attack.Data.Effects.x11B)) // Psyshock
		damagemult=Math.Round(damagemult*0.5);
	  if (opponent.hasWorkingAbility(Abilities.DRY_SKIN) && type == Types.FIRE)
		damagemult=Math.Round(damagemult*1.25);
	}
	// Gems are the first items to be considered, as Symbiosis can replace a
	// consumed Gem and the replacement item should work immediately.
	if (Effect!=Attack.Data.Effects.x145 && Effect!=Attack.Data.Effects.x146 && Effect!=Attack.Data.Effects.x147) { // Pledge moves
	  if ((attacker.hasWorkingItem(Items.NORMAL_GEM)   && type == Types.NORMAL) ||
		 (attacker.hasWorkingItem(Items.FIGHTING_GEM) && type == Types.FIGHTING) ||
		 (attacker.hasWorkingItem(Items.FLYING_GEM)   && type == Types.FLYING) ||
		 (attacker.hasWorkingItem(Items.POISON_GEM)   && type == Types.POISON) ||
		 (attacker.hasWorkingItem(Items.GROUND_GEM)   && type == Types.GROUND) ||
		 (attacker.hasWorkingItem(Items.ROCK_GEM)     && type == Types.ROCK) ||
		 (attacker.hasWorkingItem(Items.BUG_GEM)      && type == Types.BUG) ||
		 (attacker.hasWorkingItem(Items.GHOST_GEM)    && type == Types.GHOST) ||
		 (attacker.hasWorkingItem(Items.STEEL_GEM)    && type == Types.STEEL) ||
		 (attacker.hasWorkingItem(Items.FIRE_GEM)     && type == Types.FIRE) ||
		 (attacker.hasWorkingItem(Items.WATER_GEM)    && type == Types.WATER) ||
		 (attacker.hasWorkingItem(Items.GRASS_GEM)    && type == Types.GRASS) ||
		 (attacker.hasWorkingItem(Items.ELECTRIC_GEM) && type == Types.ELECTRIC) ||
		 (attacker.hasWorkingItem(Items.PSYCHIC_GEM)  && type == Types.PSYCHIC) ||
		 (attacker.hasWorkingItem(Items.ICE_GEM)      && type == Types.ICE) ||
		 (attacker.hasWorkingItem(Items.DRAGON_GEM)   && type == Types.DRAGON) ||
		 (attacker.hasWorkingItem(Items.DARK_GEM)     && type == Types.DARK) ||
		 (attacker.hasWorkingItem(Items.FAIRY_GEM)    && type == Types.FAIRY)){
		damagemult=(Core.USENEWBATTLEMECHANICS)? Math.Round(damagemult*1.3) : Math.Round(damagemult * 1.5);
	   Battle.pbCommonAnimation("UseItem", attacker, null);
		Battle.pbDisplayBrief(Game._INTL("The {1} strengthened {2}'s power!",
		   Game.ItemData[attacker.Item].ToString(), Game.MoveData[MoveId].Name));
		attacker.pbConsumeItem();
	  }
	}
	if ((attacker.hasWorkingItem(Items.SILK_SCARF)    && type == Types.NORMAL) ||
	   (attacker.hasWorkingItem(Items.BLACK_BELT)     && type == Types.FIGHTING) ||
	   (attacker.hasWorkingItem(Items.SHARP_BEAK)     && type == Types.FLYING) ||
	   (attacker.hasWorkingItem(Items.POISON_BARB)    && type == Types.POISON) ||
	   (attacker.hasWorkingItem(Items.SOFT_SAND)      && type == Types.GROUND) ||
	   (attacker.hasWorkingItem(Items.HARD_STONE)     && type == Types.ROCK) ||
	   (attacker.hasWorkingItem(Items.SILVER_POWDER)  && type == Types.BUG) ||
	   (attacker.hasWorkingItem(Items.SPELL_TAG)      && type == Types.GHOST) ||
	   (attacker.hasWorkingItem(Items.METAL_COAT)     && type == Types.STEEL) ||
	   (attacker.hasWorkingItem(Items.CHARCOAL)       && type == Types.FIRE) ||
	   (attacker.hasWorkingItem(Items.MYSTIC_WATER)   && type == Types.WATER) ||
	   (attacker.hasWorkingItem(Items.MIRACLE_SEED)   && type == Types.GRASS) ||
	   (attacker.hasWorkingItem(Items.MAGNET)         && type == Types.ELECTRIC) ||
	   (attacker.hasWorkingItem(Items.TWISTED_SPOON)  && type == Types.PSYCHIC) ||
	   (attacker.hasWorkingItem(Items.NEVER_MELT_ICE) && type == Types.ICE) ||
	   (attacker.hasWorkingItem(Items.DRAGON_FANG)    && type == Types.DRAGON) ||
	   (attacker.hasWorkingItem(Items.BLACK_GLASSES)  && type == Types.DARK))
	  damagemult=Math.Round(damagemult*1.2);
	if ((attacker.hasWorkingItem(Items.FIST_PLATE)    && type == Types.FIGHTING) ||
	   (attacker.hasWorkingItem(Items.SKY_PLATE)      && type == Types.FLYING) ||
	   (attacker.hasWorkingItem(Items.TOXIC_PLATE)    && type == Types.POISON) ||
	   (attacker.hasWorkingItem(Items.EARTH_PLATE)    && type == Types.GROUND) ||
	   (attacker.hasWorkingItem(Items.STONE_PLATE)    && type == Types.ROCK) ||
	   (attacker.hasWorkingItem(Items.INSECT_PLATE)   && type == Types.BUG) ||
	   (attacker.hasWorkingItem(Items.SPOOKY_PLATE)   && type == Types.GHOST) ||
	   (attacker.hasWorkingItem(Items.IRON_PLATE)     && type == Types.STEEL) ||
	   (attacker.hasWorkingItem(Items.FLAME_PLATE)    && type == Types.FIRE) ||
	   (attacker.hasWorkingItem(Items.SPLASH_PLATE)   && type == Types.WATER) ||
	   (attacker.hasWorkingItem(Items.MEADOW_PLATE)   && type == Types.GRASS) ||
	   (attacker.hasWorkingItem(Items.ZAP_PLATE)      && type == Types.ELECTRIC) ||
	   (attacker.hasWorkingItem(Items.MIND_PLATE)     && type == Types.PSYCHIC) ||
	   (attacker.hasWorkingItem(Items.ICICLE_PLATE)   && type == Types.ICE) ||
	   (attacker.hasWorkingItem(Items.DRACO_PLATE)    && type == Types.DRAGON) ||
	   (attacker.hasWorkingItem(Items.DREAD_PLATE)    && type == Types.DARK) ||
	   (attacker.hasWorkingItem(Items.PIXIE_PLATE)    && type == Types.FAIRY))
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.ROCK_INCENSE)   && type == Types.ROCK)
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.ROSE_INCENSE)   && type == Types.GRASS)
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.SEA_INCENSE)    && type == Types.WATER)
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.WAVE_INCENSE)   && type == Types.WATER)
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.ODD_INCENSE)    && type == Types.PSYCHIC)
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.MUSCLE_BAND)    && pbIsPhysical(type))
	   damagemult = Math.Round(damagemult * 1.1);
	if (attacker.hasWorkingItem(Items.WISE_GLASSES)   && pbIsSpecial (type))
	   damagemult = Math.Round(damagemult * 1.1);
	if (attacker.hasWorkingItem(Items.LUSTROUS_ORB)
	   && attacker.Species == Pokemons.PALKIA &&
	   (type == Types.DRAGON || type == Types.WATER))
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.ADAMANT_ORB) 
	   && attacker.Species == Pokemons.DIALGA &&
	   (type == Types.DRAGON || type == Types.STEEL))
	  damagemult=Math.Round(damagemult*1.2);
	if (attacker.hasWorkingItem(Items.GRISEOUS_ORB) 
	   && attacker.Species == Pokemons.GIRATINA &&
	   (type == Types.DRAGON || type == Types.GHOST))
	  damagemult=Math.Round(damagemult*1.2);
	damagemult=pbBaseDamageMultiplier(damagemult, attacker, opponent);
	if (attacker.effects.MeFirst)
	  damagemult = Math.Round(damagemult * 1.5);
	if (attacker.effects.HelpingHand) //&& options&SELFCONFUSE)
	  damagemult=Math.Round(damagemult*1.5);
	if (attacker.effects.Charge>0 && type == Types.ELECTRIC)
	  damagemult=Math.Round(damagemult*2.0);
	if (type == Types.FIRE){
	  for (int i = 0; i < 4; i++)
		if (Battle.battlers[i].effects.WaterSport && !Battle.battlers[i].isFainted()){
		  damagemult = Math.Round(damagemult * 0.33);
		  break;
		}
	  if (Battle.field.WaterSportField>0)
		damagemult=Math.Round(damagemult*0.33);
	}
	if (type == Types.ELECTRIC){
	  for (int i = 0; i < 4; i++)
		if (Battle.battlers[i].effects.MudSport && !Battle.battlers[i].isFainted()){
		  damagemult = Math.Round(damagemult * 0.33);
		  break;
		}
	  if (Battle.field.MudSportField>0)
		damagemult=Math.Round(damagemult*0.33);
	}
	if (Battle.field.ElectricTerrain>0 &&
	   !attacker.isAirborne() && type == Types.ELECTRIC)
	  damagemult=Math.Round(damagemult*1.5);
	if (Battle.field.GrassyTerrain>0 &&
	   !attacker.isAirborne() && type == Types.GRASS)
	  damagemult=Math.Round(damagemult*1.5);
	if (Battle.field.MistyTerrain>0 &&
	   !opponent.isAirborne(attacker.hasMoldBreaker()) && type == Types.DRAGON)
	  damagemult=Math.Round(damagemult*0.5);
	if (opponent.effects.Minimize && tramplesMinimize(2))
	  damagemult=Math.Round(damagemult*2.0);
	basedmg=(int)Math.Round(basedmg* damagemult*1.0/0x1000);
	#endregion
	#region ##### Calculate attacker's attack stat #####
	int atk = attacker.attack;
	int atkstage=attacker.stages[(int)Stats.ATTACK]+6;
	if (Effect==Attack.Data.Effects.x12A){ // Foul Play
	  atk=opponent.attack;
	  atkstage = opponent.stages[(int)Stats.ATTACK] + 6;
	}
	if (type>=0 && pbIsSpecial (type)){
	   atk = attacker.spatk;
	  atkstage=attacker.stages[(int)Stats.SPATK]+6;
	  if (Effect==Attack.Data.Effects.x12A){ // Foul Play
		atk=opponent.spatk;
		atkstage = opponent.stages[(int)Stats.SPATK] + 6;
	  }
	}
	if (attacker.hasMoldBreaker() || !opponent.hasWorkingAbility(Abilities.UNAWARE)){
	  if (opponent.damagestate.Critical && atkstage<6) atkstage=6; 
	  atk=(int)Math.Floor(atk*1.0*stagemul[atkstage]/stagediv[atkstage]);
	}
	if (attacker.hasWorkingAbility(Abilities.HUSTLE) && pbIsPhysical (type))
	   atk = (int)Math.Round(atk * 1.5);
	double atkmult = 0x1000;
	if (Battle.internalbattle){
	  if (Battle.pbOwnedByPlayer(attacker.Index) && pbIsPhysical (type) &&
		 Battle.pbPlayer().BadgesCount>=Core.BADGESBOOSTATTACK)
		atkmult = Math.Round(atkmult * 1.1);
	  if (Battle.pbOwnedByPlayer(attacker.Index) && pbIsSpecial (type) &&
		 Battle.pbPlayer().BadgesCount >= Core.BADGESBOOSTSPATK)
		atkmult = Math.Round(atkmult * 1.1);
	}
	if (attacker.HP<=Math.Floor(attacker.TotalHP/3d))
	  if ((attacker.hasWorkingAbility(Abilities.OVERGROW) && type == Types.GRASS) ||
		 (attacker.hasWorkingAbility(Abilities.BLAZE)     && type == Types.FIRE) ||
		 (attacker.hasWorkingAbility(Abilities.TORRENT)   && type == Types.WATER) ||
		 (attacker.hasWorkingAbility(Abilities.SWARM)     && type == Types.BUG))
		atkmult=Math.Round(atkmult*1.5);
	if (attacker.hasWorkingAbility(Abilities.GUTS) &&
	   attacker.Status!=0 && pbIsPhysical (type))
	   atkmult = Math.Round(atkmult * 1.5);
	if ((attacker.hasWorkingAbility(Abilities.PLUS) || attacker.hasWorkingAbility(Abilities.MINUS)) &&
	   pbIsSpecial (type)){
	   Pokemon partner = attacker.Partner;
	  if (partner.hasWorkingAbility(Abilities.PLUS) || partner.hasWorkingAbility(Abilities.MINUS))
		atkmult=Math.Round(atkmult*1.5);
	}
	if (attacker.hasWorkingAbility(Abilities.DEFEATIST) &&
	   attacker.HP<=Math.Floor(attacker.TotalHP/2d))
	  atkmult = Math.Round(atkmult * 0.5);
	if ((attacker.hasWorkingAbility(Abilities.PURE_POWER) ||
	   attacker.hasWorkingAbility(Abilities.HUGE_POWER)) && pbIsPhysical(type))
	   atkmult = Math.Round(atkmult * 2.0);
	if (attacker.hasWorkingAbility(Abilities.SOLAR_POWER) && pbIsSpecial(type) &&
	   (Battle.Weather==Weather.SUNNYDAY ||
	   Battle.Weather==Weather.HARSHSUN))
	  atkmult=Math.Round(atkmult*1.5);
	if (attacker.hasWorkingAbility(Abilities.FLASH_FIRE) &&
	   attacker.effects.FlashFire && type == Types.FIRE)
	  atkmult=Math.Round(atkmult*1.5);
	if (attacker.hasWorkingAbility(Abilities.SLOW_START) &&
	   attacker.turncount<=5 && pbIsPhysical(type))
	   atkmult = Math.Round(atkmult * 0.5);
	if ((Battle.Weather==Weather.SUNNYDAY ||
	   Battle.Weather==Weather.HARSHSUN) && pbIsPhysical(type))
	  if (attacker.hasWorkingAbility(Abilities.FLOWER_GIFT) ||
		 attacker.Partner.hasWorkingAbility(Abilities.FLOWER_GIFT))
		atkmult=Math.Round(atkmult*1.5);
	if (attacker.hasWorkingItem(Items.THICK_CLUB) &&
	   (attacker.Species == Pokemons.CUBONE ||
	   attacker.Species == Pokemons.MAROWAK) && 
	   pbIsPhysical(type))
		 atkmult = Math.Round(atkmult * 2.0);
	if (attacker.hasWorkingItem(Items.DEEP_SEA_TOOTH) &&
	   attacker.Species == Pokemons.CLAMPERL && 
	   pbIsSpecial(type))
		 atkmult = Math.Round(atkmult * 2.0);
	if (attacker.hasWorkingItem(Items.LIGHT_BALL)
	   && attacker.Species == Pokemons.PIKACHU)
	  atkmult=Math.Round(atkmult*2.0);
	if (attacker.hasWorkingItem(Items.SOUL_DEW) &&
	   (attacker.Species == Pokemons.LATIAS ||
	   attacker.Species == Pokemons.LATIOS) && 
	   pbIsSpecial(type) &&
	   !Battle.rules.Contains("souldewclause"))//!Battle.rules["souldewclause"])
	  atkmult = Math.Round(atkmult * 1.5);
	if (attacker.hasWorkingItem(Items.CHOICE_BAND) && pbIsPhysical(type))
	   atkmult = Math.Round(atkmult * 1.5);
	if (attacker.hasWorkingItem(Items.CHOICE_SPECS) && pbIsSpecial(type))
	   atkmult = Math.Round(atkmult * 1.5);
	atk = (int)Math.Round(atk * atkmult * 1.0 / 0x1000);
	#endregion
	#region ##### Calculate opponent's defense stat #####
	int defense=opponent.defense;
	int defstage = opponent.stages[(int)Stats.DEFENSE] + 6;
	// TODO: Wonder Room should apply around here
	bool applysandstorm=false;
	if (type>=0 && pbIsSpecial(type) && Effect!=Attack.Data.Effects.x11B){ // Psyshock
	  defense=opponent.spdef;
	  defstage = opponent.stages[(int)Stats.SPDEF] + 6;
	  applysandstorm=true;
	}
	if (!attacker.hasWorkingAbility(Abilities.UNAWARE)){
	  if (Effect==Attack.Data.Effects.x130) defstage=6;  // Chip Away (ignore stat stages)
	  if (opponent.damagestate.Critical && defstage>6)defstage=6; 
	  defense=(int)Math.Floor(defense*1.0*stagemul[defstage]/stagediv[defstage]);
	}
	if (Battle.Weather==Weather.SANDSTORM &&
	   opponent.hasType(Types.ROCK) && applysandstorm)
	   defense = (int)Math.Round(defense * 1.5);
	double defmult = 0x1000;
	if (Battle.internalbattle){
	  if (Battle.pbOwnedByPlayer(opponent.Index) && pbIsPhysical(type) &&
		 Battle.pbPlayer().BadgesCount >= Core.BADGESBOOSTDEFENSE)
		defmult = Math.Round(defmult * 1.1);
	  if (Battle.pbOwnedByPlayer(opponent.Index) && pbIsSpecial(type) &&
		 Battle.pbPlayer().BadgesCount >= Core.BADGESBOOSTSPDEF)
		defmult = Math.Round(defmult * 1.1);
	}
	if (Battle.field.GrassyTerrain>0)
	  defmult=Math.Round(defmult*1.5);
	if (!attacker.hasMoldBreaker()){
	  if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE) &&
		 opponent.Status>0 && pbIsPhysical(type))
		 defmult = Math.Round(defmult * 1.5);
	  if ((Battle.Weather==Weather.SUNNYDAY ||
		 Battle.Weather==Weather.HARSHSUN) && pbIsSpecial(type))
		if (opponent.hasWorkingAbility(Abilities.FLOWER_GIFT) ||
		   opponent.Partner.hasWorkingAbility(Abilities.FLOWER_GIFT))
		  defmult=Math.Round(defmult*1.5);
	}
	if (opponent.hasWorkingItem(Items.ASSAULT_VEST) && pbIsSpecial(type))
	   defmult = Math.Round(defmult * 1.5);
	if (opponent.hasWorkingItem(Items.EVIOLITE)){
	  //evos=pbGetEvolvedFormData(opponent.Species);
	  //if (evos && evos.Length>0)      
	  if (Game.PokemonEvolutionsData[opponent.Species].Length>0)
		defmult=Math.Round(defmult*1.5);
	}
	if (opponent.hasWorkingItem(Items.DEEP_SEA_SCALE) &&
	   opponent.Species == Pokemons.CLAMPERL && 
	   pbIsSpecial(type))
		 defmult = Math.Round(defmult * 2.0);
	if (opponent.hasWorkingItem(Items.METAL_POWDER) &&
	   opponent.Species == Pokemons.DITTO &&
	   !opponent.effects.Transform)
	  defmult = Math.Round(defmult * 1.5);
	if (opponent.hasWorkingItem(Items.SOUL_DEW) &&
	   (opponent.Species == Pokemons.LATIAS ||
	   opponent.Species == Pokemons.LATIOS) && 
	   pbIsSpecial(type) &&
	   !Battle.rules.Contains("souldewclause"))//!Battle.rules["souldewclause"]
	  defmult = Math.Round(defmult * 1.5);
	defense = (int)Math.Round(defense * defmult * 1.0 / 0x1000);
	#endregion
	#region ##### Main damage calculation #####
	int damage=(int)Math.Floor(Math.Floor(Math.Floor(2.0*attacker.level/5+2)* basedmg*atk/defense)/50)+2;
	// Multi-targeting attacks
	if (pbTargetsMultiple(attacker))
	   damage = (int)Math.Round(damage * 0.75);
	// Weather
	switch (Battle.Weather) { 
	case Weather.SUNNYDAY: case Weather.HARSHSUN:
	  if (type == Types.FIRE)
		damage=(int)Math.Round(damage*1.5);
	  else if (type == Types.WATER)
		damage=(int)Math.Round(damage*0.5);
	  break;
	case Weather.RAINDANCE: case Weather.HEAVYRAIN:
	  if (type == Types.FIRE)
		damage=(int)Math.Round(damage*0.5);
	  else if (type == Types.WATER)
		damage=(int)Math.Round(damage*1.5);
	  break;
	}
	// Critical hits
	if (opponent.damagestate.Critical)
	  damage = (Core.USENEWBATTLEMECHANICS) ? (int)Math.Round(damage * 1.5) : (int)Math.Round(damage * 2.0);
	// Random variance
	//if ((options&NOWEIGHTING)==0){ //If RNG affects damage
	  int random=85+Battle.pbRandom(16);
	  damage=(int)Math.Floor(damage* random/100.0);
	 //}
	// STAB
	if (attacker.hasType(type) && !Core.IGNOREPKMNTYPES)
	  if (attacker.hasWorkingAbility(Abilities.ADAPTABILITY))
		damage= (int)Math.Round(damage*2d);
	  else
		damage= (int)Math.Round(damage*1.5);
	// Type effectiveness
	if (!Core.IGNOREPKMNTYPES){
	  double typemod=pbTypeModMessages(type, attacker, opponent);
	  damage= (int)Math.Round(damage* typemod/8.0);
	   opponent.damagestate.TypeMod=typemod;
	   if (typemod==0){
		 opponent.damagestate.CalcDamage= 0; 
		 opponent.damagestate.Critical= false;
		return 0;
	   }
	} else
	   opponent.damagestate.TypeMod= 8;
	 // Burn
	if (attacker.Status==Status.BURN && pbIsPhysical(type) &&
	   !attacker.hasWorkingAbility(Abilities.GUTS) &&
	   !(Core.USENEWBATTLEMECHANICS && Effect==Attack.Data.Effects.x0AA)) // Facade
	  damage=(int)Math.Round(damage*0.5);
	// Make sure damage is at least 1
	if (damage<1)damage=1;
	// Final damage modifiers
	double finaldamagemult=0x1000;
	if (!opponent.damagestate.Critical && Core.NOREFLECT &&
	   !attacker.hasWorkingAbility(Abilities.INFILTRATOR)){
	  // Reflect
	  if (opponent.OwnSide.Reflect>0 && pbIsPhysical(type))
		if (Battle.doublebattle)
		  finaldamagemult = Math.Round(finaldamagemult * 0.66);
		else
		  finaldamagemult=Math.Round(finaldamagemult*0.5);
	  // Light Screen
	  if (opponent.OwnSide.LightScreen>0 && pbIsSpecial(type))
		if (Battle.doublebattle)
		  finaldamagemult = Math.Round(finaldamagemult * 0.66);
		else
		  finaldamagemult=Math.Round(finaldamagemult*0.5);
	}
	if (attacker.effects.ParentalBond==1)
	  finaldamagemult=Math.Round(finaldamagemult*0.5);
	if (attacker.hasWorkingAbility(Abilities.TINTED_LENS) && opponent.damagestate.TypeMod<8)
	  finaldamagemult=Math.Round(finaldamagemult*2.0);
	if (attacker.hasWorkingAbility(Abilities.SNIPER) && opponent.damagestate.Critical)
	  finaldamagemult = Math.Round(finaldamagemult * 1.5);
	if (!attacker.hasMoldBreaker()){
	  if (opponent.hasWorkingAbility(Abilities.MULTISCALE) && opponent.HP==opponent.TotalHP)
		finaldamagemult = Math.Round(finaldamagemult * 0.5);
	  if ((opponent.hasWorkingAbility(Abilities.SOLID_ROCK) ||
		 opponent.hasWorkingAbility(Abilities.FILTER)) &&
		 opponent.damagestate.TypeMod>8)
		finaldamagemult=Math.Round(finaldamagemult*0.75);
	  if (opponent.Partner.hasWorkingAbility(Abilities.FRIEND_GUARD))
		finaldamagemult=Math.Round(finaldamagemult*0.75);
	}
	if (attacker.hasWorkingItem(Items.METRONOME)){
	  double met=1+0.2*Math.Min(attacker.effects.Metronome,5);
finaldamagemult = Math.Round(finaldamagemult * met);
	}
	if (attacker.hasWorkingItem(Items.EXPERT_BELT) &&
	   opponent.damagestate.TypeMod>8)
	  finaldamagemult=Math.Round(finaldamagemult*1.2);
	if (attacker.hasWorkingItem(Items.LIFE_ORB)){// && SELFCONFUSE
	  attacker.effects.LifeOrb=true;
	  finaldamagemult=Math.Round(finaldamagemult*1.3);
	}
	if (opponent.damagestate.TypeMod>8 && Core.IGNOREPKMNTYPES)
	  if ((opponent.hasWorkingItem(Items.CHOPLE_BERRY) && type == Types.FIGHTING) ||
		 (opponent.hasWorkingItem(Items.COBA_BERRY)    && type == Types.FLYING) ||
		 (opponent.hasWorkingItem(Items.KEBIA_BERRY)   && type == Types.POISON) ||
		 (opponent.hasWorkingItem(Items.SHUCA_BERRY)   && type == Types.GROUND) ||
		 (opponent.hasWorkingItem(Items.CHARTI_BERRY)  && type == Types.ROCK) ||
		 (opponent.hasWorkingItem(Items.TANGA_BERRY)   && type == Types.BUG) ||
		 (opponent.hasWorkingItem(Items.KASIB_BERRY)   && type == Types.GHOST) ||
		 (opponent.hasWorkingItem(Items.BABIRI_BERRY)  && type == Types.STEEL) ||
		 (opponent.hasWorkingItem(Items.OCCA_BERRY)    && type == Types.FIRE) ||
		 (opponent.hasWorkingItem(Items.PASSHO_BERRY)  && type == Types.WATER) ||
		 (opponent.hasWorkingItem(Items.RINDO_BERRY)   && type == Types.GRASS) ||
		 (opponent.hasWorkingItem(Items.WACAN_BERRY)   && type == Types.ELECTRIC) ||
		 (opponent.hasWorkingItem(Items.PAYAPA_BERRY)  && type == Types.PSYCHIC) ||
		 (opponent.hasWorkingItem(Items.YACHE_BERRY)   && type == Types.ICE) ||
		 (opponent.hasWorkingItem(Items.HABAN_BERRY)   && type == Types.DRAGON) ||
		 (opponent.hasWorkingItem(Items.COLBUR_BERRY)  && type == Types.DARK) ||
		 (opponent.hasWorkingItem(Items.ROSELI_BERRY)  && type == Types.FAIRY)) { 
		finaldamagemult=Math.Round(finaldamagemult*0.5);
		opponent.damagestate.BerryWeakened=true;
		Battle.pbCommonAnimation("UseItem", opponent, null);
	  }
	if (opponent.hasWorkingItem(Items.CHILAN_BERRY) && type == Types.NORMAL &&
	   Core.IGNOREPKMNTYPES){
	  finaldamagemult=Math.Round(finaldamagemult*0.5);
	  opponent.damagestate.BerryWeakened=true;
	  Battle.pbCommonAnimation("UseItem", opponent, null);
	}
	#endregion
	finaldamagemult = pbModifyDamage(finaldamagemult, attacker, opponent);
	damage= (int)Math.Round(damage* finaldamagemult*1.0/0x1000);
	 opponent.damagestate.CalcDamage=damage;
	 GameDebug.Log($"Move's damage calculated to be #{damage}");
	return damage;
  }

  public virtual int pbReduceHPDamage(int damage, Pokemon attacker, Pokemon opponent){
	//bool endure=false;
	if (opponent.effects.Substitute>0 && !ignoresSubstitute(attacker) &&
	   (attacker.Species == Pokemons.NONE || attacker.Index!=opponent.Index)){
	  GameDebug.Log($"[Lingering effect triggered] #{opponent.ToString()}'s Substitute took the damage");
	  if (damage>opponent.effects.Substitute)damage=opponent.effects.Substitute;
	  opponent.effects.Substitute-=damage;
	  opponent.damagestate.Substitute= true;
	  Battle.scene.pbDamageAnimation(opponent,0);
	  Battle.pbDisplayPaused(Game._INTL("The substitute took damage for {1}!",opponent.Name));
	  if (opponent.effects.Substitute<=0){
		opponent.effects.Substitute=0;
		Battle.pbDisplayPaused(Game._INTL("{1}'s substitute faded!",opponent.Name));
		GameDebug.Log($"[End of effect] #{opponent.ToString()}'s Substitute faded");
	  }
	  opponent.damagestate.HPLost=damage;
	  damage = 0;
	} else{
	  opponent.damagestate.Substitute= false;
	  if (damage>=opponent.HP){
		damage = opponent.HP;
		if (Effect==Attack.Data.Effects.x066) // False Swipe
		  damage= damage - 1;
		else if (opponent.effects.Endure){
		  damage = damage - 1;
		  opponent.damagestate.Endured= true;
		  GameDebug.Log($"[Lingering effect triggered] #{opponent.ToString()}'s Endure");}
		else if (damage==opponent.TotalHP){
		  if (opponent.hasWorkingAbility(Abilities.STURDY) && !attacker.hasMoldBreaker()){
			opponent.damagestate.Sturdy=true;
			damage= damage - 1;
			GameDebug.Log($"[Ability triggered] #{opponent.ToString()}'s Sturdy");}
		  else if (opponent.hasWorkingItem(Items.FOCUS_SASH) && opponent.HP==opponent.TotalHP){
			opponent.damagestate.FocusSash=true;
			damage= damage - 1;
			GameDebug.Log($"[Item triggered] #{opponent.ToString()}'s Focus Sash");}
		  else if (opponent.hasWorkingItem(Items.FOCUS_BAND) && Battle.pbRandom(10)==0){
			opponent.damagestate.FocusBand=true;
			damage=damage-1;
			GameDebug.Log($"[Item triggered] #{opponent.ToString()}'s Focus Band");
		  }
		}
		if (damage<0)damage=0;
	  }
	  int oldhp=opponent.HP;
	  opponent.HP-=damage;
	  int effectiveness=0;
	  if (opponent.damagestate.TypeMod<8)
		effectiveness=1;   // "Not very effective"
	  else if (opponent.damagestate.TypeMod>8)
		effectiveness=2;   // "Super effective"
	  if (opponent.damagestate.TypeMod!=0)
		Battle.scene.pbDamageAnimation(opponent, effectiveness);
	  Battle.scene.pbHPChanged(opponent, oldhp);
	  opponent.damagestate.HPLost=damage;
	}
	return damage;
  }
#endregion

#region Effects
  public virtual void pbEffectMessages(Pokemon attacker, Pokemon opponent, bool ignoretype= false, int[] alltargets= null){
	if (opponent.damagestate.Critical)
	  if (alltargets != null && alltargets.Length>1)
		Battle.pbDisplay(Game._INTL("A critical hit on {1}!",opponent.ToString(true)));
	  else
		Battle.pbDisplay(Game._INTL("A critical hit!"));
	if (!pbIsMultiHit() && attacker.effects.ParentalBond==0){
	  if (opponent.damagestate.TypeMod>8)
		if (alltargets != null && alltargets.Length>1)
		  Battle.pbDisplay(Game._INTL("It's super effective on {1}!",opponent.ToString(true)));
		else
		  Battle.pbDisplay(Game._INTL("It's super effective!"));
	  else if (opponent.damagestate.TypeMod>=1 && opponent.damagestate.TypeMod<8)
		if (alltargets != null && alltargets.Length>1)
		  Battle.pbDisplay(Game._INTL("It's not very effective on {1}...",opponent.ToString(true)));
		else
		  Battle.pbDisplay(Game._INTL("It's not very effective..."));
	}
	if (opponent.damagestate.Endured)
	  Battle.pbDisplay(Game._INTL("{1} endured the hit!", opponent.ToString()));
	else if (opponent.damagestate.Sturdy)
	  Battle.pbDisplay(Game._INTL("{1} hung on with Sturdy!", opponent.ToString()));
	else if (opponent.damagestate.FocusSash){
	  Battle.pbDisplay(Game._INTL("{1} hung on using its Focus Sash!", opponent.ToString()));
	  opponent.pbConsumeItem();}
	else if (opponent.damagestate.FocusBand)
	  Battle.pbDisplay(Game._INTL("{1} hung on using its Focus Band!", opponent.ToString()));
  }

  public virtual int pbEffectFixedDamage(int damage, Pokemon attacker, Pokemon opponent, int hitnum= 0, int[] alltargets= null, bool showanimation= true){
	Types type=pbType(this.Type, attacker, opponent);
	double typemod=pbTypeModMessages(type, attacker, opponent);
	opponent.damagestate.Critical=false;
	opponent.damagestate.TypeMod=0;
	opponent.damagestate.CalcDamage=0;
	opponent.damagestate.HPLost=0;
	if (typemod!=0){
	  opponent.damagestate.CalcDamage=damage;
	  opponent.damagestate.TypeMod=8;
	  pbShowAnimation(MoveId, attacker, opponent, hitnum, alltargets, showanimation);
	  if (damage<1)damage = 1;  // HP reduced can't be less than 1
	  damage=pbReduceHPDamage(damage, attacker, opponent);
	  pbEffectMessages(attacker, opponent, alltargets: alltargets);
	  pbOnDamageLost(damage, attacker, opponent);
	  return damage;
	}
	return 0;
  }

  public virtual int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int[] alltargets= null, bool showanimation= true){
	if (opponent.Species == Pokemons.NONE)return 0;
	int damage = pbCalcDamage(attacker, opponent);
	if (opponent.damagestate.TypeMod!=0)
	  pbShowAnimation(MoveId, attacker, opponent, hitnum, alltargets, showanimation);
	damage = pbReduceHPDamage(damage, attacker, opponent);
	pbEffectMessages(attacker, opponent);
	pbOnDamageLost(damage, attacker, opponent);
	return damage;   // The HP lost by the opponent due to this attack
  }

  public virtual void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects){
  }
#endregion

#region Using the move
  public virtual bool pbOnStartUse(Pokemon attacker){
	return true;
  }

  public virtual void pbAddTarget(Pokemon[] targets, Pokemon attacker){
  }

/// <summary>
/// </summary>
/// <param name="attacker"></param>
/// <returns>
/// Return values:
/// -1 if the attack should exit as a failure
/// 0 if the attack should proceed with its effect
/// 1 if the attack should exit as a success
/// 2 if Bide is storing energy
/// </returns>
  public virtual int pbDisplayUseMessage(Pokemon attacker){
	Battle.pbDisplayBrief(Game._INTL("{1} used\r\n{2}!",attacker.ToString(), Game.MoveData[MoveId].Name));
	return 0;
  }

  public virtual void pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, int hitnum= 0, int[] alltargets= null, bool showanimation= true){
	if (!showanimation)return;
	if (attacker.effects.ParentalBond == 1) { 
	  Battle.pbCommonAnimation("ParentalBond",attacker,opponent);
	  return;
	}
	Battle.pbAnimation(id, attacker, opponent, hitnum);
  }

  public virtual void pbOnDamageLost(int damage, Pokemon attacker, Pokemon opponent){
	// Used by Counter/Mirror Coat/Revenge/Focus Punch/Bide
	Types type= this.Type;
	type = pbType(type, attacker, opponent);
	if (opponent.effects.Bide>0){
	  opponent.effects.BideDamage+=damage;
	  opponent.effects.BideTarget= attacker.Index;
	}
	if (Effect==Attack.Data.Effects.x088) // Hidden Power
	  type=Types.NORMAL; //getConst(Types.NORMAL) || 0;
	if (pbIsPhysical(type)){
	  opponent.effects.Counter= damage;
	  opponent.effects.CounterTarget= attacker.Index;
	}else if (pbIsSpecial(type)){
	  opponent.effects.MirrorCoat= damage;
	  opponent.effects.MirrorCoatTarget= attacker.Index;
	}
	opponent.lastHPLost= damage; // for Revenge/Focus Punch/Metal Burst
	if (damage>0)opponent.tookDamage= true;  // for Assurance
	opponent.lastAttacker.Add(attacker.Index); // for Revenge/Metal Burst
  }

  public virtual bool pbMoveFailed(Pokemon attacker, Pokemon opponent){
	// Called to determine whether the move failed
	return false;
  }
	   #endregion
	}
}
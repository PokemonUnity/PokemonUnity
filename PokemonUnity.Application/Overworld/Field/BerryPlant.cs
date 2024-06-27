using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonUnity.EventArg;


namespace PokemonUnity//.Inventory.Plants
{
	public partial class PokemonTemp : ITempMetadataBerryPlants {
		public IDictionary<int, BerryData> berryPlantData { get; private set; }
		public BerryData GetBerryPlantData(Items item) {
			if (@berryPlantData == null) {
				//RgssOpen("Data/berryplants.dat","rb"){|f|
				//   @berryPlantData=Marshal.load(f);
				//}
			}
			//if (@berryPlantData != null && @berryPlantData[(int)item]!=null) return @berryPlantData[(int)item];
			if (@berryPlantData != null && @berryPlantData.ContainsKey((int)item)) return @berryPlantData[(int)item];
			//return new int[] { 3, 15, 2, 5 }; // Hours/stage, drying/hour, min yield, max yield
			return Kernal.BerryData[item];
		}
	}

	public struct PlantBerryEntryData
	{
		/// <summary>
		/// time of last checkup
		/// </summary>
		/// Can be (long)DateTime.Ticks
		public DateTime LastCheck { get; private set; }
		/// <summary>
		/// time planted
		/// </summary>
		/// Can be (long)DateTime.Ticks
		public DateTime PlantDate { get; private set; }
		/// <summary>
		/// item ID of planted berry
		/// </summary>
		public Items Id { get; private set; }
		/// <summary>
		/// growth stage (1-5)
		/// </summary>
		/// default 1, if planted
		/// ToDo: Make into an Enum?
		public byte GrowthStage { get; private set; }
		/// <summary>
		/// seconds alive
		/// </summary>
		public int LifeSpan { get; private set; }
		/// <summary>
		/// watered in this stage?
		/// </summary>
		public bool Watered { get; private set; }
		/// <summary>
		/// dampness value
		/// </summary>
		/// GenIV: total waterings
		public byte Dampness { get; private set; }
		/// <summary>
		/// number of replants
		/// </summary>
		public byte Replants { get; private set; }
		/// <summary>
		/// yield penalty
		/// </summary>
		public byte Penalty { get; private set; }
		/// <summary>
		/// </summary>
		/// ToDo... need more info to confirm logic
		public Items Mulch { get; private set; }

		public PlantBerryEntryData(Items id, byte growthStage, int lifeSpan, DateTime plantDate, byte dampness, byte replants, byte penalty, Items mulch = Items.NONE, DateTime? lastCheck = null)
		{
			Id = id;
			GrowthStage = growthStage;
			LifeSpan = lifeSpan;
			PlantDate = plantDate;
			Dampness = dampness;
			Replants = replants;
			Penalty = penalty;
			Mulch = mulch;
			LastCheck = lastCheck??plantDate;
			Watered = true;
		}

		public PlantBerryEntryData(Items id, byte growthStage, bool watered, DateTime plantDate, byte dampness, byte replants, byte? penalty, Items? mulch = Items.NONE, DateTime? lastCheck = null)
		{
			Id = id;
			GrowthStage = growthStage;
			LifeSpan = 0;
			PlantDate = plantDate;
			Dampness = dampness;
			Replants = replants;
			Penalty = penalty??0;
			Mulch = mulch??Items.NONE;
			LastCheck = lastCheck??plantDate;
			Watered = watered;
		}
	}

/*Events.onSpritesetCreate+=delegate(object sender, EventArgs e) {
   spriteset=e[0];
   viewport=e[1];
   map=spriteset.map;
   foreach (var i in map.events.keys) {
	 if (map.events[i].Name=="BerryPlant") {
	   spriteset.addUserSprite(new BerryPlantMoistureSprite(map.events[i],map,viewport));
	   spriteset.addUserSprite(new BerryPlantSprite(map.events[i],map,viewport));
	 }
   }
}*/

/*public class BerryPlantMoistureSprite {
  public void initialize(@event,map,viewport=null) {
	this.@event=@event;
	@map=map;
	@light = new IconSprite(0,0,viewport);
	@light.ox=16;
	@light.oy=24;
	@oldmoisture=-1;   // -1=none, 0=dry, 1=damp, 2=wet
	updateGraphic();
	@disposed=false;
  }

  public bool disposed { get {
	return @disposed;
  } }

  public void dispose() {
	@light.dispose();
	@map=null;
	@event=null;
	@disposed=true;
  }

  public void updateGraphic() {
	switch (@oldmoisture) {
	case -1:
	  @light.setBitmap("");
	  break;
	case 0:
	  @light.setBitmap("Graphics/Characters/berrytreeDry");
	  break;
	case 1:
	  @light.setBitmap("Graphics/Characters/berrytreeDamp");
	  break;
	case 2:
	  @light.setBitmap("Graphics/Characters/berrytreeWet");
	  break;
	}
  }

  public void update() {
	if (@light == null || @event == null) return;
	int newmoisture=-1;
	if (@event.variable && @event.variable.Length>6 && @event.variable[1]>0) {
	  // Berry was planted, show moisture patch
	  newmoisture=(@event.variable[4]>50) ? 2 : (@event.variable[4]>0) ? 1 : 0;
	}
	if (@oldmoisture!=newmoisture) {
	  @oldmoisture=newmoisture;
	  updateGraphic();
	}
	@light.update();
	//if ((Object.const_defined(:ScreenPosHelper))) { //rescue false
	//  @light.x = ScreenPosHelper.ScreenX(@event);
	//  @light.y = ScreenPosHelper.ScreenY(@event);
	//  @light.zoom_x = ScreenPosHelper.ScreenZoomX(@event);
	//}
	//else {
	  @light.x = @event.screen_x;
	  @light.y = @event.screen_y;
	  @light.zoom_x = 1.0;
	//}
	@light.zoom_y = @light.zoom_x;
	DayNightTint(@light);
  }
}*/

/*public class BerryPlantSprite{
  public const int REPLANTS = 9;
  public void initialize(@event,map,viewport) {
	this.@event=@event;
	@map=map;
	@oldstage=0;
	@disposed=false;
	berryData=@event.variable;
	if (berryData == null) return;
	@oldstage=berryData[0];
	@event.character_name="";
	berryData=updatePlantDetails(berryData);
	setGraphic(berryData,true);      // Set the event's graphic
	@event.setVariable(berryData);   // Set new berry data
  }

  public void dispose() {
	@event=null;
	@map=null;
	@disposed=true;
  }

  public bool disposed { get {
	@disposed;
  } }

	// Constantly updates, used only to immediately
  public void update() {
	berryData=@event.variable;     // change sprite when planting/picking berries
	if (berryData != null) {
	  if (berryData.Length>6) berryData=updatePlantDetails(berryData);
	  setGraphic(berryData);
	  @event.setVariable(berryData);
	}
  }

  public void updatePlantDetails(berryData) {
	if (berryData[0]==0) return berryData;
	berryvalues=Game.GameData.PokemonTemp.GetBerryPlantData(berryData[1]);
	int timeperstage=berryvalues[0]*3600;
	DateTime timenow=GetTimeNow();
	if (berryData.Length>6) {
	  // Gen 4 growth mechanisms
	  // Check time elapsed since last check
	  int timeDiff=(timenow.to_i-berryData[3]);   // in seconds
	  if (timeDiff<=0) return berryData;
	  berryData[3]=timenow.to_i;   // last updated now
	  // Mulch modifiers
	  int dryingrate=berryvalues[1];
	  int maxreplants=REPLANTS;
	  int ripestages=4;
	  if (berryData[7] == Items.GROWTHMULCH) {
		timeperstage=(timeperstage*0.75).to_i;
		dryingrate=(dryingrate*1.5).ceil;
	  } else if (berryData[7] == Items.DAMPMULCH) {
		timeperstage=(timeperstage*1.25).to_i;
		dryingrate=(int)Math.Floor(dryingrate*0.5);
	  } else if (berryData[7] == Items.GOOEYMULCH) {
		maxreplants=(maxreplants*1.5).ceil;
	  } else if (berryData[7] == Items.STABLEMULCH) {
		ripestages=6;
	  }
	  // Cycle through all replants since last check
	  do { //loop;
		int secondsalive=berryData[2];
		int growinglife=(berryData[5]>0) ? 3 : 4; // number of growing stages
		int numlifestages=growinglife+ripestages; // number of growing + ripe stages
		// Should replant itself?
		if (secondsalive+timeDiff>=timeperstage*numlifestages) {
		  // Should replant
		  if (berryData[5]>=maxreplants) {		// Too many replants
			return [0,0,0,0,0,0,0,0];
		  }
		  // Replant
		  berryData[0]=2;   // replants start in sprouting stage
		  berryData[2]=0;   // seconds alive
		  berryData[5]+=1;  // add to replant count
		  berryData[6]=0;   // yield penalty
		  timeDiff-=(timeperstage*numlifestages-secondsalive);
		}
		else {
		  break;
		}
	  } while (true);
	  // Update current stage and dampness
	  if (berryData[0]>0) {
		// Advance growth stage
		int oldlifetime=berryData[2];
		int newlifetime=oldlifetime+timeDiff;
		if (berryData[0]<5) {
		  berryData[0]=1+(int)Math.Floor(newlifetime/timeperstage);
		  if (berryData[5]>0  ) berryData[0]+=1;	// replants start at stage 2
		  if (berryData[0]>5) berryData[0]=5;
		}
		// Update the "seconds alive" counter
		berryData[2]=newlifetime;
		// Reduce dampness, apply yield penalty if dry
		int growinglife=(berryData[5]>0) ? 3 : 4; // number of growing stages
		int oldhourtick=(int)Math.Floor(oldlifetime/3600);
		int newhourtick=(int)Math.Floor(((int)Math.Min(newlifetime,timeperstage*growinglife))/3600);
		int n = 0; do { //(newhourtick-oldhourtick).times
		  if (berryData[4]>0) {
			berryData[4]=(int)Math.Max(berryData[4]-dryingrate,0);
		  }
		  else {
			berryData[6]+=1;
		  }
		} while (n < newhourtick-oldhourtick);
	  }
	}
	else {
		// Gen 3 growth mechanics
		do { //loop;
			if (berryData[0]>0 && berryData[0]<5) {
				int levels=0;
				// Advance time
				int timeDiff=(timenow.to_i-berryData[3]); // in seconds
				if (timeDiff>=timeperstage) {
					levels+=1;
					if (timeDiff>=timeperstage*2) {
						levels+=1;
						if (timeDiff>=timeperstage*3) {
							levels+=1;
							if (timeDiff>=timeperstage*4) {
								levels+=1;
							}
						}
					}
				}
				if (levels>5-berryData[0]) levels=5-berryData[0];
				if (levels==0) break;
				berryData[2]=false;						// not watered this stage
				berryData[3]+=levels*timeperstage;		// add to time existed
				berryData[0]+=levels;					// increase growth stage
				if (berryData[0]>5) berryData[0]=5;
			}
			if (berryData[0]>=5) {
				// Advance time
				int timeDiff=(timenow.to_i-berryData[3]);	// in seconds
				if (timeDiff>=timeperstage*4) {				// ripe for 4 times as long as a stage
					// Replant
					berryData[0]=2;							// replants start at stage 2
					berryData[2]=false;						// not watered this stage
					berryData[3]+=timeperstage*4;			// add to time existed
					berryData[4]=0;							// reset total waterings count
					berryData[5]+=1;						// add to replanted count
					if (berryData[5]>REPLANTS) {			// Too many replants
						berryData=[0,0,false,0,0,0];
						break;
					}
				}
				else {
					break;
				}
			}
		} while (true);
		// Check auto-watering
		if (berryData[0]>0 && berryData[0]<5) {
			// Reset watering
			if (Game.GameData.GameScreen != null &&
				(Game.GameData.GameScreen.weather_type==FieldWeathers.Rain ||
				Game.GameData.GameScreen.weather_type==FieldWeathers.HeavyRain ||
				Game.GameData.GameScreen.weather_type==FieldWeathers.Storm)) {
				// If raining, plant is already watered
				if (berryData[2]==false) {
					berryData[2]=true;
					berryData[4]+=1;
				}
			}
		}
	}
	return berryData;
}

  public void setGraphic(berryData,bool fullcheck=false) {
	if (berryData == null || (@oldstage==berryData[0] && !fullcheck)) return;
	switch (berryData[0]) {
	case 0:
	  @event.character_name="";
	  break;
	case 1:
	  @event.character_name="berrytreeplanted";		// Common to all berries
	  @event.turn_down(); //face camera?
	  break;
	default:
	  string filename=string.Format("berrytree{0}",getConstantName(Items,berryData[1])); //rescue null
	  if (!ResolveBitmap("Graphics/Characters/"+filename)) filename=string.Format("berrytree%03d",berryData[1]);
	  if (ResolveBitmap("Graphics/Characters/"+filename)) {
		@event.character_name=filename;
		switch (berryData[0]) {
		case 2: @event.turn_down();		// X sprouted
		  break;
		case 3: @event.turn_left();		// X taller
		  break;
		case 4: @event.turn_right();	// X flowering
		  break;
		case 5: @event.turn_up();		// X berries
		  break;
		}
	  }
	  else {
		@event.character_name="Object ball";
	  }
	  if (@oldstage!=berryData[0] && berryData.Length>6) {		// Gen 4 growth mechanisms
		if (Game.GameData.Scene.spriteset) Game.GameData.Scene.spriteset.addUserAnimation(Core.PLANT_SPARKLE_ANIMATION_ID,@event.x,@event.y);
	  }
	  break;
	}
	@oldstage=berryData[0];
	if (Input.trigger(Input.CTRL)) Core.Logger?.Log(string.Format("here: {0},{1}",berryData.ToString(),@oldstage)); //p
  }
}*/

	public partial class Game : IGameBerryPlants {
		event Action<object, PokemonEssentials.Interface.EventArg.IOnSpritesetCreateEventArgs> IGameBerryPlants.OnSpritesetCreate { add { OnSpritesetCreateEvent += value; } remove { OnSpritesetCreateEvent -= value; } }
		public IBerryPlantMoistureSprite BerryPlantMoistureSprite { get; set; }
		public IBerryPlantSprite BerryPlantSprite { get; set; }

		public void BerryPlant() {
			IInterpreterFieldMixin interp=null;
			if (this is IGameMessage gm) interp=(IInterpreterFieldMixin)gm.MapInterpreter();
			IGameCharacter thisEvent=((IInterpreter)interp).get_character(0);
			PlantBerryEntryData berryData=(PlantBerryEntryData)interp.getVariable();
			if (Input.trigger(PokemonUnity.Input.CTRL)) Core.Logger?.Log(berryData.ToString()); //p berryData;
			//if (berryData == null) {
			//	if (Core.NEWBERRYPLANTS) {
			//		berryData=[0,0,0,0,0,0,0,0];
			//	}
			//	else {
			//		berryData=[0,0,false,0,0,0];
			//	}
			//}
			// Stop the event turning towards the player
			switch (berryData.GrowthStage) {	//Stage
				case 1: thisEvent.turn_down();	// X planted
					break;
				case 2: thisEvent.turn_down();	// X sprouted
					break;
				case 3: thisEvent.turn_left();	// X taller
					break;
				case 4: thisEvent.turn_right();	// X flowering
					break;
				case 5: thisEvent.turn_up();	// X berries
					break;
			}
			List<Items> watering=new List<Items>();
			watering.Add(Items.SPRAYDUCK);
			watering.Add(Items.SQUIRT_BOTTLE);
			watering.Add(Items.WAILMER_PAIL);
			watering.Add(Items.SPRINKLOTAD);
			//watering.compact();
			Items berry=berryData.Id;				//[1];
			switch (berryData.GrowthStage) {		//[0]
				case 0:  // empty
					int cmd = 0;
					if (Core.NEWBERRYPLANTS && this is IGameMessage gm6) { //if no message output then just skip entirely...
						// Gen 4 planting mechanics
						if (berryData.Mulch==0) {		//[7] No mulch used yet | berryData.Mulch==null ||
							cmd=gm6.Message(Game._INTL("It's soft, earthy soil."),new string[] {
											Game._INTL("Fertilize"),
											Game._INTL("Plant Berry"),
											Game._INTL("Exit") },-1);
							if (cmd==0) {		// Fertilize
								Items ret=0;
								FadeOutIn(99999, block:() => {
									IBagScene scene=Game.GameData.Scenes.Bag.initialize();//new PokemonBag_Scene();
									IBagScreen screen=Game.GameData.Screens.Bag.initialize(scene,Game.GameData.Bag);//new PokemonBagScreen(scene,Game.GameData.Bag);
									berry=screen.ChooseBerryScreen();
								});
								if (ret>0) {
									if (IsMulch(ret)) { //this is IGameItem gi && gi
										//berryData.Mulch=ret;
										gm6.Message(Game._INTL("The {1} was scattered on the soil.",ret.ToString(TextScripts.Name)));
										if (gm6.ConfirmMessage(Game._INTL("Want to plant a Berry?"))) {
											FadeOutIn(99999, block: () => {
											   IBagScene scene=Game.GameData.Scenes.Bag.initialize();//new PokemonBag_Scene();
											   IBagScreen screen=Game.GameData.Screens.Bag.initialize(scene,Game.GameData.Bag);//new PokemonBagScreen(scene,Game.GameData.Bag);
											   berry=screen.ChooseBerryScreen();
											});
											if (berry>0) {
												DateTime timenow=Game.GetTimeNow;
												berryData = new PlantBerryEntryData(
													berry,				//[1] item ID of planted berry
													1,					//[0] growth stage (1-5)
													0,					//[2] seconds alive
													timenow,			//[3] time of last checkup (now)
													100,				//[4] dampness value
													0,					//[5] number of replants
													0);					//[6] yield penalty
												Game.GameData.Bag.DeleteItem(berry,1);
												gm6.Message(Game._INTL("The {1} was planted in the soft, earthy soil.",
													berry.ToString(TextScripts.Name)));
											}
										}
										interp.setVariable(berryData);
									}
									else {
										gm6.Message(Game._INTL("That won't fertilize the soil!"));
									}
									return;
								}
							} else if (cmd==1) {		// Plant Berry
								FadeOutIn(99999, block: () => {
									IBagScene scene=Game.GameData.Scenes.Bag.initialize();//new PokemonBag_Scene();
									IBagScreen screen=Game.GameData.Screens.Bag.initialize(scene,Game.GameData.Bag);//new PokemonBagScreen(scene,Game.GameData.Bag);
									berry=screen.ChooseBerryScreen();
								});
								if (berry>0) {
									DateTime timenow=Game.GetTimeNow;
									berryData = new PlantBerryEntryData(
										berry,			//[1] item ID of planted berry
										1,				//[0] growth stage (1-5)
										0,				//[2] seconds alive
										timenow,		//[3] time of last checkup (now)
										100,			//[4] dampness value
										0,				//[5] number of replants
										0);				//[6] yield penalty
									Game.GameData.Bag.DeleteItem(berry,1);
									gm6.Message(Game._INTL("The {1} was planted in the soft, earthy soil.",
										berry.ToString(TextScripts.Name)));
									interp.setVariable(berryData);
								}
								return;
							}
						}
						else {
							gm6.Message(Game._INTL("{1} has been laid down.",berryData.Mulch.ToString(TextScripts.Name))); //[7]
							if (gm6.ConfirmMessage(Game._INTL("Want to plant a Berry?"))) {
								FadeOutIn(99999, block: () => {
									IBagScene scene=Game.GameData.Scenes.Bag.initialize();//new PokemonBag_Scene();
									IBagScreen screen=Game.GameData.Screens.Bag.initialize(scene,Game.GameData.Bag);//new PokemonBagScreen(scene,Game.GameData.Bag);
									berry=screen.ChooseBerryScreen();
								});
								if (berry>0) {
									DateTime timenow=Game.GetTimeNow;
									berryData = new PlantBerryEntryData(
										berry,			//[1] item ID of planted berry
										1,				//[0] growth stage (1-5)
										0,				//[2] seconds alive
										timenow,		//[3] time of last checkup (now)
										100,			//[4] dampness value
										0,				//[5] number of replants
										0);				//[6] yield penalty
									Game.GameData.Bag.DeleteItem(berry,1);
									gm6.Message(Game._INTL("The {1} was planted in the soft, earthy soil.",
										berry.ToString(TextScripts.Name)));
									interp.setVariable(berryData);
								}
								return;
							}
						}
					}
					else {
						// Gen 3 planting mechanics
						if (this is IGameMessage gm2 && gm2.ConfirmMessage(Game._INTL("It's soft, loamy soil.\nPlant a berry?"))) {
							FadeOutIn(99999, block: () => {
								IBagScene scene=Game.GameData.Scenes.Bag.initialize();//new PokemonBag_Scene();
								IBagScreen screen=Game.GameData.Screens.Bag.initialize(scene,Game.GameData.Bag);//new PokemonBagScreen(scene,Game.GameData.Bag);
								berry=screen.ChooseBerryScreen();
							});
							if (berry>0) {
								DateTime timenow=Game.GetTimeNow;
								berryData = new PlantBerryEntryData(
									berry,			//[1] item ID of planted berry
									1,				//[0] growth stage (1-5)
									false,			//[2] watered in this stage?
									timenow,		//[3] time planted
									0,				//[4] total waterings
									0,				//[5] number of replants
									null,			//[6] yield penalty
									null);			//[7] No mulch used yet
								//berryData.compact(); // for compatibility
								Game.GameData.Bag.DeleteItem(berry,1);
								gm2.Message(Game._INTL("{1} planted a {2} in the soft loamy soil.",
									Game.GameData.Trainer.name,berry.ToString(TextScripts.Name)));
								interp.setVariable(berryData);
							}
							return;
						}
					}
					break;
				case 1: // X planted
					if (this is IGameMessage gm3) gm3.Message(Game._INTL("A {1} was planted here.",berry.ToString(TextScripts.Name)));
					break;
				case 2:  // X sprouted
					if (this is IGameMessage gm4) gm4.Message(Game._INTL("The {1} has sprouted.",berry.ToString(TextScripts.Name)));
					break;
				case 3:  // X taller
					if (this is IGameMessage gm5) gm5.Message(Game._INTL("The {1} plant is growing bigger.",berry.ToString(TextScripts.Name)));
					break;
				case 4:  // X flowering
					if (Core.NEWBERRYPLANTS) {
						if (this is IGameMessage gm2) gm2.Message(Game._INTL("This {1} plant is in bloom!",berry.ToString(TextScripts.Name)));
					}
					else {
						if (this is IGameMessage gm2) {
							switch (berryData.Dampness) { //berryData[4]
								case 4:
									gm2.Message(Game._INTL("This {1} plant is in fabulous bloom!",berry.ToString(TextScripts.Name)));
									break;
								case 3:
									gm2.Message(Game._INTL("This {1} plant is blooming very beautifully!",berry.ToString(TextScripts.Name)));
									break;
								case 2:
									gm2.Message(Game._INTL("This {1} plant is blooming prettily!",berry.ToString(TextScripts.Name)));
									break;
								case 1:
									gm2.Message(Game._INTL("This {1} plant is blooming cutely!",berry.ToString(TextScripts.Name)));
									break;
								default:
									gm2.Message(Game._INTL("This {1} plant is in bloom!",berry.ToString(TextScripts.Name)));
									break;
							}
						}
					}
					break;
				case 5:  // X berries
					BerryData berryvalues = Kernal.BerryData[berryData.Id]; //Game.GameData.PokemonTemp.GetBerryPlantData(berryData.Id);
					if (Game.GameData.PokemonTemp is ITempMetadataBerryPlants tbp) tbp.GetBerryPlantData(berryData.Id);	//[1]
					// Get berry yield (berrycount)
					int berrycount=1;
					if (Core.NEWBERRYPLANTS) { //berryData.Length>6
						// Gen 4 berry yield calculation
						//berrycount=(int)Math.Max(berryvalues[3]-berryData[6],berryvalues[2]);
						berrycount=(int)Math.Max(berryvalues.maxBerries-berryData.Penalty,berrycount);
					}
					else {
						// Gen 3 berry yield calculation
						if (berryData.Dampness>0) { //berryData[4]>0
							//int randomno=Core.Rand.Next(1+berryvalues[3]-berryvalues[2]);
							//berrycount=(int)Math.Floor(((berryvalues[3]-berryvalues[2])*(berryData[4]-1)+randomno)/4)+berryvalues[2];
							int randomno=Core.Rand.Next(1+berryvalues.maxBerries-berrycount);
							berrycount=(int)Math.Floor(((berryvalues.maxBerries-berrycount)*(berryData.Dampness-1f)+randomno)/4f)+berrycount;
						}
						else {
							berrycount=berryvalues.maxBerries; //berryvalues[2]
						}
					}
					string itemname=(berrycount>1) ? berry.ToString(TextScripts.NamePlural) : berry.ToString(TextScripts.Name);
					string message = string.Empty;
					if (berrycount>1) {
						message=Game._INTL("There are {1} {2}!\nWant to pick them?",berrycount,itemname);
					}
					else {
						message=Game._INTL("There is 1 {1}!\nWant to pick it?",itemname);
					}
					if (this is IGameMessage gm1 && gm1.ConfirmMessage(message)) {
						if (!Game.GameData.Bag.CanStore(berry,berrycount)) {
							gm1.Message(Game._INTL("Too bad...\nThe bag is full."));
							return;
						}
						Game.GameData.Bag.StoreItem(berry,berrycount);
						if (berrycount>1) {
							gm1.Message(Game._INTL("You picked the {1} {2}.\\wtnp[30]",berrycount,itemname));
						}
						else {
							gm1.Message(Game._INTL("You picked the {1}.\\wtnp[30]",itemname));
						}
						gm1.Message(Game._INTL("{1} put away the {2} in the <icon=bagPocket#{BERRYPOCKET}>\\c[1]Berries\\c[0] Pocket.\\1",
							Game.GameData.Trainer.name,itemname));
						if (Core.NEWBERRYPLANTS) {
							gm1.Message(Game._INTL("The soil returned to its soft and earthy state.\\1"));
							//berryData=new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
							berryData=new PlantBerryEntryData();
						}
						else {
							gm1.Message(Game._INTL("The soil returned to its soft and loamy state.\\1"));
							//berryData=new int[] { 0, 0, false, 0, 0, 0 };
							berryData=new PlantBerryEntryData();
						}
						//interp.setVariable(berryData);
					}
					break;
			}
			switch (berryData.GrowthStage) {
				case 1: case 2: case 3: case 4:
					foreach (Items i in watering) {
						if (i!=0 && Game.GameData.Bag.Quantity(i)>0) { //GetItemAmount
							if (this is IGameMessage gm1 && gm1.ConfirmMessage(Game._INTL("Want to sprinkle some water with the {1}?",i.ToString(TextScripts.Name)))) {
								if (Core.NEWBERRYPLANTS) { //berryData.Length>6
									// Gen 4 berry watering mechanics
									//berryData[4]=100;
									berryData = new PlantBerryEntryData(
									berryData.Id,				//[1] item ID of planted berry
									berryData.GrowthStage,		//[0] growth stage (1-5)
									berryData.LifeSpan,         //[2] seconds alive
									berryData.PlantDate,		//[3] time planted
									100,						//[4] total waterings
									berryData.Replants,			//[5] number of replants
									berryData.Penalty);			//[6] yield penalty
								}
								else {
									// Gen 3 berry watering mechanics
									if (!berryData.Watered) { //[2]==false
										//berryData[4]+=1;
										//berryData[2]=true;
										berryData = new PlantBerryEntryData(
										berryData.Id,					//[1] item ID of planted berry
										berryData.GrowthStage,			//[0] growth stage (1-5)
										true,							//[2] watered in this stage?
										berryData.PlantDate,			//[3] time planted
										(byte)(berryData.Dampness+1),	//[4] total waterings
										berryData.Replants,				//[5] number of replants
										berryData.Penalty,				//[6] yield penalty
										berryData.Mulch);				//[7] No mulch used yet
									}
								}
								//interp.setVariable(berryData);
								gm1.Message(Game._INTL("{1} watered the plant.\\wtnp[40]",Game.GameData.Trainer.name));
								if (Core.NEWBERRYPLANTS) {
									gm1.Message(Game._INTL("There! All happy!"));
								}
								else {
									gm1.Message(Game._INTL("The plant seemed to be delighted."));
								}
							}
							break;
						}
					}
					break;
			}
		}

		public void PickBerry(Items berry,int qty=1) {
			IInterpreterFieldMixin interp=null;
			if (this is IGameMessage gm) interp=(IInterpreterFieldMixin) gm.MapInterpreter();
			IGameCharacter thisEvent=((IInterpreter)interp).get_character(0);
			PlantBerryEntryData berryData=(PlantBerryEntryData)interp.getVariable();
			//if (berry.is_a(String) || berry.is_a(Symbol)) {
			//  berry=getID(Items,berry);
			//}
			string itemname=(qty>1) ? berry.ToString(TextScripts.NamePlural) : berry.ToString(TextScripts.Name);
			string message = string.Empty;
			if (qty>1) {
				message=Game._INTL("There are {1} {2}!\nWant to pick them?",qty,itemname);
			}
			else {
				message=Game._INTL("There is 1 {1}!\nWant to pick it?",itemname);
			}
			if (this is IGameMessage gm1 && gm1.ConfirmMessage(message)) {
				if (!Game.GameData.Bag.CanStore(berry,qty)) {
					gm1.Message(Game._INTL("Too bad...\nThe bag is full."));
					return;
				}
				Game.GameData.Bag.StoreItem(berry,qty);
				//Game.GameData.Player.Bag.AddItem(berry,qty);
				ItemPockets pocket=Kernal.ItemData[berry].Pocket??ItemPockets.BERRY;
				if (qty>1) {
					gm1.Message(Game._INTL("You picked the {1} {2}.\\wtnp[30]",qty,itemname));
				}
				else {
					gm1.Message(Game._INTL("You picked the {1}.\\wtnp[30]",itemname));
				}
				gm1.Message(Game._INTL($"{{1}} put away the {{2}} in the <icon=bagPocket#{pocket}>\\c[1]Berries\\c[0] Pocket.\\1",
					Game.GameData.Trainer.name,itemname));
				if (Core.NEWBERRYPLANTS) {
					gm1.Message(Game._INTL("The soil returned to its soft and earthy state.\\1"));
					//berryData=[0,0,0,0,0,0,0,0];
					berryData = new PlantBerryEntryData();
				}
				else {
					gm1.Message(Game._INTL("The soil returned to its soft and loamy state.\\1"));
					//berryData=[0,0,false,0,0,0];
					berryData = new PlantBerryEntryData();
				}
				interp.setVariable(berryData);
				if (((Game)Game.GameData).Interpreter is IInterpreterMixinMessage im) im.SetSelfSwitch(thisEvent.id,"A",true);
			}
		}

		protected virtual void OnSpritesetCreate(object sender, OnSpritesetCreateEventArgs e) {
			ISpritesetMap spriteset = e.SpritesetId;//e[0];
			IViewport viewport = e.Viewport; //e[1];
			IGameMap map=spriteset.map;
			foreach (int i in map.events.Keys) {
				if (map.events[i] is IGameEvent ge && ge.name=="BerryPlant") {
					//spriteset.addUserSprite(new BerryPlantMoistureSprite(map.events[i],map,viewport));
					//spriteset.addUserSprite(new BerryPlantSprite(map.events[i],map,viewport));
					spriteset.addUserSprite(BerryPlantMoistureSprite.initialize(map.events[i],map,viewport));
					spriteset.addUserSprite(BerryPlantSprite.initialize(map.events[i],map,viewport));
				}
			}
		}
	}
}
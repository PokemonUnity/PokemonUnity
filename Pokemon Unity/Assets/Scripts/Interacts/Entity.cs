using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;
using PokemonUnity.Utility;

namespace PokemonUnity.Character
{
	//ToDo: Somehow manage to combine the two set of information into one complete class logic
	public class Entities
	{		   
		/// <summary>
		/// An event with this text in its name is treated as occupying multiple tiles at once(in this case, a rectangle 2 tiles wide and and 3 tiles tall). The event's rectangle is positioned such that its bottom left corner is where the event has been placed in the map.
		/// A "large" event behaves as you would expect: no part of it can move onto a non-passable tile or another event (unless its "Through" setting is set), the player and other events cannot move into any part of the large event, and the player can interact with any part of it. If its event name contains "Trainer(3)" or "Counter(3)", it can detect whether the player moves into any of the tiles in front of any part of it.
		/// </summary>
		///If the event is given a graphic which is a tile from the tileset, its graphic will be a set of tiles equal to its size, with the chosen tile being the bottom left one of that set. For example, you can make an event depicting a rock of size(2,2) by using this text to make it that size, giving it a graphic of the bottom left tile of that rock from the tileset, and placing the event where the bottom left corner of the rock sits in the map. The the whole event will use the passability/terrain tag/other tile properties of just the selected tile; if the other tiles used by the event in its graphic have different tile properties, they will be ignored.
		public const string SIZE = "Size({0},{1})";
	
		/// <summary>
		/// The event should be an item ball.
		/// The SOMETHING is the ID of the item contained in that item ball.
		/// The Compiler will rewrite events named like this and turn them into proper item ball events with all appropriate commands; 
		/// the name of the event will also be changed to "Item" (which has no special effect by itself).
		/// </summary>
		///The "Item" part is case-insensitive. This text must be the whole of the event's name.
		public const string ITEMBALL = "Item:{0}";
	 
		/// <summary>
		/// The event should be a hidden item ball. The SOMETHING is the ID of the item contained in that hidden item ball. The Compiler will rewrite events named like this and turn them into proper hidden item ball events with all appropriate commands; the name of the event will also be changed to "HiddenItem".
		/// </summary>
		/// The "HiddenItem" part is case-insensitive. This text must be the whole of the event's name.
		public const string HIDDENITEM_SOMETHING = "HiddenItem:{0}";
 
		/// <summary>
		/// An event with this text in its name can be detected by the Itemfinder. Also, the event cannot be interacted with while standing on it. Note the lack of a space between the two words.
		/// </summary>
		public const string HIDDENITEM = "HiddenItem";
 
		/// <summary>
		/// An event with this text in its name will be reset by the Debug mode function "Reset Map's Trainers" (i.e. the event's Self Switches A and B will be turned off). Also, if using trainer comments to create a trainer event, and the event's name doesn't already contain this text, the event's name will be changed to "Trainer(3)". This text has no other effect.
		/// </summary>
		public const string TRAINERDEBUG = "Trainer";
	
		/// <summary>
		/// An event with this text in its name has a "line of sight", which is a set of tiles in front of it that it can "see".If the player walks into this line of sight, the event will be triggered as though it was interacted with. The event must be able to walk in a straight line up to the player(onto the tile next to the player that is between them) in order to be triggered; if there is an impassible tile /event between them, it will not be triggered. An event using this functionality must also have the "Event Touch" trigger.
		/// The number in the brackets is how far in front of itself the event can see(in tiles). It can be any number greater than 0.
		/// </summary>
		/// This text is named because it is usually NPC trainers who behave like this.However, this text can be used for non - trainer events as well.
		public const string TRAINER = "Trainer({0})";
  
		/// <summary>
		/// An event with this text in its name has a "line of sight", which is a set of tiles in front of it that it can "see".If the player walks into this line of sight, the event will be triggered as though it was interacted with.The event must be looking in the direction of the player in order to be triggered; unlike the "Trainer(3)" text, this text does not require the event to be able to walk up to the player in order to trigger.An event using this functionality must also have the "Event Touch" trigger.
		/// The number in the brackets is how far in front of itself the event can see(in tiles). It can be any number greater than 0.
		/// </summary>
		/// This text is named because it is usually used for events standing behind desks or counters that need to detect when the player tries to walk past(e.g.a guard in a gatehouse at the end of a Cycling Road, who will prevent the player from proceeding if they don't have a Bicycle).
		public const string COUNTER = "Counter({0})";
 
		/// <summary>
		/// This event can have a berry planted in it.Specifically, this text ensures that the event shows an appropriate berry tree graphic and a patch of watered(or dried out) soil beneath it.The script command pbBerryPlant, run by interacting with the event, is responsible for the actual planting and subsequent interactions with the plant(i.e.to water it and to pick the berries once fully grown).
		/// </summary>
		public const string BERRYPLANT = "BerryPlant";
   
		/// <summary>
		/// The event will display a graphic from the Graphics/ Pictures folder above it.This graphic is "LE.png" by default, but you can name the event "Light(otherlight)", where the text in brackets is the name of the graphic to use.The graphic is centered on the middle of the tile the event is in (assuming the graphic is 64x64 pixels in size).
		/// </summary>
		public const string LIGHT = "Light";
 
		/// <summary>
		/// The event will display a graphic from the Graphics/ Pictures folder above it.This graphic is "LE.png" by default, but you can name the event "OutdoorLight(otherlight)", where the text in brackets is the name of the graphic to use.The graphic is centered on the middle of the tile the event is in (assuming the graphic is 64x64 pixels in size).
		/// </summary>
		/// The opacity of the graphic depends on the time of day.It is fully transparent during the day, is semi - transparent in the morning and evening, and is fully visible at night.
		public const string OUTDOORLIGHT = "OutdoorLight";
 
		/// <summary>
		/// This event will have a reflection(usually seen in still water). Having a reflection is opt -in, and by default only the player has a reflection. If the event is on / just north of still water, or can move to that position, it should use this text to ensure it shows a reflection in the water.
		/// </summary>
		/// This text can have a number in brackets after it, e.g. "Reflection(2)".If so, the reflection will be shifted south by a distance of half this number of tiles, and the shadow will be colored a solid blue rather than be semi - transparent.This effect is for events that are standing on a bridge over still water; the same effect is applied to the player's shadow if the player is on a bridge. The number is how many layers of cliffs the bridge is above the water.
		public const string REFLECTION = "Reflection";
 
		/// <summary>
		/// This event can be destroyed by using Cut on it.If standing in front of this event, Cut will be shown as an option in the Ready menu, and Cut can be used from the party screen.It also ensures that the correct sound effect is played when the event is destroyed(because Rock Smash uses the same code but plays a different sound effect).
		/// </summary>
		/// This text used to be just "Tree" and this had to be the whole of the event's name. As it can now be just part of the event's name, this text was renamed to ensure it wouldn't appear in an event's name by accident. The Compiler will check for events named just "Tree" and will rename them to "CutTree".
		public const string CUTTREE = "CutTree";
   
		/// <summary>
		/// This event can be destroyed by using Rock Smash on it. If standing in front of this event, Rock Smash will be shown as an option in the Ready menu, and Rock Smash can be used from the party screen.It also ensures that the correct sound effect is played when the event is destroyed(because Cut uses the same code but plays a different sound effect).
		/// </summary>
		/// This text used to be just "Rock" and this had to be the whole of the event's name. As it can now be just part of the event's name, this text was renamed to ensure it wouldn't appear in an event's name by accident. The Compiler will check for events named just "Rock" and will rename them to "SmashRock".
		public const string SMASHROCK = "SmashRock";
 
		/// <summary>
		/// This event can be moved by using Strength and walking into it.Interacting with an event that has this text in its name will prompt the player to use Strength if they haven't already used it on this map.
		/// </summary>
		/// This text used to be just "Boulder" and this had to be the whole of the event's name. As it can now be just part of the event's name, this text was renamed to ensure it wouldn't appear in an event's name by accident. The Compiler will check for events named just "Boulder" and will rename them to "StrengthBoulder".
		public const string STRENGTHBOULDER = "StrengthBoulder";
	
		/// <summary>
		/// This event can be knocked by using Headbutt on it, potentially triggering a wild encounter.If standing in front of this event, Headbutt will be shown as an option in the Ready menu, and Headbutt can be used from the party screen.
		/// </summary>
		public const string HEADBUTTTREE = "HeadbuttTree";

		/// <summary>
		/// An event with this text in its name will always update itself, even if it is off - screen and ordinarily wouldn't be updated.
		/// </summary>
		public const string UPDATE = "Update";
 
		/// <summary>
		/// An event with this text in its name will not be shaded according to the time of day. It can be used for events depicting lit windows of buildings and so forth.Note the lack of a space between the two words.
		/// </summary>
		public const string REGULARTONE = "RegularTone";

		/// <summary>
		/// </summary>
		/// <param name="id">Positive Id is above ground npc entity, and Negative Id is special map tile?</param>
		/// <param name="name"></param>
		protected Entities(int id, string name) : base(id, name) { }
		public static readonly Entities Floor = new Entities(0, "Entities_Text_Floor");//Default?...
		/// <summary>
		/// Renders sides from all directions.
		/// </summary>
		public static readonly Entities AllSidesObject = new Entities(0, "Entities_Text_AllSidesObject");// or 
		public static readonly Entities Cube = new Entities(0, "Entities_Text_Cube");
		/// <summary>
		/// Creates an Apricorn Plant.
		/// <para></para>
		/// An integer defining which Apricorn is used.
		/// </summary>
		/// 0 - White
		/// 1 - Black
		/// 2 - Pink
		/// 3 - Blue
		/// 4 - Red
		/// 5 - Green
		/// 6 - Yellow
		public static readonly Entities ApricornPlant = new Entities(0, "Entities_Text_ApricornPlant");
		public static readonly Entities BerryPlant = new Entities(0, "Entities_Text_BerryPlant");
		/// <summary>
		/// Allows the player to use cut on the Entity.
		/// </summary>
		public static readonly Entities CutTree = new Entities(0, "Entities_Text_CutTree");
		/// <summary>
		/// Creates long grass where the player may run into wild pokemon.
		/// <para></para>
		/// An integer defining properties of grass.
		/// </summary>
		public static readonly Entities Grass = new Entities(0, "Entities_Text_Grass");
		/// <summary>
		/// Allows the player to use headbutt on the Entity.
		/// </summary>
		public static readonly Entities HeadbuttTree = new Entities(0, "Entities_Text_HeadbuttTree");
		/// <summary>
		/// Creates an item, if the "Action" tag = 1 then it is a "hidden" item.
		/// <para></para>
		/// int,int - the first value is the ID for the item on that map(each item should have a unique ID) The second is the item's ID.
		/// </summary>
		public static readonly Entities ItemObject = new Entities(0, "Entities_Text_ItemObject");
		/// <summary>
		/// Creates a spot to grow a berry in.
		/// </summary>
		public static readonly Entities LoamySoil = new Entities(0, "Entities_Text_LoamySoil");
		/// <summary>
		/// Creates a block to trigger a script when it is either walked on or clicked on.
		/// <para></para>
		/// Depending on the "Action" tag the way the "AdditionalValue" tag is interpreted is changed
		/// </summary>
		/// 0 - Activates the given script when walked on. Automatically invisible.
		/// 1 - Activates the given script when clicked on.
		/// 2 - Displays the given text when clicked on.
		/// 3 - Interprets the given text as a script.
		/// 4 - Activates the given script when walked on.
		public static readonly Entities ScriptBlock = new Entities(0, "Entities_Text_ScriptBlock");
		/// <summary>
		/// Creates an entity that either displays text or activates a script when the player "talks" to it.
		/// <para></para>
		/// Based on the "Action" tag the "AdditionalValue" tag is interpreted differently:
		/// </summary>
		/// 0 - Displays the given text as text.
		/// 1 - Activates the given script.
		/// 2 - Converts the given text to a script.
		public static readonly Entities SignBlock = new Entities(0, "Entities_Text_SignBlock");
		/// <summary>
		/// Creates a stairway.
		/// </summary>
		public static readonly Entities SlideBlock = new Entities(0, "Entities_Text_SlideBlock");
		/// <summary>
		/// Allows the player to use rock smash on the Entity.
		/// </summary>
		public static readonly Entities SmashRock = new Entities(0, "Entities_Text_SmashRock");
		/// <summary>
		/// Creates a ledge the player can lead down.
		/// </summary>
		public static readonly Entities Step = new Entities(0, "Entities_Text_Step");
		/// <summary>
		///  Allows the player to use strength on the Entity.
		/// </summary>
		public static readonly Entities StrengthRock = new Entities(0, "Entities_Text_StrengthRock");
		/// <summary>
		/// Creates a Trigger that activates when a "StrengthRock" is pushed onto it.
		/// <para></para>
		/// bool,bool, str - the bools are for if the rock is removed immediately, and if the rock is nolonger loaded with the map, the string is the script activated when the trigger is activated.
		/// </summary>
		public static readonly Entities StrengthTrigger = new Entities(0, "Entities_Text_StrengthTrigger");
		/// <summary>
		/// A sign that spins at the center.
		/// <para></para>
		/// An int defining what texture is used for the sign.
		/// </summary>
		/// 0 - PokeCenter
		/// 1 - Mart
		/// 2 - Gym Sign
		public static readonly Entities TurningSign = new Entities(0, "Entities_Text_TurningSign");
		/// <summary>
		/// An entity that always shows the same face to the player(spins on the center.
		/// </summary>
		public static readonly Entities WallBill = new Entities(0, "Entities_Text_WallBill");
		/// <summary>
		/// Basic Entity.
		/// </summary>
		public static readonly Entities WallBlock = new Entities(0, "Entities_Text_WallBlock");
		/// <summary>
		/// Creates a warp.
		/// <para></para>
		/// mapfilepath, X, Y, Z, R, E - The mapfile, position, number of 1/4 turns to warp, and directions from which it can be accessed separated by "|".
		/// </summary>
		public static readonly Entities WarpBlock = new Entities(0, "Entities_Text_WarpBlock");
		/// <summary>
		/// Creates a surf spot, ignores texture(s) given.
		/// </summary>
		public static readonly Entities Water = new Entities(0, "Entities_Text_Water");
		/// <summary>
		/// Creates a Waterfall, ignores texture(s) given.
		/// </summary>
		public static readonly Entities Waterfall = new Entities(0, "Entities_Text_Waterfall");
		/// <summary>
		/// Creates a Whirlpool, ignores texture(s) given.
		/// </summary>
		public static readonly Entities Whirlpool = new Entities(0, "Entities_Text_Whirlpool");
		/// <summary>
		/// Creates a dive spot, ignores texture(s) given.
		/// </summary>
		public static readonly Entities DiveTile = new Entities(0, "Entities_Text_DiveTile");
		public static readonly Entities RockClimbEntity = new Entities(0, "Entities_Text_RockClimbEntity");
		public static readonly Entities NPC = new Entities(0, "Entities_Text_NPC");
		public static readonly Entities ModelEntity = new Entities(0, "Entities_Text_ModelEntity");
		public static readonly Entities RotationTile = new Entities(0, "Entities_Text_RotationTile");
		public static readonly Entities AnimatedBlock = new Entities(0, "Entities_Text_AnimatedBlock");
		public static readonly Entities NetworkPokemon = new Entities(0, "Entities_Text_NetworkPokemon");
		public static readonly Entities MessageBulb = new Entities(0, "Entities_Text_MessageBulb");
		public static readonly Entities OverworldPokemon = new Entities(0, "Entities_Text_OverworldPokemon");
		public static readonly Entities OwnPlayer = new Entities(0, "Entities_Text_OwnPlayer");
		public static readonly Entities Particle = new Entities(0, "Entities_Text_Particle");
	}
}
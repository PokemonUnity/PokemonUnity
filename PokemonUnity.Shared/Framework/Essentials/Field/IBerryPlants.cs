using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
    /// <summary>
    /// Used to track individual entity status for berry plants. 
    /// This is what gets saved and loaded to game memory
    /// </summary>
    public interface IBerryMetadata 
    {
        // Hours/stage, drying/hour, min yield, max yield
    }
    public interface ITempMetadataBerryPlants {
        int berryPlantData				{ get; }

        //int[] pbGetBerryPlantData(Items item);
        BerryData pbGetBerryPlantData(Items item);
    }

//Events.onSpritesetCreate+=delegate(object sender, EventArgs e) {
//   spriteset=e[0];
//   viewport=e[1];
//   map=spriteset.map;
//   foreach (var i in map.events.keys) {
//     if (map.events[i].name=="BerryPlant") {
//       spriteset.addUserSprite(new BerryPlantMoistureSprite(map.events[i],map,viewport));
//       spriteset.addUserSprite(new BerryPlantSprite(map.events[i],map,viewport));
//     }
//   }
//}

    public interface IBerryPlantMoistureSprite : IDisposable {
        IBerryPlantMoistureSprite initialize(IGameCharacter @event,IGameMap map,IViewport viewport=null);

        bool disposed();

        void dispose();

        void updateGraphic();

        void update();
    }

    public interface IBerryPlantSprite : IDisposable {
        //REPLANTS = 9;

        IBerryPlantSprite initialize(IGameCharacter @event, IGameMap map,IViewport viewport);

        void dispose();

        bool disposed();

        /// <summary>
        /// Constantly updates, used only to immediately
        /// change sprite when planting/picking berries
        /// </summary>
        IEnumerator update();

        BerryData updatePlantDetails(BerryData berryData);

        void setGraphic(BerryData berryData, bool fullcheck = false);    
    }

    public interface IGameBerryPlants
	{
        void pbBerryPlant();

        void pbPickBerry(Items berry, int qty = 1);
	}
}
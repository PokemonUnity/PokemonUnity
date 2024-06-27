using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Overworld;
using PokemonUnity.Interface;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// Used to track individual entity status for berry plants.
	/// This is what gets saved and loaded to game memory
	/// </summary>
	//ToDo: Berry Field Data (0x18 per tree, 36 trees)
	public interface IBerryMetadata
	{
		// Hours/stage, drying/hour, min yield, max yield
	}

	public interface ITempMetadataBerryPlants : ITempMetadata {
		IDictionary<int, BerryData> berryPlantData				{ get; }

		//int[] GetBerryPlantData(Items item);
		BerryData GetBerryPlantData(Items item);
	}

	public interface IBerryPlantMoistureSprite : ISpriteAnimation, IDisposable {
		IBerryPlantMoistureSprite initialize(IGameCharacter @event,IGameMap map,IViewport viewport=null);

		bool disposed();

		//void dispose();

		void updateGraphic();

		//void update();
	}

	public interface IBerryPlantSprite : ISpriteAnimation, IDisposable {
		//REPLANTS = 9;

		IBerryPlantSprite initialize(IGameCharacter @event, IGameMap map,IViewport viewport);

		//void dispose();

		bool disposed();

		/// <summary>
		/// Constantly updates, used only to immediately
		/// change sprite when planting/picking berries
		/// </summary>
		//IEnumerator update();

		/// <summary>
		/// </summary>
		/// <param name="berryData"></param>
		/// <returns></returns>
		BerryData updatePlantDetails(BerryData berryData);

		void setGraphic(BerryData berryData, bool fullcheck = false);
	}

	public interface IGameBerryPlants : IGame
	{
		void BerryPlant();

		void PickBerry(Items berry, int qty = 1);

		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		//event EventHandler<IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		event Action<object, EventArg.IOnSpritesetCreateEventArgs> OnSpritesetCreate;
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
	}
}
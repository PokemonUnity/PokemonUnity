using PokemonUnity.Inventory;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class LoamySoil : Entity
	{
		public override void Initialize()
		{
			base.Initialize();

			this.Visible = false;
		}

		public override void ClickFunction()
		{
			bool hasBerry = false;
			foreach (Entity Entity in Game.Level.Entities)
			{
				if (Entity.EntityID == Entities.BerryPlant & Entity.Position == this.Position)
				{
					hasBerry = true;
					Entity.ClickFunction();
					break;
				}
			}
			if (!hasBerry)
			{
				Game.TextBox.Show("Do you want to plant a~berry here?%Yes|No%", new Entity[] { this });
				SoundManager.PlaySound("select");
			}
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				//NewInventoryScreen selScreen = new NewInventoryScreen(Core.CurrentScreen, new int[]
				//{
				//    2
				//}, 2, null/* TODO Change to default(_) if this is not a reference type */);
				//selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection;
				//selScreen.CanExit = true;
				//
				//selScreen.SelectedObject += PlantBerryHandler;
				//Core.SetScreen(selScreen);
			}
		}

		public void PlantBerryHandler(params object[] @params)
		{
			PlantBerry((Items)System.Convert.ToInt32(@params[0]));
		}

		public void PlantBerry(Items ChosenBerry)
		{
			Inventory.Item testItem = Inventory.Item.GetItem(ChosenBerry);
			if (testItem.IsBerry != null)
			{
				Inventory.Item.Berry Berry = testItem.IsBerry;//(Inventory.Inventory.Berry)Inventory.Inventory.GetItem(ChosenBerry);

				BerryPlant.AddBerryPlant(Game.Level.LevelFile, this.Position, Berry.BerryIndex);
				Game.TextBox.reDelay = 0.0f;
				Game.TextBox.Show("You planted a~" + /*Berry*/testItem.Name + " Berry here.", null);
			}
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}
	}
}
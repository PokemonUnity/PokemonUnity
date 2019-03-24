using PokemonUnity.Item;

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
        foreach (Entity Entity in Screen.Level.Entities)
        {
            if (Entity.EntityID == Entities.BerryPlant & Entity.Position == this.Position)
            {
                hasBerry = true;
                Entity.ClickFunction();
                break;
            }
        }
        if (hasBerry == false)
        {
            Screen.TextBox.Show("Do you want to plant a~berry here?%Yes|No%", this);
            SoundManager.PlaySound("select");
        }
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            NewInventoryScreen selScreen = new NewInventoryScreen(Core.CurrentScreen, new int[]
            {
                2
            }, 2, null/* TODO Change to default(_) if this is not a reference type */);
            selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection;
            selScreen.CanExit = true;

            selScreen.SelectedObject += PlantBerryHandler;
            Core.SetScreen(selScreen);
        }
    }

    public void PlantBerryHandler(params object[] @params)
    {
        PlantBerry(System.Convert.ToInt32(@params[0]));
    }

    public void PlantBerry(int ChosenBerry)
    {
        Item.Item testItem = Item.Item.GetItem(ChosenBerry);
        if (testItem.isBerry)
        {
            Item.Berry Berry = (Item.Berry)Item.Item.GetItem(ChosenBerry);

            BerryPlant.AddBerryPlant(Screen.Level.LevelFile, this.Position, Berry.BerryIndex);
            Screen.TextBox.reDelay = 0.0F;
            Screen.TextBox.Show("You planted a~" + Berry.Name + " Berry here.", null);
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}
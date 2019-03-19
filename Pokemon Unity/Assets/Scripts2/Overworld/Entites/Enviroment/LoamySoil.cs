using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

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
            if (Entity.EntityID == "BerryPlant" & Entity.Position == this.Position)
            {
                hasBerry = true;
                Entity.ClickFunction();
                break;
            }
        }
        if (hasBerry == false)
        {
            Screen.TextBox.Show("Do you want to plant a~berry here?%Yes|No%",
            {
                this
            });
            SoundManager.PlaySound("select");
        }
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            NewInventoryScreen selScreen = new NewInventoryScreen(Core.CurrentScreen,
            {
                2
            }, 2, null/* TODO Change to default(_) if this is not a reference type */);
            selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection;
            selScreen.CanExit = true;

            selScreen.SelectedObject += PlantBerryHandler;
            Core.SetScreen(selScreen);
        }
    }

    public void PlantBerryHandler(object[] @params)
    {
        PlantBerry(System.Convert.ToInt32(@params[0]));
    }

    public void PlantBerry(int ChosenBerry)
    {
        Item testItem = Item.GetItemByID(ChosenBerry);
        if (testItem.isBerry == true)
        {
            Items.Berry Berry = (Items.Berry)Item.GetItemByID(ChosenBerry);

            BerryPlant.AddBerryPlant(Screen.Level.LevelFile, this.Position, Berry.BerryIndex);
            Screen.TextBox.reDelay = 0.0F;
            Screen.TextBox.Show("You planted a~" + Berry.Name + " Berry here.",
            {
            });
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}

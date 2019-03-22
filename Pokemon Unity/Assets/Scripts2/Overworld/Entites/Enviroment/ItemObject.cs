using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class ItemObject : Entity
{
    private static Dictionary<string, Texture2D> AnimationTexturesTemp = new Dictionary<string, Texture2D>();
    private string AnimationName = "";
    private Animation Animation = null/* TODO Change to default(_) if this is not a reference type */;

    private Item Item;
    private int ItemID = 0;
    private bool checkedExistence = false;
    private string AnimationPath = "";
    private int X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn;
    private Rectangle CurrentRectangle = new Rectangle(0, 0, 0, 0);
    private bool CanInteractWith = true;

    public new void Initialize(List<List<int>> AnimationData = null)
    {
        base.Initialize();

        this.Item = Item.GetItemByID(System.Convert.ToInt32(this.AdditionalValue.GetSplit(1)));
        this.ItemID = System.Convert.ToInt32(this.AdditionalValue.GetSplit(0));

        this.Textures(0) = this.Item.Texture;
        if (this.ActionValue == 0)
            this.Visible = Visible;
        else if (this.ActionValue == 1)
        {
            this.Visible = false;
            this.Collision = false;
        }
        else if (this.ActionValue == 2)
        {
            if (GameVariables.playerTrainer.Inventory.HasMegaBracelet())
            {
                this.Visible = Visible;
                // sparkles
                if (AnimationData != null)
                {
                    X = AnimationData[0][0];
                    Y = AnimationData[0][1];
                    width = AnimationData[0][2];
                    height = AnimationData[0][3];
                    rows = AnimationData[0][4];
                    columns = AnimationData[0][5];
                    animationSpeed = AnimationData[0][6];
                    startRow = AnimationData[0][7];
                    startColumn = AnimationData[0][8];
                    AnimationPath = "ItemAnimations";
                }
                else
                {
                    X = 0;
                    Y = 0;
                    width = 48;
                    height = 48;
                    rows = 5;
                    columns = 10;
                    animationSpeed = 60;
                    startRow = 0;
                    startColumn = 0;
                    AnimationPath = "SparkleAnimation";
                }
                CreateAnimationTextureTemp();

                this.Animation = new Animation(TextureManager.GetTexture(@"Textures\Routes"), rows, columns, 16, 16, animationSpeed, startRow, startColumn);
            }
            else
            {
                this.Visible = false;
                this.Collision = false;
                CanInteractWith = false;
            }
        }

        this.NeedsUpdate = true;
    }

    public static void ClearAnimationResources()
    {
        AnimationTexturesTemp.Clear();
    }

    private void CreateAnimationTextureTemp()
    {
        // If Core.GameOptions.GraphicStyle = 1 Then
        Rectangle r = new Rectangle(X, Y, width, height);
        this.AnimationName = AnimationPath + "," + X + "," + Y + "," + height + "," + width;
        if (AnimationTexturesTemp.ContainsKey(AnimationName + "_0") == false)
        {
            for (var i = 0; i <= this.rows - 1; i++)
            {
                for (var j = 0; j <= this.columns - 1; j++)
                    AnimationTexturesTemp.Add(AnimationName + "_" + (j + columns * i).ToString(), TextureManager.GetTexture(AnimationPath, new Rectangle(r.x + r.Width * j, r.y + r.Height * i, r.Width, r.Height)));
            }
        }
    }

    public override void Update()
    {
        if (checkedExistence == false)
        {
            checkedExistence = true;

            if (ItemExists(this) == true)
                RemoveItem(this);
        }

        if (this.IsHiddenItem() == true)
        {
            if (this.Opacity > 0.0F)
            {
                this.Opacity -= 0.01F;
                if (this.Opacity <= 0.0F)
                {
                    this.Opacity = 1.0F;
                    this.Visible = false;
                }
            }
        }

        base.Update();
    }

    public override void UpdateEntity()
    {
        if (this.Rotation.y != Screen.Camera.Yaw)
        {
            this.Rotation.y = Screen.Camera.Yaw;
            this.CreatedWorld = false;
        }

        if (Animation != null)
        {
            Animation.Update(0.01);
            if (CurrentRectangle != Animation.TextureRectangle)
            {
                ChangeTexture();
                CurrentRectangle = Animation.TextureRectangle;
            }
        }

        base.UpdateEntity();
    }

    private void ChangeTexture()
    {
        // If Core.GameOptions.GraphicStyle = 1 Then

        if (AnimationTexturesTemp.Count == 0)
        {
            ClearAnimationResources();
            CreateAnimationTextureTemp();
        }
        var i = Animation.CurrentRow;
        var j = Animation.CurrentColumn;
        this.Textures(0) = ItemObject.AnimationTexturesTemp[AnimationName + "_" + (j + columns * i)];
    }

    public override void ClickFunction()
    {
        if (CanInteractWith)
        {
            RemoveItem(this);
            SoundManager.PlaySound("item_found", true);
            Screen.TextBox.TextColor = TextBox.PlayerColor;
            Screen.TextBox.Show(GameVariables.playerTrainer.Name + " found~" + this.Item.Name + "!*" + GameVariables.playerTrainer.Inventory.GetMessageReceive(Item, 1), this);
            GameVariables.playerTrainer.Inventory.AddItem(this.Item.ID, 1);
            PlayerStatistics.Track("Items found", 1);

            GameVariables.playerTrainer.AddPoints(1, "Found an item.");
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }

    public static bool ItemExists(ItemObject ItemObject)
    {
        if (GameVariables.playerTrainer.ItemData != "")
        {
            if (GameVariables.playerTrainer.ItemData.Contains(",") == true)
            {
                string[] IDs = GameVariables.playerTrainer.ItemData.ToLower().Split(System.Convert.ToChar(","));

                if (IDs.Contains((Screen.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower()) == true)
                    return true;
                else
                    return false;
            }
            else if (GameVariables.playerTrainer.ItemData.ToLower() == (Screen.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower())
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public static void RemoveItem(ItemObject ItemObject)
    {
        Screen.Level.Entities.Remove(ItemObject);

        if (GameVariables.playerTrainer.ItemData == "")
            GameVariables.playerTrainer.ItemData = (Screen.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower();
        else
        {
            string[] IDs = GameVariables.playerTrainer.ItemData.Split(System.Convert.ToChar(","));
            if (IDs.Contains((Screen.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower()) == false)
                GameVariables.playerTrainer.ItemData += "," + (Screen.Level.LevelFile + "|" + ItemObject.ItemID.ToString()).ToLower();
        }
    }

    public bool IsHiddenItem()
    {
        if (this.Collision == false & this.ActionValue == 1)
            return true;
        else
            return false;
    }
}
}
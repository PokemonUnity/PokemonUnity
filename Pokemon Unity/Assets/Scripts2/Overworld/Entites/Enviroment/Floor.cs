using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class Floor : Entity
{
    private bool changedWeatherTexture = false;
    public bool hasSnow = true;
    public bool hasSand = true;
    public bool IsIce = false;

    private bool _changedToSnow = false;
    private bool _changedToSand = false;

    public Floor()
    {
    }

    public Floor(float X, float Y, float Z, Texture2D[] Textures, int[] TextureIndex, bool Collision, int Rotation, Vector3 Scale, BaseModel Model, int ActionValue, string AdditionalValue, bool Visible, Vector3 Shader, bool hasSnow, bool IsIce, bool hasSand) : base(X, Y, Z, "Floor", Textures, TextureIndex, Collision, Rotation, Scale, Model, ActionValue, AdditionalValue, Shader)
    {
        this.hasSnow = hasSnow;
        this.hasSand = hasSand;
        this.IsIce = IsIce;
        this.Visible = Visible;
    }

    public new void Initialize(bool hasSnow, bool IsIce, bool hasSand)
    {
        base.Initialize();

        this.hasSnow = hasSnow;
        this.hasSand = hasSand;
        this.IsIce = IsIce;
    }

    public void SetRotation(int Rotation)
    {
        switch (Rotation)
        {
            case 0:
                {
                    this.Rotation.y = 0;
                    break;
                }

            case 1:
                {
                    this.Rotation.y = MathHelper.PiOver2;
                    break;
                }

            case 2:
                {
                    this.Rotation.y = MathHelper.Pi;
                    break;
                }

            case 3:
                {
                    this.Rotation.y = MathHelper.Pi * 1.5F;
                    break;
                }
        }
        this.CreatedWorld = false;
    }

    public override void Render()
    {
        if (changedWeatherTexture == false)
        {
            changedWeatherTexture = true;
            if ((Screen.Level.World.CurrentMapWeather == P3D.World.Weathers.Snow | Screen.Level.World.CurrentMapWeather == P3D.World.Weathers.Blizzard) == true & this.hasSnow == true)
                ChangeSnow();
            if (Screen.Level.World.CurrentMapWeather == P3D.World.Weathers.Sandstorm & this.hasSand == true)
                ChangeSand();
        }

        this.Draw(this.Model, Textures, false);
    }

    private static Dictionary<string, Entity> FloorDictionary = new Dictionary<string, Entity>();

    private void ChangeSnow()
    {
        this.Rotation = new Vector3(this.Rotation.x, 0.0F, this.Rotation.z);
        if (Core.CurrentScreen.Identification == Screen.Identifications.BattleScreen)
            this.Textures(0) = P3D.TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 16));
        else
        {
            bool hasEntityOnAllSides = true;
            Entity[] ent = new Entity[5];
            int[] sides = new[] { -1, -1, -1, -1 };

            if (this.IsOffsetMapContent == true)
            {
                ent[0] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }
            else
            {
                ent[0] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }

            for (var i = 0; i <= 3; i++)
            {
                if (ent[i] == null)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
                else if ((Floor)ent[i].hasSnow == false)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
            }

            if (hasEntityOnAllSides == false)
            {
                this.Textures = new Texture2D[]
                {
                    P3D.TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 2)),
                    P3D.TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 16))
                };
                this.Model = BaseModel.BlockModel;
                this.TextureIndex = new[] { sides[0], sides[0], sides[1], sides[1], sides[2], sides[2], sides[3], sides[3], 1, 1 };
                this.Scale = new Vector3(1, 0.1F, 1);
                this.Position.y -= 0.45F;
            }
            else
            {
                this.Textures(0) = P3D.TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 16));
                this.Position.y += 0.1F;
            }
        }

        this.Visible = true;
        this.CreatedWorld = false;
        this.UpdateEntity();

        if (FloorDictionary.ContainsKey(this.Position.ToString()) == false)
            FloorDictionary.Add(this.Position.ToString(), this);

        this._changedToSnow = true;
    }

    private void ChangeSand()
    {
        this.Rotation = new Vector3(this.Rotation.x, 0.0F, this.Rotation.z);
        if (Core.CurrentScreen.Identification == Screen.Identifications.BattleScreen)
            this.Textures(0) = P3D.TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 16));
        else
        {
            bool hasEntityOnAllSides = true;
            Entity[] ent = new Entity[5];
            int[] sides = new[] { -1, -1, -1, -1 };

            if (this.IsOffsetMapContent == true)
            {
                ent[0] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(Screen.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }
            else
            {
                ent[0] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(Screen.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }

            for (var i = 0; i <= 3; i++)
            {
                if (ent[i] == null)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
                else if ((Floor)ent[i].hasSnow == false)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
            }

            if (hasEntityOnAllSides == false)
            {
                this.Textures = new Texture2D[]
                {
                    P3D.TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 2)),
                    P3D.TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 16))
                };
                this.Model = BaseModel.BlockModel;
                this.TextureIndex = new[] { sides[0], sides[0], sides[1], sides[1], sides[2], sides[2], sides[3], sides[3], 1, 1 };
                this.Scale = new Vector3(1, 0.1F, 1);
                this.Position.y -= 0.45F;
            }
            else
            {
                this.Textures(0) = P3D.TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 16));
                this.Position.y += 0.1F;
            }
        }

        this.Visible = true;
        this.CreatedWorld = false;
        this.UpdateEntity();

        if (FloorDictionary.ContainsKey(this.Position.ToString()) == false)
            FloorDictionary.Add(this.Position.ToString(), this);

        this._changedToSand = true;
    }

    public int GetIceFloors()
    {
        int Steps = 0;

        Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
        checkPosition.y = checkPosition.y.ToInteger();

        bool foundSteps = true;
        while (foundSteps == true)
        {
            Entity e = base.GetEntity(Screen.Level.Floors, checkPosition, true, typeof(Floor));
            if (e != null)
            {
                if (e.EntityID == "Floor")
                {
                    if (((Floor)e).IsIce == true)
                    {
                        if ((OverworldCamera)Screen.Camera.CheckCollision(checkPosition) == false)
                        {
                            Steps += 1;
                            checkPosition.x += Screen.Camera.GetMoveDirection().x;
                            checkPosition.z += Screen.Camera.GetMoveDirection().z;

                            Screen.Level.OverworldPokemon.Visible = false;
                            Screen.Level.OverworldPokemon.warped = true;
                        }
                        else
                            foundSteps = false;
                    }
                    else
                    {
                        if ((OverworldCamera)Screen.Camera.CheckCollision(checkPosition) == false)
                            Steps += 1;
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }
            else
                foundSteps = false;
        }

        return Steps;
    }

    private new Entity GetEntity(List<Entity> List, Vector3 Position)
    {
        string positionString = Position.ToString();
        if (FloorDictionary.ContainsKey(positionString) == false)
            FloorDictionary.Add(positionString, (from ent in List
                                                 where ent.Position == Position
                                                 select ent)(0));

        return FloorDictionary[positionString];
    }

    /// <summary>
    /// Clears the list that stores the placements of floors.
    /// </summary>
    public static void ClearFloorTemp()
    {
        FloorDictionary.Clear();
    }
}
}
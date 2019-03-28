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

    public Floor(float X, float Y, float Z, Texture2D[] Textures, int[] TextureIndex, bool Collision, int Rotation, Vector3 Scale, UnityEngine.Mesh Model, int ActionValue, string AdditionalValue, bool Visible, Vector3 Shader, bool hasSnow, bool IsIce, bool hasSand) : base(X, Y, Z, "Floor", Textures, TextureIndex, Collision, Rotation, Scale, Model, ActionValue, AdditionalValue, Shader)
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
                    this.Rotation.y = MathHelper.Pi * 1.5f;
                    break;
                }
        }
        this.CreatedWorld = false;
    }

    public override void Render()
    {
        if (!changedWeatherTexture)
        {
            changedWeatherTexture = true;
            if ((GameVariables.Level.World.CurrentMapWeather == P3D.World.Weathers.Snow | GameVariables.Level.World.CurrentMapWeather == P3D.World.Weathers.Blizzard) & this.hasSnow)
                ChangeSnow();
            if (GameVariables.Level.World.CurrentMapWeather == P3D.World.Weathers.Sandstorm & this.hasSand)
                ChangeSand();
        }

        this.Draw(this.Model, Textures, false);
    }

    private static Dictionary<string, Entity> FloorDictionary = new Dictionary<string, Entity>();

    private void ChangeSnow()
    {
        this.Rotation = new Vector3(this.Rotation.x, 0.0f, this.Rotation.z);
        if (Core.CurrentScreen.Identification == Screen.Identifications.BattleScreen)
            this.Textures[0] = TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 16));
        else
        {
            bool hasEntityOnAllSides = true;
            Entity[] ent = new Entity[5];
            int[] sides = new[] { -1, -1, -1, -1 };

            if (this.IsOffsetMapContent)
            {
                ent[0] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }
            else
            {
                ent[0] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }

            for (var i = 0; i <= 3; i++)
            {
                if (ent[i] == null)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
                else if (!((Floor)ent[i]).hasSnow)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
            }

            if (!hasEntityOnAllSides)
            {
                this.Textures = new Texture2D[]
                {
                    TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 2)),
                    TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 16))
                };
                this.Model = UnityEngine.Mesh.BlockModel;
                this.TextureIndex = new[] { sides[0], sides[0], sides[1], sides[1], sides[2], sides[2], sides[3], sides[3], 1, 1 };
                this.Scale = new Vector3(1, 0.1f, 1);
                this.Position.y -= 0.45f;
            }
            else
            {
                this.Textures[0] = TextureManager.GetTexture("Routes", new Rectangle(208, 16, 16, 16));
                this.Position.y += 0.1f;
            }
        }

        this.Visible = true;
        this.CreatedWorld = false;
        this.UpdateEntity();

        if (!FloorDictionary.ContainsKey(this.Position.ToString()))
            FloorDictionary.Add(this.Position.ToString(), this);

        this._changedToSnow = true;
    }

    private void ChangeSand()
    {
        this.Rotation = new Vector3(this.Rotation.x, 0.0f, this.Rotation.z);
        if (Core.CurrentScreen.Identification == Screen.Identifications.BattleScreen)
            this.Textures[0] = TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 16));
        else
        {
            bool hasEntityOnAllSides = true;
            Entity[] ent = new Entity[5];
            int[] sides = new[] { -1, -1, -1, -1 };

            if (this.IsOffsetMapContent)
            {
                ent[0] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(GameVariables.Level.OffsetmapFloors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }
            else
            {
                ent[0] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z + 1));
                ent[1] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x + 1, this.Position.y, this.Position.z));
                ent[2] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x - 1, this.Position.y, this.Position.z));
                ent[3] = GetEntity(GameVariables.Level.Floors, new Vector3(this.Position.x, this.Position.y, this.Position.z - 1));
            }

            for (var i = 0; i <= 3; i++)
            {
                if (ent[i] == null)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
                else if (!((Floor)ent[i]).hasSnow)
                {
                    hasEntityOnAllSides = false;
                    sides[i] = 0;
                }
            }

            if (!hasEntityOnAllSides)
            {
                this.Textures = new Texture2D[]
                {
                    TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 2)),
                    TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 16))
                };
                this.Model = UnityEngine.Mesh.BlockModel;
                this.TextureIndex = new[] { sides[0], sides[0], sides[1], sides[1], sides[2], sides[2], sides[3], sides[3], 1, 1 };
                this.Scale = new Vector3(1, 0.1f, 1);
                this.Position.y -= 0.45f;
            }
            else
            {
                this.Textures[0] = TextureManager.GetTexture("Routes", new Rectangle(240, 80, 16, 16));
                this.Position.y += 0.1f;
            }
        }

        this.Visible = true;
        this.CreatedWorld = false;
        this.UpdateEntity();

        if (!FloorDictionary.ContainsKey(this.Position.ToString()))
            FloorDictionary.Add(this.Position.ToString(), this);

        this._changedToSand = true;
    }

    public int GetIceFloors()
    {
        int Steps = 0;

        Vector3 checkPosition = GameVariables.Camera.GetForwardMovedPosition();
        checkPosition.y = checkPosition.y.ToInteger();

        bool foundSteps = true;
        while (foundSteps)
        {
            Entity e = base.GetEntity(GameVariables.Level.Floors, checkPosition, true, typeof(Floor));
            if (e != null)
            {
                if (e.EntityID == Entities.Floor)
                {
                    if (((Floor)e).IsIce)
                    {
                        if (!(OverworldCamera)GameVariables.Camera.CheckCollision(checkPosition))
                        {
                            Steps += 1;
                            checkPosition.x += GameVariables.Camera.GetMoveDirection().x;
                            checkPosition.z += GameVariables.Camera.GetMoveDirection().z;

                            GameVariables.Level.OverworldPokemon.Visible = false;
                            GameVariables.Level.OverworldPokemon.warped = true;
                        }
                        else
                            foundSteps = false;
                    }
                    else
                    {
                        if (!(OverworldCamera)GameVariables.Camera.CheckCollision(checkPosition))
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
        if (!FloorDictionary.ContainsKey(positionString))
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
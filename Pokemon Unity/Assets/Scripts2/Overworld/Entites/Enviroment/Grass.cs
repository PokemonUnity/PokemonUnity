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

public class Grass : Entity
{
    protected override float CalculateCameraDistance(Vector3 CPosition)
    {
        return base.CalculateCameraDistance(CPosition) - 0.4F;
    }

    public override void UpdateEntity()
    {
        if (this.Rotation.Y != Screen.Camera.Yaw)
        {
            this.Rotation.Y = Screen.Camera.Yaw;
            CreatedWorld = false;
        }

        base.UpdateEntity();
    }

    public override bool WalkIntoFunction()
    {
        Screen.Level.PokemonEncounter.TryEncounterWildPokemon(this.Position, Spawner.EncounterMethods.Land, this.AdditionalValue);

        return false;
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }

    public static List<Entity> GetGrassTilesAroundPlayer(float radius)
    {
        List<Entity> l = new List<Entity>();

        foreach (Entity e in Screen.Level.Entities)
        {
            if (e.EntityID == "Grass")
            {
                if (e.Visible == true)
                {
                    if (System.Convert.ToInt32(e.Position.Y) == System.Convert.ToInt32(Screen.Camera.Position.Y))
                    {
                        float distance = Vector3.Distance(Screen.Camera.Position, e.Position);

                        if (distance <= radius)
                            l.Add(e);
                    }
                }
            }
        }

        return l;
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class Grass : Entity
{
    protected override float CalculateCameraDistance(Vector3 CPosition)
    {
        return base.CalculateCameraDistance(CPosition) - 0.4f;
    }

    public override void UpdateEntity()
    {
        if (this.Rotation.y != GameVariables.Camera.Yaw)
        {
            this.Rotation.y = GameVariables.Camera.Yaw;
            CreatedWorld = false;
        }

        base.UpdateEntity();
    }

    public override bool WalkIntoFunction()
    {
        GameVariables.Level.PokemonEncounter.TryEncounterWildPokemon(this.Position, Spawner.EncounterMethods.Land, this.AdditionalValue);

        return false;
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }

    public static List<Entity> GetGrassTilesAroundPlayer(float radius)
    {
        List<Entity> l = new List<Entity>();

        foreach (Entity e in GameVariables.Level.Entities)
        {
            if (e.EntityID == Entities.Grass)
            {
                if (e.Visible)
                {
                    if (System.Convert.ToInt32(e.Position.y) == System.Convert.ToInt32(GameVariables.Camera.Position.y))
                    {
                        float distance = Vector3.Distance(GameVariables.Camera.Position, e.Position);

                        if (distance <= radius)
                            l.Add(e);
                    }
                }
            }
        }

        return l;
    }
}
}
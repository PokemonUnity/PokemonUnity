using System.Collections.Generic;
using PokemonUnity.Pokemon;
using PokemonUnity.Player;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class Whirlpool : Entity
{
    public static bool LoadedWaterTemp = false;
    public static List<Texture2D> WaterTexturesTemp = new List<Texture2D>();

    private Animation WaterAnimation;
    private Rectangle currentRectangle = new Rectangle(0, 0, 0, 0);

    public override void Initialize()
    {
        base.Initialize();

        WaterAnimation = new Animation(P3D.TextureManager.GetTexture(@"Textures\Routes"), 1, 4, 16, 16, 9, 12, 0);

        if (!Whirlpool.LoadedWaterTemp)
        {
        }
    }

    public static void CreateWaterTextureTemp()
    {
        if (Core.GameOptions.GraphicStyle == 1)
        {
            Whirlpool.WaterTexturesTemp.Clear();

            Whirlpool.WaterTexturesTemp.Add(P3D.TextureManager.GetTexture("Routes", new Rectangle(0, 176, 16, 16)));
            Whirlpool.WaterTexturesTemp.Add(P3D.TextureManager.GetTexture("Routes", new Rectangle(16, 176, 16, 16)));
            Whirlpool.WaterTexturesTemp.Add(P3D.TextureManager.GetTexture("Routes", new Rectangle(32, 176, 16, 16)));
            Whirlpool.WaterTexturesTemp.Add(P3D.TextureManager.GetTexture("Routes", new Rectangle(48, 176, 16, 16)));
            Whirlpool.LoadedWaterTemp = true;
        }
    }

    public override void UpdateEntity()
    {
        if (WaterAnimation != null)
        {
            WaterAnimation.Update(0.01f);
            if (currentRectangle != WaterAnimation.TextureRectangle)
            {
                ChangeTexture();

                currentRectangle = WaterAnimation.TextureRectangle;
            }
        }

        base.UpdateEntity();
    }

    private void ChangeTexture()
    {
        if (Core.GameOptions.GraphicStyle == 1)
        {
            if (!Whirlpool.LoadedWaterTemp)
                Whirlpool.CreateWaterTextureTemp();
            switch (WaterAnimation.CurrentColumn)
            {
                case 0:
                    {
                        this.Textures[0] = Whirlpool.WaterTexturesTemp[0];
                        break;
                    }
                case 1:
                    {
                        this.Textures[0] = Whirlpool.WaterTexturesTemp[1];
                        break;
                    }
                case 2:
                    {
                        this.Textures[0] = Whirlpool.WaterTexturesTemp[2];
                        break;
                    }
                case 3:
                    {
                        this.Textures[0] = Whirlpool.WaterTexturesTemp[3];
                        break;
                    }
            }
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }

    private string ReturnWhirlPoolPokemonName()
    {
        foreach (Pokemon.Pokemon p in GameVariables.playerTrainer.Party)
        {
            if (!p.isEgg)
            {
                foreach (Attack.Move a in p.moves)
                {
                    if (a.MoveId == Moves.WHIRLPOOL)
                        return p.Name;
                }
            }
        }
        return "";
    }

    public override bool WalkAgainstFunction()
    {
        if (this.ActionValue == 1)
            return this.Collision;

        if (GameVariables.Level.Surfing)
        {
            string pName = ReturnWhirlPoolPokemonName();
            string s = "";

            if (Badge.CanUseHMMove(Badge.HMMoves.Whirlpool) & pName != "" | GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
            {
                s = @"version=2
@text.show(" + pName + @" used~Whirlpool!)
@player.move(2)
:end";
                PlayerStatistics.Track("Whirlpool used", 1);
            }
            else
                s = @"version=2
@player.move(1)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.move(1)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@text.show(It's a vicious~whirlpool!*A Pokémon may be~able to pass it.)
:end";

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
            return true;
        }

        return true;
    }
}
}
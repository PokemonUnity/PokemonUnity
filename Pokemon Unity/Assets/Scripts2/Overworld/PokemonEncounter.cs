using System.Linq;
using PokemonUnity.Pokemon;
using UnityEngine;

namespace PokemonUnity.Overworld
{
/// <summary>
/// A class to handle wild Pokémon encounters.
/// </summary>
public class PokemonEncounter
{
    // Stores a reference to the level instance:
    private Level _levelReference;

    /// <summary>
    /// Creates a new instance of the PokemonEncounter class.
    /// </summary>
    /// <param name="levelReference">The reference to the level instance.</param>
    public PokemonEncounter(Level levelReference)
    {
        this._levelReference = levelReference;
    }

    /// <summary>
    /// Checks if the player should encounter a wild Pokémon.
    /// </summary>
    /// <param name="Position">The position the encounter should happen.</param>
    /// <param name="Method">The method of the encounter.</param>
    /// <param name="pokeFile">The source .poke file. If left empty, the game will assume the levelfile as source .poke file.</param>
    public void TryEncounterWildPokemon(Vector3 Position, Spawner.EncounterMethods Method, string pokeFile)
    {
        {
            var withBlock = this._levelReference;
            if (withBlock.WalkedSteps > 3)
            {
                // Compose the correct .poke file from the levelfile, if the pokeFile parameter is empty:
                if (pokeFile == "")
                    pokeFile = withBlock.LevelFile.Remove(withBlock.LevelFile.Length - 4, 4) + ".poke";

                if (System.IO.File.Exists(GameModeManager.GetPokeFilePath(pokeFile)) == true)
                {
                    int startRandomValue = 12;
                    int minRandomValue = 5;

                    if (GameVariables.playerTrainer.Pokemons.Count > 0)
                    {
                        PokemonUnity.Pokemon.Pokemon p = GameVariables.playerTrainer.Pokemons(0);

                        // Arena Trap/Illuminate/No Guard/Swarm Ability:
                        if (p.Ability.Name.ToLower() == "arena trap" | p.Ability.Name.ToLower() == "illuminate" | p.Ability.Name.ToLower() == "no guard" | p.Ability.Name.ToLower() == "swarm")
                        {
                            startRandomValue = 6;
                            minRandomValue = 3;
                        }

                        // Intimidate/Keen Eye/Quick Feet/Stench/White Smoke Ability:
                        if (p.Ability.Name.ToLower() == "intimidate" | p.Ability.Name.ToLower() == "keen eye" | p.Ability.Name.ToLower() == "quick feet" | p.Ability.Name.ToLower() == "stench" | p.Ability.Name.ToLower() == "white smoke")
                        {
                            startRandomValue = 24;
                            minRandomValue = 10;
                        }

                        // Sand Veil Ability:
                        if (withBlock.WeatherType == 7 & p.Ability.Name.ToLower() == "sand veil")
                        {
                            if (Core.Random.Next(0, 100) < 50)
                                return;
                        }

                        // Snow Cloak Ability:
                        if (p.Ability.Name.ToLower() == "snow cloak")
                        {
                            if (withBlock.WeatherType == 2 | withBlock.WeatherType == 9)
                            {
                                if (Core.Random.Next(0, 100) < 50)
                                    return;
                            }
                        }
                    }

                    // Determine if the wild Pokémon will be met or not:
                    int randomValue = startRandomValue - withBlock.WalkedSteps;
                    randomValue = System.Convert.ToInt32(MathHelper.Clamp(randomValue, minRandomValue, startRandomValue));

                    if (Core.Random.Next(0, randomValue * 2) == 0)
                    {
                        // Don't encounter a Pokémon if the left control key is held down, for Debug or Sandbox Mode:
                        if (GameController.IS_DEBUG_ACTIVE == true | GameVariables.playerTrainer.SandBoxMode == true)
                        {
                            if (KeyBoardHandler.KeyDown(Keys.LeftControl) == true)
                                return;
                        }

                        // Reset walked steps and set the wild Pokémon data:
                        withBlock.WalkedSteps = 0;

                        withBlock.PokemonEncounterData.Position = Position;
                        withBlock.PokemonEncounterData.EncounteredPokemon = true;
                        withBlock.PokemonEncounterData.Method = Method;
                        withBlock.PokemonEncounterData.PokeFile = pokeFile;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Triggers a battle with a wild Pokémon if the requirements are met.
    /// </summary>
    public void TriggerBattle()
    {
        // If the encounter check is true:
        if (this._levelReference.PokemonEncounterData.EncounteredPokemon == true & Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
        {
            // If the player met the set position:
            if (Screen.Camera.Position.X == this._levelReference.PokemonEncounterData.Position.X & Screen.Camera.Position.z == this._levelReference.PokemonEncounterData.Position.z)
            {
                // Make the player stop and set encounter check to false:
                this._levelReference.PokemonEncounterData.EncounteredPokemon = false;
                Screen.Camera.StopMovement();

                // Generate new wild Pokémon:
                Pokemon.Pokemon Pokemon = Spawner.GetPokemon(Screen.Level.LevelFile, this._levelReference.PokemonEncounterData.Method, true, this._levelReference.PokemonEncounterData.PokeFile);

                if (Pokemon != null & (OverworldScreen)Core.CurrentScreen.TrainerEncountered == false & (OverworldScreen)Core.CurrentScreen.ActionScript.IsReady == true)
                {
                    Screen.Level.RouteSign.Hide(); // When a battle starts, hide the Route sign.

                    // If the player h.x a Repel going and the first Pokémon in the party's le.xl is greater than the wild Pokémon's level, don't start the battle:
                    if (GameVariables.playerTrainer.RepelSteps > 0)
                    {
                        Pokemon.Pokemon p = GameVariables.playerTrainer.GetWalkPokemon();
                        if (p != null)
                        {
                            if (p.Level >= Pokemon.Level)
                                return;
                        }
                    }

                    // Cleanse Tag prevents wild Pokémon encounters if held by the first Pokémon in the party:
                    if (GameVariables.playerTrainer.Pokemons(0).Level >= Pokemon.Level)
                    {
                        if (GameVariables.playerTrainer.Pokemons(0).Item != null)
                        {
                            if (GameVariables.playerTrainer.Pokemons(0).Item.ID == 94)
                            {
                                if (Core.Random.Next(0, 3) == 0)
                                    return;
                            }
                        }
                    }

                    // Pure Incense lowers the chance of encountering wild Pokémon if held by the first Pokémon in the party:
                    if (GameVariables.playerTrainer.Pokemons(0).Level >= Pokemon.Level)
                    {
                        if (GameVariables.playerTrainer.Pokemons(0).Item != null)
                        {
                            if (GameVariables.playerTrainer.Pokemons(0).Item.ID == 291)
                            {
                                if (Core.Random.Next(0, 3) == 0)
                                    return;
                            }
                        }
                    }

                    // Register the wild Pokémon as Seen in the Pokédex:
                    GameVariables.playerTrainer.PokedexData = Pokedex.ChangeEntry(GameVariables.playerTrainer.PokedexData, Pokemon.Number, 1);

                    // Determine wild Pokémon intro type. If it's a Roaming Pokémon battle, set to 12:
                    int introType = Core.Random.Next(0, 10);
                    if (BattleSystem.BattleScreen.RoamingBattle == true)
                        introType = 12;

                    BattleSystem.BattleScreen b = new BattleSystem.BattleScreen(Pokemon, Core.CurrentScreen, this._levelReference.PokemonEncounterData.Method);
                    Core.SetScreen(new BattleIntroScreen(Core.CurrentScreen, b, introType));
                }
            }
        }
    }
}
}
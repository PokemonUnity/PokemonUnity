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
	/// <summary>
	/// Stores a reference to the level instance:
	/// </summary>
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

                if (System.IO.File.Exists(GameModeManager.GetPokeFilePath(pokeFile)))
                {
                    int startRandomValue = 12;
                    int minRandomValue = 5;

                    if (GameVariables.playerTrainer.Party.GetCount() > 0)
                    {
                        PokemonUnity.Pokemon.Pokemon p = GameVariables.playerTrainer.Party[0];

                        // Arena Trap/Illuminate/No Guard/Swarm Ability:
                        if (p.Ability == Abilities.ARENA_TRAP | p.Ability == Abilities.ILLUMINATE | p.Ability == Abilities.NO_GUARD | p.Ability == Abilities.SWARM)
                        {
                            startRandomValue = 6;
                            minRandomValue = 3;
                        }

                        // Intimidate/Keen Eye/Quick Feet/Stench/White Smoke Ability:
                        if (p.Ability == Abilities.INTIMIDATE | p.Ability == Abilities.KEEN_EYE | p.Ability == Abilities.QUICK_FEET | p.Ability == Abilities.STENCH | p.Ability == Abilities.WHITE_SMOKE)
                        {
                            startRandomValue = 24;
                            minRandomValue = 10;
                        }

                        // Sand Veil Ability:
                        if (withBlock.WeatherType == 7 & p.Ability == Abilities.SAND_VEIL)
                        {
                            if (Settings.Rand.Next(0, 100) < 50)
                                return;
                        }

                        // Snow Cloak Ability:
                        if (p.Ability == Abilities.SNOW_CLOAK)
                        {
                            if (withBlock.WeatherType == 2 | withBlock.WeatherType == 9)
                            {
                                if (Settings.Rand.Next(0, 100) < 50)
                                    return;
                            }
                        }
                    }

                    // Determine if the wild Pokémon will be met or not:
                    int randomValue = startRandomValue - withBlock.WalkedSteps;
                    randomValue = System.Convert.ToInt32(MathHelper.Clamp(randomValue, minRandomValue, startRandomValue));

                    if (Settings.Rand.Next(0, randomValue * 2) == 0)
                    {
                        // Don't encounter a Pokémon if the left control key is held down, for Debug or Sandbox Mode:
                        if (GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
                        {
                            //if (KeyBoardHandler.KeyDown(Keys.LeftControl))
                            if (Input.GetKeyDown(KeyCode.LeftControl))
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
        if (this._levelReference.PokemonEncounterData.EncounteredPokemon & Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
        {
            // If the player met the set position:
            if (GameVariables.Camera.Position.x == this._levelReference.PokemonEncounterData.Position.x & GameVariables.Camera.Position.z == this._levelReference.PokemonEncounterData.Position.z)
            {
                // Make the player stop and set encounter check to false:
                this._levelReference.PokemonEncounterData.EncounteredPokemon = false;
                GameVariables.Camera.StopMovement();

                // Generate new wild Pokémon:
                Pokemon.Pokemon Pokemon = Spawner.GetPokemon(GameVariables.Level.LevelFile, this._levelReference.PokemonEncounterData.Method, true, this._levelReference.PokemonEncounterData.PokeFile);

                if (Pokemon != null & (OverworldScreen)Core.CurrentScreen.TrainerEncountered == false & (OverworldScreen)Core.CurrentScreen.ActionScript.IsReady)
                {
                    GameVariables.Level.RouteSign.Hide(); // When a battle starts, hide the Route sign.

                    // If the player has a Repel going and the first Pokémon in the party's le.xl is greater than the wild Pokémon's level, don't start the battle:
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
                    if (GameVariables.playerTrainer.Party[0].Level >= Pokemon.Level)
                    {
                        if (GameVariables.playerTrainer.Party[0].Item != Item.Items.NONE)
                        {
                            if (GameVariables.playerTrainer.Party[0].Item == Item.Items.CLEANSE_TAG)
                            {
                                if (Settings.Rand.Next(0, 3) == 0)
                                    return;
                            }
                        }
                    }

                    // Pure Incense lowers the chance of encountering wild Pokémon if held by the first Pokémon in the party:
                    if (GameVariables.playerTrainer.Party[0].Level >= Pokemon.Level)
                    {
                        if (GameVariables.playerTrainer.Party[0].Item != Item.Items.NONE)
                        {
                            if (GameVariables.playerTrainer.Party[0].Item == Item.Items.PURE_INCENSE)
                            {
                                if (Settings.Rand.Next(0, 3) == 0)
                                    return;
                            }
                        }
                    }

                    // Register the wild Pokémon as Seen in the Pokédex:
                    //GameVariables.playerTrainer.PokedexData = Pokedex.ChangeEntry(GameVariables.playerTrainer.PokedexData, Pokemon.Species, 1);
                    GameVariables.playerTrainer.PlayerPokedex[(int)Pokemon.Species, 0] = 1;

                    // Determine wild Pokémon intro type. If it's a Roaming Pokémon battle, set to 12:
                    int introType = Settings.Rand.Next(0, 10);
                    //if (BattleSystem.BattleScreen.RoamingBattle)
                    //    introType = 12;
					//
                    //BattleSystem.BattleScreen b = new BattleSystem.BattleScreen(Pokemon, Core.CurrentScreen, this._levelReference.PokemonEncounterData.Method);
                    //Core.SetScreen(new BattleIntroScreen(Core.CurrentScreen, b, introType));
                }
            }
        }
    }
}
}
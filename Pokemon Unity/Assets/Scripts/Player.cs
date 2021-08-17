using PokemonUnity;
using PokemonUnity.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom
public class Player
{
    #region Variables
    private int? trainerId { get; set; }
    private int? secretId { get; set; }
    public string Name { get; private set; }
    public bool IsMale { get; private set; }
    public Pokemon[] Party { get; private set; }
    public TrainerData Trainer { get { return new TrainerData(name: Name, gender: IsMale, tID: trainerId, sID: secretId); } }
    
    public PC PC { get; private set; }
    public Bag Bag { get; private set; }

    public int Money { get { return playerMoney; } set { playerMoney = value > Core.MAXMONEY ? Core.MAXMONEY : value; } }
    private int playerMoney { get; set; }
    public int playerScore { get; set; } // Need this?

    public Vector3 Position { get; set; }
    // North, South, East or West
    public int Direction { get; set; }
    public string playerOutfit { get; set; }
    public string[] registeredItems { get; set; }

    // ToDo: use GymBadges
    public bool[] gymsEncountered { get; set; }
    public bool[] gymsBeaten { get; set; }
    public string[] gymsBeatTime { get; set; }

    public System.DateTime StartDate { get; set; }
    public System.TimeSpan PlayTime { get; set; }
    #endregion
    public Player()
    {
        Name = "Test";
        IsMale = true;
        Bag = new Bag();
        PC = new PC();
        Party = new Pokemon[6];
    }

    public Player(string Name, bool IsMale)
    {
        this.Name = Name;
        this.IsMale = IsMale;
        Bag = new Bag();
        PC = new PC();
        Party = new Pokemon[6];
    }

    /// <summary>
    /// Skims every available box player has, and attempts to add pokemon.
    /// </summary>
    /// <param name="pokemon"></param>
    /// <returns>returns storage location of caught pokemon</returns>
    public bool addPokemon(Pokemon pokemon)
    {
        //attempt to add to party first. pack the party array if space available.
        if (Party.HasSpace(Party.Length))
        {
            Party.PackParty();
            Party[Party.Length - 1] = pokemon;
            Party.PackParty();
            return true;
        }
        else
        {
            for (int i = 0; i < PC.boxes.Length; i++)
            {
                if (PC.hasSpace(i))
                {
                    for (int i2 = 0; i2 < PC.boxes[i].Length; i2++)
                    {
                        if (PC.boxes[i][i2] == null)
                        {
                            PC.boxes[i][i2] = pokemon;
                            return true;
                        }
                    }
                }
            }
        }
        //if could not add a pokemon, return false. Party and PC are both full.
        return false;
    }

    public void swapPartyPokemon(int pos1, int pos2)
    {
        Pokemon temp = Party[pos1];
        Party[pos1] = Party[pos2];
        Party[pos2] = temp;
    }

    // Party and PC
    public void swapPokemon(int index1, int pos1, int index2, int pos2)
    {
        Pokemon temp;
        Pokemon temp2;
        if (index1 == -1)
            temp = Party[pos1];
        else
            temp = PC.boxes[index1][pos1];

        if (index2 == -1)
            temp2 = Party[pos2];
        else
            temp2 = PC.boxes[index2][pos2];

        if (index1 == -1 && index2 == -1)
        {
            swapPartyPokemon(pos1, pos2);
        }
        else if (index1 != -1 && index2 == -1 || index1 == -1 && index2 != -1)
        {
            if (index1 == -1)
            {
                PC.boxes[index2][pos2] = temp;
                Party[pos1] = temp2;
            }
            else
            {
                Party[pos2] = temp;
                PC.boxes[index1][pos1] = temp2;
            }
        }
        else
        {
            PC.boxes[index1][pos1] = temp2;
            PC.boxes[index2][pos2] = temp;
        }
    }

    public Pokemon getFirstFEUserInParty(string moveName)
    {
        for (int i = 0; i < 6; i++)
        {
            if (Party[i] != null)
            {
                string[] moveset = Party[i].getMoveset();
                for (int i2 = 0; i2 < moveset.Length; i2++)
                {
                    if (moveset[i2] != null)
                    {
                        if (MoveDatabase.getMove(moveset[i2]).getFieldEffect() == moveName)
                        {
                            return Party[i];
                        }
                    }
                }
            }
        }
        return null;
    }
}

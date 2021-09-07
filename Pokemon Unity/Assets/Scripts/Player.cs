using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
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

    public Player(int? trainerId, int? secretId, string Name, bool IsMale, Pokemon[] Party, PC Pc, PokemonUnity.Inventory.Items[] Bag, int playerMoney, int playerScore, Vector3 Position, int Direction,
        string playerOutfit, string[] registeredItems, bool[] gymsEncountered, bool[] gymsBeaten, string[] gymsBeatTime, System.DateTime StartDate, System.TimeSpan PlayTime)
    {
        this.trainerId = trainerId; this.secretId = secretId; this.Name = Name; this.IsMale = IsMale; this.Party = Party; PC = Pc; this.Bag = new Bag(Bag); this.playerMoney = playerMoney; this.playerScore = playerScore;
        this.Position = Position; this.Direction = Direction; this.playerOutfit = playerOutfit; this.registeredItems = registeredItems; this.gymsEncountered = gymsEncountered; this.gymsBeaten = gymsBeaten;
        this.StartDate = StartDate; this.PlayTime = PlayTime;
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
            return true; //true
        }
        else
        {
            //Could not be stored in PC because all boxes full
            System.Collections.Generic.KeyValuePair<int, int>? slot = null;
            //attempt to add to the earliest available PC box. 
            for (int numOfBoxes = 0, curBox = PC.ActiveBox; numOfBoxes < PC.AllBoxes.Length; numOfBoxes++, curBox++)
            {
                slot = PC[(byte)(curBox % Core.STORAGEBOXES)].addPokemon(pokemon);
                if (slot != null)
                    //Returns the box pokemon was stored to
                    return true; //true
                if (!Game.GameData.Features.OverflowPokemonsIntoNextBox) break; //else PC.ActiveBox = curBox; //change active box too?
            }
            return false;
        }
    }

    // ToDo: Merge swapPartyPokemon and swapPokemon
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
            temp = PC.AllBoxes[index1][pos1];

        if (index2 == -1)
            temp2 = Party[pos2];
        else
            temp2 = PC.AllBoxes[index2][pos2];

        if (index1 == -1 && index2 == -1)
        {
            swapPartyPokemon(pos1, pos2);
        }
        else if (index1 != -1 && index2 == -1 || index1 == -1 && index2 != -1)
        {
            if (index1 == -1)
            {
                PC.addPokemon(index2, pos2, temp);
                //PC.AllBoxes[index2][pos2] = temp;
                Party[pos1] = temp2;
            }
            else
            {
                Party[pos2] = temp;
                PC.addPokemon(index1, pos1, temp2);
                //PC.AllBoxes[index1][pos1] = temp2;
            }
        }
        else
        {
            PC.swapPokemon(index1, pos1, index2, pos2);
            //PC.AllBoxes[index1][pos1] = temp2;
            //PC.AllBoxes[index2][pos2] = temp;
        }
    }

    public Pokemon getFirstFEUserInParty(Moves moveName)
    {
        for (int i = 0; i < 6; i++)
        {
            if (Party[i] != null)
            {
                for (int i2 = 0; i2 < Party[i].moves.Length; i2++)
                {
                    if (Party[i].moves[i2].MoveId != Moves.NONE)
                    {
                        if (Party[i].moves[i2].MoveId == moveName)
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

using System;
using PokemonUnity.Saving.SerializableClasses;


[Serializable]
public class SeriPlayer
{
    private int? trainerId;
    private int? secretId;
    private string Name;
    private bool IsMale;

    private SeriPokemon[] Party;
    private SeriPC PC;
    private PokemonUnity.Inventory.Items[] Bag;

    private int playerMoney;
    private int playerScore;

    private SeriV3 Position;
    private int Direction;
    private string playerOutfit;
    private string[] registeredItems;

    // ToDo: use GymBadges
    private bool[] gymsEncountered;
    private bool[] gymsBeaten;
    private string[] gymsBeatTime;

    private DateTime StartDate;
    private TimeSpan PlayTime;

    public Player GetPlayer()
    {
        PokemonUnity.Character.PC pc = new PokemonUnity.Character.PC(PC.Pokemons.Deserialize(), PC.GetItemsFromSeri().Compress(),
            PC.ActiveBox, PC.BoxNames, PC.BoxTextures);
        return new Player(trainerId, secretId, Name, IsMale, Party.Deserialize(), pc, Bag, playerMoney, playerScore, Position,
            Direction, playerOutfit, registeredItems, gymsEncountered, gymsBeaten, gymsBeatTime, StartDate, PlayTime);
    }

    public SeriPlayer(Player player)
    {
        trainerId = player.Trainer.TrainerID;
        secretId = player.Trainer.SecretID;
        Name = player.Name;
        IsMale = player.IsMale;

        Party = new SeriPokemon[player.Party.Length];
        for (int i = 0; i < player.Party.Length; i++)
        {
            Party[i] = player.Party[i];
        }

        PC = new SeriPC(player.PC);
        Bag = player.Bag.Contents;

        playerMoney = player.Money;
        playerScore = player.playerScore;

        Position = new SeriV3(player.Position);
        Direction = player.Direction;
        playerOutfit = player.playerOutfit;
        registeredItems = player.registeredItems;
        gymsEncountered = player.gymsEncountered;
        gymsBeaten = player.gymsBeaten;
        gymsBeatTime = player.gymsBeatTime;

        StartDate = player.StartDate;
        PlayTime = player.PlayTime;
    }
}
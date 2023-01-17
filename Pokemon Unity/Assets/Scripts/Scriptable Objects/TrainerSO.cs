using PokemonEssentials.Interface.PokeBattle;
using PokemonUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trainer", menuName = "Pokemon Unity/Trainer")]
public class TrainerSO : ScriptableObject, ITrainer {
    public List<SpriteAnimation> Animations;
    [SerializeField] PokemonSO[] Party;

    #region ITrainer Properties

    public string name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int? metaID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public TrainerTypes trainertype { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int? outfit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool[] badges => throw new System.NotImplementedException();

    public int Money { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IDictionary<Pokemons, bool> seen => throw new System.NotImplementedException();

    public IDictionary<Pokemons, bool> owned => throw new System.NotImplementedException();

    public int?[][] formseen { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public KeyValuePair<int, int?>[] formlastseen { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public IList<Pokemons> shadowcaught { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public IPokemon[] party { get => Party; set => Party = (PokemonSO[])value; }
    public bool pokedex { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool pokegear { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Languages? language => throw new System.NotImplementedException();

    public string trainerTypeName => throw new System.NotImplementedException();

    public string fullname => throw new System.NotImplementedException();

    public int moneyEarned => throw new System.NotImplementedException();

    public int skill => throw new System.NotImplementedException();

    public string skillCode => throw new System.NotImplementedException();

    public int numbadges => throw new System.NotImplementedException();

    public int gender => throw new System.NotImplementedException();

    public bool isMale => throw new System.NotImplementedException();

    public bool isFemale => throw new System.NotImplementedException();

    public IEnumerable<IPokemon> pokemonParty => throw new System.NotImplementedException();

    public IEnumerable<IPokemon> ablePokemonParty => throw new System.NotImplementedException();

    public int partyCount => throw new System.NotImplementedException();

    public int pokemonCount => throw new System.NotImplementedException();

    public int ablePokemonCount => throw new System.NotImplementedException();

    public IPokemon firstParty => throw new System.NotImplementedException();

    public IPokemon firstPokemon => throw new System.NotImplementedException();

    public IPokemon firstAblePokemon => throw new System.NotImplementedException();

    public IPokemon lastParty => throw new System.NotImplementedException();

    public IPokemon lastPokemon => throw new System.NotImplementedException();

    public IPokemon lastAblePokemon => throw new System.NotImplementedException();

    #endregion

    #region ITrainer Functions

    public void clearPokedex() {
        throw new System.NotImplementedException();
    }

    public int getForeignID() {
        throw new System.NotImplementedException();
    }

    public bool hasOwned(Pokemons species) {
        throw new System.NotImplementedException();
    }

    public bool hasSeen(Pokemons species) {
        throw new System.NotImplementedException();
    }

    public bool hasSkillCode(string code) {
        throw new System.NotImplementedException();
    }

    public ITrainer initialize(string name, TrainerTypes trainertype) {
        throw new System.NotImplementedException();
    }

    public int numFormsSeen(Pokemons species) {
        throw new System.NotImplementedException();
    }

    public int pokedexOwned(Regions? region = null) {
        throw new System.NotImplementedException();
    }

    public int pokedexSeen(Regions? region = null) {
        throw new System.NotImplementedException();
    }

    public int publicID(int? id = null) {
        throw new System.NotImplementedException();
    }

    public int secretID(int? id = null) {
        throw new System.NotImplementedException();
    }

    public void setForeignID(ITrainer other) {
        throw new System.NotImplementedException();
    }

    public void setOwned(Pokemons species) {
        throw new System.NotImplementedException();
    }

    public void setSeen(Pokemons species) {
        throw new System.NotImplementedException();
    }

    #endregion

}

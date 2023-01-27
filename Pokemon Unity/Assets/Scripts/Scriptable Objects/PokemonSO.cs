using MarkupAttributes;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon Unity/Pokemon")]
public class PokemonSO : ScriptableObject, IPokemon {
    [SerializeField] Pokemons species;
    [TitleGroup("Overworld Graphics", contentBox: true)]
    public List<SpriteAnimation> OverworldAnimations;
    [TitleGroup("Battle Graphics", contentBox: true)]
    public SpriteAnimation Front;
    public SpriteAnimation Back;

    #region IPokemon Properties

    public int TotalHP => throw new NotImplementedException();

    public int ATK => throw new NotImplementedException();

    public int DEF => throw new NotImplementedException();

    public int SPE => throw new NotImplementedException();

    public int SPA => throw new NotImplementedException();

    public int SPD => throw new NotImplementedException();

    public int[] IV => throw new NotImplementedException();

    public byte[] EV => throw new NotImplementedException();

    public Pokemons Species => species;

    public int PersonalId => throw new NotImplementedException();

    public int trainerID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int HP { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int[] Pokerus => throw new NotImplementedException();

    public Items Item => throw new NotImplementedException();

    public Items itemRecycle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Items itemInitial { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool belch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IMail mail { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IPokemon[] fused { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Name => throw new NotImplementedException();

    public int Exp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int Happiness => throw new NotImplementedException();

    public Status Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int StatusCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int EggSteps { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IMove[] moves => throw new NotImplementedException();

    public IList<Moves> firstMoves => throw new NotImplementedException();

    public Items ballUsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int obtainMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int obtainMap { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string obtainText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int obtainLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int hatchedMap { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int otgender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int abilityflag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool genderflag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int natureflag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool shinyflag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IList<Ribbons> ribbons { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int cool { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int beauty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int cute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int smart { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int tough { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int sheen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int publicID => throw new NotImplementedException();

    public DateTime? timeReceived { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime? timeEggHatched { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int Level => throw new NotImplementedException();

    public bool isEgg => throw new NotImplementedException();

    public LevelingRate GrowthRate => throw new NotImplementedException();

    public int baseExp => throw new NotImplementedException();

    public bool? Gender => throw new NotImplementedException();

    public bool isSingleGendered => throw new NotImplementedException();

    public bool IsMale => throw new NotImplementedException();

    public bool IsFemale => throw new NotImplementedException();

    public bool IsGenderless => throw new NotImplementedException();

    public int abilityIndex => throw new NotImplementedException();

    public Abilities Ability => throw new NotImplementedException();

    public Natures Nature => throw new NotImplementedException();

    public bool IsShiny => throw new NotImplementedException();

    public bool? PokerusStage => throw new NotImplementedException();

    public PokemonUnity.Types Type1 => throw new NotImplementedException();

    public PokemonUnity.Types Type2 => throw new NotImplementedException();

    public int numMoves => throw new NotImplementedException();

    public int language => throw new NotImplementedException();

    public bool[] Markings => throw new NotImplementedException();

    public char unownShape => throw new NotImplementedException();

    public float height => throw new NotImplementedException();

    public float weight => throw new NotImplementedException();

    public int[] evYield => throw new NotImplementedException();

    public string kind => throw new NotImplementedException();

    public string dexEntry => throw new NotImplementedException();

    public int[] baseStats => throw new NotImplementedException();

    #endregion

    #region IPokemon Functions

    public int calcHP(int _base, int level, int iv, int ev) {
        throw new NotImplementedException();
    }

    public void calcStat(int _base, int level, int iv, int ev, int pv) {
        throw new NotImplementedException();
    }

    public void calcStats() {
        throw new NotImplementedException();
    }

    public void changeHappiness(HappinessMethods method) {
        throw new NotImplementedException();
    }

    public void clearAllRibbons() {
        throw new NotImplementedException();
    }

    public Abilities[] getAbilityList() {
        throw new NotImplementedException();
    }

    public Moves[] getMoveList(LearnMethod? method = null) {
        throw new NotImplementedException();
    }

    public void GivePokerus(int strain = 0) {
        throw new NotImplementedException();
    }

    public void giveRibbon(Ribbons ribbon) {
        throw new NotImplementedException();
    }

    public bool hasAbility(Abilities value = Abilities.NONE) {
        throw new NotImplementedException();
    }

    public bool hasHiddenAbility() {
        throw new NotImplementedException();
    }

    public bool hasItem(Items value = Items.NONE) {
        throw new NotImplementedException();
    }

    public bool hasMove(Moves move) {
        throw new NotImplementedException();
    }

    public bool hasNature(Natures? value = null) {
        throw new NotImplementedException();
    }

    public bool hasRibbon(Ribbons ribbon) {
        throw new NotImplementedException();
    }

    public bool hasType(PokemonUnity.Types type) {
        throw new NotImplementedException();
    }

    public bool Heal() {
        throw new NotImplementedException();
    }

    public bool HealHP() {
        throw new NotImplementedException();
    }

    public bool HealPP(int index = -1) {
        throw new NotImplementedException();
    }

    public bool HealStatus() {
        throw new NotImplementedException();
    }

    public IPokemon initialize(Pokemons species, int level, ITrainer player = null, bool withMoves = true) {
        throw new NotImplementedException();
    }

    public bool isCompatibleWithMove(Moves move) {
        throw new NotImplementedException();
    }

    public bool isFemale(int b, int genderRate) {
        throw new NotImplementedException();
    }

    public bool isForeign(ITrainer trainer) {
        throw new NotImplementedException();
    }

    public bool knowsMove(Moves move) {
        throw new NotImplementedException();
    }

    public void lowerPokerusCount() {
        throw new NotImplementedException();
    }

    public void makeFemale() {
        throw new NotImplementedException();
    }

    public void makeMale() {
        throw new NotImplementedException();
    }

    public void makeNotShiny() {
        throw new NotImplementedException();
    }

    public void makeShiny() {
        throw new NotImplementedException();
    }

    public void pbDeleteAllMoves() {
        throw new NotImplementedException();
    }

    public void pbDeleteMove(Moves move) {
        throw new NotImplementedException();
    }

    public void pbDeleteMoveAtIndex(int index) {
        throw new NotImplementedException();
    }

    public void pbLearnMove(Moves move) {
        throw new NotImplementedException();
    }

    public void pbRecordFirstMoves() {
        throw new NotImplementedException();
    }

    public void resetMoves() {
        throw new NotImplementedException();
    }

    public void resetPokerusTime() {
        throw new NotImplementedException();
    }

    public int ribbonCount() {
        throw new NotImplementedException();
    }

    public void setAbility(int value) {
        throw new NotImplementedException();
    }

    public void setGender(int value) {
        throw new NotImplementedException();
    }

    public void setItem(Items value) {
        throw new NotImplementedException();
    }

    public void setNature(Natures value) {
        throw new NotImplementedException();
    }

    public void takeRibbon(Ribbons ribbon) {
        throw new NotImplementedException();
    }

    public int upgradeRibbon(params Ribbons[] arg) {
        throw new NotImplementedException();
    }

    public Items[] wildHoldItems() {
        throw new NotImplementedException();
    }

    #endregion
}

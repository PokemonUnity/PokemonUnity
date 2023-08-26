using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;

namespace PokemonUnity.Inventory
{
  public static class UnityItemHandlers
  {
    private static Dictionary<Items, Func<ItemUseResults>> UseFromBag = new Dictionary<Items, Func<ItemUseResults>>();
    private static Dictionary<Items, Action> UseInField = new Dictionary<Items, Action>();
    private static Dictionary<Items, UseOnPokemonDelegate> UseOnPokemon = new Dictionary<Items, UseOnPokemonDelegate>();
    private static Dictionary<Items, BattleUseOnBattlerDelegate> BattleUseOnBattler = new Dictionary<Items, BattleUseOnBattlerDelegate>();
    private static Dictionary<Items, BattleUseOnPokemonDelegate> BattleUseOnPokemon = new Dictionary<Items, BattleUseOnPokemonDelegate>();
    private static Dictionary<Items, UseInBattleDelegate> UseInBattle = new Dictionary<Items, UseInBattleDelegate>();

    public static void addUseFromBag(Items item, Func<ItemUseResults> proc)
    {
      if (!UnityItemHandlers.UseFromBag.ContainsKey(item))
        UnityItemHandlers.UseFromBag.Add(item, proc);
      else
        UnityItemHandlers.UseFromBag[item] = proc;
    }

    public static void addUseInField(Items item, Action proc)
    {
      if (!UnityItemHandlers.UseInField.ContainsKey(item))
        UnityItemHandlers.UseInField.Add(item, proc);
      else
        UnityItemHandlers.UseInField[item] = proc;
    }

    public static void addUseOnPokemon(Items item, UseOnPokemonDelegate proc)
    {
      if (!UnityItemHandlers.UseOnPokemon.ContainsKey(item))
        UnityItemHandlers.UseOnPokemon.Add(item, proc);
      else
        UnityItemHandlers.UseOnPokemon[item] = proc;
    }

    public static void addBattleUseOnBattler(Items item, BattleUseOnBattlerDelegate proc)
    {
      if (!UnityItemHandlers.BattleUseOnBattler.ContainsKey(item))
        UnityItemHandlers.BattleUseOnBattler.Add(item, proc);
      else
        UnityItemHandlers.BattleUseOnBattler[item] = proc;
    }

    public static void addBattleUseOnPokemon(Items item, BattleUseOnPokemonDelegate proc)
    {
      if (!UnityItemHandlers.BattleUseOnPokemon.ContainsKey(item))
        UnityItemHandlers.BattleUseOnPokemon.Add(item, proc);
      else
        UnityItemHandlers.BattleUseOnPokemon[item] = proc;
    }

    public static bool hasOutHandler(Items item) => !UnityItemHandlers.UseFromBag.ContainsKey(item) && UnityItemHandlers.UseFromBag[item] != null || !UnityItemHandlers.UseOnPokemon.ContainsKey(item) && UnityItemHandlers.UseOnPokemon[item] != null;

    public static bool hasKeyItemHandler(Items item) => !UnityItemHandlers.UseInField.ContainsKey(item) && UnityItemHandlers.UseInField[item] != null;

    public static bool hasUseOnPokemon(Items item) => !UnityItemHandlers.UseOnPokemon.ContainsKey(item) && UnityItemHandlers.UseOnPokemon[item] != null;

    public static bool hasBattleUseOnBattler(Items item) => !UnityItemHandlers.BattleUseOnBattler.ContainsKey(item) && UnityItemHandlers.BattleUseOnBattler[item] != null;

    public static bool hasBattleUseOnPokemon(Items item) => !UnityItemHandlers.BattleUseOnPokemon.ContainsKey(item) && UnityItemHandlers.BattleUseOnPokemon[item] != null;

    public static bool hasUseInBattle(Items item) => !UnityItemHandlers.UseInBattle.ContainsKey(item) && UnityItemHandlers.UseInBattle[item] != null;

    public static ItemUseResults triggerUseFromBag(Items item)
    {
      if (UnityItemHandlers.UseFromBag.ContainsKey(item) && UnityItemHandlers.UseFromBag[item] != null)
        return UnityItemHandlers.UseFromBag[item]();
      if (UnityItemHandlers.UseInField[item] == null)
        return ItemUseResults.NotUsed;
      UnityItemHandlers.UseInField[item]();
      return ItemUseResults.UsedNotConsumed;
    }

    public static bool triggerUseInField(Items item)
    {
      if (!UnityItemHandlers.UseInField.ContainsKey(item) || UnityItemHandlers.UseInField[item] == null)
        return false;
      UnityItemHandlers.UseInField[item]();
      return true;
    }

    public static bool triggerUseOnPokemon(Items item, IPokemon pokemon, IHasDisplayMessage scene) => UnityItemHandlers.UseOnPokemon.ContainsKey(item) && UnityItemHandlers.UseOnPokemon[item] != null && UnityItemHandlers.UseOnPokemon[item](item, pokemon, scene);

    public static bool triggerBattleUseOnBattler(
      Items item,
      IBattler battler,
      IHasDisplayMessage scene)
    {
      return false;
    }

    public static bool triggerBattleUseOnPokemon(
      Items item,
      IPokemon pokemon,
      IBattler battler,
      IHasDisplayMessage scene)
    {
      return false;
    }

    public static bool triggerBattleUseOnBattler(
      Items item,
      IBattler battler,
      IPokeBattle_Scene scene)
    {
      return UnityItemHandlers.BattleUseOnBattler.ContainsKey(item) && UnityItemHandlers.BattleUseOnBattler[item] != null && UnityItemHandlers.BattleUseOnBattler[item](item, battler, scene);
    }

    public static bool triggerBattleUseOnPokemon(
      Items item,
      IPokemon pokemon,
      IBattler battler,
      IPokeBattle_Scene scene)
    {
      return UnityItemHandlers.BattleUseOnPokemon.ContainsKey(item) && UnityItemHandlers.BattleUseOnPokemon[item] != null && UnityItemHandlers.BattleUseOnPokemon[item](pokemon, battler, scene);
    }

    public static void triggerUseInBattle(Items item, IBattler battler, UnityBattle battle)
    {
      if (!UnityItemHandlers.UseInBattle.ContainsKey(item) || UnityItemHandlers.UseInBattle[item] == null)
        return;
      UnityItemHandlers.UseInBattle[item](item, battler, (IBattle) battle);
    }
  }
}

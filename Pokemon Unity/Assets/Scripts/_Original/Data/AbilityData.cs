using System.Collections;
using UnityEngine;

public class AbilityData
{
    protected string name, fr_name, sp_name;

    public virtual IEnumerator EffectOnSent(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on sent");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnFainted(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on fainted");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnEachTurn(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on each turn");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnPermanently(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on permanently");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnWhenAttacking(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on attacking");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnWhenHurt(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on hurt");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnOnHp(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on HP");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnOnClimate(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect on climate");
        yield return null;
    }
    
    public virtual IEnumerator EffectOnAfterBattle(BattleHandler battleHandler, int pokemonIndex, int other = -1)
    {
        Debug.Log("[Ability] Calling " + name + " Ability Effect after battle");
        yield return null;
    }

    public AbilityData()
    {
        this.name = "Default";
    }

    public string GetLangName()
    {
        switch (Language.getLang())
        {
            default:
                return name;
            case Language.Country.FRANCAIS:
                return fr_name;
        }
    }

    public string GetName()
    {
        return name;
    }
}
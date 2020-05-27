using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Localization;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Combat
{
    //ToDo: Separate into new class and inherit?
    //public class ShadowPokemon : Pokemon 
    public partial class Pokemon 
    {
  public void InitPokemon(Pokemon pkmn, sbyte pkmnIndex, string placeholder) { 
    if (pokemonIndex>0 && inHyperMode() && !isFainted()) { 
      // Called out of hypermode
      pkmn.isHyperMode=false;
      //pkmn.adjustHeart(-50);
      pkmn.pokemon.decreaseShadowLevel(Monster.Pokemon.PokemonActions.Battle);
    }
    this.InitPokemon(pkmn, pkmnIndex);
    // Called into battle
    if (isShadow()) { 
      //if (hasConst(Types.SHADOW))
        Type1=Types.SHADOW;
        Type2=Types.SHADOW;
      //}
      //if (@battle.pbOwnedByPlayer(@Index)) this.pokemon.adjustHeart(-30);
    }
  }


  public void pbEndTurn(Battle.Choice choice, string placeholder) { 
    this.pbEndTurn(choice);
    if (inHyperMode() && !this.battle.pbAllFainted(this.battle.party1) &&
       !this.battle.pbAllFainted(this.battle.party2)) { 
      this.battle.pbDisplay(_INTL("Its hyper mode attack hurt {1}!",this.ToString(true)));
      pbConfusionDamage();
    }
  }

  public bool isShadow() {
    Monster.Pokemon p=this.pokemon;
    if (p.IsNotNullOrNone()) //&& p.respond_to("heartgauge") && 
        //p.ShadowLevel.HasValue && p.ShadowLevel.Value>0)
      return p.isShadow;//true;
    return false;
  }

  public bool inHyperMode() { 
    if (isFainted()) return false;
    Monster.Pokemon p=this.pokemon;
    if (p.IsNotNullOrNone() && //p.respond_to?("hypermode") && 
        IsHyperMode)//p.hypermode)
      return true;
    return false;
  }

  public void pbHyperMode() { 
    Monster.Pokemon p=this.pokemon;
    if (isShadow() && !IsHyperMode)
      if (@battle.pbRandom(p.ShadowLevel.Value)<=p.HeartGuageSize/4) { //PokeBattle_Pokemon.HEARTGAUGESIZE
        isHyperMode=true;
        @battle.pbDisplay(_INTL("{1}'s emotions rose to a fever pitch!\nIt entered Hyper Mode!",this.ToString()));
      }
  }

  public bool pbHyperModeObedience(Move move) { 
    if (!move.IsNotNullOrNone()) return true;
    if (this.inHyperMode() && move.Type!=Types.SHADOW)
      return @battle.pbRandom(10)<8 ? false : true;
    return true;
  }
    }
}
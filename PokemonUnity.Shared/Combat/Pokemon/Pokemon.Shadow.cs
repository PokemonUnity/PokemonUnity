using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;


namespace PokemonUnity.Combat
{
    public partial class Pokemon : Combat.IShadowPokemon
    {
  public void InitPokemon(Monster.Pokemon pkmn, sbyte pkmnIndex) { //, params object[] placeholder
    if (pokemonIndex>0 && inHyperMode() && !isFainted()) { 
      // Called out of hypermode
      pkmn.hypermode=false;
      //pkmn.isHyperMode=false;
      //pkmn.adjustHeart(-50);
      pkmn.decreaseShadowLevel(Monster.Pokemon.PokemonActions.CallTo);
    }
    this._InitPokemon(pkmn, pkmnIndex);
    // Called into battle
    if (isShadow()) { 
      //if (hasConst(Types.SHADOW))
        Type1=Types.SHADOW;
        Type2=Types.SHADOW;
      //}
      //if (@battle.pbOwnedByPlayer(@Index)) this.pokemon.adjustHeart(-30);
      if (@battle.pbOwnedByPlayer(@Index)) this.pokemon.decreaseShadowLevel(Monster.Pokemon.PokemonActions.Battle);
    }
  }


  public virtual void pbEndTurn(Choice choice) { //, params object[] placeholder
    this._pbEndTurn(choice);
    if (inHyperMode() && !this.battle.pbAllFainted(this.battle.party1) &&
       !this.battle.pbAllFainted(this.battle.party2)) { 
      this.battle.pbDisplay(Game._INTL("Its hyper mode attack hurt {1}!",this.ToString(true)));
      pbConfusionDamage();
    }
  }

  public bool isShadow() {
    Monster.Pokemon p=this.pokemon;
    if (p.IsNotNullOrNone() && p is Monster.IShadowPokemon && ((Monster.IShadowPokemon)p).heartgauge>0) //ToDo: p.ShadowLevel.HasValue && p.ShadowLevel.Value>0)
      return true;//p.isShadow;
    return false;
  }

  public bool inHyperMode() { 
    if (isFainted()) return false;
    Monster.Pokemon p=this.pokemon;
    if (p.IsNotNullOrNone() && p is Monster.IShadowPokemon && ((Monster.IShadowPokemon)p).hypermode) //&& IsHyperMode)
      return true;
    return false;
  }

  public void pbHyperMode() { 
    Monster.Pokemon p=this.pokemon;
    if (isShadow() && !IsHyperMode)
      if (@battle.pbRandom(p.ShadowLevel.Value)<=p.HeartGuageSize/4) { //Pokemon.HEARTGAUGESIZE
        isHyperMode=true;
        @battle.pbDisplay(Game._INTL("{1}'s emotions rose to a fever pitch!\nIt entered Hyper Mode!",this.ToString()));
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
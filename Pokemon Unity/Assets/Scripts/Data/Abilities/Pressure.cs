using System.Collections;
using System.Diagnostics;

namespace Data.Abilities
{
    public class Pressure : AbilityData
    {
        public Pressure()
        {
            name = "Pressure";
            fr_name = "Pression";
        }

        public override IEnumerator EffectOnSent(BattleHandler battleHandler, int pokemonIndex, int other = -1)
        {
            DialogBoxHandlerNew dialog = battleHandler.getDialog();
            //Pokemon user = battleHandler.getPokemon(pokemonIndex);
            string dialog_string = PokemonUnity.Game._INTL(" is exerting its pressure!");
            //Language.getLang() switch
            //{
            //    Language.Country.FRANCAIS => " exerce la pression!",
            //    _ => " is exerting its pressure!"
            //};

            yield return base.EffectOnSent(battleHandler, pokemonIndex);

            yield return battleHandler.StartCoroutine(battleHandler.DisplayAbility(pokemonIndex, GetLangName()));

            dialog.DrawBlackFrame();
            yield return battleHandler.StartCoroutine(battleHandler.drawTextAndWait(battleHandler.preStringName(pokemonIndex) + dialog_string, 2f, 1f));
            dialog.UndrawDialogBox();

            if (pokemonIndex > 2)
            {
                battleHandler.pressureOpponent = true;
            }
            else
            {
                battleHandler.pressurePlayer = true;
            }
            
            yield return battleHandler.StartCoroutine(battleHandler.HideAbility(pokemonIndex));

            
        }

        public override IEnumerator EffectOnFainted(BattleHandler battleHandler, int pokemonIndex, int other = -1)
        {
            int partner = pokemonIndex;
            //pokemonIndex switch
            //{
            //    0 => 1,
            //    1 => 0,
            //    2 => 3,
            //    3 => 2,
            //    _ => -1
            //};
            
            yield return base.EffectOnFainted(battleHandler, pokemonIndex);

            if (battleHandler.getPokemon(partner) == null || battleHandler.getPokemon(partner) != null)
                //&& PokemonDatabase.getPokemon(battleHandler.getPokemon(partner).getID()).getAbility(battleHandler.getPokemon(1).getAbility()) != "Pressure")
            {
                if (pokemonIndex > 2)
                {
                    battleHandler.pressureOpponent = false;
                }
                else
                {
                    battleHandler.pressurePlayer = false;
                }
            }
        }
    }
}
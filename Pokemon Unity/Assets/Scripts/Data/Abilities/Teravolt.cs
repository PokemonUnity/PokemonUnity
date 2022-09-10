using System.Collections;

namespace Data.Abilities
{
    public class Teravolt : AbilityData
    {
        public Teravolt()
        {
            name = "Teravolt";
            fr_name = "Téra-Voltage";
        }
        
        public override IEnumerator EffectOnSent(BattleHandler battleHandler, int pokemonIndex, int other)
        {
            DialogBoxHandlerNew dialog = battleHandler.getDialog();
            //Pokemon user = battleHandler.getPokemon(pokemonIndex);
            string dialog_string = PokemonUnity.Game._INTL(" is radiating\na bursting aura!");
            //Language.getLang() switch
            //{
            //    Language.Country.FRANCAIS => " dégage une aura\nélectrique instable",
            //    _ => " is radiating\na bursting aura!"
            //};

            yield return base.EffectOnSent(battleHandler, pokemonIndex);

            yield return battleHandler.StartCoroutine(battleHandler.DisplayAbility(pokemonIndex, GetLangName()));

            dialog.DrawBlackFrame();
            yield return battleHandler.StartCoroutine(battleHandler.drawTextAndWait(battleHandler.preStringName(pokemonIndex) + dialog_string, 2f, 1f));
            dialog.UndrawDialogBox();

            //TODO Pressure effect
            
            yield return battleHandler.StartCoroutine(battleHandler.HideAbility(pokemonIndex));
        }
    }
}
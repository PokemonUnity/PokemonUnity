using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface;

namespace PokemonUnity
{
	namespace Monster {
		public partial class Pokemon : PokemonEssentials.Interface.PokeBattle.IPokemonChatter {
			public PokemonEssentials.Interface.IWaveData chatter				{ get; set; }
		}
	}
	public partial class Game : PokemonEssentials.Interface.IGameChatter {
		public void Chatter(PokemonEssentials.Interface.PokeBattle.IPokemonChatter pokemon) {
			//using (PokemonEssentials.Interface.IPictureWindow iconwindow = null) {
				IPictureWindow iconwindow=null; //new PictureWindow(UI.LoadPokemonBitmap(pokemon));
				iconwindow.x=(Graphics.width/2)-(iconwindow.width/2);
				iconwindow.y=((Graphics.height-96)/2)-(iconwindow.height/2);
				if (pokemon.chatter != null)
				{
					(this as IGameMessage).Message(_INTL("It will forget the song it knows."));
					if (!(this as IGameMessage).ConfirmMessage(_INTL("Are you sure you want to change it?")))
					{
						iconwindow.Dispose();
						return;
					}
				}
				if ((this as IGameMessage).ConfirmMessage(_INTL("Do you want to change its song now?"))) {
					PokemonEssentials.Interface.IWaveData wave=null;//Record(null,5);
					if (wave != null && pokemon is IPokemon p) {
						pokemon.chatter=wave;
						(this as IGameMessage).Message(_INTL("{1} learned a new song!",p.Name));
					}
				}
				iconwindow.Dispose();
				return;
			//}
		}
	}

	//HiddenMoveHandlers.addCanUseMove(:CHATTER,proc {|item,pokemon|
	//   return true;
	//});
	//
	//HiddenMoveHandlers.addUseMove(:CHATTER,proc {|item,pokemon|
	//   Chatter(pokemon);
	//   return true;
	//});

	//public partial class PokeBattle_Scene : IBattle, ISceneHasChatter {
	//  public System.Collections.IEnumerator Chatter(IBattler attacker,IBattler opponent) {
	//    //if (attacker.pokemon.IsNotNullOrNone()) {
	//    //  PlayCry(attacker.pokemon,90,100);
	//    //}
	//    //int i = 0; do { //;Graphics.frame_rate.times
	//    //  Graphics?.update();
	//    //  Input.update(); i++;
	//    //} while (i < Graphics.frame_rate);
	//    yield return null;
	//  }
	//}
}
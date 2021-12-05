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
			public PokemonEssentials.Interface.IAudioObject chatter				{ get; set; }
		}
	}
	public partial class Game : PokemonEssentials.Interface.IGameChatter { 
		public void pbChatter(PokemonEssentials.Interface.PokeBattle.IPokemonChatter pokemon) {
			using (PokemonEssentials.Interface.IWindow iconwindow = null) { 
				//IPictureWindow iconwindow=new PictureWindow(UI.pbLoadPokemonBitmap(pokemon));
				//iconwindow.x=(Graphics.width/2)-(iconwindow.width/2);
				//iconwindow.y=((Graphics.height-96)/2)-(iconwindow.height/2);
				if (pokemon.chatter != null)
				{
					pbMessage(_INTL("It will forget the song it knows."));
					if (!pbConfirmMessage(_INTL("Are you sure you want to change it?")))
					{
						//iconwindow.dispose();
						return;
					}
				}
				if (pbConfirmMessage(_INTL("Do you want to change its song now?"))) {
					PokemonEssentials.Interface.IAudioObject wave=null;//UI.pbRecord(null,5);
					if (wave != null) {
						pokemon.chatter=wave;
						pbMessage(_INTL("{1} learned a new song!",pokemon.Name));
					}
				}
				//iconwindow.dispose();
				return;
			}
		}
	}

	//HiddenMoveHandlers.addCanUseMove(:CHATTER,proc {|item,pokemon|
	//   return true;
	//});
	//
	//HiddenMoveHandlers.addUseMove(:CHATTER,proc {|item,pokemon|
	//   pbChatter(pokemon);
	//   return true;
	//});

	//public partial class PokeBattle_Scene : IBattle, ISceneHasChatter {
	//  public System.Collections.IEnumerator pbChatter(IBattler attacker,IBattler opponent) {
	//    //if (attacker.pokemon.IsNotNullOrNone()) {
	//    //  pbPlayCry(attacker.pokemon,90,100);
	//    //}
	//    //int i = 0; do { //;Graphics.frame_rate.times 
	//    //  Graphics.update();
	//    //  Input.update(); i++;
	//    //} while (i < Graphics.frame_rate);
	//    yield return null;
	//  }
	//}
}
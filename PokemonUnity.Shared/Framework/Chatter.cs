using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
namespace Combat { 
public partial class Pokemon {
  public IAudioObject chatter				{ get; set; }
}
}
public partial class Game { 
public void pbChatter(Combat.Pokemon pokemon) {
  IWindow iconwindow = null;
  //PictureWindow iconwindow=new PictureWindow(UI.pbLoadPokemonBitmap(pokemon));
  //iconwindow.x=(Graphics.width/2)-(iconwindow.width/2);
  //iconwindow.y=((Graphics.height-96)/2)-(iconwindow.height/2);
  if (pokemon.chatter != null) {
    UI.pbMessage(_INTL("It will forget the song it knows."));
    if (!UI.pbConfirmMessage(_INTL("Are you sure you want to change it?"))) {
      iconwindow.dispose();
      return;
    }
  }
  if (UI.pbConfirmMessage(_INTL("Do you want to change its song now?"))) {
    IAudioObject wave=null;//UI.pbRecord(null,5);
    if (wave != null) {
      pokemon.chatter=wave;
      UI.pbMessage(_INTL("{1} learned a new song!",pokemon.Name));
    }
  }
  iconwindow.dispose();
  return;
}
}
}
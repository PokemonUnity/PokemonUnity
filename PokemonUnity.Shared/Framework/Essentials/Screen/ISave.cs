using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
    public interface IGameSave {

        void pbEmergencySave();

        bool pbSave(bool safesave = false);
    }

	public interface ISaveScene : IScene {
        void pbStartScreen();

        void pbEndScreen();
    }

    public interface ISaveScreen : IScreen {
        ISaveScreen initialize(ISaveScene scene);

        void pbDisplay(string text, bool brief = false);

        void pbDisplayPaused(string text);

        bool pbConfirm(string text);

        bool pbSaveScreen();
    }
}
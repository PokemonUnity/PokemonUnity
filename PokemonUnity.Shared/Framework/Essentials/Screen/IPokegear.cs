using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Interface;
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
    /// <summary>
    /// PokéGear Scene
    /// </summary>
    /// Modified By Harshboy
    /// Modified by Peter O.
    /// Also Modified By OblivionMew
    /// Overhauled by Maruno
    public interface IPokegearScene : IScene {
		// -----------------------------------------------------------------------------
		//  initialize
		// -----------------------------------------------------------------------------
		IPokegearScene initialize(int menu_index = 0);
		// -----------------------------------------------------------------------------
		//  main
		// -----------------------------------------------------------------------------
		void main();
		/// <summary>
		/// update the scene
		/// </summary>
		void update();
		/// <summary>
		/// update the command window
		/// </summary>
		void update_command();
	}

	#region UI Elements
	public interface IPokegearButton : ISpriteWrapper {
		int index				{ get; }
		int name				{ get; }
		bool selected			{ get; }

		IPokegearButton initialize(float x, float y, string name = "", int index = 0, IViewport viewport = null);

		//void dispose();

		void refresh();

		//void update();
	}
	#endregion
}
using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonUnity;
using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// Pokémon sprite (used out of battle)
	/// </summary>
	public interface IPokemonSprite : ISpriteWrapper 
	{
		new IPokemonSprite initialize(IViewport viewport = null);

		//void dispose();

		//void update();

		void clearBitmap();

		void setPokemonBitmap(IPokemon pokemon, bool back = false);

		void setPokemonBitmapSpecies(IPokemon pokemon, Pokemons species, bool back = false);

		void setSpeciesBitmap(Pokemons species, bool female = false, int form = 0, bool shiny = false, bool shadow = false, bool back = false, bool egg = false);
	}

	/// <summary>
	/// Pokémon sprite (used in battle)
	/// </summary>
	public interface IPokemonBattlerSprite //: RPG.ISprite 
	{
		int selected				{ get; set; }

		IPokemonBattlerSprite initialize(bool doublebattle, int index, IViewport viewport = null);

		float x { get; set; }
		float y { get; set; }
		/// <summary>
		/// not sure what this is used for...
		/// </summary>
		float z { get; set; }
		/// <summary>
		/// not sure what this is used for...
		/// </summary>
		ITone tone { get; set; }

		float width { get; }
		float height { get; }

		void dispose();

		//int selected { set; }

		bool visible { get; set; }

		void setPokemonBitmap(IPokemon pokemon, bool back = false);
		void setPokemonBitmap(PokemonUnity.Monster.Forms pokemon, bool back = false);

		void setPokemonBitmapSpecies(IPokemon pokemon, Pokemons species, bool back = false);

		void update();
	}

	/// <summary>
	/// Pokémon icon (for defined Pokémon)
	/// </summary>
	public interface IPokemonIconSprite : ISpriteWrapper {
		int selected				{ get; set; }
		int active				{ get; set; }
		IPokemon pokemon				{ get; set; }

		IPokemonIconSprite initialize(IPokemon pokemon, IViewport viewport = null);


		//void dispose();

		float x { get; set; }

		float y { get; set; }

		//void update();
	}

	/// <summary>
	/// Pokémon icon (for species)
	/// </summary>
	public interface IPokemonSpeciesIconSprite : ISpriteWrapper {
		int selected				{ get; set; }
		bool active					{ get; set; }
		Pokemons species			{ get; set; }
		int gender					{ get; set; }
		int form					{ get; set; }

		float x { get; set; }
		float y { get; set; }

		IPokemonSpeciesIconSprite initialize(Pokemons species, IViewport viewport = null);

		void pbSetParams(Pokemons species, int gender, int form);

		//void dispose();

		void refresh();

		//void update();
	}

	/// <summary>
	/// Sprite position adjustments
	/// </summary>
	public interface IGameSprite
	{
		float getBattleSpriteMetricOffset(Pokemons species, int index, int[] metrics = null);

		int adjustBattleSpriteY(ISprite sprite, Pokemons species, int index, int[] metrics = null);

		void pbPositionPokemonSprite(ISprite sprite, float left, float top);

		void pbSpriteSetCenter(ISprite sprite, float cx, float cy);

		bool showShadow(Pokemons species);
	}
}
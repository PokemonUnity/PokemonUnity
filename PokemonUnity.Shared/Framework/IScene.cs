using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
	/// <summary>
	/// A scene basically represents unity (or any frontend) where code pauses 
	/// for user interaction (animation, and user key inputs).
	/// </summary>
	/// <remarks>
	/// When code has a scene variable calling a method in middle of script
	/// everything essentially comes to a hault as the frontend takes over 
	/// and the code awaits a result or response to begin again.
	/// </remarks>
	public interface IScene
	{
		/// <summary>
		/// Shows the player's Poké Ball being thrown to capture a Pokémon.
		/// </summary>
		void pokeballThrow(Items ball, int shakes,bool critical,Combat.Pokemon targetBattler,IScene scene,Combat.Pokemon battler, int burst = -1, bool showplayer = false);
		void pbDisplay(string v);
		bool pbConfirm(string v);
	}
	/*
	/// <summary>
	/// Command menu (Fight/Pokémon/Bag/Run)
	/// </summary>
	interface ICommandMenuDisplay
	{
		void initialize(viewport= null);
		double x; //@window.x;
		double y; //@window.y;
		double z; //@window.z;
		double ox; //@window.ox;
		double oy; //@window.oy;
		bool visible; //@window.visible;
		int color; //@window.color;
		bool disposed();
		void dispose();
		int index; //@window.index;

		void setTexts(string value);
		void refresh();
		void update();
	}

	interface ICommandMenuButtons //: BitmapSprite
	{
		void initialize(int index = 0, int mode = 0, viewport= null);
		void dispose();
		void update(int index = 0, int mode = 0);
		void refresh(int index, int mode = 0);
	}

	/// <summary>
	/// Fight menu (choose a move)
	/// </summary>
	interface IFightMenuDisplay
	{
		float x		{ get; }
		float y		{ get; }
		float z		{ get; }
		float ox		{ get; }
		float oy		{ get; }
		bool visible	{ get; }
		int color		{ get; }
		Combat.Pokemon battler	{ get; }
		void initialize(Combat.Pokemon battler, viewport= null);
		bool disposed();
		void dispose();
		void setIndex(int value);
		void refresh();
		void update();
	}

	interface IFightMenuButtons //: BitmapSprite
	{
		void initialize(int index = 0,Moves[] moves= null, viewport= null);
		void dispose();
		void update(int index = 0,Moves[] moves= null, int megaButton = 0);
		void refresh(int index,Moves[] moves,int megaButton);
	}

	/// <summary>
	/// Data box for safari battles
	/// </summary>
	interface ISafariDataBox //: SpriteWrapper
	{
		void initialize(battle, viewport= null);
		void appear();
		void refresh();
		void update();
	}

	/// <summary>
	/// Data box for regular battles (both single and double)
	/// </summary>
	interface IPokemonDataBox //: SpriteWrapper
	{
		void initialize(Combat.Pokemon battler, bool doublebattle, viewport= null);
		void dispose();
		void refreshExpLevel();
		void exp();
		void hp();
		void animateHP(int oldhp, int newhp);
		void animateEXP(int oldexp, int newexp);
		void appear();
		void refresh();
		void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s)'s Pokémon being thrown out.  It appears at coords
	/// (@spritex,@spritey), and moves in y to @endspritey where it stays for the rest
	/// of the battle, i.e. the latter is the more important value.
	/// Doesn't show the ball itself being thrown.
	/// </summary>
	interface IPokeballSendOutAnimation
	{
		void initialize(sprite, spritehash, pkmn, illusionpoke,bool doublebattle);
		bool disposed();
		bool animdone();
		void dispose();
		void update();
	}

	/// <summary>
	/// Shows the player's (or partner's) Pokémon being thrown out.  It appears at
	/// (@spritex,@spritey), and moves in y to @endspritey where it stays for the rest
	/// of the battle, i.e. the latter is the more important value.
	/// Doesn't show the ball itself being thrown.
	/// </summary>
	interface IPokeballPlayerSendOutAnimation
	{
		void initialize(sprite, spritehash, pkmn, illusionpoke,bool doublebattle);
		bool disposed();
		bool animdone();
		void dispose();
		void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s) and the enemy party lineup sliding off screen.
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	interface ITrainerFadeAnimation
	{
		void initialize(sprites);
		bool animdone();
		void update();
	}

	/// <summary>
	/// Shows the player (and partner) and the player party lineup sliding off screen.
	/// Shows the player's/partner's throwing animation (if they have one).
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	interface IPlayerFadeAnimation
	{
		void initialize(sprites);
		bool animdone();
		void update();
	}*/
}
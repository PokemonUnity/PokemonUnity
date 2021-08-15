using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;

namespace PokemonUnity.UI
{
	public class Scene : IPokeBattle_Scene
	{
		#region EventHandlers
		public event EventHandler<OnLevelUpEventArgs> OnLevelUp;
		public event EventHandler<OnBattlePhaseEventArgs> OnBattlePhase;
		public event EventHandler<OnHPChangedEventArgs> OnHPChanged;
		public event EventHandler<OnFaintedEventArgs> OnFainted;
		public event EventHandler<OnDisplayEventArgs> OnDisplay;
		public event EventHandler<OnBattleMenuEventArgs> OnBattleMenu;
		//public event FightMenuEventDelegate OnFightMenuEvent;
		#endregion

		#region EventArgs
		public class OnLevelUpEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnLevelUpEventArgs).GetHashCode();

			public int Id { get { return Pokemon.GetHashCode(); } } //EventId;
			public Combat.Pokemon Pokemon { get; set; }
			public Combat.Pokemon Battler { get; set; }
			public int HP { get; set; }
			public int Atk { get; set; }
			public int Def { get; set; }
			public int Spe { get; set; }
			public int Spa { get; set; }
			public int Spd { get; set; }
		}
		public class OnBattlePhaseEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnBattlePhaseEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public int Phase { get; set; }
		}
		public class OnHPChangedEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnHPChangedEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public int Index { get; set; }
			public int Oldhp { get; set; }
			public bool Animate { get; set; }
		}
		public class OnFaintedEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnFaintedEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public int Index { get; set; }
		}
		public class OnDisplayEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnDisplayEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public bool Pause { get; set; }
			/// <summary>
			/// Will continue after a short wait, regardless of player interaction
			/// </summary>
			public bool Brief { get; set; }
			public string Message { get; set; }
		}
		public class OnBattleMenuEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnBattleMenuEventArgs).GetHashCode();

			public int Id { get; }
			public int Index { get; set; }
			public int Option { get; set; }
			public int Mode { get; set; }
		}
		#endregion

		#region Delegate
		//public delegate int FightMenuEventDelegate(int i);
		#endregion

		public void ChangePokemon()
		{
			throw new NotImplementedException();
		}

		public void Fainted(int pkmn)
		{
			if (OnFainted != null) OnFainted.Invoke(this, new OnFaintedEventArgs { Index = pkmn });
		}

		public void HPChanged(int index, int oldhp, bool animate = false)
		{
			if (OnHPChanged != null) OnHPChanged.Invoke(this, new OnHPChangedEventArgs { Index = index, Oldhp = oldhp, Animate = animate });
		}

		public void initialize()
		{
			throw new NotImplementedException();
		}

		public bool inPartyAnimation()
		{
			throw new NotImplementedException();
		}

		public void partyAnimationUpdate()
		{
			throw new NotImplementedException();
		}

		public void pbAddPlane(int id, string filename, int viewport)
		{
			throw new NotImplementedException();
		}

		public void pbAddSprite(int id, double x, double y, string filename, int viewport)
		{
			throw new NotImplementedException();
		}

		public void pbAnimation(Moves moveid, Combat.Pokemon user, Combat.Pokemon target, int hitnum = 0)
		{
			throw new NotImplementedException();
		}

		public void pbAnimationCore(string animation, Combat.Pokemon user, Combat.Pokemon target, bool oppmove = false)
		{
			throw new NotImplementedException();
		}

		public void pbBackdrop()
		{
			throw new NotImplementedException();
		}

		public void pbBeginAttackPhase()
		{
			if (OnBattlePhase != null) OnBattlePhase.Invoke(this, new OnBattlePhaseEventArgs { Phase = 0 });
		}

		public void pbBeginCommandPhase()
		{
			if (OnBattlePhase != null) OnBattlePhase.Invoke(this, new OnBattlePhaseEventArgs { Phase = 0 });
		}

		public void pbChangePokemon(Combat.Pokemon attacker, Forms pokemon)
		{
			throw new NotImplementedException();
		}

		public void pbChangeSpecies(Combat.Pokemon attacker, Pokemons species)
		{
			throw new NotImplementedException();
		}

		public void pbChooseEnemyCommand(int index)
		{
			throw new NotImplementedException();
		}

		public void pbChooseMove(Combat.Pokemon pokemon, string message)
		{
			throw new NotImplementedException();
		}

		public int pbChooseNewEnemy(int index, Combat.Pokemon[] party)
		{
			throw new NotImplementedException();
		}

		public void pbChooseTarget(int index, int targettype)
		{
			throw new NotImplementedException();
		}

		public int pbCommandMenu(int index)
		{
			OnBattleMenuEventArgs e = new OnBattleMenuEventArgs { Index = index, Mode = 0 };
			if (OnBattleMenu != null) OnBattleMenu.Invoke(this, e);
			return e.Option;
		}

		public int pbCommandMenuEx(int index, string texts, int mode = 0)
		{
			OnBattleMenuEventArgs e = new OnBattleMenuEventArgs { Index = index, Mode = mode };
			if (OnBattleMenu != null) OnBattleMenu.Invoke(this, e);
			return e.Option;
		}

		public void pbCommonAnimation(string animname, Combat.Pokemon user, Combat.Pokemon target, int hitnum = 0)
		{
			throw new NotImplementedException();
		}

		public void pbDamageAnimation(Combat.Pokemon pkmn, float effectiveness)
		{
			throw new NotImplementedException();
		}

		public void pbDisplay(string msg, bool brief = false)
		{
			if (OnDisplay != null) OnDisplay.Invoke(this, new OnDisplayEventArgs { Message = msg, Brief = brief, Pause = false });
		}

		public void pbDisplayConfirmMessage(string msg)
		{
			//ToDo: Add Confirmation Prompt
			if (OnDisplay != null) OnDisplay.Invoke(this, new OnDisplayEventArgs { Message = msg, Brief = false, Pause = true });
		}

		public void pbDisplayMessage(string msg, bool brief = false)
		{
			if (OnDisplay != null) OnDisplay.Invoke(this, new OnDisplayEventArgs { Message = msg, Brief = brief, Pause = false });
		}

		public void pbDisplayPausedMessage(string msg)
		{
			if (OnDisplay != null) OnDisplay.Invoke(this, new OnDisplayEventArgs { Message = msg, Brief = false, Pause = true });
		}

		public void pbDisposeSprites()
		{
			throw new NotImplementedException();
		}

		public void pbEndBattle(BattleResults result)
		{
			if (OnBattlePhase != null) OnBattlePhase.Invoke(this, new OnBattlePhaseEventArgs { Phase = 0 });
		}

		public void pbEXPBar(Combat.Pokemon pokemon, Combat.Pokemon battler, int startexp, int endexp, int tempexp1, int tempexp2)
		{
			//Always animate from 0 to 100...
			int zero = 0;
			int hundred = endexp - startexp;
			int start = tempexp1 - startexp;
			int finish = tempexp2 - startexp;
			throw new NotImplementedException();
		}

		public void pbFainted(int pkmn)
		{
			if (OnFainted != null) OnFainted.Invoke(this, new OnFaintedEventArgs { Index = pkmn });
		}

		public int pbFightMenu(int index)
		{
			OnBattleMenuEventArgs e = new OnBattleMenuEventArgs { Index = index, Mode = 0 };
			if (OnBattleMenu != null) OnBattleMenu.Invoke(this, e);
			return e.Option;
		}

		public void pbFindAnimation(Moves moveid, int userIndex, int hitnum)
		{
			throw new NotImplementedException();
		}

		public void pbFirstTarget(int index, int targettype)
		{
			throw new NotImplementedException();
		}

		public int pbForgetMove(Combat.Pokemon pokemon, Moves moveToLearn)
		{
			throw new NotImplementedException();
		}

		public void pbFrameUpdate(object cw = null)
		{
			throw new NotImplementedException();
		}

		public void pbGraphicsUpdate()
		{
			throw new NotImplementedException();
		}

		public void pbHideCaptureBall()
		{
			throw new NotImplementedException();
		}

		public void pbHideHelp()
		{
			throw new NotImplementedException();
		}

		public void pbHideOpponent()
		{
			throw new NotImplementedException();
		}

		public void pbHPChanged(int pkmn, int oldhp, bool anim = false)
		{
			if (OnHPChanged != null) OnHPChanged.Invoke(this, new OnHPChangedEventArgs { Index = pkmn, Oldhp = oldhp, Animate = anim });
		}

		public void pbHPChanged(Combat.Pokemon pkmn, int oldhp, bool animate = false)
		{
			if (OnHPChanged != null) OnHPChanged.Invoke(this, new OnHPChangedEventArgs { Index = pkmn.Index, Oldhp = oldhp, Animate = animate });
		}

		public void pbInputUpdate()
		{
			throw new NotImplementedException();
		}

		public int[] pbItemMenu(int index)
		{
			throw new NotImplementedException();
		}

		public void pbLevelUp(Combat.Pokemon pokemon, Combat.Pokemon battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
		{
			if (OnLevelUp != null) OnLevelUp.Invoke(this, new OnLevelUpEventArgs { Pokemon = pokemon, Battler = battler, HP = oldtotalhp, Atk = oldattack, Def = olddefense, Spe = oldspatk, Spa = oldspatk, Spd = oldspdef });
		}

		public void pbMoveString(string move)
		{
			throw new NotImplementedException();
		}

		public string pbNameEntry(string helptext, Monster.Pokemon pokemon)
		{
			throw new NotImplementedException();
		}

		public void pbRecall(int battlerindex)
		{
			throw new NotImplementedException();
		}

		public void pbRefresh()
		{
			throw new NotImplementedException();
		}

		public void pbResetCommandIndices()
		{
			throw new NotImplementedException();
		}

		public void pbResetMoveIndex(int index)
		{
			throw new NotImplementedException();
		}

		public int pbSafariCommandMenu(int index)
		{
			OnBattleMenuEventArgs e = new OnBattleMenuEventArgs { Index = index, Mode = 0 };
			if (OnBattleMenu != null) OnBattleMenu.Invoke(this, e);
			return e.Option;
		}

		public void pbSafariStart()
		{
			throw new NotImplementedException();
		}

		public void pbSaveShadows()
		{
			throw new NotImplementedException();
		}

		public void pbSelectBattler(int index, int selectmode = 1)
		{
			throw new NotImplementedException();
		}

		public void pbSendOut(int battlerindex, Combat.Pokemon pkmn)
		{
			throw new NotImplementedException();
		}

		public void pbSetMessageMode(int mode)
		{
			throw new NotImplementedException();
		}

		public void pbShowCommands(string msg, string commands, bool defaultValue)
		{
			throw new NotImplementedException();
		}

		public void pbShowHelp(int text)
		{
			throw new NotImplementedException();
		}

		public void pbShowOpponent(int index)
		{
			throw new NotImplementedException();
		}

		public void pbShowPokedex(Pokemons species, int form = 0)
		{
			throw new NotImplementedException();
		}

		public void pbShowWindow(int windowtype)
		{
			throw new NotImplementedException();
		}

		public void pbStartBattle(Combat.Battle battle)
		{
			throw new NotImplementedException();
		}

		public int pbSwitch(int index, bool lax, bool cancancel)
		{
			throw new NotImplementedException();
		}

		public void pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer = false)
		{
			throw new NotImplementedException();
		}

		public void pbThrowAndDeflect(Items ball, int targetBattler)
		{
			throw new NotImplementedException();
		}

		public void pbThrowBait()
		{
			throw new NotImplementedException();
		}

		public void pbThrowRock()
		{
			throw new NotImplementedException();
		}

		public void pbThrowSuccess()
		{
			throw new NotImplementedException();
		}

		public void pbTrainerBattleSuccess()
		{
			throw new NotImplementedException();
		}

		public void pbTrainerSendOut(int battlerindex, Combat.Pokemon pkmn)
		{
			throw new NotImplementedException();
		}

		public void pbTrainerWithdraw(Combat.Battle battle, Combat.Pokemon pkmn)
		{
			throw new NotImplementedException();
		}

		public void pbUpdate()
		{
			throw new NotImplementedException();
		}

		public void pbUpdateSelected(int index)
		{
			throw new NotImplementedException();
		}

		public void pbWaitMessage()
		{
			throw new NotImplementedException();
		}

		public void pbWildBattleSuccess()
		{
			throw new NotImplementedException();
		}

		public void pbWithdraw(Combat.Battle battle, Combat.Pokemon pkmn)
		{
			throw new NotImplementedException();
		}

		public void pokeballThrow(Items ball, int shakes, bool critical, Combat.Pokemon targetBattler, IScene scene, Combat.Pokemon battler, int burst = -1, bool showplayer = false)
		{
			throw new NotImplementedException();
		}
	}
}
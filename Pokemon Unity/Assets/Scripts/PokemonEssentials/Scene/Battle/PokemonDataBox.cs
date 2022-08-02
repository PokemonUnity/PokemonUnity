using System;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// Same class can be used for both regular battles and safari battles, 
	/// but just need to call the initialize for the right one to load the correct attributes.
	/// Otherwise, you might just need to make an abstract factory for the constructor
	public class PokemonDataBox : SafariDataBox, IPokemonDataBox
	{
		public IBattler battler { get; }
		//public int selected { get; set; }
		//public bool appearing { get; }
		public bool animatingHP { get; }
		public bool animatingEXP { get; }
		public int exp { get; }
		public int hp { get; }

		public IPokemonDataBox initialize(IBattler battler, bool doublebattle, IViewport viewport = null)
		{
			return this;
		}

		//public void Dispose()
		//{
		//
		//}
				
		public void refreshExpLevel()
		{

		}
				
		public void animateHP(int oldhp, int newhp)
		{
			//Start Coroutine on Slider...
		}
				
		public void animateEXP(int oldexp, int newexp)
		{
			//Start Coroutine on Slider...
		}
		 
		//public void appear()
		//{
		//	//Toggle active and animate sliding onto screen from side
		//}
		//		
		//public override void refresh()
		//{
		//
		//}
		//		
		//public override void update()
		//{
		//
		//}
	}

	/// <summary>
	/// </summary>
	public class SafariDataBox : SpriteWrapper, ISafariDataBox
	{
		public int selected { get; set; }
		public bool appearing { get; }

		public ISafariDataBox initialize(IBattle battle, IViewport viewport = null)
		{
			return this;
		}
		 
		//public virtual void appear()
		//{
		//	//Toggle active and animate sliding onto screen from side
		//}
				
		public virtual void refresh()
		{

		}
				
		public override void update()
		{

		}
	}
}
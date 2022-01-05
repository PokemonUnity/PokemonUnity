using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	/// <summary>
	/// All the moves this pokemon species can learn, and the methods by which they learn them
	/// </summary>
	public struct PokemonMoveTree
	{
		#region Properties
		/// <summary>
		/// to use: LevelUp.OrderBy(x => x.Value).ThenBy(x => x.Key)
		/// </summary>
		public SortedList<Moves, int> LevelUp { get; private set; }
		public Moves[] Egg { get; private set; }
		public Moves[] Tutor { get; private set; }
		public Moves[] Machine { get; private set; }
		//Teach pikachu surf?... do we really need it to know surf?... Maybe if "specal form" (surfboard pickachu) is added to game
		//public Move.MoveData.Move[] stadium_surfing_pikachu { get; private set; }
		/*// <summary>
		/// If <see cref="Items.LIGHT_BALL"/> is held by either parent of a <see cref="Pokemons.Pichu"/> when the Egg is produced,
		/// the Pichu that hatches will know the move <see cref="Move.MoveData.Move.Volt_Tackle"/>.
		/// </summary>
		/// Not sure about this one
		public Moves[] light_ball_egg { get; private set; }*/
		//public Move.MoveData.Move[] colosseum_purification { get; private set; }
		//public Move.MoveData.Move[] xd_shadow { get; private set; }
		//public Move.MoveData.Move[] xd_purification { get; private set; }
		/// <summary>
		/// </summary>
		/// Merge both Colosseum and XD into one list
		public Moves[] Shadow { get; private set; }
		/// <summary>
		/// When a pokemon is purified from a shadow state, the moves they can potentially unlock?...
		/// </summary>
		/// Merge both Colosseum and XD into one list
		public Moves[] Purification { get; private set; }
		public Moves[] FormChange { get; private set; }
		#endregion
		public PokemonMoveTree(
				SortedList<Moves, int> levelup = null,
				Moves[] egg = null,
				Moves[] tutor = null,
				Moves[] machine = null,
				//Move.MoveData.Move[] stadium_surfing_pikachu = null,
				//Move.MoveData.Move[] light_ball_egg = null,
				Moves[] shadow = null,
				Moves[] purification = null,
				Moves[] form_change = null
			)
		{
			this.LevelUp = levelup ?? new SortedList<Moves, int>();
			this.Egg = egg ?? new Moves[0];
			this.Tutor = tutor ?? new Moves[0];
			this.Machine = machine ?? new Moves[0];
			//this.stadium_surfing_pikachu = 5,
			//this.light_ball_egg = light_ball_egg ?? new Move.MoveData.Move[0];
			this.Shadow = shadow ?? new Moves[0];
			this.Purification = purification ?? new Moves[0];
			this.FormChange = form_change ?? new Moves[0];
		}
		public PokemonMoveTree(PokemonMoveset[] moveset)
		{
			#region Foreach-Loop
			SortedList<Moves, int> level = new SortedList<Moves, int>();
			List<Moves> egg = new List<Moves>();
			List<Moves> tutor = new List<Moves>();
			List<Moves> machine = new List<Moves>();
			List<Moves> shadow = new List<Moves>();
			List<Moves> purify = new List<Moves>();
			List<Moves> form = new List<Moves>();
			if(moveset != null)
				foreach (PokemonMoveset move in moveset)
				{
					//ToDo: Generation Filter
					//if(move.Generation == Core.pokemonGeneration || Core.pokemonGeneration < 1)
					switch (move.TeachMethod)
					{
						case LearnMethod.levelup:
							if (!level.ContainsKey(move.MoveId)) level.Add(move.MoveId, move.Level);
							break;
						case LearnMethod.egg:
							if (!egg.Contains(move.MoveId)) egg.Add(move.MoveId);
							break;
						case LearnMethod.tutor:
							if (!tutor.Contains(move.MoveId)) tutor.Add(move.MoveId);
							break;
						case LearnMethod.machine:
							if (!machine.Contains(move.MoveId)) machine.Add(move.MoveId);
							break;
						case LearnMethod.stadium_surfing_pikachu:
							break;
						case LearnMethod.light_ball_egg:
							break;
						case LearnMethod.purification:
						case LearnMethod.xd_purification:
						case LearnMethod.colosseum_purification:
							if (!purify.Contains(move.MoveId)) purify.Add(move.MoveId);
							break;
						case LearnMethod.shadow:
						case LearnMethod.xd_shadow:
							if (!shadow.Contains(move.MoveId)) shadow.Add(move.MoveId);
							break;
						case LearnMethod.form_change:
							if (!form.Contains(move.MoveId)) form.Add(move.MoveId);
							break;
						default:
							break;
					}
				}
			#endregion
			/*return PokemonMoveTree(
					levelup: level,
					egg: egg.ToArray(),
					tutor: tutor.ToArray(),
					machine: machine.ToArray(),
					shadow: shadow.ToArray(),
					purification: purify.ToArray(),
					form_change: form.ToArray()
				);*/
			this.LevelUp = level;
			this.Egg = egg.ToArray();
			this.Tutor = tutor.ToArray();
			this.Machine = machine.ToArray();
			this.Shadow = shadow.ToArray();
			this.Purification = purify.ToArray();
			this.FormChange = form.ToArray();
		}
	}
}
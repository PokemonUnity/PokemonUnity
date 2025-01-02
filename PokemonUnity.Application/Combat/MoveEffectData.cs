using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.Combat
{
	/// <summary>
	/// </summary>
	public class MoveEffectData : IDictionary<Attack.Data.Effects, PokemonEssentials.Interface.PokeBattle.IBattleMove>
	{
		#region Variables
		#endregion

		public MoveEffectData() { }

		#region IDictionary Interface
		public IBattleMove this[Attack.Data.Effects key] { get; set; }

		public ICollection<Attack.Data.Effects> Keys { get; }
		public ICollection<IBattleMove> Values { get; }
		public int Count { get; }
		public bool IsReadOnly { get; }

		public void Add(Attack.Data.Effects key, IBattleMove value)
		{
			throw new NotImplementedException();
		}

		public void Add(KeyValuePair<Attack.Data.Effects, IBattleMove> item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(KeyValuePair<Attack.Data.Effects, IBattleMove> item)
		{
			throw new NotImplementedException();
		}

		public bool ContainsKey(Attack.Data.Effects key)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(KeyValuePair<Attack.Data.Effects, IBattleMove>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<KeyValuePair<Attack.Data.Effects, IBattleMove>> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public bool Remove(Attack.Data.Effects key)
		{
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<Attack.Data.Effects, IBattleMove> item)
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue(Attack.Data.Effects key, out IBattleMove value)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
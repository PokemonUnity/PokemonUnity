using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity.Monster;
using PokemonUnity;

namespace PokemonUnity.Character
{
	public partial class Party : IList<Pokemon>, ICollection<Pokemon>
	{
		public virtual List<Pokemon> Pokemons { get; protected set; }
		public virtual bool Full { get { return Count == Limit; } }
		public virtual int Count { get { return Pokemons.Count(x => x.IsNotNullOrNone()); } }
		public virtual int Limit { get { return Pokemons.Capacity; } }
		public virtual int Length { get { return Count; } }
		public virtual bool IsReadOnly { get; set; }

		public Pokemon this[int i]
		{
			get
			{
				//If pokemon is null return empty
				if ((IsReadOnly && Pokemons[i].IsNotNullOrNone()))
					return new Pokemon(PokemonUnity.Pokemons.NONE);
				else
					return Pokemons[i];
			}
			set
			{
				//If pokemon is null dont add to list, count only not nulls
				if (!Pokemons.Contains(value) 
						&& (IsReadOnly && value.IsNotNullOrNone()))
					Pokemons[i] = value;
			}
		}

		public Party(int? maxPokemon = null)
		{
			if (!maxPokemon.HasValue) maxPokemon = Game.GameData.Features.LimitPokemonPartySize;
			Pokemons = new List<Pokemon>(
				capacity: maxPokemon.Value > Core.MAXPARTYSIZE 
					? Core.MAXPARTYSIZE 
					: maxPokemon.Value);
			//for (int i = 0; i < maxPokemon; i++)
			//{
			//	Pokemons[i] = null;
			//}
		}

		public virtual void Add(Pokemon item)
		{
			//if there is space add to party
			if(!Full)
			{
				if (!Pokemons.Contains(item) 
						&& item.IsNotNullOrNone())
					Pokemons.Add(item);
			}				
			//otherwise send to player's pc
			//ToDo: add pc specific to this player's party
		}

		public virtual void Clear()
		{
			//Player party cannot be empty?...
			Pokemons.Clear();
		}

		public virtual bool Contains(Pokemon item)
		{
			//should return false for both null and none
			return Pokemons.Contains(item);
		}

		public virtual void CopyTo(Pokemon[] array, int arrayIndex)
		{
			Pokemons.CopyTo(array, arrayIndex);
		}

		public virtual bool Remove(Pokemon item)
		{
			//if object is removed, it will remove nulls and empty space
			return Pokemons.Remove(item);
		}

		public virtual IEnumerator<Pokemon> GetEnumerator()
		{
			return Pokemons.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual int IndexOf(Pokemon item)
		{
			return ((IList<Pokemon>)Pokemons).IndexOf(item);
		}

		public virtual void Insert(int index, Pokemon item)
		{
			((IList<Pokemon>)Pokemons).Insert(index, item);
		}

		public virtual void RemoveAt(int index)
		{
			((IList<Pokemon>)Pokemons).RemoveAt(index);
		}

		public static implicit operator Pokemon[](Party party)
		{
			return party.Pokemons.ToArray();
		}

		public static implicit operator List<Pokemon>(Party party)
		{
			return party.Pokemons;
		}

		public static implicit operator Party(Pokemon[] party)
		{
			Party p = new Party();
			foreach (Pokemon pkmn in party)
				p.Add(pkmn);
			return p;
		}

		public static implicit operator Party(List<Pokemon> party)
		{
			Party p = new Party();
			foreach (Pokemon pkmn in party)
				p.Add(pkmn);
			return p;
		}
	}
}
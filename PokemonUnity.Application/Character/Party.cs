using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity.Monster;
using PokemonUnity;

namespace PokemonUnity.Character
{
	public partial class Party : IList<PokemonEssentials.Interface.PokeBattle.IPokemon>, ICollection<PokemonEssentials.Interface.PokeBattle.IPokemon>
	{
		public virtual List<PokemonEssentials.Interface.PokeBattle.IPokemon> Pokemons { get; protected set; }
		public virtual bool Full { get { return Count == Limit; } }
		public virtual int Count { get { return Pokemons.Count(x => x.IsNotNullOrNone()); } }
		public virtual int Limit { get { return Pokemons.Capacity; } }
		public virtual int Length { get { return Count; } }
		public virtual bool IsReadOnly { get; set; }

		public PokemonEssentials.Interface.PokeBattle.IPokemon this[int i]
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
			if (!maxPokemon.HasValue) maxPokemon = (Game.GameData as Game).Features.LimitPokemonPartySize;
			Pokemons = new List<PokemonEssentials.Interface.PokeBattle.IPokemon>(
				capacity: maxPokemon.Value > Core.MAXPARTYSIZE 
					? Core.MAXPARTYSIZE 
					: maxPokemon.Value);
			//for (int i = 0; i < maxPokemon; i++)
			//{
			//	Pokemons[i] = null;
			//}
		}

		public virtual void Add(PokemonEssentials.Interface.PokeBattle.IPokemon item)
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

		public virtual bool Contains(PokemonEssentials.Interface.PokeBattle.IPokemon item)
		{
			//should return false for both null and none
			return Pokemons.Contains(item);
		}

		public virtual void CopyTo(PokemonEssentials.Interface.PokeBattle.IPokemon[] array, int arrayIndex)
		{
			Pokemons.CopyTo(array, arrayIndex);
		}

		public virtual bool Remove(PokemonEssentials.Interface.PokeBattle.IPokemon item)
		{
			//if object is removed, it will remove nulls and empty space
			return Pokemons.Remove(item);
		}

		public virtual IEnumerator<PokemonEssentials.Interface.PokeBattle.IPokemon> GetEnumerator()
		{
			return Pokemons.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual int IndexOf(PokemonEssentials.Interface.PokeBattle.IPokemon item)
		{
			return ((IList<PokemonEssentials.Interface.PokeBattle.IPokemon>)Pokemons).IndexOf(item);
		}

		public virtual void Insert(int index, PokemonEssentials.Interface.PokeBattle.IPokemon item)
		{
			((IList<PokemonEssentials.Interface.PokeBattle.IPokemon>)Pokemons).Insert(index, item);
		}

		public virtual void RemoveAt(int index)
		{
			((IList<PokemonEssentials.Interface.PokeBattle.IPokemon>)Pokemons).RemoveAt(index);
		}

		public static implicit operator PokemonEssentials.Interface.PokeBattle.IPokemon[](Party party)
		{
			return party.Pokemons.ToArray();
		}

		public static implicit operator List<PokemonEssentials.Interface.PokeBattle.IPokemon>(Party party)
		{
			return party.Pokemons;
		}

		public static implicit operator Party(PokemonEssentials.Interface.PokeBattle.IPokemon[] party)
		{
			Party p = new Party();
			foreach (PokemonEssentials.Interface.PokeBattle.IPokemon pkmn in party)
				p.Add(pkmn);
			return p;
		}

		public static implicit operator Party(List<PokemonEssentials.Interface.PokeBattle.IPokemon> party)
		{
			Party p = new Party();
			foreach (PokemonEssentials.Interface.PokeBattle.IPokemon pkmn in party)
				p.Add(pkmn);
			return p;
		}
	}
}
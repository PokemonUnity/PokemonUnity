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
	public struct EvolutionTrigger
	{
		public Pokemons Species { get; private set; }
		public EvoTrigger Evo { get; private set; }
		public Items Trigger { get; private set; }
		public int? MinLevel { get; private set; }
		public bool? Gender { get; private set; }
		public int? Location { get; private set; }
		public Items Held { get; private set; }
		/// <summary>
		/// Time of Day
		/// </summary>
		public int? Time { get; private set; }
		public Moves KnownMove { get; private set; }
		public Types KnownType { get; private set; }
		public int? Happiness { get; private set; }
		public int? Beauty { get; private set; }
		public int? Affection { get; private set; }
		public int? PhysicalStat { get; private set; }
		public Pokemons PartySpecies { get; private set; }
		public Types PartyType { get; private set; }
		public Pokemons TradeSpecies { get; private set; }
		public bool OverworldRain { get; private set; }
		public bool TurnUpsideDown { get; private set; }

		public EvolutionTrigger(Pokemons species = Pokemons.NONE, EvoTrigger evo = EvoTrigger.level_up, Items trigger = Items.NONE, int? minLevel = null, bool? gender = null, int? location = null, 
			Items held = Items.NONE, int? time = null, Moves knownMove = Moves.NONE, Types knownType = Types.NONE, int? happiness = null, 
			int? beauty = null, int? affection = null, int? physicalStat = null, Pokemons partySpecies = Pokemons.NONE, Types partyType = Types.NONE, 
			Pokemons tradeSpecies = Pokemons.NONE, bool overworldRain = false, bool turnUpsideDown = true)
		{
			Species = species;
			Evo = evo;
			Trigger = trigger;
			MinLevel = minLevel;
			Gender = gender;
			Location = location;
			Held = held;
			Time = time;
			KnownMove = knownMove;
			KnownType = knownType;
			Happiness = happiness;
			Beauty = beauty;
			Affection = affection;
			PhysicalStat = physicalStat;
			PartySpecies = partySpecies;
			PartyType = partyType;
			TradeSpecies = tradeSpecies;
			OverworldRain = overworldRain;
			TurnUpsideDown = turnUpsideDown;
		}
	}
	public enum EvoTrigger
	{
		level_up=1,
		trade	=2,
		use_item=3,
		shed	=4
	}
	/// <summary>
	/// The evolution paths this species can take. 
	/// For each possible evolution of this species, 
	/// there are three parts
	/// </summary>
	public struct PokemonEvolution //: IPokemonEvolution
	{
		/// <summary>
		/// The PokemonId of the evolved species.
		/// The PokemonId of the species this pokemon evolves into.
		/// </summary>
		public Pokemons Species { get; private set; }
		/// <summary>
		/// The evolution method.
		/// </summary>
		public EvolutionMethod EvolveMethod { get; private set; }
		/// <summary>
		/// The value-parameter to <see cref="EvolveMethod"/> as mentioned KEY.
		/// </summary>
		public object EvolveValue { get; private set; }
		//public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, Type ValueType, object ObjectValue) 
		//{
		//    PokemonEvolution<ValueType>(EvolveTo, EvolveHow, ObjectValue);
		//}
		public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, object Value = null) 
		{
			this.Species = EvolveTo;
			this.EvolveMethod = EvolveHow;
			this.EvolveValue = Value;
			#region Switch
			switch (EvolveHow)
			{
				case EvolutionMethod.Level:
				case EvolutionMethod.LevelFemale:
				case EvolutionMethod.LevelMale:
				case EvolutionMethod.Ninjask:
				case EvolutionMethod.Beauty:
				case EvolutionMethod.Happiness:
				case EvolutionMethod.HappinessDay:
				case EvolutionMethod.HappinessNight:
				case EvolutionMethod.Hatred:
					//if (!Value.GetType().Equals(typeof(int)))
					if (!(Value is int))
					{
						this.EvolveValue = (int)0;
					}
					break;
				case EvolutionMethod.Item:
				case EvolutionMethod.ItemFemale:
				case EvolutionMethod.ItemMale:
				case EvolutionMethod.TradeItem:
				case EvolutionMethod.HoldItem:
				case EvolutionMethod.HoldItemDay:
				case EvolutionMethod.HoldItemNight:
					//if (!Value.GetType().Equals(typeof(Items)))
					if (!(Value is Items))
					{
						this.EvolveValue = (Items)Items.NONE;
					}
					break;
				case EvolutionMethod.TradeSpecies:
				case EvolutionMethod.Party:
				//case EvolutionMethod.Shedinja:
					//if (!Value.GetType().Equals(typeof(Pokemons)))
					if (!(Value is Pokemons))
					{
						this.EvolveValue = (Pokemons)Pokemons.NONE;
					}
					break;
				case EvolutionMethod.Move:
					//if (!Value.GetType().Equals(typeof(Moves)))
					if (!(Value is Moves))
					{
						this.EvolveValue = (Moves)Moves.NONE;
					}
					break;
				case EvolutionMethod.Type:
					//if (!Value.GetType().Equals(typeof(Types)))
					if (!(Value is Types))
					{
						this.EvolveValue = (Types)Types.NONE;
					}
					break;
				case EvolutionMethod.Shedinja:
				case EvolutionMethod.Time:
				case EvolutionMethod.Season:
				case EvolutionMethod.Location:
				case EvolutionMethod.Weather:
				default:
					//if there's no problem, just ignore it, and move on...
					this.EvolveValue = Value;
					break;
			}
			#endregion
		}

		/*// <summary>
		/// This is used to ensure that the constructor class was created properly,
		/// by returning a new instance if the class was used incorrectly.
		/// </summary>
		/// <returns>returns a valid <see cref="PokemonEvolution{T}"/>.this</returns>
		public abstract Data.IPokemonEvolution<object> Validate()
		{
			//dynamic value = null;
			#region Switch
			//This should trigger after the class has been initialized, right?
			switch (EvolveHow)
			{
				case EvolutionMethod.Level:
				case EvolutionMethod.LevelFemale:
				case EvolutionMethod.LevelMale:
				case EvolutionMethod.Ninjask:
				case EvolutionMethod.Beauty:
				case EvolutionMethod.Happiness:
				case EvolutionMethod.HappinessDay:
				case EvolutionMethod.HappinessNight:
				case EvolutionMethod.Hatred:
					//if(typeof(T) || this.GetType() == typeof(string))
					if (!typeof(T).Equals(typeof(int)))
					{
						//throw new Exception("Type not acceptable for Method-Value pair.");
						//Instead of throwing an exception, i'll correct the problem instead?
						//int.TryParse(Value.ToString, out EvolveValue);
						//Convert.ChangeType(EvolveValue, typeof(int));
						//Convert.ChangeType(Value, typeof(int));
						//Value = 0; //default(T);
						//value = 0;
						//this.EvolveValue = 0;
						return new PokemonEvolution<int>(this.Species, this.EvolveMethod, 0);
					}
					break;
				case EvolutionMethod.Item:
				case EvolutionMethod.ItemFemale:
				case EvolutionMethod.ItemMale:
				case EvolutionMethod.TradeItem:
				case EvolutionMethod.HoldItem:
				case EvolutionMethod.HoldItemDay:
				case EvolutionMethod.HoldItemNight:
					if (!typeof(T).Equals(typeof(Items)))
					{
						//Convert.ChangeType(EvolveValue, typeof(Items));
						//Convert.ChangeType(Value, typeof(Items));
						//Value = Items.NONE; //default(T);
						//value = Items.NONE; //default(T);
						//this.EvolveValue = Items.NONE;
						return new PokemonEvolution<Items>(this.Species, this.EvolveMethod, Items.NONE);
					}
					break;
				case EvolutionMethod.TradeSpecies:
				case EvolutionMethod.Party:
				//case EvolutionMethod.Shedinja:
					if (!typeof(T).Equals(typeof(Pokemons)))
					{
						//Convert.ChangeType(EvolveValue, typeof(Pokemons));
						//Convert.ChangeType(Value, typeof(Pokemons));
						//Value = Pokemons.NONE; //default(T);
						//value = Pokemons.NONE; //default(T);
						//this.EvolveValue = Pokemons.NONE;
						return new PokemonEvolution<Pokemons>(this.Species, this.EvolveMethod, Pokemons.NONE);
					}
					break;
				case EvolutionMethod.Move:
					if (!typeof(T).Equals(typeof(Moves)))
					{
						//Convert.ChangeType(EvolveValue, typeof(Moves));
						//Convert.ChangeType(Value, typeof(Moves));
						//Value = Moves.NONE; //default(T);
						//value = Moves.NONE; //default(T);
						//this.EvolveValue = Moves.NONE;
						return new PokemonEvolution<int>(this.Species, this.EvolveMethod, Moves.NONE);
					}
					break;
				case EvolutionMethod.Type:
					if (!typeof(T).Equals(typeof(Types)))
					{
						//Convert.ChangeType(EvolveValue, typeof(Types));
						//Convert.ChangeType(Value, typeof(Types));
						//Value = Types.NONE; //default(T);
						//value = Types.NONE; //default(T);
						//this.EvolveValue = Types.NONE;
						return new PokemonEvolution<int>(this.Species, this.EvolveMethod, Types.NONE);
					}
					break;
				case EvolutionMethod.Shedinja:
				case EvolutionMethod.Time:
				case EvolutionMethod.Season:
				case EvolutionMethod.Location:
				case EvolutionMethod.Weather:
				default:
					//if there's no problem, just ignore it, and move on...
					//value = Value;
					//this.EvolveValue = Value;
					break;
			}
			#endregion
			//this.EvolveValue = value; //Value;
			//this.EvolveValue = Convert.ChangeType(value, typeof(T));
			return this;
		}*/
	}
	public interface IPokemonEvolution<out T> : IPokemonEvolution
	{
		//Pokemons Species { get; }
		//EvolutionMethod EvolveMethod { get; }
		T EvolveValue { get; }
	}
	public interface IPokemonEvolution
	{
		Pokemons Species { get; }

		EvolutionMethod EvolveMethod { get; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster
{
    public partial class Pokemon
    {
		public partial class PokemonData
        {
            /// <summary>
            /// The evolution paths this species can take. 
            /// For each possible evolution of this species, 
            /// there are three parts
            /// </summary>
            public class PokemonEvolution : IPokemonEvolution //<T> where T : new()
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
				//public object EvolutionMethodValue;
				//public PokemonEvolution<T> EvolutionMethodValue;
				//public class T { }
				//public PokemonEvolution(){}
				public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow)
                {
                    this.Species = EvolveTo;
                    this.EvolveMethod = EvolveHow;
					this.EvolveValue = null;
                }
				//public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, Type ValueType, object ObjectValue) 
                //{
                //    PokemonEvolution<ValueType>(EvolveTo, EvolveHow, ObjectValue);
                //}
				public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, object Value) : this(EvolveTo: EvolveTo, EvolveHow: EvolveHow)
				{
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
							if (!Value.GetType().Equals(typeof(int)))
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
							if (!Value.GetType().Equals(typeof(Items)))
							{
								this.EvolveValue = (Items)Items.NONE;
							}
							break;
						case EvolutionMethod.TradeSpecies:
						case EvolutionMethod.Party:
							//case EvolutionMethod.Shedinja:
							if (!Value.GetType().Equals(typeof(Pokemons)))
							{
								this.EvolveValue = (Pokemons)Pokemons.NONE;
							}
							break;
						case EvolutionMethod.Move:
							if (!Value.GetType().Equals(typeof(Moves)))
							{
								this.EvolveValue = (Moves)Moves.NONE;
							}
							break;
						case EvolutionMethod.Type:
							if (!Value.GetType().Equals(typeof(Types)))
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
				//public PokemonEvolution<T>(Pokemons EvolveTo, EvolutionMethod EvolveHow, T Value) where T : object
				//	: this(EvolveTo: EvolveTo, EvolveHow: EvolveHow)
				//{
				//}
				public IPokemonEvolution GetEvolution()
				{
					return this;
				}
				public virtual bool isGenericT()
                {
                    return false;
                }
            }
            public class PokemonEvolution<T> : PokemonEvolution
            {
                /*// <summary>
                /// The PokemonId of the evolved species.
                /// </summary>
                public PokemonData.Pokemon EvolvesTo;
                /// <summary>
                /// The evolution method.
                /// </summary>
                public int EvolveMethod;
                //public object EvolveValue;
                //public T EvolveValue<T>() { return GetValue(); };*/
                /// <summary>
                /// The value-parameter to <see cref="EvolveMethod"/> as mentioned KEY.
                /// </summary>
                new public T EvolveValue { get; private set; }

                public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow, Value: Value)
                {
					//if (!typeof(T).Equals(base.EvolveValue.GetType()))
					//{
					//	Convert.ChangeType(base.EvolveValue, typeof(T));
					//	this.EvolveValue = (T)base.EvolveValue;
					//}
					//else
						this.EvolveValue = (T)base.EvolveValue;
				}
                /*public PokemonEvolution(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow) {
                    #region Switch
                    //This should trigger after the class has been initialized, right?
                    switch (this.EvolveMethod)
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
                            if (this.EvolveValue.GetType() != typeof(int))
                                //throw new Exception("Type not acceptable for Method-Value pair.");
                                //Instead of throwing an exception, i'll correct the problem instead?
                                Convert.ChangeType(this.EvolveValue, typeof(int));
                                this.EvolveValue = default(T);
                            break;
                        case EvolutionMethod.Item:
                        case EvolutionMethod.ItemFemale:
                        case EvolutionMethod.ItemMale:
                        case EvolutionMethod.TradeItem:
                        case EvolutionMethod.HoldItem:
                        case EvolutionMethod.HoldItemDay:
                        case EvolutionMethod.HoldItemNight:
                            if (this.EvolveValue.GetType() != typeof(eItems))
                                Convert.ChangeType(this.EvolveValue, typeof(eItems));
                                this.EvolveValue = default(T);
                            break;
                        case EvolutionMethod.TradeSpecies:
                        case EvolutionMethod.Party:
                        case EvolutionMethod.Shedinja:
                            if (this.EvolveValue.GetType() != typeof(PokemonData.Pokemon))
                                Convert.ChangeType(this.EvolveValue, typeof(PokemonData.Pokemon));
                                this.EvolveValue = default(T);
                            break;
                        case EvolutionMethod.Move:
                            if (this.EvolveValue.GetType() != typeof(Move.MoveData.Move))
                                Convert.ChangeType(this.EvolveValue, typeof(Move.MoveData.Move));
                                this.EvolveValue = default(T);
                            break;
                        case EvolutionMethod.Type:
                            if (this.EvolveValue.GetType() != typeof(PokemonData.Type))
                                Convert.ChangeType(this.EvolveValue, typeof(PokemonData.Type));
                                this.EvolveValue = default(T);
                            break;
                        case EvolutionMethod.Time:
                        case EvolutionMethod.Season:
                        case EvolutionMethod.Location:
                        case EvolutionMethod.Weather:
                        default:
                            //if there's no problem, just ignore it, and move on...
                            break;
                    }
                    #endregion
                }
                //private static Con<T2>(object ObjIn)
                /*public int IntValue;
                public string StringValue;
                private int GetValue(int p)
                {
                    return this.IntValue;
                }
                private string GetValue(string p)
                {
                    return this.StringValue;
                }*/

                public override bool isGenericT()
                {
                    return true;// base.isGenericT();
                }

				/* ToDo: Why is build failing to compile this method?
				/// <summary>
				/// This is used to ensure that the constructor class was created properly,
				/// by returning a new instance if the class was used incorrectly.
				/// </summary>
				/// <returns>returns a valid <see cref="PokemonEvolution{T}"/>.this</returns>
				public IPokemonEvolution Validate()
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

				//void evolve(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) { }
			}
        }
    }
	public interface IPokemonEvolution
	{
		Pokemons Species { get; }

		EvolutionMethod EvolveMethod { get; }
	}
}
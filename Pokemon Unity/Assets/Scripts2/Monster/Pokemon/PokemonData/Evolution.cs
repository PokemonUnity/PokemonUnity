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
                //public object EvolutionMethodValue;
                //public PokemonEvolution<T> EvolutionMethodValue;
                //public class T { }
                //public PokemonEvolution(){}
                public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow)
                {
                    this.Species = EvolveTo;
                    this.EvolveMethod = EvolveHow;
                }
                /*public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, Type ValueType, object ObjectValue) 
                {
                    PokemonEvolution<ValueType>(EvolveTo, EvolveHow, ObjectValue);
                }*/
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
                public T EvolveValue { get; private set; }

                //public PokemonEvolution<T> (T objects){}
                //void evolve(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) { }

                public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow)
                {
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
                            if (T.GetType() != typeof(int))
							{
                                //throw new Exception("Type not acceptable for Method-Value pair.");
                                //Instead of throwing an exception, i'll correct the problem instead?
								//int.TryParse(Value.ToString, out EvolveValue);
								Convert.ChangeType(T, typeof(int));
								Value = 0; //default(T);
							}
                            break;
                        case EvolutionMethod.Item:
                        case EvolutionMethod.ItemFemale:
                        case EvolutionMethod.ItemMale:
                        case EvolutionMethod.TradeItem:
                        case EvolutionMethod.HoldItem:
                        case EvolutionMethod.HoldItemDay:
                        case EvolutionMethod.HoldItemNight:
                            if (T.GetType() != typeof(Items))
							{
								Convert.ChangeType(T, typeof(Items));
								Value = Items.NONE; //default(T);
							}
                            break;
                        case EvolutionMethod.TradeSpecies:
                        case EvolutionMethod.Party:
                        //case EvolutionMethod.Shedinja:
                            if (T.GetType() != typeof(Pokemons))
							{
								Convert.ChangeType(T, typeof(Pokemons));
								Value = Pokemons.NONE; //default(T);
							}
                            break;
                        case EvolutionMethod.Move:
                            if (T.GetType() != typeof(Moves))
							{
								Convert.ChangeType(T, typeof(Moves));
								Value = Moves.NONE; //default(T);
							}
                            break;
                        case EvolutionMethod.Type:
                            if (T.GetType() != typeof(Types))
							{
								Convert.ChangeType(T, typeof(Types));
								Value = Types.NONE; //default(T);
							}
                            break;
                        case EvolutionMethod.Shedinja:
                        case EvolutionMethod.Time:
                        case EvolutionMethod.Season:
                        case EvolutionMethod.Location:
                        case EvolutionMethod.Weather:
                        default:
                            //if there's no problem, just ignore it, and move on...
                            break;
                    }
                    #endregion
                    this.EvolveValue = Value;
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
            }
        }
    }
}
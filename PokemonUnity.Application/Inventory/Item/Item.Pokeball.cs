using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.EventArg;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Item;

namespace PokemonUnity
{
	namespace EventArg
	{
		#region PokeBall Handlers EventArgs
		public class OnCatchEventArgs : EventArgs, IOnCatchEventArgs
		{
			public static readonly int EventId = typeof(OnCatchEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Ball { get; set; }
			public IBattle Battle { get; set; }
			public IPokemon Pokemon { get; set; }
		}
		public class OnFailCatchEventArgs : EventArgs, IOnFailCatchEventArgs
		{
			public static readonly int EventId = typeof(OnCatchEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Ball { get; set; }
			public IBattle Battle { get; set; }
			public IBattler Battler { get; set; }
		}
		#endregion
	}

	namespace Combat { 
		public static class BallHandlers //: PokemonEssentials.Interface.BallHandlers 
		{
			//IsUnconditional = new ItemHandlerHash;
			//ModifyCatchRate = new ItemHandlerHash;
			//OnCatch         = new ItemHandlerHash;
			//OnFailCatch     = new ItemHandlerHash;

			public static Items[] BallTypes= new Items[] {
				Items.POKE_BALL,         //0=>
				Items.GREAT_BALL,        //1=>
				Items.SAFARI_BALL,       //2=>
				Items.ULTRA_BALL,        //3=>
				Items.MASTER_BALL,       //4=>
				Items.NET_BALL,          //5=>
				Items.DIVE_BALL,         //6=>
				Items.NEST_BALL,         //7=>
				Items.REPEAT_BALL,       //8=>
				Items.TIMER_BALL,        //9=>
				Items.LUXURY_BALL,       //10=>
				Items.PREMIER_BALL,      //11=>
				Items.DUSK_BALL,         //12=>
				Items.HEAL_BALL,         //13=>
				Items.QUICK_BALL,        //14=>
				Items.CHERISH_BALL,      //15=>
				Items.FAST_BALL,         //16=>
				Items.LEVEL_BALL,        //17=>
				Items.LURE_BALL,         //18=>
				Items.HEAVY_BALL,        //19=>
				Items.LOVE_BALL,         //20=>
				Items.FRIEND_BALL,       //21=>
				Items.MOON_BALL,         //22=>
				Items.SPORT_BALL         //23=>
			};

			public static bool IsUnconditional(Items ball,IBattle battle,IBattler battler) {
				//if (!IsUnconditional[ball]) return false;
				//return IsUnconditional.trigger(ball,battle,battler);
				if (ball == Items.MASTER_BALL) {
					return true;
				}
				else return false;
			}

			public static int ModifyCatchRate(Items ball,int catchRate,IBattle battle,IBattler battler) {
				//if (!ModifyCatchRate[ball]) return catchRate;
				//return ModifyCatchRate.trigger(ball,catchRate,battle,battler);
				#region Pokeball ModifyCatchRate
				if (ball == Items.GREAT_BALL) {
					return (int)Math.Floor(catchRate*3/2f);
				}
				else if (ball == Items.ULTRA_BALL) {
					return (int)Math.Floor(catchRate*2f);
				}
				else if (ball == Items.SAFARI_BALL) {
					return (int)Math.Floor(catchRate*3/2f);
				}
				else if (ball == Items.NET_BALL) {
					if (battler.pbHasType(Types.BUG) || battler.pbHasType(Types.WATER)) catchRate*=3;
					return catchRate;
				}
				else if (ball == Items.DIVE_BALL) {
					if (battle.environment==Environments.Underwater) catchRate=(int)Math.Floor(catchRate*7/2f);
					return catchRate;
				}
				else if (ball == Items.NEST_BALL) {
					if (battler.Level<=40) {
						catchRate*=(int)Math.Max((41-battler.Level)/10,1);
					}
					return catchRate;
				}
				else if (ball == Items.REPEAT_BALL) {
					if (battle.pbPlayer().owned[battler.Species]) catchRate*=3;
					//if (battle.pbPlayer().Pokedex[(int)battler.Species,1] == 1) catchRate*=3;
					return catchRate;
				}
				else if (ball == Items.TIMER_BALL) {
					int multiplier=(int)Math.Min(1+(0.3*battle.turncount),4);
					catchRate*=multiplier;
					return catchRate;
				}
				else if (ball == Items.DUSK_BALL) {
					//if (PBDayNight.isNight()) catchRate*=7/2;
					if (Game.GetTime == Overworld.DayTime.Night) catchRate*=7/2;
					return catchRate;
				}
				else if (ball == Items.QUICK_BALL) {
					if (battle.turncount<=1) catchRate*=5;
					return catchRate;
				}
				else if (ball == Items.FAST_BALL) {
					//dexdata=pbOpenDexData;
					//pbDexDataOffset(dexdata,battler.Species,13);
					//basespeed=dexdata.fgetb;
					//dexdata.close;
					int basespeed=Kernal.PokemonData[battler.Species].BaseStatsSPE;
					if (basespeed>=100) catchRate*=4;
					return (int)Math.Min(catchRate,255);
				}
				else if (ball == Items.LEVEL_BALL) {
					int pbattler=battle.battlers[0].Level;
					if (battle.battlers[2].IsNotNullOrNone() &&
						battle.battlers[2].Level>pbattler) pbattler=battle.battlers[2].Level;
					if (pbattler>=battler.Level*4) {
						catchRate*=8;
					} else if (pbattler>=battler.Level*2) {
						catchRate*=4;
					} else if (pbattler>battler.Level) {
						catchRate*=2;
					}
					return (int)Math.Min(catchRate,255);
				}
				else if (ball == Items.LURE_BALL) {
					if (Game.GameData.PokemonTemp is PokemonEssentials.Interface.Field.ITempMetadataField f && (
						f.encounterType==EncounterTypes.OldRod ||
						f.encounterType==EncounterTypes.GoodRod ||
						f.encounterType==EncounterTypes.SuperRod)) catchRate*=3;
					//if (Game.GameData.PokemonTemp.encounterType==Overworld.Method.OLD_ROD || //EncounterTypes.OldRod
					//	Game.GameData.PokemonTemp.encounterType==Overworld.Method.GOOD_ROD || //EncounterTypes.GoodRod
					//	Game.GameData.PokemonTemp.encounterType==Overworld.Method.SUPER_ROD) catchRate*=3; //EncounterTypes.SuperRod
					return (int)Math.Min(catchRate,255);
				}
				else if (ball == Items.HEAVY_BALL) {
					float weight=battler.Weight();
					if (weight>=4096) {
						catchRate+=40;
					} else if (weight>=3072) {
						catchRate+=30;
					} else if (weight>=2048) {
						catchRate+=20;
					}
					else {
						catchRate-=20;
					}
					catchRate=(int)Math.Max(catchRate,1);
					return (int)Math.Min(catchRate,255);
				}
				else if (ball == Items.LOVE_BALL) {
					IBattler pbattler=battle.battlers[0];
					IBattler pbattler2=null;
					if (battle.battlers[2].IsNotNullOrNone()) pbattler2=battle.battlers[2];
					if (pbattler.Species==battler.Species &&
						((battler.Gender==false && pbattler.Gender==true) ||
						(battler.Gender==true && pbattler.Gender==false))) {
						catchRate*=8;
					} else if (pbattler2.IsNotNullOrNone() && pbattler2.Species==battler.Species &&
						((battler.Gender==false && pbattler2.Gender==true) ||
						(battler.Gender==true && pbattler2.Gender==false))) {
						catchRate*=8;
					}
					return (int)Math.Min(catchRate,255);
				}
				else if (ball == Items.MOON_BALL) {
					if (battler.Species == Pokemons.NIDORAN_F ||
						battler.Species == Pokemons.NIDORINA ||
						battler.Species == Pokemons.NIDOQUEEN ||
						battler.Species == Pokemons.NIDORAN_M ||
						battler.Species == Pokemons.NIDORINO ||
						battler.Species == Pokemons.NIDOKING ||
						battler.Species == Pokemons.CLEFFA ||
						battler.Species == Pokemons.CLEFAIRY ||
						battler.Species == Pokemons.CLEFABLE ||
						battler.Species == Pokemons.IGGLYBUFF ||
						battler.Species == Pokemons.JIGGLYPUFF ||
						battler.Species == Pokemons.WIGGLYTUFF ||
						battler.Species == Pokemons.SKITTY ||
						battler.Species == Pokemons.DELCATTY ||
						battler.Species == Pokemons.MUNNA ||
						battler.Species == Pokemons.MUSHARNA) {
						catchRate*=4;
					}
					return (int)Math.Min(catchRate,255);
				}
				else if (ball == Items.SPORT_BALL) {
					return (int)Math.Floor(catchRate*3/2f);
				}
				#endregion
				else return catchRate;
			}

			public static void OnCatch(Items ball,IBattle battle,IPokemon pokemon) {
				//if (!OnCatch[ball]) return;
				//OnCatch.trigger(ball,battle,pokemon);
				//if (OnCatch != null) OnCatch.Invoke(ball,battle,pokemon);
				if (ball == Items.HEAL_BALL) {
					pokemon.Heal();
				}
				else if (ball == Items.FRIEND_BALL) {
					//pokemon.Happiness=200;
					//pokemon.ChangeHappiness(HappinessMethods.FRIENDBALL);
				}
				else return;
			}

			private static void OnCatch(object sender, IOnCatchEventArgs e) {
				//if (!OnCatch[ball]) return;
				//OnCatch.trigger(ball,battle,pokemon);
				if (e.Ball == Items.HEAL_BALL) {
					e.Pokemon.Heal();
				}
				else if (e.Ball == Items.FRIEND_BALL) {
					//e.Pokemon.Happiness=200;
					//e.Pokemon.ChangeHappiness(HappinessMethods.FRIENDBALL);
				}
				else return;
			}

			public static void OnFailCatch(Items ball,IBattle battle,IBattler battler) {
				//if (!OnFailCatch[ball]) return;
				//OnFailCatch.trigger(ball,battle,battler);
				//if (OnFailCatch != null) OnFailCatch.Invoke(ball,battle,battler);
				return;
			}

			public static void OnFailCatch(object sender, IOnFailCatchEventArgs e) {
				//if (!OnFailCatch[ball]) return;
				//OnFailCatch.trigger(ball,battle,battler);
				return;
			}

			public static Items pbBallTypeToBall(int balltype) {
				Items ret = Items.POKE_BALL;
				//if (BallTypes[balltype]) {
				if (balltype > 0 && balltype < BallTypes.Length) {
					ret=BallTypes[balltype];
					//if (ret!=0) return ret;
				}
				//if (BallTypes[0] != null) {
				//  ret=BallTypes[0];
				//  if (ret!=0) return ret;
				//}
				return ret; //Items.POKEBALL;
			}

			public static int pbGetBallType(Items ball) {
				//ball=getID(PBItems,ball);
				//foreach (var key in BallTypes) {
				for (int key = 0; key < BallTypes.Length; key++) {
					if (ball==BallTypes[key]) return key;
				}
				return 0;
			}
		}
	}
}
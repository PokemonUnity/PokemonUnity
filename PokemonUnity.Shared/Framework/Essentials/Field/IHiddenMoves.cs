using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface
{

	namespace EventArg
	{
		#region Hidden Moves EventArgs
		/// <summary>
		/// Parameters:
		/// e[0] = Move being created
		/// e[1] = Pokemon using the move
		/// </summary>
		public class HiddenMoveEventArgs : EventArgs
		{
			public readonly int EventId = typeof(HiddenMoveEventArgs).GetHashCode();

			public int Id { get; }
			/// <summary>
			/// Move being created
			/// </summary>
			public Moves Move { get; set; }
			/// <summary>
			/// Pokemon using the move
			/// </summary>
			public IPokemon Pokemon { get; set; }
		}
		#endregion
	}

	namespace Field
	{
		#region Interpolators
		// ===============================================================================
		// Interpolators
		// ===============================================================================
		public interface IRectInterpolator
		{
			IRectInterpolator initialize(IRect oldrect, IRect newrect, int frames);

			void restart(IRect oldrect, IRect newrect, int frames);

			void set(IRect rect);

			bool done();

			void update();
		}

		public interface IPointInterpolator
		{
			IPointInterpolator initialize(float oldx, float oldy, float newx, float newy, int frames);

			void restart(float oldx, float oldy, float newx, float newy, int frames);

			float x { get; }
			float y { get; }

			bool done();

			void update();
		}
		#endregion

		#region Hidden move handlers
		// ===============================================================================
		// Hidden move handlers
		// ===============================================================================
		//public interface IMoveHandlerHash : IHandlerHash {
		//    void initialize();
		//}

		public interface IHiddenMoveHandlers
		{
			IDictionary<Moves, Func<Moves, IPokemon, bool>> CanUseMove { get; }
			IDictionary<Moves, Func<Moves, IPokemon, bool>> UseMove { get; }

			event EventHandler<HiddenMoveEventArgs> OnCanUseMove;
			event EventHandler<HiddenMoveEventArgs> OnUseMove;

			void addCanUseMove(Moves item, Func<Moves, IPokemon, bool> proc);

			void addUseMove(Moves item, Func<Moves, IPokemon, bool> proc);

			bool hasHandler(Moves item);

			bool triggerCanUseMove(Moves item, IPokemon pokemon);
			bool triggerUseMove(Moves item, IPokemon pokemon);
		}

		public interface IGameHiddenMoves
		{
			bool pbCanUseHiddenMove(IPokemon pkmn, Moves move);

			bool pbUseHiddenMove(IPokemon pokemon, Moves move);

			void pbHiddenMoveEvent();

			#region Hidden move animation
			// ===============================================================================
			// Hidden move animation
			// ===============================================================================
			void pbHiddenMoveAnimation(IPokemon pokemon);
			#endregion

			#region Cut
			bool pbCut();

			//HiddenMoveHandlers.CanUseMove.add(:CUT,proc{|move,pkmn|

			//HiddenMoveHandlers.UseMove.add(:CUT, proc{| move,pokemon |
			#endregion

			#region Headbutt
			void pbHeadbuttEffect(IGameCharacter @event);

			void pbHeadbutt(IGameCharacter @event);

			//HiddenMoveHandlers.CanUseMove.add(:HEADBUTT, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:HEADBUTT, proc{| move,pokemon |
			#endregion

			#region Rock Smash
			void pbRockSmashRandomEncounter();

			bool pbRockSmash();

			//HiddenMoveHandlers.CanUseMove.add(:ROCKSMASH, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:ROCKSMASH, proc{| move,pokemon |
			#endregion

			#region Strength
			bool pbStrength();

			//Events.onAction += proc{| sender,e |

			//HiddenMoveHandlers.CanUseMove.add(:STRENGTH, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:STRENGTH, proc{| move,pokemon |
			#endregion

			#region Surf
			bool pbSurf();

			void pbStartSurfing();

			bool pbEndSurf(float xOffset, float yOffset);

			void pbTransferSurfing(int mapid, float xcoord, float ycoord, float direction); //= Game.GameData.GamePlayer.direction

			//Events.onAction += proc{| sender,e |

			//HiddenMoveHandlers.CanUseMove.add(:SURF, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:SURF, proc{| move,pokemon |
			#endregion

			#region Waterfall
			void pbAscendWaterfall(IGameCharacter @event = null);

			void pbDescendWaterfall(IGameCharacter @event = null);

			bool pbWaterfall();

			//Events.onAction += proc{| sender,e |

			//HiddenMoveHandlers.CanUseMove.add(:WATERFALL, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:WATERFALL, proc{| move,pokemon |
			#endregion

			#region Dive
			bool pbDive();

			bool pbSurfacing();

			void pbTransferUnderwater(int mapid, float xcoord, float ycoord, float direction); //= Game.GameData.GamePlayer.direction

			//Events.onAction += proc{| sender,e |

			//HiddenMoveHandlers.CanUseMove.add(:DIVE, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:DIVE, proc{| move,pokemon |
			#endregion

			#region Fly
			//HiddenMoveHandlers.CanUseMove.add(:FLY, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:FLY, proc{| move,pokemon |
			#endregion

			#region Flash
			//HiddenMoveHandlers.CanUseMove.add(:FLASH, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:FLASH, proc{| move,pokemon |
			#endregion

			#region Teleport
			//HiddenMoveHandlers.CanUseMove.add(:TELEPORT, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:TELEPORT, proc{| move,pokemon |
			#endregion

			#region Dig
			//HiddenMoveHandlers.CanUseMove.add(:DIG, proc{| move,pkmn |

			//HiddenMoveHandlers.UseMove.add(:DIG, proc{| move,pokemon |
			#endregion

			#region Sweet Scent
			void pbSweetScent();

			//HiddenMoveHandlers.CanUseMove.add(:SWEETSCENT,proc{|move,pkmn|

			//HiddenMoveHandlers.UseMove.add(:SWEETSCENT, proc{| move,pokemon |
			#endregion
		}
		#endregion
	}
}
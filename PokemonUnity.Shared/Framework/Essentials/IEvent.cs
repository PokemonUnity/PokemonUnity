using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// Defines an event that procedures can subscribe to.
	/// </summary>
	public interface IEvent {
		/// <summary>
		/// Add/Removes an event handler procedure from the event.
		/// </summary>
		event EventHandler EventName; //{ add; remove; }

		IEvent initialize();

		/// <summary>
		/// Sets an event handler for this event and removes all other event handlers.
		/// </summary>
		/// <param name=""></param>
		void set(Action method);

		// Removes an event handler procedure from the event.
		//public void -(method) {
		//  for (int i = 0; i < @callbacks.length; i++) {
		//    if (@callbacks[i]==method) {
		//      @callbacks.delete_at(i);
		//      break;
		//    }
		//  }
		//  return self;
		//}
		//
		//// Adds an event handler procedure from the event.
		//public void +(method) {
		//  for (int i = 0; i < @callbacks.length; i++) {
		//    if (@callbacks[i]==method) {
		//      return self;
		//    }
		//  }
		//  @callbacks.Add(method);
		//  return self;
		//}

		/// <summary>
		/// Clears the event of event handlers.
		/// </summary>
		void clear();

		/// <summary>
		/// Triggers the event and calls all its event handlers. Normally called only
		/// by the code where the event occurred.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// The first argument is the sender of the event, the second argument contains
		/// the event's parameters. If three or more arguments are given, this method
		/// supports the following callbacks:
		/// proc {|sender,params| } where params is an array of the other parameters, and
		/// proc {|sender,arg0,arg1,...| }
		//void trigger(params int[] arg);
		void trigger(object sender, EventArgs e);

		/// <summary>
		/// Triggers the event and calls all its event handlers. Normally called only
		/// by the code where the event occurred. The first argument is the sender of the
		/// event, the other arguments are the event's parameters.
		/// </summary>
		/// <param name=""></param>
		//void trigger2(*arg);
		void trigger2(object sender, EventArgs e);
	}
}
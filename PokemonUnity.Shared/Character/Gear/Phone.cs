using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Character
{
	/// <summary>
	/// The phone allows the player to store the phone numbers of various trainers and other important people, 
	/// and to call them and receive calls from them at any time.
	/// The numbers in the phone are stored in the array <see cref="IGlobalMetadata.phoneNumbers"/>. 
	/// </summary>
	public class Phone
	{
		private static string[] generics;
		private static string[] greetings;
		private static string[] greetingsMorning;
		private static string[] greetingsEvening;
		private static string[] bodies1;
		private static string[] bodies2;
		private static string[] battleRequests;
		private static string[] trainers;

		public string this[int msgType, int msgId]
		{
			get
			{
				if (msgType == (int)PhoneMsgTypes.Greeting)
				{
					//if it's morning, and greetingMorning is not null
					//if it's evening, and greetingEvening is not null
					//else
					return greetings[msgId];
				}
				else return generics[msgId];
			}
		}

		//Constructor (string[] => private variables)
	}

	/// <summary>
	/// </summary>
	/// <remarks>
	/// When the player receives a phone call from a trainer, 
	/// it is from a randomly-chosen trainer out of all the ones that can call the player. 
	/// Possible callers are those that are on a different map to the one the player is currently on, 
	/// but are in the same region as the player.
	/// You cannot be called by someone if you are on the same map as them.
	/// </remarks>
	public interface IPhoneMessageData
	{
		/// <summary>
		/// The first part of all phone calls. 
		/// </summary>
		/// <remarks>
		/// The rest of a phone call is one of the below three options (body, generic, request).
		/// </remarks>
		string Greetings		{ get; }
		string GreetingsMorning	{ get; }
		string GreetingsEvening	{ get; }
		/// <summary>
		/// These two (<see cref="bodies1"/> & <see cref="bodies2"/>) bits of dialogue are always used together.
		/// </summary>
		string Bodies1			{ get; }
		/// <summary>
		/// These two (<see cref="bodies1"/> & <see cref="bodies2"/>) bits of dialogue are always used together.
		/// </summary>
		string Bodies2			{ get; }
		/// <summary>
		/// This is a complete phone call in itself, minus the "Greetings" part.
		/// </summary>
		string Generics			{ get; }
		/// <summary>
		/// A phrase which declares that the caller is ready for a rematch.
		/// </summary>
		string BattleRequests	{ get; }
		//string trainer		{ get; }

		//string this[int msgType, int msgId] { get; }
	}
	/// <summary>
	/// For non-trainer contacts ("special" contacts).
	/// Each special contact in your phone can be called at any time by choosing them from the phone. 
	/// They will never call the player, though.
	/// </summary>
	public interface IPhoneContact
	{
		/// <summary>
		/// A number unique to each contact.
		/// It is used to determine which charset to show in the contact list, 
		/// and which Common Event contains the phone messages for this contact.
		/// </summary>
		int Id { get; }
		/// <summary>
		/// This is either TRUE or FALSE.
		/// If this is TRUE, then the contact is displayed in the phone list (so the player can call them).
		/// </summary>
		bool IsVisible { get; }
		/// <summary>
		/// The full display name of the contact (e.g. "Professor Oak").
		/// </summary>
		string Name { get; }
		/// <summary>
		/// The ID of the map corresponding to this contact.
		/// It is only used to show the contact's location in the contact list.
		/// </summary>
		/// <remarks>
		/// The contact can always be called regardless of location, 
		/// and you may want to add in appropriate messages to the contact's Common Event depending on the player's location 
		/// (i.e.the NPC telling the player to just go up and talk to them instead of calling them).
		/// </remarks>
		int MapId { get; }
	}
	/// <summary>
	/// For trainer contacts
	/// </summary>
	public interface IPhoneTrainerContact : IPhoneContact
	{
		/// <summary>
		/// The trainer type of the contact (e.g. 14, <see cref="TrainerTypes.CAMPER"/>).
		/// In the contact list, this is the first part of the displayed name, e.g.the "Camper" in "Camper Andrew".
		/// </summary>
		/// <remarks>
		/// Remember that all trainer contacts must have an associated defined trainer, even if that trainer is never battled.
		/// </remarks>
		TrainerTypes Type { get; }
		/// <summary>
		/// When the trainer is registered, and each time the player defeats them in a rematch, this value is set to 2000+rand(2000), 
		/// and reduces by 1 every 1/4 second (except when messages are being displayed or the player is being forced to move by a move route). 
		/// When it reaches 0, the <see cref="CanBattle"/> value is changed from 1 to 2.
		/// </summary>
		/// <remarks>
		/// This value will hit 0 between 8m 20s and 16m 40s after the trainer was last defeated(not including time spent displaying messages, etc.).
		/// </remarks>
		int NextBattleTime { get; }
		/// <summary>
		/// This value is reset to 0 after the player defeats the trainer and then to 1 once its <see cref="NextBattleTime"/> has been set to a value. 
		/// It is set to 2 when the <see cref="NextBattleTime"/> hits zero. 
		/// It is changed from 2 to 3 after the contact calls the player to tell them they are ready for a rematch.
		/// <para></para>
		/// If this value is 2, when the contact calls the player (or vice versa), 
		/// they will tell the player they are ready for a rematch. 
		/// If this value is 3, when the contact calls the player (or vice versa), 
		/// there is only a 50% chance they will remind the player they are ready for a rematch 
		/// - the other 50% of the time they will say the usual random dialogue.
		/// </summary>
		/// <remarks>
		/// Note that the contact does not need to have called the player before they can be rebattled, 
		/// as the rematch is set up as soon as its <see cref="NextBattleTime"/> hits zero.
		/// </remarks>
		PhoneBattleStatuses CanBattle { get; }
		/// <summary>
		/// This number is the version of the trainer that the player will battle next 
		/// (0 is the original battle, 1 is the first rematch, etc.). 
		/// This is increased by 1 after each time the player defeats the trainer.
		/// </summary>
		/// <remarks>
		/// This value cannot be increased beyond a maximum number, 
		/// which is the number of "Battle" comments in the event. 
		/// Once all the different versions of the trainer have been battled, 
		/// all subsequent rematches with that trainer will be with the final version of that trainer.
		/// </remarks>
		int? RematchId { get; }
		/// <summary>
		/// The ID of the trainer's event on the map mentioned above.
		/// If this is defined along with the map ID, and if the <see cref="IsVisible"/> value is TRUE, 
		/// then their event's Self Switches will be altered to allow rematches when they become available. 
		/// This value has no other effect.
		/// </summary>
		int? EventId { get; }
	}

	public enum PhoneMsgTypes : int
	{
		Generic			= 0,
		Greeting		= 1,
		Body			= 2,
		BattleRequest	= 3
	}
	public enum PhoneBattleStatuses : int
	{
		/// <summary>
		/// The trainer cannot be rebattled, and its <see cref="IPhoneTrainerContact.NextBattleTime"/> needs resetting.
		/// </summary>
		BATTLED		= 0,
		/// <summary>
		/// The trainer cannot be rebattled yet, and it is counting down.
		/// </summary>
		RESETTING	= 1,
		/// <summary>
		/// The trainer can be rebattled, but has not yet called the player to say so.
		/// </summary>
		IDLE		= 2,
		/// <summary>
		/// The trainer can be rebattled, and has called the player to say so.
		/// </summary>
		READY		= 3
	}
}
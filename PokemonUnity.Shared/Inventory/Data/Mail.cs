using System.Collections;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Inventory;

namespace PokemonUnity.Inventory//.ItemData
{
	/// <summary>
	/// Data structure representing mail that the Pokémon can hold
	/// </summary>
	public class Mail
	{
		public Items Background { get; private set; }
		public string Message { get; set; }
		/// <summary>
		/// Used when displaying in UI, one character at a time.
		/// </summary>
		public char[] Display { get { return Message == null? null: Message.ToCharArray(); } }
		public string Sender { get { return sender.Name; } }
		private TrainerId sender { get; set; }//ToDO: Only need name and message..
		//public bool IsLetter { get { return Game.ItemData[item].IsLetter; } }
		//public static bool IsMail(Items item) { return Game.ItemData[item].IsLetter; }//{ return new Item(item).IsMail; }

		public Mail(Items letter)
		{
			Background = Game.ItemData[letter].IsLetter ? letter : Items.NONE;
		}

		/// <summary>
		/// </summary>
		/// <param name="item">Item represented by this mail</param>
		/// <param name="message">Message text</param>
		/// <param name="sender">Name of the message's sender</param>
		public Mail(Items item, string message, TrainerId sender) : this(item)
		{
			if (!string.IsNullOrEmpty(message)) Message = message.Length > 255 ? message.Substring(0, (byte)255) : message;
			this.sender = sender;
		}
	}
}
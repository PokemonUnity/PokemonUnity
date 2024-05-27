using System.Collections;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Item;

namespace PokemonUnity.Inventory
{
	/// <summary>
	/// Data structure representing mail that the Pokémon can hold
	/// </summary>
	public class Mail : IMail
	{
		/// <summary>
		/// If using special letter template
		/// </summary>
		public Items Background { get; private set; }
		public string Message { get; set; }
		/// <summary>
		/// Used when displaying in UI, one character at a time.
		/// </summary>
		public char[] Display { get { return Message == null? null: Message.ToCharArray(); } }
		public string Sender { get { return sender.name; } }
		/// <summary>
		/// Item attached to the letter
		/// </summary>
		public Items item { get; set; }

		public string message { get { return Message; } set { Message = value; } }

		string IMail.sender { get { return Sender; } }

		public int poke1 { get { throw new System.NotImplementedException(); } }

		public int poke2 { get { throw new System.NotImplementedException(); } }

		public int poke3 { get { throw new System.NotImplementedException(); } }

		private ITrainer sender; //ToDO: Only need name and message..
		//public bool IsLetter { get { return Game.ItemData[item].IsLetter; } }
		//public static bool IsMail(Items item) { return Game.ItemData[item].IsLetter; }//{ return new Item(item).IsMail; }

		public Mail(Items letter)
		{
			Background = ItemData.IsLetter(letter) ? letter : Items.NONE;
		}

		/// <summary>
		/// </summary>
		/// <param name="bg">Item represented by this mail; default none</param>
		/// <param name="item">Item attached to letter</param>
		/// <param name="message">Message text</param>
		/// <param name="sender">Name of the message's sender</param>
		public Mail(Items bg, Items item, string message, ITrainer sender) : this(bg)
		{
			initialize(item, message, sender.name);
			//if (!string.IsNullOrEmpty(message)) Message = message.Length > 255 ? message.Substring(0, (byte)255) : message;
			this.sender = sender;
		}

		public IMail initialize(Items item, string message, string sender, IPokemon poke1 = null, IPokemon poke2 = null, IPokemon poke3 = null)
		{
			this.item = item;
			if (!string.IsNullOrEmpty(message)) Message = message.Length > 255 ? message.Substring(0, (byte)255) : message;
			//this.sender.name = sender;
			return this;
		}
	}
}
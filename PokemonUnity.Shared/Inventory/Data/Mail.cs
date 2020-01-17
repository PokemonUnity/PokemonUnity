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
		public int Background { get; private set; }
		public string Message { get; set; }
		/// <summary>
		/// Used when displaying in UI, one character at a time.
		/// </summary>
		public char[] Display { get { return Message.ToCharArray(); } }
		public string Sender { get { return sender.Name; } }
		private Trainer sender { get; set; }
		public bool IsLetter { get; private set; }

		public static bool IsMail(Items item) { return Game.ItemData[item].IsLetter; }//{ return new Item(item).IsMail; }

		public Mail(Items letter)
		{
			IsLetter = true;
			//assign background art of letter based on item
			switch (letter)
			{
				case Items.AIR_MAIL:
				case Items.BEAD_MAIL:
				case Items.BLOOM_MAIL:
				case Items.BRICK_MAIL:
				case Items.BRIDGE_MAIL_D:
				case Items.BRIDGE_MAIL_M:
				case Items.BRIDGE_MAIL_S:
				case Items.BRIDGE_MAIL_T:
				case Items.BRIDGE_MAIL_V:
				case Items.BUBBLE_MAIL:
				case Items.DREAM_MAIL:
				case Items.FAB_MAIL:
				case Items.FAVORED_MAIL:
				case Items.FLAME_MAIL:
				case Items.GLITTER_MAIL:
				case Items.GRASS_MAIL:
				case Items.GREET_MAIL:
				case Items.HARBOR_MAIL:
				case Items.HEART_MAIL:
				case Items.INQUIRY_MAIL:
				case Items.LIKE_MAIL:
				case Items.MECH_MAIL:
				case Items.MOSAIC_MAIL:
				case Items.ORANGE_MAIL:
				case Items.REPLY_MAIL:
				case Items.RETRO_MAIL:
				case Items.RSVP_MAIL:
				case Items.SHADOW_MAIL:
				case Items.SNOW_MAIL:
				case Items.SPACE_MAIL:
				case Items.STEEL_MAIL:
				case Items.THANKS_MAIL:
				case Items.TROPIC_MAIL:
				case Items.TUNNEL_MAIL:
				case Items.WAVE_MAIL:
				case Items.WOOD_MAIL:
					break;
				default:
					IsLetter = false;
					break;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="item">Item represented by this mail</param>
		/// <param name="message">Message text</param>
		/// <param name="sender">Name of the message's sender</param>
		public Mail(Items item, string message, Trainer sender) : this(item)
		{
			if (!string.IsNullOrEmpty(message)) Message = message.Length > 255 ? message.Substring(0, (byte)255) : message;
			this.sender = sender;
		}
	}
}
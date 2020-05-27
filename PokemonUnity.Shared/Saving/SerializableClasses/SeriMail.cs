using PokemonUnity.Inventory;

namespace PokemonUnity.Saving.SerializableClasses
{
	[System.Serializable]
	public struct SeriMail
	{
		private int MailId { get; set; }
		public string Message { get; private set; }
		//Background will stay 0 (new int) until Mail's background feature is implemented
		//public int Background { get; private set; }

		public static implicit operator Mail(SeriMail mail)
		{
			Mail newMail = new Mail((Items)mail.MailId);
			newMail.Message = mail.Message;
			return newMail;
		}

		public SeriMail(Items mailItem, string messsage)
		{
			MailId = (int)mailItem;
			Message = messsage;
		}
	}
}
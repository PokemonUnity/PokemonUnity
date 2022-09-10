﻿using System;

namespace PokemonUnity.Client.Hubs
{
	public class Subscription
	{
		public event Action<object[]> Data;

		internal void OnData(object[] data)
		{
			if (Data != null)
				Data(data);
		} 
	}
}

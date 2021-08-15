using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonUnity.Application;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonUnity.Saving.SerializableClasses;

namespace PokemonUnity.Saving
{
	public class SaveDataConverter : JsonConverter
	{
		public override bool CanConvert(System.Type objectType)
		{
			return typeof(SaveData) == objectType;
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			//object[] obj = jo["GameStates"].ToObject<object[]>();
			//object[] obj = jo["GameStates"].ToObject<List<object>>();
			var obj = (IList)ToObject(jo["GameStates"]);
			//if doesnt work, use object[] and create a new GameState object using for-loop...
			GameState[] states = new GameState[obj.Count]; //jo["GameStates"].ToObject<GameState[]>();
			for (int i = 0; i < states.Length; i++)
			{
				//ToDo: string is (int?)null else int
				states[i] = new GameState(
					player: new Character.Player(
						name:			((IDictionary<string, object>)obj[i])["PlayerName"].ToString()
						, gender:		bool.Parse(((IDictionary<string, object>)obj[i])["IsMale"].ToString())
						, party:		((List<SeriPokemon>)((IDictionary<string, object>)obj[i])["PlayerParty"]).ToArray().Deserialize()
						, bag:			((List<Items>)((IDictionary<string, object>)obj[i])["PlayerBag"]).ToArray()
						, pc_poke:		((SeriPC)((IDictionary<string, object>)obj[i])["PlayerPC"]).Pokemons.Deserialize()//.GetPokemonsFromSeri()
						, pc_items:		((SeriPC)((IDictionary<string, object>)obj[i])["PlayerPC"]).GetItemsFromSeri().Compress()
						, pc_box:		((SeriPC)((IDictionary<string, object>)obj[i])["PlayerPC"]).ActiveBox
						, pc_names:		((SeriPC)((IDictionary<string, object>)obj[i])["PlayerPC"]).BoxNames
						, pc_textures:	((SeriPC)((IDictionary<string, object>)obj[i])["PlayerPC"]).BoxTextures
						, trainerid:	int.Parse(((IDictionary<string, object>)obj[i])["TrainerID"].ToString())
						, secretid:		int.Parse(((IDictionary<string, object>)obj[i])["SecretID"].ToString())
						, money:		int.Parse(((IDictionary<string, object>)obj[i])["PlayerMoney"].ToString())
						, coin:			int.Parse(((IDictionary<string, object>)obj[i])["PlayerCoins"].ToString())
						, bank:			int.Parse(((IDictionary<string, object>)obj[i])["PlayerSavings"].ToString())
						, repel:		int.Parse(((IDictionary<string, object>)obj[i])["RepelSteps"].ToString())
						, time:			TimeSpan.Parse(((IDictionary<string, object>)obj[i])["PlayTime"].ToString())
						//, position:		(Vector?)((IDictionary<string, object>)obj[i])["PlayerPosition"]
						, follower:		byte.Parse(((IDictionary<string, object>)obj[i])["FollowerPokemon"].ToString())
						, creator:		bool.Parse(((IDictionary<string, object>)obj[i])["IsCreator"].ToString())
						//, map: 
						, pokecenter:	int.Parse(((IDictionary<string, object>)obj[i])["PokeCenterId"].ToString())
						//, gym:			((List<KeyValuePair<GymBadges, DateTime?>>)((IDictionary<string, object>)obj[i])["GymsChallenged"]).ToArray()
					)
					//,challenge: (Challenges?)obj[i]["Challenge"]
				);
			}
			string ver = jo["BuildVersion"].ToObject<string>();
			string create = jo["TimeCreated"].ToObject<string>();
			int lan = jo["Language"].ToObject<int>();
			int win = jo["WindowBorder"].ToObject<int>();
			int dia = jo["TextSpeed"].ToObject<int>();
			int spd = jo["TextSpeed"].ToObject<int>();
			float m = jo["mVol"].ToObject<float>();
			float s = jo["sVol"].ToObject<float>();
			return new SaveData(state: states, build: ver, date: create, language: (Languages?)lan, windowBorder: (byte?)win, dialogBorder: (byte?)dia, textSpeed: (byte?)spd, mvol: m, svol: s);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value);
		}

		//public static ExpandoObject ToExpando(string json)
		//{
		//	if (string.IsNullOrEmpty(json))
		//		return null;
		//	return (ExpandoObject)ToExpandoObject(JToken.Parse(json));
		//}
		//
		//private static object ToExpandoObject(JToken token)
		//{
		//
		//	switch (token.Type)
		//	{
		//		case JTokenType.Object:
		//			var expando = new ExpandoObject();
		//			var expandoDic = (IDictionary<string, object>)expando;
		//			//Dictionary<string, object> expandoDic = new Dictionary<string, object>();
		//			foreach (var prop in token.Children<JProperty>())
		//				expandoDic.Add(prop.Name, ToExpandoObject(prop.Value));
		//			return expando;
		//		case JTokenType.Array:
		//			return token.Select(ToExpandoObject).ToList();
		//
		//		default:
		//			return ((JValue)token).Value;
		//	}
		//}

		public static object ToObject(string json)
		{
			if (string.IsNullOrEmpty(json))
				return null;
			return ToObject(JToken.Parse(json));
		}

		private static object ToObject(JToken token)
		{
			switch (token.Type)
			{
				case JTokenType.Object:
					return token.Children<JProperty>()
								.ToDictionary(prop => prop.Name,
											  prop => ToObject(prop.Value),
											  StringComparer.OrdinalIgnoreCase);

				case JTokenType.Array:
					return token.Select(ToObject).ToList();

				default:
					return ((JValue)token).Value;
			}
		}
	}
}

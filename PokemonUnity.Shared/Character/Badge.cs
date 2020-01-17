using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Character
{
	/// <summary>
	/// This class handles all actions regarding Badge loading and displaying.
	/// </summary
	public class Badge
	{
		//ToDo: public static Moves[] HMs
		//ToDo: Move HMs to Machine class... Match TMs to MoveId?
		public enum HMMoves
		{
			Surf,
			Cut,
			Strength,
			Flash,
			Fly,
			Whirlpool,
			Waterfall,
			Ride,
			Dive,
			RockClimb
		}

		private static List<BadgeDeclaration> Badges = new List<BadgeDeclaration>();

		/// <summary>
		/// This describes one Badge loaded by a GameMode with ID, Name, Texture and Arguments.
		/// </summary>
		//ToDo: Rename to BadgeData
		[System.Serializable]
		private struct BadgeDeclaration
		{
			/// <summary>
			/// Id of Trainer Badge
			/// </summary>
			/// ToDo: Use enum here
			public GymBadges ID { get; private set; } //= 0;
			public string Name { get; private set; } //= "";
			public byte LevelCap { get; private set; } //= -1;
			public List<HMMoves> HMs { get; private set; } //= new List<HMMoves>();
			//ToDo: Vector4 to SeriV4
			//public UnityEngine.Vector4 TextureRectangle { get; set; }
			public string TexturePath { get; private set; } //= @"GUI\Badges";
			public Regions Region { get; private set; } //= "Johto";

			public BadgeDeclaration(string inputData)
			{
				HMs = new List<HMMoves>();
				//TextureRectangle = new UnityEngine.Vector4(0, 0, 50, 50);
				string[] data = inputData.Split(System.Convert.ToChar("|"));

				this.ID = (GymBadges)System.Convert.ToInt32(data[0]);
				this.Name = data[1];
				this.LevelCap = 0;
				this.Region = Regions.NOT_IN_OVERWORLD;
				this.TexturePath = @"GUI\Badges";

				if (data.Count() > 2)
				{
					for (var i = 2; i <= data.Count() - 1; i++)
					{
						string argName = data[i].Remove(data[i].IndexOf("="));
						string argData = data[i].Remove(0, data[i].IndexOf("=") + 1);

						switch (argName.ToLower())
						{
							case "level":
								{
									this.LevelCap = System.Convert.ToByte(argData);
									break;
								}
							case "hm":
								{
									string[] hms = argData.Split(System.Convert.ToChar(","));

									foreach (string hm in hms)
									{
										switch (hm.ToLower())
										{
											case "surf":
												{
													this.HMs.Add(HMMoves.Surf);
													break;
												}
											case "cut":
												{
													this.HMs.Add(HMMoves.Cut);
													break;
												}
											case "strength":
												{
													this.HMs.Add(HMMoves.Strength);
													break;
												}
											case "flash":
												{
													this.HMs.Add(HMMoves.Flash);
													break;
												}
											case "fly":
												{
													this.HMs.Add(HMMoves.Fly);
													break;
												}
											case "whirlpool":
												{
													this.HMs.Add(HMMoves.Whirlpool);
													break;
												}
											case "waterfall":
												{
													this.HMs.Add(HMMoves.Waterfall);
													break;
												}
											case "ride":
												{
													this.HMs.Add(HMMoves.Ride);
													break;
												}
											case "dive":
												{
													this.HMs.Add(HMMoves.Dive);
													break;
												}
											case "rockclimb":
												{
													this.HMs.Add(HMMoves.RockClimb);
													break;
												}
										}
									}
									break;
								}
							case "texture":
								{
									string[] texData = argData.Split(System.Convert.ToChar(","));
									this.TexturePath = texData[0];
									//this.TextureRectangle = new UnityEngine.Vector4(System.Convert.ToInt32(texData[1]), System.Convert.ToInt32(texData[2]), System.Convert.ToInt32(texData[3]), System.Convert.ToInt32(texData[4]));
									break;
								}
							case "region":
								{
									//this.Region = argData;
									this.Region = (Regions)System.Convert.ToInt32(argData);
									break;
								}
						}
					}
				}
			}
		}

		/// <summary>
		/// Loads the Badges. Only use after the GameMode got set.
		/// </summary>
		public static void Load()
		{
			Badges.Clear();

			string file = "";//GameModeManager.GetContentFilePath(@"Data\badges.dat");
							 //System.Security.FileValidation.CheckFileValid(file, false, "Badge.vb");
			string[] data = System.IO.File.ReadAllLines(file);
			foreach (string line in data)
			{
				if (line.Contains("|"))
					Badges.Add(new BadgeDeclaration(line));
			}
		}


		/* ToDo: use XML
		/// <summary>
		/// Gets the Badge name.
		/// </summary>
		/// <param name="ID">The ID of the Badge.</param>
		public static string GetBadgeName(int ID)
		{
			foreach (BadgeDeclaration b in Badges)
			{
				if (b.ID == ID)
					return b.Name;
			}
			return "Plain";
		}

		/// <summary>
		/// Gets the badge texture.
		/// </summary>
		/// <param name="ID">The ID of the badge.</param>
		public static UnityEngine.Texture2D GetBadgeTexture(int ID)
		{
			foreach (BadgeDeclaration b in Badges)
			{
				if (b.ID == ID)
					return TextureManager.GetTexture(b.TexturePath, b.TextureRectangle, "");
			}
			return TextureManager.GetTexture(@"GUI\Badges", new UnityEngine.Vector4(0, 0, 50, 50), "");
		}

		/// <summary>
		/// Gets the highest level cap the player can use traded Pokémon on.
		/// </summary>
		public static int GetLevelCap()
		{
			int[] trainerBadges = Game.GameData.Player.Badges;
			int highestCap = 10;
			foreach (BadgeDeclaration b in Badges)
			{
				if (b.LevelCap > highestCap & trainerBadges.Contains(b.ID))
					highestCap = b.LevelCap;
			}
			return highestCap;
		}

		/// <summary>
		/// Checks if the player is able to perform a certain Hidden Machine move.
		/// </summary>
		/// <param name="HM">The Hidden Machine move the player tries to use.</param>
		public static bool CanUseHMMove(HMMoves HM)
		{
			int[] trainerBadges = Game.GameData.Player.Badges;
			foreach (BadgeDeclaration b in Badges)
			{
				if (b.HMs.Contains(HM) & trainerBadges.Contains(b.ID) | b.ID == 0)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the region.
		/// </summary>
		/// <param name="index">The index of the region in the Badges enumeration.</param>
		public static string GetRegion(int index)
		{
			List<string> regions = new List<string>();
			foreach (BadgeDeclaration b in Badges)
			{
				if (!regions.Any(m => m.ToLowerInvariant() == b.Region.ToLowerInvariant()))
					regions.Add(b.Region);
			}
			if (regions.Count - 1 >= index)
				return regions[index];
			else
				return "Johto";
		}

		/// <summary>
		/// Gets the amount of Badges in a certain region.
		/// </summary>
		/// <param name="region">The region to count the Badges.</param>
		public static int GetBadgesCount(string region)
		{
			int c = 0;
			foreach (BadgeDeclaration b in Badges)
			{
				if (b.Region.ToLower() == region.ToLower())
					c += 1;
			}
			return c;
		}

		/// <summary>
		/// Returns the amount of regions that exists in total.
		/// </summary>
		public static int GetRegionCount()
		{
			List<string> regions = new List<string>();
			foreach (BadgeDeclaration b in Badges)
			{
				if (!regions.Any(m => m.ToLowerInvariant() == b.Region.ToLowerInvariant()))
					regions.Add(b.Region);
			}
			return regions.Count;
		}

		/// <summary>
		/// Gets the ID of a Badge.
		/// </summary>
		/// <param name="region">The region this Badge is from.</param>
		/// <param name="index">The index of this Badge.</param>
		public static int GetBadgeID(string region, int index)
		{
			List<BadgeDeclaration> cBadges = new List<BadgeDeclaration>();
			foreach (BadgeDeclaration b in Badges)
			{
				if (b.Region.ToLower() == region.ToLower())
					cBadges.Add(b);
			}
			if (cBadges.Count - 1 >= index)
				return cBadges[index].ID;
			else
				return 1;
		}

		/// <summary>
		/// Checks if the player has a certain Badge.
		/// </summary>
		/// <param name="BadgeID">The Badge ID to check for.</param>
		public static bool PlayerHasBadge(int BadgeID)
		{
			return Game.GameData.Player.Badges.Contains(BadgeID);
		}*/
	}
}
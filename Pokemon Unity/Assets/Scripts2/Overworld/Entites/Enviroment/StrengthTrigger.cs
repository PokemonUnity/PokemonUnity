namespace PokemonUnity.Overworld.Entity.Environment
{
	public class StrengthTrigger : Entity
	{
		private bool RemoveRock = false;
		private bool RemoveForever = false;
		private string ActivateScript = "";

		private bool Activated = false;
		public static bool RemovedRegistered = false;

		public override void Initialize()
		{
			base.Initialize();

			string[] v = this.AdditionalValue.Split(System.Convert.ToChar(","));

			for (var i = 0; i <= v.Length; i++)
			{
				switch (i)
				{
					case 0:
						{
							RemoveRock = System.Convert.ToBoolean(v[i]);
							break;
						}
					case 1:
						{
							RemoveForever = System.Convert.ToBoolean(v[i]);
							break;
						}
					case 2:
						{
							ActivateScript = v[i];
							break;
						}
				}
			}

			string[] registers = null;//Game.Player.RegisterData.Split(System.Convert.ToChar(","));
			foreach (string r in registers)
			{
				if (r.StartsWith("ACTIVATOR_REMOVE_STRENGTH_ROCK_" + Game.Level.LevelFile + "_"))
				{
					string RemoveID = r.Remove(0, ("ACTIVATOR_REMOVE_STRENGTHT_ROCK_" + Game.Level.LevelFile + "_").Length - 1);
					foreach (Entity sRock in Game.Level.Entities)
					{
						if (sRock.EntityID == Entities.StrengthRock)
						{
							if (sRock.ID == System.Convert.ToInt32(RemoveID))
								sRock.CanBeRemoved = true;
						}
					}
				}
			}

			this.NeedsUpdate = true;
		}

		public override void Update()
		{
			if (!Activated)
			{
				foreach (Entity sRock in Game.Level.Entities)
				{
					if (sRock.EntityID == Entities.StrengthRock)
					{
						if (sRock.Position.x == this.Position.x & sRock.Position.z == this.Position.z)
						{
							if (RemoveRock)
								((StrengthRock)sRock).CanBeRemoved = true;
							//if (RemoveForever)
							//    ActionScript.RegisterID("ACTIVATOR_REMOVE_STRENGTH_ROCK_" + Game.Level.LevelFile + "_" + sRock.ID.ToString());
							if (ActivateScript != "")
							{
								//if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
								//    ((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(this.ActivateScript, 0, false);
							}

							Activated = true;
						}
					}
				}
			}

			base.Update();
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, true);
		}
	}
}
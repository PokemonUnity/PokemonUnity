namespace PokemonUnity.Overworld.Entity.Misc
{
	public class MessageBulb : Entity
{
	public enum NotifcationTypes
	{
		Waiting = 0,
		Exclamation = 1,
		Shouting = 2,
		Question = 3,
		Note = 4,
		Heart = 5,
		Unhappy = 6,
		Happy = 7,
		Friendly = 8,
		Poisoned = 9,
		Battle = 10,
		Wink = 11,
		AFK = 12,
		Angry = 13,
		CatFace = 14,
		Unsure = 15
	}

	public NotifcationTypes NotificationType = NotifcationTypes.Exclamation;
	private bool setTexture = false;
	private float delay = 0.0F;

	public MessageBulb(Vector3 Position, NotifcationTypes NotificationType) : base(Position.x, Position.y, Position.z, "MessageBulb", null,
		new int[] {
			0,
			0
		}, false, 0, new Vector3(0.8F), UnityEngine.Mesh.BillModel, 0, "", new Vector3(1.0F))
    {
        this.NotificationType = NotificationType;

		LoadTexture();
        this.NeedsUpdate = true;
        this.delay = 8.0F;

        this.DropUpdateUnlessDrawn = false;
    }

	public override void Update()
	{
		if (this.delay > 0.0F)
		{
			this.delay -= 0.1F;
			if (this.delay <= 0.0F)
			{
				this.delay = 0.0F;
				this.CanBeRemoved = true;
			}
		}
	}

	private void LoadTexture()
	{
		if (!this.setTexture)
		{
			this.setTexture = true;

			Rectangle r = new Rectangle(0, 0, 16, 16);
			switch (this.NotificationType)
			{
				case NotifcationTypes.Waiting:
					{
						r = new Rectangle(0, 0, 16, 16);
						break;
					}

				case NotifcationTypes.Exclamation:
					{
						r = new Rectangle(16, 0, 16, 16);
						break;
					}

				case NotifcationTypes.Shouting:
					{
						r = new Rectangle(32, 0, 16, 16);
						break;
					}

				case NotifcationTypes.Question:
					{
						r = new Rectangle(48, 0, 16, 16);
						break;
					}

				case NotifcationTypes.Note:
					{
						r = new Rectangle(0, 16, 16, 16);
						break;
					}

				case NotifcationTypes.Heart:
					{
						r = new Rectangle(16, 16, 16, 16);
						break;
					}

				case NotifcationTypes.Unhappy:
					{
						r = new Rectangle(32, 16, 16, 16);
						break;
					}

				case NotifcationTypes.Happy:
					{
						r = new Rectangle(0, 32, 16, 16);
						break;
					}

				case NotifcationTypes.Friendly:
					{
						r = new Rectangle(16, 32, 16, 16);
						break;
					}

				case NotifcationTypes.Poisoned:
					{
						r = new Rectangle(32, 32, 16, 16);
						break;
					}

				case NotifcationTypes.Battle:
					{
						r = new Rectangle(48, 16, 16, 16);
						break;
					}

				case NotifcationTypes.Wink:
					{
						r = new Rectangle(48, 32, 16, 16);
						break;
					}

				case NotifcationTypes.AFK:
					{
						r = new Rectangle(0, 48, 16, 16);
						break;
					}

				case NotifcationTypes.Angry:
					{
						r = new Rectangle(16, 48, 16, 16);
						break;
					}

				case NotifcationTypes.CatFace:
					{
						r = new Rectangle(32, 48, 16, 16);
						break;
					}

				case NotifcationTypes.Unsure:
					{
						r = new Rectangle(48, 48, 16, 16);
						break;
					}
			}

			this.Textures =
				new UnityEngine.Texture2D()
				{
					P3D.TextureManager.GetTexture("emoticons", r)
				};
		}
	}

	public override void UpdateEntity()
	{
		if (this.Rotation.y != GameVariables.Camera.Yaw)
		{
			this.Rotation.y = GameVariables.Camera.Yaw;
			CreatedWorld = false;
		}

		base.UpdateEntity();
	}

	public override void Render()
	{
		this.Draw(this.Model, this.Textures, true);
	}
}
}
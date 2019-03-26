using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Misc
{
public class NPC : Entity
{
	const float STANDARD_SPEED = 0.04F;

	public enum Movements
	{
		Still,
		Looking,
		FacePlayer,
		Walk,
		Straight,
		Turning,
		Pokeball
	}

	public string Name;
	public int NPCID;
	public int faceRotation;
	public string TextureID;
	private Rectangle lastRectangle = new Rectangle(0, 0, 0, 0);
	private Vector2 FrameSize = new Vector2(32, 32);
	private Texture2D Texture;
	public int ActivationValue = 0;

	public bool HasPokemonTexture = false;

	public bool IsTrainer = false;
	public int TrainerSight = 1;
	public bool TrainerBeaten = false;
	public bool TrainerChecked = false;

	private bool AnimateIdle = true;
	private int AnimationX = 1;
	const float AnimationDelayLenght = 1.1F;
	private float AnimationDelay = AnimationDelayLenght;

	public Movements Movement = Movements.Still;
	public List<Rectangle> MoveRectangles = new List<Rectangle>();
	public float TurningDelay = 2.0F;

	public float MoveY = 0.0F;
	public bool MoveAsync = false;

	public new void Initialize(string TextureID, int Rotation, string Name, int ID, bool AnimateIdle, string Movement, List<Rectangle> MoveRectangles)
	{
		base.Initialize();

		this.Name = Name;
		this.NPCID = ID;
		this.faceRotation = Rotation;
		this.TextureID = TextureID;
		this.MoveRectangles = MoveRectangles;
		this.AnimateIdle = AnimateIdle;

		ApplyNPCData();

		switch (Movement.ToLower())
		{
			case "pokeball":
				{
					this.Movement = Movements.Pokeball;
					break;
				}
			case "still":
				{
					this.Movement = Movements.Still;
					break;
				}
			case "looking":
				{
					this.Movement = Movements.Looking;
					break;
				}
			case "faceplayer":
				{
					this.Movement = Movements.FacePlayer;
					break;
				}
			case "walk":
				{
					this.Movement = Movements.Walk;
					break;
				}
			case "straight":
				{
					this.Movement = Movements.Straight;
					break;
				}
			case "turning":
				{
					this.Movement = Movements.Turning;
					break;
				}
		}

		SetupSprite(this.TextureID, "", false);

		this.NeedsUpdate = true;
		this.CreateWorldEveryFrame = true;

		if (ActionValue == 2)
		{
			this.IsTrainer = true;

			this.TrainerSight = System.Convert.ToInt32(this.AdditionalValue.GetSplit(0, "|"));
			this.AdditionalValue = this.AdditionalValue.GetSplit(1, "|");
		}

		this.DropUpdateUnlessDrawn = false;
	}

	public void SetupSprite(string UseTextureID, string GameJoltID, bool UseGameJoltID)
	{
		this.TextureID = UseTextureID;

		string texturePath = @"Textures\NPC\";
		HasPokemonTexture = false;
		if (this.TextureID.StartsWith("[POKEMON|N]") == true | this.TextureID.StartsWith("[Pokémon|N]") == true)
		{
			this.TextureID = this.TextureID.Remove(0, 11);
			texturePath = @"Pokemon\Overworld\Normal\";
			HasPokemonTexture = true;
		}
		else if (this.TextureID.StartsWith("[POKEMON|S]") == true | this.TextureID.StartsWith("[Pokémon|S]") == true)
		{
			this.TextureID = this.TextureID.Remove(0, 11);
			texturePath = @"Pokemon\Overworld\Shiny\";
			HasPokemonTexture = true;
		}

		string PokemonAddition = "";

		if (UseTextureID.StartsWith(@"Pokemon\Overworld\") == true)
		{
			texturePath = "";
			HasPokemonTexture = true;
			if (StringHelper.IsNumeric(TextureID) == true)
				PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(System.Convert.ToInt32(TextureID));
		}

		if (UseGameJoltID == true & GameVariables.playerTrainer.IsGameJoltSave == true & GameJolt.API.LoggedIn == true && !GameJolt.Emblem.GetOnlineSprite(GameJoltID) == null)
			this.Texture = GameJolt.Emblem.GetOnlineSprite(GameJoltID);
		else
			this.Texture = P3D.TextureManager.GetTexture(texturePath + this.TextureID + PokemonAddition);

		this.FrameSize = new Vector2(System.Convert.ToInt32(this.Texture.Width / (double)3), System.Convert.ToInt32(this.Texture.Height / (double)4));

		if (HasPokemonTexture == true)
			this.FrameSize = new Vector2(this.FrameSize.x, this.FrameSize.y);
		if (this.Movement == Movements.Pokeball)
			this.FrameSize = new Vector2(32, 32);

		lastRectangle = new Rectangle(0, 0, 0, 0);

		this.ChangeTexture();
	}

	private void ApplyNPCData()
	{
		if (GameVariables.playerTrainer.NPCData != "")
		{
			string[] Data = GameVariables.playerTrainer.NPCData.SplitAtNewline();

			foreach (string line in Data)
			{
				string l = line.Remove(0, 1);
				l = l.Remove(l.Length - 1, 1);

				string file = l.GetSplit(0, "|");
				int ID = System.Convert.ToInt32(l.GetSplit(1, "|"));
				string action = l.GetSplit(2, "|");
				string addition = l.GetSplit(3, "|");

				if (this.NPCID == ID & this.MapOrigin.ToLower() == file.ToLower())
				{
					switch (action.ToLower())
					{
						case "position":
							{
								string[] PositionData = addition.Split(System.Convert.ToChar(","));
								this.Position = new Vector3(System.Convert.ToSingle(PositionData[0].Replace(".", GameController.DecSeparator)) + Offset.x, System.Convert.ToSingle(PositionData[1].Replace(".", GameController.DecSeparator)) + Offset.y, System.Convert.ToSingle(PositionData[2].Replace(".", GameController.DecSeparator)) + Offset.z);
								break;
							}
						case "remove":
							{
								this.CanBeRemoved = true;
								break;
							}
					}
				}
			}
		}
	}

	public static void AddNPCData(string Data)
	{
		Data = "{" + Data + "}";

		if (GameVariables.playerTrainer.NPCData == "")
			GameVariables.playerTrainer.NPCData = Data;
		else
			GameVariables.playerTrainer.NPCData += System.Environment.NewLine + Data;
	}

	public static void RemoveNPCData(string file, int ID, string action, string addition)
	{
		string Data = "{" + file + "|" + ID + "|" + action + "|" + addition + "}";

		string[] NData = GameVariables.playerTrainer.NPCData.SplitAtNewline();
		List<string> nList = NData.ToList();
		if (nList.Contains(Data) == true)
			nList.Remove(Data);
		NData = nList.ToArray();

		Data = "";
		for (var i = 0; i <= NData.Count() - 1; i++)
		{
			if (i != 0)
				Data += System.Environment.NewLine;

			Data += NData[i];
		}

		GameVariables.playerTrainer.NPCData = Data;
	}

	public static void RemoveNPCData(string FullData)
	{
		string[] Data = FullData.Split(System.Convert.ToChar("|"));

		RemoveNPCData(Data[0], System.Convert.ToInt32(Data[1]), Data[2], Data[3]);
	}

	private int GetAnimationX()
	{
		if (this.HasPokemonTexture == true)
		{
			switch (AnimationX)
			{
				case 1:
					{
						return 0;
					}
				case 2:
					{
						return 1;
					}
				case 3:
					{
						return 0;
					}
				case 4:
					{
						return 1;
					}
			}
		}
		switch (AnimationX)
		{
			case 1:
				{
					return 0;
				}
			case 2:
				{
					return 1;
				}
			case 3:
				{
					return 0;
				}
			case 4:
				{
					return 2;
				}
		}
		return 1;
	}

	private void ChangeTexture()
	{
		if (this.Texture != null)
		{
			Rectangle r = new Rectangle(0, 0, 0, 0);
			int cameraRotation = this.getCameraRotation();
			int spriteIndex = this.faceRotation - cameraRotation;

			if (spriteIndex < 0)
				spriteIndex += 4;

			int x = 0;
			if (this.Moved > 0.0F | AnimateIdle == true)
				x = System.Convert.ToInt32(FrameSize.x) * GetAnimationX();

			if (this.Movement == Movements.Pokeball)
			{
				spriteIndex = 0;
				x = 0;
			}

			int y = System.Convert.ToInt32(FrameSize.y) * spriteIndex;

			int xFrameSize = System.Convert.ToInt32(this.FrameSize.x);
			int yFrameSize = System.Convert.ToInt32(this.FrameSize.y);

			if (x < 0)
				x = 0;

			if (y < 0)
				y = 0;

			if (x + xFrameSize > this.Texture.Width)
				xFrameSize = this.Texture.Width - x;

			if (y + yFrameSize > this.Texture.Height)
				yFrameSize = this.Texture.Height - y;

			r = new Rectangle(x, y, xFrameSize, yFrameSize);

			if (r != lastRectangle)
			{
				lastRectangle = r;

				Textures(0) = TextureManager.GetTexture(this.Texture, r, 1);
			}
		}
	}

	public void ActivateScript()
	{
		OverworldScreen oScreen = (OverworldScreen)Core.CurrentScreen;
		if (oScreen.ActionScript.IsReady == true)
		{
			SoundManager.PlaySound("select");
			switch (this.ActionValue)
			{
				case 0:
					{
						oScreen.ActionScript.StartScript(this.AdditionalValue, 1);
						break;
					}
				case 1:
					{
						oScreen.ActionScript.StartScript(this.AdditionalValue, 0);
						break;
					}
				case 3:
					{
						oScreen.ActionScript.StartScript(this.AdditionalValue.Replace("<br>", System.Environment.NewLine), 2);
						break;
					}
				default:
					{
						oScreen.ActionScript.StartScript(this.AdditionalValue, 0);
						break;
					}
			}
		}
	}

	public void CheckInSight()
	{
		if (this.TrainerSight > -1 & GameVariables.Level.PokemonEncounterData.EncounteredPokemon == false & Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
		{
			if (System.Convert.ToInt32(this.Position.y) == System.Convert.ToInt32(Screen.Camera.Position.y) & Screen.Camera.IsMoving() == false)
			{
				if (Moved == 0.0F & this.CanBeRemoved == false)
				{
					if (Screen.Camera.Position.x == System.Convert.ToInt32(this.Position.x) | System.Convert.ToInt32(this.Position.z) == Screen.Camera.Position.z)
					{
						int distance = 0;
						bool correctFacing = false;

						if (Screen.Camera.Position.x == System.Convert.ToInt32(this.Position.x))
						{
							distance = System.Convert.ToInt32(System.Convert.ToInt32(this.Position.z) - Screen.Camera.Position.z);

							if (distance > 0)
							{
								if (this.faceRotation == 0)
									correctFacing = true;
							}
							else if (this.faceRotation == 2)
								correctFacing = true;
						}
						else if (Screen.Camera.Position.z == System.Convert.ToInt32(this.Position.z))
						{
							distance = System.Convert.ToInt32(System.Convert.ToInt32(this.Position.x) - Screen.Camera.Position.x);

							if (distance > 0)
							{
								if (this.faceRotation == 1)
									correctFacing = true;
							}
							else if (this.faceRotation == 3)
								correctFacing = true;
						}

						if (correctFacing == true)
						{
							distance = distance.ToPositive();

							if (distance <= this.TrainerSight)
							{
								string InSightMusic = "nomusic";

								if (this.IsTrainer == true)
								{
									string trainerFilePath = GameModeManager.GetScriptPath(this.AdditionalValue + ".dat");
									//System.Security.FileValidation.CheckFileValid(trainerFilePath, false, "NPC.vb");

									string[] trainerContent = System.IO.File.ReadAllLines(trainerFilePath);
									foreach (string line in trainerContent)
									{
										if (line.StartsWith("@Trainer:") == true)
										{
											string trainerID = line.GetSplit(1, ":");
											if (Trainer.IsBeaten(trainerID) == true)
												return;
											else
											{
												Trainer t = new Trainer(trainerID);
												InSightMusic = t.GetInSightMusic();
											}
										}
										else if (line.ToLower().StartsWith("@battle.starttrainer(") == true)
										{
											string trainerID = line.Remove(line.Length - 1, 1).Remove(0, "@battle.starttrainer(".Length);
											if (Trainer.IsBeaten(trainerID) == true)
												return;
											else
											{
												Trainer t = new Trainer(trainerID);
												InSightMusic = t.GetInSightMusic();
											}
										}
									}
								}

								int needFacing = 0;
								switch (this.faceRotation)
								{
									case 0:
										{
											needFacing = 2;
											break;
										}
									case 1:
										{
											needFacing = 3;
											break;
										}
									case 2:
										{
											needFacing = 0;
											break;
										}
									case 3:
										{
											needFacing = 1;
											break;
										}
								}
								int turns = needFacing - Screen.Camera.GetPlayerFacingDirection();
								if (turns < 0)
									turns = 4 - turns.ToPositive();

								(OverworldScreen)Core.CurrentScreen.TrainerEncountered = true;
								if (InSightMusic != "nomusic" & InSightMusic != "")
									MusicManager.Play(InSightMusic, true, 0.0F);
								Screen.Camera.StopMovement();
								this.Movement = Movements.Still;

								Vector2 offset = new Vector2(0, 0);
								switch (this.faceRotation)
								{
									case 0:
										{
											offset.y = -0.01F;
											break;
										}
									case 1:
										{
											offset.x = -0.01F;
											break;
										}
									case 2:
										{
											offset.y = 0.01F;
											break;
										}
									case 3:
										{
											offset.x = 0.01F;
											break;
										}
								}

								string s = "version=2" + System.Environment.NewLine + "@player.turn(" + turns + ")" + System.Environment.NewLine;

								{
									var withBlock = (OverworldCamera)Screen.Camera;
									if ((OverworldCamera)Screen.Camera.ThirdPerson == true & IsOnScreen() == false)
									{
										s += "@camera.setfocus(npc," + this.NPCID + ")" + System.Environment.NewLine;
										var cPosition = withBlock.ThirdPersonOffset.x.ToString() + "," + withBlock.ThirdPersonOffset.y.ToString() + "," + withBlock.ThirdPersonOffset.z.ToString();
										s += "@entity.showmessagebulb(1|" + this.Position.x + offset.x + "|" + this.Position.y + 0.7F + "|" + this.Position.z + offset.y + ")" + System.Environment.NewLine + "@npc.move(" + this.NPCID + "," + (distance - 1) + ")" + System.Environment.NewLine + "@camera.resetfocus" + System.Environment.NewLine + "@camera.setposition(" + cPosition + ")" + System.Environment.NewLine + "@script.start(" + this.AdditionalValue + ")" + System.Environment.NewLine + ":end";
									}
									else
										s += "@entity.showmessagebulb(1|" + this.Position.x + offset.x + "|" + this.Position.y + 0.7F + "|" + this.Position.z + offset.y + ")" + System.Environment.NewLine + "@npc.move(" + this.NPCID + "," + (distance - 1) + ")" + System.Environment.NewLine + "@script.start(" + this.AdditionalValue + ")" + System.Environment.NewLine + ":end";
								}


								GameVariables.Level.OwnPlayer.Opacity = 0.5F;
								(OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
								ActionScript.IsInsightScript = true;
							}
						}
					}
				}
			}
		}
	}

	public override void ClickFunction()
	{
		int newHeading = Screen.Camera.GetPlayerFacingDirection() - 2;
		if (newHeading < 0)
			newHeading += 4;
		this.faceRotation = newHeading;

		if (this.Moved == 0.0F)
			ActivateScript();
	}

	public override void Update()
	{
		NPCMovement();
		Move();

		ChangeTexture();

		base.Update();
	}

	protected override float CalculateCameraDistance(Vector3 CPosition)
	{
		return base.CalculateCameraDistance(CPosition) - 0.2F;
	}

	public override void UpdateEntity()
	{
		if (this.Rotation.y != Screen.Camera.Yaw)
			this.Rotation.y = Screen.Camera.Yaw;

		base.UpdateEntity();
	}

	public override void Render()
	{
		var state = GraphicsDevice.DepthStencilState;
		GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
		Draw(this.Model, this.Textures, true);
		GraphicsDevice.DepthStencilState = state;
	}

	private void NPCMovement()
	{
		if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
		{
			if ((OverworldScreen)Core.CurrentScreen.ActionScript.IsReady == false)
				return;
		}
		switch (this.Movement)
		{
			case Movements.Still:
			case Movements.Pokeball:
				{
					break;
				}
			case Movements.Turning:
				{
					if (this.TurningDelay > 0.0F)
					{
						TurningDelay -= 0.1F;
						if (TurningDelay <= 0.0F)
						{
							this.TurningDelay = 3.0F;

							this.faceRotation += 1;
							if (this.faceRotation == 4)
								this.faceRotation = 0;
							if (this.IsTrainer == true)
								CheckInSight();
						}
					}

					break;
				}
			case Movements.Looking:
				{
					if (this.Moved == 0.0F)
					{
						if (Settings.Rand.Next(0, 220) == 0)
						{
							int newRotation = this.faceRotation;
							while (newRotation == this.faceRotation)
								newRotation = Settings.Rand.Next(0, 4);
							this.faceRotation = newRotation;
							if (this.IsTrainer == true)
								CheckInSight();
						}
					}

					break;
				}
			case Movements.FacePlayer:
				{
					if (this.Moved == 0.0F)
					{
						int oldRotation = this.faceRotation;

						if (Screen.Camera.Position.x == this.Position.x | Screen.Camera.Position.z == this.Position.z)
						{
							if (this.Position.x < Screen.Camera.Position.x)
								this.faceRotation = 3;
							else if (this.Position.x > Screen.Camera.Position.x)
								this.faceRotation = 1;
							if (this.Position.z < Screen.Camera.Position.z)
								this.faceRotation = 2;
							else if (this.Position.z > Screen.Camera.Position.z)
								this.faceRotation = 0;
						}

						if (oldRotation != this.faceRotation & this.IsTrainer == true)
							CheckInSight();
					}

					break;
				}
			case Movements.Walk:
				{
					if (this.Moved == 0.0F)
					{
						if (Settings.Rand.Next(0, 120) == 0)
						{
							if (Settings.Rand.Next(0, 3) == 0)
							{
								int newRotation = this.faceRotation;
								while (newRotation == this.faceRotation)
									newRotation = Settings.Rand.Next(0, 4);
								this.faceRotation = newRotation;
							}
							bool contains = false;
							Vector3 newPosition = (GetMove() / (double)Speed) + this.Position;
							if (CheckCollision(newPosition) == true)
							{
								foreach (Rectangle r in this.MoveRectangles)
								{
									if (r.Contains(new Point(System.Convert.ToInt32(newPosition.x), System.Convert.ToInt32(newPosition.z))) == true)
									{
										contains = true;
										break;
									}
								}
								if (contains == true)
									Moved = 1.0F;
							}
						}
					}

					break;
				}
			case Movements.Straight:
				{
					if (this.Moved == 0.0F)
					{
						if (Settings.Rand.Next(0, 15) == 0)
						{
							int newRotation = this.faceRotation;
							while (newRotation == this.faceRotation)
								newRotation = Settings.Rand.Next(0, 4);
							this.faceRotation = newRotation;
						}
						bool contains = false;
						Vector3 newPosition = (GetMove() / (double)Speed) + this.Position;
						if (CheckCollision(newPosition) == true)
						{
							foreach (Rectangle r in this.MoveRectangles)
							{
								if (r.Contains(new Point(System.Convert.ToInt32(newPosition.x), System.Convert.ToInt32(newPosition.z))) == true)
								{
									contains = true;
									break;
								}
							}
							if (contains == true)
								Moved = 1.0F;
						}
					}

					break;
				}
		}
	}

	private bool CheckCollision(Vector3 newPosition)
	{
		newPosition = new Vector3(System.Convert.ToInt32(newPosition.x), System.Convert.ToInt32(newPosition.y), System.Convert.ToInt32(newPosition.z));

		bool interactPlayer = true;

		if (Screen.Camera.IsMoving() == false)
		{
			if (System.Convert.ToInt32(Screen.Camera.Position.x) != newPosition.x | System.Convert.ToInt32(Screen.Camera.Position.z) != newPosition.z)
			{
				if (System.Convert.ToInt32(GameVariables.Level.OverworldPokemon.Position.x) != newPosition.x | System.Convert.ToInt32(GameVariables.Level.OverworldPokemon.Position.z) != newPosition.z)
					interactPlayer = false;
			}
		}

		if (interactPlayer == true)
			return false;

		bool HasFloor = false;

		Vector3 Position2D = new Vector3(newPosition.x, newPosition.y - 0.1F, newPosition.z);
		foreach (Entity Floor in GameVariables.Level.Floors)
		{
			if (Floor.boundingBox.Contains(Position2D) == ContainmentType.Contains)
				HasFloor = true;
		}

		if (HasFloor == false)
			return false;

		foreach (Entity Entity in GameVariables.Level.Entities)
		{
			if (Entity.boundingBox.Contains(newPosition) == ContainmentType.Contains)
			{
				if (Entity.Collision == true)
					return false;
			}
		}

		return true;
	}

	private void Move()
	{
		if (Moved > 0.0F)
		{
			if (!isDancing)
				this.Position += GetMove();

			if (this.Speed < 0)
				Moved += this.Speed;
			else
				Moved -= this.Speed;

			if (this.MoveY < 0.0F)
			{
				this.MoveY += this.Speed;
				if (MoveY >= 0.0F)
					this.MoveY = 0.0F;
			}
			else if (this.MoveY > 0.0F)
			{
				this.MoveY -= this.Speed;
				if (MoveY <= 0.0F)
					this.MoveY = 0.0F;
			}

			this.AnimationDelay -= System.Convert.ToSingle(0.13 * (Math.Abs(this.Speed) / (double)NPC.STANDARD_SPEED));
			if (AnimationDelay <= 0.0F)
			{
				AnimationDelay = AnimationDelayLenght;
				AnimationX += 1;
				if (AnimationX > 4)
					AnimationX = 1;
			}

			if (Moved <= 0.0F)
			{
				MoveAsync = false;
				Moved = 0.0F;
				MoveY = 0.0F;
				AnimationX = 1;
				AnimationDelay = AnimationDelayLenght;
				this.Position = new Vector3(System.Convert.ToInt32(this.Position.x), System.Convert.ToInt32(this.Position.y), System.Convert.ToInt32(this.Position.z));
				ChangeTexture();
				ApplyShaders();
				Speed = NPC.STANDARD_SPEED;
			}
		}
		else if (this.AnimateIdle == true)
		{
			this.AnimationDelay -= 0.1F;
			if (AnimationDelay <= 0.0F)
			{
				AnimationDelay = AnimationDelayLenght;
				AnimationX += 1;
				if (AnimationX > 4)
					AnimationX = 1;
			}
		}
	}

	private Vector3 GetMove()
	{
		Vector3 moveVector = new Vector3();
		switch (this.faceRotation)
		{
			case 0:
				{
					moveVector = new Vector3(0, 0, -1) * Speed;
					break;
				}
			case 1:
				{
					moveVector = new Vector3(-1, 0, 0) * Speed;
					break;
				}
			case 2:
				{
					moveVector = new Vector3(0, 0, 1) * Speed;
					break;
				}
			case 3:
				{
					moveVector = new Vector3(1, 0, 0) * Speed;
					break;
				}
		}
		if (MoveY != 0.0F)
		{
			float multi = this.Speed;
			if (multi < 0.0F)
				multi *= -1;
			if (MoveY > 0)
				moveVector.y = multi * 1;
			else
				moveVector.y = multi * -1;
		}
		return moveVector;
	}

	private int getCameraRotation()
	{
		int cameraRotation = 0;
		Camera c = Screen.Camera;

		float Yaw = c.Yaw;

		while (Yaw < 0)
			Yaw += MathHelper.TwoPi;

		if (Yaw <= MathHelper.Pi * 0.25F | Yaw > MathHelper.Pi * 1.75F)
			cameraRotation = 0;
		if (Yaw <= MathHelper.Pi * 0.75F & Yaw > MathHelper.Pi * 0.25F)
			cameraRotation = 1;
		if (Yaw <= MathHelper.Pi * 1.25F & Yaw > MathHelper.Pi * 0.75F)
			cameraRotation = 2;
		if (Yaw <= MathHelper.Pi * 1.75F & Yaw > MathHelper.Pi * 1.25F)
			cameraRotation = 3;

		return cameraRotation;
	}

	private void ApplyShaders()
	{
		this.Shaders.Clear();
		foreach (Shader Shader in GameVariables.Level.Shaders)
			Shader.ApplyShader(this);
	}

	internal bool InCameraFocus()
	{
		if (Screen.Camera.Name == "Overworld")
		{
			var c = (OverworldCamera)Screen.Camera;

			if (c.CameraFocusType == OverworldCamera.CameraFocusTypes.NPC)
			{
				if (c.CameraFocusID == this.NPCID)
					return true;
			}
		}
		return false;
	}

	private bool IsOnScreen()
	{
		return Screen.Camera.BoundingFrustum.Contains(this.Position) != ContainmentType.Disjoint;
	}
}
}
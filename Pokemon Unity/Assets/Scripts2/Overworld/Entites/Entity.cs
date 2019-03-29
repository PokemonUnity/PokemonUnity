using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Overworld.Entity.Misc;
using PokemonUnity.Overworld.Entity.Environment;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity
{
public class Entity : BaseEntity
{
	public static bool MakeShake = false;
	public static bool drawViewBox = false;

	public int ID = -1;

	public Entities EntityID;// = null;
	public string MapOrigin = "";
	public bool IsOffsetMapContent = false;
	public UnityEngine.Vector3 Offset = new UnityEngine.Vector3(0,0,0);
	public UnityEngine.Vector3 Position;
	public UnityEngine.Vector3 Rotation = new UnityEngine.Vector3(0,0,0);
	public UnityEngine.Vector3 Scale = new UnityEngine.Vector3(1,1,1);
	public Texture2D[] Textures;
	public int[] TextureIndex;
	public int ActionValue;
	public string AdditionalValue;
	public UnityEngine.Mesh Model;
	public bool Visible = true;
	public UnityEngine.Vector3 Shader = new UnityEngine.Vector3(1.0f,1,1);
	public List<UnityEngine.Vector3> Shaders = new List<UnityEngine.Vector3>();

	public float CameraDistanceDelta = 0.0f;

	public string SeasonColorTexture = "";

	public int FaceDirection = 0;
	public float Moved = 0.0f;
	public float Speed = 0.04f;
	public bool CanMove = false;
	public bool isDancing = false;

	public float Opacity = 1.0f;
	private float _normalOpactity = 1.0f;
	public float NormalOpacity
	{
		get
		{
			return this._normalOpactity;
		}
		set
		{
			this.Opacity = value;
			this._normalOpactity = value;
		}
	}

	public UnityEngine.Vector3 boundingBoxScale = new UnityEngine.Vector3(1.25f,1.25f,1.25f);
	public UnityEngine.Bounds boundingBox;

	public UnityEngine.Bounds ViewBox;
	public UnityEngine.Vector3 viewBoxScale = new UnityEngine.Vector3(1.0f,1,1);

	public float CameraDistance;
	public Matrix World;
	public bool CreatedWorld = false;
	public bool CreateWorldEveryFrame = false;

	public bool Collision = true;

	public bool CanBeRemoved = false;
	public bool NeedsUpdate = false;

	//private static RasterizerState newRasterizerState;
	//private static RasterizerState oldRasterizerState;

	private UnityEngine.Vector3 BoundingPositionCreated = new UnityEngine.Vector3(1110,1110,1110);
	private UnityEngine.Vector3 BoundingRotationCreated = new UnityEngine.Vector3(-1,-1,-1);

	public int HasEqualTextures = -1;

	private bool DrawnLastFrame = true;
	protected bool DropUpdateUnlessDrawn = true;

	public Entity() : base(EntityTypes.Entity)
	{
	}

	public Entity(float X, float Y, float Z, Entities EntityID, Texture2D[] Textures, int[] TextureIndex, bool Collision, int Rotation, UnityEngine.Vector3 Scale, UnityEngine.Mesh Model, int ActionValue, string AdditionalValue, UnityEngine.Vector3 Shader) : base(EntityTypes.Entity)
	{
		this.Position = new UnityEngine.Vector3(X, Y, Z);
		this.EntityID = EntityID;
		this.Textures = Textures;
		this.TextureIndex = TextureIndex;
		this.Collision = Collision;
		this.Rotation = GetRotationFromInteger(Rotation);
		this.Scale = Scale;
		this.Model = Model;
		this.ActionValue = ActionValue;
		this.AdditionalValue = AdditionalValue;
		this.Shader = Shader;

		Initialize();
	}

	public virtual void Initialize()
	{
		//if (GetRotationFromVector(this.Rotation) % 2 == 1)
		//	ViewBox = new UnityEngine.Bounds(
		//		new UnityEngine.Vector3(-(this.Scale.z / (float)2), -(this.Scale.y / (float)2), -(this.Scale.x / (float)2)), 
		//		UnityEngine.Vector3.Dot(
		//			UnityEngine.Vector3.Scale(viewBoxScale), 
		//			Matrix.CreateTranslation(Position)
		//		), 
		//		//UnityEngine.Vector3.Transform(
		//			new UnityEngine.Vector3((this.Scale.z / (float)2), (this.Scale.y / (float)2), (this.Scale.x / (float)2)), 
		//			Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position));
		//else																														  																																																					  
		//	ViewBox = new UnityEngine.Bounds(
		//		//UnityEngine.Vector3.Transform(
		//			new UnityEngine.Vector3(-(this.Scale.x / (float)2), -(this.Scale.y / (float)2), -(this.Scale.z / (float)2)), 
		//			Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position), 
		//		//UnityEngine.Vector3.Transform(
		//			new UnityEngine.Vector3((this.Scale.x / (float)2), (this.Scale.y / (float)2), (this.Scale.z / (float)2)), 
		//			Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position));
		//
		//boundingBox = new UnityEngine.Bounds(
		//	//UnityEngine.Vector3.Transform(
		//		new UnityEngine.Vector3(-0.5f), 
		//		Matrix.CreateScale(boundingBoxScale) * Matrix.CreateTranslation(Position), 
		//	//UnityEngine.Vector3.Transform(
		//		new UnityEngine.Vector3(0.5f, 0.5f, 0.5f), 
		//		Matrix.CreateScale(boundingBoxScale) * Matrix.CreateTranslation(Position));

		this.BoundingPositionCreated = this.Position;
		this.BoundingRotationCreated = this.Rotation;

		//if (newRasterizerState == null)
		//{
		//	newRasterizerState = new RasterizerState();
		//	oldRasterizerState = new RasterizerState();
		//
		//	newRasterizerState.CullMode = CullMode.None;
		//	oldRasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
		//}

		this.LoadSeasonTextures();

		this.UpdateEntity();
	}

	public static Entity GetNewEntity(Entities EntityID, UnityEngine.Vector3 Position, Texture2D[] Textures, int[] TextureIndex, bool Collision, UnityEngine.Vector3 Rotation, UnityEngine.Vector3 Scale, UnityEngine.Mesh Model, int ActionValue, string AdditionalValue, bool Visible, UnityEngine.Vector3 Shader, int ID, string MapOrigin, string SeasonColorTexture, UnityEngine.Vector3 Offset, object[] Params = null, float Opacity = 1.0f, List<List<int>> AnimationData = null, float CameraDistanceDelta = 0.0f)
	{
		Entity newEnt = new Entity();
		Entity propertiesEnt = new Entity();

		propertiesEnt.EntityID = EntityID;
		propertiesEnt.Position = Position;
		propertiesEnt.Textures = Textures;
		propertiesEnt.TextureIndex = TextureIndex;
		propertiesEnt.Collision = Collision;
		propertiesEnt.Rotation = Rotation;
		propertiesEnt.Scale = Scale;
		propertiesEnt.Model = Model;
		propertiesEnt.ActionValue = ActionValue;
		propertiesEnt.AdditionalValue = AdditionalValue;
		propertiesEnt.Visible = Visible;
		propertiesEnt.Shader = Shader;
		propertiesEnt.NormalOpacity = Opacity;

		propertiesEnt.ID = ID;
		propertiesEnt.MapOrigin = MapOrigin;
		propertiesEnt.SeasonColorTexture = SeasonColorTexture;
		propertiesEnt.Offset = Offset;
		propertiesEnt.CameraDistanceDelta = CameraDistanceDelta;

		switch (EntityID)
		{
			case Entities.AnimatedBlock:// "animatedblock":
				{
					newEnt = new AnimatedBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((AnimatedBlock)newEnt).Initialize(AnimationData);
					break;
				}
			case Entities.WallBlock:
				{
					newEnt = new WallBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((WallBlock)newEnt).Initialize();
					break;
				}
			case Entities.Cube:
			case Entities.AllSidesObject:
				{
					newEnt = new AllSidesObject();
					SetProperties(ref newEnt, propertiesEnt);
					((AllSidesObject)newEnt).Initialize();
					break;
				}
			case Entities.SlideBlock:
				{
					newEnt = new SlideBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((SlideBlock)newEnt).Initialize();
					break;
				}
			case Entities.WallBill:
				{
					newEnt = new WallBill();
					SetProperties(ref newEnt, propertiesEnt);
					((WallBill)newEnt).Initialize();
					break;
				}
			case Entities.SignBlock:
				{
					newEnt = new SignBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((SignBlock)newEnt).Initialize();
					break;
				}
			case Entities.WarpBlock:
				{
					newEnt = new WarpBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((WarpBlock)newEnt).Initialize();
					break;
				}
			case Entities.Floor:
				{
					newEnt = new Floor();
					SetProperties(ref newEnt, propertiesEnt);
					((Floor)newEnt).Initialize(true, false, true);
					break;
				}
			case Entities.Step:
				{
					newEnt = new StepBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((StepBlock)newEnt).Initialize();
					break;
				}
			case Entities.CutTree:
				{
					newEnt = new CutDownTree();
					SetProperties(ref newEnt, propertiesEnt);
					((CutDownTree)newEnt).Initialize();
					break;
				}
			case Entities.Water:
				{
					newEnt = new Water();
					SetProperties(ref newEnt, propertiesEnt);
					((Water)newEnt).Initialize();
					break;
				}
			case Entities.Grass:
				{
					newEnt = new Grass();
					SetProperties(ref newEnt, propertiesEnt);
					((Grass)newEnt).Initialize();
					break;
				}
			case Entities.BerryPlant://"berryplant":
				{
					newEnt = new BerryPlant();
					SetProperties(ref newEnt, propertiesEnt);
					((BerryPlant)newEnt).Initialize();
					break;
				}
			case Entities.LoamySoil:
				{
					newEnt = new LoamySoil();
					SetProperties(ref newEnt, propertiesEnt);
					((LoamySoil)newEnt).Initialize();
					break;
				}
			case Entities.ItemObject:
				{
					newEnt = new ItemObject();
					SetProperties(ref newEnt, propertiesEnt);
					((ItemObject)newEnt).Initialize();
					break;
				}
			case Entities.ScriptBlock:
				{
					newEnt = new ScriptBlock();
					SetProperties(ref newEnt, propertiesEnt);
					((ScriptBlock)newEnt).Initialize();
					break;
				}
			case Entities.TurningSign:
				{
					newEnt = new TurningSign();
					SetProperties(ref newEnt, propertiesEnt);
					((TurningSign)newEnt).Initialize();
					break;
				}
			case Entities.ApricornPlant:
				{
					newEnt = new ApricornPlant();
					SetProperties(ref newEnt, propertiesEnt);
					((ApricornPlant)newEnt).Initialize();
					break;
				}
			case Entities.HeadbuttTree:
				{
					newEnt = new HeadbuttTree();
					SetProperties(ref newEnt, propertiesEnt);
					((HeadbuttTree)newEnt).Initialize();
					break;
				}
			case Entities.SmashRock:
				{
					newEnt = new SmashRock();
					SetProperties(ref newEnt, propertiesEnt);
					((SmashRock)newEnt).Initialize();
					break;
				}
			case Entities.StrengthRock:
				{
					newEnt = new StrengthRock();
					SetProperties(ref newEnt, propertiesEnt);
					((StrengthRock)newEnt).Initialize();
					break;
				}
			case Entities.NPC:
				{
					newEnt = new NPC();
					SetProperties(ref newEnt, propertiesEnt);
					((NPC)newEnt).Initialize(
						System.Convert.ToString(Params[0]), 
						System.Convert.ToInt32(Params[1]), 
						System.Convert.ToString(Params[2]), 
						System.Convert.ToInt32(Params[3]), 
						System.Convert.ToBoolean(Params[4]), 
						System.Convert.ToString(Params[5]), 
						(List<Rectangle>)Params[6]);
					break;
				}
			case Entities.Waterfall:
				{
					newEnt = new Waterfall();
					SetProperties(ref newEnt, propertiesEnt);
					((Waterfall)newEnt).Initialize();
					break;
				}
			case Entities.Whirlpool:
				{
					newEnt = new Whirlpool();
					SetProperties(ref newEnt, propertiesEnt);
					((Whirlpool)newEnt).Initialize();
					break;
				}
			case Entities.StrengthTrigger:
				{
					newEnt = new StrengthTrigger();
					SetProperties(ref newEnt, propertiesEnt);
					((StrengthTrigger)newEnt).Initialize();
					break;
				}
			case Entities.ModelEntity://"modelentity":
				{
					newEnt = new ModelEntity();
					SetProperties(ref newEnt, propertiesEnt);
					((ModelEntity)newEnt).Initialize();
					break;
				}
			case Entities.RotationTile://"rotationtile":
				{
					newEnt = new RotationTile();
					SetProperties(ref newEnt, propertiesEnt);
					((RotationTile)newEnt).Initialize();
					break;
				}
			case Entities.DiveTile://"divetile":
				{
					newEnt = new DiveTile();
					SetProperties(ref newEnt, propertiesEnt);
					((DiveTile)newEnt).Initialize();
					break;
				}
			case Entities.RockClimbEntity://"rockclimbentity":
				{
					newEnt = new RockClimbEntity();
					SetProperties(ref newEnt, propertiesEnt);
					((RockClimbEntity)newEnt).Initialize();
					break;
				}
		}

		return newEnt;
	}

	internal static void SetProperties(ref Entity newEnt, Entity PropertiesEnt)
	{
		newEnt.EntityID = PropertiesEnt.EntityID;
		newEnt.Position = PropertiesEnt.Position;
		newEnt.Textures = PropertiesEnt.Textures;
		newEnt.TextureIndex = PropertiesEnt.TextureIndex;
		newEnt.Collision = PropertiesEnt.Collision;
		newEnt.Rotation = PropertiesEnt.Rotation;
		newEnt.Scale = PropertiesEnt.Scale;
		newEnt.Model = PropertiesEnt.Model;
		newEnt.ActionValue = PropertiesEnt.ActionValue;
		newEnt.AdditionalValue = PropertiesEnt.AdditionalValue;
		newEnt.Visible = PropertiesEnt.Visible;
		newEnt.Shader = PropertiesEnt.Shader;
		newEnt.ID = PropertiesEnt.ID;
		newEnt.MapOrigin = PropertiesEnt.MapOrigin;
		newEnt.SeasonColorTexture = PropertiesEnt.SeasonColorTexture;
		newEnt.Offset = PropertiesEnt.Offset;
		newEnt.NormalOpacity = PropertiesEnt.Opacity;
		newEnt.CameraDistanceDelta = PropertiesEnt.CameraDistanceDelta;
	}

	public static UnityEngine.Vector3 GetRotationFromInteger(int i)
	{
		switch (i)
		{
			case 0:
				{
					return new UnityEngine.Vector3(0, 0, 0);
				}
			case 1:
				{
					return new UnityEngine.Vector3(0, MathHelper.PiOver2, 0);
				}
			case 2:
				{
					return new UnityEngine.Vector3(0, MathHelper.Pi, 0);
				}
			case 3:
				{
					return new UnityEngine.Vector3(0, MathHelper.Pi * 1.5f, 0);
				}
		}
			return new UnityEngine.Vector3(0, 0, 0);
	}

	public static int GetRotationFromVector(UnityEngine.Vector3 v)
	{
		switch (v.y.ToString())
		{
			case "0":
				{
					return 0;
				}
			case MathHelper.PiOver2:
                {
					return 1;
				}
			case MathHelper.Pi:
                {
					return 2;
				}
			case MathHelper.Pi * 1.5f:
                {
					return 3;
				}
		}

		return 0;
	}

	protected internal void LoadSeasonTextures()
	{
		if (SeasonColorTexture != "")
		{
			List<Texture2D> newTextures = new List<Texture2D>();
			foreach (Texture2D t in Textures)
				newTextures.Add(PokemonUnity.Overworld.World.GetSeasonTexture(TextureManager.GetTexture(@"Textures\Seasons\" + this.SeasonColorTexture), t));
			this.Textures = newTextures.ToArray();
		}
	}

	public virtual void Update()
	{
	}

	public virtual void OpacityCheck()
	{
		if (this.CameraDistance > 10.0f | GameVariables.Level.OwnPlayer != null && CameraDistance > GameVariables.Level.OwnPlayer.CameraDistance)
		{
			this.Opacity = this._normalOpactity;
			return;
		}

		string[] notNames = new string[] { "Floor", "OwnPlayer", "Water", "Whirlpool", "Particle", "OverworldPokemon", "ItemObject", "NetworkPokemon", "NetworkPlayer" };
		if (GameVariables.Camera.Name == "Overworld" && !notNames.ToList().Contains(this.EntityID.ToString()))
		{
			this.Opacity = this._normalOpactity;
			if (((OverworldCamera)GameVariables.Camera).ThirdPerson)
			{
				UnityEngine.Ray Ray = GameVariables.Camera.Ray;
				//float? result = Ray.Intersects(this.boundingBox);
				UnityEngine.RaycastHit result;// = new UnityEngine.RaycastHit(). Ray.Intersects(this.boundingBox);
				//if (result.HasValue)
				if (UnityEngine.Physics.Raycast(Ray, out result))
				{
					//if (result.Value < 0.3f + ((OverworldCamera)GameVariables.Camera.ThirdPersonOffset.z - 1.5f))
					if (result.distance < 0.3f + (((OverworldCamera)GameVariables.Camera).ThirdPersonOffset.z - 1.5f))
					{
						this.Opacity = this._normalOpactity - 0.5f;
						if (this.Opacity < 0.3f)
							this.Opacity = 0.3f;
					}
				}
			}
		}
	}

	protected virtual UnityEngine.Vector3 GetCameraDistanceCenterPoint()
	{
		return this.Position + this.GetCenter();
	}

	protected virtual float CalculateCameraDistance(UnityEngine.Vector3 CPosition)
	{
		return UnityEngine.Vector3.Distance(this.GetCameraDistanceCenterPoint(), CPosition) + CameraDistanceDelta;
	}

	public virtual void UpdateEntity()
	{
		//UnityEngine.Vector3 CPosition = GameVariables.Camera.Position;
		//bool ActionScriptActive = false;
		//
		//
		//if (Core.CurrentScreen != null)
		//{
		//	CPosition = GameVariables.Camera.CPosition;
		//	if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
		//		ActionScriptActive = !(OverworldScreen)Core.CurrentScreen.ActionScript.IsReady;
		//}
		//
		//
		//CameraDistance = CalculateCameraDistance(CPosition);
		//
		//if (this.DropUpdateUnlessDrawn & !this.DrawnLastFrame & this.Visible & !ActionScriptActive)
		//	return;
		//
		//
		//if (this.Moved > 0.0f & this.CanMove)
		//{
		//	this.Moved -= this.Speed;
		//
		//	UnityEngine.Vector3 movement = UnityEngine.Vector3.zero;
		//	switch (this.FaceDirection)
		//	{
		//		case 0:
		//			{
		//				movement = new UnityEngine.Vector3(0, 0, -1);
		//				break;
		//			}
		//		case 1:
		//			{
		//				movement = new UnityEngine.Vector3(-1, 0, 0);
		//				break;
		//			}
		//		case 2:
		//			{
		//				movement = new UnityEngine.Vector3(0, 0, 1);
		//				break;
		//			}
		//		case 3:
		//			{
		//				movement = new UnityEngine.Vector3(1, 0, 0);
		//				break;
		//			}
		//	}
		//
		//	movement *= Speed;
		//
		//	this.Position += movement;
		//	this.CreatedWorld = false;
		//
		//	if (this.Moved <= 0.0f)
		//	{
		//		this.Moved = 0.0f;
		//
		//		this.Position.x = System.Convert.ToInt32(this.Position.x);
		//		this.Position.z = System.Convert.ToInt32(this.Position.z);
		//	}
		//}
		//
		//if (!this.IsOffsetMapContent)
		//	OpacityCheck();
		//
		//if (!CreatedWorld | CreateWorldEveryFrame)
		//{
		//	World = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.y, Rotation.x, Rotation.z) * Matrix.CreateTranslation(Position);
		//	CreatedWorld = true;
		//}
		//
		//if (CameraDistance < GameVariables.Camera.FarPlane * 2)
		//{
		//	if (this.Position != this.BoundingPositionCreated)
		//	{
		//		List<float> diff = new List<float>();
		//		diff.AddRange(new float[]
		//		{
		//			this.BoundingPositionCreated.x - this.Position.x,
        //            this.BoundingPositionCreated.y - this.Position.y,
        //            this.BoundingPositionCreated.z - this.Position.z
		//
		//		});
		//
		//		ViewBox.min.x -= diff[0];
		//		ViewBox.min.y -= diff[1];
		//		ViewBox.min.z -= diff[2];
		//				
		//		ViewBox.max.x -= diff[0];
		//		ViewBox.max.y -= diff[1];
		//		ViewBox.max.z -= diff[2];
		//
		//		boundingBox.min.x -= diff[0];
		//		boundingBox.min.y -= diff[1];
		//		boundingBox.min.z -= diff[2];
		//					
		//		boundingBox.max.x -= diff[0];
		//		boundingBox.max.y -= diff[1];
		//		boundingBox.max.z -= diff[2];
		//
		//		this.BoundingPositionCreated = this.Position;
		//	}
		//}
		//
		//if (MakeShake)
		//{
		//	if (Settings.Rand.Next(0, 1) == 0)
		//	{
		//		this.Rotation.x += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//		this.Rotation.z += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//		this.Rotation.y += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//
		//		this.Position.x += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//		this.Position.z += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//		this.Position.y += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//
		//		this.Scale.x += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//		this.Scale.z += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//		this.Scale.y += System.Convert.ToSingle((Settings.Rand.Next(1, 6) - 3) / (double)100);
		//
		//		CreatedWorld = false;
		//	}
		//}
		//
		//if (GameVariables.Level.World != null)
		//{
		//	switch (GameVariables.Level.World.EnvironmentType)
		//	{
		//		case World.EnvironmentTypes.Outside:
        //            {
		//				this.Shader = SkyDome.GetDaytimeColor(true).ToVector3();
		//				break;
		//			}
		//		case World.EnvironmentTypes.Dark:
        //            {
		//				this.Shader = new UnityEngine.Vector3(0.5f, 0.5f, 0.6f);
		//				break;
		//			}
		//		default:
		//			{
		//				this.Shader = new UnityEngine.Vector3(1.0f,1f,1f);
		//				break;
		//			}
		//	}
		//}
		//
		//foreach (UnityEngine.Vector3 s in this.Shaders)
		//	this.Shader = UnityEngine.Vector3.Dot(this.Shader, s);
	}

	private UnityEngine.Vector3 tempCenterVector = UnityEngine.Vector3.zero;

	/// <summary>
	/// Returns the offset from the 0,0,0 center of the position of the entity.
	/// </summary>
	private UnityEngine.Vector3 GetCenter()
	{
		if (!CreatedWorld | CreateWorldEveryFrame)
		{
			UnityEngine.Vector3 v = UnityEngine.Vector3.zero; // (Me.ViewBox.Min - Me.Position) + (Me.ViewBox.Max - Me.Position)

			if (this.Model != null)
			{
				//switch (this.Model.ID)
				//{
				//	case 0:
				//	case 9:
				//	case 10:
				//	case 11:
				//		{
				//			v.y -= 0.5f;
				//			break;
				//		}
				//}
			}
			this.tempCenterVector = v;
		}

		return this.tempCenterVector;
	}

	public virtual void Draw(UnityEngine.Mesh Model, Texture2D[] Textures, bool setRasterizerState)
	{
		if (Visible)
		{
			if (this.IsInFieldOfView())
			{
				//if (setRasterizerState)
				//	Core.GraphicsDevice.RasterizerState = newRasterizerState;

				//Model.Draw(this, Textures);

				//if (setRasterizerState)
				//	Core.GraphicsDevice.RasterizerState = oldRasterizerState;

				this.DrawnLastFrame = true;

				if (this.EntityID != Entities.Floor & this.EntityID != Entities.Water)
				{
					//if (drawViewBox)
					//	BoundingBoxRenderer.Render(ViewBox, Core.GraphicsDevice, GameVariables.Camera.View, GameVariables.Camera.Projection, new UnityEngine.Color(240,128,128));
				}
			}
			else
				this.DrawnLastFrame = false;
		}
		else
			this.DrawnLastFrame = false;
	}

	public virtual void Render()
	{
	}

	public virtual void ClickFunction()
	{
	}

	public virtual bool WalkAgainstFunction()
	{
		return true;
	}

	public virtual bool WalkIntoFunction()
	{
		return false;
	}

	public virtual void WalkOntoFunction()
	{
	}

	public virtual void ResultFunction(int Result)
	{
	}

	public virtual bool LetPlayerMove()
	{
		return true;
	}

	public bool _visibleLastFrame = false;
	public bool _occluded = false;

	public bool IsInFieldOfView()
	{
		if (GameVariables.Camera.BoundingFrustum.Contains(this.ViewBox))// != ContainmentType.Disjoint
		{
			this._visibleLastFrame = true;
			return true;
		}
		else
		{
			this._visibleLastFrame = false;
			return false;
		}
	}

	private int _cachedVertexCount = -1; // Stores the vertex count so it doesnt need to be recalculated.

	public int VertexCount
	{
		get
		{
			if (this._cachedVertexCount == -1)
			{
				if (this.Model != null)
				{
					int c = System.Convert.ToInt32(this.Model.vertexCount / (double)3);
					int min = 0;

					for (var i = 0; i <= this.TextureIndex.Length - 1; i++)
					{
						if (i <= c - 1)
						{
							if (TextureIndex[i] > -1)
								min += 1;
						}
					}

					this._cachedVertexCount = min;
				}
				else
					this._cachedVertexCount = 0;
			}
			return this._cachedVertexCount;
		}
	}

	protected Entity GetEntity(List<Entity> List, UnityEngine.Vector3 Position, bool IntComparison, Type[] validEntitytypes)
	{
		foreach (Entity e in (from selEnt in List
							  where validEntitytypes.Contains(selEnt.GetType())
							  select selEnt))
		{
			if (IntComparison)
			{
				if ((int)e.Position.x == (int)Position.x & (int)e.Position.y == (int)Position.y & (int)e.Position.z == (int)Position.z)
					return e;
			}
			else if (e.Position.x == Position.x & e.Position.y == Position.y & e.Position.z == Position.z)
				return e;
		}
		return null;
	}
}
}
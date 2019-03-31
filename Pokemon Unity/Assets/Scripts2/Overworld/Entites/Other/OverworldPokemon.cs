using System;
using System.Linq;
using System.Runtime.CompilerServices;
using PokemonUnity.Monster;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Misc
{
	public class OverworldPokemon : Entity
	{
		public OverworldPokemon(float X, float Y, float Z) : base(X, Y, Z, Entities.OverworldPokemon, new Texture2D[] { TextureManager.DefaultTexture },
			new int[]{
			0,
			0
			}, false, 0, new Vector3(1.0f, 1, 1)/*, UnityEngine.Mesh.BillModel*/, 0, "", new Vector3(1, 1, 1))
		{
			PokemonReference = null/* TODO Change to default(_) if this is not a reference type */;
			this.Respawn();
			if (Game.playerTrainer.LastPokemonPosition == new Vector3(999, 999, 999))
			{
				this.Position = new Vector3(Game.Camera.Position.x, Game.Camera.Position.y, Game.Camera.Position.z);
				this.Visible = false;
				this.warped = false;
			}
			else
				this.Position = Game.playerTrainer.LastPokemonPosition;

			this.Position = new Vector3(System.Convert.ToInt32(this.Position.x), this.GetYPosition(), System.Convert.ToInt32(this.Position.z));
			this.NeedsUpdate = true;
			this.CreateWorldEveryFrame = true;

			this.DropUpdateUnlessDrawn = false;

		}

		//public int PokemonID = 0;
		public Pokemons PokemonID = Pokemons.NONE;
		private Monster.Pokemon _PokemonReference;

		public Monster.Pokemon PokemonReference
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get
			{
				return _PokemonReference;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				//if (_PokemonReference != null)
				//{
				//	_PokemonReference.TexturesCleared -= PokemonReference_TexturesCleared;
				//}
				//
				_PokemonReference = value;
				//if (_PokemonReference != null)
				//{
				//	_PokemonReference.TexturesCleared += PokemonReference_TexturesCleared;
				//}
			}
		}

		public Texture2D Texture;
		private Vector4 lastRectangle = new Vector4(0, 0, 0, 0);
		public int faceRotation = 0;
		public float MoveSpeed = 0.04f;
		public bool warped = true;

		private int AnimationX = 1;
		private float AnimationDelayLenght = 2.2f;
		private float AnimationDelay = 2.2f;

		private void ChangeTexture()
		{
			//if (this.Texture == null)
			//	this.Texture = PokemonReference.GetOverworldTexture();

			Vector4 r = new Vector4(0, 0, 0, 0);
			int cameraRotation = Game.Camera.GetFacingDirection();
			int spriteIndex = this.faceRotation - cameraRotation;

			spriteIndex = this.faceRotation - cameraRotation;
			if (spriteIndex < 0)
				spriteIndex += 4;

			int width = System.Convert.ToInt32(this.Texture.width / (double)3);

			int x = 0;
			x = AnimationX * width;

			int height = System.Convert.ToInt32(this.Texture.height / (double)4);

			int y = height * spriteIndex;

			r = new Vector4(x, y, width, height);

			if (r != lastRectangle)
			{
				lastRectangle = r;

				Texture2D t = TextureManager.GetTexture(this.Texture, r, 1);
				Textures[0] = t;
			}
		}

		public override void Update()
		{
			if (Game.playerTrainer.GetWalkPokemon() != null)
			{
				bool differentAdditionalData = false;
				bool differentShinyState = false;
				if (this.PokemonReference != null)
				{
					//differentAdditionalData = (this.PokemonReference.AdditionalData != Game.playerTrainer.GetWalkPokemon().AdditionalData);
					differentShinyState = (this.PokemonReference.IsShiny != Game.playerTrainer.GetWalkPokemon().IsShiny);
				}

				if (this.PokemonID != Game.playerTrainer.GetWalkPokemon().Species | differentAdditionalData | differentShinyState)
				{
					this.Texture = null;
					this.PokemonID = Game.playerTrainer.GetWalkPokemon().Species;
					this.PokemonReference = Game.playerTrainer.GetWalkPokemon();
				}

				this.ChangeTexture();

				this.AnimationDelay -= 0.1f;
				if (AnimationDelay <= 0.0f)
				{
					AnimationDelay = AnimationDelayLenght;
					AnimationX += 1;
					if (AnimationX > 2)
						AnimationX = 1;
				}

				ChangePosition();
			}
		}

		protected override float CalculateCameraDistance(Vector3 CPosition)
		{
			return base.CalculateCameraDistance(CPosition) - 0.2f;
		}

		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
				this.Rotation.y = Game.Camera.Yaw;
			this.Scale = new Vector3(1.0f, 1.0f, 1.0f);
			this.Position.y = this.GetYPosition();

			base.UpdateEntity();
		}

		public override void Render()
		{
			if (this.IsVisible())
			{
				//var state = GraphicsDevice.DepthStencilState;
				//GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
				//Draw(this.Model, this.Textures[0], false);
				//GraphicsDevice.DepthStencilState = state;
			}
		}

		/// <summary>
		/// If the OverworldPokémon should be rendered.
		/// </summary>
		public bool IsVisible()
		{
			if (System.Convert.ToBoolean(/*GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")*/""))
			{
				if (Game.Level.ShowOverworldPokemon)
				{
					if (IsCorrectScreen())
					{
						if (Game.playerTrainer.GetWalkPokemon() != null || Game.playerTrainer.GetWalkPokemon().Species != Pokemons.NONE)
						{
							if (!Game.Level.Surfing & !Game.Level.Riding)
							{
								if (this.PokemonID > 0)
								{
									if (this.Textures != null)
										return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public void ChangeRotation()
		{
			this.Position = new Vector3(System.Convert.ToInt32(this.Position.x), System.Convert.ToSingle(this.Position.y) + 0.001f, System.Convert.ToInt32(this.Position.z));
			if (Game.Camera.Position.x == System.Convert.ToInt32(this.Position.x) | Game.Camera.Position.z == System.Convert.ToInt32(this.Position.z))
			{
				if (this.Position.x < Game.Camera.Position.x)
					this.faceRotation = 3;
				else if (this.Position.x > Game.Camera.Position.x)
					this.faceRotation = 1;
				if (this.Position.z < Game.Camera.Position.z)
					this.faceRotation = 2;
				else if (this.Position.z > Game.Camera.Position.z)
					this.faceRotation = 0;
			}
		}

		private void ChangePosition()
		{
			if (Game.Camera.IsMoving)
			{
				if (System.Convert.ToInt32(this.Position.x) != System.Convert.ToInt32(Game.Camera.Position.x) | System.Convert.ToInt32(this.Position.z) != System.Convert.ToInt32(Game.Camera.Position.z))
				{
					this.Position += GetMove();
					this.AnimationDelayLenght = 1.1f;
				}
			}
			else
				this.AnimationDelayLenght = 2.2f;
		}

		private Vector3 GetMove()
		{
			Vector3 moveVector = new Vector3();
			switch (this.faceRotation)
			{
				case 0:
					{
						moveVector = new Vector3(0, 0, -1) * MoveSpeed;
						break;
					}
				case 1:
					{
						moveVector = new Vector3(-1, 0, 0) * MoveSpeed;
						break;
					}
				case 2:
					{
						moveVector = new Vector3(0, 0, 1) * MoveSpeed;
						break;
					}
				case 3:
					{
						moveVector = new Vector3(1, 0, 0) * MoveSpeed;
						break;
					}
			}
			return moveVector;
		}

		private bool IsCorrectScreen()
		{
			//Screen.Identifications[] screens = new[] { Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleGrowStatsScreen, Screen.Identifications.BattleScreen, Screen.Identifications.CreditsScreen, Screen.Identifications.BattleAnimationScreen, Screen.Identifications.ViewModelScreen, Screen.Identifications.HallofFameScreen };
			//if (screens.Contains(Core.CurrentScreen.Identification))
			//	return false;
			//else if (Core.CurrentScreen.Identification == Screen.Identifications.TransitionScreen)
			//{
			//	if (screens.Contains((TransitionScreen)Core.CurrentScreen.OldScreen.Identification) | screens.Contains((TransitionScreen)Core.CurrentScreen.NewScreen.Identification))
			//		return false;
			//}
			//else
			//{
			//	Screen c = Core.CurrentScreen;
			//	while (c.PreScreen != null)
			//		c = c.PreScreen;
			//	if (screens.Contains(c.Identification))
			//		return false;
			//}
			return true;
		}

		public void MakeVisible()
		{
			if (warped)
				warped = false;
			else if (!this.Visible)
			{
				this.Visible = true;
				this.Respawn();
			}
		}

		public void Respawn()
		{
			Vector3 newPosition = new Vector3(0, -2, 0);
			//if (Game.Camera.Name == "Overworld")
			//	newPosition = ((OverworldCamera)Game.Camera).LastStepPosition;
			if (newPosition != new Vector3(0, -2, 0))
				this.Position = newPosition;
			else
				switch (Game.Camera.GetPlayerFacingDirection())
				{
					case 0:
						{
							this.Position = new Vector3(Game.Camera.Position.x, this.GetYPosition(), Game.Camera.Position.z + 1);
							break;
						}
					case 1:
						{
							this.Position = new Vector3(Game.Camera.Position.x + 1, this.GetYPosition(), Game.Camera.Position.z);
							break;
						}
					case 2:
						{
							this.Position = new Vector3(Game.Camera.Position.x, this.GetYPosition(), Game.Camera.Position.z - 1);
							break;
						}
					case 3:
						{
							this.Position = new Vector3(Game.Camera.Position.x - 1, this.GetYPosition(), Game.Camera.Position.z);
							break;
						}
				}

			ChangeRotation();
		}

		public override void ClickFunction()
		{
			if (System.Convert.ToBoolean(/*GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")*/""))
			{
				if (this.Visible & (Game.playerTrainer.GetWalkPokemon() != null || Game.playerTrainer.GetWalkPokemon().Species != Pokemons.NONE) & !Game.Level.Surfing & !Game.Level.Riding & Game.Level.ShowOverworldPokemon)
				{
					Monster.Pokemon p = Game.playerTrainer.GetWalkPokemon();
					//string scriptString = PokemonInteractions.GetScriptString(p, this.Position, this.faceRotation);
					//
					//if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
					//{
					//	if (((OverworldScreen)Core.CurrentScreen).ActionScript.IsReady)
					//		((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(scriptString, 2);
					//}
				}
			}
		}

		public void ApplyShaders()
		{
			this.Shaders.Clear();
			//foreach (Shader Shader in Game.Level.Shaders)
			//	Shader.ApplyShader(this);
		}

		private void PokemonReference_TexturesCleared(object sender, EventArgs e)
		{
			this.Texture = null;
			this.ForceTextureChange();
		}

		private float GetYPosition()
		{
			return System.Convert.ToSingle(Game.Camera.Position.y);
		}

		public void ForceTextureChange()
		{
			this.lastRectangle = new Vector4(0, 0, 0, 0);
			this.ChangeTexture();
		}
	}
}
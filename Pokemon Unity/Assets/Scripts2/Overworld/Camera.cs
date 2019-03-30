using System;
using UnityEngine;

namespace PokemonUnity.Overworld
{
	public class Camera
	{
		//public BoundingFrustum BoundingFrustum;
		//public Matrix View, Projection;
		public Vector3 Position;

		public float Yaw, Pitch;

		protected Vector3 _plannedMovement = new Vector3(0F, 0, 0);
		protected bool _setPlannedMovement = false;

		public Vector3 PlannedMovement
		{
			get
			{
				return this._plannedMovement;
			}
			set
			{
				this._plannedMovement = value;
				this._setPlannedMovement = (value != Vector3.zero);
			}
		}

		public void AddToPlannedMovement(Vector3 v)
		{
			this._plannedMovement += v;
		}

		public Ray Ray = new Ray();

		public bool Turning = false;

		public float Speed = 0.04f;
		public float RotationSpeed = 0.003f;

		public float FarPlane = 30;
		public float FOV = 45.0f;

		public string Name = "INHERITS";

		public Camera(string Name)
		{
			this.Name = Name;
		}

		public virtual void Update()
		{
			throw new NotImplementedException();
		}

		public virtual void Turn(int turns)
		{
			throw new NotImplementedException();
		}

		public virtual void InstantTurn(int turns)
		{
			throw new NotImplementedException();
		}

		public int GetFacingDirection()
		{
			if (Yaw <= MathHelper.Pi * 0.25f | Yaw > MathHelper.Pi * 1.75f)
				return 0;
			if (Yaw <= MathHelper.Pi * 0.75f & Yaw > MathHelper.Pi * 0.25f)
				return 1;
			if (Yaw <= MathHelper.Pi * 1.25f & Yaw > MathHelper.Pi * 0.75f)
				return 2;
			if (Yaw <= MathHelper.Pi * 1.75f & Yaw > MathHelper.Pi * 1.25f)
				return 3;
			return 0;
		}

		public virtual int GetPlayerFacingDirection()
		{
			return this.GetFacingDirection();
		}

		public virtual Vector3 GetForwardMovedPosition()
		{
			throw new NotImplementedException();
		}

		public virtual Vector3 GetMoveDirection()
		{
			throw new NotImplementedException();
		}

		public virtual void Move(float Steps)
		{
			throw new NotImplementedException();
		}

		public virtual void StopMovement()
		{
			throw new NotImplementedException();
		}

		public virtual bool IsMoving
		{
			get
			{
				return false;
			}
		}

		public void CreateNewProjection(float newFOV)
		{
			//Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(newFOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01f, this.FarPlane);
			this.FOV = newFOV;
		}
		public virtual Vector3 CPosition
		{
			get
			{
				return this.Position;
			}
		}
	}
}
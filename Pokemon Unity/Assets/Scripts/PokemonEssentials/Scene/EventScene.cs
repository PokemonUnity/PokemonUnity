using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace PokemonUnity.Interface.UnityEngine
{
	public abstract class EventScene : MonoBehaviour, IEventScene
	{
		public event EventHandler OnATriggerEvent;
		public event EventHandler OnBTriggerEvent;
		public event EventHandler OnUpdateEvent;
		public bool OnATrigger { get { return onATrigger; } }
		public bool OnBTrigger { get { return onBTrigger; } }
		public bool OnUpdate {  get { return onUpdate; } }
		protected IList<ISpriteWrapper> picturesprites;
		protected IList<ISpriteWrapper> usersprites;
		private IViewport Viewport;
		protected bool disposed;
		private bool onATrigger;
		private bool onBTrigger;
		private bool onUpdate;
		private long framecount;

		#region Methods
		//public void initialize(viewport= null)
		public virtual IEventScene initialize(IViewport viewport= null)
		{
			Viewport = viewport;
			//@onCTrigger = new Event();
			//@onBTrigger = new Event();
			//@onUpdate = new Event();
			@OnATriggerEvent = null;
			@OnBTriggerEvent = null;
			@OnUpdateEvent = null;
			//@pictures =[];
			//@picturesprites =[];
			//@usersprites =[];
			@disposed = false;
			return this;
		}

		public IEnumerator main()
		{
			//This would do the same thing as Unity.Monobehavior.OnUpdate
			//while (!disposed)
			if (!disposed)
			{
				//Set this game object from false to true, to enable and begin game cycle.
				gameObject.SetActive(true);
				yield return update();
			}
		}

		public void addImage(float x, float y, string name)
		{
			//instantiate a sprite
		}

		public void addLabel(float x, float y, float width, string text)
		{
			//instantiate a text
		}

		public void getPicture(int num)
		{
			//ToDo: Load images into an array/dictionary and use input parameter to access
			//Change and cycle image used in scene based on input parameter
		}

		public void pictureWait(int extraframes = 0)
		{
			do //;loop
			{
				bool hasRunning = false;
				//foreach (var pic in @pictures)
				//{
				//	if (pic.running) hasRunning = true;
				//}
				if (hasRunning)
				{
					update();
				}
				else
				{
					break;
				}
			} while (true);

			//extraframes.times { update(); };
			for(int i = 0; i < extraframes; i++) { update(); };
		}

		//http://answers.unity.com/answers/46120/view.html
		//https://docs.unity3d.com/Manual/class-MonoManager.html
		//ToDo: split this logic function to operate using unity api
		public virtual IEnumerator update()
		{
			if (disposed) yield break;
			//Graphics.update();
			//Input.update();
			//foreach (var picture in @pictures)
			//{
			//	picture.update();
			//}
			//foreach (var sprite in @picturesprites)
			//{
			//	sprite.update();
			//}
			//foreach (var sprite in @usersprites)
			//{
			//	if (sprite && !sprite.disposed)
			//	{
			//		if (sprite is Sprite) sprite.update();
			//	}
			//}
			//@onUpdate.trigger(this);
			@OnUpdateEvent?.Invoke(this, EventArgs.Empty);
			//ToDo: I have some pending updates that will include these in future...
			//for now, you can use unity and bypass this.
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))
			{
				//@onCTrigger.trigger(this);
				@OnATriggerEvent?.Invoke(this, EventArgs.Empty);
			}
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.B))
			{
				//@onBTrigger.trigger(this);
				@OnBTriggerEvent?.Invoke(this, EventArgs.Empty);
			}
			// each update one tick... but it wont be reflected in unity, unless you create an artificial pause
			//Use fixed update as it represents how quickly your frames tick
			yield return new WaitForSeconds(.1f);
		}

		public void wait(int frames)
		{
			//frames.times { update };
			//for(int i = 0; i < frames; i++) { update(); };
			framecount = frames;
		}

		/// <summary>
		/// Resets any events triggered by listener for <see cref="OnATriggerEvent"/> handler.
		/// </summary>
		/// Clears all the event listeners subscribed to <see cref="OnATriggerEvent"/> handler.
		protected void ClearOnTriggerA()
		{
			@OnATriggerEvent = null;
			onATrigger = false;
		}

		/// <summary>
		/// Resets any events triggered by listener for <see cref="OnBTriggerEvent"/> handler.
		/// </summary>
		/// Clears all the event listeners subscribed to <see cref="OnBTriggerEvent"/> handler.
		protected void ClearOnTriggerB()
		{
			@OnBTriggerEvent = null;
			onBTrigger = false;
		}

		/// <summary>
		/// Resets any events triggered by listener for <see cref="OnUpdateEvent"/> handler.
		/// </summary>
		/// Clears all the event listeners subscribed to <see cref="OnUpdateEvent"/> handler.
		protected void ClearOnUpdate()
		{
			@OnUpdateEvent = null;
			@onUpdate = false;
		}
		#endregion

		#region Scene Inheritence
		public abstract int Id { get; }
		bool IEventScene.disposed { get { return disposed; } }

		public abstract void Refresh();
		public abstract void Display(string v);
		public abstract bool DisplayConfirm(string v);
		#endregion

		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;
			if (disposing)
			{
				// TODO: dispose managed state (managed objects)
				//Destroy all gameobjects created by scene
				foreach (var sprite in @picturesprites)
				{
					sprite.Dispose();
				}
				foreach (var sprite in @usersprites)
				{
					sprite.Dispose();
				}
				//@onCTrigger.clear;
				//@onBTrigger.clear;
				//@onUpdate.clear;
				@OnATriggerEvent = null;
				@OnBTriggerEvent = null;
				@OnUpdateEvent = null;
				//@pictures.clear;
				//@picturesprites.clear;
				//@usersprites.clear;
			}

			// TODO: free unmanaged resources (unmanaged objects) and override finalizer
			// TODO: set large fields to null
			@disposed = true;
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~EventScene()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		void IDisposable.Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#region Unity Monobehavior
		private void Awake()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		private void OnEnable()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		private void Start()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		private void Update()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (framecount > 0)
				framecount--;
			if (framecount == 0)
				update();
		}
		private void FixedUpdate()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		private void LateUpdate()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		private void OnDisable()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		private void OnDestroy()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		#endregion
	}
}
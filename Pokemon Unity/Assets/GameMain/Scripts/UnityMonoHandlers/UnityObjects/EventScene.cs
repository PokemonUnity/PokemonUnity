using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonUnity;

namespace PokemonUnity.UX
{
	//Maybe change to ScriptableObject instead...
	public class EventScene : MonoBehaviour, IEventScene
	{
		public bool onATrigger { get; set; }
		public bool onBTrigger { get; set; }
		public bool onUpdate { get; set; }
		public bool disposed { get; private set; }

		//public void initialize(viewport= null)
		public virtual void initialize()
		{
			//@viewport = viewport;
			//@onCTrigger = new Event();
			//@onBTrigger = new Event();
			//@onUpdate = new Event();
			@onATrigger = false;
			@onBTrigger = false;
			@onUpdate = false;
			//@pictures =[];
			//@picturesprites =[];
			//@usersprites =[];
			@disposed = false;
		}

		public void main()
		{
			//while (!disposed)
			if (!disposed)
			{
				update();
			}
		}

		public void addImage(float x, float y, string name)
		{
			throw new System.NotImplementedException();
		}

		public void addLabel(float x, float y, float width, string text)
		{
			throw new System.NotImplementedException();
		}

		public void dispose()
		{
			if (disposed) return;
			//Destroy all go created by scene
			//foreach (var sprite in @picturesprites)
			//{
			//	sprite.dispose();
			//}
			//foreach (var sprite in @usersprites)
			//{
			//	sprite.dispose();
			//}
			//@onCTrigger.clear;
			//@onBTrigger.clear;
			//@onUpdate.clear;
			@onATrigger = false;
			@onBTrigger = false;
			@onUpdate = false;
			//@pictures.clear;
			//@picturesprites.clear;
			//@usersprites.clear;
			@disposed = true;
		}

		public void getPicture(int num)
		{
			throw new System.NotImplementedException();
		}


		public void pictureWait(int extraframes = 0)
		{
			//do //;loop
			//{ 
			//	bool hasRunning = false;
			//	foreach (var pic in @pictures)
			//	{
			//		if (pic.running) hasRunning = true;
			//	}
			//	if (hasRunning)
			//	{
			//		update();
			//	}
			//	else
			//	{
			//		break;
			//	}
			//} while (true);

			//extraframes.times { update(); };
			for(int i = 0; i < extraframes; i++) { update(); };
		}

		public void update()
		{
			if (disposed) return;
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
			@onUpdate = true;
			//I have some pending updates that will include these in future...
			//for now, you can use unity and bypass this.
			//if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))
			//{
			//	//@onCTrigger.trigger(this);
			//	@onATrigger = true;
			//}
			//if (PokemonUnity.Input.trigger(PokemonUnity.Input.B))
			//{
			//	//@onBTrigger.trigger(this);
			//	@onBTrigger = true;
			//}
			// each update one tick... but it wont be reflected in unity, unless you create an artificial pause
			StartCoroutine(Tick()); 
		}

		public void wait(int frames)
		{
			//frames.times { update };
			for(int i = 0; i < frames; i++) { update(); };
		}

		IEnumerator Tick()
		{
			//Use a global const float, to represent how quickly your frames tick
			yield return new WaitForSeconds(.1f);
		}
	}
}
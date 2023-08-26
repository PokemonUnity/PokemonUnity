using System;

namespace PokemonEssentials.Interface
{
	public interface IReflectedSprite : IDisposable 
	{
		int visible					{ get; }
		IGameEvent @event           { get; }

		IReflectedSprite initialize(ISprite sprite,IGameEvent _event, IViewport viewport= null);

		//void dispose();
		//bool disposed { get; set; }

		void update();
	}

	public interface IClippableSprite : ISpriteCharacter 
	{
		//IClippableSprite initialize(IViewport viewport,IGameEvent @event,ITilemapLoader tilemap);

		//void update();
	}

	public interface ISpritesetMap : IDisposable 
	{
		IGameMap map			{ get; }
		IViewport viewport1		{ get; }
		//ITilemapLoader tilemap	{ get; }

		ISpritesetMap initialize(IGameMap map = null);

		void dispose();

		bool in_range(IGameEvent obj);

		void update();
	}
}
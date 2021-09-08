namespace PokemonEssentials.Interface
{
	public interface ISpriteWrapper
	{
		float angle { get; set; }
		//Bitmap bitmap { get; set; }
		int blend_type { get; set; }
		int bush_depth { get; set; }
		IColor color { get; set; }
		bool disposed { get; }
		bool mirror { get; set; }
		float opacity { get; set; }
		float ox { get; set; }
		float oy { get; set; }
		IRect src_rect { get; set; }
		ITone tone { get; set; }
		IViewport viewport { get; set; }
		bool visible { get; set; }
		float x { get; set; }
		float y { get; set; }
		float z { get; set; }
		float zoom_x { get; set; }
		float zoom_y { get; set; }

		void dispose();
		void flash(IColor color, int duration);
		ISpriteWrapper initialize(IViewport viewport  = null);
		void update();
	}
}
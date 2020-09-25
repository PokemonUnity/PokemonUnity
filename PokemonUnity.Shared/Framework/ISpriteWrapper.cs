public interface ISpriteWrapper
{
	float angle { get; set; }
	//Bitmap bitmap { get; set; }
	int blend_type { get; set; }
	int bush_depth { get; set; }
	PokemonUnity.Color color { get; set; }
	bool disposed { get; }
	bool mirror { get; set; }
	float opacity { get; set; }
	float ox { get; set; }
	float oy { get; set; }
	//Rect src_rect { get; set; }
	//Tone tone { get; set; }
	object viewport { get; set; }
	bool visible { get; set; }
	float x { get; set; }
	float y { get; set; }
	float z { get; set; }
	float zoom_x { get; set; }
	float zoom_y { get; set; }

	void dispose();
	void flash(PokemonUnity.Color color, int duration);
	//void initialize(viewport  = null);
	void update();
}
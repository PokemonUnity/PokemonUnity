using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.UX;

//Interface for RPG Maker User Experience
//namespace PokemonUnity.UX.RPGMaker
namespace PokemonEssentials.Interface
{
	#region Audio
	/// <summary>
	/// The module that carries out music and sound processing.
	/// </summary>
	public interface IAudio
	{
		/// <summary>
		/// Starts BGM playback. Sets the file name, volume, and pitch in turn.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="volume"></param>
		/// <param name="pitch"></param>
		void bgm_play(string filename, float volume, float pitch);
		/// <summary>
		/// Stops BGM playback.
		/// </summary>
		void bgm_stop();
		/// <summary>
		/// Starts BGM fadeout. 
		/// </summary>
		/// <param name="time">time is the length of the fadeout in milliseconds.</param>
		void bgm_fade(int time);
		/// <summary>
		/// Starts BGS playback. Sets the file name, volume, and pitch in turn.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="volume"></param>
		/// <param name="pitch"></param>
		void bgs_play(string filename, float volume, float pitch);
		/// <summary>
		/// Stops BGS playback.
		/// </summary>
		void bgs_stop();
		/// <summary>
		/// Starts BGS fadeout. 
		/// </summary>
		/// <param name="time">time is the length of the fadeout in milliseconds.</param>
		void bgs_fade(int time);
		/// <summary>
		/// Starts ME playback. Sets the file name, volume, and pitch in turn.
		/// When ME is playing, the BGM will temporarily stop.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="volume"></param>
		/// <param name="pitch"></param>
		void me_play(string filename, float volume, float pitch);
		/// <summary>
		/// Stops ME playback.
		/// </summary>
		void me_stop();
		/// <summary>
		/// Starts ME fadeout. 
		/// </summary>
		/// <param name="time">time is the length of the fadeout in milliseconds.</param>
		void me_fade(int time);
		/// <summary>
		/// Starts SE playback. Sets the file name, volume, and pitch in turn.
		/// When attempting to play the same SE more than once in a very short period, they will automatically be filtered to prevent choppy playback.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="volume"></param>
		/// <param name="pitch"></param>
		void se_play(string filename, float volume, float pitch);
		/// <summary>
		/// Stops ME playback.
		/// </summary>
		void se_stop();
	}
	public interface IAudioObject
	{
		string name { get; }
		float volume { get; }
		float pitch { get; }
	}
	public interface IAudioBGM : IAudioObject
	{
		/// <summary>
		/// Returns BGM (<seealso cref="IAudioBGM"/>) that playing now. If no playing BGM, returns null.
		/// </summary>
		/// <returns></returns>
		IAudioBGM last();
		/// <summary>
		/// Stops BGM playback.
		/// </summary>
		void stop();
		/// <summary>
		/// Starts BGM fadeout. 
		/// </summary>
		/// <param name="time">time is the length of the fadeout in milliseconds.</param>
		void fade(int time);
		/// <summary>
		/// Starts the BGM playback.
		/// </summary>
		void play();
	}
	public interface IAudioBGS : IAudio
	{
		/// <summary>
		/// Returns BGS (<seealso cref="IAudioBGS"/>) that playing now. If no playing BGS, returns null.
		/// </summary>
		/// <returns></returns>
		IAudioBGS last();
		/// <summary>
		/// Stops BGS playback.
		/// </summary>
		void stop();
		/// <summary>
		/// Starts BGS fadeout. 
		/// </summary>
		/// <param name="time">time is the length of the fadeout in milliseconds.</param>
		void fade(int time);
		/// <summary>
		/// Starts the BGS playback.
		/// </summary>
		void play();
	}
	public interface IAudioME : IAudio
	{
		/// <summary>
		/// Stops ME playback.
		/// </summary>
		void stop();
		/// <summary>
		/// Starts ME fadeout. 
		/// </summary>
		/// <param name="time">time is the length of the fadeout in milliseconds.</param>
		void fade(int time);
		/// <summary>
		/// Starts the ME playback.
		/// </summary>
		void play();
	}
	public interface IAudioSE : IAudio
	{
		/// <summary>
		/// Stops SE playback.
		/// </summary>
		void stop();
		/// <summary>
		/// Starts the SE playback.
		/// </summary>
		void play();
	}
	#endregion

	/// <summary>
	/// The RGBA color class. Each component is handled with a floating point value (Float).
	/// </summary>
	public interface IColor
	{
		float red { get; }
		float green { get; }
		float blue { get; }
		float alpha { get; }
		/// <summary>
		/// The RGBAA color class. Each component is handled with a floating point value. (0<>255)
		/// </summary>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <param name="alpha"></param>
		void set(float red, float green, float blue, float alpha = 0);
	}
	/// <summary>
	/// The color tone class. Each component is handled with a floating point value (Float).
	/// </summary>
	public interface ITone
	{
		float red { get; }
		float green { get; }
		float blue { get; }
		float gray { get; }
		/// <summary>
		/// The color tone class. Each component is handled with a floating point value. (-255<>255)
		/// </summary>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <param name="gray">only 0-255</param>
		void set(float red, float green, float blue, float gray = 0);
	}
	/// <summary>
	/// The rectangle class.
	/// </summary>
	public interface IRect
	{
		/// <summary>
		/// The X-coordinate of the rectange's upper left corner.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The Y-coordinate of the rectange's upper left corner.
		/// </summary>
		float y { get; set; }
		/// <summary>
		/// The rectangle's width.
		/// </summary>
		float width { get; set; }
		/// <summary>
		/// The rectangle's height.
		/// </summary>
		float height { get; set; }
		/// <summary>
		/// Sets all parameters at once.
		/// </summary>
		void set(float x, float y, float width, float height);
	}
	/// <summary>
	/// The game window class. Created internally from multiple sprites.
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// Refers to the bitmap (Bitmap) used as a windowskin.
		/// </summary>
		bool windowskin { get; set; }
		/// <summary>
		/// Refers to the (Viewport) associated with the window.
		/// </summary>
		IViewport viewport { get; set; }
		/// <summary>
		/// The cursor box (Rect). Sets the window's upper left corner using relative coordinates (-16, -16).
		/// </summary>
		//IRect cursor_rect { get; set; }
		/// <summary>
		/// Cursor blink status. If TRUE, the cursor is blinking.
		/// </summary>
		bool active { get; set; }
		/// <summary>
		/// The viewport's visibility. If TRUE, the viewport is visible.
		/// </summary>
		bool visible { get; set; }
		/// <summary>
		/// The pause graphic's visibility. This is a symbol that appears in the message window when waiting for the player to press a button. If TRUE, the graphic is visible.
		/// </summary>
		bool pause { get; set; }
		/// <summary>
		/// The window's X-coordinate.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The window's Y-coordinate.
		/// </summary>
		float y { get; set; }
		/// <summary>
		/// The window's width.
		/// </summary>
		float width { get; set; }
		/// <summary>
		/// The window's height.
		/// </summary>
		float height { get; set; }
		/// <summary>
		/// The window's Z-coordinate. The larger this value, the closer to the player the plane will be displayed. 
		/// </summary>
		float z { get; set; }
		/// <summary>
		/// The X-coordinate of the viewport's starting point. Change this value to shake the screen, etc.
		/// </summary>
		float ox { get; set; }
		/// <summary>
		/// The Y-coordinate of the viewport's starting point. Change this value to shake the screen, etc.
		/// </summary>
		float oy { get; set; }
		/// <summary>
		/// The window's opacity (0-255). 
		/// </summary>
		byte opacity { get; set; }
		/// <summary>
		/// The window background's opacity (0-255). 
		/// </summary>
		byte back_opacity { get; set; }
		/// <summary>
		/// The opacity of the window's contents (0-255). 
		/// </summary>
		byte contents_opacity { get; set; }
		/// <summary>
		/// By changing this value from 0 (completely closed) to 255 (completely open), 
		/// it is possible to create an animation of the window opening and closing. 
		/// If the openness is less than 255, the contents of the window will not be displayed. 
		/// The default value is 255.
		/// </summary>
		byte openness { get; set; }
		/// <summary>
		/// The color (Color) to be blended with the viewport. Alpha values are used in the blending ratio.
		/// Handled separately from the color blended into a flash effect.
		/// </summary>
		IColor color { get; set; }
		IEnumerator update();
		void dispose();
		/// <summary>
		/// Creates a Window object. Specifies a Viewport (Viewport) when necessary.
		/// </summary>
		/// <param name="viewport"></param>
		IWindow initialize(IViewport viewport);
	}

	/// <summary>
	/// The viewport class. 
	/// Used when displaying sprites in one portion of the screen, 
	/// with no overflow into other regions.
	/// </summary>
	public interface IViewport : IDisposable
	{
		/// <summary>
		/// The viewport's visibility. If TRUE, the viewport is visible.
		/// </summary>
		bool visible { get; }
		/// <summary>
		/// The X-coordinate of the viewport's starting point. Change this value to shake the screen, etc.
		/// </summary>
		float ox { get; }
		/// <summary>
		/// The Y-coordinate of the viewport's starting point. Change this value to shake the screen, etc.
		/// </summary>
		float oy { get; }
		/// <summary>
		/// The color (Color) to be blended with the viewport. Alpha values are used in the blending ratio.
		/// Handled separately from the color blended into a flash effect.
		/// </summary>
		IColor color { get; }
		/// <summary>
		/// Returns TRUE if the viewport has been freed.
		/// </summary>
		bool disposed { get; }
		/// <summary>
		/// Frees the viewport. If the viewport has already been freed, does nothing.
		/// </summary>
		void dispose();
		/// <summary>
		/// Begins flashing the viewport. duration specifies the number of frames the flash will last.
		/// If color is set to nil, the viewport will disappear while flashing.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		void flash(IColor color, int duration);
		/// <summary>
		/// Refreshes the viewport flash. As a rule, this method is called once per frame.
		/// It is not necessary to call this method if no flash effect is needed.
		/// </summary>
		/// <returns></returns>
		IEnumerator update();
	}
	/// <summary>
	/// A module that handles input data from a gamepad or keyboard.
	/// </summary>
	public interface IInput
	{
		/// <summary>
		/// Updates input data. As a rule, this method is called once per frame.
		/// </summary>
		/// <returns></returns>
		IEnumerator update();
		/// <summary>
		/// Determines whether the button num is currently being pressed.
		/// </summary>
		/// <param name="num"></param>
		/// <returns>If the button is being pressed, returns TRUE.If not, returns FALSE.</returns>
		bool press(InputKeys num);
		/// <summary>
		/// Determines whether the button num is being pressed again.
		/// </summary>
		/// <param name="num"></param>
		/// <returns>If the button is being pressed, returns TRUE. If not, returns FALSE.</returns>
		/// <remarks>"Pressed again" is seen as time having passed between the button being not pressed and being pressed.</remarks>
		bool trigger(InputKeys num);
		/// <summary>
		/// Determines whether the button num is being pressed again.
		/// </summary>
		/// <param name="num"></param>
		/// <returns>If the button is being pressed, returns TRUE. If not, returns FALSE.</returns>
		/// <remarks>Unlike <see cref="trigger"/>, takes into account the repeat input of a button being held down continuously.</remarks>
		bool repeat(InputKeys num);
	}

	public interface IRPGSprite : IDisposable
	{
		/// <summary>
		/// The sprite's angle of rotation. Specifies up to 360 degrees of counterclockwise rotation.
		/// </summary>
		float angle { get; set; }
		/// <summary>
		/// Refers to the bitmap (Bitmap) used for the sprite's starting point.
		/// </summary>
		//IBitmap bitmap { get; set; }
		/// <summary>
		/// The sprite's blending mode (0: normal, 1: addition, 2: subtraction).
		/// </summary>
		int blend_type { get; set; }
		/// <summary>
		/// This can be used to represent something like characters' legs being hidden by bushes.
		/// For bush_depth, the number of pixels for the bush section is specified.The default value is 0.
		/// </summary>
		int bush_depth { get; set; }
		/// <summary>
		/// The color (Color) to be blended with the sprite. Alpha values are used in the blending ratio.
		/// </summary>
		IColor color { get; set; }
		/// <summary>
		/// Returns TRUE if the sprite has been freed.
		/// </summary>
		bool disposed { get; }
		/// <summary>
		/// Flag denoting the sprite has been flipped horizontally. If TRUE, the sprite will be drawn flipped.
		/// </summary>
		bool mirror { get; set; }
		/// <summary>
		/// The sprite's opacity (0-255). Values out of range are automatically corrected.
		/// </summary>
		float opacity { get; set; }
		/// <summary>
		/// The X-coordinate of the sprite's starting point.
		/// </summary>
		float ox { get; set; }
		/// <summary>
		/// The Y-coordinate of the sprite's starting point.
		/// </summary>
		float oy { get; set; }
		IRect src_rect { get; set; }
		ITone tone { get; set; }
		/// <summary>
		/// Refers to the viewport (Viewport) associated with the sprite.
		/// </summary>
		IViewport viewport { get; set; }
		/// <summary>
		/// The sprite's visibility. If TRUE, the sprite is visible.
		/// </summary>
		bool visible { get; set; }
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		float y { get; set; }
		/// <summary>
		/// The sprite's Z-coordinate. The larger this value, the closer to the player the sprite will be displayed.
		/// </summary>
		float z { get; set; }
		/// <summary>
		/// The sprite's X-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		float zoom_x { get; set; }
		/// <summary>
		/// The sprite's Y-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		float zoom_y { get; set; }
		/// <summary>
		/// Gets the width of the sprite. Equivalent to src_rect.width.
		/// </summary>
		float width { get; set; }
		/// <summary>
		/// Gets the height of the sprite. Equivalent to src_rect.height.
		/// </summary>
		float height { get; set; }
		/// <summary>
		/// If a flash or wave is not needed, it is not necessary to call this.
		/// </summary>
		/// <returns></returns>
		IEnumerator update();
		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		void dispose();
		/// <summary>
		/// Begins flashing the sprite. duration specifies the number of frames the flash will last.
		/// If color is set to nil, the sprite will disappear while flashing.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		void flash(IColor color, int duration);
		ISprite initialize(IViewport viewport = null);
	}
	public interface ISpriteResizer //: ISprite
	{
		/// <summary>
		/// This can be used to represent something like characters' legs being hidden by bushes.
		/// For bush_depth, the number of pixels for the bush section is specified.The default value is 0.
		/// </summary>
		int bush_depth { get; set; }
		/// <summary>
		/// The X-coordinate of the sprite's starting point.
		/// </summary>
		float ox { get; set; }
		/// <summary>
		/// The Y-coordinate of the sprite's starting point.
		/// </summary>
		float oy { get; set; }
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		float y { get; set; }
		/// <summary>
		/// The sprite's X-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		float zoom_x { get; set; }
		/// <summary>
		/// The sprite's Y-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		float zoom_y { get; set; }
		ISprite initialize(IViewport viewport = null);
	}
	/// <summary>
	/// The module that carries out graphics processing.
	/// </summary>
	public interface IGraphics
	{
		/// <summary>
		/// The number of times the screen is refreshed per second. The larger the value, the more CPU power is required. Normally set at 60.
		/// Changing this property is not recommended; however, it can be set anywhere from 10 to 120. 
		/// </summary>
		int frame_rate { get; set; }
		/// <summary>
		/// The screen's refresh rate count. Set this property to 0 at game start and the game play time (in seconds) 
		/// can be calculated by dividing this value by the frame_rate property value.
		/// </summary>
		int frame_count { get; set; }
		/// <summary>
		/// The brightness of the screen. Takes a value from 0 to 255. 
		/// The fadeout, fadein, and transition methods change this value internally, as required.
		/// </summary>
		/// ToDO: Takes a value from -126 to 126. Negative use black overlay, positive use white.
		byte brightness { get; set; }
		/// <summary>
		/// Gets the width of the game screen.
		/// </summary>
		int width { get; }
		/// <summary>
		/// Gets the height of the game screen.
		/// </summary>
		int height { get; }
		/// <summary>
		/// Refreshes the game screen and advances time by 1 frame. This method must be called at set intervals.
		/// </summary>
		/// <returns></returns>
		IEnumerator update();
		/// <summary>
		/// Waits for the specified number of frames.
		/// </summary>
		/// <example>
		/// do {
		///		<see cref="update"/>
		/// } while (duration.times)
		/// </example>
		/// <param name="duration"></param>
		void wait(float duration);
		/// <summary>
		/// Performs a fade-out of the screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames to spend on the fade-out.</param>
		void fadeout(float duration);
		/// <summary>
		/// Performs a fade-in of the screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames to spend on the fade-in.</param>
		void fadein(float duration);
		/// <summary>
		/// Fixes the current screen in preparation for transitions.
		/// Screen rewrites are prohibited until the transition method is called.
		/// </summary>
		void freeze();
		/// <summary>
		/// Resets the screen refresh timing. After a time-consuming process, call this method to prevent extreme frame skips.
		/// </summary>
		void frame_reset();
		/// <summary>
		/// Carries out a transition from the screen fixed in Graphics.freeze to the current screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames the transition will last. When omitted, this value is set to 8.</param>
		/// <param name="filename">filename specifies the transition graphic file name. When not specified, a standard fade will be used.</param>
		/// <param name="vague">vague sets the ambiguity of the borderline between the graphic's starting and ending points. The larger the value, the greater the ambiguity. When omitted, this value is set to 40.</param>
		void transition(float duration, string filename, int vague);
	}
}
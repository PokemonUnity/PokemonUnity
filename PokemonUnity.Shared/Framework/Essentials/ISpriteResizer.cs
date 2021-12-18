using System;
using System.Collections;

namespace PokemonEssentials.Interface
{
	public interface IGameResizer
	{
		float ResizeFactor		{ get; set; }
		int ResizeFactorMul		{ get; set; }
		int ResizeOffsetX		{ get; set; }
		int ResizeOffsetY		{ get; set; }
		bool ResizeFactorSet	{ get; set; }
		bool HaveResizeBorder	{ get; set; }

		void pbSetResizeFactor(float factor = 1, bool norecalc = false);

		void pbSetResizeFactor2(float factor, bool force = false);

		void pbConfigureFullScreen();

		void pbConfigureWindowedScreen(float value);

		void setScreenBorderName(string border);
	}

	public interface Graphics
	{
		// // Nominal screen size
		int @width { get; }
		int @height { get; }

		int brightness { get; set; }

		void fadein(int frames);

		void wait(int frames);

		void fadeout(int frames);

		void resize_screen(int w, int h);

		bool deletefailed { get; set; }

		void snap_to_bitmap();
	}

	/// <summary>
	/// <seealso cref="ISprite"/>
	/// </summary>
	public interface ISpriteResizer2
	{
		//unless (@SpriteResizerMethodsAliased) {
		//  alias _initialize_SpriteResizer initialize;
		//  alias _x_SpriteResizer x;
		//  alias _y_SpriteResizer y;
		//  alias _ox_SpriteResizer ox;
		//  alias _oy_SpriteResizer oy;
		//  alias _zoomx_SpriteResizer zoom_x;
		//  alias _zoomy_SpriteResizer zoom_y;
		//  alias _xeq_SpriteResizer x=;
		//  alias _yeq_SpriteResizer y=;
		//  alias _zoomxeq_SpriteResizer zoom_x=;
		//  alias _zoomyeq_SpriteResizer zoom_y=;
		//  alias _oxeq_SpriteResizer ox=;
		//  alias _oyeq_SpriteResizer oy=;
		//  alias _bushdeptheq_SpriteResizer bush_depth=;
		//  @SpriteResizerMethodsAliased=true;
		//}

		ISprite initialize(IViewport viewport = null);

		float zoom_x { get; set; }

		float zoom_y { get; set; }

		int x { get; set; }

		int bush_depth { get; set; }

		int y { get; set; }

		float ox { get; set; }

		float oy { get; set; }
	}

	public interface INotifiableRect : IRect
	{
		void setNotifyProc(Action proc);

		//void set(float x, float y, float width, float height);

		//float x { set; }

		//float y { set; }

		//float width { set; }

		//float height { set; }
	}

	/// <summary>
	/// <seealso cref="IViewport"/>
	/// </summary>
	public interface IViewportResizer
	{
		//unless (@SpriteResizerMethodsAliased) {
		//  alias _initialize_SpriteResizer initialize;
		//  alias _rect_ViewportResizer rect;
		//  alias _recteq_SpriteResizer rect=;
		//  alias _oxeq_SpriteResizer ox=;
		//  alias _oyeq_SpriteResizer oy=;
		//  @SpriteResizerMethodsAliased=true;
		//}

		IViewport initialize(params object[] arg);

		float ox { get; set; }
		
		float oy { get; set; }
		
		IRect rect { get; set; }
	}

	public interface IPlane : IPlaneSpriteWindow
	{
		//unless (@SpriteResizerMethodsAliased) {
		//  alias _initialize_SpriteResizer initialize;
		//  alias _zoomxeq_SpriteResizer zoom_x=;
		//  alias _zoomyeq_SpriteResizer zoom_y=;
		//  alias _oxeq_SpriteResizer ox=;
		//  alias _oyeq_SpriteResizer oy=;
		//  @SpriteResizerMethodsAliased=true;
		//}

		IPlane initialize(IViewport viewport = null);

		float ox { get; set; }

		float oy { get; set; }

		float zoom_x { get; set; }

		float zoom_y { get; set; }
	}

	public interface IScreenBorder
	{
		//void initialize() {
		IScreenBorder initialize();

		void initializeInternal();

		void dispose();

		void adjustZ(int z);

		string bordername { set; }

		void refresh();
	}

	public interface IBitmap
	{
		//  Fast methods for retrieving bitmap data
		//RtlMoveMemory_pi = new Win32API('kernel32', 'RtlMoveMemory', 'pii', 'i');
		//RtlMoveMemory_ip = new Win32API('kernel32', 'RtlMoveMemory', 'ipi', 'i');
		//SwapRgb = new Win32API('./rubyscreen.dll', 'SwapRgb', 'pi', '') rescue null;

		void setData(byte[] x);

		byte[] getData();

		long swap32(byte x);

		void asOpaque();

		void saveToPng(string filename);

		string address { get; }
	}
}
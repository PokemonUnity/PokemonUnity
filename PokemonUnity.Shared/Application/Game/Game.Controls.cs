using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
//public bool pbSameThread(wnd) {
//  if (wnd==0) return false;
//  processid= new int[]{ 0 }; //.pack('l')
//  getCurrentThreadId=new Win32API('kernel32','GetCurrentThreadId', '%w()','l');
//  getWindowThreadProcessId=new Win32API('user32','GetWindowThreadProcessId', '%w(l p)','l');
//  threadid=getCurrentThreadId.call;
//  wndthreadid=getWindowThreadProcessId.call(wnd,processid);
//  return (wndthreadid==threadid);
//}



public static partial class Input {
  public const int DOWN  = 2;
  public const int LEFT  = 4;
  public const int RIGHT = 6;
  public const int UP    = 8;
  public const int A     = 11;
  public const int B     = 12;
  public const int C     = 13;
  public const int X     = 14;
  public const int Y     = 15;
  public const int Z     = 16;
  public const int L     = 17;
  public const int R     = 18;
  public const int SHIFT = 21; //SELECT
  public const int CTRL  = 22; //START
  public const int ALT   = 23;
  public const int F5    = 25;
  public const int F6    = 26;
  public const int F7    = 27;
  public const int F8    = 28;
  public const int F9    = 29;
  public const int LeftMouseKey  = 0;
  public const int RightMouseKey = 1;
        private static Dictionary<int, int> keystate;
        private static Dictionary<int, bool> stateUpdated;
        private static Dictionary<int, bool> triggerstate;
        private static Dictionary<int, bool> releasestate;

  //  GetAsyncKeyState or GetKeyState will work here
  //@GetKeyState=new Win32API("user32", "GetAsyncKeyState", "i", "i");
  //@GetForegroundWindow=new Win32API("user32", "GetForegroundWindow", "", "i");

  /// <summary>
  /// Returns whether a key is being pressed
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  public static bool getstate(int key) {
    //return (@GetKeyState.call(key)&0x8000)>0;
    return keystate[key]>0;
  }

  public static void updateKeyState(int i) {
    //bool gfw=pbSameThread(@GetForegroundWindow.call());
    if (!@stateUpdated[i]) {
      bool newstate=Input.getstate(i); //&& gfw
      @triggerstate[i]=(newstate&&@keystate[i]==0);
      @releasestate[i]=(!newstate&&@keystate[i]>0);
      @keystate[i]=newstate ? @keystate[i]+1 : 0;
      @stateUpdated[i]=true;
    }
  }

  public static void update() {
    if (@keystate != null) {
      for (int i = 0; i < 256; i++) {
        //  just noting that the state should be updated
        //  instead of thunking to Win32 256 times
        @stateUpdated[i]=false;
        if (@keystate[i] > 0) {
          //  If there is a repeat count, update anyway
          //  (will normally apply only to a very few keys)
          updateKeyState(i);
        }
      }
    } else {
      @stateUpdated=new Dictionary<int, bool>();
      @keystate=new Dictionary<int, int>();
      @triggerstate=new Dictionary<int, bool>();
      @releasestate=new Dictionary<int, bool>();
      for (int i = 0; i < 256; i++) {
        @stateUpdated[i]=true;
        @keystate[i]=Input.getstate(i) ? 1 : 0;
        @triggerstate[i]=false;
        @releasestate[i]=false;
      }
    }
  }

  public static int[] buttonToKey(int button) {
    switch (button) {
    case Input.DOWN:
      return new int[] { 0x28 }; // Down
      break;
    case Input.LEFT:
      return new int[] { 0x25 }; // Left
      break;
    case Input.RIGHT:
      return new int[] { 0x27 }; // Right
      break;
    case Input.UP:
      return new int[] { 0x26 }; // Up
      break;
    case Input.A:
      return new int[] { 0x5A,0x10 }; // Z, Shift
      break;
    case Input.B:
      return new int[] { 0x58,0x1B }; // X, ESC 
      break;
    case Input.C:
      return new int[] { 0x43,0x0D,0x20 }; // C, ENTER, Space
      break;
    case Input.X:
      return new int[] { 0x41 }; // A
      break;
    case Input.Y:
      return new int[] { 0x53 }; // S
      break;
    case Input.Z:
      return new int[] { 0x44 }; // D
      break;
    case Input.L:
      return new int[] { 0x51,0x21 }; // Q, Page Up
      break;
    case Input.R:
      return new int[] { 0x57,0x22 }; // W, Page Down
      break;
    case Input.SHIFT:
      return new int[] { 0x10 }; // Shift
      break;
    case Input.CTRL:
      return new int[] { 0x11 }; // Ctrl
      break;
    case Input.ALT:
      return new int[] { 0x12 }; // Alt
      break;
    case Input.F5:
      return new int[] { 0x74 }; // F5
      break;
    case Input.F6:
      return new int[] { 0x75 }; // F6
      break;
    case Input.F7:
      return new int[] { 0x76 }; // F7
      break;
    case Input.F8:
      return new int[] { 0x77 }; // F8
      break;
    case Input.F9:
      return new int[] { 0x78 }; // F9
      break;
    default:
      return new int[0];
    }
  }

  public static int dir4() {
    int button=0;
    int repeatcount=0;
    if (Input.press(Input.DOWN) && Input.press(Input.UP)) {
      return 0;
    }
    if (Input.press(Input.LEFT) && Input.press(Input.RIGHT)) {
      return 0;
    }
    foreach (int b in new int[] { Input.DOWN, Input.LEFT, Input.RIGHT, Input.UP }) {
      int rc=Input.count(b);
      if (rc>0) {
        if (repeatcount==0 || rc<repeatcount) {
          button=b;
          repeatcount=rc;
        }
      }
    }
    return button;
  }

  //public static int dir8() {
  //  List<KeyValuePair<int,int>> buttons=new List<KeyValuePair<int, int>>();
  //  foreach (var b in new int[] { Input.DOWN, Input.LEFT, Input.RIGHT, Input.UP }) {
  //    int rc=Input.count(b);
  //    if (rc>0) {
  //      buttons.Add(new KeyValuePair<int, int>(b,rc));
  //    }
  //  }
  //  if (buttons.Count==0) {
  //    return 0;
  //  } else if (buttons.Count==1) {
  //    return buttons[0][0];
  //  } else if (buttons.Count==2) {
  //    //  since buttons sorted by button, no need to sort here
  //    if ((buttons[0][0]==Input.DOWN && buttons[1][0]==Input.UP)) {
  //      return 0;
  //    }
  //    if ((buttons[0][0]==Input.LEFT && buttons[1][0]==Input.RIGHT)) {
  //      return 0;
  //    }
  //  }
  //  //buttons.sort!{|a,b| a[1]<=>b[1]}
  //  buttons.Sort();
  //  int updown=0;
  //  int leftright=0;
  //  foreach (var b in buttons) {
  //    if (updown==0 && (b.Key==Input.UP || b.Key==Input.DOWN)) {
  //      updown=b.Key;
  //    }
  //    if (leftright==0 && (b.Key==Input.LEFT || b.Key==Input.RIGHT)) {
  //      leftright=b.Key;
  //    }
  //  }
  //  if (updown==Input.DOWN) {
  //    if (leftright==Input.LEFT) return 1;
  //    if (leftright==Input.RIGHT) return 3;
  //    return 2;
  //  } else if (updown==Input.UP) {
  //    if (leftright==Input.LEFT) return 7;
  //    if (leftright==Input.RIGHT) return 9;
  //    return 8;
  //  } else {
  //    if (leftright==Input.LEFT) return 4;
  //    if (leftright==Input.RIGHT) return 6;
  //    return 0;
  //  }
  //}

  public static int count(int button) {
    foreach (var btn in Input.buttonToKey(button)) {
      int c=Input.repeatcount(btn);
      if (c>0) return c;
    }
    return 0;
  }

  public static bool release(int button) {
    int rc=0;
    foreach (var btn in Input.buttonToKey(button)) {
      int c=Input.repeatcount(btn);
      if (c>0) return false;
      if (Input.releaseex(btn)) rc+=1;
    }
    return rc>0;
  }

  public static bool trigger(int button) {
    return Input.buttonToKey(button).Any(item => Input.triggerex(item));
  }

  public static bool repeat(int button) {
    return Input.buttonToKey(button).Any(item => Input.repeatex(item));
  }

  public static bool press(int button) {
    return Input.count(button)>0;
  }

  public static bool repeatex(int key) {
    if (@keystate == null) return false;
    updateKeyState(key);
    return @keystate[key]==1 || (@keystate[key]>20 && (@keystate[key]&1)==0);
  }

  public static bool releaseex(int key) {
    if (@releasestate == null) return false;
    updateKeyState(key);
    return @releasestate[key];
  }

  public static bool triggerex(int key) {
    if (@triggerstate == null) return false;
    updateKeyState(key);
    return @triggerstate[key];
  }

  public static int repeatcount(int key) {
    if (@keystate == null) return 0;
    updateKeyState(key);
    return @keystate[key];
  }

  public static bool pressex(int key) {
    return Input.repeatcount(key)>0;
  }
}



// Requires Win32API
/*public static partial class Mouse {
  gsm = new Win32API('user32', 'GetSystemMetrics', 'i', 'i');
  @GetCursorPos = new Win32API('user32', 'GetCursorPos', 'p', 'i');
  @SetCapture = new Win32API('user32', 'SetCapture', 'p', 'i');
  @ReleaseCapture = new Win32API('user32', 'ReleaseCapture', '', 'i');
  module_function
  public void getMouseGlobalPos() {
    pos = new []{ 0, 0 }.pack('ll');
    if (@GetCursorPos.call(pos) != 0) {
      return pos.unpack('ll');
    } else {
      return null;
    }
  }

  public void screen_to_client(x, y) {
    unless (x and y) return null;
    screenToClient = new Win32API('user32', 'ScreenToClient', %w(l p), 'i');
    pos = new []{ x, y }.pack('ll');
    if (screenToClient.call(Win32API.pbFindRgssWindow, pos) != 0) {
      return pos.unpack('ll');
    } else {
      return null;
    }
  }

  public void setCapture() {
    @SetCapture.call(Win32API.pbFindRgssWindow);
  }

  public void releaseCapture() {
    @ReleaseCapture.call;
  }

//  Returns the position of the mouse relative to the game window.
  public void getMousePos(catch_anywhere = false) {
    resizeFactor=($ResizeFactor) ? $ResizeFactor : 1;
    x, y = screen_to_client(*getMouseGlobalPos);
    width, height = Win32API.client_size;
    if (catch_anywhere || (x >= 0 and y >= 0 and x < width and y < height)) {
      return (x/resizeFactor).to_i, (y/resizeFactor).to_i;
    } else {
      return null;
    }
  }

  public void del() {
    if (@oldcursor == null) {
      return;
    } else {
      @SetClassLong.call(Win32API.pbFindRgssWindow,-12, @oldcursor);
      @oldcursor = null;
    }
  }
}*/
}
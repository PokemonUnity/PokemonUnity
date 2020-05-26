using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//[RequireComponent(typeof(TMP_Animated))]
public class CanvasUIHandler : UnityEngine.MonoBehaviour
{
	public CanvasUI CurrentCanvas;
	public UICanvas ActiveCanvasUI { get; private set; }
	/// <summary>
	/// This is text-script meant to be rendered by front-end UI
	/// </summary>
	//private TMP_Animated DialogTextTMP { get; set; }
	private string DialogText { get; set; }
	/// <summary>
	/// This is the text that's displayed to user on front-end.
	/// The text is properly formatted for real-time rendering display in Unity.
	/// </summary>
	private static UnityEngine.UI.Text DialogUIText { get; set; }// = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
	/// <summary>
	/// This is the text that's hidden from user, and is used in background processing.
	/// The text is the final rendered display of the text properly formatted for UI in Unity
	/// </summary>
	private static UnityEngine.UI.Text DialogUITextDump { get; set; }// = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
	private static UnityEngine.UI.Text DialogUIScrollText;// = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
	
	#region Unity Stuff
	void Awake()
	{
		//Game.SetCanvasManager(this);
		//Event onload = Game.Onload;
		//onload.event += private void
		//DialogTextTMP = UnityEngine.GameObject.Find("DialogTextTMP").GetComponent<TMP_Animated>();//GetComponent<TMP_Animated>();
		DialogUIText = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
		DialogUITextDump = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
		DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
	}

	public void Start()
	{
		ShowMenu(CurrentCanvas);
	}

	/// <summary>
	/// Not needed, just wanted to test features...
	/// </summary>
	void Update()
	{
		//Test dialog skin
		//if (UnityEngine.Input.anyKeyDown) RefreshWindowSkin();
	}

	void ChangeScene(UICanvas scene)
	{
		if (ActiveCanvasUI == scene) return;
		else
		{
			UnityEngine.CanvasGroup cgroup;
			UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("CanvasUI");
			switch (scene)
			{
				case UICanvas.DialogWindow:
					foreach(UnityEngine.GameObject go in gos)
					{
						cgroup = go.GetComponent<UnityEngine.CanvasGroup>();
						if (go.name == scene.ToString())
						{
							go.SetActive(true);
							ActiveCanvasUI = scene;
							cgroup.blocksRaycasts = cgroup.interactable = true;
						}
						else
						{
							go.SetActive(false);
							cgroup.blocksRaycasts = cgroup.interactable = false;
						}
					}
					break;
				case UICanvas.MainMenu:
				case UICanvas.SettingsMenu:
					foreach(UnityEngine.GameObject go in gos)
					{
						if (go.name == scene.ToString())
						{
							go.SetActive(true);
							ActiveCanvasUI = scene;
							cgroup = go.GetComponent<UnityEngine.CanvasGroup>();
							cgroup.blocksRaycasts = cgroup.interactable = true;
						}
						else go.SetActive(false);
					}
					break;
				case UICanvas.NONE:
				default:
					foreach(UnityEngine.GameObject go in gos)
					{
						go.SetActive(false);
					}
					//ClearDialogText();
					break;
			}
		}
	}

	public void ShowMenu(CanvasUI screen)
	{
		if (CurrentCanvas != null) CurrentCanvas.IsActive = false;

		CurrentCanvas = screen;
		CurrentCanvas.IsActive = true;
	}
	#endregion

	#region Enumerator
	public enum UICanvas
	{
		NONE,
		/// <summary>
		/// DialogTextWindow:
		/// This is the Text Window for all dialog used throughout game 
		/// (between the Player and the Game), regardless of their purpose
		/// </summary>
		DialogWindow,
		/// <summary>
		/// StartupMenu:
		/// The main menu that occurs right after the StartupScene/Sequence
		/// </summary>
		MainMenu,
		/// <summary>
		/// StartupMenu_Settings:
		/// The settings option located on the <seealso cref="MainMenu"/> Screen
		/// </summary>
		SettingsMenu
	}
	#endregion

	#region Methods
	/// <summary>
	/// Reassigns all the <seealso cref="UnityEngine.Sprite"/> images 
	/// for every <seealso cref="UnityEngine.GameObject"/> with the tag "DialogWindow"
	/// </summary>
	public static void RefreshWindowSkin()
	{
		//if (Game.WindowSkinSprite != null)
		//{
		//	UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("DialogWindow");
		//	foreach (UnityEngine.GameObject go in gos)
		//	{
		//		go.GetComponent<UnityEngine.UI.Image>().sprite = Game.WindowSkinSprite;
		//	}
		//}
		//else return;
	}

	/// <summary>
	/// Reassigns all the <seealso cref="UnityEngine.Sprite"/> images 
	/// for every <seealso cref="UnityEngine.GameObject"/> with the tag "DialogWindow"
	/// </summary>
	public static void RefreshDialogSkin()
	{
		//if (Game.DialogSkinSprite != null)
		//{
		//	UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("DialogWindow");
		//	foreach (UnityEngine.GameObject go in gos)
		//	{
		//		go.GetComponent<UnityEngine.UI.Image>().sprite = Game.DialogSkinSprite;
		//	}
		//}
		//else return;
	}
	#endregion

	public class CanvasUI : UnityEngine.MonoBehaviour
	{
		private UnityEngine.GameObject _canvasScene;
		private UnityEngine.CanvasGroup _canvasGroup;
		private UnityEngine.Animator _animator;
		//public UICanvas CanvasUITag { get; private set; }

		private bool _isActive
		{
			get
			{
				return _canvasScene.activeSelf;
			}
			set
			{
				_canvasScene.SetActive(value);
			}
		}
		public bool IsActive
		{
			get
			{
				return _animator.GetBool("");
			}
			set
			{
				_animator.SetBool("", value);
			}
		}

		public void Awake()
		{
			_canvasScene = gameObject;
			_animator = GetComponent<UnityEngine.Animator>();
		}

		public void Update()
		{
			//if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(""))
			//    _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
			//else _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
		}
	}
}
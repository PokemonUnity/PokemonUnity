using System.Collections;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	[ExecuteInEditMode]
	public class TypingForm : UGuiForm
	{
		//[SerializeField]
		//private RectTransform m_Transform = null;
		//[SerializeField]
		//private float m_ScrollSpeed = 1f;
		//private float m_InitPosition = 0f;
		
		//private bool UseKeyboard;
		private System.Text.StringBuilder typeSpaceText;
		//public string typedString;

		public int selectorIndex  = 0;
		public int pageIndex      = 0;
		public int typeSpaceIndex = 0;
		public int charLimit      = 12;
		
		/// <summary>
		/// If the typing screen is for naming a Player, PC Box, or a Pokemon; 
		/// Change the icon to match sprite that represents noun.
		/// </summary>
		private UnityEngine.Sprite   icon;
		public  UnityEngine.Sprite   PC_Icon;
		public  UnityEngine.Sprite[] PageBackground;
		public  UnityEngine.UI.Image Page;
		//public  CharKeyItem         CharKeyPrefab;
		//public  GameObject          CharKeyPrefab;
		//public  GameObject          PagePrefab;
		
		private GameFrameworkAction<object> m_OnClickConfirm = null;

		public string TypeEntryText { get { return typeSpaceText.ToString(); } }

		public static readonly string[] PageCharArray = new string[] {
			"QWERTYUIOP ,." +
			"ASDFGHJKL  '-" +
			" ZXCVBNM   ♂♀" +
			"             " +
			"0123456789   ",
			"qwertyuiop ,." +
			"asdfghjkl  '-" +
			" zxcvbnm   ♂♀" +
			"             " +
			"0123456789   ",
			",.:;!?   ♂♀  " +
			"“”‘’()       " +
			"…·~@#%+-*/=  " +
			"◎○□△◇♠♥♦♣★♪  " +
			"☀☁☂☃     ⤴⤵  "
		};

		#region UnityMonobehavior
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);

			//CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
			//if (canvasScaler == null)
			//{
			//	Log.Warning("Can not find CanvasScaler component.");
			//	return;
			//}

			//m_InitPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;
			/*for (int i = 0; i < 56; i++)
			{
				GameObject go = (GameObject)Instantiate(original: CharKeyPrefab);//, parent: this.transform
				//CharKeyItem key = Instantiate(original: CharKeyPrefab, parent: this.transform);
				CharKeyItem key = go.GetComponent<CharKeyItem>();
				key.CharKeyIndex = i;
			}*/
			
			if(GameEntry.UseKeyboard) 
				StartCoroutine(TypeWithKeyboard());
		}

		protected override void OnOpen(object userData)
		{
			//if (GameMode != Online)
				GameEntry.Base.PauseGame();
			
			base.OnOpen(userData);
			
			for (int i = 0; i < 56; i++)
			{
				//GameObject go = (GameObject)Instantiate(original: CharKeyPrefab);//, parent: this.transform
				//CharKeyItem key = Instantiate(original: CharKeyPrefab, parent: this.transform);
				//CharKeyItem key = go.GetComponent<CharKeyItem>();
				//key.CharKeyIndex = i;
				GameEntry.CharKey.ShowCharKey(i);
			}

			/* ToDo: Create struct object to hold param data
			TypingParams typingParams = (TypingParams)userData;
			if (dialogParams == null)
			{
				//Should contain:
				//typing mode: is naming a Player, PC Box, or a Pokemon;
				//typing value: if pokemon, the id and gender, if player gender and sprite, else null
				Log.Warning("TypingParams is invalid.");
				return;
			}*/

			//m_Transform.SetLocalPositionY(m_InitPosition);

			// 换个音乐
			GameEntry.Sound.PlayMusic(3);
		}

		protected override void OnClose(bool isShutdown, object userData)
		{
			//if (GameMode != Online)
				GameEntry.Base.ResumeGame();

			icon = null;
			//typedString = null;
			typeSpaceText = null;
			
			for (int i = 0; i < 56; i++)
			{
				GameEntry.CharKey.HideCharKey(i);
			}
			
			base.OnClose(isShutdown, userData);

			// 还原音乐
			GameEntry.Sound.PlayMusic(1);
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			//m_Transform.AddLocalPositionY(m_ScrollSpeed * elapseSeconds);
			//if (m_Transform.localPosition.y > m_Transform.sizeDelta.y - m_InitPosition)
			//{
			//	m_Transform.SetLocalPositionY(m_InitPosition);
			//}
		}
		#endregion

		public void OnKeysButtonClick()
		{
			GameEntry.UseKeyboard = !GameEntry.UseKeyboard;
			//image.enabled = GameEntry.UseKeyboard;
			
			if(GameEntry.UseKeyboard) 
				StartCoroutine(TypeWithKeyboard());
		}

		//public void OnConfirmButtonClick()
		//{
		//	Close();
		//
		//	if (m_OnClickConfirm != null)
		//	{
		//		m_OnClickConfirm(m_UserData);
		//	}
		//}

		//public void OnBackButtonClick()
		//{
		//	Close();
		//
		//	if (m_OnClickBack != null)
		//	{
		//		m_OnClickBack(m_UserData);
		//	}
		//}

		#region MyRegion
		private bool CanBackspace() { if (typeSpaceIndex == 0) return false; else return true; }
		private void SelectUp() { typeSpaceIndex++; if (typeSpaceIndex > 12) typeSpaceIndex = 12; }
		private void SelectDown() { typeSpaceIndex--; if (typeSpaceIndex < 0) typeSpaceIndex = 0; }

		private void addCharacterToString(char character, char capsCharacter)
		{
			if (pageIndex == 3)
			{
				if (typeSpaceIndex < charLimit)
				{
					if (!GameEntry.IsCapLockOn)
					{
						//typeSpaceText[typeSpaceIndex].text = character;
						typeSpaceText[typeSpaceIndex] = character;
					}
					else
					{
						//typeSpaceText[typeSpaceIndex].text = capsCharacter;
						typeSpaceText[typeSpaceIndex] = capsCharacter;
					}
					//typeSpaceTextShadow[typeSpaceIndex].text = typeSpaceText[typeSpaceIndex].text;
					typeSpaceIndex += 1;
				}
			}
		}

		public void Backspace()
		{
			if (typeSpaceIndex > 0)
			{
				for(int a = typeSpaceIndex - 1; a < charLimit-1; a++)
				{
					//Backspace removes char infront, by replacing it with any character that follows after
					//typeSpaceText[a].text = typeSpaceText[a + 1].text;
					//typeSpaceTextShadow[a].text = typeSpaceText[a].text;
					typeSpaceText[a] = typeSpaceText[a + 1];
				}
				//Last char is replaced with an empty, since length shifted down by one
				//typeSpaceText[charLimit].text = ' ';
				//typeSpaceTextShadow[charLimit].text = typeSpaceText[charLimit].text;
				typeSpaceText[charLimit] = ' ';
				//typeSpaceIndex -= 1;
				SelectDown();
				//panelButton[4].enabled = true;
				//yield return new WaitForSeconds(0.1f);
				//panelButton[4].enabled = false;
			}
		}

		/*// Remove forward facing character
		private IEnumerator Delete()
		{
			if (typeSpaceIndex > 0)
			{
				typeSpaceText[typeSpaceIndex - 1].text = ' ';
				typeSpaceTextShadow[typeSpaceIndex - 1].text = typeSpaceText[typeSpaceIndex - 1].text;
				//typeSpaceIndex -= 1;
				SelectDown();
				//panelButton[4].enabled = true;
				yield return new WaitForSeconds(0.1f);
				//panelButton[4].enabled = false;
			}
		}*/

		private IEnumerator TypeWithKeyboard()
		{
			//keyboardText.text = "Press Esc to stop typing";
			//keyboardTextShadow.text = keyboardText.text;
			yield return null;

			while (GameEntry.UseKeyboard)
			{
				if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.CapsLock))
				{
					GameEntry.IsCapLockOn = !GameEntry.IsCapLockOn;
				}
				if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
				{
					GameEntry.IsCapLockOn = !GameEntry.IsCapLockOn;
				}

				if (Input.GetKeyDown(KeyCode.BackQuote))
				{
					addCharacterToString('‘', '~');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					addCharacterToString('1', '!');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					addCharacterToString('2', '@');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					addCharacterToString('3', '#');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					addCharacterToString('4', '4');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					addCharacterToString('5', '%');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					addCharacterToString('6', '6');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					addCharacterToString('7', '&');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					addCharacterToString('8', '*');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					addCharacterToString('9', '(');
				}
				else if (Input.GetKeyDown(KeyCode.Alpha0))
				{
					addCharacterToString('0', ')');
				}

				else if (Input.GetKeyDown(KeyCode.Q))
				{
					addCharacterToString('q', 'Q');
				}
				else if (Input.GetKeyDown(KeyCode.W))
				{
					addCharacterToString('w', 'W');
				}
				else if (Input.GetKeyDown(KeyCode.E))
				{
					addCharacterToString('e', 'E');
				}
				else if (Input.GetKeyDown(KeyCode.R))
				{
					addCharacterToString('r', 'R');
				}
				else if (Input.GetKeyDown(KeyCode.T))
				{
					addCharacterToString('t', 'T');
				}
				else if (Input.GetKeyDown(KeyCode.Y))
				{
					addCharacterToString('y', 'Y');
				}
				else if (Input.GetKeyDown(KeyCode.U))
				{
					addCharacterToString('u', 'U');
				}
				else if (Input.GetKeyDown(KeyCode.I))
				{
					addCharacterToString('i', 'I');
				}
				else if (Input.GetKeyDown(KeyCode.O))
				{
					addCharacterToString('o', 'O');
				}
				else if (Input.GetKeyDown(KeyCode.P))
				{
					addCharacterToString('p', 'P');
				}

				else if (Input.GetKeyDown(KeyCode.A))
				{
					addCharacterToString('a', 'A');
				}
				else if (Input.GetKeyDown(KeyCode.S))
				{
					addCharacterToString('s', 'S');
				}
				else if (Input.GetKeyDown(KeyCode.D))
				{
					addCharacterToString('d', 'D');
				}
				else if (Input.GetKeyDown(KeyCode.F))
				{
					addCharacterToString('f', 'F');
				}
				else if (Input.GetKeyDown(KeyCode.G))
				{
					addCharacterToString('g', 'G');
				}
				else if (Input.GetKeyDown(KeyCode.H))
				{
					addCharacterToString('h', 'H');
				}
				else if (Input.GetKeyDown(KeyCode.J))
				{
					addCharacterToString('j', 'J');
				}
				else if (Input.GetKeyDown(KeyCode.K))
				{
					addCharacterToString('k', 'K');
				}
				else if (Input.GetKeyDown(KeyCode.L))
				{
					addCharacterToString('l', 'L');
				}
				else if (Input.GetKeyDown(KeyCode.Semicolon))
				{
					addCharacterToString(';', ':');
				}
				else if (Input.GetKeyDown(KeyCode.Quote))
				{
					addCharacterToString('’', '”');
				}

				else if (Input.GetKeyDown(KeyCode.Z))
				{
					addCharacterToString('z', 'Z');
				}
				else if (Input.GetKeyDown(KeyCode.X))
				{
					addCharacterToString('x', 'X');
				}
				else if (Input.GetKeyDown(KeyCode.C))
				{
					addCharacterToString('c', 'C');
				}
				else if (Input.GetKeyDown(KeyCode.V))
				{
					addCharacterToString('v', 'V');
				}
				else if (Input.GetKeyDown(KeyCode.B))
				{
					addCharacterToString('b', 'B');
				}
				else if (Input.GetKeyDown(KeyCode.N))
				{
					addCharacterToString('n', 'N');
				}
				else if (Input.GetKeyDown(KeyCode.M))
				{
					addCharacterToString('m', 'M');
				}
				else if (Input.GetKeyDown(KeyCode.Comma))
				{
					addCharacterToString(',', ',');
				}
				else if (Input.GetKeyDown(KeyCode.Period))
				{
					addCharacterToString('.', '.');
				}
				else if (Input.GetKeyDown(KeyCode.Slash))
				{
					addCharacterToString('/', '?');
				}
				else if (Input.GetKeyDown(KeyCode.Tab))
				{
					pageIndex = pageIndex + 1 % 2;
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					SelectDown();
				}
				else if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					SelectUp();
				}

				else if (Input.GetKeyDown(KeyCode.Space))
				{
					addCharacterToString(' ', ' ');
				}

				else if (Input.GetKeyDown(KeyCode.Backspace))
				{
					//if(CanBackspace())
					//	yield return StartCoroutine(Backspace());
				}

				//else if (Input.GetButtonDown("Back") && !Input.GetKeyDown(KeyCode.X))
				else if (Input.GetButtonDown("Back") && !Input.GetKeyDown(KeyCode.X))
				{
					//running = false;
					GameEntry.UseKeyboard = false;
				}

				yield return null;
			}

			//keyboardText.text = "Select to use the Keyboard";
			//keyboardTextShadow.text = keyboardText.text;
		}
		#endregion
	}
}
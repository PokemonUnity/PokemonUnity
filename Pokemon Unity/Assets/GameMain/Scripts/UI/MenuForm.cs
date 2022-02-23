//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public class MenuForm : UGuiForm
	{
		[SerializeField]
		private GameObject m_QuitButton = null;

		private ProcedureMenu m_ProcedureMenu = null;

		public void OnStartButtonClick()
		{
			m_ProcedureMenu.StartGame();
		}

		public void OnSettingButtonClick()
		{
			GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
		}

		public void OnAboutButtonClick()
		{
			GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
		}

		public void OnQuitButtonClick()
		{
			GameEntry.UI.OpenDialog(new DialogParams()
			{
				Mode = 2,
				Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
				Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
				OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
			});
		}
		public void OnContinueSavedGame()
		{
			//If Continue Option is select
			//if (MenuOptions.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn)
			//{
			//	switch (eventData.selectedObject.name)
			//	{
			//		//If the object is slots, submit continue
			//		case "":
			//		//If the object is continue, transistion to next scene
			//		case "1":
			//		default:
			//			break;
			//	}
			//    //Get Toggle Value from Toggle group for which toggleOption is selected
			//    //use gamesave toggle to load game from that slot
			//    //Game.Load();
			//}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);

			m_ProcedureMenu = (ProcedureMenu)userData;
			if (m_ProcedureMenu == null)
			{
				Log.Warning("ProcedureMenu is invalid when open MenuForm.");
				return;
			}

			/* If Continue option is available:
			 * file slot data should reflect in the 
			 * playerData window on the right side;
			 * disable slot options with no data
			 */

			m_QuitButton.SetActive(UnityEngine.Application.platform != RuntimePlatform.IPhonePlayer);
		}

		protected override void OnClose(bool isShutdown, object userData)
		{
			m_ProcedureMenu = null;

			base.OnClose(isShutdown, userData);
		}
	}
}

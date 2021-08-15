using UnityEngine;
using System.Collections;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace PokemonUnity
{
	/// <summary>
	/// Typing State Opens a UI Typing Form
	/// </summary>
	public class ProcedureTyping : ProcedureBase, IPokemonEntry
	{
		//bool GameEntry.IsCapLockOn = false;
		private bool UseKeyboard = true;
		private string m_Text = null;
		//Instead of using a prefab, will try use a scene instead
		//private TypingForm m_TypeForm = null;
		private static IPokemonEntryScene Scene = null;

		public override bool UseNativeDialog
		{
			get { return false; }
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{
			base.OnEnter(procedureOwner);

			// Subscribe to custom events from typing scene...
			//GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

			m_Text = "";
			//GameEntry.UI.OpenUIForm(UIFormId.TypingForm, this);
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);

			//GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

			//if (m_TypeForm != null)
			//{
			//	m_TypeForm.Close(isShutdown);
			//	m_TypeForm = null;
			//}

			if (Scene != null)
			{
				Scene.pbEndScene();
				Scene = null;
			}
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

			if (string.IsNullOrEmpty(m_Text))
			{
				procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Main"));
				//procedureOwner.SetData<VarInt>(Constant.ProcedureData.GameMode, (int)GameMode.Survival);
				ChangeState<ProcedureChangeScene>(procedureOwner);
			}
		}

		//private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
		//{
		//	OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
		//	if (ne.UserData != this)
		//	{
		//		return;
		//	}
		//
		//	m_TypeForm = (TypingForm)ne.UIForm.Logic;
		//}

		public void initialize(IPokemonEntryScene scene)
		{
			// Set variables
			Scene = scene;

			// Change Scene
			UnityEngine.SceneManagement.SceneManager.LoadScene("Scene.Main");
		}

		public string pbStartScreen(string helptext,int minlength,int maxlength,string initialText,UX.TextEntryTypes mode= 0,Monster.Pokemon pokemon= null)
		{
			@Scene.pbStartScene(helptext, minlength, maxlength, initialText, mode, pokemon);
			string ret = @Scene.pbEntry();
			@Scene.pbEndScene();
			return ret;
		}
	}
}
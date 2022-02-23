//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace PokemonUnity
{
	/// <summary>
	/// Preload scene is when a game boots up, display the logos of developers, and load assets.
	/// Usually this is displayed first as a way to credit everyones' contributions/copyright/sponsors,
	/// handles the asset loading process, then play intro animations, before the game begins
	/// </summary>
	public class ProcedurePreload : ProcedureBase
	{
		public static readonly string[] DataTableNames = new string[]
		{
			"Test", // 这是个测试资源，并没有使用

			"Aircraft",
			"Armor",
			"Asteroid",
			"Entity",
			"Music",
			"Scene",
			"Sound",
			"Thruster",
			"UIForm",
			"UISound",
			"Weapon",
		};

		private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

		public override bool UseNativeDialog
		{
			get
			{
				return true;
			}
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{
			base.OnEnter(procedureOwner);

			// Register event listeners to update as scene begins regular procedure
			GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
			GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
			GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
			GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
			GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
			GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

			m_LoadedFlag.Clear();

			PreloadResources();
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			// Since scene is no longer active, get rid of all of the event listeners registered
			GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
			GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
			GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
			GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
			GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
			GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

			// Store loading procedure into a `Dictionary<string name, bool loaded>`
			IEnumerator<bool> iter = m_LoadedFlag.Values.GetEnumerator();
			// Loop through each bool, if true move to next..
			while (iter.MoveNext())
			{
				// If bool is false
				if (!iter.Current)
				{
					// End update here and repeat process on next update
					return;
				}
			}

			// If all the stored procedures are loaded, move to next scene.
			procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Menu"));
			ChangeState<ProcedureChangeScene>(procedureOwner);
		}

		private void PreloadResources()
		{
			// Preload fonts
			//LoadFont("MainFont");

			// Preload configs
			//LoadConfig("DefaultConfig");

			// Preload data tables
			foreach (string dataTableName in DataTableNames)
			{
				//LoadDataTable(dataTableName);
			}

			// Preload dictionaries
			//LoadDictionary("Default");
		}

		/*private void LoadConfig(string configName)
		{
			m_LoadedFlag.Add(GameFramework.Utility.Text.Format("Config.{0}", configName), false);
			GameEntry.Config.LoadConfig(configName, LoadType.Bytes, this);
		}

		private void LoadDataTable(string dataTableName)
		{
			m_LoadedFlag.Add(GameFramework.Utility.Text.Format("DataTable.{0}", dataTableName), false);
			GameEntry.DataTable.LoadDataTable(dataTableName, LoadType.Bytes, this);
		}

		private void LoadDictionary(string dictionaryName)
		{
			m_LoadedFlag.Add(GameFramework.Utility.Text.Format("Dictionary.{0}", dictionaryName), false);
			GameEntry.Localization.LoadDictionary(dictionaryName, LoadType.Text, this);
		}*/

		private void LoadFont(string fontName)
		{
			m_LoadedFlag.Add(GameFramework.Utility.Text.Format("Font.{0}", fontName), false);
			GameEntry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
				(assetName, asset, duration, userData) =>
				{
					m_LoadedFlag[GameFramework.Utility.Text.Format("Font.{0}", fontName)] = true;
					UGuiForm.SetMainFont((Font)asset);
					Log.Info("Load font '{0}' OK.", fontName);
				},

				(assetName, status, errorMessage, userData) =>
				{
					Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage);
				}));
		}

		private void OnLoadConfigSuccess(object sender, GameEventArgs e)
		{
			LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			m_LoadedFlag[GameFramework.Utility.Text.Format("Config.{0}", ne.ConfigName)] = true;
			Log.Info("Load config '{0}' OK.", ne.ConfigName);
		}

		private void OnLoadConfigFailure(object sender, GameEventArgs e)
		{
			LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigName, ne.ConfigAssetName, ne.ErrorMessage);
		}

		private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
		{
			LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			m_LoadedFlag[GameFramework.Utility.Text.Format("DataTable.{0}", ne.DataTableName)] = true;
			Log.Info("Load data table '{0}' OK.", ne.DataTableName);
		}

		private void OnLoadDataTableFailure(object sender, GameEventArgs e)
		{
			LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
		}

		private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
		{
			LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			m_LoadedFlag[GameFramework.Utility.Text.Format("Dictionary.{0}", ne.DictionaryName)] = true;
			Log.Info("Load dictionary '{0}' OK.", ne.DictionaryName);
		}

		private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
		{
			LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryName, ne.DictionaryAssetName, ne.ErrorMessage);
		}
	}
}

//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using UnityEngine;
//using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public abstract class GameBase
	{
		public abstract GameMode GameMode
		{
			get;
		}

		//public abstract PokemonUnity.Game Game
		//{
		//    get;
		//}

		//protected ScrollableBackground SceneBackground
		//{
		//    get;
		//    private set;
		//}

		protected TMP_Animated DialogTextTMP
		{
		    get;
		    private set;
		}

		public bool GameOver
		{
			get;
			protected set;
		}

		//private MyAircraft m_MyAircraft = null;

		public virtual void Initialize()
		{
			//GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
			//GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

			DialogTextTMP = UnityEngine.GameObject.Find("DialogTextTMP").GetComponent<TMP_Animated>();//GetComponent<TMP_Animated>();
			//SceneBackground = Object.FindObjectOfType<ScrollableBackground>();
			//if (SceneBackground == null)
			//{
			//    Log.Warning("Can not find scene background.");
			//    return;
			//}
			//
			//SceneBackground.VisibleBoundary.gameObject.GetOrAddComponent<HideByBoundary>();
			//GameEntry.Entity.ShowMyAircraft(new MyAircraftData(GameEntry.Entity.GenerateSerialId(), 10000)
			//{
			//    Name = "My Aircraft",
			//    Position = Vector3.zero,
			//});

			GameOver = false;
			//m_MyAircraft = null;
		}

		public virtual void Shutdown()
		{
			//GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
			//GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
		}

		public virtual void Update(float elapseSeconds, float realElapseSeconds)
		{
			//if (m_MyAircraft != null && m_MyAircraft.IsDead)
			//{
			//    GameOver = true;
			//    return;
			//}
		}

		protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
		{
			//ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
			//if (ne.EntityLogicType == typeof(MyAircraft))
			//{
			//    m_MyAircraft = (MyAircraft)ne.Entity.Logic;
			//}
		}

		protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
		{
			//ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
			//Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
		}
	}
}

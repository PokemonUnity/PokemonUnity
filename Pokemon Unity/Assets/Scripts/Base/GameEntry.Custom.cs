//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace PokemonUnity
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static HPBarComponent HPBar
        {
            get;
            private set;
		}

		#region Unity Scene Manager
		//ToDo: This whole region to be redone... maybe as abstract/virtual?
		//public static CanvasUIHandler CanvasManager { get; private set; }
		//public static DialogHandler TextBox { get; private set; }
		//public static StartupSceneHandler StartScene { get; private set; }
		//public static BattlePokemonHandler BattleScene { get; private set; }
		////public static ItemHandler ItemScene { get; private set; }
		////public static SummaryHandler SummaryScene { get; private set; }
		////public static SettingsHandler SettingsScene { get; private set; }
		//#region Scene Manager Methods
		//public static void SetCanvasManager(CanvasUIHandler canvas) { CanvasManager = canvas; }
		//public static void SetStartScene(StartupSceneHandler start) { StartScene = start; }
		//#endregion
		#endregion

		private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
        }
    }
}

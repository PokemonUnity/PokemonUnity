//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Debugger;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    public class ChangeLanguageDebuggerWindow : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;
        private bool m_NeedRestart = false;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_NeedRestart)
            {
                m_NeedRestart = false;
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
            }
        }

        public void OnDraw()
        {
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionChangeLanguage();
            }
            GUILayout.EndScrollView();
        }

        private void DrawSectionChangeLanguage()
        {
            GUILayout.Label("<b>Change Language</b>");
            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Chinese Simplified", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.ChineseSimplified;
                    SaveLanguage();
                }
                if (GUILayout.Button("Chinese Traditional", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.ChineseTraditional;
                    SaveLanguage();
                }
                if (GUILayout.Button("English", GUILayout.Height(30)))
                {
                    GameEntry.Localization.Language = Language.English;
                    SaveLanguage();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void SaveLanguage()
        {
            GameEntry.Setting.SetString(Constant.Setting.Language, GameEntry.Localization.Language.ToString());
            GameEntry.Setting.Save();
            m_NeedRestart = true;
        }
    }
}

//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    public class SettingForm : UGuiForm
    {
        [SerializeField]
        private Toggle m_MusicMuteToggle = null;

        [SerializeField]
        private Slider m_MusicVolumeSlider = null;

        [SerializeField]
        private Toggle m_SoundMuteToggle = null;

        [SerializeField]
        private Slider m_SoundVolumeSlider = null;

        [SerializeField]
        private Toggle m_UISoundMuteToggle = null;

        [SerializeField]
        private Slider m_UISoundVolumeSlider = null;

        [SerializeField]
        private CanvasGroup m_LanguageTipsCanvasGroup = null;

        [SerializeField]
        private Toggle m_EnglishToggle = null;

        [SerializeField]
        private Toggle m_ChineseSimplifiedToggle = null;

        [SerializeField]
        private Toggle m_ChineseTraditionalToggle = null;

        [SerializeField]
        private Toggle m_KoreanToggle = null;

        //private Language m_SelectedLanguage = Language.Unspecified;
        private Languages m_SelectedLanguage = Languages.English;

        public void OnMusicMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute("Music", !isOn);
            m_MusicVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute("Sound", !isOn);
            m_SoundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Sound", volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute("UISound", !isOn);
            m_UISoundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("UISound", volume);
        }

        public void OnEnglishSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Languages.English;
            RefreshLanguageTips();
        }

        public void OnChineseSimplifiedSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            //m_SelectedLanguage = Languages.ChineseSimplified;
            RefreshLanguageTips();
        }

        public void OnChineseTraditionalSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            //m_SelectedLanguage = Language.ChineseTraditional;
            RefreshLanguageTips();
        }

        public void OnKoreanSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            //m_SelectedLanguage = Language.Korean;
            RefreshLanguageTips();
        }

        public void OnSubmitButtonClick()
        {
            //if (m_SelectedLanguage == GameEntry.Localization.Language)
            if (m_SelectedLanguage == Game.UserLanguage)
            {
                Close();
                return;
            }

            GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
            GameEntry.Setting.Save();

            GameEntry.Sound.StopMusic();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_MusicMuteToggle.isOn = !GameEntry.Sound.IsMuted("Music");
            m_MusicVolumeSlider.value = GameEntry.Sound.GetVolume("Music");

            m_SoundMuteToggle.isOn = !GameEntry.Sound.IsMuted("Sound");
            m_SoundVolumeSlider.value = GameEntry.Sound.GetVolume("Sound");

            m_UISoundMuteToggle.isOn = !GameEntry.Sound.IsMuted("UISound");
            m_UISoundVolumeSlider.value = GameEntry.Sound.GetVolume("UISound");

            //m_SelectedLanguage = GameEntry.Localization.Language;
            m_SelectedLanguage = Game.UserLanguage;
            switch (m_SelectedLanguage)
            {
                case Languages.English:
                    m_EnglishToggle.isOn = true;
                    break;
                //case Language.ChineseSimplified:
                //    m_ChineseSimplifiedToggle.isOn = true;
                //    break;
                //case Language.ChineseTraditional:
                //    m_ChineseTraditionalToggle.isOn = true;
                //    break;
                //case Language.Korean:
                //    m_KoreanToggle.isOn = true;
                //    break;
                default:
                    break;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_LanguageTipsCanvasGroup.gameObject.activeSelf)
            {
                m_LanguageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }

        private void RefreshLanguageTips()
        {
            //m_LanguageTipsCanvasGroup.gameObject.SetActive(m_SelectedLanguage != GameEntry.Localization.Language);
            m_LanguageTipsCanvasGroup.gameObject.SetActive(m_SelectedLanguage != Game.UserLanguage);
        }
    }
}
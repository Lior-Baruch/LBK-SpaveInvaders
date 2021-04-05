using System;
using System.Collections.Generic;
using infastructure;
using Infrastructure.Managers;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel;

namespace Invaders.Screens.MenuManagers
{
    public class SoundSettingsScreenManuManager : MenuManager
    {
        const int k_NumberOfButtens = 4;
        const int k_RangeValue = 100;
        const int k_JumpValue = 10;

        public SoundSettingsScreenManuManager(GameScreen i_GameScreen)
        : base(i_GameScreen, k_NumberOfButtens)
        {
            if (m_SoundManager.ToggleSound)
            {
                this.m_ButtenList[0].TextList = new List<string> { "Toggle Sound: ON", "Toggle Sound: OFF" };
            }
            else
            {
                this.m_ButtenList[0].TextList = new List<string> { "Toggle Sound: OFF", "Toggle Sound: ON" };
            }

            this.m_ButtenList[0].TextList = new List<string> { "Toggle Sound: ON", "Toggle Sound: OFF" };
            this.m_ButtenList[1].RangeValueButten(k_RangeValue, k_JumpValue, "Background Music Volume: ");
            this.m_ButtenList[2].RangeValueButten(k_RangeValue, k_JumpValue, "Sound Effect Volume: ");
            this.m_ButtenList[3].Text = "Done";

            m_ButtenList[0].OnTextChange += Button_OnClickToggleSound;
            m_ButtenList[1].OnTextChange += Button_OnClcikBackgroundMusicVolume;
            m_ButtenList[2].OnTextChange += Button_OnClcikSoundEffectVolume;
            m_ButtenList[3].OnClick += MenuManager_OnClickReturn;
            GetMaxTextSize();
            m_ScreenTilte.Text = "Sound Settings";
            SetButtonSize();
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SoundManager.BackgroundMusicVolume = 1;
            m_SoundManager.SoundsEffectsVolume = 1;
        }

        public void ToggelWasChnaged()
        {
            this.m_ButtenList[0].ButtonNextText(); 
        }

        private void Button_OnClcikBackgroundMusicVolume(object sender, EventArgs e)
        {
            GameButten button = sender as GameButten;
            m_SoundManager.BackgroundMusicVolume = button.GetFloatValue();
        }

        private void Button_OnClcikSoundEffectVolume(object sender, EventArgs e)
        {
            GameButten button = sender as GameButten;
            m_SoundManager.SoundsEffectsVolume = button.GetFloatValue();
        }

        private void Button_OnClickToggleSound(object sender, EventArgs e)
        {
            m_SoundManager.ToggleSound = !m_SoundManager.ToggleSound;
        }
    }
}

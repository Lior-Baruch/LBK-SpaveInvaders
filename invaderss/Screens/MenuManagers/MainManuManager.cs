using System;
using System.Collections.Generic;
using Invaders.Screens;
using infastructure;
using Infrastructure.Managers;
using Infrastructure.ObjectModel.Screens;
using Invaders.Screens.MenuManagers;
using Microsoft.Xna.Framework;

namespace Invaders.Screens.MenuManagers
{
    public class MainManuManager : MenuManager
    {
        const int k_NumberOfButtens = 5;

        public MainManuManager(GameScreen i_GameScreen)
        : base(i_GameScreen, k_NumberOfButtens)
        {
            this.m_ButtenList[0].Text = "ScreenSettings";
            this.m_ButtenList[1].TextList = new List<string> { "two player's", "one player" };
            this.m_ButtenList[2].Text = "sound Sittings";
            this.m_ButtenList[3].Text = " Play Game";
            this.m_ButtenList[4].Text = "EXIT";
            m_ButtenList[0].OnClick += Button_OnClickScreenSittings;
            m_ButtenList[1].OnClick += Button_onClcikPlayerCount;
            m_ButtenList[1].OnTextChange += Button_onClcikPlayerCount;
            m_ButtenList[2].OnClick += _Button_OnClickSoundSettings;
            m_ButtenList[3].OnClick += Button_OnClcikPlay;
            m_ButtenList[4].OnClick += this.Button_OnClickExit;
            GetMaxTextSize();
            m_ScreenTilte.Text = "Main Menu";
            SetButtonSize();
        }

        private void Button_onClcikPlayerCount(object sender, EventArgs e)
        {
            ChangePlayerNumber();
        }

        protected override void Button_OnClcikPlay(object sender, EventArgs e)
        {
            if (m_SinglePlayer)
            {
                m_MyScreen.ScreensManager.SetCurrentScreen(new NextLevelTransitionScreen(m_MyScreen.Game, 0, -1, 0));
            }
            else
            {
                m_MyScreen.ScreensManager.SetCurrentScreen(new NextLevelTransitionScreen(m_MyScreen.Game));
            }
        }

        private void _Button_OnClickSoundSettings(object sender, EventArgs e)
        {
            m_MyScreen.ScreensManager.SetCurrentScreen(new SoundSettingsScreen(m_MyScreen.Game)); //// new Sound setting screen  
        }

        private void Button_OnClickScreenSittings(object sender, EventArgs e)
        {
            m_MyScreen.ScreensManager.SetCurrentScreen(new ScreenSettingsScreen(m_MyScreen.Game)); //// new Screen settings screen 
        }

        private void ChangePlayerNumber()
        {
            m_SinglePlayer = !m_SinglePlayer;
        }
    }
}

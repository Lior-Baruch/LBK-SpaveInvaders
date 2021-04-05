using System;
using Infrastructure.Managers;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.Screens.MenuManagers
{
    public class GameOverManuManager : MenuManager
    {
        const int k_NumberOfButtens = 3;

        public GameOverManuManager(GameScreen i_GameScreen, bool i_IsWelcomeScreen)
         : base(i_GameScreen, k_NumberOfButtens)
        {
            this.m_ButtenList[0].Text = "New Game";
            this.m_ButtenList[1].Text = "Main Manu";
            this.m_ButtenList[2].Text = "EXIT";

            m_ButtenList[0].OnClick += Button_OnClcikPlay;
            m_ButtenList[1].OnClick += Butten_OnClickMainManuScreen;
            m_ButtenList[2].OnClick += this.Button_OnClickExit;
            GetMaxTextSize();

            if (i_IsWelcomeScreen)
            {
                m_MenuLabel.Text = @"Press [ENTER] To Start The Game.   Press [M] for Main Manue.   press [ESC] To Exit.";
                m_ScreenTilte.Text = @"SPACE INVADERS";
            }
            else
            {
                m_MenuLabel.Text = @"Press [HOME] To Start The Game.   Press [M] For Main Manue.   Press [ESC] To Exit.";
                m_ScreenTilte.TextScale = 2;
            }

            SetButtonSize();
        }

        private void Butten_OnClickMainManuScreen(object sender, EventArgs e)
        {
            m_MyScreen.ScreensManager.SetCurrentScreen(new MainManuScreen(m_MyScreen.Game)); // new Screen MainMANU screen 
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
    }
}

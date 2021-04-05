using System;
using Infrastructure.ObjectModel.Screens;
using Invaders.ObjectModel;
using Invaders.Screens.MenuManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    class GameOverScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private string m_MassageBox;
        private Background m_BackRound;
        private bool m_IsSinglePlyer;
        private int m_Player2Score;
        private int m_Player1Score;
        private GameOverManuManager m_ManuManager;
        
        public GameOverScreen(Game i_Game, int i_Scoreplayer1, int i_ScorePlayer2)
           : base(i_Game)
        {
            m_BackRound = new Background(this);
            m_IsSinglePlyer = false;
            m_Player1Score = i_Scoreplayer1;
            m_Player2Score = i_ScorePlayer2;            
        }
       
        public override void Initialize()
        {
            base.Initialize();
            m_ManuManager = new GameOverManuManager(this, false);
            m_ManuManager.ExitScreen += ManuManager_OnclickExitGameOverScreen;
            m_ManuManager.Title.Text = m_MassageBox;
            m_ManuManager.SetButtonSize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            if (m_Player2Score == -1)
            {
                m_IsSinglePlyer = true;
            }

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
            m_MassageBox = string.Empty;
            if (m_IsSinglePlyer)
            {
                m_MassageBox = "GAME OVER :(" + Environment.NewLine + "Your Score is: " + m_Player1Score; //// player 1 socre 
            }
            else
            {
                m_MassageBox = string.Empty; //// player one nad two socre 
                string WinnerMessage;
                if (m_Player1Score > m_Player2Score)
                {
                    WinnerMessage = "Player1 Wins!";
                }
                else if (m_Player1Score < m_Player2Score)
                {
                    WinnerMessage = "Player2 Wins!";
                }
                else
                {
                    WinnerMessage = "It is a tie!";
                }

                string PlayerOnePointsMsg = "Player1: " + m_Player1Score.ToString() + " points! ";
                string PlayerTwoPointsMsg = "Player2: " + m_Player2Score.ToString() + " points!";

                m_MassageBox = WinnerMessage + Environment.NewLine + PlayerOnePointsMsg + Environment.NewLine + PlayerTwoPointsMsg;
            }
        }
       
        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                ExitGameOverScreen();
            }
            else if (InputManager.KeyPressed(Keys.Enter))
            {
                ////StartGame
                this.ScreensManager.SetCurrentScreen(new SpaceInvadersScreen(this.Game));
            }
            else if (InputManager.KeyPressed(Keys.M))
            {
                ////Go to main menu
                MainManuScreen mainMenuScreen = new MainManuScreen(Game);
                this.ScreensManager.SetCurrentScreen(mainMenuScreen);
            }
        }

        private void ExitGameOverScreen()
        {
            this.ExitScreen();
            this.Game.Exit();
        }

        public void ManuManager_OnclickExitGameOverScreen(object sender, EventArgs e)
        {
            ExitGameOverScreen();
        }
    }
}
using System;
using Infrastructure.ObjectModel.Screens;
using Invaders.ObjectModel;
using Invaders.Screens.MenuManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class WelcomeScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private Background m_BackRound;
        private MainManuScreen m_MainmanueScreen;
        private GameOverManuManager m_ManuManager;
       
        public WelcomeScreen(Game i_Game) : base(i_Game)
        {
            m_BackRound = new Background(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
        }

        public override void Initialize()
        {
            base.Initialize();
            m_ManuManager = new GameOverManuManager(this, true);
            m_ManuManager.ExitScreen += ManuManager_OnclickExitGameOverScreen;
        }
      
        public override void Update(GameTime I_GameTime)
        {
            base.Update(I_GameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                this.ExitScreen();
            }
            else if (InputManager.KeyPressed(Keys.Enter))
            {
                this.ScreensManager.SetCurrentScreen(new NextLevelTransitionScreen(this.Game));
            }
            else if(InputManager.KeyPressed(Keys.M))
            {            
                m_MainmanueScreen = new MainManuScreen(Game);
                this.ScreensManager.SetCurrentScreen(m_MainmanueScreen);
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
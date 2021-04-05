using System;
using Infrastructure.Managers;
using Infrastructure.ObjectModel.Screens;
using Invaders.ObjectModel;
using Invaders.Screens.MenuManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.Screens
{
    public class MainManuScreen : GameScreen, IGameComponent
    {
        private SpriteFont m_FontCalibri;
        private Background m_BackRound;
        private MainManuManager m_ManuManager;
        
        public MainManuScreen(Game i_Game)
            : base(i_Game)
        {
            m_BackRound = new Background(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_ManuManager = new MainManuManager(this);
            m_ManuManager.ExitScreen += manuManager_OnClickExit;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
        }

        private void manuManager_OnClickExit(object sender, EventArgs e)
        {
            this.Game.Exit();
            this.ExitScreen(); //// exit game 
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp) || InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown))
            {
                m_ManuManager.IsSinglePlayer = !m_ManuManager.IsSinglePlayer;
            }
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                this.ScreensManager.SetCurrentScreen(new NextLevelTransitionScreen(this.Game));
            }
        }
    }
}
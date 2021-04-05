using System;
using System.ComponentModel;
using infastructure;
using Infrastructure.ObjectModel.Screens;
using Invaders.ObjectModel;
using Invaders.Screens;
using Invaders.Screens.MenuManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class SoundSettingsScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(0, 0);
        private Background m_backround;
        private SoundSettingsScreenManuManager m_MainmanueScreen;

        public SoundSettingsScreen(Game i_Game)
            : base(i_Game)
        {
            m_backround = new Background(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_MainmanueScreen = new SoundSettingsScreenManuManager(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputManager.KeyPressed(Keys.M))
            {
                m_MainmanueScreen.ToggelWasChnaged();
            }
        }
    }
    }
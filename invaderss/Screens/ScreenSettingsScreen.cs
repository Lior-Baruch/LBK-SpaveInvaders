using System;
using System.ComponentModel;
using Infrastructure.ObjectModel.Screens;
using Invaders.ObjectModel;
using Invaders.Screens;
using Invaders.Screens.MenuManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class ScreenSettingsScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(0, 0);
        private Background m_BackRound;
        private ScreenSettingsManuManager m_ScreenManuManager;

        public ScreenSettingsScreen(Game i_Game)
            : base(i_Game)
        {
            m_BackRound = new Background(this);
        }

        public override void Initialize()
        {
            base.Initialize();
           m_ScreenManuManager = new ScreenSettingsManuManager(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
        }
    }
}
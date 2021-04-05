using System;
using System.ComponentModel;
using Infrastructure.ObjectModel.Screens;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Invaders.Screens;
using Accessibility;

namespace Invaders.Screens
{
    class PauseScreen : GameScreen
    {
        private const int k_TextScale = 3;
        private SpriteFont m_FontCalibri;
        private string m_ScreenMasage;
        private Vector2 m_TextLength;
        private Vector2 m_MsgPosition = new Vector2(0, 0);

        public PauseScreen(Game i_Game) : base(i_Game)
        {
            this.IsOverlayed = true;
            this.BlackTintAlpha = 0.4f;
            this.UseGradientBackground = false;
            this.Game.Window.ClientSizeChanged += OnScreenChanges;
        }

        private void OnScreenChanges(object sender, EventArgs e)
        {
            m_MsgPosition = new Vector2((this.Game.Window.ClientBounds.Width - (k_TextScale * m_TextLength.X)) / 2, (this.Game.Window.ClientBounds.Height - m_TextLength.Y) / 2);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
            m_ScreenMasage = " Press (R) To Resume ";
        }

        public override void Initialize()
        {
            base.Initialize();
            m_TextLength = m_FontCalibri.MeasureString(m_ScreenMasage);
            m_MsgPosition = new Vector2((this.Game.Window.ClientBounds.Width - (k_TextScale * m_TextLength.X)) / 2, this.Game.Window.ClientBounds.Height - m_TextLength.Y) / 2;
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_FontCalibri, m_ScreenMasage, m_MsgPosition, Color.Red, 0, Vector2.Zero, k_TextScale, SpriteEffects.None, 0);
            SpriteBatch.End();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (InputManager.KeyPressed(Keys.R))
            {
                m_PreviousScreen.Enabled = true;
                ScreensManager.SetCurrentScreen(m_PreviousScreen);
            }
        }
    }
}
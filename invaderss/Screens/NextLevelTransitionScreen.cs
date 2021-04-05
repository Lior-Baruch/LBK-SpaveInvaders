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
    class NextLevelTransitionScreen : GameScreen
    {
        private const int k_TextScale = 3;
        private readonly int r_Player1Points;
        private readonly int r_Player2Points;
        private readonly int r_Player1LifeLeft;
        private readonly int r_Player2LifeLeft;
        private int m_GameLevel;
        private Background m_BackRound;
        private float m_CountDownTime = 2.5f;
        private Vector2 m_TimerPosition;
        private SpriteFont m_FontCalibri;
        private string m_ScreenMasage;
        private Vector2 m_MsgPosition = new Vector2(0, 0);
       
        public NextLevelTransitionScreen(Game i_Game, int i_Player1Points = 0, int i_Player2Points = 0, int i_GameLevel = 0, int i_Player1LifeLeft = 3, int i_Player2LifeLeft = 3)
        : base(i_Game)
        {
            r_Player1Points = i_Player1Points;
            r_Player2Points = i_Player2Points;
            r_Player1LifeLeft = i_Player1LifeLeft;
            r_Player2LifeLeft = i_Player2LifeLeft;

            m_GameLevel = i_GameLevel;
            m_BackRound = new Background(this);
            this.Game.Window.ClientSizeChanged += onClientSizeChanged;
        }

        private void onClientSizeChanged(object sender, EventArgs e)
        {
            centereText();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Font\Consolas");
            m_ScreenMasage = " LEVEL " + (m_GameLevel + 1).ToString();
            centereText();
        }

        private void centereText()
        {
            Vector2 SizeOfTimerText = m_FontCalibri.MeasureString(Math.Ceiling(m_CountDownTime).ToString());
            m_TimerPosition = new Vector2((this.Game.Window.ClientBounds.Width - SizeOfTimerText.X) / 2, (this.Game.Window.ClientBounds.Height - SizeOfTimerText.Y) / 2);

            Vector2 SizeOfText = m_FontCalibri.MeasureString(m_ScreenMasage);
            m_MsgPosition = new Vector2((this.Game.Window.ClientBounds.Width - (k_TextScale * SizeOfText.X)) / 2, m_TimerPosition.Y - (SizeOfTimerText.Y * k_TextScale));
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            SpriteBatch.Begin();

            SpriteBatch.DrawString(m_FontCalibri, m_ScreenMasage, m_MsgPosition, Color.Red, 0, Vector2.Zero, k_TextScale, SpriteEffects.None, 0);

            SpriteBatch.DrawString(m_FontCalibri, Math.Ceiling(m_CountDownTime).ToString(), m_TimerPosition, Color.OrangeRed, 0, Vector2.Zero, k_TextScale, SpriteEffects.None, 0);
            SpriteBatch.End();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_CountDownTime -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_CountDownTime < 0)
            {
                GameScreen invaderScreen = new SpaceInvadersScreen(Game, r_Player1Points, r_Player2Points, m_GameLevel, r_Player1LifeLeft, r_Player2LifeLeft);
                this.ScreensManager.SetCurrentScreen(invaderScreen);
            }
        }
    }
}
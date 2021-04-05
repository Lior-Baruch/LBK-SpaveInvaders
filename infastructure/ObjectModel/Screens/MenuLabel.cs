using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel.Screens
{
    public class MenuLabel : Sprite
    {
        private const string k_AssetName = @"Butten\buttons_PNG176";
        private readonly GameScreen r_MyScreen;
        private string m_Text = "";
        private SpriteFont m_ConsolasFont;
        private Vector2 m_MsgPosition;

        public MenuLabel(GameScreen i_GameScreen, string i_Text)
            : base(k_AssetName, i_GameScreen.Game)
        {
            m_Text = i_Text;
            r_MyScreen = i_GameScreen;
            r_MyScreen.Add(this);
            this.Game.Window.ClientSizeChanged += menuLabel_OnClientSizeChanged;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_ConsolasFont = this.Game.Content.Load<SpriteFont>(@"Font\Consolas");
        }

        public override void Initialize()
        {
            base.Initialize();
            this.TintColor = Color.LightGray;
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            m_SpriteBatch.DrawString(m_ConsolasFont, m_Text, m_MsgPosition, Color.White, 0, Vector2.Zero, this.Width / m_ConsolasFont.MeasureString(m_Text).X, SpriteEffects.None, 0);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.TopLeftPosition = new Vector2(0, this.Game.Window.ClientBounds.Height - this.Height);
            SetButtonScaleForText();
        }

        public string Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                SetButtonScaleForText();
            }
        }

        private void menuLabel_OnClientSizeChanged(object sender, EventArgs e)
        {
            SetButtonScaleForText();
        }

        public void SetButtonScaleForText()
        {
            float scaleX = this.Game.Window.ClientBounds.Width / this.WidthBeforeScale;
            float scaleY = m_ConsolasFont.MeasureString(m_Text).Y * scaleX / this.HeightBeforeScale;

            this.Scales = new Vector2(scaleX, scaleY);
            this.TopLeftPosition = new Vector2(0, this.Game.Window.ClientBounds.Height - this.Height);
            centerText();
        }

        private void centerText()
        {
            m_MsgPosition = new Vector2(0, this.TopLeftPosition.Y);
        }
    }
}
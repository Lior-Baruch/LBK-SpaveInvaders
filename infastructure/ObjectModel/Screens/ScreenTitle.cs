
using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel.Screens
{
    public class ScreenTitle : Sprite
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";
        private readonly GameScreen m_MyScreen;
        private float m_TextScale = 3;
        private string m_Text = "";
        private SpriteFont m_ConsolasFont;
        private Color m_Color = Color.BlueViolet;
        private Vector2 m_TextSizeBeforeScale;
        private Vector2 m_TextSizeAfterScale;
        private Vector2 m_TextPosition;


        public ScreenTitle(GameScreen i_GameScreen)
            : base(k_AssetName, i_GameScreen.Game)
        {
            m_MyScreen = i_GameScreen;
            m_MyScreen.Add(this);
            this.Game.Window.ClientSizeChanged += screenTitile_OnClientSizeChanged;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_ConsolasFont = this.Game.Content.Load<SpriteFont>(@"Font\Consolas");
        }

        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.DrawString(m_ConsolasFont, m_Text, m_TextPosition, m_Color, 0, Vector2.Zero, m_TextScale, SpriteEffects.None, 0);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            centerText();
        }

        private void screenTitile_OnClientSizeChanged(object sender, EventArgs e)
        {
            centerText();
        }

        public string Text 
        {
            get { return m_Text; }
            set 
            {
                m_Text = value;
                centerText();
            }
        }

        public float TextScale 
        {
            get 
            {
                return m_TextScale; 
            }
            set 
            {
                m_TextScale = value;
                centerText();
            }
        }

        private void centerText()
        {
            m_TextSizeBeforeScale = m_ConsolasFont.MeasureString(m_Text);
            m_TextSizeAfterScale = m_TextSizeBeforeScale * m_TextScale;
            m_TextPosition = new Vector2((this.Game.Window.ClientBounds.Width - (m_TextSizeBeforeScale.X * m_TextScale)) / 2, 0); 
        }

        public Vector2 SizeAfterScale
        {
            get
            {
                return m_TextSizeAfterScale;
            }
        }
    }
}
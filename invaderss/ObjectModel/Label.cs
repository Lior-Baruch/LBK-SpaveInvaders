using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.ObjectModel
{
    class label : Sprite
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";
        private const float k_SecondLabelY = 20;
        private readonly GameScreen m_MyScreen;
        private string m_Text = "0";
        private bool m_IsPlayer1;
        private SpriteFont m_ConsolasFont;
        private int m_Points = 0;
        private string m_PlayerText;

        public label(GameScreen i_GameScreen, bool i_IsPlayer1)
            : base(k_AssetName, i_GameScreen.Game)
        {
            m_MyScreen = i_GameScreen;
            m_IsPlayer1 = i_IsPlayer1;
            m_MyScreen.Add(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_ConsolasFont = this.Game.Content.Load<SpriteFont>(@"Font\Consolas");
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Text = m_Points.ToString();
        }

        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.DrawString(m_ConsolasFont, m_PlayerText + m_Text, this.Position, this.TintColor);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            if (m_IsPlayer1)
            {
                m_PlayerText = "P1 Score: ";
                this.Position = Vector2.Zero;
                this.TintColor = Color.Blue;
            }
            else
            {
                m_PlayerText = "P2 Score: ";
                this.Position = new Vector2(0, k_SecondLabelY);
                this.TintColor = Color.Green;
            }
        }

        public int points
        {
            set { m_Points = value; }
        }
    }
}
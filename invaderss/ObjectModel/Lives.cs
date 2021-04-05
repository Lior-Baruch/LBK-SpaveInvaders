using System;
using System.Collections.Generic;
using System.Text;
using infastructure;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.ObjectModel
{
    class Live : Sprite
    {
        private const string k_AssetName1 = @"Sprites\Ship01_32x32";
        private const string k_AssetName2 = @"Sprites\Ship02_32x32";
        private const float k_SpacePersent = 1.4f;
        private const float k_ExtraSpace = 5;
        private const float k_Opacity = 0.5f;
        private readonly GameScreen m_MyScreen;
        private readonly Vector2 r_Scale = new Vector2(0.5f, 0.5f);
        private Vector2 m_StartPosition = Vector2.Zero;
        private int m_LifeNumber = -1;
        private bool m_IsPlayer1;

        public Live(GameScreen i_GameScreen, int i_LifeNumber, bool i_IsPlayer1 = true)
            : base(k_AssetName1, i_GameScreen.Game)
        {
            m_MyScreen = i_GameScreen;
            m_IsPlayer1 = i_IsPlayer1;
            if (!m_IsPlayer1)
            {
                this.AssetName = k_AssetName2;
            }

            m_LifeNumber = i_LifeNumber;
            m_MyScreen.Add(this);
            this.Game.Window.ClientSizeChanged += fixPos;
        }

        private void fixPos(object sender, EventArgs e)
        {
            updatePosition();
        }

        public void LifeLost()
        {
            m_SoundManager.PlaySoundEffect("LifeDie");
            this.Visible = false;
            this.Enabled = false;
            this.Dispose();
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.Opacity = k_Opacity;
            this.Scales = r_Scale;
            updatePosition();
        }

        private void updatePosition()
        {
            if (m_IsPlayer1)
            {
                m_StartPosition = new Vector2(this.Game.Window.ClientBounds.Width - (this.Width * (m_LifeNumber + 1) * k_SpacePersent), 0);
            }
            else
            {
                float x = this.Game.Window.ClientBounds.Width - ((this.Width * (m_LifeNumber + 1)) * k_SpacePersent);
                m_StartPosition = new Vector2(x, this.Height + k_ExtraSpace);
                this.AssetName = k_AssetName2;
            }

            this.TopLeftPosition = m_StartPosition;
        }

        public override void Draw(GameTime gameTime)
        {
            this.BlendState = BlendState.NonPremultiplied;
            base.Draw(gameTime);
        }
    }
}
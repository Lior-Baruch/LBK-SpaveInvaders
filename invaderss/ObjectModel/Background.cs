using System;
using infastructure;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace Invaders.ObjectModel
{
    public class Background : Sprite
    {
        private const string k_AssetName = @"Sprites\BG_Space01_1024x768";
        private readonly GameScreen m_MyScreen;

        public Background(GameScreen i_GameScreen, float i_Opacity = 1)
            : base(k_AssetName, i_GameScreen.Game)
        {
            m_MyScreen = i_GameScreen;
            this.Opacity = i_Opacity;
            m_MyScreen.Add(this);
            this.Game.Window.ClientSizeChanged += fixScales;
        }

        private void fixScales(object sender, EventArgs e)
        {
            float widthScale = this.Game.GraphicsDevice.Viewport.Width / this.WidthBeforeScale;
            float heightScale = this.Game.GraphicsDevice.Viewport.Height / this.HeightBeforeScale;
            this.Scales = new Vector2(widthScale, heightScale);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            float widthScale = this.Game.GraphicsDevice.Viewport.Width / this.WidthBeforeScale;
            float heightScale = this.Game.GraphicsDevice.Viewport.Height / this.HeightBeforeScale;

            this.Scales = new Vector2(widthScale, heightScale);
            this.DrawOrder = int.MinValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

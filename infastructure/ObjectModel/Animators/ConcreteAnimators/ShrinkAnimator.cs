using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class ShrinkAnimator : SpriteAnimator
    {
        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
        }

        public ShrinkAnimator(TimeSpan i_AnimationLength)
            : base("ShrinkAnimator", i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float scaleX = this.BoundSprite.Scales.X;
            float scaleY = this.BoundSprite.Scales.Y;

            scaleX -= m_OriginalSpriteInfo.Scales.X / (float)AnimationLength.TotalSeconds * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            scaleY -= m_OriginalSpriteInfo.Scales.Y / (float)AnimationLength.TotalSeconds * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            this.BoundSprite.Scales = new Vector2(scaleX, scaleY);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
        }
    }
}

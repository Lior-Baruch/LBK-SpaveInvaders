using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FadeAnimator : SpriteAnimator
    { 
        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
        }

        public FadeAnimator(TimeSpan i_AnimationLength)
            : base("FadeAnimator", i_AnimationLength)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Opacity -= (this.m_OriginalSpriteInfo.Opacity / (float)AnimationLength.TotalSeconds) * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }
    }
}

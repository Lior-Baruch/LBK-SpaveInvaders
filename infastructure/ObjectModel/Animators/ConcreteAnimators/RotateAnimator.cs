using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class RotateAnimator : SpriteAnimator
    {
        private readonly float r_SpinPerSec;

        // CTORs
        public RotateAnimator(string i_Name, TimeSpan i_AnimationLength, float i_SpinsPerSec = 1)
            : base(i_Name, i_AnimationLength)
        {
            r_SpinPerSec = i_SpinsPerSec;
        }

        public RotateAnimator(TimeSpan i_AnimationLength, float i_SpinsPerSec = 1)
            : base("RotateAnimator", i_AnimationLength)
        {
            r_SpinPerSec = i_SpinsPerSec;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Rotation += (float)(i_GameTime.ElapsedGameTime.TotalSeconds  * (Math.PI * 2 * r_SpinPerSec) );
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = this.m_OriginalSpriteInfo.Rotation;
        }
    }
}

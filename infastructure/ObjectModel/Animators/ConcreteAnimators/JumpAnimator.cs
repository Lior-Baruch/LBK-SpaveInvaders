using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    // TODO?
    public class JumpAnimator : SpriteAnimator
    {
        private TimeSpan m_JumpLength;
        private TimeSpan m_TimeLeftForNextJump;

        public TimeSpan JumpLength
        {
            get { return m_JumpLength; }
            set { m_JumpLength = value; }
        }

        // CTORs
        public JumpAnimator(string i_Name, TimeSpan i_JumpLength, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_JumpLength = i_JumpLength;
            this.m_TimeLeftForNextJump = i_JumpLength;
        }

        public JumpAnimator(TimeSpan i_JumpLength, TimeSpan i_AnimationLength)
            : this("JumpAnimator", i_JumpLength, i_AnimationLength)
        {
            this.m_JumpLength = i_JumpLength;
            this.m_TimeLeftForNextJump = i_JumpLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextJump -= i_GameTime.ElapsedGameTime;
            if (m_TimeLeftForNextJump.TotalSeconds < 0)
            {
                // we have elapsed, so jump
                this.BoundSprite.Visible = !this.BoundSprite.Visible;
                m_TimeLeftForNextJump = m_JumpLength;
            }
        }

        protected override void RevertToOriginal()
        {
          this.BoundSprite.Rotation = this.m_OriginalSpriteInfo.Rotation;
        }
    }
}

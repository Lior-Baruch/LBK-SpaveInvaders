////*** Guy Ronen © 2008-2011 ***//
using System;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class PulseAnimator : SpriteAnimator
    {
        protected float m_Scale;
        public float Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        protected float m_PulsePerSecond;
        public float PulsePerSecond
        {
            get { return m_PulsePerSecond; }
            set { m_PulsePerSecond = value; }
        }

        private bool m_Shrinking;
        private float m_TargetScaleX;
        private float m_TargetScaleY;
        private float m_SourceScaleX;
        private float m_SourceScaleY;
        private float m_DeltaScaleX;
        private float m_DeltaScaleY;

        public PulseAnimator(string i_Name, TimeSpan i_AnimationLength, float i_TargetScale, float i_PulsePerSecond)
            : base(i_Name, i_AnimationLength)
        {
            m_Scale = i_TargetScale;
            m_PulsePerSecond = i_PulsePerSecond;
            
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;

            m_SourceScaleX = m_OriginalSpriteInfo.Scales.X;
            m_SourceScaleY = m_OriginalSpriteInfo.Scales.Y;

            m_TargetScaleX = m_OriginalSpriteInfo.Scales.X / m_Scale;
            m_TargetScaleY = m_OriginalSpriteInfo.Scales.Y / m_Scale;
            m_DeltaScaleX = m_TargetScaleX - m_SourceScaleX;
            m_DeltaScaleY = m_TargetScaleY - m_SourceScaleY;

            m_Shrinking = m_DeltaScaleX < 0;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_Shrinking)
            {
                if (this.BoundSprite.Scales.X > m_TargetScaleX)
                {
                    this.BoundSprite.Scales -= new Vector2(totalSeconds * 2 * m_PulsePerSecond * m_DeltaScaleX, totalSeconds * 2 * m_PulsePerSecond * m_DeltaScaleY);
                }
                else
                {
                    this.BoundSprite.Scales = new Vector2(m_TargetScaleX, m_TargetScaleY);
                    m_Shrinking = false;
                    m_TargetScaleX = m_SourceScaleX;
                    m_SourceScaleX = this.BoundSprite.Scales.X;
                    m_TargetScaleY = m_SourceScaleY;
                    m_SourceScaleY = this.BoundSprite.Scales.Y;
                }
            }
            else
            {
                if (this.BoundSprite.Scales.X < m_TargetScaleX)
                {
                    this.BoundSprite.Scales += new Vector2(totalSeconds * 2 * m_PulsePerSecond * m_DeltaScaleX, totalSeconds * 2 * m_PulsePerSecond * m_DeltaScaleY);
                }
                else
                {
                    this.BoundSprite.Scales = new Vector2(m_TargetScaleX, m_TargetScaleY);
                    m_Shrinking = true;
                    m_TargetScaleX = m_SourceScaleX;
                    m_TargetScaleY = m_SourceScaleY;
                    m_SourceScaleX = this.BoundSprite.Scales.X;
                    m_SourceScaleY = this.BoundSprite.Scales.Y;
                }
            }
        }
    }
}

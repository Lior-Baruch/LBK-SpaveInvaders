////*** Guy Ronen © 2008-2011 ***////
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class CellAnimator : SpriteAnimator
    {
        private readonly int r_NumOfCells = 1;
        private readonly bool r_IsThereDummyPixel = false;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;

        // CTORs
        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength)
            : base("CelAnimation", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        // CTORs if needed to start with Cell that not start with 0
        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartCellIdx, bool i_IsThereDummyPixel)
            : this(i_CellTime, i_NumOfCells, i_AnimationLength)
        {
            m_CurrCellIdx = i_StartCellIdx;
            r_IsThereDummyPixel = i_IsThereDummyPixel;
        }

        public TimeSpan CellTime
        {
            set
            {
                m_CellTime = value;
            }
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx++;
            if (m_CurrCellIdx >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = 0;
                }
                else
                {
                    m_CurrCellIdx = r_NumOfCells - 1; /// lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            int startOfSet = 0;
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so blink
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            if (r_IsThereDummyPixel && m_CurrCellIdx != 0)
            {
                startOfSet = 1;
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
             (m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width) + startOfSet,
              this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}

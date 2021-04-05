using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace Invaders.ObjectModel
{
    class BarrierLine : GameComponent
    {
        private const int k_NumberOfBarriers = 4;
        private readonly int m_GameLevel;
        private readonly GameScreen m_MyScreen;
        private Barrier[] m_BarriersList;

        public BarrierLine(GameScreen i_GameScreen, int i_GameLevel = 0)
            : base(i_GameScreen.Game)
        {
            m_GameLevel = i_GameLevel;
            m_MyScreen = i_GameScreen;
            m_MyScreen.Add(this);
        }

        public override void Initialize()
        {
            m_BarriersList = new Barrier[k_NumberOfBarriers];
            for (int i = 0; i < k_NumberOfBarriers; i++)
            {
                m_BarriersList[i] = new Barrier(m_MyScreen, i, m_GameLevel);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            foreach (Barrier barrier in m_BarriersList)
            {
                if (barrier.OutOfGameBounds())
                {
                    changeDiraction();
                    break;
                }
            }
        }

        public Barrier[] BarrierList
        {
            get
            {
                return m_BarriersList;
            }
        }

        private void changeDiraction()
        {
            foreach (Barrier barrier in m_BarriersList)
            {
                barrier.Velocity *= -1;
            }
        }

        internal void deleteBarriers()
        {
            foreach (Barrier barrier in this.BarrierList)
            {
                barrier.Visible = false;
            }
        }
    }
}

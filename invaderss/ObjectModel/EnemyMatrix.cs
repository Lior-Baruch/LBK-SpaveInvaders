using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.ObjectModel.Screens;
using Invaders.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Invaders.ObjectModel
{
    public class EnemyMatrix : GameComponent
    {
        private const float k_SpaceBetweenEnemy = 1.6f;
        private const int k_EnemyHightAndWidth = 32;
        private readonly int m_GameLevel;
        private readonly GameScreen m_MyScreen;
        private Enemy[,] r_EnemyMatrix;
        private int m_TotalAliveEnemyNum;
        private int m_RowNum;
        private int m_ColNum;
        private bool m_DidHitWall = false;

        public event EventHandler Finished;

        public EnemyMatrix(GameScreen i_Gamescreen, int i_GameLevel = 0, int i_ColNum = 9, int i_RowNum = 5)
            : base(i_Gamescreen.Game)
        {
            m_GameLevel = i_GameLevel;
            m_MyScreen = i_Gamescreen;
            m_MyScreen.Add(this);
            m_RowNum = i_RowNum;
            m_ColNum = i_ColNum;
        }

        public override void Initialize()
        {
            base.Initialize();
            Finished += (m_MyScreen as SpaceInvadersScreen).Endgame;
            m_TotalAliveEnemyNum = m_ColNum * m_RowNum;
            r_EnemyMatrix = new Enemy[m_RowNum, m_ColNum];

            for (int i = 0; i < m_RowNum; i++)
            {
                for (int j = 0; j < m_ColNum; j++)
                {
                    Vector2 enemyStartPosition = new Vector2(j * k_EnemyHightAndWidth * k_SpaceBetweenEnemy, (i * k_EnemyHightAndWidth * k_SpaceBetweenEnemy) + (3 * k_EnemyHightAndWidth));
                    Enemy newEnemy = new Enemy(m_MyScreen, enemyStartPosition, i + 1, m_GameLevel);
                    newEnemy.Killed += OnFinished;
                    newEnemy.OutOfBounds += enemyWallHit;
                    r_EnemyMatrix[i, j] = newEnemy;
                }
            }
        }

        public int NumberOfEnemysAlive 
        {
            get { return m_TotalAliveEnemyNum; }
        }

        protected virtual void OnFinished(object sender, EventArgs e)
        {
            if (m_TotalAliveEnemyNum > 0)
            {
                m_TotalAliveEnemyNum--;
            }

            if (Finished != null && m_TotalAliveEnemyNum == 0)
            {
                Finished(this, EventArgs.Empty);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            updateDirection();
            if (m_DidHitWall == true)
            {
                m_DidHitWall = false;
            }
        }

        private void updateDirection()
        {
            if (m_DidHitWall)
            {
                for (int i = 0; i < r_EnemyMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < r_EnemyMatrix.GetLength(1); j++)
                    {
                        r_EnemyMatrix[i, j].NeedToChangeDiraction = true;
                    }
                }
            }
        }

        private void enemyWallHit(object sender, EventArgs e)
        {
            if (sender is Enemy)
            {
                if (m_DidHitWall == false)
                {
                    m_DidHitWall = true;
                }
            }
        }
    }
}
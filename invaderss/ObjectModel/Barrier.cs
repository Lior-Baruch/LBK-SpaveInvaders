using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using infastructure;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.ObjectModel
{
    class Barrier : Sprite, ICollidable2D
    {
        private const int k_numOfLevels = 4;
        private const string k_AssetName = @"Sprites\Barrier_44x32";
        private const float k_PresentOfPixelBite = 0.35f;
        private const float k_Velocity = 35;
        private const float k_SpaceBetweenBarriers = 2.6f;
        private const float k_BerrierLineWidth = 8.8f; //// depends on the number 
        private readonly int m_GameLevel;
        private readonly GameScreen m_MyScreen;
        private float m_InitDistanceFromLeftWall = 200;
        private float m_TopLeftY = 375;
        private Vector2 m_StartPosition = Vector2.Zero;
        private int m_BarrierNum = 0;
       
        public event EventHandler<EventArgs> Disposed;

        public Barrier(GameScreen i_GameScreen, int i_BarrierNum, int i_GameLevel) 
            : base(k_AssetName, i_GameScreen.Game)
        {
            m_GameLevel = i_GameLevel % k_numOfLevels; 
            m_MyScreen = i_GameScreen;
            m_BarrierNum = i_BarrierNum;
            this.Game.Window.ClientSizeChanged += fixPos;
            m_MyScreen.Add(this);
        }

        private void fixPos(object sender, EventArgs e)
        {
            getStartPos();
        }

        public override void Initialize()
        {
            float LeveLVilocity = 0;
            if (m_GameLevel == 0)
            {
                LeveLVilocity = 0;
            }
            else 
            {
                LeveLVilocity = (float)Math.Pow(1.06f, m_GameLevel - 1) * k_Velocity;            
            }

            this.Velocity = new Vector2(LeveLVilocity, 0);
            base.Initialize();
        }

        public override bool OutOfGameBounds()
        {
            bool solution = false;

            if(this.Bounds.Left < m_StartPosition.X - (this.Width / 2) || 
                this.Bounds.Left > m_StartPosition.X + (this.Width / 2)) 
            {
                solution = true;
            }

            return solution;
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            Color[] dataTemp = new Color[(int)(this.Height * this.Width)];

            this.Texture.GetData(dataTemp);
            this.Texture = new Texture2D(this.Game.GraphicsDevice, (int)this.Width, (int)this.Height);
            this.Texture.SetData(dataTemp);
            getStartPos();
        }

        private void getStartPos()
        {
            m_InitDistanceFromLeftWall = (m_MyScreen.Game.Window.ClientBounds.Width - (k_BerrierLineWidth * this.Texture.Width)) / 2;
            m_TopLeftY = m_MyScreen.Game.Window.ClientBounds.Height - (3 * this.Texture.Height);
            this.TopLeftPosition = new Vector2(m_InitDistanceFromLeftWall + (m_BarrierNum * this.Width * k_SpaceBetweenBarriers), m_TopLeftY);
            m_StartPosition = this.TopLeftPosition;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if(i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
             
                if (this.CheckPixelCollision(bullet))
                {
                    m_SoundManager.PlaySoundEffect("BarrierHit");
                    GotHitByBullet(bullet);
                    bullet.BarrierkillBullet(this, EventArgs.Empty);
                }
            }
            else if(i_Collidable is Enemy)
            {
                Enemy enemy = i_Collidable as Enemy;
                if (this.CheckPixelCollision(enemy) && m_SoundManager != null)
                {
                    m_SoundManager.PlaySoundEffect("BarrierHit");
                    GotHitByEnemy(enemy);
                }
            }
        }

        private void GotHitByEnemy(Enemy i_Enemy)
        {
            Rectangle intersectRectangle = Rectangle.Intersect(this.Bounds, i_Enemy.Bounds);
            Color[] barrierColorData = new Color[(int)(this.Width * this.Height)];
            Color[] enemyColorData = new Color[(int)(i_Enemy.Texture.Width * i_Enemy.Texture.Height)];
            float leftBiteX = intersectRectangle.Left;
            float rightBiteX = intersectRectangle.Right;
            float topBiteY = intersectRectangle.Top;
            float bottomBiteY = intersectRectangle.Bottom;

            this.Texture.GetData(barrierColorData);
            i_Enemy.Texture.GetData(enemyColorData);

            for (int y = (int)topBiteY; y <= bottomBiteY; y++)
            {
                for (int x = (int)leftBiteX; x < rightBiteX; x++)
                {
                    if ((x - this.Bounds.Left + ((y - this.Bounds.Top) * this.Width)) < this.Width * this.Height)
                    {
                        if (barrierColorData[(int)(x - this.Bounds.Left + ((y - this.Bounds.Top) * this.Width))].A != 0 &&
                            enemyColorData[(int)(x - i_Enemy.Bounds.Left + ((y - i_Enemy.Bounds.Top) * i_Enemy.Width))].A != 0)
                        {
                            barrierColorData[(int)(x - this.Bounds.Left + ((y - this.Bounds.Top) * this.Width))] = Color.Transparent;
                        }
                    }
                }
            }

            this.Texture.SetData(barrierColorData);
        }

        private void GotHitByBullet(Bullet i_Bullet)
        {
            Color[] barrierColorData = new Color[this.Texture.Width * this.Texture.Height];
            Rectangle intersectRectangle = Rectangle.Intersect(this.Bounds, i_Bullet.Bounds);
            float leftBiteX = intersectRectangle.Left - this.TopLeftPosition.X;
            float rightBiteX = intersectRectangle.Right - this.TopLeftPosition.X;
            float topBiteY = intersectRectangle.Top - this.TopLeftPosition.Y;
            float bottomBiteY = intersectRectangle.Bottom - this.TopLeftPosition.Y;
            this.Texture.GetData(barrierColorData);

            if (i_Bullet is PlayerBullet)
            {
                topBiteY = Math.Min(bottomBiteY - (i_Bullet.Height * k_PresentOfPixelBite), topBiteY);
            }
            else
            {
                bottomBiteY = Math.Max(topBiteY + (i_Bullet.Height * k_PresentOfPixelBite), bottomBiteY);
            }
            
            for (int y = (int)topBiteY; y < bottomBiteY; y++)
            {
                for(int x = (int)leftBiteX; x < rightBiteX; x++)
                {
                    if (x + (y * Width) < this.Width * this.Height)
                    {
                        barrierColorData[(int)(x + (y * Width))] = Color.Transparent;
                    }
                }
            }

            this.Texture.SetData(barrierColorData);
        }
    }
}

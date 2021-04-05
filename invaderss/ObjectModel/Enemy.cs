using System;
using infastructure;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.Screens;
using Microsoft.Xna.Framework;

namespace Invaders.ObjectModel
{
    public class Enemy : Sprite, ICollidable2D
    {
        private const string k_AssetNameAllEnemy = @"Sprites\EnemyCollection_65x98";
        private const int k_Enemy1Points = 300;
        private const int k_Enemy2Points = 200;
        private const int k_Enemy3Points = 70;
        private const int k_numOfLevels = 4;
        private const float k_AnnimationTime = 1.7f;
        private const float k_SpinPerSec = 5;
        private const int k_MaxTimeToShoot = 20;
        private const int k_MinTimeToShoot = 1;
        private const float k_ChangeJumpTimePersent = 0.95f;
        private const int k_NumOfFrames = 2;
        private const int k_NumOfEnemyType = 3;
        private readonly int m_GameLevel;
        private readonly GameScreen m_MyScreen;
        private readonly Random r_Random = new Random();
        private readonly int r_EnemyType = 0;
        private readonly int r_Points;
        private int m_JumpLength;
        private Vector2 m_StartPos;
        private bool m_CanShoot = true;
        private float m_TimeToShoot;
        private float m_TimeToJump = 0.5f;
        private float m_TimeLeftToJump = 0.5f;
        private bool m_GoToRight = true;
        private bool m_IsDoingDieAnnimation = false;
        private bool m_NeedToChangeDirection = false;
        private CompositeAnimator m_AnimationWhenDead;
        private CompositeAnimator m_AnimationChangeTexture;  

        public event EventHandler Finished;

        public event EventHandler OutOfBounds;

        public event EventHandler Killed;

        public event EventHandler<EventArgs> Disposed;

        public Enemy(GameScreen i_Gamescreen, Vector2 i_StartPosition, int i_EnemyTypeNum, int i_GameLevel)
            : base(k_AssetNameAllEnemy, i_Gamescreen.Game)
        {
            m_GameLevel = i_GameLevel % k_numOfLevels;
            m_MyScreen = i_Gamescreen;
            m_MyScreen.Add(this);
            m_StartPos = i_StartPosition;
            m_TimeToShoot = (float)r_Random.Next(k_MinTimeToShoot, k_MaxTimeToShoot);
            r_EnemyType = i_EnemyTypeNum;
            if (r_EnemyType == 1)
            {
                this.TintColor = Color.Pink;
                r_Points = k_Enemy1Points;
            }
            else if (r_EnemyType == 2 || r_EnemyType == 3)
            {
                this.TintColor = Color.LightSkyBlue;
                r_Points = k_Enemy2Points;
            }
            else
            {
                this.TintColor = Color.Yellow;
                r_Points = k_Enemy3Points;
            }

            r_Points += 100 * m_GameLevel;
            this.Game.Window.ClientSizeChanged += fixPos;
        }

        private void fixPos(object sender, EventArgs e)
        {
            this.Position = new Vector2(m_StartPos.X, this.Position.Y);
            m_GoToRight = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
            m_JumpLength = (int)this.Width / 2;
            Finished += (m_MyScreen as SpaceInvadersScreen ).Endgame;     
        }

        private void initAnimations()
        {
            m_AnimationWhenDead = new CompositeAnimator(this, "AnimationWhenDead");
            m_AnimationChangeTexture = new CompositeAnimator(this, "ChangeTexture");
            CellAnimator cellAnimator = new CellAnimator(TimeSpan.FromSeconds(m_TimeToJump), k_NumOfFrames, TimeSpan.FromSeconds(0), r_EnemyType % 2, false);
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_AnnimationTime));
            RotateAnimator rotateAnimator = new RotateAnimator(TimeSpan.FromSeconds(k_AnnimationTime), k_SpinPerSec);

            m_AnimationChangeTexture.Add(cellAnimator);
            m_AnimationWhenDead.Add(shrinkAnimator);
            m_AnimationWhenDead.Add(rotateAnimator);
            shrinkAnimator.Finished += killEnemy;
            this.Animations = m_AnimationChangeTexture;
            this.Animations.Resume();
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            m_WidthBeforeScale = m_WidthBeforeScale / k_NumOfFrames;
            m_HeightBeforeScale = m_HeightBeforeScale / k_NumOfEnemyType;
            int x = 0;

            if (r_EnemyType % 2 == 0)
            {
                x = (int)Width;
            }

            this.SourceRectangle = new Rectangle(
                x,
                ((r_EnemyType / k_NumOfFrames) * (int)this.Height) + 1,
                (int)Width + 1,
                (int)Height);

            this.RotationOrigin = this.SourceRectangleCenter;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.Bounds.Bottom >= Game.GraphicsDevice.Viewport.Height - this.Height)
            {
                Finished(this, EventArgs.Empty);
            }

            base.Update(i_GameTime);
            ifNeedShoot(i_GameTime);
            ifNeedMove(i_GameTime);
        }

        private void ifNeedMove(GameTime i_GameTime)
        {
            this.m_TimeLeftToJump -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (this.m_TimeLeftToJump <= 0)
            {
                if (m_NeedToChangeDirection)
                {
                    changeDirection();
                    m_NeedToChangeDirection = false;
                }

                m_TimeLeftToJump = m_TimeToJump;
                float nextX = getNextPosX();

                this.TopLeftPosition = new Vector2(nextX, this.TopLeftPosition.Y);

                nextX = MathHelper.Clamp(nextX, -1, this.Game.GraphicsDevice.Viewport.Width - this.Width + 1);
                this.TopLeftPosition = new Vector2(nextX, this.TopLeftPosition.Y);
                nextjumpcheack();
            }
        }

        private bool nextjumpcheack()
        {
            int boundX = Game.GraphicsDevice.Viewport.Width - (int)this.Width;
            float nextX;

            nextX = getNextPosX();

            bool didHitWall = false;

            if (nextX < 0 || nextX >= boundX)
            {
                didHitWall = true;
            }

            if (didHitWall)
            {
                OutOfBounds(this, EventArgs.Empty);
            }

            return didHitWall;
        }

        private void changeDirection()
        {
            this.m_GoToRight = !this.m_GoToRight;
            this.m_TimeToJump *= k_ChangeJumpTimePersent; // jump_time 0.95
            (m_AnimationChangeTexture["CelAnimation"] as CellAnimator).CellTime = TimeSpan.FromSeconds(m_TimeToJump);
            float nextX = getNextPosX();

            this.TopLeftPosition = new Vector2(nextX, this.TopLeftPosition.Y + (this.Height / 2));
        }

        public bool NeedToChangeDiraction
        {
            get
            {
                return m_NeedToChangeDirection;
            }

            set
            {
                m_NeedToChangeDirection = value;
            }
        }

        private float getNextPosX()
        {
            float nextX;

            if (!m_GoToRight)
            {
                nextX = this.TopLeftPosition.X - (this.Width / 2);
            }
            else
            {
                nextX = this.TopLeftPosition.X + (this.Width / 2);
            }

            return nextX;
        }

        private void ifNeedShoot(GameTime i_GameTime)
        {
            m_TimeToShoot -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeToShoot <= 0 && m_CanShoot && !m_IsDoingDieAnnimation)
            {
                shoot();
                m_TimeToShoot = r_Random.Next(k_MinTimeToShoot, k_MaxTimeToShoot);
            }
        }

        private void shoot()
        {
            EnemyBullet bullet = new EnemyBullet(m_MyScreen, this);
            m_CanShoot = false;
        }

        public bool CanShoot
        {
            set
            {
                m_CanShoot = value;
            }
        }

        private void killEnemy(object sender, EventArgs e)
        {
            this.Visible = false;
            Killed(this, EventArgs.Empty);
            this.Enabled = false;
            m_IsDoingDieAnnimation = true;
            this.Game.Components.Remove(this);
            this.Dispose();
        }

        public int GetPoints
        {
            get
            {
                return r_Points;
            }
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.Position = m_StartPos;
        }

        public bool IsDoingAnnimation
        {
            get
            {
                return m_IsDoingDieAnnimation;
            }

            set
            {
                m_IsDoingDieAnnimation = value;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is PlayerBullet && this.Animations != m_AnimationWhenDead)
            {
                this.Animations = m_AnimationWhenDead;
                this.Animations.Resume();
                m_SoundManager.PlaySoundEffect("EnemyKill");
            }
        }
    }
}
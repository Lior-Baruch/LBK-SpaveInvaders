using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using infastructure;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel.Screens;
using Invaders.Screens;

namespace Invaders.ObjectModel
{
    public class SpaceShip : Sprite, ICollidable2D
    {
        private const string k_AssetName1 = @"Sprites\Ship01_32x32";
        private const string k_AssetName2 = @"Sprites\Ship02_32x32";
        private const Keys k_Player1LeftKey = Keys.Left;
        private const Keys k_Player1RightKey = Keys.Right;
        private const Keys k_Player1ShootKey = Keys.Space;
        private const Keys k_Player2LeftKey = Keys.A;
        private const Keys k_Player2RightKey = Keys.D;
        private const Keys k_Player2ShootKey = Keys.D3;
        private const int k_MaxBulletNumToFire = 2;
        private const float k_BlinkLength = 0.125f;
        private const float k_AnimationBlinkLength = 2;
        private const float k_FadeLength = 4.3f;
        private const float k_RotaionLength = 2.6f;
        private const float k_RotaionPerSec = 6;
        private const float k_Velocity = 140f;
        private readonly Keys r_LeftButton;
        private readonly Keys r_RightButton;
        private readonly Keys r_ShootButton;
        private readonly label r_Label;
        private readonly bool r_IsPlayer1;
        private readonly GameScreen m_MyScreen;
        private bool m_firstPosition = true;
        private bool m_AnimationStarted;
        private int m_Points = 0;
        private int m_LifeLeft = 3;
        private int m_BulletCount = 0;
        private Vector2 m_StartPosition;
        private InputManager m_InputManager;
        private Live[] m_Souls;
        private CompositeAnimator m_AnimatorForHit;
        private CompositeAnimator m_AnimatorForDestruction;

        public event EventHandler Finished;

        public event EventHandler<EventArgs> Disposed;

        public SpaceShip(GameScreen i_GameScreen, bool i_IsPlayer1 = true, int i_PlayerPoints = 0, int i_PlayerLifes = 3)
            : base(k_AssetName1, i_GameScreen.Game)
        {
            m_MyScreen = i_GameScreen;
            r_IsPlayer1 = i_IsPlayer1;
            m_LifeLeft = i_PlayerLifes;
            if (r_IsPlayer1)
            {
                r_Label = new label(m_MyScreen, r_IsPlayer1);
                r_LeftButton = k_Player1LeftKey;
                r_RightButton = k_Player1RightKey;
                r_ShootButton = k_Player1ShootKey;
            }
            else
            {
                r_Label = new label(m_MyScreen, r_IsPlayer1);
                r_LeftButton = k_Player2LeftKey;
                r_RightButton = k_Player2RightKey;
                r_ShootButton = k_Player2ShootKey;
                this.AssetName = k_AssetName2;
            }

            this.Game.Window.ClientSizeChanged += fixPos;
            m_Points = i_PlayerPoints;
            m_MyScreen.Add(this);
        }

        private void fixPos(object sender, EventArgs e)
        {
            m_StartPosition = getStartPosition();
            this.TopLeftPosition = m_StartPosition;
        }

        public void AddPoints(int i_Points)
        {
            m_Points += i_Points;
            m_Points = Math.Max(m_Points, 0);
        }

        public int PlayerPoints
        {
            get
            {
                return m_Points;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.BlendState = BlendState.NonPremultiplied;
            Finished += (m_MyScreen as SpaceInvadersScreen).Endgame;
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            initAnimations();
            this.RotationOrigin = this.SourceRectangleCenter;
            m_Souls = new Live[m_LifeLeft];
            for (int i = 0; i < m_LifeLeft; i++)
            {
                m_Souls[i] = new Live(m_MyScreen, i, r_IsPlayer1);
            }

            if(m_LifeLeft <= 0)
            {
                killSpaceShip();
            }
        }

        private void initAnimations()
        {
            m_AnimatorForHit = new CompositeAnimator(this, "ShipBlink");
            m_AnimatorForDestruction = new CompositeAnimator(this, "KillShip");
            RotateAnimator rotateAnimator = new RotateAnimator(TimeSpan.FromSeconds(k_RotaionLength), k_RotaionPerSec);
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(k_BlinkLength), TimeSpan.FromSeconds(k_AnimationBlinkLength));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(k_FadeLength));

            rotateAnimator.Finished += onkillSpaceShip;
            blinkAnimator.Finished += onFinsihAfterHit;
            m_AnimatorForHit.Add(blinkAnimator);
            m_AnimatorForDestruction.Add(fadeAnimator);
            m_AnimatorForDestruction.Add(rotateAnimator);
            this.Animations = m_AnimatorForHit;
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.TopLeftPosition = getStartPosition();
            m_StartPosition = this.TopLeftPosition;
        }

        private Vector2 getStartPosition()
        {
            float x;
            float y = (float)GraphicsDevice.Viewport.Height - this.Height;
            if (r_IsPlayer1)
            {
                x = 0;
            }
            else
            {
                x = this.Width;
            }

            return new Vector2(x, y);
        }

        public int LifeLeft
        {
            get
            {
                return m_LifeLeft;
            }
        }

        public void ReSetLives()
        {
            m_LifeLeft = 3;            
        }
    
        public int BulletCount
        {
            get
            {
                return m_BulletCount;
            }

            set
            {
                m_BulletCount = value;
            }
        }

        private void onkillSpaceShip(object sender, EventArgs e)
        {
            killSpaceShip();
        }

        private void killSpaceShip()
        {
            this.Visible = false;
            this.Enabled = false;
            if (Finished != null && (m_MyScreen as SpaceInvadersScreen).m_OnlyOneSpaceShipsIsALive)
            {
                Finished(this, EventArgs.Empty);
            }
            else
            {
                (m_MyScreen as SpaceInvadersScreen).m_OnlyOneSpaceShipsIsALive = true;
            }

            this.Dispose();
        }

        private void BackToStart()
        {
            this.TopLeftPosition = m_StartPosition;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (m_InputManager.KeyboardState.IsKeyDown(r_LeftButton) && m_LifeLeft > 0)
            {
                m_Velocity.X = k_Velocity * -1;
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(r_RightButton) && m_LifeLeft > 0)
            {
                m_Velocity.X = k_Velocity;
            }
            else
            {
                m_Velocity.X = 0;
            }

            r_Label.points = m_Points;
            if (m_BulletCount < k_MaxBulletNumToFire && m_LifeLeft > 0 &&
                (m_InputManager.KeyPressed(r_ShootButton) ||
                (r_IsPlayer1 && (m_InputManager.MouseState.LeftButton == ButtonState.Pressed
                && m_InputManager.PrevMouseState.LeftButton == ButtonState.Released))))
            {
                shoot();
            }

            base.Update(i_GameTime);
            float x = this.TopLeftPosition.X;

            if (r_IsPlayer1 && m_LifeLeft > 0)
            {
                if (m_firstPosition)
                {
                    Mouse.SetPosition((int)m_StartPosition.X, (int)m_Position.Y);
                    m_firstPosition = false;
                }
                else
                {
                    x += m_InputManager.MousePositionDelta.X;
                }
            }

            x = MathHelper.Clamp(x, 0, GraphicsDevice.Viewport.Width - this.Width);
            this.TopLeftPosition = new Vector2(x, this.Position.Y);
        }

        private void shoot()
        {
            if (!m_AnimationStarted && m_BulletCount < k_MaxBulletNumToFire)
            {
                PlayerBullet bullet = new PlayerBullet(m_MyScreen, this);
                m_BulletCount++;
            }
        }

        private void onFinsihAfterHit(object sender, EventArgs e)
        {
            m_AnimationStarted = false;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is EnemyBullet && m_LifeLeft > 0 && !m_AnimationStarted)
            {
                m_LifeLeft--;
                m_Souls[m_LifeLeft].LifeLost();
                if (m_LifeLeft > 0)
                {
                    this.Animations.Restart();
                    m_AnimationStarted = true;
                    BackToStart();
                }
                else if (m_LifeLeft == 0)
                {
                    this.Animations = m_AnimatorForDestruction;
                    this.Animations.Resume();
                }
            }
        }
    }
}

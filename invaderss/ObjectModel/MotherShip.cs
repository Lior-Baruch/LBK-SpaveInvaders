using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.ObjectModel
{
    class MotherShip : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_Speed = 95;
        private const int k_MinTimeToMotherShip = 1;
        private const int k_MaxTimeToMotherShip = 15;
        private const int k_numOfLevels = 4;
        private const float k_StartPosX = -120;
        private const float k_StartPosY = 32;
        private const float k_AnnimationLength = 3;
        private const float k_BlinkRate = 0.3f;
        private const int k_PointsUpEachLevel = 100;
        private readonly int r_Points = 600;
        private readonly Random r_Random = new Random();
        private readonly GameScreen r_MyScreen;
        private bool m_IsDoingAnnimation = false;
        private float m_TimeToMotherShip;
       
        public event EventHandler<EventArgs> Disposed;

        public MotherShip(GameScreen i_GameScreen, int i_GameLevel)
            : base(k_AssetName, i_GameScreen.Game)
        {
            r_MyScreen = i_GameScreen;
            r_Points += (i_GameLevel % k_numOfLevels) * k_PointsUpEachLevel;
            this.TintColor = Color.Red;
            m_TimeToMotherShip = r_Random.Next(k_MinTimeToMotherShip, k_MaxTimeToMotherShip);
            this.Velocity = new Vector2(k_Speed, 0);
            r_MyScreen.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.BlendState = BlendState.NonPremultiplied;
            initAnimations();          
        }

        private void initAnimations()
        {
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(k_AnnimationLength));
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(k_BlinkRate), TimeSpan.FromSeconds(k_AnnimationLength));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(k_AnnimationLength));
            shrinkAnimator.Finished += gotHit;
            this.Animations.Add(shrinkAnimator);
            this.Animations.Add(blinkAnimator);
            this.Animations.Add(fadeAnimator);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.PositionOrigin = this.SourceRectangleCenter;
            this.TopLeftPosition = new Vector2(k_StartPosX, k_StartPosY);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_TimeToMotherShip -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeToMotherShip <= 0)
            {
                this.Visible = true;
                if(this.TopLeftPosition.X >= this.Game.GraphicsDevice.Viewport.Width)
                {
                    backToStart();
                }

                base.Update(i_GameTime);
            }
        }

        public int GetPoints
        {
            get
            {
                return r_Points;
            }
        }

        public bool IsDoingAnnimation
        {
            get
            {
                return m_IsDoingAnnimation;
            }

            set
            {
                m_IsDoingAnnimation = value;
            }
        }

        private void backToStart()
        {
            m_TimeToMotherShip = r_Random.Next(k_MinTimeToMotherShip, k_MaxTimeToMotherShip);
            this.TopLeftPosition = new Vector2(k_StartPosX, k_StartPosY);
            this.Visible = false;
            m_IsDoingAnnimation = false;
        }

        private void gotHit(object sender, EventArgs e)
        {
            backToStart();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is PlayerBullet && !m_IsDoingAnnimation)
            {
                m_SoundManager.PlaySoundEffect("MotherShipKill");
                this.Animations.Restart();
            }
        }
    }
}

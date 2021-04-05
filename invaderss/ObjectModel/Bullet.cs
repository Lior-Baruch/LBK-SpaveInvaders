using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using infastructure;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.ObjectModel;
using Infrastructure.Managers;

namespace Invaders.ObjectModel
{
    public class Bullet : Sprite, ICollidable2D 
    {
        private const string k_AssetName = @"Sprites\Bullet";
        private readonly Vector2 r_Velocity = new Vector2(0, 140);
        private readonly GameScreen m_MyScreen;

        public event EventHandler<EventArgs> Disposed;

        public Bullet(GameScreen i_Gamescreen)
            : base(k_AssetName, i_Gamescreen.Game)
        {
            m_MyScreen = i_Gamescreen;
            m_MyScreen.Add(this);
            this.Velocity = r_Velocity;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (this.OutOfGameBounds())
            {
                KillBullet();
            }
        }

        protected override void InitBounds()
        {
            base.InitBounds();
        }

        protected virtual void KillBullet()
        {
            this.Visible = false;
            this.Enabled = false;
            this.Dispose();
        }

        public void BarrierkillBullet(object sender, EventArgs e)
        {
            KillBullet();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }

    class PlayerBullet : Bullet
    {
        SpaceShip m_Player;

        public PlayerBullet(GameScreen i_Gamescreen, SpaceShip i_Player) :
            base(i_Gamescreen)
        {
            m_Player = i_Player;
            this.TintColor = Color.Red;
            this.Position = new Vector2(m_Player.Bounds.Left + (m_Player.Width / 2), m_Player.Bounds.Top - (m_Player.Height / 2));
            this.Velocity *= -1;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SoundManager.PlaySoundEffect("SSGunShot");
        }

        protected override void KillBullet()
        {
            base.KillBullet();
            if (this.m_Player.BulletCount > 0)
            {
                this.m_Player.BulletCount--;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Enemy && !(i_Collidable as Enemy).IsDoingAnnimation)
            {
                (i_Collidable as Enemy).IsDoingAnnimation = true;
                KillBullet();
                m_Player.AddPoints(((Enemy)i_Collidable).GetPoints);
            }
            else if (i_Collidable is MotherShip && !(i_Collidable as MotherShip).IsDoingAnnimation)
            {
                (i_Collidable as MotherShip).IsDoingAnnimation = true;
                KillBullet();
                m_Player.AddPoints(((MotherShip)i_Collidable).GetPoints);
            }
            else if (i_Collidable is EnemyBullet)
            {
                KillBullet();
            }
        }
    }

    class EnemyBullet : Bullet
    {
        private const float k_PresentKillBulletIfHitPlayerBullet = 0.6f;
        private readonly Random r_Random = new Random();
        private Enemy m_Enemy;

        public EnemyBullet(GameScreen i_Gamescreen, Enemy i_Enemy)
            : base(i_Gamescreen)
        {
            m_Enemy = i_Enemy;
            this.TintColor = Color.Blue;
            this.Position = new Vector2(i_Enemy.Bounds.Left + (i_Enemy.Width / 2), i_Enemy.Bounds.Top + (i_Enemy.Height / 2));
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SoundManager.PlaySoundEffect("EnemyGunShot");
        }

        protected override void KillBullet()
        {
            base.KillBullet();
            m_Enemy.CanShoot = true;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is SpaceShip && (i_Collidable as SpaceShip).LifeLeft > 0)
            {
                KillBullet();
                (i_Collidable as SpaceShip).AddPoints(-600);
            }
            else if (i_Collidable is PlayerBullet && r_Random.NextDouble() < k_PresentKillBulletIfHitPlayerBullet)
            {
                KillBullet();
            }
        }
    }
}
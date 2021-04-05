using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Managers;
using infastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Invaders.ObjectModel;
using Invaders.Screens;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Invaders
{
    public class SpaceInvaders : BaseGame
    {
        private const int k_MinScreenSizeHight = 500;
        private const int k_MinScreenSizeWidth = 700;
        private ScreensMananger m_ScreensMananger;

        public SpaceInvaders() : base()
        {
            System.Windows.Forms.Form gameForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(this.Window.Handle);
            gameForm.MinimumSize = new System.Drawing.Size(k_MinScreenSizeWidth, k_MinScreenSizeHight);
        }

        protected override void Initialize()
        {
            m_ScreensMananger = new ScreensMananger(this);
            WelcomeScreen WelcomeScreen = new WelcomeScreen(this);
            m_ScreensMananger.SetCurrentScreen(WelcomeScreen);
            base.Initialize();
            m_SoundsManager.PlayBackRoundSong();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SoundEffect EnemyGunShot = this.Content.Load<SoundEffect>(@"EnemyGunShot");
            SoundEffect SSGunShot = this.Content.Load<SoundEffect>(@"SSGunShot");
            SoundEffect EnemyKill = this.Content.Load<SoundEffect>(@"EnemyKill");
            SoundEffect MotherShipKill = this.Content.Load<SoundEffect>(@"MotherShipKill");
            SoundEffect BarrierHit = this.Content.Load<SoundEffect>(@"BarrierHit");
            SoundEffect GameOver = this.Content.Load<SoundEffect>(@"GameOver");
            SoundEffect LevelWin = this.Content.Load<SoundEffect>(@"LevelWin");
            SoundEffect LifeDie = this.Content.Load<SoundEffect>(@"LifeDie");
            SoundEffect MenuMove = this.Content.Load<SoundEffect>(@"MenuMove");
           SoundEffect BGMusic = this.Content.Load<SoundEffect>(@"BGMusic");
            this.m_SoundsManager.AddSoundEffect("EnemyGunShot", EnemyGunShot);
            this.m_SoundsManager.AddSoundEffect("SSGunShot", SSGunShot);
            this.m_SoundsManager.AddSoundEffect("EnemyKill", EnemyKill);
            this.m_SoundsManager.AddSoundEffect("MotherShipKill", MotherShipKill);
            this.m_SoundsManager.AddSoundEffect("BarrierHit", BarrierHit);
            this.m_SoundsManager.AddSoundEffect("GameOver", GameOver);
            this.m_SoundsManager.AddSoundEffect("LevelWin", LevelWin);
            this.m_SoundsManager.AddSoundEffect("LifeDie", LifeDie);
            this.m_SoundsManager.AddSoundEffect("MenuMove", MenuMove);
            this.m_SoundsManager.AddBackRoundSong("BGMusic", BGMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.m_InputManager.KeyPressed(Keys.M))
            {
                m_SoundsManager.ToggleSound = !m_SoundsManager.ToggleSound;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
using System;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;

namespace Invaders.Screens
{
    public class SpaceInvadersScreen : GameScreen
    {
        private const int k_ScreenHeight = 650;
        private const int k_numOfLevels = 4;
        private const int k_NumOfMatrixColAtStart = 9;
        private readonly bool r_GameOfOnlyOnePlayer = true;
        private Background m_Background;
        private SpaceShip m_SpaceShip1;
        private SpaceShip m_SpaceShip2;
        private BarrierLine m_BarriersLine;
        private int m_GameLevel;
        private EnemyMatrix m_EnemyMatrix;
        private MotherShip m_MotherShipTest;
        private SoundManager m_SoundManager;
        public bool m_OnlyOneSpaceShipsIsALive = true;

        // if player2points < 0, only 1 player. else 2 players
        public SpaceInvadersScreen(Game i_Game, int i_Player1Points = 0, int i_Player2Poitns = 0, int i_GameLevel = 0, int i_Player1Lifes = 3, int i_Player2Lifes = 3) : base(i_Game)
        {
            m_Background = new Background(this);

            m_Background = new Background(this);

            m_EnemyMatrix = new EnemyMatrix(this, i_GameLevel, k_NumOfMatrixColAtStart + (i_GameLevel % k_numOfLevels));

            m_MotherShipTest = new MotherShip(this, i_GameLevel);

            m_SpaceShip1 = new SpaceShip(this, true, i_Player1Points, i_Player1Lifes);

            if (i_Player2Poitns >= 0)
            {
                m_OnlyOneSpaceShipsIsALive = false;
                r_GameOfOnlyOnePlayer = false;
                m_SpaceShip2 = new SpaceShip(this, false, i_Player2Poitns, i_Player2Lifes);
            }

            m_BarriersLine = new BarrierLine(this, i_GameLevel);
            m_GameLevel = i_GameLevel;
            this.BlendState = BlendState.NonPremultiplied;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            //// we want to fade in only uppon first activation:
            this.ActivationLength = TimeSpan.Zero;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SoundManager = Game.Services.GetService(typeof(SoundManager)) as SoundManager;
        }

        public void Endgame(object sender, EventArgs e)
        {
            int player2Points = -1;
            if (!r_GameOfOnlyOnePlayer)
            {
                player2Points = m_SpaceShip2.PlayerPoints;
            }

            if (sender is EnemyMatrix)
            {
                m_SoundManager.PlaySoundEffect("LevelWin");
                NextLevelTransitionScreen levelTransition = new NextLevelTransitionScreen(Game, m_SpaceShip1.PlayerPoints, player2Points, m_GameLevel + 1, m_SpaceShip1.LifeLeft, m_SpaceShip2.LifeLeft);
                this.ScreensManager.SetCurrentScreen(levelTransition);
            }
            else if (sender is Enemy || sender is SpaceShip)
            {
                m_SoundManager.PlaySoundEffect("GameOver");
                GameScreen m_GameOverScreen = new GameOverScreen(Game, m_SpaceShip1.PlayerPoints, player2Points);
                this.ScreensManager.SetCurrentScreen(m_GameOverScreen);
            }

            m_BarriersLine.deleteBarriers();
            this.Enabled = false; 
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.P))
            {
                this.Enabled = false;
                ScreensManager.SetCurrentScreen(new PauseScreen(this.Game));
            }
        }
    }
}
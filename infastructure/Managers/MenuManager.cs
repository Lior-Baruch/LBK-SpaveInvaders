using infastructure;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;

namespace Infrastructure.Managers
{
    public class MenuManager : GameComponent
    {
        protected const float k_ButtonSpacing = 50;
        private readonly int m_NumberOfButtens;

        protected GameScreen m_MyScreen;
        protected GameButten[] m_ButtenList;
        protected bool m_SinglePlayer;
        protected GameButten m_CurrentButton;
        protected Vector2 m_MaxTextSize;
        protected MenuLabel m_MenuLabel;
        protected ScreenTitle m_ScreenTilte;
        protected float m_TopButtonY;
        protected SoundManager m_SoundManager;

        public event EventHandler ExitScreen;

        public MenuManager(GameScreen i_GamsScreen, int i_NumberOfButtens) : base(i_GamsScreen.Game)
        {

            m_MyScreen = i_GamsScreen;
            m_MyScreen.Add(this);
            m_NumberOfButtens = i_NumberOfButtens;
            m_SinglePlayer = false;
            m_ButtenList = new GameButten[m_NumberOfButtens];

            for (int i = 0; i < m_NumberOfButtens; i++)
            {
                m_ButtenList[i] = new GameButten(m_MyScreen, i.ToString(), i);
                m_ButtenList[i].OnMouseChangedButten += Button_OnMouseChangedButten;
                m_ButtenList[i].OnMouseLeftHover += Button_OnMouseLeftHover;
                m_ButtenList[i].OnRsetsetButtens += menuManager_OnRsetsetButtens;
            }
            this.Game.Window.ClientSizeChanged += menuManager_ClientSizeChanged;
            m_CurrentButton = null;
            m_MenuLabel = new MenuLabel(m_MyScreen, "Press [Page UP / Down] Or (Mouse Right Butten/Scroll) To Naivate Between Button Options");
            m_ScreenTilte = new ScreenTitle(m_MyScreen);
        }

        private void menuManager_OnRsetsetButtens(object sender, EventArgs e)
        {
            GetMaxTextSize();
            SetButtonSize();
        }

        private void menuManager_ClientSizeChanged(object sender, EventArgs e)
        {
            SetButtonSize();
        }

        public void GetMaxTextSize()
        {
            foreach (GameButten button in m_ButtenList)
            {
                if (button.MaxSize.X > m_MaxTextSize.X)
                {
                    m_MaxTextSize = button.MaxSize;
                }
            }
        }

        public bool IsSinglePlayer
        {
            get
            {
                return m_SinglePlayer;
            }
            set
            {
                m_SinglePlayer = value;
            }
        }

        public void SetButtonSize()
        {
            float blockHight = 0;

            foreach (GameButten button in m_ButtenList)
            {
                button.SetButtonScaleForText(m_MaxTextSize);
                blockHight = (m_ButtenList[0].Height * m_ButtenList.Length) + ((k_ButtonSpacing - m_ButtenList[0].Height) * (m_ButtenList.Length - 1));
                button.TopLeftPosition = new Vector2((this.Game.Window.ClientBounds.Width - button.Width) / 2, button.Index * k_ButtonSpacing + ((this.Game.Window.ClientBounds.Height - blockHight) / 2));
            }

            m_TopButtonY = m_ButtenList[0].TopLeftPosition.Y;
            fixTitleScale();
        }

        protected void MenuManager_OnClickReturn(object sender, EventArgs e)
        {
            m_MyScreen.ScreensManager.SetCurrentScreen(m_MyScreen.PreviousScreen);
        }

        private void fixTitleScale()
        {
            while (m_TopButtonY < m_ScreenTilte.TopLeftPosition.Y + m_ScreenTilte.SizeAfterScale.Y && m_ScreenTilte.TopLeftPosition.Y < m_TopButtonY)
            {
                m_ScreenTilte.TextScale /= 1.05f;
            }
            while (m_TopButtonY > m_ScreenTilte.TopLeftPosition.Y + m_ScreenTilte.SizeAfterScale.Y && m_ScreenTilte.TextScale < 3)
            {
                m_ScreenTilte.TextScale *= 1.05f;
            }

        }

        public ScreenTitle Title
        {
            get
            {
                return m_ScreenTilte;
            }
            set
            {
                m_ScreenTilte = value;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SoundManager = Game.Services.GetService(typeof(SoundManager)) as SoundManager;
        }

        protected void Button_OnClickExit(object sender, EventArgs e)
        {
            if (ExitScreen != null)
            {
                ExitScreen(this, EventArgs.Empty);
            }
        }

        protected void Button_OnMouseLeftHover(object sender, EventArgs e)
        {
            GameButten button = sender as GameButten;
            m_CurrentButton = null;
        }

        protected void Button_OnMouseChangedButten(object sender, EventArgs e)
        {
            GameButten butten = (sender as GameButten);
            changeCurrentButton(butten);
        }

        protected virtual void Button_OnClcikPlay(object sender, EventArgs e)
        {
         
        }

        private void changeCurrentButton(GameButten i_Button)
        {
            if (m_CurrentButton != null)
            {
                m_CurrentButton.IsActive = false;
            }

            m_SoundManager.PlaySoundEffect("MenuMove");
            m_CurrentButton = i_Button;
            m_CurrentButton.IsActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int buttonIndex;
            bool pressedUp = m_MyScreen.InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Up);
            bool pressedDown = m_MyScreen.InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Down);
            
            if (pressedUp || pressedDown)
            {
                if (m_CurrentButton == null)
                {
                    buttonIndex = 0;
                }
                else if (pressedUp)
                {
                    buttonIndex = (m_CurrentButton.Index - 1 + m_NumberOfButtens) % m_NumberOfButtens;
                }
                else //// if (pressedDown)
                {
                    buttonIndex = (m_CurrentButton.Index + 1) % m_NumberOfButtens;
                }

                changeCurrentButton(m_ButtenList[buttonIndex]);
            }
        }
    }
}
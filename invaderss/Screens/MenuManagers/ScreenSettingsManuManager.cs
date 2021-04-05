using System;
using System.Collections.Generic;
using Infrastructure.Managers;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace Invaders.Screens.MenuManagers
{
    public class ScreenSettingsManuManager : MenuManager
    {
        const int k_NumberOfButtens = 4;
        private GraphicsDeviceManager m_GrapicDIvice_;

        public ScreenSettingsManuManager(GameScreen i_GameScreen)
         : base(i_GameScreen, k_NumberOfButtens)
        {
            this.m_ButtenList[0].TextList = new List<string> { "Allow Window Resizing: OFF ", "Allow Window Resizing: ON " };
            this.m_ButtenList[1].TextList = new List<string> { "Full Screen Mode: OFF ", "Full Screen Mode : ON " };
            this.m_ButtenList[2].TextList = new List<string> { "Mouse Visabillity: Visiblie ", "Mouse Visabillity: Invisiblie " };
            this.m_ButtenList[3].Text = "Done";
            m_ButtenList[0].OnTextChange += Button_OnClickWindowResizing;
            m_ButtenList[1].OnTextChange += Button_OnClickFullScreenMode;
            m_ButtenList[2].OnTextChange += Button_OnClickMouseVisabillity;
            m_ButtenList[3].OnClick += MenuManager_OnClickReturn;
            GetMaxTextSize();
            m_ScreenTilte.Text = @"Screen Settings";
            SetButtonSize();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Game.IsMouseVisible = true;
            this.Game.Window.AllowUserResizing = false;
        }

        private void OnClickMainManuScreen(object sender, EventArgs e)
        {
            m_MyScreen.ScreensManager.SetCurrentScreen(new MainManuScreen(m_MyScreen.Game)); // new Screen MainMANU screen 
        }

        private void Button_OnClickWindowResizing(object sender, EventArgs e)
        {
            this.Game.Window.AllowUserResizing = !this.Game.Window.AllowUserResizing;
        }

        private void Button_OnClickFullScreenMode(object sender, EventArgs e)
        {
            m_GrapicDIvice_ = this.Game.Services.GetService(typeof(GraphicsDeviceManager)) as GraphicsDeviceManager;
            m_GrapicDIvice_.ToggleFullScreen();
        }

        private void Button_OnClickMouseVisabillity(object sender, EventArgs e)
        {
            this.Game.IsMouseVisible = !this.Game.IsMouseVisible;
        }
    }
}

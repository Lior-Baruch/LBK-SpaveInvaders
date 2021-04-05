using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XInput;

namespace Infrastructure.ObjectModel
{
    public class GameButten : Sprite
    {
        private const string k_AssetName = @"Butten\buttons_PNG176";
        private const float k_TargetScalePulse = 0.95f;
        private const float k_PulsePerSec = 2f;

        private readonly GameScreen m_MyScreen;
        private readonly int m_ButtenIndex;

        private string m_Text;
        private InputManager m_InputManager;
        private SpriteFont m_ConsolasFont;
        private bool m_IsActive = false;
        private int m_TextIndex = 0;
        private List<string> m_TextList;
        private bool m_IsFirstFrame = true;
        private bool m_PrevIsMouseHoverButton = false;
        private Vector2 m_MaxTextSize = Vector2.Zero;
        private List<float> m_ButtenValues;

        public event EventHandler OnMouseLeftHover;
        public event EventHandler OnClick;
        public event EventHandler OnMouseChangedButten;
        public event EventHandler OnTextChange;
        public event EventHandler OnRsetsetButtens;

        public GameButten(GameScreen i_GameScreen, string i_Text, int i_ButtenIndex)
            : base(k_AssetName, i_GameScreen.Game)
        {
            m_TextList = new List<string>();
            m_TextList.Add(i_Text);
            m_ButtenIndex = i_ButtenIndex;
            m_MyScreen = i_GameScreen;
            m_MyScreen.Add(this);
        }
        

        protected override void LoadContent()
        {
            base.LoadContent();
            m_ConsolasFont = this.Game.Content.Load<SpriteFont>(@"Font\Consolas");
        }

        public override void Initialize()
        {
            base.Initialize();
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            m_Text = m_TextList[0];
        }

        private void initAnimations()
        {
            this.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, k_TargetScalePulse, k_PulsePerSec));
        }

        public float GetFloatValue()
        {
            return m_ButtenValues[m_TextIndex];
        }
        
        public void RangeValueButten(int i_RangeValue, int i_ClickJump, string i_Text)
        {
            List<string> tempTextList = new List<string>();
            m_ButtenValues = new List<float>();

            for (int i = i_RangeValue; i >= 0; i -= i_ClickJump)
            {
                m_ButtenValues.Add((float)i / (float)i_RangeValue);
                string textToAdd = i_Text + i.ToString();
                tempTextList.Add(textToAdd);
            }
           
            this.TextList = tempTextList;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_IsFirstFrame)
            {
                m_IsFirstFrame = false;
                this.TintColor = Color.Red;
                initAnimations();
            }
            else
            {
                bool currIsMouseHoverButton = IsMoueHover();

                if (m_IsActive)
                {
                    int textIndex = -1;
                    
                    if (m_InputManager.MouseState.LeftButton == ButtonState.Pressed && m_InputManager.PrevMouseState.LeftButton == ButtonState.Released && currIsMouseHoverButton || m_MyScreen.InputManager.KeyPressed(Keys.Enter))
                    {
                        if (OnClick != null)
                        {
                            OnClick(this, EventArgs.Empty); 
                        }
                    }
                    else if (m_MyScreen.InputManager.KeyPressed(Keys.PageUp) || m_InputManager.MouseState.ScrollWheelValue > m_InputManager.PrevMouseState.ScrollWheelValue || m_InputManager.MouseState.RightButton == ButtonState.Pressed && m_InputManager.PrevMouseState.RightButton == ButtonState.Released && currIsMouseHoverButton)
                    {
                        textIndex = (m_TextIndex - 1 + m_TextList.Count) % m_TextList.Count;
                    }
                    else if (m_MyScreen.InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown) || m_InputManager.MouseState.ScrollWheelValue < m_InputManager.PrevMouseState.ScrollWheelValue)
                    {
                        textIndex = (m_TextIndex + 1) % m_TextList.Count;
                    }

                    if (textIndex != -1)
                    {
                        m_TextIndex = textIndex;
                        m_Text = m_TextList[m_TextIndex];
                        if (OnTextChange != null)
                        {
                            OnTextChange(this, EventArgs.Empty);
                        }
                    }
                }

                ////check if mouse left hover
                if (m_PrevIsMouseHoverButton && !currIsMouseHoverButton)
                {
                    if (OnMouseLeftHover != null)
                    {
                        OnMouseLeftHover(this, EventArgs.Empty);
                    }
                    IsActive = false;

                }

                m_PrevIsMouseHoverButton = currIsMouseHoverButton;
            }
        }

        public bool IsMoueHover()
        {
            Point mousePosition = m_InputManager.MouseState.Position;
            bool IsOverButten = (mousePosition.X > this.Bounds.Left && mousePosition.X < this.Bounds.Right && mousePosition.Y > this.Bounds.Top && mousePosition.Y < this.Bounds.Bottom);
            
            if (IsOverButten && !IsActive)
            {
                OnMouseChangedButten(this, EventArgs.Empty);
            }

            return IsOverButten;
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            
            Vector2 size = m_ConsolasFont.MeasureString(m_Text);
            m_SpriteBatch.DrawString(m_ConsolasFont, m_Text, new Vector2(this.Bounds.Left + ((this.Width - size.X) / 2), this.Bounds.Top + this.Height / 3), this.TintColor);
        }
        
        protected override void InitBounds()
        {
            base.InitBounds();
        }

        private void fixButtonScale()
        {
            foreach(string text in m_TextList)
            {
                Vector2 size = m_ConsolasFont.MeasureString(m_Text);
                if(size.X > m_MaxTextSize.X)
                {
                    m_MaxTextSize = size;
                }
            }

            SetButtonScaleForText(m_MaxTextSize);
        }

        public void SetButtonScaleForText(Vector2 i_Size)
        {
            float scaleX = i_Size.X / this.WidthBeforeScale;
            float scaleY = i_Size.Y / this.HeightBeforeScale;

            this.Scales = new Vector2(scaleX * 2, scaleY * 2f);
        }

        public Vector2 MaxSize
        {
            get
            {
                return m_MaxTextSize;
            }
        }

        public string Text
        {
            get 
            {
                return m_Text; 
            }
            set
            {
                m_Text = value;
                m_TextList.Clear();
                m_TextList.Add(m_Text);
                fixButtonScale();
            }
        }

        public bool IsActive
        {
            get 
            {
                return m_IsActive;
            }
            set
            {
                if (value)
                {
                    this.TintColor = Color.Blue;
                    this.Animations.Restart();
                }
                else
                {
                    this.TintColor = Color.Red;
                    this.Animations.Reset();
                    this.Animations.Pause();
                }

                m_IsActive = value; 
            }
        }

        public void ButtonNextText() 
        {
                m_TextIndex = (m_TextIndex + 1) % m_TextList.Count; 
                m_Text = m_TextList[m_TextIndex];
        }

        public List<string> TextList
        {
            get
            {
                return m_TextList;
            }
            set
            {
                m_TextList = value;
                m_Text = m_TextList[0];
                m_TextIndex = 0;
                fixButtonScale();
                OnRsetsetButtens(this, EventArgs.Empty);
            }
        }

        public int Index
        {
            get { return m_ButtenIndex; }
        }
    }
}
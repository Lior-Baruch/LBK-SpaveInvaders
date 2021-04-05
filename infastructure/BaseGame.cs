using Infrastructure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace infastructure
{
    public class BaseGame : Game
    {
        protected InputManager m_InputManager;
        protected CollisionsManager m_CollisionManager;
        protected GraphicsDeviceManager m_Grapic;
        protected SoundManager m_SoundsManager;

        public BaseGame()
            : base()
        {
            m_Grapic = new GraphicsDeviceManager(this);
            this.Services.AddService(typeof(GraphicsDeviceManager), m_Grapic);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_InputManager = new InputManager(this);
            this.Services.AddService(typeof(InputManager), m_InputManager);
            
            m_CollisionManager = new CollisionsManager(this);
            this.Services.AddService(typeof(CollisionsManager), m_CollisionManager);

            m_SoundsManager = new SoundManager(this);
            this.Services.AddService(typeof(SoundManager), m_SoundsManager);
             
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime i_GameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(i_GameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}

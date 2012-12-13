using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace electrolyte
{
    class DeadState : DrawableGameComponent
    {
        private Texture2D background;
        private Rectangle bgFrame;

        public DeadState(Game game)
            : base(game)
        {
            base.Initialize();

            background = game.Content.Load<Texture2D>("gameover");
            bgFrame = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            GamePadState pad2 = GamePad.GetState(PlayerIndex.Two);
            if (kb.IsKeyDown(Keys.R) || 
                (pad1.IsConnected && pad1.IsButtonDown(Buttons.Start)) ||
                (pad2.IsConnected && pad2.IsButtonDown(Buttons.Start)))
            {
                StateManager.GetInstance().ResetState(StateManager.GameState.PLAY);
                StateManager.GetInstance().SetState(StateManager.GameState.PLAY);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameHelper.SpriteBatch.Draw(background, bgFrame, Color.White);

            base.Draw(gameTime);
        }
    }
}

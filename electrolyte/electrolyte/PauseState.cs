using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace electrolyte
{
    class PauseState : DrawableGameComponent
    {
        private Texture2D background;
        private Rectangle bgFrame;

        public PauseState(Game game)
            : base(game)
        {
            base.Initialize();

            background = game.Content.Load<Texture2D>("pause");
            bgFrame = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            GamePadState pad2 = GamePad.GetState(PlayerIndex.Two);
            if (kb.IsKeyDown(Keys.B) ||
                (pad1.IsConnected && pad1.IsButtonDown(Buttons.B)) ||
                (pad2.IsConnected && pad2.IsButtonDown(Buttons.B)))
            {
                StateManager.GetInstance().SetState(StateManager.GameState.PLAY);
            }
            if (kb.IsKeyDown(Keys.M) ||
                (pad1.IsConnected && pad1.IsButtonDown(Buttons.X)) || (pad2.IsConnected && pad2.IsButtonDown(Buttons.X)))
            {
                StateManager.GetInstance().ChangePlayStateLevel(1);
                StateManager.GetInstance().SetState(StateManager.GameState.MAIN_MENU);
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

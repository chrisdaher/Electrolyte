using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace electrolyte
{
    class SettingsState : DrawableGameComponent
    {
        private Texture2D background;
        private Rectangle bgFrame;
        private Texture2D mVolumeBar;
        private float mCurrentVolume;

        public SettingsState(Game game)
            : base(game)
        {
            base.Initialize();

            background = game.Content.Load<Texture2D>("settings-images/settings");
            mVolumeBar = game.Content.Load<Texture2D>("settings-images/volumebox");
            bgFrame = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            mCurrentVolume = MediaPlayer.Volume;
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
                StateManager.GetInstance().ChangePlayStateLevel(1);
                StateManager.GetInstance().SetState(StateManager.GameState.MAIN_MENU);
            }

            if ((pad1.IsConnected && pad1.IsButtonDown(Buttons.DPadLeft)) ||
                pad2.IsConnected && pad2.IsButtonDown(Buttons.DPadLeft) || kb.IsKeyDown(Keys.Down))
            {
                // Lower music volume
                if (MediaPlayer.Volume != 0)
                {
                    MediaPlayer.Volume -= 0.003f;
                    mCurrentVolume = MediaPlayer.Volume;
                }
            }

            if ((pad1.IsConnected && pad1.IsButtonDown(Buttons.DPadRight)) ||
               pad2.IsConnected && pad2.IsButtonDown(Buttons.DPadRight) || kb.IsKeyDown(Keys.Up))
            {
                // Raise music volume
                if (MediaPlayer.Volume != 1)
                {
                    MediaPlayer.Volume += 0.003f;
                    mCurrentVolume = MediaPlayer.Volume;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameHelper.SpriteBatch.Draw(background, bgFrame, Color.White);

            GameHelper.SpriteBatch.Draw(mVolumeBar, new Rectangle(Resolution.resWidth / 2 - mVolumeBar.Width / 2, (int)(Resolution.resHeight * 0.8), mVolumeBar.Width, 44), new Rectangle(0, 45, mVolumeBar.Width, 44), Color.Gray);
            GameHelper.SpriteBatch.Draw(mVolumeBar, new Rectangle(Resolution.resWidth / 2 - mVolumeBar.Width / 2, (int)(Resolution.resHeight * 0.8), (int)(mVolumeBar.Width * mCurrentVolume), 44), new Rectangle(0, 45, mVolumeBar.Width, 44), Color.Blue);
            GameHelper.SpriteBatch.Draw(mVolumeBar, new Rectangle(Resolution.resWidth / 2 - mVolumeBar.Width / 2, (int)(Resolution.resHeight * 0.8), mVolumeBar.Width, 44), new Rectangle(0, 0, mVolumeBar.Width, 44), Color.White);

            //Draw the text
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Volume: " + ((int)(mCurrentVolume * 100)),
                new Vector2(Resolution.resWidth / 2 - mVolumeBar.Width / 2 + 10, (int)(Resolution.resHeight * 0.82)), Color.Red);

            base.Draw(gameTime);
        }
    }
}

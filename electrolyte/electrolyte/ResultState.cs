using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace electrolyte
{
    class ResultState : DrawableGameComponent
    {
        private int mP1Score;
        private int mP2Score;
        private Score.AlphaScore mAlpha;
        private string mTime;

        private string mP1Time;
        private string mP2Time;
        private Score.AlphaScore mP1Alpha;
        private Score.AlphaScore mP2Alpha;

        private Texture2D resultsImage;
        private bool mSolo;

        public ResultState(Game game, int player1Score, int player2Score, Score.AlphaScore score, string time) :
            base(game)
        {
            mP1Score = player1Score;
            mP2Score = player2Score;
            mAlpha = score;
            mTime = time;

            mSolo = false;
        }

        public ResultState(Game game, int player1Score, int player2Score, Score.AlphaScore score1, Score.AlphaScore score2, string time1, string time2) :
            base(game)
        {
            mP1Score = player1Score;
            mP2Score = player2Score;
            mP1Time = time1;
            mP2Time = time2;
            mP1Alpha = score1;
            mP2Alpha = score2;

            mSolo = true;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            GamePadState g1 = GamePad.GetState(PlayerIndex.One);
            GamePadState g2 = GamePad.GetState(PlayerIndex.Two);

            if (kb.IsKeyDown(Keys.Enter) ||
                (g1.IsConnected && g1.IsButtonDown(Buttons.Start)) ||
                (g2.IsConnected && g2.IsButtonDown(Buttons.Start)))
            {
                StateManager.GetInstance().IncreasePlayLevel();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            resultsImage = GameHelper.GetGame().Content.Load<Texture2D>("results");
            GameHelper.SpriteBatch.Draw(resultsImage, new Rectangle(0,0,Resolution.resWidth,Resolution.resHeight), Color.White);
            if (!mSolo)
            {
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "TIME: " + mTime, new Vector2(100, 100), Color.White);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Plus + Score: " + mP1Score.ToString(), new Vector2(100, 150), Color.Red);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Minus - Score: " + mP2Score.ToString(), new Vector2(100, 200), Color.Red);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Score: " + mAlpha.ToString(), new Vector2(100, 250), Color.Red);
            }
            else
            {
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Plus +", new Vector2(100, 100), Color.White);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Collected: " + mP1Score.ToString(), new Vector2(100, 150), Color.Red);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Time " + mP1Time, new Vector2(100, 200), Color.Red);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Grade: " + mP1Alpha, new Vector2(100, 250), Color.Red);

                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Minus -", new Vector2(500, 100), Color.White);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Collected: " + mP2Score.ToString(), new Vector2(500, 150), Color.Red);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Time " + mP2Time, new Vector2(500, 200), Color.Red);
                GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Grade: " + mP2Alpha, new Vector2(500, 250), Color.Red);
            }
            
            base.Draw(gameTime);
        }
    }
}

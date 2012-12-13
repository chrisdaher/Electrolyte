using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class Hud : DrawableGameComponent
    {
        public const float HUD_HEIGHT = 60f;

        // Text display stuff
        private const int TEXT_LINE_LIMIT = 35;
        private const int TEXT_LINE_HEIGHT= 20;
        private const int TEXTBOX_WIDTH_PADDING = 5;
        private const int TEXTBOX_HEIGHT_PADDING = 5;
        private const int MAX_DISPLAY_LINE_TEXT = 2;
        private float mTextDisplayTimer = 0f;
        private float mTextDisplayTimeLimit = 3000;
        List<List<String>> mLevelText;
        List<String> mCurrentText;
        // End Text display stuff
        
        private Pointf mPosition;
        private Texture2D mTextBox;

        private int mPlayer1Score;
        private int mPlayer2Score;
        private static int playerDeathIndex = -1;
        
        private float HUD_WIDTH;
        private double mCurrentTime;
        public double CurrentTime { get { return mCurrentTime; } }

        public Hud(Pointf position, Game game, int level)
            : base(game)
        {
            HUD_WIDTH = Game.Window.ClientBounds.Width;
            mTextBox = game.Content.Load<Texture2D>("textbox");
            mPosition = position;
            mCurrentTime = 0;
            playerDeathIndex = -1;

            // Set up variables to display text for level one
            mCurrentText = new List<String>();
            mLevelText = new List<List<String>>();

            // Text to display for level 1
            switch (level)
            {
                case 1:
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Level1PlusSpeech1, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Level1MinusSpeech1, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Level1Info, TEXT_LINE_LIMIT));
                    break;
                case 2:
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.OneVsOneNarration, TEXT_LINE_LIMIT));
                    break;
                case 3:
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1PlusSpeech1, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1BossSpeech1, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1MinusSpeech1, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1PlusSpeech2, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1BossSpeech2, TEXT_LINE_LIMIT));
                    break;
            }
        }

        public void GameWin(int level, PlayerIndex winner)
        {
            mLevelText.Clear();
            switch (level)
            {
                case 3:
                    if (winner != PlayerIndex.Three)
                    {
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1PlusSpeechVictory1, TEXT_LINE_LIMIT));
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1BossSpeechDefeat1, TEXT_LINE_LIMIT));
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1MinusSpeechVictory1, TEXT_LINE_LIMIT));
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1BossSpeechDefeat2, TEXT_LINE_LIMIT));
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1MinusSpeechVictory2, TEXT_LINE_LIMIT));
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1PlusSpeechVictory2, TEXT_LINE_LIMIT));
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1MinusSpeechVictory3, TEXT_LINE_LIMIT));
                    }
                    else
                    {
                        mLevelText.Add(SpeechText.getTextArray(SpeechTextState.Boss1BossSpeechVictory1, TEXT_LINE_LIMIT));
                    }
                    break;
                default:
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.PlusSpeechVictory, TEXT_LINE_LIMIT));
                    mLevelText.Add(SpeechText.getTextArray(SpeechTextState.MinusSpeechVictory, TEXT_LINE_LIMIT));
                    break;
            }
        }

        public void UpdatePlayerScore(int p1, int p2)
        {
            mPlayer1Score = p1;
            mPlayer2Score = p2;
        }

        public override void Update(GameTime gameTime)
        {
            mCurrentTime += gameTime.ElapsedGameTime.Milliseconds;
            mTextDisplayTimer += gameTime.ElapsedGameTime.Milliseconds; // Added timer for text
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(mCurrentTime);
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, "TIME: " + time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2"), new Vector2(mPosition.X + HUD_WIDTH / 2 - 40, mPosition.Y), Color.White);
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Plus +", new Vector2(mPosition.X, mPosition.Y), Color.Red);
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Score: " + mPlayer1Score.ToString(), new Vector2(mPosition.X, mPosition.Y + 30), Color.Red);
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Minus -", new Vector2(mPosition.X + HUD_WIDTH - 100, mPosition.Y ), Color.SkyBlue);
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, "Score: " + mPlayer2Score.ToString(), new Vector2(mPosition.X + HUD_WIDTH - 100, mPosition.Y + 30), Color.SkyBlue);
            if (playerDeathIndex == -1)
            {
                DrawTextBox();
            }
            else
            {
                DrawFallenTextBox();
            }
            base.Draw(gameTime);
        }

        public static void SetPlayerDeathIndex(int index)
        {
            playerDeathIndex = index;
        }

        private void DrawFallenTextBox()
        {
            String textLine = "Plus has fallen!";
            if (playerDeathIndex == 2)
            {
                textLine = "Minus has fallen!";
            }
            else
            {
                if (playerDeathIndex == 0)
                {
                    textLine = "Both our heroes has fallen!";
                }
            }

            // Image for Text box (Can stretch it out as needed)
            GameHelper.SpriteBatch.Draw(mTextBox,
                new Rectangle(GraphicsDeviceManager.DefaultBackBufferWidth / 4,
                (int)mPosition.Y + 1,
                GraphicsDeviceManager.DefaultBackBufferWidth / 2,
                mTextBox.Height - 15), Color.White);

            // Draw the text line
            GameHelper.SpriteBatch.DrawString(GameHelper.Font, textLine,
                new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 4 + TEXTBOX_WIDTH_PADDING,
                (int)mPosition.Y + TEXTBOX_HEIGHT_PADDING), Color.White);
        }

        private void DrawTextBox()
        {
            // If there is text to be shown
            if (mLevelText.Count != 0 || mCurrentText.Count != 0)
            {
                // Image for Text box (Can stretch it out as needed)
                GameHelper.SpriteBatch.Draw(mTextBox,
                    new Rectangle(GraphicsDeviceManager.DefaultBackBufferWidth / 4,
                    (int)mPosition.Y + 1,
                    GraphicsDeviceManager.DefaultBackBufferWidth / 2,
                    mTextBox.Height - 15), Color.White);

                // If the text display time is up
                if (mTextDisplayTimer > mTextDisplayTimeLimit)
                {
                    // Remove it from current text
                    int numElements = mCurrentText.Count;
                    for (int j = 0; j < MAX_DISPLAY_LINE_TEXT; j++)
                    {
                        if (j < numElements)
                        {
                            mCurrentText.RemoveAt(0);
                        }
                    }
                    mTextDisplayTimer = 0f;
                }

                // If the current text is finished being displayed 
                // and there is more speech text to display, load it in current text
                if (mCurrentText.Count == 0 && mLevelText.Count > 0)
                {
                    mCurrentText = mLevelText.First();
                    mLevelText.Remove(mCurrentText);
                }

                // Display the current text
                int pixelOffset = 0;
                for (int i = 0; i < MAX_DISPLAY_LINE_TEXT; i++)
                {
                    // Get a text line
                    if (i < mCurrentText.Count)
                    {
                        // Get the text line
                        String textLine = (mCurrentText.ToArray())[i];

                        // Draw the text line
                        GameHelper.SpriteBatch.DrawString(GameHelper.Font, textLine,
                            new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 4 + TEXTBOX_WIDTH_PADDING,
                            (int)mPosition.Y + TEXTBOX_HEIGHT_PADDING + pixelOffset), Color.White);
                        pixelOffset += TEXT_LINE_HEIGHT;
                    }
                }
            }  
        }
    }
}

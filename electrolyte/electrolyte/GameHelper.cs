using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    static class GameHelper
    {
        private static Game mGame;
        private static SpriteFont mFont;

        public static SpriteFont Font
        {
            get
            {
                if (mFont == null) throw new Exception("Attempted to use GameHelper.Font without setting GameHelper.SetFont");
                return mFont;
            }
        }

        public static SpriteBatch SpriteBatch
        {
            get
            {
                if (mGame == null) throw new Exception("Attempted to use GameHelper.SpriteBatch without setting GameHelper.SetGame");
                return (SpriteBatch)mGame.Services.GetService(typeof(SpriteBatch));
            }
        }

        public static void SetFont(SpriteFont font)
        {
            mFont = font;
        }

        public static Game GetGame()
        {
            return mGame;
        }

        public static void SetGame(Game game)
        {
            mGame = game;
        }

        public static void SetSpeechText(Game game)
        {
            SpeechText.initializeTextEng();
        }
    }
}

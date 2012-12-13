using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class WaterModel : Model
    {
        private int interval = 40;
        private int currentTime=0;
        private enum Sprites
        {
            FIRST,
            SECOND,
            THIRD,
            FOURTH,
            FIFTH,
            LAST
        }

        public WaterModel(Game game, Pointf position) :
            base(game, position, new Size(10, 2), new Velocity())
        {

        }
        protected override Texture2D LoadTexture()
        {

            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture = Game.Content.Load<Texture2D>("watersprite");
            return rectangleTexture;
        }

        public override void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (currentTime >= interval)
            {
                currentTime = 0;
                if (mFrameIndex + 1 >= (int)Sprites.LAST)
                {
                    mFrameIndex = 0;
                }
                else
                {
                    mFrameIndex++;
                }
            }
            base.Update(gameTime);
        }

        protected override void GenerateFrames()
        {
            mFrames.Add(new Frame(new Rectangle(75, 62, 45, 20)));
            mFrames.Add(new Frame(new Rectangle(75, 82, 45, 20)));
            mFrames.Add(new Frame(new Rectangle(75, 102, 45, 20)));
            mFrames.Add(new Frame(new Rectangle(75, 122, 45, 20)));
            mFrames.Add(new Frame(new Rectangle(75, 142, 45, 20)));
        }
    }
}

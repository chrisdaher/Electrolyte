using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class Wall : Model
    {
        protected Color mColor;
      protected bool isHorizontal;
        public enum Colors
        {
            WHITE,
            BLACK,
            BLUE,
            RED
        };

        public Wall(Game game, Pointf position, Size size, Colors color) :
            base(game, position, size, new Velocity(), (int)color)
        {
            mBlocksOnCollision = true;
            isHorizontal = true;
            if (size.Height > size.Width)
            {
                isHorizontal = false;
            }
        }

        protected override void Init(int param)
        {
            Colors col = (Colors)param;
            switch (col)
            {
                case Colors.WHITE:
                    mColor = Color.White;
                    break;
                case Colors.BLUE:
                    mColor = Color.Blue;
                    break;
                case Colors.BLACK:
                    mColor = Color.Black;
                    break;
                case Colors.RED:
                    mColor = Color.Red;
                    break;
            }
        }

        protected override Microsoft.Xna.Framework.Graphics.Texture2D LoadTexture()
        {
            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
              rectangleTexture = Game.Content.Load<Texture2D>("Wall");

            return rectangleTexture;
        }

        protected override void GenerateFrames()
        {
          if (isHorizontal)
          {
            mFrames.Add(new Frame(new Rectangle(0, 0, (int)mSize.Width, (int)mSize.Height)));
          }
          else
          {
            mFrames.Add(new Frame(new Rectangle(3, 0, (int)mSize.Width, (int)mSize.Height)));
          }
        }
    }
}

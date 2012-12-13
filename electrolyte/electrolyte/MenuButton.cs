using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace electrolyte
{
    class MenuButton : DrawableGameComponent
    {
        private Rectangle mPosition;
        private bool mIsColliding;
        private Texture2D mImage;
        public bool IsColliding { get { return mIsColliding; } }
        public int selectedIndex = 0;
        public int index = 0;

        public MenuButton(Game game, Rectangle position, Texture2D image, int selectedValue) :
            base(game)
        {
            mPosition = position;
            mImage = image;
            index = selectedValue;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
            mIsColliding = mouseRect.Intersects(mPosition) && Mouse.GetState().LeftButton == ButtonState.Pressed;
            if (mouseRect.Intersects(mPosition) || selectedIndex == index)
            {
                mPosition = new Rectangle(mPosition.X, mPosition.Y, mImage.Width, mImage.Height);
            }
            else
            {
                mPosition = new Rectangle(mPosition.X, mPosition.Y, (int)(mImage.Width * 0.7f), (int)(mImage.Height * 0.7f));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameHelper.SpriteBatch.Draw(mImage, new Rectangle(mPosition.X, mPosition.Y, mPosition.Width, mPosition.Height), Color.White);
            base.Draw(gameTime);
        }

        public void SetSelected(int value)
        {
            selectedIndex = value;
        }
    }
}

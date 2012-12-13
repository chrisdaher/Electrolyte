using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    abstract class Drawable : DrawableGameComponent
    {
        // component xna
        protected Texture2D mTexture;
        protected Texture2D Texture { get { return mTexture; } }

        protected float mScale;
        protected float mRotation;

        protected Displayable mDisplayable;
     
        protected Frame mCurrentFrame;

        public Drawable(Game game,
                        Displayable displayable,
                        int parameterized)
            : base(game)
        {
            Init(parameterized);

            Visible = true;
            mDisplayable = displayable;

            mTexture = LoadTexture();            

            mCurrentFrame = new Frame();
            mScale = 1.0f;
            mRotation = 0.0f;

            base.Initialize();
        }

        public Drawable(Game game,
                     Displayable displayable)
            : this(game, displayable, 0)
        {
        }

        protected virtual void Init(int param) { }

        protected abstract Texture2D LoadTexture();

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            mDisplayable.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameHelper.SpriteBatch.Draw(mTexture,
                                        mDisplayable.Position.VectorPoint,
                                        mCurrentFrame.FrameArea,
                                        Color.White,
                                        mRotation,
                                        mDisplayable.Origin,
                                        mScale,
                                        SpriteEffects.None,
                                        0);
            base.Draw(gameTime);
        }
    }
}

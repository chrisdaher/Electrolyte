using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class Collectable : Model
    {
        private int mAnimCntr = 0;
        private const int ANIM_CNTR_INTERVAL = 200;
        private bool mGoingDown = true;
        private int mPoints;

        public Collectable(Game game, Pointf position, int points) :
            base(game, position, new Size(30,30), new Velocity())
        {
            mPoints = points;
            DisableFreeFall();
        }

        protected override Texture2D LoadTexture()
        {
            Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture = Game.Content.Load<Texture2D>("atom_full");
            return rectangleTexture;
        }

        public override void Update(GameTime gameTime)
        {
            //go up and down
            if (ProjectHelper.IsAnimationEnabled)
            {
                mAnimCntr += gameTime.ElapsedGameTime.Milliseconds;
                if (mAnimCntr >= ANIM_CNTR_INTERVAL)
                {
                    mAnimCntr = 0;

                    if (mGoingDown)
                    {
                        this.mPosition.Y += 2.0f;
                    }
                    else
                    {
                        this.mPosition.Y -= 2.0f;
                    }
                    mGoingDown = !mGoingDown;
                }
            }
            base.Update(gameTime);
        }

        protected override void GenerateFrames()
        {
            mFrames.Add(new Frame(new Rectangle(0, 0, 246, 217)));
        }

        public int GetPoints()
        {
            return mPoints;
        }
    }
}

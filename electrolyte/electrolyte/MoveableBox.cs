using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class MoveableBox : Model
    {
        private int mBlockedWallId;

        public MoveableBox(Game game, Pointf position, Size size) :
            base(game, position, size, new Velocity())
        {
            mBlockedWallId = -1;
            EnableFreeFall();
        }


        protected override Microsoft.Xna.Framework.Graphics.Texture2D LoadTexture()
        {
          Texture2D rectangleTexture = new Texture2D(GameHelper.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
          rectangleTexture = Game.Content.Load<Texture2D>("Box");
            return rectangleTexture;
        }

        public override void NotColliding(Model with)
        {
            if (with.ModelId == mBlockedWallId)
            {
                mBlockedWallId = -1;
            }
            EnableFreeFall();
        }

        public override void Collided(Model with, Displayable.CollisionStatus status)
        {
            if (status == Displayable.CollisionStatus.TOP) return; //lol???
            if (status == Displayable.CollisionStatus.BOTTOM)
            {
                DisableFreeFall();
                this.Velocity = new Velocity();
                return;
            }
            
            if (with.GetType() == typeof(Wall))
            {
                mBlockedWallId = with.ModelId;
                return;
            }

            if (with.GetType() != typeof(Character))
            {
                return;
            }

            if (mBlockedWallId != -1) return;

            if (status == Displayable.CollisionStatus.LEFT)
            {
                this.mPosition.X = with.Left - this.Origin.X;
            }
            else if (status == Displayable.CollisionStatus.RIGHT)
            {
                this.mPosition.X = with.Right + this.Origin.X;
            }
        }

        protected override void GenerateFrames()
        {
          mFrames.Add(new Frame(new Rectangle(0, 24, 48, 24)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    abstract class Model : Drawable
    {
        protected List<Frame> mFrames;
        protected int mFrameIndex = 0;

        // Expose from Displayable
        public Pointf mPosition { get { return mDisplayable.Position; } set { mDisplayable.Position = value; } }
        public Velocity Velocity { get { return mDisplayable.Velocity; } set { mDisplayable.Velocity = value; } }
        public float Top { get { return mDisplayable.Top; } }
        public float Bottom { get { return mDisplayable.Bottom; } }
        public float Left { get { return mDisplayable.Left; } }
        public float Right { get { return mDisplayable.Right; } }
        public Vector2 Origin { get { return mDisplayable.Origin; } }
        public Size mSize { get { return mDisplayable.Size; } set { mDisplayable.Size = value; } }

        private int mId;
        public int ModelId { get { return mId; } }
        private static int idCntr = 0;
        private bool mDestroy;
        public bool IsDestroyed { get { return mDestroy; } }

        // collision bypass
        protected bool mBlocksOnCollision = true;
        public bool BlocksOnCollision { get { return mBlocksOnCollision; } }

        // free fall code
        protected bool mIsFreeFall;
        const float GRAVITY = 4.0f;
        private float gravValue;

        public Model(Game game,
                     Pointf position,
                     Size size,
                     Velocity velocity,
                     int parameterized) :
            base(game,
                 new Displayable(position, size, velocity), parameterized)
        {
            mDestroy = false;
            mId = idCntr++;
            mIsFreeFall = false;
            mFrames = new List<Frame>();
            GenerateFrames();

            mPosition.X += size.Width / 2;
            mPosition.Y += size.Height / 2;            
        }

        public Model(Game game,
                     Pointf position,
                     Size size,
                     Velocity velocity) :
            this(game, position, size, velocity, 0)
        { }

        public void Destroy()
        {
            mDestroy = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (mIsFreeFall)
            {
                Velocity.PointDown();
                Velocity.SetYSpeed(gravValue);
                gravValue /= 0.9f;
            }

            mCurrentFrame = mFrames[mFrameIndex];
            mScale = mSize.Width / mCurrentFrame.FrameArea.Width;
            base.Update(gameTime);
        }

        public void EnableFreeFall()
        {
            mIsFreeFall = true;
            gravValue = GRAVITY;
        }

        public void DisableFreeFall()
        {
            mIsFreeFall = false;
        }

        public bool DoesFreeFall()
        {
            return mIsFreeFall;
        }

        protected abstract void GenerateFrames();

        public virtual void Collided(Model with, Displayable.CollisionStatus status)
        { }

        public virtual void NotColliding(Model with)
        { }

        public class Collision
        {
            public Displayable.CollisionStatus Status;
            public bool BlocksMovement;

            public Collision()
            {
                Status = Displayable.CollisionStatus.NONE;
                BlocksMovement = true;
            }

            public Collision(Displayable.CollisionStatus status, bool blocks)
            {
                Status = status;
                BlocksMovement = blocks;
            }
        }

        public virtual Collision IsColliding(Model other)
        {
            Displayable.CollisionStatus status = mDisplayable.IsColliding(other.mDisplayable);
            if (status != Displayable.CollisionStatus.NONE)
            {
                switch (status)
                {
                    case Displayable.CollisionStatus.BOTTOM:
                        if (other.GetType() == typeof(Character))
                        {
                            Character c = (Character)other;
                            if (c.mCurrentJumpState == Character.State.Idle && !c.JumpRequest && mBlocksOnCollision)
                            {
                                other.mPosition.Y = this.Top - other.Origin.Y;
                            }
                        }
                        else if (this.mBlocksOnCollision)
                        {
                            other.mPosition.Y = this.Top - other.Origin.Y;
                        }
                        break;
                }
                if (!other.IsDestroyed && !this.IsDestroyed)
                {
                    this.Collided(other, status);
                    other.Collided(this, status);
                }
            }
            else
            {
                if (!other.IsDestroyed && !this.IsDestroyed)
                {
                    this.NotColliding(other);
                    other.NotColliding(this);
                }
            }
            return new Collision(status, this.mBlocksOnCollision && other.mBlocksOnCollision);
        }

        public override bool Equals(object obj)
        {
            Model other = (Model)obj;
            return other.mId == this.mId;
        }
    }
}

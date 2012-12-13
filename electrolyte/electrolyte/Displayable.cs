using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace electrolyte
{
    class Displayable
    {
        // position of this component
        public Pointf Position { get; set; }
        public float Top { get { return Position.Y - mOrigin.Y; } }
        public float Bottom { get { return Position.Y + mOrigin.Y; } }
        public float Left { get { return Position.X - mOrigin.X; } }
        public float Right { get { return Position.X + mOrigin.X; } }

        private const int INTERVAL = 5;
        protected Rectangle TopWall { get { return new Rectangle((int)Left, (int)Top, (int)(Right - Left), INTERVAL); } }
        protected Rectangle BottomWall { get { return new Rectangle((int)Left, (int)Bottom - INTERVAL, (int)(Right - Left), INTERVAL); } }
        protected Rectangle LeftWall { get { return new Rectangle((int)Left, (int)Top, INTERVAL, (int)(Bottom - Top)); } }
        protected Rectangle RightWall { get { return new Rectangle((int)Right - INTERVAL, (int)Top, INTERVAL, (int)(Bottom - Top)); } }
        protected Rectangle AreaWall { get { return new Rectangle((int)Left, (int)Top, (int)(Right - Left), (int)(Bottom - Top)); } }

        protected Vector2 mOrigin { get { return new Vector2(this.Size.Width / 2, this.Size.Height / 2); } }
        public Vector2 Origin { get { return mOrigin; } }

        // move code
        protected Velocity mVelocity {get;set;}
        public Velocity Velocity { get { return mVelocity; } set { mVelocity = value; } }
        public bool IsMoving { get{return (mVelocity.Direction.X != 0 || mVelocity.Direction.Y != 0); }}

        // size of this component
        public Size Size { get; set; }

        public Displayable(Pointf position,
                           Size size,
                           Velocity velocity)
        {
            Position = new Pointf(position);
            Size = new Size(size);
            mVelocity = new Velocity(velocity);
        }

        public virtual void Update(GameTime time)
        {
            // update the position with velocity
            Position = mVelocity.Move(Position);
        }


        public enum CollisionStatus
        {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT,
            UNKNOWN,
            NONE
        }

        public virtual CollisionStatus IsColliding(Displayable other)
        {
           if (!other.AreaWall.Intersects(this.AreaWall)) return CollisionStatus.NONE;
            if (other.LeftWall.Intersects(this.RightWall))
            {
                return CollisionStatus.LEFT;
            }
            if (other.RightWall.Intersects(this.LeftWall))
            {
                return CollisionStatus.RIGHT;
            }
            if (other.BottomWall.Intersects(this.TopWall))
            {
                return CollisionStatus.BOTTOM;
            }
            if (other.TopWall.Intersects(this.BottomWall))
                return CollisionStatus.TOP;
            if (other.AreaWall.Intersects(this.AreaWall)) 
                return CollisionStatus.UNKNOWN;
            return CollisionStatus.NONE;
        }
    }
}

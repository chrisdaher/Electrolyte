using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace electrolyte
{
    interface IVelocity
    {
        Vector2 Direction { get; }
        float Speed { get; }
    }

    class Velocity
    {
        private Vector2 mDirection;
        public Vector2 Direction { get { return mDirection; } set { mDirection = value; } }

        private Vector2 mSpeed;
        public Vector2 Speed { get { return mSpeed; } set { mSpeed = value; } }

        public Velocity()
        {
            mSpeed = new Vector2();
            mDirection = new Vector2();
        }

        public Velocity(Velocity other)
        {
            mSpeed = new Vector2(other.Speed.X, other.Speed.Y);
            mDirection = new Vector2(other.Direction.X, other.Direction.Y);
        }

        public Velocity(Vector2 vector, Vector2 speed)
        {
            vector = Vector2.Normalize(vector);
            mDirection = new Vector2(vector.X, vector.Y);
            mSpeed = new Vector2(speed.X, speed.Y);
        }

        public Pointf Move(Pointf currentPosition)
        {
            return new Pointf(currentPosition.X += mSpeed.X * mDirection.X,
                              currentPosition.Y += mSpeed.Y * mDirection.Y);
        }

        public void SetYSpeed(float speed)
        {
            mSpeed.Y = speed;
        }

        public void SetXSpeed(float speed)
        {
            mSpeed.X = speed;
        }

        public void PointDown()
        {
            if (mDirection.Y == 0) mDirection.Y = 1;

            if (mDirection.Y < 0)
            {
                mDirection.Y = -mDirection.Y;
            }
        }

        public void PointUp()
        {
            if (mDirection.Y == 0) mDirection.Y = -1;

            if (mDirection.Y > 0)
            {
                mDirection.Y = -mDirection.Y;
            }
        }

        public Vector2 GetVector()
        {
            return Vector2.Multiply(mDirection, mSpeed);
        }

    }
}

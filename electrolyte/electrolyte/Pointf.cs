using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace electrolyte
{
    class Pointf
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2 VectorPoint { get { return new Vector2(X, Y); } }

        public Pointf()
        {
            X = 0;
            Y = 0;
        }

        public Pointf(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Pointf(Pointf other)
        {
            X = other.X;
            Y = other.Y;
        }

        public override bool Equals(object obj)
        {
            Pointf other = (Pointf)obj;
            return (X == other.X && Y == other.Y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

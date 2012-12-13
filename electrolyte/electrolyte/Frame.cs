using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace electrolyte
{
    class Frame
    {
        protected Rectangle mArea;
        public Rectangle FrameArea { get { return mArea; } }

        public Vector2 FrameOrigin { get { return new Vector2(mArea.X,mArea.Y); } }

        public Frame()
        {
            mArea = new Rectangle();
        }

        public Frame(Rectangle area)
        {
            mArea = new Rectangle(area.X, area.Y, area.Width, area.Height);
        }

        public Frame(Pointf position, Size size)
        {
            mArea = new Rectangle((int)position.X, (int)position.Y,
                                  (int)size.Width, (int)size.Height);
        }

        public Frame(Frame other)
        {
            mArea = new Rectangle(other.FrameArea.X, other.FrameArea.Y,
                                  other.FrameArea.Width, other.FrameArea.Height);
        }

    }
}

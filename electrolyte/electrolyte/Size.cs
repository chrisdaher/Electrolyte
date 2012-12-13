using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace electrolyte
{
    class Size
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Size(Size other)
        {
            Width = other.Width;
            Height = other.Height;
        }
    }
}

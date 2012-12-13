using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace electrolyte
{
    class Range
    {
        private double mUpperBound;
        private double mLowerBound;

        public double UpperBound { get { return mUpperBound; } }
        public double LowerBound { get { return mLowerBound; } }

        public Range(double lowerBound, double upperBound)
        {
            mUpperBound = upperBound;
            mLowerBound = lowerBound;
        }

        public bool InRange(double value)
        {
            return (value >= mLowerBound && value <= mUpperBound);
        }
    }
}

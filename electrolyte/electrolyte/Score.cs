using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace electrolyte
{
    class Score
    {
        public enum AlphaScore
        {
            A,
            B,
            C,
            D,            
            F
        }

        private Dictionary<AlphaScore, Range> mAlphaRange;

        public Score(Dictionary<AlphaScore, Range> dic)
        {
            mAlphaRange = dic;
        }

        public AlphaScore GetAlphaFromTimeMs(double ms)
        {
            foreach (AlphaScore alpha in mAlphaRange.Keys)
            {
                if (mAlphaRange[alpha].InRange(ms)) return alpha;
            }
            throw new Exception("Invalid range");
        }

        public static Score GenerateDefaultScore()
        {
            // define the score mapper
            Dictionary<Score.AlphaScore, Range> mapper = new Dictionary<Score.AlphaScore, Range>();
            mapper.Add(Score.AlphaScore.A, new Range(0, TimeSpan.FromMinutes(0.5).TotalMilliseconds));
            mapper.Add(Score.AlphaScore.B, new Range(TimeSpan.FromMinutes(0.5).TotalMilliseconds, TimeSpan.FromMinutes(1).TotalMilliseconds));
            mapper.Add(Score.AlphaScore.C, new Range(TimeSpan.FromMinutes(1).TotalMilliseconds, TimeSpan.FromMinutes(1.5).TotalMilliseconds));
            mapper.Add(Score.AlphaScore.D, new Range(TimeSpan.FromMinutes(1.5).TotalMilliseconds, TimeSpan.FromMinutes(2).TotalMilliseconds));
            mapper.Add(Score.AlphaScore.F, new Range(TimeSpan.FromMinutes(2).TotalMilliseconds, int.MaxValue));

            return new Score(mapper);
        }
    }
}

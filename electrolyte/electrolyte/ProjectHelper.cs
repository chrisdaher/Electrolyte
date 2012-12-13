using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace electrolyte
{
    static class ProjectHelper
    {
        private const bool mSoundEnabled = true;
        private const bool mAnimationEnabled = true;
        private const bool mDebugNoKill = false;

        public static bool IsDebugNoKill { get { return mDebugNoKill; } }
        public static bool IsAnimationEnabled { get { return mAnimationEnabled; } }
        public static bool IsSoundEnabled { get { return mSoundEnabled; } }
    }
}

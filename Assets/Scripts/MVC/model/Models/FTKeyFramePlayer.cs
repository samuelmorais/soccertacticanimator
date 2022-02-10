using System;
using UnityEngine;

namespace FootTactic{
    public class FTKeyFramePlayer : FTKeyFrame
    {
        public FTKeyFramePlayer(Vector3 vec, TimeSpan time, bool smooth = false) : base(vec, time, smooth)
        {
            this.vec = vec;
            this.time = time;
            this.smooth = smooth;
        }

        public float Rot { get; set; }

        public FTEnums.FTAnimationType AnimType { get; set; }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FootTactic{
    public class FTKeyFrame : IFTKeyFrame, IComparable
    {
        
        public Vector3 vec
        {
            get;
            set;
        }

        // The time of the keyframe, which is also the unique identifier.
        // We are using TimeSpan here rather than float to avoid floating point inaccuracies.
        public TimeSpan time;

        // This boolean indicates wether SmoothStep blending should be used for this keyframe.
        // If this is false then percentage blend is used.
        public bool smooth;

        public int animIndex;

        public float Rotation
        {
            get;
            set;
        }

        public FTKeyFrame(Vector3 vec, TimeSpan time, bool smooth = false, int animIndex = 0, float rotation = Mathf.Infinity)
        {
            this.vec = vec;
            this.time = time;
            this.smooth = smooth;
            this.animIndex = animIndex;
            this.Rotation = rotation;
        }

        // Comparison for Array.Sort
        public int CompareTo(object other)
        {
            // Send all null keyframes to the back of the array
            if (other == null)
                return -1;

            // Sort actual keyframes
            FTKeyFrame otherKf = other as FTKeyFrame;
            if (otherKf != null)
            {
                return time.CompareTo(otherKf.time);
            }
            else
            {
                throw new ArgumentException("Comparison object is not a FTKeyFramez!");
            }
        }


    }

    
}

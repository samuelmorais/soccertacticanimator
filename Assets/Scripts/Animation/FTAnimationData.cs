using System;
using System.Collections.Generic;
using UnityEngine;

namespace FootTactic
{
    public class FTAnimationData
    {
        public FTAnimationData(
            string name,
            int animationIndex,
            float duration,
            List<FTAnimEvent> events,
            Vector3 finalPosition,
            float finalRotation
            )
        {
            Name = name;
            AnimationIndex = animationIndex;
            Duration = duration;
            Events = events;
            FinalPosition = finalPosition;
            FinalRotation = finalRotation;
        }

        public string Name { get; private set; }
        public int AnimationIndex { get; private set; }
        public float Duration { get; private set; }
        public List<FTAnimEvent> Events { get; private set; }
        public Vector3 FinalPosition {get; private set;}

        public float FinalRotation {get; private set;}
    }
}

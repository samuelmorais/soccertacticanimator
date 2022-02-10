using System;
using UnityEngine;

namespace FootTactic
{
    public class FTKeyFrameBall : FTKeyFrame
    {
        public BallEventData.BodyPartEnum BodyPart;

        public BallEventData.ActionEnum Action;

        public int playerIndex;
        public FTKeyFrameBall(Vector3 vec, TimeSpan time, BallEventData.BodyPartEnum bodyPart, BallEventData.ActionEnum action, int playerIndex, bool smooth = false) : base(vec, time, smooth)
        {
            this.vec = vec;
            this.time = time;
            this.smooth = smooth;
            this.BodyPart = bodyPart;
            this.Action = action;
            this.playerIndex = playerIndex;
        }
        


    }
}


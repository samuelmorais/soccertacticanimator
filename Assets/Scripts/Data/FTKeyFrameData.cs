using System;
using UnityEngine;

namespace FootTactic
{
    public struct FTKeyFrameData
    {
        public FTKeyFrameData(TimeSpan time, Vector3 position, int animIndex = 0, float rotation = Mathf.Infinity, int playerIndex = 0, BallEventData.BodyPartEnum bodyPart = BallEventData.BodyPartEnum.RIGHT_FOOT, BallEventData.ActionEnum action = BallEventData.ActionEnum.IMPACT_HARD)
        {
            this.Time = time;
            this.Position = position;
            this.AnimIndex = animIndex;
            this.Rotation = rotation;
            this.playerIndex = playerIndex;
            this.bodyPart = bodyPart;
            this.Action = action;
        }

        public TimeSpan Time { get; private set; }
        public Vector3 Position { get; private set; }
        public int AnimIndex { get; private set; }
        public float Rotation { get; private set; }
        public int playerIndex;
        public BallEventData.BodyPartEnum bodyPart;
        public BallEventData.ActionEnum Action;
    }
}
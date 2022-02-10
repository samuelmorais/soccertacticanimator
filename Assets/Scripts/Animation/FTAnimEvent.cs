using System;
using UnityEngine;
namespace FootTactic
{
    public class FTAnimEvent
    {
        public FTAnimEvent(
            Vector3 offset,
            float time,
            BallEventData.ActionEnum action,
            BallEventData.BodyPartEnum bodyPart,
            Vector3 direction
            )
        {
            Offset = offset;
            Action = action;
            BodyPart = bodyPart;
            Time = time;
            Direction = direction;         
        }

        public Vector3 Offset { get; private set; }
        public BallEventData.ActionEnum Action { get; private set; }
        public BallEventData.BodyPartEnum BodyPart { get; private set; }
        public float Time { get; private set; }

        public Vector3 Direction { get; private set; }

    }

}
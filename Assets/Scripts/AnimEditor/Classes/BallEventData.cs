using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootTactic {
    public class BallEventData 
    {
        public Vector3 OffsetFromBodyPart;

        public Vector3 OffsetFromRoot;
        public Vector3 Direction;
        public float ImpactForce;
        public enum BodyPartEnum { RIGHT_FOOT, LEFT_FOOT, RIGHT_HAND, LEFT_HAND, CHEST, HEAD, RIGHT_SHOULDER, LEFT_SHOULDER, RIGHT_THIGH, LEFT_THIGH };
        public BodyPartEnum BodyPart;
        public enum ActionEnum { TRAP, IMPACT_HARD, IMPACT_MEDIUM, IMPACT_WEAK };
        public ActionEnum Action;

        public float Frame;

        public BallEventData(Vector3 offsetFromBodyPart, Vector3 direction, BodyPartEnum bodyPart, ActionEnum action, float frame, float impactForce, Vector3 offsetFromRoot){
            OffsetFromBodyPart = offsetFromBodyPart;
            Direction = direction;
            BodyPart = bodyPart;
            Action = action;
            Frame = frame;
            ImpactForce = impactForce;
            OffsetFromRoot = offsetFromRoot;
        }
    }
}
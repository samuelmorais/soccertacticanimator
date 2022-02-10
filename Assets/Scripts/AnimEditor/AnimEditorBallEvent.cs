using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FootTactic{
 [CreateAssetMenu(fileName = "BallEvent", menuName = "ScriptableObjects/BallEventScriptableObject", order = 1)]    
    public class AnimEditorBallEvent : ScriptableObject{
        public Vector3 OffsetFromBodyPart;
        public Vector3 Direction;
        public BallEventData.BodyPartEnum BodyPart;        
        public BallEventData.ActionEnum Action;
        public float Time;
        public float ImpactForce;
        public Vector3 OffsetFromRoot;        
    }
}
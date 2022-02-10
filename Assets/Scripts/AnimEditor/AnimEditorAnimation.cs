using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootTactic{
    
    [CreateAssetMenu(fileName = "Animation", menuName = "ScriptableObjects/AnimationScriptableObject", order = 1)]
    public class AnimEditorAnimation: ScriptableObject {
        public string Name;
        public int Index;
        public enum AnimCategoryEnum {Kick,Pass,Trap,Header,Goalkeeper,ThrowIn,FreeKick}
        public AnimCategoryEnum Category;
        public AnimEditorBallEvent[] BallEvents;
        public Vector3 FinalPosition;
        public float FinalRotation;
        public AnimationClip AnimationClip;

    }
}
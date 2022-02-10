using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootTactic
{


    public class AnimStates : MonoBehaviour
    {
        public AnimationClip[] Animations;
        public AnimationClip Run;
        public AnimationClip Walk;
        public AnimationClip Dash;
        public AnimationClip Idle; 

        [SerializeField]
        public AnimEditorAnimation[] Anims;      

    }

}
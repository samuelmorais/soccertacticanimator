using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;
using NG.Patterns.Structure.ObserverPattern;
using Animancer.Examples.StateMachines.Brains;
using Animancer;
using System.Linq;

namespace FootTactic
{


    public class FTAnimPlayerCalcController : Controller<FTApplication>
    {
        [SerializeField]
        AnimancerComponent _Animancer;
        
        AnimStates animStates;
        bool isVisible = true;
        public bool debug;

        public Vector3 ConvertedPosition
        {
            get { return new Vector3(transform.localPosition.x, transform.localPosition.z, 0) / 0.05f; }
            set { ConvertedPosition = value; }
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {
                //Notify("CalcFinalPosition",PlayerView.transform.localPosition,PlayerView.transform.localRotation,app.model.Tactic.Animations[animationIndex].AnimationIndex, PlayerIndex, animationIndex, delay);
           
                    case "CalcFinalPosition":
                        CalculateFinalPosition((Vector3)p_data[0], (Quaternion)p_data[1], (int)p_data[2],(int)p_data[3],(float)p_data[4]);
                    break;  
            }
        }
        private void Awake()
        {
            animStates = FindObjectOfType<AnimStates>();
            

        }
        
        public void CalculateFinalPosition(Vector3 position, Quaternion localRotation, int animationIndex, int playerIndex, float delay){
            transform.position = position;
            var state = _Animancer.CurrentState;
            state = _Animancer.Play(animStates.Animations[animationIndex]);
            delay = animStates.Animations[animationIndex].length;
            state.NormalizedTime = 1;
            _Animancer.Evaluate();
            StartCoroutine(WaitRenderAnim(playerIndex, animationIndex, delay));
        }
        
        IEnumerator WaitRenderAnim(int playerIndex, int animationIndex, float delay){
            yield return new WaitForSeconds(.1f);
            Debug.Break();
            Notify("SetFinalPosition",transform.position, transform.rotation.eulerAngles.y, animationIndex, playerIndex, delay);
        }



        


    }
}

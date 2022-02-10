using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;
using NG.Patterns.Structure.ObserverPattern;
using Animancer.Examples.StateMachines.Brains;
using Animancer.Examples.Locomotion;
using Animancer;
using System.Linq;

namespace FootTactic
{


    public class FTAnimPlayerView : View<FTApplication>
    {
        FTAnimationPlayer[] ftAnimationPlayer;
        public int PlayerIndex;
        [SerializeField]
        AnimancerComponent _Animancer;
        [SerializeField]
        Transform player2D;
        [SerializeField]
        public FTPlayerStyle PlayerStyle;

        AnimStates animStates;
        bool isVisible = true;
        public bool debug;

     

        public Vector3 ConvertedPosition
        {
            get { return new Vector3(transform.localPosition.x, transform.localPosition.z, 0) / 0.05f; }
            set { ConvertedPosition = value; }
        }
        public bool PlayerHasAnimationFrames
        {
            get { return ftAnimationPlayer[app.controller.Anim.CurrentAnimation].NumFrames > 1 || ftAnimationPlayer[app.controller.Anim.CurrentAnimation].Moved; }
        }

        public bool IsBeingDragged { get; set; }

        private void Awake()
        {
            
            ftAnimationPlayer = new FTAnimationPlayer[10];
            
            for (int i = 0; i < 10; i++)
            {
                ftAnimationPlayer[i] = new FTAnimationPlayer();
                ftAnimationPlayer[i].AddKeyframe(new FTKeyFrame(transform.localPosition, TimeSpan.FromSeconds(0)));
                ftAnimationPlayer[i].SortArray();
            }

            _Animancer = GetComponent<AnimancerComponent>();
            animStates = FindObjectOfType<AnimStates>();
            if(GameObject.Find("FTPlayerBtn" + PlayerIndex) != null)
            {
                player2D = GameObject.Find("FTPlayerBtn" + PlayerIndex).transform;
            }
            
            PlayerStyle = GetComponent<FTPlayerStyle>();

        }

        public FTAnimationPlayer[] GetAnimations() {
           return ftAnimationPlayer;
        }

        public void SetAnimations(FTAnimationPlayer[] anims)
        {
           ftAnimationPlayer = anims;
        }

        


        private void Start()
        {
            try
            {
                PlayerIndex = int.Parse(gameObject.name.Replace("Player", "").Replace("Model", ""));
                ApplyDefaultAnimBehaviour();
                RenderAnim();
            }
            catch(Exception ex)
            {
                Debug.Log("Player does not have default naming.");
            }
            
        }

        public void UpdatePos(Vector3 pos)
        {
            if (GetComponentInChildren<FTBrain>() != null)
                GetComponentInChildren<FTBrain>().UpdateInput(pos);
        }

        private void Update()
        {
            if (app.controller.Anim.timer.active)
            {
                RenderAnim();
            }
        }

        public void ToggleVisibility(FTButtonPlayerView buttonView)
        {
            isVisible = !isVisible;

            bool visible = isVisible || PlayerHasAnimationFrames;
            foreach (Renderer mesh in GetComponentsInChildren<Renderer>())
            {
                mesh.enabled = visible;
            }
           
            buttonView.gameObject.SetActive(visible);
            
        }

        public void RenderAnim()
        {

            if (app.controller.selectedPlayer == null ||
                    !(app.controller.selectedPlayer.PlayerIndex == PlayerIndex &&
                   IsBeingDragged))
            {
                var frameData = ftAnimationPlayer[app.controller.Anim.CurrentAnimation].GetValue(app.controller.Anim.CurrentTime);

                setPosition(frameData);

                setAnimationStateBySpeed(frameData);

                if (frameData.defaultFrame) { ApplyDefaultAnimBehaviour(); }
            }
            

        }

        void setPosition(FTFrameData frameData)
        {
            transform.localPosition = frameData.position;
            transform.localRotation = frameData.direction;
            if(player2D == null){
                Debug.Log(gameObject.name);
                Debug.Break();
            }
            player2D.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.z, 0) / 0.05f;  
        }

        void setAnimationStateBySpeed(FTFrameData frameData)
        {
            var state = _Animancer.CurrentState;

            if (frameData.animationIndex == 0)
            {
                if (frameData.speed < 0.1f)
                {
                    state = _Animancer.Play(animStates.Idle);
                }
                else if (frameData.speed < 2f)
                {
                    state = _Animancer.Play(animStates.Walk);
                }
                else if (frameData.speed < 4f)
                {
                    state = _Animancer.Play(animStates.Run);
                }
                else
                {
                    state = _Animancer.Play(animStates.Dash);
                }
                state.NormalizedTime = ( frameData.speed/2f * frameData.timeElapsed % animStates.Animations[frameData.animationIndex].length)/animStates.Animations[frameData.animationIndex].length;
            }
            else if (frameData.animationIndex < 4 && (frameData.timeElapsed) < animStates.Animations[frameData.animationIndex].length)
            {
                state = _Animancer.Play(animStates.Animations[frameData.animationIndex]);
                state.NormalizedTime = (frameData.timeElapsed) % animStates.Animations[frameData.animationIndex].length;
            }
            else if (frameData.animationIndex >= 4 && (frameData.timeElapsed) < animStates.Anims[frameData.animationIndex - 4].AnimationClip.length)
            {
                state = _Animancer.Play(animStates.Anims[frameData.animationIndex - 4].AnimationClip);
                state.NormalizedTime = (frameData.timeElapsed) % animStates.Anims[frameData.animationIndex - 4].AnimationClip.length;
            }

            Notify("UpdateKeyFrameTime", frameData.timeElapsed);

            state.Speed = 0;
        }

        public void AddKeyFrame(FTKeyFrameData data)
        {
            ftAnimationPlayer[app.controller.Anim.CurrentAnimation].AddKeyframe(new FTKeyFrame(data.Position, data.Time, false, data.AnimIndex, data.Rotation));
            ftAnimationPlayer[app.controller.Anim.CurrentAnimation].SortArray();
        }

        public void RemoveKeyFrame(TimeSpan timeSpan)
        {
            ftAnimationPlayer[app.controller.Anim.CurrentAnimation].RemoveKeyframe(timeSpan);
            ftAnimationPlayer[app.controller.Anim.CurrentAnimation].SortArray();
        }

        public void ApplyDefaultAnimBehaviour()
        {            
            transform.LookAt(new Vector3(app.controller.Ball.transform.position.x, transform.position.y, app.controller.Ball.transform.position.z));
        }

        public float LastKeyFrameTime(bool skipCurrentFrame = true)
        {
            return (float)ftAnimationPlayer[app.controller.Anim.CurrentAnimation].LastKeyFrame(app.controller.Anim.CurrentTime, skipCurrentFrame).time.TotalSeconds;
        }

        public Vector3 LastKeyFramePosition(bool skipCurrentFrame = true)
        {
            return ftAnimationPlayer[app.controller.Anim.CurrentAnimation].LastKeyFrame(app.controller.Anim.CurrentTime, skipCurrentFrame).vec;
        }

        public FTKeyFrame LastAnimIndex(bool skipCurrentFrame = true)
        {
            return ftAnimationPlayer[app.controller.Anim.CurrentAnimation].LastKeyFrame(app.controller.Anim.CurrentTime, skipCurrentFrame);
        }

        public Vector3 FinalPosition(FTFrameData frameData){
            var state = _Animancer.CurrentState;
            state = _Animancer.Play(animStates.Animations[frameData.animationIndex]);
            state.NormalizedTime = 1;
            _Animancer.Evaluate();
            return transform.position;
        }


        


    }
}

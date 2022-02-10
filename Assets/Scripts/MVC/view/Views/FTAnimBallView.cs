using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;
using NG.Patterns.Structure.ObserverPattern;
using Animancer.Examples.StateMachines.Brains;
//using Animancer.Examples.Locomotion;
using Animancer;
using System.Linq;

namespace FootTactic
{


    public class FTAnimBallView : View<FTApplication>
    {
        FTAnimationBall[] ftAnimationBall;
        public Transform ball2D;
        public Transform root3D;
        public RectTransform root2D;
        public Vector3 ConvertedPosition
        {
            get { return new Vector3(transform.localPosition.x, transform.localPosition.z, 0) / 0.05f; }
            set { ConvertedPosition = value; }
        }
        public bool BallHasAnimationFrames
        {
            get { return ftAnimationBall[app.controller.Anim.CurrentAnimation].NumFrames > 1; }
        }
        private void Awake()
        {            
            ftAnimationBall = new FTAnimationBall[10];            
            for (int i = 0; i < 10; i++)
            {
                ftAnimationBall[i] = new FTAnimationBall();         
            }
        }

        private void Update()
        {
            if (app.controller.Anim.timer.active)
            {
                UpdatePosition();
            }
        }

        public FTAnimationBall[] GetAnimations()
        {
            return ftAnimationBall;
        }


        public void SetAnimations(FTAnimationBall[] animationBall)
        {
            ftAnimationBall = animationBall;
        }


        public void UpdatePosition()
        {
            try
            {
                FTBallFrameData frameData = ftAnimationBall[app.controller.Anim.CurrentAnimation].GetValue(app.controller.Anim.CurrentTime);

                if (frameData.playerIndex > 0 && frameData.Action == BallEventData.ActionEnum.TRAP)
                {
                    transform.parent = FTController.Players[frameData.playerIndex].transform;
                    transform.localPosition = FTConstants.RIGHT_FOOT_GROUND_OFFSET;
                    ball2D.GetComponent<RectTransform>().SetParent(FTController.PlayerControllers[frameData.playerIndex].PlayerButton.GetComponent<RectTransform>());
                    ball2D.localPosition = Vector3.zero;
                }
                else
                {
                    transform.parent = root3D;
                    transform.localPosition = frameData.position;
                    ball2D.GetComponent<RectTransform>().SetParent(root2D);

                    ball2D.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

                }
            }
            catch(Exception ex)
            {
                transform.localPosition = Vector3.zero;
            }
          
           
               

        }

        public void AddKeyFrame(FTKeyFrameData data)
        {
            ftAnimationBall[app.controller.Anim.CurrentAnimation].AddKeyframe(new FTKeyFrameBall(FTUtil.SafeBallPosition(data.Position), data.Time, data.bodyPart, data.Action, data.playerIndex));
            ftAnimationBall[app.controller.Anim.CurrentAnimation].SortArray();
        }

        public void RemoveKeyFrame(TimeSpan timeSpan)
        {
            ftAnimationBall[app.controller.Anim.CurrentAnimation].RemoveKeyframe(timeSpan);
            ftAnimationBall[app.controller.Anim.CurrentAnimation].SortArray();
        }

        public void RemoveLastKeyFrame()
        {
            ftAnimationBall[app.controller.Anim.CurrentAnimation].RemoveLastKeyFrame();
        }

        public bool IsBallNearPlayer(Vector3 playerPosition)
        {
            return !BallHasAnimationFrames || Vector3.Distance(transform.position, playerPosition) < FTConstants.BALL_DISTANCE_TO_ACTIVATE_ACTIONS;
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;

namespace FootTactic
{
    
	public class FTAnimationController : Controller<FTApplication>
	{
        float startTime = 0;

        TimeSpan currentTime;

        float animationSize = FTConstants.ANIM_SIZE;
        
        public float AnimationSize { get => animationSize; set => animationSize = value; }

        public bool IsPlaying { get; set; }
        public TimeSpan CurrentTime { get { return TimeSpan.FromSeconds(timer.elapsed); } set {
                currentTime = value;
                timer.elapsed = (float)value.TotalSeconds;
                RenderAnims();
            } }

        int _currentAnim;

        public int CurrentAnimation { get { return _currentAnim; } set { _currentAnim = value; } }


        private float nextUpdate = 1;

        public FTAnimPlayerView[] playersAnims;
        public FTAnimationView animationView;
        public FTAnimBallView ballView;
        public TimerView timer;

        private void Start()
        {
            playersAnims = new FTAnimPlayerView[22];

            var i = 0;

            for(var t = 1; t <=2; t++)
            {
                for(var p = 1; p <= 11; p++)
                {
                    playersAnims[i] = GameObject.Find("Player" + ((t*100)+p) + "Model").GetComponent<FTAnimPlayerView>();
                    i++;

                }
            }
        }

        void RenderAnims()
        {
            UpdateAnimationCurrentTime();
        }

        private void Update()
        {
            if (timer.active)
            {
                UpdateAnimationCurrentTime();
            }
            
        }

        public void UpdateAnimationCurrentTime()
        {
            float time = timer.elapsed;

            currentTime = TimeSpan.FromSeconds(time);

            Notify("RenderAnim");

            animationView.UpdateTimeline(time);
        }

        IEnumerator PlayAnimsWithTimeout(float timeout)
        {
            if (!timer.active)
            {
                timer.Play();
                yield return new WaitForSeconds(timeout);
                timer.Pause();
            }
        }

        public void PlayWithTimeout(float timeout)
        {
            StartCoroutine(PlayAnimsWithTimeout(timeout));
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {
                
                case "StopAnim":                    
                    RenderAnims();
                    break;

                case "PrevFrame@up":
                    animationView.SetInputKeyframeColor(true);
                    break;

                case "NextFrame@up":
                    animationView.SetInputKeyframeColor(true);
                    break;

                case "PauseAnim":
                    timer.Pause();
                    break;

                case "ChangeAnim@up":
                    OnChangedAnim((int)p_data[0]);
                    break;

                case "ButtonPlayerDrag@drag":
                    
                    break;

                case "PlayerButton@up":
                    OnPlayerSelected((int)p_data[0]);
                    break;
                case "PlayersCreated":
                    OnPlayersCreated();
                    break;

                


            }
        }

        void OnChangedAnim(int newAnimIndex, bool force = false)
        {
            if (app.controller.Anim.CurrentAnimation != newAnimIndex || force)
            {
                animationView.StopAnimation();
                app.controller.Anim.CurrentAnimation = newAnimIndex;
                RenderAnims();
                animationView.UpdateKeyFrames();
                animationView.UpdateButtonsAnimsColors();                
            }

        }

        void OnNextFrame()
        {
            animationView.nextKeyFrameOfSelectedPlayer();
        }
        void OnPrevFrame()
        {
            animationView.prevKeyFrameOfSelectedPlayer();
        }

        void OnPlayerSelected(int playerSelected)
        {
            
            animationView.UpdateKeyFrames();
            
        }

        void OnPlayersCreated()
        {
            animationView.UpdateButtonsAnimsColors();
        }

        public void ResetState()
        {
            OnChangedAnim(0, true);
        }
		
    }
}

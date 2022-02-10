using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using UnityEngine.UI;
using System;
namespace FootTactic
{
	public class FTAnimationView : View<FTApplication>
    {
        
        public Slider sliderTimeline;
        public InputField inputFieldFrame;
        public InputField inputFieldSize;
        public Button playButton;
        public Button stopButton;
        public Button nextFrameButton;
        public Button previousFrameButton;
        bool justUpdatedTimeline = false;
        public Sprite spritePlay, spritePause;
        bool isPlaying = false;
        public TimerView timer;
        public GameObject imgKeyFrame;
        int lastSelectedPlayer = 0;
        public Toggle toggleAutoAdvanceTimeline;

        public Color colorButtonWithAnims;
        public Color colorButtonEmpty;
        public Color colorButtonSelected;
        public Color colorInputWithKeyFrame;
        public Color colorInputWithoutKeyFrame;

        private void Start()
        {
            UpdateInputText();            
        }


        public void OnChangeTimeline()
        {
            if (!justUpdatedTimeline)
            {
                float time = sliderTimeline.value;
                
                app.controller.Anim.CurrentTime = TimeSpan.FromSeconds(time);                
            }
            else
            {
                justUpdatedTimeline = false;
            }

            UpdateInputText();


        }

        public void UpdateTimeline(float time)
        {
            justUpdatedTimeline = true;
            sliderTimeline.value = time;
        }

        public void PlayAnimation()
        {
            if (!timer.active)
            {                
                Notify("PlayAnim", this, "");
                timer.Play();
                SetPlayButtonImage(false);
                SetInputKeyframeColor(false);
            }
            else
            {
                PauseAnimation();
            }

        }
                    

        public void StopAnimation()
        {
            timer.Stop();
            UpdateTimeline(0);
            Notify("StopAnim", this, "");
            SetPlayButtonImage(true);
        }

        public void PauseAnimation()
        {
            if (timer.active)
            {
                
                Notify("PauseAnim");
                timer.Pause();
                SetPlayButtonImage(true);
            }
            else
            {
                PlayAnimation();
            }
            
        }

        public void NextFrame()
        {
            Notify("NextFrame");
            nextKeyFrameOfSelectedPlayer();
        }

        public void PreviousFrame()
        {
            Notify("PreviouFrame");
            prevKeyFrameOfSelectedPlayer();
        }

        void SetPlayButtonImage(bool play)
        {
            if(play)
                playButton.transform.Find("Image").GetComponent<Image>().sprite = spritePlay;
            else
                playButton.transform.Find("Image").GetComponent<Image>().sprite = spritePause;

        }

        public void SetInputKeyframeColor(bool isKeyFrame)
        {
            inputFieldFrame.GetComponent<Image>().color = isKeyFrame ? colorInputWithKeyFrame : colorInputWithoutKeyFrame;            
        }

        void UpdateInputText()
        {
            inputFieldFrame.text = sliderTimeline.value.ToString("####0.00");
        }

        public void OnChangedTextTimeline()
        {
            if(inputFieldFrame.text != "")
            {
                sliderTimeline.value = float.Parse(inputFieldFrame.text);
            }
        }

        public void nextKeyFrameOfSelectedPlayer()
        {
            if (lastSelectedPlayer > 0)
            {
                FTAnimationPlayer[] anims = FTController.Players[lastSelectedPlayer].GetAnimations();
                timer.Pause();
                var time = anims[app.controller.Anim.CurrentAnimation].NextKeyFrame(app.controller.Anim.CurrentTime).time;
                app.controller.Anim.CurrentTime = time;
                UpdateTimeline((float)time.TotalSeconds);
            }
        }
        public void prevKeyFrameOfSelectedPlayer()
        {
            if (lastSelectedPlayer > 0)
            {
                FTAnimationPlayer[] anims = FTController.Players[lastSelectedPlayer].GetAnimations();
                timer.Pause();
                var time = anims[app.controller.Anim.CurrentAnimation].LastKeyFrame(app.controller.Anim.CurrentTime).time;
                app.controller.Anim.CurrentTime = time;
                UpdateTimeline((float)time.TotalSeconds);
            }
        }

        public void UpdateSliderTimeline()
        {
            UpdateTimeline((float)app.controller.Anim.CurrentTime.TotalSeconds);
        }
       
        public void UpdateKeyFrames()
        {
            Debug.Log("UpdateKeyFrames");
            FTUtil.DestroyGameObjectsWithTag("keyframeUI");

            if (app.controller.selectedPlayer != null)
            {
                lastSelectedPlayer = app.controller.selectedPlayer.PlayerIndex;
            }

            if(lastSelectedPlayer > 0) {
                FTAnimationPlayer[] anims = FTController.Players[lastSelectedPlayer].GetAnimations();
                var numFrames = anims[app.controller.Anim.CurrentAnimation].NumFrames;
                for (var i = 0; i <  numFrames; i++)
                {
                    var kf = anims[app.controller.Anim.CurrentAnimation].KeyFrames[i];
                    if (kf != null) { 
                        var imgKeyFrameInstance = Instantiate(imgKeyFrame);
                        imgKeyFrameInstance.tag = "keyframeUI";
                        imgKeyFrameInstance.GetComponent<RectTransform>().SetParent(sliderTimeline.GetComponent<RectTransform>());
                        imgKeyFrameInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                            calculateKeyFramePos(kf.time),
                            0                          
                            );
                    }
                }
            }
        }

        public bool IsAutoAdvanceEnabledAtTimeZero()
        {
            return toggleAutoAdvanceTimeline.isOn;
        }

        public void UpdateButtonsAnimsColors()
        {
            foreach(FTButtonAnimView button in FindObjectsOfType<FTButtonAnimView>())
            {
                bool hasAnims = false;
                var numPlayers = FTController.Players.Count;
                foreach (var animPlayer in FTController.Players.Keys)
                {
                   hasAnims = hasAnims || FTController.Players[animPlayer].GetAnimations()[button.index].NumFrames > 1;
                }
                hasAnims = hasAnims || app.controller.Ball.GetAnimations()[button.index].NumFrames > 1;

                button.GetComponent<Image>().color = app.controller.Anim.CurrentAnimation == button.index ? colorButtonSelected : hasAnims? colorButtonWithAnims : colorButtonEmpty;

            }
        }



        float calculateKeyFramePos(TimeSpan time)
        {
            return (float)time.TotalSeconds * 13.33f - 100f;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;
using UnityEngine.EventSystems;
//using NG.Patterns.Structure.ObserverPattern;

namespace FootTactic
{
    

    public class FTAnimPlayerController : Controller<FTApplication>
    {

        [SerializeField]
        public int PlayerIndex;
        [HideInInspector]
        public FTAnimPlayerView PlayerView;
        public FTButtonPlayerView PlayerButton;
        public void Awake()
        {

        }

        public Vector3 ConvertedPosition{
            get;// { return 0.05f * new Vector3(transform.localPosition.x, 0, transform.localPosition.y);  }
            set;// { ConvertedPosition = value;  }
        }

		// Use this for initialization
		void Start()
		{
            PlayerView = FTController.Players != null && FTController.Players[PlayerIndex] != null ? FTController.Players[PlayerIndex] : FTUtil.GetPlayerByIndex(PlayerIndex);
            PlayerButton = FTUtil.GetPlayerButtonByIndex(PlayerIndex);
        }

        FTKeyFrameData keyframeData;
        public void UpdatePos(Vector3 newPos)
        {
            ConvertedPosition = newPos;
            Debug.Log(newPos + " newPos");
            app.controller.selectedPlayer = PlayerView;

            if (!app.controller.Anim.timer.active)
            {
                PlayerView.UpdatePos(ConvertedPosition);
            }

        }


        public void SaveKeyFrameAtPosition(Vector3 newPos)
        {
            keyframeData = new FTKeyFrameData(app.controller.Anim.CurrentTime, ConvertedPosition, 0, Mathf.Infinity);
            PlayerView.AddKeyFrame(keyframeData);            
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {
                case "RenderAnim":
                    PlayerView.RenderAnim();
                    break;

                case "PlayAnim":
                    
                    break;

                case "StopAnim":
                    
                    break;

                case "PauseAnim":                    
                    break;

                
                
                case "ButtonPlayerDrag@drag":
                    if(PlayerDraggedIsThisPlayer((string)p_data[1]))
                    {
                        UpdateCurrentTimeBasedOnNewPosition((Vector3)p_data[0]);                        
                        UpdatePos((Vector3)p_data[0]);
                        app.controller.Ball.UpdatePosition();                           
                        PlayerButton.CheckValidPosition(true);
                        Notify("ValidPositionUpdate", (Vector3)p_data[0]);
                        
                    }
                    break;


                case "ButtonPlayerDrag@drag-starts":
                    if (PlayerDraggedIsThisPlayer((string)p_data[1]))
                    {
                        lastKeyFrameTime = PlayerView.LastKeyFrameTime(!app.controller.Anim.animationView.IsAutoAdvanceEnabledAtTimeZero());
                        originalDragTime = (float)app.controller.Anim.CurrentTime.TotalSeconds;
                        lastFramePosition = PlayerView.LastKeyFramePosition(!app.controller.Anim.animationView.IsAutoAdvanceEnabledAtTimeZero());
                        
                        Notify("ValidPositionStart", lastFramePosition);
                        PlayerView.IsBeingDragged = true;
                    }
                    originalPosition = PlayerView.transform.localPosition;
                    break;

                case "PlayerButton@down":
                    if (PlayerIndex == ((int)p_data[0]))
                    {
                        
                        originalDragTime = (float)app.controller.Anim.CurrentTime.TotalSeconds;
                        lastFramePosition = PlayerView.LastKeyFramePosition(!app.controller.Anim.animationView.IsAutoAdvanceEnabledAtTimeZero());
                        originalPosition = PlayerView.transform.localPosition;
                    }
                    break;

                case "PlayerButton@up":
                    OnPlayerClickedUp((int)p_data[0], (Vector3)p_data[1]);
                    break;

                case "ToggleVisibility@click":
                    PlayerView.ToggleVisibility(PlayerButton);                    
                    break;

                case "ApplyStyleOfTeam":
                    if(FTUtil.IsPlayerOfThisTeam(PlayerIndex,(int)p_data[1]) )
                    {
                        PlayerView.PlayerStyle.ApplyStyleOfTeam((EditTeamView)p_data[0]);
                        PlayerButton.SetColorsOfTeamOnButton((EditTeamView)p_data[0]);
                    }                   
                    break;


            }
        }

        void OnPlayerClickedUp(int playerIndex, Vector3 newPos)
        {
            UpdatePlayerView(playerIndex);

            if (PlayerIndex == playerIndex)
            {
                PlayerView.IsBeingDragged = false;

                if (IsMinimumDistanceMoved(newPos))
                {
                    UpdateCurrentTimeBasedOnNewPosition(newPos);

                    UpdatePlayerAnimationData(newPos);
                }

                UpdateAnimationView();

            }
        }

        bool IsMinimumDistanceMoved(Vector3 newPos)
        {
            return Vector3.Distance(newPos, originalPosition) > FTConstants.MIN_MOVEMENT_PLAYER;
        }

        void UpdatePlayerAnimationData(Vector3 newPos)
        {
            PlayerButton.CheckValidPosition(true);
            app.controller.Ball.UpdatePosition();
            UpdatePos(newPos);
            SaveKeyFrameAtPosition(newPos);
        }

        void UpdateAnimationView()
        {
            app.controller.Anim.animationView.UpdateKeyFrames();
            app.controller.Anim.UpdateAnimationCurrentTime();
            Notify("DisableLine");
        }

        void UpdateCurrentTimeBasedOnNewPosition(Vector3 newPos)
        {
            float finalTime = getFinalTimeBasedOnNewPosition(newPos, app.controller.Anim.animationView.IsAutoAdvanceEnabledAtTimeZero());
            app.controller.Anim.CurrentTime = TimeSpan.FromSeconds(finalTime);
        }

        bool PlayerDraggedIsThisPlayer(string playerDraggedName) {
           return FTUtil.TransformButtonName(playerDraggedName) == gameObject.name;
        }

        Vector3 lastFramePosition = Vector3.down;      
        float lastKeyFrameTime;
        float originalDragTime;
        Vector3 originalPosition;

        float getFinalTimeBasedOnNewPosition(Vector3 newPosition, bool autoAdvanceInTimeZero = false)
        {
            if (lastFramePosition == Vector3.down)
            {
                
                return originalDragTime;
            }
            float distance = Vector3.Distance(newPosition, lastFramePosition);
            float timeFromLastKeyFrame = originalDragTime - lastKeyFrameTime;
            if (timeFromLastKeyFrame.Equals(0) && !autoAdvanceInTimeZero) {
                
                return 0;
            }
            float speed = 0;

            if (timeFromLastKeyFrame > 0)
            {
                speed = distance / timeFromLastKeyFrame;
            }

            bool speedIsValid = speed < FTConstants.MAX_SPEED_ALLOWED;

            bool shouldStayAtInitialTime = false;
            if (!autoAdvanceInTimeZero)
            {
                if (originalDragTime.Equals(0))
                {
                    shouldStayAtInitialTime = true;
                }
                else if (speedIsValid)
                {
                    shouldStayAtInitialTime = true;
                }
                
            }

            if (speedIsValid && shouldStayAtInitialTime)
            {
              
                return originalDragTime;
            }
            else
            {
             
                return lastKeyFrameTime + distance /FTConstants.MAX_SPEED_ALLOWED;
            }           
        }

        void UpdatePlayerView(int playerIndex)
        {
            if (PlayerIndex == playerIndex)
            {
                PlayerButton.OnSelected();
            }
            else
            {
                PlayerButton.OnUnSelected();
                PlayerView.IsBeingDragged = false;
            }
        }

        public void RemovePreviousAnimationIfThereIsAnyInconsistency(int animationIndex){
            var lastAnim = PlayerView.LastAnimIndex(true);        
            var inconsistent = FTRules.IsAnimationsInconsistent(animationIndex, lastAnim.animIndex);
            if(inconsistent){            
                for(var i = 0; i < 2; i++){
                    if(i > 0){
                        lastAnim = PlayerView.LastAnimIndex(true);
                    }
                    PlayerView.RemoveKeyFrame(lastAnim.time);
                    app.model.Tactic.Animations[lastAnim.animIndex].Events.ForEach(
                        e => {
                            Debug.Log($"e.Offset: {e.Offset}");
                            app.controller.Anim.ballView.RemoveLastKeyFrame(); 
                        }
                    );
                }
            }            
        }

        public void AddAnimationCurrentFrame(int animationIndex, float rotation,  float delay = 0,  BallEventData.ActionEnum eventType = BallEventData.ActionEnum.IMPACT_HARD, BallEventData.BodyPartEnum bodyPart = BallEventData.BodyPartEnum.RIGHT_FOOT)
        {
            
             PlayerView.AddKeyFrame(new FTKeyFrameData(
                app.controller.Anim.CurrentTime.Add(TimeSpan.FromSeconds(delay)),
                PlayerView.transform.localPosition,
                app.model.Tactic.Animations[animationIndex].AnimationIndex,
                rotation)
            );

            PlayerView.transform.localRotation = Quaternion.Euler(new Vector3(0, rotation, 0));

            app.model.Tactic.Animations[animationIndex].Events.ForEach(
                e => {
                    Debug.Log($"e.Offset: {e.Offset}");
                    app.controller.Anim.ballView.AddKeyFrame(
                    new FTKeyFrameData(
                        app.controller.Anim.CurrentTime.Add(TimeSpan.FromSeconds(delay + e.Time)),
                        PlayerView.transform.TransformPoint(e.Offset),
                        0,
                        0,
                        PlayerView.PlayerIndex,
                        bodyPart,
                        eventType
                        )
                    ); 
                }
            );

            if(animationIndex == FTConstants.ANIM_KICK_INDEX)
            {
                app.controller.Anim.ballView.AddKeyFrame(
                    new FTKeyFrameData(
                        app.controller.Anim.CurrentTime.Add(TimeSpan.FromSeconds(FTConstants.TIME_KICK)),
                        FTUtil.ConvertPosition(FTTargetController.TargetPosition),
                        0,
                        0
                    ));
            }
            var finalPos = app.model.Tactic.Animations[animationIndex].FinalPosition;
            var finalPosition = PlayerView.transform.localPosition + PlayerView.transform.rotation * finalPos;
            
            FTKeyFrameData keyframeData2 = new FTKeyFrameData(
                app.controller.Anim.CurrentTime.Add(TimeSpan.FromSeconds(delay + app.model.Tactic.Animations[animationIndex].Duration)),
                finalPosition,
                0,
                rotation);

            PlayerView.AddKeyFrame(keyframeData2);

        }


    }
}

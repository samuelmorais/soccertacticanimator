using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using System.Linq;
using UnityEditor;

namespace FootTactic{
    public class AnimEditorController : Controller<AnimEditorApplication>
    {
        AnimEditorAnimation currentAnim;
        int currentBallEvent = 0;
        [SerializeField]
        Animator AnimEditorPlayerAnimController;

        [SerializeField]
        GameObject goPlayer;
        
        float currentClipLength = 0;
        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data){
            switch (p_event) {
                case  "SaveBallEvent":
                    OnSaveBallEvent((BallEventData)p_data[0]);
                break;
                case "CancelBallEvent":
                break;
                case "DeleteBallEvent":
                break;
                case "SaveCurrentAnim":
                    OnSaveCurrentAnim();
                break;
                case "OnChangeAnimEditorSelectAnim":
                    OnChangeAnimEditorSelectAnim((int)p_data[0]);
                break;
                case "SelectBallEvent":
                    currentBallEvent = (int)p_data[0];
                break;
                case "OnSliderAnimEditorChange":
                    OnSliderAnimEditorChange((float)p_data[0]);
                break;
            }
        }

        void OnChangeAnimEditorSelectAnim(int value){            
            ApplyCurrentAnimToEditorPlayer(value);
            RenderBallEvents();             
        }

        void ApplyCurrentAnimToEditorPlayer(int value){
            currentAnim = app.model.Animations[value - 1];
            AnimatorOverrideController aoc = new AnimatorOverrideController(AnimEditorPlayerAnimController.runtimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips){
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, currentAnim.AnimationClip));
                currentClipLength = currentAnim.AnimationClip.length;
            }
            aoc.ApplyOverrides(anims);
            AnimEditorPlayerAnimController.runtimeAnimatorController = aoc;
        }

        void RenderBallEvents(){
            var eventIndex = 0;
            currentAnim.BallEvents.ToList().ForEach(ballEvent => {
                var ballData = new BallEventData(ballEvent.OffsetFromBodyPart,
                    ballEvent.Direction,
                    ballEvent.BodyPart,
                    ballEvent.Action,
                    ballEvent.Time,
                    ballEvent.ImpactForce,
                    ballEvent.OffsetFromRoot);
                app.view.RenderBallEvent(ballData, eventIndex);
                eventIndex++;
            });
        }

        void OnSaveBallEvent(BallEventData data){
            if(currentAnim.BallEvents.Length <= currentBallEvent) return;
            currentAnim.BallEvents[currentBallEvent].Action = data.Action;
            currentAnim.BallEvents[currentBallEvent].BodyPart = data.BodyPart;
            currentAnim.BallEvents[currentBallEvent].OffsetFromBodyPart = data.OffsetFromBodyPart;
            currentAnim.BallEvents[currentBallEvent].Direction = data.Direction;            
            currentAnim.BallEvents[currentBallEvent].Time = currentClipLength * data.Frame;
            currentAnim.BallEvents[currentBallEvent].ImpactForce = data.ImpactForce;     
            currentAnim.BallEvents[currentBallEvent].OffsetFromRoot = data.OffsetFromRoot;   
            EditorUtility.SetDirty(currentAnim.BallEvents[currentBallEvent]);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();        
        }
        
        void OnSliderAnimEditorChange(float value){
            AnimEditorPlayerAnimController.Play("Default", 0, value);
            if(value >= 0.99f){
                var hips = goPlayer.transform.Find("Root/Hips");
                var pos = hips.position;
                pos = new Vector3(pos.x,0,pos.z);
                currentAnim.FinalPosition = pos;
                currentAnim.FinalRotation = hips.rotation.eulerAngles.y;
            }
        }

        void OnSaveCurrentAnim(){
            
        }

        public float GetCurrentClipLength(){
            return currentClipLength;
        }

    }
}

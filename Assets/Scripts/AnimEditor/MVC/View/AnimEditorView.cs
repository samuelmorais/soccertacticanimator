using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using TMPro;
using UnityEngine.UI;

namespace FootTactic{
    public class AnimEditorView : View<AnimEditorApplication>
    {
        [SerializeField]
        TMP_InputField x0;
        [SerializeField]
        TMP_InputField y0;

        [SerializeField]
        TMP_InputField z0;

        [SerializeField]
        TMP_InputField x1;
        [SerializeField]
        TMP_InputField y1;
        
        [SerializeField]
        TMP_InputField z1;

        [SerializeField]
        TMP_Dropdown ddlAction;
        [SerializeField]
        TMP_Dropdown ddlPartBody;

        [SerializeField]
        GameObject panelBallEvent;

        [SerializeField]
        Slider sliderAnimEditor;
        [SerializeField]
        TMP_Dropdown ddlCurrentAnimation;

        [SerializeField]
        Transform[] ballEventTransform;

        [SerializeField]
        Transform[] BodyPartsByIndex;

        [SerializeField]
        Transform rootTransform;

        int selectedBallEvent = 0;

        [SerializeField]
        TextMeshProUGUI BallEventIndexLabel, FrameLabel;

        public GameObject BallPrefab;

        public void SaveBallEvent(){
            BallEventData ballEventData = new BallEventData(
                getOffsetFromBodyPart(),
                calculateDirection(),
                getBodyPartFromInput(),
                getActionFromInput(),
                getFrameFromInput(),
                getImpactForce(),
                getOffsetFromRoot()
            );
            Notify("SaveBallEvent", ballEventData);
        }

        public void RenderBallEvent(BallEventData ballEventData, int i){
            ballEventTransform[i].localPosition = ballEventData.OffsetFromRoot;
            ballEventTransform[i].localRotation = Quaternion.Euler(ballEventData.Direction); 
            GameObject.Find("ballTest").transform.localPosition =      ballEventData.OffsetFromRoot;  
            GameObject.Find("ballTest").transform.localRotation =      Quaternion.Euler(ballEventData.Direction);          
        }

        
        public void DestroyBallEventsObjects(){
            GameObject[] ballEvents = GameObject. FindGameObjectsWithTag("BallEvent");
            for(int i=0; i< ballEvents. Length; i++)
            {
                Destroy(ballEvents[i]);
            }
        }

        public void CancelBallEvent(){
            Notify("CancelBallEvent");
        }

        public void DeleteBallEvent(){
            Notify("DeleteBallEvent");
        }

        public void SaveCurrentAnim(){
            Notify("SaveCurrentAnim");
        }
        
        public void OnChangeAnimEditorSelectAnim(){
            Notify("OnChangeAnimEditorSelectAnim", ddlCurrentAnimation.value);
        }

        public void SelectBallEvent(int index){
            selectedBallEvent = index;
            BallEventIndexLabel.text = (index + 1).ToString();
            Notify("SelectBallEvent", index);
        }

        public void OnSliderAnimEditorChange(){
            UpdateFrameLabel(sliderAnimEditor.value);
            Notify("OnSliderAnimEditorChange", sliderAnimEditor.value);
        }

        void UpdateFrameLabel(float value){
            var frameNumber = value * app.controller.GetCurrentClipLength() * 30;
            FrameLabel.text = frameNumber.ToString();
        }

        Vector3 getOffsetFromBodyPart(){
            var bodyPartTransform = BodyPartsByIndex[ddlPartBody.value];
            return ballEventTransform[selectedBallEvent].position - bodyPartTransform.position;
        }

        Vector3 calculateDirection(){
            var localEuler = ballEventTransform[selectedBallEvent].localRotation.eulerAngles;
            return localEuler;
        }

        Vector3 getOffsetFromRoot(){
            return ballEventTransform[selectedBallEvent].position - rootTransform.position;
        }

        float getImpactForce(){
            return ballEventTransform[selectedBallEvent].localScale.x;
        }
        BallEventData.BodyPartEnum getBodyPartFromInput(){
            return (BallEventData.BodyPartEnum)ddlPartBody.value;
        }
        BallEventData.ActionEnum getActionFromInput(){
            return (BallEventData.ActionEnum)ddlAction.value;
        }

        float getFrameFromInput(){
            return sliderAnimEditor.value;
        }

        public void SetSelectedBallEvent(int index){
            selectedBallEvent = index;
        }
    }
}
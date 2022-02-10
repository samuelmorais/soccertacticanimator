using UnityEngine;
using System.Collections.Generic;
using thelab.mvc;
using System;
using FootTactic;
using Newtonsoft.Json;
using System.Linq;

namespace FootTactic
{
	/// <summary>
	/// Class that handles the application data.
	/// </summary>
	public class FTModel : Model<FTApplication>
	{
        Action<string> OnSuccessInitialized;
        Action<string> OnErrorInitialized;
		Tactic tactic;  
        
        [SerializeField]
        AnimEditorAnimation[] Animations;      
		
		public Tactic Tactic
		{
			get
			{
				return tactic;
			}

            set
            {
                tactic = value;
            }
            
		}

		public void InitializeModel(
                        			string filePath,                        			
			                        Action<string> onInitialized,
			                        Action<string> onErrorOccurred
		                            ){
            OnSuccessInitialized = onInitialized;
            OnErrorInitialized = onErrorOccurred;

            StartCoroutine(DatabaseService.OpenTactic(
				filePath,
                OnSuccessInitialize,
                OnErrorInitialize));

        }

        void OnSuccessInitialize(string json)
        {
            OnSuccessInitialized(json);
            tactic = JsonUtility.FromJson<Tactic>(json);
        }

        void OnErrorInitialize(string errorMessage)
        {
            OnErrorInitialized(errorMessage);
        }


        public void SaveModel(
                                    string filePath,                                    
                                    Action<string> OnSave,
                                    Action<string> OnErrorSave
                                    )
        {
            string contents = JsonUtility.ToJson(Tactic);
            DatabaseService.SaveTactic(
                filePath,
                contents,
                OnSave,
                OnErrorSave);
            
        }

        public void CreateModel(
                                    string filePath,
                                    Action<string> OnSave,
                                    Action<string> OnErrorSave
                                    )
        {
            string contents = JsonUtility.ToJson(Tactic);
            DatabaseService.SaveTactic(
                filePath,
                contents,
                OnSave,
                OnErrorSave);

        }

        private void Start()
        {
            Tactic = new Tactic(FindObjectOfType<AnimStates>());
            OpenAnimationAssets(OnAnimAssetsLoaded);
        }

        public void OpenFile(string fileName)
        {
           StartCoroutine(DatabaseService.OpenTactic(fileName, OnSuccessOpen, OnErrorOpen));
        }

        public void SaveFile(string fileName)
        {
            Tactic.UpdateAnimations(app.controller.Ball);
            
            string contents = JsonConvert.SerializeObject(Tactic);
           
            DatabaseService.SaveTactic(fileName, contents, OnSuccessSave, OnErrorSave);
        }

        void OnSuccessSave(string returnSave)
        {
            Debug.Log("[FT] File saved to " + returnSave);
        }

        void OnErrorSave(string message)
        {
            Debug.LogError("[FT] Error saving: " + message);
        }

        void OnSuccessOpen(string contents)
        {
            contents = contents.Replace("\"Infinity\"", FTConstants.UNSET_ROTATION_STR);//HACK

            var tacticJson = QuickType.TacticJson.FromJson(contents);

            tactic = new Tactic(tacticJson, FindObjectOfType<AnimStates>());

            tactic.LoadAnimations(app.controller.Ball);

            tactic.LoadTeams();

            Notify("OpennedFile");
        }

        void OnErrorOpen(string errorMessage)
        {
            Debug.LogError("[FT] Error openning: " + errorMessage);
        }


        public void OpenAnimationAssets(Action<List<FTAnimationData>> callBack){
            var listAnims = new List<FTAnimationData>();
            Animations.ToList().ForEach(anim => {
                listAnims.Add(ImportAnimation(anim));
            });
            callBack(listAnims);
        }

        FTAnimationData ImportAnimation(AnimEditorAnimation anim){
            var animEventsPass = new List<FTAnimEvent>();
            anim.BallEvents.ToList().ForEach(ballEvent => {
                animEventsPass.Add(
                    new FTAnimEvent(
                        ballEvent.OffsetFromRoot,
                        ballEvent.Time,
                        ballEvent.Action,
                        ballEvent.BodyPart,
                        ballEvent.Direction
                    ));
            });
            FTAnimationData importedAnim = new FTAnimationData(anim.Name, anim.Index, anim.AnimationClip.length, animEventsPass, anim.FinalPosition, anim.FinalRotation);
            return importedAnim;
        }

        void OnAnimAssetsLoaded(List<FTAnimationData> listAnims){
            listAnims.ForEach(anim => {
                Tactic.Animations.Add(anim);
            });
        }



    }
}
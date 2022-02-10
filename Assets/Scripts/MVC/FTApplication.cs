using UnityEngine;
using System.Collections;
using thelab.mvc;
using System;

namespace FootTactic
{
	public class FTApplication : BaseApplication<FTModel, FTView, FTController>
	{
	    public void OpenTactic(string filePath)
        {
            model.InitializeModel(filePath,OnSuccessOpen,OnErrorOpen);
        }

        void OnSuccessOpen(string json)
        {
            
        }

        void OnErrorOpen(string strErrorMessage)
        {
            Debug.LogError(strErrorMessage);
        }

        public void SaveTactic(string filePath)
        {
            model.SaveModel(filePath, OnSuccessSave, OnErrorSave);
        }

        void OnSuccessSave(string json)
        {
            
        }

        void OnErrorSave(string strErrorMessage)
        {
            Debug.LogError(strErrorMessage);
        }
    }
}

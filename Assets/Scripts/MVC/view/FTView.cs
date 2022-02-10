using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine.UI;
using System.Linq;

namespace FootTactic{
    /// <summary>
    /// Root class for all views.
    /// </summary>
    
    public class FTView : View<FTApplication>
    {
        
        public FTFileSelector openFileSelector;
        public FTFileSelector saveFileSelector;
        public Sprite imageSelected;
        public Sprite imageUnselected;

        public void ShowSaveAsDialog()
        {
            saveFileSelector.transform.parent.gameObject.SetActive(true);
            saveFileSelector.gameObject.SetActive(true);
            saveFileSelector.SelectFile();
        }

        public void ShowOpenDialog()
        {
            openFileSelector.transform.parent.gameObject.SetActive(true);
            openFileSelector.gameObject.SetActive(true);
            openFileSelector.SelectFile();
        }
        
		
    }
}
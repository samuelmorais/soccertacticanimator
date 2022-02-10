using UnityEngine;
using System.Collections;
using thelab.mvc;
using UnityEngine.UI;

namespace FootTactic
{
    public class FTTargetView : DragView<FTApplication>
    {
        public void Enable()
        {
            GetComponent<Image>().enabled = true;
        }

        public void Disable()
        {
            GetComponent<Image>().enabled = false;
        }

        
        public void OnClickField(Vector3 clickedPosition)
        {
            transform.position = clickedPosition;
        }

        public void OnDragOverField(Vector3 clickedPosition)
        {
            transform.position = clickedPosition;
        }

    }
        
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FootTactic
{
    public class FTButtonAnimView : ButtonView<FTApplication>
    {
        [SerializeField]
        public int index;



        public override void OnPointerUp(PointerEventData eventData)
        {
            down = false;
            string objectName = FTUtil.TransformButtonNameToModel(gameObject.name);
            
            Notify(notification + "@up", index);
            hold = 0f;
        }

        
    }

}


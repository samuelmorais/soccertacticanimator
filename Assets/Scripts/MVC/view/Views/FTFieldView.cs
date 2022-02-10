using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

namespace FootTactic
{
    public class FTFieldView : DragView<FTApplication>
    {
        public virtual void OnClick()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = cam2D.ScreenToWorldPoint(curScreenPoint);

            Notify(notification + "@up", curPosition);
           
        }
    }

}


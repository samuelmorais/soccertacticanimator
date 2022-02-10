using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
namespace FootTactic
{
    
    public class FTTargetController : Controller<FTApplication>
    {
        public static FTTargetView targetView;
        
        public static Vector3 TargetPosition { get; set; }

        private void Start()
        {
            targetView = FindObjectOfType<FTTargetView>();
        }
        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {
                case "EnableTarget":
                    targetView.Enable();
                    break;

                case "DisableTarget":
                    targetView.Disable();
                    break;


                case "Pass":
                    targetView.Disable();
                    break;

                case "KeepBall":
                    targetView.Disable();
                    break;

                case "PlayerButton@up":
                    targetView.Disable();
                    break;

                case "ClosedMenu":
                    targetView.Disable();
                    break;

                case "Field2D@drag":
                    TargetPosition = (Vector3)p_data[0];
                    targetView.OnDragOverField(TargetPosition);
                    break;

                case "Field2D@up":
                    TargetPosition = (Vector3)p_data[0];
                    targetView.OnClickField(TargetPosition);
                    break;
            }
        }


    }

    

}

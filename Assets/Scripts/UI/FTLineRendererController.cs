using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;
using NG.Patterns.Structure.ObserverPattern;
using Animancer.Examples.StateMachines.Brains;
using Animancer.Examples.Locomotion;
using Animancer;
using System.Linq;
using UnityEngine.UI.Extensions;

namespace FootTactic
{

    public class FTLineRendererController :  Controller<FTApplication>
    {
        public UILineRenderer uiLineRenderer;

        Vector2 start;

        Vector2 end;

        public void SetLine()
        {
            uiLineRenderer.Points[0] = start;
            uiLineRenderer.Points[1] = end;
            
            Disable();
            Enable();
        }

        private void Enable()
        {
            uiLineRenderer.enabled = true;
        }

        private void Disable()
        {
            uiLineRenderer.enabled = false;
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            
            switch (p_event)
            {
               
                case "ValidPositionUpdate":
                    Vector3 endPoint = (Vector3)p_data[0];
                    end = new Vector2(endPoint.x/0.05f, endPoint.z/0.05f);
                    SetLine();
                    break;


                case "ValidPositionStart":
                    Enable();
                    Vector3 startPoint = (Vector3)p_data[0];
                    start = new Vector2(startPoint.x / 0.05f, startPoint.z / 0.05f);

                    break;

                case "DisableLine":
                    Disable();
                    break;

                case "EnableLine":
                    Disable();
                    break;



            }
        }

    }
}

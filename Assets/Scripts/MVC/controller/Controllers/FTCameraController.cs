using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
namespace FootTactic
{
    
    public class FTCameraController : Controller<FTApplication>
    {
        public Camera camera2D;
        public Camera camera3D;
        public GameObject canvas2D;

        void Enable3D()
        {
            Notify("teste");
            camera2D.enabled = false;
            canvas2D.SetActive(false);
        }

        void Enable2D()
        {
            camera2D.enabled = true;
            canvas2D.SetActive(true);
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {


            switch (p_event)
            {
                case "Enable3DView":
                    Enable3D();
                    break;


                case "Enable2DView":
                    Enable2D();
                    break;
            }
        }

    }

    

}

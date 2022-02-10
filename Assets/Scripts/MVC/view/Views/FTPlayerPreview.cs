using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
namespace FootTactic {

    public class FTPlayerPreview : View<FTApplication>
    {
        public FTPlayerStyle playerStyle;
        [SerializeField]
        Camera cameraPreview;
        [SerializeField]
        Transform cameraPosBody;
        [SerializeField]
        Transform cameraPosFace;

        public void CameraFocusOnFace()
        {
            StartCoroutine(MovementCamera(cameraPosFace));
            StartCoroutine(RotatePlayer());
        }

        public void CameraFocusOnBody()
        {
            StartCoroutine(MovementCamera(cameraPosBody));
            StartCoroutine(RotatePlayer());
        }

        IEnumerator MovementCamera(Transform target)
        {
            float initialTime = Time.time;
            float finalTime = Time.time + 1f;
            Vector3 iniPos = cameraPreview.transform.position;
            while(Time.time <= finalTime)
            {
                cameraPreview.transform.position = Vector3.Lerp(iniPos, target.transform.position, (finalTime - initialTime)/ 1f);
                yield return null;
            }
            yield return null;
            
        }

        IEnumerator RotatePlayer()
        {
            
            float finalTime = Time.time + 14f;
            while (Time.time <= finalTime)
            {
                playerStyle.transform.rotation = Quaternion.Euler(0, playerStyle.transform.rotation.eulerAngles.y + Time.deltaTime, 0);

                yield return new WaitForEndOfFrame();
            }
            yield return null;

        }


    }
}
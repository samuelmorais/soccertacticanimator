using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class GetACurve : MonoBehaviour {
 
    CurvesData[] curves;
    public AnimationClip[] clips;
    
    public string[] animationNames;
    void Start () {
        for(var i = 0; i < clips.Length; i++){
            ScriptableObjectUtility.CreateAsset<CurvesData>(i);
            var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(clips[i]);
            Save(curveBindings, clips[i], animationNames[i] !=null? animationNames[i] : "");
        }
    }

    public static void Save(EditorCurveBinding[] curveBindings, AnimationClip clip, string animName = "")
         {
             string assetName = animName == ""?clip.name:animName;
             string[] result = AssetDatabase.FindAssets($"{assetName}.asset");
             CurvesData curvesObject= null;
     
             if (result.Length > 1)
             {
                 Debug.LogError("More than 1 Asset founded");
                 return;
             }
     
             if(result.Length == 0)
             {
                 Debug.Log("Create new Asset");
                 curvesObject = ScriptableObject.CreateInstance<CurvesData >();
                 AssetDatabase.CreateAsset(curvesObject, $"Assets\\Lab\\{assetName}.asset");
             }
             else
             {
                 string path = AssetDatabase.GUIDToAssetPath(result[0]);
                 curvesObject= (CurvesData)AssetDatabase.LoadAssetAtPath(path, typeof(CurvesData));
                 Debug.Log("Found Asset File !!!");
             }

             foreach (var curveBinding in curveBindings)
            {
                Debug.Log(curveBinding.propertyName);
                if (curveBinding.propertyName == "MotionT.x")
                {
                    curvesObject.root_x = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
                if (curveBinding.propertyName == "MotionT.y")
                {
                    curvesObject.root_y = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
                if (curveBinding.propertyName == "MotionT.z")
                {
                    
                    curvesObject.root_z = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
                if (curveBinding.propertyName == "MotionQ.x")
                {
                    curvesObject.root_Qx = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
                if (curveBinding.propertyName == "MotionQ.w")
                {
                    curvesObject.root_Qw = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
                if (curveBinding.propertyName == "MotionQ.y")
                {
                    curvesObject.root_Qy = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
                if (curveBinding.propertyName == "MotionQ.z")
                {
                    curvesObject.root_Qz = AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
            }
     
             EditorUtility.SetDirty(curvesObject);
             AssetDatabase.SaveAssets();
             AssetDatabase.Refresh();
         }
}

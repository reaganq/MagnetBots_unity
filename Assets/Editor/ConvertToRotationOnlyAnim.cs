using UnityEditor;
using UnityEngine;
 
using System.IO;
 
public class ConvertToRotationOnlyAnim
{
    [MenuItem("Assets/Convert To Rotation Animation")]
    static void ConvertToRotationAnimation()
    {
        // Get Selected Animation Clip
        AnimationClip sourceClip = Selection.activeObject as AnimationClip;
        if (sourceClip == null)
        {
            Debug.Log("Please select an animation clip");
            return;
        }
 
        // Rotation only anim clip will have "_rot" post fix at the end
        const string destPostfix = "_rot";
 
        string sourcePath = AssetDatabase.GetAssetPath(sourceClip);
        string destPath = Path.Combine(Path.GetDirectoryName(sourcePath), sourceClip.name) + destPostfix + ".anim";
 
        // first try to open existing clip to avoid losing reference to this animation from other meshes that are already using it.
        AnimationClip destClip = AssetDatabase.LoadAssetAtPath(destPath, typeof(AnimationClip)) as AnimationClip;
        if (destClip == null)
        {
            // existing clip not found.  Let's create a new one
            Debug.Log("creating a new rotation only animation at " + destPath);
 
            destClip = new AnimationClip();
            destClip.name = sourceClip.name + destPostfix;
 
            AssetDatabase.CreateAsset(destClip, destPath);
            AssetDatabase.Refresh();
 
            // and let's load it back, just to make sure it's created?
            destClip = AssetDatabase.LoadAssetAtPath(destPath, typeof(AnimationClip)) as AnimationClip;
        }
 
        if (destClip == null)
        {
            Debug.Log("cannot create/open the rotation only anim at " + destPath);
            return;
        }
 
        // clear all the existing curves from destination.
        destClip.ClearCurves();
 
        // Now copy only rotation curves
        
        
        
        AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves(sourceClip, true);
        foreach (AnimationClipCurveData curveData in curveDatas)
        {
            if(curveData.path == string.Empty)
            {
                AnimationUtility.SetEditorCurve(
                    destClip,
                    curveData.path,
                    curveData.type,
                    curveData.propertyName,
                    curveData.curve
                );
            }
            else
            {
                if (curveData.propertyName.Contains("m_LocalRotation"))
                {
                    AnimationUtility.SetEditorCurve(
                        destClip,
                        curveData.path,
                        curveData.type,
                        curveData.propertyName,
                        curveData.curve
                    );
                }
            }
        }
 
        Debug.Log("Hooray! Coverting to rotation-only anim to " + destClip.name + " is done");
    }
}
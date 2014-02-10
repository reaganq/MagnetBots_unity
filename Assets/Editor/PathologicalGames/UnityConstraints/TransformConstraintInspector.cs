/// <Licensing>
/// © 2011 (Copyright) Path-o-logical Games, LLC
/// If purchased from the Unity Asset Store, the following license is superseded 
/// by the Asset Store license.
/// Licensed under the Unity Asset Package Product License (the "License");
/// You may not use this file except in compliance with the License.
/// You may obtain a copy of the License at: http://licensing.path-o-logical.com
/// </Licensing>
using UnityEditor;
using UnityEngine;
using System.Collections;


namespace PathologicalGames
{

    [CustomEditor(typeof(TransformConstraint))]
    public class TransformConstraintInspector : ConstraintBaseInspector
    {
        protected override void OnInspectorGUIUpdate()
        {
            base.OnInspectorGUIUpdate();

            var script = (TransformConstraint)target;

            GUILayout.BeginHorizontal();

            script.constrainPosition = EditorGUILayout.Toggle("Position", script.constrainPosition);

            if (script.constrainPosition)
            {
                GUIStyle style = EditorStyles.toolbarButton;
                style.alignment = TextAnchor.MiddleCenter;
                style.stretchWidth = true;

                script.outputPosX = GUILayout.Toggle(script.outputPosX, "X", style);
                script.outputPosY = GUILayout.Toggle(script.outputPosY, "Y", style);
                script.outputPosZ = GUILayout.Toggle(script.outputPosZ, "Z", style);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            script.constrainRotation = EditorGUILayout.Toggle("Rotation", script.constrainRotation);
            if (script.constrainRotation)
                script.output = PGEditorUtils.EnumPopup<UnityConstraints.OUTPUT_ROT_OPTIONS>(script.output);
            GUILayout.EndHorizontal();

            script.constrainScale = EditorGUILayout.Toggle("Scale", script.constrainScale);
        }
    }
}
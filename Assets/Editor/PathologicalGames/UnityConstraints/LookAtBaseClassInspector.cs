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

    [CustomEditor(typeof(LookAtBaseClass))]
    public class LookAtBaseClassInspector : ConstraintBaseInspector
    {
        protected override void OnInspectorGUIUpdate()
        {
            base.OnInspectorGUIUpdate();
            var script = (LookAtBaseClass)target;

            script.pointAxis = EditorGUILayout.Vector3Field("Point Axis", script.pointAxis);
            script.upAxis = EditorGUILayout.Vector3Field("Up Axis", script.upAxis);

        }
    }
}
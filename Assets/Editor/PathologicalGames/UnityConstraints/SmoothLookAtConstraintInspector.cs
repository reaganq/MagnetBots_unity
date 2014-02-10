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

    [CustomEditor(typeof(SmoothLookAtConstraint))]
    public class SmoothLookAtConstraintInspector : LookAtBaseClassInspector
    {
        protected override void OnInspectorGUIUpdate()
        {
            base.OnInspectorGUIUpdate();

            var script = (SmoothLookAtConstraint)target;

            script.upTarget = PGEditorUtils.ObjectField<Transform>("Up Target (Optional)", script.upTarget);

            script.interpolation = PGEditorUtils.EnumPopup<UnityConstraints.INTERP_OPTIONS>
            (
                "Interpolation",
                script.interpolation
            );

            script.speed = EditorGUILayout.FloatField("Speed", script.speed);

            script.output = PGEditorUtils.EnumPopup<UnityConstraints.OUTPUT_ROT_OPTIONS>
            (
                "Output",
                script.output
            );
        }
    }
}
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
    // Only compile if not using Unity iPhone
    [CustomEditor(typeof(Targetable))]
    public class TargetableInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            var script = (Targetable)target;

            EditorGUI.indentLevel = 0;
            PGEditorUtils.LookLikeControls();

            script.isTargetable = EditorGUILayout.Toggle("Is Targetable", script.isTargetable);

            script.debugLevel = PGEditorUtils.EnumPopup<DEBUG_LEVELS>("Debug Level", script.debugLevel);

            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

    }
}
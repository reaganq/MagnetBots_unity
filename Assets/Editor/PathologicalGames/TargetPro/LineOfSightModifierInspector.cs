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
    [CustomEditor(typeof(LineOfSightModifier))]
    public class LineOfSightModifierInspector : Editor
    {
        // The main foldout state
        public bool expandEffects = true;

        public override void OnInspectorGUI()
        {
            var script = (LineOfSightModifier)target;

            PGEditorUtils.LookLikeControls();
            EditorGUI.indentLevel = 1;

            // Display some information
            GUILayout.Space(6);
            GUIStyle style = new GUIStyle(EditorStyles.textField);
            style.wordWrap = true;
            style.fontStyle = FontStyle.Italic;
            style.padding = new RectOffset(4, 4, 4, 4);
            EditorGUILayout.LabelField
            (
                "Add layers to activate LOS filtering.\n" +
                "  - Target Tracker to ignore targets\n" +
                "  - Fire Controller to hold fire",
                style
            );

            script.targetTrackerLayerMask = PGEditorUtils.LayerMaskField
            (
                "Target Tracker Mask",
                script.targetTrackerLayerMask
            );

            // Might as well set the component if we are going to do GetComponent.
            //   Also works as a singleton, not that it matters much in Editor scripts.
            if (script.fireCtrl == null)
                script.fireCtrl = script.GetComponent<FireController>();

            if (script.fireCtrl)
                script.fireControllerLayerMask = PGEditorUtils.LayerMaskField
                (
                    "Fire Controller Mask",
                    script.fireControllerLayerMask
                );

            GUILayout.Space(6);

            script.testMode = PGEditorUtils.EnumPopup<LineOfSightModifier.TEST_MODE>
            (
                "LOS Test Mode",
                script.testMode
            );

            if (script.testMode == LineOfSightModifier.TEST_MODE.SixPoint)
            {
                EditorGUI.indentLevel = 2;
                script.radius = EditorGUILayout.FloatField("Radius", script.radius);
                EditorGUI.indentLevel = 1;
            }

            GUILayout.Space(4);
            script.debugLevel = (DEBUG_LEVELS)EditorGUILayout.EnumPopup("Debug Level", (System.Enum)script.debugLevel);


            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
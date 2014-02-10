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
    [CustomEditor(typeof(Detonator))]
    public class DetonatorInspector : Editor
    {

        // The main foldout state
        public bool expandEffects = true;

        public override void OnInspectorGUI()
        {
            var script = (Detonator)target;

            GUIStyle style;
            PGEditorUtils.LookLikeControls();
            EditorGUI.indentLevel = 1;

            script.targetLayers = PGEditorUtils.LayerMaskField
            (
                "Hit Layers",
                script.targetLayers
            );

            script.perimeterLayer = EditorGUILayout.LayerField("Range Layer", script.perimeterLayer);

            EditorGUILayout.BeginHorizontal();

            // Only trigger the update if it actually changes. This runs code in a peroperty 
            //   that may be expensive
            var shape = script.perimeterShape;
            shape = PGEditorUtils.EnumPopup<TargetTracker.PERIMETER_SHAPES>("Detonation Shape", shape);
            if (shape != script.perimeterShape) script.perimeterShape = shape;

            script.overrideGizmoVisibility = false;

            GUILayout.Label("Gizmo", GUILayout.MaxWidth(40));
            script.drawGizmo = EditorGUILayout.Toggle(script.drawGizmo, GUILayout.MaxWidth(47));
            EditorGUILayout.EndHorizontal();

            if (script.drawGizmo)
            {
                EditorGUI.indentLevel = 3;
                EditorGUILayout.BeginHorizontal();

                script.gizmoColor = EditorGUILayout.ColorField("Gizmo Color", script.gizmoColor);

                style = EditorStyles.miniButton;
                style.alignment = TextAnchor.MiddleCenter;
                style.fixedWidth = 52;

                bool clicked = GUILayout.Toggle(false, "Reset", style);
                if (clicked)
                    script.gizmoColor = script.defaultGizmoColor;

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel = 2;

                GUILayout.Space(4);
            }

            script.durration = EditorGUILayout.FloatField("Expand Durration", script.durration);

            // Display some information
            GUILayout.Space(6);
            style = new GUIStyle(EditorStyles.label);
            style.wordWrap = true;
            style.fontStyle = FontStyle.Italic;
            GUILayout.Label
            (
                "If spawned by a projectile, " +
                    "the folowing range and effects are inherited from the projectile...",
                style
            );


            Vector3 range = script.range;
            switch (script.perimeterShape)
            {
                case TargetTracker.PERIMETER_SHAPES.Sphere:
                    range.x = EditorGUILayout.FloatField("Max Range", range.x);
                    range.y = range.x;
                    range.z = range.x;
                    break;

                case TargetTracker.PERIMETER_SHAPES.Box:
                    range = EditorGUILayout.Vector3Field("Max Range", range);
                    break;

                case TargetTracker.PERIMETER_SHAPES.Capsule:
                    range = EditorGUILayout.Vector2Field("Max Range", range);
                    range.z = range.x;
                    break;
            }
            script.range = range;

            GUILayout.Space(6);

            this.expandEffects = PGEditorUtils.SerializedObjFoldOutList<HitEffectGUIBacker>
            (
                "EffectOnTargets",
                script._effectsOnTarget,
                this.expandEffects,
                ref script._editorListItemStates,
                true
            );

            GUILayout.Space(4);
            script.debugLevel = (DEBUG_LEVELS)EditorGUILayout.EnumPopup("Debug Level", (System.Enum)script.debugLevel);


            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
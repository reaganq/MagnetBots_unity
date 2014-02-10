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
    [CustomEditor(typeof(TargetTracker))]
    public class TargetTrackerInspector : Editor
    {
        private bool showPerimeter = true;

        public override void OnInspectorGUI()
        {
            var script = (TargetTracker)target;

            EditorGUI.indentLevel = 1;
            PGEditorUtils.LookLikeControls();
            script.numberOfTargets = EditorGUILayout.IntField("Targets (-1 for all)", script.numberOfTargets);
            script.targetLayers = PGEditorUtils.LayerMaskField("Target Layers", script.targetLayers);
            script.sortingStyle = PGEditorUtils.EnumPopup<TargetTracker.SORTING_STYLES>
            (
                "Sorting Style",
                script.sortingStyle
            );

            if (script.sortingStyle != TargetTracker.SORTING_STYLES.None)
            {
                EditorGUI.indentLevel = 2;
                script.sortInterval = EditorGUILayout.FloatField("Minimum Interval", script.sortInterval);
                EditorGUI.indentLevel = 1;
            }

            EditorGUI.indentLevel = 0;
            this.showPerimeter = EditorGUILayout.Foldout(this.showPerimeter, "Perimeter Settings");

            if (this.showPerimeter)
            {
                EditorGUI.indentLevel = 2;
                script.perimeterLayer = EditorGUILayout.LayerField("Perimeter Layer", script.perimeterLayer);

                EditorGUILayout.BeginHorizontal();

                // Only trigger the update if it actually changes. This runs code in a peroperty 
                //   that may be expensive
                var shape = script.perimeterShape;
                shape = PGEditorUtils.EnumPopup<TargetTracker.PERIMETER_SHAPES>("Perimeter Shape", shape);
                if (shape != script.perimeterShape) script.perimeterShape = shape;


                GUILayout.Label("Gizmo", GUILayout.MaxWidth(40));
                script.drawGizmo = EditorGUILayout.Toggle(script.drawGizmo, GUILayout.MaxWidth(47));
                EditorGUILayout.EndHorizontal();

                if (script.drawGizmo)
                {
                    EditorGUI.indentLevel = 3;
                    EditorGUILayout.BeginHorizontal();

                    script.gizmoColor = EditorGUILayout.ColorField("Gizmo Color", script.gizmoColor);

                    GUIStyle style = EditorStyles.miniButton;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fixedWidth = 52;

                    bool clicked = GUILayout.Toggle(false, "Reset", style);
                    if (clicked)
                        script.gizmoColor = script.defaultGizmoColor;

                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel = 2;

                    GUILayout.Space(4);
                }

                Vector3 range = script.range;
                switch (script.perimeterShape)
                {
                    case TargetTracker.PERIMETER_SHAPES.Sphere:
                        range.x = EditorGUILayout.FloatField("Range", range.x);
                        range.y = range.x;
                        range.z = range.x;
                        break;

                    case TargetTracker.PERIMETER_SHAPES.Box:
                        range = EditorGUILayout.Vector3Field("Range", range);
                        break;

                    case TargetTracker.PERIMETER_SHAPES.Capsule:
                        range = EditorGUILayout.Vector2Field("Range", range);
                        range.z = range.x;
                        break;
                }
                script.range = range;


                script.perimeterPositionOffset = EditorGUILayout.Vector3Field("Position Offset", script.perimeterPositionOffset);
                script.perimeterRotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", script.perimeterRotationOffset);
            }
            EditorGUI.indentLevel = 1;


            GUILayout.Space(4);
            script.debugLevel = (DEBUG_LEVELS)EditorGUILayout.EnumPopup("Debug Level", (System.Enum)script.debugLevel);


            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }


    class PerimeterGizmo
    {
        /// <summary>
        /// A transform used for all Perimeter Gizmos to calculate the final 
        /// position and rotation of the drawn gizmo
        /// </summary>
        static GameObject spaceCalculator;

        [DrawGizmo(GizmoType.Selected | GizmoType.NotSelected)]
        static void RenderPerimeterGizmo(TargetTracker tt, GizmoType gizmoType)
        {
            if (!tt.drawGizmo || !tt.enabled || tt.overrideGizmoVisibility) return;

            Color color = tt.gizmoColor;
            color.a = 0.3f;
            Gizmos.color = color;

            // Set the space everything is drawn in.
            if (PerimeterGizmo.spaceCalculator == null)
            {
                PerimeterGizmo.spaceCalculator = new GameObject();
                PerimeterGizmo.spaceCalculator.hideFlags = HideFlags.HideAndDontSave;
            }

            Transform xform = PerimeterGizmo.spaceCalculator.transform;

            //xform.position = tt.transform.position + tt.perimeterPositionOffset;
            xform.position = (tt.transform.rotation * tt.perimeterPositionOffset) + tt.transform.position;

            var rotOffset = Quaternion.Euler(tt.perimeterRotationOffset);
            xform.rotation = tt.transform.rotation * rotOffset;

            Gizmos.matrix = xform.localToWorldMatrix;

            //UnityEngine.Object.DestroyImmediate(xform.gameObject);


            Vector3 range = tt.GetNormalizedRange();
            Vector3 pos = Vector3.zero;  // We set the sapce relative above
            Vector3 capsuleBottomPos = pos;
            Vector3 capsuleTopPos = pos;
            switch (tt.perimeterShape)
            {
                case TargetTracker.PERIMETER_SHAPES.Sphere:
                    Gizmos.DrawWireSphere(pos, range.x);
                    break;

                case TargetTracker.PERIMETER_SHAPES.Box:
                    Gizmos.DrawWireCube(pos, range);
                    break;

                case TargetTracker.PERIMETER_SHAPES.Capsule:
                    float delta = (range.y * 0.5f) - range.x;

                    capsuleTopPos.y += Mathf.Clamp(delta, 0, delta);
                    Gizmos.DrawWireSphere(capsuleTopPos, range.x);

                    capsuleBottomPos.y -= Mathf.Clamp(delta, 0, delta);
                    Gizmos.DrawWireSphere(capsuleBottomPos, range.x);

                    // Draw 4 lines to connect the two spheres to make a capsule
                    Vector3 start;
                    Vector3 end;

                    start = capsuleTopPos;
                    end = capsuleBottomPos;
                    start.x += range.x;
                    end.x += range.x;
                    Gizmos.DrawLine(start, end);

                    start = capsuleTopPos;
                    end = capsuleBottomPos;
                    start.x -= range.x;
                    end.x -= range.x;
                    Gizmos.DrawLine(start, end);

                    start = capsuleTopPos;
                    end = capsuleBottomPos;
                    start.z += range.x;
                    end.z += range.x;
                    Gizmos.DrawLine(start, end);

                    start = capsuleTopPos;
                    end = capsuleBottomPos;
                    start.z -= range.x;
                    end.z -= range.x;
                    Gizmos.DrawLine(start, end);

                    break;
            }

            color.a = 0.1f;
            Gizmos.color = color;
            switch (tt.perimeterShape)
            {
                case TargetTracker.PERIMETER_SHAPES.Sphere:
                    Gizmos.DrawSphere(pos, range.x);
                    break;

                case TargetTracker.PERIMETER_SHAPES.Box:
                    Gizmos.DrawCube(pos, range);
                    break;

                case TargetTracker.PERIMETER_SHAPES.Capsule:
                    Gizmos.DrawSphere(capsuleTopPos, range.x);  // Set above
                    Gizmos.DrawSphere(capsuleBottomPos, range.x);  // Set above
                    break;
            }

            Gizmos.matrix = Matrix4x4.zero;  // Just to be clean
        }
    }
}
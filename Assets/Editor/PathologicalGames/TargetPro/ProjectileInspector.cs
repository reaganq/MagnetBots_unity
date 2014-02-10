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
    [CustomEditor(typeof(Projectile))]
    public class ProjectileInspector : Editor
    {

        // The main foldout state
        public bool expandEffects = true;

        public override void OnInspectorGUI()
        {
            var script = (Projectile)target;

            PGEditorUtils.LookLikeControls();
            GUIContent content;
            EditorGUI.indentLevel = 1;

            script.targetLayers = PGEditorUtils.LayerMaskField
            (
                "Hit Layers",
                script.targetLayers
            );

            script.detonationMode = PGEditorUtils.EnumPopup<Projectile.DETONATION_MODES>
            (
                "Detonation Mode",
                script.detonationMode
            );

            content = new GUIContent
            (
                "Timer (0 = OFF)",
                "An optional timer (in seconds) to detonate this " +
                "projectile if it expires."
            );
            script.timer = EditorGUILayout.FloatField(content, script.timer);

            content = new GUIContent
            (
                "Detonate On Sleep",
                "If the projectile has a rigidbody, this will detonate it if it falls " +
                    "asleep. See Unity's docs for more information on how this happens."
            );
            script.detonateOnRigidBodySleep = EditorGUILayout.Toggle(content,
                                                        script.detonateOnRigidBodySleep);

            content = new GUIContent("Area Hit", "True to an area, not just a single target");
            script.areaHit = EditorGUILayout.Toggle(content, script.areaHit);

            if (script.areaHit)
            {
                EditorGUI.indentLevel = 2;
                script.numberOfTargets = EditorGUILayout.IntField("Targets (-1 for all)", script.numberOfTargets);

                script.sortingStyle = PGEditorUtils.EnumPopup<TargetTracker.SORTING_STYLES>
                (
                    "Sorting Style",
                    script.sortingStyle
                );


                if (script.sortingStyle != TargetTracker.SORTING_STYLES.None)
                {
                    EditorGUI.indentLevel = 3;
                    script.sortInterval = EditorGUILayout.FloatField("Min Interval", script.sortInterval);
                    EditorGUI.indentLevel = 2;
                }

                script.perimeterLayer = EditorGUILayout.LayerField("Range Layer", script.perimeterLayer);

                EditorGUILayout.BeginHorizontal();

                // Only trigger the update if it actually changes. This runs code in a peroperty 
                //   that may be expensive
                var shape = script.perimeterShape;
                shape = PGEditorUtils.EnumPopup<TargetTracker.PERIMETER_SHAPES>("Perimeter Shape", shape);
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


                //script.perimeterPositionOffset = EditorGUILayout.Vector3Field("Position Offset", script.perimeterPositionOffset);
                //script.perimeterRotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", script.perimeterRotationOffset);
                GUILayout.Space(8);
            }
            else
            {
                script.overrideGizmoVisibility = true;
            }

            EditorGUI.indentLevel = 1;

            script.notifyTargets = PGEditorUtils.EnumPopup<Projectile.NOTIFY_TARGET_OPTIONS>
            (
                "Notify Targets",
                script.notifyTargets
            );

            script.detonationPrefab = PGEditorUtils.ObjectField<Transform>
            (
                "Spawn On Detonation",
                script.detonationPrefab
            );

            EditorGUI.indentLevel = 2;

            this.expandEffects = PGEditorUtils.SerializedObjFoldOutList<HitEffectGUIBacker>
            (
                "EffectOnTargets   (May be inherited)",
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
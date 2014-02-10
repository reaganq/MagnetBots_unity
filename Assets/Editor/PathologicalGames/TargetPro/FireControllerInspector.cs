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
using System.Collections.Generic;


namespace PathologicalGames
{
    [CustomEditor(typeof(FireController))]
    public class FireControllerInspector : Editor
    {
        // The main foldout state
        public bool expandEffects = true;

        public override void OnInspectorGUI()
        {
            var script = (FireController)target;

            GUIContent content;
            EditorGUI.indentLevel = 1;
            PGEditorUtils.LookLikeControls();

            content = new GUIContent("Interval", "Fire every X seconds");
            script.interval = EditorGUILayout.FloatField(content, script.interval);

            content = new GUIContent
            (
                "Init Countdown at 0",
                "Able to fire immediatly when first spawned, before interval count begins"
            );
            script.initIntervalCountdownAtZero = EditorGUILayout.Toggle(content, script.initIntervalCountdownAtZero);

            content = new GUIContent
            (
                "Wait For Alignment",
                "Wait for the emitter to line up with the target before fireing. The " +
                    "count will continue so this will fire as soon as possible."
            );
            script.waitForAlignment = EditorGUILayout.Toggle(content, script.waitForAlignment);
            if (script.waitForAlignment)
            {
                EditorGUI.indentLevel = 2;

                script.emitter = PGEditorUtils.ObjectField<Transform>("Emitter (Optional)", script.emitter);

                content = new GUIContent
                (
                    "Angle Tolerance",
                    "If waitForAlignment is true: If the emitter is pointing towards " +
                        "the target within this angle in degrees, the target can be fired on."
                );
                script.lockOnAngleTolerance = EditorGUILayout.FloatField(content, script.lockOnAngleTolerance);

                content = new GUIContent
                (
                    "Flat Comparison",
                    "If false the true angles will be compared for alignment. " +
                        "(More precise. Emitter must point at target.)\n" +
                        "If true, only the direction matters. " +
                        "(Good when turning in a direction but perfect alignment isn't needed.)"
                );
                script.flatAngleCompare = EditorGUILayout.Toggle(content, script.flatAngleCompare);

                EditorGUI.indentLevel = 1;

            }

            script.notifyTargets = PGEditorUtils.EnumPopup<FireController.NOTIFY_TARGET_OPTIONS>
            (
                "Notify Targets",
                script.notifyTargets
            );

            if (script.notifyTargets > FireController.NOTIFY_TARGET_OPTIONS.Off)
            {
                script.ammoPrefab = PGEditorUtils.ObjectField<Transform>
                (
                    "Ammo (Optional)",
                    script.ammoPrefab
                );
            }


            EditorGUI.indentLevel = 2;

            this.expandEffects = PGEditorUtils.SerializedObjFoldOutList<HitEffectGUIBacker>
            (
                "EffectOnTargets",
                script._effectsOnTarget,
                this.expandEffects,
                ref script._editorListItemStates,
                true
            );

            EditorGUI.indentLevel = 1;


            GUILayout.Space(4);
            script.debugLevel = PGEditorUtils.EnumPopup<DEBUG_LEVELS>("Debug Level", script.debugLevel);

            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

    }


}
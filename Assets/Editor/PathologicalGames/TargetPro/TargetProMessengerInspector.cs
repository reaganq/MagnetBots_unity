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
    [CustomEditor(typeof(TargetProMessenger))]
    public class TargetProMessengerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (TargetProMessenger)target;

            EditorGUI.indentLevel = 1;
            PGEditorUtils.LookLikeControls();

            script.otherTarget = PGEditorUtils.ObjectField<GameObject>
            (
                "Other Message Target (Optional)",
                script.otherTarget
            );

            script.forComponent = (TargetProMessenger.COMPONENTS)EditorGUILayout.EnumPopup
            (
                new GUIContent("For Component", "Choose which component's events to use"),
                script.forComponent
            );


            script.messageMode = (TargetProMessenger.MESSAGE_MODE)EditorGUILayout.EnumPopup
            (
                new GUIContent("Message Mode", "SendMessage will only send to this GameObject"),
                script.messageMode
            );

            // Change the label spacing
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
			EditorGUIUtility.LookLikeControls(220);
#else
			EditorGUIUtility.labelWidth = 220;
#endif

            EditorGUI.indentLevel = 3;
            if (script.forComponent == TargetProMessenger.COMPONENTS.FireController)
            {
                script.fireController_OnStart = EditorGUILayout.Toggle("FireController_OnStart", script.fireController_OnStart);
                script.fireController_OnUpdate = EditorGUILayout.Toggle("FireController_OnUpdate", script.fireController_OnUpdate);
                script.fireController_OnTargetUpdate = EditorGUILayout.Toggle("FireController_OnTargetUpdate", script.fireController_OnTargetUpdate);
                script.fireController_OnIdleUpdate = EditorGUILayout.Toggle("FireController_OnIdleUpdate", script.fireController_OnIdleUpdate);
                script.fireController_OnFire = EditorGUILayout.Toggle("FireController_OnFire", script.fireController_OnFire);
                script.fireController_OnStop = EditorGUILayout.Toggle("FireController_OnStop", script.fireController_OnStop);
            }
            else if (script.forComponent == TargetProMessenger.COMPONENTS.Projectile)
            {
                script.projectile_OnLaunched = EditorGUILayout.Toggle("Projectile_OnLaunched", script.projectile_OnLaunched);
                script.projectile_OnLaunchedUpdate = EditorGUILayout.Toggle("Projectile_OnLaunchedUpdate", script.projectile_OnLaunchedUpdate);
                script.projectile_OnDetonation = EditorGUILayout.Toggle("Projectile_OnDetonation", script.projectile_OnDetonation);
            }
            else if (script.forComponent == TargetProMessenger.COMPONENTS.Targetable)
            {
                script.targetable_OnHit = EditorGUILayout.Toggle("Targetable_OnHit", script.targetable_OnHit);
                script.targetable_OnDetected = EditorGUILayout.Toggle("Targetable_OnDetected", script.targetable_OnDetected);
                script.targetable_OnNotDetected = EditorGUILayout.Toggle("Targetable_OnNotDetected", script.targetable_OnNotDetected);
            }

            EditorGUI.indentLevel = 1;

            GUILayout.Space(4);

            // Change the label spacing back
            PGEditorUtils.LookLikeControls();
            script.debugLevel = PGEditorUtils.EnumPopup<DEBUG_LEVELS>("Debug Level", script.debugLevel);

            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

    }


}
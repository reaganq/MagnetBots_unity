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

    [CustomEditor(typeof(BillboardConstraint))]
    public class BillboardConstraintInspector : LookAtBaseClassInspector
    {
        // Singleton cache to set some defaults on inspection
        private Camera[] cameras;

        protected override void OnInspectorGUIUpdate()
        {
            var script = (BillboardConstraint)target;

            script.vertical = EditorGUILayout.Toggle("Vertical", script.vertical);

            base.OnInspectorGUIUpdate();

            // Set some singletone defaults (will only run once)..

            // This will actually run when the inspector changes, but still better than
            //   running every update
            if (this.cameras == null)
                this.cameras = FindObjectsOfType(typeof(Camera)) as Camera[];

            // Default to the first ortho camera that is set to render this object
            if (script.target == null)
            {
                foreach (Camera cam in cameras)
                {
                    if ((cam.cullingMask & 1 << script.gameObject.layer) > 0)
                    {
                        script.target = cam.transform;
                        break;
                    }
                }
            }

        }
    }
}
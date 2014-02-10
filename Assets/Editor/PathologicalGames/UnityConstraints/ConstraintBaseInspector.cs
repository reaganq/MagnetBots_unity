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

    [CustomEditor(typeof(ConstraintBaseClass))]
    public class ConstraintBaseInspector : ConstraintFrameworkBaseInspector
    {
        protected override void OnInspectorGUIHeader()
        {
            base.OnInspectorGUIHeader();
            var script = (ConstraintBaseClass)target;
            script.target = PGEditorUtils.ObjectField<Transform>("Target", script.target);
        }


        protected override void OnInspectorGUIFooter()
        {

            var script = (ConstraintBaseClass)target;

            // For backwards compatibility. I removed an option and it moved these two
            //   Options back, so Once is now Constrain and Constrain is out of bounds.
            //   Unfortunatly I can't fix the old Once back to once.
            if (script._mode != UnityConstraints.MODE_OPTIONS.Align &&
                script._mode != UnityConstraints.MODE_OPTIONS.Constrain)
            {
                script._mode = UnityConstraints.MODE_OPTIONS.Constrain;
            }

            script._mode = PGEditorUtils.EnumPopup<UnityConstraints.MODE_OPTIONS>("Mode", script._mode);

            script._noTargetMode = PGEditorUtils.EnumPopup<UnityConstraints.NO_TARGET_OPTIONS>("No-Target Mode", script._noTargetMode);

            base.OnInspectorGUIFooter();
        }

    }
}
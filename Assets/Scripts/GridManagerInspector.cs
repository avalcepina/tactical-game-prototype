#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Regenerate"))
            {
                GridManager multilevelGrid = (GridManager)target;
                multilevelGrid.ReadLevel();
            }
        }
    }
}


#endif
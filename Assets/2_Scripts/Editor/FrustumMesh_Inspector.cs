using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FrustumMesh))]
public class FrustumMesh_Inspector : Editor {

    public override void OnInspectorGUI()
    {
        FrustumMesh mesh = (FrustumMesh)target;

        EditorGUI.BeginChangeCheck();
        float distance = EditorGUILayout.FloatField("Distance", mesh.distance);
        float halfAngle = EditorGUILayout.FloatField("Half Angle", mesh.halfAngle);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mesh, "Parameter change");
            mesh.distance = distance;
            mesh.halfAngle = halfAngle;
            mesh.GenerateMesh();
        }
    }
}

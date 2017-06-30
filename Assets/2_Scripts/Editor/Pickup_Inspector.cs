using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pickup), true)]
public class Pickup_Inspector : Editor {

    Pickup pickUp;

    public override void OnInspectorGUI()
    {
        pickUp = (Pickup)target;
        
        if (DrawDefaultInspector())
        {
            pickUp.SetPositionAndRotation();
            pickUp.UpdateSqueezeValue();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Copy Hold Parameters"))
        {
            ClipBoard.value = pickUp.GetHoldParameters().StringSerialize();
        }
        if (GUILayout.Button("Past Hold Parameters"))
        {
            Vector3[] parameters = ClipBoard.value.StringDeserialize<Vector3[]>();
            if (parameters != null && parameters.Length == 4)
            {
                pickUp.SetHoldParameters(parameters[0], parameters[1], parameters[2], parameters[3]);
            }
            else
            {
                EditorUtility.DisplayDialog("Error!", "Hold parameters couldn't be pasted. Please be sure you've copied other parameters previously", "Ok");
            }
        }
    }

    private void OnSceneGUI()
    {
        Tools.hidden = true;

        pickUp = (Pickup)target;

        Vector3 pos = pickUp.pickupT.position;
        Quaternion rot = pickUp.pickupT.rotation;
        Vector3 localScale = pickUp.pickupT.localScale;

        EditorGUI.BeginChangeCheck();

        switch (Tools.current)
        {
            case Tool.Move:
                pos = Handles.PositionHandle(pos, rot);
                break;
            case Tool.Rotate:
                rot = Handles.RotationHandle(rot, pos);
                break;
            case Tool.Scale:
                localScale = Handles.ScaleHandle(localScale, pos, rot,1);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(pickUp.transform, "Transform Handle");
            Undo.RecordObject(pickUp, "Pickup values");
            pickUp.pickupT.position = pos;
            pickUp.pickupT.rotation = rot;
            pickUp.pickupT.localScale = localScale;
            if (Application.isPlaying)
            {
                pickUp.UpdateHoldParamsFromCurrentPos();
            }
        }
    }

    private void OnDestroy()
    {
        Tools.hidden = false;
    }
}

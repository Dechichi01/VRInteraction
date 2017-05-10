using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pickup))]
public class PickupTouch_Inspector : Editor {

    Pickup pickUp;

    public override void OnInspectorGUI()
    {
        pickUp = (Pickup)target;
        
        if (DrawDefaultInspector())
            pickUp.SetPositionAndRotation();
    }

    private void OnSceneGUI()
    {
        Tools.hidden = true;

        pickUp = (Pickup)target;

        Vector3 pos = pickUp.transform.position;
        Quaternion rot = pickUp.transform.rotation;
        Vector3 localScale = pickUp.transform.localScale;

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
            pickUp.transform.position = pos;
            pickUp.transform.rotation = rot;
            pickUp.transform.localScale = localScale;
            if (Application.isPlaying)
                pickUp.UpdateOffSetsFromCurrentPos();
        }
    }

    private void OnDestroy()
    {
        Tools.hidden = false;
    }
}

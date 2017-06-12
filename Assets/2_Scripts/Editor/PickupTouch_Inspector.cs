using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pickup), true)]
public class PickupTouch_Inspector : Editor {

    Pickup pickUp;

    public override void OnInspectorGUI()
    {
        pickUp = (Pickup)target;
        
        if (DrawDefaultInspector())
        {
            pickUp.SetPositionAndRotation();
            pickUp.UpdateSqueezeValue();
        }
    }

    private void OnSceneGUI()
    {
        Tools.hidden = true;

        pickUp = (Pickup)target;

        Vector3 pos = pickUp.tRoot.position;
        Quaternion rot = pickUp.tRoot.rotation;
        Vector3 localScale = pickUp.tRoot.localScale;

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
            pickUp.tRoot.position = pos;
            pickUp.tRoot.rotation = rot;
            pickUp.tRoot.localScale = localScale;
            if (Application.isPlaying)
            {
                pickUp.UpdateOffSetsFromCurrentPos();
            }
        }
    }

    private void OnDestroy()
    {
        Tools.hidden = false;
    }
}

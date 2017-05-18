using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Pickup_Touch))]
public class Pickup_Touch_Inspector : Editor {

    Pickup_Touch pickup;

    public override void OnInspectorGUI()
    {
        pickup = (Pickup_Touch)target;
        base.OnInspectorGUI();
        Vector3 newRot = EditorGUILayout.Vector3Field("Pick Local Rot", pickup.pickLocalRotaion);
        if (newRot != pickup.pickLocalRotaion)
            pickup.SetLocalRotation(newRot);
    }
}

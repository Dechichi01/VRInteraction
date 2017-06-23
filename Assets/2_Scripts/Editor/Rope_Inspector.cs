using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rope), true)]
public class Rope_Inspector : Editor {

    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            Rope rope = (Rope)target;
            rope.SetRopeParams();
        }
    }
}

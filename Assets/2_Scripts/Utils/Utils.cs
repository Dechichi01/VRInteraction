using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClipBoard
{
    public static string value
    {
        get { return GUIUtility.systemCopyBuffer; }
        set { GUIUtility.systemCopyBuffer = value; }
    }
}

public static class MathUtils
{
    public static Vector3 GetMirroredPosition(Vector3 pos)
    {
        return new Vector3(-pos.x, pos.y, pos.z);
    }

    public static Quaternion GetMirroredRotation(Quaternion rot)
    {
        return new Quaternion(rot.x, -rot.y, -rot.z, rot.w);
    }
}

public static class ColorUtils
{
    public static Color SetAlfa(Color col, float alfa)
    {
        return new Color(col.r, col.g, col.b, alfa);
    }
}

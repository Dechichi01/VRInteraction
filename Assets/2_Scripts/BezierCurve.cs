using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BezierCurve : MonoBehaviour {

    [HideInInspector] public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[]
        {
            Vector3.right,
            Vector3.right*2,
            Vector3.right*3,
        };
    }

    public void SetPoints(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        points = new Vector3[] { p0, p1, p2 };
    }

    public void SetPoint(int index, Vector3 p)
    {
        points[index] = p;
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], t)) - transform.position;
    }

    public Vector3 GetVelocityDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
}

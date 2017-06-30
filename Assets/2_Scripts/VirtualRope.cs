using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BezierCurve))]
public class VirtualRope : MonoBehaviour {

    private Transform thisT;
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end;
    [SerializeField] private float ropeMass = 1f;
    [SerializeField] private float sectionsPerMeter = 3;
    [SerializeField] private int minSections = 10;
    [SerializeField][HideInInspector]
    private LineRenderer lineRnd;
    [SerializeField][HideInInspector]
    private BezierCurve curve;

    private List<Vector3> lineRndPositions = new List<Vector3>();

    protected virtual void Start()
    {
        thisT = transform;
        curve = GetComponent<BezierCurve>();
        lineRnd = GetComponent<LineRenderer>();
    }

    protected virtual void Update()
    {
        Vector3 startLocal = thisT.InverseTransformPoint(start.position);
        Vector3 endLocal = thisT.InverseTransformPoint(end.position);

        Vector3 centroid = (endLocal - startLocal) / 2 + GetCentroidDisplacement();
        Vector3 centroidToStart = startLocal - centroid;
        if (Vector3.Dot(centroidToStart, start.forward) < 0)
        {
            centroid = startLocal - Vector3.Reflect(centroidToStart, start.forward);
        }

        curve.SetPoints(startLocal, centroid, endLocal);

        int sectionsNum = GetSectionsNumber();

        float increment = 1f / sectionsNum;

        lineRndPositions.Clear();
        for (float t = 0f; t < 1f; t+= increment)
        {
            lineRndPositions.Add(curve.GetPoint(t));
        }
        lineRndPositions.Add(curve.GetPoint(1));

        lineRnd.positionCount = lineRndPositions.Count;
        lineRnd.SetPositions(lineRndPositions.ToArray());
    }

    private int GetSectionsNumber()
    {
        return (int)Mathf.Max(minSections, Vector3.Distance(start.transform.position, end.transform.position) * sectionsPerMeter);
    }

    private Vector3 GetCentroidDisplacement()
    {
        return 0.1f*ropeMass*(start.position - end.position).sqrMagnitude*((start.forward + end.forward) / 2).normalized;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BezierCurve))]
public class VirtualRope : MonoBehaviour {

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
        curve = GetComponent<BezierCurve>();
        lineRnd = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        Vector3 centroid = (end.localPosition - start.localPosition) / 2 + Vector3.down * .01f * ropeMass;
        curve.SetPoints(start.localPosition, centroid, end.localPosition);

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
}


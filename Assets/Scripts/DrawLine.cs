using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    public void Drawline(Transform start,Transform end)
    {
        Vector3[] points;
        points = new Vector3[2];
        points[0] = start.position;
        points[1] = end.position;
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}

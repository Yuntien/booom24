using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireTools : MonoBehaviour
{
     LineRenderer lineRenderer;

    public Transform startPointTransform;
    public Transform endPointTransform;
     Vector3 startPoint = new Vector3(0, 0, 0);
     Vector3 endPoint = new Vector3(20, 10, 0);
    public enum Direction { LShape, Horizontal, Vertical }
    public enum Position { NearStart, NearEnd, Middle }
    public Direction direction = Direction.Horizontal;
    public Position position;
    public float offset = 1f; // 偏移量
    
    void Start()
    {
       
        
    
    }
    public void DrawWire()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("No LineRenderer component found on this game object.");
            return;
        }

        if (startPointTransform == null || endPointTransform == null)
        {
            Debug.LogError("Start Point Transform or End Point Transform is not set.");
            return;
        }

        Vector3 startPoint = startPointTransform.position;
        Vector3 endPoint = endPointTransform.position;

        Vector3[] points;

        switch (direction)
        {
            case Direction.LShape:
                points = new Vector3[3];
                points[0] = startPoint;
                points[1] = new Vector3(endPoint.x, startPoint.y, 0);
                points[2] = endPoint;
                break;
            case Direction.Horizontal:
                points = new Vector3[4];
                switch (position)
                {
                    case Position.NearStart:
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x + offset, startPoint.y, 0);
                        points[2] = new Vector3(startPoint.x + offset, endPoint.y, 0);
                        points[3] = endPoint;
                        break;
                    case Position.NearEnd:
                        points[0] = startPoint;
                        points[1] = new Vector3(endPoint.x - offset, startPoint.y, 0);
                        points[2] = new Vector3(endPoint.x - offset, endPoint.y, 0);
                        points[3] = endPoint;
                        break;
                    case Position.Middle:
                        float middleX = (startPoint.x + endPoint.x) / 2;
                        points[0] = startPoint;
                        points[1] = new Vector3(middleX, startPoint.y, 0);
                        points[2] = new Vector3(middleX, endPoint.y, 0);
                        points[3] = endPoint;
                        break;
                }
                break;
            case Direction.Vertical:
                points = new Vector3[4];
                switch (position)
                {
                    case Position.NearStart:
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x, startPoint.y + offset, 0);
                        points[2] = new Vector3(endPoint.x, startPoint.y + offset, 0);
                        points[3] = endPoint;
                        break;
                    case Position.NearEnd:
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x, endPoint.y - offset, 0);
                        points[2] = new Vector3(endPoint.x, endPoint.y - offset, 0);
                        points[3] = endPoint;
                        break;
                    case Position.Middle:
                        float middleY = (startPoint.y + endPoint.y) / 2;
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x, middleY, 0);
                        points[2] = new Vector3(endPoint.x, middleY, 0);
                        points[3] = endPoint;
                        break;
                }
                break;
            default:
                Debug.LogError("Invalid direction.");
                return;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

    }

    
     void OnDrawGizmos()
    {
        if (startPointTransform == null || endPointTransform == null)
            return;

        Vector3 startPoint = startPointTransform.position;
        Vector3 endPoint = endPointTransform.position;

        Vector3[] points = new Vector3[4];

        switch (direction)
        {
            case Direction.LShape:
                points = new Vector3[3];
                points[0] = startPoint;
                points[1] = new Vector3(endPoint.x, startPoint.y, 0);
                points[2] = endPoint;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(points[0], points[1]);
                Gizmos.DrawLine(points[1], points[2]);
                break;
            case Direction.Horizontal:
                switch (position)
                {
                    case Position.NearStart:
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x + offset, startPoint.y, 0);
                        points[2] = new Vector3(startPoint.x + offset, endPoint.y, 0);
                        points[3] = endPoint;
                        break;
                    case Position.NearEnd:
                        points[0] = startPoint;
                        points[1] = new Vector3(endPoint.x - offset, startPoint.y, 0);
                        points[2] = new Vector3(endPoint.x - offset, endPoint.y, 0);
                        points[3] = endPoint;
                        break;
                    case Position.Middle:
                        float middleX = (startPoint.x + endPoint.x) / 2;
                        points[0] = startPoint;
                        points[1] = new Vector3(middleX, startPoint.y, 0);
                        points[2] = new Vector3(middleX, endPoint.y, 0);
                        points[3] = endPoint;
                        break;
                }
                break;
            case Direction.Vertical:
                switch (position)
                {
                    case Position.NearStart:
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x, startPoint.y + offset, 0);
                        points[2] = new Vector3(endPoint.x, startPoint.y + offset, 0);
                        points[3] = endPoint;
                        break;
                    case Position.NearEnd:
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x, endPoint.y - offset, 0);
                        points[2] = new Vector3(endPoint.x, endPoint.y - offset, 0);
                        points[3] = endPoint;
                        break;
                    case Position.Middle:
                        float middleY = (startPoint.y + endPoint.y) / 2;
                        points[0] = startPoint;
                        points[1] = new Vector3(startPoint.x, middleY, 0);
                        points[2] = new Vector3(endPoint.x, middleY, 0);
                        points[3] = endPoint;
                        break;
                }
                break;
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }
}

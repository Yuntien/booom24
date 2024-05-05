using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public event System.Action OnChargeReached;
    public float transitionTime = 1.0f; // The time it takes for the transition to complete. You can adjust this.
    public Color recallColor = Color.white; // The color for memory recall. You can adjust this.
    public Color transferColor = Color.white; // Th      public Color deadPortCol;
    public Color poweredWireCol;
    public Color deadWireCol;
    public Color poweredPortCol;

    public Transform startT;
    public Transform endT;
    public int numPoints = 10;
    //public float F = 5;
    public float forceMax = 200;
    public float forceMin;

    public float dstMin;
    public float dstMax;

    Vector2[] points;
    public int k = 30;
    bool charging;
    public float chargeTime = 1;
    float chargePercent;


    //List<Vector2> points = new List<Vector2>();

    Vector2 startOld;
    Vector2 endOld;
    LineRenderer lineRenderer;

public IEnumerator MemoryRecall()
{
    while (true) // Infinite loop.
    {
        // Gradient from deadWireCol to recallColor.
        for (float t = 0; t <= 1; t += Time.deltaTime / transitionTime)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(deadWireCol, 0.0f), new GradientColorKey(recallColor, 1.0f - t), new GradientColorKey(deadWireCol, 1.0f - t + 0.1f), new GradientColorKey(deadWireCol, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f - t), new GradientAlphaKey(1, 1.0f - t + 0.1f), new GradientAlphaKey(1, 1.0f) }
            );

            lineRenderer.colorGradient = gradient;

            yield return null;
        }
    }
}

public IEnumerator MemoryTransfer()
{
    while (true) // Infinite loop.
    {
        // Gradient from deadWireCol to transferColor.
        for (float t = 0; t <= 1; t += Time.deltaTime / transitionTime)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(deadWireCol, 0.0f), new GradientColorKey(transferColor, t), new GradientColorKey(deadWireCol, t + 0.1f), new GradientColorKey(deadWireCol, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, t), new GradientAlphaKey(1, t + 0.1f), new GradientAlphaKey(1, 1.0f) }
            );

            lineRenderer.colorGradient = gradient;

            yield return null;
        }
    }
}




    public void PowerDone()
    {
        //lineRenderer.startColor = deadWireCol;
        //lineRenderer.endColor = deadWireCol;
        //startT.GetComponent<MeshRenderer>().material.color = deadPortCol;
        //endT.GetComponent<MeshRenderer>().material.color = deadPortCol;

    }

    public void SetMin()
    {
        dstMin = (start - end).magnitude;
    }

    public void SetMax()
    {
        dstMax = (start - end).magnitude;
    }

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.startColor = deadWireCol;
        //lineRenderer.endColor = deadWireCol;
        //startT.GetComponent<MeshRenderer>().material.color = deadPortCol;
        //endT.GetComponent<MeshRenderer>().material.color = deadPortCol;

        lineRenderer.positionCount = numPoints;
        points = new Vector2[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            float p = i / (numPoints - 1f);
            points[i] = Vector2.Lerp(start, end, p);
        }

        for (int i = 0; i < 200; i++)
        {
            UpdateRope();
        }

    }

    void FixedUpdate()
    {
        UpdateRope();

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)points[i] + Vector3.forward * -.3f);
        }

    }

    void UpdateRope()
    {
        float t = Mathf.InverseLerp(dstMin, dstMax, (start - end).magnitude);
        float F = Mathf.Lerp(forceMin, forceMax, t);
        points[0] = start;
        points[points.Length - 1] = end;

        for (int ik = 0; ik < k; ik++)
        {
            for (int i = 1; i < points.Length - 1; i++)
            {
                Vector2 offsetPrev = (points[i - 1] - points[i]);
                Vector2 offsetNext = points[i + 1] - points[i];
                Vector2 velocity = offsetPrev.normalized * offsetPrev.magnitude * F + offsetNext.normalized * offsetNext.magnitude * F;
                points[i] += velocity * Time.deltaTime / k;
            }
            for (int i = 1; i < points.Length - 1; i++)
            {
                points[i] += Vector2.down * 9.8f * Time.deltaTime / k;
            }
        }
    }
    public void HasPower()
    {
       // startT.GetComponent<MeshRenderer>().material.color = poweredPortCol;

        lineRenderer.material.color = poweredWireCol;
        charging = true;
    }

    void Update()
    {
        if (charging && chargePercent < 1)
        {
            chargePercent += Time.deltaTime / chargeTime;
            //lineRenderer.colorGradient.colorKeys[1].time = chargePercent;

            Gradient gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(poweredWireCol, chargePercent), new GradientColorKey(deadWireCol, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
                );
            lineRenderer.colorGradient = gradient;


            if (chargePercent >= 1)
            {
                OnFullyCharged();
            }
        }
    }

    void OnFullyCharged()
    {
        endT.GetComponent<MeshRenderer>().material.color = poweredPortCol;
        if (OnChargeReached != null)
        {
            OnChargeReached();
        }
    }


    Vector2 start
    {
        get
        {
            return startT.position;
        }
    }

    Vector2 end
    {
        get
        {
            return endT.position;
        }
    }
}

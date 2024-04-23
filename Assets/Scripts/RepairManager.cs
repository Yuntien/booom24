using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AnomalyEventArgs : EventArgs
{
    public int AnomalyValue { get; }

    public AnomalyEventArgs(int anomalyValue)
    {
        AnomalyValue = anomalyValue;
    }
}
public class RepairManager : MonoBehaviour
{
    public int TotalAnomalyValue { get; private set; }

    // The total anomalies found.
    public int TotalAnomaliesFound { get; private set; }

    // Event triggered when an anomaly is found.
    public event EventHandler<AnomalyEventArgs> OnAnomalyFound;

    // Event triggered when all anomalies have been found.
    public event EventHandler OnAllAnomaliesFound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

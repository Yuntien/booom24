using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Module : MonoBehaviour
{
    public List<Port> inPorts = new List<Port>();
    public List<Port> outPorts = new List<Port>();
    public int initialAnomalyValue = 0;
    public int finalAnomalyValue = 0;
    public int anomalyThreshold = 10;
    public bool isAnomalous = false;
    public bool isChecking = false;
    public void CalculateFinalAnomalyValue()
    {   
        int inAnomalySum = inPorts.Sum(p => p.anomalyValue);
        int outAnomalySum = outPorts.Sum(p => p.anomalyValue);
        finalAnomalyValue = initialAnomalyValue + inAnomalySum - outAnomalySum;
        isAnomalous = finalAnomalyValue > anomalyThreshold;
    }
    public void CheckModule()
    {
        if (isChecking)
        {
            HighlightConnections();
        }
        else
        {
            TurnOffConnections();
            
        }
        
    }
    public void HighlightConnections()
    {
        foreach (var port in outPorts)
        {
            if (Connection.portConnections.TryGetValue(port, out Connection connection))
            {
                connection.Highlight();
                
            }
        }
        foreach (var port in inPorts)
        {
            if (Connection.portConnections.TryGetValue(port, out Connection connection))
            {
                connection.Highlight();
                
            }
        }
    }
    public void TurnOffConnections()
    {
        foreach (var port in outPorts)
        {
            if (Connection.portConnections.TryGetValue(port, out Connection connection))
            {
                connection.TurnOff();
                
            }
        }
        foreach (var port in inPorts)
        {
            if (Connection.portConnections.TryGetValue(port, out Connection connection))
            {
                connection.TurnOff();
                
            }
        }
    }

    private void Update() {
        CheckModule();
    }
}

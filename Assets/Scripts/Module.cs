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
            //TurnOffConnections();
            
        }
        
    }
    public void HighlightConnections()
    {
        foreach (var port in outPorts)
        {
            if (port.Connection!=null)
            {
                port.Connection.Highlight();
                Debug.Log(port.Connection.name);
                
            }
        }
        foreach (var port in inPorts)
        {
            if (port.Connection!=null)
            {
                
                port.Connection.Highlight();
                Debug.Log(port.Connection.name);
                
            }
        }
    }
    public void TurnOffConnections()
    {
        foreach (var port in outPorts)
        {
            if (port.Connection!=null)
            {
                port.Connection.TurnOff();
                
            }
        }
        foreach (var port in inPorts)
        {
            if (port.Connection!=null)
            {
                port.Connection.TurnOff();
                
            }
        }
    }

    private void Update() {
        //CheckModule();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;



public class Module : MonoBehaviour
{
    public List<Port> inPorts = new List<Port>();
    public List<Port> outPorts = new List<Port>();

    public string Name;

    
    //public int initialAnomalyValue = 0;
      [HideInInspector]
    public int finalAnomalyValue = 0;
    //public int anomalyThreshold = 10;
    //public bool isAnomalous = false;
    //public bool isChecking = false;
    public event Action<Module, int> OnAnomalyModuleFound;
    public event Action<Module> OnAnomalySourceFound;
    private bool hasNotifiedAnomaly = false; 

    public void CalculateFinalAnomalyValue()
    {     
        int inAnomalySum = inPorts.Sum(p => p.anomalyValue);
        int outAnomalySum = outPorts.Sum(p => p.anomalyValue);   
        finalAnomalyValue = inAnomalySum - outAnomalySum; 
        if(finalAnomalyValue > 0 && !hasNotifiedAnomaly)
        {
            //起始问题模块找到
            OnAnomalyModuleFound?.Invoke(this, finalAnomalyValue);
            hasNotifiedAnomaly = true;

        }
        bool inAnomaly = inPorts.Any(p => p.anomalyValue != 0);
        bool outAnomaly = outPorts.Any(p => p.anomalyValue != 0);

        if (!inAnomaly && outAnomaly && !hasNotifiedAnomaly)
        {
            //问题源头找到
            OnAnomalySourceFound?.Invoke(this);
            hasNotifiedAnomaly = true;
           
        }

    }
    public void HighlightConnections()
    {
        CalculateFinalAnomalyValue();

        
        foreach (var port in outPorts)
        {
            if (port.Connection!=null)
            {
                port.Connection.Highlight();
                
            }
        }
        foreach (var port in inPorts)
        {
            if (port.Connection!=null)
            {
                
                port.Connection.Highlight();
            
                
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

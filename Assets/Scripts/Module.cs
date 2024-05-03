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

      [HideInInspector]
    public int finalAnomalyValue = 0;
    public event Action<Module, int> OnAnomalyModuleFound;
    public event Action<Module> OnAnomalySourceFound;
    private bool hasNotifiedAnomaly = false; 

    public CheckPort checkport;

    void Awake()
{
    // Find the "in" and "out" child objects
    Transform inObject = transform.Find("in");
    Transform outObject = transform.Find("out");
    // Get the Port components on the child objects and their descendants
    if (inObject != null)
    {
        inPorts = new List<Port>(inObject.GetComponentsInChildren<Port>());
    }
    if (outObject != null)
    {
        outPorts = new List<Port>(outObject.GetComponentsInChildren<Port>());
    }
    // Initialize the CheckPort
    checkport = GetComponentInChildren<CheckPort>();
}
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

    public void StartHightLight()
    {
        StartCoroutine(HighlightConnectionsProcess());       
    }
public IEnumerator HighlightConnectionsProcess()
{
    CalculateFinalAnomalyValue();

    foreach (var port in inPorts)
    {
        if (port.Connection != null)
        {
            StartCoroutine(port.Connection.Highlight());
            port.Connection.startModule.checkport.OpenCover();
            port.Connection.endModule.checkport.OpenCover();
            yield return new WaitForSeconds(1.0f);  // 等待1秒
        }
    }
    foreach (var port in outPorts)
    {
        if (port.Connection != null)
        {
            StartCoroutine(port.Connection.Highlight());
            port.Connection.startModule.checkport.OpenCover();
            port.Connection.endModule.checkport.OpenCover();
            yield return new WaitForSeconds(1.0f);  // 等待1秒
        }
    }
}
    public void TurnOffConnections()
    {
        foreach (var port in outPorts)
        {
            if (port.Connection!=null)
            {
                StartCoroutine(port.Connection.TurnOff());
                //port.Connection.TurnOff();
                
            }
        }
        foreach (var port in inPorts)
        {
            if (port.Connection!=null)
            {
                StartCoroutine(port.Connection.TurnOff());
                
            }
        }
    }
    private void Update() {
        //CheckModule();
    }
}

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
    public int anomalyValue=2;

    private GameObject outline;

    [HideInInspector]
    public bool isRemovable=false;

    void Awake()
{
    // Find the "in" and "out" child objects
    Transform inObject = transform.Find("in");
    Transform outObject = transform.Find("out");
    outline=transform.Find("outline").gameObject;
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
private void OnMouseDown()
{
    // Trigger the OnModuleClicked event
    Robot.Instance.ModuleClicked(this);
}
public void SetOutline(bool isActive)
{
    outline.SetActive(isActive);

}
    public void CalculateFinalAnomalyValue()
    {     
        int inAnomalySum = inPorts.Sum(p => p.anomalyValue);
        int outAnomalySum = outPorts.Sum(p => p.anomalyValue);   
        finalAnomalyValue = inAnomalySum - outAnomalySum; 
        if(finalAnomalyValue > 0 && !hasNotifiedAnomaly)    
        {
            //起始问题模块模块
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
            isRemovable=true;
           
        }
         //UIManager.instance.UpdateAnomalyCalculationText(Name, inAnomalySum, outAnomalySum, finalAnomalyValue, inAnomaly, outAnomaly, hasNotifiedAnomaly, anomalyValue);
         checkport.isChecking=false;
    }

    public void StartHightLight()
    {
        StartCoroutine(HighlightConnectionsProcess());       
    }
public IEnumerator HighlightConnectionsProcess()
{
    
    foreach (var port in inPorts)
    {
        if (port.Connection != null)
        {
            StartCoroutine(port.Connection.Highlight());
            /*UIManager.instance.UpdateConnectionInfoText(
                port.Connection.startModule.Name,
                port.Connection.endModule.Name,
                port.portType == Port.PortType.In ? "in" : "out",
                port.anomalyValue > 0 ? "异常" : "正常"
            );*/
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
    yield return new WaitForSeconds(0.5f); 
    CalculateFinalAnomalyValue();
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

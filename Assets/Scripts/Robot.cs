using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Robot : MonoBehaviour
{
    public List<Module> Modules = new List<Module>();
    public List<Connection> Connections = new List<Connection>();

    public int TotalAnomalyValue { get; private set; }

    // The total anomalies found.
    public int TotalAnomaliesFound { get; private set; }

    public GameObject screen; // The panel named "screen"
    public TextMeshProUGUI anomalyModuleText; // The UI Text element to display anomaly module info
    public TextMeshProUGUI taskText; // The UI Text element to display task info
    public GameObject checkCompleteUI; // The UI for "check complete"

    private List<string> foundAnomalySources = new List<string>(); // Keep track of found anomaly sources

    

    // Event triggered when an anomaly is found.

    public Robot()
    {
        Modules = new List<Module>();
        Connections = new List<Connection>();
    }
    public void Init()
    {
        Transform connections = transform.Find("Connections");

    // Get the Port components on the child objects and their descendants
    if (connections != null)
    {
        Connections = new List<Connection>(connections.GetComponentsInChildren<Connection>());
    }

    }

    private void Start()
    {
        foreach (var module in Modules)
        {
            module.OnAnomalyModuleFound += HandleAnomalyModuleFound;
            module.OnAnomalySourceFound += HandleAnomalySourceFound;
        }
    }

    private void HandleAnomalyModuleFound(Module module, int anomalyValue)
    {
        Debug.Log(module.name);
        TotalAnomalyValue=anomalyValue;
        UIManager.instance.UpdateAnomalyModuleText(module.Name, anomalyValue,module.anomalyValue);
        
    }

    private void HandleAnomalySourceFound(Module module)
    {
        
        // Add the found anomaly source to our list
        foundAnomalySources.Add(module.Name);
        UIManager.instance.UpdateTaskText(foundAnomalySources.ToArray(), TotalAnomalyValue);

    
    }
    
    public void DrawAllConnections()
    {
        Init();
        foreach (var connection in Connections)
        {

            connection.Init();
        }
    }


}
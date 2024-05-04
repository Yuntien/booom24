using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Robot : MonoBehaviour
{
    public static Robot Instance { get; private set; } // Add this line
    public List<Module> Modules = new List<Module>();
    public List<Connection> Connections = new List<Connection>();

    public int TotalAnomalyValue { get; private set; }

    // The total anomalies found.
    public int TotalAnomaliesFound { get; private set; }

    private List<string> foundAnomalySources = new List<string>(); // Keep track of found anomaly sources
    public event Action<Module> OnModuleClicked;

    public void ModuleClicked(Module module)
    {
        OnModuleClicked?.Invoke(module);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy the new instance if one already exists
        }
    }

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
    public void RemoveModule(Module module)
{
    // Remove the module from the Modules list
    Modules.Remove(module);

    // Find all connections related to this module
    List<Connection> connectionsToRemove = new List<Connection>();
    foreach (var connection in Connections)
    {
        if (connection.startModule == module || connection.endModule == module)
        {
            connectionsToRemove.Add(connection);
        }
    }

    // Remove the found connections from the Connections list
    foreach (var connection in connectionsToRemove)
    {
        Connections.Remove(connection);
    }

    // Destroy the module and its related connections in the scene
    foreach (var connection in connectionsToRemove)
    {
        Destroy(connection.gameObject);
    }
    Destroy(module.gameObject);
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
        //ConversationController.Instance.ContinueChoice("全部找到");

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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Robot : MonoBehaviour
{
    public static Robot Instance { get; private set; } // Add this line
    [HideInInspector]
    public List<Module> Modules = new List<Module>();
    [HideInInspector]
    public List<Connection> Connections = new List<Connection>();

    public int TotalAnomalyValue { get; private set; }

    // The total anomalies found.
    public int TotalAnomaliesFound { get; private set; }

    private List<string> foundAnomalySources = new List<string>(); // Keep track of found anomaly sources
    public event Action<Module> OnModuleClicked;
    private List<Module> connectedModules = new List<Module>();

    public event Action OnAllModulesConnected;

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
        Modules.Clear();
        Modules = new List<Module>(GetComponentsInChildren<Module>());
        
        Transform connections = transform.Find("Connections");

    // Get the Port components on the child objects and their descendants
    if (connections != null)
    {
        Connections = new List<Connection>(connections.GetComponentsInChildren<Connection>());
    }

    }
   public void ActiveStartModule()
{
    // 在模块里寻找模组的 checkport，尝试激活 checkport 的 OpenCheckPort 并且激活模组的 lightingStartError()
    // 在这个 lightingStartError 的 DOTween 结束后调用 ConversationController.Instance.ContinueConversation()
    foreach (var module in Modules)
    {
        if (module.checkport != null && module.checkport.isCheckable)
        {
            module.checkport.OpenCheckPort();
            module.lightingStartError();
        }
    }
}

public void ActiveEndModule()
{
    // 在模块里寻找模组，如果有模组的 isRemovable 是 true，那就激活这个模块的 lightEnderror
    // 然后去调用 ConversationController.Instance.ContinueConversation()
    foreach (var module in Modules)
    {
        if (module.isRemovable)
        {
            module.lightingEndError();
            ConversationController.Instance.ContinueConversation();
        }
    }
}

public void SetStartModule(string name)
{
    // 在 modulelist 里找这个传入名称的 module，找到后将它的 checkport 的 isCheckable 改为 true
    // 然后去调用 ConversationController.Instance.ContinueConversation()
    foreach (var module in Modules)
    {
        if (module.Name == name && module.checkport != null)
        {
            module.checkport.isCheckable = true;
            ConversationController.Instance.ContinueConversation();
        }
    }
}

public void SetEndModule(string name)
{
    // 在 modulelist 里找这个传入名称的 module,找到后将它的 isRemovable 改为 true
    // 然后去调用 ConversationController.Instance.ContinueConversation()
    foreach (var module in Modules)
    {
        if (module.Name == name)
        {
            module.isRemovable = true;
            ConversationController.Instance.ContinueConversation();
        }
    }
}


    private void Start()
    {
        Init();
        foreach (var module in Modules)
        {
            module.OnAnomalyModuleFound += HandleAnomalyModuleFound;
            module.OnAnomalySourceFound += HandleAnomalySourceFound;
            module.checkport.OnFirstConnection += HandleFirstConnection;
        }
    }
    private void HandleFirstConnection(Module module)
    {
        if (!connectedModules.Contains(module))
        {
            connectedModules.Add(module);
            Debug.Log("connectedcount"+connectedModules.Count+""+"modulecount"+Modules.Count);
        }

        if (connectedModules.Count == Modules.Count)
        {
            OnAllModulesConnected?.Invoke();
            Debug.Log("allconnected");
            ConversationController.Instance.AllMoudleConnected();
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
        ConversationController.Instance.ContinueChoice("全部找到");

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
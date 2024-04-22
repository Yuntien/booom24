using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Robot : MonoBehaviour
{
    public List<Module> Modules = new List<Module>();
    public List<Connection> Connections = new List<Connection>();

    public Robot()
    {
        Modules = new List<Module>();
        Connections = new List<Connection>();
    }
    public void DrawAllConnections()
    {
        foreach (var connection in Connections)
        {
            connection.lineRenderer.startColor=Color.black;
            connection.lineRenderer.endColor=Color.black;

            connection.lineRenderer.SetPosition(0, connection.startPort.transform.position);
            connection.lineRenderer.SetPosition(1, connection.endPort.transform.position);
        }
    }

    public void DrawConnection(Transform start,Transform end)
    {

    }
}
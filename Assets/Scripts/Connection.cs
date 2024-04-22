using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class Connection : MonoBehaviour
{
    // Start is called before the first frame update
    public Port startPort;
    public Port endPort;

    public LineRenderer lineRenderer;
    public static Dictionary<Port, Connection> portConnections = new Dictionary<Port, Connection>();

private void Awake() {
        
        portConnections[startPort] = this;
        portConnections[endPort] = this;
        Debug.Log(startPort);
    
}

    public void Highlight()
    {
        if (startPort.anomalyValue > 0 || endPort.anomalyValue > 0)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        else
        {
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
    } 
    public void TurnOff()
    {
        // Set the color of the LineRenderer to highlight the connection
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    } 

}

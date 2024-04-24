using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class Connection : MonoBehaviour
{
    // Start is called before the first frame update
    public Port startPort;
    public Port endPort;
    public Module startModule;
    public Module endModule;
    public int anomalyValue;

    public LineRenderer lineRenderer;
    //public static Dictionary<Port, Connection> portConnections = new Dictionary<Port, Connection>();

private void Awake() {
    Init();
        
        //portConnections[startPort] = this;
        //portConnections[endPort] = this;
        
        
        //lineRenderer.material = new Material(Shader.Find("Standard")); // Set the material
    
}
public void Init()
{
    startPort.Connection=this;
        endPort.Connection=this;
        lineRenderer=transform.GetComponent<LineRenderer>();
        startPort.anomalyValue=anomalyValue;
        endPort.anomalyValue=anomalyValue;

        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.1f; // Set the start width
        lineRenderer.endWidth = 0.1f; // Set the end width
        lineRenderer.SetPosition(0, startPort.transform.position);
        lineRenderer.SetPosition(1, endPort.transform.position);
        Material myMaterial = Resources.Load<Material>("Materials/M_Wire");
        lineRenderer.material = myMaterial;

}

    public void Highlight()
    {
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        
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
        //Debug.Log(lineRenderer.startColor);
    } 
    public void TurnOff()
    {
        // Set the color of the LineRenderer to highlight the connection
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    } 

}

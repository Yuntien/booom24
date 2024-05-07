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
    public LineRenderer highlightLineRenderer;
    
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
        GameObject child = new GameObject("HighlightLine");

    // 将新的子GameObject设置为原始GameObject的子对象
    child.transform.SetParent(transform, false);
    gameObject.layer = LayerMask.NameToLayer("Module");
    child.gameObject.layer=LayerMask.NameToLayer("Module");
    
    if(!highlightLineRenderer)
    {
        highlightLineRenderer = child.AddComponent<LineRenderer>();
        highlightLineRenderer.sortingOrder = 3;
        highlightLineRenderer.startColor = Color.black;
        highlightLineRenderer.endColor = Color.black;
        highlightLineRenderer.startWidth = 0.1f; // Set the start width
        highlightLineRenderer.endWidth = 0.1f; // Set the end width
        highlightLineRenderer.SetPosition(0, startPort.transform.position);
        highlightLineRenderer.SetPosition(1, startPort.transform.position);  // 初始时高亮线的终点和起点相同
        highlightLineRenderer.material = myMaterial;

    }
    // 在新的子GameObject上添加一个新的LineRenderer组件
    



}

public IEnumerator Highlight()
{
    
    float duration = 0.5f;  // 点亮的持续时间
    float elapsed = 0.0f;
    Color highlightColor = (startPort.anomalyValue > 0 || endPort.anomalyValue > 0) ? Color.red : Color.white;
    highlightLineRenderer.enabled=true;
    highlightLineRenderer.startColor = highlightColor;
    highlightLineRenderer.endColor = highlightColor;

    Vector3 startPosition = startPort.transform.position;
    Vector3 endPosition = endPort.transform.position;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;
        Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
        highlightLineRenderer.SetPosition(1, currentPosition);
        yield return null;
    }

    highlightLineRenderer.SetPosition(1, endPosition);
     UIManager.instance.UpdateConnectionInfoText(
        startModule.Name,
        endModule.Name,
        startPort.portType == Port.PortType.In ? "input" : "output",
        anomalyValue > 0 ? "异常" : "正常"
    );
    
}

public void TurnOff()
    {
        
        // Set the color of the LineRenderer to highlight the connection
        highlightLineRenderer.startColor = Color.black;
        highlightLineRenderer.endColor = Color.black;
        highlightLineRenderer.enabled=false;
    } 

}

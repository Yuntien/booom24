using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
using TMPro;

public class Module : MonoBehaviour
{
    [HideInInspector]
    public List<Port> inPorts = new List<Port>();
    [HideInInspector]
    public List<Port> outPorts = new List<Port>();
    public string Name;
    [HideInInspector]
    public int finalAnomalyValue = 0;
    public event Action<Module, int> OnAnomalyModuleFound;
    public event Action<Module> OnAnomalySourceFound;
    private bool hasNotifiedAnomaly = false; 
    [HideInInspector]
    public CheckPort checkport;
    [HideInInspector]
    public int anomalyValue=2;
    private GameObject outline;
    public bool isRemovable=false;
    public bool getMemeryFormMoudule=false;
    public bool sendMemeryToMoudle=false;
    public string errortext=null;
    public string warningtext=null;
    [HideInInspector]
    public bool isSource=false;
    [HideInInspector]
    public Tweener colorTween;
    public SpriteRenderer lightSprite;
  [HideInInspector]
    public string showName;


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
    lightSprite.color=Color.gray;
    showName=transform.Find("body").GetComponentInChildren<TextMeshPro>().text;
    transform.Find("body").GetComponentInChildren<TextMeshPro>().text="";


}
public void ActiveStartModule()
{

}
public void ActiveEndModule()
{

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
        bool inAnomaly = inPorts.Any(p => p.anomalyValue != 0);
        bool outAnomaly = outPorts.Any(p => p.anomalyValue != 0);
        if(finalAnomalyValue > 0 && !hasNotifiedAnomaly)    
        {
            //起始问题模块模块
            OnAnomalyModuleFound?.Invoke(this, finalAnomalyValue);
            hasNotifiedAnomaly = true;
            lightSprite.color=Color.yellow;
        }
        

        else if (!inAnomaly && outAnomaly && !hasNotifiedAnomaly)
        {
            //问题源头找到
            OnAnomalySourceFound?.Invoke(this);
            hasNotifiedAnomaly = true;
            isRemovable=true;
            lightingEndError();
           
           
        }
        else
        {
            lightSprite.DOColor(new Color(0.8932762f,1f,0.654717f,1), 0.5f);


        }
         UIManager.instance.UpdateAnomalyCalculationText(showName, inAnomalySum, outAnomalySum, finalAnomalyValue, inAnomaly, outAnomaly, hasNotifiedAnomaly, anomalyValue,errortext,warningtext);
         checkport.isChecking=false;
         ConversationController.Instance.ContinueChoice(Name);
    }
    public void lightingEndError()
    {
        transform.Find("body").GetComponentInChildren<TextMeshPro>().text=showName;
         colorTween = lightSprite.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
         outline.SetActive(true);
        
    }
    public void lightingStartError()
{
    transform.Find("body").GetComponentInChildren<TextMeshPro>().text = showName;
    colorTween = lightSprite.DOColor(Color.yellow, 0.5f).SetLoops(4, LoopType.Yoyo)
    .OnComplete(() => 
    {
        lightSprite.color = Color.yellow;
        ConversationController.Instance.ContinueConversation();
    });  
}

    public void StartHightLight()
    {
        StartCoroutine(HighlightConnectionsProcess());       
    }
public IEnumerator HighlightConnectionsProcess()
{
    List<Coroutine> coroutines = new List<Coroutine>();
    
    
    foreach (var port in inPorts)
    {
        if (port.Connection != null)
        {
            Debug.Log(port.Connection.name);
            coroutines.Add(StartCoroutine(port.Connection.Highlight()));

            port.Connection.startModule.checkport.OpenCover();
            port.Connection.endModule.checkport.OpenCover();

        }
    }
    foreach (var port in outPorts)
    {
        if (port.Connection != null)
        {
            coroutines.Add(StartCoroutine(port.Connection.Highlight()));
            port.Connection.startModule.checkport.OpenCover();
            port.Connection.endModule.checkport.OpenCover();

        }
    }
    foreach (Coroutine coroutine in coroutines)
    {
        yield return coroutine;
    }

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

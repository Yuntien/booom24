using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;

public class CheckPort : MonoBehaviour
{
    public SpriteRenderer cover; // The cover of the check port
    public Module module;
     public event Action<Module> OnFirstConnection;
    private bool hasBeenConnected = false;
    [HideInInspector]
    public bool isCheckable=false;
     [HideInInspector]

    public bool isChecking = false;

    void Awake()
    {
        module = GetComponentInParent<Module>();
        isCheckable=false;
        isChecking = false;


        /*if(isCheckable)
        {
            cover.enabled=false;
        }
        else
        {
            cover.enabled=true;
        }*/

    }

    public void OpenCover()
    {
        cover.enabled=false;
        isCheckable=true;
        //module.transform.Find("body").GetComponentInChildren<TextMeshPro>().text=module.showName;
    }

public void OpenCheckPort()
{
    if(isCheckable)
        {
            cover.enabled=false;
        }
        else
        {
            cover.enabled=true;
        }


}
IEnumerator connectCoroutine = null;
   public void Connect()
{
    if (module != null && isCheckable)
    {
        module.transform.Find("body").GetComponentInChildren<TextMeshPro>().text=module.showName;
        if(module.getMemeryFormMoudule)
        {
            UIManager.instance.GetMemeryFormMoudule();
            isChecking=true;
            Rope rope  = FindObjectOfType<Rope>();
            rope.StartCoroutine(rope.MemoryRecall());
            return;
        }
        if(module.sendMemeryToMoudle)
        { 
            Rope rope  = FindObjectOfType<Rope>();
            rope.StartCoroutine(rope.MemoryTransfer());
             UIManager.instance.SendMemeryToMoudle();
             isChecking=true;
             return;

        }
        if (!hasBeenConnected)
        {
            OnFirstConnection?.Invoke(module);
            hasBeenConnected = true;
        }

        //这里是每次插module的地方
        isChecking = true;
        module.SetOutline(true);
        Debug.Log(module.name+"ischecking");
        UIManager.instance.UpdateModuleCheckText(module.Name);

        connectCoroutine = WaitAndHighlight(1.0f);
        StartCoroutine(connectCoroutine);
    }
}
    private IEnumerator WaitAndHighlight(float waitTime)
    {
        //这里之后需要给被check的模块一个效果
        yield return new WaitForSeconds(waitTime);
        HighlightBegin();
    }
    public void HighlightBegin()
    {
        //这里是每次插点亮的地方，现在用协程去模拟下停顿
        if (module != null && isCheckable)
        {
            
            module.StartHightLight();
            
        }

    }

    public void Disconnect()
{
    Debug.Log("disconnection");
    if (module != null)
    {
        isChecking = false;
        if(!module.isSource)
        module.SetOutline(false);
        // If a connection is in progress, stop it.
        if (connectCoroutine != null)
        {
            StopCoroutine(connectCoroutine);
            connectCoroutine = null;
        }

        module.TurnOffConnections();
    }
     UIManager.instance.ClearText();
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckPort : MonoBehaviour
{
    public SpriteRenderer cover; // The cover of the check port
    public Module module;
     public event Action<Module> OnFirstConnection;
    private bool hasBeenConnected = false;
    public bool isCheckable=false;

    public bool isChecking = false;

    void Awake()
    {
        module = GetComponentInParent<Module>();
        if(isCheckable)
        {
            cover.enabled=false;
        }
        else
        {
            cover.enabled=true;
        }

    }

    public void OpenCover()
    {
        cover.enabled=false;
        isCheckable=true;
    }


IEnumerator connectCoroutine = null;
   public void Connect()
{
    if (module != null && isCheckable)
    {
        if(module.getMemeryFormMoudule)
        {
            UIManager.instance.GetMemeryFormMoudule();
            isChecking=true;
            return;
        }
        if(module.sendMemeryToMoudle)
        { 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPort : MonoBehaviour
{
    public SpriteRenderer cover; // The cover of the check port
    public Module module;
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
        isChecking = true;
        module.SetOutline(true);
        Debug.Log(module.name+"ischecking");
        UIManager.instance.UpdateModuleCheckText(module.Name);

        // Store the coroutine in a variable so it can be stopped later.
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

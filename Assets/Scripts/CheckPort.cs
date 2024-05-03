using Unity.VisualScripting;
using UnityEngine;

public class CheckPort : MonoBehaviour
{
    public SpriteRenderer cover; // The cover of the check port
    public Module module;
    public bool isCheckable=false;

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

    public void Connect()
    {
        if (module != null && isCheckable)
        {
            module.StartHightLight();
            
        }
    }

    public void Disconnect()
    {
        if (module != null)
        {
            module.TurnOffConnections();
        }
    }
}

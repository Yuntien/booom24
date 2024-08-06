using UnityEngine;
using UnityEngine.UI;

public class ToolSelection : MonoBehaviour
{
    public ToolFollowMouse toolFollowMouseScript; // Assign this in the inspector
    //public Button toolButton; // Assign this in the inspector

    void Start()
    {
        // Add the listener to the tool button
        //toolButton.onClick.AddListener(OnToolSelected);
    }

    public void OnToolSelected()
    {
        toolFollowMouseScript.StartFollowingMouse();
    }
}

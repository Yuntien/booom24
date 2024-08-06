using UnityEngine;

public class ToolFollowMouse : MonoBehaviour
{
    public RectTransform toolRectTransform; // Assign this in the inspector
    public Canvas canvas; // Assign this in the inspector

    private bool isFollowing = false;

    void Update()
    {
        if (isFollowing)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                Input.mousePosition, 
                canvas.worldCamera, 
                out position);

            toolRectTransform.position = canvas.transform.TransformPoint(position);
        }
    }

    public void StartFollowingMouse()
    {
        isFollowing = true;
    }

    public void StopFollowingMouse()
    {
        isFollowing = false;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    public UnityEvent onClick;

    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}
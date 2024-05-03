using UnityEngine;
using DG.Tweening;
using UnityEngine.Events; 

public class Screw : MonoBehaviour
{
    public float unscrewTime = 2f;  // The time it takes to unscrew the screw
   [HideInInspector]
    public float unscrewProgress = 0f;  // The current progress of unscrewing
    private bool isUnscrewing = false;  // Whether the screw is currently being unscrewed
    public UnityEvent OnScrewRemoved;

    private bool isRemoved = false; 
    public void Initialize()
    {
        isRemoved=false;
        unscrewProgress = 0f;
        isUnscrewing=false;
    }
    void Update()
    {
    if (Input.GetMouseButtonDown(0))
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        int layerMask = 1 << 9;  
        layerMask = ~layerMask;  

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            // The screw was clicked
            isUnscrewing = true;
            transform.DORotate(new Vector3(0, 0, -360), unscrewTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }
    }

    if (Input.GetMouseButtonUp(0))
    {
        isUnscrewing = false;
        DOTween.Kill(transform);
    }
        if (isUnscrewing && !isRemoved)
        {
            unscrewProgress += Time.deltaTime;
            if (unscrewProgress >= unscrewTime)
            {
                RemoveScrew();
                //isRemoved = true;  // Set the flag to true
                
            }
        }
    }
     private void RemoveScrew()
    {
        OnScrewRemoved?.Invoke();
        OnScrewRemoved.RemoveAllListeners();  // Remove all listeners
        DOTween.Kill(transform);
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        isUnscrewing = true;
        transform.DORotate(new Vector3(0, 0, -360), unscrewTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    void OnMouseUp()
    {
        isUnscrewing = false;
        DOTween.Kill(transform);
    }
}

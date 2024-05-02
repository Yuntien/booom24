using UnityEngine;
using DG.Tweening;
using UnityEngine.Events; 

public class Screw : MonoBehaviour
{
    public float unscrewTime = 2f;  // The time it takes to unscrew the screw
    private float unscrewProgress = 0f;  // The current progress of unscrewing
    private bool isUnscrewing = false;  // Whether the screw is currently being unscrewed
    public UnityEvent OnScrewRemoved;

    void Update()
    {
        if (isUnscrewing)
        {
            unscrewProgress += Time.deltaTime;
            if (unscrewProgress >= unscrewTime)
            {
                 OnScrewRemoved?.Invoke(); 
                Destroy(gameObject);
            }
        }
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

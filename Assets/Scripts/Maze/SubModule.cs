using DG.Tweening;
using UnityEngine;
using TMPro;

public class Submodule : MonoBehaviour
{
    public SpriteRenderer lightSpriteRenderer;
    public TextMeshPro screenText;
    [HideInInspector]
    public bool isDefined=false;
    public bool isRemovable = false;
    private Tweener colorTween;

    void OnEnable()
    {
        SetScreenText("未定义");
        if (isRemovable)
        {
            // Start blinking when the submodule is enabled
            colorTween = lightSpriteRenderer.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    void OnDisable()
    {
        if (colorTween != null)
        {
            // Stop blinking when the submodule is disabled
            colorTween.Kill();
            colorTween = null;
            lightSpriteRenderer.color = Color.black; // Reset color to black
        }
    }

    public void SetScreenText(string text)
    {
        screenText.text = text;
        
    }

    public void SetRemovable(bool removable)
    {
        this.name=screenText.text;
        isRemovable = removable;
        if (isRemovable)
        {
            // Start blinking when the submodule is enabled
            colorTween = lightSpriteRenderer.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (colorTween != null)
            {
                // Stop blinking when the submodule is disabled
                colorTween.Kill();
                colorTween = null;
                lightSpriteRenderer.color = Color.black; // Reset color to black
            }
        }
    }
}

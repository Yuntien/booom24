using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EyeTarget : MonoBehaviour
{
    public Transform targetTransform; // EyeTarget的位置
    [SerializeField]
    private float burnRadiusMin = 0f; // 对应_BurnRadius的最小值

    [SerializeField]
    private float burnRadiusMax = 1f; // 对应_BurnRadius的最大值

    private Material material;

    private void Awake()
    {
        // 获取此对象的材质
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        material.SetFloat("_SourceGlowDissolveFade", burnRadiusMin);

    }
    private void Start()
    {
        
    }
    public void SetToMinBurnRadius()
    {
        material.SetFloat("_SourceGlowDissolveFade", burnRadiusMin);
    }

    public void SetToMaxBurnRadius()
    {
        material.SetFloat("_SourceGlowDissolveFade", burnRadiusMax);
    }
    public void SetToMaxBlur()
    {
        material.SetFloat("_GaussianBlurFade", 1f);
    }
     public void SetToMinBlur()
    {
        material.SetFloat("_GaussianBlurFade", 0f);
    }
    public void FadeOut(float duration)
    {
        // 使用DoTween库来改变_BurnRadius，使目标褪色
        material.DOFloat(burnRadiusMin, "_SourceGlowDissolveFade", duration);
        material.DOFloat(1f, "_GaussianBlurFade", duration).SetEase(Ease.InSine);
        material.DOFloat(1f, "_UVDistortFade", duration/2).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo);;
        


    }

    public void ColorIn(float duration)
    {
        material.DOFloat(burnRadiusMax, "_SourceGlowDissolveFade", duration).SetEase(Ease.InOutSine);
        material.DOFloat(0f, "_GaussianBlurFade", duration).SetEase(Ease.InOutSine);
        material.DOFloat(1f, "_UVDistortFade", duration/2).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);;
    }
}

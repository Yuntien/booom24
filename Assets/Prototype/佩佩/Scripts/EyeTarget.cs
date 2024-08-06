using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class EyeTarget : MonoBehaviour
{
    public Transform targetTransform; // EyeTarget的位置
    [SerializeField]
    private float burnRadiusMin = 0f; // 对应_BurnRadius的最小值

    [SerializeField]
    private float burnRadiusMax = 1f; // 对应_BurnRadius的最大值

    private Material forwardMaterial;

    [SerializeField]
    private SpriteRenderer backSpriteRenderer;

    [SerializeField]
    private SpriteRenderer wrongColorSpriteRenderer;
    private Material backMaterial;
    private Material wrongColorMaterial;

    private void Awake()
    {
        // 获取此对象的材质
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        forwardMaterial = spriteRenderer.material;
        forwardMaterial.SetFloat("_SourceGlowDissolveFade", burnRadiusMin);

        backMaterial=backSpriteRenderer.material;
        wrongColorMaterial=wrongColorSpriteRenderer.material;;



    }
    private void Start()
    {
        
    }
    public void SetToMinBurnRadius()
    {
        forwardMaterial.SetFloat("_SourceGlowDissolveFade", burnRadiusMin);
    }

    public void SetToMaxBurnRadius()
    {
        forwardMaterial.SetFloat("_SourceGlowDissolveFade", burnRadiusMax);
    }
    public void SetToMaxBlur( bool isForwardMaterial = true)
    {
        Material targetMaterial = isForwardMaterial ? forwardMaterial : backMaterial;
        targetMaterial.SetFloat("_GaussianBlurFade", 1f);
    }
     public void SetToMinBlur( bool isForwardMaterial = true)
    {
        Material targetMaterial = isForwardMaterial ? forwardMaterial : backMaterial;
        targetMaterial.SetFloat("_GaussianBlurFade", 0f);
    }
    public void SetToNoWrongColor()
    {
        wrongColorMaterial.SetFloat( "_Alpha", 0f);
    }
       public void SetToWrongColor()
    {
        wrongColorMaterial.SetFloat( "_Alpha", 1f);
    }
    public void FadeOut(float duration)
    {
        // 使用DoTween库来改变_BurnRadius，使目标褪色
        forwardMaterial.DOFloat(burnRadiusMin, "_SourceGlowDissolveFade", duration);
        forwardMaterial.DOFloat(1f, "_GaussianBlurFade", duration).SetEase(Ease.InSine);
        forwardMaterial.DOFloat(1f, "_UVDistortFade", duration/2).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo);;
        


    }

    public void ColorIn(float duration)
    {
        forwardMaterial.DOFloat(burnRadiusMax, "_SourceGlowDissolveFade", duration).SetEase(Ease.InOutSine);
        forwardMaterial.DOFloat(0f, "_GaussianBlurFade", duration).SetEase(Ease.InOutSine);
        forwardMaterial.DOFloat(1f, "_UVDistortFade", duration/2).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);;
    }

    public void WrongColorIn(float duration)
    {
        wrongColorMaterial.DOFloat(1f, "_Alpha", duration).SetEase(Ease.InOutSine);
    }
        public void WrongColorOut(float duration)
    {
        wrongColorMaterial.DOFloat(0f, "_Alpha", duration).SetEase(Ease.InOutSine);
    }
    
     public void BlurIn(float duration)
    {
         wrongColorMaterial.DOFloat(0f, "General Alpha", duration).SetEase(Ease.InOutSine);
    }

    public void BlurOut(float duration)
    {
        backMaterial.DOFloat(0f, "_GaussianBlurFade", duration).SetEase(Ease.InOutSine);
    }
     public void AddDisorderEffect(float duration)
    {
        // 这里添加错乱效果的具体实现
    }

    public void RemoveDisorderEffect(float duration)
    {
        // 这里移除错乱效果的具体实现
    }
}

using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EyeTarget : MonoBehaviour
{
    [SerializeField]
    private float burnRadiusMin = 0f; // 对应_BurnRadius的最小值

    [SerializeField]
    private float burnRadiusMax = 1f; // 对应_BurnRadius的最大值

    private Material material;

    private void Awake()
    {
        // 获取此对象的材质
        material = GetComponent<Renderer>().material;
    }
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        material.SetFloat("_BurnRadius", burnRadiusMax);
    }
    public void SetToMinBurnRadius()
    {
        material.SetFloat("_BurnRadius", burnRadiusMin);
    }

    public void SetToMaxBurnRadius()
    {
        material.SetFloat("_BurnRadius", burnRadiusMax);
    }
    public void FadeOut(float duration)
    {
        // 使用DoTween库来改变_BurnRadius，使目标褪色
        material.DOFloat(burnRadiusMax, "_BurnRadius", duration);
        material.DOFloat(1f, "_GlitchFade", duration/2).SetLoops(2, LoopType.Yoyo);;

    }


    public void ColorIn(float duration)
    {
        material.DOFloat(burnRadiusMin, "_BurnRadius", duration);
        material.DOFloat(1f, "_GlitchFade", duration/2).SetLoops(2, LoopType.Yoyo);;
    }
}

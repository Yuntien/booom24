using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingManager : MonoBehaviour
{
    public GameObject forward, mid, back;
    public Slider focusSlider;

    private int forwardFocus = 2;
    private int midFocus = 4;
    private int backFocus = 8;
    private int currentFocus;

    private List<_2dxFX_BlurHQX> forwardSprites = new List<_2dxFX_BlurHQX>();
    private List<_2dxFX_BlurHQX> midSprites = new List<_2dxFX_BlurHQX>();
    private List<_2dxFX_BlurHQX> backSprites = new List<_2dxFX_BlurHQX>();

    private void Start()
    {
        // 初始化时查找所有的Sprite并存入对应的List
        FindSpritesAndAddToList(forward, forwardSprites);
        FindSpritesAndAddToList(mid, midSprites);
        FindSpritesAndAddToList(back, backSprites);

        // 设置Slider的最大最小值和步长
        focusSlider.minValue = 0;
        focusSlider.maxValue = 8;
        focusSlider.wholeNumbers = true;
        UpdateFocus(currentFocus);

        // 添加Slider的值改变事件
        focusSlider.onValueChanged.AddListener(UpdateFocus);
    }

    private void FindSpritesAndAddToList(GameObject parent, List<_2dxFX_BlurHQX> list)
    {
        _2dxFX_BlurHQX[] sprites = parent.GetComponentsInChildren<_2dxFX_BlurHQX>();
        foreach (_2dxFX_BlurHQX sprite in sprites)
        {
            list.Add(sprite);
        }
    }

    private void UpdateFocus(float value)
    {
        currentFocus = (int)value;
        UpdateBlur(forwardSprites, forwardFocus, "Forward");
        UpdateBlur(midSprites, midFocus, "Mid");
        UpdateBlur(backSprites, backFocus, "Back");
    }

    private void UpdateBlur(List<_2dxFX_BlurHQX> sprites, int focusPoint, string layerName)
{
    float blurValue;
    foreach (_2dxFX_BlurHQX sprite in sprites)
    {
        // 计算模糊值，距离越远模糊值越大
        blurValue = Mathf.Clamp01 (Mathf.Abs(currentFocus - focusPoint) / 8f)*4f;
        sprite.Blur = blurValue;
        Debug.Log($"{layerName} blur: {blurValue}");
    }
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UFProcess : MonoBehaviour
{
    public Image memoryProcessImage; // 用于显示调取记忆的表现
    public Image memoryFinishedImage; // 用于显示记忆处理结束的表现
    public Image UFImage; // 用于显示糅合后的广告图
    public Button drawButton; // 绘图按钮

    public Sprite defaultSprite; // 默认图片
    public List<Sprite> memorySprites; // 记忆图片列表
    public List<Sprite> memoryFinishedSprites; // 记忆处理完成图片列表
    public List<Sprite> UFMemorySprites; // 糅合后的广告图列表
    private int currentMemoryIndex = 0; // 当前记忆索引
    
    private bool isProcessing = false; // 新增变量来追踪是否正在处理记忆
    private void Start()
    {
        // 绑定绘图按钮事件
        drawButton.onClick.AddListener(ProcessMemory);

        // 初始化UI状态
        memoryProcessImage.gameObject.SetActive(false);
        memoryFinishedImage.gameObject.SetActive(false);
        UFImage.gameObject.SetActive(false);
    }

    private void ProcessMemory()
    {
        if (isProcessing)
        {
            Debug.LogWarning("Already processing a memory. Please wait until the process is finished.");
            return;
        }

        isProcessing = true; // 开始处理，设置为true
        // 确保当前索引有效
        if (currentMemoryIndex >= memorySprites.Count || currentMemoryIndex >= memoryFinishedSprites.Count || currentMemoryIndex >= UFMemorySprites.Count)
        {
            Debug.LogWarning("Memory index is out of range.");
            return;
        }

        // 开始处理记忆的表现
        memoryProcessImage.gameObject.SetActive(true);
        memoryProcessImage.sprite = defaultSprite; // 设置默认图片
        memoryProcessImage.color = Color.white; // 设置初始颜色

        Sequence memorySequence = DOTween.Sequence();

        // 图片闪烁效果
       // 图片闪烁效果
        memorySequence.Append(memoryProcessImage.DOColor(Color.white, 0)); // 确保开始时是白色
        for (int i = 0; i < 20; i++) // 闪烁10次
        {
            memorySequence.Append(memoryProcessImage.DOColor(new Color(Random.value, Random.value, Random.value), 0.075f)); // 随机颜色
            memorySequence.Append(memoryProcessImage.DOColor(Color.white, 0.075f)); // 返回到白色
        }


        // 加载并显示记忆图片
        memorySequence.AppendCallback(() =>
        {
            memoryProcessImage.color = Color.white; 
            memoryProcessImage.sprite = memorySprites[currentMemoryIndex]; // 设置当前记忆图片
        });

        memorySequence.AppendInterval(4f); // 添加1.5秒的延时

        // 记忆处理结束，激活memoryFinishedImage
        memorySequence.AppendCallback(() =>
        {
            memoryProcessImage.gameObject.SetActive(false);
            memoryFinishedImage.gameObject.SetActive(true);
            memoryFinishedImage.color= Color.white;
            memoryFinishedImage.sprite = memoryFinishedSprites[currentMemoryIndex]; // 设置记忆处理结束的图片
            memoryFinishedImage.material.SetFloat("Pixel_Fade", 0); // 设置 Pixel_Fade 为0
        });

        // Pixel_Fade 从0变为1，持续1.5秒
        memorySequence.Append(DOTween.To(() => memoryFinishedImage.material.GetFloat("Pixel_Fade"), x => memoryFinishedImage.material.SetFloat("Pixel_Fade", x), 1, 3f));

        memorySequence.AppendInterval(1.5f); // 添加1.5秒的延时

        // 激活UFImage，并开始像素淡入效果
        memorySequence.AppendCallback(() =>
        {
            memoryFinishedImage.gameObject.SetActive(false);
            UFImage.gameObject.SetActive(true);
            UFImage.color= Color.white;
            UFImage.sprite = UFMemorySprites[currentMemoryIndex]; // 设置糅合后的广告图
            UFImage.material.SetFloat("Pixel_Fade", 1); // 设置 Pixel_Fade 为1
            UFImage.rectTransform.anchoredPosition = new Vector2(0, -111f); // 重置位置
        });

        // Pixel_Fade 从1变为0，持续1.5秒
        memorySequence.Append(DOTween.To(() => UFImage.material.GetFloat("Pixel_Fade"), x => UFImage.material.SetFloat("Pixel_Fade", x), 0, 3f));

        memorySequence.AppendInterval(2f); // 添加2秒的延时

        // UFImage向右移动
        memorySequence.Append(UFImage.rectTransform.DOAnchorPosX(1000, 1f)); // 假设向右移动1000单位

        // 序列完成时更新索引
        memorySequence.OnComplete(() =>
        {
            UFImage.gameObject.SetActive(false);
            isProcessing = false; // 处理完成，设置为false
            if (currentMemoryIndex < memorySprites.Count - 1)
            {
                currentMemoryIndex++;
            }
            else
            {
                Debug.LogWarning("Memory index is out of range.");
            }
        });
    }
    public bool IsProcessing()
    {
        return isProcessing;
    }
}

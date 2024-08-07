using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryProcess : MonoBehaviour
{
    public Image memoryProcessImage; // 用于显示调取记忆的表现
    public Image memoryFinishedImage; // 用于显示记忆处理结束的表现
    public Button searchMemoryButton; // 搜寻记忆按钮

    //public Sprite defaultSprite; // 默认图片
    public List<Sprite> memorySprites; // 记忆图片列表
    private int currentMemoryIndex = 0; // 当前记忆索引

    private bool isProcessing = false; // 新增变量来追踪是否正在处理记忆



    private void Start()
    {
        // 绑定搜寻记忆按钮事件
        searchMemoryButton.onClick.AddListener(SearchMemory);

        // 初始化UI状态
        memoryProcessImage.gameObject.SetActive(true);
        memoryFinishedImage.gameObject.SetActive(false);

        // 初始化材质参数
        memoryProcessImage.material.SetFloat("_StrongTintFade", 1);
        memoryProcessImage.material.SetFloat("_GlitchFade", 0);
    }

    private void SearchMemory()
    {
        if (isProcessing)
        {
            Debug.LogWarning("Already processing a memory. Please wait until the process is finished.");
            return;
        }

        memoryProcessImage.gameObject.SetActive(true); // 重新激活 memoryProcessImage
        memoryFinishedImage.gameObject.SetActive(false); // 禁用 memoryFinishedImage

        isProcessing = true; // 开始处理，设置为true
        // 确保当前索引有效
        if (currentMemoryIndex >= memorySprites.Count)
        {
            Debug.LogWarning("Memory index is out of range.");
            return;
        }

        Sequence memorySequence = DOTween.Sequence();

        // 修改memoryprocess的参数
        memorySequence.Append(memoryProcessImage.material.DOFloat(0, "_StrongTintFade", 1));
        memorySequence.Join(memoryProcessImage.material.DOFloat(1, "_GlitchFade", 1));

        memorySequence.AppendInterval(4f); // 添加4秒的延时

        // 记忆处理结束，激活memoryFinishedImage
        memorySequence.AppendCallback(() =>
        {
            memoryProcessImage.gameObject.SetActive(false);
            memoryFinishedImage.gameObject.SetActive(true);
            memoryFinishedImage.sprite = memorySprites[currentMemoryIndex]; // 设置记忆处理结束的图片
        });

        // 序列完成时更新索引
        memorySequence.OnComplete(() =>
        {
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

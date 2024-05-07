using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // 引入 DOTween 命名空间

public class Drag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 ropeInitialPosition;  // The initial position of the rope
    private const float ropeUpperLimit = 4f;
    private const float ropeLowerLimit = -4.27f;
    private Vector3 ropeFinalPos=new Vector3(7.67f,4f,0);
    private bool isAtBottom = false;

    void Start()
    {
        ropeInitialPosition = transform.position;  // 记录绳子的初始位置
    }

    void OnMouseDown()
    {
        if(!isAtBottom)
            isDragging = true;
            AudioManager.Instance.PlayLoopAudio("curtain_pull");
    }

    void OnMouseUp()
    {
        isDragging = false;
        //AudioManager.Instance.PauseLoopAudio(); 
        
        // 如果绳子的位置没有到达最下面的边界，就使绳子回到开始的位置
        if (transform.position.y > ropeLowerLimit + 2f)
        {
            AudioManager.Instance.PlayLoopAudio("curtain_back");
            transform.DOMove(ropeFinalPos, 0.5f);  // 0.5f 是动画的持续时间，你可以根据需要调整
        }
    }

    void Update()
    {
        // Drag the rope
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float newY = Mathf.Clamp(mousePosition.y, ropeLowerLimit, ropeUpperLimit);

            if (newY <= ropeLowerLimit + 2f)
            {
                AudioManager.Instance.PauseLoopAudio(); 
                transform.DOMove(new Vector3(transform.position.x, ropeLowerLimit, transform.position.z), 0.5f);
                isDragging = false;
                isAtBottom = true;
                EndDay();
            }

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public void EndDay()
    {
        DialogUIController.Instance.Hide();
        GameManager.Instance.NewDay();
    }
}

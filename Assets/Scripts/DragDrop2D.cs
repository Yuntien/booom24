using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class DragDrop2D : MonoBehaviour
{
    Vector3 offset;
    public Transform startPos;
    Collider2D collider2d;
    public string destinationTag = "DropArea";
    CheckPort checkport=null;
    private float lastDragTime = 0f;

    // The cooldown time in seconds between two dragging operations
    private float dragCooldown = 0.1f;

    private SpriteRenderer plugin;

    private bool isInRepairMode=false;
    [HideInInspector]
    public bool isDragging = false;
    float radius;
    //public GameObject plug; // The plug object

    public void DisableDrag()
    {
        isInRepairMode=true;
        transform.position = startPos.position;

    }

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        plugin=GetComponent<SpriteRenderer>();
        plugin.enabled=false;
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        radius = circleCollider.radius;
        //plug.SetActive(false);
    }

    void Start()
    {
        transform.position = startPos.position;
    }

      void OnMouseDown()
    {
        if(ConversationController.Instance.IsTalking())
    {
        return;
    }
        if(isInRepairMode)
        return;
        if (Time.time < lastDragTime + dragCooldown)
        {
            return;
        }



        if (checkport && checkport.isChecking)
        {
            // If the port is currently checking, don't allow dragging
            return;
        }

        offset = transform.position - MouseWorldPosition();

        // Disconnect the power line when starting to drag
        if (checkport)
    {
        Debug.Log("Disconnecting checkport...");
        checkport.Disconnect();
        checkport=null;
        Debug.Log("Checkport disconnected and cleared.");
    }
     //plug.SetActive(true); 
     plugin.enabled=true;
     isDragging = true;
        lastDragTime = Time.time;
    }

    void OnMouseDrag()
    {
    if(ConversationController.Instance.IsTalking())
    {
        return;
    }
    
        if(isInRepairMode)
        return;
        if (checkport && checkport.isChecking)
        {
            // If the port is currently checking, don't allow dragging
            return;
        }

        transform.position = MouseWorldPosition() + offset;
    }

void OnMouseUp()
{
    if(ConversationController.Instance.IsTalking())
    {
        return;
    }
    
    if(isInRepairMode)
        return;
    collider2d.enabled = false;

    int layerMask = LayerMask.GetMask("Checkport");
    Collider2D hitCollider = Physics2D.OverlapCircle(MouseWorldPosition()+new Vector3(-0.3f,0.3f,0), 0.3f, layerMask);

    if (hitCollider != null)
    {
        Debug.Log("鼠标抬起来发现了" + hitCollider.transform.name);
        CheckPort newCheckPort = hitCollider.transform.GetComponent<CheckPort>();
        if (newCheckPort != null && newCheckPort.isCheckable)
        {
            // If the power line is dropped on a checkable drop area,
            // connect it and update the checkport reference.
            transform.position = hitCollider.transform.position + new Vector3(0, 0, -0.01f);
            checkport = newCheckPort;
            checkport.Connect();
        }
        else
        {
            // If the power line is dropped on a non-checkable area,
            // move it back to the start position.
            // But if the old checkport is checking, don't move it.
            if (checkport == null || !checkport.isChecking)
                transform.DOMove(startPos.position, 0.15f);
        }
    }
    else
    {
        // If the power line is dropped on a non-checkable area,
        // move it back to the start position.
        // But if the old checkport is checking, don't move it.
        if (checkport == null || !checkport.isChecking)
            transform.DOMove(startPos.position, 0.15f);
    }
    //plug.SetActive(false);
    plugin.enabled=false;
    isDragging = false;
    collider2d.enabled = true;
}

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}

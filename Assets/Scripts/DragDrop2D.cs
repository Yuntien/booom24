using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    void Start()
    {
        transform.position = startPos.position;
    }

      void OnMouseDown()
    {
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
        lastDragTime = Time.time;
    }

    void OnMouseDrag()
    {
        if (checkport && checkport.isChecking)
        {
            // If the port is currently checking, don't allow dragging
            return;
        }

        transform.position = MouseWorldPosition() + offset;
    }

   void OnMouseUp()
{
    collider2d.enabled = false;

    int layerMask = LayerMask.GetMask("Checkport");
    Collider2D hitCollider = Physics2D.OverlapCircle(MouseWorldPosition(), 0.1f, layerMask);

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

    collider2d.enabled = true;
}

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}

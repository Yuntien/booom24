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
    Module module;

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
        offset = transform.position - MouseWorldPosition();

        // Disconnect the power line when starting to drag
        if (module)
        {
            module.TurnOffConnections();
        }
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        collider2d.enabled = false;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit2D hitInfo;
        if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if (hitInfo.transform.tag == destinationTag)
            {
                transform.position = hitInfo.transform.position + new Vector3(0, 0, -0.01f);
                module = hitInfo.transform.parent.GetComponent<Module>();
                if (module != null)
                {
                    // Connect the power line when dropped on a drop area
                    module.HighlightConnections();
                }
            }
            else
            {
                transform.DOMove(startPos.position, 0.15f);
            }
        }
        else
        {
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

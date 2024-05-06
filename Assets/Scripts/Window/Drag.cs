using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool isDragging = false;
    public Transform curtain;  // Your curtain object
    public Vector3 curtainFinalPosition;  // The final position of the curtain

    private Vector3 curtainInitialPosition;  // The initial position of the curtain
    private Vector3 ropeInitialPosition;  // The initial position of the rope

    private const float ropeUpperLimit = 3.65f;
    private const float ropeLowerLimit = -2.28f;

    void Start()
    {
        curtainInitialPosition = curtain.position;
        ropeInitialPosition = transform.position;
    }

    void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over the rope
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                isDragging = true;
            }
        }
        // Check if the left mouse button was released
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Drag the rope
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float newY = Mathf.Clamp(mousePosition.y, ropeLowerLimit, ropeUpperLimit);

            // If the rope is near the lower limit, snap it to the limit
            if (Mathf.Abs(newY - ropeLowerLimit) < 0.1f)
            {
                
                newY = ropeLowerLimit;
                isDragging = false;  // Stop dragging once the rope has reached the lower limit
                EndDay();
            }

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Update the curtain's position based on the rope's position
            float t = (newY - ropeInitialPosition.y) / (ropeLowerLimit - ropeInitialPosition.y);
            curtain.position = Vector3.Lerp(curtainInitialPosition, curtainFinalPosition, t);
        }
    }
    public void EndDay()
    {

    }
}

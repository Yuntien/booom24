using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FollowMouse : MonoBehaviour
{
    private Vector2 offset;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
    float xOffset = sr.bounds.size.x * 0.1f / 2;  // 10% of the sprite's width
    float yOffset = sr.bounds.size.y * 0.1f / 2;  // 10% of the sprite's height
    offset = new Vector2(-sr.bounds.size.x / 2 + xOffset, sr.bounds.size.y / 2 - yOffset);

    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector2)mousePosition + offset;
    }
}

using UnityEngine;

public class ArrowRotate : MonoBehaviour
{
    public SpriteRenderer ArrowSpriteHolder;

    void FixedUpdate()
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }
}

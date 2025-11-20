using UnityEngine;

public class Sc_CharaFacing : MonoBehaviour
{
    SpriteRenderer SpriteRenderer;
    public float LastXpost;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (LastXpost > transform.position.x) SpriteRenderer.flipX = true;
        else if (LastXpost < transform.position.x) SpriteRenderer.flipX = false;
        LastXpost = transform.position.x;
    }
}

using UnityEngine;

public class LineMouse : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void OnEnable()
    {
        SetLine();
    }

    void Update()
    {
        SetLine();
    }

    void SetLine()
    {
        lineRenderer.SetPosition(0, lineRenderer.transform.position);

        var MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseWorldPos.z = 0f;
        lineRenderer.SetPosition(1, MouseWorldPos);
    }
}

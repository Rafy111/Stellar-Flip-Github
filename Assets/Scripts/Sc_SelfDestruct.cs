using UnityEngine;

public class Sc_SelfDestruct : MonoBehaviour
{
    public float Time;

    void Start()
    {
        Destroy(gameObject, Time);
    }
}

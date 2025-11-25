using System;
using UnityEngine;

public class EnemyHitEffect : MonoBehaviour
{
    [Header("Weak or Strong")]
    public GameObject Weak;
    public GameObject Normal;
    public GameObject Strong;

    void OnEnable()
    {
        float RandomXpos = UnityEngine.Random.Range(5, 20);
        RandomXpos *= UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(RandomXpos, 175));
        bool IsShortEffect = Convert.ToBoolean(PlayerPrefs.GetInt("EnemyHitEffect"));
        Destroy(gameObject, IsShortEffect ? 1.25f : 10);
    }
}

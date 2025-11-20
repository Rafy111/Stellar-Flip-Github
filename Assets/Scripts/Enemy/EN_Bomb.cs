using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EN_Bomb : MonoBehaviour
{
    [Header("Components")]
    public float FuseDuration;
    public Vector3 AreaSize;
    public GameObject FuseIndicator;
    public GameObject AreaIndicator;
    public GameObject ExplosionPrefab;
    public AudioClip Sfx_Explosion;

    // Other
    Transform FuseTransform;
    AudioSource SoundManager;

    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        FuseTransform = FuseIndicator.transform;
        StartCoroutine(BombFuse());
    }

    public IEnumerator BombFuse()
    {
        float counter = 0;

        Vector3 startScaleSize = FuseTransform.localScale;

        while (counter < FuseDuration)
        {
            counter += Time.deltaTime;
            FuseTransform.localScale = Vector3.Lerp(startScaleSize, AreaSize, counter / FuseDuration);
            yield return null;
        }

        Destroy(FuseIndicator);
        Destroy(AreaIndicator);
        GetComponent<Renderer>().material.color = Color.red;

        SoundManager.PlayOneShot(Sfx_Explosion);
        Instantiate(ExplosionPrefab, FuseTransform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

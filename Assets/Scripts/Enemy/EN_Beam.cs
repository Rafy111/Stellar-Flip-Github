using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EN_Beam : MonoBehaviour
{
    [Header("Components")]
    public float FuseDuration;
    public float BeamStayDuration;
    public float BeamAnimDuration;
    public Vector3 FullSize;

    [Header("Sound Effects")]
    public AudioClip Sfx_Ready;

    [Header("Damage")]
    public int Damage;
    public string Type;

    // Other
    AudioSource SoundManager;
    BoxCollider2D Collider;


    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        Collider = GetComponent<BoxCollider2D>();
        StartCoroutine(BeamReady());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            collider.gameObject.GetComponent<Sc_Chara>().Damaged(Damage, Type);
        }
    }

    public IEnumerator BeamReady()
    {
        yield return new WaitForSeconds(FuseDuration);
        SoundManager.PlayOneShot(Sfx_Ready);

        float counter = 0;
        Vector3 startScaleSize = transform.localScale;
        while (counter < BeamAnimDuration)
        {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScaleSize, FullSize, counter / BeamAnimDuration);
            yield return null;
        }
        transform.localScale = FullSize;

        Collider.enabled = true;
        yield return new WaitForSeconds(BeamStayDuration);
        Collider.enabled = false;

        counter = 0;
        startScaleSize = transform.localScale;
        while (counter < BeamAnimDuration)
        {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScaleSize, new Vector3(0, startScaleSize.y, startScaleSize.z), counter / BeamAnimDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}

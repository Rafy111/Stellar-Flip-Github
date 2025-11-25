using System.Collections;
using UnityEngine;

public class EN_Hand : MonoBehaviour
{
    public float DelayBeforeStart;
    public float FlashDelay;
    public float FlashDuration;
    public int FlashAmmount;
    public GameObject FlashObj;
    public AudioClip Sfx_Flash;

    //Others
    Collider2D Collider;
    AudioSource SoundManager;

    void Start()
    {
        Collider = GetComponent<Collider2D>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        yield return new WaitForSeconds(DelayBeforeStart);
        for (int i = 0; i < FlashAmmount; i++)
        {
            yield return new WaitForSeconds(FlashDelay);
            SoundManager.PlayOneShot(Sfx_Flash);
            FlashObj.SetActive(true);
            yield return new WaitForSeconds(FlashDuration);
            FlashObj.SetActive(false);
        }
        Debug.Log("Swoop!");
    }
}

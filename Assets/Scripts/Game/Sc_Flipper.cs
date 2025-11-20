using UnityEngine;

public class Sc_Flipper : MonoBehaviour
{
    //Others
    GameManager GameManager;
    AudioSource SoundManager;

    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("CommonData").GetComponent<GameManager>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) GameManager.Combo_Reset(collision.gameObject);
    }
}

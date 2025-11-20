using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip SoundToPlay;
    AudioSource SoundManager;

    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
    }

    public void PlayTheSound()
    {
        SoundManager.PlayOneShot(SoundToPlay);
    }

    public void PlaySoundFree(AudioClip Clip)
    {
        SoundManager.PlayOneShot(Clip);
    }
}

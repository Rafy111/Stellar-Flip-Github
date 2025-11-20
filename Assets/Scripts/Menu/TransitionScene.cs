using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    [Header("Main")]
    public GameObject Canvas;
    public GameObject CurrentUi;

    [Header("Time")]
    public float ChangeSpeed;

    [Header("Play SFX When Changing UI")]
    public AudioClip Sfx_ToPlay;

    //Others
    AudioSource MusicManager;
    AudioSource SoundManager;
    float alpha = 1;


    void Start()
    {
        Canvas.GetComponent<CanvasGroup>().alpha = 1;
        MusicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();

        StartCoroutine(FadeIn());
    }


    // Change Scene
    public void StartFadeIn() { StartCoroutine(FadeIn()); }

    public void StartFadeOut(string SceneName = "")
    {
        if (SceneName != "") StartCoroutine(FadeOut(true, SceneName));
        else StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        while (alpha > 0)
        {
            float WaitTime = Time.deltaTime;
            yield return new WaitForSeconds(WaitTime);
            alpha -= ChangeSpeed * WaitTime;
            Canvas.GetComponent<CanvasGroup>().alpha = alpha;
        }
        Canvas.SetActive(false);
        yield return null;
    }

    IEnumerator FadeOut(bool ChangeScene = false, string SceneName = "")
    {
        Canvas.SetActive(true);
        while (alpha < 1)
        {
            float WaitTime = Time.deltaTime;
            yield return new WaitForSeconds(WaitTime);
            alpha += ChangeSpeed * WaitTime;
            Canvas.GetComponent<CanvasGroup>().alpha = alpha;
        }
        if (ChangeScene)
        {
            if (SceneName == "Quit") Application.Quit();
            else SceneManager.LoadScene(SceneName);
        }
        yield return null;
    }


    // Music Fade
    public void StartMusicFadeout(bool DestroyMusic = false) { StartCoroutine(MusicFadeout(DestroyMusic)); }

    IEnumerator MusicFadeout(bool DestroyMusic = false)
    {
        float musvol = MusicManager.volume;
        float musvoltime = 1f;

        while (musvoltime > 0)
        {
            float WaitTime = Time.deltaTime;
            yield return new WaitForSeconds(WaitTime);
            musvoltime -= ChangeSpeed * WaitTime;
            if (musvoltime < 0) musvoltime = 0;
            MusicManager.volume = musvol * musvoltime;
            SoundManager.volume = musvol * musvoltime;
        }

        if (DestroyMusic) Destroy(GameObject.FindGameObjectWithTag("MusicKeepPlay"));
        yield return null;
    }



    // Change UI
    public void StartChangeUi(GameObject NewUi)
    {
        StartCoroutine(ChangingUi(NewUi));
    }

    public void StartChangeUiWithSound(GameObject NewUi)
    {
        StartCoroutine(ChangingUi(NewUi, true));
    }

    IEnumerator ChangingUi(GameObject NewUi, bool PlaySfx = false)
    {
        // Fade Out
        Canvas.SetActive(true);
        while (alpha < 1)
        {
            float WaitTime = Time.deltaTime;
            yield return new WaitForSeconds(WaitTime);
            alpha += ChangeSpeed * WaitTime;
            Canvas.GetComponent<CanvasGroup>().alpha = alpha;
        }

        // Change UI
        if (PlaySfx && Sfx_ToPlay != null) SoundManager.PlayOneShot(Sfx_ToPlay);
        if (CurrentUi != null) CurrentUi.SetActive(false);
        CurrentUi = NewUi;
        CurrentUi.SetActive(true);

        // Fade In
        while (alpha > 0)
        {
            float WaitTime = Time.deltaTime;
            yield return new WaitForSeconds(WaitTime);
            alpha -= ChangeSpeed * WaitTime;
            Canvas.GetComponent<CanvasGroup>().alpha = alpha;
        }
        Canvas.SetActive(false);
        yield return null;
    }
}

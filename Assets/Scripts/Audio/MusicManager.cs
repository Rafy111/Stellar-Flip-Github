using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] TMP_Text Percent;

    void Start()
    {
        // Music Volume
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadMusicVolume();
        }
        else
        {
            LoadMusicVolume();
        }
    }


    // Music Volume
    public void ChangeMusicVolume()
    {
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>().volume = Slider.value * PlayerPrefs.GetFloat("masterVolume");
        SaveMusicVolume();
    }

    private void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", Slider.value);
        SetPercentage();
    }

    private void LoadMusicVolume()
    {
        Slider.value = PlayerPrefs.GetFloat("musicVolume");
        SetPercentage();
    }

    void SetPercentage()
    {
        float Percentage = Slider.value * 100;
        Percent.text = (int)Percentage + "%";
    }
}

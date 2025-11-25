using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] TMP_Text Percent;

    void Start()
    {
        // Sound Volume
        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
            LoadSoundVolume();
        }
        else
        {
            LoadSoundVolume();
        }
    }

    // Sound Volume
    public void ChangeSoundVolume()
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().volume = Slider.value * PlayerPrefs.GetFloat("masterVolume");
        SaveSoundVolume();
    }

    private void SaveSoundVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", Slider.value);
        SetPercentage();
    }

    private void LoadSoundVolume()
    {
        Slider.value = PlayerPrefs.GetFloat("soundVolume");
        SetPercentage();
    }

    void SetPercentage()
    {
        float Percentage = Slider.value * 100;
        Percent.text = (int)Percentage + "%";
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeManager : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] TMP_Text Percent;

    void Start()
    {
        // Master Volume
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1);
            LoadMasterVolume();
        }
        else
        {
            LoadMasterVolume();
        }
    }

    // Master Volume
    public void ChangeMasterVolume()
    {
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVolume") * Slider.value;
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("soundVolume") * Slider.value;
        SaveMasterVolume();
    }

    private void SaveMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", Slider.value);
        SetPercentage();
    }

    private void LoadMasterVolume()
    {
        Slider.value = PlayerPrefs.GetFloat("masterVolume");
        SetPercentage();
    }

    void SetPercentage()
    {
        float Percentage = Slider.value * 100;
        Percent.text = (int)Percentage + "%";
    }
}

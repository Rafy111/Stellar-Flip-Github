using System;
using UnityEngine;
using UnityEngine.UI;

public class Opt_General : MonoBehaviour
{
    [Header("Buttons")]
    public Toggle Button_UiKeybinds;
    public Toggle Button_UiElements;
    public Toggle Button_EnemyHitEffect;

    void Start()
    {
        if (!PlayerPrefs.HasKey("UiKeybinds"))
        {
            PlayerPrefs.SetInt("UiKeybinds", 1);
            PlayerPrefs.SetInt("UiElements", 1);
        }
        if (!PlayerPrefs.HasKey("EnemyHitEffect")) PlayerPrefs.SetInt("EnemyHitEffect", 1);

        Button_UiKeybinds.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("UiKeybinds"));
        Button_UiElements.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("UiElements"));
        Button_EnemyHitEffect.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("EnemyHitEffect"));
    }

    public void Set_UiKeybinds() { PlayerPrefs.SetInt("UiKeybinds", Convert.ToInt32(Button_UiKeybinds.isOn)); }
    public void Set_UiElements() { PlayerPrefs.SetInt("UiElements", Convert.ToInt32(Button_UiElements.isOn)); }
    public void Set_EnemyHitEffect() { PlayerPrefs.SetInt("EnemyHitEffect", Convert.ToInt32(Button_EnemyHitEffect.isOn)); }
}

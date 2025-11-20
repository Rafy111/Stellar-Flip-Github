using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLevel : MonoBehaviour
{
    [Header("List")]
    public List<Button> List_Levels;
    public List<TMP_Text> List_Highscore;


    void Start()
    {
        if (!PlayerPrefs.HasKey("LastUnlockedLevel")) PlayerPrefs.SetInt("LastUnlockedLevel", 1);
        SetLevelInfo();
    }

    public void SetLevelId(int WhatLevel)
    {
        PlayerPrefs.SetInt("LevelId", WhatLevel);
    }

    public void SetLevelInfo()
    {
        int UnlockedId = PlayerPrefs.GetInt("LastUnlockedLevel");

        for (int i = 1; i <= List_Levels.Count; i++)
        {
            if (i <= UnlockedId)
            {
                List_Levels[i - 1].interactable = true;
                if (PlayerPrefs.GetInt("HighscoreLv" + i) > 0)
                {
                    TMP_Text CurrentHighscore = List_Highscore[i - 1];
                    CurrentHighscore.gameObject.SetActive(true);
                    CurrentHighscore.text = PlayerPrefs.GetInt("HighscoreLv" + i).ToString("D6");
                }
            }
        }
    }
}

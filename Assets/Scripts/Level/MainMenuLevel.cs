using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLevel : MonoBehaviour
{
    [Header("Component")]
    public int WorldId;
    public bool Progressive = true;

    [Header("List")]
    public List<Button> List_Levels;
    public List<TMP_Text> List_Highscore;


    void Start()
    {
        if (Progressive && !PlayerPrefs.HasKey(WorldId + "_LastUnlockedLevel")) PlayerPrefs.SetInt(WorldId + "_LastUnlockedLevel", 1);
        SetLevelInfo();
    }

    public void SetLevelId(int WhatLevel)
    {
        PlayerPrefs.SetInt("LevelId", WhatLevel);
    }

    public void SetLevelInfo()
    {
        int UnlockedId = PlayerPrefs.GetInt(WorldId + "_LastUnlockedLevel");

        for (int i = 1; i <= List_Levels.Count; i++)
        {
            if (Progressive)
            {
                if (i <= UnlockedId)
                {
                    List_Levels[i - 1].interactable = true;
                    if (PlayerPrefs.GetInt(WorldId + "_HighscoreLv" + i) > 0)
                    {
                        TMP_Text CurrentHighscore = List_Highscore[i - 1];
                        CurrentHighscore.gameObject.SetActive(true);
                        CurrentHighscore.text = PlayerPrefs.GetInt(WorldId + "_HighscoreLv" + i).ToString("D6");
                    }
                }
            }
            else
            {
                if (PlayerPrefs.GetInt(WorldId + "_HighscoreLv" + i) > 0)
                {
                    TMP_Text CurrentHighscore = List_Highscore[i - 1];
                    CurrentHighscore.gameObject.SetActive(true);
                    CurrentHighscore.text = PlayerPrefs.GetInt(WorldId + "_HighscoreLv" + i).ToString("D6");
                }
            }
        }
    }
}

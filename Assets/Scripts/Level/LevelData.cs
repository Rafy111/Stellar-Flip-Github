using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Level Stuff")]
    public TMP_Text Text_Level;
    public List<int> EnemyAmmountEachWave;
    public List<GameObject> WaveObject;

    //Others
    GameManager GameManager;

    void Start()
    {
        GameManager = gameObject.GetComponent<GameManager>();

        int CurrentLevelId = PlayerPrefs.GetInt("LevelId");
        Text_Level.text = "Level " + CurrentLevelId.ToString("D2");
        WaveObject[CurrentLevelId - 1].SetActive(true);
        GameManager.SetLevelInfo(EnemyAmmountEachWave[CurrentLevelId - 1]);
    }
}
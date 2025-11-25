using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text Text_Level;
    public TMP_Text Text_Wave;
    public TMP_Text Text_EnemiesLeft;

    [Header("Level Stuff")]
    public List<WorldLevelsSc> WorldLevelsSc;
    public float SpawnDelay = 0.25f;

    //Other
    GameManager GameManager;
    LevelData CurrentLevelData;

    //Datas
    int CurrentWave = 0;
    int Waves;
    List<int> List_EnemyAmmount;
    List<GameObject> List_Enemies;
    int EnemiesAmmount;

    //LevelDatas
    List<GameObject> List_Levels;
    List<LevelData> List_LevelData;


    void Start()
    {
        GameManager = gameObject.GetComponent<GameManager>();

        int SelectedWorld = PlayerPrefs.GetInt("WorldId") - 1;
        List_Levels = new List<GameObject>(WorldLevelsSc[SelectedWorld].List_Levels);
        List_LevelData = new List<LevelData>(WorldLevelsSc[SelectedWorld].List_LevelData);

        int CurrentLevelId = PlayerPrefs.GetInt("LevelId");
        Text_Level.text = "Level " + CurrentLevelId.ToString("D2");

        CurrentLevelData = List_LevelData[CurrentLevelId - 1];
        List_EnemyAmmount = new List<int>(CurrentLevelData.EnemyAmmountEachWave);
        List_Enemies = new List<GameObject>(CurrentLevelData.EnemiesList);

        List_Levels[CurrentLevelId - 1].SetActive(true);
        SetWaveText();
        StartCoroutine(SetWave());
    }

    IEnumerator SetWave()
    {
        int CurrentEnemyCount = List_EnemyAmmount[CurrentWave];
        SetLevelInfo(CurrentEnemyCount);

        for (int i = 0; i < CurrentEnemyCount; i++)
        {
            yield return new WaitForSeconds(SpawnDelay);
            List_Enemies[0].SetActive(true);
            List_Enemies.RemoveAt(0);
        }
    }

    void SetLevelInfo(int EnemiesCount)
    {
        EnemiesAmmount = EnemiesCount;
        Text_EnemiesLeft.text = "Enemies Left: " + EnemiesAmmount.ToString();
    }

    public void EnemyDefeated()
    {
        EnemiesAmmount--;
        Text_EnemiesLeft.text = "Enemies Left: " + EnemiesAmmount.ToString();
        if (EnemiesAmmount <= 0)
        {
            if (CurrentWave < List_EnemyAmmount.Count - 1)
            {
                CurrentWave++;
                SetWaveText();
                StartCoroutine(SetWave());
            }
            else GameManager.StartShowingGameOverScreen(true);
        }
    }

    void SetWaveText()
    {
        Text_Wave.text = "Wave: " + (CurrentWave + 1) + "<size=-12>/" + List_EnemyAmmount.Count;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Combo")]
    public TMP_Text Text_Combo;

    [Header("Score Text")]
    public TMP_Text Text_Score;
    public TMP_Text Text_TempScore;
    public TMP_Text Text_ComboMultiplier;

    [Header("Chaos")]
    public Image ChaosBar;
    public int CountUntilChaos;
    public float ChaosDuration;
    public bool IsChaos;
    public List<GameObject> List_BoostPad;
    public AudioClip Sfx_ChaosStart;

    [Header("Flipper")]
    public float FlipperForce;
    public AudioClip Sfx_FlipSound;
    public AudioClip Sfx_PowerFlip;

    [Header("Objects")]
    public Rigidbody2D LeftFlipper;
    public Rigidbody2D RightFlipper;

    [Header("Musics")]
    public AudioClip Mus_Gameplay;
    public AudioClip Mus_GameplayChaos;

    [Header("Level Stuff")]
    public TMP_Text Text_EnemiesLeft;

    [Header("GameOverScreen")]
    public GameObject GameOverScreen;
    public TMP_Text Text_GameOverCondition;
    public TMP_Text Text_GameOverScore;
    public GameObject NextLevel;
    public float GameOverTransitionTime;


    //Others
    AudioSource MusicManager;
    AudioSource SoundManager;
    PlayerManager PlayerManager;
    SkillProcessor SkillProcessor;
    int CurrentTimeSample;
    int Combo;
    int Score;
    int TempScore;
    int ComboMultiplier = 1;
    int MultiCounter = 10;
    int ChaosCounter;
    int EnemiesAmmount;

    // Game Over Screen
    bool AlreadyGameOver;
    bool GameOverShowingUpdate;
    float GameOverCharger;


    void Start()
    {
        MusicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        PlayerManager = GetComponent<PlayerManager>();
        SkillProcessor = GetComponent<SkillProcessor>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SoundManager.PlayOneShot(Sfx_FlipSound);
            LeftFlipper.AddTorque(FlipperForce);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SoundManager.PlayOneShot(Sfx_FlipSound);
            RightFlipper.AddTorque(-FlipperForce);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !AlreadyGameOver)
        {
            AlreadyGameOver = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TransitionScene>().StartMusicFadeout();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TransitionScene>().StartFadeOut("MainMenu");
        }

        if (GameOverShowingUpdate && GameOverCharger < GameOverTransitionTime - 0.2f)
        {
            GameOverCharger += Time.deltaTime;
            Time.timeScale = 1 - (GameOverCharger/GameOverTransitionTime);
            if (GameOverCharger >= GameOverTransitionTime - 0.25f)
            {
                GameOverShowingUpdate = false;
                Time.timeScale = 0;
                GameOverScreen.SetActive(true);
            }
        }
    }

    public void Combo_Add(int Ammount)
    {
        if (SkillProcessor.DoubleHit) Ammount *= 2;
        Combo += Ammount;
        Text_Combo.text = Combo.ToString("D3");
        CountMultiplier();
    }

    public void Combo_Reset(GameObject Player)
    {
        if (Combo >= 10)
        {
            SoundManager.PlayOneShot(Sfx_PowerFlip);
            PlayerManager.SelectedChara.GetComponent<Sc_Chara>().IsPowerFlip = true;
            if (!IsChaos) AddChaosCount();
        }

        Score += TempScore * ComboMultiplier;
        Text_Score.text = Score.ToString("D6");

        TempScore = 0;
        Text_TempScore.text = TempScore.ToString("D5");

        Combo = 0;
        Text_Combo.text = Combo.ToString("D3");

        MultiCounter = 10;

        ComboMultiplier = 1;
        Text_ComboMultiplier.text = "x" + ComboMultiplier.ToString();
    }

    public void AddTempScore(int Ammount)
    {
        TempScore += Ammount;
        Text_TempScore.text = TempScore.ToString("D5");
    }

    public void CountMultiplier()
    {
        if (Combo >= MultiCounter)
        {
            ComboMultiplier++;
            Text_ComboMultiplier.text = "x" + ComboMultiplier.ToString();
            MultiCounter += 10;
        }
    }

    public void AddChaosCountFromSkill(int Ammount)
    {
        float ToAddChaos =  CountUntilChaos * (float)Ammount / 100;
        ChaosCounter += Mathf.RoundToInt(ToAddChaos);
        if (ChaosCounter > CountUntilChaos) ChaosCounter = CountUntilChaos;
        ChaosBar.fillAmount = (float)ChaosCounter / CountUntilChaos;
        ChaosProcess();
    }

    public void AddChaosCount()
    {
        ChaosCounter++;
        ChaosBar.fillAmount = (float)ChaosCounter / CountUntilChaos;
        ChaosProcess();
    }

    void ChaosProcess()
    {
        if (ChaosCounter >= CountUntilChaos)
        {
            IsChaos = true;
            SoundManager.PlayOneShot(Sfx_ChaosStart);

            CurrentTimeSample = MusicManager.timeSamples;
            MusicManager.Stop();
            MusicManager.clip = Mus_GameplayChaos;
            MusicManager.timeSamples = CurrentTimeSample;
            MusicManager.Play();

            foreach (var BoostPad in List_BoostPad) BoostPad.SetActive(true);
            StartCoroutine(ChaosIsChaosing());
            PlayerManager.Chaos();
        }
    }

    IEnumerator ChaosIsChaosing()
    {
        float t = 0;
        while (t < ChaosDuration)
        {
            t += Time.deltaTime;
            ChaosBar.fillAmount = (ChaosDuration - t) / ChaosDuration;
            yield return null;
        }
        IsChaos = false;

        CurrentTimeSample = MusicManager.timeSamples;
        MusicManager.Stop();
        MusicManager.clip = Mus_Gameplay;
        MusicManager.timeSamples = CurrentTimeSample;
        MusicManager.Play();

        foreach (var BoostPad in List_BoostPad) BoostPad.SetActive(false);
        PlayerManager.UnChaos();

        ChaosCounter = 0;
        ChaosBar.fillAmount = 0;
    }

    public void SetLevelInfo(int EnemiesCount)
    {
        EnemiesAmmount = EnemiesCount;
        Text_EnemiesLeft.text = "Enemies Left: " + EnemiesAmmount.ToString();
    }

    public void EnemyDefeated()
    {
        EnemiesAmmount--;
        Text_EnemiesLeft.text = "Enemies Left: " + EnemiesAmmount.ToString();
        if (EnemiesAmmount <= 0) StartShowingGameOverScreen(true);
    }

    public void StartShowingGameOverScreen(bool Win)
    {
        if (AlreadyGameOver) return;
        AlreadyGameOver = true;

        Text_GameOverCondition.text = Win ? "You Win!" : "You Lose...";
        Text_GameOverScore.text = "Score: " + Score.ToString("D6");
        NextLevel.SetActive(Win && PlayerPrefs.GetInt("LevelId") <= 11);
        GameOverShowingUpdate = true;

        if (Win)
        {
            int CurrentLevelId = PlayerPrefs.GetInt("LevelId");
            if (PlayerPrefs.GetInt("HighscoreLv" + CurrentLevelId) < Score) PlayerPrefs.SetInt("HighscoreLv" + CurrentLevelId, Score);
            if (PlayerPrefs.GetInt("LastUnlockedLevel") < CurrentLevelId + 1) PlayerPrefs.SetInt("LastUnlockedLevel", CurrentLevelId + 1);
        }
    }

    public void UnscaleTime()
    {
        Time.timeScale = 1;
    }

    public void SetNextLevel()
    {
        int NextLevel = PlayerPrefs.GetInt("LevelId") + 1;
        PlayerPrefs.SetInt("LevelId", NextLevel);
    }
}

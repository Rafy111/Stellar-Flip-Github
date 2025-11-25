using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Hp Bar")]
    public List<GameObject> List_Available_HpBar;
    public Transform HpBarHolder;

    [Header("Characters")]
    public List<GameObject> List_Available_Chara;
    public Transform CharaHolderCommonData;

    [Header("Prefab")]
    public GameObject Prefab_CharaHolder;
    public GameObject Prefab_CharaChange;

    [Header("Spawn")]
    public Transform SpawnPoint;
    public GameObject ArrowRotate;
    public Animator Anim_Cannon;
    public float LaunchSpeed;

    [Header("Boost")]
    public float BoostSpeed;
    public float BoostCooldown;

    [Header("Sfx")]
    public AudioClip Sfx_LaunchSound;
    public AudioClip Sfx_ChangeChara;
    public AudioClip Sfx_Boost;

    [Header("Hidden Variables")]
    public List<int> List_Selected_Character_Id;
    public List<GameObject> List_Created_HpBars;
    public List<GameObject> List_Selected_Chara;
    public GameObject SelectedChara;

    [Header("Hidden variables For Chaos")]
    public List<GameObject> List_ActiveChara;
    public List<GameObject> List_ActiveBoosterArrow;
    public List<GameObject> List_ActiveBoosterLine;
    public List<GameObject> List_AllCharaHolder;

    //Others
    AudioSource MusicManager;
    AudioSource SoundManager;
    GameManager GameManager;
    bool PlayerReleased;
    bool CanBoost;
    bool IsBoosting;
    int CharaId;
    GameObject SelectIndicator;
    GameObject CurrentCharaHolder;



    void Start()
    {
        MusicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        GameManager = GetComponent<GameManager>();

        //Spawning
        List_Selected_Character_Id = new List<int> { PlayerPrefs.GetInt("Pl1"), PlayerPrefs.GetInt("Pl2"), PlayerPrefs.GetInt("Pl3") };
        foreach(var Id in List_Selected_Character_Id)
        {
            GameObject NewHpBar = Instantiate(List_Available_HpBar[Id], HpBarHolder);
            List_Created_HpBars.Add(NewHpBar);
            NewHpBar.SetActive(true);

            GameObject NewChara = Instantiate(List_Available_Chara[Id], CharaHolderCommonData);
            List_Selected_Chara.Add(NewChara);

            Sc_Chara CharaSc = NewChara.GetComponent<Sc_Chara>();
            HpBarHolder HpBarSc = NewHpBar.GetComponent<HpBarHolder>();
            CharaSc.HpBar = HpBarSc.Hpbar;
            if (CharaSc.Sc_Skill != null) CharaSc.Sc_Skill.BarFill = HpBarSc.SkillBar;
            CharaSc.DeathObj = HpBarSc.Death;
        }
        SelectedChara = List_Selected_Chara[0];

        CurrentCharaHolder = Instantiate(Prefab_CharaHolder, SpawnPoint.position, Quaternion.identity);
        SelectedChara.transform.SetParent(CurrentCharaHolder.transform, false);
        ActivateChara();
        SelectIndicator = CurrentCharaHolder.GetComponent<PlayerPinBall>().Indicator_Selected;
        SelectIndicator.SetActive(true);
        SelectedChara.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.IsPause)
        {
            if (PlayerReleased && CanBoost && !GameManager.AlreadyGameOver)
            {
                IsBoosting = true;
                foreach (var Arrow in List_ActiveBoosterArrow) Arrow.GetComponent<ArrowRotate>().ArrowSpriteHolder.enabled = false;
                foreach (var Line in List_ActiveBoosterLine) Line.SetActive(true);

                Time.timeScale = 0.25f;
                MusicManager.pitch = 0.5f;
            }
            else if (!PlayerReleased)
            {
                SoundManager.PlayOneShot(Sfx_LaunchSound);

                Rigidbody2D PlRb = SelectedChara.transform.parent.GetComponent<Rigidbody2D>();
                PlayerPinBall PlSc = SelectedChara.transform.parent.GetComponent<PlayerPinBall>();

                PlayerReleased = true;
                PlRb.gravityScale = 1;

                Vector2 FinalVector = (ArrowRotate.transform.rotation * Vector3.down).normalized;
                PlRb.AddForce(-FinalVector * LaunchSpeed);

                PlayerReleased = true;
                StartCoolDownBooster();

                Anim_Cannon.SetTrigger("Hide");

                foreach (var Chara in List_Selected_Chara)
                {
                    Chara.GetComponent<Animator>().enabled = true;
                    Chara.GetComponent<Sc_CharaFacing>().enabled = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && !GameManager.IsPause)
        {
            if (IsBoosting && !GameManager.AlreadyGameOver)
            {
                IsBoosting = false;
                foreach (var Arrow in List_ActiveBoosterArrow) Arrow.SetActive(false);
                foreach (var Line in List_ActiveBoosterLine) Line.SetActive(false);

                Time.timeScale = 1.0f;
                MusicManager.pitch = 1.0f;

                SoundManager.PlayOneShot(Sfx_Boost);
                BoostCharacters();
            }
        }
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.LeftShift)) && !GameManager.IsPause)
        {
            if (List_Selected_Chara.Count > 1)
            {
                SoundManager.PlayOneShot(Sfx_ChangeChara);

                if (!GameManager.IsChaos)
                {
                    Instantiate(Prefab_CharaChange, CurrentCharaHolder.GetComponent<PlayerPinBall>().CenterPoint);

                    CharaId++;
                    if (CharaId > List_Selected_Chara.Count - 1) CharaId = 0;

                    SelectedChara.GetComponent<Sc_Chara>().IsPowerFlip = false;
                    SelectedChara.transform.SetParent(CharaHolderCommonData,false);
                    SelectedChara.transform.position = new Vector2(0, 0);
                    SelectedChara.SetActive(false);

                    SelectedChara = List_Selected_Chara[CharaId];
                    SelectedChara.transform.SetParent(CurrentCharaHolder.transform, false);
                    ActivateChara();
                    SelectedChara.SetActive(true);
                }
                else
                {
                    SelectIndicator.SetActive(false);

                    CharaId++;
                    if (CharaId > List_Selected_Chara.Count - 1) CharaId = 0;

                    SelectedChara.GetComponent<Sc_Chara>().IsPowerFlip = false;
                    SelectedChara = List_Selected_Chara[CharaId];
                    ActivateChara();
                    CurrentCharaHolder = SelectedChara.transform.parent.gameObject;

                    SelectIndicator = SelectedChara.transform.parent.GetComponent<PlayerPinBall>().Indicator_Selected;
                    SelectIndicator.SetActive(true);
                }
            }
        }
    }

    void ActivateChara()
    {
        List_ActiveChara.Clear();

        if (!GameManager.IsChaos) List_ActiveChara.Add(SelectedChara);
        else List_ActiveChara = new List<GameObject> (List_Selected_Chara);

        List_ActiveBoosterArrow.Clear();
        List_ActiveBoosterLine.Clear();
        int CharaTempId = 0;
        foreach (var Chara in List_ActiveChara)
        {
            PlayerPinBall PlHolderSc = List_ActiveChara[CharaTempId].transform.parent.GetComponent<PlayerPinBall>();
            List_ActiveBoosterArrow.Add(PlHolderSc.Indicator_Boost);
            List_ActiveBoosterLine.Add(PlHolderSc.Indicator_BoostLine);
            CharaTempId++;
        }
    }

    public void RemoveChar(GameObject CharaToDelete)
    {
        int IdToDelete = List_Selected_Chara.IndexOf(CharaToDelete);
        if (IdToDelete >= 0) List_Selected_Chara.RemoveAt(IdToDelete);

        if (List_Selected_Chara.Count > 0)
        {
            if (!GameManager.IsChaos)
            {
                Destroy(CharaToDelete);

                if (CharaId > List_Selected_Chara.Count - 1) CharaId = 0;

                SelectedChara = List_Selected_Chara[CharaId];
                SelectedChara.transform.SetParent(CurrentCharaHolder.transform, false);
                ActivateChara();
                SelectedChara.SetActive(true);
            }
            else
            {
                GameObject HolderObject = CharaToDelete.transform.parent.gameObject;
                List_AllCharaHolder.RemoveAt(List_AllCharaHolder.IndexOf(HolderObject));
                Destroy(HolderObject);

                if (CharaId > List_Selected_Chara.Count - 1) CharaId = 0;

                SelectedChara = List_Selected_Chara[CharaId];
                ActivateChara();
                CurrentCharaHolder = SelectedChara.transform.parent.gameObject;
                SelectIndicator = SelectedChara.transform.parent.GetComponent<PlayerPinBall>().Indicator_Selected;
                SelectIndicator.SetActive(true);
            }
        }
        else
        {
            Destroy(CurrentCharaHolder);
            GameManager.StartShowingGameOverScreen(false);
        }
    }



    // Booster
    void BoostCharacters()
    {
        int TempCounterBooster = 0;

        foreach (var ActiveChara in List_ActiveChara)
        {
            Rigidbody2D Rb = ActiveChara.transform.parent.GetComponent<Rigidbody2D>();
            GameObject ArrowNow = List_ActiveBoosterArrow[TempCounterBooster];

            Rb.velocity = new Vector2(0, 0);
            Vector2 FinalVector = (ArrowNow.transform.rotation * Vector3.down).normalized;
            FinalVector /= new Vector2(1.5f, 1);
            Rb.AddForce(-FinalVector * BoostSpeed);

            TempCounterBooster++;
        }
        CanBoost = false;
        foreach (var Arrow in List_ActiveBoosterArrow) Arrow.SetActive(false);
        //foreach (var Line in List_ActiveBoosterLine) Line.SetActive(false);
        StartCoroutine(BoostReadyCooldown());
    }

    public void StartCoolDownBooster() { StartCoroutine(BoostReadyCooldown()); }

    IEnumerator BoostReadyCooldown()
    {
        yield return new WaitForSeconds(BoostCooldown);
        if (!GameManager.AlreadyGameOver)
        {
            foreach (var ArrowBooster in List_ActiveBoosterArrow)
            {
                ArrowBooster.GetComponent<ArrowRotate>().ArrowSpriteHolder.enabled = true;
                ArrowBooster.SetActive(true);
            }
        }
        //foreach (var Line in List_ActiveBoosterLine) Line.SetActive(true);
        CanBoost = true;
    }

    public void Chaos()
    {
        Instantiate(Prefab_CharaChange, CurrentCharaHolder.GetComponent<PlayerPinBall>().CenterPoint);

        List_AllCharaHolder.Clear();
        List_AllCharaHolder.Add(CurrentCharaHolder);

        if (List_Selected_Chara.Count > 1)
        {
            int CharaIdCheck = 0;
            foreach (var Character in List_Selected_Chara)
            {
                if (CharaIdCheck != CharaId)
                {
                    GameObject MitosisChara = Instantiate(Prefab_CharaHolder, CurrentCharaHolder.transform.position, Quaternion.identity);
                    List_AllCharaHolder.Add(MitosisChara);
                    List_Selected_Chara[CharaIdCheck].transform.SetParent(MitosisChara.transform, false);
                    List_Selected_Chara[CharaIdCheck].SetActive(true);
                    MitosisChara.GetComponent<Rigidbody2D>().gravityScale = 1;
                    MitosisChara.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
                }
                else CurrentCharaHolder.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
                CharaIdCheck++;
            }
            ActivateChara();
        }

        if (CanBoost && !IsBoosting)
        {
            foreach (var Arrow in List_ActiveBoosterArrow)
            {
                Arrow.GetComponent<ArrowRotate>().ArrowSpriteHolder.enabled = true;
                Arrow.SetActive(true);
            }
        }
        if (IsBoosting) foreach (var Line in List_ActiveBoosterLine) Line.SetActive(true);
    }

    public void UnChaos()
    {
        foreach (var holder in List_AllCharaHolder)
        {
            foreach (var Chara in List_Selected_Chara)
            {
                Chara.transform.SetParent(CharaHolderCommonData, false);
                Chara.transform.position = new Vector2(0, 0);
                Chara.SetActive(false);
            }

            if (holder != CurrentCharaHolder)
            {
                Instantiate(Prefab_CharaChange, holder.GetComponent<PlayerPinBall>().CenterPoint.position, Quaternion.identity);
                Destroy(holder);
            }
            SelectedChara.transform.SetParent(CurrentCharaHolder.transform, false);
            SelectedChara.SetActive(true);
        }
        ActivateChara();
    }

    public void WinTrigger()
    {
        IsBoosting = false;

        Time.timeScale = 1.0f;
        MusicManager.pitch = 1.0f;

        foreach (var Arrow in List_ActiveBoosterArrow) Arrow.SetActive(false);
        foreach (var Line in List_ActiveBoosterLine) Line.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Chara_Skill : MonoBehaviour
{
    [Header("Options")]
    public float CooldownTime;
    public TypeList SkillType;
    public AudioClip Sfx_UseSkill;

    [Header("Skill: Fire")]
    public int DamageReductionPercentage;
    public float DamageReductionDuration;

    [Header("Skill: Water")]
    public float DoubleHitDuration;

    [Header("Skill: Nature")]
    public int HealPercentage;

    [Header("Skill: Thunder")]
    public int FillChaosBarPercentage;

    [Header("Skill: Ice")]
    public int CooldownRestorePercentage;

    [Header("Hidden Components")]
    public Image BarFill;

    //Others
    PlayerManager PlayerManager;
    SkillProcessor SkillProcessor;
    AudioSource SoundManager;
    float CooldownCounter;
    bool IsSkillReady = true;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice }


    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        GameObject CommonData = GameObject.FindGameObjectWithTag("CommonData");
        PlayerManager = CommonData.GetComponent<PlayerManager>();
        SkillProcessor = CommonData.GetComponent<SkillProcessor>();
    }

    void Update()
    {
        if (!IsSkillReady)
        {
            CooldownCounter += Time.deltaTime;
            BarFill.fillAmount = CooldownCounter/CooldownTime;
            if (CooldownCounter >= CooldownTime)
            {
                BarFill.fillAmount = 1;
                IsSkillReady = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && IsSkillReady && PlayerManager.SelectedChara == gameObject) StartSkill();
    }

    void StartSkill()
    {
        CooldownCounter = 0;
        BarFill.fillAmount = 0;
        IsSkillReady = false;

        SoundManager.PlayOneShot(Sfx_UseSkill);

        switch (SkillType.ToString())
        {
            case "Fire": SkillProcessor.StartSkillFire(DamageReductionDuration, DamageReductionPercentage); break;
            case "Water": SkillProcessor.StartSkillWater(DoubleHitDuration); break;
            case "Nature": SkillProcessor.StartSkillNature(HealPercentage); break;
            case "Electric": SkillProcessor.StartSkillElectric(FillChaosBarPercentage); break;
            case "Ice": SkillProcessor.StartSkillIce(CooldownRestorePercentage); break;
        }
    }

    public void CooldownBoost(int PercentageAmmount)
    {
        if (IsSkillReady) return;

        CooldownCounter += CooldownTime * PercentageAmmount / 100;
        if (CooldownCounter > CooldownTime) CooldownCounter = CooldownTime;
        BarFill.fillAmount = CooldownCounter / CooldownTime;
    }
}

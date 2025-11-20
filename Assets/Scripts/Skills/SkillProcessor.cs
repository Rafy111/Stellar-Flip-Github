using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProcessor : MonoBehaviour
{
    //Skills
    public int DamageReduction;
    public bool DoubleHit;

    //Other
    PlayerManager PlayerManager;
    GameManager GameManager;



    private void Start()
    {
        PlayerManager = GetComponent<PlayerManager>();
        GameManager = GetComponent<GameManager>();
    }


    // Skill: Fire
    public void StartSkillFire(float Duration, int Percent) { StartCoroutine(SkillFire(Duration, Percent)); }

    IEnumerator SkillFire(float Duration, int Percent)
    {
        DamageReduction = Percent;
        yield return new WaitForSeconds(Duration);
        DamageReduction = 0;
    }


    // Skill: Nature
    public void StartSkillNature(int Ammount)
    {
        List<GameObject> AllChara = PlayerManager.List_Selected_Chara;
        foreach (var Chara in AllChara) Chara.GetComponent<Sc_Chara>().Heal(Ammount, true);
    }
    

    // Skill: Water
    public void StartSkillWater(float Duration) { StartCoroutine(SkillWater(Duration)); }

    IEnumerator SkillWater(float Duration)
    {
        DoubleHit = true;
        yield return new WaitForSeconds(Duration);
        DoubleHit = false;
    }


    // Skill: Electric
    public void StartSkillElectric(int Ammount)
    {
        GameManager.AddChaosCountFromSkill(Ammount);
    }


    // Skill: Ice
    public void StartSkillIce(int Ammount)
    {
        List<GameObject> AllChara = PlayerManager.List_Selected_Chara;
        foreach (var Chara in AllChara) Chara.GetComponent<Chara_Skill>().CooldownBoost(Ammount);
    }
}

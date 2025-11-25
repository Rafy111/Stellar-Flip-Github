using UnityEngine;
using UnityEngine.UI;

public class Sc_Enem : MonoBehaviour
{
    [Header("HP")]
    public int MaxHp;
    public Slider HpBar;
    public int ScoreHit;
    public int ScoreDeath;

    [Header("ATK")]
    public int Attack;

    [Header("Type")]
    public TypeList Type;

    [Header("Has Scripts")]
    public bool HasShoot;
    public bool HasEliteAttacks;

    [Header("Other")]
    public SpriteRenderer EnemBody;
    public int AddScore;
    public bool CountedTowardsDeath = true;

    [Header("Sound Effects")]
    public AudioClip Sfx_Spawn;
    //public AudioClip Sfx_Hurt;
    public AudioClip Sfx_Dead;

    [Header("Prefabs")]
    public GameObject Obj_Spawn;
    public GameObject HitEffect;

    //Others
    ElementsHolder ElementsHolder;
    GameManager GameManager;
    LevelManager LevelManager;
    AudioSource SoundManager;
    int Hp;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice, None }


    void OnEnable()
    {
        if (SoundManager == null) SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        SoundManager.PlayOneShot(Sfx_Spawn);
        Instantiate(Obj_Spawn, transform.position, Quaternion.identity);
    }

    void Start()
    {
        GameObject CommonData = GameObject.FindGameObjectWithTag("CommonData");
        ElementsHolder = CommonData.GetComponent<ElementsHolder>();
        GameManager = CommonData.GetComponent<GameManager>();
        LevelManager = CommonData.GetComponent<LevelManager>();

        Hp = MaxHp;
        HpBar.maxValue = MaxHp;
        SetHpBarValue();

        Color ColorType = ElementsHolder.GetColor(Type.ToString());
        EnemBody.color = ColorType;
        HpBar.fillRect.GetComponent<Image>().color = ColorType;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Attack > 0) collision.gameObject.GetComponentInChildren<Sc_Chara>().Damaged(Attack, Type.ToString());
        }
    }

    public void Damaged(int Damage, string DamageType)
    {
        float DamageMultiplier = ElementsHolder.WeakOrStrong(Type.ToString(), DamageType);
        float DamageFloat = Damage * DamageMultiplier;
        Hp -= (int)DamageFloat;

        GameObject HitObj = Instantiate(HitEffect, transform.position, Quaternion.identity);
        EnemyHitEffect HitObjSc = HitObj.GetComponent<EnemyHitEffect>();
        switch (DamageMultiplier)
        {
            case 0.5f: HitObjSc.Weak.SetActive(true); break;
            case 1: HitObjSc.Normal.SetActive(true); break;
            case 1.5f: HitObjSc.Strong.SetActive(true); break;
        }

        if (Hp <= 0)
        {
            SoundManager.PlayOneShot(Sfx_Dead);
            float ScoreFloat = ScoreDeath * DamageMultiplier;
            GameManager.AddTempScore((int)ScoreFloat);
            if (CountedTowardsDeath) LevelManager.EnemyDefeated();

            GetComponent<Collider2D>().enabled = false;
            if (HasShoot) GetComponent<Enem_Shoot>().enabled = false;
            if (HasEliteAttacks) GetComponent<Sc_EliteEnemyAttacks>().enabled = false;
            HpBar.gameObject.SetActive(false);

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 175));

            Destroy(gameObject, 10);

            this.enabled = false;
        }
        else
        {
            //SoundManager.PlayOneShot(Sfx_Hurt);
            float ScoreFloat = ScoreHit * DamageMultiplier;
            GameManager.AddTempScore((int)ScoreFloat);
            SetHpBarValue();
        }
    }

    void SetHpBarValue()
    {
        HpBar.value = Hp;
    }

    public void Heal(int Ammount)
    {
        Hp += Ammount;
        if (Hp > MaxHp) Hp = MaxHp;
        SetHpBarValue();
    }
}

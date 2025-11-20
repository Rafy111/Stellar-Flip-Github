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

    [Header("Sound Effects")]
    //public AudioClip Sfx_Hurt;
    public AudioClip Sfx_Dead;

    //Others
    ElementsHolder ElementsHolder;
    GameManager GameManager;
    AudioSource SoundManager;
    int Hp;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice }


    void Start()
    {
        ElementsHolder = GameObject.FindGameObjectWithTag("CommonData").GetComponent<ElementsHolder>();
        GameManager = GameObject.FindGameObjectWithTag("CommonData").GetComponent<GameManager>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();

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
        float DamageFloat = Damage * ElementsHolder.WeakOrStrong(Type.ToString(), DamageType);
        Hp -= (int)DamageFloat;

        if (Hp <= 0)
        {
            SoundManager.PlayOneShot(Sfx_Dead);
            GameManager.AddTempScore(ScoreDeath);
            GameManager.EnemyDefeated();

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
            GameManager.AddTempScore(ScoreHit);
            SetHpBarValue();
        }
    }

    void SetHpBarValue()
    {
        HpBar.value = Hp;
    }
}

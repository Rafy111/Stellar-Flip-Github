using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Chara : MonoBehaviour
{
    [Header("HP")]
    public int MaxHp;
    public int Hp;

    [Header("ATK")]
    public int Attack;
    public TypeList Type;
    public GameObject Prefab_Explosion;

    [Header("Components")]
    public Transform CenterPoint;
    public bool IsPowerFlip;

    [Header("Sound Effects")]
    public AudioClip Sfx_Hurt;
    public AudioClip Sfx_Dead;
    public AudioClip Sfx_PlayerHit;

    [Header("For Skill")]
    public Chara_Skill Sc_Skill;

    [Header("Hidden variables")]
    public Image HpBar;
    public GameObject DeathObj;
    public Animator Animator;

    //Others
    GameManager GameManager;
    PlayerManager PlayerManager;
    ElementsHolder ElementsHolder;
    SkillProcessor SkillProcessor;
    AudioSource SoundManager;
    SpriteRenderer SpriteRenderer;
    Coroutine AttackAnimStay;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice }


    void Start()
    {
        GameObject CommonData = GameObject.FindGameObjectWithTag("CommonData");
        GameManager = CommonData.GetComponent <GameManager>();
        PlayerManager = CommonData.GetComponent<PlayerManager>();
        ElementsHolder = CommonData.GetComponent<ElementsHolder>();
        SkillProcessor = CommonData.GetComponent<SkillProcessor>();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Hp = MaxHp;

        SetHpBarValue();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SoundManager.PlayOneShot(Sfx_PlayerHit);

            Sc_Enem EnemSc = collision.gameObject.GetComponent<Sc_Enem>();
            GameManager.Combo_Add(2 + EnemSc.AddScore);

            int AttackHolder = Attack;
            if (IsPowerFlip)
            {
                IsPowerFlip = false;
                Instantiate(Prefab_Explosion, collision.transform.position, Quaternion.identity);
                AttackHolder *= 2;
                if (GameManager.IsChaos)
                {
                    float DamageToMultiply = AttackHolder;
                    DamageToMultiply *= 1.5f;
                    AttackHolder = (int)DamageToMultiply;
                }
            }
            EnemSc.Damaged(AttackHolder, Type.ToString());

            float HitPos = collision.gameObject.transform.position.y;
            Vector2 CheckForAnim = new Vector2(HitPos + 0.45f, HitPos - 0.45f);
            float CurrentPos = CenterPoint.position.y;
            if (CheckForAnim.x > CurrentPos && CurrentPos > CheckForAnim.y)
            {
                Animator.SetTrigger("Hit");
                if (AttackAnimStay != null) StopCoroutine(AttackAnimStay);
                GetComponent<Sc_CharaFacing>().enabled = false;
                HitPos = collision.gameObject.transform.position.x;
                SpriteRenderer.flipX = HitPos < transform.position.x;
                AttackAnimStay = StartCoroutine(WaitTillAttackAnim());
            }
        }
        else if (collision.gameObject.CompareTag("BouncingObject")) collision.gameObject.GetComponent<Sc_Object>().AddScore();
    }

    public void Damaged(int Damage, string DamageType)
    {
        float DamageFloat = Damage * ElementsHolder.WeakOrStrong(Type.ToString(), DamageType);
        if (SkillProcessor.DamageReduction > 0) DamageFloat -= DamageFloat * SkillProcessor.DamageReduction/100;
        if (SkillProcessor.DoubleHit) DamageFloat *= 2;
        Hp -= Mathf.RoundToInt(DamageFloat);

        if (Hp <= 0)
        {
            SoundManager.PlayOneShot(Sfx_Dead);
            HpBar.fillAmount = 0;
            DeathObj.SetActive(true);
            PlayerManager.RemoveChar(gameObject);
            //Destroy(gameObject);
        }
        else
        {
            SoundManager.PlayOneShot(Sfx_Hurt);
            SetHpBarValue();
        }
    }

    public void Heal(int Ammount, bool Percentage = false)
    {
        if (Percentage) Ammount = Mathf.RoundToInt(MaxHp * (float)Ammount / 100);
        Hp += Ammount;
        if (Hp > MaxHp) Hp = MaxHp;
        SetHpBarValue();
    }

    void SetHpBarValue()
    {
        HpBar.fillAmount = (float)Hp/MaxHp;
    }

    IEnumerator WaitTillAttackAnim()
    {
        yield return new WaitForSeconds(0.35f);
        Sc_CharaFacing FaceSc = GetComponent<Sc_CharaFacing>();
        GetComponent<Sc_CharaFacing>().LastXpost = transform.position.x;
        GetComponent<Sc_CharaFacing>().enabled = true;
    }
}

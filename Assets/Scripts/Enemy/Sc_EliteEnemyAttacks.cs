using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_EliteEnemyAttacks : MonoBehaviour
{
    [Header("Main Components")]
    public float AttackDelay;

    [Header("Attack #1: Meteor Shower")]
    public GameObject ExplosionIndicator;
    public GameObject Explosion;
    public int MeteorDamage;
    public TypeList MeteorType;
    public float MeteorAmmount;
    public float DelayEachMeteor;

    [Header("Attack #2: Tentacles Swipe")]
    public List<GameObject> List_Tentacles;
    public int SwipeDamage;
    public float SwipeSpeed;
    public float SwipeDelay;

    [Header("Attack #3: Targeted Fire")]
    public GameObject Beam;
    public int BeamDamage;
    public TypeList BeamType;
    public int BeamAmmount;
    public float DelayEachBeam;
    public float BeamActiveTime;


    //Others
    PlayerManager PlayerManager;
    ElementsHolder ElementsHolder;
    Enem_Shoot Enem_Shoot;
    Quaternion BaseShootPointRotation;
    float DelayCharger;
    bool CurrentlyAttacking;
    int NextAttackId;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice }


    void Start()
    {
        PlayerManager = GameObject.FindGameObjectWithTag("CommonData").GetComponent<PlayerManager>();
        ElementsHolder = GameObject.FindGameObjectWithTag("CommonData").GetComponent<ElementsHolder>();
        Enem_Shoot = GetComponent<Enem_Shoot>();
        BaseShootPointRotation = Enem_Shoot.ShootPoint.transform.rotation;

        // Meteor
        ExplosionIndicator.GetComponent<EN_Bomb>().ExplosionPrefab = Explosion;
        Sc_DamageToPlayer ExplosionSc = Explosion.GetComponent<Sc_DamageToPlayer>();
        ExplosionSc.Damage = MeteorDamage;
        ExplosionSc.TypeFix = MeteorType.ToString();

        // Tentacles
        foreach (var Tentacle in List_Tentacles) Tentacle.GetComponent<Sc_DamageToPlayer>().Damage = SwipeDamage;

        // Beam
        Beam.SetActive(false);
        GameObject NewBeam = Instantiate(Beam, transform);
        Beam = NewBeam;

        Beam.GetComponent<SpriteRenderer>().color = ElementsHolder.GetColor(BeamType.ToString());
        EN_Beam BeamSc = Beam.GetComponent<EN_Beam>();
        BeamSc.BeamStayDuration = BeamActiveTime;
        BeamSc.Damage = BeamDamage;
        BeamSc.Type = BeamType.ToString();

        Enem_Shoot.Prefab_Bullet = Beam;
        Enem_Shoot.ChainShoot = BeamAmmount - 1;
        Enem_Shoot.ChainDelay = DelayEachBeam;
    }

    void Update()
    {
        if (!CurrentlyAttacking)
        {
            DelayCharger += Time.deltaTime;
            if (DelayCharger >= AttackDelay)
            {
                CurrentlyAttacking = true;
                DelayCharger = 0;
                
                switch (NextAttackId)
                {
                    case 0: StartCoroutine(MeteorShower()); break;
                    case 1: StartCoroutine(SwipeAttack()); break;
                    case 2: StartCoroutine(BeamAttack()); break;
                }

                NextAttackId++;
                if (NextAttackId > 2) NextAttackId = 0;
            }
        }
    }

    IEnumerator MeteorShower()
    {
        for (int i = 0; i < MeteorAmmount; i++)
        {
            Instantiate(ExplosionIndicator, PlayerManager.SelectedChara.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(DelayEachMeteor);
        }
        CurrentlyAttacking = false;
    }

    IEnumerator SwipeAttack()
    {
        foreach (var Tentacles in List_Tentacles)
        {
            Tentacles.SetActive(false);
            Tentacles.GetComponent<Sc_DamageToPlayer>().IsHitted = false;
            Tentacles.SetActive(true);

            yield return new WaitForSeconds(SwipeDelay);
        }
        CurrentlyAttacking = false;
    }

    IEnumerator BeamAttack()
    {
        Enem_Shoot.ShootPoint.rotation = BaseShootPointRotation;
        Enem_Shoot.StartShoot(false);
        yield return new WaitForSeconds(DelayEachBeam * BeamAmmount * 2);
        CurrentlyAttacking = false;
    }
}

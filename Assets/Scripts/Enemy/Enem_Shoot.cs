using System.Collections;
using UnityEngine;

public class Enem_Shoot : MonoBehaviour
{
    [Header("Basic Components")]
    public GameObject Prefab_Bullet;
    public Transform ShootPoint;
    public bool Activated = true;
    public float FirstDelay;
    public float ShootDelay;
    public float BulletSpeed;
    public float BulletLifeTime;

    [Header("Multiple Shoots")]
    public int ShootAmmount;
    public Vector3 PosDifference;
    public float AngleDifference;
    public float AddBaseAngleAfterShoot;

    [Header("Chain Shoot")]
    public int ChainShoot;
    public float ChainDelay;

    [Header("Damage")]
    public int Damage;
    public TypeList Type;

    [Header("Sound Effects")]
    public AudioClip Sfx_Shoot;

    //Others
    AudioSource SoundManager;
    float DelayCharger;
    bool FirstShoot;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice, None }


    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();

        if (Prefab_Bullet != null)
        {
            Prefab_Bullet.SetActive(false);
            GameObject NewBullet = Instantiate(Prefab_Bullet, transform);
            Prefab_Bullet = NewBullet;
            Prefab_Bullet.GetComponent<SpriteRenderer>().color = GameObject.FindGameObjectWithTag("CommonData").GetComponent<ElementsHolder>().GetColor(Type.ToString());
        }
    }

    void Update()
    {
        if (Activated)
        {
            DelayCharger += Time.deltaTime;
            if (DelayCharger >= (FirstShoot ? ShootDelay : FirstDelay))
            {
                StartShoot();
                DelayCharger = 0;
                if (!FirstShoot) FirstShoot = true;
            }
        }
    }

    public void StartShoot(bool IsBullet = true)
    {
        Shoot(IsBullet);
        if (ChainShoot > 0) StartCoroutine(ChainShooting(IsBullet));
    }

    void Shoot(bool IsBullet = true)
    {
        if (Sfx_Shoot != null) SoundManager.PlayOneShot(Sfx_Shoot);
        Quaternion TempRotation = ShootPoint.rotation;
        Vector2 TempPosition = ShootPoint.position;

        for (int i = 0; i < ShootAmmount; i++)
        {
            GameObject NewBullet = Instantiate(Prefab_Bullet, ShootPoint.position, ShootPoint.rotation);
            NewBullet.SetActive(true);
            if (IsBullet)
            {
                Sc_Bullet_Damage BulletSc = NewBullet.GetComponent<Sc_Bullet_Damage>();
                BulletSc.Damage = Damage;
                BulletSc.Type = Type.ToString();
                BulletSc.StartLifeTime(BulletLifeTime);
                NewBullet.GetComponent<Rigidbody2D>().AddForce(NewBullet.transform.up * BulletSpeed);
            }
            ShootPoint.Rotate(0, 0, AngleDifference);
            ShootPoint.position += PosDifference;
        }

        ShootPoint.rotation = TempRotation;
        ShootPoint.position = TempPosition;
        if (AddBaseAngleAfterShoot > 0) ShootPoint.Rotate(0, 0, AddBaseAngleAfterShoot);
    }

    IEnumerator ChainShooting(bool IsBullet = true)
    {
        for (int i = 0; i < ChainShoot; i++)
        {
            yield return  new WaitForSeconds(ChainDelay);
            Shoot(IsBullet);
        }
    }
}

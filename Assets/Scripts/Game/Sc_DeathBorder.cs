using UnityEngine;

public class Sc_DeathBorder : MonoBehaviour
{
    [Header("Properties")]
    public Transform SpawnPoint;
    public float LaunchForce;
    public int Damage;
    public AudioClip Sfx_Crash;

    //Others
    AudioSource SoundManager;


    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            SoundManager.PlayOneShot(Sfx_Crash);
            collider.GetComponent<Sc_Chara>().Damaged(Damage, "Null");

            GameObject PlObj = collider.gameObject.transform.parent.gameObject;

            float XposLaunch = Random.Range(15, 30);
            int NegOrPlus = Random.Range(1, 3);
            XposLaunch *= NegOrPlus == 1 ? -1 : 1;
            Vector2 LaunchDirection = new Vector2(XposLaunch, LaunchForce);

            PlObj.transform.position = SpawnPoint.position;
            PlObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            PlObj.GetComponent<Rigidbody2D>().AddForce(LaunchDirection);
        }
    }
}

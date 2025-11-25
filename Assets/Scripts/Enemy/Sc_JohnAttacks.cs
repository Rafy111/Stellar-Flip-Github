using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_JohnAttacks : MonoBehaviour
{
    [Header("Main Components")]
    public float AttackDelay;

    [Header("Attack #1: Heal Spawn")]
    public List<GameObject> List_AllMinions;
    public int MinionsAmmount = 3;
    public float SpawnDelay;
    public int HealPerMinion;
    public AudioClip Sfx_Healing;
    public GameObject Prefab_DestroyEnemy;
    public List<GameObject> List_SpawnedMinions;

    //Others
    AudioSource SoundManager;
    Sc_Enem Sc_Enem;
    float DelayCharger;
    bool CurrentlyAttacking;
    int NextAttackId;

    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        Sc_Enem = GetComponent<Sc_Enem>();
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
                    case 0: StartCoroutine(SpawnMinions()); break;
                    //case 1: StartCoroutine(SwipeAttack()); break;
                    //case 2: StartCoroutine(BeamAttack()); break;
                }

                NextAttackId++;
                if (NextAttackId > 0) NextAttackId = 0;
            }
        }
    }

    IEnumerator SpawnMinions()
    {
        //Healing
        for (int i = 0; i < List_SpawnedMinions.Count; i++)
        {
            GameObject CurrentMinion = List_SpawnedMinions[i];
            Instantiate(Prefab_DestroyEnemy, CurrentMinion.transform.position, Quaternion.identity);
            Destroy(CurrentMinion);
            SoundManager.PlayOneShot(Sfx_Healing);
            Sc_Enem.Heal(HealPerMinion);
            yield return new WaitForSeconds(SpawnDelay);
        }
        List_SpawnedMinions.Clear();

        //Spawning
        List<GameObject> List_MinionsPoll = new List<GameObject>(List_AllMinions);
        for (int i = 0; i < MinionsAmmount; i++)
        {
            int RandomNum = Random.Range(0, List_MinionsPoll.Count);
            GameObject NeMinionReference = List_MinionsPoll[RandomNum];
            List_MinionsPoll.RemoveAt(RandomNum);
            GameObject NewMinion = Instantiate(NeMinionReference, NeMinionReference.transform.position, NeMinionReference.transform.rotation);
            List_SpawnedMinions.Add(NewMinion);
            NewMinion.SetActive(true);
            yield return new WaitForSeconds(SpawnDelay);
        }
        CurrentlyAttacking = false;
    }
}

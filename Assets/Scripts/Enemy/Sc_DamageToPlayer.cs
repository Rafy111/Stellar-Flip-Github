using System.Collections;
using UnityEngine;

public class Sc_DamageToPlayer : MonoBehaviour
{
    [Header("Damage")]
    public int Damage;
    public TypeList Type;

    [Header("Other")]
    public float ActiveDuration = 0;
    public bool MultipleHits = false;
    public bool IsHitted;
    public string TypeFix;

    //Enums
    public enum TypeList { Fire, Water, Nature, Electric, Ice }


    void Start()
    {
        if (ActiveDuration > 0) StartCoroutine(DisableTimer());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Character"))
        {
            if (!IsHitted)
            {
                if (!MultipleHits) IsHitted = true;
                collider.gameObject.GetComponent<Sc_Chara>().Damaged(Damage, TypeFix == "" ? Type.ToString() : TypeFix);
            }
        }
    }

    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(ActiveDuration);
        IsHitted = true;
    }
}

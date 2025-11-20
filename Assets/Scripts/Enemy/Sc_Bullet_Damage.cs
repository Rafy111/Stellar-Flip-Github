using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Sc_Bullet_Damage : MonoBehaviour
{
    [Header("Components")]
    public GameObject HitEffect;

    [Header("Others")]
    public int Damage;
    public string Type;

    [Header("Charge")]
    public bool IsHomingCharge;
    public float WaitUntilMove;
    public float BulletSpeed;

    [Header("Homing")]
    public float HomingDuration;
    public float RotateSpeed;

    //Others
    Rigidbody2D Rigidbody;
    GameObject TargetObj;
    float HomingSecond;


    void Start()
    {
        if (IsHomingCharge)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            StartCoroutine(WaitForMove());
        }
    }

    void OnTriggerEnter2D(Collider2D Obj)
    {
        if (Obj.gameObject.CompareTag("Border") || Obj.gameObject.CompareTag("Flipper")) Death();
        else if (Obj.gameObject.CompareTag("Character"))
        {
            Obj.gameObject.GetComponent<Sc_Chara>().Damaged(Damage, Type.ToString());
            Death();
        }
    }

    public void StartLifeTime(float Time)
    {
        StartCoroutine(LifeTime(Time));
    }

    IEnumerator LifeTime(float Time)
    {
        yield return new WaitForSeconds(Time);
        Death();
    }

    void Death()
    {
        GameObject NewHitEffect = Instantiate(HitEffect, transform.position, Quaternion.identity);
        NewHitEffect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        Destroy(gameObject);
    }

    IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(WaitUntilMove);
        //Rigidbody.AddForce(transform.up * BulletSpeed);
        StartCoroutine(Moving());
        StartCoroutine(Homing());
    }

    IEnumerator Homing()
    {
        TargetObj = GameObject.FindGameObjectWithTag("Player");

        while (HomingSecond < HomingDuration)
        {
            if (TargetObj == null)
            {
                TargetObj = GameObject.FindGameObjectWithTag("Player");
                if (TargetObj != null) yield break;
            }

            HomingSecond += Time.deltaTime;

            Vector3 Target = (TargetObj.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(-Target.x, Target.y) * Mathf.Rad2Deg;
            Quaternion TargetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, RotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Moving()
    {
        while (true)
        {
            transform.position += transform.up * BulletSpeed * Time.deltaTime;
            yield return null;
        }
    }
}

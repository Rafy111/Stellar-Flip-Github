using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPinBall : MonoBehaviour
{
    [Header("Components")]
    public GameObject Indicator_Selected;
    public GameObject Indicator_Boost;
    public GameObject Indicator_BoostLine;

    [Header("Others")]
    public PlayerManager PlayerManager;
    public Transform CenterPoint;

    void Start()
    {
        PlayerManager = GameObject.FindGameObjectWithTag("CommonData").GetComponent<PlayerManager>();
    }
}

using UnityEngine;

public class Sc_Object : MonoBehaviour
{
    public int ScoreAdd;
    GameManager GameManager;


    void Start() { GameManager = GameObject.FindGameObjectWithTag("CommonData").GetComponent<GameManager>(); }

    public void AddScore()
    {
        GameManager.Combo_Add(1);
        GameManager.AddTempScore(ScoreAdd);
    }
}

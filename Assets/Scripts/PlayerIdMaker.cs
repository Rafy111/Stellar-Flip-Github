using UnityEngine;

public class PlayerIdMaker : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Pl1", 0);
        PlayerPrefs.SetInt("Pl2", 1);
        PlayerPrefs.SetInt("Pl3", 2);
    }
}

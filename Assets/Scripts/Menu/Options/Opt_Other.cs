using System;
using UnityEngine;
using UnityEngine.UI;

public class Opt_Other : MonoBehaviour
{
    [Header("Worlds")]
    public Toggle World_Phobos;
    public GameObject Gm_Phobos;
    public Toggle World_Deimos;
    public GameObject Gm_Deimos;

    [Header("Transfer Data")]
    public GameObject Button_Transfer;
    public MainMenuLevel Sc_Phobos;

    //Other
    WorldSelector WorldSelector;


    void Start()
    {
        WorldSelector = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<WorldSelector>();

        if (!PlayerPrefs.HasKey("World_Phobos"))
        {
            PlayerPrefs.SetInt("World_Phobos", 0);
            PlayerPrefs.SetInt("World_Deimos", 0);
        }
        if (PlayerPrefs.HasKey("DataTransfered")) Button_Transfer.SetActive(false);

        //Phobos
        bool BoolChecker = Convert.ToBoolean(PlayerPrefs.GetInt("World_Phobos"));
        Gm_Phobos.SetActive(BoolChecker);
        World_Phobos.isOn = BoolChecker;

        BoolChecker = Convert.ToBoolean(PlayerPrefs.GetInt("World_Deimos"));
        Gm_Deimos.SetActive(BoolChecker);
        World_Deimos.isOn = BoolChecker;
    }

    public void Set_World_Phobos()
    {
        bool BoolChecker = World_Phobos.isOn;
        Gm_Phobos.SetActive(BoolChecker);
        PlayerPrefs.SetInt("World_Phobos", Convert.ToInt32(BoolChecker));

        if (PlayerPrefs.GetInt("WorldId") == 2) WorldSelector.ChangeWorld(4);
    }

    public void Set_World_Deimos()
    {
        bool BoolChecker = World_Deimos.isOn;
        Gm_Deimos.SetActive(BoolChecker);
        PlayerPrefs.SetInt("World_Deimos", Convert.ToInt32(BoolChecker));

        if (PlayerPrefs.GetInt("WorldId") == 3) WorldSelector.ChangeWorld(4);
    }

    public void TransferData()
    { 
        Button_Transfer.SetActive(false);
        PlayerPrefs.SetInt("DataTransfered", 1);

        int OldData = PlayerPrefs.GetInt("LastUnlockedLevel");
        if (OldData > PlayerPrefs.GetInt("2_LastUnlockedLevel"))PlayerPrefs.SetInt("2_LastUnlockedLevel", OldData);
        PlayerPrefs.DeleteKey("LastUnlockedLevel");

        for (int i = 1; i <= 10; i++)
        {
            OldData = PlayerPrefs.GetInt("HighscoreLv" + i);
            if (OldData > PlayerPrefs.GetInt("2_HighscoreLv" + i)) PlayerPrefs.SetInt("2_HighscoreLv" + i, OldData);
            PlayerPrefs.DeleteKey("HighscoreLv" + i);
        }

        Sc_Phobos.SetLevelInfo();
    }
}

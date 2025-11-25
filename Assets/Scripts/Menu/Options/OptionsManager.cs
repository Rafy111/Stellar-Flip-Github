using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("Tab")]
    public List<Button> List_Section;

    [Header("Options")]
    public List<GameObject> List_Tab;


    void Start()
    {
        if (!PlayerPrefs.HasKey("OptTabId")) PlayerPrefs.SetInt("OptTabId", 0);
        SetTab();
    }

    public void SwitchTab(int Id)
    {
        PlayerPrefs.SetInt("OptTabId", Id);
        SetTab();
    }

    void SetTab()
    {
        int CurrentTabId = PlayerPrefs.GetInt("OptTabId");

        for (int i = 0; i < List_Section.Count; i++)
        {
            if (i == CurrentTabId)
            {
                List_Section[i].interactable = false;
                List_Tab[i].SetActive(true);   
            }
            else
            {
                List_Section[i].interactable= true;
                List_Tab[i].SetActive(false);
            }
        }
    }
}

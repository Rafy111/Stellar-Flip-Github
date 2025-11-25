using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelector : MonoBehaviour
{
    [Header("Components")]
    public List<Sprite> List_UiUnselected;
    public List<Sprite> List_UiSelected;
    public List<Button> List_SelectWorldButton;
    public List<Image> List_SelectWorldImage;
    public List<GameObject> List_LevelSelect;

    void Start()
    {
        if (!PlayerPrefs.HasKey("WorldId")) PlayerPrefs.SetInt("WorldId", 4);
        SetLevelSelect();
    }

    void SetLevelSelect()
    {
        int CurrentWorldId = PlayerPrefs.GetInt("WorldId");

        for (int i = 0; i < List_LevelSelect.Count; i++)
        {
            if (i + 1 == CurrentWorldId)
            {
                List_SelectWorldButton[i].interactable = false;
                List_SelectWorldImage[i].sprite = List_UiSelected[i];
                List_LevelSelect[i].SetActive(true);
            }
            else
            {
                List_SelectWorldButton[i].interactable = true;
                List_SelectWorldImage[i].sprite = List_UiUnselected[i];
                List_LevelSelect[i].SetActive(false);
            }
        }
    }

    public void ChangeWorld(int WorldId)
    {
        PlayerPrefs.SetInt("WorldId", WorldId);
        SetLevelSelect();
    }
}

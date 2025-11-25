using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharaSelectHandler : MonoBehaviour
{
    [Header("Tab Toggle")]
    public GameObject Tab_CharaSelect;
    public GameObject Button_CharaSelectToggle;

    [Header("Lists")]
    public List<Image> List_PlIconsHolder;
    public List<Image> List_PlElementsHolder;

    [Header("Selection")]
    public List<Image> List_AvailableCharacter;
    public List<GameObject> List_NumberFrameHolder;
    public List<TMP_Text> List_NumberHolder;
    public Color Color_Unselected;
    public Color Color_Selected;

    [Header("Lists Sprites")]
    public List<Sprite> List_PlIcons;
    public List<Sprite> List_PlElements;

    //Others
    bool IsTabActive = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Pl1"))
        {
            PlayerPrefs.SetInt("Pl1", 0);
            PlayerPrefs.SetInt("Pl2", 1);
            PlayerPrefs.SetInt("Pl3", 2);
        }

        SetSprites();
    }

    public void SetPlayerId(int Slot, int PlId)
    {
        List<int> List_SelectedCharaIds = new List<int> {PlayerPrefs.GetInt("Pl1"), PlayerPrefs.GetInt("Pl2"), PlayerPrefs.GetInt("Pl3") };
        int FindIndex = List_SelectedCharaIds.IndexOf(PlId);
        if (FindIndex >= 0 && FindIndex != Slot) PlayerPrefs.SetInt("Pl" + (FindIndex + 1), PlayerPrefs.GetInt("Pl" + (Slot + 1)));

        PlayerPrefs.SetInt("Pl" + (Slot + 1), PlId);
        SetSprites();
    }

    void SetSprites()
    {
        for (int i = 0; i < List_PlIconsHolder.Count; i++)
        {
            int PlId = PlayerPrefs.GetInt("Pl" +  (i + 1));
            List_PlIconsHolder[i].sprite = List_PlIcons[PlId];
            List_PlElementsHolder[i].sprite = List_PlElements[PlId];
        }
        SetSelected();
    }

    void SetSelected()
    {
        foreach (var Slot in List_AvailableCharacter) Slot.color = Color_Unselected;
        foreach (var Slot in List_NumberFrameHolder) Slot.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            int CurrentCharaId = PlayerPrefs.GetInt("Pl" + (i + 1));
            List_AvailableCharacter[CurrentCharaId].color = Color_Selected;
            List_NumberHolder[CurrentCharaId].text = (i + 1).ToString();
            List_NumberFrameHolder[CurrentCharaId].SetActive(true);
        }
    }

    public void SetActiveCharaSelectToggle(bool IsActive) { Button_CharaSelectToggle.SetActive(IsActive); }

    public void ToggleTab()
    {
        IsTabActive = !IsTabActive;
        Tab_CharaSelect.SetActive(IsTabActive);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
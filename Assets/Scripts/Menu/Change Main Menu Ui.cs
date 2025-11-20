using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMainMenuUi : MonoBehaviour
{
    public GameObject Ui_Current;

    public void ChangeUi(GameObject UiToChange)
    {
        if (Ui_Current != null) Ui_Current.SetActive(false);
        Ui_Current = UiToChange;
        Ui_Current.SetActive(true);
    }
}

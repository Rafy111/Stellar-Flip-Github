using System.Collections.Generic;
using UnityEngine;

public class ElementsHolder : MonoBehaviour
{
    [Header("Components")]
    public List<string> List_Elements;
    public List<string> List_Weak;
    public List<string> List_Strong;
    public List<Color> List_Color;

    
    public float WeakOrStrong(string TypeSelf, string TypeAttack)
    {
        int AttackId = List_Elements.IndexOf(TypeSelf);
        if (TypeAttack == List_Strong[AttackId]) return 1.5f;
        else if (TypeAttack == List_Weak[AttackId]) return 0.5f;
        else return 1;
    }

    public Color GetColor(string TypeSelf)
    {
        int ColorId = List_Elements.IndexOf(TypeSelf);
        return List_Color[ColorId];
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CharaSelectSlot : MonoBehaviour, IDropHandler
{
    [Header("Components")]
    public int SlotId;

    //Others
    CharaSelectHandler CharaSelectHandler;

    void Start()
    {
        CharaSelectHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CharaSelectHandler>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject DraggedObject = eventData.pointerDrag.gameObject;
        if (DraggedObject.TryGetComponent<CharaSelectDrag>(out var Sc)) CharaSelectHandler.SetPlayerId(SlotId, Sc.CharaId); //Debug.Log("Selected Character ID: " + Sc.CharaId + " on slot ID: " + SlotId);
    }
}

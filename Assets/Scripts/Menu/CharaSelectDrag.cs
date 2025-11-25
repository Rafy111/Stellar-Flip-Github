using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharaSelectDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Components")]
    public int CharaId;

    [Header("Drag Components")]
    public Transform CharaDragHolder;
    public Vector2 DragScale;

    //Others
    CharaSelectHandler CharaSelectHandler;
    Transform ParentTransform;
    Vector3 BasePos;
    Vector3 BaseScale;
    Image Image;


    void Start()
    {
        CharaSelectHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CharaSelectHandler>();

        ParentTransform = transform.parent;
        BasePos = transform.localPosition;
        BaseScale = transform.localScale;
        Image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CharaSelectHandler.SetActiveCharaSelectToggle(false);

        transform.SetParent(CharaDragHolder, false);
        transform.localScale = DragScale;
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CharaSelectHandler.SetActiveCharaSelectToggle(true);

        transform.SetParent(ParentTransform, false);
        transform.localPosition = BasePos;
        transform.localScale = BaseScale;
        Image.raycastTarget = true;
    }
}

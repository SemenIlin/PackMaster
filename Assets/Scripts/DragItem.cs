using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DragItem dragItem;
    private Vector3 startPosition;
    private Transform startParrent;

    private CanvasGroup canvasGroup;
    private RectTransform dragLayer;
    private Transform slot;

    [SerializeField] private GameObject[] ordersForGrouping;
    private int countorOfOdersForGrouping;

    public static event Action<bool> ShowButtonGo;

    public static void ResetActionShowButtonGo()
    {
        ShowButtonGo = null;
    }
    private void Start()
    {
        countorOfOdersForGrouping = ordersForGrouping != null ? ordersForGrouping.Length : 0;
        ShowButtonGo?.Invoke(false);
        canvasGroup = GetComponent<CanvasGroup>();
        dragLayer = GameObject.
                        FindGameObjectWithTag("DragLayer").
                        GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ShowButtonGo?.Invoke(false);
        slot = null;
        startParrent = transform.parent;
        transform.SetParent(dragLayer);
        dragItem = this;
        startPosition = transform.position;
        canvasGroup.blocksRaycasts = false;        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragItem = null;
        canvasGroup.blocksRaycasts = true;
        if(slot == null)
        {
            transform.SetParent(startParrent);
            transform.position = startPosition;
        }
        else
        {
            transform.SetParent(slot);
            transform.localPosition = Vector3.zero;

            countorOfOdersForGrouping = ordersForGrouping != null ? ordersForGrouping.Length : 0;
            foreach (var cell in ordersForGrouping)
            {
                if (cell.transform.childCount == 0)
                {
                    countorOfOdersForGrouping--;
                }
            }
            if (countorOfOdersForGrouping == 0)
            {
                ShowButtonGo?.Invoke(true);
            }
            else
            {
                ShowButtonGo?.Invoke(false);
            }
        }
        slot = null;
    }

    public void SetItemToSlot(Transform slot)
    {
        this.slot = slot;
    }
}

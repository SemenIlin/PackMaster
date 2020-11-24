using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var item = DragItem.dragItem;
        if (item != null && transform.childCount == 0)
        {
            item.SetItemToSlot(transform);
        }
        
    }
}

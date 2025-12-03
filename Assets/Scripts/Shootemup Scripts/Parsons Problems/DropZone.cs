using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable == null) return;

        // Check if drop zone already has a child
        if (transform.childCount > 0)
        {
            // Drop zone occupied → do nothing, let line snap back
            return;
        }

        // Accept the draggable line
        draggable.transform.SetParent(transform);
        draggable.transform.position = transform.position; // snap into place
    }
}

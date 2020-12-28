using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlots : MonoBehaviour, IDropHandler
{
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            Transform previousParty = DragHandler.itemBeingDragged.transform.parent.parent;
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            previousParty.GetComponent<MenuParty>().updateTeam();
            transform.parent.GetComponent<MenuParty>().updateTeam();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInventory : MonoBehaviour
{
    [SerializeField] Transform party;

    // Start is called before the first frame update
    void Start()
    {
        updateTeam();
    }

    void updateTeam()
    {
        foreach(Transform slots in party)
        {
            print(slots.GetComponent<InvenSlots>().item.GetComponent<MenuUnitSaver>().unit.ID);
        }
    }
}

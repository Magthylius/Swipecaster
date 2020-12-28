using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParty : MonoBehaviour
{
    [SerializeField] Transform party;
    public List<UnitObject> partyMemberInfo = new List<UnitObject>();
    // Start is called before the first frame update
    void Start()
    { 
        updateTeam();
    }

    public void updateTeam()
    {
        int numParty = 0;
        partyMemberInfo.Clear();

        foreach (Transform slots in party)
        {
            if(slots.GetComponent<InvenSlots>().item != null)
            {              
                partyMemberInfo.Add(slots.GetComponent<InvenSlots>().item.GetComponent<MenuUnitSaver>().unit);
                //print("Party No." + numParty + " "+ slots.GetComponent<InvenSlots>().item.GetComponent<MenuUnitSaver>().unit.ID);
                numParty++;
            }       
        }

        if(ActPartySaver.instance.activeParty == this)
        {
            ActPartySaver.instance.updateTeam();
        }
      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParty : MonoBehaviour
{
    [SerializeField] Transform party;
    [SerializeField] bool isParty;
    public List<MenuUnit> partyMemberInfo = new List<MenuUnit>();
    // Start is called before the first frame update
    void Start()
    { 
        updateTeam();
    }

    public void updateTeam()
    {
        int numParty = 0;
        int numNonParty = 0;
       // partyMemberInfo.Clear();
        if(isParty) ActPartySaver.instance.activePartySO.activeUnits.Clear();

        foreach (Transform slots in party)
        {
            if(slots.GetComponent<InvenSlots>().item != null)
            {
                if(isParty == true)
                {
                    partyMemberInfo.Add(slots.GetComponent<InvenSlots>().item.GetComponent<MenuUnitSaver>().unit);
                    print("Party No." + numParty + " "+ slots.GetComponent<InvenSlots>().item.GetComponent<MenuUnitSaver>().unit.ID);
                    numParty++;
                }
                else
                {
                    print("Not Party" + numNonParty + " " + slots.GetComponent<InvenSlots>().item.GetComponent<MenuUnitSaver>().unit.ID);
                    numNonParty++;
                }
                
            }       
        }

        if(isParty)
        {
            ActPartySaver.instance.updateTeam();
        }
    }
}

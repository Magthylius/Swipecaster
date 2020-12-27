using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActPartySaver : MonoBehaviour
{
    public static ActPartySaver instance;
    public ActParty activePartySO;
    public MenuParty activeParty;
//    public List<MenuUnit> partyMemberInfo = new List<MenuUnit>();

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public void updateTeam()
    {
        activePartySO.activeUnits = activeParty.partyMemberInfo;
    }

}

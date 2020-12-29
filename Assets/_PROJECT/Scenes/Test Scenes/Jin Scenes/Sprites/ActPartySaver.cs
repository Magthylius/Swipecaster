using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActPartySaver : MonoBehaviour
{
    public static ActPartySaver instance;
    public CasterParty activePartySO;
    public MenuParty activeParty;
    public List<MenuParty> menuParties;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        //activePartySO.activeUnits = menuParties[1].partyMemberInfo;
    }

    public void updateTeam()
    {
        activePartySO.activeUnits = activeParty.partyMemberInfo;
    }

}

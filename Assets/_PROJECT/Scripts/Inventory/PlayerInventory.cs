using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    string casterLocation = "ScriptableObjects/Casters";

    //! Get all casters in the game
    [SerializeField] List<UnitObject> allCasters = new List<UnitObject>();
    [SerializeField] List<UnitObject> playerAvailableCasters = new List<UnitObject>();

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        UnitObject[] tempCaster = Resources.LoadAll<UnitObject>(casterLocation);

        foreach (var _caster in tempCaster)
        {
            allCasters.Add(_caster);
        }
    }
    

    public void UpdatePlayerInventory(List<string> castersID)
    {
        foreach (var id in castersID)
        {
            for (int i = 0; i < allCasters.Count; i++)
            {
                if (id == allCasters[i].ID)
                {
                    if (!playerAvailableCasters.Contains(allCasters[i]))
                    {
                        playerAvailableCasters.Add(allCasters[i]);
                    }
                }
            }
        }
    }

    #region Accessors

    public List<UnitObject> PlayerCasters => playerAvailableCasters;
    public List<UnitObject> AllCasters => allCasters;

    #endregion
}



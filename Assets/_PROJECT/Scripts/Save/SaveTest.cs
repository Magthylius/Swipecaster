using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{

     public PlayerInventoryData playerData;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.Save(playerData);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            playerData = SaveManager.Load();
        }
    }
}

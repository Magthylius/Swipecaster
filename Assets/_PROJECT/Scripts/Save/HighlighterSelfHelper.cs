using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighterSelfHelper : MonoBehaviour
{
    DatabaseManager databaseManager;

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        
        databaseManager.SelfAddDefaultHighliter(transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneLineManager : MonoBehaviour
{
    public static RuneLineManager instance;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {    
        if (transform.position.y <= -40f)
        {
            
            gameObject.SetActive(false);
        }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    Rigidbody2D rb;
    public float deactivateLevel = -2500.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GetComponent<RectTransform>().anchoredPosition.y <= deactivateLevel)
        {
            rb.velocity = new Vector2(0, -10);
            gameObject.SetActive(false);
        }
    }
}

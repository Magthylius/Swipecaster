using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitVelocity : MonoBehaviour
{
    float maxVelocity;
    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start()
    {
        GetMaxVelocity();
        rb.velocity = new Vector2(0, -maxVelocity);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, -maxVelocity);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void GetMaxVelocity() => maxVelocity = CastingManager.instance.maxVel;
}

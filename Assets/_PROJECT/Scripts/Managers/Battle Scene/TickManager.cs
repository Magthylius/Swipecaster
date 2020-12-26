using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public class OnTickEvent : EventArgs
    {
        public int tick;
    }

    public static TickManager instance;

    public static event EventHandler<OnTickEvent> OnTick;
    
    
    [Header("Set Max Tick Timer")]
    public float setTick = 0.2f;
    float maxTick;

    int tick = 0;
    float tickTimer;

    void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        else instance = this;
        
        maxTick = setTick;
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= maxTick)
        {
            tickTimer -= maxTick;
            tick++;
            if (OnTick != null) OnTick(this, new OnTickEvent {tick = tick});

            //print("Tick: " + tick);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;
    ConnectionManager connectionManager;

    public float runeDamage;
    [Header("Countdown Timer")]
    public float countdownTimer;

    public Slider slideTimer;
    
    List<RuneBehaviour> comboList = new List<RuneBehaviour>();
    float totalDamage;
    float timer;
    bool isStart = false;

    RuneType runeType;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        connectionManager = ConnectionManager.instance;
    }

    void Update()
    {
        if (isStart)
        {
            timer -= Time.deltaTime;
            slideTimer.value = timer;
            if (timer <= 0)
            {
                DealDamage();
            }
        }
    }

    public void CollectDamage()
    {
        comboList = connectionManager.GetSelectedRuneList();
        
        for (int i = 0; i < comboList.Count; i++)
        {
            totalDamage += runeDamage;
            comboList[i].gameObject.SetActive(false);
        }
    }

    public void SetCountdownTimer() => timer = countdownTimer;
    public void SetIsStart() => isStart = true;
    void DealDamage()
    {
        print("Deal Damage: " + totalDamage);
        totalDamage = 0;

        
        isStart = false;
    }

    #region Accessors
    public bool GetIsStart() => isStart;
    public RuneType GetRuneType() => runeType;

    #endregion

}

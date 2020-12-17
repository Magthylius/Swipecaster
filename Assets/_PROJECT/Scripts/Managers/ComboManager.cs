using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public struct RuneStorage
    {
        public RuneType runeType;
        public int amount;

        public RuneStorage(RuneType _runeType, int _amount)
        {
            runeType = _runeType;
            amount = _amount;
        }
    }
    
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

    RuneStorage groundRune = new RuneStorage(RuneType.GROUND, 0);
    RuneStorage fireRune = new RuneStorage(RuneType.FIRE, 0);
    RuneStorage electricRune = new RuneStorage(RuneType.ELECTRIC, 0);

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
            timer -= Time.unscaledDeltaTime;
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

        RuneType runetype = connectionManager.GetSelectionType();

        switch (runetype)
        {
            case RuneType.GROUND:
                groundRune.amount += comboList.Count;
                break;
            case RuneType.FIRE:
                fireRune.amount += comboList.Count;
                break;
            case RuneType.ELECTRIC:
                electricRune.amount += comboList.Count;
                break;
        }
        
        for (int i = 0; i < comboList.Count; i++)
        {
            
            comboList[i].gameObject.SetActive(false);
        }
        
    }

    public void SetCountdownTimer() => timer = countdownTimer;
    public void SetIsStart() => isStart = true;
    void DealDamage()
    {
        print("FireRune: " + fireRune.amount + "GroundRune: " + groundRune.amount + "ElectricRune: " + electricRune.amount);

        fireRune.amount = 0;
        groundRune.amount = 0;
        electricRune.amount = 0;
        
        isStart = false;
    }

    #region Accessors
    public bool GetIsStart() => isStart;
    public RuneType GetRuneType() => runeType;

    #endregion

}

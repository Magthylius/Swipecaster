using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LerpFunctions;

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

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;
    ConnectionManager connectionManager;
    RuneManager runeManager;

    public float runeDamage;

    [Header("Countdown Timer")]
    public float countdownTimer;
    public Slider slideTimer;
    public Image slider;
    
    List<RuneBehaviour> comboList = new List<RuneBehaviour>();
    float totalDamage;
    float timer;
    bool isStart = false;
    bool sliderAnim = false;

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
        runeManager = RuneManager.instance;
    }

    void Update()
    {
        if (isStart && !sliderAnim)
        {
            timer -= Time.unscaledDeltaTime;
            //slideTimer.value = timer;
            slider.fillAmount = timer / countdownTimer;
            if (timer <= 0)
            {
                isStart = false;
                sliderAnim = true;
                timer = countdownTimer;
                //slider.fillAmount = 1.0f;
                
                runeManager.SpawnDeactivate();

                //DealDamage();
            }
        }
        else if (sliderAnim)
        {
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, 1.0f, 2.0f * Time.unscaledDeltaTime);

            if (slider.fillAmount >= 0.999f)
            {
                slider.fillAmount = 1.0f;
                sliderAnim = false;
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
    void DealDamage(GameObject attacker, GameObject enemy)
    {
        print("FireRune: " + fireRune.amount + "GroundRune: " + groundRune.amount + "ElectricRune: " + electricRune.amount);
        float attackerDamage = attacker.GetComponent<UnitEntry>().GetAttack;
        float totalDamage = attackerDamage * fireRune.amount + attackerDamage * groundRune.amount + attackerDamage * electricRune.amount;
        float enemyDefense = enemy.GetComponent<UnitEntry>().GetDefence;
        attackerDamage = (attackerDamage - enemyDefense) / attackerDamage;
        print("Total Damage :" + attackerDamage);
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

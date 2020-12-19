using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public static TurnBaseManager instance;
    BattlestageManager battlestageManager;

    [SerializeField] GameStateEnum battleState;
    
    [Header("Delay between states")]
    public float delaysInBetween; // Delays in between states

    int casterUnitTurn = 0;
    int enemyUnitTurn = 0;

    [SerializeField] GameObject caster;
    [SerializeField] GameObject enemy;
    
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        battlestageManager = BattlestageManager.instance;
        battleState = GameStateEnum.INIT;
        StartCoroutine(InitBattle());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnCasterAttack();
        }
    }

    void CasterTurn()
    {
        print("Is " + caster.name + " turn");
    }

    void OnCasterAttack()
    {
        if (battleState != GameStateEnum.CASTERTURN) return;

        StartCoroutine(CasterAttack());

    }

    void OnEnemyAttack()
    {
        
    }

    void Endturn()
    {
        switch (battleState)
        {
            case GameStateEnum.CASTERTURN:
                if (casterUnitTurn > 3) casterUnitTurn = 0;
                else casterUnitTurn++;

                battleState = GameStateEnum.CASTERTURN;
                CasterTurn();
                
                break;
            case GameStateEnum.ENEMYTURN:
                break;
        }
    }
    
    public IEnumerator InitBattle()
    {

        caster = battlestageManager.GetCurrentCaster(casterUnitTurn);
        enemy = battlestageManager.GetCurrentEnemy(enemyUnitTurn);
        
        yield return new WaitForSeconds(delaysInBetween);
        battleState = GameStateEnum.CASTERTURN;
        CasterTurn();
    }

    IEnumerator CasterAttack()
    {
        yield return new WaitForSeconds(delaysInBetween);
        Endturn();
    }
    
    #region Accessors

    public GameStateEnum GetCurrentState() => battleState;
    public GameObject GetCurrentCaster() => caster;

    #endregion

}

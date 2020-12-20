using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public static TurnBaseManager instance;
    BattlestageManager battlestageManager;
    UnitPositionManager unitPositionManager;

    [SerializeField] GameStateEnum battleState;
    
    [Header("Delay between states")]
    public float delaysInBetween; // Delays in between states

    int casterUnitTurn = 0;
    int enemyUnitTurn = 0;

    bool isPlayerTurn = false;

    [SerializeField] GameObject caster;
    [SerializeField] GameObject enemy;
    [SerializeField]GameObject[] CastersTeamList = new GameObject[4];
    
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
        unitPositionManager = UnitPositionManager.instance;
        battleState = GameStateEnum.INIT;
        StartCoroutine(InitBattle());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerTurn)
        {
            OnCasterAttack();
            isPlayerTurn = false;
        }
    }

    void CasterTurn()
    {
        print("Is " + caster.name + " turn");
        //print(Array.IndexOf(battlestageManager.GetCastersTeam(), caster));
        isPlayerTurn = true;    
    }

    void EnemyTurn()
    {
        print("Is " + enemy.name + " turn");
        OnEnemyAttack();
    }

    void OnCasterAttack()
    {
        if (battleState != GameStateEnum.CASTERTURN) return;
        StartCoroutine(CasterAttack());

    }

    void OnEnemyAttack()
    {
        StartCoroutine(EnemyAttack());
    }

    void Endturn()
    {
        switch (battleState)
        {
            case GameStateEnum.CASTERTURN:
                if (casterUnitTurn >= CastersTeamList.Length - 1) casterUnitTurn = 0;
                else casterUnitTurn++;

                battleState = GameStateEnum.ENEMYTURN;
                enemy = battlestageManager.GetCurrentEnemy(enemyUnitTurn);
                EnemyTurn();
                break;
            
            case GameStateEnum.ENEMYTURN:
                if (enemyUnitTurn >= battlestageManager.GetEnemyTeam().Length - 1) enemyUnitTurn = 0;
                else enemyUnitTurn++;

                battleState = GameStateEnum.CASTERTURN;
                caster = CastersTeamList[casterUnitTurn];
                unitPositionManager.SetHolder(caster);
                CasterTurn();
                break;
        }
    }
    
    public IEnumerator InitBattle()
    {
        CastersTeamList = (GameObject[])battlestageManager.GetCastersTeam().Clone();
        
        caster = CastersTeamList[casterUnitTurn];
        unitPositionManager.SetHolder(caster);

        yield return new WaitForSeconds(delaysInBetween);
        battleState = GameStateEnum.CASTERTURN;
        CasterTurn();
    }

    IEnumerator CasterAttack()
    {
        yield return new WaitForSeconds(delaysInBetween);
        Endturn();
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(delaysInBetween);
        Endturn();
    }
    
    #region Accessors

    public GameStateEnum GetCurrentState() => battleState;
    public GameObject GetCurrentCaster() => caster;

    #endregion

}

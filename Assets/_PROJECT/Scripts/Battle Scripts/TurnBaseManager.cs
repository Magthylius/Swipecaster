using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConversionFunctions;
using Random = UnityEngine.Random;

public class TurnBaseManager : MonoBehaviour
{
    public static TurnBaseManager instance;
    BattlestageManager battlestageManager;
    UnitPositionManager unitPositionManager;
    ComboManager comboManager;
    EnemyAttackManager enemyAttackManager;
    RuneManager runeManager;
    InformationManager infoManager;
    CameraManager cameraManager;

    [SerializeField] GameStateEnum battleState;

    [Header("Delay between states")] public float delaysInBetween; // Delays in between states

    public GameObject highlighter;

    [Header("Highlighter vertical positions")]
    public float gap;

    int casterUnitTurn;
    int enemyUnitTurn;

    bool isPlayerTurn;

    [SerializeField] GameObject caster;
    [SerializeField] GameObject enemy;
    [SerializeField] List<GameObject> castersOrderList;
    [SerializeField] List<GameObject> enemiesOrderList;

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
        comboManager = ComboManager.instance;
        runeManager = RuneManager.instance;
        enemyAttackManager = EnemyAttackManager.instance;
        infoManager = InformationManager.instance;
        cameraManager = CameraManager.instance;
        battleState = GameStateEnum.INIT;
        StartCoroutine(InitBattle());
    }

    void Update()
    {
        if (!isPlayerTurn)
            return;

        highlighter.transform.position = new Vector3(caster.transform.position.x, caster.transform.position.y + gap,
            caster.transform.position.z);

        //! End caster's turn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnCasterAttack();
            isPlayerTurn = false;
        }
    }

    #region Unit Turn

    void CasterTurn()
    {
        runeManager.SpawnActivate();
        
        //! Point caster when is their turn
        caster = castersOrderList[casterUnitTurn];
        unitPositionManager.SetHolder(caster);

        cameraManager.MoveToUnit(caster);
        
        //! Reposition highlighter
        highlighter.transform.position = new Vector3(caster.transform.position.x, caster.transform.position.y + gap,
            caster.transform.position.z);
        highlighter.SetActive(true);

        print("Is " + caster.name + " turn");
        isPlayerTurn = true;

        infoManager.UpdateCasterProtrait(GetCurrentCaster().AsUnit());
        infoManager.UpdateSkillChargeBar(GetCurrentCaster().AsUnit());
    }

    void EnemyTurn()
    {
        //! Point enemy when is their turn
        enemy = enemiesOrderList[enemyUnitTurn];
        
        cameraManager.MoveToUnit(enemy);

        highlighter.SetActive(false);
        print("Is " + enemy.name + " turn");
        OnEnemyAttack();
    }

    #endregion

    #region Unit Actions

    public void OnCasterAttack()
    {
        if (battleState != GameStateEnum.CASTERTURN) return;
        StartCoroutine(CasterAttack());
    }

    void OnEnemyAttack()
    {
        if (battleState != GameStateEnum.ENEMYTURN) return; 
        StartCoroutine(EnemyAttack());
    }

    #endregion
    
    void EndTurn()
    {
        switch (battleState)
        {
            case GameStateEnum.CASTERTURN:
                
                //! if the entire casters team finish their turn, is enemy team turns
                if (casterUnitTurn >= castersOrderList.Count - 1)
                {
                    casterUnitTurn = 0;
                    battleState = GameStateEnum.ENEMYTURN;
                    enemy = battlestageManager.GetCurrentEnemy(enemyUnitTurn);
                    EnemyTurn();
                }
                else
                {
                    //! Next caster turn
                    casterUnitTurn++;
                    CasterTurn();
                }

                break;

            case GameStateEnum.ENEMYTURN:
                
                //! if the entire casters team finish their turn, is enemy team turns
                if (enemyUnitTurn >= battlestageManager.GetEnemyTeam().Count - 1)
                {
                    enemyUnitTurn = 0;
                    battleState = GameStateEnum.CASTERTURN;
                    CasterTurn();
                }
                else
                {
                    //! Next enemy turn
                    enemyUnitTurn++;
                    EnemyTurn();
                }

                break;
        }
    }

    
    //*****************************| Ienumerators |*********************************\\

    //! Set Up the battlestage
    IEnumerator InitBattle()
    {
        castersOrderList = new List<GameObject>(battlestageManager.GetCastersTeam());
        enemiesOrderList = new List<GameObject>(battlestageManager.GetEnemyTeam());
        highlighter = Instantiate(highlighter);
        highlighter.SetActive(false);

        yield return new WaitForSeconds(delaysInBetween);

        battleState = GameStateEnum.CASTERTURN;
        CasterTurn();
    }

    IEnumerator CasterAttack()
    {
        cameraManager.ZoomToCenter();
        battlestageManager.ExecuteAction(caster, battlestageManager.GetSelectedTarget());
        while (!cameraManager.GetIsFree())
        {
            yield return null;
        }
        EndTurn();
    }

    IEnumerator EnemyAttack()
    {
        cameraManager.ZoomToCenter();
        enemyAttackManager.CalculatePriotity(enemy);
        battlestageManager.ExecuteAction(enemyAttackManager.GetCaster(), battlestageManager.GetSelectedTarget());
        while (!cameraManager.GetIsFree())
        {
            yield return null;
        }
        EndTurn();
    }
    
    //******************************************************************************\\

    #region Accessors

    public GameStateEnum GetCurrentState() => battleState;
    public GameObject GetCurrentCaster() => caster;

    public bool GetIsPLayerTurn() => isPlayerTurn;
    

    #endregion
}
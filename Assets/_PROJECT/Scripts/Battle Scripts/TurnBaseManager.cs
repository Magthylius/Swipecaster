using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConversionFunctions;

public class TurnBaseManager : MonoBehaviour
{
    public static TurnBaseManager instance;
    BattlestageManager battlestageManager;
    UnitPositionManager unitPositionManager;
    EnemyAttackManager enemyAttackManager;
    RuneManager runeManager;
    InformationManager infoManager;
    CameraManager cameraManager;
    RoomManager roomManager;
    SceneTransitionManager sceneManager;
    DialogueManager dialogueManager;
    DatabaseManager databaseManager;

    [SerializeField] GameStateEnum battleState;
    [SerializeField] private string sceneNameToLoadAtGameStateEnd = "MainMenuScene";

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
        runeManager = RuneManager.instance;
        enemyAttackManager = EnemyAttackManager.instance;
        infoManager = InformationManager.instance;
        cameraManager = CameraManager.instance;
        roomManager = RoomManager.Instance;
        sceneManager = SceneTransitionManager.instance;
        dialogueManager = DialogueManager.instance;
        databaseManager = DatabaseManager.instance;
        battleState = GameStateEnum.INIT;
        StartCoroutine(InitBattle());
    }

    void Update()
    {
        if (!isPlayerTurn)
            return;

        //! End caster's turn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnCasterAttack();
            isPlayerTurn = false;
        }
    }

    void LateUpdate()
    {
        if (caster)
        {
            highlighter.transform.position = new Vector3(caster.transform.position.x, caster.transform.position.y + gap,
                caster.transform.position.z);
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

        isPlayerTurn = true;

        var current = GetCurrentCaster().AsUnit();
        print("Its " + current.GetEntityName + "'s turn");
        infoManager.SyncUserInterfaceToUnit(current);
        current.InvokeSelfTurnBeginEvent();
    }

    void EnemyTurn()
    {
        //! Point enemy when is their turn
        enemy = enemiesOrderList[enemyUnitTurn];
        
        cameraManager.MoveToUnit(enemy);

        highlighter.SetActive(false);
        print("Is " + enemy.name + " turn");

        var current = GetCurrentEnemy().AsUnit();
        current.InvokeSelfTurnBeginEvent();

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

    public void UpdateLiveTeam()
    {
        castersOrderList = new List<GameObject>(battlestageManager.GetCastersTeam());
        enemiesOrderList = new List<GameObject>(battlestageManager.GetEnemyTeam());
    }

    #endregion
    
    public void EndTurn()
    {
        bool casterWipeout = battlestageManager.GetCastersTeam().Count == 0;
        bool enemyWipeout = battlestageManager.GetEnemyTeam().Count == 0;

        if (casterWipeout)
        {
            battleState = GameStateEnum.END;
            sceneManager.ActivateTransition(sceneNameToLoadAtGameStateEnd);
            return;
        }
        if (enemyWipeout)
        {
            if (roomManager.AnyRoomsLeft)
            {
                roomManager.SetNextRoomIndex();
                battlestageManager.AssignEnemiesToRoom();
            }
            else
            {
                battleState = GameStateEnum.END;
                if (dialogueManager.tutorialPhase == TutorialPhase.guideToMap)
                    databaseManager.SaveTutorialState(TutorialPhase.guideToGacha);
                sceneManager.ActivateTransition(sceneNameToLoadAtGameStateEnd);
                return;
            }
        }

        switch (battleState)
        {
            case GameStateEnum.CASTERTURN:

                GetCurrentCaster().AsUnit().InvokeSelfTurnEndEvent();

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

                GetCurrentEnemy().AsUnit().InvokeSelfTurnEndEvent();

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
        UpdateLiveTeam();
        highlighter = Instantiate(highlighter);
        highlighter.SetActive(false);

        yield return new WaitForSeconds(3f);

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
        enemyAttackManager.CalculatePriority(enemy);
        battlestageManager.ExecuteAction(enemyAttackManager.GetCaster(), enemy);
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
    public GameObject GetCurrentEnemy() => enemy;

    public bool GetIsPLayerTurn() => isPlayerTurn;
    

    #endregion
}
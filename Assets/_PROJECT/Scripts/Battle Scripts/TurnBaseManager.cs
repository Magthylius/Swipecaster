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
    EndResultManager endResultMananger;
    AudioManager audioManager;

    [SerializeField] GameStateEnum battleState;
    [SerializeField] private string sceneNameToLoadAtGameStateEnd = "MainMenuScene";

    public AudioData audioPack;
    
    [Header("Delay between states")]
    public float delaysInBetween; // Delays in between states
    public GameObject highlighter;

    [Header("Room swapping")]
    public EnvironmentFader environmentFader;
    public RoomNumberFader roomNumberFader;

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
        endResultMananger = EndResultManager.instance;
        audioManager = AudioManager.instance;;
        
        battleState = GameStateEnum.INIT;

        roomNumberFader.ShowRoomText();
        audioManager.PlaySFX(audioPack,"BattleEffect");

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
        battlestageManager.GetStageTargetHandler().DeactivateSpriteHolder();

        var current = GetCurrentEnemy().AsUnit();
        current.InvokeSelfTurnBeginEvent();

        print("It's " + current.GetEntityName + "'s turn");
        
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
            battlestageManager.GetStageTargetHandler().DeactivateSpriteHolderAndReset();
            endResultMananger.TriggerResults(false);
            battleState = GameStateEnum.END;
            sceneManager.ActivateTransition(sceneNameToLoadAtGameStateEnd);
            audioManager.PlaySFX(audioPack,"LoseEffect");
            return;
        }
        if (enemyWipeout)
        {
            battlestageManager.GetStageTargetHandler().DeactivateSpriteHolderAndReset();
            if (roomManager.AnyRoomsLeft)
            {
                GetCurrentCaster().AsUnit().InvokeSelfTurnEndEvent();
                roomManager.SetNextRoomIndex();

                environmentFader.StartPingPong();
                roomNumberFader.ShowRoomText();
      
                battlestageManager.AssignEnemiesToRoom();
                StartCoroutine(InitBattle());
                audioManager.PlaySFX(audioPack,"BattleEffect");
                battlestageManager.SetGetFirstOrDefaultTarget();
                return;
            }
            else
            {
                endResultMananger.TriggerResults(true);
                battleState = GameStateEnum.END;
                audioManager.PlaySFX(audioPack,"WinEffect");
                if (dialogueManager.tutorialPhase == TutorialPhase.guideToMap)
                    databaseManager.SaveTutorialState(TutorialPhase.guideToGacha);
                //sceneManager.ActivateTransition(sceneNameToLoadAtGameStateEnd);
                return;
            }
        }

        battlestageManager.HandleTargetIfNull();
        battlestageManager.RegroupAllPositons(true);

        switch (battleState)
        {
            case GameStateEnum.CASTERTURN:

                //! if the entire casters team finish their turn, is enemy team turns
                if (casterUnitTurn >= castersOrderList.Count - 1)
                {
                    GetCurrentCaster().AsUnit().InvokeSelfTurnEndEvent();
                    casterUnitTurn = 0;
                    battleState = GameStateEnum.ENEMYTURN;
                    enemy = battlestageManager.GetCurrentEnemy(enemyUnitTurn);
                    EnemyTurn();
                }
                else
                {
                    GetCurrentCaster().AsUnit().InvokeSelfTurnEndEvent();
                    //! Next caster turn
                    casterUnitTurn++;
                    CasterTurn();
                }

                break;

            case GameStateEnum.ENEMYTURN:

                //! if the entire casters team finish their turn, is enemy team turns
                if (enemyUnitTurn >= battlestageManager.GetEnemyTeam().Count - 1)
                {
                    Unit.InvokeAllTurnEndEvent();
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
        if (!highlighter)
            highlighter = Instantiate(highlighter);

        highlighter.SetActive(false);
        casterUnitTurn = 0;
        enemyUnitTurn = 0;

        yield return new WaitForSeconds(3f);

        battleState = GameStateEnum.CASTERTURN;
        print("yeet");
        CasterTurn();
    }

    IEnumerator CasterAttack()
    {
        UnitObject defenderUnit = null;
        SpriteRenderer defenderSR = null;
        cameraManager.ZoomToCenter();
        highlighter.SetActive(false);
        battlestageManager.GetStageTargetHandler().DeactivateSpriteHolder();
        battlestageManager.ExecuteAction(caster, battlestageManager.GetSelectedTarget(), true);

        if (battlestageManager.GetSelectedTarget())
        {
            defenderUnit = battlestageManager.GetSelectedTarget().GetComponent<Entity>().GetBaseUnit;
            defenderSR = battlestageManager.GetSelectedTarget().GetComponent<SpriteRenderer>();
            defenderSR.sprite = defenderUnit.FullBodyDamagedArt;
        }
        else print("Unit Not Found");

        //audioManager.PlayRandomSFX(defenderUnit.audioPack, "Hurt");
        
        while (!cameraManager.GetIsFree())
        {
            yield return null;
        }
        battlestageManager.GetStageTargetHandler().ActivateSpriteHolder();
        if (defenderSR)
            defenderSR.sprite = defenderUnit.FullBodyArt;
        EndTurn();
    }

    IEnumerator EnemyAttack()
    {
        cameraManager.ZoomToCenter();
        highlighter.SetActive(false);
        battlestageManager.GetStageTargetHandler().DeactivateSpriteHolder();
        enemyAttackManager.CalculatePriority(enemy);
        battlestageManager.ExecuteAction(enemy, enemyAttackManager.GetCaster(), false);
        infoManager.SyncUserInterfaceToUnit(GetCurrentCaster().AsUnit());

        UnitObject attackerUnit = enemy.GetComponent<Entity>().GetBaseUnit;
        SpriteRenderer attackSR = enemy.GetComponent<SpriteRenderer>();

        attackSR.sprite = attackerUnit.FullBodyAttackArt;

        //audioManager.PlayRandomSFX(attackerUnit.audioPack, "Attack");
        
        while (!cameraManager.GetIsFree())
        {
            yield return null;
        }
        battlestageManager.GetStageTargetHandler().ActivateSpriteHolder();
        if (attackSR)
            attackSR.sprite = attackerUnit.FullBodyArt;
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
using ConversionFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LerpFunctions;
using UnityEngine.Rendering;
using System.Collections;

public class BattlestageManager : MonoBehaviour
{
    public static BattlestageManager instance;

    public Transform battlestageCenter;
    public Transform playerTeamGroup, enemyTeamGroup;
    private TurnBaseManager turnBaseManager;
    private StageTargetHandler targetHandler;
    private InformationManager infoManager;
    public Camera battleCamera = null;

    [Header("Gaps Settings")] 
    public float centerGap;
    public float unitGap;
    public float battleGap = 1;

    [Header("Team's Positions")] 
    public Transform[] casterPositions;
    public Transform[] enemyPositions;
    public Transform[] casterEntityPositions;
    public Transform[] enemyEntityPositions;
    List<Transform> allLeftPositions;
    List<Transform> allRightPositions;
    List<Unit> allActiveLeftPositions;
    List<Unit> allActiveRightPositions;

    [Header("Hero Zoom Size")] 
    public float casterSize = 0.25f;

    private Player player;
    private RoomManager roomManager;

    List<GameObject> playerTeam = new List<GameObject>();
    List<GameObject> enemyTeam = new List<GameObject>();
    List<GameObject> playerEntityTeam = new List<GameObject>();
    List<GameObject> enemyEntityTeam = new List<GameObject>();

    [Header("Target Selection")] 
    [SerializeField] private GameObject selectedTarget = null;

    [Header("Death VFX")]
    public GameObject deathSmokeVFXRight;
    public GameObject deathSmokeVFXLeft;
    public Transform battleStageParent;

    [Header("Debug")] public bool enableEntityDebugging = false;

    public float speed = 7;
    public float countdownTimer;
    bool allowExecutionAction { get; set; }
    
    Transform casterExecutionTransform, enemyExecutionTransform;
    Vector3 prevScaleCaster, prevScaleEnemy;
    SortingGroup casterSortGroup, enemySortingGroup;

    float timer;
    bool isRunningEnd;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        
        Unit.SubscribeDeathEvent(KillUnit);
    }

    private void Start()
    {
        player = Player.Instance;
        roomManager = RoomManager.Instance;
        turnBaseManager = TurnBaseManager.instance;
        infoManager = InformationManager.instance;
        InitPositions();

        if (enableEntityDebugging)
        {
            Debug.LogWarning("BattlestageManager: Debugging Enabled");
        }

        SetTarget(enemyTeam[0]);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = battleCamera.ScreenPointToRay(touch.position);
                RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray);
                if (hitInfo.collider != null) SelectTarget(hitInfo);
            }
        }

        if (enableEntityDebugging)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) SpawnRandomEntityLeft();
            if (Input.GetKeyDown(KeyCode.Backspace)) SpawnRandomEntityRight();
        }

        if (allowExecutionAction)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;

                if (casterExecutionTransform)
                    casterExecutionTransform.position =
                    Vector3.Lerp(casterExecutionTransform.position, new Vector3(battlestageCenter.position.x - battleGap,
                            casterExecutionTransform.position.y, casterExecutionTransform.position.z),
                        speed * Time.unscaledDeltaTime);

                if (enemyExecutionTransform)
                    enemyExecutionTransform.position =
                        Vector3.Lerp(enemyExecutionTransform.position, new Vector3(battlestageCenter.position.x + battleGap,
                                enemyExecutionTransform.position.y, enemyExecutionTransform.position.z),
                            speed * Time.unscaledDeltaTime);
            }
            else
            {
                if (casterExecutionTransform)
                    casterExecutionTransform.localScale = prevScaleCaster;
                
                if (enemyExecutionTransform)
                    enemyExecutionTransform.localScale = prevScaleEnemy;

                if (casterExecutionTransform)
                    casterExecutionTransform.position = Vector3.Lerp(casterExecutionTransform.position,
                    new Vector3(casterExecutionTransform.parent.position.x, casterExecutionTransform.parent.position.y, casterExecutionTransform.parent.position.z), speed * Time.unscaledDeltaTime);
                
                if (enemyExecutionTransform)
                    enemyExecutionTransform.position = Vector3.Lerp(enemyExecutionTransform.position,
                    new Vector3(enemyExecutionTransform.parent.position.x, enemyExecutionTransform.parent.position.y, enemyExecutionTransform.parent.position.z), speed * Time.unscaledDeltaTime);
                
            }
        }
    }

    public void SetStageTargetHandler(StageTargetHandler stageTargetHandler) => targetHandler = stageTargetHandler;

    public void ResetSortingOrder()
    {
        if (casterSortGroup)
            casterSortGroup.sortingOrder = 0;
        
        if (enemySortingGroup)
            enemySortingGroup.sortingOrder = 0;
        allowExecutionAction = false;
    }

    public void ExecuteAction(GameObject _caster, GameObject _enemy, bool isPlayerTurn)
    {
        timer = countdownTimer;

        if (isPlayerTurn)
        {
            casterExecutionTransform = _caster.transform;
            prevScaleCaster = casterExecutionTransform.localScale;
            casterExecutionTransform.localScale = new Vector3(casterSize, casterSize, 1);
            casterSortGroup = _caster.GetComponent<SortingGroup>();
            casterSortGroup.sortingOrder = 1;

            if (GetSelectedTarget())
            {
                enemyExecutionTransform = _enemy.transform;
                enemySortingGroup = _enemy.GetComponent<SortingGroup>();
                prevScaleEnemy = enemyExecutionTransform.localScale;
                enemyExecutionTransform.localScale = new Vector3(casterSize, casterSize, 1);
                enemySortingGroup.sortingOrder = 1;
            }
        }
        else
        {
            casterExecutionTransform = _enemy.transform;
            prevScaleCaster = casterExecutionTransform.localScale;
            casterExecutionTransform.localScale = new Vector3(casterSize, casterSize, 1);
            casterSortGroup = _enemy.GetComponent<SortingGroup>();
            casterSortGroup.sortingOrder = 1;

            if (GetSelectedTarget())
            {
                enemyExecutionTransform = _caster.transform;
                enemySortingGroup = _caster.GetComponent<SortingGroup>();
                prevScaleEnemy = enemyExecutionTransform.localScale;
                enemyExecutionTransform.localScale = new Vector3(casterSize, casterSize, 1);
                enemySortingGroup.sortingOrder = 1;
            }
        }

        allowExecutionAction = true;
    }

    public void Button_ActivateSkill()
    {
        var target = selectedTarget.AsUnit();
        var unit = turnBaseManager.GetCurrentCaster().AsUnit();
        if (SkillCriteriaNotMet()) return;
        bool SkillCriteriaNotMet() => unit == null || !unit.SkillIsReady || unit.GetActiveSkill == null;
        try
        {
            TargetInfo targetInfo = unit.GetActiveSkill.GetActiveSkillTargets(new TargetInfo(target, null, null, GetCasterTeamAsUnit(), GetEnemyTeamAsUnit(), GetActiveLeftPositions(), GetActiveRightPositions()));
            unit.UseSkill(targetInfo, this);
            unit.InvokeUseSkillEvent(unit, unit.GetActiveSkill);
            infoManager.SyncUserInterfaceToUnit(unit);
        }
        catch(Exception exception)
        {
            string skillName = string.Empty;
            if (unit.GetActiveSkill != null) skillName = unit.GetActiveSkill.Name;
            Debug.LogWarning($"Cannot activate skill {skillName}: {exception}");
        }
    }

    public void RegroupAllPositons(bool instant)
    {
        RegroupLeftPositions(instant);
        RegroupRightPositions(instant);
    }

    public void RegroupLeftPositions(bool instant)
    {
        ResetActiveLeftPositions();
        int activeMult = 0;
        if (instant)
        {
            foreach (Transform obj in allLeftPositions)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    obj.localPosition = new Vector2(-(unitGap * activeMult), obj.localPosition.y);
                    activeMult++;

                    if (obj.childCount == 0) continue;
                    AddActiveLeftPosition(obj.GetChild(0).AsUnit());
                }
            }
        }
        else
        {
            foreach (Transform obj in allLeftPositions)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    obj.GetComponent<UnitPositionBehavior>()
                        .SetTargetPosition(new Vector2(-(unitGap * activeMult), obj.localPosition.y));
                    activeMult++;

                    if (obj.childCount == 0) continue;
                    AddActiveLeftPosition(obj.GetChild(0).AsUnit());
                }
            }
        }
    }

    public void RegroupRightPositions(bool instant)
    {
        ResetActiveRightPositions();
        int activeMult = 0;
        if (instant)
        {
            foreach (Transform obj in allRightPositions)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    obj.localPosition = new Vector2((unitGap * activeMult), obj.localPosition.y);
                    activeMult++;

                    if (obj.childCount == 0) continue;
                    AddActiveRightPosition(obj.GetChild(0).AsUnit());
                }
            }
        }
        else
        {
            foreach (Transform obj in allRightPositions)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    obj.GetComponent<UnitPositionBehavior>()
                        .SetTargetPosition(new Vector2((unitGap * activeMult), obj.localPosition.y));
                    activeMult++;

                    if (obj.childCount == 0) continue;
                    AddActiveRightPosition(obj.GetChild(0).AsUnit());
                }
            }
        }
    }

    public void AssignEnemiesToRoom()
    {
        int index = roomManager.GetCurrentRoomIndex;
        RoomSetUp theRoom = roomManager.Level[index];
        if (theRoom.roomSO)
        {
            if (theRoom.isRandom)
            {
                List<EnemyData> availableEnemyType = new List<EnemyData>();
                for (int j = 0; j < theRoom.roomSO.enemies.Count; j++)
                {
                    if (availableEnemyType.Contains(theRoom.roomSO.enemies[j])) continue;
                    availableEnemyType.Add(theRoom.roomSO.enemies[j]);
                }

                int enemySize = UnityEngine.Random.Range(1, theRoom.maxEnemySize);

                for (int i = 0; i < enemySize; i++)
                {
                    int randomAvailableEnemy = UnityEngine.Random.Range(0, availableEnemyType.Count);
                    UnitObject unitObject = availableEnemyType[randomAvailableEnemy].enemySO;
                    GameObject temp = unitObject.InstantiateUnit(enemyPositions[i].position, Quaternion.identity, enemyPositions[i]);
                    temp.GetComponent<Foe>().SetCurrentLevelAndCalculate(availableEnemyType[randomAvailableEnemy].level);
                    enemyTeam.Add(temp);
                }
            }
            else
            {
                for (int i = 0; i < theRoom.roomSO.enemies.Count; i++)
                {
                    UnitObject unitObject = theRoom.roomSO.enemies[i].enemySO;
                    GameObject temp = unitObject.InstantiateUnit(enemyPositions[i].position, Quaternion.identity, enemyPositions[i]);
                    temp.GetComponent<Foe>().SetCurrentLevelAndCalculate(theRoom.roomSO.enemies[i].level);
                    enemyTeam.Add(temp);
                }
            }
        }
        RegroupRightPositions(true);
    }

    public void HandleTargetIfNull()
    {
        if (GetSelectedTarget() == null) SetGetFirstOrDefaultTarget();
    }

    void InitPositions()
    {
        //! Set gap between 2 teams
        playerTeamGroup.position = new Vector2(battlestageCenter.position.x - centerGap, battlestageCenter.position.y);
        enemyTeamGroup.position = new Vector2(battlestageCenter.position.x + centerGap, battlestageCenter.position.y);

        //! Initialize left side positions
        allLeftPositions = new List<Transform>();
        if (casterEntityPositions.Length <= casterPositions.Length)
            Debug.LogError("Caster entity positions less than casters!");

        allLeftPositions.Add(casterEntityPositions[0]);
        for (int i = 0; i < casterPositions.Length; i++)
        {
            allLeftPositions.Add(casterPositions[i]);
            allLeftPositions.Add(casterEntityPositions[i + 1]);
        }

        //! Deactivate entities
        foreach (Transform entity in casterEntityPositions) entity.gameObject.SetActive(false);

        //! Spawn casters
        for (int i = 0; i < player.UnitLoadOut.Count; i++)
        {
            var unitObject = player.UnitLoadOut[i].GetBaseUnit;
            if (unitObject)
            {
                playerTeam.Add(unitObject.InstantiateUnit(casterPositions[i].position, Quaternion.identity, casterPositions[i]));
            }
            
        }

        RegroupLeftPositions(false);

        //=================================================================\\

        //! Initialize right side positions
        allRightPositions = new List<Transform>();
        if (enemyEntityPositions.Length <= enemyPositions.Length)
            Debug.LogError("Enemy entity positions less than enemies!");

        allRightPositions.Add(enemyEntityPositions[0]);
        for (int i = 0; i < enemyPositions.Length; i++)
        {
            allRightPositions.Add(enemyPositions[i]);
            allRightPositions.Add(enemyEntityPositions[i + 1]);
        }

        //! Deactivate entities
        foreach (Transform entity in enemyEntityPositions) entity.gameObject.SetActive(false);

        //! Set Enemy's Position
        AssignEnemiesToRoom();
    }

    private void SelectTarget(RaycastHit2D info)
    {
        GameObject o = info.transform.gameObject;
        if (!o.CompareTag("Foe")) return;

        if (selectedTarget == o) SetTarget(null);
        else SetTarget(o);
    }

    private GameObject SetTarget(GameObject target)
    {
        selectedTarget = target;
        targetHandler.UpdateHandler(GetSelectedTarget());
        return selectedTarget;
    }

    void KillUnit(Unit u) => StartCoroutine(DeathDelay(u));

    IEnumerator DeathDelay(Unit u)
    {
        yield return new WaitForSeconds(2);

        if (GetCastersTeam().Contains(u.gameObject))
        {
            GetCastersTeam().Remove(u.gameObject);
            Instantiate(deathSmokeVFXLeft, u.transform.position, Quaternion.identity, battleStageParent);
        }
        else if (GetEnemyTeam().Contains(u.gameObject))
        {
            GetEnemyTeam().Remove(u.gameObject);
            Instantiate(deathSmokeVFXRight, u.transform.position, Quaternion.identity, battleStageParent);
        }

        turnBaseManager.UpdateLiveTeam();
        if (u.gameObject == GetSelectedTarget()) selectedTarget = null;
        Destroy(u.gameObject);
        CheckIfEnemyTeamAlive();
    }

    public void CheckIfEnemyTeamAlive()
    {
        if (enemyTeam.Count <= 0)
            turnBaseManager.OnActionExecute();
    }
    
    void OnDestroy()
    {
        Unit.UnsubscribeDeathEvent(KillUnit);
    }

    private void ResetActiveLeftPositions() => allActiveLeftPositions = new List<Unit>();
    private void AddActiveLeftPosition(Unit unit) => allActiveLeftPositions.Add(unit);
    private void ResetActiveRightPositions() => allActiveRightPositions = new List<Unit>();
    private void AddActiveRightPosition(Unit unit) => allActiveRightPositions.Add(unit);

    #region Accessors

    public GameObject GetCurrentCaster(int getCaster) => playerTeam[getCaster];
    public GameObject GetCurrentEnemy(int getEnemy) => enemyTeam[getEnemy];

    public List<GameObject> GetCastersTeam() => playerTeam;
    public List<GameObject> GetEnemyTeam() => enemyTeam;
    public List<GameObject> GetCasterEntityTeam() => playerEntityTeam;
    public List<GameObject> GetEnemyEntityTeam() => enemyEntityTeam;

    public List<Unit> GetCasterTeamAsUnit()
    {
        var list = new List<Unit>();
        GetCastersTeam().ForEach(o => list.Add(o.GetComponent<Unit>()));
        return list;
    }
    public List<Unit> GetEnemyTeamAsUnit()
    {
        var list = new List<Unit>();
        GetEnemyTeam().ForEach(o => list.Add(o.GetComponent<Unit>()));
        return list;
    }
    public List<Unit> GetCasterEntitiesAsUnit()
    {
        var list = new List<Unit>();
        GetCasterEntityTeam().ForEach(o => list.Add(o.GetComponent<Unit>()));
        return list;
    }
    public List<Unit> GetEnemyEntitiesAsUnit()
    {
        var list = new List<Unit>();
        GetEnemyEntityTeam().ForEach(o => list.Add(o.GetComponent<Unit>()));
        return list;
    }

    public List<Unit> GetActiveLeftPositions() => allActiveLeftPositions;
    public List<Unit> GetActiveRightPositions() => allActiveRightPositions;

    public GameObject GetSelectedTarget() => selectedTarget;
    public StageTargetHandler GetStageTargetHandler() => targetHandler;
    public GameObject SetGetFirstOrDefaultTarget() => SetTarget(GetEnemyTeam().FirstOrDefault(t => t != null));

    #endregion

    #region Debugs

    [ContextMenu("Spawn random entity left")]
    public void SpawnRandomEntityLeft()
    {
        int r = UnityEngine.Random.Range(0, casterEntityPositions.Length);
        casterEntityPositions[r].gameObject.SetActive(true);
        RegroupLeftPositions(false);
    }

    [ContextMenu("Spawn random entity right")]
    public void SpawnRandomEntityRight()
    {
        int r = UnityEngine.Random.Range(0, enemyEntityPositions.Length);
        enemyEntityPositions[r].gameObject.SetActive(true);
        RegroupRightPositions(false);
    }

    #endregion
}
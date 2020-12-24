using ConversionFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LerpFunctions;
using UnityEngine.Rendering;

public class BattlestageManager : MonoBehaviour
{
    public static BattlestageManager instance;

    public Transform battlestageCenter;
    public Transform playerTeamGroup, enemyTeamGroup;
    private TurnBaseManager _turnBaseManager;
    private Camera _mainCamera = null;

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

    [Header("Hero spawn (Debug use)")] 
    public GameObject heroes;

    private Player player;
    private RoomManager roomManager;

    List<GameObject> playerTeam = new List<GameObject>();
    List<GameObject> enemyTeam = new List<GameObject>();

    [Header("Target Selection")] 
    [SerializeField] private GameObject selectedTarget = null;

    [Header("Debug")] public bool enableEntityDebugging = false;

    public float speed = 7;
    public float countdownTimer;
    bool allowExecutionAction { get; set; }
    
    Transform casterExecutionTransform, enemyExecutionTransform;
    Vector3 prevPosCaster, prevPosEnemy;
    Vector3 prevScaleCaster, prevScaleEnemy;
    SortingGroup casterSortGroup, enemySortingGroup;

    float timer;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        player = Player.Instance;
        roomManager = RoomManager.Instance;
        _turnBaseManager = TurnBaseManager.instance;
        InitPositions();
        _mainCamera = Camera.main;

        if (enableEntityDebugging)
        {
            Debug.LogWarning("BattlestageManager: Debugging Enabled");
        }

        selectedTarget = enemyTeam[0];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
                SelectTarget(hitInfo);
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
                
                casterExecutionTransform.position =
                    Vector3.Lerp(casterExecutionTransform.position, new Vector3(battlestageCenter.position.x - battleGap,
                            casterExecutionTransform.position.y, casterExecutionTransform.position.z),
                        speed * Time.unscaledDeltaTime);

                if (GetSelectedTarget())
                {
                    enemyExecutionTransform.position =
                        Vector3.Lerp(enemyExecutionTransform.position, new Vector3(battlestageCenter.position.x + battleGap,
                                enemyExecutionTransform.position.y, enemyExecutionTransform.position.z),
                            speed * Time.unscaledDeltaTime);
                }
                
            }
            else
            {
                casterExecutionTransform.localScale = prevScaleCaster;
                if (GetSelectedTarget())
                {
                    enemyExecutionTransform.localScale = prevScaleEnemy;
                }

                casterExecutionTransform.position = Vector3.Lerp(casterExecutionTransform.position,
                    new Vector3(prevPosCaster.x, prevPosCaster.y, prevPosCaster.z), speed * Time.unscaledDeltaTime);

                if (GetSelectedTarget())
                {
                    enemyExecutionTransform.position = Vector3.Lerp(enemyExecutionTransform.position,
                        new Vector3(prevPosEnemy.x, prevPosEnemy.y, prevPosEnemy.z), speed * Time.unscaledDeltaTime);
                }

                if (GetSelectedTarget())
                {
                    if (Lerp.NegligibleDistance(casterExecutionTransform.position.x, prevPosCaster.x, 0.001f) 
                        && Lerp.NegligibleDistance(enemyExecutionTransform.position.x, prevPosEnemy.x, 0.001f))
                    {
                        casterSortGroup.sortingOrder = 0;
                        enemySortingGroup.sortingOrder = 0;
                        allowExecutionAction = false;
                    }
                }
                else
                {
                    if (Lerp.NegligibleDistance(casterExecutionTransform.position.x, prevPosCaster.x, 0.001f))
                    {
                        casterSortGroup.sortingOrder = 0;
                        enemySortingGroup.sortingOrder = 0;
                        allowExecutionAction = false;
                    }
                }
            }
            
            
        }
    }

    public void ExecuteAction(GameObject _caster, GameObject _enemy)
    {
        timer = countdownTimer;
        casterExecutionTransform = _caster.transform;
        prevPosCaster = casterExecutionTransform.position;
        prevScaleCaster = casterExecutionTransform.localScale;
        casterExecutionTransform.localScale = new Vector3(0.25f, 0.25f, 1);
        casterSortGroup = _turnBaseManager.GetCurrentCaster().GetComponent<SortingGroup>();
        casterSortGroup.sortingOrder = 1;

        if (GetSelectedTarget())
        {
            enemyExecutionTransform = _enemy.transform;
            enemySortingGroup = GetSelectedTarget().GetComponent<SortingGroup>();
            prevPosEnemy = enemyExecutionTransform.position;
            prevScaleEnemy = enemyExecutionTransform.localScale;
            enemyExecutionTransform.localScale = new Vector3(-0.25f, 0.25f, 1);
            enemySortingGroup.sortingOrder = 1;
        }


        allowExecutionAction = true;
    }

    public void Button_ActivateSkill()
    {
        var target = selectedTarget.AsUnit();
        var unit = _turnBaseManager.GetCurrentCaster().AsUnit();
        if (unit == null) return;
        unit.UseSkill(target, (List<Unit>) GetCasterTeamAsUnit(), (List<Unit>) GetEnemyTeamAsUnit());
    }

    public void RegroupAllPositons(bool instant)
    {
        RegroupLeftPositions(instant);
        RegroupRightPositions(instant);
    }

    public void RegroupLeftPositions(bool instant)
    {
        int activeMult = 0;
        if (instant)
        {
            foreach (Transform obj in allLeftPositions)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    obj.localPosition = new Vector2(-(unitGap * activeMult), obj.localPosition.y);
                    activeMult++;
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
                }
            }
        }
    }

    public void RegroupRightPositions(bool instant)
    {
        int activeMult = 0;
        if (instant)
        {
            foreach (Transform obj in allRightPositions)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    obj.localPosition = new Vector2((unitGap * activeMult), obj.localPosition.y);
                    activeMult++;
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
                }
            }
        }
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
        for (int i = 0; i < casterPositions.Length; i++)
        {
            GameObject loadOutUnit = player.UnitLoadOut[i].BaseUnit.FullArtPrefab;
            GameObject temp = Instantiate(loadOutUnit, casterPositions[i].position, Quaternion.identity,
                casterPositions[i]);
            playerTeam.Add(temp);
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
        RoomSetUp tempRoom = roomManager.Rooms[0];
        if (tempRoom.isRandom)
        {
            List<EnemyData> availableEnemyType = new List<EnemyData>(0);
            for (int j = 0; j < tempRoom.roomSO.enemies.Count; j++)
            {
                if (!availableEnemyType.Contains(tempRoom.roomSO.enemies[j]))
                {
                    availableEnemyType.Add(tempRoom.roomSO.enemies[j]);
                }
            }

            int enemySize = UnityEngine.Random.Range(1, tempRoom.maxEnemySize);

            for (int i = 0; i < enemySize; i++)
            {
                int randomAvailableEnemy = UnityEngine.Random.Range(0, availableEnemyType.Count);
                GameObject loadOutUnit = availableEnemyType[randomAvailableEnemy].enemySO.FullArtPrefab;
                GameObject temp = Instantiate(loadOutUnit, enemyPositions[i].position, Quaternion.identity,
                    enemyPositions[i]);
                temp.GetComponent<Foe>().SetCurrentLevel(availableEnemyType[randomAvailableEnemy].level);
                enemyTeam.Add(temp);
            }
        }
        else
        {
            for (int i = 0; i < tempRoom.roomSO.enemies.Count; i++)
            {
                GameObject loadOutUnit = tempRoom.roomSO.enemies[i].enemySO.FullArtPrefab;
                //! Bottom codes should not be use for actual gameplay
                GameObject temp = Instantiate(loadOutUnit, enemyPositions[i].position, Quaternion.identity,
                    enemyPositions[i]);
                temp.GetComponent<Foe>().SetCurrentLevel(tempRoom.roomSO.enemies[i].level);
                enemyTeam.Add(temp);
            }
        }

        RegroupRightPositions(false);
    }

    private void SelectTarget(RaycastHit info)
    {
        GameObject o = info.transform.gameObject;
        if (!o.CompareTag("Foe")) return;

        if (selectedTarget == o) selectedTarget = null;
        else selectedTarget = o;
    }


    #region Accessors

    public GameObject GetCurrentCaster(int getCaster) => playerTeam[getCaster];
    public GameObject GetCurrentEnemy(int getEnemy) => enemyTeam[getEnemy];

    public List<GameObject> GetCastersTeam() => playerTeam;
    public List<GameObject> GetEnemyTeam() => enemyTeam;

    public IEnumerable<Unit> GetCasterTeamAsUnit()
    {
        var list = new List<Unit>();
        GetCastersTeam().ForEach(o => list.Add(o.GetComponent<Unit>()));
        return list;
    }

    public IEnumerable<Unit> GetEnemyTeamAsUnit()
    {
        var list = new List<Unit>();
        GetEnemyTeam().ForEach(o => list.Add(o.GetComponent<Unit>()));
        return list;
    }

    public GameObject GetSelectedTarget() => selectedTarget;

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
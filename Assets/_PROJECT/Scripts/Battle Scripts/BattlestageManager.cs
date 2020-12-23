using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlestageManager : MonoBehaviour
{
    public static BattlestageManager instance;
    
    public Transform battlestageCenter;
    public Transform playerTeamGroup, enemyTeamGroup;

    [Header("Gaps Settings")]
    public float centerGap;
    public float unitGap;
    
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
    private Camera _mainCamera = null;

    [Header("Debug")]
    public bool enableEntityDebugging = false;

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
        InitPositions();
        _mainCamera = Camera.main;

        if (enableEntityDebugging)
        {
            Debug.LogWarning("BattlestageManager: Debugging Enabled");
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
            {
                GameObject o = hitInfo.transform.gameObject;
                if(o.CompareTag("Foe"))
                {
                    if (selectedTarget == o) selectedTarget = null;
                    else selectedTarget = o;
                }
            }
        }

        if (enableEntityDebugging)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) SpawnRandomEntityLeft();
            if (Input.GetKeyDown(KeyCode.Backspace)) SpawnRandomEntityRight();
        }
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
                    obj.GetComponent<UnitPositionBehavior>().SetTargetPosition(new Vector2(-(unitGap * activeMult), obj.localPosition.y));
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
                    obj.GetComponent<UnitPositionBehavior>().SetTargetPosition(new Vector2((unitGap * activeMult), obj.localPosition.y));
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
        if (casterEntityPositions.Length <= casterPositions.Length) Debug.LogError("Caster entity positions less than casters!");

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
            GameObject temp = Instantiate(loadOutUnit, casterPositions[i].position, Quaternion.identity, casterPositions[i]);
            playerTeam.Add(temp);
        }

        RegroupLeftPositions(false);

        //=================================================================\\

        //! Initialize right side positions
        allRightPositions = new List<Transform>();
        if (enemyEntityPositions.Length <= enemyPositions.Length) Debug.LogError("Enemy entity positions less than enemies!");

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
                GameObject temp = Instantiate(loadOutUnit, enemyPositions[i].position, Quaternion.identity, enemyPositions[i]);
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
                GameObject temp = Instantiate(loadOutUnit, enemyPositions[i].position, Quaternion.identity, enemyPositions[i]);
                temp.GetComponent<Foe>().SetCurrentLevel(tempRoom.roomSO.enemies[i].level);
                print("new level" + temp.GetComponent<Foe>().GetCurrentLevel);
                enemyTeam.Add(temp);
            }
        }

        RegroupRightPositions(false);
    }

    #region Accessors

    public GameObject GetCurrentCaster(int getCaster) => playerTeam[getCaster];
    public GameObject GetCurrentEnemy(int getEnemy) => enemyTeam[getEnemy];

    public List<GameObject> GetCastersTeam() => playerTeam;
    public List<GameObject> GetEnemyTeam() => enemyTeam;
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

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
    public float heroGap;
    
    [Header("Team's Positions")]
    public Transform[] leftSidePos;
    public Transform[] rightSidePos;

    [Header("Hero spawn (Debug use)")] 
    public GameObject heroes;

    private Player player;
    private RoomManager roomManager;

    GameObject[] playerTeam = new GameObject[4];
    List<GameObject> enemyTeam = new List<GameObject>();

    [Header("Target Selection")]
    [SerializeField] private GameObject selectedTarget = null;
    private Camera _mainCamera = null;

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
    }

    void InitPositions()
    {
        //! Set gap between 2 teams
        playerTeamGroup.position = new Vector2(battlestageCenter.position.x - centerGap, battlestageCenter.position.y);
        enemyTeamGroup.position = new Vector2(battlestageCenter.position.x + centerGap, battlestageCenter.position.y);

        //! Set Player hero's position
        for (int i = 0; i < leftSidePos.Length; i++)
        {
            GameObject loadOutUnit = player.UnitLoadOut[i].BaseUnit.FullArtPrefab;

            leftSidePos[i].localPosition = new Vector2(leftSidePos[i].localPosition.x - (heroGap * i), leftSidePos[i].localPosition.y);

            GameObject temp = Instantiate(loadOutUnit, leftSidePos[i].position, Quaternion.identity, leftSidePos[i]);
            playerTeam[i] = temp;
        }

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
                loadOutUnit.GetComponent<Foe>().SetCurrentLevel(availableEnemyType[randomAvailableEnemy].level);
                rightSidePos[i].localPosition = new Vector2(rightSidePos[i].localPosition.x + (heroGap * i),
                    rightSidePos[i].localPosition.y);

                GameObject temp = Instantiate(loadOutUnit, rightSidePos[i].position, Quaternion.identity, rightSidePos[i]);
                enemyTeam.Add(temp);
            }
        }
        else
        {
            for (int i = 0; i < tempRoom.roomSO.enemies.Count; i++)
            {
                GameObject loadOutUnit = tempRoom.roomSO.enemies[i].enemySO.FullArtPrefab;
                loadOutUnit.GetComponent<Foe>().SetCurrentLevel(tempRoom.roomSO.enemies[i].level);
                rightSidePos[i].localPosition = new Vector2(rightSidePos[i].localPosition.x + (heroGap * i),
                    rightSidePos[i].localPosition.y);

                //! Bottom codes should not be use for actual gameplay
                GameObject temp = Instantiate(loadOutUnit, rightSidePos[i].position, Quaternion.identity, rightSidePos[i]);
                enemyTeam.Add(temp);
            }
        }
    }

    #region Accessors

    public GameObject GetCurrentCaster(int getCaster) => playerTeam[getCaster];
    public GameObject GetCurrentEnemy(int getEnemy) => enemyTeam[getEnemy];

    public GameObject[] GetCastersTeam() => playerTeam;
    public List<GameObject> GetEnemyTeam() => enemyTeam;
    public GameObject GetSelectedTarget() => selectedTarget;

    #endregion

}

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
    private Enemy enemy;

    GameObject[] playerTeam = new GameObject[4];
    List<GameObject> enemyTeam = new List<GameObject>();
    

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
        enemy = Enemy.Instance;
        InitPositions();
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
        for (int i = 0; i < enemy.UnitLoadOut.Count; i++)
        {
            GameObject loadOutUnit = enemy.UnitLoadOut[i].BaseUnit.FullArtPrefab;
            rightSidePos[i].localPosition = new Vector2(rightSidePos[i].localPosition.x + (heroGap * i),
                rightSidePos[i].localPosition.y);
            
            //! Bottom codes should not be use for actual gameplay
            GameObject temp = Instantiate(loadOutUnit, rightSidePos[i].position, Quaternion.identity, rightSidePos[i]);
            enemyTeam.Add(temp);
        }

    }
    

    #region Accessors

    public GameObject GetCurrentCaster(int getCaster) => playerTeam[getCaster];
    public GameObject GetCurrentEnemy(int getEnemy) => enemyTeam[getEnemy];

    public GameObject[] GetCastersTeam() => playerTeam;
    public List<GameObject> GetEnemyTeam() => enemyTeam;

    #endregion

}

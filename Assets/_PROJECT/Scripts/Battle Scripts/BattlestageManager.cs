using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlestageManager : MonoBehaviour
{
    public static BattlestageManager instance;
    TurnBaseManager turnBaseManager;
    
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

    GameObject[] playerTeam = new GameObject[4];
    GameObject[] enemyTeam = new GameObject[4];

    Transform holder = null;
    Vector2 holderOriginalPos;
    bool isHolding = false;
    public Camera cam;

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
        InitPositions();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectHero();
        }

        if (isHolding)
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            holder.position = pos;
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            isHolding = false;
            holder.GetComponent<HeroBehaviour>().SetIsHeld(false);
            holder.position = holderOriginalPos;
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
        for (int i = 0; i < rightSidePos.Length; i++)
        {
            rightSidePos[i].localPosition = new Vector2(rightSidePos[i].localPosition.x + (heroGap * i), rightSidePos[i].localPosition.y);
            
            //! Bottom codes should not be use for actual gameplay
            GameObject temp = Instantiate(heroes, rightSidePos[i].position, Quaternion.identity, rightSidePos[i]);
            enemyTeam[i] = temp;
        }

    }

    void DetectHero()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Hero"))
            {

                holder = hit.collider.transform;
                print(holder.name);
                holder.GetComponent<HeroBehaviour>().SetIsHeld(true);

                for (int i = 0; i < playerTeam.Length; i++)
                {
                    if (playerTeam[i] == holder.gameObject)
                    {
                        holderOriginalPos = (Vector2)leftSidePos[i].position;
                    }
                }
                

                isHolding = true;
            }
           
        }
    }

    public void CheckPosition(GameObject toCompare)
    {
        for (int i = 0; i < playerTeam.Length; i++)
        {
            if (playerTeam[i] == toCompare)
            {
                playerTeam[i].transform.position = holderOriginalPos;
            }
        }
    }

    #region Accessors

    public GameObject GetCurrentCaster(int getCaster) => playerTeam[getCaster];
    public GameObject GetCurrentEnemy(int getEnemy) => enemyTeam[getEnemy];

    public GameObject[] GetCastersTeam() => playerTeam;
    public GameObject[] GetEnemyTeam() => enemyTeam;

    #endregion

}

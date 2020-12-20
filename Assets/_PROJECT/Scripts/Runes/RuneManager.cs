using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuneManager : MonoBehaviour
{
    public static RuneManager instance;
    PoolManager castPool;

    [Header("Debug Settings")]
    public bool allowMouse = true;

    [Header("Spawn Settings")]
    public Transform initialSpawn;

    [Header("Rune Settings")]
    public float maxVelocity;
    public int spawnNum;
    [SerializeField] GameState castingState = GameState.PRE_CASTING;

    int targetSpawn;
    [SerializeField] float leftSide, rightSide, topSide;
    float runeWidth;
    bool allowSpawn = false;
    Vector2 lastPos = new Vector2(0,0);
    
    List<GameObject> activeRuneList = new List<GameObject>();

    void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        else instance = this;
    }

    void Start()
    {
        castPool = PoolManager.instance;

        InitSpawn();
        TickSystem.OnTick += SpawningInterval;

        allowSpawn = true;
    }

    void SpawningInterval(object sender, TickSystem.OnTickEvent e)
    {
        if (allowSpawn)
        {
            int selfTick = e.tick;

            if (selfTick > targetSpawn)
            {
                //print("Check");
                SpawnItem();
                targetSpawn += Random.Range(1, 3);
            }
        }
    }

    void SpawnItem()
    {
        Vector2 tempPos;

        do
        {
            tempPos = new Vector2(Random.Range(leftSide + runeWidth, rightSide - runeWidth), topSide);
        } while (tempPos.x < lastPos.x - runeWidth || tempPos.x > lastPos.x + runeWidth);
        
        for (int i = 0; i < spawnNum; i++)
        {
            RuneType runeType = (RuneType) Random.Range(1, (int)RuneType.RUNE_TOTAL);
            GameObject item = castPool.GetPooledObject(runeType);
            if (item != null)
            {
                activeRuneList.Add(item);
                item.transform.SetParent(initialSpawn.transform);
                item.SetActive(true);
                item.GetComponent<Transform>().localPosition = tempPos;
                lastPos = tempPos;
            }
        }
    }

    void InitSpawn()
    {
        /*leftSide = initialSpawn.position.x - Screen.width * 0.5f;
        rightSide = initialSpawn.position.x + Screen.width * 0.5f;*/
        leftSide = initialSpawn.position.x - 3.0f;
        rightSide = initialSpawn.position.x + 3.0f;
        topSide = 0.0f;

        GameObject item = castPool.GetPooledObject(RuneType.ELECTRIC);
        SpriteRenderer runeSR = item.GetComponent<SpriteRenderer>();
        
        //runeWidth = runeSR.sprite.rect.width * 0.5f;
        runeWidth = 0.0f;

        print(runeWidth);
        Debug.DrawLine(new Vector3(leftSide, 10000.0f), new Vector3(leftSide, -10000.0f), Color.yellow, 10000.0f);
        Debug.DrawLine(new Vector3(rightSide, 10000.0f), new Vector3(rightSide, -10000.0f), Color.yellow, 10000.0f);
    }

    public void SpawnActivate()
    {
        allowSpawn = true;
    }

    public void SpawnDeactivate()
    {
        allowSpawn = false;
        while (activeRuneList.Count > 0)
        {
            activeRuneList[0].GetComponent<RuneBehaviour>().Deactivate();
        }
    }

    #region Queries
    public List<GameObject> GetActiveRuneList() => activeRuneList;
    #endregion

}

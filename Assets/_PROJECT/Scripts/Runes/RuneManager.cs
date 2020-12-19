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
    public GameObject spawner;
    public Transform initialSpawn;
    public RectTransform dropZone;

    [Header("Rune Settings")]
    public float maxVelocity;
    public int spawnNum;
    [SerializeField] GameState castingState = GameState.PRE_CASTING;

    int targetSpawn;
    public float leftSide, rightSide, topSide;
    float runeWidth;
    bool allowSpawn = false;
    
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
        for (int i = 0; i < spawnNum; i++)
        {
            RuneType runeType = (RuneType) Random.Range(1, 4);
            GameObject item = castPool.GetPooledObject(runeType);
            if (item != null)
            {
                activeRuneList.Add(item);
                item.transform.SetParent(initialSpawn.transform);
                item.SetActive(true);
                item.GetComponent<Transform>().localPosition = new Vector2(Random.Range(leftSide + runeWidth, rightSide - runeWidth), topSide);
            }
        }
    }

    void InitSpawn()
    {
        leftSide = initialSpawn.position.x - Screen.width * 0.5f;
        rightSide = initialSpawn.position.x + Screen.width * 0.5f;
        topSide = 0.0f;

        GameObject item = castPool.GetPooledObject(RuneType.ELECTRIC);
        SpriteRenderer runeSR = item.GetComponent<SpriteRenderer>();
        
        runeWidth = runeSR.sprite.rect.width * 0.5f;
        
        //print(runeWidth);
    }

    public void SpawnActivate()
    {
        allowSpawn = true;
    }

    public void SpawnDeactivate()
    {
        allowSpawn = false;
        /*foreach (GameObject rune in activeRuneList)
        {
            rune.GetComponent<RuneBehaviour>().Deactivate();
        }*/

        while (activeRuneList.Count > 0)
        {
            activeRuneList[0].GetComponent<RuneBehaviour>().Deactivate();
        }
    }

    #region Queries
    public List<GameObject> GetActiveRuneList() => activeRuneList;
    #endregion

}

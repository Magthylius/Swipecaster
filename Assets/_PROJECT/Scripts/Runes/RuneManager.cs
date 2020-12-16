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
    public RectTransform initialSpawn;
    public RectTransform dropZone;

    [Header("Rune Settings")]
    public float maxVelocity;
    public int spawnNum;
    [SerializeField] GameState castingState = GameState.PRE_CASTING;

    int targetSpawn;
    float leftSide, rightSide, topSide;
    float runeWidth, runeHeight;
    
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
    }

    #region Queries
    public List<GameObject> GetActiveRuneList() => activeRuneList;

    #endregion

    void SpawningInterval(object sender, TickSystem.OnTickEvent e)
    {
        int selfTick = e.tick;

        if (selfTick > targetSpawn)
        {
            //print("Check");
            SpawnItem();
            targetSpawn += Random.Range(1, 3);
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
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(leftSide + runeWidth, rightSide - runeWidth), topSide);
            }
        }
    }

    void InitSpawn()
    {
        Rect rect = initialSpawn.rect;
        
        leftSide = initialSpawn.anchoredPosition.x - rect.width / 2;
        rightSide = initialSpawn.anchoredPosition.x + rect.width / 2;
        topSide = 0.0f;
        
        GameObject item = castPool.GetPooledObject(RuneType.ELECTRIC);
        RectTransform runeRect = item.GetComponent<RectTransform>();
        
        runeWidth = (runeRect.offsetMax.x - runeRect.offsetMin.x) / 2;
        
        //print(runeWidth);
    }

}

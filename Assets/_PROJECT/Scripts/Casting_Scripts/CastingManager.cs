using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CastingManager : MonoBehaviour
{
    public static CastingManager instance;
    CastingPoolManager castPool;

    public GameObject spawner;
    public RectTransform initialSpawn;
    public GameObject candy;
    public RectTransform dropZone;

    [Header("Item's Max Velocity")]
    public float maxVel;
    public int spawnNum;

    [SerializeField] GameState castingState = GameState.PRE_CASTING;
    int targetSpawn;
    float leftSide, rightSide, topSide;
    float runeWidth, runeHeight;

    void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        else instance = this;
    }

    void Start()
    {
        castPool = CastingPoolManager.instance;
        Init();
        TickSystem.OnTick += SpawningInterval;
    }
    

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
            CastingPoolType pool = (CastingPoolType) Random.Range(1, 4);
            GameObject item = castPool.GetPooledObject(pool);
            if (item != null)
            {
                item.transform.SetParent(initialSpawn.transform);
                item.SetActive(true);
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(leftSide + runeWidth, rightSide - runeWidth), topSide);
            }
        }
    }

    void Init()
    {
        Rect rect = initialSpawn.rect;
        
        leftSide = initialSpawn.anchoredPosition.x - rect.width / 2;
        rightSide = initialSpawn.anchoredPosition.x + rect.width / 2;
        topSide = 0.0f;
        
        GameObject item = castPool.GetPooledObject(CastingPoolType.CANDY1);
        RectTransform runeRect = item.GetComponent<RectTransform>();
        
        runeWidth = (runeRect.offsetMax.x - runeRect.offsetMin.x) / 2;
        
        //print(runeWidth);
    }
    
    
}

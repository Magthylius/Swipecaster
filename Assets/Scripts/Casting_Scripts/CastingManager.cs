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
    public List<GameObject> spawnPosList = new List<GameObject>();


    [Header("Set Item's Max Velocity")]
    public float maxVel;

    [Header("Numbers of column indexes")]
    public int col;

    [Header("Interval")]
    public int intervalTime;

    [SerializeField] GameState castingState = GameState.PRE_CASTING;

    [SerializeField]int targetSpawn;

    bool[] slot;
    
    void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        else instance = this;
        
    }

    void Start()
    {
        castPool = CastingPoolManager.instance;
        TickSystem.OnTick += SpawningInterval;
    }



    void SpawningInterval(object sender, TickSystem.OnTickEvent e)
    {
        int selfTick = e.tick;


        if (selfTick > targetSpawn)
        {
            print("Check");
            SpawnItem();
            targetSpawn += intervalTime;
        }
        //print("Tick: " + selfTick);       
    }

    void SpawnItem()
    {
        slot = new bool[col];
        bool temp;
        
        int randItem = Random.Range(1, col);

        for (int i = 0; i < randItem; i++)
        {
            slot[i] = true;
        }

        for (int i = 0; i < slot.Length; i++)
        {
            int rnd = Random.Range(0, slot.Length);
            temp = slot[rnd];
            slot[rnd] = slot[i];
            slot[i] = temp;
        }

        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i])
            {
                GameObject item = castPool.GetPooledObject(CastingPoolType.ITEM);
                if (item != null)
                {
                    //item.transform.SetParent(dropZone, false);
                    item.SetActive(true);
                    item.GetComponent<RectTransform>().position = spawnPosList[i].GetComponent<RectTransform>().position;
                }
            }
        }
        

    }
    
}

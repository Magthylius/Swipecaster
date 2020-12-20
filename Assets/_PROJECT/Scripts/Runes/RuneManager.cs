using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RuneManager : MonoBehaviour
{
    public static RuneManager instance;
    PoolManager castPool;

    [Header("Debug Settings")]
    public bool allowMouse = true;
    public Transform leftMarginObj;
    public Transform rightMarginObj;

    [Header("Spawn Settings")]
    public Transform initialSpawn;
    public CanvasScaler referenceScale;
    public GameObject castingGroup;
    public float offsetMult = 0.1f;

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
        int loopCount = 0;
        do
        {
            tempPos = new Vector2(Random.Range(leftSide, rightSide), topSide);

            if (loopCount > 100)
            {
                Debug.LogError("Loop exceeded 100!");
                return;
            }

        } while (tempPos.x > lastPos.x - runeWidth && tempPos.x < lastPos.x + runeWidth);

        //tempPos = new Vector2(Random.Range(leftSide, rightSide), topSide);

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
        float widthRatio = referenceScale.referenceResolution.y / Screen.height;
        float heightRatio = referenceScale.referenceResolution.x / Screen.width;

        if ((float)Screen.height / Screen.width == referenceScale.referenceResolution.y / referenceScale.referenceResolution.x)
        {
            //print("ratioed");
            widthRatio = 1.0f;
            heightRatio = 1.0f;
        }

        float halfHeight = referenceScale.referenceResolution.y * 0.5f * heightRatio;
        float halfWidth = referenceScale.referenceResolution.x * 0.5f * widthRatio;

        float offsetMargin = referenceScale.referenceResolution.x * offsetMult * widthRatio;
        //print(offsetMargin);
        leftSide = initialSpawn.position.x - halfWidth + offsetMargin;
        rightSide = initialSpawn.position.x + halfWidth - offsetMargin;
        topSide = 0.0f;

        leftMarginObj.localPosition = new Vector2(leftSide, 0.0f);
        rightMarginObj.localPosition = new Vector2(rightSide, 0.0f);

        GameObject item = castPool.GetPooledObject(RuneType.ELECTRIC);
        SpriteRenderer runeSR = item.GetComponent<SpriteRenderer>();
        runeWidth = runeSR.sprite.rect.width * 0.5f ;
        //print(runeWidth);
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

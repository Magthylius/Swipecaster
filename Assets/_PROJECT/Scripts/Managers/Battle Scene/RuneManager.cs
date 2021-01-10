using LerpFunctions;
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
    public bool worldSpaceMode = false;

    [Header("Spawn Settings")] public bool enableSpawn = false;
    public Transform initialSpawn;
    public CanvasScaler referenceScale;
    public GameObject castingGroup;
    public int spawnDelay;
    public float offsetMult = 0.1f;

    [Header("Rune Settings")]
    public float maxVelocity;
    public int spawnNum;
    [SerializeField] GameState castingState = GameState.PRE_CASTING;

    int targetSpawn;
    [SerializeField] float leftSide, rightSide, topSide;
    float runeWidth = 100f;
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
        TickManager.OnTick += SpawningInterval;

    }

    void SpawningInterval(object sender, TickManager.OnTickEvent e)
    {
        int selfTick = e.tick;

        if (!allowSpawn) return;
        
        if (selfTick > targetSpawn)
        {
            SpawnItem();
            targetSpawn += spawnDelay;
        }
        
    }

    void SpawnItem()
    {
        Vector2 tempPos;

        //tempPos = new Vector2(Random.Range(leftSide, rightSide), topSide);

        for (int i = 0; i < spawnNum; i++)
        {
            int loopCount = 0;
            do
            {
                tempPos = new Vector2(Random.Range(leftSide, rightSide), topSide);
                loopCount++;
                if (loopCount > 100)
                {
                    Debug.LogError("Loop exceeded 100!");
                    allowSpawn = false;
                    return;
                }

                //print((tempPos - lastPos).sqrMagnitude);
                //if ((tempPos - lastPos).sqrMagnitude <= runeWidth * runeWidth) print((tempPos - lastPos).sqrMagnitude);

            } while ((tempPos - lastPos).sqrMagnitude <= runeWidth * runeWidth);

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
        if (worldSpaceMode)
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

            //GameObject item = castPool.GetPooledObject(RuneType.TEHK);
            //SpriteRenderer runeSR = item.GetComponent<SpriteRenderer>();
            //runeWidth = runeSR.sprite.rect.width * 0.5f;
            //print(runeWidth);
            return;
        }

        FlexibleRect cgRT = new FlexibleRect(castingGroup.GetComponent<RectTransform>());

        RectTransform rt = castPool.GetPooledObject(RuneType.TEHK).GetComponent<RectTransform>();
        runeWidth = rt.rect.width;

        float halfRWidth = runeWidth * 0.5f;
        leftSide = cgRT.center.x - cgRT.halfWidth + halfRWidth;
        rightSide = cgRT.center.x + cgRT.halfWidth - halfRWidth;
        topSide = 0.0f;

        //print(runeWidth);
    }

    public void SpawnActivate()
    {
        allowSpawn = true;
        TickManager.instance.StartTick();
    }

    public void SpawnDeactivate()
    {
        allowSpawn = false;
        while (activeRuneList.Count > 0)
        {
            activeRuneList[0].GetComponent<RuneBehaviour>().Deactivate();
        }
        TickManager.instance.StopTick();
    }

    #region Queries
    public List<GameObject> GetActiveRuneList() => activeRuneList;
    public float RuneWidth => runeWidth;
    #endregion

}

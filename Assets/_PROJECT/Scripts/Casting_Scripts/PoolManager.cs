using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    public GameObject prefab;
    public RuneType runeType;
    public int initialPoolCount;
    public bool canExpand;
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public List<PooledObject> pooledObjectsList = new List<PooledObject>();
    Dictionary<RuneType, List<GameObject>> poolList = new Dictionary<RuneType, List<GameObject>>();

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < pooledObjectsList.Count; i++)
        {
            List<GameObject> tempList = new List<GameObject>();
            for (int j = 0; j < pooledObjectsList[i].initialPoolCount; j++)
            {
                GameObject obj = (GameObject)Instantiate(pooledObjectsList[i].prefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                obj.transform.SetParent(transform, false);
                tempList.Add(obj);
            }
            poolList.Add(pooledObjectsList[i].runeType, tempList);
        }
    }

    public GameObject GetPooledObject(RuneType type)
    {
        List<GameObject> foundList = new List<GameObject>();
        if (poolList.TryGetValue(type, out foundList))
        {
            for (int i = 0; i < foundList.Count; i++)
            {
                if (!foundList[i].activeInHierarchy)
                {
                    return foundList[i];
                }
            }
        }

        for (int j = 0; j < pooledObjectsList.Count; j++)
        {
            PooledObject pooledObject = pooledObjectsList[j];
            if (pooledObject.runeType == type)
            {
                if (pooledObject.canExpand == true)
                {
                    GameObject expandedObj = (GameObject)Instantiate(pooledObject.prefab, Vector3.zero, Quaternion.identity);
                    foundList.Add(expandedObj);
                    expandedObj.SetActive(false);
                    expandedObj.transform.SetParent(this.transform, false);
                    return expandedObj;
                }
                else
                {
                    return null;
                }
            }
        }

        // check if pool can be expanded
        return null;
    }
}


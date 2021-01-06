using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParallaxType
{
    NULL = 0,
    BACKGROUND = 1,
    MIDGROUND,
    FOREGROUND,
}

[System.Serializable]
public struct ParallaxMultiplier
{
    public ParallaxType type;
    public float multiplier;
}

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance;
    public List<ParallaxBehaviour> parallaxObjList = new List<ParallaxBehaviour>();
    public List<ParallaxMultiplier> multiplierList = new List<ParallaxMultiplier>();
    
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        foreach (ParallaxBehaviour obj in parallaxObjList)
        {
            for (int i = 0; i < multiplierList.Count; i++)
            {
                if (obj.GetType() == multiplierList[i].type)
                {
                    obj.SetMultiplier(multiplierList[i].multiplier);
                }
            }
        }
        
    }

    void Start()
    {

    }

}

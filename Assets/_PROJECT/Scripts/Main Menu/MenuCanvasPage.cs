using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasPage : MonoBehaviour
{
    enum CanvasPageState
    { 
        IDLE = 0,
        ACTIVE,
    }

    public virtual void Reset()
    {
        Debug.LogWarning(gameObject.name + " no override Reset!");
    }
}

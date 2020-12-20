using System;
using EasyMobile;
using UnityEngine;

public class UnitPositionManager : MonoBehaviour
{
    public static UnitPositionManager instance;
    BattlestageManager battleStageManager;
    TurnBaseManager turnBaseManager;

    [SerializeField]GameObject holder;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        battleStageManager = BattlestageManager.instance;
        turnBaseManager = TurnBaseManager.instance;
    }

    void Update()
    {
        if (turnBaseManager.GetCurrentState() != GameStateEnum.CASTERTURN)
            return;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwapLeftPosition();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwapRightPosition();
        }
    }

    void SwapLeftPosition()
    {
        GameObject temp;
        int curCasterIndex;
        Vector2 tempHolderPos;

        curCasterIndex = Array.IndexOf(battleStageManager.GetCastersTeam(), holder);
        
        if (curCasterIndex + 1 > battleStageManager.GetCastersTeam().Length - 1)
        {
            print("Is too left");
            return;
        }
        else
        {
            temp = battleStageManager.GetCurrentCaster(curCasterIndex + 1);
            tempHolderPos = holder.transform.position;
                
            battleStageManager.GetCastersTeam()[curCasterIndex].transform.position = temp.transform.position;
            battleStageManager.GetCastersTeam()[curCasterIndex + 1].transform.position = tempHolderPos;
                
            battleStageManager.GetCastersTeam()[curCasterIndex + 1] = battleStageManager.GetCastersTeam()[curCasterIndex];
            battleStageManager.GetCastersTeam()[curCasterIndex] = temp;

        }
        
        temp = null;
        tempHolderPos = Vector2.zero;
    }

    void SwapRightPosition()
    {
        GameObject temp;
        int curCasterIndex;
        Vector2 tempHolderPos;

        curCasterIndex = Array.IndexOf(battleStageManager.GetCastersTeam(), holder);
        
        if (curCasterIndex - 1 < 0)
        {
            print("Is too left");
            return;
        }
        else
        {
            temp = battleStageManager.GetCurrentCaster(curCasterIndex - 1);
            tempHolderPos = holder.transform.position;
                
            battleStageManager.GetCastersTeam()[curCasterIndex].transform.position = temp.transform.position;
            battleStageManager.GetCastersTeam()[curCasterIndex - 1].transform.position = tempHolderPos;
                
            battleStageManager.GetCastersTeam()[curCasterIndex - 1] = battleStageManager.GetCastersTeam()[curCasterIndex];
            battleStageManager.GetCastersTeam()[curCasterIndex] = temp;
        }

        temp = null;
        tempHolderPos = Vector2.zero;
    }

    #region Accessors

    public GameObject SetHolder(GameObject _holder) => holder = _holder;

    #endregion


}

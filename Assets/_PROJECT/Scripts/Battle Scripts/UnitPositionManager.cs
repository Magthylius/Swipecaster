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
        if (!turnBaseManager.GetIsPLayerTurn())
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

    public void SwapLeftPosition()
    {
        if (turnBaseManager.GetCurrentState() != GameStateEnum.CASTERTURN)
            return;
        
        GameObject temp;
        int curCasterIndex;
        Vector2 tempHolderPos;

        curCasterIndex = battleStageManager.GetCastersTeam().IndexOf(holder);
        
        if (curCasterIndex + 1 > battleStageManager.GetCastersTeam().Count - 1)
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
        
        turnBaseManager.OnActionExecute();
        
        temp = null;
        tempHolderPos = Vector2.zero;
    }

    public void SwapRightPosition()
    {
        if (turnBaseManager.GetCurrentState() != GameStateEnum.CASTERTURN)
            return;
        
        GameObject temp;
        int curCasterIndex;
        Vector2 tempHolderPos;

        curCasterIndex = battleStageManager.GetCastersTeam().IndexOf(holder);
        
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

        turnBaseManager.OnActionExecute();
        
        temp = null;
        tempHolderPos = Vector2.zero;
    }

    #region Accessors

    public GameObject SetHolder(GameObject _holder) => holder = _holder;

    #endregion


}

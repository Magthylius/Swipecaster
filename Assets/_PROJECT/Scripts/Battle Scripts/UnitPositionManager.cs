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
        Vector2 tempHolderPos;

        for (int i = 0; i < battleStageManager.GetCastersTeam().Length; i++)
        {
            if (holder == battleStageManager.GetCurrentCaster(i))
            {
                if (i + 1 > battleStageManager.GetCastersTeam().Length - 1)
                {
                    print("Is too left");
                    return;
                }
                else
                {
                    temp = battleStageManager.GetCurrentCaster(i + 1);
                    tempHolderPos = holder.transform.position;
                    
                    battleStageManager.GetCastersTeam()[i].transform.position = temp.transform.position;
                    battleStageManager.GetCastersTeam()[i + 1].transform.position = tempHolderPos;
                    
                    battleStageManager.GetCastersTeam()[i + 1] = battleStageManager.GetCastersTeam()[i];
                    battleStageManager.GetCastersTeam()[i] = temp;
                    
                    break;
                }
            }
        }

        temp = null;
        tempHolderPos = Vector2.zero;
    }

    void SwapRightPosition()
    {
        GameObject temp;
        Vector2 tempHolderPos;

        for (int i = 0; i < battleStageManager.GetCastersTeam().Length; i++)
        {
            if (holder == battleStageManager.GetCurrentCaster(i))
            {
                if (i - 1 < 0)
                {
                    print("Is too right");
                    return;
                }
                else
                {
                    temp = battleStageManager.GetCurrentCaster(i - 1);
                    tempHolderPos = holder.transform.position;
                    
                    battleStageManager.GetCastersTeam()[i].transform.position = temp.transform.position;
                    battleStageManager.GetCastersTeam()[i - 1].transform.position = tempHolderPos;
                    
                    battleStageManager.GetCastersTeam()[i - 1] = battleStageManager.GetCastersTeam()[i];
                    battleStageManager.GetCastersTeam()[i] = temp;
                    
                    break;
                }
            }
        }

        temp = null;
        tempHolderPos = Vector2.zero;
    }

    #region Accessors

    public GameObject SetHolder(GameObject _holder) => holder = _holder;

    #endregion


}

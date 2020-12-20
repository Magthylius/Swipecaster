using UnityEngine;

public class UnitPositionManager : MonoBehaviour
{
    public static UnitPositionManager instance;
    BattlestageManager battleStageManager;
    TurnBaseManager turnBaseManager;
    
    Transform holder;
    Vector2 holderOriginalPos;
    bool isHolding;
    public Camera cam;

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
        if (Input.GetMouseButtonDown(0))
        {
            DetectHero();
        }

        if (isHolding)
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            holder.position = pos;
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            isHolding = false;
            holder.GetComponent<UnitSwapHandler>().SetIsHeld(false);
            holder.position = holderOriginalPos;
        }
    }
    
    void DetectHero()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Hero"))
            {

                holder = hit.collider.transform;
                print(holder.name);
                holder.GetComponent<UnitSwapHandler>().SetIsHeld(true);

                for (int i = 0; i < battleStageManager.GetCastersTeam().Length; i++)
                {
                    if (battleStageManager.GetCastersTeam()[i] == holder.gameObject)
                    {
                        holderOriginalPos = battleStageManager.GetCurrentCaster(i).transform.position;
                    }
                }

                isHolding = true;
            }
           
        }
    }
    
    public void CheckPosition(GameObject toCompare)
    {
        for (int i = 0; i < battleStageManager.GetCastersTeam().Length; i++)
        {
            if (battleStageManager.GetCurrentCaster(i) == toCompare)
            {
                battleStageManager.GetCurrentCaster(i).transform.position = holderOriginalPos;
            }
        }
    }
}

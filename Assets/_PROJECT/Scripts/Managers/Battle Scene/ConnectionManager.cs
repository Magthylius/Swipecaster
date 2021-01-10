using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager instance;
    ComboManager comboManager;

    public LineRenderer line;
    public UILineRenderer uiLine;
    
    RuneType selectionType;
    bool selectionStarted;
    float runeHalfWidth;
    Camera cam;

    List<RuneBehaviour> selectionList = new List<RuneBehaviour>();
    List<Vector2> linePosList = new List<Vector2>();

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        cam = Camera.main;
        line.gameObject.SetActive(false);
    }

    void Start()
    {
        comboManager = ComboManager.instance;
        runeHalfWidth = RuneManager.instance.RuneWidth;
        UpdateLines();
    }

    void Update()
    {
        /*
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rune1"))
                {
                    print(hit.collider.name);
                    Transform tempTransform = hit.collider.transform;
                    RuneBehaviour tempRB = tempTransform.GetComponent<RuneBehaviour>();

                    if (!tempRB.GetSelected())
                    {
                        
                        tempRB.Selected();
                        line.gameObject.SetActive(true);
                    }

                    if (selectionList.Count >= 2)
                    {
                        if (!comboManager.GetIsStart())
                        {
                            comboManager.SetCountdownTimer();
                            comboManager.SetIsStart();
                        }
                    }
                }
            }

        }


        if (selectionStarted)
        {
            Vector3[] posList = new Vector3[selectionList.Count];

            for (int i = 0; i < selectionList.Count; i++)
            {
                Vector2 temp = selectionList[i].GetPosition();

                
                Vector3 temp2 = new Vector3(temp.x, temp.y, -5);

                posList[i] = temp2;
            }

            line.positionCount = selectionList.Count;
            line.SetPositions(posList);

            if (Input.GetMouseButtonUp(0))
            {
                selectionStarted = false;
                Time.timeScale = 1.0f;
                line.gameObject.SetActive(false);

                if (selectionList.Count >= 2)
                {
                    comboManager.CollectDamage();
                }

                foreach (var rune in selectionList)
                {
                    rune.GetComponent<RuneBehaviour>().ResetToActivateSprite();
                }
            }
        } */

        if (selectionStarted)
        {
            //print(Input.touchCount);
            if (Input.GetMouseButtonUp(0))
            {
                selectionStarted = false;
                Time.timeScale = 1.0f;
                line.gameObject.SetActive(false);

                if (selectionList.Count >= 2)
                {
                    comboManager.CollectDamage();
                }

                foreach (var rune in selectionList)
                {
                    rune.GetComponent<RuneBehaviour>().ResetToActivateSprite();
                }

                selectionList = new List<RuneBehaviour>();
                linePosList = new List<Vector2>();
                uiLine.UpdatePoints(linePosList);
            }
        }    
    }

    public void LateUpdate()
    {
        if (selectionList.Count > 0)
        {
            UpdateLines();
        }
    }

    void UpdateLines()
    {
        linePosList = new List<Vector2>();
        foreach (RuneBehaviour rune in selectionList)
        {
            Vector2 pos = rune.runeCenter;
            pos.y += uiLine.gridSize.y * 0.5f - runeHalfWidth;
            linePosList.Add(pos);
            //print(pos);
        }
        uiLine.UpdatePoints(linePosList);
    }

    public void StartSelection(RuneBehaviour rune)
    {
        selectionStarted = true;
        selectionType = rune.type;
        selectionList = new List<RuneBehaviour>();
        selectionList.Add(rune);

        Time.timeScale = 0.2f;
    }

    public void Connect(RuneBehaviour rune)
    {
        selectionList.Add(rune);
    }

    public void Disconnect(RuneBehaviour rune)
    {
        foreach (RuneBehaviour r in selectionList)
        {
            if (r == rune)
            {
                selectionList.Remove(r);
                //! redo lines
                return;
            }
        }
    }

    #region Queries

    public bool GetSelectionStart() => selectionStarted;
    public RuneType GetSelectionType() => selectionType;

    public List<RuneBehaviour> GetSelectedRuneList() => selectionList;
    

    #endregion
}
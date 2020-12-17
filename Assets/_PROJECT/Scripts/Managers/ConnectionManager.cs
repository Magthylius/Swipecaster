﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager instance;
    public LineRenderer line;

    RuneType selectionType;
    bool selectionStarted = false;
    Camera cam;
    List<RuneBehaviour> selectionList = new List<RuneBehaviour>();

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        
        cam = Camera.main;
        line.gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rune1"))
                {
                    Transform tempTransform = hit.collider.transform;
                    RuneBehaviour tempRB = tempTransform.GetComponent<RuneBehaviour>();

                    if (!tempRB.GetSelected())
                    {
                        tempRB.Selected();
                        line.gameObject.SetActive(true);
                    }

                }
            }
        }
        
        
        if (selectionStarted)
        {
            Vector3[] posList = new Vector3[selectionList.Count];
            for (int i = 0; i < selectionList.Count; i++)
            {
                posList[i] = selectionList[i].GetPosition();
            }
            line.positionCount = selectionList.Count;
            line.SetPositions(posList);

            if(Input.GetMouseButtonUp(0))
            {
                selectionStarted = false;
                Time.timeScale = 1.0f;
                line.gameObject.SetActive(false);

                for (int i = 0; i < selectionList.Count; i++)
                {
                    selectionList[i].GetComponent<RuneBehaviour>();
                }
                
            }
        }
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
    #endregion
}

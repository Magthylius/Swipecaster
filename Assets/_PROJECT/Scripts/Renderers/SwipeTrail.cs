using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrail : MonoBehaviour
{
    RectTransform rt;
    Vector2 offset;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        offset = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void Update()
    {
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePos.x, mousePos.y, -11);
        if (Input.GetMouseButton(0))
        {
            rt.anchoredPosition = (Vector2)Input.mousePosition - offset;
            //print(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            rt.anchoredPosition = Input.GetTouch(0).position - offset;
        }
    }
}

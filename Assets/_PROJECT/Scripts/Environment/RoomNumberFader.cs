using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LerpFunctions;

public class RoomNumberFader : MonoBehaviour
{
    public TextMeshProUGUI roomText;
    public float transitionSpeed = 15f;
    public float fadeDelay = 4f;

    RoomManager roomManager;
    UIFader roomTextUIF;
    

    void Start()
    {
        roomManager = RoomManager.Instance;
        roomTextUIF = new UIFader(roomText);

        roomTextUIF.ForceTransparent();
    }

    // Update is called once per frame
    void Update()
    {
        roomTextUIF.Step(transitionSpeed * Time.unscaledDeltaTime);
    }

    public void HideRoomText()
    {
        roomTextUIF.ForceOpaque();
    }

    public void ShowRoomText()
    {
        roomText.text = "ROOM " + (roomManager.GetCurrentRoomIndex + 1).ToString();
        //roomText.color
        roomTextUIF.ForceOpaque();

        StartCoroutine(DelayFade(fadeDelay));
        
    }

    IEnumerator DelayFade(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        roomTextUIF.FadeToTransparent();
    }
}

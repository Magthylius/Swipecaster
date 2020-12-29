using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LerpFunctions;

public class PartyCanvasManager : MenuCanvasPage
{
    public struct PartyGroup
    {

    }

    public static PartyCanvasManager instance;

    public CanvasGroup interactionGroup;
    public CanvasGroup configurationGroup;

    CanvasGroupFader interactionCGF;
    CanvasGroupFader configurationCGF;

    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        else instance = this;

        interactionCGF = new CanvasGroupFader(interactionGroup, true, true);
        configurationCGF = new CanvasGroupFader(configurationGroup, true, true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

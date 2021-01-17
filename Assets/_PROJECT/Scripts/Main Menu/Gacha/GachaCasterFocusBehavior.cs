using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LerpFunctions;

public class GachaCasterFocusBehavior : MonoBehaviour
{
    [Header("References")]
    public CasterRarityUIBehavior rarityBehav;
    public RectTransform bgRectTr;
    public RectTransform fgRectTr;
    public RectTransform casterRectTr;

    public TextMeshProUGUI casterName;
    public Image casterImage;
    public GameObject continueObject;

    FlexibleRect fgRectFR;
    FlexibleRect casterRectFR;
    FlexibleRectCorners bgRectFRC;

    [Header("Settings")]
    public float transitionSpeed = 15f;
    bool transitionFinished = true;

    [Header("Hiding Objects")]
    public List<GameObject> hidingObjects;

    void Start()
    {
        fgRectFR = new FlexibleRect(fgRectTr);
        casterRectFR = new FlexibleRect(casterRectTr);
        bgRectFRC = new FlexibleRectCorners(bgRectTr);

        ResetFocus();
    }

    void Update()
    {
        fgRectFR.Step(transitionSpeed * Time.unscaledDeltaTime);
        casterRectFR.Step(transitionSpeed * Time.unscaledDeltaTime);
        bgRectFRC.CornerStep(transitionSpeed * Time.unscaledDeltaTime);

        if (!transitionFinished)
        {
            transitionFinished = !(fgRectFR.IsTransitioning || casterRectFR.IsTransitioning || bgRectFRC.IsTransitioning);
        }
    }

    void Setup(UnitObject unit)
    {
        casterImage.sprite = unit.FullBodyArt;
        casterName.text = unit.CharacterName;

        rarityBehav.Setup(unit.BaseRarity);
    }

    public void TriggerPullFocus(UnitObject unit)
    {
        Setup(unit);

        bgRectFRC.StartCenterLerp();
        casterRectFR.StartLerp(casterRectFR.originalPosition);
        fgRectFR.StartLerp(fgRectFR.originalPosition);

        continueObject.SetActive(true);
        foreach (GameObject obj in hidingObjects) obj.SetActive(false);

        transitionFinished = false;
    }

    public void BTN_ClosePullFocus()
    {
        if (!transitionFinished) return;

        bgRectFRC.StartCenterLerp();
        casterRectFR.StartLerp(casterRectFR.GetBodyOffset(Vector2.left));
        fgRectFR.StartLerp(fgRectFR.GetBodyOffset(Vector2.right));

        continueObject.SetActive(false);
        foreach (GameObject obj in hidingObjects) obj.SetActive(true);
    }

    public void ResetFocus()
    {
        fgRectFR.MoveTo(fgRectFR.GetBodyOffset(Vector2.right));
        casterRectFR.MoveTo(casterRectFR.GetBodyOffset(Vector2.left));
        bgRectFRC.Close();

        continueObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LerpFunctions;

public class InventoryManager : MenuCanvasPage
{
    public static InventoryManager instance;
    DatabaseManager databaseManager;

    [Header("Inventory")]
    public GameObject uiCasterObject;
    public Transform castersParent;

    List<UnitObject> casterInventory;
    List<GameObject> casterInvList;

    [Header("Focus settings")]
    public CanvasGroup textCanvas;
    public RectTransform artRect;
    public Button cancelButton;
    public float transitionSpeed = 10f;

    [Header("Focus References")]
    //! missing mastery
    public TextMeshProUGUI levelNum;
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI casterName;
    public TextMeshProUGUI casterDescription;
    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI defStat;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI casterArch;

    CanvasGroupFader textCGF;
    FlexibleRect artFR;
    Image splashArt;

    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        mainMenuManager = MainMenuManager.instance;

        splashArt = artRect.GetComponent<Image>();
        artFR = new FlexibleRect(artRect);
        textCGF = new CanvasGroupFader(textCanvas, true, false);

        artFR.MoveTo(artFR.GetBodyOffset(Vector2.right, 2f));
        textCGF.SetStateFadeIn();
        textCGF.SetAlpha(0f);

        UpdateCasterInventory();
    }

    void Update()
    {
        textCGF.Step(transitionSpeed * Time.unscaledDeltaTime);
        artFR.Step(transitionSpeed * Time.unscaledDeltaTime);
    }

    #region Inventory
    public void UpdateCasterInventory()
    {
        ClearCasterInventory();
        databaseManager.RefreshInventory();
        casterInventory = new List<UnitObject>();
        casterInventory = databaseManager.PlayerCasters;

        foreach (UnitObject unit in casterInventory)
        {
            GameObject caster = FindNextInactiveChild();
            caster.SetActive(true);
            caster.GetComponent<CasterInventoryBehavior>().Init(unit, this);
        }
    }

    public void ClearCasterInventory()
    {
        casterInvList = new List<GameObject>();
        for (int i = 0; i < castersParent.childCount; i++)
        {
            casterInvList.Add(castersParent.GetChild(i).gameObject);
            castersParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    GameObject FindNextInactiveChild()
    {
        foreach (GameObject child in casterInvList) if (!child.activeInHierarchy) return child;

        Debug.LogError("No caster inventory space left");
        return null;
    }
    #endregion

    public void ActivateFocus(UnitObject unit)
    {
        mainMenuManager.HideBottomOverlay();

        levelNum.text = unit.CurrentLevel.ToString();
        if (unit.CurrentLevel == unit.MaxLevel) levelLabel.text = "MAXED";
        else levelLabel.text = "LV";

        casterName.text = unit.CharacterName;
        casterDescription.text = unit.CharacterDescription;
        atkStat.text = unit.LevelTotalAttack.ToString();
        defStat.text = unit.LevelTotalDefence.ToString();
        skillName.text = unit.SkillName;
        skillDescription.text = unit.SkillDescription;
        casterArch.text = unit.ArchTypeMajor.ToString();

        splashArt.sprite = unit.FullBodyArt;

        textCGF.StartFadeIn();
        artFR.StartLerp(artFR.originalPosition);
    }

    public void DeactivateFocus()
    {
        mainMenuManager.ShowBottomOverlay();
        textCGF.StartFadeOut();
        artFR.StartLerp(artFR.GetBodyOffset(Vector2.right, 2f));
    }
}

using ConversionFunctions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    public static InformationManager instance;
    private TurnBaseManager _turnBaseManager;
    private BattlestageManager _battleStageManager;
    private List<Unit> _subscriptionList = new List<Unit>();

    [Header("UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image skillChargeBar;
    [SerializeField] private Image casterPortrait;
    [SerializeField] private TextMeshProUGUI skillTextMesh;

    public TextMeshProUGUI fyorCount;
    public TextMeshProUGUI aquaCount;
    public TextMeshProUGUI tehkCount;

    #region Update UI Methods

    public void SyncUserInterfaceToUnit(Unit unit)
    {
        if (unit == null) return;
        UpdateCasterProtrait(unit);
        UpdateHealthBar(unit);
        UpdateSkillChargeBar(unit);
        UpdateSkillDescription(unit);
    }

    private void UpdateCasterProtrait(Unit unit)
    {
        Sprite spriteArt = unit.GetBaseUnit.PortraitArt;
        if (spriteArt == null) return;
        casterPortrait.sprite = spriteArt;
    }

    private void UpdateSkillChargeBar(Unit unit) => skillChargeBar.fillAmount = unit.GetSkillChargeRatio;
    private void UpdateSkillDescription(Unit unit) => skillTextMesh.text = unit.GetBaseUnit.SkillDescription;
    private void UpdateHealthBar(Unit unit) => healthBar.fillAmount = unit.GetHealthRatio;
    private void UpdateHealthBarIfCurrentUnit(Unit unit)
    {
        var curUnit = _turnBaseManager.GetCurrentCaster().AsUnit();
        if (unit != curUnit) return;
        UpdateHealthBar(unit);
    }

    #endregion

    #region Events

    private void SubscribeAllEvents()
    {
        if (_battleStageManager == null) return;
        var casterList = _battleStageManager.GetCasterTeamAsUnit();
        casterList.ForEach(caster => SubscribeAndAdd(caster));
        Unit.SubscribeDeathEvent(UnsubscribeUnit);
    }
    private void SubscribeAndAdd(Unit caster)
    {
        caster.SubscribeHealthChangeEvent(UpdateHealthBarIfCurrentUnit);
        _subscriptionList.Add(caster);
    }
    private void UnsubscribeAllEvents()
    {
        _subscriptionList.ForEach(caster => caster.UnsubscribeHealthChangeEvent(UpdateHealthBarIfCurrentUnit));
        Unit.UnsubscribeDeathEvent(UnsubscribeUnit);
    }
    private void UnsubscribeUnit(Unit unit)
    {
        if(_subscriptionList.Contains(unit))
        {
            unit.UnsubscribeHealthChangeEvent(UpdateHealthBarIfCurrentUnit);
            _subscriptionList.Remove(unit);
        }
    }

    #endregion

    #region Shorthands

    private static bool HasValidMaxSkillCharge(Unit unit) => unit.GetMaxSkillChargeCount > 0;
    private static bool HasValidMaxHealth(Unit unit) => unit.GetMaxHealth > 0;

    #endregion

    void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        _turnBaseManager = TurnBaseManager.instance;
        _battleStageManager = BattlestageManager.instance;
        SubscribeAllEvents();
        EndConnectionUI();
    }

    private void OnDestroy() => UnsubscribeAllEvents();

    public void UpdateConnectionUI(RuneStorage storage)
    {
        TextMeshProUGUI text = null;
        switch (storage.runeType)
        {
            case RuneType.FYOR:
                text = fyorCount;
                break;
            case RuneType.KHUA:
                text = aquaCount;
                break;

            case RuneType.TEHK:
                text = tehkCount;
                break;

        }

        if (storage.amount > 0 && text != null)
        {
            text.gameObject.SetActive(true);
            text.text = storage.runeType.ToString() + ": " + storage.amount.ToString();
        }
    }

    public void EndConnectionUI()
    {
        fyorCount.gameObject.SetActive(false);
        aquaCount.gameObject.SetActive(false);
        tehkCount.gameObject.SetActive(false);
    }
}

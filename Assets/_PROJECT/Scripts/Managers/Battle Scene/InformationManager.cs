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
    [SerializeField] private GameObject skillActivator;

    public TextMeshProUGUI fyorCount;
    public TextMeshProUGUI aquaCount;
    public TextMeshProUGUI tehkCount;

    public void UpdateCasterProtrait(Unit unit)
    {
        if (unit == null) return;

        Sprite spriteArt = unit.GetBaseUnit.PortraitArt;
        if (spriteArt == null) return;
        casterPortrait.sprite = spriteArt;
    }

    public void UpdateSkillChargeBar(Unit unit)
    {
        if (unit == null) return;

        float fill = 0.01f;
        if (unit.GetMaxSkillChargeCount != 0) fill = unit.GetCurrentSkillChargeCount / unit.GetMaxSkillChargeCount;

        skillChargeBar.fillAmount = fill;
    }

    private void UpdateHealthBar(Unit unit)
    {
        if (_turnBaseManager == null) return;

        var unitObject = _turnBaseManager.GetCurrentCaster();
        if (unitObject == null) return;

        var currentUnit = unitObject.GetComponent<Unit>();
        if (currentUnit != unit) return;

        float fill = 0.01f;
        if (unit.GetMaxHealth != 0) fill = unit.GetCurrentHealth / unit.GetMaxHealth;

        healthBar.fillAmount = fill;
    }

    private void SubscribeEvents()
    {
        if (_battleStageManager == null) return;

        var casterList = _battleStageManager.GetCastersTeam();
        foreach (var casterObject in casterList)
        {
            var caster = casterObject.GetComponent<Unit>();
            if (caster == null) continue;

            caster.SubscribeHealthChangeEvent(UpdateHealthBar);
            _subscriptionList.Add(caster);
        }
    }
    private void UnsubscribeEvents() => _subscriptionList.ForEach(caster => caster.UnsubscribeHealthChangeEvent(UpdateHealthBar));

    void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        _turnBaseManager = TurnBaseManager.instance;
        _battleStageManager = BattlestageManager.instance;
        SubscribeEvents();
        EndConnectionUI();
    }

    private void OnDestroy() => UnsubscribeEvents();

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

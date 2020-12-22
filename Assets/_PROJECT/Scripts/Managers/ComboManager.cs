using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LerpFunctions;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;
    ConnectionManager connectionManager;
    RuneManager runeManager;
    InformationManager infoManager;
    TurnBaseManager turnBaseManger;
    RoomManager roomManager;
    BattlestageManager battleStageManager;
    public float runeDamage;

    [Header("Countdown Timer")]
    public float countdownTimer;
    public Slider slideTimer;
    public Image slider;
    
    List<RuneBehaviour> comboList = new List<RuneBehaviour>();
    float totalDamage;
    float timer;
    bool isStart = false;
    bool sliderAnim = false;

    RuneType runeType;

    RuneStorage gronRune = new RuneStorage(RuneType.GRON, 0);
    RuneStorage fyorRune = new RuneStorage(RuneType.FYOR, 0);
    RuneStorage tehkRune = new RuneStorage(RuneType.TEHK, 0);
    RuneStorage khuaRune = new RuneStorage(RuneType.KHUA, 0);
    RuneStorage ayroRune = new RuneStorage(RuneType.AYRO, 0);

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        connectionManager = ConnectionManager.instance;
        runeManager = RuneManager.instance;
        infoManager = InformationManager.instance;
        turnBaseManger = TurnBaseManager.instance;
        roomManager = RoomManager.Instance;
        battleStageManager = BattlestageManager.instance;
    }

    void Update()
    {
        if (isStart && !sliderAnim)
        {
            timer -= Time.unscaledDeltaTime;
            //slideTimer.value = timer;
            slider.fillAmount = timer / countdownTimer;
            if (timer <= 0)
            {
                isStart = false;
                sliderAnim = true;
                timer = countdownTimer;
                //slider.fillAmount = 1.0f;

                GameObject targetObject = (battleStageManager.GetSelectedTarget() != null) ? battleStageManager.GetSelectedTarget() : battleStageManager.GetEnemyTeam()[0];

                runeManager.SpawnDeactivate();
                AssessRunes(turnBaseManger.GetCurrentCaster(), targetObject);
                //EnemyAttack(battleStageManager.GetCurrentEnemy(0), turnBaseManger.GetCurrentCaster());

            }
        }
        else if (sliderAnim)
        {
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, 1.0f, 2.0f * Time.unscaledDeltaTime);

            if (slider.fillAmount >= 0.999f)
            {
                slider.fillAmount = 1.0f;
                sliderAnim = false;
            }
        }
    }

    public void CollectDamage()
    {
        comboList = connectionManager.GetSelectedRuneList();

        RuneType runetype = connectionManager.GetSelectionType();

        switch (runetype)
        {
            case RuneType.GRON:
                gronRune.amount += comboList.Count;
                break;
            case RuneType.FYOR:
                fyorRune.amount += comboList.Count;
                break;
            case RuneType.TEHK:
                tehkRune.amount += comboList.Count;
                break;
            case RuneType.KHUA:
                khuaRune.amount += comboList.Count;
                break;
            case RuneType.AYRO:
                ayroRune.amount += comboList.Count;
                break;
        }
        
        for (int i = 0; i < comboList.Count; i++)
        {
            
            comboList[i].gameObject.SetActive(false);
        }
        
    }

    public void SetCountdownTimer() => timer = countdownTimer;
    public void SetIsStart() => isStart = true;
    void AssessRunes(GameObject damagerObject, GameObject targetObject)
    {
        print("Gron: " + gronRune.amount + " | Fyor: " + fyorRune.amount + " | Tehk: " + tehkRune.amount + " | Khua: " + khuaRune.amount + " | Ayro: " + ayroRune.amount);
        infoManager.UpdateConnectionUI(fyorRune);
        infoManager.UpdateConnectionUI(gronRune);
        infoManager.UpdateConnectionUI(tehkRune);
        infoManager.UpdateConnectionUI(khuaRune);
        infoManager.UpdateConnectionUI(ayroRune);

        if (battleStageManager == null) { ResetRunes(); isStart = false; return; }

        Entity damager = damagerObject.GetComponent<Entity>();
        Entity target = targetObject.GetComponent<Entity>();
        List<Entity> allEntities = new List<Entity>();

        for(int i = 0; i < battleStageManager.enemyPositions.Length; i++)
        {
            Entity e = battleStageManager.enemyPositions[i].GetComponent<Entity>();
            if (e == null) continue;

            allEntities.Add(e);
        }

        TargetInfo targetInfo = damager.GetAffectedTargets(target, allEntities);
        RuneCollection collection = new RuneCollection(gronRune, fyorRune, tehkRune, khuaRune, ayroRune);
        damager.DoAction(targetInfo, collection);

        ResetRunes();
        isStart = false;
    }

    public void EnemyAttack(GameObject damagerObject, GameObject targetObject)
    {
        if (battleStageManager == null) { return; }
        Entity damager = damagerObject.GetComponent<Entity>();
        Entity target = targetObject.GetComponent<Entity>();
        List<Entity> allEntities = new List<Entity>();

        for (int i = 0; i < battleStageManager.casterPositions.Length; i++)
        {
            Entity e = battleStageManager.casterPositions[i].GetComponent<Entity>();
            if (e == null) continue;

            allEntities.Add(e);
        }

        TargetInfo targetInfo = damager.GetAffectedTargets(target, allEntities);
        RuneCollection collection = new RuneCollection(gronRune, fyorRune, tehkRune, khuaRune, ayroRune);
        damager.DoAction(targetInfo, collection);
    }

    private void ResetRunes()
    {
        gronRune.amount = 0;
        fyorRune.amount = 0;
        tehkRune.amount = 0;
        khuaRune.amount = 0;
        ayroRune.amount = 0;
    }

    #region Accessors
    public bool GetIsStart() => isStart;
    public RuneType GetRuneType() => runeType;

    #endregion

}

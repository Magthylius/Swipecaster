using ConversionFunctions;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public static EnemyAttackManager instance;
    BattlestageManager battleStageManager;
    public List<GameObject> target = new List<GameObject>();

    RuneStorage gronRune = new RuneStorage(RuneType.GRON, 0);
    RuneStorage fyorRune = new RuneStorage(RuneType.FYOR, 0);
    RuneStorage tehkRune = new RuneStorage(RuneType.TEHK, 0);
    RuneStorage khuaRune = new RuneStorage(RuneType.KHUA, 0);
    RuneStorage ayroRune = new RuneStorage(RuneType.AYRO, 0);

    GameObject targetCaster;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    void Start()
    {
        battleStageManager = BattlestageManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyAttack(GameObject damagerObject, GameObject targetObject)
    {
        print(targetObject.AsUnit().GetEntityName + " is being attacked!");
        if (battleStageManager == null) { return; }
        var damager = damagerObject.AsUnit();
        var target = targetObject.AsUnit();

        var activeAllies = battleStageManager.GetEnemyTeamAsUnit();
        var activeFoes = battleStageManager.GetCasterTeamAsUnit();
        var allAllyEntities = battleStageManager.GetActiveRightPositions();
        var allFoeEntities = battleStageManager.GetActiveLeftPositions();

        TargetInfo targetInfo = damager.GetAffectedTargets(new TargetInfo(target, null, null, activeAllies, activeFoes, allAllyEntities, allFoeEntities));
        RuneCollection collection = new RuneCollection(gronRune, fyorRune, tehkRune, khuaRune, ayroRune);
        damager.DoAction(targetInfo, collection);
    }

    public void CalculatePriority(GameObject attacker)
    {
        int highestPriotyNum = -1;

        for (int i = 0; i < battleStageManager.GetCastersTeam().Count; i++)
        {
            if (highestPriotyNum < battleStageManager.GetActiveLeftPositions()[i].GetUnitPriority)
            {              
                target.Clear();
                target.Add(battleStageManager.GetActiveLeftPositions()[i].gameObject);
                highestPriotyNum = battleStageManager.GetActiveLeftPositions()[i].GetUnitPriority;
            }
            else if (highestPriotyNum == battleStageManager.GetActiveLeftPositions()[i].GetUnitPriority)
            {
                target.Add(battleStageManager.GetActiveLeftPositions()[i].gameObject);
            }
        }

        targetCaster = target[Random.Range(0, target.Count)];
        
        EnemyAttack(attacker, targetCaster);

    }

    #region Accessors

    public GameObject GetCaster() => targetCaster;

    #endregion

}

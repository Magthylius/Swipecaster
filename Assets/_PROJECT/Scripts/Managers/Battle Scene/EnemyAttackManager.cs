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
        
        TargetInfo targetInfo = damager.GetAffectedTargets(new TargetInfo(target, null, null, (List<Unit>)battleStageManager.GetEnemyTeamAsUnit(), (List<Unit>)battleStageManager.GetCasterTeamAsUnit()));
        RuneCollection collection = new RuneCollection(gronRune, fyorRune, tehkRune, khuaRune, ayroRune);
        damager.DoAction(targetInfo, collection);
    }

    public void CalculatePriority(GameObject attacker)
    {
        for (int i = 0; i < battleStageManager.GetCastersTeam().Count; i++)
        {
            battleStageManager.GetCastersTeam()[i].GetComponent<Caster>().SetUnitPriority(1);
        }

        int highestPriotyNum = -1;

        for (int i = 0; i < battleStageManager.GetCastersTeam().Count; i++)
        {
            if (highestPriotyNum < battleStageManager.GetCastersTeam()[i].GetComponent<Caster>().GetUnitPriority)
            {              
                target.Clear();
                target.Add(battleStageManager.GetCastersTeam()[i]);
                highestPriotyNum = battleStageManager.GetCastersTeam()[i].GetComponent<Caster>().GetUnitPriority;
            }
            else if (highestPriotyNum == battleStageManager.GetCastersTeam()[i].GetComponent<Caster>().GetUnitPriority)
            {
                target.Add(battleStageManager.GetCastersTeam()[i]);
            }
        }

        targetCaster = target[Random.Range(0, target.Count)];
        
        EnemyAttack(attacker, targetCaster);

    }

    #region Accessors

    public GameObject GetCaster() => targetCaster;

    #endregion

}

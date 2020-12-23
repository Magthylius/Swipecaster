using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public static EnemyAttackManager instance;
    BattlestageManager battleStageManager;
    List<GameObject> target = null;

    RuneStorage gronRune = new RuneStorage(RuneType.GRON, 0);
    RuneStorage fyorRune = new RuneStorage(RuneType.FYOR, 0);
    RuneStorage tehkRune = new RuneStorage(RuneType.TEHK, 0);
    RuneStorage khuaRune = new RuneStorage(RuneType.KHUA, 0);
    RuneStorage ayroRune = new RuneStorage(RuneType.AYRO, 0);
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
        if (battleStageManager == null) { return; }
        var damager = damagerObject.GetComponent<Unit>();
        var target = targetObject.GetComponent<Unit>();
        var allCasters = new List<Unit>();

        for (int i = 0; i < battleStageManager.casterPositions.Length; i++)
        {
            var e = battleStageManager.casterPositions[i].GetComponent<Unit>();
            if (e == null) continue;

            allCasters.Add(e);
        }

        TargetInfo targetInfo = damager.GetAffectedTargets(target, allCasters);
        RuneCollection collection = new RuneCollection(gronRune, fyorRune, tehkRune, khuaRune, ayroRune);
        damager.DoAction(targetInfo, collection);
    }

    public void CalculatePriotity(GameObject attacker)
    {
        for (int i = 0; i < battleStageManager.GetEnemyTeam().Count; i++)
        {
            battleStageManager.GetEnemyTeam()[i].GetComponent<Caster>().SetPriorityNum(1);
        }

        int highestPriotyNum = -1;

        for (int i = 0; i < battleStageManager.GetEnemyTeam().Count; i++)
        {
            if (highestPriotyNum < battleStageManager.GetEnemyTeam()[i].GetComponent<Caster>().GetPriorityNum)
            {
                target.Clear();
                target.Add(battleStageManager.GetEnemyTeam()[i]);
                highestPriotyNum = battleStageManager.GetEnemyTeam()[i].GetComponent<Caster>().GetPriorityNum;
            }
            else if (highestPriotyNum == battleStageManager.GetEnemyTeam()[i].GetComponent<Caster>().GetPriorityNum)
            {
                target.Add(battleStageManager.GetEnemyTeam()[i]);
            }
        }

        EnemyAttack( attacker, target[Random.Range(0,target.Count)] );

    }
}

using UnityEngine;

public class UnitObjectForFoe : UnitObject
{
    [Header("Foe Settings")]
    [SerializeField] private bool randomiseStats = true;
    [Space(5)]
    [SerializeField, Range(1, 5000)] private int minRandomHealth;
    [SerializeField, Range(1, 5000)] private int maxRandomHealth;
    [Space(5)]
    [SerializeField, Range(1, 5000)] private int minRandomAttack;
    [SerializeField, Range(1, 5000)] private int maxRandomAttack;
    [Space(5)]
    [SerializeField, Range(1, 5000)] private int minRandomDefence;
    [SerializeField, Range(1, 5000)] private int maxRandomDefence;
    
    public override void CalculateRandomisedStats()
    {
        if(!randomiseStats) { CalculateActualStats(); return; }
        if (!MinIsLowerThanMax()) { DebugMinIsLargerThanMaxCase(); return; }
        int randomHealth = Random.Range(minRandomHealth, maxRandomHealth + 1);
        int randomAttack = Random.Range(minRandomAttack, maxRandomAttack + 1);
        int randomDefence = Random.Range(minRandomDefence, maxRandomDefence + 1);
        CalculateActualStats(randomHealth, randomAttack, randomDefence);
    }

    private bool MinIsLowerThanMax() =>
        minRandomHealth <= maxRandomHealth &&
        minRandomAttack <= maxRandomAttack &&
        minRandomDefence <= maxRandomDefence;

    private void DebugMinIsLargerThanMaxCase()
    {
        Debug.LogWarning("Could not calculate randomised stats as specified mininum values are larger than " +
            "maximum values.");
    }
}

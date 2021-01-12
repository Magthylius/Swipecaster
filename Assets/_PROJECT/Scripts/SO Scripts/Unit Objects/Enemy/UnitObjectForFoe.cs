using ConversionFunctions;
using UnityEngine;

public class UnitObjectForFoe : UnitObject
{
    [Header("Foe Settings")]
    [SerializeField, Range(1.0f, 5000.0f)] private float minRandomHealth;
    [SerializeField, Range(1.0f, 5000.0f)] private float maxRandomHealth;
    [SerializeField, Range(1.0f, 5000.0f)] private float minRandomAttack;
    [SerializeField, Range(1.0f, 5000.0f)] private float maxRandomAttack;
    [SerializeField, Range(1.0f, 5000.0f)] private float minRandomDefence;
    [SerializeField, Range(1.0f, 5000.0f)] private float maxRandomDefence;

    public override void CalculateRandomisedStats()
    {
        if (!MinIsLowerThanMax()) { DebugMinIsLargerThanMaxCase(); return; }
        int randomHealth = Random.Range(minRandomHealth, maxRandomHealth).AsInt();
        int randomAttack = Random.Range(minRandomAttack, maxRandomAttack).AsInt();
        int randomDefence = Random.Range(minRandomDefence, maxRandomDefence).AsInt();
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

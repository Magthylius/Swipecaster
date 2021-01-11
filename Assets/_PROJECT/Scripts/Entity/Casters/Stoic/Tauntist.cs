using UnityEngine;

public class Tauntist : Stoic
{
    [SerializeField] private int priorityIncrement = 5;

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Tauntist);
        SetUnitPriority(GetUnitPriority + priorityIncrement);
    }

    #endregion
}
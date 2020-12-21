using System.Collections.Generic;

public class Piercing : Projectile
{
    private List<float> _diminishingMultiplier = new List<float>();
    private float Multiplier(int index) => _diminishingMultiplier[index];

    public override void AssignTargetDamage(Entity damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _damageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage * Multiplier(0)));

        //! Collateral
        for (int j = 0; j < info.Collateral.Count; j++) info.Collateral[j].InvokeHitEvent(damager, Round(subtotalDamage * Multiplier(j + 1)));
    }


    public override TargetInfo GetTargets(Entity focus, List<Entity> allEntities)
    {
        if (!allEntities.Contains(focus)) return TargetInfo.Null;

        var collateral = new List<Entity>();
        var grazed = new List<Entity>();
        int focusIndex = allEntities.IndexOf(focus);

        for (int i = focusIndex; i >= 0; i--)
        {
            if (allEntities[i] == focus) continue;

            grazed.Add(allEntities[i]);
        }

        for (int j = focusIndex; j < allEntities.Count; j++)
        {
            if (allEntities[j] == focus) continue;

            collateral.Add(allEntities[j]);
        }

        return new TargetInfo(focus, collateral, grazed);
    }

    public Piercing()
    {
        _damageMultiplier = 1.0f;
        _diminishingMultiplier = new List<float>(4)
        {
            1.0f,
            0.5f,
            0.25f,
            0.13f
        };
    }
    public Piercing(float damageMultiplier) : base(damageMultiplier)
    {
        _diminishingMultiplier = new List<float>(4)
        {
            1.0f,
            0.5f,
            0.25f,
            0.13f
        };
    }
    public Piercing(float damageMultiplier, List<float> diminishingMultiplier) : base(damageMultiplier)
    {
        _diminishingMultiplier = diminishingMultiplier;
    }
}
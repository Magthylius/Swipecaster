using System.Collections.Generic;

namespace Create
{
    public static class A_Status
    {
        private static readonly float Zero = 0.0f;
        public static StatusEffect Stun(int turns, TurnBaseManager turnBase) => new Stun(turns, 1.0f, false, turnBase);
        public static StatusEffect Vulnerability(int turns) => new DefenceDown(turns, 0.5f, false, 0.1f);
        public static StatusEffect DefenceDown(int turns, float percent) => new DefenceDown(turns, 0.5f, false, percent);
        public static StatusEffect DefenceUp(int turns, float percent) => new DefenceUp(turns, Zero, false, percent);
        public static StatusEffect Aflame(int turns) => new Flame(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Flame(int turns, float maxHPPercent, float atkDownPercent) => new Flame(turns, 0.35f, false, maxHPPercent, atkDownPercent);
        public static StatusEffect Poison(int turns) => new Poison(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Weakness(int turns) => new Weakness(turns, 0.65f, false, 0.2f);
        public static StatusEffect AttackDown(int turns, float percent) => new AttackDown(turns, Zero, false, percent);
        public static StatusEffect AttackUp(int turns, float percent) => new AttackUp(turns, Zero, false, percent);
        public static StatusEffect AttackToPartyHeal(int turns, float percent) => new AttackToPartyHeal(turns, Zero, false, percent);
        public static StatusEffect Corrosion(int turns, int stacks) => new Corrosion(turns, Zero, false, stacks);
        public static StatusEffect DamageTakenUp(int turns, float percent) => new DamageTakenUp(turns, Zero, false, percent);
        public static StatusEffect DamageToDistributedPartyHeal(int turns, float percent) => new DamageToDistributedPartyHeal(turns, Zero, false, percent);
        public static StatusEffect PriorityUp(int turns, int increment) => new PriorityUp(turns, Zero, false, increment);
        public static StatusEffect Perm_PriorityUp(int increment) => new PriorityUp(0, Zero, true, increment);
        public static StatusEffect ReboundDamageUp(int turns, float percent) => new ReboundDamageUp(turns, Zero, false, percent);
        public static StatusEffect ReboundRateUpKinectist(int turns, float percent) => new ReboundRateUpKinectist(turns, Zero, false, percent);
        public static StatusEffect Ununaliving(int turns) => new Ununaliving(turns, Zero, false);
        public static StatusEffect PierceDamageUp(int turns, List<float> additiveMultiplier) => new PierceDamageUp(turns, Zero, false, additiveMultiplier);
        public static StatusEffect FixedReboundDamage(int turns, float fixedPercent) => new FixedReboundDamage(turns, Zero, false, fixedPercent);
        public static StatusEffect Perm_FixedReboundDamage(float fixedPercent) => new FixedReboundDamage(0, Zero, true, fixedPercent);
        public static StatusEffect ProjectileLocker(int turns, Projectile projectile) => new ProjectileLocker(turns, Zero, false, projectile);
        public static StatusEffect ReboundingStatus(int turns, StatusEffect statusToRebound) => new ReboundingStatus(turns, Zero, false, statusToRebound);
        public static StatusEffect StatusOnAttack(int turns, StatusEffect statusToApply) => new StatusOnAttack(turns, Zero, false, statusToApply);
        public static StatusEffect BeatDrumEffect(int turns, BattlestageManager battleStage) => new BeatDrumEffect(turns, Zero, false, battleStage);
    }

    public static class A_Skill
    {
        public static ActiveSkill TeapotCrackpot(Unit unit) => new TeapotCrackpot(3, 3, unit);
        public static ActiveSkill Shoutdown(Unit unit) => new Shoutdown(3, 1, unit);
        public static ActiveSkill HardCover(Unit unit) => new HardCover(2, 1, unit);
        public static ActiveSkill MorningDew(Unit unit) => new MorningDew(1, 1, unit);
        public static ActiveSkill CrypticMark(Unit unit) => new CrypticMark(2, 1, unit);
        public static ActiveSkill MothLamp(Unit unit) => new MothLamp(5, 0, unit);
        public static ActiveSkill SporeBurst(Unit unit) => new SporeBurst(3, 2, unit);
        public static ActiveSkill Wickfire(Unit unit) => new Wickfire(5, 5, unit);
        public static ActiveSkill SpontaneousFire(Unit unit) => new SpontaneousFire(3, 1, unit);
        public static ActiveSkill GaussCaliber(Unit unit) => new GaussCaliber(5, 3, unit);
        public static ActiveSkill WaveringMist(Unit unit) => new WaveringMist(5, 3, unit);
        public static ActiveSkill WitheringMark(Unit unit) => new WitheringMark(5, 1, unit);
        public static ActiveSkill TwinShell(Unit unit) => new TwinShell(3, 1, unit);
        public static ActiveSkill GeminiPetrification(Unit unit) => new GeminiPetrification(4, 3, unit);
        public static ActiveSkill SandCorrosion(Unit unit) => new SandCorrosion(3, 1, unit);
        public static ActiveSkill DrumBeater(Unit unit) => new DrumBeater(4, 0, unit);
        public static ActiveSkill LuringDesire(Unit unit) => new LuringDesire(5, 1, unit);
        public static ActiveSkill BackRotation(Unit unit) => new BackRotation(3, 1, unit);
        public static ActiveSkill RollingThunder(Unit unit) => new RollingThunder(4, 1, unit);
        public static ActiveSkill SecondScar(Unit unit) => new SecondScar(5, 3, unit);
        public static ActiveSkill BlackSun(Unit unit) => new BlackSun(5, 1, unit);
    }
}
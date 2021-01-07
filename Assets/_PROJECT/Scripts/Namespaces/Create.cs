﻿using System.Collections.Generic;

namespace Create
{
    public static class A_Status
    {
        private static readonly float Zero = 0.0f;
        public static StatusEffect Stun(int turns, TurnBaseManager turnBase) => new Stun(turns, 1.0f, false, turnBase);
        public static StatusEffect DefenceDown(int turns) => new DefenceDown(turns, 0.5f, false, 0.1f);
        public static StatusEffect Flame(int turns) => new Flame(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Poison(int turns) => new Poison(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Weakness(int turns) => new Weakness(turns, 0.65f, false, 0.2f);
        public static StatusEffect AttackUp(int turns, float percent) => new AttackUp(turns, Zero, false, percent);
        public static StatusEffect AttackToPartyHeal(int turns, float percent) => new AttackToPartyHeal(turns, Zero, false, percent);
        public static StatusEffect Corrosion(int turns, int stacks) => new Corrosion(turns, Zero, false, stacks);
        public static StatusEffect DamageTakenUp(int turns, float percent) => new DamageTakenUp(turns, Zero, false, percent);
        public static StatusEffect DamageToDistributedPartyHeal(int turns, float percent) => new DamageToDistributedPartyHeal(turns, Zero, false, percent);
        public static StatusEffect PriorityUp(int turns, int increment) => new PriorityUp(turns, Zero, false, increment);
        public static StatusEffect ReboundDamageUp(int turns, float percent) => new ReboundDamageUp(turns, Zero, false, percent);
        public static StatusEffect ReboundRateUpKinectist(int turns, float percent) => new ReboundRateUpKinectist(turns, Zero, false, percent);
        public static StatusEffect Ununaliving(int turns) => new Ununaliving(turns, Zero, false);
        public static StatusEffect PierceDamageUp(int turns, List<float> multipliers) => new PierceDamageUp(turns, Zero, false, multipliers);
    }

    public static class A_Skill
    {
        public static ActiveSkill TeapotCrackpot(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill Shoutdown(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill HardCover(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill MorningDew(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill CrypticMark(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill MothLamp(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill SporeBurst(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill Wickfire(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill SpontaneousFire(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill GaussCaliber(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill WaveringMist(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill WitheringMark(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill TwinShell(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill GeminiPetrification(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill SandCorrosion(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill DrumBeater(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill LuringDesire(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill BackRotation(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill RollingThunder(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill SecondScar(Unit unit) => new TeapotCrackpot(unit);
        public static ActiveSkill KuroiTaiyou(Unit unit) => new TeapotCrackpot(unit);
    }
}
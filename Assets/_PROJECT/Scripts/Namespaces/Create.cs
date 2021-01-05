namespace Create
{
    public static class A_Status
    {
        public static StatusEffect Stun(int turns = 1) => new Stun(turns, 1.0f, false, null);
        public static StatusEffect DefenceDown(int turns) => new DefenceDown(turns, 0.5f, false, 0.1f);
        public static StatusEffect Flame(int turns) => new Flame(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Poison(int turns) => new Poison(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Weakness(int turns) => new Weakness(turns, 0.65f, false, 0.2f);
        public static StatusEffect AttackUp(int turns, float percent) => new AttackUp(turns, 0.0f, false, percent);
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
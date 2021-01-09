public class NullStatus : StatusTemplate<NullStatus>
{
    public override string StatusName => "Null Status";

    public NullStatus() : base() { }
    public NullStatus(int turns, float baseResistance, bool isPermanent)
        : base(turns, baseResistance, isPermanent) { }
}
public struct RuneCollection
{
    private RuneStorage _ground;
    private RuneStorage _fire;
    private RuneStorage _electric;

    public RuneStorage Ground => _ground;
    public RuneStorage Fire => _fire;
    public RuneStorage Electric => _electric;

    public RuneCollection(RuneStorage ground, RuneStorage fire, RuneStorage electric)
    {
        _ground = ground;
        _fire = fire;
        _electric = electric;
    }
}

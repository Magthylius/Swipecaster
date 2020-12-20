using System.Collections.Generic;

public struct RuneRelations
{
    public List<RuneType> advantage;
    public List<RuneType> weakness;

    public static RuneRelations GetRelations(RuneType rune)
    {
        var advant = new List<RuneType>();
        var weak = new List<RuneType>();

        switch(rune)
        {
            case RuneType.GRON:
                break;
            case RuneType.FYOR:
                break;
            case RuneType.TEHK:
                break;
            case RuneType.AQUA:
                break;
            case RuneType.LIGHT:
                break;
            case RuneType.DARK:
                break;
            case RuneType.MIND:
                break;
            case RuneType.AYRO:
                break;
            case RuneType.NULL:
                break;
        }

        return new RuneRelations(new List<RuneType>(advant), new List<RuneType>(weak));
    }

    public RuneRelations(List<RuneType> advantage, List<RuneType> weakness)
    {
        this.advantage = advantage;
        this.weakness = weakness;
    }
}

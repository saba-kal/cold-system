public static class UnitUtils
{
    public static string GetOppositeUnitGroup(string unitGroup)
    {
        if (unitGroup == Constants.PLAYER_UNIT_GROUP)
        {
            return Constants.ENEMY_UNIT_GROUP;
        }
        if (unitGroup == Constants.ENEMY_UNIT_GROUP)
        {
            return Constants.PLAYER_UNIT_GROUP;
        }
        return null;
    }
}

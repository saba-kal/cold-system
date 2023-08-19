public static class Constants
{
    public const string PLAYER_UNIT_GROUP = "PlayerUnit";
    public const string ENEMY_UNIT_GROUP = "EnemyUnit";
    public const string UNIT_GROUPS = PLAYER_UNIT_GROUP + "," + ENEMY_UNIT_GROUP;
    public const string UNIT_CENTER_NODE_NAME = "UnitCenter";
    public const uint GROUND_COLLISION_LAYER = 1;
    public const uint PLAYER_UNIT_COLLISION_LAYER = 1 << 1;
    public const uint ENEMY_UNIT_COLLISION_LAYER = 1 << 2;
}

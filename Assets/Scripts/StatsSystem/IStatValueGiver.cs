using Assets.Scripts.StatsSystem.Enums;

namespace Assets.Scripts.StatsSystem
{
    public interface IStatValueGiver
    {
        float GetStatValue(StatType statType);
    }
}

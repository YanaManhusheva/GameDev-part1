using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StatsSystem
{
    [CreateAssetMenu(fileName = "StatsStorage", menuName = "Stats/StatsStorage")]
    public class StatsStorage : ScriptableObject
    {
        [field: SerializeField] public List<Stat> Stats { get; private set; }
    }
}

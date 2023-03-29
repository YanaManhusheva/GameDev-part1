using Assets.Scripts.Core.Services.Updater;
using Assets.Scripts.StatsSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StatsSystem
{
    public class StatsController : IDisposable, IStatValueGiver
    {
        private readonly List<Stat> _currentStats;
        private readonly List<StatModificator> _activeModificators;

        public StatsController(List<Stat> currentStats)
        {
            _currentStats = currentStats;
            _activeModificators = new List<StatModificator>();
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public float GetStatValue(StatType type) => 
            _currentStats.First(s => s.Type == type);

        public void ProccessModificator(StatModificator modificator)
        {
            var statToChange = _currentStats.Find(stat => stat.Type == modificator.Stat.Type);

            if (statToChange is null)
                return;

            var addedValue = 
                modificator.StatModificatorType == StatModificatorType.Additive ? 
                    statToChange + modificator.Stat : statToChange * modificator.Stat;

            statToChange.SetStatValue(statToChange + addedValue);

            if (modificator.Duration < 0)
                return;

            if (_activeModificators.Contains(modificator))
                _activeModificators.Remove(modificator);
            else
            {
                var addedStat = new Stat(modificator.Stat.Type, -addedValue);
                var tempModificator = 
                    new StatModificator
                    (
                        addedStat, 
                        StatModificatorType.Additive, 
                        modificator.Duration, 
                        Time.time
                    );
                _activeModificators.Add(tempModificator);
            }
        }
        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if(_activeModificators.Count == 0)
                return;

            var expiredModificators = _activeModificators
                .Where(m => m.StartTime + m.Duration <= Time.time);

            foreach (var modificator in expiredModificators)
                ProccessModificator(modificator);
        }
    }
}

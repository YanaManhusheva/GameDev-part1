using Assets.Scripts.Core.Services.Updater;
using Assets.Scripts.InputReader;
using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;

        private ExternalDevicesInputReader _externalDevicesInputReader;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;

        private List<IDisposable> _disposables;

        private void Awake()
        {
            _disposables = new List<IDisposable>();

            if (ProjectUpdater.Instance == null)
            {
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            }
            else
            {
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
            }

            _externalDevicesInputReader = new ExternalDevicesInputReader();
            _disposables.Add(_externalDevicesInputReader);

            _playerSystem = new PlayerSystem(_playerEntity, new List<IEntityInputSource>
            {
                _gameUIInputView,
                _externalDevicesInputReader
            });
            _disposables.Add(_playerSystem);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
            }
        }
        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}

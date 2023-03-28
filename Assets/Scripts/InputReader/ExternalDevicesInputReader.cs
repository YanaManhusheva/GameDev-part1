using UnityEngine;
using Player;
using UnityEngine.EventSystems;
using Assets.Scripts.Player;
using Assets.Scripts.Core.Services.Updater;
using System;

namespace Assets.Scripts
{
    public class ExternalDevicesInputReader : IEntityInputSource, IDisposable
    {
       
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public float VerticalDirection => Input.GetAxisRaw("Vertical");

        public bool Jump { get; private set; }
        public bool Attack { get; private set; }

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }
        public void ResetOneTimeAction()
        {
            Jump = false;
            Attack = false;
        }

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
                Jump = true;
            if (!isPointerOverUI() && Input.GetButtonDown("Fire1"))
                Attack = true;
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private bool isPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    }
}

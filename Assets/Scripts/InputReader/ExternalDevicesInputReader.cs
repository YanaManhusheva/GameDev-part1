using UnityEngine;
using Player;
using UnityEngine.EventSystems;
using Assets.Scripts.Player;

namespace Assets.Scripts
{
    public class ExternalDevicesInputReader : IEntityInputSource
    {
       
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public float VerticalDirection => Input.GetAxisRaw("Vertical");

        public bool Jump { get; private set; }
        public bool Attack { get; private set; }

        public void OnUpdate()
        {
           

            if (Input.GetButtonDown("Jump"))
                Jump = true;


           //if (Input.GetButtonDown("Fire3"))
           //     _playerEntity.LookUp();

           // if (Input.GetButtonUp("Fire3"))
           //     _playerEntity.EndLookUp();
           
            if (!isPointerOverUI() && Input.GetButtonDown("Fire1"))
                Attack = true;
        }

        private bool isPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
        public void ResetOneTimeAction()
        {
            Jump = false;
            Attack = false;
        }
    
    }
}

﻿using UnityEngine;
using Player;


namespace Assets.Scripts
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;

        private float _horizontalDirection;
        private float _verticalDirection;

        private void Update()
        {
            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _verticalDirection = Input.GetAxisRaw("Vertical");
            
        }
        private void FixedUpdate()
        {
            _playerEntity.MoveHorizontally(_horizontalDirection);
            _playerEntity.MoveVertically(_verticalDirection);
        }
    }
}

using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Movement.Controller
{
    class DirectionalMover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly DirectionalMovementData _directionalMovementData;
        private readonly float _sizeModificator;

        private Vector2 _movement;
 
        public Direction Direction { get; private set; }
        public bool IsMoving => _movement.magnitude > 0;

        public DirectionalMover(Rigidbody2D rigidbody , DirectionalMovementData directionalMovementData )
        {
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _directionalMovementData = directionalMovementData;
            float positionDifference = _directionalMovementData.MaxVericalPosition - _directionalMovementData.MinVericalPosition;
            float sizeDifference = _directionalMovementData.MaxSize - _directionalMovementData.MinSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpdateSize();
        }
        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _directionalMovementData.HorizontalSpeed;
            _rigidbody.velocity = velocity;
        }
        public void MoveVertically(float direction)
        {
           
            _movement.y = direction;
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction * _directionalMovementData.VerticalSpeed;
            _rigidbody.velocity = velocity;

            if (direction == 0)
                return;
            float verticalPosition = Mathf.Clamp(_rigidbody.position.y, _directionalMovementData.MinVericalPosition, _directionalMovementData.MaxVericalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);

            UpdateSize();
        }
        private void UpdateSize()
        {
            float verticalDelte = _directionalMovementData.MaxVericalPosition - _transform.position.y;
            float currentSizeModificator = _directionalMovementData.MinSize + _sizeModificator * verticalDelte;
            _transform.localScale = Vector2.one * currentSizeModificator;
        }
        private void SetDirection(float direction)
        {
            if (Direction == Direction.Right && direction < 0 || Direction == Direction.Left && direction > 0)
            {
                Flip();
            }
        }
        private void Flip()
        {
            _transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
           
        }

    }
}

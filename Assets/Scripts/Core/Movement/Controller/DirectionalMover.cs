﻿using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Movement.Data;
using Assets.Scripts.StatsSystem;
using Assets.Scripts.StatsSystem.Enums;
using UnityEngine;

namespace Assets.Scripts.Core.Movement.Controller
{
    public class DirectionalMover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly DirectionalMovementData _directionalMovementData;
        private readonly float _sizeModificator;
        private readonly IStatValueGiver _statValueGiver;

        private Vector2 _movement;

        public Direction Direction { get; private set; }
        public bool IsMoving => _movement.magnitude > 0;

        public DirectionalMover(Rigidbody2D rigidbody,
            DirectionalMovementData directionalMovementData, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _directionalMovementData = directionalMovementData;
            _statValueGiver = statValueGiver;
            var positionDifference = _directionalMovementData.MaxVerticalPosition -
                _directionalMovementData.MinVerticalPosition;
            var sizeDifference = _directionalMovementData.MaxSize -
                _directionalMovementData.MinSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpdateSize();
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            var velocity = _rigidbody.velocity;
            velocity.x = _statValueGiver.GetStatValue(StatType.Speed) * direction;
            _rigidbody.velocity = velocity;
        }

        public void MoveVertically(float direction)
        {
            _movement.y = direction;
            var velocity = _rigidbody.velocity;
            velocity.y = _statValueGiver.GetStatValue(StatType.Speed) / 2 * direction;
            _rigidbody.velocity = velocity;

            if (direction == 0)
                return;

            var verticalPosition = Mathf.Clamp(
                _rigidbody.position.y,
                _directionalMovementData.MinVerticalPosition,
                _directionalMovementData.MaxVerticalPosition
                );

            _rigidbody.position = new(_rigidbody.position.x, verticalPosition);
            UpdateSize();
        }
        private void UpdateSize()
        {
            var verticalDelta = _directionalMovementData.MaxVerticalPosition - _transform.position.y;
            var currentSizeModificator = _directionalMovementData.MinSize +
                _sizeModificator * verticalDelta;
            _transform.localScale = Vector2.one * currentSizeModificator;
        }

        private void SetDirection(float direction)
        {
            if (Direction == Direction.Right && direction < 0 ||
                Direction == Direction.Left && direction > 0)
                Flip();
        }

        private void Flip()
        {
            _transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}

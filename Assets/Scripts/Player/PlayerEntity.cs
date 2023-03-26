using System;
using UnityEngine;

namespace Player
{

    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [Header("HorizintalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private bool _faceRight;


        [Header("VerticalMovement")]
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _minVericalPosition;
        [SerializeField] private float _maxVericalPosition;


        private Rigidbody2D _rigidbody;

        private float _sizeModificator;


        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            float positionDifference = _maxVericalPosition - _minVericalPosition;
        }

        public void MoveHorizontally(float direction)
        {
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }
        public void MoveVertically(float direction)
        {
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction * _verticalSpeed;
            _rigidbody.velocity = velocity;
        }

      private void SetDirection(float direction)
        {
            if (_faceRight && direction<0 || !_faceRight && direction > 0)
            {
                Flip();
            }
        }
        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
    }
}
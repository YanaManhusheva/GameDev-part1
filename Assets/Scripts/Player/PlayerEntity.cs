using Assets.Scripts.Player;
using Assets.Scripts.Player.PlayerAnimation;
using Core.Enums;
using Core.Tools;
using System;
using UnityEngine;

namespace Player
{

    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;
        [Header("HorizintalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;


        [Header("VerticalMovement")]
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _minVericalPosition;
        [SerializeField] private float _maxVericalPosition;


        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;
        [SerializeField] private SpriteRenderer _shadow;
        [SerializeField] [Range(0, 1)] private float _shadowSizeModificator;
        [SerializeField] [Range(0, 1)] private float _shadowAlphaModificator;

        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;

        private float _sizeModificator;
        private bool _isJumping;
        private float _startJumpVericalPosition;
        private Vector2 _shadowLocalPosition;
        private float _shadowVerticalPosition;
        private Vector2 _shadowInitialScale;
        private Color _shadowInitialColor;

        private Vector2 _movement;
       

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _shadowLocalPosition = _shadow.transform.localPosition;
            _shadowInitialScale = _shadow.transform.localScale;
            _shadowInitialColor = _shadow.color;
            float positionDifference = _maxVericalPosition - _minVericalPosition;
            float sizeDifference = _maxSize - _minSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpdateSize();

        }

        private void Update()
        {
            if (_isJumping)
                UpdateJump();

            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _movement.magnitude > 0);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
         
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }
        public void MoveVertically(float direction)
        {
            if (_isJumping)
                return;

            _movement.y = direction;
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction * _verticalSpeed;
            _rigidbody.velocity = velocity;

            if (direction == 0)
                return;
            float verticalPosition = Mathf.Clamp(transform.position.y, _minVericalPosition, _maxVericalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);

            UpdateSize();
        }

        public void Jump()
        {
            if (_isJumping)
                return;

            _isJumping = true;
            float jumpModificator = transform.localScale.y / _maxSize;
            _rigidbody.AddForce(Vector2.up * _jumpForce * jumpModificator);
            _rigidbody.gravityScale = _gravityScale * jumpModificator;
            _startJumpVericalPosition = transform.position.y;
            _shadowVerticalPosition = _shadow.transform.position.y;
        }

        private void UpdateSize()
        {
            float verticalDelte = _maxVericalPosition - transform.position.y;
            float currentSizeModificator = _minSize + _sizeModificator * verticalDelte;
            transform.localScale = Vector2.one * currentSizeModificator;
        }
        private void SetDirection(float direction)
        {
            if (_direction == Direction.Right && direction<0 || _direction==Direction.Left && direction > 0)
            {
                Flip();
            }
        }
        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _direction;
        }

        private void UpdateJump()
        {
            if(_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVericalPosition)
            {
                ResetJump();
                return;
            }
            _shadow.transform.position = new Vector2(_shadow.transform.position.x, _shadowVerticalPosition);
            float distance = transform.position.y - _startJumpVericalPosition;
            var newShadowColor = _shadowInitialColor;
            newShadowColor.a -= distance * _shadowAlphaModificator;
            _shadow.color = newShadowColor;
            _shadow.transform.localScale =_shadowInitialScale * (1 + _shadowSizeModificator * distance);
        }
        private void ResetJump()
        {
            _isJumping = false;
            _shadow.transform.localPosition = _shadowLocalPosition;
            _shadow.color = _shadowInitialColor;
            _rigidbody.position = new Vector2(_rigidbody.position.x, _startJumpVericalPosition);
            _rigidbody.gravityScale = 0;
        }
        
        public void StartAttack()
        {
            if (!_animator.PlayAnimation(AnimationType.Attack, true))
                return;

            _animator.AnimationEnded += EndAttack;
            _animator.AnimationRequested += Attack;
        }
        private void Attack()
        {

        }
         private void EndAttack()
        {
            _animator.AnimationEnded -= EndAttack;
            _animator.AnimationRequested -= Attack;
            _animator.PlayAnimation(AnimationType.Attack, false);
        }
        public void LookUp() => _animator.PlayAnimation(AnimationType.LookUp, true);
        
        public void EndLookUp()=> _animator.PlayAnimation(AnimationType.LookUp, false);
       


    }
}
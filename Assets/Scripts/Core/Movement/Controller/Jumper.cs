using Assets.Scripts.Core.Movement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Movement.Controller
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _maxVerticalSize;
        private readonly Transform _transform;

        private readonly Transform _shadowTransform;
        private readonly Vector2 _shadowLocalPosition;
        private readonly Vector2 _shadowLocalScale;
        private readonly Color _shadowColor;


        private float _startJumpVerticalPosition;
        private float _shadowVerticalPosition;
        public bool IsJumping { get; private set; }

        public Jumper( Rigidbody2D rigidbody, JumpData jumpData, float maxVerticalSize)
        {
            _jumpData = jumpData;
            _rigidbody = rigidbody;
            _maxVerticalSize = maxVerticalSize;
  
            _shadowTransform = _jumpData.Shadow.transform;
            _shadowLocalPosition = _shadowTransform.localPosition;
            _shadowLocalScale = _shadowTransform.localScale;
            _shadowColor = _jumpData.Shadow.color;
            _transform = _rigidbody.transform;
        }

        public void Jump()
        {
            if (IsJumping)
                return;

            IsJumping = true;
            _startJumpVerticalPosition = _rigidbody.position.y;
            var jumpModificator = _transform.localScale.y / _maxVerticalSize;
            var currentJumpForce = _jumpData.JumpForce  * jumpModificator;
            _rigidbody.gravityScale = _jumpData.GravityScale * jumpModificator;
            _rigidbody.AddForce(Vector2.up * currentJumpForce);
            _shadowVerticalPosition = _shadowTransform.position.y;
        }
        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _transform.position.y < _startJumpVerticalPosition)
            {
                ResetJump();
                return;
            }

            var distance = _rigidbody.transform.position.y - _startJumpVerticalPosition;
            _shadowTransform.position = new(_shadowTransform.position.x, _shadowVerticalPosition);
            _shadowTransform.localScale = _shadowLocalScale * (1 + (_jumpData.ShadowSizeModificator * distance));
            var newShadowColor = _shadowColor;
            newShadowColor.a -= distance * _jumpData.ShadowAlphaModificator;
            _jumpData.Shadow.color = newShadowColor;
        }

        private void ResetJump()
        {
            _rigidbody.gravityScale = 0;
            _transform.position = new(_transform.position.x, _startJumpVerticalPosition);

            _shadowTransform.localScale = _shadowLocalScale;
            _shadowTransform.localPosition = _shadowLocalPosition;
            _jumpData.Shadow.color = _shadowColor;

            IsJumping = false;
        }
    }
}

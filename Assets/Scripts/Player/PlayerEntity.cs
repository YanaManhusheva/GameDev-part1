using Assets.Scripts.Core.Animation;
using Assets.Scripts.Core.Movement.Controller;
using Assets.Scripts.Core.Movement.Data;
using Assets.Scripts.Core.Tools;
using Assets.Scripts.StatsSystem;
using UnityEngine;

namespace Assets.Scripts.Player
{

    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;
        [SerializeField] private DirectionalMovementData _directionalMovementData;
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        private DirectionalMover _directionalMover;
        private Jumper _jumper;


        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
     
            _directionalMover = new DirectionalMover(_rigidbody, _directionalMovementData, statValueGiver);
            _jumper = new Jumper(_rigidbody, _jumpData, _directionalMovementData.MaxSize, statValueGiver);
         
        }

        private void Update()
        {
            if (_jumper.IsJumping)
                _jumper.UpdateJump();

            UpdateAnimations();
            UndateCameras();
        }
        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _directionalMover.IsMoving);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);

        }

        private void UndateCameras()
        {
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _directionalMover.Direction;
        }

        public void MoveHorizontally(float direction) => _directionalMover.MoveHorizontally(direction);

        public void MoveVertically(float direction)
        {
            if (_jumper.IsJumping)
                return;

            _directionalMover.MoveVertically(direction);
        }

        public void Jump() => _jumper.Jump();

        public void StartAttack()
        {
            if (!_animator.PlayAnimation(AnimationType.Attack, true))
                return;

            _animator.AnimationEnded += EndAttack;
            _animator.AnimationRequested += Attack;
        }
        private void Attack() { }
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

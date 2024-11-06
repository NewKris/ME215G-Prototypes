using System;
using Demos.Runtime.Common;
using UnityEngine;

namespace Demos.Runtime.RogueLike {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacter : MonoBehaviour {
        [Header("Moving")]
        public float maxMoveSpeed;
        public float accelerationTime;
        public AnimationCurve velocityCurve;

        [Header("Jumping")] 
        public float jumpHeight;
        public float jumpDuration;
        public int noOfAirJumps;
        
        [Header("Ground Check")]
        public float radius;
        public float height;
        public float distance;
        public LayerMask groundMask;

        private bool _isGrounded;
        private float _jumpForce;
        private int _airJumpCount;
        private SoftAxis _moveDirection;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidBody;

        public float MoveDirection {
            set => _moveDirection.TargetValue = value;
        }
        
        private bool IsMoving => _moveDirection.TargetValue != 0;
        private Vector2 GroundCastStart => transform.position.To2D() + Vector2.up * height;

        public void Jump() {
            if (!_isGrounded) {
                if (_airJumpCount >= noOfAirJumps) {
                    return;
                }

                _airJumpCount++;
            }
            
            _rigidBody.AddForce(Vector2.up * (_jumpForce -_rigidBody.velocity.y), ForceMode2D.Impulse);
        }
        
        private void Awake() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _moveDirection = new SoftAxis(accelerationTime);
            
            CalculateJumpValues();
        }

        private void FixedUpdate() {
            _isGrounded = CalculateIsGrounded();
            
            if (_isGrounded) {
                _airJumpCount = 0;
            }
            
            MoveCharacter(Time.fixedDeltaTime);
        }
        
        private void MoveCharacter(float dt) {
            if (IsMoving) {
                _spriteRenderer.flipX = _moveDirection.TargetValue < 0;
            }
            
            float targetVelocity = _moveDirection.CurrentDirection 
                                   * maxMoveSpeed 
                                   * velocityCurve.Evaluate(Mathf.Abs(_moveDirection.Tick(dt)));
            
            float deltaVelocity = targetVelocity - _rigidBody.velocity.x;
            _rigidBody.AddForce(Vector2.right * deltaVelocity, ForceMode2D.Impulse);
        }
        
        private bool CalculateIsGrounded() {
            RaycastHit2D hit = Physics2D.CircleCast(
                GroundCastStart, 
                radius, 
                Vector2.down, 
                distance, 
                groundMask
            );

            return hit.collider;
        }

        private void CalculateJumpValues() {
            float t = jumpDuration * 0.5f;
            
            _jumpForce = (2 * jumpHeight) / t;

            float targetGravity = (-2 * jumpHeight) / (t * t);
            _rigidBody.gravityScale = targetGravity / Physics2D.gravity.y;
        }
        
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GroundCastStart, radius);
            Gizmos.DrawWireSphere(GroundCastStart + Vector2.down * distance, radius);
        }
    }
}
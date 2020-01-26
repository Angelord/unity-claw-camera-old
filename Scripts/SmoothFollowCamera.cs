using System.Collections.Generic;
using UnityEngine;

namespace Claw.CameraControl {
    public class SmoothFollowCamera : CameraBehaviour {

        public float MinSpeed = 1.0f;
        public float MaxSpeed = 5.0f;
        public float MaxSpeedDistance = 3.0f;
        [SerializeField] private List<Transform> _targets = new List<Transform>();
        
        private void Update() {
            
            if(_targets.Count == 0) return;

            Vector2 center = CalculateTargetCenter();
            
            FocusOn(center);
        }
        
        private Vector2 CalculateTargetCenter() {

            Vector2 center = Vector2.zero;
            foreach (var target in _targets) {
                center += (Vector2)target.position;
            }

            center /= _targets.Count;

            return center;
        }

        private void FocusOn(Vector2 position) {

            Vector2 moveDir = position - (Vector2)transform.position;

            float magnitude = moveDir.magnitude;
            
            if (magnitude <= MinSpeed * Time.deltaTime) {
                transform.Translate(moveDir);
                return;
            }

            float lerpFactor = Mathf.Clamp(magnitude / MaxSpeedDistance, 0.0f, 1.0f);
            float speed = MinSpeed + (MaxSpeed - MinSpeed) * lerpFactor;
            
            moveDir.Normalize();
            
            transform.Translate(speed * Time.deltaTime * moveDir);
        }
        
        #region EditorOnly

        private void OnDrawGizmosSelected() {

            Gizmos.color = Color.yellow;

            Vector2 center = CalculateTargetCenter();
            
            Gizmos.DrawWireSphere(center, MaxSpeedDistance);
            
            Gizmos.color = new Color(1f, 0.51f, 0.27f);

            Gizmos.DrawLine(center, transform.position);
        }

        #endregion
    }
}

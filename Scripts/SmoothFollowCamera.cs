using System.Collections.Generic;
using UnityEngine;

namespace Claw.CameraControl {
    public class SmoothFollowCamera : CameraBehaviour {

        public bool FocusAtStart = true;
        public float MinSpeed = 1.0f;
        public float MaxSpeed = 5.0f;
        public float MaxSpeedDistance = 3.0f;
        [SerializeField] private List<Transform> _targets = new List<Transform>();

        private void Start() {
            if (FocusAtStart) {
                Vector3 targetPos = CalculateTargetCenter();
                targetPos.z = transform.position.z;
                transform.position = targetPos;
            }
        }

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

            float speed = Mathf.Lerp(MinSpeed, MaxSpeed, magnitude / MaxSpeedDistance);
            
            float step = Time.deltaTime * speed;
            if (magnitude < step) {
                transform.Translate(moveDir);
                return;
            }
            
            if (magnitude > 1.0f) {
                moveDir.Normalize();
            }

            transform.Translate(step * moveDir);
        }
        
        #region EditorOnly

        private void OnDrawGizmosSelected() {

            if(_targets == null || _targets.Count == 0) return;
            
            Gizmos.color = Color.yellow;

            Vector2 center = CalculateTargetCenter();
            
            Gizmos.DrawWireSphere(center, MaxSpeedDistance);
            
            Gizmos.color = new Color(1f, 0.51f, 0.27f);

            Gizmos.DrawLine(center, transform.position);
        }

        #endregion
    }
}

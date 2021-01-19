using System.Collections.Generic;
using UnityEngine;

namespace Claw.Camera {
    public class SmoothFollowCamera : CameraBehaviour {

        [SerializeField] private bool focusAtStart = true;
        [SerializeField] private float minSpeed = 1.0f;
        [SerializeField] private float maxSpeed = 5.0f;
        [SerializeField] private float maxSpeedDistance = 3.0f;
        [SerializeField] private List<Transform> targets = new List<Transform>();

        private void Start() {
            if (focusAtStart) {
                Vector3 targetPos = CalculateTargetCenter();
                targetPos.z = transform.position.z;
                transform.position = targetPos;
            }
        }

        private void Update() {
            
            if(targets.Count == 0) return;

            Vector2 center = CalculateTargetCenter();
            
            FocusOn(center);
        }
        
        private Vector2 CalculateTargetCenter() {

            Vector2 center = Vector2.zero;
            foreach (var target in targets) {
                center += (Vector2)target.position;
            }

            center /= targets.Count;

            return center;
        }

        private void FocusOn(Vector2 position) {

            Vector2 moveDir = position - (Vector2)transform.position;

            float magnitude = moveDir.magnitude;

            float speed = Mathf.Lerp(minSpeed, maxSpeed, magnitude / maxSpeedDistance);
            
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

            if(targets == null || targets.Count == 0) return;
            
            Gizmos.color = Color.yellow;

            Vector2 center = CalculateTargetCenter();
            
            Gizmos.DrawWireSphere(center, maxSpeedDistance);
            
            Gizmos.color = new Color(1f, 0.51f, 0.27f);

            Gizmos.DrawLine(center, transform.position);
        }

        #endregion
    }
}

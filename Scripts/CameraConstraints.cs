using Claw.Objects;
using UnityEngine;

namespace Claw.CameraControl {
    public class CameraConstraints : CameraBehaviour {

        public WorldBounds2D Bounds;
        public bool XAxis = true;
        public bool YAxis = true;
        private Vector3 _posLastFrame;

        protected override void OnStart() {
            _posLastFrame = transform.position;
        }

        private void LateUpdate() {
            if(Bounds == null) return;
            
            Vector2 cameraBottomLeft = Camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
            Vector2 cameraTopRight = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

            Vector3 adjustmentOffset = Vector2.zero;

            if (XAxis) {
                adjustmentOffset.x = CalculateOffset(
                    Bounds.Left,
                    Bounds.Right,
                    cameraBottomLeft.x,
                    cameraTopRight.x,
                    _posLastFrame.x - transform.position.x);
            }

            if (YAxis) {
                adjustmentOffset.y = CalculateOffset(
                    Bounds.Bottom,
                    Bounds.Top,
                    cameraBottomLeft.y,
                    cameraTopRight.y,
                    _posLastFrame.y - transform.position.y);
            }

            transform.position += adjustmentOffset;
        }

        private float CalculateOffset(float boundsMin, float boundsMax, float camMin, float camMax, float posDiffLastFrame) {
            bool minOverflow = camMin < boundsMin;
            bool maxOverflow = camMax > boundsMax;
            
            if (minOverflow && maxOverflow) {    // If we're overflowing in both directions, it is better to lock the camera from any movement.
                return posDiffLastFrame;
            }
            else if (minOverflow) {
                return boundsMin - camMin;
            }
            else if (maxOverflow) {
                return boundsMax - camMax;
            }

            return 0.0f;
        }

        #region EditorOnly
        
        private void OnDrawGizmosSelected() {
            if(Bounds == null) return;
            
            Gizmos.color = Color.blue;

            if (XAxis) {
                Gizmos.DrawLine(Bounds.BottomLeft, Bounds.TopLeft);
                Gizmos.DrawLine(Bounds.BottomRight, Bounds.TopRight);
            }

            if (YAxis) {
                Gizmos.DrawLine(Bounds.TopLeft, Bounds.TopRight);
                Gizmos.DrawLine(Bounds.BottomLeft, Bounds.BottomRight);
            }
        }
        #endregion
    }
}
using UnityEngine;

namespace Claw.CameraControl {
    public class CameraConstraints : CameraBehaviour {

        [SerializeField] private Bounds bounds;
        [SerializeField] private bool xAxis = true;
        [SerializeField] private bool yAxis = true;
        
        private Vector3 posLastFrame;

        public Bounds Bounds { get => bounds; set => bounds = value; }

        protected override void OnStart() {
            posLastFrame = transform.position;
        }

        private void LateUpdate() {
            Vector2 cameraBottomLeft = Camera.ScreenToWorldPoint(new Vector3(Screen.width * Camera.rect.xMin, Screen.height * Camera.rect.yMin, 0.0f));
            Vector2 cameraTopRight = Camera.ScreenToWorldPoint(new Vector3(Screen.width * Camera.rect.xMax, Screen.height * Camera.rect.yMax, 0.0f));

            Vector3 adjustmentOffset = Vector2.zero;
            
            if (xAxis) {
                adjustmentOffset.x = CalculateOffset(
                    bounds.min.x,
                    bounds.max.x,
                    cameraBottomLeft.x,
                    cameraTopRight.x,
                    posLastFrame.x - transform.position.x);
            }

            if (yAxis) {
                adjustmentOffset.y = CalculateOffset(
                    bounds.min.y,
                    bounds.max.y,
                    cameraBottomLeft.y,
                    cameraTopRight.y,
                    posLastFrame.y - transform.position.y);
            }

            transform.position += adjustmentOffset;
        }

        private float CalculateOffset(float boundsMin, float boundsMax, float camMin, float camMax, float posDiffLastFrame) {
            bool minOverflow = camMin < boundsMin;
            bool maxOverflow = camMax > boundsMax;
            
            if (minOverflow && maxOverflow) {    // If we're overflowing in both directions, it is better to lock the camera from any movement.
                return posDiffLastFrame;
            }
            if (minOverflow) {
                return boundsMin - camMin;
            }
            if (maxOverflow) {
                return boundsMax - camMax;
            }

            return 0.0f;
        }
    }
}
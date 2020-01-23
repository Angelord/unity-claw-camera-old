using System;
using UnityEngine;

namespace Claw.CameraControl {
    public class PanCamera : CameraBehaviour {

        [Range(0.0f, 0.4f)] public float Margin = 0.08f;
        public Bounds2D Bounds = new Bounds2D(Vector2.zero, new Vector2(10.0f, 10.0f));
        public float Speed = 5.0f;
        public string HorizontalInputAxis = "Horizontal";
        public string VerticalInputAxis = "Vertical";
        private bool _lockX;
        private bool _lockY;

        private void Update() {
            
            HandlePan();
            
            ApplyConstraints();
        }

        
        private void HandlePan() {
            Vector2 mousePos = Input.mousePosition;

            float mousePosXNormalized = mousePos.x / Screen.width;
            float mousePosYNormalized = mousePos.y / Screen.height;

            Vector2 pan = Vector2.zero;

            pan.x = _lockX ? 0.0f : CalculatePan(mousePosXNormalized);
            pan.y = _lockY ? 0.0f : CalculatePan(mousePosYNormalized);
            
            pan.x += Input.GetAxis(HorizontalInputAxis);
            pan.y += Input.GetAxis(VerticalInputAxis);
            
            pan.Normalize();

            transform.Translate(Speed * Time.deltaTime * (Vector3)pan);
        }

        private float CalculatePan(float mousePosNormalized) {
            if (mousePosNormalized < Margin) {
                return -1.0f;
            }
            if (mousePosNormalized > 1.0f - Margin) {
                return 1.0f;
            }

            return 0.0f;
        }

        private void ApplyConstraints() {
            Vector2 cameraBottomLeft = Camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
            Vector2 cameraTopRight = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

            Vector3 adjustmentOffset = Vector3.zero;

            bool leftOverflow = cameraBottomLeft.x < Bounds.Left;
            bool rightOverflow = cameraTopRight.x > Bounds.Right;
            bool bottomOverflow = cameraBottomLeft.y < Bounds.Bottom;
            bool topOverflow = cameraTopRight.y > Bounds.Top;
            
            // Lock movement on a specific axis if we have overflow in both directions,
            _lockX = leftOverflow && rightOverflow;
            _lockY = topOverflow && bottomOverflow;

            // If we have overflow in both directions we don't adjust, as that
            // would only lead to the camera jittering back and forth.
            if (leftOverflow && !rightOverflow) {
                adjustmentOffset.x += Bounds.Left - cameraBottomLeft.x;
            }
            else if (rightOverflow && !leftOverflow) {
                adjustmentOffset.x -= cameraTopRight.x - Bounds.Right;
            }
            
            if (bottomOverflow && !topOverflow) {
                adjustmentOffset.y += Bounds.Bottom - cameraBottomLeft.y;
            }
            else if (topOverflow && !bottomOverflow) {
                adjustmentOffset.y -= cameraTopRight.y - Bounds.Top;
            }
            
            transform.position += adjustmentOffset;
        }
        
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Bounds.Center, Bounds.Size);
        }
    }
}

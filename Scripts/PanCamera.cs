using System;
using UnityEngine;

namespace Claw.CameraControl {
    public class PanCamera : CameraBehaviour {

        [Range(0.0f, 0.4f)] public float Margin = 0.08f;
        public Bounds2D Bounds = new Bounds2D(Vector2.zero, new Vector2(10.0f, 10.0f));
        public float Speed = 5.0f;
        public string HorizontalInput = "Horizontal";
        public string VerticalInput = "Vertical";
        
        private void Update() {
            
            HandlePan();
            
            ClampPos();
        }

        
        private void HandlePan() {
            Vector2 mousePos = Input.mousePosition;

            float mousePosXNormalized = mousePos.x / Screen.width;
            float mousePosYNormalized = mousePos.y / Screen.height;

            Vector2 pan = Vector2.zero;
            
            if (mousePosXNormalized < Margin) {
                pan.x = -1.0f;
            }
            else if (mousePosXNormalized > 1.0f - Margin) {
                pan.x = 1.0f;
            }

            if (mousePosYNormalized < Margin) {
                pan.y = -1.0f;
            }
            else if (mousePosYNormalized > 1.0f - Margin) {
                pan.y = 1.0f;
            }

            pan.x += Input.GetAxis(HorizontalInput);
            pan.y += Input.GetAxis(VerticalInput);
            
            pan.Normalize();

            transform.Translate(Speed * Time.deltaTime * (Vector3)pan);
        }

        private void ClampPos() {
            Vector2 cameraBottomLeft = Camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
            Vector2 cameraTopRight = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

            Vector3 adjustmentOffset = Vector3.zero;
        
            if (cameraBottomLeft.x < Bounds.Left) {
                adjustmentOffset.x += Bounds.Left - cameraBottomLeft.x;
            }
            else if (cameraTopRight.x > Bounds.Right) {
                adjustmentOffset.x -= cameraTopRight.x - Bounds.Right;
            }
        
            if (cameraBottomLeft.y < Bounds.Bottom) {
                adjustmentOffset.y += Bounds.Bottom - cameraBottomLeft.y;
            }
            else if (cameraTopRight.y > Bounds.Top) {
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

using System;
using UnityEngine;

namespace Claw.CameraControl {
    public class PanCamera : CameraBehaviour {

        [Range(0.0f, 0.4f)] public float Margin = 0.08f;
        public float Speed = 10.0f;
        public string HorizontalInputAxis = "Horizontal";
        public string VerticalInputAxis = "Vertical";

        private void Update() {
         
            Vector2 mousePos = Input.mousePosition;

            float mousePosXNormalized = mousePos.x / Screen.width;
            float mousePosYNormalized = mousePos.y / Screen.height;

            Vector2 pan = Vector2.zero;

            pan.x = CalculatePan(mousePosXNormalized);
            pan.y = CalculatePan(mousePosYNormalized);
            
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
    }
}

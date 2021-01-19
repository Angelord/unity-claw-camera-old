using System;
using UnityEngine;

namespace Claw.CameraControl {
    public class PanCamera : CameraBehaviour {

        [SerializeField][Range(0.0f, 0.4f)] private float margin = 0.08f;
        [SerializeField] public float speed = 10.0f;
        [SerializeField] public string horizontalInputAxis = "Horizontal";
        [SerializeField] public string verticalInputAxis = "Vertical";

        private void Update() {
         
            Vector2 mousePos = Input.mousePosition;

            float mousePosXNormalized = mousePos.x / Screen.width;
            float mousePosYNormalized = mousePos.y / Screen.height;

            Vector2 pan = Vector2.zero;

            pan.x = CalculatePan(mousePosXNormalized);
            pan.y = CalculatePan(mousePosYNormalized);
            
            pan.x += Input.GetAxis(horizontalInputAxis);
            pan.y += Input.GetAxis(verticalInputAxis);
            
            pan.Normalize();

            transform.Translate(speed * Time.deltaTime * (Vector3)pan);
        }

        private float CalculatePan(float mousePosNormalized) {
            if (mousePosNormalized < margin) {
                return -1.0f;
            }
            if (mousePosNormalized > 1.0f - margin) {
                return 1.0f;
            }

            return 0.0f;
        }
    }
}

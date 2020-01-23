using UnityEngine;

namespace Claw.CameraControl {
    public class ZoomOrthographicCamera : CameraBehaviour {

        public float ZoomSpeed = 5.0f;
        public float MaxOrthographicSize = 7.0f;
        public float MinOrthographicSize = 4.0f; 
        public string InputAxis = "Mouse ScrollWheel";

        private void Update() {
            
            float orthographicSize = Camera.orthographicSize;
            orthographicSize -= Input.GetAxis(InputAxis) * ZoomSpeed * Time.deltaTime;
            
            Camera.orthographicSize = Mathf.Clamp(orthographicSize, MinOrthographicSize, MaxOrthographicSize);
        }
    }
}
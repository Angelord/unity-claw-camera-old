using UnityEngine;

namespace Claw.Camera {
    public class ZoomOrthographicCamera : CameraBehaviour {

        [SerializeField] private float zoomSpeed = 5.0f;
        [SerializeField] private float maxOrthographicSize = 7.0f;
        [SerializeField] private float minOrthographicSize = 4.0f; 
        [SerializeField] private string inputAxis = "Mouse ScrollWheel";

        private void Update() {
            
            float orthographicSize = Camera.orthographicSize;
            orthographicSize -= Input.GetAxis(inputAxis) * zoomSpeed * Time.deltaTime;
            
            Camera.orthographicSize = Mathf.Clamp(orthographicSize, minOrthographicSize, maxOrthographicSize);
        }
    }
}
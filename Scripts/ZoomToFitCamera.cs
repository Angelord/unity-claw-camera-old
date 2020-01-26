using System.Collections.Generic;
using UnityEngine;

namespace Claw.CameraControl {

    public class ZoomToFitCamera : CameraBehaviour {

        public float MinOrthographicSize = 3.0f;
        public float MinZoomSpeed = 0.5f;    // The speed at the outer border
        public float MaxZoomSpeed = 1.0f;    // The speed at the margin
        [Range(0.0f, 1.0f)] public float Margin = 0.1f;    // How much space is left between the targets and the edge of the screen
        [SerializeField] private List<Transform> _targets = new List<Transform>();
        
        private void Update() {

            Vector2 targetExtents = CalculateTargetExtents();

            float targetOrthoSize = CalculateOrthographicSizeForExtents(targetExtents);

            SmoothZoom(targetOrthoSize);
        }

        private Vector2 CalculateTargetExtents() {
            
            Vector2 targetExtents = Vector2.zero;

            foreach (var target in _targets) {

                Vector2 distance = target.position - transform.position;
                distance.x = Mathf.Abs(distance.x);
                distance.y = Mathf.Abs(distance.y);

                targetExtents.x = Mathf.Max(targetExtents.x, distance.x);

                targetExtents.y = Mathf.Max(targetExtents.y, distance.y);
            }

            return targetExtents;
        }
        
        private float CalculateOrthographicSizeForExtents(Vector2 extents) {
            
            float screenRatio = Screen.height / (float)Screen.width;
            extents.x *= screenRatio;

            return Mathf.Max(extents.x, extents.y) * (1.0f + Margin);
        }

        private void SmoothZoom(float targetOrthoSize) {
            
            float curOrthoSize = Camera.orthographicSize;
            
            float diff = targetOrthoSize - curOrthoSize;

            float speed = CalculateSpeed(targetOrthoSize, curOrthoSize);
            
            float step = speed * Time.deltaTime;
            
            if (Mathf.Abs(diff) <= step) {
                Camera.orthographicSize = targetOrthoSize;
                return;
            }

            curOrthoSize += Mathf.Sign(diff) * step;

            curOrthoSize = Mathf.Clamp(curOrthoSize, MinOrthographicSize, float.MaxValue);

            Camera.orthographicSize = curOrthoSize;
        }

        private float CalculateSpeed(float targetOrthoSize, float curOrthoSize) {

            float marginInUnits = curOrthoSize * Margin;

            float lerpFactor = (targetOrthoSize - curOrthoSize) / marginInUnits;

            lerpFactor = Mathf.Clamp(lerpFactor, 0.0f, 1.0f);

            return MinZoomSpeed + (MaxZoomSpeed - MinZoomSpeed) * lerpFactor;
        }
    }
}
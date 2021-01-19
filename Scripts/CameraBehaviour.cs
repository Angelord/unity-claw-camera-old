using UnityEngine;

namespace Claw.CameraControl {
    /// <summary>
    /// Base class for camera-related behaviours. Gives easy access to the attached camera component.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class CameraBehaviour : MonoBehaviour {

        private new Camera camera;

        protected Camera Camera => camera;

        private void Start() {
            camera = GetComponent<Camera>();
            
            OnStart();
        }

        protected virtual void OnStart() { }
    }
}
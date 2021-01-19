using UnityEngine;

namespace Claw.Camera {
    /// <summary>
    /// Base class for camera-related behaviours. Gives easy access to the attached camera component.
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    public abstract class CameraBehaviour : MonoBehaviour {

        private new UnityEngine.Camera camera;

        protected UnityEngine.Camera Camera => camera;

        private void Start() {
            camera = GetComponent<UnityEngine.Camera>();
            
            OnStart();
        }

        protected virtual void OnStart() { }
    }
}
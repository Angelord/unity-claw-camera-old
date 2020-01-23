using UnityEngine;

namespace Claw.CameraControl {
    [RequireComponent(typeof(Camera))]
    public abstract class CameraBehaviour : MonoBehaviour {

        private Camera _camera;

        protected Camera Camera { get { return _camera;  } }

        private void Start() {
            _camera = GetComponent<Camera>();
        }
    }
}
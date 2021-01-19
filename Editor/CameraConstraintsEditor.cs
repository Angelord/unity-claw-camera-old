using Claw.CameraControl;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Claw.Camera.Editors {
    [CustomEditor(typeof(CameraConstraints)), CanEditMultipleObjects]
    public sealed class CameraConstraintsEditor : Editor {
        
        private readonly BoxBoundsHandle boundsHandle = new BoxBoundsHandle();

        // the OnSceneGUI callback uses the Scene view camera for drawing handles by default
        private void OnSceneGUI() {
            CameraConstraints cameraConstraints = (CameraConstraints)target;

            // copy the target object's data to the handle
            boundsHandle.center = cameraConstraints.Bounds.center;
            boundsHandle.size = cameraConstraints.Bounds.size;

            // draw the handle
            EditorGUI.BeginChangeCheck();
            boundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck()) {
                // record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(cameraConstraints, "Change Bounds");

                // copy the handle's updated data back to the target object
                cameraConstraints.Bounds = new Bounds {
                    center = boundsHandle.center,
                    size = boundsHandle.size
                };
            }
        }
    }
}
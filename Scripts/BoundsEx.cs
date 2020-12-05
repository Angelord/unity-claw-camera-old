using UnityEngine;

namespace Claw.CameraControl {
    public static class BoundsEx {

        public static Vector2 BottomLeft(this Bounds bounds) {
            return new Vector2(bounds.min.x, bounds.min.y);
        }
        
        public static Vector2 TopLeft(this Bounds bounds) {
            return new Vector2(bounds.min.x, bounds.max.y);
        }

        public static Vector2 TopRight(this Bounds bounds) {
            return new Vector2(bounds.max.x, bounds.max.y);
        }

        public static Vector2 BottomRight(this Bounds bounds) {
            return new Vector2(bounds.max.x, bounds.min.y);
        }
    }
}
using UnityEngine;

namespace Demos.Runtime.Common {
    public class Drone : MonoBehaviour {
        public float damping;
        public Transform target;

        private float _zPosition;
        private Vector3 _velocity;

        private Vector3 CurrentPosition => transform.position;
        private Vector3 TargetPosition => new Vector3(target.position.x, target.position.y, _zPosition);

        private void Awake() {
            _zPosition = transform.position.z;
        }

        private void Update() {
            transform.position = Vector3.SmoothDamp(CurrentPosition, TargetPosition, ref _velocity, damping);
        }
    }
}

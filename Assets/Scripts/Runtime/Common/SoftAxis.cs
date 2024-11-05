using UnityEngine;

namespace Demos.Runtime.Common {
    public struct SoftAxis {
        private readonly float _accelerationTime;

        public float TargetValue { get; set; }
        public float CurrentValue { get; set; }
        public float CurrentDirection => Mathf.Sign(CurrentValue);

        public SoftAxis(float accelerationTime) {
            _accelerationTime = accelerationTime;
            TargetValue = 0;
            CurrentValue = 0;
        }

        public void SnapToTarget() {
            CurrentValue = TargetValue;
        }
        
        public float Tick(float dt) {
            float deltaValue = TargetValue - CurrentValue;
            float maxStepSize = dt / _accelerationTime;

            CurrentValue += Mathf.Clamp(deltaValue, -maxStepSize, maxStepSize);

            return CurrentValue;
        }
    }
}
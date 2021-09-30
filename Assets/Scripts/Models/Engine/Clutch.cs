using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPhysics.Models.Engine {
    public class Clutch {
        private readonly float _minEngineVelocity;
        private readonly float _maxEngineVelocity;
        private float _clutchVelocity;
        private float _engineVelocity;
        private float _gearRatio;

        public float Torque { get; private set; }
        public float ClutchSleep { get; private set; }
        public float ClutchLock { get; private set; }
        public float ClutchStiffness { get; private set; }
        public float ClutchCapacity { get; private set; }
        public float ClutchDamping { get; private set; }
        public float ClutchMaxTorque { get; private set; }

        public Clutch(CarEngine engine, ClutchSetup setup) {
            _minEngineVelocity = setup.minEngineVelocity;
            _maxEngineVelocity = setup.maxEngineVelocity;
            ClutchStiffness = setup.clutchStiffness;
            ClutchCapacity = setup.clutchCapacity;
            ClutchDamping = setup.clutchDamping;
            ClutchMaxTorque = engine.MaxTorque * ClutchCapacity;
        }

        public void FixedUpdate(float shaftVelocity, float engineVelocity, float gearRatio) {
            _clutchVelocity = shaftVelocity;
            _engineVelocity = engineVelocity;
            _gearRatio = gearRatio;
            CalculateTorque();
        }

        private void CalculateTorque() {
            CalculateLock();
            CalculateSleep();
            var torque = ClutchSleep * ClutchLock * ClutchStiffness;
            torque = Mathf.Clamp(torque, -ClutchMaxTorque, ClutchMaxTorque);
            var changeTorque = (Torque - torque) * ClutchDamping;
            Torque = changeTorque + torque;
        }

        private void CalculateLock() {
            var _engineRPM = _engineVelocity * GPhysic.RadToRPM;
            var clampEngineVelocity = Mathf.Clamp(_engineRPM, _minEngineVelocity, _maxEngineVelocity);
            var engineVelocityRange = Mathf.InverseLerp(_minEngineVelocity, _maxEngineVelocity, clampEngineVelocity);
            engineVelocityRange += _gearRatio == 0 ? 1 : 0;
            ClutchLock = Mathf.Min(engineVelocityRange, 1);
        }

        private void CalculateSleep() {
            var velocity = _engineVelocity - _clutchVelocity;
            ClutchSleep = _gearRatio == 0 ? 0 : velocity;
        }
    }

    [System.Serializable]
    public struct ClutchSetup {
        public float minEngineVelocity;
        public float maxEngineVelocity;
        [Range(0, 100)] public float clutchStiffness;
        public float clutchCapacity;
        [Range(0, 0.9f)] public float clutchDamping;
    }
}



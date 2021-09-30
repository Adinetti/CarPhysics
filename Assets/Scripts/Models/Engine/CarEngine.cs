using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPhysics.Models.Engine {
    [System.Serializable]
    public class CarEngine {
        private readonly CarEngineSetup _carSetup;
        private readonly HoverEngineSetup _hoverSetup;
        private readonly float _maxAngularVelocity;
        private readonly float _minAngularVelocity;

        public float RPM { get; private set; }
        public float MinRPM { get; private set; }
        public float MaxRPM { get; private set; }
        public float Torque { get; private set; }
        public float MaxTorque { get; private set; }
        public float AngularVelocity { get; private set; }
        public bool IsRun { get; private set; }

        public CarEngine(CarEngineSetup setup) {
            _carSetup = setup;
            MinRPM = _carSetup.idleRPM;
            MaxRPM = _carSetup.maxRPM;
            _minAngularVelocity = MinRPM * GPhysic.RpmToRad;
            _maxAngularVelocity = MaxRPM * GPhysic.RpmToRad;
            MaxTorque = _carSetup.maxTorque;
            AngularVelocity = 100;
        }

        public CarEngine(HoverEngineSetup setup, float hoverMass) {
            _hoverSetup = setup;
            MinRPM = _hoverSetup.idleRPM;
            MaxRPM = _hoverSetup.maxRPM;
            MaxTorque = _hoverSetup.hoverAcceleration * hoverMass;
        }

        public void Start() {
            IsRun = true;
        }

        public void Stop() {
            IsRun = false;
            RPM = 0;
            Torque = 0;
            AngularVelocity = 0;
            RPM = 0;
        }

        public void FixedUpdate(float loadTorque, float throttle, float deltaTime) {
            if (IsRun) {
                Torque = CalculateTorque(throttle);
                AngularVelocity += ((Torque - loadTorque) / _carSetup.inertia) * deltaTime;
                AngularVelocity = Mathf.Clamp(AngularVelocity, _minAngularVelocity, _maxAngularVelocity);
                RPM = AngularVelocity * GPhysic.RadToRPM;
            }
        }

        private float CalculateTorque(float throttle) {
            var torque = _carSetup.torqueCurve.Evaluate(RPM / MaxRPM) * MaxTorque;
            var friction = _carSetup.startFriction + RPM * _carSetup.frictionCoeff;
            torque += friction;
            torque *= throttle;
            torque -= friction;
            return torque;
        }
    }

    [System.Serializable]
    public struct CarEngineSetup {
        public AnimationCurve torqueCurve;
        public float startFriction;
        public float frictionCoeff;
        public float maxTorque;
        public float idleRPM;
        public float maxRPM;
        public float inertia;
    }

    [System.Serializable]
    public struct HoverEngineSetup {
        public AnimationCurve torqueCurve;
        public float hoverAcceleration;
        public float idleRPM;
        public float maxRPM;
        public float rpmAcceleration;
        public float backRpmAcceleration;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CarPhysics.Models.Engine;
using System;

namespace CarPhysics.Models {
    public class Car {
        private readonly CarSetup _setup;
        private AckermannSteering _ackermannSteering;
        private float _rightAngle;
        private float _leftAngle;
        private float _steeringAngle;

        public IDrivetrain Drivetrain { get; private set; }
        public CarType Type => _setup.type;
        public float MaxSteerAngle => _setup.steeringAngle;
        public float Speed { get; private set; }
        public float MaxSpeed { get; private set; }
        public float Turn { get; private set; }
        public float Throttle { get; private set; }
        public float Breaks { get; private set; }

        public Car(CarSetup setup, AxesInfo forward, AxesInfo rear) {
            _setup = setup;
            if (_setup.withAckermannSteering) {
                CalsulateAckermannBase(forward, rear);
            }
            CreateDrivetrain(setup);
        }

        private void CalsulateAckermannBase(AxesInfo forward, AxesInfo rear) {
            var wheelFLpos = forward.leftWheel.transform.localPosition;
            var wheelBLpos = rear.leftWheel.transform.localPosition;
            var wheelBRpos = rear.rightWheel.transform.localPosition;
            var _wheelBase = Vector3.Distance(wheelFLpos, wheelBLpos);
            var _rearBase = Vector3.Distance(wheelBLpos, wheelBRpos);
            var radianAngle = Mathf.Deg2Rad * MaxSteerAngle;
            var _turnRadius = _wheelBase / Mathf.Tan(radianAngle);
            _ackermannSteering = new AckermannSteering(_wheelBase, _rearBase, _turnRadius);
        }

        private void CreateDrivetrain(CarSetup setup) {
            switch (Type) {
                case CarType.WithRealEngine:
                    Drivetrain = new ProDrivetrain(setup.proDrivetrain);
                    break;
                case CarType.WithAcradeEngine:
                    Drivetrain = new ArcadeDrivetrain(setup.arcadeDrivetrain, this);
                    break;
            }
        }

        public void Update(float deltaTime) {
            Drivetrain.Update(deltaTime);
        }

        public void FixedUpdate(AxesInfo rotateWheels, AxesInfo motorWheels, float deltaTime) {
            Steering(rotateWheels);
            Drivetrain.FixedUpdate(Throttle, motorWheels, deltaTime);
            CalculateSpeed(motorWheels);
            Breaking(rotateWheels, motorWheels);
        }

        private void Steering(AxesInfo rotateWheels) {
            CalculateSteerAngles();
            rotateWheels.leftWheel.steerAngle = _rightAngle;
            rotateWheels.rightWheel.steerAngle = _leftAngle;
        }

        private void CalculateSteerAngles() {
            if (_setup.withAckermannSteering) {
                _rightAngle = _ackermannSteering.GetAngleFor(true, Turn > 0) * Turn;
                _leftAngle = _ackermannSteering.GetAngleFor(false, Turn > 0) * Turn;
            } else {
                _rightAngle = _steeringAngle * Turn;
                _leftAngle = _steeringAngle * Turn;
            }
        }

        private void CalculateSpeed(AxesInfo motorWheels) {
            var rpm = motorWheels.leftWheel.rpm;
            rpm += motorWheels.rightWheel.rpm;
            rpm /= 2;
            Speed = 2 * Mathf.PI * motorWheels.leftWheel.radius * rpm * .06f;
            Speed = Mathf.Abs(Speed);
        }

        private void Breaking(AxesInfo rotateWheels, AxesInfo motorWheels) {
            rotateWheels.leftWheel.brakeTorque = _setup.breaksTorque * Breaks;
            rotateWheels.rightWheel.brakeTorque = _setup.breaksTorque * Breaks;
            motorWheels.leftWheel.brakeTorque = _setup.breaksTorque * Breaks;
            motorWheels.rightWheel.brakeTorque = _setup.breaksTorque * Breaks;
        }

        public void SetThrottle(float throttle) {
            Throttle = Mathf.Clamp01(throttle);
        }

        public virtual void SetSteering(float turn) {
            turn = Mathf.Clamp(turn, -1, 1);
            Turn = turn;
        }

        public void SetBreaking(float breaking) {
            Breaks = Mathf.Clamp01(breaking);
        }
    }

    [System.Serializable]
    public struct CarSetup {
        public CarType type;
        public DrivetrainSetup proDrivetrain;
        public ArcadeSetup arcadeDrivetrain;
        public float breaksTorque;
        public float steeringAngle;
        public bool withAckermannSteering;
    }
}



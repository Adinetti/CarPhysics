using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPhysics.Models.Engine {
    public class ArcadeDrivetrain : IDrivetrain {
        private readonly ArcadeSetup _setup;
        private readonly Car _car;
        private float _accelerationPower;

        public ArcadeDrivetrain(ArcadeSetup setup, Car car) {
            _setup = setup;
            _car = car;
            Gearbox = new Gearbox(_setup.arcadeGears);
            Engine = new CarEngine(_setup.engine);
        }

        public override void FixedUpdate(float throttle, AxesInfo motorWheels, float deltaTime) {
            var speed = Mathf.Abs(_car.Speed);
            var maxSpeed = GetMaxSpeed();
            var torqueLimite = maxSpeed > 0 ? _setup.motorCurve.Evaluate(speed / maxSpeed) : 0;
            var torque = torqueLimite * (Engine.Torque + _accelerationPower) * _car.Throttle;
            torque = torque * Gearbox.Ratio;
            motorWheels.leftWheel.motorTorque = torque;
            motorWheels.rightWheel.motorTorque = torque;
            Engine.FixedUpdate(0, throttle, deltaTime);
        }

        public float GetMaxSpeed() {
            for (int i = 0; i < _setup.speeds.Length; i++) {
                if (_setup.speeds[i].gear == Gearbox.Gear) {
                    return _setup.speeds[i].maxSpeed;
                }
            }
            return 0;
        }
    }

    [System.Serializable]
    public class ArcadeSetup {
        public ArcadeSpeed[] speeds;
        public AnimationCurve motorCurve;
        [Range(0, 1)] public float drag;
        public GearboxSetup arcadeGears;
        public CarEngineSetup engine;
    }
}



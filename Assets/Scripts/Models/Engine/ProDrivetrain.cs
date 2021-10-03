using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPhysics.Models.Engine { 
	public class ProDrivetrain : IDrivetrain {
        private float _leftWheelAngularVelocity;
        private float _rightWheelAngularVelocity;
        private Differential _differential;
        private Clutch _clutch;


        public ProDrivetrain(DrivetrainSetup setup) {
            Engine = new CarEngine(setup.engine);
            Gearbox = new Gearbox(setup.gearbox);
            _differential = new Differential(setup.differential);
            _clutch = new Clutch(Engine, setup.clutch);
        }

        public override void FixedUpdate(float throttle, AxesInfo motorWheels, float deltaTime) {
            Gearbox.FixedUpdate(_clutch.Torque);
            _differential.FixedUpdate(Gearbox.Torque);
            UpdateWheels(motorWheels);
            var shaftVelocity = _differential.GetShaftVelocity(_leftWheelAngularVelocity, _rightWheelAngularVelocity);
            shaftVelocity = Gearbox.GetInputShaftVelocity(shaftVelocity);
            _clutch.FixedUpdate(shaftVelocity, Engine.AngularVelocity, Gearbox.Ratio);
            Engine.FixedUpdate(_clutch.Torque, throttle, deltaTime);
        }

        private void UpdateWheels(AxesInfo motorWheels) {
            motorWheels.leftWheel.motorTorque = _differential.LeftRearTorque;
            motorWheels.rightWheel.motorTorque = _differential.RightRearTorque;
            _leftWheelAngularVelocity = motorWheels.leftWheel.rpm * GPhysic.RpmToRad;
            _rightWheelAngularVelocity = motorWheels.leftWheel.rpm * GPhysic.RpmToRad;
        }
    }

    [System.Serializable] 
    public class DrivetrainSetup {
        public CarEngineSetup engine;
        public GearboxSetup gearbox;
        public DifferentialSetup differential;
        public ClutchSetup clutch;
    }
}



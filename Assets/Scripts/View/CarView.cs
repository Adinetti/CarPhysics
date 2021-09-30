using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarPhysics.Models.Engine;
using CarPhysics.Models;

namespace CarPhysics {
    [System.Serializable]
    public enum CarType { WithRealEngine, WithAcradeEngine }

    [System.Serializable]
    public struct ArcadeSpeed {
        public GearName gear;
        public float maxSpeed;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CarView : MonoBehaviour {
        [Header("Wheels")]
        [SerializeField] private AxesInfo _forwardAxe;
        [SerializeField] private AxesInfo _rearAxe;
        [Header("Car setup")]
        [SerializeField] private CarSetup _carSetup;

        public Car Car { get; private set; }

        private void Awake() {
            Car = new Car(_carSetup, _forwardAxe, _rearAxe);
            Car.Drivetrain.Engine.Start();
        }


        private void Update() {
            Car.Update(Time.deltaTime);
        }

        private void FixedUpdate() {
            Car.FixedUpdate(_forwardAxe, _rearAxe, Time.fixedDeltaTime);
        }
    }
}



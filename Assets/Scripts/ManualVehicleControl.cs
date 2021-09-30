using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CarPhysics {
    public class ManualVehicleControl : MonoBehaviour {
        [SerializeField] private bool _isActive = true;
        [SerializeField] private CarView _carView;
        private bool _readyToSwicthGear;

        private void Awake() {
            _readyToSwicthGear = true;
        }

        private void Update() {
            if (_isActive && _carView != null) {
                if (Input.GetKeyUp(KeyCode.KeypadEnter)) {
                    _carView.Car.Drivetrain.Engine.Start();
                }
                if (Input.GetKeyUp(KeyCode.Backspace)) {
                    _carView.Car.Drivetrain.Engine.Stop();
                }
                var turn = Input.GetAxis("Horizontal");
                _carView.Car.SetSteering(turn);
                var throttle = Input.GetAxis("Throttle");
                _carView.Car.SetThrottle(throttle);
                var breaking = Input.GetAxis("Break");
                _carView.Car.SetBreaking(breaking);
                SwitchGear();
            }
        }

        private void SwitchGear() {
            var gear = Input.GetAxis("SwitchGear");
            SwicthGear(gear);
            if (Mathf.Abs(gear) < 0.001f) {
                _readyToSwicthGear = true;
            }
        }

        private void SwicthGear(float gear) {
            if (Mathf.Abs(gear) > 0.5) {
                if (_readyToSwicthGear) {
                    if (gear > 0f) {
                        _carView.Car.Drivetrain.Gearbox.SwitchToNextGear();
                
                    } else {
                        _carView.Car.Drivetrain.Gearbox.SwitchToPrevGear();
                    }
                    _readyToSwicthGear = false;
                }
            }
        }
    }
}


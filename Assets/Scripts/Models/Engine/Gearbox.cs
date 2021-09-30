using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CarPhysics.Models.Engine {
    [System.Serializable]
    public enum GearName { R, N, G01, G02, G03, G04, G05, G06, G07, G08, G09 }
    public enum GearboxState { Wait, Swicth }

    [System.Serializable]
    public class Gearbox {
        private readonly GearSetup[] _gears;
        private float _swicthTime;
        private float _timer;
        private int _nextGearID;
        private int _gearID;

        public GearName Gear => _gears[_gearID].name;
        public int GearCount => _gears.Length;
        public int GearID => _gearID;
        public GearboxState State { get; private set; }
        public float Torque { get; private set; }
        public float Ratio => _gears[_gearID].ratio;

        public Gearbox(GearboxSetup setup) {
            _gears = setup.gears;
            SwicthToNeitralGear();
            _swicthTime = setup.switchTime;
            State = GearboxState.Wait;
            _nextGearID = -1;
        }

        public bool SwicthToNeitralGear() {
            if (State == GearboxState.Wait) {
                for (int i = 0; i < _gears.Length; i++) {
                    if (_gears[i].name == GearName.N) {
                        _gearID = i;
                        State = GearboxState.Swicth;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SwitchToNextGear() {
            return SwitchToGear(true);
        }

        private bool SwitchToGear(bool next) {
            var isValideSwicth = next ? _gearID < _gears.Length - 1 : _gearID > 0;
            if (State == GearboxState.Wait && isValideSwicth) {
                _nextGearID = next ? _gearID + 1 : _gearID - 1;
                return SwicthToNeitralGear();
            }
            return false;
        }

        public bool SwitchToPrevGear() {
            return SwitchToGear(false);
        }

        public void Update(float deltaTime) {
            if (State == GearboxState.Swicth) {
                _timer += deltaTime;
                if (_timer >= _swicthTime) {
                    if (_nextGearID > -1) {
                        _gearID = _nextGearID;
                        _nextGearID = -1;
                    }
                    _timer = 0;
                    State = GearboxState.Wait;
                }
            }
        }

        public void FixedUpdate(float clutchTorque) {
            CalculateTorque(clutchTorque);            
        }

        private void CalculateTorque(float torque) {
            Torque = torque * Ratio;
        }

        public float GetInputShaftVelocity(float outputShaftVelocity) {
            return outputShaftVelocity * Ratio;
        }
    }

    [System.Serializable]
    public struct GearSetup {
        public GearName name;
        public float ratio;
    }

    [System.Serializable]
    public struct GearboxSetup {
        public GearSetup[] gears;
        public float switchTime;
    }
}



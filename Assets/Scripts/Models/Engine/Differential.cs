using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CarPhysics.Models.Engine { 
	public class Differential  {
		public float Ratio { get; private set; }
		public float LeftRearTorque;
		public float RightRearTorque;

		public Differential(DifferentialSetup setup) {
			Ratio = setup.ratio;
		}

		public void FixedUpdate(float torque) {			
			CalculateTorque(torque);
        }

		private void CalculateTorque(float torque) {
			LeftRearTorque = torque * Ratio * 0.5f;
			RightRearTorque = LeftRearTorque;
		}

		public float GetShaftVelocity(float shaftVelocityLeft, float shaftVelocityRight) {
			var input = shaftVelocityLeft + shaftVelocityRight;
			return input * Ratio * 0.5f;
		}
	}

	[System.Serializable]
	public struct DifferentialSetup {
		public float ratio;
    }
}



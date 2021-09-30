using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CarPhysics { 
	public static class GPhysic {
		private static float _rpmToRad;
		private static float _radToRPM;

		public static float RpmToRad {
			get {
				if (_rpmToRad == 0) {
					_rpmToRad = Mathf.PI / 30f; 
				}
				return _rpmToRad;
			}
		}

		public static float RadToRPM {
			get {
				if (_radToRPM == 0) {
					_radToRPM = 1f / RpmToRad;
				}
				return _radToRPM;
			}
		}
	}
}



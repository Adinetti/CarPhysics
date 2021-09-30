using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CarPhysics.Models.Engine { 
	public abstract class IDrivetrain {
		public CarEngine Engine { get; protected set; }
		public Gearbox Gearbox { get; protected set; }

		public void Update(float deltaTime) {
			Gearbox.Update(deltaTime);
        }

		public abstract void FixedUpdate(float throttle, AxesInfo motorWheels, float deltaTime);
	}
}



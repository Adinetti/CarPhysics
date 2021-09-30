using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CarPhysics.Models {
    public class AckermannSteering {
        public float BigAngle { get; private set; }
        public float SmallAngle { get; private set; }

        public AckermannSteering(float wheelBase, float rearTrack, float turnRadius) {
            BigAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2)));
            SmallAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2)));
        }

        public float GetAngleFor(bool rightWheel, bool rightTurn) {
            if (rightTurn) {
                return rightWheel ? SmallAngle : BigAngle;
            }
            return rightWheel ? BigAngle : SmallAngle;
        }
    }
}



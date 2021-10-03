# This is arcade and pro physics for unity car.

**DEFAULT SETTINGS.**

ENGINE:
- startFriction = 50
- frictionCoeff = 0.2
- maxTorque = 400
- idleRPM = 700
- maxRPM = 7000
- inertia = 0.3
	
CLUTCH:
- minEngineVelocity = 1000;
- maxEngineVelocity = 1300
- clutchStiffness = 30
- clutchCapacity = 1.3
- clutchDamping = 0.7

**WHEEL COLLIDERS SETUP**

FORWARD FRICTION:
- Extremum slip = 2
- Extremum value = 5
- Asymptote slip = 5
- Asymptote value = 2
- Stiffness = 1

SIDE FRICTION:
- Extremum slip = 0.4
- Extremum value = 1
- Asymptote slip = .5
- Asymptote value = .75
- Stiffness = 1 (forward wheels) | 1.5-2 (rear wheels)

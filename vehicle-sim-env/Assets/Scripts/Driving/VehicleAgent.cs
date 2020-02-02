using UnityEngine;
using System;

using MLAgents;

public class VehicleAgent : Agent
{
	[Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
	[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("Lane Size of the lane from Road Architech")]
	public float laneWidth = 5f;
	public float vehicleWidth = 1.89f;
	public float shoulderWidth = 3f;
	public float markerWidthFromRoad = 7f;

	public float wheelAngle = 0.0f;
	public float constantTorque = 100f;

	public GameObject fd_collision;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

	private WheelCollider[] m_Wheels;
    private CollidingWheel frontDriver;
	private CollidingWheel frontPassenger;
	private CollidingWheel backDriver;
	private CollidingWheel backPassenger;

	private float frontDistanceToCenter = 0.0f;
	private float backDistanceToCenter = 0.0f;

	// Find all the WheelColliders down in the hierarchy.
	void Start()
	{
		m_Wheels = GetComponentsInChildren<WheelCollider>();

		var chassis = GameObject.Find("FamilyCarChassis").GetComponent<MeshFilter>().mesh.bounds.size;

		for (int i = 0; i < m_Wheels.Length; ++i)
		{
            var wheel = m_Wheels[i];
            var children = wheel.GetComponentsInChildren<CollidingWheel>();
            print("wheel name: " + wheel.name);
            for (int j = 0; j < children.Length; ++j) {
				var child = children[j];

                switch (child.name)
                {
					case "front_driver_collision_box":
						frontDriver = child;
						break;
					case "front_passenger_collision_box":
						frontPassenger = child;
						break;
					case "back_driver_collision_box":
					    backDriver = child;
						break;
					case "back_passenger_collision_box":
						backPassenger = child;
						break;
				}
            }

			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate(wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
	}

    public override void CollectObservations()
    {
        AddVectorObs(wheelAngle);
        AddVectorObs(frontDistanceToCenter);
		AddVectorObs(backDistanceToCenter);
    }

    public override void AgentAction(float[] vectorAction)
    {
		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);


		// calculate distance
		var frontDistanceToCenter = (frontDriver.distanceToMarker - 5.0f) - frontPassenger.distanceToMarker;
        var backDistanceToCenter = (backDriver.distanceToMarker - 5.0f) - backPassenger.distanceToMarker;

		float wheelDelta = vectorAction[0];

        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                float newAngle = wheel.steerAngle + wheelDelta;
                if (newAngle < -maxAngle)
                {
					wheel.steerAngle = -maxAngle;
				} else if (newAngle > maxAngle)
                {
					wheel.steerAngle = maxAngle;
                } else
                {
					wheel.steerAngle = newAngle;
                }
                
            }


			wheel.motorTorque = constantTorque;


			//if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
			//{
			//    wheel.motorTorque = torque;
			//}

			//if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
			//{
			//    wheel.motorTorque = torque;
			//}

			if (wheelShape)
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose(out p, out q);

				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild(0);

				if (wheel.name == "a0l" || wheel.name == "a1l" || wheel.name == "a2l")
				{
					shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
					shapeTransform.position = p;
				}
				else
				{
					shapeTransform.position = p;
					shapeTransform.rotation = q;
				}
			}
		}


        // REWARD
        if (Math.Abs(frontDistanceToCenter) > 7)
        {
			Done();
        }

		if (transform.position.y > 5.0f)
        {
			Done();
        }

		if (transform.position.y < 0.0f)

		{
			Done();
		}
	}


	public override float[] Heuristic()
	{
		//var action = new float[2];

		//action[0] = Input.GetAxis("Horizontal");
		//action[1] = Input.GetAxis("Vertical");
		//return action;

		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

		float angle = maxAngle * Input.GetAxis("Horizontal");

        print("angle " + angle);

        //float torque = maxTorque * Input.GetAxis("Vertical");

        //float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;

        foreach (WheelCollider wheel in m_Wheels)
		{
			// A simple car where front wheels steer while rear ones drive.
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;
		}

		float[] signal = new float[1];
		signal[0] = angle - wheelAngle;

		return signal;
	}

	public override void AgentReset()
	{
		this.transform.localPosition = new Vector3(
			-169.2f,
			139.60f,
			-115.86f
		);
	}
}

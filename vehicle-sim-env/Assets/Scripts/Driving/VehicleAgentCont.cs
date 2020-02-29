using UnityEngine;
using System;

using MLAgents;

public class VehicleAgentCont : Agent
{
    // PUBLIC CLASS VARIABLE
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

    [Tooltip("Constant Torque on the wheels.")]
	public float constantTorque = 100f;

	public float roadGuideOffset = 15f;
	public float laneWidth = 5f;

    [Tooltip("Vehicle Starting Position")]
    public Vector3 agentResetPosition;
	[Tooltip("Vehicle Rotation Position")]
	public Vector3 agentResetRotation;

	[Tooltip("Ending Object: ends episode when reached")]
	public GameObject endBox;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

    public float currentAngle = 0f;

    // PRIVATE VARIABLES

    // wheels
    private WheelCollider[] m_Wheels;
    private CollidingWheel frontDriver;
	private CollidingWheel frontPassenger;
	private CollidingWheel backDriver;
	private CollidingWheel backPassenger;

    // positioning
    private float wheelAngle = 0.0f;
	private float frontDistanceToCenter = 0.0f;
	private float backDistanceToCenter = 0.0f;

	private float timer = 0.0f;

	private Rigidbody rBody;

	// Find all the WheelColliders down in the hierarchy.
	void Start()
	{
		m_Wheels = GetComponentsInChildren<WheelCollider>();
		rBody = GetComponent<Rigidbody>();

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
        // Normalized obs

        // angle of the wheel (-1, 1)
        AddVectorObs(this.wheelAngle / maxAngle);

        // front axle alignment (-1, 1)
        AddVectorObs(this.frontDistanceToCenter);

        // back axle alignment (-1, 1)
        AddVectorObs(this.backDistanceToCenter);

        // Vehicle Velocity
        AddVectorObs(rBody.velocity.x);
		AddVectorObs(rBody.velocity.z);
	}

    public override void AgentAction(float[] vectorAction)
    {
		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

		/*
         *int signal = Mathf.FloorToInt(vectorAction[0]);
		Monitor.Log(
			"Signal",
			"" + signal,
			null
		 );
		var newAngle = this.wheelAngle;
		if (signal > 30) {
			newAngle = (signal+1) - 30;
        } else if (signal < 30)
        {
			newAngle = (signal+1) * -1;
        }
		if (newAngle < -maxAngle)
		{
			newAngle = -maxAngle;
		}
		else if (newAngle > maxAngle)
		{
			newAngle = maxAngle;
		}
		this.wheelAngle = newAngle;
		Monitor.Log(
            "Wheel Angle",
            "" + newAngle,
            null
         );
         */


		// calculate distance
		frontDistanceToCenter = ((frontDriver.distanceToMarker - this.laneWidth) - frontPassenger.distanceToMarker) / this.roadGuideOffset;
		backDistanceToCenter = ((backDriver.distanceToMarker - this.laneWidth) - backPassenger.distanceToMarker) / this.roadGuideOffset;

		Monitor.Log(
            "Front Distance",
            frontDistanceToCenter,
            null
        );
		Monitor.Log(
            "Back Distance",
            backDistanceToCenter,
            null
        );

		float newAngle = vectorAction[0] * this.maxAngle;
		if (newAngle < -maxAngle)
		{
			newAngle = -maxAngle;
		}
		else if (newAngle > maxAngle)
		{
			newAngle = maxAngle;
		}

        currentAngle = newAngle;

		Monitor.Log(
            "Wheel Angle",
            "" + newAngle,
            null
         );

        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
				wheel.steerAngle = newAngle; 
            }


            // Torque is constant, may change in the future
            wheel.motorTorque = constantTorque;

			//if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
			//{
			//    wheel.motorTorque = torque;
			//}

			//if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
			//{
			//    wheel.motorTorque = torque;
			//}

            // Update wheel shape (visually only)
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

        // continuous reward based on position in the lane.

		this.timer += Time.deltaTime;

		var frontDistAbs = Math.Abs(this.frontDistanceToCenter);
		var backDistAbs = Math.Abs(backDistanceToCenter);


		if (frontDistAbs < 0.2 && backDistAbs < 0.2)
		{
			var reward = (1 - frontDistAbs) * 0.25f;
			reward += (1 - backDistAbs) * 0.25f;
			AddReward(reward);

			Monitor.Log(
			    "Status",
			    "In Lane",
			    null
		    );
			Monitor.Log(
				"Reward",
				"" + Math.Round(reward, 2),
				null
			);
		}
		else {
			var reward = frontDistAbs * 0.25f;
			reward += backDistAbs * 0.25f;

            // bigger penatly for being in the other lane
            if (this.frontDistanceToCenter < 0f || this.backDistanceToCenter < 0f) {
				reward *= 2;
            }
			reward *= -1;

			AddReward(reward);

			Monitor.Log(
				"Status",
				"Out of Lane",
				null
			);
			Monitor.Log(
				"Reward",
				"" + Math.Round(reward, 2),
				null
			);
		}


        // Terminate Episode Cases

		if (frontDistAbs > 0.95 || backDistAbs > 0.95)
        {

			Monitor.Log(
				"Status",
				"Terminate",
				null
			);

			AddReward(-1.0f);
			Done();
			return;
		}

		if (this.transform.position.y > 5.0f)
        {
			Monitor.Log(
				"Status",
				"Terminate",
				null
			);

			AddReward(-1.0f);
			Done();
			return;
		}

		if (this.transform.position.y < 0.0f)

		{
			Monitor.Log(
				"Status",
				"Terminate",
				null
			);

			AddReward(-1.0f);
			Done();
			return;
		}


		var distanceToEnd = Vector3.Distance(
			endBox.transform.localPosition,
			this.transform.localPosition
		);

		if (distanceToEnd < 10.0f)
        {

			Monitor.Log(
				"Status",
				"Success",
				null
			);

			AddReward(1.0f);
			Done();
			return;
		}

		//Monitor.Log("Current Reward", "" + this.reward, null);




		//     if (Math.Abs(this.transform.eulerAngles.z) > 5)
		//     {
		//print("END: rotation z > 5");
		//Done();
		//     }
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

		this.timer = 0.0f;
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;

		this.transform.localPosition = agentResetPosition;
		this.transform.eulerAngles = agentResetRotation;
		this.wheelAngle = 0.0f;

		foreach (WheelCollider wheel in m_Wheels)
		{
			wheel.steerAngle = 0.0f;
		}
	}
}
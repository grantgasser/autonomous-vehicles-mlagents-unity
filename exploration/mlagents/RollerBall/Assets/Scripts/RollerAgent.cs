using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{

    public Rigidbody rBody;
    public Transform Target;
    public float directionAngle;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        Debug.Log("Starting RollerAgent");
        Monitor.SetActive(true);
    }

    public override void CollectObservations()
    {
        // Target and Agent Positions
        AddVectorObs(Target.localPosition);
        AddVectorObs(this.transform.localPosition);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }

    public override void AgentAction(float[] vectorAction)
    {
        // Action Size  = 2  m ,
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);
        directionAngle = Mathf.Atan2(rBody.velocity.y, rBody.velocity.x) * Mathf.Rad2Deg;

        // Rewards
        float distanceToTarget = Vector3.Distance(
            this.transform.localPosition,
            Target.localPosition
        );

        // Reached Target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            Done();
        }
    }

    public override void AgentReset()
    {
        if (this.transform.localPosition.y < 0)
        {
            // If the agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(
            Random.value * 21f - 10.5f,
            0.5f,
            Random.value * 21f - 10.5f
        );
    }

    public override float[] Heuristic()
    {
        var action = new float[2];

        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

}

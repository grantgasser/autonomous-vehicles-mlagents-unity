using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarSide
{
    Driver,
    Passenger
}

public class CollidingWheel : MonoBehaviour

{

    public CarSide side;
    public float distanceToMarker;

    private void Start()
    {
        print(name);
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 rayDirection;

        if (side == CarSide.Driver)
        {
            rayDirection = transform.TransformDirection(Vector3.left);
        }
        else
        {
            rayDirection = transform.TransformDirection(Vector3.right);
        }

        Debug.DrawRay(transform.position, rayDirection * 20, Color.green, Time.deltaTime, false);

        if (Physics.Raycast(transform.position, rayDirection, out hit, 32.0f))
        {
            var distance = Vector3.Distance(hit.point, transform.position);
            switch (hit.collider.name)
            {
                case "Road-Guide-Right":
                    distanceToMarker = distance;
                    break;
                case "Road-Guide-Left":
                    distanceToMarker = distance;
                    break;
            }
        } else
        {
            distanceToMarker = 100.0f;
        }
    }
}

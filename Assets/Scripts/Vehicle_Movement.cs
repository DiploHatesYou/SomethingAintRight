using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Vehicle_Movement : MonoBehaviour
{
	public float speed;
	public int lane;

	int roadId;

	Vector3 vehiclePosition;
	Vector3 vehicleDirection;
	Vector3 roadDirection;

	int laneCount;
	float laneWidth;

	bool laneChange;
	float laneChangeTimer;

	private List<GameObject> Wheels = new List<GameObject>();
	private GameObject[] Roads;

	float circumference;


	GameObject currentRoad;
	GameObject laneOneStart;


	void Start()
    {

		speed = UnityEngine.Random.Range(1.0f, 5.0f);
		laneWidth = 1.1f;

		laneChange = false;

		//	Randomly Assigning a ROAD to This Vehicle
		Roads = GameObject.FindGameObjectsWithTag("Road");
		int roadIDX = UnityEngine.Random.Range(0, Roads.Length);
		currentRoad = Roads[roadIDX];
		int roadID = currentRoad.GetComponent<Road_Settings>().id;
		if (currentRoad == null)
		{
			Debug.Log("//////////////////////////////\n/////  NO CURRENT ROAD  //////\n//////////////////////////////");
		}

		//	Randomly Assigning a LANE to This Vehicle
		laneCount = currentRoad.GetComponent<Road_Settings>().getLaneCount();
		lane = UnityEngine.Random.Range(1, laneCount + 1);


		foreach (Transform child in GameObject.Find("Wheels").transform)
		{
			if (child.tag == "Wheel")
				Wheels.Add(child.gameObject);
		}

		float wheelSize = Wheels[0].GetComponent<Renderer>().bounds.size.x;
		circumference = wheelSize * Mathf.PI;


		//	Getting the Road Direction
		roadDirection = (currentRoad.GetComponent<Road_Settings>().getRoadDirection());

		//	Setting the Vehicle Rotation to the Road Direction
		this.gameObject.transform.eulerAngles = roadDirection;

		//	Finding the Beginning of the Road
		laneOneStart = currentRoad.transform.Find("Lane_01/Start").gameObject;

		//	Just to make it easier to read
		float rd = roadDirection.y;

		//	Setting the Vehicle Position according to the Beginning of the Road
		vehiclePosition = new Vector3 (
			laneOneStart.transform.position.x - 2 * (float)Math.Cos(rd * Math.PI / 180) - (lane - 1) * laneWidth * (float)Math.Sin(rd * Math.PI / 180),
			laneOneStart.transform.position.y - 1,
			laneOneStart.transform.position.z + 2 * (float)Math.Sin(rd * Math.PI / 180) - (lane - 1) * laneWidth * (float)Math.Cos(rd * Math.PI / 180)
		);

		vehicleDirection = new Vector3(	-speed * (float)Math.Cos(roadDirection.y*Math.PI/180), 0, speed * (float)Math.Sin(roadDirection.y * Math.PI / 180));

		transform.position = vehiclePosition;

		Debug.Log("ROAD:  " + roadID);
		Debug.Log("LANE:  " + lane);
	}

	void Update()
    {
		//	Lane Change
		float laneChangeDuration = 1/speed;
		if (laneChange == true)
		{
			laneChangeTimer += Time.deltaTime;
			if(laneChangeTimer > laneChangeDuration)
			{
				laneChange = false;
				vehicleDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y), 0, speed * (float)Math.Sin(roadDirection.y));
			}
		}

		//	Wheel Rotation
		float distance = speed * Time.deltaTime;
		float wheelRotationAngle = distance * 360 / circumference;
		foreach(GameObject wheel in Wheels)
		{
			wheel.transform.Rotate(new Vector3(0, 1, 0), wheelRotationAngle);
		}

		//	Vehicle Movement
		vehiclePosition += vehicleDirection * Time.deltaTime;
		transform.position = vehiclePosition;
	}


	public void changeLane(char dir)
	{
		if (laneChange == false)
		{
			if ((dir == 'R' || dir == 'r') && lane > 1)
			{
				lane--;
				laneChange = true;
				laneChangeTimer = 0.0f;
				//vehicleDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y), 0, -1.1f * speed * (float)Math.Sin(roadDirection.y));
				//vehicleDirection = new Vector3(speed, 0, -1.1f * speed);
			}
			if ((dir == 'L' || dir == 'l') && lane < laneCount)
			{
				lane++;
				laneChange = true;
				laneChangeTimer = 0.0f;
				//vehicleDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y), 0, 1.1f * speed * (float)Math.Sin(roadDirection.y));
				//vehicleDirection = new Vector3(speed, 0, 1.1f * speed);
			}
		}
	}


}

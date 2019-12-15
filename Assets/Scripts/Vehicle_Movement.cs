using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Vehicle_Movement : MonoBehaviour
{
	bool visualDebug = false;
	bool consoleDebug = true;

	int vehicleID;

	float iniSpeed, speed;
	int lane;

	int roadId;

	Vector3 vehicleStartPosition;
	Vector3 vehiclePosition;
	Vector3 vehicleDirection;
	Vector3 roadDirection;
	Vector3 frontRayDirection;


	int laneCount;
	float laneWidth;
	float roadLength;

	bool laneEndTriggered;
	bool atRedLight;

	bool laneChange;
	float minTimeInLane;        //	Minimum Time Needs to be Spent in Lane
	float timeSpentInLane;      //	Time spent in Current Lane

	float laneChangeSpeed;      //	(Division) Bigger Number is Slower Speed
	float laneChangeTimer;
	float sDLC;                 //	Speed During the Lane Change

	private List<GameObject> Wheels = new List<GameObject>();
	private List<float> wheelCircumference = new List<float>();
	private GameObject[] Roads;

	float operationHeight;
	float drivingCircumference;

	GameObject city;
	GameObject currentRoad;
	GameObject laneOneStart;

	GameObject frontBumper;
	GameObject rearBumper;

	GameObject frontSensor;
	GameObject backSensor;
	GameObject leftSensor;
	GameObject rightSensor;
	GameObject backSensorL;
	GameObject backSensorR;

	GameObject emitterFL;
	GameObject emitterFF;
	GameObject emitterFR;
	GameObject emitterRR;
	GameObject emitterBR;
	GameObject emitterBB;
	GameObject emitterBL;
	GameObject emitterLL;

	GameObject Intersection;



	LayerMask VehicleSensorsMask;
	LayerMask RedLightSensorMask;

	int layerMask;

	string dt;  //	Debug Text Variable to use through out the script



	//	■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	void Start()
	{
		//layerMask = 1 << LayerMask.NameToLayer("VehicleSensorsMask");
		//layerMask = ~layerMask;

		atRedLight = false;
		Intersection = null;

		//	Let's Take Care of the Object Findings  ------------------------------------------
		city = GameObject.Find("City");
		//frontBumper = this.gameObject.transform.Find("Bumpers/Front_Bumper").gameObject;
		//rearBumper  = this.gameObject.transform.Find("Bumpers/Rear_Bumper").gameObject;

		frontSensor = this.gameObject.transform.Find("Hit Sensors/Front_Sensor").gameObject;
		backSensor = this.gameObject.transform.Find("Hit Sensors/Back_Sensor").gameObject;
		leftSensor = this.gameObject.transform.Find("Hit Sensors/Left_Sensor").gameObject;
		rightSensor = this.gameObject.transform.Find("Hit Sensors/Right_Sensor").gameObject;
		backSensorL = this.gameObject.transform.Find("Hit Sensors/Back_Sensor_Left").gameObject;
		backSensorR = this.gameObject.transform.Find("Hit Sensors/Back_Sensor_Right").gameObject;

		emitterFL = this.gameObject.transform.Find("Emitters/FL").gameObject;
		emitterFF = this.gameObject.transform.Find("Emitters/FF").gameObject;
		emitterFR = this.gameObject.transform.Find("Emitters/FR").gameObject;
		emitterRR = this.gameObject.transform.Find("Emitters/RR").gameObject;
		emitterBR = this.gameObject.transform.Find("Emitters/BR").gameObject;
		emitterBB = this.gameObject.transform.Find("Emitters/BB").gameObject;
		emitterBL = this.gameObject.transform.Find("Emitters/BL").gameObject;
		emitterLL = this.gameObject.transform.Find("Emitters/LL").gameObject;

		VehicleSensorsMask = LayerMask.GetMask("VehicleSensors");
		RedLightSensorMask = LayerMask.GetMask("RedLightSensor");

		//	----------------------------------------------------------------------------------


		//	Vehicle ID Assingment  -----------------------------------------------------------
		vehicleID = city.GetComponent<Setup_Traffic>().getVehicheID();
		city.GetComponent<Setup_Traffic>().setVehicleID(vehicleID + 1);
		transform.Find("Vehicle_ID").GetComponent<TextMeshPro>().text = vehicleID.ToString();
		//	----------------------------------------------------------------------------------



		//	Speed Assignment  ----------------------------------------------------------------
		iniSpeed = UnityEngine.Random.Range(2.0f, 6.0f);
		//iniSpeed = 10.0f;                                                    //**************************
		//		if (vehicleID == 1) { iniSpeed = 5; } else { iniSpeed = 2; }
		speed = iniSpeed;
		//			speed = 10;
		//	----------------------------------------------------------------------------------


		//	Road Assignment  -----------------------------------------------------------------
		//	Randomly Assigning a ROAD to This Vehicle
		Roads = GameObject.FindGameObjectsWithTag("Road");
		int roadIDX = UnityEngine.Random.Range(0, Roads.Length);
		//		int roadIDX = 1;
		currentRoad = Roads[roadIDX];
		int roadID = currentRoad.GetComponent<Road_Settings>().getRoadID();
		if (currentRoad == null)
		{
			Debug.Log("///////////////////////\n" +
					  "//  NO CURRENT ROAD  //\n" +
					  "///////////////////////");
		}

		//	Getting Lane Width & Road Length
		laneWidth = currentRoad.GetComponent<Road_Settings>().getLaneWidth();
		roadLength = currentRoad.GetComponent<Road_Settings>().getRoadLength();
		//	----------------------------------------------------------------------------------


		//	Randomly Assigning a LANE to This Vehicle  ---------------------------------------
		laneCount = currentRoad.GetComponent<Road_Settings>().getLaneCount();
		lane = UnityEngine.Random.Range(1, laneCount + 1);
		//		if (vehicleID == 0) { lane = 4; } else { lane = 1; }	//**************************
		//		lane = 2;
		//	----------------------------------------------------------------------------------


		//	Creating the Wheel List and Circumference for each wheel  ------------------------
		foreach (Transform child in transform.Find("Wheels"))
		{
			//Debug.Log(child.name);
			if (child.tag == "Wheel")
			{
				Wheels.Add(child.gameObject);
				wheelCircumference.Add(child.GetComponent<Renderer>().bounds.size.x * Mathf.PI);
			}
			if (child.tag == "DrivingWheel")
			{
				Wheels.Add(child.gameObject);
				wheelCircumference.Add(child.GetComponent<Renderer>().bounds.size.x * Mathf.PI);
				drivingCircumference = child.GetComponent<Renderer>().bounds.size.x * Mathf.PI;
			}
		}
		//Debug.Log("Wheel Count: " + Wheels.Count);
		//	----------------------------------------------------------------------------------


		//	Getting the Road Direction & Calculating Front Ray Direction
		//	----------------------------------------------------------------------------------
		roadDirection = (currentRoad.GetComponent<Road_Settings>().getRoadDirection());
		frontRayDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y * Math.PI / 180), 0, speed * (float)Math.Sin(roadDirection.y * Math.PI / 180));
		//	----------------------------------------------------------------------------------


		//	Setting the Vehicle Rotation to the Road Direction
		//	----------------------------------------------------------------------------------
		this.gameObject.transform.eulerAngles = roadDirection;
		//	----------------------------------------------------------------------------------


		//	Finding the Beginning of the Road
		//	----------------------------------------------------------------------------------
		laneOneStart = currentRoad.transform.Find("Lane_01/Lane_Start").gameObject;
		//	----------------------------------------------------------------------------------


		//	Setting the Vehicle Position according to the Beginning of the Road
		//	----------------------------------------------------------------------------------
		operationHeight = this.gameObject.GetComponent<Vehicle_Settings>().getOperationHeight();

		int tryCount = 0;
		bool isVP_OK = false;
		while (isVP_OK == false && tryCount < 10)
		{
			//float beginning = UnityEngine.Random.Range(2.0f, 300.0f);
			float beginning = UnityEngine.Random.Range(2.0f, roadLength - 5.0f);
			//		if (vehicleID == 1) { beginning = 5; } else { beginning = 30; }
			//		beginning = 10.0f;												  //**************************

			float rd = roadDirection.y;             //	Just to make it easier to read
			vehicleStartPosition = new Vector3(
				laneOneStart.transform.position.x - beginning * (float)Math.Cos(rd * Math.PI / 180) - (lane - 1) * laneWidth * (float)Math.Sin(rd * Math.PI / 180),
				laneOneStart.transform.position.y + operationHeight,
				laneOneStart.transform.position.z + beginning * (float)Math.Sin(rd * Math.PI / 180) - (lane - 1) * laneWidth * (float)Math.Cos(rd * Math.PI / 180)
			);

			isVP_OK = city.GetComponent<Setup_Traffic>().isVehiclePositionOK(vehicleID, roadID, lane, speed, vehicleStartPosition);

			if (isVP_OK == false)
			{
				Debug.Log(Time.time + "    " + "vID: " + vehicleID + "   NOT OK   /  " + tryCount);
			}
			tryCount++;
		}

		vehiclePosition = vehicleStartPosition;
		transform.position = vehiclePosition;
		//	----------------------------------------------------------------------------------


		//	Setting the Vehicle Direction same as Raod Direction
		//	----------------------------------------------------------------------------------
		vehicleDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y * Math.PI / 180), 0, speed * (float)Math.Sin(roadDirection.y * Math.PI / 180));
		//	----------------------------------------------------------------------------------


		laneChangeSpeed = 4.0f;
		laneChange = false;
		laneEndTriggered = false;

		minTimeInLane = UnityEngine.Random.Range(1.5f, 4.0f);
		timeSpentInLane = 0.0f;

		//		GameObject Vehicle = (GameObject)this.gameObject;


		Debug.Log("   ► ► ►    VEHICLE ID: " + vehicleID + "\tROAD ID:  " + roadID + "\tLANE: " + lane + "\tSPEED: " + speed);
		//Debug.Log("LANE:  " + lane);
		//Debug.Log("Speed: " + speed);
	}



	//	■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	void Update()
	{
		//Debug.Log("   ► ► ►    VEHICLE ID: " + vehicleID + "\tSPEED: " + speed);
		//Debug.Log("Lane Width: " + laneWidth);

		timeSpentInLane += Time.deltaTime;
		float distance;


		//	Lane Change  ---------------------------------------------------------------------
		float laneChangeDuration = laneWidth * laneChangeSpeed / sDLC;
		if (laneChange == true)
		{
			laneChangeTimer += Time.deltaTime;
			if (laneChangeTimer > laneChangeDuration)
			{
				laneChange = false;
				//	vehicleDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y), 0, speed * (float)Math.Sin(roadDirection.y));
				vehicleDirection = new Vector3(-sDLC * (float)Math.Cos(roadDirection.y * Math.PI / 180), 0, sDLC * (float)Math.Sin(roadDirection.y * Math.PI / 180));
				timeSpentInLane = 0.0f;
				backSensorR.SetActive(false);
				backSensorL.SetActive(false);
			}
			distance = sDLC * Time.deltaTime;
		}
		else
		{
			vehicleDirection = new Vector3(-speed * (float)Math.Cos(roadDirection.y * Math.PI / 180), 0, speed * (float)Math.Sin(roadDirection.y * Math.PI / 180));
			distance = speed * Time.deltaTime;
		}
		//	----------------------------------------------------------------------------------

		

		//	Wheel Rotation  ------------------------------------------------------------------
		float wheelRotationAngle = distance * 360 / drivingCircumference;
		for (int i = 0; i < Wheels.Count; i++)
		{
			Wheels[i].transform.Rotate(new Vector3(0, 1, 0), wheelRotationAngle * (drivingCircumference / wheelCircumference[i]));
		}
		//	----------------------------------------------------------------------------------



		//	Vehicle Movement  ----------------------------------------------------------------
		vehiclePosition += vehicleDirection * Time.deltaTime;
		transform.position = vehiclePosition;
		//	----------------------------------------------------------------------------------




		//check_front();
		//check_side('L');
		//check_side('R');
	}

	private void LateUpdate()
	{
		check_front();
		//check_side('L');
		//check_side('R');
	}

	public void changeLane(char dir)
	{
		float sin = (float)Math.Sin(roadDirection.y * Math.PI / 180);
		float cos = (float)Math.Cos(roadDirection.y * Math.PI / 180);

		// Debug.Log("LANE CHANGE:   Lane: " + lane + "   /   Lane Count: " + laneCount + "   /   Lane Width: " + laneWidth + "   /   Lane Angle: " + roadDirection.y + "   /   COS: " + cos + "   /   SIN: " + sin);

		if (laneChange == false)
		{
			if ((dir == 'R' || dir == 'r') && lane > 1)
			{
				//Debug.Log("LANE CHANGE - RIGHT");
				lane--;
				laneChange = true;
				laneChangeTimer = 0.0f;
				backSensorR.SetActive(true);
				if (visualDebug) { backSensorR.GetComponent<Renderer>().enabled = true; } else { backSensorR.GetComponent<Renderer>().enabled = false; }
				if (speed < 2.0f) { speed = 2.0f; }
				sDLC = speed;

				////	With Out Lane Change Speed
				//vehicleDirection = new Vector3(
				//	speed * laneWidth * sin - (speed * cos),
				//	0,
				//	speed * laneWidth * cos + (speed * sin)
				//);

				////	Long Hand - With Lane Change Speed
				//vehicleDirection = new Vector3(
				//	((speed * laneWidth * sin) / laneChangeSpeed - (speed * laneWidth * cos)) / laneWidth,
				//	0,
				//	((speed * laneWidth * cos) / laneChangeSpeed + (speed * laneWidth * sin)) / laneWidth
				//);

				//	Simplified - With Lane Change Speed
				vehicleDirection = new Vector3(
					((speed * sin) / laneChangeSpeed - (speed * cos)),
					0,
					((speed * cos) / laneChangeSpeed + (speed * sin))
				);
			}

			if ((dir == 'L' || dir == 'l') && lane < laneCount)
			{
				//Debug.Log("LANE CHANGE - LEFT");
				lane++;
				laneChange = true;
				laneChangeTimer = 0.0f;
				backSensorL.SetActive(true);
				if (visualDebug) { backSensorL.GetComponent<Renderer>().enabled = true; } else { backSensorL.GetComponent<Renderer>().enabled = false; }
				if (speed < 1.0f) { speed = 1.0f; }
				sDLC = speed;

				////	With Out Lane Change Speed
				//vehicleDirection = new Vector3(
				//	-speed * laneWidth * sin - (speed * cos),
				//	0,
				//	-speed * laneWidth * cos + (speed * sin)
				//);

				////	Long Hand - With Lane Change Speed
				//vehicleDirection = new Vector3(
				//	((-speed * laneWidth * sin) / laneChangeSpeed - (speed * laneWidth * cos)) / laneWidth,
				//	0,
				//	((-speed * laneWidth * cos) / laneChangeSpeed + (speed * laneWidth * sin)) / laneWidth
				//);

				//	Simplified - With Lane Change Speed
				vehicleDirection = new Vector3(
					((-speed * sin) / laneChangeSpeed - (speed * cos)),
					0,
					((-speed * cos) / laneChangeSpeed + (speed * sin))
				);
			}
		}
	}


	void check_front()
	{
		bool hitVehicle = false;

		float rayLength = speed / 3;
		if (rayLength < 1)
		{
			rayLength = 1;
		}

		if (visualDebug) { Debug.DrawRay(emitterFF.transform.position, frontRayDirection * rayLength); }

		RaycastHit hit;
		Ray ray_CheckFront = new Ray(emitterFF.transform.position, frontRayDirection);


		if (Physics.Raycast(ray_CheckFront, out hit, rayLength, RedLightSensorMask))
		{
			if (hit.collider.CompareTag("Red_Light_Sensor"))
			{
				//	WE ARE AT A RED LIGHT
				Intersection = hit.collider.transform.root.gameObject;
				//Debug.Log("AT THE RED LIGHT:   " + Intersection.name + "   /   " + this.gameObject.name);
				Intersection.GetComponent<TrafficControl>().addToListOfVehiclesWaitingAtRedLight(this.gameObject);
				atRedLight = true;
			}
		}

		if (Physics.Raycast(ray_CheckFront, out hit, rayLength, VehicleSensorsMask) && !laneChange && !atRedLight)
		{
			//Debug.Log("   HIT !!!   " + hit.collider.name);
			if
				(
				hit.collider.CompareTag("Vehicle_Sensor_Back") ||
				hit.collider.CompareTag("Vehicle_Sensor_Front") ||
				hit.collider.CompareTag("Vehicle_Sensor_Left") ||
				hit.collider.CompareTag("Vehicle_Sensor_Right")
				)
			{
				//	WE ARE CLOSE TO A VEHICLE
				hitVehicle = true;

				////////////////////////////////////////////////////////
				///// CHECK IF IT HIT A RED LIGHT VEHICLE

				//Debug.Log("HIT THE CAR INFRONT : " + hit.collider.gameObject.name);
				GameObject vehicleHit = hit.collider.transform.parent.parent.gameObject;

				//	Let's Check if the Vehiche We are Close to is At The Red Light
				if (vehicleHit.GetComponent<Vehicle_Movement>().getAtRedLight())
				{
					//	It is at Red
					//Debug.Log(Time.time + "\t" + hit.collider.name + "\t/\t " + vehicleHit.name);
					//Debug.Log("HIT A RED LIGHT VEHICLE: " + vehicleHit.GetComponent<Vehicle_Movement>().getVehicleID());

					//	Let's get which Intersection it is waiting currently
					GameObject intersection = vehicleHit.GetComponent<Vehicle_Movement>().getIntersection();
					//Debug.Log(Time.time + "   Intersection Name: " + intersection.name);

					//	Add THIS Vehicle to    Waiting At Red   LIST
					if (Intersection != null)
					{
						intersection.GetComponent<TrafficControl>().addToListOfVehiclesWaitingAtRedLight(this.gameObject);
						atRedLight = true;
					}
				}



				//Debug.Log("Vehicle  " + vehicleID + "  is too close to Vehicle  " + hit.collider.transform.parent.parent.GetComponent<Vehicle_Movement>().getVehicleID());
				dt = "Reducing Speed:   " + speed.ToString() + "   -->   ";
				speed -= speed / 20.0f;
				dt = dt + speed.ToString();

				//	Let's See if we can change a lane to go faster
				//Debug.Log("   ► ► ►    VEHICLE ID: " + vehicleID + "\tSPEED: " + speed + " / " + iniSpeed + " \t\t " + timeSpentInLane + " / " + minTimeInLane);
				if (speed < iniSpeed && timeSpentInLane > minTimeInLane && !atRedLight)
				{
					//Debug.Log("GOTTA CHANGE LANES !!!" + "   " + lane + "  /  " + laneCount);
					if (lane == 1)
					{
						//	Has to be Left
						if (check_side('L')) { changeLane('L'); }
					}
					else if (lane == laneCount)
					{
						//	Has to be Right
						if (check_side('R')) { changeLane('R'); }
					}
					else
					{
						int dir = UnityEngine.Random.Range(0, 2);
						if (dir == 0)
						{
							if (check_side('L')) { changeLane('L'); }
						}
						else if (dir == 1)
						{
							if (check_side('R')) { changeLane('R'); }
						}
					}
				}
			}
		}


		if (atRedLight)
		{
			speed -= speed / 10.0f;
			if (speed < 0) { speed = 0; }
		}

		if (!hitVehicle && !atRedLight)
		{
			//	We did NOT hit a Vehicle
			if (speed < iniSpeed && !laneEndTriggered)
			{
				//speed += iniSpeed / 100.0f;
				if (speed < 1.0f)
				{
					speed += .1f;
				} else
				{
					speed += speed / 100.0f;
				}
			}
		}
	}



	bool check_side(char dir)
	{
		float rva = 60;                             //	Max Ray Angle in one Direction ( 45 means 45 and -45)
		int numberOfRays = 7;                       //	How that Ray Angle will be divided (Ray Density),  *** Has to be odd number ***

		float rayLength = laneWidth * 1.1f;

		Vector3[] rayDirection = new Vector3[numberOfRays];
		float[] rayLengthAngle = new float[numberOfRays];


		RaycastHit hit;

		if ((dir == 'L' || dir == 'l'))
		{
			//Debug.Log("Trying to change LANE: [L]");
			for (int i = 0; i < numberOfRays; i++)
			{
				float angle = i * rva / ((int)numberOfRays / 2) - rva;
				rayDirection[i] = new Vector3((float)Math.Cos((roadDirection.y + 90 + angle) * Math.PI / 180), 0, -(float)Math.Sin((roadDirection.y + 90 + angle) * Math.PI / 180));
				rayLengthAngle[i] = rayLength / (float)Math.Cos(angle * Math.PI / 180);
				if (visualDebug) { Debug.DrawRay(emitterLL.transform.position, rayDirection[i] * rayLengthAngle[i]); }

				Ray ray_CheckLeft = new Ray(emitterLL.transform.position, rayDirection[i]);
				if (Physics.Raycast(ray_CheckLeft, out hit, rayLengthAngle[i], VehicleSensorsMask))
				{
					//Debug.Log("Vehicle  " + vehicleID + "    ->>    " + hit.collider.name + "    ->>    " + hit.collider.transform.root.GetComponent<Vehicle_Movement>().getVehicleID());
					//Debug.Log("HIT - L Lane Change: " + hit.collider.name);
					return false;
				}
			}
		}


		if ((dir == 'R' || dir == 'r'))
		{
			//Debug.Log("Trying to change LANE: [R]");
			for (int i = 0; i < numberOfRays; i++)
			{
				float angle = i * rva / ((int)numberOfRays / 2) - rva;
				//rayDirection[i] = new Vector3((float)Math.Cos((roadDirection.y - 90 + angle) * Math.PI / 180), 0, -(float)Math.Sin((roadDirection.y + 90 - angle) * Math.PI / 180));
				rayDirection[i] = new Vector3((float)Math.Cos((roadDirection.y + 90 + angle) * Math.PI / 180), 0, -(float)Math.Sin((roadDirection.y - 90 - angle) * Math.PI / 180));
				rayLengthAngle[i] = rayLength / (float)Math.Cos(angle * Math.PI / 180);
				if (visualDebug) { Debug.DrawRay(emitterRR.transform.position, rayDirection[i] * rayLengthAngle[i]); }
				Ray ray_CheckRight = new Ray(emitterRR.transform.position, rayDirection[i]);
				if (Physics.Raycast(ray_CheckRight, out hit, rayLengthAngle[i], VehicleSensorsMask))
				{
					//Debug.Log("Vehicle  " + vehicleID + "    ->>    " + hit.collider.name + "    ->>    " + hit.collider.transform.root.GetComponent<Vehicle_Movement>().getVehicleID());
					//Debug.Log("HIT - R Lane Change: " + hit.collider.name);
					return false;
				}
			}
		}
		return true;
	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Lane_End"))
		{
			//Debug.Log("Vehicle Lane End: " + vehicleID);
			laneEndTriggered = true;
			StartCoroutine(WaitCoroutine(4.0f));
		}
		if (other.gameObject.CompareTag("Red_Light_Sensor"))
		{
			//	Vehicle Passed te Red Light and Entered the Intersection
			//	Needs to clear the intersection
			Debug.Log("    ► ► ►  VEHICLE ENTERED THE INTERSECTION AT RED");
			atRedLight = false;
		}
	}

	private void OnCollisionEnter(Collision other)
	{

	}


	IEnumerator WaitCoroutine(float n)
	{
		yield return new WaitForSeconds(n);

		laneEndTriggered = false;
		float rd = roadDirection.y;
		vehicleStartPosition = new Vector3(
			laneOneStart.transform.position.x - 2 * (float)Math.Cos(rd * Math.PI / 180) - (lane - 1) * laneWidth * (float)Math.Sin(rd * Math.PI / 180),
			laneOneStart.transform.position.y + operationHeight,
			laneOneStart.transform.position.z + 2 * (float)Math.Sin(rd * Math.PI / 180) - (lane - 1) * laneWidth * (float)Math.Cos(rd * Math.PI / 180)
		);

		vehiclePosition = vehicleStartPosition;
	}

	public void setAtRedLight(bool status)
	{
		atRedLight = status;
	}

	public bool getAtRedLight()
	{
		return atRedLight;
	}

	public GameObject getIntersection()
	{
		return Intersection;
	}

	public Vector3 getVehicleDirection()
	{
		return vehicleDirection;
	}

	public float getVehicleSpeed()
	{
		return speed;
	}

	public int getVehicleID()
	{
		return vehicleID;
	}
}

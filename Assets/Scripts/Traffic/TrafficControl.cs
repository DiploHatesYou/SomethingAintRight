using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficControl : MonoBehaviour
{
	bool visualDebug = true;

	char currentLight;

	public float GreenLightDuration;
	public float YellowLightDuration;
	public float BothDirRedDuration;


	float gLightDuration;
	float yLightDuration;
	float rLightDuration;
	float clear_Duration;

	float gLightTimeElapsed;
	float yLightTimeElapsed;
	float rLightTimeElapsed;
	float clear_TimeElapsed;

	int trafficFlowDir;
	int otherDir;


	private GameObject[] FlowDir = new GameObject[2];
	private List<GameObject>[] TrafficLights = new List<GameObject>[2];
	private List<GameObject>[] Red_Light_Sensors = new List<GameObject>[2];
	private List<GameObject> vehiclesWaitingAtRedLight = new List<GameObject>();



	private List<GameObject>[] r = new List<GameObject>[2];
	private List<GameObject>[] y = new List<GameObject>[2];
	private List<GameObject>[] g = new List<GameObject>[2];

	void Start()
    {

		///////////////////////////////////////
		///////////////////////////////////////
		//gLightDuration = 5.0f;
		//yLightDuration = 2.0f;
		////rLightDuration = 5.0f;
		//clear_Duration = 3.0f;
		gLightDuration = 5.0f;
		yLightDuration = 2.0f;
		clear_Duration = 3.0f;
		if (GreenLightDuration != 0)	{ gLightDuration = GreenLightDuration; }
		if (YellowLightDuration != 0)	{ yLightDuration = YellowLightDuration; }
		if (BothDirRedDuration != 0)	{ clear_Duration = BothDirRedDuration; }
			
		///////////////////////////////////////
		trafficFlowDir = 0;
		otherDir = (trafficFlowDir + 1) % 2;
		///////////////////////////////////////
		///////////////////////////////////////

		currentLight = 'G';
		resetElapsedTimes();
		//-------------------------------------



		//	■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ 
		//	LET THE FUN BEGIN   ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■
		//	■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ 

		FlowDir[0] = this.gameObject.transform.Find("Direction0").gameObject;
		FlowDir[1] = this.gameObject.transform.Find("Direction1").gameObject;

		//	List Initilizations
		for (int i=0; i<2; i++) {
			Red_Light_Sensors[i] = new List<GameObject>();
			TrafficLights[i] = new List<GameObject>();
			r[i] = new List<GameObject>();
			y[i] = new List<GameObject>();
			g[i] = new List<GameObject>();
		}


		//	Find Object For Both Directions
		for (int i=0; i<2; i++)		
		{
			//	Find all Traffic Light in This Direction
			foreach (Transform child in transform.Find(FlowDir[i].name))
			{
				if (child.tag == "Traffic_Light")
				{
					TrafficLights[i].Add(child.gameObject);
				}
			}
			//Debug.Log("Direction: " + i + "   /   Traffic Lights: " + TrafficLights[i].Count);

			//	Find all Red_Light_Sensors in This Direction
			foreach (Transform child in transform.Find(FlowDir[i].name))
			{
				//Debug.Log("Direction: " + i + "   /   " + child.tag);
				if (child.tag == "Lane")
				{
					//Red_Light_Sensors[i].Add(child.gameObject.transform.Find("Red_Light_Indicator").gameObject);
					GameObject redLightSensor = child.gameObject.transform.Find("Red_Light_Indicator").gameObject;
					Red_Light_Sensors[i].Add(redLightSensor);
					redLightSensor.GetComponent<Renderer>().enabled = visualDebug;
				}
			}
			//Debug.Log("Direction: " + i + "   /   Red Light Sensors: " + Red_Light_Sensors[i].Count);
		}

		for(int i=0; i<2; i++)
		{
			foreach (GameObject trafficLight in TrafficLights[i])
			{
				//Debug.Log("Direction: " + i + "   " + trafficLight.name);
				r[i].Add(trafficLight.transform.Find("Red").gameObject);
				y[i].Add(trafficLight.transform.Find("Yellow").gameObject);
				g[i].Add(trafficLight.transform.Find("Green").gameObject);
			}
		}

		setTrafficFlowTo(trafficFlowDir);
	}



	void Update()
    {
		//Debug.Log("CURRENT LIGHT: " + currentLight);


		if(currentLight == 'g' || currentLight == 'G')
		{
			gLightTimeElapsed += Time.deltaTime;
			if (gLightTimeElapsed >= gLightDuration)
			{
				//	Turn Off Green, Turn On Yellow
				currentLight = 'Y';
				resetElapsedTimes();

				setTrafficLights(trafficFlowDir, 'y');
			}
		}
		else if (currentLight == 'y' || currentLight == 'Y')
		{
			yLightTimeElapsed += Time.deltaTime;
			if (yLightTimeElapsed >= yLightDuration)
			{
				//	Turn Off Yellow, Turn On Red
				//currentLight = 'G';
				//resetElapsedTimes();

				//setTrafficFlowTo(otherDir);

				currentLight = 'C';
				resetElapsedTimes();

				setTrafficLights(trafficFlowDir, 'r');
				setRedLightIndicators(trafficFlowDir, true);

			}
		}
		//else if (currentLight == 'r' || currentLight == 'R')
		//{
		//	rLightTimeElapsed += Time.deltaTime;
		//	if (rLightTimeElapsed >= rLightDuration)
		//	{
		//		//	Keep The Red ON, Turn On Red on the other Direction
		//		currentLight = 'C';
		//		resetElapsedTimes();

		//		setTrafficLights(otherDir, 'r');
		//		setRedLightIndicators(otherDir, true);
		//	}
		//}
		else if (currentLight == 'c' || currentLight == 'C')
		{
			clear_TimeElapsed += Time.deltaTime;
			if (clear_TimeElapsed >= clear_Duration)
			{
				//	Turn Off Red, Turn On Green
				currentLight = 'G';
				resetElapsedTimes();

				setTrafficFlowTo(otherDir);
			}
		}
	}

	void setTrafficFlowTo(int go)
	{
		int other = (go + 1) % 2;
		//Debug.Log("This Lane: " + go + "   /   Other Lane: " + other);

		trafficFlowDir = go;
		otherDir = other;

		//Debug.Log(Time.time + "    trafficFlowDir: " + trafficFlowDir + "   /   otherDir: " + otherDir);

		setTrafficLights(go, 'g');
		setTrafficLights(other, 'r');

		setRedLightIndicators(go, false);
		setRedLightIndicators(other, true);

		clearListOfVehiclesWaitingAtRedLight();
	}

	void setTrafficLights(int dir, char color)
	{

		if (color == 'r' || color == 'R')
		{
			foreach (GameObject light in r[dir]) { light.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); }
			foreach (GameObject light in y[dir]) { light.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
			foreach (GameObject light in g[dir]) { light.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
		}

		if (color == 'y' || color == 'Y')
		{
			foreach (GameObject light in r[dir]) { light.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
			foreach (GameObject light in y[dir]) { light.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); }
			foreach (GameObject light in g[dir]) { light.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
		}

		if (color == 'g' || color == 'G')
		{
			foreach (GameObject light in r[dir]) { light.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
			foreach (GameObject light in y[dir]) { light.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
			foreach (GameObject light in g[dir]) { light.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); }
		}
	}

	void setRedLightIndicators(int dir, bool status)
	{
		foreach (GameObject rli in Red_Light_Sensors[dir]) { rli.SetActive(status); }
	}

	void resetElapsedTimes()
	{
		gLightTimeElapsed = 0;
		yLightTimeElapsed = 0;
		rLightTimeElapsed = 0;
		clear_TimeElapsed = 0;
	}

	void clearListOfVehiclesWaitingAtRedLight()
	{
		foreach(GameObject vehicle in vehiclesWaitingAtRedLight)
		{
			vehicle.GetComponent<Vehicle_Movement>().setAtRedLight(false);
		}
		vehiclesWaitingAtRedLight.Clear();
		//Debug.Log("Number of Vehicles Waiting In Red Light: " + vehiclesWaitingAtRedLight.Count);
	}

	public void addToListOfVehiclesWaitingAtRedLight(GameObject vehicle)
	{
		//Debug.Log("AT THE LISTADD:   " + vehicle.name);
		if(!vehiclesWaitingAtRedLight.Contains(vehicle))
		{
			vehiclesWaitingAtRedLight.Add(vehicle);
		}
		//Debug.Log("Number of Vehicles Waiting In Red Light: " + vehiclesWaitingAtRedLight.Count);
	}
}

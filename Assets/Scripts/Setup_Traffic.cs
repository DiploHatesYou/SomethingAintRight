using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Setup_Traffic : MonoBehaviour
{
	private class VehicleMetaData
	{
		public int vehicleID;
		public int roadID;
		public int laneNumber;
		public float speed;
		public Vector3 vehicleLocation;

		public VehicleMetaData (int vID, int rID, int lNumber, float s, Vector3 vLocation)
		{
			vehicleID = vID;
			roadID = rID;
			laneNumber = lNumber;
			speed = s;
			vehicleLocation = vLocation;
		}
	}

	private List<VehicleMetaData> trafficInfo = new List<VehicleMetaData>();

	public int trafficDensity;
	public float checkAroundTimer;


	public GameObject myVehicle_1;
	public GameObject myVehicle_2;
	public GameObject myVehicle_3;
	public GameObject myVehicle_4;
	public GameObject myVehicle_5;

	private int vehicleID;

	void Start()
    {
		vehicleID = 0;

		for (int i = 0; i < trafficDensity; i++)
		{
			Instantiate(myVehicle_1);
			//Instantiate(myVehicle_2);
			//Instantiate(myVehicle_3);
			//Instantiate(myVehicle_4);
			//Instantiate(myVehicle_5);
		}
	}

	void Update()
    {
        
    }

	public int getVehicheID()
	{
		return vehicleID;
	}

	public void setVehicleID(int n)
	{
		vehicleID = n;
	}

	public void addVehiclePosition (Vector3 vp)
	{
		
	}

	public bool isVehiclePositionOK(Vector3 vp)
	{
		return true;
	}

	public bool isVehiclePositionOK(int vehicleID, int roadID, int laneNumber, float speed, Vector3 vehicleLocation)
	{
		for (int i = 0; i < trafficInfo.Count; i++)
		{
			if (Math.Abs(Vector3.Distance(vehicleLocation, trafficInfo[i].vehicleLocation)) < 3)
			{
				//	Distance is too Small, Checking for Road and Lane Assignments
				if(trafficInfo[i].roadID == roadID && trafficInfo[i].laneNumber == laneNumber)
				{
					//	The Road and the Lane is the same
					//	We are too close to another vehicle, reject the location
					return false;
				}
			}
		}

		//	We went through the List of Vehicles, suggested location is OK
		//	We wil; add the vehicle to the List
		VehicleMetaData vm = new VehicleMetaData(vehicleID, roadID, laneNumber, speed, vehicleLocation);
		trafficInfo.Add(vm);
		return true;
	}
}

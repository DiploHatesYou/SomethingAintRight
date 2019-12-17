using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Road_Settings : MonoBehaviour
{
	private int roadID;

	private Vector3 roadDirection;
	private Vector3 parentDirection;
	private int laneCount;
	private float laneWidth;
	private float roadLength;

	private List<GameObject> Lanes = new List<GameObject>();

	void Start()
    {
		laneCount = 0;
		foreach (Transform child in this.gameObject.transform)
		{
			if (child.tag == "Lane")
			{
				Lanes.Add(child.gameObject);
				laneCount++;

				foreach(Transform grandChild in child.transform)
				{
					if (grandChild.tag == "Lane_End")
					{
						grandChild.GetComponent<Renderer>().enabled = false;
					}
				}
			}

		}
		//Debug.Log("Lane Count: " + laneCount);

		GameObject parent;

		//	Finding The Parent's Rotation  ---------------------------------------------------
		if (transform.parent != null)
		{
			parent = this.gameObject.transform.parent.gameObject;
			parentDirection = parent.transform.localRotation.eulerAngles;
			//Debug.Log("PARENT :" + parent.name);
			//Debug.Log("PARENT Direction:" + parentDirection.x + "  /  " + parentDirection.y + "  /  " + parentDirection.z);
		}
		else
		{
			parent = null;
			parentDirection = new Vector3(0,0,0);
		}
		//	----------------------------------------------------------------------------------


		//	Store the Current Road Rotation  -------------------------------------------------
		//roadDirection = this.gameObject.transform.localRotation.eulerAngles;

		//	Add Parent's Rotation to the Local Rotation
		roadDirection = this.gameObject.transform.localRotation.eulerAngles + parentDirection;
		//Debug.Log("ROAD Direction:" + roadDirection.x + "  /  " + roadDirection.y + "  /  " + roadDirection.z);
		//	----------------------------------------------------------------------------------


		//	Calculating Lane Width  ----------------------------------------------------------
		//	Turn the ROAD temporarily, so we can Get the LANE Width
		transform.rotation = Quaternion.identity;
		laneWidth = Lanes[0].GetComponent<Renderer>().bounds.size.z;
		roadLength = Lanes[0].GetComponent<Renderer>().bounds.size.x;

		//	Turn the ROAD back to its original direction
		transform.rotation = Quaternion.Euler(new Vector3(roadDirection.x, roadDirection.y, roadDirection.z));
		//	----------------------------------------------------------------------------------
		//Debug.Log("Lane Width: " + laneWidth);
		//Debug.Log("Road Length: " + roadLength);
		//	----------------------------------------------------------------------------------
	}


	void Update()
    {
        
    }


	//	---------------------------------------------------------------------
	//	Getter and Setters
	//	---------------------------------------------------------------------

	public int getLaneCount ()
	{
		return laneCount;
	}

	public float getLaneWidth()
	{
		return laneWidth;
	}

	public float getRoadLength()
	{
		return roadLength;
	}

	public Vector3 getRoadDirection ()
	{
		//return this.gameObject.transform.localRotation.eulerAngles;
		return roadDirection;
	}


	public int getRoadID()
	{
		return roadID;
	}

	public void setRoadID(int n)
	{
		roadID = n;
	}

}

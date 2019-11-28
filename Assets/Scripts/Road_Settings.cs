using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Settings : MonoBehaviour
{
	public int id;

	Vector3 roadDirection;
	int laneCount;

	private List<GameObject> Lanes = new List<GameObject>();
	float laneWidth;

	void Start()
    {
		laneCount = 0;
		foreach (Transform child in this.gameObject.transform)
		{
			if (child.tag == "Lane")
			{
				Lanes.Add(child.gameObject);
				laneCount++;
			}
		}
		//Debug.Log("Lane Count: " + laneCount);

		roadDirection = this.gameObject.transform.localRotation.eulerAngles;

	}

	void Update()
    {
        
    }

	public int getLaneCount ()
	{
		return laneCount;
	}

	public Vector3 getRoadDirection ()
	{
		return this.gameObject.transform.localRotation.eulerAngles;
	}

}

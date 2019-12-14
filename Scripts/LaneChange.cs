using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneChange : MonoBehaviour
{
	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			GetComponent<Vehicle_Movement>().changeLane('L');
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			GetComponent<Vehicle_Movement>().changeLane('R');
		}
	}
}

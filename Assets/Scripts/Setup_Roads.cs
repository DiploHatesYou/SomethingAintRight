using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Setup_Roads : MonoBehaviour
{
	private GameObject[] Roads;

	void Start()
    {
		int road_id = 0;

		Roads = GameObject.FindGameObjectsWithTag("Road");

		foreach(GameObject road in Roads)
		{
			road.GetComponent<Road_Settings>().setRoadID(road_id);
			road.transform.Find("Road_ID").GetComponent<TextMeshPro>().text = road.GetComponent<Road_Settings>().getRoadID().ToString();
			road_id++;
		}
	}

	void Update()
    {
        
    }
}

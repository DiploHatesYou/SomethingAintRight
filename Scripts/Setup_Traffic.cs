using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup_Traffic : MonoBehaviour
{
	public GameObject myVehicle_1;
	public GameObject myVehicle_2;

	void Start()
    {
		for (int i = 0; i < 5; i++)
		{
			Instantiate(myVehicle_1);
			Instantiate(myVehicle_2);
		}
	}

	void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionEntryAtRed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Red_Light_Sensor"))
		{
			//	Vehicle Passed te Red Light and Entered the Intersection
			//	Needs to clear the intersection
			Debug.Log("    ► ► ►  FRONT SENSOR ENTERED THE INTERSECTION");
			this.gameObject.transform.parent.parent.gameObject.GetComponent<Vehicle_Movement>().setAtRedLight(false);
		}

	}
}

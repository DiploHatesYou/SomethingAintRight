using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
	GameObject r;
	GameObject y;
	GameObject g;

	void Start()
    {
		r = this.gameObject.transform.Find("Direction1/TrafficLight/Red").gameObject;
		y = this.gameObject.transform.Find("Direction1/TrafficLight/Yellow").gameObject; ;
		g = this.gameObject.transform.Find("Direction1/TrafficLight/Green").gameObject; ;
	}




	void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("R");
			if (r.GetComponent<Renderer>().material.IsKeywordEnabled("_EMISSION"))
			{
				r.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
			} else
			{
				r.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
			}
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			Debug.Log("Y");
			if (y.GetComponent<Renderer>().material.IsKeywordEnabled("_EMISSION"))
			{
				y.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
			}
			else
			{
				y.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
			}
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			Debug.Log("G");
			if (g.GetComponent<Renderer>().material.IsKeywordEnabled("_EMISSION"))
			{
				g.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
			}
			else
			{
				g.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
			}
		}
	}

	
}

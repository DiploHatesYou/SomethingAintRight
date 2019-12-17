using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource rain;
    public AudioSource traffic;

    // Update is called once per frame
    void Update()
    {
        if (Pause.gameIsPaused == true)
        {
            rain.volume = 0f;
        }
        else
        {
            rain.volume = .1f;
        }

        if (Pause.gameIsPaused == true)
        {
            traffic.volume =  0f;
        }
        else
        {
            traffic.volume = 1f;
        }
    }
}

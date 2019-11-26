using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerDetection : MonoBehaviour
{
    private AICharacterControl _agent;

    private void Start()
    {
        _agent = GetComponent<AICharacterControl>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _agent.target = other.transform;
        }
    }

}

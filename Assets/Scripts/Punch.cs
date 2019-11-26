using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class Punch : MonoBehaviour
{
    float _coolDown = 1;
    float _nextHitTime = 0;
    public float thrust;

    public GameObject hand;
    private NavMeshAgent agent;

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Hit();
        if (agent.GetComponent<Rigidbody>().velocity.z <= .2f && agent.GetComponent<Rigidbody>().velocity.x <= .2f && agent.GetComponent<Rigidbody>().velocity.y <= .2f)
        {
            Debug.Log("Velocity is 0");
            Vector3 enemyPos = agent.transform.position;
            agent.Warp(enemyPos);
            agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
            agent.GetComponent<AICharacterControl>().agent.updateRotation = true;

        }
        else
        {
            return;
        }
    }

    //void Hit()
    //{
    //    if (Time.time >= _nextHitTime && Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 fwd = hand.transform.TransformDirection(Vector3.forward);
    //        Debug.DrawRay(hand.transform.position, fwd);

    //        if (Physics.Raycast(hand.transform.position, fwd, out RaycastHit hit, 2) && hit.collider.CompareTag("Enemy"))
    //        {
    //            Vector3 dir = hit.transform.position - transform.position;
    //            agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
    //            Debug.Log("Hit Enemy");

    //            hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
    //            hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
    //            hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);

    //            //_nextHitTime = Time.time + _coolDown;

    //            if (hit.rigidbody.velocity.z <= .2f && hit.rigidbody.velocity.x <= .2f && hit.rigidbody.velocity.y <= .2f)
    //            {
    //                Debug.Log("Velocity is 0");
    //                Vector3 enemyPos = hit.transform.position;
    //                agent.Warp(enemyPos);
    //                agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
    //                agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
    //                //StartCoroutine("DisableUpdatePosition");
    //            }




    //        }

    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            agent = other.gameObject.GetComponent<NavMeshAgent>();
            Vector3 dir = other.transform.position - transform.position;
            other.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
            other.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
            other.attachedRigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);
            
        }
    }

    IEnumerator DisableUpdatePosition()
    {

        yield return new WaitForSeconds(.25f);

        agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
        agent.GetComponent<AICharacterControl>().agent.updateRotation = true;

    }


}

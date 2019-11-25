using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class Punch : MonoBehaviour
{
    float _coolDown = 2;
    float _nextHitTime = 0;
    public float thrust;

    public GameObject hand;
    private NavMeshAgent agent;

    private RaycastHit _enemy;
    

    void Update()
    {
        Hit();
        //enemyPos = agent.transform.position;
    }

    void Hit()
    {
        if (/*Time.time >= _nextHitTime &&*/ Input.GetMouseButtonDown(0))
        {
            Vector3 fwd = hand.transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(hand.transform.position, fwd);

            if (Physics.Raycast(hand.transform.position, fwd, out RaycastHit hit, 2) && hit.collider.CompareTag("Enemy"))
            {

                _enemy = hit;
                Vector3 dir = hit.transform.position - transform.position;
                agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
                Debug.Log("Hit Enemy");

                hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
                hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);

                //_nextHitTime = Time.time + _coolDown;

                if (Mathf.Approximately(hit.rigidbody.velocity.z, 0) || Mathf.Approximately(hit.rigidbody.velocity.x, 0) || Mathf.Approximately(hit.rigidbody.velocity.y, 0))
                {
                    Debug.Log("Velocity is 0");
                    Vector3 enemyPos = hit.transform.position;
                    //enemyPos = hit.collider.gameObject.GetComponent<Transform>()
                    agent.Warp(enemyPos);
                    StartCoroutine("DisableUpdatePosition");
                    //agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
                }
                

                

            }

        }
    }

    IEnumerator DisableUpdatePosition()
    {

        yield return new WaitForSeconds(.25f);

        agent.GetComponent<AICharacterControl>().agent.updatePosition = true;

    }


}

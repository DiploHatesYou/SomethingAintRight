using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class Punch : MonoBehaviour
{
    float _coolDown = 2f;
    float _nextHitTime = 0;
    float _beenHit = 1f;
    public float thrust;
    private bool punching = false;

    //bool enemyHit = false;

    public GameObject hand;
    private NavMeshAgent agent;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Hit();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            punching = true;
        }
        else
        {
            punching = false;
        }

        if (punching == false)
        {
            anim.SetBool("PunchingRight", false);
        }

        if (punching == true)
        {
            anim.SetBool("PunchingRight", true);
        }   
    }

    void Hit()
    {
        Vector3 fwd = hand.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(hand.transform.position, fwd, out RaycastHit hit, 3))
        {
            Debug.DrawRay(hand.transform.position, fwd);

           if (hit.collider.CompareTag("Enemy") && Input.GetMouseButtonDown(0))
           {
                Vector3 dir = hit.transform.position - transform.position;
                agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
                Debug.Log("Hit Enemy");
                hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
                hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
                hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);

                if (hit.rigidbody.velocity.z <= .2f && hit.rigidbody.velocity.x <= .2f && hit.rigidbody.velocity.y <= .2f)
                {
                    Debug.Log("Velocity is 0");
                    Vector3 enemyPos = hit.transform.position;
                    agent.Warp(enemyPos);
                    agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
                    agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
                }
                else if (Time.time >= _beenHit)
                {
                    agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
                    agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
                }
           }
            
        }
    }
}

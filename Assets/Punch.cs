using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Punch : MonoBehaviour
{
    float _coolDown = 5;
    float _nextHitTime = 0;
    public float thrust;

    public GameObject hand;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Hit();
    }

    void Hit()
    {
        if (Time.time >= _nextHitTime && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 fwd = hand.transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(hand.transform.position, fwd);
            
            if (Physics.Raycast(hand.transform.position, fwd, out hit, 2) && hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit Enemy");
                Vector3 dir = hit.transform.position - transform.position;
                hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
                hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);
                _nextHitTime = Time.time + _coolDown;
            }
            else if (Time.time >= _nextHitTime)
            {
                hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = true;
            }

        }
    }
}

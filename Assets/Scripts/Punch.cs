using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Punch : MonoBehaviour
{
    float _coolDown = 2;
    float _nextHitTime = 0;
    public float thrust;

    public GameObject hand;

    private RaycastHit _enemy;

    void Update()
    {
        Hit();
    }

    void Hit()
    {
        if (Time.time >= _nextHitTime && Input.GetMouseButtonDown(0))
        {
            Vector3 fwd = hand.transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(hand.transform.position, fwd);

            if (Physics.Raycast(hand.transform.position, fwd, out RaycastHit hit, 2) && hit.collider.CompareTag("Enemy"))
            {
                _enemy = hit;
                Vector3 dir = hit.transform.position - transform.position;

                Debug.Log("Hit Enemy");

                hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
                hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);
                _nextHitTime = Time.time + _coolDown;

                StartCoroutine("DisableUpdatePosition");

            }

        }
    }

    IEnumerator DisableUpdatePosition()
    {
        yield return new WaitForSeconds(2f);

        _enemy.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = true;
    }


}

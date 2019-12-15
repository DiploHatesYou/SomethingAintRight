using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Punch : MonoBehaviour
{
    float _coolDown = 1f;
    float _nextHitTime = 0;

    public static float onePunchDamage = .1f;
    public static float doublePunchDamage = .2f;
    public float thrust;

    bool _oneClick = false;
    bool _timerRunning;
    float _timer;
    float _clickDelay = .5f;

    bool _punching = false;
    bool _doublePunch = false;

    public static bool beenHit = false;

    public GameObject rayCaster;

    private NavMeshAgent _agent;
    private Animator _enemyAnim;
    private Animator _anim;

    public GameObject bloodSplatter;
    public Transform bloodSplatterLocation;

    Enemy Enemy;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        Enemy = FindObjectOfType<Enemy>();
    }

    private void FixedUpdate()
    {
        Hit();
    }

    private void Update()
    {
        DoubleClick();

        if (Input.GetMouseButtonDown(0))
        {
            _punching = true;
        }
        else
        {
            _punching = false;
        }

        if (_punching == false)
        {
            _anim.SetBool("PunchingRight", false);
        }

        if (_punching == true)
        {
            _anim.SetBool("PunchingRight", true);
        }

        if (_doublePunch == false)
        {
            _anim.SetBool("DoublePunch", false);
        }
        if (_doublePunch == true)
        {
            _anim.SetBool("DoublePunch", true);
        }
    }

    void Hit()
    {
        Vector3 fwd = rayCaster.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(rayCaster.transform.position, fwd, out RaycastHit hit, 2))
        {
            Debug.DrawRay(rayCaster.transform.position, fwd);

           if (hit.collider.CompareTag("Enemy") && Input.GetMouseButtonDown(0))
           {
                

                if (hit.collider.CompareTag("Enemy") && Time.time > _nextHitTime && _oneClick == false)
                {
                    beenHit = true;
                    Debug.Log("One Punch");
                    //StartCoroutine(HitReaction());
                    _nextHitTime = Time.time + _coolDown;
                    //_enemyAnim = hit.collider.GetComponent<Animator>();
                    //Enemy.TakeDamage(onePunchDamage);
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(onePunchDamage);

                    Vector3 dir = hit.transform.position - transform.position;
                    _agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
                    Debug.Log("Hit Enemy");
                    hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
                    hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
                    hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);

                    if (hit.rigidbody.velocity.z <= .2f && hit.rigidbody.velocity.x <= .2f && hit.rigidbody.velocity.y <= .2f)
                    {
                        Debug.Log("Velocity is 0");
                        Vector3 enemyPos = hit.transform.position;
                        _agent.Warp(enemyPos);
                        _agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
                        _agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
                    }
                    else if (Time.time >= _nextHitTime)
                    {
                        _agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
                        _agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
                    }

                    //_agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
                    //hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
                    //hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
                }
                else
                {
                    beenHit = false;
                }

                //if (hit.collider.CompareTag("Enemy") && Time.time > _nextHitTime && _doublePunch == true)
                //{
                //    StartCoroutine(HitReaction());
                //    _nextHitTime = Time.time + _coolDown;
                //    _enemyAnim = hit.collider.GetComponent<Animator>();
                //    enemyHit = true;
                //}
            }
        }
    }

    //public IEnumerator HitReaction()
    //{
    //    yield return new WaitForSeconds(.1f);
    //    _enemyAnim.SetBool("PunchingRight", false);
    //    _enemyAnim.SetBool("DoublePunch", false);
    //    _enemyAnim.SetBool("BeenHit", true);
    //    var PS = Instantiate(bloodSplatter, new Vector3(bloodSplatterLocation.position.x, bloodSplatterLocation.position.y, bloodSplatterLocation.position.z), Quaternion.identity);
    //    Destroy(PS, 2f);
    //}

    void DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_oneClick)
            {
                Debug.Log("One click");
                _oneClick = true;
                _timer = Time.time;
                _doublePunch = false;
            }
            else
            {
                _oneClick = false;
                _doublePunch = true;
                Debug.Log("Double click");
            }
        }
        if (_oneClick)
        {
            if ((Time.time - _timer) > _clickDelay)
            {
                _oneClick = false;
            }
        }
    }
}



//////////////////ApplyForce////////////////////////////


//Vector3 dir = hit.transform.position - transform.position;
//agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
//Debug.Log("Hit Enemy");
//hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
//hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
//hit.rigidbody.AddForce(dir.normalized * thrust, ForceMode.Impulse);

//if (hit.rigidbody.velocity.z <= .2f && hit.rigidbody.velocity.x <= .2f && hit.rigidbody.velocity.y <= .2f)
//{
//    Debug.Log("Velocity is 0");
//    Vector3 enemyPos = hit.transform.position;
//    agent.Warp(enemyPos);
//    agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
//    agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
//}
//else if (Time.time >= _beenHit)
//{
//    agent.GetComponent<AICharacterControl>().agent.updatePosition = true;
//    agent.GetComponent<AICharacterControl>().agent.updateRotation = true;
//}

//_agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
//hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updatePosition = false;
//hit.collider.gameObject.GetComponent<AICharacterControl>().agent.updateRotation = false;
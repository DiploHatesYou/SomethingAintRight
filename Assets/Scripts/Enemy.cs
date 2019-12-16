using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 1f;
    public int worth = 50;
    public int xpWorth = 25;

    public float damage;

    bool _attack = false;
    bool _doublePunch = false;
    bool _hitReaction = false;
    bool _death = false;

    private Animator _anim;
    private AICharacterControl _agent;
    private PlayerStats PlayerStats;
    private Punch Punch;
    public Collider trigger;

    public GameObject bloodSplatter;
    public Transform bloodSplatterLocation;
    private NavMeshAgent navAgent;
    public GameObject enemyAI;
    private GameObject enemyTarget;
    //public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        enemyTarget = GameObject.Find("EnemyTarget");
        _anim = GetComponent<Animator>();
        _agent = GetComponent<AICharacterControl>();
        PlayerStats = FindObjectOfType<PlayerStats>();
        Punch = FindObjectOfType<Punch>();
        navAgent = GetComponent<NavMeshAgent>();
        _agent.target = enemyTarget.transform;
        //var _enemyAI = Instantiate(enemyAI, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity);
    }

    private void Update()
    {
        Attack();
        DoDamage();

        if (_death == true)
        {
            PlayerStats.xp += xpWorth;
            _death = false;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        _hitReaction = true;

        if (health <= 0)
        {
            Die();
            _death = true;
        }
    }

    void Die()
    {
        
        PlayerStats.money += worth;
        _anim.SetBool("Death", true);
        

        navAgent.isStopped = true;
        _agent.agent.updatePosition = false;
        _agent.agent.updatePosition = false;
        Destroy(enemyAI, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    _attack = true;
        //    _doublePunch = true;
        //    DoDamage();
        //}
        if (_hitReaction == true)
        {
            //_attack = false;
            //_doublePunch = false;
            //_anim.SetBool("BeenHit", true);
            //var PS = Instantiate(bloodSplatter, new Vector3(bloodSplatterLocation.position.x, bloodSplatterLocation.position.y, bloodSplatterLocation.position.z), Quaternion.identity);
            //Destroy(PS, 2f);
            //_hitReaction = false;
            
            StartCoroutine(HitReaction());
        }
        else if (_hitReaction == false && other.CompareTag("Player"))
        {
            _attack = true;
            _doublePunch = true;
            DoDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _attack = false;
            _doublePunch = false;
        }
    }

    void Attack()
    {
        if (_attack == true)
            _anim.SetBool("PunchingRight", true);
        if (_attack == false)
            _anim.SetBool("PunchingRight", false);
        if (_doublePunch == true)
            _anim.SetBool("DoublePunch", true);
        if (_doublePunch == false)
            _anim.SetBool("DoublePunch", false);
    }

    public void DoDamage()
    {
        int rand = Random.Range(0, 200);
        if (_attack == true && Pause.gameIsPaused == false)
        {
            if (rand == 1)
            {
                PlayerStats.TakeDamage(damage);
            }
        }
    }

    public IEnumerator HitReaction()
    {
        trigger.isTrigger = false;
        yield return new WaitForSeconds(.1f);
        
        _anim.SetBool("PunchingRight", false);
        _anim.SetBool("DoublePunch", false);
        _anim.SetBool("BeenHit", true);
        var PS = Instantiate(bloodSplatter, new Vector3(bloodSplatterLocation.position.x, bloodSplatterLocation.position.y, bloodSplatterLocation.position.z), Quaternion.identity);
        Destroy(PS, 2f);
        _hitReaction = false;
        yield return new WaitForSeconds(.3f);
        trigger.isTrigger = true;
        navAgent.Warp(transform.position);
        _agent.agent.updatePosition = true;
        _agent.agent.updatePosition = true;
    }
}

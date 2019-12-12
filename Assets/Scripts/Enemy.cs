using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    public float health = 1f;
    public int worth = 50;

    public float damage;

    bool _attack = false;
    bool _doublePunch = false;

    private Animator _anim;
    private AICharacterControl _agent;
    private PlayerStats PlayerStats;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<AICharacterControl>();
        PlayerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        Attack();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.money += worth;
        _anim.SetBool("Death", true);
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _attack = true;
            _doublePunch = true;
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

    void DoDamage()
    {
        int rand = Random.Range(0, 2000);

        if (rand == 1)
        {
            PlayerStats.TakeDamage(damage);
        }
    }
}

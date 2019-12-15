using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    public float health = 1f;
    public int worth = 50;
    public int xpWorth = 25;

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
        DoDamage();


    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        _anim.SetBool("PunchingRight", false);
        _anim.SetBool("DoublePunch", false);
        _anim.SetBool("BeenHit", true);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.money += worth;
        PlayerStats.xp += xpWorth;
        _anim.SetBool("Death", true);
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        int rand = Random.Range(0, 300);
        if (_attack == true && Pause.gameIsPaused == false)
        {
            if (rand == 1)
            {
                PlayerStats.TakeDamage(damage);
            }
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    public float health = 1f;

    public int worth = 50;

    private Animator _anim;
    private AICharacterControl _agent;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<AICharacterControl>();
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
        _agent.GetComponent<AICharacterControl>().agent.updatePosition = false;
        _agent.GetComponent<AICharacterControl>().agent.updateRotation = false;
        PlayerStats.money += worth;
        _anim.SetBool("Death", true);
        Destroy(gameObject, 10f);
    }
}

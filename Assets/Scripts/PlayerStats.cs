using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 100;

    public static float health;
    public float startHealth = 1f;

    public static int xp;
    public int startXp = 0;

    public static int level = 0;

    public GameObject player;

    Animator _anim;

    void Start()
    {
        money = startMoney;
        health = startHealth;
        xp = startXp;
        _anim = player.GetComponent<Animator>();
    }

    private void Update()
    {
        LevelUp();
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
        _anim.SetBool("Death", true);
        //Load death screen
    }

    void LevelUp()
    {
        if (xp == 100)
        {
            xp = 0;
            level++;
            Punch.onePunchDamage = Punch.onePunchDamage + .1f;
        }
    }
}

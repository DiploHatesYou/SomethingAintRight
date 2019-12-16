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
    public static int nextLevelXP = 100;

    private bool _dead = false;

    public GameObject player;
    private GameObject cam;
    public GameObject deadScreenUI;

    Punch punchClass;

    Animator _anim;

    void Start()
    {
        money = startMoney;
        health = startHealth;
        xp = startXp;
        _anim = player.GetComponent<Animator>();
        punchClass = player.GetComponent<Punch>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
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
            _dead = true;
        }
    }

    void Die()
    {
        _anim.SetBool("Death", true);
        
        if (_dead == true)
        {
            deadScreenUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            cam.GetComponent<MouseOrbit>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    void LevelUp()
    {
        if (xp == nextLevelXP)
        {
            xp = 0;
            level++;
            punchClass.onePunchDamage = punchClass.onePunchDamage + .05f;
            nextLevelXP += 25;
        }
    }
}

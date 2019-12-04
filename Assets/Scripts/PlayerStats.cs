using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 100;

    public static float health;
    public float startHealth = 1f;

    public static int xp;
    public int startXp = 0;

    void Start()
    {
        money = startMoney;
        health = startHealth;
        xp = startXp;
    }
}

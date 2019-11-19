using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 100;

    public static int health;
    public int startHealth = 100;

    public static int xp;
    public int startXp = 0;

    void Start()
    {
        money = startMoney;
        health = startHealth;
        xp = startXp;
    }
}

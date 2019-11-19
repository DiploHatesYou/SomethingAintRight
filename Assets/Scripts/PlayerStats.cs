using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 100;

    public static int health;
    public int startHealth = 100;

    void Start()
    {
        money = startMoney;
        health = startHealth;
    }
}

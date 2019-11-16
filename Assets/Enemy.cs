using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 3;
    public float maxDist = 15;
    public float minDist;


    void Update()
    {
        Follow();
    }

    void Follow()
    {
        transform.LookAt(player);

        if (Vector3.Distance(transform.position, player.position) >= minDist)
        {

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Enemy Hit");
    //        collision.transform.Translate(0, 0, -3, Space.Self);
    //    }
    //}
}

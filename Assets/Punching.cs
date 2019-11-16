using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punching : MonoBehaviour
{
    private bool punching = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!punching && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Punch(0.5f, 1.25f, transform.forward));
        }
    }

    IEnumerator Punch(float time, float distance, Vector3 direction)
    {
        punching = true;
        var timer = 0.0f;
        var orgPos = transform.position;
        direction.Normalize();
        Debug.Log("Above the loop");
        while (timer <= time)
        {
            Debug.Log("----");
            rb.MovePosition(orgPos + (Mathf.Sin(timer / time * Mathf.PI) + 1.0f) * direction);
            //transform.position = orgPos + (Mathf.Sin(timer / time * Mathf.PI) + 1.0f) * direction;
            yield return null;
            timer += Time.deltaTime;
        }
        transform.position = orgPos;
        punching = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private Transform spawnPoint;
    public int enemiesToSpawn;
    bool spawned = false;
    private float timer = 0f;
    bool exit = false;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (exit)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && timer <= 0)
        {
            StartCoroutine(SpawnWave());
           //spawned = true;
            timer = 45f;
            exit = false;
        }
    }

    IEnumerator SpawnWave()
    {

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        //enemiesToSpawn++;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //spawned = false;
            exit = true;
        }
    }
}

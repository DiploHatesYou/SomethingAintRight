using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private Transform spawnPoint;
    public int enemiesToSpawn;
    bool spawned = false;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawned == false)
        {
            StartCoroutine(SpawnWave());
            spawned = true;
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
}

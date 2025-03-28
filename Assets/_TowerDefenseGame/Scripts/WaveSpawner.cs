using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefab;
        public int enemyCount = 5;
        public float spawnInterval = 2f;
    }

    public Wave[] waves;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(enemyPrefab[0]);
            yield return new WaitForSeconds(spawnInterval);
        }
    } */

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}

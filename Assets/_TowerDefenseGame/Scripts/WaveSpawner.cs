using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public int enemyCount = 5;
        public float spawnInterval = 2f;
    }

    public Wave[] waves;
    public float timeBetweenWaves = 5f;

    public int currentWaveIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(SpawnWave(waves[0]));
        StartCoroutine(SpawnMultipleWaves());
    }

    IEnumerator SpawnMultipleWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Debug.Log("Wave incoming: " + (currentWaveIndex + 1));
            yield return new WaitForSeconds(timeBetweenWaves);

            Debug.Log("Spawning wave " + (currentWaveIndex + 1));
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            Debug.Log("Waiting for all enemies to die");
            //yield return new WaitUntil(AreAllEnemiesDestroyed);
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            //lambda function...reminds me of anonymous functions in javascript
            
            currentWaveIndex++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, wave.enemyPrefabs.Length);
            GameObject enemyPrefab = wave.enemyPrefabs[enemyIndex];
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    bool AreAllEnemiesDestroyed()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
}

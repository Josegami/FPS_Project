using System.Collections;
using UnityEngine;

public class SimpleInfiniteSpawner : MonoBehaviour
{
    [Header("Configuración del Spawn")]
    public GameObject monsterPrefab;     
    public float spawnInterval = 2.0f;   
    public float spawnRange = 3.0f;     

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnMonster();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster()
    {
        if (monsterPrefab == null) return;

        Vector3 spawnOffset = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            0f,
            Random.Range(-spawnRange, spawnRange)
        );

        Vector3 spawnPosition = transform.position + spawnOffset;

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }
}
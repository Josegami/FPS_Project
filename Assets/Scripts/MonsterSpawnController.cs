using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawnController : MonoBehaviour
{
    public int initialMonsterPerWave = 5;
    public int currentMonsterPerWave;

    public float spawnDelay = 0.5f; //Delay between spawning each monster in a wave

    public int currentWave = 0;
    public float waveCooldown = 10.0f; //Time in seconds between waves

    public bool inCooldown;
    public float cooldownCounter = 0; // Weonly use this for testing and the UI

    public List<Enemy> currentMonstersAlive;

    public GameObject monsterPrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;

    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentMonsterPerWave = initialMonsterPerWave;

        GlobalReferences.Instance.waveNumber = currentWave;

        StartNewWave();
    }

    private void StartNewWave()
    {
        currentMonstersAlive.Clear();

        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;

        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentMonsterPerWave; i++)
        {
            // Generate a random offset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            //Instantiate the Monster
            var monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

            //Get Enemy Script
            Enemy enemyScript = monster.GetComponent<Enemy>();

            //Track this monster
            currentMonstersAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> monsterToRemove = new List<Enemy>();
        foreach (Enemy monster in currentMonstersAlive)
        {
            if (monster.isDead)
            {
                monsterToRemove.Add(monster);
            }
        }

        foreach (Enemy monster in monsterToRemove)
        {
            currentMonstersAlive.Remove(monster);
        }

        monsterToRemove.Clear();

        if (currentMonstersAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentMonsterPerWave *= 2;
        StartNewWave();
    }
}


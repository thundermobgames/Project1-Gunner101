using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.PlayerLoop;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Level currLevel;   

    public event Action clearAllEnemies;
    List<Transform> spawnPoints;
    ObjectPooler objectPooler;
    bool enemySpawned;
    int spawnCounter;
    float nextSpawn;
    int totalKills;
    bool spawnProcessCompleted;
    bool allWavesCompleted;

    public delegate void EnemySpawnerAction(GameObject enemy);
    public static event EnemySpawnerAction OnEnemySpawned;

    public delegate void EnemySpawnResetAction();
    public static event EnemySpawnResetAction OnEnemySpawnProcessReset;
    public static event EnemySpawnResetAction OnEnemySpawnWavesCompleted;

    private void OnEnable() {
        Breakable.OnEnemyKilled += Breakable_OnEnemyKilled;
    }

    private void OnDisable() {
        Breakable.OnEnemyKilled -= Breakable_OnEnemyKilled;    
    }

    void Start()
    {       
        objectPooler = ObjectPooler.Instance;
        spawnPoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++) {
            spawnPoints.Add(transform.GetChild(i));
        }

        currLevel.currWave = 0;
    }

    void Update()
    {
        if (allWavesCompleted) { return; }

        if(!spawnProcessCompleted){
            startSpawningProcess();
        }
    }
    
    void spawnEnemy() {
        
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        if(spawnPoint!=null)
        {
            GameObject enemy = objectPooler.spawnFromPool("EnemyObject1", spawnPoint.position, spawnPoint.rotation);
            enemy.GetComponent<NavMeshAgent>().enabled = true;
            OnEnemySpawned?.Invoke(enemy);
        }
    }

    void startSpawningProcess()
    {
        if (this == null) { return; }

        if (spawnCounter < currLevel.noOfSpawns)
        {

            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + currLevel.spawnTimeGap;
                spawnEnemy();
                spawnCounter++;
            }
        }

        else if (totalKills == currLevel.noOfSpawns) {

            spawnProcessCompleted = true;
            StartCoroutine(waitTillNextSpawnProcess());
        }
    }

    IEnumerator waitTillNextSpawnProcess() {
        yield return new WaitForSeconds(1f);

        clearAllEnemies?.Invoke();
        spawnCounter = 0;
        totalKills = 0;
        spawnProcessCompleted = false;
        currLevel.currWave++;
        OnEnemySpawnProcessReset?.Invoke();



        if(currLevel.currWave >= currLevel.noOfWaves)
        {
            allWavesCompleted = true;
            OnEnemySpawnWavesCompleted?.Invoke();
        }

    }

    void Breakable_OnEnemyKilled(GameObject enemy)
    {
        totalKills++;
    }

}

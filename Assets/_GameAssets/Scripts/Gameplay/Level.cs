using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    
    [SerializeField] public string levelTitle = "Level";
    [SerializeField] public string levelDesc = "Level desc";
    [SerializeField] public float spawnTimeGap = 2f;
    [SerializeField] public int noOfSpawns = 5;
    [SerializeField] public int noOfWaves = 2;

    [HideInInspector] public int levelIndex;
    [HideInInspector] public int currWave;

    public delegate void LevelAction(Level level);
    public static event LevelAction OnLevelStarted;
    public static event LevelAction OnLevelCompleted;
    public static event LevelAction OnLevelFailed;

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        EnergyCore.OnEnergyCoreDestroyed += EnergyCore_OnEnergyCoreDestroyed;
        EnemySpawner.OnEnemySpawnWavesCompleted += EnemySpawner_OnEnemySpawnWavesCompleted;
    }

    private void OnDisable()
    {
        EnergyCore.OnEnergyCoreDestroyed -= EnergyCore_OnEnergyCoreDestroyed;
        EnemySpawner.OnEnemySpawnWavesCompleted -= EnemySpawner_OnEnemySpawnWavesCompleted;
    }

    private void Start() 
    {
        OnLevelStarted?.Invoke(this);       
    }
    private void EnemySpawner_OnEnemySpawnWavesCompleted()
    {
        StartCoroutine(CompleteLevelWithDelay(3f));
    }

    private void EnergyCore_OnEnergyCoreDestroyed()
    {
        StartCoroutine(FailLevelWithDelay(3f));
    }

    IEnumerator CompleteLevelWithDelay(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        OnLevelCompleted?.Invoke(this);
    }

    IEnumerator FailLevelWithDelay(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        OnLevelFailed?.Invoke(this);
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class EnergyCore : MonoBehaviour
{
        
    public class EnemyInfo
    {
        public Transform enemy;
        public int enemyState;

        public EnemyInfo(Transform e, int state)
        {
            enemy = e;
            enemyState = state;
        }
    }

    List<EnemyInfo> enemyInfos; 
    
    [SerializeField] ParticleSystem energyCore;
    [SerializeField] ParticleSystem energyCoreBlast;


    float currTime;
    float currCoreDamage;
    float hitDamage = 1f;
    bool coreDestroyed;
    int damageSteps;

    public delegate void EnergyCoreAction();
    public static event EnergyCoreAction OnEnergyCoreDestroyed;
    public static event EnergyCoreAction OnEnergyCoreScaled;


    private void Awake()
    {
        enemyInfos = new List<EnemyInfo>();
    }

    private void OnEnable()
    {
        GameplayUI.OnGamePaused += GameplayUI_OnGamePaused;
        GameplayUI.OnGameResumed += GameplayUI_OnGameResumed;
        EnemySpawner.OnEnemySpawned += EnemySpawner_OnEnemySpawned;
        EnemySpawner.OnEnemySpawnProcessReset += EnemySpawner_OnEnemySpawnProcessReset;
        EnemyController.OnEnemyShootLaserAtTarget += EnemyController_OnEnemyShootLaserAtTarget;
        Breakable.OnEnemyKilled += Breakable_OnEnemyKilled;
        Level.OnLevelCompleted += Level_OnLevelCompleted;
        Level.OnLevelFailed += Level_OnLevelFailed;


    }

    private void OnDisable()
    {
        GameplayUI.OnGamePaused -= GameplayUI_OnGamePaused;
        GameplayUI.OnGameResumed -= GameplayUI_OnGameResumed;
        EnemySpawner.OnEnemySpawned -= EnemySpawner_OnEnemySpawned;
        EnemySpawner.OnEnemySpawnProcessReset -= EnemySpawner_OnEnemySpawnProcessReset;
        EnemyController.OnEnemyShootLaserAtTarget -= EnemyController_OnEnemyShootLaserAtTarget;
        Breakable.OnEnemyKilled -= Breakable_OnEnemyKilled;
        Level.OnLevelCompleted -= Level_OnLevelCompleted;
        Level.OnLevelFailed -= Level_OnLevelFailed;

        coreScaleTween.Kill();
    }

    Tween coreScaleTween;

    void Start()
    {

           
    }
   

    private void Update()
    {
        if (coreDestroyed) { return; }

        if(enemyInfos.Count>0)
        {
            currTime += Time.deltaTime;

            if(currTime>=3)
            {
                applyDamageToCore();
                currTime = 0;
            }
        }
    }

    void applyDamageToCore()
    {
        int damageMultiplier=0;

        foreach (EnemyInfo info in enemyInfos)
        {
            if (info.enemyState==2)
            {
                damageMultiplier++;
            }
        }

        currCoreDamage += hitDamage * damageMultiplier;

        if(currCoreDamage > 10 && currCoreDamage <= 20 && damageSteps==0) 
        {
            damageSteps = 1;
            ScaleCore(damageSteps);          
        }

        else if (currCoreDamage > 20 && currCoreDamage <= 30 && damageSteps == 1)
        {
            damageSteps = 2;
            ScaleCore(damageSteps);           
        }

        else if (currCoreDamage > 30 && currCoreDamage <= 40 && damageSteps == 2)
        {
            damageSteps = 3;
            ScaleCore(damageSteps);           
        }

        else if (currCoreDamage > 40 && currCoreDamage <= 50 && damageSteps == 3)
        {
            damageSteps = 4;
            ScaleCore(damageSteps);           
        }

        else if (currCoreDamage > 50 && currCoreDamage <= 60 && damageSteps == 4)
        {
            damageSteps = 5;
            ScaleCore(damageSteps);           
            StartCoroutine(WaitForCoreToExplode(1f));

        }
    }

    void ScaleCore(float scaleValue)
    {
        coreScaleTween.Kill();
        coreScaleTween = energyCore.transform.DOScale(scaleValue, 0.5f).SetEase(Ease.InOutBack);
        OnEnergyCoreScaled?.Invoke();
    }

    private void Level_OnLevelFailed(Level level)
    {
        energyCore.gameObject.SetActive(false);
        energyCoreBlast.gameObject.SetActive(false);
    }

    private void Level_OnLevelCompleted(Level level)
    {
        energyCore.gameObject.SetActive(false);
        energyCoreBlast.gameObject.SetActive(false);
    }


    private void GameplayUI_OnGamePaused()
    {
        energyCore.gameObject.SetActive(false);
    }

    private void GameplayUI_OnGameResumed()
    {
        if(!coreDestroyed)
        {
            energyCore.gameObject.SetActive(true);   
        }
    }

    private void Breakable_OnEnemyKilled(GameObject enemy)
    {
        foreach (EnemyInfo info in enemyInfos)
        {
            if (info.enemy.Equals(enemy.transform))
            {
                info.enemyState = 3;
                break;
            }
        }
    }

    private void EnemyController_OnEnemyShootLaserAtTarget(GameObject enemy)
    {
        foreach (EnemyInfo info in enemyInfos)
        {
            if (info.enemy.Equals(enemy.transform))
            {
                info.enemyState = 2;
                break;
            }
        }
    }

    private void EnemySpawner_OnEnemySpawnProcessReset()
    {
        enemyInfos.Clear();
    }

    private void EnemySpawner_OnEnemySpawned(GameObject enemy)
    {
        bool addEnemyInfo=true;
        foreach(EnemyInfo info in enemyInfos)
        {
            if(info.enemy.Equals(enemy.transform))
            {
                addEnemyInfo = false;
                break;
            }
        }

        if(addEnemyInfo)
        {
            enemyInfos.Add(new EnemyInfo(enemy.transform, 1));
        }
    }


    IEnumerator WaitForCoreToExplode(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        energyCore.Stop();
        energyCoreBlast.gameObject.SetActive(true);
        energyCoreBlast.Play();
        coreDestroyed = true;

        OnEnergyCoreDestroyed?.Invoke();
    }

}

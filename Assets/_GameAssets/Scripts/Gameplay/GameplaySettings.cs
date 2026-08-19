using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySettings : MonoBehaviour
{

    [SerializeField] bool debugMode;
    [SerializeField] int testLevel;
    [SerializeField] AudioClip selectClip;
    [SerializeField] AudioClip moveClip;
    [SerializeField] AudioClip rotateClip;
    [SerializeField] AudioClip laserClip;
    [SerializeField] AudioClip largeBlastClip;
    [SerializeField] AudioSource ObjectSound;

    GameManager mgr;
    GameObject levelObject;
    Level level;

    public static GameplaySettings Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void OnEnable()
    {
        EnemyController.OnEnemyShootLaserAtTarget += EnemyController_OnEnemyShootLaserAtTarget;
        EnergyCore.OnEnergyCoreDestroyed += EnergyCore_OnEnergyCoreDestroyed;
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyShootLaserAtTarget -= EnemyController_OnEnemyShootLaserAtTarget;
        EnergyCore.OnEnergyCoreDestroyed -= EnergyCore_OnEnergyCoreDestroyed;
    }


    private void Start() 
    {
        mgr = GameManager.Instance;        
        StartCoroutine(createLevel());
    }

    IEnumerator createLevel()
    {
        yield return new WaitForSeconds(0.2f);
        Object obj=null;
        if (debugMode)
        {
            obj = Resources.Load("Levels/Level" + testLevel);
        }
        else
        {
            obj = Resources.Load("Levels/Level" + (mgr.Pref_CurrentLevel + 1));
        }

        levelObject = Instantiate((GameObject)obj);        
        level = levelObject.GetComponent<Level>();
        level.levelIndex = mgr.Pref_CurrentLevel;

    }

    private void EnergyCore_OnEnergyCoreDestroyed()
    {
        ObjectSound.PlayOneShot(largeBlastClip);
    }


    private void EnemyController_OnEnemyShootLaserAtTarget(GameObject enemy)
    {       
        ObjectSound.PlayOneShot(laserClip);
    }

}

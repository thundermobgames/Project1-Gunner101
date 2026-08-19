using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro; 

public class ControlsUI : MonoBehaviour
{

    [SerializeField] GameObject topBar;
    [SerializeField] Image splashBG;
    [SerializeField] TextMeshProUGUI levelNoText;
    [SerializeField] GameObject IntroUI;
    [SerializeField] TextMeshProUGUI waveNoText;
    [SerializeField] TextMeshProUGUI eCoreText;


    bool isUIFadeout;
    GameManager mgr;
    Level currLevel;
    float eCoreScaleCounter;

    private void OnEnable() 
    {
        Level.OnLevelStarted += Level_OnLevelStarted;
        EnemySpawner.OnEnemySpawnProcessReset += EnemySpawner_OnEnemySpawnProcessReset;
        EnergyCore.OnEnergyCoreScaled += EnergyCore_OnEnergyCoreScaled;
    }

    private void OnDisable() 
    {
        Level.OnLevelStarted -= Level_OnLevelStarted;
        EnemySpawner.OnEnemySpawnProcessReset -= EnemySpawner_OnEnemySpawnProcessReset;
        EnergyCore.OnEnergyCoreScaled -= EnergyCore_OnEnergyCoreScaled;
    }

    private void Start() {
        mgr = GameManager.Instance;
        
        if(mgr.Pref_CurrentLevel==0)
        {
            IntroUI.SetActive(true);
            splashBG.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            splashBG.gameObject.SetActive(true);
            StartCoroutine(fadeOutSplashBG(0.5f));
        }
    }

    IEnumerator fadeOutSplashBG(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        splashBG.DOFade(0, 2f);
        yield return new WaitForSecondsRealtime(2);        
        splashBG.gameObject.SetActive(false);
    }
    
    public void SkipIntro()
    {
        Time.timeScale = 1;
        mgr.playBtnClick();
        IntroUI.SetActive(false);
        splashBG.gameObject.SetActive(true);
        StartCoroutine(fadeOutSplashBG(0.5f));
    }

    private void EnergyCore_OnEnergyCoreScaled()
    {
        eCoreScaleCounter++;
        eCoreText.text = "E-Core: " + (float)(eCoreScaleCounter / 5f * 100f) + "%";
        Debug.Log("E-CORE : "+eCoreScaleCounter);
    }

    private void EnemySpawner_OnEnemySpawnProcessReset()
    {
        waveNoText.text = "Enemy Waves: "+currLevel.currWave+"/"+currLevel.noOfWaves;
    }

    private void Level_OnLevelStarted(Level level)
    {
        currLevel = level;
        levelNoText.text = currLevel.levelTitle.Substring(6);
        waveNoText.text = "Enemy Waves: " + currLevel.currWave + "/" + currLevel.noOfWaves;
        eCoreText.text = "E-Core: " + (eCoreScaleCounter / 5f * 100f) + "%";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] GameObject controlsUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject completeUI;
    [SerializeField] GameObject failUI;
    [SerializeField] AudioSource gameplayAmbience; 

    [HideInInspector] public Level currentLevel;
    GameManager mgr;   
    public static GameplayUI Instance { get; private set; }

    public delegate void GameplayUIAction();
    public static event GameplayUIAction OnGamePaused;
    public static event GameplayUIAction OnGameResumed;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;           
        }       
    }
    
    private void OnEnable() 
    {
        Level.OnLevelStarted += Level_OnLevelStarted;
        Level.OnLevelCompleted += OnLevelComplete;
        Level.OnLevelFailed += OnLevelFail;     
    }
    
    private void OnDisable() 
    {
        Level.OnLevelStarted -= Level_OnLevelStarted;
        Level.OnLevelCompleted -= OnLevelComplete;
        Level.OnLevelFailed -= OnLevelFail;      
    }

    private void Start() 
    {
        mgr = GameManager.Instance;        
        controlsUI.SetActive(true);
        pauseUI.SetActive(false);
        completeUI.SetActive(false);
        failUI.SetActive(false);
        gameplayAmbience.volume = mgr.Pref_MusicVolume;
    }

    public void pauseGame() 
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        mgr.playBtnClick();
        gameplayAmbience.Pause();

        OnGamePaused?.Invoke();
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        mgr.playBtnClick();
        gameplayAmbience.UnPause();

        OnGameResumed?.Invoke();
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        mgr.goToGameplay(); 
        mgr.playBtnClick();       
    }

    public void nextGame()
    {
        Time.timeScale = 1;
        mgr.goToGameplay();
        mgr.playBtnClick();        
    }

    public void restartAfterComplete()
    {
        mgr.Pref_CurrentLevel--;
        Time.timeScale = 1;
        mgr.goToGameplay();
        mgr.playBtnClick();        
    }

    public void exitGame()
    {
        Time.timeScale = 1;
        mgr.goToMainmenu();
        mgr.playBtnClick();        
    }

    private void Level_OnLevelStarted(Level level)
    {
        currentLevel = level;
    }

    public void OnLevelComplete(Level level)
    {
        mgr.completeLevel();
        Time.timeScale = 0;
        completeUI.SetActive(true);
        gameplayAmbience.Stop();
    }

    public void OnLevelFail(Level level)
    {        
        Time.timeScale = 0;
        failUI.SetActive(true);
        gameplayAmbience.Stop();
    }

    void OnPlayerHealthDecreased(float healthValue)
    {
        //healthBar.fillAmount = healthValue;
    }

}

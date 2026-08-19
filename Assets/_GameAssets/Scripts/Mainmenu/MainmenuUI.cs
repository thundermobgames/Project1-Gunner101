using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainmenuUI : MonoBehaviour
{
    [SerializeField] GameObject startUI;
    [SerializeField] GameObject levelUI;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject quitUI;
    [SerializeField] AudioSource mainmenuMusic;
    GameManager mgr;

    public static MainmenuUI Instance { get; private set; }
    
    private void Awake() 
    {
        if (Instance == null) { Instance = this; }        
        startUI.SetActive(true);
        levelUI.SetActive(false);
        settingsUI.SetActive(false);
        quitUI.SetActive(false);                        
    }

    private void Start() {
        mgr = GameManager.Instance;
        mainmenuMusic.volume = mgr.Pref_MusicVolume;
    }

    public void openStartUI() 
    {
        startUI.SetActive(true);
        levelUI.SetActive(false);
        settingsUI.SetActive(false);
        quitUI.SetActive(false);
        mgr.playBtnClick();
    }

    public void openLevelUI() 
    {
        startUI.SetActive(false);
        levelUI.SetActive(true);
        settingsUI.SetActive(false);
        quitUI.SetActive(false);
        mgr.playBtnClick();
    }

    public void openSettingsUI() 
    {
        startUI.SetActive(false);
        levelUI.SetActive(false);
        settingsUI.SetActive(true);
        quitUI.SetActive(false);
        mgr.playBtnClick();
    }
    public void openQuitUI() 
    {
        startUI.SetActive(false);
        levelUI.SetActive(false);
        settingsUI.SetActive(false);
        quitUI.SetActive(true);
        mgr.playBtnClick();
    }

    public void quitGame() 
    {
        Application.Quit();
    }

    public void changeMusicVolume(float volume)
    {
        mgr.Pref_MusicVolume = volume;
        mainmenuMusic.volume = mgr.Pref_MusicVolume;
    }

}

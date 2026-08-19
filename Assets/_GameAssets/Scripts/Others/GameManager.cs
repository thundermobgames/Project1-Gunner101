using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Save-Load Stuff
    public int Pref_CurrentLevel
    {
        get { if (PlayerPrefs.HasKey("CurrentLevel")) { return PlayerPrefs.GetInt("CurrentLevel"); } else { return 0; } }
        set { PlayerPrefs.SetInt("CurrentLevel", value); }
    }

    public int Pref_TotalLevelsUnlocked
    {
        get { if (PlayerPrefs.HasKey("TotalLevelsUnlocked")) { return PlayerPrefs.GetInt("TotalLevelsUnlocked"); } else { return 0; } }
        set { PlayerPrefs.SetInt("TotalLevelsUnlocked", value); }
    }

    public float Pref_SFXVolume
    {
        get { if (PlayerPrefs.HasKey("SFXVolume")) { return PlayerPrefs.GetFloat("SFXVolume"); } else { return 0.7f; } }
        set { PlayerPrefs.SetFloat("SFXVolume", value); }
    }

    public float Pref_MusicVolume
    {
        get { if (PlayerPrefs.HasKey("MusicVolume")) { return PlayerPrefs.GetFloat("MusicVolume"); } else { return 1; } }
        set { PlayerPrefs.SetFloat("MusicVolume", value); }
    }

    public class LevelProperties
    {
        int levelIndex;

        public LevelProperties(int index)
        {
            levelIndex = index;
        }

        [HideInInspector]
        public int Pref_LevelStatus
        {
            get { if (PlayerPrefs.HasKey("LevelStatus" + levelIndex)) { return PlayerPrefs.GetInt("LevelStatus" + levelIndex); } else { return 0; } }
            set { PlayerPrefs.SetInt("LevelStatus" + levelIndex, value); }
        }
    }

    #endregion

    [HideInInspector] public int previousSceneIndex;
    [SerializeField] AudioSource btnClickSound;

    [HideInInspector] public LevelProperties[] levelProperties;
    [HideInInspector] public bool allLevelsCompleted;
    int totalLevels = 5;

    private void Start()
    {
        Application.targetFrameRate = 30;
        applyDefaultSettings();
        btnClickSound.volume = Pref_SFXVolume;
    }
    void applyDefaultSettings()
    {
        levelProperties = new LevelProperties[totalLevels];
        for(int i=0;i<levelProperties.Length;i++){ levelProperties[i] = new LevelProperties(i); }

        if (Pref_TotalLevelsUnlocked == 0)
        {
            for (int i = 0; i < levelProperties.Length; i++)
            {
                if (i == 0)
                {
                    levelProperties[i].Pref_LevelStatus = 1;
                }
                else
                {
                    levelProperties[i].Pref_LevelStatus = 0;
                }
            }

            Pref_TotalLevelsUnlocked = 1;
            
        }
    }

    public void goToGameplay()
    {
        previousSceneIndex = 1;
        SceneManager.LoadScene(0);
    }

    public void goToMainmenu()
    {
        previousSceneIndex = 2;
        SceneManager.LoadScene(0);
    }

    public void completeLevel()
    {
        levelProperties[Pref_CurrentLevel].Pref_LevelStatus = 2;
        if (Pref_CurrentLevel == levelProperties.Length - 1) { allLevelsCompleted = true; }

        if (Pref_CurrentLevel < levelProperties.Length - 1)
        {
            Pref_CurrentLevel++;
            if (levelProperties[Pref_CurrentLevel].Pref_LevelStatus == 0)
            {
                levelProperties[Pref_CurrentLevel].Pref_LevelStatus = 1;
                Pref_TotalLevelsUnlocked++;
            }

        }
    }


    public void playBtnClick()
    {
        btnClickSound.Play();
    }

    public void changeBtnClickVolume(float volume)        
    {
        Pref_SFXVolume = volume;
        btnClickSound.volume = Pref_SFXVolume;
    }


}

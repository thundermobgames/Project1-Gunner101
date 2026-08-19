using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CompleteUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelTitle;
    [SerializeField] TextMeshProUGUI levelDesc;
    [SerializeField] Button nextButton;
    [SerializeField] AudioSource levelCompleteAudio;

    GameManager mgr;
    GameplayUI gameplayUI;

    private void Start()
    {
        mgr = GameManager.Instance;
        gameplayUI = GameplayUI.Instance;
        levelTitle.text = gameplayUI.currentLevel.levelTitle;
        levelCompleteAudio.volume = mgr.Pref_SFXVolume;
        if (gameplayUI.currentLevel.levelIndex==mgr.levelProperties.Length-1) 
        { 
            nextButton.interactable = false;
            levelDesc.text = "All energy cores neutralized, Thanks for playing!";
        }
        levelCompleteAudio.Play();
    }
}

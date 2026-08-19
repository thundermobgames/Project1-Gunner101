using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelTitle;
    
    GameplayUI gameplayUI;

    private void Start()
    {
        gameplayUI = GameplayUI.Instance;
        levelTitle.text = gameplayUI.currentLevel.levelTitle;       
    }


}

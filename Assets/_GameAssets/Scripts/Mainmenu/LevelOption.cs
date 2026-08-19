using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelOption : MonoBehaviour
{
    [SerializeField] Button levelBtn;
    [SerializeField] GameObject btnHighlight;
    [SerializeField] TextMeshProUGUI levelNoText;    
    [SerializeField] GameObject locked;
    [SerializeField] GameObject unlocked;
    [SerializeField] GameObject completed;
    [SerializeField] Color enabledColor;
    [SerializeField] Color disabledColor;
    [HideInInspector] public int levelIndex;
    bool isSelected;
    int levelStatus;

    GameManager mgr;

    Image btnImage;
    Image outlineImage;

    public delegate void LevelSelectionAction(int index);
    public static event LevelSelectionAction onLevelSelected;

    private void Awake() 
    {
        mgr = GameManager.Instance;
        btnImage = levelBtn.GetComponent<Image>();
        outlineImage = levelBtn.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void OnEnable() 
    {
        onLevelSelected += onLevelOptionSelected;
        if (levelIndex == mgr.Pref_TotalLevelsUnlocked-1) { selectLevel(); }
    }

    private void OnDisable() 
    {
        onLevelSelected -= onLevelOptionSelected;    
    }

    private void Start() 
    {
        int levelNo = (levelIndex + 1);
        levelNoText.text = levelNo<10? "0"+levelNo.ToString():levelNo.ToString();
        if (levelIndex == mgr.Pref_TotalLevelsUnlocked-1) { selectLevel(); }                
        updateLevelStatus();
    }

    void updateLevelStatus()
    {
        levelStatus = mgr.levelProperties[levelIndex].Pref_LevelStatus;

        if (levelStatus == 0)
        {
            levelBtn.interactable = false;
            locked.SetActive(true);
            unlocked.SetActive(false);
            completed.SetActive(false);
            //btnImage.color = disabledColor;
            //outlineImage.color = disabledColor;
        }

        else if (levelStatus == 1)
        {
            levelBtn.interactable = true;
            locked.SetActive(false);
            unlocked.SetActive(true);
            completed.SetActive(false);
            //btnImage.color = enabledColor;
            //outlineImage.color = enabledColor;
        }

        else if (levelStatus == 2)
        {
            levelBtn.interactable = true;
            locked.SetActive(false);
            unlocked.SetActive(false);
            completed.SetActive(true);
            //btnImage.color = enabledColor;
            //outlineImage.color = enabledColor;
        }
    }

    public void selectLevel() 
    {
        isSelected = true;

        if (onLevelSelected != null)
        {
            onLevelSelected.Invoke(levelIndex);
        }
    }

    void onLevelOptionSelected(int index) 
    {
        if(levelIndex==index)
        {
            mgr.Pref_CurrentLevel = index;
            btnHighlight.SetActive(true);
        }
        else 
        {
            btnHighlight.SetActive(false);
        }
    }

}

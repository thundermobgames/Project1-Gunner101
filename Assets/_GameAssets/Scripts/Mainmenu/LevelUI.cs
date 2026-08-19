using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] GameObject levelBtnPrefab;
    [SerializeField] Transform levelBtnsParent;
    [SerializeField] ScrollRect scrollRect;

    List<GameObject> levelbtns;

    GameManager mgr;

    private void Awake() 
    {
        mgr = GameManager.Instance;
        levelbtns = new List<GameObject>();
        for (int i = 0; i < levelBtnsParent.childCount; i++)
        {
            GameObject btn = Instantiate(levelBtnPrefab, levelBtnsParent.GetChild(i));
            btn.GetComponent<LevelOption>().levelIndex=i;
            levelbtns.Add(btn);
        }       
    }

    private void OnEnable() 
    {
        updateScrollView();
        LevelOption.onLevelSelected += LevelOption_OnLevelSelected;
    }

    private void OnDisable() {
        LevelOption.onLevelSelected -= LevelOption_OnLevelSelected;
    }

    private void Start()
    {
        updateScrollView();
    }
    
    public void gotoNext() 
    {
        mgr.goToGameplay();        
    }

    void updateScrollView()
    {
        float scrollRectValue = ((float)mgr.Pref_TotalLevelsUnlocked-1) / (float)mgr.levelProperties.Length;
        scrollRect.horizontalNormalizedPosition = scrollRectValue;
    }

     void LevelOption_OnLevelSelected(int index)
    {
        mgr.playBtnClick();
    }

}

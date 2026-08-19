using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] Image fillBar;
    [SerializeField] float waitTime = 2f;

    float startTime, currentTime;
    GameManager mgr;
    bool loadingCompleted;

    private void Start() {

        Time.timeScale = 1;
        mgr = GameManager.Instance;
        fillBar.fillAmount = 0f;
        startTime = Time.time;
        currentTime = 0;
        
        Resources.UnloadUnusedAssets();

        if (mgr.previousSceneIndex == 1)
        { 
            StartCoroutine(loadScene(2));
        }

        else if (mgr.previousSceneIndex == 0 || mgr.previousSceneIndex == 2)
        { 
            StartCoroutine(loadScene(1));
        }
    }

    private void Update()
    {
        if (!loadingCompleted && currentTime <= waitTime)
        {
            currentTime = Time.time - startTime;
            fillBar.fillAmount = currentTime / waitTime;
        }
    }

    IEnumerator loadScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitUntil(() => currentTime >= waitTime);
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);
        loadingCompleted = true;             
    }

}

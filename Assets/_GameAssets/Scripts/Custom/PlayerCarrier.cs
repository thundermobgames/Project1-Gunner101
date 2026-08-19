using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarrier : MonoBehaviour
{

    [SerializeField] float rotationSpeed=1f;
    [SerializeField] Renderer gun1;
    [SerializeField] Renderer gun2;

    AudioSource interiorSound;

    private void OnEnable()
    {
        Level.OnLevelStarted += Level_OnLevelStarted;
        Level.OnLevelCompleted += Level_OnLevelCompleted;
        Level.OnLevelFailed += Level_OnLevelFailed;
        GameplayUI.OnGamePaused += GameplayUI_OnGamePaused;
        GameplayUI.OnGameResumed += GameplayUI_OnGameResumed;
    }

    private void OnDisable()
    {
        Level.OnLevelStarted -= Level_OnLevelStarted;
        Level.OnLevelCompleted -= Level_OnLevelCompleted;
        Level.OnLevelFailed -= Level_OnLevelFailed;
        GameplayUI.OnGamePaused -= GameplayUI_OnGamePaused;
        GameplayUI.OnGameResumed -= GameplayUI_OnGameResumed;
    }


    private void Start() {
        interiorSound = GetComponent<AudioSource>();
        gun1.enabled = false;
        gun2.enabled = false;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        if (Time.timeScale == 0) { if (interiorSound.isPlaying) { interiorSound.Stop(); } }
        else { if (!interiorSound.isPlaying) { interiorSound.Play(); } }
    }

    private void Level_OnLevelStarted(Level level)
    {
        gun1.enabled = true;
        gun2.enabled = true;
    }

    private void GameplayUI_OnGameResumed()
    {
        gun1.enabled = true;
        gun2.enabled = true;
    }

    private void GameplayUI_OnGamePaused()
    {
        gun1.enabled = false;
        gun2.enabled = false;
    }

    private void Level_OnLevelFailed(Level level)
    {
        gun1.enabled = false;
        gun2.enabled = false;
    }

    private void Level_OnLevelCompleted(Level level)
    {
        gun1.enabled = false;
        gun2.enabled = false;
    }

}

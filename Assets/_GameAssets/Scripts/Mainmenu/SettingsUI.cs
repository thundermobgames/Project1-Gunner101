using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    GameManager mgr;
    MainmenuUI mainmenuUI;
    private void Start() 
    {
        mgr = GameManager.Instance;
        mainmenuUI = MainmenuUI.Instance;
        sfxSlider.value = mgr.Pref_SFXVolume;
        musicSlider.value = mgr.Pref_MusicVolume;
    }

    public void resetSettings()
    {
        mgr.changeBtnClickVolume(0.7f);
        mainmenuUI.changeMusicVolume(1f);
        sfxSlider.value = mgr.Pref_SFXVolume;
        musicSlider.value = mgr.Pref_MusicVolume;        
        mgr.playBtnClick();
    }

    public void openAboutUs()
    {
        mgr.playBtnClick();
        Application.OpenURL("https://x.com/thundermobgames");
    }

    public void changeSFXVolume(float x)
    {        
        mgr.changeBtnClickVolume(sfxSlider.value);
    }

    public void changeMusicVolume(float x)
    {
        mainmenuUI.changeMusicVolume(musicSlider.value);
    }


}

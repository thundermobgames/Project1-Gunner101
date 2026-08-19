using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{

    [SerializeField] Text levelDescText;

    string levelDesc;

    private void Start() {

        foreach (char character in levelDesc)
        { 
            
        }

    }

    IEnumerator typeText()
    {
        foreach (char character in levelDesc)
        { 
            yield return new WaitForSeconds(1);
        }
    }









}

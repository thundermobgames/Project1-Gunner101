using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCalculator : MonoBehaviour
{
    [SerializeField] Text FPS_text;
    
    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;

        }

        FPS_text.text = "FPS : "+Mathf.Floor(fps).ToString();
    }
}

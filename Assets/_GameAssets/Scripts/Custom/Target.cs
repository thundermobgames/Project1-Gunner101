using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Target : MonoBehaviour
{    
    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject breakableObject;
    [SerializeField] float breakForce;
    [SerializeField] string sourceOfDestruction;
    [SerializeField] int targetStrength = 10;
    [SerializeField] float startSize=0.5f;
    [SerializeField] float endSize=1f;
    [SerializeField] float scaleSpeed=0.01f;
    [SerializeField] GameObject healthBarObj;
    [SerializeField] SpriteRenderer targetCirle;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    
    bool objectDestroyed;
    MeshRenderer mesh;
    Transform targetTransform;
    Slider healthBar;
    int targetHealth = 100;
    int hitCounter;
    float currentScale;
    bool targetScaleCompleted;

    public delegate void TargetAction(GameObject target);
    public static event TargetAction OnTargetDestroyed;
    public static event TargetAction OnTargetScaled;


    private void Start()
    {
        mesh = targetObject.GetComponent<MeshRenderer>();
        healthBar = healthBarObj.transform.GetChild(0).GetComponent<Slider>();
        targetTransform = targetObject.transform;
        targetTransform.localScale = new Vector3(startSize,startSize,startSize);
        currentScale = startSize;
    }

    private void Update()
    {

        if(!targetScaleCompleted)
        {
            if(currentScale<endSize)
            {
                currentScale += scaleSpeed * Time.deltaTime;
                targetTransform.localScale = new Vector3(currentScale,currentScale,currentScale);
                if(currentScale>=endSize)
                {
                    targetTransform.localScale = new Vector3(endSize,endSize,endSize);
                    targetScaleCompleted = true;
                    OnTargetScaled?.Invoke(gameObject);
                }
            }

            updateColor(targetCirle);

        }        
    }

    void breakObject()
    {
        GameObject obj = Instantiate(breakableObject, transform.position, transform.rotation);
        foreach (Rigidbody rb in obj.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(transform.up * breakForce);
        }

        Destroy(obj, 3);
        objectDestroyed = true;
        mesh.enabled = false;
        healthBarObj.SetActive(false);
        OnTargetDestroyed?.Invoke(gameObject);

    }

    float targetColorLerpRatio;
    bool colorUpdated;
    void updateColor(SpriteRenderer sprite)
    {
        if(!colorUpdated)
        {
            if(targetColorLerpRatio<1)
            {
                Color colorOutput = Color.Lerp(startColor, endColor, targetColorLerpRatio);
                targetColorLerpRatio += Time.deltaTime * scaleSpeed*2;
                sprite.color = colorOutput;
                if(targetColorLerpRatio>=1)
                {
                    targetColorLerpRatio = 0;
                    sprite.color = endColor;
                    colorUpdated = true;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider col) {

        if (col.tag.Equals(sourceOfDestruction) && !objectDestroyed)
        {
            hitCounter++;
            if (hitCounter >= targetStrength) {
                targetHealth--;
                healthBar.value = targetHealth;
                hitCounter = 0;                
            }
            
            if (targetHealth == 0) {
                breakObject();
            }            
        }
    }
}

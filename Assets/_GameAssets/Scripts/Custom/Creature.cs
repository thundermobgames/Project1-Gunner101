using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;


public class Creature : MonoBehaviour
{
    [System.Serializable]
    public class PartCategory
    {
        [System.Serializable]
        public class Part
        {
            public Transform obj;
            public float startScale = 0;
            public float endScale = 1;
        }

        public float minScaleDuration;
        public float maxScaleDuration;
        public List<Part> parts;
    }

    [SerializeField] float minGrowthTime;
    [SerializeField] float maxGrowthTime;
    [SerializeField] string sourceOfDestruction;
    [SerializeField] int targetStrength = 1;
    [SerializeField] GameObject healthBarObj;
    [SerializeField] List<PartCategory> partCategories;

    bool objectDestroyed;
    MeshRenderer mesh;
    Transform targetTransform;
    Slider healthBar;
    int targetHealth = 100;
    int hitCounter;


    int currentPartCategory;
    float currTimeWithoutHit;
    float growthTime;
    bool targetGrowthCompleted;

    public delegate void TargetAction();
    public static event TargetAction OnTargetUnderFire;
    public static event TargetAction OnTargetGrowth;
    public static event TargetAction OnTargetDestroyed;
    
    private void Awake() {
        healthBar = healthBarObj.transform.GetChild(0).GetComponent<Slider>();
    }

    private void Start()
    {
        
        growthTime = Random.Range(minGrowthTime, maxGrowthTime);

        foreach (PartCategory category in partCategories)
        {
            foreach(PartCategory.Part part in category.parts)
            {
                part.obj.localScale = new Vector3(part.startScale, part.startScale, part.startScale);
                part.obj.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    private void Update()
    {        
        if(!targetGrowthCompleted && !objectDestroyed)
        {
            if(currTimeWithoutHit<growthTime)
            {
                currTimeWithoutHit += Time.deltaTime;
                if(currTimeWithoutHit>=growthTime)
                {
                    scaleParts(currentPartCategory);
                    growthTime = Random.Range(minGrowthTime, maxGrowthTime);
                    currentPartCategory++;
                    currTimeWithoutHit = 0;

                    if(currentPartCategory==partCategories.Count)
                    {
                        targetGrowthCompleted = true;
                        OnTargetGrowth?.Invoke();
                    }
                }
            }
        }
    }

    void scaleParts(int partCategoryIndex)
    {
        float duration = Random.Range(partCategories[partCategoryIndex].minScaleDuration, partCategories[partCategoryIndex].minScaleDuration);
        foreach (PartCategory.Part part in partCategories[partCategoryIndex].parts)
        {
            part.obj.GetComponent<Renderer>().enabled = true;
            // implement growth 
            part.obj.DOScale(part.endScale, duration);           
        }
    }

    private void OnTriggerEnter(Collider col) {

        if (col.tag.Equals(sourceOfDestruction) && !objectDestroyed)
        {
            
            currTimeWithoutHit = 0;
            OnTargetUnderFire?.Invoke();
            hitCounter++;
            if (hitCounter >= targetStrength) {
                targetHealth--;
                healthBar.value = targetHealth;
                hitCounter = 0;                              
            }
            
            if (targetHealth == 0) {
                StartCoroutine(destroyCreature(1));
                objectDestroyed = true;
                OnTargetDestroyed?.Invoke();
            }            
        }
    }

    IEnumerator destroyCreature(float delay)
    {
        for (int i = partCategories.Count - 1; i >= 0;i--)
        {
            yield return new WaitForSeconds(delay);
            float duration = Random.Range(partCategories[i].minScaleDuration, partCategories[i].minScaleDuration);
            foreach (PartCategory.Part part in partCategories[i].parts)
            { 
                part.obj.DOScale(0, duration);           
            }
        }        
    }

}

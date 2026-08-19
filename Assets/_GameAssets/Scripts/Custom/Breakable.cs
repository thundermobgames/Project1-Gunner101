using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject breakableObject;
    [SerializeField] float breakForce;
    [SerializeField] string sourceOfDestruction;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] int targetStrength = 5;
    [SerializeField] GameObject healthBarObj;
    [SerializeField] GameObject laserGunObj;

    bool objectDestroyed;    
    ObjectPooler objectPooler;
    EnemySpawner enemySpawner;

    Slider healthBar;
    int targetHealth = 100;
    int hitCounter;

    public delegate void EnemyAction(GameObject enemy);
    public static event EnemyAction OnEnemyKilled;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        healthBar = healthBarObj.transform.GetChild(0).GetComponent<Slider>();
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        enemySpawner.clearAllEnemies += destroyObjectCompletely;
    }

    void breakObject()
    {
        GameObject obj = Instantiate(breakableObject, transform.position, transform.rotation);
        foreach (Rigidbody rb in obj.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(transform.forward * breakForce);
        }

        Destroy(obj, 3);
        objectDestroyed = true;
        healthBarObj.SetActive(false);
        laserGunObj.SetActive(false);
        mesh.enabled = false;
        OnEnemyKilled?.Invoke(gameObject);  
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals(sourceOfDestruction) && !objectDestroyed)
        {
            hitCounter++;
            if (hitCounter >= targetStrength)
            {
                targetHealth--;
                healthBar.value = targetHealth;
                hitCounter = 0;
            }

            if (targetHealth == 0)
            {
                breakObject();
            }
        }
    }

    void destroyObjectCompletely() {

        objectDestroyed = false;        
        mesh.enabled = true;
        healthBarObj.SetActive(true);
        laserGunObj.SetActive(true);
        targetHealth = 100;
        healthBar.value = targetHealth;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        objectPooler.returnToPool("EnemyObject1",gameObject);   
    }

    
    
}

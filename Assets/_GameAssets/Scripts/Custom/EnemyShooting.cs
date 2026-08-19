using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 0.6f;
    [SerializeField] float fireRate = 0.06f;
    [SerializeField] GameObject bulletSpawner;
    
    float nextFire;
    float waitToDestroyBullet = 1f;
    
    ObjectPooler objectPooler;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;        
    }
    
    public void shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = objectPooler.spawnFromPool("EnemyBullet", bulletSpawner.transform.position, bulletSpawner.transform.rotation);            
            bullet.GetComponent<Rigidbody>().linearVelocity = (bulletSpawner.transform.forward) * bulletSpeed;            
            StartCoroutine(destroyBullet(bullet));
        }        
    }
    
    IEnumerator destroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(waitToDestroyBullet);
        objectPooler.returnToPool("EnemyBullet", bullet);
    }
}

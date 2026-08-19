using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 0.6f;
    [SerializeField] float fireRate = 0.06f;
    [SerializeField] GameObject leftBulletSpawner;
    [SerializeField] GameObject rightBulletSpawner;
    
    float nextFire;
    ObjectPooler objectPooler;
    AudioSource shootingAudio;

    private void OnEnable()
    {
        Level.OnLevelStarted += Level_OnLevelStarted;
    }

    private void OnDisable()
    {
        Level.OnLevelStarted -= Level_OnLevelStarted;
    }


    void Start()
    {
        shootingAudio = GetComponent<AudioSource>();
        objectPooler = ObjectPooler.Instance;        
    }
    
    public void shoot(bool isShooting)
    {
        if(isShooting)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                
                GameObject bullet1 = objectPooler.spawnFromPool("PlayerBullet",leftBulletSpawner.transform.position,leftBulletSpawner.transform.rotation);
                GameObject bullet2 = objectPooler.spawnFromPool("PlayerBullet",rightBulletSpawner.transform.position, rightBulletSpawner.transform.rotation);
                bullet1.GetComponent<Rigidbody>().linearVelocity = (leftBulletSpawner.transform.forward) * bulletSpeed;
                bullet2.GetComponent<Rigidbody>().linearVelocity = (rightBulletSpawner.transform.forward) * bulletSpeed;
                if (shootingAudio!=null && !shootingAudio.isPlaying) { shootingAudio.Play(); }
                StartCoroutine(destroyBullet(bullet1, bullet2));

            }
        }
        else
        {
            if (shootingAudio!=null && shootingAudio.isPlaying) { shootingAudio.Stop(); }
        }
        
    }

    IEnumerator destroyBullet(GameObject bullet1, GameObject bullet2)
    {
        yield return new WaitForSeconds(1.5f);
        objectPooler.returnToPool("PlayerBullet", bullet1);
        objectPooler.returnToPool("PlayerBullet", bullet2);
    }

    private void Level_OnLevelStarted(Level level)
    {
        objectPooler = ObjectPooler.Instance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float lookRadius = 3f;    
    [SerializeField] Breakable breakable;
    [SerializeField] Transform laserGunHolder;
    [SerializeField] LineRenderer laserGun;

    Transform target;
    Transform energyCoreTarget;

    float lerpRatio;
    float laserRaySpeed = 1f;
    bool startLaserBeam;
    bool coreDestroyed;
   
    NavMeshAgent agent;
    bool shoot;
    AudioSource explosionAudio;

    public delegate void EnemyLaserAction(GameObject enemy);
    public static event EnemyLaserAction OnEnemyShootLaserAtTarget;


    private void OnEnable() 
    {
        Breakable.OnEnemyKilled += Breakable_OnEnemyKilled;
        EnergyCore.OnEnergyCoreDestroyed += EnergyCore_OnEnergyCoreDestroyed;  
        
        shoot = true;
        startLaserBeam = false;
        lerpRatio = 0;
        laserGun.SetPosition(1, new Vector3(0, 0, 0));
        
    }

    private void OnDisable() 
    {
        Breakable.OnEnemyKilled -= Breakable_OnEnemyKilled;
        EnergyCore.OnEnergyCoreDestroyed -= EnergyCore_OnEnergyCoreDestroyed;       
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Target").transform;
        energyCoreTarget = target.GetChild(0);
        explosionAudio = GetComponent<AudioSource>();
        explosionAudio.pitch = Random.Range(0.5f, 0.8f);      
    }

    void Update()
    {
        if (coreDestroyed) { return; }

        float distance = Vector3.Distance(target.position,transform.position);
        if (distance <= lookRadius) {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance) {
                faceTarget();
                if (!startLaserBeam) { startLaserBeam = true; OnEnemyShootLaserAtTarget?.Invoke(gameObject); }
            }            
        }

        laserGunHolder.LookAt(energyCoreTarget, Vector3.up);

        if (startLaserBeam)
        {
            if (lerpRatio <= 0.99f)
            {
                laserGun.SetPosition(1, new Vector3(0, 0, Mathf.Lerp(0, Vector3.Distance(laserGun.transform.position, energyCoreTarget.position), lerpRatio)));
                lerpRatio += laserRaySpeed * Time.deltaTime;
                if (lerpRatio > 0.99f)
                {
                    lerpRatio = 1;
                    laserGun.SetPosition(1, new Vector3(0, 0, Mathf.Lerp(0, Vector3.Distance(laserGun.transform.position, energyCoreTarget.position), 1)));
                    startLaserBeam = false;
                }
            }
        }

        if (lerpRatio == 1)
        {
            Vector3 localTarget = laserGun.transform.InverseTransformPoint(energyCoreTarget.position);
            laserGun.SetPosition(1, new Vector3(0, 0, localTarget.z));
        }

    }

    public void Breakable_OnEnemyKilled(GameObject enemy) {
        if(enemy.Equals(gameObject))
        {
            shoot = false;
            explosionAudio.Play();
        }
    }

    private void EnergyCore_OnEnergyCoreDestroyed()
    {
        coreDestroyed = true;
        laserGun.SetPosition(1, new Vector3(0, 0, 0));
    }

    void faceTarget() {

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
